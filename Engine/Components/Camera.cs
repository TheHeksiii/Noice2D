using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Engine;
using Scripts;
using Microsoft.Xna.Framework.Graphics;
using Form = System.Windows.Forms.Form;
using System;
namespace Engine
{
    public class Camera : Component
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
            StaticMatrix = TranslationMatrix;
        }
        [System.ComponentModel.Editor(typeof(Editor.ColorPickerEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [ShowInEditor] public Color BackgroundColor { get; set; } = new Color(34, 34, 34);


        [ShowInEditor]
        public int AntialiasingStrength { get; set; } = 0;


        [ShowInEditor] public float Zoom { get; set; }
        [ShowInEditor] public float Rotation { get; set; }

        [ShowInEditor] public Vector2 Size { get; set; } = new Vector2(800, 600);


        public Matrix StaticMatrix;
        public Matrix TranslationMatrix
        {
            get
            {
                return Matrix.CreateTranslation(-(int)transform.Position.X,
                   -(int)transform.Position.Y, 0) *
                   //Matrix.CreateRotationZ(Rotation) *
                   Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                Matrix.CreateRotationY(Rotation);
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

        public void Move(Vector2 moveVector)
        {
            transform.Position += moveVector;
        }

        public Vector2 WorldToScreen(Vector2 worldPosition, bool staticMatrix = false)
        {
            return Vector2.Transform(worldPosition, staticMatrix ? StaticMatrix : TranslationMatrix);
        }

        public Vector2 ScreenToWorld(Vector2 screenPosition, bool staticMatrix = false)
        {
            return Vector2.Transform(screenPosition,
                Matrix.Invert(staticMatrix ? StaticMatrix : TranslationMatrix));
        }

        public override void Update()
        {
            if (EditorSceneView.GetInstance().graphics.PreferMultiSampling != (AntialiasingStrength == 0 ? false : true))
            {
                EditorSceneView.GetInstance().graphics.PreferMultiSampling = AntialiasingStrength == 0 ? false : true;
                EditorSceneView.GetInstance().GraphicsDevice.PresentationParameters.MultiSampleCount = AntialiasingStrength;
                EditorSceneView.GetInstance().graphics.ApplyChanges();
            }
            if (EditorSceneView.GetInstance().graphics.PreferredBackBufferWidth != Size.X || EditorSceneView.GetInstance().graphics.PreferredBackBufferHeight != Size.Y)
            {
                EditorSceneView.GetInstance().graphics.PreferredBackBufferWidth = (int)Size.X;
                EditorSceneView.GetInstance().graphics.PreferredBackBufferHeight= (int)Size.Y;
                EditorSceneView.GetInstance().graphics.ApplyChanges();
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