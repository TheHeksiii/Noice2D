using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts
{
    public class ImageRenderer : Renderer
    {
        [System.ComponentModel.Editor(typeof(Editor.TextureEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [System.Xml.Serialization.XmlIgnore] [ShowInEditor] public Texture2D texture { get; set; }
        public string texturePath;
        public override void Awake()
        {
            if (texture == null && texturePath != null)
            {
                var scene = EditorSceneView.GetInstance();
                System.IO.Stream stream = TitleContainer.OpenStream(texturePath);
                texture = Texture2D.FromStream(scene.GraphicsDevice, stream);
                stream.Close();
            }
            base.Awake();
        }
        public override void Draw(SpriteBatch batch)
        {
            if (gameObject == null || texture == null) { return; }
            batch.Draw(texture: texture, position: transform.Position, color: this.Color, rotation: -transform.Rotation, scale: transform.Scale
                , origin: new Microsoft.Xna.Framework.Vector2(texture.Width / 2, texture.Height / 2));
            base.Draw(batch);
        }
    }
}
