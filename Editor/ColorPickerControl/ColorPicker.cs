using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Color = Microsoft.Xna.Framework.Color;

namespace Editor
{
      public partial class ColorPicker : UserControl
      {
            private bool leftMouseClicked = false;
            public Color color;

            public TrackBar trackBar;
            public delegate void OnProgressChanged(float progress);
            public OnProgressChanged HueChanged;
            public ColorMap ColorMap { get { return colorMap; } }

            public ColorPicker()
            {
                  InitializeComponent();

                  colorMap.ColorPreview = colorPreview;
                  //colorMap1.ColorPreview = colorPreview;

                  //colorMapTrackBar1.colorMapPictureBox = colorMap1.ColorPreview;
                  HueChanged += colorMap.SetHue;
                  HueChanged += colorMap.UpdateColorMap;

                  colorMapHue1.MouseDown += colorMap.EnableRedrawing;
                  colorMapHue1.MouseUp += colorMap.DisableRedrawing;
                  colorMap.OnColorChanged += UpdateColor;
            }
            void UpdateColor()
            {
                  color = colorPreview.BackColor.ToOtherColor();
            }
            protected override void OnControlRemoved(ControlEventArgs e)
            {
                  colorMap.running = false;

                  base.OnControlRemoved(e);
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
