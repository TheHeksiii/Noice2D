using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Threading.Tasks;
using System.Windows.Forms;
using Color = System.Drawing.Color;
using Point = System.Drawing.Point;
using ButtonState = System.Windows.Forms.ButtonState;
using System.Drawing;
using Editor;
using Engine;
using System.IO;

namespace Editor
{
    public class BoolEditor : UITypeEditor
    {
        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override void PaintValue(PaintValueEventArgs e)
        {
            var rect = e.Bounds;
            rect.Inflate(1, 1);
            ControlPaint.DrawCheckBox(e.Graphics, rect, ButtonState.Flat |
                (((bool)e.Value) ? ButtonState.Checked : ButtonState.Normal));
            /*ControlPaint.DrawButton(e.Graphics, rect, ButtonState.Flat |
     (((bool)e.Value) ? ButtonState.Checked : ButtonState.Normal));*/
        }
    }
    public class MethodEditor : UITypeEditor
    {
        Pen pen = new Pen(Color.LightCoral);
        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override void PaintValue(PaintValueEventArgs e)
        {
            //var rect = e.Bounds;
            //rect.Offset(new Point(20, 0));
            //e.Graphics.DrawRectangle(pen, rect);
            /*ControlPaint.DrawRadioButton(e.Graphics, rect, ButtonState.Flat |
                (((bool)e.Value) ? ButtonState.Checked : ButtonState.Normal));*/
        }
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if ((value as Delegate) == null)
            {
                return value;
            }
            (value as Delegate).DynamicInvoke();
            return value;
        }
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;

        }
    }
    public class ColorPickerEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }
        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        private Microsoft.Xna.Framework.Color GetColorFromClass(object instance)
        {
            if (instance is Scripts.Renderer)
            {
                return ((Scripts.Renderer)instance).Color;
            }
            else if (instance is Camera)
            {
                return ((Camera)instance).BackgroundColor;
            }
            return Microsoft.Xna.Framework.Color.White;
        }
        private void SetColorInClass(object instance, Microsoft.Xna.Framework.Color color)
        {
            if (instance is Scripts.Renderer)
            {
                ((Scripts.Renderer)instance).Color = color;
            }
            else if (instance is Camera)
            {
                ((Camera)instance).BackgroundColor = color;
            }
        }
        public override void PaintValue(PaintValueEventArgs e)
        {
            var rect = e.Bounds;
            rect.Inflate(1, 1);
            /*ControlPaint.DrawCheckBox(e.Graphics, rect, ButtonState.Flat |
                (((bool)e.Value) ? ButtonState.Checked : ButtonState.Normal));*/
            //Brush brush = new SolidBrush(Color.White);
            Brush brush = new SolidBrush(GetColorFromClass(e.Context.Instance).ToOtherColor());
            RectangleF rectF = new RectangleF(rect.Location, rect.Size);
            e.Graphics.FillRectangle(brush, rectF);
        }
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (context == null || context.Instance == null || provider == null)
            {
                return base.EditValue(context, provider, value);
            }
            using (ColorPickerForm colorPickerForm = new ColorPickerForm())
            {
                colorPickerForm.StartPosition = FormStartPosition.Manual;
                colorPickerForm.Location = new Point((int)(Cursor.Position.X - colorPickerForm.Width / 2), Cursor.Position.Y);
                colorPickerForm.colorMap.OnColorChanged += delegate
                {
                    SetColorInClass(context.Instance, colorPickerForm.color);
                    value = colorPickerForm.color;
                };
                colorPickerForm.ShowDialog();

            }
            return value;
        }
    }
    public class TextureEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (context == null || context.Instance == null || provider == null)
            {
                return base.EditValue(context, provider, value);
            }
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                DialogResult dialogResult = openFileDialog.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    string assetsPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "AssetsInUse");
                    Directory.CreateDirectory(assetsPath);

                    string newFilePath = Path.Combine(assetsPath, openFileDialog.SafeFileName);

                    File.Copy(openFileDialog.FileName, newFilePath, overwrite: true);


                    //(context.Instance as Scripts.ImageRenderer).texturePath = Path.Combine("AssetsInUse", openFileDialog.SafeFileName);

                    var scene = EditorSceneView.GetInstance();

                    System.IO.Stream stream = TitleContainer.OpenStream(Path.Combine("AssetsInUse", openFileDialog.SafeFileName));
                    value = Texture2D.FromStream(scene.GraphicsDevice, stream);
                    stream.Close();
                }
            }
            return value;
        }
    }
    public class EffectEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (context == null || context.Instance == null || provider == null)
            {
                return base.EditValue(context, provider, value);
            }
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                DialogResult dialogResult = openFileDialog.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {


                    //(context.Instance as Scripts.Renderer).effect = EditorSceneView.GetInstance().Content.Load<Effect>(openFileDialog.FileName);
                    value = EditorSceneView.GetInstance().Content.Load<Effect>(openFileDialog.FileName.Replace(".xnb", ""));
                }
            }
            return value;
        }
    }
}

