using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Scripts
{
      public class TextRenderer : Renderer
      {
            [LinkableComponent]
            public Text text;

            public override void Draw(SpriteBatch batch)
            {
                  if (GameObject == null) { return; }
                  batch.DrawString(Scene.Instance.spriteFont, text.Value,
                              transform.Position, Color, transform.Rotation, Vector2.Zero, transform.Scale, SpriteEffects.None, 0);
            }
      }
}
