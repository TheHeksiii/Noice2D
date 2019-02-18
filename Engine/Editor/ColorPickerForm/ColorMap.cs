using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Point = System.Drawing.Point;
using Color = System.Drawing.Color;
using Rectangle = System.Drawing.Rectangle;
using System.Threading;
using System;
using System.Diagnostics;

namespace Editor
{
    public partial class ColorMap : PictureBox
    {
        public delegate void ColorChanged();
        public ColorChanged OnColorChanged;
        public bool running = true;

        // Synchronization objects
        private readonly object _trackbarProgressLock = new object();
        private readonly object _redrawGradientFlagLock = new object();
        private readonly object _bitmapLock = new object();
        bool redrawGradient = false;
        public PictureBox ColorPreview;
        Bitmap bitmap;
        /// <summary>
        /// LockBitmap is only used in 
        /// </summary>
        LockBitmap lockBitmap;
        SolidBrush brush = new SolidBrush(Color.White);

        private Point dotPosition = new Point(0, 0);
        private Color penColor;

        float trackbarProgress = 0;

        private void GenerateColorMap(float hue = 0)
        {
            lock (_bitmapLock)
            {
                lockBitmap.LockBits();

                Parallel.For(0, Size.Height, new Action<int>((y) =>
                {
                    Parallel.For(0, Size.Width, new Action<int>((x) =>
                    {
                        Color rgb = Extensions.ColorFromHSV(hue, (float)x / Size.Width, (float)y / Size.Height);
                        lockBitmap.SetPixel(x, y, rgb);
                    }));
                }));
                lockBitmap.UnlockBits();
            }
            lock (_bitmapLock)
            {
                bitmap = lockBitmap.source;
            }
        }
        public ColorMap()
        {
            InitializeComponent();
            Size = base.Size;

            bitmap = new Bitmap(Size.Width, Size.Height);


            lockBitmap = new LockBitmap(bitmap);

            GenerateColorMap(0);

            Task.Factory.StartNew(ColorMapUpdater);
        }

        void ColorMapUpdater()
        {
            float myProgress;
            bool tempRedrawGradient;
            while (running)
            {
                lock (_redrawGradientFlagLock)
                {
                    tempRedrawGradient = redrawGradient;
                }
                if (tempRedrawGradient)
                {
                    lock (_trackbarProgressLock)
                    {
                        myProgress = trackbarProgress;
                    }
                    GenerateColorMap(myProgress);
                    Thread.Sleep(5);
                }
                else
                {
                    Thread.Sleep(20);
                }
            }
        }
        public void SetHue(float progress)
        {
            lock (_trackbarProgressLock)
            {
                trackbarProgress = progress;
            }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            lock (_bitmapLock)
            {
                pe.Graphics.DrawImage(bitmap, new Point(0, 0));
            }
            Pen myPen = new Pen(penColor);
            myPen.Width = 2;
            pe.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            pe.Graphics.DrawEllipse(myPen, new Rectangle(new Point(dotPosition.X - 5, dotPosition.Y - 5), new Size(10, 10)));

        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            OnMouseMove(e);
            base.OnMouseClick(e);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var x = Extensions.Clamp(e.Location.X, 0, base.Size.Width - 1);
                var y = Extensions.Clamp(e.Location.Y, 0, base.Size.Height - 1);
                dotPosition = new Point(x, y);
                UpdateColorMap();
                Invalidate();
            }
            base.OnMouseMove(e);
        }
        public void EnableRedrawing(object sender, MouseEventArgs e)
        {
            lock (_redrawGradientFlagLock)
            {
                redrawGradient = true;
            }
        }
        public void DisableRedrawing(object sender, MouseEventArgs e)
        {
            lock (_redrawGradientFlagLock)
            {
                redrawGradient = false;
            }
        }

        public void UpdateColorMap(float progress = 0)
        {
            lock (_bitmapLock)
            {
                ColorPreview.BackColor = bitmap.GetPixel(dotPosition.X, dotPosition.Y);
            }
            penColor = Color.FromArgb(255 - ColorPreview.BackColor.R, 255 - ColorPreview.BackColor.G, 255 - ColorPreview.BackColor.B);
            OnColorChanged();
            Invalidate();
        }
    }
}
