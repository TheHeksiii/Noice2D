using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public static class CustomEditorsActions
    {
        public static Action<System.Drawing.Design.PaintValueEventArgs> BoolEditor_Paint;
        public static Action<ITypeDescriptorContext, IServiceProvider, object> ColorPickerEditor_EditValue;
        public static Action<ITypeDescriptorContext, IServiceProvider, object> TextureEditor_EditValue;
        public static Action<ITypeDescriptorContext, IServiceProvider, object> EffectEditor_EditValue;
        
    }
}
