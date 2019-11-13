using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Drawing.Design;
namespace Scripts
{
    public class Renderer : Component, Engine.IColorable
    {
        public virtual void Draw(SpriteBatch batch) { }
        [System.ComponentModel.Editor(typeof(Editor.ColorPickerEditor), typeof(UITypeEditor))]
        [ShowInEditor] public Color Color { get; set; } = Color.White;
        [ShowInEditor] public bool Fill { get; set; } = false;
        [System.ComponentModel.Editor(typeof(Editor.EffectEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [System.Xml.Serialization.XmlIgnore] [ShowInEditor] public Effect effect { get; set; }
    }
}
