using Engine;
using Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Editor
{
    class HierarchyNode : TreeNode
    {
        public GameObject GameObject { get { return Tag as GameObject; } set { Tag = value; } }
        public Component Component { get { return Tag as Component; } set { Tag = value; } }

        public HierarchyNode GetNode(object obj)
        {
            for (int i = 0; i < Nodes.Count; i++)
            {
                if ((Nodes[i] as HierarchyNode).GameObject == obj ||
                    (Nodes[i] as HierarchyNode).Component == obj) { return (Nodes[i] as HierarchyNode); }
            }
            return null;
        }
    }
}
