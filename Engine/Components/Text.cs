using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;
namespace Scripts
{
    public class Text : Component
    {
        [ShowInEditor]
        public string Value { get; set;} = "test";
    }
}
