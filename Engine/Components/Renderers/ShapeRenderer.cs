using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Scripts
{
    public class ShapeRenderer : Renderer
    {
        CircleF circle = new CircleF(new Vector2(0, 0), 0);
        public int? Sides = null;
        public override void Draw(SpriteBatch batch)
        {
            if (GameObject == null) { return; }
            circle.Radius = Extensions.MaxVectorMember(transform.Scale);
            circle.Center = new Point2((int)transform.Position.X, (int)transform.Position.Y);
            if (Fill)
            {
                batch.DrawCircle(circle, Sides != null ? (int)Sides : (int)(circle.Radius), Color, circle.Radius);

            }
            else
            {
                batch.DrawCircle(circle, (int)(circle.Radius), Color, 1);
            }
            //batch.DrawLine(point: circle.Center, length: circle.Radius, angle: transform.Rotation, color: Color, thickness: 1);

            base.Draw(batch);
        }
    }
}
