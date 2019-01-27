using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Diagnostics;
using Engine;
namespace Scripts
{
    public class LineRenderer : Renderer
    {
        [ShowInEditor] public float StrokeSize { get; set; } = 2;
        public bool lineStarted = false;

        [LinkableComponent]
        public LineCollider lineCollider;

        public override void Awake()
        {
            lineCollider = GetComponent<LineCollider>();
            base.Awake();
        }
        public override void Draw(SpriteBatch batch)
        {
            if (gameObject == null || lineCollider == null) { return; }

            batch.DrawLine(lineCollider.GetLineStart(), lineCollider.GetLineEnd(), Color, StrokeSize);
            /*batch.DrawPoint(lineCollider.GetLineStart(), Color.Red, StrokeSize * 4);
            batch.DrawPoint(lineCollider.GetLineEnd(), Color.Red, StrokeSize * 4);*/

            base.Draw(batch);
        }
    }
}
