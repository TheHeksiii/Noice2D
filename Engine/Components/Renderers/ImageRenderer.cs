using Engine;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts
{
    class ImageRenderer : Renderer
    {
        [System.ComponentModel.Editor(typeof(Editor.TextureEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [ShowInEditor] public Texture2D texture { get; set; }

        public override void Draw(SpriteBatch batch)
        {
            if (gameObject == null || texture == null) { return; }
            batch.Draw(texture: texture, position: transform.Position, color: this.Color, rotation: transform.Rotation, scale: transform.Scale);
            base.Draw(batch);
        }
    }
}
