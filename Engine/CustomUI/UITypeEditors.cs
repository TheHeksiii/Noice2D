using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Color = System.Drawing.Color;
using Point = System.Drawing.Point;

namespace Engine.UITypeEditors
{
      public class EffectEditor : UITypeEditor
      {
            public static Action<ITypeDescriptorContext, IServiceProvider, object> EffectEditor_EditValue;

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

                  EffectEditor_EditValue?.Invoke(context, provider, value);
                  return value;
            }
      }
      public class TextureEditor : UITypeEditor
      {
            public static Action<ITypeDescriptorContext, IServiceProvider, object> TextureEditor_EditValue;

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
                  TextureEditor_EditValue?.Invoke(context, provider, value);
                  return value;
            }
      }
      public class ColorPickerEditor : UITypeEditor
      {
            public static Action<ITypeDescriptorContext, IServiceProvider, object> ColorPickerEditor_EditValue;
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
                  Type sourceType = instance.GetType();
                  PropertyInfo colorField = sourceType.GetProperty("color");
                  if (colorField == null)
                  {
                        colorField = sourceType.GetProperty("Color");
                  }
                  return (Microsoft.Xna.Framework.Color)colorField?.GetValue(instance);
            }


            public override void PaintValue(PaintValueEventArgs e)
            {
                  var rect = e.Bounds;
                  rect.Inflate(1, 1);

                  //Brush brush = new SolidBrush(GetColorFromClass(e.Context.Instance).ToOtherColor());
                  //RectangleF rectF = new RectangleF(rect.Location, rect.Size);
                  //e.Graphics.FillRectangle(brush, rectF);
            }
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                  if (context == null || context.Instance == null || provider == null)
                  {
                        return base.EditValue(context, provider, value);
                  }
                  ColorPickerEditor_EditValue?.Invoke(context, provider, value);

                  return value;
            }
      }
      public class BoolEditor : UITypeEditor
      {
            public static Action<PaintValueEventArgs> BoolEditor_Paint;
            Pen pen = new Pen(Color.LightCoral);

            public override bool GetPaintValueSupported(ITypeDescriptorContext context)
            {
                  return true;
            }
            public override void PaintValue(PaintValueEventArgs e)
            {

                  var rect = e.Bounds;

                  //e.Graphics.DrawRectangle(pen, rect);
                  ControlPaint.DrawRadioButton(e.Graphics, rect, ButtonState.Flat | ButtonState.Checked);
            }
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {

                  return value;
                  return value;
            }
            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                  return UITypeEditorEditStyle.None;

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




}

