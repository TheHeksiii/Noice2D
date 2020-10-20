using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
namespace Scripts
{
	public class BoxRenderer : Renderer
	{
		public float StrokeSize { get; set; } = 1;
		[LinkableComponent]
		public BoxShape boxCollider;

		[ShowInEditor] public bool Fill { get; set; } = false;

		public override void Draw(SpriteBatch batch)
		{
			if (GameObject == null || boxCollider == null) { return; }
			RectangleF drawRect = new RectangleF(boxCollider.rect.Position,boxCollider.rect.Size);
			
			drawRect.Size *= transform.Scale;
			drawRect.Offset(-boxCollider.rect.Size.Width * transform.Anchor.X, -boxCollider.rect.Size.Height * transform.Anchor.Y);

			if (Fill)
			{
				batch.FillRectangle(drawRect, Color);
			}
			else
			{
				batch.DrawRectangle(drawRect, Color, StrokeSize);
			}
			base.Draw(batch);
		}

	}
}
