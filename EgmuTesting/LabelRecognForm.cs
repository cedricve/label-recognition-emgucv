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
namespace EgmuTesting
{

    public partial class LabelRecognition : Form
    {
        // Globale structuren
        private Capture cap;
        private Tesseract _ocr;
        private SearchTable searchTable;
        private PointW mostcommon;
        private Image<Bgr, byte> img;
        private bool captureInProgress;
        private Rectangle label;
        private LGD lgd;
        private Parameters parameters;
        private MCvBox2D region_of_interest;
        private string video_url = "";

        // Output scherm
        Image<Rgb, Byte> rgb_img;
        Image<Rgb, Byte> labelImage;
        Image<Gray, Byte> imgRolnummer;
        string rolnummer;
        int teller_isgelijk = 0;

        public LabelRecognition()
        {
            InitializeComponent();
            // Initialiseren van beeldverwerkingparameters
            InitializeRecognition();
            next_frame.Text = "Start!";
            trackBarAvi.Enabled = false;
            this.Show();
        }

        /*
         * 
         *  Events
         *  
        **/

        // Openen van het film bestand
        // controle of goed type en of geopend kan worden
        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            FileStream myStream = null;

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "*.wmv|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                try
                {
                    if ((myStream = (FileStream)openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            video_url = myStream.Name;
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

        // Openen van eeb afbeelding
        // controle of goed type en of geopend kan worden
        private void btnOpenImage_Click(object sender, EventArgs e)
        {

        }

        private void trackBarAvi_Scroll(object sender, EventArgs e)
        {
            cap.SetCaptureProperty(CAP_PROP.CV_CAP_PROP_POS_FRAMES, trackBarAvi.Value);
            lgd = new LGD();
            teller_isgelijk = 0;
        }

        // Starten Video, Pause Video
        private void next_frame_Click(object sender, EventArgs e)
        {
            if (cap == null && video_url != "")
            {
                try
                {
                    searchTable = new SearchTable(Int32.Parse(txtHighboundFrequency.Text));
                    mostcommon = new PointW();
                    cap = new Capture(video_url);

                    // controle als video bestand
                    // Instellen trackback
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

        // Frame handler
        private void ProcessFrame(object sender, EventArgs arg)
        {
            img = cap.QueryFrame();
            if ((object)img != null)
            {
                // Crop afbeelding met region of interest
                img = img.Copy(region_of_interest).Convert<Bgr, Byte>();

                // Voer herkenning van rolnummer uit op afbeelding
                bool herkend = executeRecognition(txtVerwacht.Text);

                // *****
                // Uitvoer op scherm tonen
                // ****

                // Clustering frame met op aangeduid gunstige positie
                // voor het rolnummer
                imageCluster.Image = labelImage;
                // Originele frame
                imageOriginal.Image = rgb_img;
                // het label -> adaptive threshold
                rolnummerLabel.Image = imgRolnummer;

                // Als Tesseract een poging gedaan heeft, 
                // toon resultaat
                if(rolnummer!=null)
                    txtTesseract.Text = rolnummer;
                
                if (herkend)
                    teller_isgelijk++;
                txtOutput.Text = teller_isgelijk.ToString();

            }
        }

        /*
         * 
         *  Beeldverwerking functies
         *  
        **/

        // Initialiseren Paramaters
        private void InitializeRecognition() {

            // Get parameters from XML file
            parameters = new Parameters(@"Z:\Bureaublad\EgmuTesting\EgmuTesting\config\parameters.xml");

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

            // Initialiseer label grootte
            label = new Rectangle();
            label.Width = Int32.Parse(txtLabelWidth.Text);
            label.Height = Int32.Parse(txtLabelHeight.Text);

            // Region of interest
            Rectangle roi_position = new Rectangle();
            roi_position.Location = new Point(Int32.Parse(txtRoiX.Text), Int32.Parse(txtRoiY.Text));
            roi_position.Width = Int32.Parse(txtRoiWidth.Text);
            roi_position.Height = Int32.Parse(txtRoiHeight.Text);
            region_of_interest = new MCvBox2D(roi_position.Location, roi_position.Size, 0);

            // Initialisatie training data tesseract & whitelist (alleen nummers zijn toegelaten) 
            _ocr = new Tesseract(@"C:\tessdata", "eng", Tesseract.OcrEngineMode.OEM_TESSERACT_ONLY);
            _ocr.SetVariable("tessedit_char_whitelist", "1234567890");

            // initialiseer instantie voor algoritme: langst gemeenschappelijke deelstring
            lgd = new LGD();

        }

        // Herkenning starten
        private bool executeRecognition(string verwacht)
        {
            // Omzetten naar grijswaarden
            Image<Gray, Byte> gray_img = img.Convert<Gray, Byte>();
            rgb_img = img.Convert<Rgb, Byte>();

            // Laplacian berekenen + Gaussiaanse kernel 
            // om ruis te vervagen + Thresholding
            Image<Gray, float> laplace = gray_img.Laplace(3);
            laplace = laplace.SmoothGaussian(3, 3, Double.Parse(txtGaussianStd1.Text), Double.Parse(txtGaussianStd2.Text));

            // Bereken threshold: rekenkundig gemiddelde grijsintensiteiten
            int threshold = berekenAdaptievethreshold();
            Image<Gray, Byte> result = laplace.ThresholdBinary(new Gray(threshold), new Gray(255)).Convert<Gray, Byte>();

            // Kernels voor eroderen en dilateren
            IntPtr kernel_s_d = CvInvoke.cvCreateStructuringElementEx(Int32.Parse(txtDilate.Text), Int32.Parse(txtDilate.Text), Int32.Parse(txtDilate.Text) / 2 - 1, Int32.Parse(txtDilate.Text) / 2 - 1, CV_ELEMENT_SHAPE.CV_SHAPE_RECT, IntPtr.Zero);
            IntPtr kernel_s_e = CvInvoke.cvCreateStructuringElementEx(Int32.Parse(txtErode.Text), Int32.Parse(txtErode.Text), Int32.Parse(txtErode.Text) / 2 - 1, Int32.Parse(txtErode.Text) / 2 - 1, CV_ELEMENT_SHAPE.CV_SHAPE_RECT, IntPtr.Zero);

            // Eroderen en dilateren
            CvInvoke.cvDilate(result.Ptr, result.Ptr, kernel_s_d, 1);
            CvInvoke.cvErode(result.Ptr, result.Ptr, kernel_s_e, 1);

            // Aanmaken afbeelding voor label
            labelImage = result.Convert<Rgb, Byte>();

            // Zoeken naar positie voor het label
            using (MemStorage storage = new MemStorage())
            {
                PointW bestpoint = new PointW();
                // Zoek de contouren in de afbeelding resultaat (Border following algoritme, Connected components)
                // Contour<Point> contours levert alle contouren op die gevonden
                // zijn in de afbeelding
                for (Contour<Point> contours = result.FindContours(); contours != null; contours = contours.HNext)
                {
                    // Construeren van een rechthoek door het
                    // bepalen van (top-left) punt en  (bottom-right) punt in
                    // de collectie punten van een contour
                    Rectangle rect = contours.BoundingRectangle;

                    // Kijken of rechthoek kan voldoen aan formaat van het label
                    if (
                        rect.Width > Int32.Parse(txtLabelWidth.Text) - Int32.Parse(txtLabelFault.Text) && rect.Width < Int32.Parse(txtLabelWidth.Text) + Int32.Parse(txtLabelFault.Text)
                        && rect.Height > Int32.Parse(txtLabelHeight.Text) - Int32.Parse(txtLabelFault.Text) && rect.Height < Int32.Parse(txtLabelHeight.Text) + Int32.Parse(txtLabelFault.Text))
                    {
                        PointF center_rect = new PointF(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
                        bestpoint.p = center_rect;
                    }
                }

                // Als er een kandidaat is voor het label
                // Tekenen we hem in het groen op de clustering afbeelding
                if (bestpoint.p.X != 0 && bestpoint.p.Y != 0)
                {
                    // We tekenen een groene box op de afbeelding van de clusters
                    // voor de kandidaat van het label
                    MCvBox2D box_G = new MCvBox2D(bestpoint.p, label.Size, 0);
                    rgb_img.Draw(box_G, new Rgb(Color.Green), 3);

                    // Zoek het dichtste punt op
                    PointW closestPoint = searchTable.findNearest(bestpoint, Int32.Parse(txtMaxDistance.Text));

                    // Als het punt dicht genoeg ligt bij het meest voorkomende punt
                    // dan passen we de coordinaten aan van het meest voorkomende punt (een rol kan lichtjes bewegen
                    // tijdens het verplaatsen waardoor de coordinaten ook lichtjes opschuiven)/
                    if (bestpoint.closeEnough(mostcommon, Int32.Parse(txtMaxDistance.Text)))
                        mostcommon = bestpoint;
                    // als het punt niet dicht genoeg ligt, dan zou het rolnummer op een andere positie gevonden zijn
                    // als de frequentie van dat punt meer is dan de frequentie van het huidige meest
                    // voorkomende dan is de waarschijnlijkheid dat het label hier ligt groter
                    // en kiezen we ervoor dat het punt ons nieuwe meest voorkomende positie wordt.
                    else if (closestPoint.frequency >= mostcommon.frequency)
                        mostcommon = closestPoint;

                }

                // We tekenen naast de kandidaat voor het label
                // ook telkens de het meest voorkomende positie (in het rood)
                // op de clustering afbeelding
                if (mostcommon.p.X != 0 && mostcommon.p.Y != 0)
                {
                    // Berekenen coordinaten voor meest voorkomende positie (eroderen kleine invloed op de grootte
                    // van het label
                    Rectangle label_position = new Rectangle();
                    int x_coord = Convert.ToInt32(mostcommon.p.X - (Int32.Parse(txtLabelWidth.Text) + Int32.Parse(txtLabelFault.Text)) / 2 - Int32.Parse(txtErode.Text) / 4);
                    int y_coord = Convert.ToInt32(mostcommon.p.Y - (Int32.Parse(txtLabelHeight.Text) + Int32.Parse(txtLabelFault.Text)) / 2 - Int32.Parse(txtErode.Text) / 4);

                    // Hou coordinaten van meest voorkomende positie 
                    // binnen de matrix
                    if (x_coord < 0)
                        x_coord = 0;
                    if (y_coord < 0)
                        y_coord = 0;
                
                    // Stel positie op voor het label
                    label_position.Location = new Point(x_coord,y_coord);
                    label_position.Width = Int32.Parse(txtLabelWidth.Text) + Int32.Parse(txtLabelFault.Text) + Int32.Parse(txtErode.Text) / 2;
                    label_position.Height = Int32.Parse(txtLabelHeight.Text) + Int32.Parse(txtLabelFault.Text) + Int32.Parse(txtErode.Text) / 2;

                    // Teken de box op de clustering frame (rood)
                    MCvBox2D label_box = new MCvBox2D(mostcommon.p, label_position.Size, 0);
                    rgb_img.Draw(label_box, new Rgb(Color.Red), 3);

                    // Knip de gunstige positie uit voor het label en herken
                    rolnummer = herkenRolnummer(label_box, img, bestpoint);
                    lgd.Equal(rolnummer, verwacht);
                    return lgd.isgelijk;
                }
            }
            return false;
        }

        // Bereken een adaptieve threshold op basis
        // van een deelgebied in de afbeelding
        private int berekenAdaptievethreshold() {
            Image<Gray, Byte> img_gray = img.Convert<Gray, Byte>();
            return (int)img_gray.GetAverage().Intensity+80;
        }

        // Herken het rolnummer aan de hand van Tesseract
        // - Knip gebied (waar rolnummer gevonden is) uit, in origenele afbeelding.
        // - Herken het rolnummer
        private string herkenRolnummer(MCvBox2D label_box, Image<Bgr, Byte> img, PointW bestpoint)
        {
            // Knip de gunstige positie uit voor het label
            // en toon in een aparte picture box
            Image<Gray, Byte> tmp = img.Copy(label_box).Convert<Gray, Byte>();
            tmp = tmp.Resize(120, 80, INTER.CV_INTER_LINEAR);
            
            // Preprocessen rolnummer
            tmp = preprocessRolnummer(tmp);

            // Herken rolnnumer
            if (chkTesseract.Checked && tmp != null) // mostcommon.p.X == bestpoint.p.X && mostcommon.p.Y == bestpoint.p.Y)
            {
                _ocr.Recognize(tmp);
                // Als aantal karakters binnen bereik liggen
                // dan is het een mogelijk resultaat
                if (_ocr.GetCharactors().Length > Int32.Parse(txtMinimumChars.Text) &&
                    _ocr.GetCharactors().Length < Int32.Parse(txtMaximumChars.Text))
                {
                    return _ocr.GetText();
                }
            }
            return null;
        }

        // Preprocess rolnummer
        // - adaptieve thresholding
        // - automatisch croppen
        private Image<Gray, Byte> preprocessRolnummer(Image<Gray, Byte> img)
        {
            Image<Gray, Byte> preprocessed = img.Convert<Gray, Byte>();
            Image<Gray, Byte> found_contours = new Image<Gray, Byte>(img.Size);
            imgRolnummer = null;

            // Adaptieve thresholding, gemiddelde waarden
            preprocessed = preprocessed.ThresholdAdaptive(new Gray(255), ADAPTIVE_THRESHOLD_TYPE.CV_ADAPTIVE_THRESH_MEAN_C,
                THRESH.CV_THRESH_BINARY, 33, new Gray(15));

            // automatisch croppen
            using (MemStorage storage = new MemStorage())
            {
                for (Contour<Point> contours = preprocessed.FindContours(); contours != null; contours = contours.HNext)
                {
                    Rectangle rect = contours.BoundingRectangle;
                    // Een getal heeft een bepaalde grootte
                    // filteren op deze eigenschap
                    if (
                        rect.Width > 15  && rect.Width < 40  
                        && 
                        rect.Height > 10 && rect.Height < 35)
                    {
                        found_contours.Draw(rect, new Gray(255), 3);
                    }
                }
            }

            // Bereken minimaal gebied waar alle gevonden contouren in liggen
            // Hierdoor zijn enkel nog de mogelijke kandidaten voor een getal
            // zichtbaar
            MCvBox2D box = minAreaRect(found_contours);
            if (box.size.Width > 0 && box.size.Height > 0)
                imgRolnummer = preprocessed.Copy(box);

            return imgRolnummer;
        }

        // Bepaal een zo klein mogelijk gebied waarin
        // alle componeten lgigen
        public MCvBox2D minAreaRect(Image<Gray, Byte> found_contours) {
            MCvBox2D min_area = new MCvBox2D();
            int links = found_contours.Cols, rechts = 0, boven = found_contours.Rows, onder = 0;
            for (int i = 0; i < found_contours.Cols; i++)
            {
                for (int j = 0; j < found_contours.Rows; j++)
                {
                    // Als de pixel een component pixel is
                    // controleer of meeste bovenste, meeste onderste, 
                    // meeste linkse of meest rechtse
                    if (found_contours[j, i].Intensity == 255) {
                        if (i < links)
                            links = i;
                        if (i > rechts)
                            rechts = i;
                        if (j < boven)
                            boven = j;
                        if (j > onder)
                            onder = j;
                    }
                }
            }
            // Kijk of er componenten zijn gevonden
            if (links < rechts && boven < onder)
            {
                min_area.size = new Size(rechts - links, onder - boven);
                min_area.center = new Point((rechts + links) / 2, (onder + boven) / 2);
            }
            else {
                min_area.size = new Size(0,0);
                min_area.center = new Point(0,0);
            }
            return min_area;        
        }
    }
}
