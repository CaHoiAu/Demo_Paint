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
            bm = new Bitmap(canvas.Width, canvas.Height);
            g = Graphics.FromImage(bm);
            g.Clear(Color.White);
            canvas.Image = bm;
        }
        Bitmap bm;
        Graphics g;
        bool paint = false;
        Point px, py;
        Pen p = new Pen(Color.Black, 1);
        int index = 1;
        private void canvas_MouseDown(object sender, MouseEventArgs e)
        {
            paint = true;
            py = e.Location;

        }
        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            string s;
            s = e.X.ToString() + ", " + e.Y.ToString() + "px";
            toolStripStatusLabel2.Text = s;

            if (paint)
            {
                if (index == 1)
                {
                    px = e.Location;
                    g.DrawLine(p, px, py);
                    py = px;
                }
            }
            canvas.Invalidate();
        }
        private void canvas_MouseUp(object sender, MouseEventArgs e)
        {
            paint = false;
        }

        private void canvas_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = "";
        }

        private void btnPen_Click(object sender, EventArgs e)
        {
            index = 1;
        }

        private void canvas_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
