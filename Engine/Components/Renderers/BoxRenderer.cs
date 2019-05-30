using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Diagnostics;
using Engine;
namespace Scripts
{
    public class BoxRenderer : Renderer
    {
        public float StrokeSize { get; set; } = 1;
        [LinkableComponent]
        public BoxCollider boxCollider;

        public override void Draw(SpriteBatch batch)
        {
            if (gameObject == null || boxCollider == null) { return; }
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
