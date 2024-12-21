using Demo_Paint.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Demo_Paint.Form3;
using static System.Net.Mime.MediaTypeNames;

namespace Demo_Paint
{
    
    public partial class Form1 : Form
    {
        Bitmap bm;
        Graphics g;
        bool paint = false;
        Point px, py;
        int brushsize = 1;
        int index = 0;

        float zoomFactor = 1.0f; // Tỷ lệ phóng to/thu nhỏ (1.0 = 100%)
        
        private List<DrawLine> lines = new List<DrawLine>();
        //các biến undo, redo
        Stack<Bitmap> undoStack = new Stack<Bitmap>();
        Stack<Bitmap> redoStack = new Stack<Bitmap>();
        //các biến text
        private Rectangle textBoxRect; // Hình chữ nhật chứa văn bản
        private string inputText = ""; // Văn bản nhập
        private Font textFont = new Font("Arial", 14); // Font chữ
        private Brush textBrush = Brushes.Black; // Màu chữ
        private bool isTyping = false; // Cờ nhập văn bản
        private int padding = 5;
        private List<Tuple<Rectangle, string>> textBoxes = new List<Tuple<Rectangle, string>>();
        private string currentAlignment;
        //Các biến cho Selection
        private enum SelectionMode { Rectangle, FreeForm }
        private SelectionMode currentMode = SelectionMode.Rectangle; // Mặc định chọn hình chữ nhật

        private bool isSelecting = false; // Trạng thái vẽ vùng chọn
        private Point startPoint; // Điểm bắt đầu vùng chọn
        private Rectangle selectionRectangle; // Hình chữ nhật vùng chọn

        private List<Point> freeFormPoints; // Danh sách điểm cho hình tự do
        private Region freeFormRegion; // Vùng chọn hình tự do


        //Form cho font
        Form3 fontDialog = new Form3();
        
        public Form1()
        {
            this.KeyPreview = true; // Đảm bảo Form nhận sự kiện KeyDown trước các control khác

            InitializeComponent();
            InitializeCanvas();
            fontDialog.FormBorderStyle = FormBorderStyle.None;
            fontDialog.Owner = this;

            freeFormPoints = new List<Point>(); // Khởi tạo danh sách
        }

        public class DrawLine
        {
            public Point StartPoint { get; set; }
            public Point EndPoint { get; set; }
            public Pen DrawingPen { get; set; }

            public DrawLine(Point start, Point end)
            {
                StartPoint = start;
                EndPoint = end;
            }
        }

        private void InitializeCanvas()
        {
            // Khởi tạo Bitmap và Graphics để vẽ
            bm = new Bitmap(canvas.Width, canvas.Height);
            g = Graphics.FromImage(bm);
            g.Clear(Color.White);
            canvas.Image = bm;
        }
        public class CustomCursor
        {
            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            private static extern IntPtr CreateIconIndirect(ref ICONINFO iconInfo);

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            private static extern bool DestroyIcon(IntPtr hIcon);

            [StructLayout(LayoutKind.Sequential)]
            private struct ICONINFO
            {
                public bool fIcon;          // Nếu false thì đây là con trỏ (không phải icon)
                public int xHotspot;       // Điểm hotspot X
                public int yHotspot;       // Điểm hotspot Y
                public IntPtr hbmMask;     // Bitmap mask
                public IntPtr hbmColor;    // Bitmap màu
            }

            public static Cursor CreateCursorFromBitmap(Bitmap bitmap, int hotspotX, int hotspotY)
            {
                ICONINFO iconInfo = new ICONINFO
                {
                    fIcon = false, // Là con trỏ
                    xHotspot = hotspotX,
                    yHotspot = hotspotY,
                    hbmMask = bitmap.GetHbitmap(), // Tạo mask từ Bitmap
                    hbmColor = bitmap.GetHbitmap() // Tạo màu từ Bitmap
                };

                IntPtr cursorPtr = CreateIconIndirect(ref iconInfo);
                Cursor customCursor = new Cursor(cursorPtr);

                // Giải phóng tài nguyên
                DestroyIcon(iconInfo.hbmMask);
                DestroyIcon(iconInfo.hbmColor);

                return customCursor;
            }
        }
        private void canvas_MouseDown(object sender, MouseEventArgs e)
        {
            paint = true;
            py = e.Location;
            SaveState();
            if (index == 7)
            {
                if (e.Button == MouseButtons.Left)
                {
                    isSelecting = true;

                    if (currentMode == SelectionMode.Rectangle)
                    {
                        // Chế độ hình chữ nhật
                        startPoint = e.Location;
                        selectionRectangle = new Rectangle(e.Location, new Size(0, 0));
                    }

                    canvas.Invalidate();
                }
            }
            if (index == 8)
            {
                if (e.Button == MouseButtons.Left)
                {
                    isSelecting = true;
                    if (currentMode == SelectionMode.FreeForm)
                    {
                        if (freeFormPoints == null)
                        {
                            freeFormPoints = new List<Point>(); // Đảm bảo khởi tạo nếu chưa có
                        }
                        freeFormPoints.Add(e.Location); // Thêm điểm mới vào danh sách
                    }
                    canvas.Invalidate();
                }
            }
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
            if(isSelecting)
            {
                if (currentMode == SelectionMode.Rectangle)
                {
                    // Cập nhật kích thước hình chữ nhật
                    int width = Math.Abs(e.X - startPoint.X);
                    int height = Math.Abs(e.Y - startPoint.Y);
                    int x = Math.Min(e.X, startPoint.X);
                    int y = Math.Min(e.Y, startPoint.Y);

                    selectionRectangle = new Rectangle(x, y, width, height);
                }
                else if (currentMode == SelectionMode.FreeForm)
                {
                    // Cập nhật đường vẽ tự do
                    freeFormPoints.Add(e.Location);
                }
            }
            canvas.Invalidate();
        }
        private bool isShapeClosed = false;
        private void canvas_MouseUp(object sender, MouseEventArgs e)
        {
            paint = false;
            if (isSelecting)
            {
                isSelecting = false;
                if (currentMode == SelectionMode.Rectangle) { return; }
                if (currentMode == SelectionMode.FreeForm && freeFormPoints.Count > 1)
                {
                    // Ensure the shape is closed by adding the final point only once
                    if (!isShapeClosed)
                    {
                        freeFormPoints.Add(e.Location); // Add the final point where the mouse was released
                        isShapeClosed = true;
                    }
                    GraphicsPath path = new GraphicsPath();
                    path.AddPolygon(freeFormPoints.ToArray());
                    freeFormRegion = new Region(path);
                    canvas.Invalidate(); // Redraw the canvas to show the closed shape
                }
                canvas.Invalidate();
            }
        }
        private void canvas_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = "";
            this.Cursor = Cursors.Default;
        }

        private void btnPen_Click(object sender, EventArgs e)
        {
            index = 1;
            if (sender is Button btn && btn.Image != null)
            {

                // Lấy biểu tượng từ hình ảnh của Button
                Bitmap bitmap = new Bitmap(Properties.Resources.pencil_drawing2);

                // Tạo con trỏ chuột từ hình ảnh
                Cursor customCursor = CustomCursor.CreateCursorFromBitmap(bitmap, 0, 32);

                // Đặt con trỏ chuột thành con trỏ tùy chỉnh
                this.Cursor = customCursor;
            }
            fontDialog.Hide(); // Ẩn form phụ
            if (isTyping)
            {
                // Lưu văn bản hiện tại nếu có
                if (!string.IsNullOrEmpty(inputText))
                {
                    textBoxes.Add(new Tuple<Rectangle, string>(textBoxRect, inputText));
                }

                isTyping = false; // Tắt chế độ nhập
                inputText = ""; // Xóa văn bản đang nhập (sau khi đã lưu)
                canvas.Invalidate(); // Yêu cầu vẽ lại canvas để xóa khung hình chữ nhật
            }
        }
        private Bitmap ResizeIcon(Bitmap originalIcon, int width, int height)
        {
            // Tạo một bitmap mới với kích thước mong muốn
            Bitmap resizedBitmap = new Bitmap(width, height);
            using (Graphics graphics = Graphics.FromImage(resizedBitmap))
            {
                // Vẽ lại hình ảnh với kích thước mới
                graphics.DrawImage(originalIcon, 0, 0, width, height);
            }
            return resizedBitmap;
        }

        private void btnEraser_Click(object sender, EventArgs e)
        {
            index = 2;
            if (sender is Button btn && btn.Image != null)
            {
                Bitmap bitmap = ResizeIcon(Properties.Resources.square_drawing11, brushsize, brushsize);
                // Tạo con trỏ chuột từ hình ảnh
                Cursor customCursor = CustomCursor.CreateCursorFromBitmap(bitmap, bitmap.Size.Width/2, bitmap.Size.Height/2);

                // Đặt con trỏ chuột thành con trỏ tùy chỉnh
                this.Cursor = customCursor;
            }
            if (isTyping)
            {
                if (!string.IsNullOrEmpty(inputText))
                {
                    using (Graphics graphics = Graphics.FromImage(bm))
                    {
                        DrawTextInRectangle(graphics, inputText, textBoxRect, textFont, textBrush);
                    }

                    // Reset trạng thái nhập
                    isTyping = false;
                    inputText = "";
                    canvas.Invalidate();
                }
            }
            fontDialog.Hide(); // Ẩn form phụ
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
            else if (index == 4)
            {
                Point point=set_point(canvas, e.Location);
                if(point.X>=0 && point.X<bm.Width && point.Y>=0 && point.Y < bm.Height)
                {
                    Color pickedClr=bm.GetPixel(point.X,point.Y);
                    pic_ColorStroke.BackColor=pickedClr;
                }
            }
            else if (index == 5)
            {
                if (e.Button == MouseButtons.Left)
                {
                    zoomFactor += 0.1f;
                    UpdateCanvasZoom();
                }
                else if(e.Button == MouseButtons.Right)
                {
                    zoomFactor = Math.Max(0.1f, zoomFactor - 0.1f);
                    UpdateCanvasZoom();
                }
            }
            else if (index == 6) // Chế độ nhập văn bản
            {
                // Lưu văn bản hiện tại vào Bitmap trước khi tạo khung mới
                if (isTyping && !string.IsNullOrEmpty(inputText))
                {
                    using (Graphics graphics = Graphics.FromImage(bm))
                    {
                        DrawTextInRectangle(graphics, inputText, textBoxRect, textFont, textBrush);
                    }

                    isTyping = false;
                    inputText = "";
                    canvas.Invalidate();
                }

                // Tạo khung văn bản mới
                int rectWidth = 200; // Chiều rộng cố định
                int rectHeight = 100; // Chiều cao ban đầu
                textBoxRect = new Rectangle(e.X, e.Y, rectWidth, rectHeight);

                // Bắt đầu nhập văn bản
                isTyping = true;
                canvas.Invalidate();
            }
        }
        private void btnBucket_Click(object sender, EventArgs e)
        {
            index = 3;
            if (sender is Button btn && btn.Image != null)
            {
                Bitmap bitmap = new Bitmap(Properties.Resources.bucket_drawing1);

                // Tạo con trỏ chuột từ hình ảnh
                Cursor customCursor = CustomCursor.CreateCursorFromBitmap(bitmap, 0, 32);

                // Đặt con trỏ chuột thành con trỏ tùy chỉnh
                this.Cursor = customCursor;
            }
            fontDialog.Hide(); // Ẩn form phụ
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
            index = 7;
            currentMode= SelectionMode.Rectangle;
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            btnSelection.Image = Properties.Resources.freeform1;
            index = 8;
            currentMode = SelectionMode.FreeForm;
        }

        private void canvas_MouseEnter(object sender, EventArgs e)
        {
            Cursor customCursor = Cursors.Default; // Mặc định là con trỏ bình thường

            if (index == 1)
            {
                Bitmap iconBitmap = new Bitmap(Properties.Resources.pencil_drawing2);
                customCursor = CustomCursor.CreateCursorFromBitmap(iconBitmap, 0, 32); // Tạo con trỏ từ icon
            }
            else if (index == 2)
            {
                Bitmap iconBitmap = ResizeIcon(Properties.Resources.square_drawing11, brushsize, brushsize);
                customCursor = CustomCursor.CreateCursorFromBitmap(iconBitmap, iconBitmap.Size.Width/2, iconBitmap.Size.Height/2); // Tạo con trỏ từ icon
            }
            else if (index == 3)
            {
                Bitmap iconBitmap = new Bitmap(Properties.Resources.bucket_drawing1);
                customCursor = CustomCursor.CreateCursorFromBitmap(iconBitmap, 0, 30); // Tạo con trỏ từ icon
            }
            else if (index == 4)
            {
                Bitmap iconBitmap = new Bitmap(Properties.Resources.eyedropper_drawing1);
                customCursor = CustomCursor.CreateCursorFromBitmap(iconBitmap, 0, 30); // Tạo con trỏ từ icon
            }
            else if (index == 5)
            {
                Bitmap iconBitmap = new Bitmap(Properties.Resources.manfier_drawing1);
                customCursor = CustomCursor.CreateCursorFromBitmap(iconBitmap, 0, 0); // Tạo con trỏ từ icon
            }
            this.Cursor = customCursor; // Đặt con trỏ tùy chỉnh
        }

        private void btnEyedropper_Click(object sender, EventArgs e)
        {
            index = 4;
            if (sender is Button btn && btn.Image != null)
            {
                Bitmap bitmap = new Bitmap(Properties.Resources.eyedropper_drawing1);

                // Tạo con trỏ chuột từ hình ảnh
                Cursor customCursor = CustomCursor.CreateCursorFromBitmap(bitmap, 0, 30);

                // Đặt con trỏ chuột thành con trỏ tùy chỉnh
                this.Cursor = customCursor;
            }
            fontDialog.Hide(); // Ẩn form phụ
        }

        private void btnMagnifier_Click(object sender, EventArgs e)
        {
            index = 5;
            if (sender is Button btn && btn.Image != null)
            {
                Bitmap bitmap = new Bitmap(Properties.Resources.manfier_drawing1);

                // Tạo con trỏ chuột từ hình ảnh
                Cursor customCursor = CustomCursor.CreateCursorFromBitmap(bitmap, 0, 30);

                // Đặt con trỏ chuột thành con trỏ tùy chỉnh
                this.Cursor = customCursor;
            }
            fontDialog.Hide(); // Ẩn form phụ
        }
        private void UpdateCanvasZoom()
        {
            // Tính lại kích thước của PictureBox
            canvas.Width = (int)(bm.Width * zoomFactor);
            canvas.Height = (int)(bm.Height * zoomFactor);
            canvas.Invalidate(); // Vẽ lại hình ảnh
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            if (fontDialog != null && !fontDialog.IsDisposed)
            {
                Point btnPosition = btnText.PointToScreen(Point.Empty);
                fontDialog.Location = new Point(btnPosition.X-30, btnPosition.Y + 80);
            }
            UpdateCanvasSize();

        }
        private void UpdateCanvasSize()
        {
            if (canvas.Width <= 0 || canvas.Height <= 0)
                return;

            // Tạo Bitmap mới với kích thước mới của PictureBox
            Bitmap newBitmap = new Bitmap(canvas.Width, canvas.Height);
            Graphics newGraphics = Graphics.FromImage(newBitmap);
            newGraphics.Clear(Color.White);

            // Sao chép nội dung của Bitmap cũ vào Bitmap mới
            newGraphics.DrawImage(bm, 0, 0);

            // Cập nhật lại Bitmap và Graphics
            bm = newBitmap;
            g = Graphics.FromImage(bm);
            canvas.Image = bm;
        }

        private void SaveState()
        {
            redoStack.Clear();

            Bitmap stateCopy = new Bitmap(bm);
            undoStack.Push(stateCopy);
        }
        private void Undo()
        {
            if (undoStack.Count > 0)
            {
                redoStack.Push(new Bitmap(bm));
                bm = undoStack.Pop();
                g=Graphics.FromImage(bm);
                canvas.Image = bm;
                canvas.Refresh();
            }
        }

        private void btnUndo_Click(object sender, EventArgs e)
        {
            Undo();
        }
        private void Redo()
        {
            if (redoStack.Count > 0)
            {
                undoStack.Push(new Bitmap(bm));

                // Khôi phục trạng thái từ redoStack
                bm = redoStack.Pop();
                g = Graphics.FromImage(bm);
                canvas.Image = bm;
                canvas.Refresh();
            }
        }
        private void btnRedo_Click(object sender, EventArgs e)
        {
            Redo();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Control && e.KeyCode == Keys.Z)
            {
                Undo();
                e.Handled = true;
            }
            if (e.Control && e.KeyCode == Keys.Y)
            {
                Redo();
                e.Handled = true;
            }
            bool isCapsLock = Control.IsKeyLocked(Keys.CapsLock);

            if (!isTyping) return;
            if (e.KeyCode == Keys.Space)
            {
                // Ngăn không cho phím Space hoạt động trên nút
                e.SuppressKeyPress = true;
            }
            if (e.KeyCode == Keys.Back && inputText.Length > 0)
            {
                inputText = inputText.Substring(0, inputText.Length - 1);
            }
            else if (e.KeyCode == Keys.Enter)
            {
                // Lưu vùng chữ nhật và văn bản vào danh sách
                if (!string.IsNullOrEmpty(inputText))
                {
                    using (Graphics graphics = Graphics.FromImage(bm))
                    {
                        DrawTextInRectangle(graphics, inputText, textBoxRect, textFont, textBrush);
                    }

                    // Reset trạng thái nhập
                    isTyping = false;
                    inputText = "";
                    canvas.Invalidate(); // Vẽ lại canvas với nội dung đã được lưu vào Bitmap
                }
            }
            else if (e.KeyCode == Keys.Escape)
            {
                inputText = ""; // Hủy nhập
                isTyping = false;
            }
            else
            {
                bool isShiftPressed = e.Shift;

                // Chuyển đổi phím KeyCode thành ký tự
                char inputChar = ConvertKeyToChar(e.KeyCode, isShiftPressed);

                // Áp dụng quy tắc: Khi Shift + Caps Lock được nhấn thì không viết hoa
                if (isCapsLock && !isShiftPressed)
                {
                    inputChar = char.ToUpper(inputChar);
                }
                else if (isShiftPressed && !isCapsLock)
                {
                    inputChar = char.ToUpper(inputChar);
                }
                else
                {
                    inputChar = char.ToLower(inputChar);
                }

                if (inputChar != '\0') inputText += inputChar;
            }

            canvas.Invalidate();
        }
        private char ConvertKeyToChar(Keys key, bool isShiftPressed)
        {
            if (key >= Keys.A && key <= Keys.Z)
            {
                return isShiftPressed ? key.ToString()[0] : char.ToLower(key.ToString()[0]);
            }
            if (key >= Keys.D0 && key <= Keys.D9)
            {
                if (isShiftPressed)
                {
                    string shiftNumbers = ")!@#$%^&*("; // Ký tự khi Shift + số
                    return shiftNumbers[key - Keys.D0];
                }
                else
                    return (char)(key - Keys.D0 + '0');
            }

            // Phím đặc biệt
            if (key == Keys.Space)
                return ' ';
            if (key == Keys.OemMinus)
                return isShiftPressed ? '_' : '-';
            if (key == Keys.Oemplus)
                return isShiftPressed ? '+' : '=';
            if (key == Keys.OemOpenBrackets)
                return isShiftPressed ? '{' : '[';
            if (key == Keys.OemCloseBrackets)
                return isShiftPressed ? '}' : ']';
            if (key == Keys.OemPipe)
                return isShiftPressed ? '|' : '\\';
            if (key == Keys.OemSemicolon)
                return isShiftPressed ? ':' : ';';
            if (key == Keys.OemQuotes)
                return isShiftPressed ? '"' : '\'';
            if (key == Keys.Oemcomma)
                return isShiftPressed ? '<' : ',';
            if (key == Keys.OemPeriod)
                return isShiftPressed ? '>' : '.';
            if (key == Keys.OemQuestion)
                return isShiftPressed ? '?' : '/';
            if (key == Keys.Oemtilde)
                return isShiftPressed ? '~' : '`';

            return '\0'; // Không hợp lệ
        }
        
        private void btnText_Click(object sender, EventArgs e)
        {
            index = 6;
            if (string.IsNullOrEmpty(fontDialog.SelectedFont))
            {
                fontDialog.SelectedFont = "Arial";
            }
            if (string.IsNullOrEmpty(fontDialog.SelectedFontSize))
            {
                fontDialog.SelectedFontSize = "14";
            }
            if (fontDialog == null || fontDialog.IsDisposed) // Kiểm tra fontDialog đã bị dispose chưa
            {
                fontDialog = new Form3(); // Tạo lại form phụ
            }
            fontDialog.StartPosition = FormStartPosition.Manual;
            Point btnPosition = btnText.PointToScreen(Point.Empty);
            fontDialog.Location = new Point(btnPosition.X-30, btnPosition.Y + 80);
            fontDialog.Show();

            FontStyle style = FontStyle.Regular;
            if (fontDialog.IsBold)
                style |= FontStyle.Bold;
            if (fontDialog.IsItalic)
                style |= FontStyle.Italic;
            if (fontDialog.IsUnderline)
                style |= FontStyle.Underline;
            Color txtColor = pic_ColorStroke.BackColor;
            textBrush = new SolidBrush(txtColor);
            textFont = new Font(fontDialog.GetFontFamily(), fontDialog.GetBrushSize(), style);
            currentAlignment = fontDialog.TextAlign;

            this.Invalidate(); // Gọi sự kiện vẽ lại

        }
        // Phương thức vẽ hình chữ nhật nét đứt và văn bản
        private void DrawTextInRectangle(Graphics g, string text, Rectangle rect, Font font, Brush brush)
        {
            // Đo chiều rộng của từng dòng và xuống dòng khi cần
            List<string> lines = new List<string>();
            string currentLine = "";
            int maxWidth = rect.Width - 2 * padding;

            foreach (char c in text)
            {
                string tempLine = currentLine + c;

                // Đo chiều rộng của dòng tạm
                SizeF textSize = g.MeasureString(tempLine, font);

                if (textSize.Width > maxWidth || c == '\n') // Thêm dòng mới khi gặp ký tự xuống dòng
                {
                    // Nếu vượt quá chiều rộng hoặc gặp ký tự xuống dòng
                    lines.Add(currentLine.TrimEnd());
                    currentLine = c == '\n' ? "" : c.ToString(); // Bắt đầu dòng mới
                }
                else
                {
                    currentLine = tempLine;
                }
            }

            if (!string.IsNullOrEmpty(currentLine))
            {
                lines.Add(currentLine.TrimEnd());
            }
            StringFormat stringFormat = new StringFormat();
            switch (currentAlignment)
            {
                case "Left":
                    stringFormat.Alignment = StringAlignment.Near; // Căn trái
                    break;
                case "Center":
                    stringFormat.Alignment = StringAlignment.Center; // Căn giữa
                    break;
                case "Right":
                    stringFormat.Alignment = StringAlignment.Far; // Căn phải
                    break;
                default:
                    stringFormat.Alignment = StringAlignment.Near; // Mặc định căn trái
                    break;
            }

            // Cấu hình StringFormat
            float y = rect.Y + padding; // Vị trí Y ban đầu
            foreach (string line in lines)
            {
                if (y + font.Height > rect.Bottom) break; // Nếu vượt quá chiều cao, dừng vẽ

                // Xác định hình chữ nhật cho từng dòng
                RectangleF lineRect = new RectangleF(rect.X + padding, y, rect.Width - 2 * padding, font.Height);

                // Vẽ dòng hiện tại với căn chỉnh
                g.DrawString(line, font, brush, lineRect, stringFormat);

                // Chuyển xuống dòng tiếp theo
                y += font.Height;
            }
        }


        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(bm, 0, 0);
            // Vẽ tất cả các văn bản từ danh sách
            foreach (var textBox in textBoxes)
            {
                DrawTextInRectangle(e.Graphics, textBox.Item2, textBox.Item1, textFont, textBrush);
            }

            // Vẽ hình chữ nhật và văn bản đang nhập (nếu có)
            if (isTyping)
            {
                using (Pen rectPen = new Pen(Color.Black, 1))
                {
                    e.Graphics.DrawRectangle(rectPen, textBoxRect);
                }

                DrawTextInRectangle(e.Graphics, inputText, textBoxRect, textFont, textBrush);
            }

            if (currentMode == SelectionMode.Rectangle && isSelecting)
            {
                // Vẽ vùng chọn hình chữ nhật
                using (Pen dashedPen = new Pen(Color.Black))
                {
                    dashedPen.DashStyle = DashStyle.Dash;
                    e.Graphics.DrawRectangle(dashedPen, selectionRectangle);
                }
            }
            if (currentMode == SelectionMode.FreeForm)
            {
                if (isSelecting && freeFormPoints.Count > 1)
                {
                    // Vẽ đường tự do khi đang vẽ
                    using (Pen dashedPen = new Pen(Color.Black))
                    {
                        dashedPen.DashStyle = DashStyle.Dash;
                        e.Graphics.DrawLines(dashedPen, freeFormPoints.ToArray());
                    }
                }
                if (freeFormRegion != null)
                {
                    using (Pen solidPen = new Pen(Color.Black))
                    {
                        solidPen.DashStyle = DashStyle.Solid;
                        GraphicsPath path = new GraphicsPath();
                        path.AddPolygon(freeFormPoints.ToArray());
                        e.Graphics.DrawPath(solidPen, path);
                    }
                }
            }
        }

        private void btnSelection_Click(object sender, EventArgs e)
        {

        }

        private void Fill(Bitmap bm, int x, int y, Color new_clr)
        {
            Color old_clr=bm.GetPixel(x, y);
            if (old_clr == new_clr)
            {
                return;
            }
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
