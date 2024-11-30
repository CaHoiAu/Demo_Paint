using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ColorPicker.Controls
{
    public class NickButton : Button
    {
        #region Fields
        //Fields
        private int borderSize = 1;
        private int borderRadius = 40;
        private Color borderColor = Color.PaleGreen;
        private Color textColor = Color.Silver;
        #endregion

        #region Properties
        [Category("NickButton")]
        public int BorderSize
        {
            get
            {
                return borderSize;
            }

            set
            {
                borderSize = value;
                Invalidate();
            }
        }

        [Category("NickButton")]
        public int BorderRadius
        {
            get
            {
                return borderRadius;
            }

            set
            {
                if (value <= Height)
                    borderRadius = value;
                else borderRadius = Height;
                Invalidate();
            }
        }

        [Category("NickButton")]
        public Color BorderColor
        {
            get
            {
                return borderColor;
            }

            set
            {
                borderColor = value;
                Invalidate();
            }
        }

        [Category("NickButton")]
        public Color BackgroundColor
        {
            get
            {
                return BackColor;
            }

            set
            {
                BackColor = value;
            }
        }


        [Category("NickButton")]
        public Color TextColor
        {
            get
            {
                return ForeColor;
            }

            set
            {
                ForeColor = value;
            }
        }
        #endregion

        #region Constructor
        //Constructor
        public NickButton()
        {
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            Size = new Size(150, 40);
            this.BackColor = Color.MediumSlateBlue;
            this.ForeColor = Color.White;
            Resize += new EventHandler(Button_Resize);
        }
        #endregion

        #region Private Methods
        //Methode
        private GraphicsPath GetFigurePath(RectangleF r, float radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(r.X, r.Y, radius, radius, 180, 90);
            path.AddArc(r.Width - radius, r.Y, radius, radius, 270, 90);
            path.AddArc(r.Width - radius, r.Height - radius, radius, radius, 0, 90);
            path.AddArc(r.X, r.Height - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void Container_BackColorChanged(object sender, EventArgs e)
        {
            if (DesignMode)
                Invalidate();
        }

        private void Button_Resize(object sender, EventArgs e)
        {
            if (borderRadius > Height)
                borderRadius = Height;
        }

        #endregion

        #region Overridden Methods
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            RectangleF rectSurface = new RectangleF(0, 0, Width, Height);
            RectangleF rectBorder = new RectangleF(1, 1, Width - 0.8f, Height - 1);

            if (borderRadius > 2)//rounded button
            {
                using (GraphicsPath pathSurface = GetFigurePath(rectSurface, borderRadius))
                using (GraphicsPath pathBorder = GetFigurePath(rectBorder, borderRadius - 1f))
                using (Pen penSurface = new Pen(Parent.BackColor, 2))
                using (Pen penBorder = new Pen(borderColor, borderSize))
                {
                    penBorder.Alignment = PenAlignment.Inset;
                    Region = new Region(pathSurface);
                    //draw surface border for better result
                    g.DrawPath(penSurface, pathSurface);
                    //draw border
                    if (borderSize >= 1)
                    {
                        g.DrawPath(penBorder, pathBorder);
                    }
                }
            }
            else//normal button
            {
                Region = new Region(rectSurface);
                if (borderSize >= 1)
                {
                    using (Pen penBorder = new Pen(borderColor, borderSize))
                    {
                        penBorder.Alignment = PenAlignment.Inset;
                        g.DrawRectangle(penBorder, 0, 0, Width - 1, Height - 1);
                    }
                }
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            Parent.BackColorChanged += new EventHandler(Container_BackColorChanged);

        }
        #endregion

    }
}
