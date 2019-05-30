using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using MonoGame.Extended;
using Scripts;
using Editor;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Engine
{
    public class EditorSceneView : Game
    {
        public string scenePath = "";
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

        public GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteBatch uiBatch;
        public event EventHandler<GameObject> GameObjectCreated;
        public event EventHandler<GameObject> GameObjectDestroyed;
        public event EventHandler SceneLoaded;

        public event EventHandler<SceneData> SceneUpdated;

        Stopwatch updateStopwatch = new Stopwatch();
        Stopwatch renderStopwatch = new Stopwatch();

        public float updateTime = 0;
        public float renderTime = 0;

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
        public GameObject FindGameObject(Type type)
        {
            foreach (var gameObject in gameObjects)
            {
                var bl = gameObject.GetComponent(type);
                if (bl != null) { return gameObject; }
            }
            return null;
        }
        public int GetGameObjectIndex(int ID)
        {
            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (gameObjects[i].ID == ID)
                {
                    return i;
                }
            }
            return -1;
        }
        public void OnGameObjectCreated(GameObject gameObject)
        {
            if (gameObject is SilentGameObject)
            {
                gameObjects.Add(gameObject);

                //editorGameObjects.Add(gameObject);
            }
            else
            {
                gameObjects.Add(gameObject);
            }
            GameObjectCreated?.Invoke(this, gameObject);
        }
        public bool LoadScene()
        {
            using (System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog())
            {
                openFileDialog.InitialDirectory = System.IO.Directory.GetCurrentDirectory();

                System.Windows.Forms.DialogResult dialogResult = openFileDialog.ShowDialog();
                if (dialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    //Add method to clean scene
                    for (int i = 0; i < gameObjects.Count; i++)
                    {
                        gameObjects[i].Destroy();
                    }
                    gameObjects.Clear();

                    //Physics.rigidbodies.Clear();

                    gameObjects = new List<GameObject>();

                    GameObject[] des = Serializer.GetInstance().LoadScene(openFileDialog.FileName).ToArray();
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
                        SceneLoaded?.Invoke(this, null);
                    }
                    return true;
                }
            }
            return false;
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
                PreferredBackBufferWidth = 800,
                PreferredBackBufferHeight = 600,
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
                    transformHandle.transform = closestGameObject.transform;
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
            transformHandleGameObject.Name = "Transform Handle";

            var CameraGO = new GameObject(name: "Camera");
            camera = CameraGO.AddComponent<Camera>();

            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Awake();
            }
            //CreateLightSceneObjects();
            void CreateLightSceneObjects()
            {
                GameObject light = new GameObject();

                light.Name = "Light";
                light.transform.Position = new Vector2(camera.Size.X / 2, camera.Size.Y / 2);
                light.AddComponent<Light>();


                light.Awake();
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

            transformHandle.transform = gameObject.transform;
            transformHandle.objectSelected = true;
        }
        public SpriteBatch CreateSpriteBatch()
        {
            return new SpriteBatch(GraphicsDevice);
        }
        protected override void Initialize()
        {
            CreateDefaultObjects();

            TargetElapsedTime = TimeSpan.FromMilliseconds(15);

            Window.AllowUserResizing = true;

            spriteBatch = new SpriteBatch(GraphicsDevice);
            uiBatch = new SpriteBatch(GraphicsDevice);

            /*GameObject a = new GameObject();
            a.AddComponent<ParticleSystem>();
            a.Awake();*/

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
                            box.GetComponent<BoxCollider>().rect = new RectangleF(Vector2.Zero, MouseInput.Position - box.transform.Position);
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
                            line.transform.Rotation = Angle.FromVector(line.transform.Position -
                                MouseInput.Position).Radians;
                            line.GetComponent<LineCollider>().length = Vector2.Distance(
                                MouseInput.Position,
                                line.transform.Position);
                        }
                        break;
                }
            }
            else if (drawingLine == true)// mouse released
            {
                drawingLine = false;
                line.transform.Rotation = Angle.FromVector(line.transform.Position - MouseInput.Position).Radians;
                line.GetComponent<LineCollider>().length = Vector2.Distance(MouseInput.Position, line.transform.Position);
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

            updateStopwatch.Start();

            if (Global.GameRunning == false) { return; }
            Time.deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Time.elapsedTime += Time.deltaTime;

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
                gameObjects[i].Update();
            }
            for (int i = 0; i < editorGameObjects.Count; i++)
            {
                editorGameObjects[i].Update();
            }
            colliderEditor.Update();
            SceneUpdated?.Invoke(this, new SceneData() { gameObjects = this.gameObjects, tool = Tools.CurrentTool });
            base.Update(gameTime);

            updateStopwatch.Stop();
            updateTime = updateStopwatch.ElapsedMilliseconds;
            updateStopwatch.Reset();

        }
        protected override void Draw(GameTime gameTime)
        {
            renderStopwatch.Start();
            GraphicsDevice.Clear(camera.BackgroundColor);
            /*spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque,
                        null, null, null, null, camera.staticMatrix);*/
            /*List<GameObject> gameObjectsWithShader = gameObjects.Where((go) => go.GetComponent<Renderer>()?.effect != null).ToList();
            for (int i = 0; i < gameObjectsWithShader.Count; i++)
            {
                if (gameObjectsWithShader[i].Active)
                {

                    spriteBatch.Begin(transformMatrix: camera.TranslationMatrix, effect: gameObjectsWithShader[i].GetComponent<Renderer>().effect);
                    gameObjectsWithShader[i].GetComponent<Renderer>().effect.CurrentTechnique.Passes[0].Apply();

                    gameObjectsWithShader[i].Draw(spriteBatch);

                    spriteBatch.End();
                }
            }*/


            spriteBatch.Begin(transformMatrix: camera.TranslationMatrix);
            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Draw(spriteBatch);
            }
            if (transformHandle.gameObject != null)
            {
                transformHandle.gameObject.Draw(spriteBatch);
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


            renderStopwatch.Stop();
            renderTime = renderStopwatch.ElapsedMilliseconds;
            renderStopwatch.Reset();
        }
    }
}
