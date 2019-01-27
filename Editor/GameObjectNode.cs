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
namespace Engine.Editor
{

    /// <summary>
    /// Bridge between Editor and ComponentItems
    /// </summary>
    public partial class GameObjectNode
    {
        /// <summary>
        /// Name of GameObject
        /// </summary>
        public string Name { get; set; }
        public int ID { get { return GameObject.ID; }set { GameObject.ID = value; } }


        public GameObject GameObject;
        public List<ComponentNode> ComponentNodes { get; set; } = new List<ComponentNode>();

        public GameObjectNode(GameObject gameObject)
        {
            GameObject = gameObject;

            ComponentNodes.Add(new TransformComponentNode(this, gameObject.transform));

            for (int i = 0; i < gameObject.components.Count; i++)
            {
                if (gameObject.components[i] is LineRenderer)
                {
                    ComponentNodes.Add(new LineRendererComponentNode(this, gameObject.components[i]));
                    continue;
                }
                if (gameObject.components[i] is ShapeRenderer)
                {
                    ComponentNodes.Add(new ShapeRendererComponentNode(this, gameObject.components[i]));
                    continue;
                }
                if (gameObject.components[i] is Renderer)
                {
                    ComponentNodes.Add(new RendererComponentNode(this, gameObject.components[i]));
                    continue;
                }
                if (gameObject.components[i] is Rigidbody)
                {
                    ComponentNodes.Add(new RigidbodyComponentNode(this, gameObject.components[i]));
                    continue;
                }
                /*if (gameObject.components[i] is CircleCollider)
                {
                    Components.Add(new CircleColliderComponentNode(this, gameObject.components[i]));
                    continue;
                }*/

            }
        }

        public void PullPropertiesFromScene()
        {
            
        }
    }

}