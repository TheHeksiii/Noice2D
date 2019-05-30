using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Engine
{
    public class SilentGameObject : GameObject
    {
        public SilentGameObject() : base()
        {
        }

        public SilentGameObject(Vector2? position = null, Vector2? scale = null, string name = "", bool linkComponents = true) : base(position, scale, name, linkComponents)
        {
        }
    }
}
