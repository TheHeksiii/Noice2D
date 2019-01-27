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
    public partial class Slider
    {
        public TrackBar trackBar;
        public delegate void OnProgressChanged(float progress);
        public OnProgressChanged ProgressChanged;

        public void ValueChanged()
        {
            ProgressChanged(trackBar.Value);
        }
    }
}
