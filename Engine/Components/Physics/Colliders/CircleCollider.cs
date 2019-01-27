using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;
using Microsoft.Xna.Framework;

namespace Scripts
{
    public class CircleCollider : Collider
    {
        public float Radius { get { return transform.Scale.MaxVectorMember(); } set { if (gameObject != null) { transform.Scale = new Vector2(value, value); } } }
    }
}
