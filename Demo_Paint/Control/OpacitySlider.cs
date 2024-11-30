using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ColorPicker
{
    [DefaultEvent("OpacityChanged")]
    public partial class OpacitySlider : UserControl
    {
        #region Events
        public delegate void OpacityChangedEventHandler(object sender, OpacityChangedEventArgs args);
        public event OpacityChangedEventHandler OpacityChanged;
        #endregion

        #region Enum
        public enum Direction
        {
            Horizontal,
            Vertical
        }
        #endregion

        #region Fields
        private Color colorRGB = Color.Empty;
        private bool mouseMoving;
        private int position;
        private Color nubFillColor = Color.White;
        private Color nubPenColor = Color.Blue;
        private int alpha = 200;
        private Direction orientation = Direction.Horizontal;
        private bool hover;
        #endregion

        #region Properties
        [Category("OpacitySlider")]
        public int Alpha
        {
            get { return alpha; }
            set
            {
                alpha = value;
                ResetSlider();
                Refresh();
            }
        }

        [Category("OpacitySlider")]
        public Color ColorRGB
        {
            get { return colorRGB; }
            set
            {
                colorRGB = value;
                Refresh();
            }
        }

        [Category("OpacitySlider")]
        [DefaultValue(typeof(Color), "White")]
        public Color NubFillColor
        {
            get { return nubFillColor; }
            set
            {
                nubFillColor = value;
                Refresh();
            }
        }

        [Category("OpacitySlider")]
        [DefaultValue(typeof(Color), "Black")]
        public Color NubPenColor
        {
            get { return nubPenColor; }
            set
            {
                nubPenColor = value;
                Refresh();
            }
        }

        [Category("OpacitySlider")]
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
                    ResetAlpha();
                    Refresh();
                    if (OpacityChanged != null)
                    {
                        OpacityChanged(this, new OpacityChangedEventArgs(alpha));
                    }
                }
            }
        }

        [Category("OpacitySlider")]
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
        public OpacitySlider()
        {
            //InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
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

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            hover = true;
            Refresh();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            hover = false;
            Refresh();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            mouseMoving = false;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
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

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Rectangle r = new Rectangle();

            switch (orientation)
            {
                case Direction.Horizontal:
                    r = new Rectangle(ClientRectangle.X + 4, ClientRectangle.Y + 8, Width - 8, Height - 15);
                    break;

                case Direction.Vertical:
                    r = new Rectangle(ClientRectangle.X + 8, ClientRectangle.Y + 4, Width - 15, Height - 8);
                    break;
            }

            using (HatchBrush hb = new HatchBrush(HatchStyle.LargeCheckerBoard, Color.Silver, Color.White))
            using (LinearGradientBrush lgb = new LinearGradientBrush(r, ColorRGB, Color.Transparent, orientation == Direction.Horizontal ? -180f : 90f, true))
            {
                switch (orientation)
                {
                    case Direction.Horizontal:
                        e.Graphics.FillRectangle(hb, r.X + 1, r.Y, r.Width - 3, r.Height);
                        e.Graphics.FillRectangle(lgb, r.X + 1, r.Y, r.Width, r.Height);
                        break;

                    case Direction.Vertical:
                        e.Graphics.FillRectangle(hb, r.X, r.Y + 1, r.Width, r.Height - 3);
                        e.Graphics.FillRectangle(lgb, r.X, r.Y + 1, r.Width, r.Height);
                        break;
                }
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
                        //g.FillPolygon(hover ? Brushes.DimGray : fill, points);
                        //g.DrawPolygon(pen, points);
                        break;
                }
            }
        }

        private void ResetSlider()
        {
            double n = 0.0;
            n = Alpha / 255.0;
            switch (orientation)
            {
                case Direction.Horizontal:
                    position = ClientRectangle.X + MathExtensions.Round((Width - 9) * n);
                    break;

                case Direction.Vertical:
                    position = (Height - 9) - MathExtensions.Round((Height - 9) * n);
                    break;
            }
        }

        private void ResetAlpha()
        {
            switch (orientation)
            {
                case Direction.Horizontal:
                    Alpha = MathExtensions.Round((255.0f * position) / (Width - 9) - 1 / 255);
                    break;

                case Direction.Vertical:
                    Alpha = 255 - MathExtensions.Round((255.0 * position) / (Height - 9));
                    break;
            }
        }
        #endregion

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // OpacitySlider
            // 
            this.Name = "OpacitySlider";
            this.Size = new System.Drawing.Size(100, 100);
            this.ResumeLayout(false);

        }
    }
}
