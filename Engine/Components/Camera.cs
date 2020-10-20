
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Scripts;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Engine
{
      public class Camera : Component
      {
            [System.Xml.Serialization.XmlIgnore] [ShowInEditor] public Effect effect { get; set; }
            //public Vector2 ReferenceResolution { get; set; } = new Vector2(800, 600);
            [XmlIgnore] public RenderTarget2D renderTarget;
            public override bool Enabled { get { return true; } }

            public static Camera Instance { get; private set; }

            public override void Awake()
            {
                  Instance = this;
                  Zoom = 1f;

                  renderTarget = new RenderTarget2D(
                Scene.Instance.GraphicsDevice,
                Scene.Instance.GraphicsDevice.PresentationParameters.BackBufferWidth,
                Scene.Instance.GraphicsDevice.PresentationParameters.BackBufferHeight,
                false,
                Scene.Instance.GraphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.Depth24);
            }

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
                        Matrix.CreateRotationZ(transform.Rotation) *
                        Matrix.CreateTranslation(-(int)transform.Position.X,
                           -(int)transform.Position.Y, 0);
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

            public void Move(Vector2 moveVector)
            {
                  transform.Position += moveVector;
            }

            public Vector2 WorldToScreen(Vector2 worldPosition)
            {
                  return Vector2.Transform(worldPosition, TranslationMatrix);
            }

            public Vector2 ScreenToWorld(Vector2 screenPosition)
            {
                  return Vector2.Transform(screenPosition,
                      Matrix.Invert(TranslationMatrix));
            }
            public override void Update()
            {
                  if (Scene.Instance.graphics.PreferMultiSampling != (AntialiasingStrength == 0 ? false : true))
                  {
                        Scene.Instance.graphics.PreferMultiSampling = AntialiasingStrength == 0 ? false : true;
                        Scene.Instance.GraphicsDevice.PresentationParameters.MultiSampleCount = AntialiasingStrength;
                        Scene.Instance.graphics.ApplyChanges();
                  }
                  if (Scene.Instance.graphics.PreferredBackBufferWidth != Size.X || Scene.Instance.graphics.PreferredBackBufferHeight != Size.Y)
                  {
                        Size = new Vector2(Scene.Instance.graphics.PreferredBackBufferWidth, Scene.Instance.graphics.PreferredBackBufferHeight);
                        Scene.Instance.graphics.ApplyChanges();
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