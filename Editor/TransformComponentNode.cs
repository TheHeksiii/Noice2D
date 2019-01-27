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
    class TransformComponentNode : ComponentNode
    {
        public override string Name { get; set; } = "Transform";
        public TransformComponentNode(GameObjectNode gameObjectNode, Component component) : base(gameObjectNode, component)
        {
            Transform = component as Transform;
            if (Transform!=null)
            {
                Transform transformComponent = component as Transform;
                Position = transformComponent.Position;
                Rotation = transformComponent.Rotation;
                Scale = transformComponent.Scale;
            }
        }
        public Transform Transform;


        public Vector2 Position { get { return Transform.Position; }set { Transform.Position = value; } }
        public float Rotation { get { return Transform.Rotation; } set { Transform.Rotation = value; } }
        public Vector2 Scale { get { return Transform.Scale; } set { Transform.Scale = value; } }


    }
}
