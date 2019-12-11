using Engine.Properties;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Scripts;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Engine
{
    public class ParticleSystemRenderer : Renderer
    {
        [XmlIgnore] public SpriteBatch spriteBatch;
        //[System.ComponentModel.Editor(typeof(Editor.TextureEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [XmlIgnore] [ShowInEditor] public Texture2D circleTexture { get; set; }
        CircleF circle = new CircleF(new Vector2(0, 0), 10);
        public ParticleSystem particleSystem;

        public ParticleSystemRenderer()
        {
            spriteBatch = Scene.GetInstance().CreateSpriteBatch();

            MemoryStream ms = new MemoryStream();
            Resources.ParticleTexture.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            circleTexture = Texture2D.FromStream(Scene.GetInstance().GraphicsDevice, ms);

        }
        public override void Draw(SpriteBatch batch)
        {
            if (circleTexture == null) { return; }
            spriteBatch.Begin(blendState: BlendState.Additive);
            Parallel.For(0, particleSystem.particles.Count, (i) =>
            {
                lock (particleSystem.listLock)
                {
                    circle.Center = particleSystem.particles[i].position;
                    circle.Radius = particleSystem.particles[i].radius;
                    spriteBatch.Draw(circleTexture, destinationRectangle: new Rectangle((int)circle.Center.X - (int)circle.Radius / 2, (int)circle.Center.Y - (int)circle.Radius / 2, (int)circle.Radius, (int)circle.Radius),
                        color: particleSystem.particles[i].color);
                }
            });
            spriteBatch.End();
        }
    }
}

