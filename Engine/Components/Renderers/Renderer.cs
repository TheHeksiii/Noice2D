using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Diagnostics;
using System.Drawing.Design;
using Engine;
namespace Scripts
{
    public class Renderer : Component
    {
        public virtual void Draw(SpriteBatch batch) { }
        [System.ComponentModel.Editor(typeof(Editor.ColorPickerEditor), typeof(UITypeEditor))]
        [ShowInEditor] public Color Color { get; set; } = Color.White;
        [ShowInEditor] public bool Fill { get; set; } = false;
        [System.ComponentModel.Editor(typeof(Editor.EffectEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [System.Xml.Serialization.XmlIgnore] [ShowInEditor] public Effect effect { get; set; }
    }
}
