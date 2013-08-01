using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.CvEnum;
using Emgu.CV.UI;
using Emgu.CV.Structure;
using Emgu.CV.OCR;
using Emgu.CV.GPU;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using System.Xml;
namespace Recognition
{

    public partial class LabelRecognition : Form
    {
        // Global structure.
        private Capture cap;
        private Tesseract _ocr;
        private SearchTable searchTable;
        private PointW mostCommon;
        private Image<Bgr, byte> img;
        private bool captureInProgress;
        private Rectangle label;
        private LGD lgd;
        private Parameters parameters;
        private MCvBox2D regionOfInterest;
        private string filePath = "";

        // Processed images for output screen.
        Image<Rgb, Byte> rgbImg;
        Image<Rgb, Byte> labelImage;
        Image<Gray, Byte> imgMaterialID;
        string materialID;
        int counterRecognized = 0;

        public LabelRecognition()
        {
            InitializeComponent();
            // Initialize all parameters
            InitializeRecognition();
            this.Show();
        }

        /// <summary>
        /// Event: (Button click) open image or video.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            FileStream myStream = null;

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                try
                {
                    if ((myStream = (FileStream)openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            filePath = myStream.Name;
                            captureInProgress = false;
                            next_frame.Text = "Start";
                            Application.Idle -= ProcessFrame;
                            cap = null;
                            trackBarAvi.Value = 0;
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
        }

        /// <summary>
        /// Event: when a video is opened, you can search for a specific frame.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trackBarAvi_Scroll(object sender, EventArgs e)
        {
            cap.SetCaptureProperty(CAP_PROP.CV_CAP_PROP_POS_FRAMES, trackBarAvi.Value);
            lgd = new LGD();
            counterRecognized = 0;
        }

        /// <summary>
        /// Event: (Button click) pause or start video.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void next_frame_Click(object sender, EventArgs e)
        {
            if (cap == null && filePath != "")
            {
                try
                {
                    searchTable = new SearchTable(Int32.Parse(txtHighboundFrequency.Text));
                    mostCommon = new PointW();
                    cap = new Capture(filePath);
                    trackBarAvi.Maximum = Convert.ToInt32(cap.GetCaptureProperty(CAP_PROP.CV_CAP_PROP_FRAME_COUNT));
                    trackBarAvi.Minimum = 0;
                    trackBarAvi.TickFrequency = 500;
                    trackBarAvi.TickStyle = TickStyle.Both;
                    trackBarAvi.Enabled = true;

                }
                catch (NullReferenceException excpt)
                {
                    MessageBox.Show(excpt.Message);
                }
            }
            if (cap != null)
            {
                if (captureInProgress)
                {
                    next_frame.Text = "Start";
                    Application.Idle -= ProcessFrame;
                }
                else
                {
                    next_frame.Text = "Pause";
                    Application.Idle += ProcessFrame;
                }
                captureInProgress = !captureInProgress;
            }
        }

        /// <summary>
        /// Start recognition of an image. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="arg"></param>
        private void ProcessFrame(object sender, EventArgs arg)
        {
            img = cap.QueryFrame();
            if ((object)img != null)
            {
                // Crop image with region of interest
                img = img.Copy(regionOfInterest).Convert<Bgr, Byte>();

                // Process image and try to recognize the expected materialID
                bool isRecognized = executeRecognition(txtExpected.Text);

                // *****
                // Show results on output screen
                // ****

                // Clustering frame
                imageCluster.Image = labelImage;
                // Original frame
                imageOriginal.Image = rgbImg;
                // The label after adaptive thresholding
                // (the image that's evaluated by tesseract)
                rolnummerLabel.Image = imgMaterialID;

                // If tesseract recognized something, show it on the screen
                if (materialID != null && materialID != "")
                    txtTesseract.Text = materialID;
                
                if (isRecognized)
                    counterRecognized++;

                int isEqualThreshold = 10;
                // When teller >= winning threshold
                if (counterRecognized >= isEqualThreshold)
                     txtOutput.Text = "it's ok";

            }
        }

        /// <summary>
        /// Initialisation of all parameters
        /// </summary>
        private void InitializeRecognition() {
            next_frame.Text = "Start!";
            trackBarAvi.Enabled = false;

            // Get parameters from XML file
            parameters = new Parameters(@"W:\Clipbrd\LabelRecognition\LabelRecognitionTool\Recognition\config\parameters.xml");

            txtDilate.Text = (string)parameters.get("dilate");
            txtErode.Text = (string)parameters.get("erode");
            txtLabelHeight.Text = (string)parameters.get("label_height");
            txtLabelWidth.Text = (string)parameters.get("label_width");
            txtLabelFault.Text = (string)parameters.get("label_fault");
            txtGaussianStd1.Text = (string)parameters.get("gaussuan_smt");
            txtGaussianStd2.Text = (string)parameters.get("gaussuan_smt2");
            txtHighboundFrequency.Text = (string)parameters.get("highbound_frequency");
            txtMaxDistance.Text = (string)parameters.get("max_distance_between_points");
            txtMinimumChars.Text = (string)parameters.get("min_char");
            txtMaximumChars.Text = (string)parameters.get("max_char");
            txtRoiX.Text = (string)parameters.get("roi_x");
            txtRoiY.Text = (string)parameters.get("roi_y");
            txtRoiWidth.Text = (string)parameters.get("roi_width");
            txtRoiHeight.Text = (string)parameters.get("roi_height");

            // Initialisation (expected) size of label
            label = new Rectangle();
            label.Width = Int32.Parse(txtLabelWidth.Text);
            label.Height = Int32.Parse(txtLabelHeight.Text);

            // Region of interest
            Rectangle roiPosition = new Rectangle();
            roiPosition.Location = new Point(Int32.Parse(txtRoiX.Text), Int32.Parse(txtRoiY.Text));
            roiPosition.Width = Int32.Parse(txtRoiWidth.Text);
            roiPosition.Height = Int32.Parse(txtRoiHeight.Text);
            regionOfInterest = new MCvBox2D(roiPosition.Location, roiPosition.Size, 0);

            // Initialisation training data tesseract & whitelist (only numbers are allowed) 
            _ocr = new Tesseract(@"C:\tessdata", "eng", Tesseract.OcrEngineMode.OEM_TESSERACT_ONLY);
            _ocr.SetVariable("tessedit_char_whitelist", "1234567890");

            lgd = new LGD();
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
        private bool executeRecognition(string expectedMaterialID)
        {
            // reset LGD 
            lgd.isEqual = false;

            // Convert to grayscale
            Image<Gray, Byte> gray_img = img.Convert<Gray, Byte>();
            rgbImg = img.Convert<Rgb, Byte>();

            // Laplacian of Gaussiaanse ( second order derivative)
            Image<Gray, float> laplace = gray_img.Laplace(3);
            laplace = laplace.SmoothGaussian(3, 3, Double.Parse(txtGaussianStd1.Text), Double.Parse(txtGaussianStd2.Text));

            // Threshold with mean of the graylevel intensities
            int threshold = adaptiveThresold();
            Image<Gray, Byte> result = laplace.ThresholdBinary(new Gray(threshold), new Gray(255)).Convert<Gray, Byte>();

            // Closing operation
            // First dilate and then eroding
            IntPtr kernel_s_d = CvInvoke.cvCreateStructuringElementEx(Int32.Parse(txtDilate.Text), Int32.Parse(txtDilate.Text), Int32.Parse(txtDilate.Text) / 2 - 1, Int32.Parse(txtDilate.Text) / 2 - 1, CV_ELEMENT_SHAPE.CV_SHAPE_RECT, IntPtr.Zero);
            IntPtr kernel_s_e = CvInvoke.cvCreateStructuringElementEx(Int32.Parse(txtErode.Text), Int32.Parse(txtErode.Text), Int32.Parse(txtErode.Text) / 2 - 1, Int32.Parse(txtErode.Text) / 2 - 1, CV_ELEMENT_SHAPE.CV_SHAPE_RECT, IntPtr.Zero);
            CvInvoke.cvDilate(result.Ptr, result.Ptr, kernel_s_d, 1);
            CvInvoke.cvErode(result.Ptr, result.Ptr, kernel_s_e, 1);

            // Aanmaken afbeelding voor label
            labelImage = result.Convert<Rgb, Byte>();

            // Search for position of label
            using (MemStorage storage = new MemStorage())
            {
                PointW bestPoint = new PointW();
                Rectangle currentBest = new Rectangle();
                // Start border following: get the connected components (Contour<Point> contours)
                for (Contour<Point> contours = result.FindContours(); contours != null; contours = contours.HNext)
                {
                    // Construct a rectangle
                    // (top-left) point en  (bottom-right) point
                    Rectangle rect = contours.BoundingRectangle;

                    // Check if the rectangle matches the expected size of label
                    if (
                        rect.Width > Int32.Parse(txtLabelWidth.Text) - Int32.Parse(txtLabelFault.Text) && rect.Width < Int32.Parse(txtLabelWidth.Text) + Int32.Parse(txtLabelFault.Text)
                        && rect.Height > Int32.Parse(txtLabelHeight.Text) - Int32.Parse(txtLabelFault.Text) && rect.Height < Int32.Parse(txtLabelHeight.Text) + Int32.Parse(txtLabelFault.Text)
                        // If better then currentBest
                        && currentBest.Width < rect.Width && current.Height < rect.Height)
                        )
                    {
                        bestPoint.p = new PointF(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
                        currentBest = rect;
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
                    PointW mostCommonPoint = searchTable.findMostCommonPoint(bestPoint, Int32.Parse(txtMaxDistance.Text));

                    // If close enough to the mostcommon point, change the position of the mostcommon point.
                    // ( the cranes can move slightly)
                    if (bestPoint.closeEnough(mostCommon, Int32.Parse(txtMaxDistance.Text)))
                        mostCommon = bestPoint;
                    // If nog close enough the bestpoint is added to the searchTable or a point his frequency 
                    // is incremented => check if that point has a larger frequency than the mostcommon point.
                    else if (mostCommonPoint.frequency >= mostCommon.frequency)
                            mostCommon = mostCommonPoint;
                }

                // Draw the region of the mostCommon point (red rect)
                if (mostCommon.p.X != 0 && mostCommon.p.Y != 0)
                {
                    // Calculate the size of the label at the mostcommon point
                    // (eroding en dilating have influence on the size of the located label)
                    Rectangle labelPosition = new Rectangle();
                    int x_coord = Convert.ToInt32(mostCommon.p.X - (Int32.Parse(txtLabelWidth.Text) + Int32.Parse(txtLabelFault.Text)) / 2 - Int32.Parse(txtErode.Text) / 4);
                    int y_coord = Convert.ToInt32(mostCommon.p.Y - (Int32.Parse(txtLabelHeight.Text) + Int32.Parse(txtLabelFault.Text)) / 2 - Int32.Parse(txtErode.Text) / 4);

                    if (x_coord < 0)
                        x_coord = 0;
                    if (y_coord < 0)
                        y_coord = 0;
                
                    labelPosition.Location = new Point(x_coord,y_coord);
                    labelPosition.Width = Int32.Parse(txtLabelWidth.Text) + Int32.Parse(txtLabelFault.Text) + Int32.Parse(txtErode.Text) / 2;
                    labelPosition.Height = Int32.Parse(txtLabelHeight.Text) + Int32.Parse(txtLabelFault.Text) + Int32.Parse(txtErode.Text) / 2;

                    MCvBox2D labelRect = new MCvBox2D(mostCommon.p, labelPosition.Size, 0);
                    rgbImg.Draw(labelRect, new Rgb(Color.Red), 3);

                    // Execute Tesseract
                    materialID = recognizeMaterialID(labelRect, img, bestPoint);
                    lgd.Equal(materialID, expectedMaterialID);
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
        private int adaptiveThresold() {
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
            if (chkTesseract.Checked && tmp != null && 
                // If bestPoint matches mostCommong point
                mostCommon.p.X == bestPoint.p.X && mostCommon.p.Y == bestPoint.p.Y)
            {
                _ocr.Recognize(tmp);
                // Check if (size) result of tesseract is allowed.
                if (_ocr.GetCharactors().Length > Int32.Parse(txtMinimumChars.Text) &&
                    _ocr.GetCharactors().Length < Int32.Parse(txtMaximumChars.Text))
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
                        rect.Width > 15  && rect.Width < 40  
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
        public MCvBox2D minAreaRect(Image<Gray, Byte> foundContours) {
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
            else {
                minArea.size = new Size(0, 0);
                minArea.center = new Point(0, 0);
            }
            return minArea;        
        }
    }
}
