namespace Demo_Paint
{
    partial class Form2
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
            this.components = new System.ComponentModel.Container();
            this.tbHex = new System.Windows.Forms.TextBox();
            this.lbHex = new System.Windows.Forms.Label();
            this.lbBasicColor = new System.Windows.Forms.Label();
            this.lbCustomColor = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.numA = new System.Windows.Forms.NumericUpDown();
            this.numHue = new System.Windows.Forms.NumericUpDown();
            this.numSaturation = new System.Windows.Forms.NumericUpDown();
            this.numLuminance = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numBlue = new System.Windows.Forms.NumericUpDown();
            this.numGreen = new System.Windows.Forms.NumericUpDown();
            this.numRed = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.radioB = new ColorPicker.Controls.NickRadioButton();
            this.radioG = new ColorPicker.Controls.NickRadioButton();
            this.radioR = new ColorPicker.Controls.NickRadioButton();
            this.radioL = new ColorPicker.Controls.NickRadioButton();
            this.radioS = new ColorPicker.Controls.NickRadioButton();
            this.radioH = new ColorPicker.Controls.NickRadioButton();
            this.nickButton16 = new ColorPicker.Controls.NickButton();
            this.nickButton15 = new ColorPicker.Controls.NickButton();
            this.nickButton14 = new ColorPicker.Controls.NickButton();
            this.nickButton13 = new ColorPicker.Controls.NickButton();
            this.nickButton12 = new ColorPicker.Controls.NickButton();
            this.nickButton11 = new ColorPicker.Controls.NickButton();
            this.nickButton10 = new ColorPicker.Controls.NickButton();
            this.nickButton9 = new ColorPicker.Controls.NickButton();
            this.nickButton8 = new ColorPicker.Controls.NickButton();
            this.nickButton7 = new ColorPicker.Controls.NickButton();
            this.nickButton6 = new ColorPicker.Controls.NickButton();
            this.nickButton5 = new ColorPicker.Controls.NickButton();
            this.nickButton4 = new ColorPicker.Controls.NickButton();
            this.nickButton3 = new ColorPicker.Controls.NickButton();
            this.nickButton2 = new ColorPicker.Controls.NickButton();
            this.nickButton1 = new ColorPicker.Controls.NickButton();
            this.gradientPanel1 = new GradientPanel.GradientPanel();
            this.opacitySlider1 = new ColorPicker.OpacitySlider();
            this.colorSlider1 = new ColorPicker.ColorSlider();
            this.colorBox2D1 = new ColorPicker.ColorBox2D();
            ((System.ComponentModel.ISupportInitialize)(this.numA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSaturation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLuminance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBlue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGreen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRed)).BeginInit();
            this.SuspendLayout();
            // 
            // tbHex
            // 
            this.tbHex.Location = new System.Drawing.Point(190, 355);
            this.tbHex.MaxLength = 9;
            this.tbHex.Name = "tbHex";
            this.tbHex.Size = new System.Drawing.Size(125, 20);
            this.tbHex.TabIndex = 4;
            this.tbHex.TextChanged += new System.EventHandler(this.tbHex_TextChanged);
            // 
            // lbHex
            // 
            this.lbHex.AutoSize = true;
            this.lbHex.Location = new System.Drawing.Point(155, 358);
            this.lbHex.Name = "lbHex";
            this.lbHex.Size = new System.Drawing.Size(29, 13);
            this.lbHex.TabIndex = 5;
            this.lbHex.Text = "HEX";
            // 
            // lbBasicColor
            // 
            this.lbBasicColor.AutoSize = true;
            this.lbBasicColor.Location = new System.Drawing.Point(184, 274);
            this.lbBasicColor.Name = "lbBasicColor";
            this.lbBasicColor.Size = new System.Drawing.Size(65, 13);
            this.lbBasicColor.TabIndex = 6;
            this.lbBasicColor.Text = "Basic Colors";
            // 
            // lbCustomColor
            // 
            this.lbCustomColor.AutoSize = true;
            this.lbCustomColor.Location = new System.Drawing.Point(347, 274);
            this.lbCustomColor.Name = "lbCustomColor";
            this.lbCustomColor.Size = new System.Drawing.Size(74, 13);
            this.lbCustomColor.TabIndex = 13;
            this.lbCustomColor.Text = "Custom Colors";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(42, 395);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(128, 23);
            this.btnOK.TabIndex = 24;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(324, 395);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(128, 23);
            this.btnCancel.TabIndex = 25;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(345, 236);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 26;
            this.label1.Text = "Opacity";
            // 
            // numA
            // 
            this.numA.Location = new System.Drawing.Point(394, 234);
            this.numA.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numA.Name = "numA";
            this.numA.Size = new System.Drawing.Size(58, 20);
            this.numA.TabIndex = 27;
            // 
            // numHue
            // 
            this.numHue.Location = new System.Drawing.Point(394, 32);
            this.numHue.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.numHue.Name = "numHue";
            this.numHue.Size = new System.Drawing.Size(58, 20);
            this.numHue.TabIndex = 30;
            this.numHue.ValueChanged += new System.EventHandler(this.numHue_ValueChanged);
            // 
            // numSaturation
            // 
            this.numSaturation.Location = new System.Drawing.Point(394, 57);
            this.numSaturation.Name = "numSaturation";
            this.numSaturation.Size = new System.Drawing.Size(58, 20);
            this.numSaturation.TabIndex = 31;
            this.numSaturation.ValueChanged += new System.EventHandler(this.numSaturation_ValueChanged);
            // 
            // numLuminance
            // 
            this.numLuminance.Location = new System.Drawing.Point(394, 82);
            this.numLuminance.Name = "numLuminance";
            this.numLuminance.Size = new System.Drawing.Size(58, 20);
            this.numLuminance.TabIndex = 33;
            this.numLuminance.ValueChanged += new System.EventHandler(this.numLuminance_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(391, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 13);
            this.label2.TabIndex = 36;
            this.label2.Text = "HSL";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(391, 122);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(30, 13);
            this.label4.TabIndex = 44;
            this.label4.Text = "RGB";
            // 
            // numBlue
            // 
            this.numBlue.Location = new System.Drawing.Point(394, 192);
            this.numBlue.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numBlue.Name = "numBlue";
            this.numBlue.Size = new System.Drawing.Size(58, 20);
            this.numBlue.TabIndex = 43;
            this.numBlue.ValueChanged += new System.EventHandler(this.numBlue_ValueChanged);
            // 
            // numGreen
            // 
            this.numGreen.Location = new System.Drawing.Point(394, 167);
            this.numGreen.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numGreen.Name = "numGreen";
            this.numGreen.Size = new System.Drawing.Size(58, 20);
            this.numGreen.TabIndex = 41;
            this.numGreen.ValueChanged += new System.EventHandler(this.numGreen_ValueChanged);
            // 
            // numRed
            // 
            this.numRed.Location = new System.Drawing.Point(394, 142);
            this.numRed.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numRed.Name = "numRed";
            this.numRed.Size = new System.Drawing.Size(58, 20);
            this.numRed.TabIndex = 40;
            this.numRed.ValueChanged += new System.EventHandler(this.numRed_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 274);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 45;
            this.label3.Text = "Color";
            // 
            // radioB
            // 
            this.radioB.AutoSize = true;
            this.radioB.CheckedColor = System.Drawing.Color.Gray;
            this.radioB.HoverColor = System.Drawing.Color.White;
            this.radioB.Location = new System.Drawing.Point(354, 190);
            this.radioB.MinimumSize = new System.Drawing.Size(0, 21);
            this.radioB.Name = "radioB";
            this.radioB.Size = new System.Drawing.Size(44, 21);
            this.radioB.TabIndex = 42;
            this.radioB.TabStop = true;
            this.radioB.Text = "B";
            this.radioB.UncheckedColor = System.Drawing.Color.Gray;
            this.radioB.UseVisualStyleBackColor = true;
            this.radioB.CheckedChanged += new System.EventHandler(this.ColorModeChangedHandler);
            // 
            // radioG
            // 
            this.radioG.AutoSize = true;
            this.radioG.CheckedColor = System.Drawing.Color.Gray;
            this.radioG.HoverColor = System.Drawing.Color.White;
            this.radioG.Location = new System.Drawing.Point(354, 165);
            this.radioG.MinimumSize = new System.Drawing.Size(0, 21);
            this.radioG.Name = "radioG";
            this.radioG.Size = new System.Drawing.Size(45, 21);
            this.radioG.TabIndex = 39;
            this.radioG.TabStop = true;
            this.radioG.Text = "G";
            this.radioG.UncheckedColor = System.Drawing.Color.Gray;
            this.radioG.UseVisualStyleBackColor = true;
            this.radioG.CheckedChanged += new System.EventHandler(this.ColorModeChangedHandler);
            // 
            // radioR
            // 
            this.radioR.AutoSize = true;
            this.radioR.CheckedColor = System.Drawing.Color.Gray;
            this.radioR.HoverColor = System.Drawing.Color.White;
            this.radioR.Location = new System.Drawing.Point(353, 140);
            this.radioR.MinimumSize = new System.Drawing.Size(0, 21);
            this.radioR.Name = "radioR";
            this.radioR.Size = new System.Drawing.Size(45, 21);
            this.radioR.TabIndex = 38;
            this.radioR.TabStop = true;
            this.radioR.Text = "R";
            this.radioR.UncheckedColor = System.Drawing.Color.Gray;
            this.radioR.UseVisualStyleBackColor = true;
            this.radioR.CheckedChanged += new System.EventHandler(this.ColorModeChangedHandler);
            // 
            // radioL
            // 
            this.radioL.AutoSize = true;
            this.radioL.CheckedColor = System.Drawing.Color.Gray;
            this.radioL.HoverColor = System.Drawing.Color.White;
            this.radioL.Location = new System.Drawing.Point(354, 80);
            this.radioL.MinimumSize = new System.Drawing.Size(0, 21);
            this.radioL.Name = "radioL";
            this.radioL.Size = new System.Drawing.Size(43, 21);
            this.radioL.TabIndex = 32;
            this.radioL.TabStop = true;
            this.radioL.Text = "L";
            this.radioL.UncheckedColor = System.Drawing.Color.Gray;
            this.radioL.UseVisualStyleBackColor = true;
            this.radioL.CheckedChanged += new System.EventHandler(this.ColorModeChangedHandler);
            // 
            // radioS
            // 
            this.radioS.AutoSize = true;
            this.radioS.CheckedColor = System.Drawing.Color.Gray;
            this.radioS.HoverColor = System.Drawing.Color.White;
            this.radioS.Location = new System.Drawing.Point(354, 55);
            this.radioS.MinimumSize = new System.Drawing.Size(0, 21);
            this.radioS.Name = "radioS";
            this.radioS.Size = new System.Drawing.Size(44, 21);
            this.radioS.TabIndex = 29;
            this.radioS.TabStop = true;
            this.radioS.Text = "S";
            this.radioS.UncheckedColor = System.Drawing.Color.Gray;
            this.radioS.UseVisualStyleBackColor = true;
            this.radioS.CheckedChanged += new System.EventHandler(this.ColorModeChangedHandler);
            // 
            // radioH
            // 
            this.radioH.AutoSize = true;
            this.radioH.CheckedColor = System.Drawing.Color.Gray;
            this.radioH.HoverColor = System.Drawing.Color.White;
            this.radioH.Location = new System.Drawing.Point(353, 30);
            this.radioH.MinimumSize = new System.Drawing.Size(0, 21);
            this.radioH.Name = "radioH";
            this.radioH.Size = new System.Drawing.Size(45, 21);
            this.radioH.TabIndex = 28;
            this.radioH.TabStop = true;
            this.radioH.Text = "H";
            this.radioH.UncheckedColor = System.Drawing.Color.Gray;
            this.radioH.UseVisualStyleBackColor = true;
            this.radioH.CheckedChanged += new System.EventHandler(this.ColorModeChangedHandler);
            // 
            // nickButton16
            // 
            this.nickButton16.BackColor = System.Drawing.Color.Transparent;
            this.nickButton16.BackgroundColor = System.Drawing.Color.Transparent;
            this.nickButton16.BorderColor = System.Drawing.Color.Black;
            this.nickButton16.BorderRadius = 20;
            this.nickButton16.BorderSize = 1;
            this.nickButton16.FlatAppearance.BorderSize = 0;
            this.nickButton16.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.nickButton16.ForeColor = System.Drawing.Color.White;
            this.nickButton16.Location = new System.Drawing.Point(335, 316);
            this.nickButton16.Name = "nickButton16";
            this.nickButton16.Size = new System.Drawing.Size(20, 20);
            this.nickButton16.TabIndex = 23;
            this.nickButton16.TextColor = System.Drawing.Color.White;
            this.nickButton16.UseVisualStyleBackColor = false;
            // 
            // nickButton15
            // 
            this.nickButton15.BackColor = System.Drawing.Color.Transparent;
            this.nickButton15.BackgroundColor = System.Drawing.Color.Transparent;
            this.nickButton15.BorderColor = System.Drawing.Color.Black;
            this.nickButton15.BorderRadius = 20;
            this.nickButton15.BorderSize = 1;
            this.nickButton15.FlatAppearance.BorderSize = 0;
            this.nickButton15.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.nickButton15.ForeColor = System.Drawing.Color.White;
            this.nickButton15.Location = new System.Drawing.Point(335, 290);
            this.nickButton15.Name = "nickButton15";
            this.nickButton15.Size = new System.Drawing.Size(20, 20);
            this.nickButton15.TabIndex = 22;
            this.nickButton15.TextColor = System.Drawing.Color.White;
            this.nickButton15.UseVisualStyleBackColor = false;
            // 
            // nickButton14
            // 
            this.nickButton14.BackColor = System.Drawing.Color.MediumAquamarine;
            this.nickButton14.BackgroundColor = System.Drawing.Color.MediumAquamarine;
            this.nickButton14.BorderColor = System.Drawing.Color.Transparent;
            this.nickButton14.BorderRadius = 20;
            this.nickButton14.BorderSize = 1;
            this.nickButton14.FlatAppearance.BorderSize = 0;
            this.nickButton14.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.nickButton14.ForeColor = System.Drawing.Color.White;
            this.nickButton14.Location = new System.Drawing.Point(242, 316);
            this.nickButton14.Name = "nickButton14";
            this.nickButton14.Size = new System.Drawing.Size(20, 20);
            this.nickButton14.TabIndex = 21;
            this.nickButton14.TextColor = System.Drawing.Color.White;
            this.toolTip1.SetToolTip(this.nickButton14, "Medium Aquamarine");
            this.nickButton14.UseVisualStyleBackColor = false;
            this.nickButton14.Click += new System.EventHandler(this.basicColor_Click);
            // 
            // nickButton13
            // 
            this.nickButton13.BackColor = System.Drawing.Color.DarkKhaki;
            this.nickButton13.BackgroundColor = System.Drawing.Color.DarkKhaki;
            this.nickButton13.BorderColor = System.Drawing.Color.Transparent;
            this.nickButton13.BorderRadius = 20;
            this.nickButton13.BorderSize = 1;
            this.nickButton13.FlatAppearance.BorderSize = 0;
            this.nickButton13.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.nickButton13.ForeColor = System.Drawing.Color.White;
            this.nickButton13.Location = new System.Drawing.Point(242, 290);
            this.nickButton13.Name = "nickButton13";
            this.nickButton13.Size = new System.Drawing.Size(20, 20);
            this.nickButton13.TabIndex = 20;
            this.nickButton13.TextColor = System.Drawing.Color.White;
            this.toolTip1.SetToolTip(this.nickButton13, "Dark Khaki");
            this.nickButton13.UseVisualStyleBackColor = false;
            this.nickButton13.Click += new System.EventHandler(this.basicColor_Click);
            // 
            // nickButton12
            // 
            this.nickButton12.BackColor = System.Drawing.Color.Transparent;
            this.nickButton12.BackgroundColor = System.Drawing.Color.Transparent;
            this.nickButton12.BorderColor = System.Drawing.Color.Black;
            this.nickButton12.BorderRadius = 20;
            this.nickButton12.BorderSize = 1;
            this.nickButton12.FlatAppearance.BorderSize = 0;
            this.nickButton12.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.nickButton12.ForeColor = System.Drawing.Color.White;
            this.nickButton12.Location = new System.Drawing.Point(413, 316);
            this.nickButton12.Name = "nickButton12";
            this.nickButton12.Size = new System.Drawing.Size(20, 20);
            this.nickButton12.TabIndex = 19;
            this.nickButton12.TextColor = System.Drawing.Color.White;
            this.nickButton12.UseVisualStyleBackColor = false;
            // 
            // nickButton11
            // 
            this.nickButton11.BackColor = System.Drawing.Color.Transparent;
            this.nickButton11.BackgroundColor = System.Drawing.Color.Transparent;
            this.nickButton11.BorderColor = System.Drawing.Color.Black;
            this.nickButton11.BorderRadius = 20;
            this.nickButton11.BorderSize = 1;
            this.nickButton11.FlatAppearance.BorderSize = 0;
            this.nickButton11.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.nickButton11.ForeColor = System.Drawing.Color.White;
            this.nickButton11.Location = new System.Drawing.Point(387, 316);
            this.nickButton11.Name = "nickButton11";
            this.nickButton11.Size = new System.Drawing.Size(20, 20);
            this.nickButton11.TabIndex = 18;
            this.nickButton11.TextColor = System.Drawing.Color.White;
            this.nickButton11.UseVisualStyleBackColor = false;
            // 
            // nickButton10
            // 
            this.nickButton10.BackColor = System.Drawing.Color.Transparent;
            this.nickButton10.BackgroundColor = System.Drawing.Color.Transparent;
            this.nickButton10.BorderColor = System.Drawing.Color.Black;
            this.nickButton10.BorderRadius = 20;
            this.nickButton10.BorderSize = 1;
            this.nickButton10.FlatAppearance.BorderSize = 0;
            this.nickButton10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.nickButton10.ForeColor = System.Drawing.Color.White;
            this.nickButton10.Location = new System.Drawing.Point(361, 316);
            this.nickButton10.Name = "nickButton10";
            this.nickButton10.Size = new System.Drawing.Size(20, 20);
            this.nickButton10.TabIndex = 17;
            this.nickButton10.TextColor = System.Drawing.Color.White;
            this.nickButton10.UseVisualStyleBackColor = false;
            // 
            // nickButton9
            // 
            this.nickButton9.BackColor = System.Drawing.Color.Transparent;
            this.nickButton9.BackgroundColor = System.Drawing.Color.Transparent;
            this.nickButton9.BorderColor = System.Drawing.Color.Black;
            this.nickButton9.BorderRadius = 20;
            this.nickButton9.BorderSize = 1;
            this.nickButton9.FlatAppearance.BorderSize = 0;
            this.nickButton9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.nickButton9.ForeColor = System.Drawing.Color.White;
            this.nickButton9.Location = new System.Drawing.Point(413, 290);
            this.nickButton9.Name = "nickButton9";
            this.nickButton9.Size = new System.Drawing.Size(20, 20);
            this.nickButton9.TabIndex = 16;
            this.nickButton9.TextColor = System.Drawing.Color.White;
            this.nickButton9.UseVisualStyleBackColor = false;
            // 
            // nickButton8
            // 
            this.nickButton8.BackColor = System.Drawing.Color.Transparent;
            this.nickButton8.BackgroundColor = System.Drawing.Color.Transparent;
            this.nickButton8.BorderColor = System.Drawing.Color.Black;
            this.nickButton8.BorderRadius = 20;
            this.nickButton8.BorderSize = 1;
            this.nickButton8.FlatAppearance.BorderSize = 0;
            this.nickButton8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.nickButton8.ForeColor = System.Drawing.Color.White;
            this.nickButton8.Location = new System.Drawing.Point(387, 290);
            this.nickButton8.Name = "nickButton8";
            this.nickButton8.Size = new System.Drawing.Size(20, 20);
            this.nickButton8.TabIndex = 15;
            this.nickButton8.TextColor = System.Drawing.Color.White;
            this.nickButton8.UseVisualStyleBackColor = false;
            // 
            // nickButton7
            // 
            this.nickButton7.BackColor = System.Drawing.Color.Transparent;
            this.nickButton7.BackgroundColor = System.Drawing.Color.Transparent;
            this.nickButton7.BorderColor = System.Drawing.Color.Black;
            this.nickButton7.BorderRadius = 20;
            this.nickButton7.BorderSize = 1;
            this.nickButton7.FlatAppearance.BorderSize = 0;
            this.nickButton7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.nickButton7.ForeColor = System.Drawing.Color.White;
            this.nickButton7.Location = new System.Drawing.Point(361, 290);
            this.nickButton7.Name = "nickButton7";
            this.nickButton7.Size = new System.Drawing.Size(20, 20);
            this.nickButton7.TabIndex = 14;
            this.nickButton7.TextColor = System.Drawing.Color.White;
            this.nickButton7.UseVisualStyleBackColor = false;
            // 
            // nickButton6
            // 
            this.nickButton6.BackColor = System.Drawing.Color.Fuchsia;
            this.nickButton6.BackgroundColor = System.Drawing.Color.Fuchsia;
            this.nickButton6.BorderColor = System.Drawing.Color.Transparent;
            this.nickButton6.BorderRadius = 20;
            this.nickButton6.BorderSize = 1;
            this.nickButton6.FlatAppearance.BorderSize = 0;
            this.nickButton6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.nickButton6.ForeColor = System.Drawing.Color.White;
            this.nickButton6.Location = new System.Drawing.Point(216, 316);
            this.nickButton6.Name = "nickButton6";
            this.nickButton6.Size = new System.Drawing.Size(20, 20);
            this.nickButton6.TabIndex = 12;
            this.nickButton6.TextColor = System.Drawing.Color.White;
            this.toolTip1.SetToolTip(this.nickButton6, "Fuchsia");
            this.nickButton6.UseVisualStyleBackColor = false;
            this.nickButton6.Click += new System.EventHandler(this.basicColor_Click);
            // 
            // nickButton5
            // 
            this.nickButton5.BackColor = System.Drawing.Color.Blue;
            this.nickButton5.BackgroundColor = System.Drawing.Color.Blue;
            this.nickButton5.BorderColor = System.Drawing.Color.Transparent;
            this.nickButton5.BorderRadius = 20;
            this.nickButton5.BorderSize = 1;
            this.nickButton5.FlatAppearance.BorderSize = 0;
            this.nickButton5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.nickButton5.ForeColor = System.Drawing.Color.White;
            this.nickButton5.Location = new System.Drawing.Point(190, 316);
            this.nickButton5.Name = "nickButton5";
            this.nickButton5.Size = new System.Drawing.Size(20, 20);
            this.nickButton5.TabIndex = 11;
            this.nickButton5.TextColor = System.Drawing.Color.White;
            this.toolTip1.SetToolTip(this.nickButton5, "Blue");
            this.nickButton5.UseVisualStyleBackColor = false;
            this.nickButton5.Click += new System.EventHandler(this.basicColor_Click);
            // 
            // nickButton4
            // 
            this.nickButton4.BackColor = System.Drawing.Color.SpringGreen;
            this.nickButton4.BackgroundColor = System.Drawing.Color.SpringGreen;
            this.nickButton4.BorderColor = System.Drawing.Color.Transparent;
            this.nickButton4.BorderRadius = 20;
            this.nickButton4.BorderSize = 1;
            this.nickButton4.FlatAppearance.BorderSize = 0;
            this.nickButton4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.nickButton4.ForeColor = System.Drawing.Color.White;
            this.nickButton4.Location = new System.Drawing.Point(164, 316);
            this.nickButton4.Name = "nickButton4";
            this.nickButton4.Size = new System.Drawing.Size(20, 20);
            this.nickButton4.TabIndex = 10;
            this.nickButton4.TextColor = System.Drawing.Color.White;
            this.toolTip1.SetToolTip(this.nickButton4, "Spring Green");
            this.nickButton4.UseVisualStyleBackColor = false;
            this.nickButton4.Click += new System.EventHandler(this.basicColor_Click);
            // 
            // nickButton3
            // 
            this.nickButton3.BackColor = System.Drawing.Color.Yellow;
            this.nickButton3.BackgroundColor = System.Drawing.Color.Yellow;
            this.nickButton3.BorderColor = System.Drawing.Color.Transparent;
            this.nickButton3.BorderRadius = 20;
            this.nickButton3.BorderSize = 1;
            this.nickButton3.FlatAppearance.BorderSize = 0;
            this.nickButton3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.nickButton3.ForeColor = System.Drawing.Color.White;
            this.nickButton3.Location = new System.Drawing.Point(216, 290);
            this.nickButton3.Name = "nickButton3";
            this.nickButton3.Size = new System.Drawing.Size(20, 20);
            this.nickButton3.TabIndex = 9;
            this.nickButton3.TextColor = System.Drawing.Color.White;
            this.toolTip1.SetToolTip(this.nickButton3, "Yellow");
            this.nickButton3.UseVisualStyleBackColor = false;
            this.nickButton3.Click += new System.EventHandler(this.basicColor_Click);
            // 
            // nickButton2
            // 
            this.nickButton2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.nickButton2.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.nickButton2.BorderColor = System.Drawing.Color.Transparent;
            this.nickButton2.BorderRadius = 20;
            this.nickButton2.BorderSize = 1;
            this.nickButton2.FlatAppearance.BorderSize = 0;
            this.nickButton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.nickButton2.ForeColor = System.Drawing.Color.White;
            this.nickButton2.Location = new System.Drawing.Point(190, 290);
            this.nickButton2.Name = "nickButton2";
            this.nickButton2.Size = new System.Drawing.Size(20, 20);
            this.nickButton2.TabIndex = 8;
            this.nickButton2.TextColor = System.Drawing.Color.White;
            this.toolTip1.SetToolTip(this.nickButton2, "Orange");
            this.nickButton2.UseVisualStyleBackColor = false;
            this.nickButton2.Click += new System.EventHandler(this.basicColor_Click);
            // 
            // nickButton1
            // 
            this.nickButton1.BackColor = System.Drawing.Color.Red;
            this.nickButton1.BackgroundColor = System.Drawing.Color.Red;
            this.nickButton1.BorderColor = System.Drawing.Color.Transparent;
            this.nickButton1.BorderRadius = 20;
            this.nickButton1.BorderSize = 1;
            this.nickButton1.FlatAppearance.BorderSize = 0;
            this.nickButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.nickButton1.ForeColor = System.Drawing.Color.White;
            this.nickButton1.Location = new System.Drawing.Point(164, 290);
            this.nickButton1.Name = "nickButton1";
            this.nickButton1.Size = new System.Drawing.Size(20, 20);
            this.nickButton1.TabIndex = 7;
            this.nickButton1.TextColor = System.Drawing.Color.White;
            this.toolTip1.SetToolTip(this.nickButton1, "Red");
            this.nickButton1.UseVisualStyleBackColor = false;
            this.nickButton1.Click += new System.EventHandler(this.basicColor_Click);
            // 
            // gradientPanel1
            // 
            this.gradientPanel1.BoundPen = System.Drawing.Color.Empty;
            this.gradientPanel1.ColorBottom = System.Drawing.Color.Empty;
            this.gradientPanel1.ColorTop = System.Drawing.Color.Empty;
            this.gradientPanel1.IsDisabled = false;
            this.gradientPanel1.Location = new System.Drawing.Point(24, 291);
            this.gradientPanel1.Name = "gradientPanel1";
            this.gradientPanel1.Size = new System.Drawing.Size(78, 61);
            this.gradientPanel1.TabIndex = 3;
            // 
            // opacitySlider1
            // 
            this.opacitySlider1.Alpha = 0;
            this.opacitySlider1.ColorRGB = System.Drawing.Color.Empty;
            this.opacitySlider1.Location = new System.Drawing.Point(316, 12);
            this.opacitySlider1.Name = "opacitySlider1";
            this.opacitySlider1.NubPenColor = System.Drawing.Color.Blue;
            this.opacitySlider1.NubPosition = 0;
            this.opacitySlider1.Orientation = ColorPicker.OpacitySlider.Direction.Vertical;
            this.opacitySlider1.Size = new System.Drawing.Size(32, 255);
            this.opacitySlider1.TabIndex = 2;
            this.opacitySlider1.OpacityChanged += new ColorPicker.OpacitySlider.OpacityChangedEventHandler(this.opacitySlider1_OpacityChanged);
            // 
            // colorSlider1
            // 
            this.colorSlider1.ColorMode = ColorPicker.ColorModes.Hue;
            this.colorSlider1.ColorRGB = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.colorSlider1.Location = new System.Drawing.Point(283, 12);
            this.colorSlider1.Name = "colorSlider1";
            this.colorSlider1.NubPenColor = System.Drawing.Color.Blue;
            this.colorSlider1.NubPosition = 0;
            this.colorSlider1.Orientation = ColorPicker.ColorSlider.Direction.Vertical;
            this.colorSlider1.Size = new System.Drawing.Size(32, 255);
            this.colorSlider1.TabIndex = 1;
            this.colorSlider1.ColorChanged += new ColorPicker.ColorSlider.ColorChangedEventHandler(this.colorSlider1_ColorChanged_1);
            // 
            // colorBox2D1
            // 
            this.colorBox2D1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.colorBox2D1.ColorMode = ColorPicker.ColorModes.Hue;
            this.colorBox2D1.ColorRGB = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.colorBox2D1.Location = new System.Drawing.Point(24, 12);
            this.colorBox2D1.Name = "colorBox2D1";
            this.colorBox2D1.Size = new System.Drawing.Size(255, 255);
            this.colorBox2D1.TabIndex = 0;
            this.colorBox2D1.ColorChanged += new ColorPicker.ColorBox2D.ColorChangedEventHandler(this.colorBox2D1_ColorChanged);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 430);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.numBlue);
            this.Controls.Add(this.radioB);
            this.Controls.Add(this.numGreen);
            this.Controls.Add(this.numRed);
            this.Controls.Add(this.radioG);
            this.Controls.Add(this.radioR);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numLuminance);
            this.Controls.Add(this.radioL);
            this.Controls.Add(this.numSaturation);
            this.Controls.Add(this.numHue);
            this.Controls.Add(this.radioS);
            this.Controls.Add(this.radioH);
            this.Controls.Add(this.numA);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.nickButton16);
            this.Controls.Add(this.nickButton15);
            this.Controls.Add(this.nickButton14);
            this.Controls.Add(this.nickButton13);
            this.Controls.Add(this.nickButton12);
            this.Controls.Add(this.nickButton11);
            this.Controls.Add(this.nickButton10);
            this.Controls.Add(this.nickButton9);
            this.Controls.Add(this.nickButton8);
            this.Controls.Add(this.nickButton7);
            this.Controls.Add(this.lbCustomColor);
            this.Controls.Add(this.nickButton6);
            this.Controls.Add(this.nickButton5);
            this.Controls.Add(this.nickButton4);
            this.Controls.Add(this.nickButton3);
            this.Controls.Add(this.nickButton2);
            this.Controls.Add(this.nickButton1);
            this.Controls.Add(this.lbBasicColor);
            this.Controls.Add(this.lbHex);
            this.Controls.Add(this.tbHex);
            this.Controls.Add(this.gradientPanel1);
            this.Controls.Add(this.opacitySlider1);
            this.Controls.Add(this.colorSlider1);
            this.Controls.Add(this.colorBox2D1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form2";
            ((System.ComponentModel.ISupportInitialize)(this.numA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSaturation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLuminance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBlue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGreen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRed)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ColorPicker.ColorBox2D colorBox2D1;
        private ColorPicker.ColorSlider colorSlider1;
        private ColorPicker.OpacitySlider opacitySlider1;
        private GradientPanel.GradientPanel gradientPanel1;
        private System.Windows.Forms.TextBox tbHex;
        private System.Windows.Forms.Label lbHex;
        private System.Windows.Forms.Label lbBasicColor;
        private ColorPicker.Controls.NickButton nickButton1;
        private ColorPicker.Controls.NickButton nickButton2;
        private ColorPicker.Controls.NickButton nickButton3;
        private ColorPicker.Controls.NickButton nickButton4;
        private ColorPicker.Controls.NickButton nickButton5;
        private ColorPicker.Controls.NickButton nickButton6;
        private System.Windows.Forms.Label lbCustomColor;
        private ColorPicker.Controls.NickButton nickButton7;
        private ColorPicker.Controls.NickButton nickButton8;
        private ColorPicker.Controls.NickButton nickButton9;
        private ColorPicker.Controls.NickButton nickButton10;
        private ColorPicker.Controls.NickButton nickButton11;
        private ColorPicker.Controls.NickButton nickButton12;
        private ColorPicker.Controls.NickButton nickButton13;
        private ColorPicker.Controls.NickButton nickButton14;
        private ColorPicker.Controls.NickButton nickButton15;
        private ColorPicker.Controls.NickButton nickButton16;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numA;
        private ColorPicker.Controls.NickRadioButton radioH;
        private ColorPicker.Controls.NickRadioButton radioS;
        private System.Windows.Forms.NumericUpDown numHue;
        private System.Windows.Forms.NumericUpDown numSaturation;
        private System.Windows.Forms.NumericUpDown numLuminance;
        private ColorPicker.Controls.NickRadioButton radioL;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numBlue;
        private ColorPicker.Controls.NickRadioButton radioB;
        private System.Windows.Forms.NumericUpDown numGreen;
        private System.Windows.Forms.NumericUpDown numRed;
        private ColorPicker.Controls.NickRadioButton radioG;
        private ColorPicker.Controls.NickRadioButton radioR;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label3;
    }
}