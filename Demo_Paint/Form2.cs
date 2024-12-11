using ColorPicker;
using ColorPicker.Controls;
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
        public Color selectedColor { get; set; }
        private HslColor colorHsl = HslColor.FromAhsl(255);
        private ColorModes colorMode = ColorModes.Hue;
        private Color colorArgb = Color.FromArgb(255, Color.Crimson);
        private bool lockUpdates = false;
        public Form2()
        {
            InitializeComponent();
            radioH.Checked = true;
            radioH.PerformClick();

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

        private void UpdateColorFields()
        {
            lockUpdates = true;
            numA.Value = colorArgb.A;
            numRed.Value = colorArgb.R;
            numGreen.Value = colorArgb.G;
            numBlue.Value = colorArgb.B;
            numHue.Value = (int)(((decimal)colorHsl.H) * 360M);
            numSaturation.Value = (int)(((decimal)colorHsl.S) * 100M);
            numLuminance.Value = (int)(((decimal)colorHsl.L) * 100M);
            UpdateGradientPanel();
            lockUpdates = false;
        }
        private void UpdateRgbFields(Color newColor)
        {
            colorHsl = HslColor.FromColor(newColor);
            colorArgb = Color.FromArgb(colorArgb.A, newColor);
            lockUpdates = true;
            numHue.Value = (int)(((decimal)colorHsl.H) * 360M);
            numSaturation.Value = (int)(((decimal)colorHsl.S) * 100M);
            numLuminance.Value = (int)(((decimal)colorHsl.L) * 100M);
            lockUpdates = false;
            colorSlider1.ColorHSL = colorHsl;
            colorBox2D1.ColorHSL = colorHsl;
        }
        private bool isUpdating = false;

        private void UpdateHslFields(HslColor newColor)
        {
            colorHsl = newColor;
            colorArgb = Color.FromArgb(colorArgb.A, newColor.RgbValue);
            lockUpdates = true;
            numRed.Value = colorArgb.R;
            numGreen.Value = colorArgb.G;
            numBlue.Value = colorArgb.B;
            lockUpdates = false;
            colorSlider1.ColorHSL = colorHsl;
            colorBox2D1.ColorHSL = colorHsl;
        }
        private void numRed_ValueChanged(object sender, EventArgs e)
        {
            if (!lockUpdates)
            {
                Color newColor = Color.FromArgb((int)numRed.Value, (int)numGreen.Value, (int)numBlue.Value);
                UpdateRgbFields(newColor);
                UpdateGradientPanel();
            }
        }

        private void numGreen_ValueChanged(object sender, EventArgs e)
        {
            if (!lockUpdates)
            {
                Color newColor = Color.FromArgb((int)numRed.Value, (int)numGreen.Value, (int)numBlue.Value);
                UpdateRgbFields(newColor);
                UpdateGradientPanel();
            }
        }

        private void numBlue_ValueChanged(object sender, EventArgs e)
        {
            if (!lockUpdates)
            {
                Color newColor = Color.FromArgb((int)numRed.Value, (int)numGreen.Value, (int)numBlue.Value);
                UpdateRgbFields(newColor);
                UpdateGradientPanel();
            }
        }

        private void numHue_ValueChanged(object sender, EventArgs e)
        {
            if (!lockUpdates)
            {
                HslColor newColor = HslColor.FromAhsl((int)numHue.Value / 360f, colorHsl.S, colorHsl.L);
                UpdateHslFields(newColor);
                UpdateGradientPanel();
            }
        }

        private void numSaturation_ValueChanged(object sender, EventArgs e)
        {
            if (!lockUpdates)
            {
                HslColor newColor = HslColor.FromAhsl(colorHsl.A, colorHsl.H, (int)numSaturation.Value / 100f, colorHsl.L);
                UpdateHslFields(newColor);
                UpdateGradientPanel();
            }
        }

        private void numLuminance_ValueChanged(object sender, EventArgs e)
        {
            if (!lockUpdates)
            {
                HslColor newColor = HslColor.FromAhsl(colorHsl.A, colorHsl.H, colorHsl.S, (int)numLuminance.Value / 100f);
                UpdateHslFields(newColor);
                UpdateGradientPanel();
            }
        }


        private void ColorModeChangedHandler(object sender, EventArgs e)
        {
            if (sender == radioR)
            {
                colorMode = ColorModes.Red;
            }
            else if (sender == radioG)
            {
                colorMode = ColorModes.Green;
            }
            else if (sender == radioB)
            {
                colorMode = ColorModes.Blue;
            }
            else if (sender == radioH)
            {
                colorMode = ColorModes.Hue;
            }
            else if (sender == radioS)
            {
                colorMode = ColorModes.Saturation;
            }
            else if (sender == radioL)
            {
                colorMode = ColorModes.Luminance;
            }

            colorSlider1.ColorMode = colorMode;
            colorBox2D1.ColorMode = colorMode;
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

        private void opacitySlider1_OpacityChanged(object sender, OpacityChangedEventArgs args)
        {
            if (!lockUpdates)
            {
                colorArgb = (Color.FromArgb(opacitySlider1.Alpha, colorArgb));
                numA.Value = colorArgb.A;
                UpdateGradientPanel();
            }
        }

        private void colorSlider1_ColorChanged_1(object sender, ColorChangedEventArgs args)
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
        public static class ColorUtils
        {
            public static (int R, int G, int B, int A) HexToRgba(string hex)
            {
                // Loại bỏ dấu # nếu có
                hex = hex.TrimStart('#');

                // Kiểm tra nếu HEX có đúng 8 ký tự
                if (hex.Length == 8)
                {
                    // Chuyển đổi từng phần HEX sang giá trị RGB và Alpha
                    int a = Convert.ToInt32(hex.Substring(0, 2), 16);
                    int r = Convert.ToInt32(hex.Substring(2, 2), 16);
                    int g = Convert.ToInt32(hex.Substring(4, 2), 16);
                    int b = Convert.ToInt32(hex.Substring(6, 2), 16);

                    return (r, g, b, a);
                }
                else
                {
                    throw new ArgumentException("Invalid HEX format. Expected 8 characters.");
                }
            }

            public static (float H, float S, float L) HexToHsl(string hex)
            {
                // Xử lý HEX (loại bỏ # nếu có)
                hex = hex.TrimStart('#');
                int r = Convert.ToInt32(hex.Substring(2, 2), 16);
                int g = Convert.ToInt32(hex.Substring(4, 2), 16);
                int b = Convert.ToInt32(hex.Substring(6, 2), 16);

                return RgbToHsl(r, g, b);
            }

            public static (float H, float S, float L) RgbToHsl(int r, int g, int b)
            {
                // Chuyển đổi RGB sang HSL
                float h = 0, s = 0, l = 0;

                // Chuẩn hóa RGB
                float rf = r / 255f;
                float gf = g / 255f;
                float bf = b / 255f;

                float max = Math.Max(rf, Math.Max(gf, bf));
                float min = Math.Min(rf, Math.Min(gf, bf));
                float delta = max - min;

                // Luminance
                l = (max + min) / 2;

                // Saturation
                if (delta == 0)
                {
                    s = 0;
                }
                else
                {
                    s = (l > 0.5f) ? delta / (2 - max - min) : delta / (max + min);
                }

                // Hue
                if (delta == 0)
                {
                    h = 0;
                }
                else
                {
                    if (max == rf)
                    {
                        h = (gf - bf) / delta + (gf < bf ? 6 : 0);
                    }
                    else if (max == gf)
                    {
                        h = (bf - rf) / delta + 2;
                    }
                    else
                    {
                        h = (rf - gf) / delta + 4;
                    }
                    h /= 6;
                }

                // Trả về HSL (H từ 0 đến 1, S và L từ 0 đến 1)
                return (h * 360, s * 100, l * 100); // HSL: H từ 0-360, S và L từ 0-100
            }
        }

        private void basicColor_Click(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                numA.Value = 255;
                //RGB
                numRed.Value = btn.BackColor.R;
                numBlue.Value = btn.BackColor.B;
                numGreen.Value = btn.BackColor.G;
                //HSL
                var (h, s, l) = ColorUtils.HexToHsl(tbHex.Text);
            }
        }
        private void tbHex_TextChanged(object sender, EventArgs e)
        {
            if (sender is TextBox tb)
            {
                // Kiểm tra nếu giá trị trong TextBox có dạng HEX hợp lệ (8 ký tự #RRGGBBAA)
                if (tb.Text.Length == 9 && tb.Text.StartsWith("#"))
                {

                    // Cập nhật giá trị RGB từ HEX
                    var (r, g, b, a) = ColorUtils.HexToRgba(tb.Text);  // Cập nhật thêm giá trị Alpha
                    numRed.Value = r;
                    numGreen.Value = g;
                    numBlue.Value = b;
                    numA.Value = a;

                    // Cập nhật màu sắc trong các điều khiển khác nếu cần
                    colorArgb = Color.FromArgb(a, r, g, b); // Cập nhật Alpha (giả sử Alpha được dùng)
                    colorSlider1.ColorRGB = colorArgb;
                    colorBox2D1.ColorRGB = colorArgb;
                    UpdateGradientPanel();
                }
            }
        }

        private void numA_ValueChanged_1(object sender, EventArgs e)
        {
            if (!lockUpdates)
            {
                colorArgb = (Color.FromArgb((int)numA.Value, colorArgb));
                opacitySlider1.Alpha = colorArgb.A;
                UpdateGradientPanel();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            selectedColor = colorArgb;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
