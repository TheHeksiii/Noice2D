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
    }
}
