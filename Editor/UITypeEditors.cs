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
using Engine.Editor;
namespace Engine.Editor
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
        public override void PaintValue(PaintValueEventArgs e)
        {
            var rect = e.Bounds;
            rect.Inflate(1, 1);
            /*ControlPaint.DrawCheckBox(e.Graphics, rect, ButtonState.Flat |
                (((bool)e.Value) ? ButtonState.Checked : ButtonState.Normal));*/
            //Brush brush = new SolidBrush(Color.White);
            Brush brush = new SolidBrush((e.Context.Instance as RendererComponentNode).Color.ToOtherColor());
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
                    (context.Instance as RendererComponentNode).Color = colorPickerForm.color;
                    value = colorPickerForm.color;
                };
                colorPickerForm.ShowDialog();

            }


            return value;
        }
    }
}
