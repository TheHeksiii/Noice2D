using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Editor
{
    public abstract class ComponentItem
    {
        public ComponentItem(GameObjectNode node,Component comp)
        {
            gameObjectNode = node;
            component = comp;
            Enabled = comp.Enabled;
        }
        protected abstract Type ComponentType();
        public GameObjectNode gameObjectNode;
        public Component component;

        public bool Enabled { get { return component.Enabled; }set { component.Enabled = value; } }
    }
}
