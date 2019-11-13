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
            batch.DrawString(Scene.GetInstance().spriteFont, text.Value,
                        transform.Position.ToVector2(), Color, transform.Rotation.Z, Vector2.Zero, transform.Scale.ToVector2(), SpriteEffects.None, 0);
        }
    }
}
