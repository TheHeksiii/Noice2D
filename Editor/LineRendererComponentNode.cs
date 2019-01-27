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
    class LineRendererComponentNode : RendererComponentNode
    {
        public override string Name { get; set; } = "Line Renderer";

        public LineRendererComponentNode(GameObjectNode gameObjectNode, Component component) : base(gameObjectNode, component)
        {
            LineRenderer = Component as LineRenderer;

            if (LineRenderer != null)
            {
                Color = LineRenderer.color;
                StrokeSize = LineRenderer.StrokeSize;
            }
        }

        private LineRenderer LineRenderer;
        public float StrokeSize { get { return LineRenderer.StrokeSize; }set { LineRenderer.StrokeSize = value; } }
    }
}
