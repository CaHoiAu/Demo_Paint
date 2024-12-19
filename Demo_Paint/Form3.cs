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
    public partial class Form3 : Form
    {
        public bool IsBold { get; private set; }
        public bool IsItalic { get; private set; }
        public bool IsUnderline { get; private set; }

        public Form3()
        {
            InitializeComponent();
            foreach (FontFamily font in FontFamily.Families)
            {
                cbbFont.Items.Add(font.Name);
            }
            int[] fontSizes = { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            cbbSize.Items.AddRange(fontSizes.Select(s => s.ToString()).ToArray());
            cbbFont.SelectedItem = "Arial";
            cbbSize.SelectedItem = "14";
        }
        public string SelectedFont
        {
            get => cbbFont.SelectedItem?.ToString();
            set => cbbFont.SelectedItem = value;
        }

        public string SelectedFontSize
        {
            get => cbbSize.SelectedItem?.ToString();
            set => cbbSize.SelectedItem = value;
        }

        private void Form3_Load(object sender, EventArgs e)
        {        }
        public FontFamily GetFontFamily()
        {
            return new FontFamily(cbbFont.SelectedItem.ToString());
        }
        public Font GetFont()
        {
            return new Font(GetFontFamily(), float.Parse(cbbSize.SelectedItem.ToString()));
        }

        public int GetBrushSize()
        {
            if (cbbSize.SelectedItem != null)
            {
                // Lấy giá trị được chọn và chuyển đổi thành số
                return int.Parse(cbbSize.SelectedItem.ToString());
            }
            throw new InvalidOperationException("Brush size not selected.");
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void btnBold_Click(object sender, EventArgs e)
        {
            IsBold = !IsBold;
        }

        private void btnItalic_Click(object sender, EventArgs e)
        {
            IsItalic = !IsItalic;
        }

        private void btnUnderline_Click(object sender, EventArgs e)
        {
            IsUnderline = !IsUnderline;
        }
    }
}
