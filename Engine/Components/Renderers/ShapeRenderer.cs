using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Diagnostics;
using Engine;

namespace Scripts
{
    public class ShapeRenderer : Renderer
    {
        CircleF circle = new CircleF(new Vector2(0, 0), 0);

        public override void Draw(SpriteBatch batch)
        {
            if (gameObject == null) { return; }
            circle.Radius = Extensions.MaxVectorMember(transform.Scale);
            circle.Center = new Point2((int)transform.Position.X, (int)transform.Position.Y);
            batch.DrawCircle(circle, (int)(circle.Radius), Color, 1);
            batch.DrawLine(point: circle.Center, length: circle.Radius, angle: transform.Rotation, color: Color,thickness:1);

            base.Draw(batch);
        }
    }
}
