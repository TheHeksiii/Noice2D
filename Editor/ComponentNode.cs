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
    /// <summary>
    /// This class serves as bridge between Editor and Component in Scene.
    /// In editor, it's placed as Tag of Component nodes in Component list.
    /// When component is selected, we show properties of this class in InspectorWindow
    /// </summary>
    public class ComponentNode
    {
        public ComponentNode(GameObjectNode gameObjectNode,Component component)
        {
            GameObjectNode = gameObjectNode;
            Component = component;
        }
        #region Properties
        /// <summary>
        /// Name of Component
        /// </summary>
        public virtual string Name { get; set; } = "Component";
        /// <summary>
        /// Reference to Component we are referencing. Every property change is mirrored to this Component.
        /// </summary>
        [Browsable(false)]
        public Component Component { get; set; }
        /// <summary>
        /// Is this Component enabled?
        /// </summary>
        [Editor(typeof(BoolEditor), typeof(UITypeEditor))]
        public bool Enabled { get { return Component.Enabled; }set { Component.Enabled = value; } }
        /// <summary>
        /// GameObject holding this component.
        /// </summary>
        [Browsable(false)]
        public GameObject GameObject { get { return Component.gameObject; } set { Component.gameObject = value; } }
        /// <summary>
        /// GameObjectNode of this Component
        /// </summary>
        [Browsable(false)]
        public GameObjectNode GameObjectNode { get; set; }
        /// <summary>
        /// ID of component.
        /// Every component in the scene has different ID.
        /// </summary>
        public int ID { get { return Component.ID; }}
        #endregion
    }
}
