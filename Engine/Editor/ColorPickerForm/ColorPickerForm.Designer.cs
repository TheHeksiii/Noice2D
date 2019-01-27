namespace Editor
{
    partial class ColorPickerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColorPickerForm));
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.colorPreview = new System.Windows.Forms.PictureBox();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.colorMap1 = new Editor.ColorMap();
            this.colorMapHue1 = new Editor.ColorMapHue();
            ((System.ComponentModel.ISupportInitialize)(this.colorPreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.colorMap1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.colorMapHue1)).BeginInit();
            this.SuspendLayout();
            // 
            // colorDialog1
            // 
            this.colorDialog1.FullOpen = true;
            // 
            // colorPreview
            // 
            this.colorPreview.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.colorPreview.Dock = System.Windows.Forms.DockStyle.Top;
            this.colorPreview.Location = new System.Drawing.Point(0, 0);
            this.colorPreview.Name = "colorPreview";
            this.colorPreview.Size = new System.Drawing.Size(296, 50);
            this.colorPreview.TabIndex = 3;
            this.colorPreview.TabStop = false;
            // 
            // trackBar1
            // 
            this.trackBar1.BackColor = System.Drawing.SystemColors.Control;
            this.trackBar1.LargeChange = 20;
            this.trackBar1.Location = new System.Drawing.Point(0, 186);
            this.trackBar1.Maximum = 360;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.trackBar1.Size = new System.Drawing.Size(296, 45);
            this.trackBar1.SmallChange = 20;
            this.trackBar1.TabIndex = 5;
            this.trackBar1.TabStop = false;
            this.trackBar1.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar1.Value = 20;
            this.trackBar1.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
            // 
            // colorMap1
            // 
            this.colorMap1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.colorMap1.Dock = System.Windows.Forms.DockStyle.Top;
            this.colorMap1.Location = new System.Drawing.Point(0, 50);
            this.colorMap1.Name = "colorMap1";
            this.colorMap1.Size = new System.Drawing.Size(296, 136);
            this.colorMap1.TabIndex = 4;
            this.colorMap1.TabStop = false;
            // 
            // colorMapHue1
            // 
            this.colorMapHue1.Image = ((System.Drawing.Image)(resources.GetObject("colorMapHue1.Image")));
            this.colorMapHue1.Location = new System.Drawing.Point(8, 216);
            this.colorMapHue1.Name = "colorMapHue1";
            this.colorMapHue1.Size = new System.Drawing.Size(281, 54);
            this.colorMapHue1.TabIndex = 6;
            this.colorMapHue1.TabStop = false;
            // 
            // ColorPickerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(296, 377);
            this.Controls.Add(this.colorMap1);
            this.Controls.Add(this.colorMapHue1);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.colorPreview);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ColorPickerForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "ColorPickerForm";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.colorPreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.colorMap1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.colorMapHue1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ColorDialog colorDialog1;
        private Slider slider1;
        public System.Windows.Forms.PictureBox colorPreview;
        private ColorMap colorMap1;
        private System.Windows.Forms.TrackBar trackBar1;
        private ColorMapHue colorMapHue1;
    }
}