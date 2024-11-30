using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ColorPicker.Controls
{
    public class NickRadioButton : RadioButton
    {
        #region Fields
        //Fields
        private Color checkedColor = Color.Gray;
        private Color uncheckedColor = Color.Gray;
        private Color hoverColor = Color.White;
        private bool hover;
        #endregion

        #region Properties
        //Properties
        [Category("NickRadioButton")]
        public Color CheckedColor
        {
            get
            {
                return checkedColor;
            }

            set
            {
                checkedColor = value;
                Invalidate();
            }
        }

        [Category("NickRadioButton")]
        public Color UncheckedColor
        {
            get
            {
                return uncheckedColor;
            }

            set
            {
                uncheckedColor = value;
                Invalidate();
            }
        }

        [Category("NickRadioButton")]
        public Color HoverColor
        {
            get
            {
                return hoverColor;
            }

            set
            {
                hoverColor = value;
            }
        }
        #endregion

        #region Constructor
        //Constructor
        public NickRadioButton()
        {
            MinimumSize = new Size(0, 21);
        }
        #endregion

        #region Overridden Methods
        //Override methodes
        protected override void OnPaint(PaintEventArgs e)
        {
            //Fields
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            float rbBorderSize = 15f;
            float rbCheckSize = 9f;
            RectangleF rectRbBorder = new RectangleF()
            {
                X = 0.5f,
                Y = (Height - rbBorderSize) / 2,
                Width = rbBorderSize,
                Height = rbBorderSize
            };
            RectangleF rectRbCheck = new RectangleF()
            {
                X = rectRbBorder.X + ((rectRbBorder.Width - rbCheckSize) / 2),
                Y = (Height - rbCheckSize) / 2,
                Width = rbCheckSize,
                Height = rbCheckSize
            };

            //Drawing
            using (Pen penBorder = new Pen(checkedColor, 1.6f))
            using (SolidBrush brushRbCheck = new SolidBrush(checkedColor))
            using (SolidBrush brushText = new SolidBrush(ForeColor))
            {
                //Draw surface
                g.Clear(BackColor);

                //Draw Radio Button
                if (Checked)
                {
                    g.DrawEllipse(penBorder, rectRbBorder);
                    g.FillEllipse(brushRbCheck, rectRbCheck);
                }
                else
                {
                    penBorder.Color = uncheckedColor;
                    g.DrawEllipse(penBorder, rectRbBorder);
                }
                if (hover)
                {
                    penBorder.Color = hoverColor;
                    g.DrawEllipse(penBorder, rectRbBorder);
                }

                //Draw text
                g.DrawString(Text,
                    Font,
                    brushText,
                    rbBorderSize + 8,
                    (Height - TextRenderer.MeasureText(Text, Font).Height) / 2);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Width = TextRenderer.MeasureText(Text, Font).Width + 30;
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            hover = true;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            hover = false;
        }
        #endregion
    }
}
