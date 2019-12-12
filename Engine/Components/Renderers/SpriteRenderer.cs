using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Scripts
{
    public class SpriteRenderer : Renderer
    {
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

            batch.Draw(
                texture: texture,
                position: transform.Position, sourceRectangle: null,
                color: this.Color,
                rotation: -transform.Rotation,
                origin: new Vector2(transform.Anchor.X * texture.Width, transform.Anchor.Y * texture.Height),
                scale: transform.Scale.Abs(),
                effects: RenderingHelpers.GetSpriteFlipEffects(transform),
                layerDepth: 0);
            base.Draw(batch);
        }

        public virtual void OnTextureLoaded(Texture2D _texture, string _path)
        {
            texturePath = _path;
        }

    }
}
