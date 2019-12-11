using Engine;
using Microsoft.Xna.Framework;

namespace Scripts
{
    public class CircleCollider : Collider
    {
        [ShowInEditor] public float Radius { get { return transform.Scale.MaxVectorMember(); } set { if (GameObject != null) { transform.Scale = new Vector2(value, value); } } }
    }
}
