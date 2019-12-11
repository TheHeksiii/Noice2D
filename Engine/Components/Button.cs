using Microsoft.Xna.Framework;
using Scripts;

namespace Engine
{
    public class Button : Component
    {
        public delegate void OnClickedAction();
        OnClickedAction onClickedAction;
        ShapeRenderer sprite;
        public override void Awake()
        {
            //onClickedAction += ChangeColorOnClick;
            //sprite = gameObject.GetComponent<ShapeRenderer>();

        }

        public override void Update()
        {

            /*if (cam.ScreenToWorld(Mouse.GetState().Position).In(
                gameObject.transform.position,
                sprite.texture2D.Width,
                sprite.texture2D.Height) == true)
            {
                sprite.color = Color.Red;
                mouseIsOver = true;
            }
            else
            {
                sprite.color = Color.White;
                mouseIsOver = false;
            }
            //gameObject.transform.position = Vector2.Lerp(gameObject.transform.position,Mouse.GetState().Position, 0.1f);
            if (mouseIsOver == true && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                //just wanted to place button,but maybe still interactable,whatever
                //active = false;
                onClickedAction.Invoke();
            }*/
        }

        public void ChangeColorOnClick()
        {
            sprite.Color = Color.Cyan;
        }
    }
}
