using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MonoGame.Extended;
using Color = Microsoft.Xna.Framework.Color;
using Engine;
namespace Editor
{
    public partial class ColorPickerForm : Form
    {
        public ColorMap colorMap;
        public Color color;
        Slider slider = new Slider();
        public ColorPickerForm()
        {
            InitializeComponent();
            colorMap = colorMap1;
            colorMap1.ColorPreview = colorPreview;

            //colorMapTrackBar1.colorMapPictureBox = colorMap1.ColorPreview;
            slider.trackBar = trackBar1;
            slider.ProgressChanged += colorMap1.SetHue;
            slider.ProgressChanged += colorMap1.UpdateColorMap;
            slider.trackBar.MouseDown += colorMap1.EnableRedrawing;
            slider.trackBar.MouseUp += colorMap1.DisableRedrawing;
            colorMap1.OnColorChanged += UpdateColor;
        }
        void UpdateColor()
        {
            color = colorPreview.BackColor.ToOtherColor();
        }
        protected override void OnClosed(EventArgs e)
        {
            colorMap1.running = false;
            base.OnClosed(e);
        }
        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            slider.ValueChanged();
        }
    }
}
