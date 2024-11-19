namespace Demo_Paint
{
    partial class Form1
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tooltip = new System.Windows.Forms.ToolTip(this.components);
            this.btnText = new System.Windows.Forms.Button();
            this.btnMagnifier = new System.Windows.Forms.Button();
            this.btnEyedropper = new System.Windows.Forms.Button();
            this.btnBucket = new System.Windows.Forms.Button();
            this.btnPen = new System.Windows.Forms.Button();
            this.btnEraser = new System.Windows.Forms.Button();
            this.btnRedo = new System.Windows.Forms.Button();
            this.btnUndo = new System.Windows.Forms.Button();
            this.btnLine = new System.Windows.Forms.Button();
            this.btn = new System.Windows.Forms.Button();
            this.btnStar = new System.Windows.Forms.Button();
            this.btnTriangle = new System.Windows.Forms.Button();
            this.btnDiamond = new System.Windows.Forms.Button();
            this.btnPentagon = new System.Windows.Forms.Button();
            this.btnHexagon = new System.Windows.Forms.Button();
            this.btnHeart = new System.Windows.Forms.Button();
            this.btn4PointStar = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSelection = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnEllipse = new System.Windows.Forms.Button();
            this.pic_ColorFill = new System.Windows.Forms.Button();
            this.btnColorFill = new System.Windows.Forms.Button();
            this.btn_ColorStroke = new System.Windows.Forms.Button();
            this.btnColorStroke = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.canvas = new System.Windows.Forms.PictureBox();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.canvas)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1538, 35);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(54, 29);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Image = global::Demo_Paint.Properties.Resources._160476;
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(158, 34);
            this.newToolStripMenuItem.Text = "New";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = global::Demo_Paint.Properties.Resources.open;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(158, 34);
            this.openToolStripMenuItem.Text = "Open";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = global::Demo_Paint.Properties.Resources.PikPng_com_save_icon_png_1408395;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(158, 34);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(58, 29);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // cToolStripMenuItem
            // 
            this.cToolStripMenuItem.Image = global::Demo_Paint.Properties.Resources.cut;
            this.cToolStripMenuItem.Name = "cToolStripMenuItem";
            this.cToolStripMenuItem.Size = new System.Drawing.Size(156, 34);
            this.cToolStripMenuItem.Text = "Cut";
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Image = global::Demo_Paint.Properties.Resources.copy;
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(156, 34);
            this.copyToolStripMenuItem.Text = "Copy";
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Image = global::Demo_Paint.Properties.Resources._6583091;
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(156, 34);
            this.pasteToolStripMenuItem.Text = "Paste";
            // 
            // btnText
            // 
            this.btnText.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnText.FlatAppearance.BorderSize = 0;
            this.btnText.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnText.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnText.Location = new System.Drawing.Point(324, 9);
            this.btnText.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnText.Name = "btnText";
            this.btnText.Size = new System.Drawing.Size(44, 45);
            this.btnText.TabIndex = 15;
            this.btnText.Text = "B";
            this.tooltip.SetToolTip(this.btnText, "Text");
            this.btnText.UseVisualStyleBackColor = true;
            this.btnText.Click += new System.EventHandler(this.btn_Click);
            this.btnText.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_MouseDown);
            // 
            // btnMagnifier
            // 
            this.btnMagnifier.FlatAppearance.BorderSize = 0;
            this.btnMagnifier.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMagnifier.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMagnifier.Image = global::Demo_Paint.Properties.Resources.kinhlup1;
            this.btnMagnifier.Location = new System.Drawing.Point(324, 58);
            this.btnMagnifier.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnMagnifier.Name = "btnMagnifier";
            this.btnMagnifier.Size = new System.Drawing.Size(44, 45);
            this.btnMagnifier.TabIndex = 16;
            this.tooltip.SetToolTip(this.btnMagnifier, "Magnifier");
            this.btnMagnifier.UseVisualStyleBackColor = true;
            this.btnMagnifier.Click += new System.EventHandler(this.btn_Click);
            this.btnMagnifier.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_MouseDown);
            // 
            // btnEyedropper
            // 
            this.btnEyedropper.FlatAppearance.BorderSize = 0;
            this.btnEyedropper.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEyedropper.Image = global::Demo_Paint.Properties.Resources.eyedropper1;
            this.btnEyedropper.Location = new System.Drawing.Point(272, 58);
            this.btnEyedropper.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnEyedropper.Name = "btnEyedropper";
            this.btnEyedropper.Size = new System.Drawing.Size(44, 45);
            this.btnEyedropper.TabIndex = 14;
            this.tooltip.SetToolTip(this.btnEyedropper, "Color Picker");
            this.btnEyedropper.UseVisualStyleBackColor = true;
            this.btnEyedropper.Click += new System.EventHandler(this.btn_Click);
            this.btnEyedropper.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_MouseDown);
            // 
            // btnBucket
            // 
            this.btnBucket.FlatAppearance.BorderSize = 0;
            this.btnBucket.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBucket.Image = global::Demo_Paint.Properties.Resources.paint_bucket2;
            this.btnBucket.Location = new System.Drawing.Point(272, 9);
            this.btnBucket.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnBucket.Name = "btnBucket";
            this.btnBucket.Size = new System.Drawing.Size(44, 45);
            this.btnBucket.TabIndex = 13;
            this.tooltip.SetToolTip(this.btnBucket, "Bucket");
            this.btnBucket.UseVisualStyleBackColor = true;
            this.btnBucket.Click += new System.EventHandler(this.btn_Click);
            this.btnBucket.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_MouseDown);
            // 
            // btnPen
            // 
            this.btnPen.FlatAppearance.BorderSize = 0;
            this.btnPen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPen.Image = global::Demo_Paint.Properties.Resources.pencil2;
            this.btnPen.Location = new System.Drawing.Point(219, 9);
            this.btnPen.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnPen.Name = "btnPen";
            this.btnPen.Size = new System.Drawing.Size(44, 45);
            this.btnPen.TabIndex = 12;
            this.tooltip.SetToolTip(this.btnPen, "Pen");
            this.btnPen.UseVisualStyleBackColor = true;
            this.btnPen.Click += new System.EventHandler(this.btn_Click);
            this.btnPen.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_MouseDown);
            // 
            // btnEraser
            // 
            this.btnEraser.FlatAppearance.BorderSize = 0;
            this.btnEraser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEraser.Image = global::Demo_Paint.Properties.Resources.eraser2;
            this.btnEraser.Location = new System.Drawing.Point(219, 58);
            this.btnEraser.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnEraser.Name = "btnEraser";
            this.btnEraser.Size = new System.Drawing.Size(44, 45);
            this.btnEraser.TabIndex = 11;
            this.tooltip.SetToolTip(this.btnEraser, "Eraser");
            this.btnEraser.UseVisualStyleBackColor = true;
            this.btnEraser.Click += new System.EventHandler(this.btn_Click);
            this.btnEraser.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_MouseDown);
            // 
            // btnRedo
            // 
            this.btnRedo.FlatAppearance.BorderSize = 0;
            this.btnRedo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRedo.ForeColor = System.Drawing.Color.Transparent;
            this.btnRedo.Image = global::Demo_Paint.Properties.Resources.redo;
            this.btnRedo.Location = new System.Drawing.Point(200, 2);
            this.btnRedo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnRedo.Name = "btnRedo";
            this.btnRedo.Size = new System.Drawing.Size(60, 35);
            this.btnRedo.TabIndex = 2;
            this.tooltip.SetToolTip(this.btnRedo, "Redo");
            this.btnRedo.UseVisualStyleBackColor = true;
            // 
            // btnUndo
            // 
            this.btnUndo.FlatAppearance.BorderSize = 0;
            this.btnUndo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUndo.ForeColor = System.Drawing.Color.Transparent;
            this.btnUndo.Image = global::Demo_Paint.Properties.Resources.undo;
            this.btnUndo.Location = new System.Drawing.Point(130, 2);
            this.btnUndo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnUndo.Name = "btnUndo";
            this.btnUndo.Size = new System.Drawing.Size(60, 35);
            this.btnUndo.TabIndex = 1;
            this.tooltip.SetToolTip(this.btnUndo, "Undo");
            this.btnUndo.UseVisualStyleBackColor = true;
            // 
            // btnLine
            // 
            this.btnLine.BackColor = System.Drawing.Color.White;
            this.btnLine.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnLine.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
            this.btnLine.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnLine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLine.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLine.Location = new System.Drawing.Point(394, 18);
            this.btnLine.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnLine.Name = "btnLine";
            this.btnLine.Size = new System.Drawing.Size(36, 37);
            this.btnLine.TabIndex = 19;
            this.btnLine.Text = "\\";
            this.tooltip.SetToolTip(this.btnLine, "Line");
            this.btnLine.UseVisualStyleBackColor = false;
            this.btnLine.Click += new System.EventHandler(this.btn_Click);
            this.btnLine.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_MouseDown);
            // 
            // btn
            // 
            this.btn.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btn.FlatAppearance.BorderSize = 0;
            this.btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn.Image = global::Demo_Paint.Properties.Resources.rec213;
            this.btn.Location = new System.Drawing.Point(434, 18);
            this.btn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btn.Name = "btn";
            this.btn.Size = new System.Drawing.Size(36, 37);
            this.btn.TabIndex = 6;
            this.btn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.tooltip.SetToolTip(this.btn, "Rectangle");
            this.btn.UseVisualStyleBackColor = false;
            this.btn.Click += new System.EventHandler(this.btn_Click);
            this.btn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_MouseDown);
            // 
            // btnStar
            // 
            this.btnStar.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnStar.FlatAppearance.BorderSize = 0;
            this.btnStar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStar.Image = global::Demo_Paint.Properties.Resources.star_khac11;
            this.btnStar.Location = new System.Drawing.Point(524, 18);
            this.btnStar.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnStar.Name = "btnStar";
            this.btnStar.Size = new System.Drawing.Size(36, 37);
            this.btnStar.TabIndex = 21;
            this.btnStar.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.tooltip.SetToolTip(this.btnStar, "Star");
            this.btnStar.UseVisualStyleBackColor = false;
            this.btnStar.Click += new System.EventHandler(this.btn_Click);
            this.btnStar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_MouseDown);
            // 
            // btnTriangle
            // 
            this.btnTriangle.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnTriangle.FlatAppearance.BorderSize = 0;
            this.btnTriangle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTriangle.Image = global::Demo_Paint.Properties.Resources.triangle1;
            this.btnTriangle.Location = new System.Drawing.Point(568, 18);
            this.btnTriangle.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnTriangle.Name = "btnTriangle";
            this.btnTriangle.Size = new System.Drawing.Size(36, 37);
            this.btnTriangle.TabIndex = 22;
            this.btnTriangle.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.tooltip.SetToolTip(this.btnTriangle, "Triangle");
            this.btnTriangle.UseVisualStyleBackColor = false;
            this.btnTriangle.Click += new System.EventHandler(this.btn_Click);
            this.btnTriangle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_MouseDown);
            // 
            // btnDiamond
            // 
            this.btnDiamond.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnDiamond.FlatAppearance.BorderSize = 0;
            this.btnDiamond.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDiamond.Image = global::Demo_Paint.Properties.Resources.diamond1;
            this.btnDiamond.Location = new System.Drawing.Point(394, 60);
            this.btnDiamond.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnDiamond.Name = "btnDiamond";
            this.btnDiamond.Size = new System.Drawing.Size(36, 37);
            this.btnDiamond.TabIndex = 23;
            this.btnDiamond.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.tooltip.SetToolTip(this.btnDiamond, "Diamond");
            this.btnDiamond.UseVisualStyleBackColor = false;
            this.btnDiamond.Click += new System.EventHandler(this.btn_Click);
            this.btnDiamond.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_MouseDown);
            // 
            // btnPentagon
            // 
            this.btnPentagon.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnPentagon.FlatAppearance.BorderSize = 0;
            this.btnPentagon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPentagon.Image = global::Demo_Paint.Properties.Resources.pentagon11;
            this.btnPentagon.Location = new System.Drawing.Point(434, 58);
            this.btnPentagon.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnPentagon.Name = "btnPentagon";
            this.btnPentagon.Size = new System.Drawing.Size(38, 38);
            this.btnPentagon.TabIndex = 24;
            this.btnPentagon.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.tooltip.SetToolTip(this.btnPentagon, "Pentagon");
            this.btnPentagon.UseVisualStyleBackColor = false;
            this.btnPentagon.Click += new System.EventHandler(this.btn_Click);
            this.btnPentagon.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_MouseDown);
            // 
            // btnHexagon
            // 
            this.btnHexagon.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnHexagon.FlatAppearance.BorderSize = 0;
            this.btnHexagon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHexagon.Image = global::Demo_Paint.Properties.Resources.hexagon1;
            this.btnHexagon.Location = new System.Drawing.Point(478, 58);
            this.btnHexagon.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnHexagon.Name = "btnHexagon";
            this.btnHexagon.Size = new System.Drawing.Size(38, 38);
            this.btnHexagon.TabIndex = 25;
            this.btnHexagon.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.tooltip.SetToolTip(this.btnHexagon, "Hexagon");
            this.btnHexagon.UseVisualStyleBackColor = false;
            this.btnHexagon.Click += new System.EventHandler(this.btn_Click);
            this.btnHexagon.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_MouseDown);
            // 
            // btnHeart
            // 
            this.btnHeart.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnHeart.FlatAppearance.BorderSize = 0;
            this.btnHeart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHeart.Image = global::Demo_Paint.Properties.Resources.heart1;
            this.btnHeart.Location = new System.Drawing.Point(524, 58);
            this.btnHeart.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnHeart.Name = "btnHeart";
            this.btnHeart.Size = new System.Drawing.Size(38, 38);
            this.btnHeart.TabIndex = 26;
            this.btnHeart.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.tooltip.SetToolTip(this.btnHeart, "Heart");
            this.btnHeart.UseVisualStyleBackColor = false;
            this.btnHeart.Click += new System.EventHandler(this.btn_Click);
            this.btnHeart.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_MouseDown);
            // 
            // btn4PointStar
            // 
            this.btn4PointStar.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btn4PointStar.FlatAppearance.BorderSize = 0;
            this.btn4PointStar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn4PointStar.Image = global::Demo_Paint.Properties.Resources._4pointstar12;
            this.btn4PointStar.Location = new System.Drawing.Point(568, 58);
            this.btn4PointStar.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btn4PointStar.Name = "btn4PointStar";
            this.btn4PointStar.Size = new System.Drawing.Size(36, 37);
            this.btn4PointStar.TabIndex = 27;
            this.btn4PointStar.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.tooltip.SetToolTip(this.btn4PointStar, "Four-Point Star");
            this.btn4PointStar.UseVisualStyleBackColor = false;
            this.btn4PointStar.Click += new System.EventHandler(this.btn_Click);
            this.btn4PointStar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_MouseDown);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.btnSelection);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.btn4PointStar);
            this.panel1.Controls.Add(this.btnHeart);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.btnHexagon);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btnPentagon);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.btnDiamond);
            this.panel1.Controls.Add(this.btnMagnifier);
            this.panel1.Controls.Add(this.btnTriangle);
            this.panel1.Controls.Add(this.btnText);
            this.panel1.Controls.Add(this.btnStar);
            this.panel1.Controls.Add(this.btnEyedropper);
            this.panel1.Controls.Add(this.btnEllipse);
            this.panel1.Controls.Add(this.btnBucket);
            this.panel1.Controls.Add(this.btn);
            this.panel1.Controls.Add(this.btnLine);
            this.panel1.Controls.Add(this.btnPen);
            this.panel1.Controls.Add(this.btnEraser);
            this.panel1.Controls.Add(this.pic_ColorFill);
            this.panel1.Controls.Add(this.btnColorFill);
            this.panel1.Controls.Add(this.btn_ColorStroke);
            this.panel1.Controls.Add(this.btnColorStroke);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 35);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1538, 134);
            this.panel1.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 105);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 20);
            this.label3.TabIndex = 29;
            this.label3.Text = "Selection";
            // 
            // btnSelection
            // 
            this.btnSelection.Location = new System.Drawing.Point(34, 9);
            this.btnSelection.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSelection.Name = "btnSelection";
            this.btnSelection.Size = new System.Drawing.Size(64, 94);
            this.btnSelection.TabIndex = 28;
            this.btnSelection.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.panel4.Location = new System.Drawing.Point(627, 9);
            this.panel4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(2, 115);
            this.panel4.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(453, 104);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 20);
            this.label2.TabIndex = 18;
            this.label2.Text = "Hình nè con";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(266, 106);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 20);
            this.label1.TabIndex = 17;
            this.label1.Text = "Tools";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.panel3.Location = new System.Drawing.Point(380, 9);
            this.panel3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(2, 115);
            this.panel3.TabIndex = 1;
            // 
            // btnEllipse
            // 
            this.btnEllipse.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnEllipse.FlatAppearance.BorderSize = 0;
            this.btnEllipse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEllipse.Image = global::Demo_Paint.Properties.Resources.ellipse1;
            this.btnEllipse.Location = new System.Drawing.Point(478, 18);
            this.btnEllipse.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnEllipse.Name = "btnEllipse";
            this.btnEllipse.Size = new System.Drawing.Size(39, 37);
            this.btnEllipse.TabIndex = 20;
            this.btnEllipse.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnEllipse.UseVisualStyleBackColor = false;
            this.btnEllipse.Click += new System.EventHandler(this.btn_Click);
            this.btnEllipse.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_MouseDown);
            // 
            // pic_ColorFill
            // 
            this.pic_ColorFill.BackColor = System.Drawing.Color.White;
            this.pic_ColorFill.Enabled = false;
            this.pic_ColorFill.ForeColor = System.Drawing.Color.Black;
            this.pic_ColorFill.Location = new System.Drawing.Point(867, 20);
            this.pic_ColorFill.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pic_ColorFill.Name = "pic_ColorFill";
            this.pic_ColorFill.Size = new System.Drawing.Size(69, 71);
            this.pic_ColorFill.TabIndex = 9;
            this.pic_ColorFill.UseVisualStyleBackColor = false;
            // 
            // btnColorFill
            // 
            this.btnColorFill.BackColor = System.Drawing.Color.Transparent;
            this.btnColorFill.FlatAppearance.BorderSize = 0;
            this.btnColorFill.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnColorFill.Location = new System.Drawing.Point(857, 9);
            this.btnColorFill.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnColorFill.Name = "btnColorFill";
            this.btnColorFill.Size = new System.Drawing.Size(92, 114);
            this.btnColorFill.TabIndex = 10;
            this.btnColorFill.Text = "Fill nè con";
            this.btnColorFill.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnColorFill.UseVisualStyleBackColor = false;
            this.btnColorFill.Click += new System.EventHandler(this.btn_Click);
            this.btnColorFill.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_MouseDown);
            // 
            // btn_ColorStroke
            // 
            this.btn_ColorStroke.BackColor = System.Drawing.Color.White;
            this.btn_ColorStroke.Enabled = false;
            this.btn_ColorStroke.ForeColor = System.Drawing.Color.Black;
            this.btn_ColorStroke.Location = new System.Drawing.Point(754, 20);
            this.btn_ColorStroke.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btn_ColorStroke.Name = "btn_ColorStroke";
            this.btn_ColorStroke.Size = new System.Drawing.Size(69, 71);
            this.btn_ColorStroke.TabIndex = 7;
            this.btn_ColorStroke.UseVisualStyleBackColor = false;
            // 
            // btnColorStroke
            // 
            this.btnColorStroke.BackColor = System.Drawing.Color.Transparent;
            this.btnColorStroke.FlatAppearance.BorderSize = 0;
            this.btnColorStroke.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnColorStroke.Location = new System.Drawing.Point(744, 9);
            this.btnColorStroke.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnColorStroke.Name = "btnColorStroke";
            this.btnColorStroke.Size = new System.Drawing.Size(92, 114);
            this.btnColorStroke.TabIndex = 8;
            this.btnColorStroke.Text = "Stroke";
            this.btnColorStroke.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnColorStroke.UseVisualStyleBackColor = false;
            this.btnColorStroke.Click += new System.EventHandler(this.btn_Click);
            this.btnColorStroke.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_MouseDown);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.panel2.Location = new System.Drawing.Point(208, 9);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(2, 115);
            this.panel2.TabIndex = 0;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Margin = new System.Windows.Forms.Padding(0, 3, 0, 2);
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 660);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(2, 0, 21, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1538, 32);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Image = global::Demo_Paint.Properties.Resources.vecteezy_mouse_cursor_symbol_on_transparent_background_17178335;
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(203, 25);
            this.toolStripStatusLabel2.Text = "toolStripStatusLabel2";
            // 
            // canvas
            // 
            this.canvas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.canvas.BackColor = System.Drawing.Color.White;
            this.canvas.Location = new System.Drawing.Point(130, 180);
            this.canvas.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.canvas.Name = "canvas";
            this.canvas.Size = new System.Drawing.Size(1258, 465);
            this.canvas.TabIndex = 4;
            this.canvas.TabStop = false;
            this.canvas.MouseLeave += new System.EventHandler(this.canvas_MouseLeave);
            this.canvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.canvas_MouseMove);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1538, 692);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnRedo);
            this.Controls.Add(this.btnUndo);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.canvas);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Form1";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.canvas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.Button btnUndo;
        private System.Windows.Forms.Button btnRedo;
        private System.Windows.Forms.ToolTip tooltip;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.PictureBox canvas;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btn_ColorStroke;
        private System.Windows.Forms.Button btnColorStroke;
        private System.Windows.Forms.Button pic_ColorFill;
        private System.Windows.Forms.Button btnColorFill;
        private System.Windows.Forms.Button btnEraser;
        private System.Windows.Forms.Button btnPen;
        private System.Windows.Forms.Button btnBucket;
        private System.Windows.Forms.Button btnEyedropper;
        private System.Windows.Forms.Button btnText;
        private System.Windows.Forms.Button btnMagnifier;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn4PointStar;
        private System.Windows.Forms.Button btnHeart;
        private System.Windows.Forms.Button btnHexagon;
        private System.Windows.Forms.Button btnPentagon;
        private System.Windows.Forms.Button btnDiamond;
        private System.Windows.Forms.Button btnTriangle;
        private System.Windows.Forms.Button btnStar;
        private System.Windows.Forms.Button btnEllipse;
        private System.Windows.Forms.Button btn;
        private System.Windows.Forms.Button btnLine;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSelection;
    }
}

