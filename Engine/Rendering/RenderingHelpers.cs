using Microsoft.Xna.Framework.Graphics;
using Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public static class RenderingHelpers
    {
        public static SpriteEffects GetSpriteFlipEffects(Transform transform)
        {
            return (transform.Scale.X < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None) | (transform.Scale.Y < 0 ? SpriteEffects.FlipVertically : SpriteEffects.None);
        }
    }
}
