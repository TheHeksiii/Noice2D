using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
namespace Engine
{
    public static class MouseInput
    {
        public delegate void OnMouseClick();
        public delegate void OnMouseReleased();
        public static OnMouseClick Mouse1Clicked;
        public static OnMouseClick Mouse1Released;

        public static OnMouseClick Mouse2Clicked;
        public static OnMouseClick Mouse2Released;

        public static ButtonState MouseButton1State;
        public static ButtonState MouseButton2State;

        public static Vector2 Delta;
        public static Vector2 Position = Vector2.Zero;
        public static void Update(MouseState state)
        {
            // Left Button
            if (MouseButton1State == ButtonState.Released && state.LeftButton == ButtonState.Pressed)
            {
                Mouse1Clicked?.Invoke();
            }
            if (MouseButton1State == ButtonState.Pressed && state.LeftButton == ButtonState.Released)
            {
                Mouse1Released?.Invoke();
            }
            MouseButton1State = state.LeftButton;

            // Right Button
            if (MouseButton2State == ButtonState.Released && state.RightButton == ButtonState.Pressed)
            {
                Mouse2Clicked?.Invoke();
            }
            if (MouseButton2State == ButtonState.Pressed && state.RightButton == ButtonState.Released)
            {
                Mouse2Released?.Invoke();
            }
            MouseButton2State = state.RightButton;



            Delta = Camera.GetInstance().ScreenToWorld(state.Position.ToVector2()) - Position;

            Position = Camera.GetInstance().ScreenToWorld(state.Position.ToVector2());
        }

    }
}
