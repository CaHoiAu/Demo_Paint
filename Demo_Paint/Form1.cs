using Demo_Paint.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
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
        //Các biến làm việc vs File
        private string currentFilePath = string.Empty;
        private bool hasUnsavedChanges = false;
        //Các biến cho Shape
        public enum ShapeType
        {
            Line,
            Rectangle,
            Ellipse,
            Curve,
            Triangle,
            Star,
            Pentagon,
            Diamond,
            Heart,
            FourPointStar
        }

        public class Shape
        {
            public Shape()
            {
                // Initialize all points to avoid null reference
                StartPoint = new Point(0, 0);
                EndPoint = new Point(0, 0);
                p3 = new Point(0, 0);
                p4 = new Point(0, 0);
            }
            public Point StartPoint { get; set; }
            public Point EndPoint { get; set; }
            public Point p3 { get; set; }
            public Point p4 { get; set; }
            public ShapeType Type { get; set; }
            public Color Color { get; set; }
            public float Size { get; set; }
            public List<Point> CurvePoints { get; set; }
        }

        private int countBezier = 0;
        private List<Shape> shapes = new List<Shape>();
        private bool isStart = false;
        private Shape currentShape;
        private ShapeType currentShapeType = ShapeType.Line; // Default shape type

        // Các biến cho layers
        public class Layer
        {
            public Bitmap Bitmap { get; set; }
            public string Name { get; set; }
            public bool Visible { get; set; }
            public float Opacity { get; set; }
            public List<TextObject> TextObjects { get; set; }
            private bool[,] pixelMask;

            public Layer(int width, int height, string name)
            {
                Bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
                pixelMask = new bool[width, height];
                using (Graphics g = Graphics.FromImage(Bitmap))
                {
                    g.Clear(Color.FromArgb(0, 0, 0, 0));  // Completely transparent
                }
                Name = name;
                Visible = true;
                Opacity = 1.0f;
                TextObjects = new List<TextObject>();
            }
            public void DeleteRegion(Rectangle region)
            {
                using (Graphics g = Graphics.FromImage(Bitmap))
                {
                    g.CompositingMode = CompositingMode.SourceCopy;
                    using (SolidBrush clearBrush = new SolidBrush(Color.FromArgb(0, 0, 0, 0)))
                    {
                        g.FillRectangle(clearBrush, region);
                    }
                    g.CompositingMode = CompositingMode.SourceOver;
                }
            }

            public void DeleteRegion(GraphicsPath path)
            {
                using (Graphics g = Graphics.FromImage(Bitmap))
                {
                    g.CompositingMode = CompositingMode.SourceCopy;
                    using (SolidBrush clearBrush = new SolidBrush(Color.FromArgb(0, 0, 0, 0)))
                    {
                        g.FillPath(clearBrush, path);
                    }
                    g.CompositingMode = CompositingMode.SourceOver;
                }
            }
            public bool HasPixelAt(int x, int y)
            {
                if (x >= 0 && x < Bitmap.Width && y >= 0 && y < Bitmap.Height)
                {
                    return pixelMask[x, y];
                }
                return false;
            }

            public void SetPixel(int x, int y, Color color)
            {
                if (x >= 0 && x < Bitmap.Width && y >= 0 && y < Bitmap.Height)
                {
                    pixelMask[x, y] = true;
                    Bitmap.SetPixel(x, y, color);
                }
            }
        }
        private List<Layer> layers;
        private int currentLayerIndex;
        private Layer CurrentLayer
        {
            get
            {
                // Add null check and bounds check
                if (layers == null || layers.Count == 0)
                {
                    // Create a default layer if none exists
                    Layer newLayer = new Layer(canvas.Width, canvas.Height, "Layer 1");
                    layers = new List<Layer> { newLayer };
                    currentLayerIndex = 0;
                    UpdateLayerListUI();
                    return newLayer;
                }

                // Ensure currentLayerIndex is valid
                if (currentLayerIndex < 0 || currentLayerIndex >= layers.Count)
                {
                    currentLayerIndex = layers.Count - 1;  // Set to last layer
                }

                return layers[currentLayerIndex];
            }
        }
        Bitmap bm;
        Graphics g;
        bool paint = false;
        Point px, py;
        int brushsize = 1;
        int index = 0;

        private Point GetImagePoint(Point screenPoint)
        {
            return new Point(
                (int)(screenPoint.X / zoomFactor),
                (int)(screenPoint.Y / zoomFactor)
            );
        }
        private Point GetScreenPoint(Point imagePoint)
        {
            return new Point(
                (int)(imagePoint.X * zoomFactor),
                (int)(imagePoint.Y * zoomFactor)
            );
        }
        float zoomFactor = 1.0f; // Tỷ lệ phóng to/thu nhỏ (1.0 = 100%)

        private Rectangle GetScaledRectangle(Rectangle rect)
        {
            return new Rectangle(
                (int)(rect.X * zoomFactor),
                (int)(rect.Y * zoomFactor),
                (int)(rect.Width * zoomFactor),
                (int)(rect.Height * zoomFactor)
            );
        }


        private List<DrawLine> lines = new List<DrawLine>();
        //các biến undo, redo
        private Stack<SavedState> undoStack = new Stack<SavedState>();
        private Stack<SavedState> redoStack = new Stack<SavedState>();
        private int currentIndex = -1;
        //các biến text
        private Rectangle textBoxRect; // Hình chữ nhật chứa văn bản
        private string inputText = ""; // Văn bản nhập
        private Font textFont = new Font("Arial", 14); // Font chữ
        private Brush textBrush = Brushes.Black; // Màu chữ
        private bool isTyping = false; // Cờ nhập văn bản
        private int padding = 5;
        private List<Tuple<Rectangle, string>> textBoxes = new List<Tuple<Rectangle, string>>();
        private string currentAlignment;
        private List<TextObject> textObjects = new List<TextObject>(); // Add this field

        // Add this class to store text information
        public class TextObject
        {
            public string Text { get; set; }
            public Rectangle Rect { get; set; }
            public Font Font { get; set; }
            public Color Color { get; set; }
            public string Alignment { get; set; }
        }
        //Các biến cho Selection
        private enum SelectionMode { Rectangle, FreeForm }
        private SelectionMode currentMode = SelectionMode.Rectangle; // Mặc định chọn hình chữ nhật

        private bool isSelecting = false; // Trạng thái vẽ vùng chọn
        private Point startPoint; // Điểm bắt đầu vùng chọn
        private Rectangle selectionRectangle; // Hình chữ nhật vùng chọn

        private List<Point> freeFormPoints; // Danh sách điểm cho hình tự do
        private Region freeFormRegion; // Vùng chọn hình tự do
        private bool isDrawingSelection = false;  // Add this field to track if we're currently drawing a selection
        private Rectangle tempSelectionRect;
        private bool selectionToolActive = false;
        //Thao tác vs vùng selection
        private Bitmap copiedRegionBitmap = null; // To store the copied region bitmap
        private Point pastePosition; // To store the paste position
        private bool isMoving = false; // Flag to indicate if a region is being moved
        private Rectangle currentSelectionRectangle; // For drawing the current selection rectangle


        //Form cho font
        Form3 fontDialog = new Form3();

        public Form1()
        {
            this.KeyPreview = true;
            InitializeComponent();

            // Initialize canvas
            if (this.canvas == null)
            {
                this.canvas = new PictureBox();
                this.canvas.BackColor = Color.White;
                this.canvas.Size = new Size(1024, 800);
                this.canvas.Location = new Point(0, 0);
            }
            this.panelCanvas.Controls.Add(this.canvas);

            // Initialize bitmap
            bm = new Bitmap(canvas.Width, canvas.Height);
            g = Graphics.FromImage(bm);
            g.Clear(Color.White);
            canvas.Image = bm;

            // Initialize layers
            layers = new List<Layer>();

            Layer backgroundLayer = new Layer(canvas.Width, canvas.Height, "Background");
            using (Graphics g = Graphics.FromImage(backgroundLayer.Bitmap))
            {
                g.Clear(Color.White);  // Make this layer white
            }
            layers.Add(backgroundLayer);

            // Create and add the first drawing layer (transparent)
            Layer layer1 = new Layer(canvas.Width, canvas.Height, "Layer 1");
            layers.Add(layer1);

            currentLayerIndex = 1;  // Set current layer to Layer 1

            UpdateLayerListUI();
            CenterCanvas();
            fontDialog.FormBorderStyle = FormBorderStyle.None;
            fontDialog.Owner = this;

            freeFormPoints = new List<Point>(); // Khởi tạo danh sách
        }
        private void InitializeLayers()
        {
            if (layers == null)
                layers = new List<Layer>();

            // Create the base layer
            Layer baseLayer = new Layer(canvas.Width, canvas.Height, "Layer 1");

            // Copy the current bitmap to the base layer if it exists
            if (bm != null)
            {
                using (Graphics g = Graphics.FromImage(baseLayer.Bitmap))
                {
                    g.DrawImage(bm, 0, 0);
                }
            }

            // Add the base layer
            layers.Add(baseLayer);
            currentLayerIndex = 0;

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
            UpdateCanvasSize();
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
        private void ClearOriginalArea()
        {
            using (Graphics g = Graphics.FromImage(CurrentLayer.Bitmap))
            {
                g.CompositingMode = CompositingMode.SourceCopy;
                using (SolidBrush clearBrush = new SolidBrush(Color.FromArgb(0, 0, 0, 0)))
                {
                    if (currentMode == SelectionMode.Rectangle)
                    {
                        g.FillRectangle(clearBrush, selectionRectangle);
                    }
                    else if (currentMode == SelectionMode.FreeForm && freeFormPoints.Count > 2)
                    {
                        GraphicsPath path = new GraphicsPath();
                        path.AddPolygon(freeFormPoints.ToArray());
                        g.FillPath(clearBrush, path);
                    }
                }
                g.CompositingMode = CompositingMode.SourceOver;
            }
        }
        private void canvas_MouseDown(object sender, MouseEventArgs e)
        {
            paint = true;
            Point imagePoint = GetImagePoint(e.Location);
            py = imagePoint;
            if (index != 6 && textObjects.Count > 0)
            {
                CommitAllTexts();
            }

            // Only save state if we're not using selection tool
            if (index != 5 && !selectionToolActive) // Modified condition
            {
                SaveState();
            }

            if (index == 1 || index == 2)
            {
                SaveDrawingState();
                px = e.Location;
            }
            isStart = true;
            if (index == 9)  // Shape tool
            {
                if (currentShapeType == ShapeType.Curve)
                {
                    if (countBezier == 0)
                    {
                        Shape newShape = new Shape
                        {
                            StartPoint = imagePoint,
                            EndPoint = imagePoint,
                            p3 = imagePoint,      // Note: Changed from p3 to P3
                            p4 = imagePoint,      // Note: Changed from p4 to P4
                            Type = ShapeType.Curve,
                            Color = pic_ColorStroke.BackColor,
                            Size = brushsize
                        };
                        shapes.Add(newShape);
                    }
                    // Remove the shape removal code here since we want to keep modifying the same shape
                }
                else
                {
                    // Non-curve shapes
                    Shape newShape = new Shape
                    {
                        StartPoint = imagePoint,
                        EndPoint = imagePoint,
                        Type = currentShapeType,
                        Color = pic_ColorStroke.BackColor,
                        Size = brushsize
                    };
                    shapes.Add(newShape);
                }
            }
            if (selectionToolActive)
            {
                if (!isSelectionInRegion(e.Location))
                {
                    // If there was a previous selection, finalize it and save state once
                    if (!selectionRectangle.IsEmpty && copiedRegionBitmap != null)
                    {
                        SaveState(); // Save state before committing the selection
                        ClearOriginalArea();
                        PasteCopiedRegion(new Point(selectionRectangle.X, selectionRectangle.Y));
                        copiedRegionBitmap = null;
                        selectionRectangle = Rectangle.Empty;
                        freeFormPoints.Clear();
                    }

                    // Start new selection
                    isSelecting = true;
                    startPoint = e.Location;
                    if (currentMode == SelectionMode.Rectangle)
                    {
                        selectionRectangle = new Rectangle(e.Location, new Size(0, 0));
                    }
                    else if (currentMode == SelectionMode.FreeForm)
                    {
                        freeFormPoints = new List<Point> { e.Location };
                        isShapeClosed = false;
                    }
                }
                else
                {
                    isMoving = true;
                    pastePosition = e.Location;
                    if (copiedRegionBitmap == null)
                    {
                        SaveState(); // Save state before moving selection
                        CopySelectedRegion();
                        ClearOriginalArea();
                    }
                }
                canvas.Invalidate();
            }
        }
        private bool isSelectionInRegion(Point clickLocation)
        {
            if (currentMode == SelectionMode.Rectangle)
            {
                return selectionRectangle.Contains(clickLocation);
            }
            else if (currentMode == SelectionMode.FreeForm && freeFormPoints.Count > 2)
            {
                GraphicsPath path = new GraphicsPath();
                path.AddPolygon(freeFormPoints.ToArray());
                return path.IsVisible(clickLocation);
            }
            return false;
        }
        private void ResetFreeFormSelection()
        {
            freeFormPoints.Clear();
            freeFormRegion.Dispose();
            freeFormRegion = null;
            isSelecting = false;
            canvas.Invalidate();
        }
        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            // Update mouse coordinates in the status bar
            toolStripStatusLabel2.Text = $"{e.X}, {e.Y}px";
            if (isStart && index == 9 && shapes.Count > 0)
            {
                try
                {
                    Point imagePoint = GetImagePoint(e.Location);
                    Shape lastShape = shapes[shapes.Count - 1];

                    if (currentShapeType == ShapeType.Curve)
                    {
                        // Handle curve drawing
                        switch (countBezier)
                        {
                            case 0:
                                lastShape.EndPoint = imagePoint;
                                lastShape.p3 = imagePoint;
                                lastShape.p4 = imagePoint;
                                break;
                            case 1:
                                lastShape.p3 = imagePoint;
                                lastShape.EndPoint = imagePoint;
                                break;
                            case 2:
                                lastShape.EndPoint = imagePoint;
                                break;
                        }
                    }
                    else
                    {
                        // Handle other shapes
                        lastShape.EndPoint = imagePoint;
                    }
                    canvas.Invalidate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in MouseMove: {ex.Message}");
                }
            }
            // Handle drawing tools (pen or eraser)
            if (paint && (index == 1 || index == 2))
            {
                // Get graphics context for current layer instead of using global 'g'
                using (Graphics currentG = Graphics.FromImage(CurrentLayer.Bitmap))
                {
                    // Skip selection logic if drawing
                    if (index == 1) // Pen tool
                    {
                        Pen p = new Pen(pic_ColorStroke.BackColor, brushsize)
                        {
                            StartCap = LineCap.Round,
                            EndCap = LineCap.Round,
                            LineJoin = LineJoin.Round
                        };

                        Point imagePoint1 = GetImagePoint(px);
                        Point imagePoint2 = GetImagePoint(e.Location);
                        currentG.DrawLine(p, imagePoint1.X, imagePoint1.Y, imagePoint2.X, imagePoint2.Y);

                        // Store current position for next line segment
                        px = e.Location;
                        py = e.Location;
                    }
                    else if (index == 2) // Eraser tool
                    {
                        currentG.CompositingMode = CompositingMode.SourceCopy;  // Add this line
                        Pen eraser = new Pen(Color.FromArgb(0, 0, 0, 0), brushsize)
                        {
                            StartCap = LineCap.Round,
                            EndCap = LineCap.Round,
                            LineJoin = LineJoin.Round
                        };

                        Point imagePoint1 = GetImagePoint(px);
                        Point imagePoint2 = GetImagePoint(e.Location);
                        currentG.DrawLine(eraser, imagePoint1.X, imagePoint1.Y, imagePoint2.X, imagePoint2.Y);
                        currentG.CompositingMode = CompositingMode.SourceOver;
                        // Store current position for next line segment
                        px = e.Location;
                        py = e.Location;
                    }
                }

                canvas.Invalidate(); // Redraw the canvas
                return; // Stop here to avoid interfering with selection logic
            }
            if (selectionToolActive)
            {
                if (isMoving)
                {
                    int dx = e.X - pastePosition.X;
                    int dy = e.Y - pastePosition.Y;

                    // Move both the selection rectangle and freeform points
                    selectionRectangle.Offset(dx, dy);

                    // Move all freeform points
                    if (currentMode == SelectionMode.FreeForm)
                    {
                        for (int i = 0; i < freeFormPoints.Count; i++)
                        {
                            freeFormPoints[i] = new Point(
                                freeFormPoints[i].X + dx,
                                freeFormPoints[i].Y + dy
                            );
                        }
                    }

                    pastePosition = e.Location;
                    canvas.Invalidate();
                }
                else if (isSelecting)
                {
                    if (currentMode == SelectionMode.Rectangle)
                    {
                        Point endPoint = e.Location;
                        selectionRectangle = new Rectangle(
                            Math.Min(startPoint.X, endPoint.X),
                            Math.Min(startPoint.Y, endPoint.Y),
                            Math.Abs(startPoint.X - endPoint.X),
                            Math.Abs(startPoint.Y - endPoint.Y)
                        );
                    }
                    else if (currentMode == SelectionMode.FreeForm && !isShapeClosed)
                    {
                        freeFormPoints.Add(e.Location);
                    }
                    canvas.Invalidate();
                }
            }
        }
        private void TestBitmapTransparency(Bitmap bmp)
        {
            // Check a few pixels
            Color pixel = bmp.GetPixel(0, 0);
            MessageBox.Show($"Alpha: {pixel.A}, R: {pixel.R}, G: {pixel.G}, B: {pixel.B}");
        }
        private void CopySelectedRegion()
        {
            if (currentMode == SelectionMode.Rectangle && !selectionRectangle.IsEmpty)
            {
                copiedRegionBitmap = new Bitmap(selectionRectangle.Width, selectionRectangle.Height, PixelFormat.Format32bppArgb);
                using (Graphics g = Graphics.FromImage(copiedRegionBitmap))
                {
                    g.Clear(Color.FromArgb(0, 0, 0, 0));  // Fully transparent
                    g.CompositingMode = CompositingMode.SourceCopy;

                    // Copy the exact pixels
                    Rectangle destRect = new Rectangle(0, 0, selectionRectangle.Width, selectionRectangle.Height);
                    Rectangle sourceRect = new Rectangle(selectionRectangle.X, selectionRectangle.Y, selectionRectangle.Width, selectionRectangle.Height);

                    using (ImageAttributes ia = new ImageAttributes())
                    {
                        ColorMatrix matrix = new ColorMatrix();
                        matrix.Matrix33 = 1.0f; // preserve alpha
                        ia.SetColorMatrix(matrix, ColorMatrixFlag.Default);

                        g.DrawImage(CurrentLayer.Bitmap, destRect, sourceRect.X, sourceRect.Y,
                            sourceRect.Width, sourceRect.Height, GraphicsUnit.Pixel, ia);
                    }
                }
            }
            else if (currentMode == SelectionMode.FreeForm && freeFormPoints.Count > 2)
            {
                // Similar changes for freeform selection
                copiedRegionBitmap = new Bitmap(selectionRectangle.Width, selectionRectangle.Height, PixelFormat.Format32bppArgb);
                using (Graphics g = Graphics.FromImage(copiedRegionBitmap))
                {
                    g.Clear(Color.FromArgb(0, 0, 0, 0));  // Fully transparent
                    g.CompositingMode = CompositingMode.SourceCopy;

                    using (GraphicsPath path = new GraphicsPath())
                    {
                        Point[] translatedPoints = freeFormPoints
                            .Select(p => new Point(p.X - selectionRectangle.X, p.Y - selectionRectangle.Y))
                            .ToArray();
                        path.AddPolygon(translatedPoints);
                        g.SetClip(path);

                        using (ImageAttributes ia = new ImageAttributes())
                        {
                            ColorMatrix matrix = new ColorMatrix();
                            matrix.Matrix33 = 1.0f; // preserve alpha
                            ia.SetColorMatrix(matrix, ColorMatrixFlag.Default);

                            Rectangle destRect = new Rectangle(0, 0, selectionRectangle.Width, selectionRectangle.Height);
                            Rectangle sourceRect = new Rectangle(selectionRectangle.X, selectionRectangle.Y,
                                selectionRectangle.Width, selectionRectangle.Height);

                            g.DrawImage(CurrentLayer.Bitmap, destRect, sourceRect.X, sourceRect.Y,
                                sourceRect.Width, sourceRect.Height, GraphicsUnit.Pixel, ia);
                        }
                    }
                }
            }
        }
        private void PasteCopiedRegion(Point pastePoint)
        {
            if (copiedRegionBitmap != null)
            {
                Point scaledPastePoint = new Point(
                    (int)(pastePoint.X * zoomFactor),
                    (int)(pastePoint.Y * zoomFactor)
                );

                using (Graphics g = Graphics.FromImage(CurrentLayer.Bitmap))
                {
                    g.CompositingMode = CompositingMode.SourceOver;
                    using (ImageAttributes ia = new ImageAttributes())
                    {
                        ColorMatrix matrix = new ColorMatrix();
                        matrix.Matrix33 = 1.0f; // preserve alpha
                        ia.SetColorMatrix(matrix, ColorMatrixFlag.Default);

                        g.DrawImage(copiedRegionBitmap,
                            new Rectangle(scaledPastePoint.X, scaledPastePoint.Y,
                                copiedRegionBitmap.Width, copiedRegionBitmap.Height),
                            0, 0, copiedRegionBitmap.Width, copiedRegionBitmap.Height,
                            GraphicsUnit.Pixel,
                            ia);
                    }
                }
                canvas.Invalidate();
            }
        }
        private bool isShapeClosed = false;
        private void DrawShapeOnGraphics(Graphics g, Shape shape)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (Pen pen = new Pen(shape.Color, shape.Size))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;

                switch (shape.Type)
                {
                    case ShapeType.Line:
                        g.DrawLine(pen, shape.StartPoint, shape.EndPoint);
                        break;

                    case ShapeType.Rectangle:
                        Rectangle rect = new Rectangle(
                            Math.Min(shape.StartPoint.X, shape.EndPoint.X),
                            Math.Min(shape.StartPoint.Y, shape.EndPoint.Y),
                            Math.Abs(shape.EndPoint.X - shape.StartPoint.X),
                            Math.Abs(shape.EndPoint.Y - shape.StartPoint.Y)
                        );
                        g.DrawRectangle(pen, rect);
                        break;

                    case ShapeType.Ellipse:
                        Rectangle ellipseRect = new Rectangle(
                            Math.Min(shape.StartPoint.X, shape.EndPoint.X),
                            Math.Min(shape.StartPoint.Y, shape.EndPoint.Y),
                            Math.Abs(shape.EndPoint.X - shape.StartPoint.X),
                            Math.Abs(shape.EndPoint.Y - shape.StartPoint.Y)
                        );
                        g.DrawEllipse(pen, ellipseRect);
                        break;
                    case ShapeType.Triangle:
                        Point[] trianglePoints = new Point[3];
                        trianglePoints[0] = new Point((shape.StartPoint.X + shape.EndPoint.X) / 2, shape.StartPoint.Y);
                        trianglePoints[1] = new Point(shape.StartPoint.X, shape.EndPoint.Y);
                        trianglePoints[2] = new Point(shape.EndPoint.X, shape.EndPoint.Y);
                        g.DrawPolygon(pen, trianglePoints);
                        break;
                    case ShapeType.Pentagon:
                        Point[] pentagonPoints= new Point[6];
                        pentagonPoints[0] = new Point((shape.StartPoint.X + shape.EndPoint.X) / 2, shape.StartPoint.Y);
                        pentagonPoints[1] = new Point(shape.StartPoint.X, shape.StartPoint.Y + (shape.EndPoint.Y - shape.StartPoint.Y) / 4);
                        pentagonPoints[2] = new Point(shape.StartPoint.X, shape.StartPoint.Y + (shape.EndPoint.Y - shape.StartPoint.Y) * 3 / 4);
                        pentagonPoints[3] = new Point((shape.StartPoint.X + shape.EndPoint.X) / 2, shape.EndPoint.Y);
                        pentagonPoints[4] = new Point(shape.EndPoint.X, shape.StartPoint.Y + (shape.EndPoint.Y - shape.StartPoint.Y) * 3 / 4);
                        pentagonPoints[5] = new Point(shape.EndPoint.X, shape.StartPoint.Y + (shape.EndPoint.Y - shape.StartPoint.Y) / 4);
                        g.DrawPolygon(pen, pentagonPoints);
                        break;
                    case ShapeType.Star:
                        // Calculate the center and radius
                        int centerX = (shape.StartPoint.X + shape.EndPoint.X) / 2;
                        int centerY = (shape.StartPoint.Y + shape.EndPoint.Y) / 2;
                        double radius = Math.Min(
                            Math.Abs(shape.EndPoint.X - shape.StartPoint.X),
                            Math.Abs(shape.EndPoint.Y - shape.StartPoint.Y)) / 2;
                        double innerRadius = radius * 0.4; // Inner radius is 40% of outer radius

                        // Create points array for the star (10 points for 5 points star)
                        Point[] starPoints = new Point[10];

                        // Calculate the 10 points (5 outer and 5 inner points)
                        for (int i = 0; i < 10; i++)
                        {
                            // Alternate between outer and inner radius
                            double currentRadius = (i % 2 == 0) ? radius : innerRadius;

                            // Calculate angle (36 degrees = 360/10 points)
                            double angle = i * 36 * Math.PI / 180;

                            // Calculate point position
                            starPoints[i] = new Point(
                                (int)(centerX + currentRadius * Math.Sin(angle)),
                                (int)(centerY - currentRadius * Math.Cos(angle))
                            );
                        }

                        // Draw the star
                        g.DrawPolygon(pen, starPoints);
                        break;
                    case ShapeType.Diamond:
                        Point[] diamondPoints = new Point[4];
                        diamondPoints[0] = new Point((shape.StartPoint.X + shape.EndPoint.X) / 2, shape.StartPoint.Y);
                        diamondPoints[1] = new Point(shape.StartPoint.X, shape.StartPoint.Y + (shape.EndPoint.Y - shape.StartPoint.Y) / 2);
                        diamondPoints[2] = new Point((shape.StartPoint.X + shape.EndPoint.X) / 2, shape.EndPoint.Y);
                        diamondPoints[3] = new Point(shape.EndPoint.X, shape.StartPoint.Y + (shape.EndPoint.Y - shape.StartPoint.Y) / 2);
                        g.DrawPolygon(pen, diamondPoints);
                        break;
                    case ShapeType.FourPointStar:
                        int ctX = (shape.StartPoint.X + shape.EndPoint.X) / 2;
                        int ctY = (shape.StartPoint.Y + shape.EndPoint.Y) / 2;
                        double R = Math.Min(
                            Math.Abs(shape.EndPoint.X - shape.StartPoint.X),
                            Math.Abs(shape.EndPoint.Y - shape.StartPoint.Y)) / 2;
                        double innerR = R * 0.4; // Inner radius is 40% of outer radius

                        Point[] fourStarPoints = new Point[8];

                        for (int i = 0; i < 8; i++)
                        {
                            double currentRadius = (i % 2 == 0) ? R : innerR;

                            double angle = i * 45 * Math.PI / 180;

                            fourStarPoints[i] = new Point(
                                (int)(ctX + currentRadius * Math.Sin(angle)),
                                (int)(ctY - currentRadius * Math.Cos(angle))
                            );
                        }

                        g.DrawPolygon(pen, fourStarPoints);
                        break;
                    case ShapeType.Heart:
                        int width = Math.Abs(shape.EndPoint.X - shape.StartPoint.X);
                        int height = Math.Abs(shape.EndPoint.Y - shape.StartPoint.Y);
                        int x = Math.Min(shape.StartPoint.X, shape.EndPoint.X);
                        int y = Math.Min(shape.StartPoint.Y, shape.EndPoint.Y);

                        // Create path for the heart
                        using (GraphicsPath path = new GraphicsPath())
                        {
                            // Move to top center
                            int topCenterX = x + width / 2;

                            // Create the left curve
                            path.AddBezier(
                                topCenterX, y + height / 3,    // Start point
                                x, y,                          // Control point 1
                                x, y + height * 8/10,            // Control point 2
                                topCenterX, y + height       // End point
                            );

                            // Create the right curve
                            path.AddBezier(
                                topCenterX, y + height,        // Start point
                                x + width, y + height * 8/10,     // Control point 1
                                x + width, y,                  // Control point 2
                                topCenterX, y + height / 3     // End point
                            );

                            // Draw the heart
                            g.DrawPath(pen, path);
                        }
                        break;
                    case ShapeType.Curve:
                        g.DrawBezier(pen,
                            shape.StartPoint,  // Start point
                            shape.p3,         // Control point 1
                            shape.p4,         // Control point 2
                            shape.EndPoint    // End point
                        );
                        break;
                }
            }
        }
        private void canvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (index == 9)
            {
                if (currentShapeType == ShapeType.Curve)
                {
                    countBezier++;
                    if (countBezier == 3)
                    {
                        countBezier = 0;
                        if (isStart)
                        {
                            SaveDrawingState();
                            if (shapes.Count > 0)
                            {
                                Shape finalShape = shapes[shapes.Count - 1];
                                using (Graphics g = Graphics.FromImage(CurrentLayer.Bitmap))
                                {
                                    DrawShapeOnGraphics(g, finalShape);
                                }
                                shapes.Clear();
                            }
                            canvas.Invalidate();
                        }
                        isStart = false;
                    }
                }
                else
                {
                    // Handle other shapes
                    if (isStart)
                    {
                        SaveDrawingState();
                        if (shapes.Count > 0)
                        {
                            Shape finalShape = shapes[shapes.Count - 1];
                            using (Graphics g = Graphics.FromImage(CurrentLayer.Bitmap))
                            {
                                DrawShapeOnGraphics(g, finalShape);
                            }
                            shapes.Clear();
                        }
                        canvas.Invalidate();
                    }
                    isStart = false;
                }
            }
            paint = false;
            if (selectionToolActive)
            {
                if (isSelecting)
                {
                    if (currentMode == SelectionMode.Rectangle)
                    {
                        Point endPoint = e.Location;
                        selectionRectangle = new Rectangle(
                            Math.Min(startPoint.X, endPoint.X),
                            Math.Min(startPoint.Y, endPoint.Y),
                            Math.Abs(startPoint.X - endPoint.X),
                            Math.Abs(startPoint.Y - endPoint.Y)
                        );
                    }
                    else if (currentMode == SelectionMode.FreeForm)
                    {
                        if (!isShapeClosed)
                        {
                            freeFormPoints.Add(e.Location);
                            isShapeClosed = true;
                            // Calculate bounding rectangle for the freeform selection
                            int minX = freeFormPoints.Min(p => p.X);
                            int minY = freeFormPoints.Min(p => p.Y);
                            int maxX = freeFormPoints.Max(p => p.X);
                            int maxY = freeFormPoints.Max(p => p.Y);
                            selectionRectangle = new Rectangle(minX, minY, maxX - minX, maxY - minY);
                        }
                    }
                    isSelecting = false;
                }
                isMoving = false;
                canvas.Invalidate();
            }
        }
        private void canvas_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = "";
            this.Cursor = Cursors.Default;
        }
        private void CommitAllTexts()
        {
            if (textObjects.Count > 0)
            {
                SaveDrawingState();

                // Only commit text to the current layer
                using (Graphics graphics = Graphics.FromImage(CurrentLayer.Bitmap))
                {
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                    graphics.CompositingMode = CompositingMode.SourceOver;

                    foreach (var textObj in textObjects)
                    {
                        using (Brush brush = new SolidBrush(textObj.Color))
                        {
                            DrawTextInRectangle(graphics, textObj.Text, textObj.Rect, textObj.Font, brush);
                        }
                    }
                }

                textObjects.Clear(); // Clear the text objects after committing
                isTyping = false;
                inputText = "";
                canvas.Invalidate();
            }
        }
        private void btnPen_Click(object sender, EventArgs e)
        {
            if (currentIndex == 6) // If switching from text tool
            {
                CommitAllTexts();
            }
            currentIndex = index = 1;
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
            selectionToolActive = false;
            if (!selectionToolActive)
            {
                selectionRectangle = Rectangle.Empty;
                freeFormPoints.Clear();
                canvas.Invalidate();
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
                Cursor customCursor = CustomCursor.CreateCursorFromBitmap(bitmap, bitmap.Size.Width / 2, bitmap.Size.Height / 2);

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
            selectionToolActive = false;
            if (!selectionToolActive)
            {
                selectionRectangle = Rectangle.Empty;
                freeFormPoints.Clear();
                canvas.Invalidate();
            }
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
            Color cx = bm.GetPixel(x, y);
            if (cx == old_clr)
            {
                sp.Push(new Point(x, y));
                bm.SetPixel(x, y, new_clr);
            }
        }
        static Point set_point(PictureBox pb, Point pt)
        {
            float pX = 1f * pb.Image.Width / pb.Width;
            float pY = 1f * pb.Height / pb.Height;
            return new Point((int)(pt.X * pX), (int)(pt.Y * pY));
        }

        private void canvas_MouseClick(object sender, MouseEventArgs e)
        {
            if (index == 3)
            {
                Point imagePoint = GetImagePoint(e.Location); // Convert screen coordinates to image coordinates
                Fill(CurrentLayer.Bitmap, imagePoint.X, imagePoint.Y, pic_ColorFill.BackColor);
                canvas.Invalidate();
            }
            else if (index == 4) // Color picker tool
            {
                Point imagePoint = GetImagePoint(e.Location);

                // Make sure we're within bounds
                if (imagePoint.X >= 0 && imagePoint.X < CurrentLayer.Bitmap.Width &&
                    imagePoint.Y >= 0 && imagePoint.Y < CurrentLayer.Bitmap.Height)
                {
                    // Get color from the current layer
                    Color pickedColor = CurrentLayer.Bitmap.GetPixel(imagePoint.X, imagePoint.Y);

                    // If the picked pixel is transparent, check other visible layers from top to bottom
                    if (pickedColor.A == 0)
                    {
                        for (int i = layers.Count - 1; i >= 0; i--)
                        {
                            if (layers[i].Visible)
                            {
                                Color layerColor = layers[i].Bitmap.GetPixel(imagePoint.X, imagePoint.Y);
                                if (layerColor.A > 0)
                                {
                                    pickedColor = layerColor;
                                    break;
                                }
                            }
                        }
                    }

                    // Update the color picker
                    pic_ColorStroke.BackColor = pickedColor;
                }
            }
            else if (index == 5)
            {
                Point zoomCenter = e.Location;
                if (e.Button == MouseButtons.Left)
                {
                    if (zoomFactor < 5.0f) // Maximum 500% zoom
                    {
                        zoomFactor += 0.25f;
                        UpdateCanvasZoom(zoomCenter);
                    }
                }
                else if (e.Button == MouseButtons.Right)
                {
                    if (zoomFactor > 0.25f) // Minimum 25% zoom
                    {
                        zoomFactor -= 0.25f;
                        UpdateCanvasZoom(zoomCenter);
                    }
                }
            }
            if (index == 6) // Text mode
            {
                Point imagePoint = GetImagePoint(e.Location);

                if (isTyping && !string.IsNullOrEmpty(inputText))
                {
                    // Add the text to textObjects list instead of drawing directly
                    TextObject textObj = new TextObject
                    {
                        Text = inputText,
                        Rect = textBoxRect,
                        Font = textFont,
                        Color = pic_ColorStroke.BackColor,
                        Alignment = currentAlignment
                    };
                    textObjects.Add(textObj);

                    isTyping = false;
                    inputText = "";
                }
                else
                {
                    int rectWidth = (int)(200 / zoomFactor);
                    int rectHeight = (int)(100 / zoomFactor);
                    textBoxRect = new Rectangle(imagePoint.X, imagePoint.Y, rectWidth, rectHeight);
                    isTyping = true;
                    inputText = "";
                }
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
            selectionToolActive = false;
            if (!selectionToolActive)
            {
                selectionRectangle = Rectangle.Empty;
                freeFormPoints.Clear();
                canvas.Invalidate();
            }
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            toolStripbtnFlip.Image = Properties.Resources.flipvertical;
            FlipHorizontal();
        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            toolStripbtnFlip.Image = Properties.Resources.flipvertical1;
            FlipVertical();
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            RotateRight90();
            toolStripbtnRotateRight.Image = Properties.Resources.rotate11;
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            RotateLeft90();
            toolStripbtnRotateRight.Image = Properties.Resources.rotate21;
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            Rotate180();
            toolStripbtnRotateRight.Image = Properties.Resources.rotate180_1;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            btnSelection.Image = Properties.Resources.noun_dotted_rectangle11;
            index = 7;
            currentMode = SelectionMode.Rectangle;
            selectionToolActive = true;

            selectionRectangle = Rectangle.Empty;
            freeFormPoints.Clear();
            isSelecting = false;
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            btnSelection.Image = Properties.Resources.freeform1;
            index = 8;
            currentMode = SelectionMode.FreeForm;
            selectionToolActive = true;

            selectionRectangle = Rectangle.Empty;
            freeFormPoints.Clear();
            isSelecting = false;
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
                customCursor = CustomCursor.CreateCursorFromBitmap(iconBitmap, iconBitmap.Size.Width / 2, iconBitmap.Size.Height / 2); // Tạo con trỏ từ icon
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
            selectionToolActive = false;
            if (!selectionToolActive)
            {
                selectionRectangle = Rectangle.Empty;
                freeFormPoints.Clear();
                canvas.Invalidate();
            }
        }

        private void btnMagnifier_Click(object sender, EventArgs e)
        {
            index = 5;

            selectionRectangle = Rectangle.Empty;
            freeFormPoints.Clear();
            isSelecting = false;
            selectionToolActive = false;

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
        private void UpdateCanvasZoom(Point zoomCenter)
        {
            // If no bitmap exists, return
            if (layers == null || layers.Count == 0) return;

            // Use CurrentLayer property instead of direct bitmap reference
            Layer baseLayer = layers[0];  // Use base layer for size calculations

            // Calculate new dimensions based on the base layer dimensions
            int baseWidth = baseLayer.Bitmap.Width;
            int baseHeight = baseLayer.Bitmap.Height;
            int newWidth = (int)(baseWidth * zoomFactor);
            int newHeight = (int)(baseHeight * zoomFactor);

            // Update canvas size
            canvas.Size = new Size(newWidth, newHeight);

            // Update the main bitmap size if needed
            if (bm == null || bm.Size != baseLayer.Bitmap.Size)
            {
                if (bm != null) bm.Dispose();
                bm = new Bitmap(baseWidth, baseHeight);
                canvas.Image = bm;
            }

            if (panelCanvas != null)
            {
                if (zoomCenter != Point.Empty)
                {
                    // Calculate relative position for zoom center
                    float relativeX = (float)zoomCenter.X / canvas.Width;
                    float relativeY = (float)zoomCenter.Y / canvas.Height;

                    int newX = (int)(newWidth * relativeX - panelCanvas.ClientSize.Width / 2);
                    int newY = (int)(newHeight * relativeY - panelCanvas.ClientSize.Height / 2);

                    // Update scroll position
                    panelCanvas.AutoScrollPosition = new Point(
                        Math.Max(0, Math.Min(newX, panelCanvas.HorizontalScroll.Maximum)),
                        Math.Max(0, Math.Min(newY, panelCanvas.VerticalScroll.Maximum))
                    );
                }

                // Update scroll area
                panelCanvas.AutoScrollMinSize = new Size(
                    Math.Max(newWidth, panelCanvas.ClientSize.Width),
                    Math.Max(newHeight, panelCanvas.ClientSize.Height)
                );
            }

            // Update zoom level display
            toolStripStatusLabel1.Text = $"Zoom: {(int)(zoomFactor * 100)}%";

            // Make sure to redraw all layers
            RedrawLayers();
            canvas.Invalidate();
        }
        private void RedrawLayers()
        {
            if (bm == null || layers == null || layers.Count == 0) return;

            using (Graphics g = Graphics.FromImage(bm))
            {
                g.Clear(Color.Transparent);

                // Draw each visible layer
                foreach (Layer layer in layers)
                {
                    if (layer.Visible)
                    {
                        g.DrawImage(layer.Bitmap, 0, 0, layer.Bitmap.Width, layer.Bitmap.Height);
                    }
                }
            }
        }
        private void CenterCanvas()
        {
            if (panelCanvas != null && canvas != null)
            {
                // Calculate the center position
                int x = (panelCanvas.ClientSize.Width - canvas.Width) / 2;
                int y = (panelCanvas.ClientSize.Height - canvas.Height) / 2;

                // Ensure we don't position the canvas outside the visible area
                x = Math.Max(0, x);
                y = Math.Max(0, y);

                // Set the new location
                canvas.Location = new Point(x, y);

                // Update scroll position
                if (panelCanvas.HorizontalScroll.Visible)
                    panelCanvas.HorizontalScroll.Value = 0;
                if (panelCanvas.VerticalScroll.Visible)
                    panelCanvas.VerticalScroll.Value = 0;
            }
        }
        private Point GetActualPoint(Point mousePoint)
        {
            // Convert mouse coordinates to actual image coordinates
            int x = (int)((mousePoint.X - canvas.Location.X + panelCanvas.HorizontalScroll.Value) / zoomFactor);
            int y = (int)((mousePoint.Y - canvas.Location.Y + panelCanvas.VerticalScroll.Value) / zoomFactor);
            return new Point(x, y);
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            if (fontDialog != null && !fontDialog.IsDisposed)
            {
                Point btnPosition = btnText.PointToScreen(Point.Empty);
                fontDialog.Location = new Point(btnPosition.X - 30, btnPosition.Y + 80);
            }
            this.panelCanvas.Size = new Size(
            this.ClientSize.Width,
            this.ClientSize.Height - label1.Bottom - statusStrip1.Height
            );
            CenterCanvas();
        }
        private void UpdateCanvasSize()
        {
            if (bm == null) return;

            // Calculate scaled dimensions
            int scaledWidth = (int)(bm.Width * zoomFactor);
            int scaledHeight = (int)(bm.Height * zoomFactor);

            // Update canvas size
            canvas.Width = scaledWidth;
            canvas.Height = scaledHeight;

            // Calculate center position
            int x = (panelCanvas.ClientSize.Width - canvas.Width) / 2;
            int y = (panelCanvas.ClientSize.Height - canvas.Height) / 2;

            // Ensure position is never negative
            x = Math.Max(0, x);
            y = Math.Max(0, y);

            // Update canvas position
            canvas.Location = new Point(x, y);

            // Update scroll area
            panelCanvas.AutoScrollMinSize = new Size(
                Math.Max(scaledWidth, panelCanvas.ClientSize.Width),
                Math.Max(scaledHeight, panelCanvas.ClientSize.Height)
            );

            canvas.Invalidate();
        }

        private void SaveState()
        {
            if (layers != null && index != 5)  // Remove specific tool checks to simplify
            {
                // Create deep copies of all layers
                List<Layer> layersCopy = new List<Layer>();
                foreach (var layer in layers)
                {
                    Layer layerCopy = new Layer(layer.Bitmap.Width, layer.Bitmap.Height, layer.Name);
                    using (Graphics gr = Graphics.FromImage(layerCopy.Bitmap))
                    {
                        gr.Clear(Color.Transparent);
                        gr.DrawImage(layer.Bitmap, 0, 0);
                    }
                    layerCopy.Visible = layer.Visible;
                    layersCopy.Add(layerCopy);
                }

                // Save state with layers and text objects
                undoStack.Push(new SavedState
                {
                    Layers = layersCopy,
                    TextObjects = new List<TextObject>(textObjects)
                });
                redoStack.Clear();
            }
        }

        private class SelectionState
        {
            public Bitmap SelectionBitmap { get; set; }
            public Rectangle SelectionRectangle { get; set; }
            public List<Point> FreeFormPoints { get; set; }
            public bool IsSelectionActive { get; set; }
            public bool IsMoving { get; set; }
            public Point PastePosition { get; set; }
            public SelectionMode CurrentMode { get; set; }
            public Bitmap LayerBeforeChange { get; set; }  // Store layer state before selection operation
        }

        // Update SavedState to only contain drawing-related info
        private class SavedState
        {
            public List<Layer> Layers { get; set; }
            public List<TextObject> TextObjects { get; set; }
            public bool IsSelectionOperation { get; set; }

            public SavedState()
            {
                Layers = new List<Layer>();
                TextObjects = new List<TextObject>();
                IsSelectionOperation = false;
            }

            // Add parameterized constructor if needed
            public SavedState(List<Layer> layers, List<TextObject> textObjects, bool isSelectionOperation = false)
            {
                Layers = layers;
                TextObjects = textObjects;
                IsSelectionOperation = isSelectionOperation;
            }
        }
        private Stack<SelectionState> selectionUndoStack = new Stack<SelectionState>();
        private Stack<SelectionState> selectionRedoStack = new Stack<SelectionState>();
        private void SaveDrawingState()
        {
            // First commit any pending text
            if (textObjects.Count > 0)
            {
                // Draw text onto current layer before saving state
                using (Graphics graphics = Graphics.FromImage(CurrentLayer.Bitmap))
                {
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                    graphics.CompositingMode = CompositingMode.SourceOver;

                    foreach (var textObj in textObjects)
                    {
                        using (Brush brush = new SolidBrush(textObj.Color))
                        {
                            DrawTextInRectangle(graphics, textObj.Text, textObj.Rect, textObj.Font, brush);
                        }
                    }
                }
                textObjects.Clear();
            }

            // Then save the state with the committed text
            if (layers != null)
            {
                List<Layer> layersCopy = new List<Layer>();
                foreach (var layer in layers)
                {
                    Layer layerCopy = new Layer(layer.Bitmap.Width, layer.Bitmap.Height, layer.Name);
                    using (Graphics gr = Graphics.FromImage(layerCopy.Bitmap))
                    {
                        gr.Clear(Color.Transparent);  // Clear first to ensure proper transparency
                        gr.DrawImage(layer.Bitmap, 0, 0);
                    }
                    layerCopy.Visible = layer.Visible;
                    layersCopy.Add(layerCopy);
                }

                undoStack.Push(new SavedState
                {
                    Layers = layersCopy,
                    TextObjects = new List<TextObject>() // Empty list since text is committed
                });
                redoStack.Clear();
            }

            canvas.Invalidate();
        }

        private void SaveSelectionState()
        {
            // Create a copy of the current layer before modification
            Bitmap layerCopy = new Bitmap(CurrentLayer.Bitmap.Width, CurrentLayer.Bitmap.Height);
            using (Graphics g = Graphics.FromImage(layerCopy))
            {
                g.DrawImage(CurrentLayer.Bitmap, 0, 0);
            }

            // Save selection state
            Bitmap selectionCopy = null;
            if (copiedRegionBitmap != null)
            {
                selectionCopy = new Bitmap(copiedRegionBitmap);
            }

            selectionUndoStack.Push(new SelectionState
            {
                SelectionBitmap = selectionCopy,
                SelectionRectangle = selectionRectangle,
                FreeFormPoints = new List<Point>(freeFormPoints),
                IsSelectionActive = selectionToolActive,
                IsMoving = isMoving,
                PastePosition = pastePosition,
                CurrentMode = currentMode,
                LayerBeforeChange = layerCopy
            });
            selectionRedoStack.Clear();
        }
        private void SaveCurrentSelectionToRedo()
        {
            // Create a copy of the current layer
            Bitmap layerCopy = new Bitmap(CurrentLayer.Bitmap.Width, CurrentLayer.Bitmap.Height);
            using (Graphics g = Graphics.FromImage(layerCopy))
            {
                g.DrawImage(CurrentLayer.Bitmap, 0, 0);
            }

            // Save current selection bitmap if it exists
            Bitmap selectionCopy = null;
            if (copiedRegionBitmap != null)
            {
                selectionCopy = new Bitmap(copiedRegionBitmap);
            }

            // Push current state to redo stack
            selectionRedoStack.Push(new SelectionState
            {
                SelectionBitmap = selectionCopy,
                SelectionRectangle = selectionRectangle,
                FreeFormPoints = new List<Point>(freeFormPoints),
                IsSelectionActive = selectionToolActive,
                IsMoving = isMoving,
                PastePosition = pastePosition,
                CurrentMode = currentMode,
                LayerBeforeChange = layerCopy
            });
        }

        private void SaveCurrentSelectionToUndo()
        {
            // Create a copy of the current layer
            Bitmap layerCopy = new Bitmap(CurrentLayer.Bitmap.Width, CurrentLayer.Bitmap.Height);
            using (Graphics g = Graphics.FromImage(layerCopy))
            {
                g.DrawImage(CurrentLayer.Bitmap, 0, 0);
            }

            // Save current selection bitmap if it exists
            Bitmap selectionCopy = null;
            if (copiedRegionBitmap != null)
            {
                selectionCopy = new Bitmap(copiedRegionBitmap);
            }

            // Push current state to undo stack
            selectionUndoStack.Push(new SelectionState
            {
                SelectionBitmap = selectionCopy,
                SelectionRectangle = selectionRectangle,
                FreeFormPoints = new List<Point>(freeFormPoints),
                IsSelectionActive = selectionToolActive,
                IsMoving = isMoving,
                PastePosition = pastePosition,
                CurrentMode = currentMode,
                LayerBeforeChange = layerCopy
            });
        }

        private void SaveCurrentDrawingToRedo()
        {
            List<Layer> currentLayers = new List<Layer>();
            foreach (var layer in layers)
            {
                Layer layerCopy = new Layer(layer.Bitmap.Width, layer.Bitmap.Height, layer.Name);
                using (Graphics gr = Graphics.FromImage(layerCopy.Bitmap))
                {
                    gr.DrawImage(layer.Bitmap, 0, 0);
                }
                layerCopy.Visible = layer.Visible;
                currentLayers.Add(layerCopy);
            }

            redoStack.Push(new SavedState
            {
                Layers = currentLayers,
                TextObjects = new List<TextObject>(textObjects)
            });
        }

        private void SaveCurrentDrawingToUndo()
        {
            List<Layer> currentLayers = new List<Layer>();
            foreach (var layer in layers)
            {
                Layer layerCopy = new Layer(layer.Bitmap.Width, layer.Bitmap.Height, layer.Name);
                using (Graphics gr = Graphics.FromImage(layerCopy.Bitmap))
                {
                    gr.DrawImage(layer.Bitmap, 0, 0);
                }
                layerCopy.Visible = layer.Visible;
                currentLayers.Add(layerCopy);
            }

            undoStack.Push(new SavedState
            {
                Layers = currentLayers,
                TextObjects = new List<TextObject>(textObjects)
            });
        }
        private void Undo()
        {
            if (selectionToolActive && selectionUndoStack.Count > 0)
            {
                // Handle selection undo without saving current state
                SelectionState previousState = selectionUndoStack.Pop();
                selectionRedoStack.Push(previousState);
                RestoreSelectionState(previousState);
                canvas.Invalidate();
            }
            else if (undoStack.Count > 0)
            {
                // Handle regular drawing undo without saving current state
                SavedState previousState = undoStack.Pop();
                redoStack.Push(previousState);
                RestoreState(previousState);
                canvas.Invalidate();

                // Only update layer UI if necessary
                if (HasLayerStructureChanged(previousState))
                {
                    UpdateLayerListUI();
                }
            }
        }

        private void Redo()
        {
            if (selectionToolActive && selectionRedoStack.Count > 0)
            {
                // Handle selection redo without saving current state
                SelectionState nextState = selectionRedoStack.Pop();
                selectionUndoStack.Push(nextState);
                RestoreSelectionState(nextState);
                canvas.Invalidate();
            }
            else if (redoStack.Count > 0)
            {
                // Handle regular drawing redo without saving current state
                SavedState nextState = redoStack.Pop();
                undoStack.Push(nextState);
                RestoreState(nextState);
                canvas.Invalidate();

                // Only update layer UI if necessary
                if (HasLayerStructureChanged(nextState))
                {
                    UpdateLayerListUI();
                }
            }
        }
        private void btnUndo_Click(object sender, EventArgs e)
        {
            Undo();
        }
        
        private bool HasLayerStructureChanged(SavedState state)
        {
            if (layers.Count != state.Layers.Count)
                return true;

            for (int i = 0; i < layers.Count; i++)
            {
                if (layers[i].Name != state.Layers[i].Name ||
                    layers[i].Visible != state.Layers[i].Visible)
                    return true;
            }

            return false;
        }
        private void RestoreSelectionState(SelectionState state)
        {
            // Restore the layer to its previous state
            if (state.LayerBeforeChange != null)
            {
                using (Graphics g = Graphics.FromImage(CurrentLayer.Bitmap))
                {
                    g.Clear(Color.Transparent);
                    g.DrawImage(state.LayerBeforeChange, 0, 0);
                }
            }

            // Restore selection properties
            selectionRectangle = state.SelectionRectangle;
            freeFormPoints = new List<Point>(state.FreeFormPoints);
            selectionToolActive = state.IsSelectionActive;
            isMoving = state.IsMoving;
            pastePosition = state.PastePosition;
            currentMode = state.CurrentMode;

            // Restore the selection bitmap
            if (state.SelectionBitmap != null)
            {
                if (copiedRegionBitmap != null)
                {
                    copiedRegionBitmap.Dispose();
                }
                copiedRegionBitmap = new Bitmap(state.SelectionBitmap);
            }
            else
            {
                copiedRegionBitmap = null;
            }

            canvas.Invalidate();
        }
        private void RestoreState(SavedState state)
        {
            // Clear any active selection
            copiedRegionBitmap = null;
            selectionRectangle = Rectangle.Empty;
            freeFormPoints.Clear();
            selectionToolActive = false;

            // Restore layers
            layers.Clear();
            foreach (var layer in state.Layers)
            {
                layers.Add(layer);
            }

            // Restore text objects
            textObjects = state.TextObjects;
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
            if (e.Control && e.KeyCode == Keys.C)
            {
                copyToolStripMenuItem_Click(null, EventArgs.Empty);
                e.Handled = true;
            }
            if (e.Control && e.KeyCode == Keys.X)
            {
                cutToolStripMenuItem_Click(null, EventArgs.Empty);
                e.Handled = true;
            }
            if (e.Control && e.KeyCode == Keys.V)
            {
                pasteToolStripMenuItem_Click(null, EventArgs.Empty);
                e.Handled = true;
            }
            else if(e.KeyCode==Keys.Delete && selectionToolActive && !selectionRectangle.IsEmpty)
            {
                SaveSelectionState();
                if (currentMode == SelectionMode.Rectangle)
                {
                    CurrentLayer.DeleteRegion(selectionRectangle);
                }
                else if (currentMode == SelectionMode.FreeForm && freeFormPoints.Count > 2)
                {
                    GraphicsPath path = new GraphicsPath();
                    path.AddPolygon(freeFormPoints.ToArray());
                    CurrentLayer.DeleteRegion(path);
                }

                selectionRectangle = Rectangle.Empty;
                freeFormPoints.Clear();
                copiedRegionBitmap = null;
                isMoving = false;

                canvas.Invalidate();
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
            else if (e.KeyCode == Keys.Delete && selectionToolActive && !selectionRectangle.IsEmpty)
            {
                SaveState();
                using (Graphics g = Graphics.FromImage(CurrentLayer.Bitmap))
                {
                    // Set compositing mode to copy source
                    g.CompositingMode = CompositingMode.SourceCopy;

                    if (currentMode == SelectionMode.Rectangle)
                    {
                        // Create a bitmap with the same size as the selection
                        using (Bitmap tempBitmap = new Bitmap(selectionRectangle.Width, selectionRectangle.Height))
                        {
                            // Draw the transparent area
                            using (Graphics tempG = Graphics.FromImage(tempBitmap))
                            {
                                tempG.Clear(Color.Transparent);
                            }
                            // Copy the transparent area to the layer
                            g.DrawImage(tempBitmap, selectionRectangle);
                        }
                    }
                    else if (currentMode == SelectionMode.FreeForm && freeFormPoints.Count > 2)
                    {
                        GraphicsPath path = new GraphicsPath();
                        path.AddPolygon(freeFormPoints.ToArray());

                        // Create a region from the path
                        using (Region region = new Region(path))
                        {
                            // Clear the region with transparency
                            g.SetClip(region, CombineMode.Replace);
                            g.Clear(Color.Transparent);
                        }
                    }

                    // Reset compositing mode
                    g.CompositingMode = CompositingMode.SourceOver;
                }

                selectionRectangle = Rectangle.Empty;
                freeFormPoints.Clear();
                copiedRegionBitmap = null;
                isMoving = false;

                canvas.Invalidate();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                inputText = ""; // Hủy nhập
                isTyping = false;
                canvas.Invalidate();
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
            SaveDrawingState();
            if (currentIndex != 6 && currentIndex != -1) // If switching to text from another tool
            {
                // Don't commit texts when switching to text tool
                currentIndex = index = 6;
            }
            else
            {
                currentIndex = index = 6;
            }
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
            fontDialog.Location = new Point(btnPosition.X - 30, btnPosition.Y + 80);
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
            selectionToolActive = false;
            if (!selectionToolActive)
            {
                selectionRectangle = Rectangle.Empty;
                freeFormPoints.Clear();
                canvas.Invalidate();
            }
        }
        // Phương thức vẽ hình chữ nhật nét đứt và văn bản
        private void DrawTextInRectangle(Graphics g, string text, Rectangle rect, Font font, Brush brush)
        {
            using (Graphics currentG = Graphics.FromImage(CurrentLayer.Bitmap))
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
            
        }
        private void DrawTransparencyCheckerboard(Graphics g, Rectangle rect)
        {
            int squareSize = 8; // Size of each checkerboard square
            using (Brush lightBrush = new SolidBrush(Color.FromArgb(255, 255, 255)))
            using (Brush darkBrush = new SolidBrush(Color.FromArgb(204, 204, 204)))
            {
                for (int y = rect.Top; y < rect.Bottom; y += squareSize)
                {
                    for (int x = rect.Left; x < rect.Right; x += squareSize)
                    {
                        Brush brush = ((x + y) / squareSize) % 2 == 0 ? lightBrush : darkBrush;
                        g.FillRectangle(brush, x, y, squareSize, squareSize);
                    }
                }
            }
        }
        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.Half;
            DrawTransparencyCheckerboard(e.Graphics, canvas.ClientRectangle);

            if (layers != null)
            {
                // Create a temporary bitmap for compositing
                using (Bitmap compositeBitmap = new Bitmap(canvas.Width, canvas.Height))
                using (Graphics compositeG = Graphics.FromImage(compositeBitmap))
                {
                    compositeG.Clear(Color.Transparent);

                    // Draw each visible layer
                    foreach (var layer in layers)
                    {
                        if (layer.Visible)
                        {
                            // Create color matrix to handle opacity
                            ColorMatrix matrix = new ColorMatrix();
                            matrix.Matrix33 = layer.Opacity; // Set opacity

                            ImageAttributes attributes = new ImageAttributes();
                            attributes.SetColorMatrix(matrix);

                            Rectangle rect = new Rectangle(0, 0, canvas.Width, canvas.Height);
                            compositeG.DrawImage(layer.Bitmap, rect, 0, 0,
                                layer.Bitmap.Width, layer.Bitmap.Height,
                                GraphicsUnit.Pixel, attributes);
                        }
                    }

                    // Draw the final composite
                    e.Graphics.DrawImage(compositeBitmap, 0, 0);
                }
            }

            // Draw textboxes
            foreach (var textObj in textObjects)
            {
                using (Brush brush = new SolidBrush(textObj.Color))
                {
                    DrawTextInRectangle(e.Graphics, textObj.Text, textObj.Rect, textObj.Font, brush);
                }
            }

            if (index == 6) // Text tool selected
            {
                // Draw current text box if typing
                if (isTyping)
                {
                    using (Pen pen = new Pen(Color.Black, 1))
                    {
                        e.Graphics.DrawRectangle(pen, textBoxRect);
                    }
                    if (!string.IsNullOrEmpty(inputText))
                    {
                        using (Brush brush = new SolidBrush(pic_ColorStroke.BackColor))
                        {
                            DrawTextInRectangle(e.Graphics, inputText, textBoxRect, textFont, brush);
                        }
                    }
                }
            }

            // Draw shapes
            foreach (var shape in shapes)
            {
                DrawShapeOnGraphics(e.Graphics, shape);
            }

            // Draw current shape preview while dragging
            if (paint && index == 9 && currentShape != null)
            {
                DrawShapeOnGraphics(e.Graphics, currentShape);
            }

            // Draw selection UI elements
            if (isSelecting || selectionToolActive || isMoving)
            {
                // Draw the moving content first
                if (copiedRegionBitmap != null && selectionToolActive)
                {
                    Rectangle scaledRect = new Rectangle(
                        (int)(selectionRectangle.X * zoomFactor),
                        (int)(selectionRectangle.Y * zoomFactor),
                        (int)(selectionRectangle.Width * zoomFactor),
                        (int)(selectionRectangle.Height * zoomFactor)
                    );

                    if (currentMode == SelectionMode.FreeForm)
                    {
                        using (GraphicsPath path = new GraphicsPath())
                        {
                            Point[] scaledPoints = freeFormPoints.Select(p => new Point(
                                (int)(p.X * zoomFactor),
                                (int)(p.Y * zoomFactor)
                            )).ToArray();

                            path.AddPolygon(scaledPoints);
                            Region originalClip = e.Graphics.Clip;
                            e.Graphics.SetClip(path);
                            e.Graphics.DrawImage(copiedRegionBitmap, scaledRect.Location);
                            e.Graphics.Clip = originalClip;
                        }
                    }
                    else
                    {
                        e.Graphics.DrawImage(copiedRegionBitmap, scaledRect.Location);
                    }
                }

                // Draw selection outline
                using (Pen selectionPen = new Pen(Color.Black, 2) { DashStyle = DashStyle.Dash })
                {
                    if (currentMode == SelectionMode.Rectangle && !selectionRectangle.IsEmpty)
                    {
                        Rectangle scaledRect = new Rectangle(
                            (int)(selectionRectangle.X * zoomFactor),
                            (int)(selectionRectangle.Y * zoomFactor),
                            (int)(selectionRectangle.Width * zoomFactor),
                            (int)(selectionRectangle.Height * zoomFactor)
                        );
                        e.Graphics.DrawRectangle(selectionPen, scaledRect);
                    }
                    else if (currentMode == SelectionMode.FreeForm && freeFormPoints.Count > 1)
                    {
                        Point[] scaledPoints = freeFormPoints.Select(p => new Point(
                            (int)(p.X * zoomFactor),
                            (int)(p.Y * zoomFactor)
                        )).ToArray();
                        e.Graphics.DrawPolygon(selectionPen, scaledPoints);
                    }
                }
            }
        }
        private void btnSelection_Click(object sender, EventArgs e)
        {
            selectionRectangle = Rectangle.Empty;
            freeFormPoints.Clear();
            isSelecting = false;
            selectionToolActive = false;
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectionToolActive && !selectionRectangle.IsEmpty)
            {
                // Create a new bitmap for the selected region if it hasn't been copied yet
                if (copiedRegionBitmap == null)
                {
                    if (currentMode == SelectionMode.Rectangle)
                    {
                        copiedRegionBitmap = new Bitmap(selectionRectangle.Width, selectionRectangle.Height);
                        using (Graphics g = Graphics.FromImage(copiedRegionBitmap))
                        {
                            g.DrawImage(CurrentLayer.Bitmap, new Rectangle(0, 0, copiedRegionBitmap.Width, copiedRegionBitmap.Height),
                                selectionRectangle, GraphicsUnit.Pixel);
                        }
                    }
                    else if (currentMode == SelectionMode.FreeForm && freeFormPoints.Count > 2)
                    {
                        copiedRegionBitmap = new Bitmap(selectionRectangle.Width, selectionRectangle.Height);
                        using (Graphics g = Graphics.FromImage(copiedRegionBitmap))
                        {
                            // Create path for clipping
                            GraphicsPath path = new GraphicsPath();
                            Point[] translatedPoints = freeFormPoints
                                .Select(p => new Point(p.X - selectionRectangle.X, p.Y - selectionRectangle.Y))
                                .ToArray();
                            path.AddPolygon(translatedPoints);

                            // Set clipping region
                            g.SetClip(path);

                            // Copy only the area inside the path
                            g.DrawImage(CurrentLayer.Bitmap, new Rectangle(0, 0, copiedRegionBitmap.Width, copiedRegionBitmap.Height),
                                selectionRectangle, GraphicsUnit.Pixel);
                        }
                    }
                }

                // Copy to clipboard if we have a valid bitmap
                if (copiedRegionBitmap != null)
                {
                    Clipboard.SetImage(copiedRegionBitmap);
                }
            }
        }
        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveSelectionState();
            if (Clipboard.ContainsImage())
            {
                selectionRectangle = Rectangle.Empty;
                freeFormPoints.Clear();

                using (Bitmap originalBitmap = new Bitmap(Clipboard.GetImage()))
                {
                    copiedRegionBitmap = new Bitmap(originalBitmap.Width, originalBitmap.Height, PixelFormat.Format32bppArgb);

                    // Check if the bitmap has any non-white pixels (content)
                    bool hasContent = false;
                    for (int x = 0; x < originalBitmap.Width && !hasContent; x++)
                    {
                        for (int y = 0; y < originalBitmap.Height && !hasContent; y++)
                        {
                            Color pixelColor = originalBitmap.GetPixel(x, y);
                            // If pixel is not white, it's considered content
                            if (pixelColor.R != 255 || pixelColor.G != 255 || pixelColor.B != 255)
                            {
                                hasContent = true;
                                break;
                            }
                        }
                    }

                    // If no content found, make it transparent
                    if (!hasContent)
                    {
                        using (Graphics g = Graphics.FromImage(copiedRegionBitmap))
                        {
                            g.Clear(Color.Transparent);
                        }
                    }
                    else  // If has content, copy the original image
                    {
                        using (Graphics g = Graphics.FromImage(copiedRegionBitmap))
                        {
                            g.Clear(Color.Transparent);
                            g.DrawImage(originalBitmap, 0, 0);
                        }
                    }
                }

                Point pasteLocation = new Point(10, 10);
                selectionRectangle = new Rectangle(
                    pasteLocation.X,
                    pasteLocation.Y,
                    copiedRegionBitmap.Width,
                    copiedRegionBitmap.Height
                );

                isMoving = true;
                pastePosition = pasteLocation;
                currentMode = SelectionMode.Rectangle;
                btnSelection.Image = Properties.Resources.noun_dotted_rectangle11;
                selectionToolActive = true;
                canvas.Invalidate();
            }
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveSelectionState();
            if (selectionToolActive && !selectionRectangle.IsEmpty)
            {
                // Copy the region if it hasn't been copied yet
                if (copiedRegionBitmap == null)
                {
                    if (currentMode == SelectionMode.Rectangle)
                    {
                        copiedRegionBitmap = new Bitmap(selectionRectangle.Width, selectionRectangle.Height);
                        using (Graphics g = Graphics.FromImage(copiedRegionBitmap))
                        {
                            g.DrawImage(CurrentLayer.Bitmap, new Rectangle(0, 0, copiedRegionBitmap.Width, copiedRegionBitmap.Height),
                                selectionRectangle, GraphicsUnit.Pixel);
                        }
                    }
                    else if (currentMode == SelectionMode.FreeForm && freeFormPoints.Count > 2)
                    {
                        copiedRegionBitmap = new Bitmap(selectionRectangle.Width, selectionRectangle.Height);
                        using (Graphics g = Graphics.FromImage(copiedRegionBitmap))
                        {
                            GraphicsPath path = new GraphicsPath();
                            Point[] translatedPoints = freeFormPoints
                                .Select(p => new Point(p.X - selectionRectangle.X, p.Y - selectionRectangle.Y))
                                .ToArray();
                            path.AddPolygon(translatedPoints);
                            g.SetClip(path);
                            g.DrawImage(CurrentLayer.Bitmap, new Rectangle(0, 0, copiedRegionBitmap.Width, copiedRegionBitmap.Height),
                                selectionRectangle, GraphicsUnit.Pixel);
                        }
                    }
                }

                // Copy to clipboard
                if (copiedRegionBitmap != null)
                {
                    Clipboard.SetImage(copiedRegionBitmap);
                }

                // Clear the selected area with true transparency
                using (Graphics g = Graphics.FromImage(CurrentLayer.Bitmap))
                {
                    g.CompositingMode = CompositingMode.SourceCopy;  // Important!

                    if (currentMode == SelectionMode.Rectangle)
                    {
                        using (SolidBrush clearBrush = new SolidBrush(Color.FromArgb(0, 0, 0, 0)))
                        {
                            g.FillRectangle(clearBrush, selectionRectangle);
                        }
                    }
                    else if (currentMode == SelectionMode.FreeForm && freeFormPoints.Count > 2)
                    {
                        GraphicsPath path = new GraphicsPath();
                        path.AddPolygon(freeFormPoints.ToArray());
                        using (SolidBrush clearBrush = new SolidBrush(Color.FromArgb(0, 0, 0, 0)))
                        {
                            g.FillPath(clearBrush, path);
                        }
                    }

                    g.CompositingMode = CompositingMode.SourceOver;  // Reset
                }

                // Clear selection
                selectionRectangle = Rectangle.Empty;
                freeFormPoints.Clear();
                copiedRegionBitmap = null;
                isMoving = false;
                selectionToolActive = false;

                canvas.Invalidate();
            }
        }
        private void FlipHorizontal()
        {
            SaveSelectionState();
            if (selectionToolActive && !selectionRectangle.IsEmpty)
            {
                // Create initial copy if it doesn't exist
                if (copiedRegionBitmap == null)
                {
                    copiedRegionBitmap = new Bitmap(selectionRectangle.Width, selectionRectangle.Height, PixelFormat.Format32bppArgb);
                    using (Graphics g = Graphics.FromImage(copiedRegionBitmap))
                    {
                        g.Clear(Color.Transparent);
                        if (currentMode == SelectionMode.FreeForm && freeFormPoints.Count > 2)
                        {
                            GraphicsPath path = new GraphicsPath();
                            Point[] translatedPoints = freeFormPoints
                                .Select(p => new Point(p.X - selectionRectangle.X, p.Y - selectionRectangle.Y))
                                .ToArray();
                            path.AddPolygon(translatedPoints);
                            g.SetClip(path);
                        }
                        g.DrawImage(CurrentLayer.Bitmap,
                            new Rectangle(0, 0, copiedRegionBitmap.Width, copiedRegionBitmap.Height),
                            selectionRectangle,
                            GraphicsUnit.Pixel);
                    }

                    // Clear the original area
                    ClearOriginalArea();
                }

                // Create new bitmap for flipped image
                Bitmap flippedBitmap = new Bitmap(copiedRegionBitmap.Width, copiedRegionBitmap.Height, PixelFormat.Format32bppArgb);
                flippedBitmap.SetResolution(copiedRegionBitmap.HorizontalResolution, copiedRegionBitmap.VerticalResolution);

                using (Graphics g = Graphics.FromImage(flippedBitmap))
                {
                    g.Clear(Color.Transparent);
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.CompositingMode = CompositingMode.SourceOver;
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.SmoothingMode = SmoothingMode.HighQuality;

                    // Use matrix transformation for flipping
                    g.TranslateTransform(flippedBitmap.Width, 0);
                    g.ScaleTransform(-1, 1);
                    g.DrawImage(copiedRegionBitmap, 0, 0);
                }

                // Update the bitmap
                copiedRegionBitmap.Dispose();
                copiedRegionBitmap = flippedBitmap;

                canvas.Invalidate();
            }
        }

        private void FlipVertical()
        {
            SaveSelectionState();
            if (selectionToolActive && !selectionRectangle.IsEmpty)
            {
                // Create initial copy if it doesn't exist
                if (copiedRegionBitmap == null)
                {
                    copiedRegionBitmap = new Bitmap(selectionRectangle.Width, selectionRectangle.Height, PixelFormat.Format32bppArgb);
                    using (Graphics g = Graphics.FromImage(copiedRegionBitmap))
                    {
                        g.Clear(Color.Transparent);
                        if (currentMode == SelectionMode.FreeForm && freeFormPoints.Count > 2)
                        {
                            GraphicsPath path = new GraphicsPath();
                            Point[] translatedPoints = freeFormPoints
                                .Select(p => new Point(p.X - selectionRectangle.X, p.Y - selectionRectangle.Y))
                                .ToArray();
                            path.AddPolygon(translatedPoints);
                            g.SetClip(path);
                        }
                        g.DrawImage(CurrentLayer.Bitmap,
                            new Rectangle(0, 0, copiedRegionBitmap.Width, copiedRegionBitmap.Height),
                            selectionRectangle,
                            GraphicsUnit.Pixel);
                    }

                    // Clear the original area
                    ClearOriginalArea();
                }

                // Create new bitmap for flipped image
                Bitmap flippedBitmap = new Bitmap(copiedRegionBitmap.Width, copiedRegionBitmap.Height, PixelFormat.Format32bppArgb);
                flippedBitmap.SetResolution(copiedRegionBitmap.HorizontalResolution, copiedRegionBitmap.VerticalResolution);

                using (Graphics g = Graphics.FromImage(flippedBitmap))
                {
                    g.Clear(Color.Transparent);
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.CompositingMode = CompositingMode.SourceOver;
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.SmoothingMode = SmoothingMode.HighQuality;

                    // Use matrix transformation for flipping
                    g.TranslateTransform(0, flippedBitmap.Height);
                    g.ScaleTransform(1, -1);
                    g.DrawImage(copiedRegionBitmap, 0, 0);
                }

                // Update the bitmap
                copiedRegionBitmap.Dispose();
                copiedRegionBitmap = flippedBitmap;

                canvas.Invalidate();
            }
        }
        private void RotateLeft90()
        {
            SaveSelectionState();
            if (selectionToolActive && !selectionRectangle.IsEmpty)
            {
                // Create initial copy if it doesn't exist
                if (copiedRegionBitmap == null)
                {
                    copiedRegionBitmap = new Bitmap(selectionRectangle.Width, selectionRectangle.Height, PixelFormat.Format32bppArgb);
                    using (Graphics g = Graphics.FromImage(copiedRegionBitmap))
                    {
                        g.Clear(Color.Transparent);  // Clear with transparency
                        if (currentMode == SelectionMode.FreeForm && freeFormPoints.Count > 2)
                        {
                            // Handle freeform selection
                            GraphicsPath path = new GraphicsPath();
                            Point[] translatedPoints = freeFormPoints
                                .Select(p => new Point(p.X - selectionRectangle.X, p.Y - selectionRectangle.Y))
                                .ToArray();
                            path.AddPolygon(translatedPoints);
                            g.SetClip(path);
                        }
                        g.DrawImage(CurrentLayer.Bitmap,
                            new Rectangle(0, 0, copiedRegionBitmap.Width, copiedRegionBitmap.Height),
                            selectionRectangle,
                            GraphicsUnit.Pixel);
                    }

                    // Clear the original area
                    ClearOriginalArea();
                }

                // Create new bitmap with swapped dimensions
                Bitmap rotatedBitmap = new Bitmap(copiedRegionBitmap.Height, copiedRegionBitmap.Width, PixelFormat.Format32bppArgb);
                rotatedBitmap.SetResolution(copiedRegionBitmap.HorizontalResolution, copiedRegionBitmap.VerticalResolution);

                using (Graphics g = Graphics.FromImage(rotatedBitmap))
                {
                    g.Clear(Color.Transparent);
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.CompositingMode = CompositingMode.SourceOver;
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.SmoothingMode = SmoothingMode.HighQuality;

                    // Calculate center points
                    float centerX = rotatedBitmap.Width / 2f;
                    float centerY = rotatedBitmap.Height / 2f;
                    float oldCenterX = copiedRegionBitmap.Width / 2f;
                    float oldCenterY = copiedRegionBitmap.Height / 2f;

                    // Apply transformation
                    g.TranslateTransform(centerX, centerY);
                    g.RotateTransform(-90);
                    g.TranslateTransform(-oldCenterX, -oldCenterY);

                    // Draw the image
                    g.DrawImage(copiedRegionBitmap, 0, 0);
                }

                // Update the bitmap
                copiedRegionBitmap.Dispose();
                copiedRegionBitmap = rotatedBitmap;

                // Update selection rectangle
                Point center = new Point(
                    selectionRectangle.X + selectionRectangle.Width / 2,
                    selectionRectangle.Y + selectionRectangle.Height / 2
                );
                selectionRectangle = new Rectangle(
                    center.X - rotatedBitmap.Width / 2,
                    center.Y - rotatedBitmap.Height / 2,
                    rotatedBitmap.Width,
                    rotatedBitmap.Height
                );

                canvas.Invalidate();
            }
        }
        private void RotateRight90()
        {
            SaveSelectionState();
            if (selectionToolActive && !selectionRectangle.IsEmpty)
            {
                // Create initial copy if it doesn't exist
                if (copiedRegionBitmap == null)
                {
                    copiedRegionBitmap = new Bitmap(selectionRectangle.Width, selectionRectangle.Height, PixelFormat.Format32bppArgb);
                    using (Graphics g = Graphics.FromImage(copiedRegionBitmap))
                    {
                        g.Clear(Color.Transparent);  // Clear with transparency
                        if (currentMode == SelectionMode.FreeForm && freeFormPoints.Count > 2)
                        {
                            // Handle freeform selection
                            GraphicsPath path = new GraphicsPath();
                            Point[] translatedPoints = freeFormPoints
                                .Select(p => new Point(p.X - selectionRectangle.X, p.Y - selectionRectangle.Y))
                                .ToArray();
                            path.AddPolygon(translatedPoints);
                            g.SetClip(path);
                        }
                        g.DrawImage(CurrentLayer.Bitmap,
                            new Rectangle(0, 0, copiedRegionBitmap.Width, copiedRegionBitmap.Height),
                            selectionRectangle,
                            GraphicsUnit.Pixel);
                    }

                    // Clear the original area
                    ClearOriginalArea();
                }

                // Create new bitmap with swapped dimensions
                Bitmap rotatedBitmap = new Bitmap(copiedRegionBitmap.Height, copiedRegionBitmap.Width, PixelFormat.Format32bppArgb);
                rotatedBitmap.SetResolution(copiedRegionBitmap.HorizontalResolution, copiedRegionBitmap.VerticalResolution);

                using (Graphics g = Graphics.FromImage(rotatedBitmap))
                {
                    g.Clear(Color.Transparent);
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.CompositingMode = CompositingMode.SourceOver;
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.SmoothingMode = SmoothingMode.HighQuality;

                    // Calculate center points
                    float centerX = rotatedBitmap.Width / 2f;
                    float centerY = rotatedBitmap.Height / 2f;
                    float oldCenterX = copiedRegionBitmap.Width / 2f;
                    float oldCenterY = copiedRegionBitmap.Height / 2f;

                    // Apply transformation
                    g.TranslateTransform(centerX, centerY);
                    g.RotateTransform(90);
                    g.TranslateTransform(-oldCenterX, -oldCenterY);

                    // Draw the image
                    g.DrawImage(copiedRegionBitmap, 0, 0);
                }

                // Update the bitmap
                copiedRegionBitmap.Dispose();
                copiedRegionBitmap = rotatedBitmap;

                // Update selection rectangle
                Point center = new Point(
                    selectionRectangle.X + selectionRectangle.Width / 2,
                    selectionRectangle.Y + selectionRectangle.Height / 2
                );
                selectionRectangle = new Rectangle(
                    center.X - rotatedBitmap.Width / 2,
                    center.Y - rotatedBitmap.Height / 2,
                    rotatedBitmap.Width,
                    rotatedBitmap.Height
                );

                canvas.Invalidate();
            }
        }
        private void Rotate180()
        {
            // Call Rotate90 twice
            RotateRight90();
            RotateRight90();
        }
        private void CropSelection()
        {
            if (selectionToolActive && !selectionRectangle.IsEmpty)
            {
                SaveSelectionState();

                // Initialize the cropped bitmap
                Bitmap croppedBitmap = null;

                if (currentMode == SelectionMode.Rectangle)
                {
                    croppedBitmap = new Bitmap(selectionRectangle.Width, selectionRectangle.Height);
                    using (Graphics g = Graphics.FromImage(croppedBitmap))
                    {
                        g.DrawImage(CurrentLayer.Bitmap, new Rectangle(0, 0, croppedBitmap.Width, croppedBitmap.Height),
                            selectionRectangle, GraphicsUnit.Pixel);
                    }
                }
                else if (currentMode == SelectionMode.FreeForm && freeFormPoints.Count > 2)
                {
                    croppedBitmap = new Bitmap(selectionRectangle.Width, selectionRectangle.Height);
                    using (Graphics g = Graphics.FromImage(croppedBitmap))
                    {
                        GraphicsPath path = new GraphicsPath();
                        Point[] translatedPoints = freeFormPoints
                            .Select(p => new Point(p.X - selectionRectangle.X, p.Y - selectionRectangle.Y))
                            .ToArray();
                        path.AddPolygon(translatedPoints);
                        g.SetClip(path);
                        g.DrawImage(CurrentLayer.Bitmap, new Rectangle(0, 0, croppedBitmap.Width, croppedBitmap.Height),
                            selectionRectangle, GraphicsUnit.Pixel);
                    }
                }

                // Check if we have a valid cropped bitmap
                if (croppedBitmap != null)
                {
                    // Update all layers to new size
                    List<Layer> newLayers = new List<Layer>();
                    foreach (var layer in layers)
                    {
                        Layer newLayer = new Layer(croppedBitmap.Width, croppedBitmap.Height, layer.Name);
                        using (Graphics g = Graphics.FromImage(newLayer.Bitmap))
                        {
                            // Draw the old layer content with offset
                            g.DrawImage(layer.Bitmap,
                                new Rectangle(0, 0, croppedBitmap.Width, croppedBitmap.Height),
                                selectionRectangle,
                                GraphicsUnit.Pixel);
                        }
                        newLayer.Visible = layer.Visible;
                        newLayers.Add(newLayer);
                    }

                    // Update layers
                    layers = newLayers;

                    // Update canvas size
                    canvas.Size = croppedBitmap.Size;

                    // Adjust text objects positions
                    foreach (var textObj in textObjects)
                    {
                        textObj.Rect = new Rectangle(
                            textObj.Rect.X - selectionRectangle.X,
                            textObj.Rect.Y - selectionRectangle.Y,
                            textObj.Rect.Width,
                            textObj.Rect.Height
                        );
                    }

                    // Clear selection
                    selectionRectangle = Rectangle.Empty;
                    freeFormPoints.Clear();
                    copiedRegionBitmap = null;
                    isMoving = false;
                    selectionToolActive = false;

                    // Reset zoom to fit
                    zoomFactor = 1.0f;
                    CenterCanvas();

                    // Update UI
                    canvas.Invalidate();
                    UpdateLayerListUI();
                }
            }
        }
        private void btnSelection_Click_1(object sender, EventArgs e)
        {

        }

        private void btnCrop_Click(object sender, EventArgs e)
        {
            CropSelection();
        }

        private void btnFitWindow_Click(object sender, EventArgs e)
        {
            zoomFactor = 1.0f;  // Reset to default zoom (100%)

            // Reset canvas size to original bitmap dimensions
            canvas.Width = CurrentLayer.Bitmap.Width;
            canvas.Height = CurrentLayer.Bitmap.Height;

            // Reset scroll position first
            panelCanvas.AutoScrollPosition = Point.Empty;

            // Update scroll area
            panelCanvas.AutoScrollMinSize = new Size(
                Math.Max(canvas.Width, panelCanvas.ClientSize.Width),
                Math.Max(canvas.Height, panelCanvas.ClientSize.Height)
            );

            // Center the canvas
            CenterCanvas();

            // Update zoom level display
            toolStripStatusLabel1.Text = "Zoom: 100%";

            canvas.Invalidate();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnAddLayer_Click(object sender, EventArgs e)
        {
            int maxLayerNumber = 0;
            foreach (var layer in layers)
            {
                if (layer.Name.StartsWith("Layer "))
                {
                    string numberStr = layer.Name.Substring(6); // "Layer " is 6 characters
                    if (int.TryParse(numberStr, out int number))
                    {
                        maxLayerNumber = Math.Max(maxLayerNumber, number);
                    }
                }
            }

            // Create new layer with next number
            string name = $"Layer {maxLayerNumber + 1}";
            Layer newLayer = new Layer(canvas.Width, canvas.Height, name);
            layers.Add(newLayer);
            currentLayerIndex = layers.Count - 1;
            UpdateLayerListUI();
            canvas.Invalidate();
        }

        private void UpdateLayerListUI()
        {
            if (textObjects.Count > 0 && index == 6)
            {
                CommitAllTexts();
            }
            layerPanel.Controls.Clear();

            // Add layers from bottom (oldest) to top (newest)
            for (int i = 0; i < layers.Count; i++)  // Changed iteration order
            {
                Layer layer = layers[i];
                Panel layerItemPanel = new Panel
                {
                    Dock = DockStyle.Top,
                    Height = 30
                };

                CheckBox visibilityCheckBox = new CheckBox
                {
                    Checked = layer.Visible,
                    Location = new Point(10, 10),
                    Text = layer.Name,
                    Font = new Font("Arial", 12, FontStyle.Regular),
                    AutoSize = true
                };

                int index = i;
                visibilityCheckBox.CheckedChanged += (s, ev) =>
                {
                    layers[index].Visible = visibilityCheckBox.Checked;
                    canvas.Invalidate();
                };

                layerItemPanel.Controls.Add(visibilityCheckBox);
                layerItemPanel.ContextMenuStrip = layerContextMenuStrip;

                if (i == currentLayerIndex)
                {
                    layerItemPanel.BackColor = Color.LightBlue;
                }

                layerItemPanel.Click += (s, ev) =>
                {
                    currentLayerIndex = index;
                    UpdateLayerListUI();
                    canvas.Invalidate();
                };
                visibilityCheckBox.Click += (s, ev) =>
                {
                    currentLayerIndex = index;
                    UpdateLayerListUI();
                    canvas.Invalidate();
                };

                layerPanel.Controls.Add(layerItemPanel);
            }

            // Add the button last (will appear at top)
            btnAddLayer.Location = new Point(10, 5);
            btnAddLayer.Dock = DockStyle.Top;
            layerPanel.Controls.Add(btnAddLayer);
        }

        private void moveUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentLayerIndex < layers.Count - 1)
            {
                // Commit any pending text before moving layers
                if (textObjects.Count > 0)
                {
                    CommitAllTexts();
                }

                SaveDrawingState(); // Save state before moving
                Layer temp = layers[currentLayerIndex];
                layers[currentLayerIndex] = layers[currentLayerIndex + 1];
                layers[currentLayerIndex + 1] = temp;
                currentLayerIndex++;
                UpdateLayerListUI();
                canvas.Invalidate();
            }
        }

        private void moveDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentLayerIndex > 0)
            {
                // Commit any pending text before moving layers
                if (textObjects.Count > 0)
                {
                    CommitAllTexts();
                }

                SaveDrawingState(); // Save state before moving
                Layer temp = layers[currentLayerIndex];
                layers[currentLayerIndex] = layers[currentLayerIndex - 1];
                layers[currentLayerIndex - 1] = temp;
                currentLayerIndex--;
                UpdateLayerListUI();
                canvas.Invalidate();
            }
        }
        private void deleteLayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (textObjects.Count > 0)
            {
                CommitAllTexts();
            }
            if (layers.Count > 1) // Prevent deleting the last layer
            {
                SaveDrawingState(); // Save state before deleting
                layers.RemoveAt(currentLayerIndex);

                // Adjust current layer index
                if (currentLayerIndex >= layers.Count)
                {
                    currentLayerIndex = layers.Count - 1;
                }

                UpdateLayerListUI();
                canvas.Invalidate();
            }
            else
            {
                MessageBox.Show("Cannot delete the last layer.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnLine_Click(object sender, EventArgs e)
        {
            index = 9;
            currentShapeType=ShapeType.Line;
            selectionToolActive = false;
            selectionRectangle = Rectangle.Empty;
            freeFormPoints.Clear();
            canvas.Invalidate();
        }

        private void btnRect_Click(object sender, EventArgs e)
        {
            index = 9;
            currentShapeType = ShapeType.Rectangle;
            selectionToolActive = false;
            selectionRectangle = Rectangle.Empty;
            freeFormPoints.Clear();
            canvas.Invalidate();
        }

        private void btnEllipse_Click(object sender, EventArgs e)
        {
            index = 9;
            currentShapeType = ShapeType.Ellipse;
            selectionToolActive = false;
            selectionRectangle = Rectangle.Empty;
            freeFormPoints.Clear();
            canvas.Invalidate();
        }

        private void btnCurve_Click(object sender, EventArgs e)
        {
            index = 9;
            countBezier = 0;
            currentShapeType = ShapeType.Curve;
            selectionToolActive = false;
            selectionRectangle = Rectangle.Empty;
            freeFormPoints.Clear();
            canvas.Invalidate();
        }

        private void btnTriangle_Click(object sender, EventArgs e)
        {
            index = 9;
            currentShapeType = ShapeType.Triangle;
            selectionToolActive = false;
            selectionRectangle = Rectangle.Empty;
            freeFormPoints.Clear();
            canvas.Invalidate();
        }

        private void btnStar_Click(object sender, EventArgs e)
        {
            index = 9;
            currentShapeType = ShapeType.Star;
            selectionToolActive = false;
            selectionRectangle = Rectangle.Empty;
            freeFormPoints.Clear();
            canvas.Invalidate();
        }

        private void btnPentagon_Click(object sender, EventArgs e)
        {
            index = 9;
            currentShapeType = ShapeType.Pentagon;
            selectionToolActive = false;
            selectionRectangle = Rectangle.Empty;
            freeFormPoints.Clear();
            canvas.Invalidate();
        }

        private void btnDiamond_Click(object sender, EventArgs e)
        {
            index = 9;
            currentShapeType = ShapeType.Diamond;
            selectionToolActive = false;
            selectionRectangle = Rectangle.Empty;
            freeFormPoints.Clear();
            canvas.Invalidate();
        }

        private void btn4PointStar_Click(object sender, EventArgs e)
        {
            index = 9;
            currentShapeType = ShapeType.FourPointStar;
            selectionToolActive = false;
            selectionRectangle = Rectangle.Empty;
            freeFormPoints.Clear();
            canvas.Invalidate();
        }

        private void btnHeart_Click(object sender, EventArgs e)
        {
            index = 9;
            currentShapeType = ShapeType.Heart;
            selectionToolActive = false;
            selectionRectangle = Rectangle.Empty;
            freeFormPoints.Clear();
            canvas.Invalidate();
        }
        private void NewDocument()
        {
            // Ask user if they want to save current work
            if (HasUnsavedChanges())
            {
                DialogResult result = MessageBox.Show(
                    "Do you want to save changes?",
                    "Save Changes",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    SaveDocument();
                }
                else if (result == DialogResult.Cancel)
                {
                    return;
                }
            }

            // Show dialog for new canvas size
            using (var sizeDialog = new NewDocumentDialog())
            {
                if (sizeDialog.ShowDialog() == DialogResult.OK)
                {
                    // Get the new dimensions
                    int width = sizeDialog.CanvasWidth;
                    int height = sizeDialog.CanvasHeight;

                    // Clear undo/redo history
                    undoStack.Clear();
                    redoStack.Clear();

                    // Reset zoom
                    zoomFactor = 1.0f;

                    // Clear all layers
                    layers.Clear();

                    // Initialize new bitmap
                    bm = new Bitmap(width, height);
                    g = Graphics.FromImage(bm);
                    g.Clear(Color.White);
                    canvas.Image = bm;

                    // Create background layer (white)
                    Layer backgroundLayer = new Layer(width, height, "Background");
                    using (Graphics g = Graphics.FromImage(backgroundLayer.Bitmap))
                    {
                        g.Clear(Color.White);
                    }
                    layers.Add(backgroundLayer);

                    // Create and add the first drawing layer (transparent)
                    Layer layer1 = new Layer(width, height, "Layer 1");
                    layers.Add(layer1);

                    currentLayerIndex = 1;  // Set current layer to Layer 1

                    // Reset all tools and states
                    ResetToolStates();

                    // Update canvas size
                    canvas.Size = new Size(width, height);
                    UpdateCanvasZoom(Point.Empty);

                    // Reset file info
                    currentFilePath = string.Empty;
                    hasUnsavedChanges = false;
                    UpdateFormTitle();

                    // Update UI
                    UpdateLayerListUI();
                    CenterCanvas();
                    canvas.Invalidate();
                }
            }
        }
        private bool HasUnsavedChanges()
        {
            // Check if there are any changes in the undo stack
            return undoStack.Count > 0;
        }

        private void ResetToolStates()
        {
            // Reset all tool-related variables
            isStart = false;
            paint = false;
            selectionToolActive = false;
            isSelecting = false;
            isMoving = false;
            countBezier = 0;
            currentMode = SelectionMode.Rectangle;
            currentShapeType = ShapeType.Line;

            // Clear temporary data
            shapes.Clear();
            textObjects.Clear();
            selectionRectangle = Rectangle.Empty;
            freeFormPoints.Clear();
            copiedRegionBitmap = null;
        }
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewDocument();
        }
        private void SaveDocument()
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "PNG Image|*.png|JPEG Image|*.jpg;*.jpeg|BMP Image|*.bmp|All Files|*.*";
                saveDialog.Title = "Save Image";
                saveDialog.DefaultExt = "png";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Create a bitmap that combines all visible layers
                        Bitmap finalImage = new Bitmap(
                            CurrentLayer.Bitmap.Width,
                            CurrentLayer.Bitmap.Height,
                            PixelFormat.Format32bppArgb
                        );

                        using (Graphics g = Graphics.FromImage(finalImage))
                        {
                            g.Clear(Color.White); // Set white background

                            // Draw each visible layer from bottom to top
                            foreach (Layer layer in layers)
                            {
                                if (layer.Visible)
                                {
                                    // Create color matrix that includes the layer's opacity
                                    ColorMatrix matrix = new ColorMatrix();
                                    matrix.Matrix33 = layer.Opacity; // Set opacity

                                    ImageAttributes imageAttributes = new ImageAttributes();
                                    imageAttributes.SetColorMatrix(matrix);

                                    // Draw the layer with its opacity
                                    g.DrawImage(layer.Bitmap,
                                        new Rectangle(0, 0, layer.Bitmap.Width, layer.Bitmap.Height),
                                        0, 0, layer.Bitmap.Width, layer.Bitmap.Height,
                                        GraphicsUnit.Pixel,
                                        imageAttributes);
                                }
                            }
                        }

                        // Determine file format based on extension
                        ImageFormat format = ImageFormat.Png; // Default to PNG
                        string ext = Path.GetExtension(saveDialog.FileName).ToLower();
                        switch (ext)
                        {
                            case ".jpg":
                            case ".jpeg":
                                format = ImageFormat.Jpeg;
                                break;
                            case ".bmp":
                                format = ImageFormat.Bmp;
                                break;
                        }

                        // Save the final image
                        finalImage.Save(saveDialog.FileName, format);

                        // Update the current file path
                        currentFilePath = saveDialog.FileName;

                        // Clear the undo stack since we've saved
                        hasUnsavedChanges = false;

                        // Update the form title to show the file name
                        UpdateFormTitle();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                            $"Error saving file: {ex.Message}",
                            "Save Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                    }
                }
            }
        }
        private void UpdateFormTitle()
        {
            string fileName = string.IsNullOrEmpty(currentFilePath)
                ? "Untitled"
                : Path.GetFileName(currentFilePath);

            this.Text = $"{fileName} - Paint Application";
            if (hasUnsavedChanges)
                this.Text += "*";
        }
        private void MarkAsUnsaved()
        {
            if (!hasUnsavedChanges)
            {
                hasUnsavedChanges = true;
                UpdateFormTitle();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentFilePath))
            {
                SaveDocument(); // If no current file, show Save As dialog
            }
            else
            {
                try
                {
                    // Save directly to current file
                    Bitmap finalImage = new Bitmap(
                        CurrentLayer.Bitmap.Width,
                        CurrentLayer.Bitmap.Height,
                        PixelFormat.Format32bppArgb
                    );

                    using (Graphics g = Graphics.FromImage(finalImage))
                    {
                        g.Clear(Color.White);
                        foreach (Layer layer in layers)
                        {
                            if (layer.Visible)
                            {
                                ColorMatrix matrix = new ColorMatrix();
                                matrix.Matrix33 = layer.Opacity;
                                ImageAttributes imageAttributes = new ImageAttributes();
                                imageAttributes.SetColorMatrix(matrix);

                                g.DrawImage(layer.Bitmap,
                                    new Rectangle(0, 0, layer.Bitmap.Width, layer.Bitmap.Height),
                                    0, 0, layer.Bitmap.Width, layer.Bitmap.Height,
                                    GraphicsUnit.Pixel,
                                    imageAttributes);
                            }
                        }
                    }

                    // Determine format from existing file extension
                    ImageFormat format = ImageFormat.Png;
                    string ext = Path.GetExtension(currentFilePath).ToLower();
                    switch (ext)
                    {
                        case ".jpg":
                        case ".jpeg":
                            format = ImageFormat.Jpeg;
                            break;
                        case ".bmp":
                            format = ImageFormat.Bmp;
                            break;
                    }

                    finalImage.Save(currentFilePath, format);
                    hasUnsavedChanges = false;
                    UpdateFormTitle();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Error saving file: {ex.Message}",
                        "Save Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
        }

        private void Fill(Bitmap bm, int x, int y, Color new_clr)
        {
            bm = CurrentLayer.Bitmap;
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
