using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
                var scene = Scene.GetInstance();
                System.IO.Stream stream = TitleContainer.OpenStream(texturePath);
                texture = Texture2D.FromStream(scene.GraphicsDevice, stream);
                stream.Close();
            }
            base.Awake();
        }
        public override void Draw(SpriteBatch batch)
        {
            if (GameObject == null || texture == null) { return; }
            batch.Draw(texture: texture, position: transform.Position.ToVector2(), color: this.Color, rotation: -transform.Rotation.Z, scale: transform.Scale.ToVector2()
                , origin: new Microsoft.Xna.Framework.Vector2(texture.Width / 2, texture.Height / 2));
            base.Draw(batch);
        }
    }
}
