using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Scripts
{
    public class PolygonRenderer : Renderer
    {
        [LinkableComponent]
        public PolygonCollider polygonCollider;
        public bool editingPoints = false;
        public override void Draw(SpriteBatch batch)
        {
            if (gameObject == null || polygonCollider == null) { return; }

            for (int i = 0; i < polygonCollider.Points.Count; i++)
            {
                Vector2 point1 = polygonCollider.Points[i];
                Vector2 point2 = i + 1 >= polygonCollider.Points.Count ? polygonCollider.Points[0] : polygonCollider.Points[i + 1];
                batch.DrawLine(TransformToWorld(point1), TransformToWorld(point2), Color, 3);
            }
            batch.DrawPoint(polygonCollider.Center, Color, 4);

            base.Draw(batch);
        }
    }
}
