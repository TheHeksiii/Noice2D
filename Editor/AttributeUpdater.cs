using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor
{
	class AttributeUpdater
	{
		public static void Update()
		{
			System.ComponentModel.TypeDescriptor.AddAttributes(typeof(Vector2), new Attribute[] { new System.ComponentModel.EditorAttribute(typeof(Engine.UITypeEditors.Vector2Editor), typeof(System.Drawing.Design.UITypeEditor)) });
			//System.ComponentModel.TypeDescriptor.AddAttributes(typeof(Microsoft.Xna.Framework.Vector2), new Attribute[] { new System.ComponentModel.EditorAttribute(typeof(Engine.UITypeEditors.BoolEditor), typeof(System.Drawing.Design.UITypeEditor)) });
			System.ComponentModel.TypeDescriptor.AddAttributes(typeof(Action), new Attribute[] { new System.ComponentModel.EditorAttribute(typeof(Engine.UITypeEditors.MethodEditor), typeof(System.Drawing.Design.UITypeEditor)) });
			System.ComponentModel.TypeDescriptor.AddAttributes(typeof(Microsoft.Xna.Framework.Color), new Attribute[] { new System.ComponentModel.EditorAttribute(typeof(Engine.UITypeEditors.ColorPickerEditor), typeof(System.Drawing.Design.UITypeEditor)) });
			System.ComponentModel.TypeDescriptor.AddAttributes(typeof(Microsoft.Xna.Framework.Graphics.Texture2D), new Attribute[] { new System.ComponentModel.EditorAttribute(typeof(Engine.UITypeEditors.TextureEditor), typeof(System.Drawing.Design.UITypeEditor)) });
			System.ComponentModel.TypeDescriptor.AddAttributes(typeof(Microsoft.Xna.Framework.Graphics.Effect), new Attribute[] { new System.ComponentModel.EditorAttribute(typeof(Engine.UITypeEditors.EffectEditor), typeof(System.Drawing.Design.UITypeEditor)) });
		}
	}
}
