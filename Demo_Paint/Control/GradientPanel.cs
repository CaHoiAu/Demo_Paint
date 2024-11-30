using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GradientPanel
{
    [DefaultEvent("Click")]

    public partial class GradientPanel : Panel
    {
        #region Fields
        private Color colorTop;
        private Color colorBottom;
        private Color boundPen;
        //private bool isDisabled;
        #endregion

        #region Properties
        [Category("GradientPanel")]
        public Color ColorTop
        {
            get { return colorTop; }
            set
            {
                colorTop = value;
                Refresh();
            }
        }

        [Category("GradientPanel")]
        public Color ColorBottom
        {
            get { return colorBottom; }
            set
            {
                colorBottom = value;
                Refresh();
            }
        }

        [Category("GradientPanel")]
        public Color BoundPen
        {
            get { return boundPen; }
            set
            {
                boundPen = value;
                Refresh();
            }
        }

        [Category("GradientPanel")]
        public bool IsDisabled { get; set; }

        [Category("GradientPanel")]
        public bool hover = false;
        #endregion

        #region Constructor
        public GradientPanel()
        {
            DoubleBuffered = true;
        }
        #endregion

        #region Overriden methodes
        protected override void OnPaint(PaintEventArgs e)
        {
            using (LinearGradientBrush lgb = new LinearGradientBrush(ClientRectangle, IsDisabled ? Color.LightGray : ColorTop, IsDisabled ? Color.LightGray : ColorBottom, LinearGradientMode.Vertical))
            using (HatchBrush brush = new HatchBrush(HatchStyle.LargeCheckerBoard, Color.Silver, Color.White))
            using (Pen pen = new Pen(BoundPen))
            {
                Graphics g = e.Graphics;
                //g.DrawRectangle(hover ? Pens.Blue : Pens.Silver, ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 1, ClientRectangle.Height - 1);

                g.DrawRectangle(pen, ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 1, ClientRectangle.Height - 1);


                g.FillRectangle(brush, ClientRectangle.X + 3, ClientRectangle.Y + 3, ClientRectangle.Width - 6, ClientRectangle.Height - 6);
                g.FillRectangle(lgb, ClientRectangle.X + 3, ClientRectangle.Y + 3, ClientRectangle.Width - 6, ClientRectangle.Height - 6);
            }
            base.OnPaint(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            hover = true;
            //Cursor = Cursors.Hand;
            //Size = new Size(Width + 5, Height + 5);
            Refresh();
            base.OnMouseHover(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            hover = false;
            //Cursor = Cursors.Default;
            //Size = new Size(Width - 5, Height - 5);
            Refresh();
            base.OnMouseLeave(e);
        }
        #endregion
    }
}