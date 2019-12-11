using Microsoft.Xna.Framework;

namespace Engine
{
    public class SilentGameObject : GameObject
    {
        public SilentGameObject() : base()
        {
        }

        public SilentGameObject(Vector2? position = null, Vector2? scale = null, string name = "", bool linkComponents = true) : base(position, scale, name, linkComponents)
        {
        }
    }
}
