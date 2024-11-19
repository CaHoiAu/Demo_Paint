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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            string s;
            s = e.X.ToString() + ", " + e.Y.ToString() + "px";
            toolStripStatusLabel2.Text = s;
        }

        private void canvas_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = "";
        }
        private void btn_MouseDown(object sender, MouseEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                btn.FlatAppearance.BorderSize = 1; // Thêm viền khi nhấn
                btn.FlatAppearance.BorderColor = Color.Gray; // Màu viền khi nhấn
                btn.BackColor = Color.LightGray; // Màu nền khi nhấn
            }
        }

        private void btn_Click(object sender, EventArgs e)
        {
            // Lấy nút vừa được nhấn
            Button clickedBtn = sender as Button;

            // Kiểm tra xem nút không null
            if (clickedBtn == null)
                return;

            foreach (Control ctrl in panel1.Controls)
            {
                if (ctrl is Button btn)
                {
                    btn.FlatAppearance.BorderSize = 0; // Loại bỏ viền
                    btn.BackColor = Color.White; // Màu nền mặc định
                }
            }

            // Đặt trạng thái đặc biệt cho nút vừa nhấn
            clickedBtn.FlatAppearance.BorderSize = 1; // Thêm viền
            clickedBtn.FlatAppearance.BorderColor = Color.LightGray; // Màu viền đặc biệt
            clickedBtn.BackColor = Color.LightGray; // Màu nền đặc biệt
        }
    }
}
