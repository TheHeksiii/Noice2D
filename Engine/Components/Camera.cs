using Microsoft.Xna.Framework;
using Scripts;
namespace Engine
{
    public class Camera : Component, IColorable
    {
        public override bool Enabled { get { return true; } }

        public static Camera GetInstance()
        {
            return instance;
        }
        public static Camera instance;

        public override void Awake()
        {
            instance = this;
            Zoom = 1f;
        }
        [System.ComponentModel.Editor(typeof(Editor.ColorPickerEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [ShowInEditor] public Color Color { get; set; } = new Color(34, 34, 34);


        [ShowInEditor]
        public int AntialiasingStrength { get; set; } = 0;


        [ShowInEditor] public float Zoom { get; set; }

        [ShowInEditor] public Vector2 Size { get; set; } = new Vector2(800, 600);


        public Matrix TranslationMatrix
        {
            get
            {
                return
                Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                Matrix.CreateRotationX(transform.Rotation.X) *
                Matrix.CreateRotationY(transform.Rotation.Z) *
                Matrix.CreateRotationZ(transform.Rotation.Y) *
                Matrix.CreateTranslation(-(int)transform.Position.X,
                   -(int)transform.Position.Y, -(int)transform.Position.Z);
                //Matrix.CreateRotationZ(Rotation);
            }
        }

        public void AdjustZoom(float amount)
        {
            Zoom += amount;
            if (Zoom < 0.1f)
            {
                Zoom = 0.1f;
            }
        }

        public void Move(Vector3 moveVector)
        {
            transform.Position += moveVector;
        }

        public Vector3 WorldToScreen(Vector3 worldPosition)
        {
            return Vector3.Transform(worldPosition, TranslationMatrix);
        }

        public Vector3 ScreenToWorld(Vector3 screenPosition)
        {
            return Vector3.Transform(screenPosition,
                Matrix.Invert(TranslationMatrix));
        }
        public override void Update()
        {
            if (Scene.GetInstance().graphics.PreferMultiSampling != (AntialiasingStrength == 0 ? false : true))
            {
                Scene.GetInstance().graphics.PreferMultiSampling = AntialiasingStrength == 0 ? false : true;
                Scene.GetInstance().GraphicsDevice.PresentationParameters.MultiSampleCount = AntialiasingStrength;
                Scene.GetInstance().graphics.ApplyChanges();
            }
            if (Scene.GetInstance().graphics.PreferredBackBufferWidth != Size.X || Scene.GetInstance().graphics.PreferredBackBufferHeight != Size.Y)
            {
                Scene.GetInstance().graphics.PreferredBackBufferWidth = (int)Size.X;
                Scene.GetInstance().graphics.PreferredBackBufferHeight = (int)Size.Y;
                Scene.GetInstance().graphics.ApplyChanges();
            }


            /*Vector2 cameraMovement = Vector2.Zero;
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                cameraMovement.X = -1;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                cameraMovement.X = 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                cameraMovement.Y = -1;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                cameraMovement.Y = 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.OemPlus))
            {
                AdjustZoom(0.1f);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.OemMinus))
            {
                AdjustZoom(-0.1f);
            }

            if (cameraMovement != Vector2.Zero)
            {
                cameraMovement.Normalize();
            }
            cameraMovement *= 10f;
            
            Move(cameraMovement);*/
        }
    }
}