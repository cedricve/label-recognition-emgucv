namespace Recognition
{
    partial class LabelRecognition
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.imageCluster = new Emgu.CV.UI.ImageBox();
            this.next_frame = new System.Windows.Forms.Button();
            this.imageOriginal = new Emgu.CV.UI.ImageBox();
            this.lblDilate = new System.Windows.Forms.Label();
            this.txtDilate = new System.Windows.Forms.TextBox();
            this.lblErode = new System.Windows.Forms.Label();
            this.txtErode = new System.Windows.Forms.TextBox();
            this.txtLabelHeight = new System.Windows.Forms.TextBox();
            this.txtLabelWidth = new System.Windows.Forms.TextBox();
            this.lblLabelWidth = new System.Windows.Forms.Label();
            this.lblLabelHeight = new System.Windows.Forms.Label();
            this.lblLabelFault = new System.Windows.Forms.Label();
            this.txtLabelFault = new System.Windows.Forms.TextBox();
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.rolnummerLabel = new Emgu.CV.UI.ImageBox();
            this.txtNumber = new System.Windows.Forms.Label();
            this.trackBarAvi = new System.Windows.Forms.TrackBar();
            this.txtTesseract = new System.Windows.Forms.TextBox();
            this.chkTesseract = new System.Windows.Forms.CheckBox();
            this.txtGaussianStd1 = new System.Windows.Forms.TextBox();
            this.txtGaussianStd2 = new System.Windows.Forms.TextBox();
            this.lblGaussianStd1 = new System.Windows.Forms.Label();
            this.lblGaussianStd2 = new System.Windows.Forms.Label();
            this.lblImageProcessing = new System.Windows.Forms.Label();
            this.lblTesseractParameters = new System.Windows.Forms.Label();
            this.lblMaximumChars = new System.Windows.Forms.Label();
            this.lblMinChar = new System.Windows.Forms.Label();
            this.txtMaximumChars = new System.Windows.Forms.TextBox();
            this.txtMinimumChars = new System.Windows.Forms.TextBox();
            this.lblMemoryParameters = new System.Windows.Forms.Label();
            this.lblHighboundFrequency = new System.Windows.Forms.Label();
            this.lblMaxDistance = new System.Windows.Forms.Label();
            this.txtHighboundFrequency = new System.Windows.Forms.TextBox();
            this.txtMaxDistance = new System.Windows.Forms.TextBox();
            this.Herkenning = new System.Windows.Forms.Label();
            this.txtExpected = new System.Windows.Forms.TextBox();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.roi = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtRoiX = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtRoiY = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtRoiWidth = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtRoiHeight = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.imageCluster)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageOriginal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rolnummerLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarAvi)).BeginInit();
            this.SuspendLayout();
            // 
            // imageCluster
            // 
            this.imageCluster.Location = new System.Drawing.Point(12, 196);
            this.imageCluster.Name = "imageCluster";
            this.imageCluster.Size = new System.Drawing.Size(542, 336);
            this.imageCluster.TabIndex = 2;
            this.imageCluster.TabStop = false;
            // 
            // next_frame
            // 
            this.next_frame.Location = new System.Drawing.Point(1068, 58);
            this.next_frame.Name = "next_frame";
            this.next_frame.Size = new System.Drawing.Size(100, 38);
            this.next_frame.TabIndex = 3;
            this.next_frame.Text = "button1";
            this.next_frame.UseVisualStyleBackColor = true;
            this.next_frame.Click += new System.EventHandler(this.next_frame_Click);
            // 
            // imageOriginal
            // 
            this.imageOriginal.Location = new System.Drawing.Point(721, 196);
            this.imageOriginal.Name = "imageOriginal";
            this.imageOriginal.Size = new System.Drawing.Size(447, 336);
            this.imageOriginal.TabIndex = 2;
            this.imageOriginal.TabStop = false;
            // 
            // lblDilate
            // 
            this.lblDilate.AutoSize = true;
            this.lblDilate.Location = new System.Drawing.Point(12, 46);
            this.lblDilate.Name = "lblDilate";
            this.lblDilate.Size = new System.Drawing.Size(37, 13);
            this.lblDilate.TabIndex = 5;
            this.lblDilate.Text = "Dilate:";
            // 
            // txtDilate
            // 
            this.txtDilate.Location = new System.Drawing.Point(77, 43);
            this.txtDilate.Name = "txtDilate";
            this.txtDilate.Size = new System.Drawing.Size(100, 20);
            this.txtDilate.TabIndex = 6;
            this.txtDilate.Tag = "dilate";
            this.txtDilate.TextChanged += new System.EventHandler(this.updateRecognition);
            // 
            // lblErode
            // 
            this.lblErode.AutoSize = true;
            this.lblErode.Location = new System.Drawing.Point(12, 77);
            this.lblErode.Name = "lblErode";
            this.lblErode.Size = new System.Drawing.Size(38, 13);
            this.lblErode.TabIndex = 8;
            this.lblErode.Text = "Erode:";
            // 
            // txtErode
            // 
            this.txtErode.Location = new System.Drawing.Point(77, 74);
            this.txtErode.Name = "txtErode";
            this.txtErode.Size = new System.Drawing.Size(100, 20);
            this.txtErode.TabIndex = 9;
            this.txtErode.Tag = "erode";
            this.txtErode.TextChanged += new System.EventHandler(this.updateRecognition);
            // 
            // txtLabelHeight
            // 
            this.txtLabelHeight.Location = new System.Drawing.Point(272, 74);
            this.txtLabelHeight.Name = "txtLabelHeight";
            this.txtLabelHeight.Size = new System.Drawing.Size(100, 20);
            this.txtLabelHeight.TabIndex = 12;
            this.txtLabelHeight.Tag = "label_height";
            this.txtLabelHeight.TextChanged += new System.EventHandler(this.updateRecognition);
            // 
            // txtLabelWidth
            // 
            this.txtLabelWidth.Location = new System.Drawing.Point(272, 43);
            this.txtLabelWidth.Name = "txtLabelWidth";
            this.txtLabelWidth.Size = new System.Drawing.Size(100, 20);
            this.txtLabelWidth.TabIndex = 13;
            this.txtLabelWidth.Tag = "label_width";
            this.txtLabelWidth.TextChanged += new System.EventHandler(this.updateRecognition);
            // 
            // lblLabelWidth
            // 
            this.lblLabelWidth.AutoSize = true;
            this.lblLabelWidth.Location = new System.Drawing.Point(188, 46);
            this.lblLabelWidth.Name = "lblLabelWidth";
            this.lblLabelWidth.Size = new System.Drawing.Size(64, 13);
            this.lblLabelWidth.TabIndex = 14;
            this.lblLabelWidth.Text = "Label width:";
            // 
            // lblLabelHeight
            // 
            this.lblLabelHeight.AutoSize = true;
            this.lblLabelHeight.Location = new System.Drawing.Point(188, 77);
            this.lblLabelHeight.Name = "lblLabelHeight";
            this.lblLabelHeight.Size = new System.Drawing.Size(68, 13);
            this.lblLabelHeight.TabIndex = 15;
            this.lblLabelHeight.Text = "Label height:";
            // 
            // lblLabelFault
            // 
            this.lblLabelFault.AutoSize = true;
            this.lblLabelFault.Location = new System.Drawing.Point(188, 110);
            this.lblLabelFault.Name = "lblLabelFault";
            this.lblLabelFault.Size = new System.Drawing.Size(59, 13);
            this.lblLabelFault.TabIndex = 16;
            this.lblLabelFault.Text = "Label fault:";
            // 
            // txtLabelFault
            // 
            this.txtLabelFault.Location = new System.Drawing.Point(272, 107);
            this.txtLabelFault.Name = "txtLabelFault";
            this.txtLabelFault.Size = new System.Drawing.Size(100, 20);
            this.txtLabelFault.TabIndex = 17;
            this.txtLabelFault.Tag = "label_height";
            this.txtLabelFault.TextChanged += new System.EventHandler(this.updateRecognition);
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Location = new System.Drawing.Point(1068, 12);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(100, 40);
            this.btnOpenFile.TabIndex = 18;
            this.btnOpenFile.Text = "Open File";
            this.btnOpenFile.UseVisualStyleBackColor = true;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // rolnummerLabel
            // 
            this.rolnummerLabel.Location = new System.Drawing.Point(573, 196);
            this.rolnummerLabel.Name = "rolnummerLabel";
            this.rolnummerLabel.Size = new System.Drawing.Size(127, 69);
            this.rolnummerLabel.TabIndex = 19;
            this.rolnummerLabel.TabStop = false;
            // 
            // txtNumber
            // 
            this.txtNumber.AutoSize = true;
            this.txtNumber.Location = new System.Drawing.Point(1101, 117);
            this.txtNumber.Name = "txtNumber";
            this.txtNumber.Size = new System.Drawing.Size(0, 13);
            this.txtNumber.TabIndex = 21;
            // 
            // trackBarAvi
            // 
            this.trackBarAvi.Location = new System.Drawing.Point(12, 141);
            this.trackBarAvi.Name = "trackBarAvi";
            this.trackBarAvi.Size = new System.Drawing.Size(558, 45);
            this.trackBarAvi.TabIndex = 22;
            this.trackBarAvi.Scroll += new System.EventHandler(this.trackBarAvi_Scroll);
            // 
            // txtTesseract
            // 
            this.txtTesseract.Location = new System.Drawing.Point(573, 347);
            this.txtTesseract.Multiline = true;
            this.txtTesseract.Name = "txtTesseract";
            this.txtTesseract.Size = new System.Drawing.Size(127, 44);
            this.txtTesseract.TabIndex = 25;
            // 
            // chkTesseract
            // 
            this.chkTesseract.AutoSize = true;
            this.chkTesseract.Location = new System.Drawing.Point(574, 291);
            this.chkTesseract.Name = "chkTesseract";
            this.chkTesseract.Size = new System.Drawing.Size(73, 17);
            this.chkTesseract.TabIndex = 26;
            this.chkTesseract.Text = "Tesseract";
            this.chkTesseract.UseVisualStyleBackColor = true;
            // 
            // txtGaussianStd1
            // 
            this.txtGaussianStd1.Location = new System.Drawing.Point(470, 43);
            this.txtGaussianStd1.Name = "txtGaussianStd1";
            this.txtGaussianStd1.Size = new System.Drawing.Size(100, 20);
            this.txtGaussianStd1.TabIndex = 27;
            this.txtGaussianStd1.Tag = "gaussuan_smt";
            this.txtGaussianStd1.TextChanged += new System.EventHandler(this.updateRecognition);
            // 
            // txtGaussianStd2
            // 
            this.txtGaussianStd2.Location = new System.Drawing.Point(470, 74);
            this.txtGaussianStd2.Name = "txtGaussianStd2";
            this.txtGaussianStd2.Size = new System.Drawing.Size(100, 20);
            this.txtGaussianStd2.TabIndex = 28;
            this.txtGaussianStd2.Tag = "gaussuan_smt2";
            this.txtGaussianStd2.TextChanged += new System.EventHandler(this.updateRecognition);
            // 
            // lblGaussianStd1
            // 
            this.lblGaussianStd1.AutoSize = true;
            this.lblGaussianStd1.Location = new System.Drawing.Point(387, 46);
            this.lblGaussianStd1.Name = "lblGaussianStd1";
            this.lblGaussianStd1.Size = new System.Drawing.Size(77, 13);
            this.lblGaussianStd1.TabIndex = 29;
            this.lblGaussianStd1.Text = "Gaussian std1:";
            // 
            // lblGaussianStd2
            // 
            this.lblGaussianStd2.AutoSize = true;
            this.lblGaussianStd2.Location = new System.Drawing.Point(387, 77);
            this.lblGaussianStd2.Name = "lblGaussianStd2";
            this.lblGaussianStd2.Size = new System.Drawing.Size(74, 13);
            this.lblGaussianStd2.TabIndex = 30;
            this.lblGaussianStd2.Text = "Gaussian std2";
            // 
            // lblImageProcessing
            // 
            this.lblImageProcessing.AutoSize = true;
            this.lblImageProcessing.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblImageProcessing.Location = new System.Drawing.Point(8, 9);
            this.lblImageProcessing.Name = "lblImageProcessing";
            this.lblImageProcessing.Size = new System.Drawing.Size(235, 20);
            this.lblImageProcessing.TabIndex = 31;
            this.lblImageProcessing.Text = "Image Processing Parameters";
            // 
            // lblTesseractParameters
            // 
            this.lblTesseractParameters.AutoSize = true;
            this.lblTesseractParameters.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTesseractParameters.Location = new System.Drawing.Point(584, 107);
            this.lblTesseractParameters.Name = "lblTesseractParameters";
            this.lblTesseractParameters.Size = new System.Drawing.Size(176, 20);
            this.lblTesseractParameters.TabIndex = 32;
            this.lblTesseractParameters.Text = "Tesseract Parameters";
            // 
            // lblMaximumChars
            // 
            this.lblMaximumChars.AutoSize = true;
            this.lblMaximumChars.Location = new System.Drawing.Point(585, 173);
            this.lblMaximumChars.Name = "lblMaximumChars";
            this.lblMaximumChars.Size = new System.Drawing.Size(84, 13);
            this.lblMaximumChars.TabIndex = 36;
            this.lblMaximumChars.Text = "Maximum Chars:";
            // 
            // lblMinChar
            // 
            this.lblMinChar.AutoSize = true;
            this.lblMinChar.Location = new System.Drawing.Point(585, 142);
            this.lblMinChar.Name = "lblMinChar";
            this.lblMinChar.Size = new System.Drawing.Size(81, 13);
            this.lblMinChar.TabIndex = 35;
            this.lblMinChar.Text = "Minimum Chars:";
            // 
            // txtMaximumChars
            // 
            this.txtMaximumChars.Location = new System.Drawing.Point(711, 170);
            this.txtMaximumChars.Name = "txtMaximumChars";
            this.txtMaximumChars.Size = new System.Drawing.Size(100, 20);
            this.txtMaximumChars.TabIndex = 34;
            this.txtMaximumChars.Tag = "max_char";
            this.txtMaximumChars.TextChanged += new System.EventHandler(this.updateRecognition);
            // 
            // txtMinimumChars
            // 
            this.txtMinimumChars.Location = new System.Drawing.Point(711, 139);
            this.txtMinimumChars.Name = "txtMinimumChars";
            this.txtMinimumChars.Size = new System.Drawing.Size(100, 20);
            this.txtMinimumChars.TabIndex = 33;
            this.txtMinimumChars.Tag = "min_char";
            this.txtMinimumChars.TextChanged += new System.EventHandler(this.updateRecognition);
            // 
            // lblMemoryParameters
            // 
            this.lblMemoryParameters.AutoSize = true;
            this.lblMemoryParameters.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMemoryParameters.Location = new System.Drawing.Point(584, 9);
            this.lblMemoryParameters.Name = "lblMemoryParameters";
            this.lblMemoryParameters.Size = new System.Drawing.Size(161, 20);
            this.lblMemoryParameters.TabIndex = 37;
            this.lblMemoryParameters.Text = "Memory Parameters";
            // 
            // lblHighboundFrequency
            // 
            this.lblHighboundFrequency.AutoSize = true;
            this.lblHighboundFrequency.Location = new System.Drawing.Point(585, 77);
            this.lblHighboundFrequency.Name = "lblHighboundFrequency";
            this.lblHighboundFrequency.Size = new System.Drawing.Size(115, 13);
            this.lblHighboundFrequency.TabIndex = 41;
            this.lblHighboundFrequency.Text = "Highbound Frequency:";
            // 
            // lblMaxDistance
            // 
            this.lblMaxDistance.AutoSize = true;
            this.lblMaxDistance.Location = new System.Drawing.Point(585, 46);
            this.lblMaxDistance.Name = "lblMaxDistance";
            this.lblMaxDistance.Size = new System.Drawing.Size(75, 13);
            this.lblMaxDistance.TabIndex = 40;
            this.lblMaxDistance.Text = "Max Distance:";
            this.lblMaxDistance.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtHighboundFrequency
            // 
            this.txtHighboundFrequency.Location = new System.Drawing.Point(711, 74);
            this.txtHighboundFrequency.Name = "txtHighboundFrequency";
            this.txtHighboundFrequency.Size = new System.Drawing.Size(100, 20);
            this.txtHighboundFrequency.TabIndex = 39;
            this.txtHighboundFrequency.Tag = "highbound_frequency";
            this.txtHighboundFrequency.TextChanged += new System.EventHandler(this.updateRecognition);
            // 
            // txtMaxDistance
            // 
            this.txtMaxDistance.Location = new System.Drawing.Point(711, 43);
            this.txtMaxDistance.Name = "txtMaxDistance";
            this.txtMaxDistance.Size = new System.Drawing.Size(100, 20);
            this.txtMaxDistance.TabIndex = 38;
            this.txtMaxDistance.Tag = "max_distance_between_points";
            this.txtMaxDistance.TextChanged += new System.EventHandler(this.updateRecognition);
            // 
            // Herkenning
            // 
            this.Herkenning.AutoSize = true;
            this.Herkenning.Location = new System.Drawing.Point(570, 320);
            this.Herkenning.Name = "Herkenning";
            this.Herkenning.Size = new System.Drawing.Size(87, 13);
            this.Herkenning.TabIndex = 43;
            this.Herkenning.Text = "Tesseract Result";
            // 
            // txtExpected
            // 
            this.txtExpected.Location = new System.Drawing.Point(573, 420);
            this.txtExpected.Multiline = true;
            this.txtExpected.Name = "txtExpected";
            this.txtExpected.Size = new System.Drawing.Size(127, 34);
            this.txtExpected.TabIndex = 44;
            // 
            // txtOutput
            // 
            this.txtOutput.Location = new System.Drawing.Point(574, 488);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.Size = new System.Drawing.Size(126, 44);
            this.txtOutput.TabIndex = 45;
            // 
            // roi
            // 
            this.roi.AutoSize = true;
            this.roi.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.roi.Location = new System.Drawing.Point(843, 9);
            this.roi.Name = "roi";
            this.roi.Size = new System.Drawing.Size(145, 20);
            this.roi.TabIndex = 46;
            this.roi.Text = "Region Of Interest";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(848, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 48;
            this.label1.Text = "Roi X:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtRoiX
            // 
            this.txtRoiX.Location = new System.Drawing.Point(927, 46);
            this.txtRoiX.Name = "txtRoiX";
            this.txtRoiX.Size = new System.Drawing.Size(100, 20);
            this.txtRoiX.TabIndex = 47;
            this.txtRoiX.Tag = "roi_x";
            this.txtRoiX.TextChanged += new System.EventHandler(this.updateRecognition);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(848, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 50;
            this.label2.Text = "Roi Y:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtRoiY
            // 
            this.txtRoiY.Location = new System.Drawing.Point(927, 76);
            this.txtRoiY.Name = "txtRoiY";
            this.txtRoiY.Size = new System.Drawing.Size(100, 20);
            this.txtRoiY.TabIndex = 49;
            this.txtRoiY.Tag = "roi_y";
            this.txtRoiY.TextChanged += new System.EventHandler(this.updateRecognition);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(848, 112);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 52;
            this.label3.Text = "Roi width:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtRoiWidth
            // 
            this.txtRoiWidth.Location = new System.Drawing.Point(927, 109);
            this.txtRoiWidth.Name = "txtRoiWidth";
            this.txtRoiWidth.Size = new System.Drawing.Size(100, 20);
            this.txtRoiWidth.TabIndex = 51;
            this.txtRoiWidth.Tag = "roi_width";
            this.txtRoiWidth.TextChanged += new System.EventHandler(this.updateRecognition);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(848, 146);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 54;
            this.label4.Text = "Roi height:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtRoiHeight
            // 
            this.txtRoiHeight.Location = new System.Drawing.Point(927, 142);
            this.txtRoiHeight.Name = "txtRoiHeight";
            this.txtRoiHeight.Size = new System.Drawing.Size(100, 20);
            this.txtRoiHeight.TabIndex = 53;
            this.txtRoiHeight.Tag = "roi_height";
            this.txtRoiHeight.TextChanged += new System.EventHandler(this.updateRecognition);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(570, 399);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 13);
            this.label5.TabIndex = 55;
            this.label5.Text = "Expected Result";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(570, 464);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 13);
            this.label6.TabIndex = 56;
            this.label6.Text = "Is equal?";
            // 
            // LabelRecognition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1174, 544);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtRoiHeight);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtRoiWidth);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtRoiY);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtRoiX);
            this.Controls.Add(this.roi);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.txtExpected);
            this.Controls.Add(this.Herkenning);
            this.Controls.Add(this.lblHighboundFrequency);
            this.Controls.Add(this.lblMaxDistance);
            this.Controls.Add(this.txtHighboundFrequency);
            this.Controls.Add(this.txtMaxDistance);
            this.Controls.Add(this.lblMemoryParameters);
            this.Controls.Add(this.lblMaximumChars);
            this.Controls.Add(this.lblMinChar);
            this.Controls.Add(this.txtMaximumChars);
            this.Controls.Add(this.txtMinimumChars);
            this.Controls.Add(this.lblTesseractParameters);
            this.Controls.Add(this.lblImageProcessing);
            this.Controls.Add(this.lblGaussianStd2);
            this.Controls.Add(this.lblGaussianStd1);
            this.Controls.Add(this.txtGaussianStd2);
            this.Controls.Add(this.txtGaussianStd1);
            this.Controls.Add(this.chkTesseract);
            this.Controls.Add(this.txtTesseract);
            this.Controls.Add(this.trackBarAvi);
            this.Controls.Add(this.txtNumber);
            this.Controls.Add(this.rolnummerLabel);
            this.Controls.Add(this.btnOpenFile);
            this.Controls.Add(this.txtLabelFault);
            this.Controls.Add(this.lblLabelFault);
            this.Controls.Add(this.lblLabelHeight);
            this.Controls.Add(this.lblLabelWidth);
            this.Controls.Add(this.txtLabelWidth);
            this.Controls.Add(this.txtLabelHeight);
            this.Controls.Add(this.txtErode);
            this.Controls.Add(this.lblErode);
            this.Controls.Add(this.txtDilate);
            this.Controls.Add(this.lblDilate);
            this.Controls.Add(this.imageOriginal);
            this.Controls.Add(this.next_frame);
            this.Controls.Add(this.imageCluster);
            this.Name = "LabelRecognition";
            this.Text = "LabelRecognition";
            ((System.ComponentModel.ISupportInitialize)(this.imageCluster)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageOriginal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rolnummerLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarAvi)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Emgu.CV.UI.ImageBox imageCluster;
        private System.Windows.Forms.Button next_frame;
        private Emgu.CV.UI.ImageBox imageOriginal;
        private System.Windows.Forms.Label lblDilate;
        private System.Windows.Forms.TextBox txtDilate;
        private System.Windows.Forms.Label lblErode;
        private System.Windows.Forms.TextBox txtErode;
        private System.Windows.Forms.TextBox txtLabelHeight;
        private System.Windows.Forms.TextBox txtLabelWidth;
        private System.Windows.Forms.Label lblLabelWidth;
        private System.Windows.Forms.Label lblLabelHeight;
        private System.Windows.Forms.Label lblLabelFault;
        private System.Windows.Forms.TextBox txtLabelFault;
        private System.Windows.Forms.Button btnOpenFile;
        private Emgu.CV.UI.ImageBox rolnummerLabel;
        private System.Windows.Forms.Label txtNumber;
        private System.Windows.Forms.TrackBar trackBarAvi;
        private System.Windows.Forms.TextBox txtTesseract;
        private System.Windows.Forms.CheckBox chkTesseract;
        private System.Windows.Forms.TextBox txtGaussianStd1;
        private System.Windows.Forms.TextBox txtGaussianStd2;
        private System.Windows.Forms.Label lblGaussianStd1;
        private System.Windows.Forms.Label lblGaussianStd2;
        private System.Windows.Forms.Label lblImageProcessing;
        private System.Windows.Forms.Label lblTesseractParameters;
        private System.Windows.Forms.Label lblMaximumChars;
        private System.Windows.Forms.Label lblMinChar;
        private System.Windows.Forms.TextBox txtMaximumChars;
        private System.Windows.Forms.TextBox txtMinimumChars;
        private System.Windows.Forms.Label lblMemoryParameters;
        private System.Windows.Forms.Label lblHighboundFrequency;
        private System.Windows.Forms.Label lblMaxDistance;
        private System.Windows.Forms.TextBox txtHighboundFrequency;
        private System.Windows.Forms.TextBox txtMaxDistance;
        private System.Windows.Forms.Label Herkenning;
        private System.Windows.Forms.TextBox txtExpected;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.Label roi;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtRoiX;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtRoiY;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtRoiWidth;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtRoiHeight;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
    }
}