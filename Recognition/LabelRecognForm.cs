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
        private Recognition recognition;
        private string tessdataDir;
        private string tessdataLang;
        private Capture cap;
        private Image<Bgr, byte> img;
        private bool captureInProgress;
        private Property properties;
        private string filePath = "";
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
            recognition.mostCommon = new PointW();
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
                // Process image and try to recognize the expected materialID
                bool isRecognized = recognition.executeRecognition("DEBUG ONLY",img,"DEBUG ONLY",txtExpected.Text);

                // *****
                // Show results on output screen
                // ****

                // Clustering frame
                imageCluster.Image = recognition.labelImage;
                // Original frame
                imageOriginal.Image = recognition.rgbImg;
                // The label after adaptive thresholding
                // (the image that's evaluated by tesseract)
                rolnummerLabel.Image = recognition.imgMaterialID;

                if (chkTesseract.Checked)
                {
                    // If tesseract recognized something, show it on the screen
                    if (recognition.detectedNumber != null && recognition.detectedNumber != "")
                        txtTesseract.Text = recognition.detectedNumber;

                    if (isRecognized)
                        counterRecognized++;

                    int isEqualThreshold = 10;
                    // When teller >= winning threshold
                    if (counterRecognized >= isEqualThreshold)
                        txtOutput.Text = "it's ok";
                }
            }
        }

        /// <summary>
        /// Initialisation of all parameters
        /// </summary>
        private void InitializeRecognition() {
            next_frame.Text = "Start!";
            trackBarAvi.Enabled = false;

            // Get parameters from XML file
            properties = new Property(@"Z:\Bureaublad\recognition_xp\Recognition\config\parameters.xml");
            
            // Pass parameters to Recognition
            tessdataDir = @"C:\tessdata";
            tessdataLang = "eng";
            recognition = new Recognition(properties,tessdataDir,tessdataLang);

            // Initialize textfields
            txtDilate.Text = (string)properties.get("dilate");
            txtErode.Text = (string)properties.get("erode");
            txtLabelHeight.Text = (string)properties.get("label_height");
            txtLabelWidth.Text = (string)properties.get("label_width");
            txtLabelFault.Text = (string)properties.get("label_fault");
            txtGaussianStd1.Text = (string)properties.get("gaussian_smt");
            txtGaussianStd2.Text = (string)properties.get("gaussian_smt2");
            txtHighboundFrequency.Text = (string)properties.get("upper_limit_frequency");
            txtMaxDistance.Text = (string)properties.get("max_distance");
            txtMinimumChars.Text = (string)properties.get("min_char");
            txtMaximumChars.Text = (string)properties.get("max_char");
            txtRoiX.Text = (string)properties.get("roi_x");
            txtRoiY.Text = (string)properties.get("roi_y");
            txtRoiWidth.Text = (string)properties.get("roi_width");
            txtRoiHeight.Text = (string)properties.get("roi_height");
        }

        private void updateRecognition(object sender, EventArgs e)
        {
            TextBox field = (TextBox) sender;
            properties.set(field.Tag.ToString(), field.Text);
            recognition.updateProperties(properties);
        }
    }
}
