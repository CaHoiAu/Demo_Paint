using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ColorPicker
{
    [DefaultEvent("ColorChanged")]
    public partial class ColorSlider : UserControl
    {
        #region Enum
        public enum Direction
        {
            Horizontal,
            Vertical
        }
        #endregion

        #region Events
        public delegate void ColorChangedEventHandler(object sender, ColorChangedEventArgs args);
        public event ColorChangedEventHandler ColorChanged;
        #endregion

        #region Fields
        private HslColor colorHSL = HslColor.FromAhsl(255);
        private ColorModes colorMode;
        private Color colorRGB = Color.Empty;
        private bool mouseMoving;
        private int position;
        private bool setHueSilently;
        private Color nubFillColor = Color.White;
        private Color nubPenColor = Color.Blue;
        private Direction orientation = Direction.Horizontal;
        private bool hover;
        #endregion

        #region Properties
        [Category("ColorSlider")]
        public Color ColorRGB
        {
            get { return colorRGB; }
            set
            {
                colorRGB = value;
                if (!setHueSilently)
                {
                    colorHSL = HslColor.FromColor(ColorRGB);
                }
                ResetSlider();
                Refresh();
            }
        }

        [Category("ColorSlider")]
        public HslColor ColorHSL
        {
            get { return colorHSL; }
            set
            {
                colorHSL = value;
                colorRGB = colorHSL.RgbValue;
                ResetSlider();
                Refresh();
            }
        }

        [Category("ColorSlider")]
        public ColorModes ColorMode
        {
            get { return colorMode; }
            set
            {
                colorMode = value;
                ResetSlider();
                Refresh();
            }
        }

        [Category("ColorSlider")]
        [DefaultValue(typeof(Color), "White")]
        public Color NubFillColor
        {
            get { return nubFillColor; }
            set
            {
                nubFillColor = value;
                Invalidate();
            }
        }

        [Category("ColorSlider")]
        [DefaultValue(typeof(Color), "Black")]
        public Color NubPenColor
        {
            get { return nubPenColor; }
            set
            {
                nubPenColor = value;
                Invalidate();
            }
        }

        [Category("ColorSlider")]
        public int NubPosition
        {
            get { return position; }
            set
            {
                int val = value;
                switch (orientation)
                {
                    case Direction.Horizontal:
                        val = MathExtensions.LimitToRange(val, 0, Width - 9);
                        break;
                    case Direction.Vertical:
                        val = MathExtensions.LimitToRange(val, 0, Height - 9);
                        break;

                }

                if (val != position)
                {
                    position = val;
                    ResetHSLRGB();
                    Refresh();
                    if (ColorChanged != null)
                    {
                        ColorChanged(this, new ColorChangedEventArgs(colorRGB));
                    }
                }
            }
        }

        [Category("ColorSlider")]
        public Direction Orientation
        {
            get { return orientation; }
            set
            {
                orientation = value;
                Refresh();
            }
        }
        #endregion

        #region Constructors
        public ColorSlider()
        {
            //InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

            colorHSL = HslColor.FromAhsl(1.0, 1.0, 1.0);
            colorRGB = colorHSL.RgbValue;
            colorMode = ColorModes.Hue;
        }
        #endregion

        #region Overridden Methods
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            mouseMoving = true;
            switch (orientation)
            {
                case Direction.Horizontal:
                    NubPosition = e.X - 4;
                    break;

                case Direction.Vertical:
                    NubPosition = e.Y - 4;
                    break;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            mouseMoving = false;
        }

        protected override void OnMouseLeave(System.EventArgs e)
        {
            base.OnMouseLeave(e);
            hover = false;
            Refresh();
        }

        protected override void OnMouseEnter(System.EventArgs e)
        {
            base.OnMouseEnter(e);
            hover = true;
            Refresh();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (mouseMoving)
            {
                if (mouseMoving)
                {
                    switch (orientation)
                    {
                        case Direction.Horizontal:
                            NubPosition = e.X - 4;
                            break;

                        case Direction.Vertical:
                            NubPosition = e.Y - 4;
                            break;
                    }
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            HslColor color = HslColor.FromAhsl(255);
            switch (ColorMode)
            {
                case ColorModes.Hue:
                    color.L = color.S = 1.0;
                    break;

                case ColorModes.Saturation:
                    color.H = ColorHSL.H;
                    color.L = ColorHSL.L;
                    break;

                case ColorModes.Luminance:
                    color.H = ColorHSL.H;
                    color.S = ColorHSL.S;
                    break;
            }

            //draw line by line from Top to Bottom with Pen color
            switch (orientation)
            {
                case Direction.Vertical:
                    for (int i = 0; i < (Height - 8); i++)
                    {
                        double d = 0.0;
                        if (ColorMode < ColorModes.Hue)
                        {
                            d = 255.0 - MathExtensions.Round((255.0 * i) / (Height - 8.0));
                        }
                        else
                        {
                            d = 1.0 - (i / ((double)(Height - 8)));
                        }
                        Color pencol = Color.Empty;
                        switch (ColorMode)
                        {
                            case ColorModes.Red:
                                pencol = Color.FromArgb((int)d, ColorRGB.G, ColorRGB.B);
                                break;

                            case ColorModes.Green:
                                pencol = Color.FromArgb(ColorRGB.R, (int)d, ColorRGB.B);
                                break;

                            case ColorModes.Blue:
                                pencol = Color.FromArgb(ColorRGB.R, ColorRGB.G, (int)d);
                                break;

                            case ColorModes.Hue:
                                color.H = d;
                                pencol = color.RgbValue;
                                break;

                            case ColorModes.Saturation:
                                color.S = d;
                                pencol = color.RgbValue;
                                break;

                            case ColorModes.Luminance:
                                color.L = d;
                                pencol = color.RgbValue;
                                break;
                        }

                        using (Pen pen = new Pen(pencol))
                        {
                            e.Graphics.DrawLine(pen, 8, i + 4, Width - 8, i + 4);
                        }
                    }
                    break;

                //draw line by line from Left to Right with Pen color
                case Direction.Horizontal:
                    for (int i = 0; i < (Width - 8); i++)
                    {
                        double d = 0.0;
                        if (ColorMode < ColorModes.Hue)
                        {
                            d = 255.0 - MathExtensions.Round((255.0 * i) / (Width - 8.0));
                        }
                        else
                        {
                            d = 1.0 - (i / ((double)(Width - 8)));
                        }
                        Color pencol = Color.Empty;
                        switch (ColorMode)
                        {
                            case ColorModes.Red:
                                pencol = Color.FromArgb((int)d, ColorRGB.G, ColorRGB.B);
                                break;

                            case ColorModes.Green:
                                pencol = Color.FromArgb(ColorRGB.R, (int)d, ColorRGB.B);
                                break;

                            case ColorModes.Blue:
                                pencol = Color.FromArgb(ColorRGB.R, ColorRGB.G, (int)d);
                                break;

                            case ColorModes.Hue:
                                color.H = d;
                                pencol = color.RgbValue;
                                break;

                            case ColorModes.Saturation:
                                color.S = d;
                                pencol = color.RgbValue;
                                break;

                            case ColorModes.Luminance:
                                color.L = d;
                                pencol = color.RgbValue;
                                break;
                        }

                        using (Pen pen = new Pen(pencol))
                        {
                            //e.Graphics.DrawLine(pen, i + 4, 8, i + 4, Height - 8);
                            e.Graphics.DrawLine(pen, Width - (i + 4), 8, Width - (i + 4), Height - 8);
                        }
                    }
                    break;
            }
            DrawNubSlider(e.Graphics);
        }
        #endregion

        #region Private Methods
        private void DrawNubSlider(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Point[] points = new Point[7];

            using (Pen pen = new Pen(nubPenColor))
            using (SolidBrush fill = new SolidBrush(nubFillColor))
            {
                switch (orientation)
                {
                    case Direction.Horizontal:
                        points[0] = new Point(position, 1);
                        points[1] = new Point(position, 3);
                        points[2] = new Point(position + 4, 7);
                        points[3] = new Point(position + 8, 3);
                        points[4] = new Point(position + 8, 1);
                        points[5] = new Point(position + 7, 0);
                        points[6] = new Point(position + 1, 0);
                        g.FillPolygon(hover ? Brushes.DimGray : fill, points);
                        g.DrawPolygon(pen, points);

                        //points[0] = new Point(position, Height - 2);
                        //points[1] = new Point(position, Height - 4);
                        //points[2] = new Point(position + 4, Height - 8);
                        //points[3] = new Point(position + 8, Height - 4);
                        //points[4] = new Point(position + 8, Height - 2);
                        //points[5] = new Point(position + 7, Height - 1);
                        //points[6] = new Point(position + 1, Height - 1);
                        //g.FillPolygon(hover ? Brushes.DimGray : fill, points);
                        //g.DrawPolygon(pen, points);
                        break;

                    case Direction.Vertical:
                        points[0] = new Point(1, position);
                        points[1] = new Point(3, position);
                        points[2] = new Point(7, position + 4);
                        points[3] = new Point(3, position + 8);
                        points[4] = new Point(1, position + 8);
                        points[5] = new Point(0, position + 7);
                        points[6] = new Point(0, position + 1);
                        g.FillPolygon(hover ? Brushes.DimGray : fill, points);
                        g.DrawPolygon(pen, points);

                        //points[0] = new Point(Width - 2, position);
                        //points[1] = new Point(Width - 4, position);
                        //points[2] = new Point(Width - 8, position + 4);
                        //points[3] = new Point(Width - 4, position + 8);
                        //points[4] = new Point(Width - 2, position + 8);
                        //points[5] = new Point(Width - 1, position + 7);
                        //points[6] = new Point(Width - 1, position + 1);
                        //g.FillPolygon(fill, points);
                        //g.DrawPolygon(pen, points);
                        break;
                }
            }
        }

        private void ResetSlider()
        {
            double h = 0.0;
            switch (ColorMode)
            {
                case ColorModes.Red:
                    h = colorRGB.R / 255.0;
                    break;

                case ColorModes.Green:
                    h = colorRGB.G / 255.0;
                    break;

                case ColorModes.Blue:
                    h = colorRGB.B / 255.0;
                    break;

                case ColorModes.Hue:
                    h = colorHSL.H;
                    break;

                case ColorModes.Saturation:
                    h = colorHSL.S;
                    break;

                case ColorModes.Luminance:
                    h = colorHSL.L;
                    break;
            }
            switch (orientation)
            {
                case Direction.Horizontal:
                    position = ClientRectangle.X + MathExtensions.Round((Width - 9) * h);
                    break;

                case Direction.Vertical:
                    position = (Height - 9) - MathExtensions.Round((Height - 9) * h);
                    break;
            }
        }

        private void ResetHSLRGB()
        {
            setHueSilently = true;

            switch (orientation)
            {
                case Direction.Vertical:
                    switch (ColorMode)
                    {
                        case ColorModes.Red:
                            ColorRGB = Color.FromArgb(255 - MathExtensions.Round((255.0 * position) / (Height - 9)), ColorRGB.G, ColorRGB.B);
                            ColorHSL = HslColor.FromColor(ColorRGB);
                            break;

                        case ColorModes.Green:
                            ColorRGB = Color.FromArgb(ColorRGB.R, 255 - MathExtensions.Round((255.0 * position) / (Height - 9)), ColorRGB.B);
                            ColorHSL = HslColor.FromColor(ColorRGB);
                            break;

                        case ColorModes.Blue:
                            ColorRGB = Color.FromArgb(ColorRGB.R, ColorRGB.G, 255 - MathExtensions.Round((255.0 * position) / (Height - 9)));
                            ColorHSL = HslColor.FromColor(ColorRGB);
                            break;

                        case ColorModes.Hue:
                            colorHSL.H = 1.0 - (position / ((double)(Height - 9)));
                            ColorRGB = ColorHSL.RgbValue;
                            break;

                        case ColorModes.Saturation:
                            colorHSL.S = 1.0 - (position / ((double)(Height - 9)));
                            ColorRGB = ColorHSL.RgbValue;
                            break;

                        case ColorModes.Luminance:
                            colorHSL.L = 1.0 - (position / ((double)(Height - 9)));
                            ColorRGB = ColorHSL.RgbValue;
                            break;
                    }
                    break;

                case Direction.Horizontal:
                    switch (ColorMode)
                    {
                        case ColorModes.Red:
                            ColorRGB = Color.FromArgb(MathExtensions.Round((255.0 * position) / (Width - 9)), ColorRGB.G, ColorRGB.B);
                            ColorHSL = HslColor.FromColor(ColorRGB);
                            break;

                        case ColorModes.Green:
                            ColorRGB = Color.FromArgb(ColorRGB.R, MathExtensions.Round((255.0 * position) / (Width - 9)), ColorRGB.B);
                            ColorHSL = HslColor.FromColor(ColorRGB);
                            break;

                        case ColorModes.Blue:
                            ColorRGB = Color.FromArgb(ColorRGB.R, ColorRGB.G, MathExtensions.Round((255.0 * position) / (Width - 9)));
                            ColorHSL = HslColor.FromColor(ColorRGB);
                            break;

                        case ColorModes.Hue:
                            colorHSL.H = (position / ((double)(Width - 9)));
                            ColorRGB = ColorHSL.RgbValue;
                            break;

                        case ColorModes.Saturation:
                            colorHSL.S = (position / ((double)(Width - 9)));
                            ColorRGB = ColorHSL.RgbValue;
                            break;

                        case ColorModes.Luminance:
                            colorHSL.L = (position / ((double)(Width - 9)));
                            ColorRGB = ColorHSL.RgbValue;
                            break;
                    }
                    break;
            }

            setHueSilently = false;
        }
        #endregion

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ColorSlider
            // 
            this.Name = "ColorSlider";
            this.Size = new System.Drawing.Size(100, 100);
            this.ResumeLayout(false);

        }
    }
}
