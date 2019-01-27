using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Editor
{
    public class ListConverter : CollectionConverter
    {
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            IList list = value as IList;
            if (list == null || list.Count == 0)
                return base.GetProperties(context, value, attributes);

            var items = new PropertyDescriptorCollection(null);
            for (int i = 0; i < list.Count; i++)
            {
                object item = list[i];
                items.Add(new ExpandableCollectionPropertyDescriptor(list, i));
            }
            return items;
        }
    }
}
