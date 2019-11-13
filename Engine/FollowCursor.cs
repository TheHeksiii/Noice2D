using Scripts;
namespace Engine
{
    public class FollowCursor : Component
    {
        public override void Update()
        {
            transform.Position = MouseInput.Position.ToVector3();
            base.Update();
        }
    }
}
