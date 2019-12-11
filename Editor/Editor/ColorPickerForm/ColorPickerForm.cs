using System;
using System.Windows.Forms;
using Color = Microsoft.Xna.Framework.Color;
namespace Editor
{
    public partial class ColorPickerForm : Form
    {
        private bool leftMouseClicked = false;
        public ColorMap colorMap;
        public Color color;

        public TrackBar trackBar;
        public delegate void OnProgressChanged(float progress);
        public OnProgressChanged HueChanged;


        public ColorPickerForm()
        {
            InitializeComponent();
            colorMap = colorMap1;
            colorMap1.ColorPreview = colorPreview;

            //colorMapTrackBar1.colorMapPictureBox = colorMap1.ColorPreview;
            HueChanged += colorMap1.SetHue;
            HueChanged += colorMap1.UpdateColorMap;

            colorMapHue1.MouseDown += colorMap1.EnableRedrawing;
            colorMapHue1.MouseUp += colorMap1.DisableRedrawing;
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
        private void ColorMapHue_MouseDown(object sender, MouseEventArgs e)
        {
            leftMouseClicked = true;
        }

        private void ColorMapHue_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftMouseClicked)
            {
                int newValue = Extensions.Clamp(e.Location.X, 0, colorMapHue1.Width);
                colorMapHue1.huePositionX = newValue;
                HueChanged(newValue);
                colorMapHue1.Invalidate();
            }
        }

        private void ColorMapHue_MouseUp(object sender, MouseEventArgs e)
        {
            leftMouseClicked = false;
        }

        private void TrackBar1_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void TrackBar1_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void TrackBar1_MouseUp(object sender, MouseEventArgs e)
        {

        }
    }
}