using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Editor
{
    class RigidbodyComponentNode : ComponentNode
    {
        public override string Name { get; set; } = "Rigidbody";

        public RigidbodyComponentNode(GameObjectNode gameObjectNode, Component component) : base(gameObjectNode, component)
        {
            Rigidbody = component as Rigidbody;
        }
        private Rigidbody Rigidbody;

        [Editor(typeof(BoolEditor), typeof(UITypeEditor))]
        public bool IsStatic { get { return Rigidbody.isStatic; } set { Rigidbody.isStatic = value; } }
        public Vector2 Velocity { get { return Rigidbody.velocity; } set { Rigidbody.velocity = value; } }
        public float VelocityDrag { get { return Rigidbody.velocityDrag; } set { Rigidbody.velocityDrag = value; } }
        public float Bounciness { get { return Rigidbody.bounciness; } set { Rigidbody.bounciness = value; } }
        public float AngularVelocity { get { return Rigidbody.angularVelocity; } set { Rigidbody.angularVelocity = value; } }
        public float AngularDrag { get { return Rigidbody.angularDrag; } set { Rigidbody.angularDrag = value; } }
        public float Friction { get { return Rigidbody.friction; } set { Rigidbody.friction = value; } }
        public float Mass { get { return Rigidbody.mass; } set { Rigidbody.mass = value; } }

    }
}
