using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Editor
{
    class ShapeRendererComponentNode : RendererComponentNode
    {
        public override string Name { get; set; } = "Shape Renderer";

        public ShapeRendererComponentNode(GameObjectNode gameObjectNode, Component component) : base(gameObjectNode, component)
        {
        }

        private ShapeRenderer ShapeRenderer;
    }
}
