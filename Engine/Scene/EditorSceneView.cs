using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using MonoGame.Extended;
using Scripts;
using Editor;
namespace Engine
{
    public class EditorSceneView : Game
    {
        public TransformHandle transformHandle { get { return TransformHandle.GetInstance(); } }
        private ColliderEditor colliderEditor;
        public SpriteFont spriteFont;
        private Camera camera = new Camera();
        public List<GameObject> gameObjects = new List<GameObject>();
        public List<GameObject> editorGameObjects = new List<GameObject>();
        private static EditorSceneView instance;
        public static EditorSceneView GetInstance()
        {
            return instance;
        }

        public Point windowSize;
        public GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteBatch uiBatch;

        public event EventHandler<GameObject> GameObjectCreated;
        public event EventHandler<GameObject> GameObjectDestroyed;
        public event EventHandler SceneLoaded;

        public event EventHandler<SceneData> SceneUpdated;

        public void FindNewDefaultObjects()
        {
            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (gameObjects[i].GetComponent<Camera>() != null)
                {
                    camera = gameObjects[i].GetComponent<Camera>();
                }
            }
        }
        public void OnGameObjectCreated(GameObject gameObject)
        {
            if (gameObject is SilentGameObject)
            {
                gameObject.silentInScene = true;
                editorGameObjects.Add(gameObject);
            }
            else
            {
                gameObjects.Add(gameObject);
            }
            GameObjectCreated?.Invoke(this, gameObject);
        }
        public void SaveScene()
        {
            Serializer.GetInstance().Serialize(gameObjects);
        }
        public void LoadScene()
        {
            //Add method to clean scene
            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Destroy();
            }
            gameObjects.Clear();

            //Physics.rigidbodies.Clear();

            gameObjects = new List<GameObject>();
            GameObject[] des = Serializer.GetInstance().Deserialize().ToArray();
            for (int i = 0; i < des.Length; i++)
            {

                for (int j = 0; j < des[i].Components.Count; j++)
                {
                    des[i].InitializeMemberComponents(des[i].Components[j]);

                    des[i].LinkComponents(des[i], des[i].Components[j]);

                    des[i].Components[j].gameObject = des[i];
                    des[i].Components[j].transform.gameObject = des[i];
                    des[i].Components[j].Awake();
                    des[i].Components[j].Awoken = true;

                }
                des[i].Awake();
            }

            SceneLoaded?.Invoke(this, null);
        }
        public void OnGameObjectDestroyed(GameObject gameObject)
        {
            if (gameObjects.Contains(gameObject))
            {
                gameObjects.Remove(gameObject);
            }
            GameObjectDestroyed?.Invoke(this, gameObject);
        }
        public EditorSceneView()
        {
            instance = this;

            IsMouseVisible = true;

            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 350,
                PreferredBackBufferHeight = 350,
                PreferMultiSampling = true,
                SynchronizeWithVerticalRetrace = true
                //GraphicsProfile = GraphicsProfile.HiDef
            };
            graphics.ApplyChanges();
            //Window.IsBorderless = true;
            Window.Position = new Point(50, 50);

            MouseInput.Mouse1Clicked += OnMouseClicked;
            MouseInput.Mouse1Released += OnMouseReleased;


            Content.RootDirectory = "Content";
        }
        private void OnMouseReleased()
        {
            if (ColliderEditor.editing == true)
            { return; }
            if (Tools.CurrentTool == Tools.ToolTypes.Select)
            {
                transformHandle.CurrentAxisSelected = null;
            }
        }
        private void OnMouseClicked()
        {
            if (ColliderEditor.editing == true)
            { return; }
            if (Tools.CurrentTool == Tools.ToolTypes.Select)
            {
                float minDistance = 99999999999;
                GameObject closestGameObject = null;
                for (int i = 0; i < gameObjects.Count; i++)
                {
                    if (gameObjects[i] == transformHandle.gameObject || gameObjects[i].Active == false) { continue; }
                    (bool intersects, float distance) detection = MouseInput.Position.In(gameObjects[i].GetComponent<Collider>());
                    if (detection.distance < minDistance && detection.intersects)
                    {
                        closestGameObject = gameObjects[i];
                        minDistance = detection.distance;
                    }
                    gameObjects[i].selected = false;
                }

                transformHandle.clicked = false;
                if (MouseInput.Position.In(transformHandle.boxColliderX).intersects)
                {
                    transformHandle.CurrentAxisSelected = TransformHandle.Axis.X;
                    transformHandle.clicked = true;
                }
                if (MouseInput.Position.In(transformHandle.boxColliderY).intersects)
                {
                    transformHandle.CurrentAxisSelected = TransformHandle.Axis.Y;
                    transformHandle.clicked = true;
                }
                if (MouseInput.Position.In(transformHandle.boxColliderXY).intersects)
                {
                    transformHandle.CurrentAxisSelected = TransformHandle.Axis.XY;
                    transformHandle.clicked = true;
                }

                if (closestGameObject != null && minDistance < 100)
                {
                    transformHandle.transform = closestGameObject.Transform;
                    transformHandle.objectSelected = true;
                    //transformHandle.Enabled = true;
                }
                else if (transformHandle.clicked == false)
                {
                    transformHandle.objectSelected = false;
                }
                //if (closestGameObject == null && transformHandle.clicked == false)
                //{
                //    transformHandle.Enabled = false;
                //}
            }
            else if (Tools.CurrentTool == Tools.ToolTypes.Text)
            {
                GameObject go = new GameObject(MouseInput.Position, new Vector2(0.1f, 0.1f), "Text");
                go.AddComponent<Text>();
                go.AddComponent<TextRenderer>();
                go.Awake();
            }
            else if (Tools.CurrentTool == Tools.ToolTypes.Polygon)
            {
                drawingPolygon = true;

                var polygon = new GameObject(MouseInput.Position, new Vector2(1, 1), "Polygon");
                polygon.AddComponent<PolygonCollider>();
                polygon.AddComponent<PolygonRenderer>();
                polygon.AddComponent<Rigidbody>();

                polygon.GetComponent<PolygonCollider>().Points.Add(new Vector2(0, -2) * 30);
                polygon.GetComponent<PolygonCollider>().Points.Add(new Vector2(-1, 1) * 30);
                polygon.GetComponent<PolygonCollider>().Points.Add(new Vector2(1, 1) * 30);

                polygon.Awake();
            }
        }
        public GameObject GetSelectedGameObject()
        {
            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (gameObjects[i].selected) { return gameObjects[i]; }
            }
            return null;
        }
        private void CreateDefaultObjects()
        {
            colliderEditor = new ColliderEditor();
            GameObject transformHandleGameObject = new GameObject();
            transformHandleGameObject.AddComponent<TransformHandle>();
            transformHandleGameObject.name = "Transform Handle";

            var CameraGO = new GameObject(name: "Camera");
            camera = CameraGO.AddComponent<Camera>();

            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Awake();
            }
        }
        public void SelectGameObject(GameObject gameObject)
        {
            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (gameObjects[i].ID != gameObject.ID)
                {
                    gameObjects[i].selected = false;
                }
            }
            gameObject.selected = true;

            transformHandle.transform = gameObject.Transform;
            transformHandle.objectSelected = true;
        }
        protected override void Initialize()
        {
            CreateDefaultObjects();

            windowSize = new Point(Window.ClientBounds.Width, Window.ClientBounds.Height);
            this.Window.ClientSizeChanged += delegate
            {
                windowSize = new Point(Window.ClientBounds.Width, Window.ClientBounds.Height);
            };

            TargetElapsedTime = TimeSpan.FromMilliseconds(15);

            Window.AllowUserResizing = true;

            spriteBatch = new SpriteBatch(GraphicsDevice);
            uiBatch = new SpriteBatch(GraphicsDevice);

            base.Initialize();

        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteFont = Content.Load<SpriteFont>("File");
            //ball.GetComponent<Sprite>().texture2D = Content.Load<Texture2D>("LongButton");

        }

        protected override void UnloadContent()
        {
        }
        float spawnTimer = 0.1f;
        float gridChangeTimer = 0.5f;
        bool drawingLine = false;
        bool drawingBox = false;
        bool drawingPolygon = false;
        GameObject line;
        GameObject box;

        int scrollValue = 0;

        void HandleInput()
        {
            if (this.IsActive == false)
            {
                return;
            }
            if (Mouse.GetState().ScrollWheelValue != scrollValue)
            {
                if (Mouse.GetState().ScrollWheelValue < scrollValue)
                {
                    Tools.PreviousTool();
                }
                else
                {
                    Tools.NextTool();
                }
            }


            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                switch (Tools.CurrentTool)
                {
                    case Tools.ToolTypes.Select:
                        if (transformHandle.gameObject.Active)
                        {// if mouse is not on axis line, disable gameobject
                            if (transformHandle.clicked)
                            {
                                transformHandle.Move(MouseInput.Delta);
                            }
                        }
                        break;
                    case Tools.ToolTypes.Circle:
                        spawnTimer -= Time.deltaTime;
                        if (spawnTimer < 0)
                        {
                            GameObject go = new GameObject(MouseInput.Position, new Vector2(20, 20), "Circle");
                            go.AddComponent<ShapeRenderer>();
                            go.AddComponent<Rigidbody>();
                            go.AddComponent<CircleCollider>();
                            //go.Destroy(20);
                            go.Awake();
                            spawnTimer = 0.5f;
                        }
                        break;
                    case Tools.ToolTypes.Box:
                        spawnTimer -= Time.deltaTime;
                        if (drawingBox == false)
                        {
                            drawingBox = true;

                            box = new GameObject(MouseInput.Position, new Vector2(1, 1), "Box");
                            box.AddComponent<BoxCollider>();
                            box.AddComponent<Rigidbody>().UseGravity = false;
                            box.AddComponent<BoxRenderer>();

                            box.Awake();
                        }
                        else
                        {
                            box.GetComponent<BoxCollider>().rect = new RectangleF(Vector2.Zero, MouseInput.Position - box.Transform.Position);
                        }
                        break;
                    case Tools.ToolTypes.Line:
                        if (drawingLine == false)
                        {
                            drawingLine = true;

                            line = new GameObject(MouseInput.Position, Vector2.One, "Line");
                            line.AddComponent<LineCollider>();
                            line.AddComponent<Rigidbody>().IsStatic = true;
                            line.AddComponent<LineRenderer>();
                            line.Awake();

                            line.GetComponent<LineRenderer>().lineStarted = true;
                        }
                        else
                        {
                            // im making 0 from 0
                            line.Transform.Rotation = Angle.FromVector(line.Transform.Position -
                                MouseInput.Position).Radians;
                            line.GetComponent<LineCollider>().length = Vector2.Distance(
                                MouseInput.Position,
                                line.Transform.Position);
                        }
                        break;
                }
            }
            else if (drawingLine == true)// mouse released
            {
                drawingLine = false;
                line.Transform.Rotation = Angle.FromVector(line.Transform.Position - MouseInput.Position).Radians;
                line.GetComponent<LineCollider>().length = Vector2.Distance(MouseInput.Position, line.Transform.Position);
            }
            else if (drawingBox == true)// mouse released
            {
                drawingBox = false;
                box.GetComponent<Rigidbody>().UseGravity = true;
            }

            scrollValue = Mouse.GetState().ScrollWheelValue;
        }
        protected override void Update(GameTime gameTime)
        {
            if (Global.GameRunning == false) { return; }
            Time.deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (IsActive)
            {
                MouseInput.Update(Mouse.GetState());
            }

            if (Global.EditorAttached && ColliderEditor.editing == false)
            {
                HandleInput();
            }

            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (gameObjects[i].Active || gameObjects[i].updateWhenDisabled)
                {
                    gameObjects[i].Update();
                }
            }
            for (int i = 0; i < editorGameObjects.Count; i++)
            {
                if (editorGameObjects[i].Active || editorGameObjects[i].updateWhenDisabled)
                {
                    editorGameObjects[i].Update();
                }
            }
            colliderEditor.Update();
            SceneUpdated?.Invoke(this, new SceneData() { gameObjects = this.gameObjects, tool = Tools.CurrentTool });
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(camera.BackgroundColor);
            /*spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque,
                        null, null, null, null, camera.staticMatrix);*/
            spriteBatch.Begin(transformMatrix: camera.TranslationMatrix);
            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (gameObjects[i].Active)
                {
                    gameObjects[i].Draw(spriteBatch);
                }
            }
            spriteBatch.End();

            if (Global.EditorAttached)
            {
                uiBatch.Begin();

                for (int i = 0; i < editorGameObjects.Count; i++)
                {
                    editorGameObjects[i].Draw(uiBatch);
                }

                uiBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}
