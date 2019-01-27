using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;
using Microsoft.Xna.Framework;

namespace Scripts
{
    public class TextRenderer : Renderer
    {
        [LinkableComponent]
        public Text text;
        public override void Draw(SpriteBatch batch)
        {
            if (gameObject == null) { return; }
            batch.DrawString(EditorSceneView.GetInstance().spriteFont, text.Value,
                        transform.Position, Color, transform.Rotation, Vector2.Zero, transform.Scale, SpriteEffects.None, 0);
        }
    }
}
