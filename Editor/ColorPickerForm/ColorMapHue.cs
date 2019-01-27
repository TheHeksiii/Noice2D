using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Engine.Editor
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

        void CreateGradient()
        {
            Point ScaleSize = new Point(296, 136);
            Size = new Size(ScaleSize.X, ScaleSize.Y);

            gradient = new Bitmap(ScaleSize.X, ScaleSize.Y);
            Graphics graphics = Graphics.FromImage(gradient);

            for (int x = 0; x <= 360; x++)
            {
                Color rgb = Extensions.ColorFromHSV(x,1,1);

                graphics.FillRectangle(new SolidBrush(Color.FromArgb(rgb.R, rgb.G, rgb.B)), ScaleSize.X / 360f * x, 0, 1,ScaleSize.Y);

            }
            /*Color rgb = Extensions.ColorFromHSV(slider1.progress*360, 1, 1);
            Console.WriteLine(rgb);
            graphics.FillRectangle(new SolidBrush(Color.FromArgb(rgb.R, rgb.G, rgb.B)), 0,0, 100,50);*/

            Image = gradient;
        }
    }
}
