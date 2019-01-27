using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Editor
{
    class ColliderComponentNode : ComponentNode
    {
        public override string Name { get; set; } = "Collider";
        private Collider collider;
        public ColliderComponentNode(GameObjectNode gameObjectNode, Component component) : base(gameObjectNode, component)
        {
            collider = component as Collider;
        }
    }
}
