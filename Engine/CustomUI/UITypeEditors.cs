using Engine;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Design;
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



            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                  ColorPickerEditor_EditValue?.Invoke(context, provider, value);

                  return value;
            }
            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                  return UITypeEditorEditStyle.DropDown;

            }
      }
      public class BoolEditor : UITypeEditor
      {
            public static Action<PaintValueEventArgs> BoolEditor_Paint;
            Pen pen = new Pen(Color.LightCoral);

 

            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                  IWindowsFormsEditorService editorService = null;
                  if (provider != null)
                  {
                        editorService = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
                  }

                  if (editorService != null)
                  {
                        Panel panel = new Panel();

                        NumericUpDown udControl1 = new NumericUpDown();
                        udControl1.DecimalPlaces = 5;
                        udControl1.TextAlign =HorizontalAlignment.Center;
                        udControl1.Width = 150;
                        udControl1.Dock = DockStyle.Left;

                        NumericUpDown udControl2 = new NumericUpDown();
                        udControl2.DecimalPlaces = 5;
                        udControl2.TextAlign = HorizontalAlignment.Center;
                        udControl2.Width = 150;
                        udControl2.Dock = DockStyle.Left;

                        panel.Size = new Size(300, udControl1.Height);

                        panel.Controls.Add(udControl1);
                        panel.Controls.Add(udControl2);


                        editorService.DropDownControl(panel);
                  }

                  return value;
            }
            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                  return UITypeEditorEditStyle.DropDown;

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

