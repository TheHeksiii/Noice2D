using Engine;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Scripts
{
    public class BoxCollider : Collider
    {
        public RectangleF rect;
        [ShowInEditor]
        public Vector2 Size { get { return rect.Size.ToVector2(); } set { rect.Size = value; } }
        public override void Update()
        {
            rect.Position = transform.Position.ToPoint2();
            base.Update();
        }
    }
}
