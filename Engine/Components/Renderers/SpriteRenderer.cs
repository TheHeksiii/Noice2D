using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Scripts
{
    public class SpriteRenderer : Renderer
    {
        [System.ComponentModel.Editor(typeof(TextureEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [System.Xml.Serialization.XmlIgnore] [ShowInEditor] public Texture2D texture { get; set; }
        public string texturePath;

        public override void Awake()
        {
            if (texture == null && texturePath != null)
            {
                LoadTexture(texturePath);
            }
            base.Awake();
        }
        public void LoadTexture(string _texturePath)
        {
            System.IO.Stream stream = TitleContainer.OpenStream(_texturePath);
            texture = Texture2D.FromStream(Scene.GetInstance().GraphicsDevice, stream);
            stream.Close();
            OnTextureLoaded(texture, _texturePath);

        }

        public override void Draw(SpriteBatch batch)
        {
            if (GameObject == null || texture == null) { return; }
            batch.Draw(texture: texture, position: transform.Position, color: this.Color, rotation: -transform.Rotation, scale: transform.Scale
                , origin: new Microsoft.Xna.Framework.Vector2(transform.Anchor.X * texture.Width, transform.Anchor.Y * texture.Height));

            base.Draw(batch);
        }

        public virtual void OnTextureLoaded(Texture2D _texture, string _path)
        {
            texturePath = _path;
        }

    }
}
