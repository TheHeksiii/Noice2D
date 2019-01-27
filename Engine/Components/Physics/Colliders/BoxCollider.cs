using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Engine;
using MonoGame.Extended;

namespace Scripts
{
    public class BoxCollider : Collider
    {
        public RectangleF rect;

        [ShowInEditor]
        public Vector2 Size { get { return rect.Size.ToVector2(); } set { rect.Size = value; } }
    }
}
