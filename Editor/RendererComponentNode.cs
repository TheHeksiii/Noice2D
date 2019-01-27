#region Usings
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using Point = System.Drawing.Point;
#endregion
namespace Engine.Editor
{
    class RendererComponentNode : ComponentNode
    {
        public override string Name { get; set; } = "Renderer";

        public RendererComponentNode(GameObjectNode gameObjectNode, Component component) : base(gameObjectNode, component)
        {
            Renderer = Component as Renderer;
            if (Renderer != null)
            {
                Color = Renderer.color;
            }
        }

        private Renderer Renderer;
        /*[Editor(typeof(ColorPickerEditor), typeof(UITypeEditor))]
        public Color Color { get; set; }*/
        [Editor(typeof(ColorPickerEditor), typeof(UITypeEditor))]
        public Color Color { get { return Renderer.color; } set { Renderer.color = value; } }
    }
}
