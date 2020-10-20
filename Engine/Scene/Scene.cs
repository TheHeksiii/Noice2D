using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Scripts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Engine
{
	public class Scene : Game
	{
		public string scenePath = "";
		public TransformHandle transformHandle { get { return TransformHandle.GetInstance(); } }
		private ColliderEditor colliderEditor;
		public SpriteFont spriteFont;
		private Camera camera { get { return Camera.Instance; } }
		public List<GameObject> gameObjects = new List<GameObject>();
		public List<GameObject> editorGameObjects = new List<GameObject>();
		public static Scene Instance { get; private set; }

		public GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		SpriteBatch uiBatch;
		public event EventHandler<GameObject> GameObjectCreated;
		public event EventHandler<GameObject> GameObjectDestroyed;
		public event EventHandler SceneLoad;

		public event EventHandler<SceneData> SceneUpdated;

		Stopwatch updateStopwatch = new Stopwatch();
		Stopwatch renderStopwatch = new Stopwatch();

		public float updateTime = 0;
		public float renderTime = 0;

		float spawnTimer = 0.1f;
		float gridChangeTimer = 0.5f;
		bool drawingLine = false;
		bool drawingBox = false;
		bool drawingPolygon = false;
		GameObject line;
		GameObject box;
		int scrollValue = 0;
		GameObject image;

		public Scene()
		{
			Instance = this;

			IsMouseVisible = true;

			graphics = new GraphicsDeviceManager(this)
			{
				PreferredBackBufferWidth = 800,
				PreferredBackBufferHeight = 600,
				//PreferMultiSampling = true,
				SynchronizeWithVerticalRetrace = false,
				
				//GraphicsProfile = GraphicsProfile.HiDef
			};
			graphics.ApplyChanges();
			this.IsFixedTimeStep = false;
			//Window.IsBorderless = true;
			Window.Position = new Point(0, 100);

			MouseInput.Mouse1Down += OnMouse1Clicked;
			MouseInput.Mouse1Up += OnMouse1Released;

			MouseInput.Mouse3Down += OnMouse3Clicked;
			MouseInput.Mouse3Up += OnMouse3Released;

			Content.RootDirectory = "Content";
		}
		private void CreateDefaultObjects()
		{
			colliderEditor = new ColliderEditor();
			GameObject transformHandleGameObject = GameObject.Create(_silent: true);
			transformHandleGameObject.AddComponent<TransformHandle>();
			transformHandleGameObject.Name = "Transform Handle";
			transformHandleGameObject.Active = false;

			var CameraGO = GameObject.Create(name: "Camera");
			CameraGO.AddComponent<Camera>();
			for (int i = 0; i < gameObjects.Count; i++)
			{
				gameObjects[i].Awake();
			}


			/*image = new GameObject(name: "Image");
			var renderer = image.AddComponent<ImageRenderer>();
			renderer.texture = Content.Load<Texture`2D>("rock");
			renderer.transform.Position = new Vector3(renderer.texture.Width / 2, renderer.texture.Height / 2, 0);
			image.Awake();*/
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

			transformHandle.transform.Position = gameObject.transform.Position;
			transformHandle.selectedTransform = gameObject.transform;
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
			if (Serializer.lastScene != "" && File.Exists(Serializer.lastScene))
			{
				LoadScene(Serializer.lastScene);
			}
			base.Initialize();

		}

		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteFont = Content.Load<SpriteFont>("File");
		}

		protected override void UnloadContent()
		{
		}
		public SceneFile GetSceneFile()
		{
			SceneFile sf = new SceneFile();
			sf.Components = new List<Component>();
			for (int i = 0; i < gameObjects.Count; i++)
			{
				sf.Components.AddRange(gameObjects[i].Components);
			}
			sf.GameObjects = gameObjects;
			sf.gameObjectNextID = IDsManager.gameObjectNextID;
			return sf;
		}
		public void FindNewDefaultObjects()
		{
			for (int i = 0; i < gameObjects.Count; i++)
			{
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
			gameObjects.Add(gameObject);

			GameObjectCreated?.Invoke(this, gameObject);
		}
		public bool LoadScene(string path = null)
		{
			if (path == null)
			{
				using (System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog())
				{
					openFileDialog.InitialDirectory = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "/Scenes");

					System.Windows.Forms.DialogResult dialogResult = openFileDialog.ShowDialog();
					if (dialogResult == System.Windows.Forms.DialogResult.OK)
					{
						path = openFileDialog.FileName;
					}
					else
					{
						return false;
					}
				}
			}
			//Add method to clean scene
			for (int i = 0; i < gameObjects.Count; i++)
			{
				gameObjects[i].Destroy();
			}
			gameObjects.Clear();

			//Physics.rigidbodies.Clear();

			gameObjects = new List<GameObject>();
			SceneFile sceneFile = Serializer.GetInstance().LoadGameObjects(path);



			Serializer.GetInstance().ConnectGameObjectsWithComponents(sceneFile);
			IDsManager.gameObjectNextID = sceneFile.gameObjectNextID;

			for (int i = 0; i < sceneFile.GameObjects.Count; i++)
			{
				Scene.Instance.OnGameObjectCreated(sceneFile.GameObjects[i]);

				sceneFile.GameObjects[i].Awake();
			}

			SceneLoad?.Invoke(this, null);

			scenePath = path;

			return true;
		}
		public void OnGameObjectDestroyed(GameObject gameObject)
		{
			if (gameObjects.Contains(gameObject))
			{
				gameObjects.Remove(gameObject);
			}
			GameObjectDestroyed?.Invoke(this, gameObject);
		}

		private void OnMouse3Clicked()
		{
		}
		private void OnMouse3Released()
		{
		}


		private void OnMouse1Released()
		{
			if (ColliderEditor.editing == true)
			{ return; }
			if (true)//Tools.CurrentTool == Tools.ToolTypes.Select)
			{
				transformHandle.CurrentAxisSelected = null;
			}
		}
		private void OnMouse1Clicked()
		{
			if (ColliderEditor.editing == true)
			{ return; }
			if (true)//Tools.CurrentTool == Tools.ToolTypes.Select)
			{
				float minDistance = 99999999999;
				GameObject closestGameObject = null;
				for (int i = 0; i < gameObjects.Count; i++)
				{
					if (gameObjects[i] == transformHandle.GameObject || gameObjects[i].Active == false) { continue; }
					(bool intersects, float distance) detection = MouseInput.Position.In(gameObjects[i].GetComponent<Shape>());
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
					SelectGameObject(closestGameObject);
				}
				else if (transformHandle.clicked == false)
				{
					transformHandle.objectSelected = false;
				}
			}
		}

		void HandleInput()
		{
			if (this.IsActive == false)
			{
				return;
			}

			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();
			scrollValue = Mouse.GetState().ScrollWheelValue;
		}
		protected override void Update(GameTime gameTime)
		{

			updateStopwatch.Start();

			if (Global.GameRunning == false) { return; }
			Time.deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
			Time.elapsedTime += Time.deltaTime;
			Time.elapsedTicks++;

			MouseInput.Update(Mouse.GetState());

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
			SceneUpdated?.Invoke(this, new SceneData() { gameObjects = this.gameObjects });
			base.Update(gameTime);

			updateStopwatch.Stop();
			updateTime = updateStopwatch.ElapsedMilliseconds;
			updateStopwatch.Reset();

		}

		protected override void Draw(GameTime gameTime)
		{
			if (camera?.renderTarget == null) { return; }
			renderStopwatch.Start();



			DrawSceneToTarget();




			GraphicsDevice.Clear(Color.Black);

			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
						SamplerState.LinearClamp, DepthStencilState.Default,
						RasterizerState.CullNone, effect: Camera.Instance.effect);


			spriteBatch.Draw(texture: camera.renderTarget, destinationRectangle: new Rectangle(0, 0, camera.renderTarget.Width, camera.renderTarget.Height), color: Color.White);

			spriteBatch.End();

			base.Draw(gameTime);

			renderStopwatch.Stop();
			renderTime = renderStopwatch.ElapsedMilliseconds;
			renderStopwatch.Reset();
		}
		void DrawSceneToTarget()
		{
			GraphicsDevice.SetRenderTarget(camera.renderTarget);
			GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };

			GraphicsDevice.Clear(camera.Color);


			spriteBatch.Begin(transformMatrix: camera.TranslationMatrix, blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp, depthStencilState: DepthStencilState.Default);
			for (int i = 0; i < gameObjects.Count; i++)
			{
				gameObjects[i].Draw(spriteBatch);
			}
			if (transformHandle.GameObject != null)
			{
				transformHandle.GameObject.Draw(spriteBatch);
			}
			spriteBatch.End();
			if (Global.EditorAttached)
			{
				uiBatch.Begin(transformMatrix: camera.TranslationMatrix, blendState: BlendState.AlphaBlend, depthStencilState: DepthStencilState.Default);

				for (int i = 0; i < editorGameObjects.Count; i++)
				{
					editorGameObjects[i].Draw(uiBatch);
				}

				uiBatch.End();
			}

			GraphicsDevice.SetRenderTarget(null);

		}
	}
}
