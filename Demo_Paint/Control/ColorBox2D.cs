using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ColorPicker
{
    [DefaultEvent("ColorChanged")]
    public partial class ColorBox2D: UserControl
    {
        //ATTENTION: This control must be square
        //because otherwise when we select Saturation or Luminance
        //with the radio buttons we will have different width of control.

        #region Events
        public delegate void ColorChangedEventHandler(object sender, ColorChangedEventArgs args);
        public event ColorChangedEventHandler ColorChanged;
        #endregion

        #region Fields
        private HslColor colorHSL;
        private ColorModes colorMode;
        private Color colorRGB = Color.Empty;
        private Point markerPoint = Point.Empty;
        private bool mouseMoving;
        #endregion

        #region Properties
        public ColorModes ColorMode
        {
            get { return colorMode; }
            set
            {
                colorMode = value;
                ResetMarker();
                Refresh();
            }
        }

        public HslColor ColorHSL
        {
            get { return colorHSL; }
            set
            {
                colorHSL = value;
                colorRGB = colorHSL.RgbValue;
                ResetMarker();
                Refresh();
            }
        }

        public Color ColorRGB
        {
            get { return colorRGB; }
            set
            {
                colorRGB = value;
                colorHSL = HslColor.FromColor(colorRGB);
                ResetMarker();
                Refresh();
            }
        }
        #endregion

        #region Constructors
        public ColorBox2D()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

            colorHSL = HslColor.FromAhsl(1.0, 1.0, 1.0);
            colorRGB = colorHSL.RgbValue;
            colorMode = ColorModes.Hue;
        }
        #endregion

        #region Overriden Methods
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            mouseMoving = true;
            SetMarker(e.X, e.Y);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (mouseMoving)
            {
                SetMarker(e.X, e.Y);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            mouseMoving = false;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            HslColor color = HslColor.FromAhsl(255);
            HslColor color2 = HslColor.FromAhsl(255);
            switch (ColorMode)
            {
                case ColorModes.Hue:
                    color.H = ColorHSL.H;
                    color2.H = ColorHSL.H;
                    color.S = 0.0;
                    color2.S = 1.0;
                    break;

                case ColorModes.Saturation:
                    color.S = ColorHSL.S;
                    color2.S = ColorHSL.S;
                    color.L = 1.0;
                    color2.L = 0.0;
                    break;

                case ColorModes.Luminance:
                    color.L = ColorHSL.L;
                    color2.L = ColorHSL.L;
                    color.S = 1.0;
                    color2.S = 0.0;
                    break;
            }
            for (int i = 0; i < Height; i++)
            {
                int green = MathExtensions.Round(255.0 - ((255.0 * i) / Height));
                Color empty = Color.Empty;
                Color rgbValue = Color.Empty;
                switch (ColorMode)
                {
                    case ColorModes.Red:
                        empty = Color.FromArgb(ColorRGB.R, green, 0);
                        rgbValue = Color.FromArgb(ColorRGB.R, green, 255);
                        break;

                    case ColorModes.Green:
                        empty = Color.FromArgb(green, ColorRGB.G, 0);
                        rgbValue = Color.FromArgb(green, ColorRGB.G, 255);
                        break;

                    case ColorModes.Blue:
                        empty = Color.FromArgb(0, green, ColorRGB.B);
                        rgbValue = Color.FromArgb(255, green, ColorRGB.B);
                        break;

                    case ColorModes.Hue:
                        color2.L = color.L = 1.0 - (i / (double)Height);
                        empty = color.RgbValue;
                        rgbValue = color2.RgbValue;
                        break;

                    case ColorModes.Saturation:
                    case ColorModes.Luminance:
                        color2.H = color.H = i / (double)Width;
                        empty = color.RgbValue;
                        rgbValue = color2.RgbValue;
                        break;
                }

                Rectangle rect = new Rectangle(0, 0, Width, 1);
                Rectangle rectangle2 = new Rectangle(0, i, Width, 1);
                if ((ColorMode == ColorModes.Saturation) || (ColorMode == ColorModes.Luminance))
                {
                    rect = new Rectangle(0, 0, 1, Height);
                    rectangle2 = new Rectangle(i, 0, 1, Height);
                    using (LinearGradientBrush brush = new LinearGradientBrush(rect, empty, rgbValue, 90f, false))
                    {
                        e.Graphics.FillRectangle(brush, rectangle2);
                        continue;
                    }
                }
                using (LinearGradientBrush brush2 = new LinearGradientBrush(rect, empty, rgbValue, 0f, false))
                {
                    e.Graphics.FillRectangle(brush2, rectangle2);
                }
            }
            Pen white = Pens.White;
            if (colorHSL.L >= 0.78431372549019607)
            {
                if ((colorHSL.H < 0.072222222222222215) || (colorHSL.H > 0.55555555555555558))
                {
                    if (colorHSL.S <= 0.27450980392156865)
                    {
                        white = Pens.Black;
                    }
                }
                else
                {
                    white = Pens.Black;
                }
            }
            e.Graphics.DrawEllipse(white, markerPoint.X - 6, markerPoint.Y - 6, 12, 12);
        }


        #endregion

        #region Private Methods

        private HslColor GetColor(int x, int y)
        {
            int r;
            int g;
            int b;
            HslColor color = HslColor.FromAhsl(0xff);
            switch (ColorMode)
            {
                case ColorModes.Red:
                    g = MathExtensions.Round(255.0 * (1.0 - (y / ((double)(Height - 4)))));
                    b = MathExtensions.Round((255.0 * x) / (Width - 4));
                    return HslColor.FromColor(Color.FromArgb(colorRGB.R, g, b));

                case ColorModes.Green:
                    r = MathExtensions.Round(255.0 * (1.0 - (y / ((double)(Height - 4)))));
                    b = MathExtensions.Round((255.0 * x) / (Width - 4));
                    return HslColor.FromColor(Color.FromArgb(r, colorRGB.G, b));

                case ColorModes.Blue:
                    r = MathExtensions.Round((255.0 * x) / (Width - 4));
                    g = MathExtensions.Round(255.0 * (1.0 - (y / ((double)(Height - 4)))));
                    return HslColor.FromColor(Color.FromArgb(r, g, colorRGB.B));

                case ColorModes.Hue:
                    color.H = colorHSL.H;
                    color.S = x / ((double)(Width - 4));
                    color.L = 1.0 - (y / ((double)(Height - 4)));
                    return color;

                case ColorModes.Saturation:
                    color.S = colorHSL.S;
                    color.H = x / ((double)(Width - 4));
                    color.L = 1.0 - (y / ((double)(Height - 4)));
                    return color;

                case ColorModes.Luminance:
                    color.L = colorHSL.L;
                    color.H = x / ((double)(Width - 4));
                    color.S = 1.0 - (y / ((double)(Height - 4)));
                    return color;
            }
            return color;
        }

        private void ResetMarker()
        {
            switch (colorMode)
            {
                case ColorModes.Red:
                    markerPoint.X = MathExtensions.Round(((Width - 4) * colorRGB.B) / 255.0);
                    markerPoint.Y = MathExtensions.Round((Height - 4) * (1.0 - (colorRGB.G / 255.0)));
                    return;

                case ColorModes.Green:
                    markerPoint.X = MathExtensions.Round(((Width - 4) * colorRGB.B) / 255.0);
                    markerPoint.Y = MathExtensions.Round((Height - 4) * (1.0 - (colorRGB.R / 255.0)));
                    return;

                case ColorModes.Blue:
                    markerPoint.X = MathExtensions.Round(((Width - 4) * colorRGB.R) / 255.0);
                    markerPoint.Y = MathExtensions.Round((Height - 4) * (1.0 - (colorRGB.G / 255.0)));
                    return;

                case ColorModes.Hue:
                    markerPoint.X = MathExtensions.Round((Width - 4) * colorHSL.S);
                    markerPoint.Y = MathExtensions.Round((Height - 4) * (1.0 - colorHSL.L));
                    return;

                case ColorModes.Saturation:
                    markerPoint.X = MathExtensions.Round((Width - 4) * colorHSL.H);
                    markerPoint.Y = MathExtensions.Round((Height - 4) * (1.0 - colorHSL.L));
                    return;

                case ColorModes.Luminance:
                    markerPoint.X = MathExtensions.Round((Width - 4) * colorHSL.H);
                    markerPoint.Y = MathExtensions.Round((Height - 4) * (1.0 - colorHSL.S));
                    return;
            }
        }

        private void SetMarker(int x, int y)
        {
            x = MathExtensions.LimitToRange(x, 0, Width - 4);
            y = MathExtensions.LimitToRange(y, 0, Height - 4);
            if ((markerPoint.X != x) || (markerPoint.Y != y))
            {
                markerPoint = new Point(x, y);
                colorHSL = GetColor(x, y);
                colorRGB = colorHSL.RgbValue;
                Refresh();
                if (ColorChanged != null)
                {
                    ColorChanged(this, new ColorChangedEventArgs(colorRGB));
                }
            }
        }
        #endregion

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ColorBox2D
            // 
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Name = "ColorBox2D";
            this.Size = new System.Drawing.Size(341, 304);
            this.ResumeLayout(false);

        }
    }
}
