using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// EmguCV namespaces
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.CvEnum;
using Emgu.CV.UI;
using Emgu.CV.Structure;
using Emgu.CV.OCR;
using Emgu.CV.GPU;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Recognition
{
    class Recognition
    {
        // Global vars
        private Capture cap;
        private Tesseract _ocr;
        private SearchTable searchTable;
        public PointW mostCommon;
        private Rectangle label;
        private LGD lgd;
        private MCvBox2D regionOfInterest;
        public string detectedNumber;

        // Image processing vars
        private int dilate;
        private int erode;
        private int labelHeight;
        private int labelWidth;
        private int labelFault;
        private double gaussianStd1;
        private double gaussianStd2;
        private int highboundFrequency;
        private int maxDistance;
        private int minimumChars;
        private int maximumChars;
        private int roiX;
        private int roiY;
        private int roiWidth;
        private int roiHeigth;

        // Images
        public Image<Rgb, Byte> rgbImg;
        public Image<Rgb, Byte> labelImage;
        public Image<Gray, Byte> imgMaterialID;

        public Recognition(Property properties, string tessdataDir, string tessdataLang)
        {
            updateProperties(properties);
            // Initialisation training data tesseract & whitelist (only numbers allowed) 
            _ocr = new Tesseract(tessdataDir, tessdataLang, Tesseract.OcrEngineMode.OEM_TESSERACT_ONLY);
            _ocr.SetVariable("tessedit_char_whitelist", "1234567890");
        }

        public void updateProperties(Property properties) 
        {
            try
            {
                // Image processing vars
                dilate = Int32.Parse((string)properties.get("dilate"));
                erode = Int32.Parse((string)properties.get("erode"));
                labelHeight = Int32.Parse((string)properties.get("label_height"));
                labelWidth = Int32.Parse((string)properties.get("label_width"));
                labelFault = Int32.Parse((string)properties.get("label_fault"));
                gaussianStd1 = Double.Parse((string)properties.get("gaussian_smt"));
                gaussianStd2 = Double.Parse((string)properties.get("gaussian_smt2"));
                highboundFrequency = Int32.Parse((string)properties.get("upper_limit_frequency"));
                maxDistance = Int32.Parse((string)properties.get("max_distance"));
                minimumChars = Int32.Parse((string)properties.get("min_char"));
                maximumChars = Int32.Parse((string)properties.get("max_char"));
                roiX = Int32.Parse((string)properties.get("roi_x"));
                roiY = Int32.Parse((string)properties.get("roi_y"));
                roiWidth = Int32.Parse((string)properties.get("roi_width"));
                roiHeigth = Int32.Parse((string)properties.get("roi_height"));
            }
            catch (Exception ex) {
                MessageBox.Show("Error while parsing values. Original error: " + ex.Message);   
            }

            // Initialize size label
            label = new Rectangle();
            label.Width = labelWidth;
            label.Height = labelHeight;

            // Region of interest
            Rectangle roiPosition = new Rectangle();
            roiPosition.Location = new Point(roiX, roiY);
            roiPosition.Width = roiWidth;
            roiPosition.Height = roiHeigth;
            regionOfInterest = new MCvBox2D(roiPosition.Location, roiPosition.Size, 0);

            // Other structures
            lgd = new LGD();
            searchTable = new SearchTable(highboundFrequency);
            mostCommon = new PointW();
        }

        /// <summary>
        /// Start the image processing
        ///     - Laplace Of Gaussian
        ///     - Thresholding (only strong changes occur)
        ///     - Closing operation
        ///     - Border following (S. Suzuki algorithm) only External Borders are allowed
        ///     - Calculate contour which is most likely with expected size of label
        ///     - Calculate most common point (SearchTable)
        ///     - Recognize the region of the most common point (Tesseract)
        ///     - Evaluate the expected result and Tesseract result (with LGD algorithm)
        /// </summary>
        /// <param name="expectedMaterialID"></param>
        /// <returns>true if recognized</returns>
        public bool executeRecognition(string imageId, string imagePath, string crane, string expectedMaterialID)
        {
            // Get image from path
            cap = new Capture(imagePath);
            Image<Bgr,byte> img = cap.QueryFrame();
            return executeRecognition(imageId, img, crane, expectedMaterialID);
        }

        public bool executeRecognition(string imageId, Image<Bgr,byte> img, string crane, string expectedMaterialID)
        {
            // reset LGD 
            lgd.isEqual = false;

            // Crop Image with region of interest
            img = img.Copy(regionOfInterest);

            // Convert to grayscale
            Image<Gray, Byte> gray_img = img.Convert<Gray, Byte>();
            rgbImg = img.Convert<Rgb, Byte>();

            // Laplacian of Gaussiaanse ( second order derivative)
            Image<Gray, float> laplace = gray_img.Laplace(3);
            laplace = laplace.SmoothGaussian(3, 3, gaussianStd1, gaussianStd2);

            // Threshold with mean of the graylevel intensities
            int threshold = adaptiveThreshold(img);
            Image<Gray, Byte> result = laplace.ThresholdBinary(new Gray(threshold), new Gray(255)).Convert<Gray, Byte>();

            // Closing operation
            // First dilate and then eroding
            IntPtr kernel_s_d = CvInvoke.cvCreateStructuringElementEx(dilate, dilate, dilate / 2 - 1, dilate / 2 - 1, CV_ELEMENT_SHAPE.CV_SHAPE_RECT, IntPtr.Zero);
            IntPtr kernel_s_e = CvInvoke.cvCreateStructuringElementEx(erode, erode, erode / 2 - 1, erode / 2 - 1, CV_ELEMENT_SHAPE.CV_SHAPE_RECT, IntPtr.Zero);
            CvInvoke.cvDilate(result.Ptr, result.Ptr, kernel_s_d, 1);
            CvInvoke.cvErode(result.Ptr, result.Ptr, kernel_s_e, 1);

            // Aanmaken afbeelding voor label
            labelImage = result.Convert<Rgb, Byte>();

            // Search for position of label
            using (MemStorage storage = new MemStorage())
            {
                PointW bestPoint = new PointW();
                double currentBest = 9999999;
                double labelArea = labelWidth * labelHeight;
                // Start border following: get the connected components (Contour<Point> contours)
                for (Contour<Point> contours = result.FindContours(); contours != null; contours = contours.HNext)
                {
                    // Construct a rectangle
                    // (top-left) point en  (bottom-right) point
                    Rectangle rect = contours.BoundingRectangle;
                    double rectDifference = Math.Abs(rect.Width - labelWidth) + Math.Abs(rect.Height - labelHeight);
                    double rectAreaDifference = rectDifference + Math.Abs(labelArea - contours.Area);
                    int rectArea = rect.Width * rect.Height;

                    // Check if the rectangle matches the expected size of label
                    if (
                        // If approximate size
                        rectDifference <= labelFault
                        // Area check with Formula of Green (with 35% error rate)
                        && contours.Area / rectArea >= 0.65
                        // If better then currentBest
                        && rectAreaDifference < currentBest
                        )
                    {
                        currentBest = rectDifference;
                        bestPoint.p = new PointF(rect.X + rect.Width / 2, rect.Y + rect.Height / 2); ;
                    }
                }
                // If a bestpoint is found:
                //  - draw a green rect on the image.
                //  - look for the nearest point (increase it's frequency, or add as new point).
                if (bestPoint.p.X != 0 && bestPoint.p.Y != 0)
                {
                    // Draw green rect
                    MCvBox2D box_G = new MCvBox2D(bestPoint.p, label.Size, 0);
                    rgbImg.Draw(box_G, new Rgb(Color.Green), 3);

                    // Search for the mostCommon point
                    // (there can be better point now)
                    PointW mostCommonPoint = searchTable.findMostCommonPoint(bestPoint, maxDistance);
                        
                    // If close enough to the mostcommon point, change the position of the mostcommon point.
                    // ( the cranes can move slightly)
                    if (bestPoint.closeEnough(mostCommon, maxDistance))
                        mostCommon = bestPoint;
                    // If nog close enough the bestpoint is added to the searchTable or a point his frequency 
                    // is incremented => check if that point has a larger frequency than the mostcommon point.
                    else if (mostCommonPoint.frequency >= mostCommon.frequency)
                            mostCommon = mostCommonPoint;
                }

                // Draw the region of the mostcommong point (red rect)
                if (mostCommon.p.X != 0 && mostCommon.p.Y != 0)
                {
                    // Calculate the size of the label at the mostcommon point
                    // (eroding en dilating have influence on the size of the located label)
                    Rectangle labelPosition = new Rectangle();
                    int x_coord = Convert.ToInt32(mostCommon.p.X - labelWidth + labelFault / 2 - erode / 4);
                    int y_coord = Convert.ToInt32(mostCommon.p.Y - labelHeight + labelFault / 2 - erode / 4);

                    if (x_coord < 0)
                        x_coord = 0;
                    if (y_coord < 0)
                        y_coord = 0;

                    labelPosition.Location = new Point(x_coord, y_coord);
                    labelPosition.Width = labelWidth + labelFault + erode / 2;
                    labelPosition.Height = labelHeight + labelFault + erode / 2;

                    MCvBox2D labelRect = new MCvBox2D(mostCommon.p, labelPosition.Size, 0);
                    rgbImg.Draw(labelRect, new Rgb(Color.Red), 3);

                    // DEBUG only, write file to local drive 
                    // If directory does not exist, create it. 
                    //string root = @"c:\" + crane + "\\" + expectedMaterialID;
                    //if (!Directory.Exists(root))
                    //{
                    //    Directory.CreateDirectory(root);
                    //}

                    //// save file to disk, make path valid
                    //foreach (char c in System.IO.Path.GetInvalidFileNameChars())
                    //{
                    //    imageId = imageId.Replace(c, '_');
                    //}
                    //rgbImg.Save(root + "\\" + imageId + ".jpeg");

                    // Execute Tesseract
                    detectedNumber = recognizeMaterialID(labelRect, img, bestPoint);
                    lgd.Equal(detectedNumber, expectedMaterialID);
                    return lgd.isEqual;
                }
            }
            return false;
        }

        /// <summary>
        /// Calculate adaptive threshold
        /// (when over exposed strong changes, under exposed small changes)
        /// </summary>
        /// <returns></returns>
        private int adaptiveThreshold(Image<Bgr,byte> img)
        {
            Image<Gray, Byte> img_gray = img.Convert<Gray, Byte>();
            // small increasement of intensity (+70)
            return (int)img_gray.GetAverage().Intensity + 70;
        }


        /// <summary>
        /// Recognition of MaterialID with tesseract
        ///     - Preprocess image (auto-cropping)
        ///     - Check if count of characters is ok.
        /// </summary>
        /// <param name="labelBox">box where label is found</param>
        /// <param name="img"></param>
        /// <param name="bestPoint"></param>
        /// <returns></returns>
        private string recognizeMaterialID(MCvBox2D labelBox, Image<Bgr, Byte> img, PointW bestPoint)
        {
            Image<Gray, Byte> tmp = img.Copy(labelBox).Convert<Gray, Byte>();
            tmp = tmp.Resize(120, 80, INTER.CV_INTER_LINEAR);

            // Auto cropping (only materialID is left)
            tmp = preprocessMaterialID(tmp);

            // If Tesseract is checked recognize
            if (tmp != null &&
                // If bestPoint matches mostCommong point
                mostCommon.p.X == bestPoint.p.X && mostCommon.p.Y == bestPoint.p.Y)
            {
                _ocr.Recognize(tmp);
                // Check if (size) result of tesseract is allowed.
                if (_ocr.GetCharactors().Length > minimumChars &&
                    _ocr.GetCharactors().Length < maximumChars)
                {
                    return _ocr.GetText();
                }
            }
            return null;
        }

        /// <summary>
        /// Autocropping of image
        ///     - Adapative threshold
        ///     - Border following
        ///     - Check if size of components matches size of a number
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        private Image<Gray, Byte> preprocessMaterialID(Image<Gray, Byte> img)
        {
            Image<Gray, Byte> preprocessed = img.Convert<Gray, Byte>();
            Image<Gray, Byte> found_contours = new Image<Gray, Byte>(img.Size);
            imgMaterialID = null;

            preprocessed = preprocessed.ThresholdAdaptive(new Gray(255), ADAPTIVE_THRESHOLD_TYPE.CV_ADAPTIVE_THRESH_MEAN_C,
                THRESH.CV_THRESH_BINARY, 33, new Gray(15));

            using (MemStorage storage = new MemStorage())
            {
                for (Contour<Point> contours = preprocessed.FindContours(); contours != null; contours = contours.HNext)
                {
                    Rectangle rect = contours.BoundingRectangle;
                    // Check if size is ok for a number
                    if (
                        rect.Width > 15 && rect.Width < 40
                        &&
                        rect.Height > 10 && rect.Height < 35)
                    {
                        found_contours.Draw(rect, new Gray(255), 3);
                    }
                }
            }

            MCvBox2D box = minAreaRect(found_contours);
            if (box.size.Width > 0 && box.size.Height > 0)
                imgMaterialID = preprocessed.Copy(box);

            return imgMaterialID;
        }

        /// <summary>
        /// Calculated minimal rect so only numbers are left.
        /// </summary>
        /// <param name="foundContours">contours find in region of the label</param>
        /// <returns>region where the numbers are found</returns>
        public MCvBox2D minAreaRect(Image<Gray, Byte> foundContours)
        {
            MCvBox2D minArea = new MCvBox2D();
            int left = foundContours.Cols, right = 0, top = foundContours.Rows, bottom = 0;
            for (int i = 0; i < foundContours.Cols; i++)
            {
                for (int j = 0; j < foundContours.Rows; j++)
                {
                    if (foundContours[j, i].Intensity == 255)
                    {
                        if (i < left)
                            left = i;
                        if (i > right)
                            right = i;
                        if (j < top)
                            top = j;
                        if (j > bottom)
                            bottom = j;
                    }
                }
            }
            if (left < right && top < bottom)
            {
                minArea.size = new Size(right - left, bottom - top);
                minArea.center = new Point((right + left) / 2, (bottom + top) / 2);
            }
            else
            {
                minArea.size = new Size(0, 0);
                minArea.center = new Point(0, 0);
            }
            return minArea;
        }
    }
}
