using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
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
        int brushsize = 1;
        int index;
        
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
                    Pen p = new Pen(pic_ColorStroke.BackColor, brushsize);
                    p.StartCap = LineCap.Round;
                    p.EndCap = LineCap.Round;
                    p.LineJoin = LineJoin.Round;
                    px = e.Location;
                    g.DrawLine(p, px, py);
                    py = px;
                }
                if (index == 2)
                {
                    Pen eraser = new Pen(Color.White, brushsize);
                    eraser.StartCap = LineCap.Round;
                    eraser.EndCap = LineCap.Round;
                    eraser.LineJoin = LineJoin.Round;
                    px = e.Location;
                    g.DrawLine(eraser, px, py);
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
        private void btnEraser_Click(object sender, EventArgs e)
        {
            index = 2;
        }
        private void numUD_Size_ValueChanged(object sender, EventArgs e)
        {
            brushsize = int.Parse(numUD_Size.Value.ToString());
        }

        private void btnColorStroke_Click(object sender, EventArgs e)
        {
            using (Form2 colorDialog = new Form2())
            {
                colorDialog.StartPosition = FormStartPosition.CenterParent;
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    pic_ColorStroke.BackColor = colorDialog.selectedColor;
                }
            }
        }

        private void btnColorFill_Click(object sender, EventArgs e)
        {
            using (Form2 colorDialog = new Form2())
            {
                colorDialog.StartPosition = FormStartPosition.CenterParent;
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    pic_ColorFill.BackColor = colorDialog.selectedColor;
                }
            }
        }
        private void validate(Bitmap bm, Stack<Point> sp, int x, int y, Color old_clr, Color new_clr)
        {
            Color cx=bm.GetPixel(x, y);
            if (cx == old_clr)
            {
                sp.Push(new Point(x, y));
                bm.SetPixel(x, y, new_clr);
            }
        }
        static Point set_point(PictureBox pb, Point pt)
        {
            float pX = 1f * pb.Image.Width / pb.Width;
            float pY=1f*pb.Height / pb.Height;
            return new Point((int)(pt.X*pX),(int)(pt.Y*pY));
        }

        private void canvas_MouseClick(object sender, MouseEventArgs e)
        {
            if (index == 3)
            {
                Point point = set_point(canvas, e.Location);
                Fill(bm, point.X,point.Y,pic_ColorFill.BackColor);
            }
        }
        private void btnBucket_Click(object sender, EventArgs e)
        {
            index = 3;
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            toolStripbtnFlip.Image = Properties.Resources.flipvertical;

        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            toolStripbtnFlip.Image = Properties.Resources.flipvertical1;
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            toolStripbtnRotateRight.Image = Properties.Resources.rotate11;
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            toolStripbtnRotateRight.Image = Properties.Resources.rotate21;
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            toolStripbtnRotateRight.Image = Properties.Resources.rotate180_1;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            btnSelection.Image = Properties.Resources.noun_dotted_rectangle11;
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            btnSelection.Image = Properties.Resources.freeform1;
        }

        private void Fill(Bitmap bm, int x, int y, Color new_clr)
        {
            Color old_clr=bm.GetPixel(x, y);
            Stack<Point> pixel = new Stack<Point>();
            pixel.Push(new Point(x, y));
            bm.SetPixel(x, y, new_clr);
            if (old_clr == new_clr) return;
            while(pixel.Count > 0)
            {
                Point pt=(Point)pixel.Pop();
                if(pt.X>0 && pt.Y>0 && pt.X<bm.Width-1 && pt.Y < bm.Height - 1)
                {
                    validate(bm,pixel,pt.X-1,pt.Y,old_clr,new_clr);
                    validate(bm, pixel, pt.X, pt.Y-1, old_clr, new_clr);
                    validate(bm, pixel, pt.X + 1, pt.Y, old_clr, new_clr);
                    validate(bm, pixel, pt.X, pt.Y+1, old_clr, new_clr);
                }
            }
        }
    }
}
