using ColorPicker;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo_Paint
{
    public partial class Form2 : Form
    {
        private HslColor colorHsl = HslColor.FromAhsl(255);
        private ColorModes colorMode = ColorModes.Hue;
        private Color colorArgb = Color.FromArgb(255, Color.Crimson);
        private bool lockUpdates = false;
        public Form2()
        {
            InitializeComponent();
            colorSlider1.ColorRGB = colorArgb;
            HslColor colorHSL = colorSlider1.ColorHSL;
            colorHsl = colorHSL;
            colorBox2D1.ColorHSL = colorHsl;
            numA.Value = colorArgb.A;

            UpdateGradientPanel();
            UpdateColorFields();
        }
        private void UpdateGradientPanel()
        {
            opacitySlider1.ColorRGB = Color.FromArgb(255, colorArgb);
            gradientPanel1.ColorTop = Color.FromArgb(255, colorArgb);
            gradientPanel1.ColorBottom = Color.FromArgb((int)numA.Value, colorArgb);
            //textBoxHex.Text = ColorTranslator.ToHtml(this.colorArgb);
            string hexValueA = colorArgb.A.ToString("X2");
            string hexValueR = colorArgb.R.ToString("X2");
            string hexValueG = colorArgb.G.ToString("X2");
            string hexValueB = colorArgb.B.ToString("X2");
            tbHex.Text = hexValueA.ToString() + hexValueR.ToString() + hexValueG.ToString() + hexValueB.ToString();
            gradientPanel1.Invalidate();
        }
        private void lbBasicColor_Click(object sender, EventArgs e)
        {

        }

        private void colorSlider1_ColorChanged(object sender, ColorChangedEventArgs args)
        {
            if (!lockUpdates)
            {
                HslColor colorHSL = colorSlider1.ColorHSL;
                colorHsl = colorHSL;
                colorArgb = Color.FromArgb(colorArgb.A, colorHsl.RgbValue);
                lockUpdates = true;
                colorBox2D1.ColorHSL = colorHsl;
                lockUpdates = false;
                UpdateGradientPanel();
                UpdateColorFields();
            }
        }

        private void colorBox2D1_ColorChanged(object sender, ColorChangedEventArgs args)
        {
            if (!lockUpdates)
            {
                HslColor colorHSL = colorBox2D1.ColorHSL;
                colorHsl = colorHSL;
                colorArgb = Color.FromArgb(colorArgb.A, colorHsl.RgbValue);
                lockUpdates = true;
                colorSlider1.ColorHSL = colorHsl;
                lockUpdates = false;
                UpdateGradientPanel();
                UpdateColorFields();
            }
        }
        private void UpdateColorFields()
        {
            lockUpdates = true;
            numA.Value = colorArgb.A;
            UpdateGradientPanel();
            lockUpdates = false;
        }

        private void numA_ValueChanged(object sender, EventArgs e)
        {
            if (!lockUpdates)
            {
                colorArgb = (Color.FromArgb((int)numA.Value, colorArgb));
                opacitySlider1.Alpha = colorArgb.A;
                UpdateGradientPanel();
            }
        }

        private void opacitySlider1_OpacityChanged(object sender, OpacityChangedEventArgs args)
        {
            if (!lockUpdates)
            {
                colorArgb = (Color.FromArgb(opacitySlider1.Alpha, colorArgb));
                numA.Value = colorArgb.A;
                UpdateGradientPanel();
            }
        }
    }
}
