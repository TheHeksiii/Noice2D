using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Editor
{
    public partial class ColorMapHue : PictureBox
    {
        public ColorMapHue()
        {
            InitializeComponent();
            Image = null;
            CreateGradient();
        }
        Bitmap gradient;
        public int huePositionX = 0;
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            huePositionX = Extensions.Clamp(huePositionX, 0, gradient.Width - 1);
            Color gradientColor = gradient.GetPixel(huePositionX, 0);
            Pen pen = new Pen(Color.FromArgb(255 - gradientColor.R, 255 - gradientColor.G, 255 - gradientColor.B), 1);
            pe.Graphics.DrawRectangle(pen, new Rectangle(new Point(huePositionX - 3, -1), new Size(6, this.Height + 1)));
        }
        void CreateGradient()
        {
            Point ScaleSize = new Point(296, 136);
            Size = new Size(ScaleSize.X, ScaleSize.Y);

            gradient = new Bitmap(ScaleSize.X, ScaleSize.Y);
            Graphics graphics = Graphics.FromImage(gradient);

            SolidBrush brush = new SolidBrush(Color.Empty);

            for (int x = 0; x <= 360; x++)
            {
                Color rgb = Extensions.ColorFromHSV(x, 1, 1);
                brush.Color = Color.FromArgb(rgb.R, rgb.G, rgb.B);
                graphics.FillRectangle(brush, ScaleSize.X / 360f * x, 0, 1, ScaleSize.Y);
            }

            /*Color rgb = Extensions.ColorFromHSV(slider1.progress*360, 1, 1);
            Console.WriteLine(rgb);
            graphics.FillRectangle(new SolidBrush(Color.FromArgb(rgb.R, rgb.G, rgb.B)), 0,0, 100,50);*/

            Image = gradient;
        }
    }
}
