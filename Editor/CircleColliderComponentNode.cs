using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Editor
{
    class CircleColliderComponentNode : ColliderComponentNode
    {
        public override string Name { get; set; } = "Circle Collider";

        private CircleCollider CircleCollider;

        public CircleColliderComponentNode(GameObjectNode gameObjectNode, Component component) : base(gameObjectNode, component)
        {
            CircleCollider = component as CircleCollider;
        }

        public float Radius { get { return CircleCollider.Radius; } set { CircleCollider.Radius = value; } }
    }
}
