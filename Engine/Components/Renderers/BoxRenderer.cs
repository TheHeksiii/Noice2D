using Engine;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
namespace Scripts
{
    public class BoxRenderer : Renderer
    {
        public float StrokeSize { get; set; } = 1;
        [LinkableComponent]
        public BoxCollider boxCollider;

        public override void Draw(SpriteBatch batch)
        {
            if (GameObject == null || boxCollider == null) { return; }
            boxCollider.rect.Position = transform.Position;
            if (Fill)
            {
                batch.FillRectangle(boxCollider.rect, Color);
            }
            else
            {
                batch.DrawRectangle(boxCollider.rect, Color, StrokeSize);
            }

            base.Draw(batch);
        }

    }
}
