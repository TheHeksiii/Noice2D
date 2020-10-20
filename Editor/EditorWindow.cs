using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Color = System.Drawing.Color;
using Component = Scripts.Component;

namespace Editor
{
	public partial class EditorWindow : Form
	{
		public static EditorWindow GetInstance()
		{
			return instance;
		}
		public static EditorWindow instance;

		bool ignoreHierarchySelect = false;

		bool updateEditor = true;

		Serializer serializer = new Serializer();
		private bool gameRunning = false;
		public Scene scene;
		private Thread GameViewThread;
		public PropertyGrid GetInspectorWindow()
		{
			return InspectorWindow;
		}

		public EditorWindow()
		{
			instance = this;
			InitializeComponent();

			EditorConsole.TextBox = txt_Console;

			//ScriptsManager.CreateComponentDataScripts(); // TODO
			//ShowInTaskbar = false;

			//InspectorWindow.SelectedObject = testScene;
			MouseWheel += MouseWheelValueChanged;

			InspectorWindow.MouseWheel += MouseWheelValueChanged;
			InspectorWindow.AllowDrop = true;

			saveSceneAsToolStripMenuItem.Click += SaveSceneAsClicked;
			saveSceneToolStripMenuItem.Click += SaveSceneClicked;

			openSceneToolStripMenuItem.Click += LoadSceneClicked;

			Hierarchy.AfterCheck += HierarchyItemChecked;

			Engine.Debug.Console.OnMessageLogged += (obj) => { EditorConsole.Log(obj); };
			//UpdateFileExplorer();
			cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");

			ScriptsManager.CompileScriptsAssembly();
			AttributeUpdater.Update();

			StartGame();
		}
		void PopulateCustomEditorsActions()
		{

			/*Engine.UITypeEditors.BoolEditor.BoolEditor_Paint += (e) =>
			{
				var rect = e.Bounds;
				rect.Inflate(1, 1);
				ControlPaint.DrawCheckBox(e.Graphics, rect, ButtonState.Flat |
					(((bool)e.Value) ? ButtonState.Checked : ButtonState.Normal));
				//ControlPaint.DrawButton(e.Graphics, rect, ButtonState.Flat |
				//(((bool)e.Value) ? ButtonState.Checked : ButtonState.Normal));
			};*/
			Engine.UITypeEditors.ColorPickerEditor.ColorPickerEditor_EditValue += (context, provider, value) =>
			{
				void SetColorInClass(object instance, Microsoft.Xna.Framework.Color color, string propertyName)
				{
					Type sourceType = instance.GetType();
					PropertyInfo colorField = sourceType.GetProperty(propertyName);
					if (colorField == null)
					{
						colorField = sourceType.GetProperty(propertyName);
					}
					colorField?.SetValue(instance, color, null);
				};

				System.Windows.Forms.Design.IWindowsFormsEditorService editorService = null;
				if (provider != null)
				{
					editorService = provider.GetService(typeof(System.Windows.Forms.Design.IWindowsFormsEditorService))
						  as System.Windows.Forms.Design.IWindowsFormsEditorService;
				}

				if (editorService != null)
				{
					Panel panel = new Panel();
					panel.Size = new System.Drawing.Size(300, 400);

					ColorPicker colorPicker = new ColorPicker();

					panel.Controls.Add(colorPicker);
					colorPicker.ColorMap.OnColorChanged += delegate
						  {
							  SetColorInClass(context.Instance, colorPicker.color, ((ITypeDescriptorContext)context).PropertyDescriptor.Name);
							  value = colorPicker.color;
						  };
					editorService.DropDownControl(panel);
				}
			};
			Engine.UITypeEditors.TextureEditor.TextureEditor_EditValue += (context, provider, value) =>
			{

				using (OpenFileDialog openFileDialog = new OpenFileDialog())
				{
					openFileDialog.InitialDirectory = Path.Combine(Directory.GetCurrentDirectory(), "2D");

					DialogResult dialogResult = openFileDialog.ShowDialog();
					if (dialogResult == DialogResult.OK)
					{
						string assetsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AssetsInUse");
						Directory.CreateDirectory(assetsPath);

						//string newFilePath = Path.Combine(assetsPath, openFileDialog.SafeFileName);

						//File.Copy(openFileDialog.FileName, newFilePath, overwrite: true);

						string texturePath = openFileDialog.FileName;
						texturePath = texturePath.Remove(0, Directory.GetCurrentDirectory().Length + 1);
						if (context.Instance is Scripts.SpriteRenderer)
						{
							(context.Instance as Scripts.SpriteRenderer).LoadTexture(texturePath);
						}
						if (context.Instance is Scripts.ParticleSystemRenderer)
						{
							(context.Instance as Scripts.ParticleSystemRenderer).LoadTexture(texturePath);
						}
					}
				}
			};
			Engine.UITypeEditors.EffectEditor.EffectEditor_EditValue += (context, provider, value) =>
			{
				using (OpenFileDialog openFileDialog = new OpenFileDialog())
				{
					DialogResult dialogResult = openFileDialog.ShowDialog();
					if (dialogResult == DialogResult.OK)
					{


						//(context.Instance as Scripts.Renderer).effect = EditorSceneView.GetInstance().Content.Load<Effect>(openFileDialog.FileName);
						value = Scene.Instance.Content.Load<Effect>(openFileDialog.FileName.Replace(".xnb", ""));
					}
				}
			};
		}
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == (Keys.Delete) && GetSelectedGameObject() != null && Hierarchy.Focused)
			{
				GetSelectedGameObject().Destroy();
				return true;
			}
			if (keyData == (Keys.Delete) && Hierarchy.SelectedNode != null && GetSelectedComponent() != null && Hierarchy.Focused)
			{
				GetSelectedComponent().GameObject.RemoveComponent(GetSelectedComponent().GetType());
				Hierarchy.SelectedNode.Remove();

				return true;
			}
			return base.ProcessCmdKey(ref msg, keyData);
		}
		private void HierarchyItemChecked(object sender, TreeViewEventArgs e)
		{
			if (e.Node.Tag is GameObject)
			{
				((GameObject)e.Node.Tag).Active = e.Node.Checked;
			}
			else if (e.Node.Tag is Component)
			{
				((Component)e.Node.Tag).Enabled = e.Node.Checked;
			}
			//InspectorWindow.Refresh();
			//InspectorWindow.Invalidate();
			//Invalidate();
		}
		/*private void UpdateFileExplorer()
		{
			ImageList imageList = new ImageList();
			imageList.ImageSize = new Size(50, 50);
			imageList.ColorDepth = ColorDepth.Depth32Bit;
			listView1.Items.Clear();

			string[] files = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "Assets"));

			for (int i = 0; i < files.Length; i++)
			{
				if (files[i].Contains(".prefab") == false)
				{
					continue;
				}
				imageList.Images.Add(key: i.ToString(), image: Image.FromFile(files[i]));
				listView1.Items.Add(key: i.ToString(), text: "x", imageIndex: i);
			}
			listView1.LargeImageList = imageList;
		}*/
		private void SaveSceneClicked(object sender, EventArgs e)
		{
			if (scene.scenePath == "")
			{
				SaveSceneAsClicked(this, null);
			}
			else
			{
				SaveCurrentScene();
			}
		}
		private void SaveSceneAsClicked(object sender, EventArgs e)
		{
			using (SaveFileDialog saveFileDialog = new SaveFileDialog())
			{
				if (scene.scenePath.Length > 0)
				{
					saveFileDialog.InitialDirectory = scene.scenePath;
					saveFileDialog.FileName = scene.scenePath.Substring(scene.scenePath.LastIndexOf("\\") + 1);

				}
				else
				{
					saveFileDialog.InitialDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Scenes");
				}
				saveFileDialog.DefaultExt = "scene.xml";
				saveFileDialog.Title = "Save Scene";
				DialogResult dialogResult = saveFileDialog.ShowDialog();
				if (dialogResult == DialogResult.OK)
				{
					scene.scenePath = saveFileDialog.FileName;
					Serializer.GetInstance().SaveGameObjects(
						scene.GetSceneFile(),
						scene.scenePath);
				}
			}
		}
		private void LoadSceneClicked(object sender, EventArgs e)
		{
			if (scene.LoadScene())
			{
				InspectorWindow.SelectedObject = null;

				Hierarchy.Nodes.Clear();
			}
		}


		private void MouseWheelValueChanged(object sender, MouseEventArgs e)
		{
			if (InspectorWindow.SelectedGridItem?.Value is float)
			{
				if (InspectorWindow.SelectedGridItem.PropertyDescriptor.Name == "Rotation")
				{
					InspectorWindow.SelectedGridItem.PropertyDescriptor.SetValue(InspectorWindow.SelectedObject,
	(float)InspectorWindow.SelectedGridItem.Value + e.Delta / 10000f);
				}
				else
				{
					InspectorWindow.SelectedGridItem.PropertyDescriptor.SetValue(InspectorWindow.SelectedObject,
						(float)InspectorWindow.SelectedGridItem.Value + e.Delta / 10000f);
				}
				InspectorWindow.Refresh();
			}
			if (InspectorWindow.SelectedGridItem?.Value is Vector3)
			{
				InspectorWindow.SelectedGridItem.PropertyDescriptor.SetValue(InspectorWindow.SelectedObject,
					(Vector3)InspectorWindow.SelectedGridItem.Value + new Vector3(e.Delta, e.Delta, e.Delta) / 1000f);
				InspectorWindow.Refresh();
			}
		}

		public void OpenGameView()
		{
			scene = new Scene();
			Control sceneForm = Form.FromHandle(scene.Window.Handle);
			PopulateCustomEditorsActions();
			//scene.Window.Position = Screen.AllScreens[Screen.AllScreens.Length - 1].Bounds.Location.ToGamePoint();
			scene.GameObjectCreated += OnGameObjectCreated;
			scene.GameObjectDestroyed += OnGameObjectDestroyed;
			scene.SceneLoad += OnSceneLoad;
			//scene.SceneLoaded += FixReferences;
			scene.SceneUpdated += OnSceneUpdated;


			scene.Run();// this is end point, dont put anything after Run


		}

		private void OnSceneLoad(object sender, EventArgs e)
		{
			Hierarchy.Invoke((Action)(() =>
			{
				Hierarchy.Nodes.Clear();
				Hierarchy.Invalidate();

				for (int i = 0; i < scene.gameObjects.Count; i++)
				{
					OnGameObjectCreated(this, scene.gameObjects[i]);
				}
			}));

			// 

		}

		private void OnGameObjectDestroyed(object sender, GameObject e)
		{
			Hierarchy.Invoke(new Action(() =>
			{
				for (int i = 0; i < Hierarchy.Nodes.Count; i++)
				{
					if (Hierarchy.Nodes[i].Tag == e)
					{
						Hierarchy.Nodes.RemoveAt(i);
						return;
					}
				}
			}));
		}
		public GameObject GetSelectedGameObject()
		{
			if (Hierarchy.SelectedNode?.Tag is GameObject == false)
			{
				return null;
			}
			return (GameObject)Hierarchy.SelectedNode?.Tag;
		}
		public Component GetSelectedComponent()
		{

			if (Hierarchy.SelectedNode?.Tag is Component == false)
			{
				return null;
			}

			return ((Component)Hierarchy.SelectedNode.Tag);

		}

		PerformanceCounter cpuCounter;

		private void OnSceneUpdated(object sender, SceneData sceneData)
		{
			BeginInvoke((Action)(() =>
			{

				label_updateTime.Text = scene.updateTime.ToString() + " ms";
				label_renderTime.Text = scene.renderTime.ToString() + " ms";
				if (updateEditor)
				{
					if (Time.elapsedTicks % 60 == 0)
					{
						InspectorWindow.Refresh();
						InspectorWindow.Invalidate();
					}
				}
				for (int i = 0; i < Hierarchy.Nodes.Count; i++)
				{
					Hierarchy.Nodes[i].Checked = ((GameObject)Hierarchy.Nodes[i].Tag).Active;
					if (Hierarchy.Nodes[i].Text != ((GameObject)Hierarchy.Nodes[i].Tag).Name)
					{
						Hierarchy.Nodes[i].Text = ((GameObject)Hierarchy.Nodes[i].Tag).Name;
					}

					for (int j = 0; j < Hierarchy.Nodes[i].Nodes.Count; j++)
					{
						if (Hierarchy.Nodes[i].Nodes[j].Tag is Component)
						{
							Hierarchy.Nodes[i].Nodes[j].Checked = ((Component)Hierarchy.Nodes[i].Nodes[j].Tag).Enabled;
						}
						else if (Hierarchy.Nodes[i].Nodes[j].Tag is GameObject)
						{
							Hierarchy.Nodes[i].Nodes[j].Checked = ((GameObject)Hierarchy.Nodes[i].Nodes[j].Tag).Active;
							if (Hierarchy.Nodes[i].Nodes[j].Text != ((GameObject)Hierarchy.Nodes[i].Nodes[j].Tag).Name)
							{
								Hierarchy.Nodes[i].Nodes[j].Text = ((GameObject)Hierarchy.Nodes[i].Nodes[j].Tag).Name;
							}
						}
					}

				}
			}));

		}

		private void OnComponentAdded(GameObject gameObject, Component component)
		{
			for (int i = 0; i < Hierarchy.Nodes.Count; i++)
			{
				if (Hierarchy.Nodes[i].Tag == gameObject)
				{
					TreeNode gameObjectNode;

					gameObjectNode = Hierarchy.Nodes[i];

					Hierarchy.Invoke((Action)(() =>
					{
						gameObjectNode.Nodes.Add(new HierarchyNode()
						{
							Tag = component,
							Text = component.ToString(),
							Checked = component.Enabled
						});
					}));
				}
			}
		}

		public void OnGameObjectCreated(object sender, GameObject gameObject)
		{
			if (gameObject.parentID != -1) { return; }
			if (gameObject.silent) { return; }
			for (int i = 0; i < Hierarchy.Nodes.Count; i++)
			{
				if (Hierarchy.Nodes[i].Tag is GameObject)
				{
					//var id1 = (Hierarchy.Nodes[i].Tag as GameObject).ID;
					if ((Hierarchy.Nodes[i].Tag as GameObject).ID == gameObject.ID) { return; }
				}
			}
			Engine.Debug.Console.Log("Added ID " + gameObject.ID);
			BeginInvoke((Action)(() =>
			{
				gameObject.OnComponentAdded += OnComponentAdded;
				HierarchyNode[] componentNodes = new HierarchyNode[gameObject.Components.Count];//we create node for every component

				HierarchyNode gameObjectNode;//we create GameObject node which will hold other componentNodes


				Hierarchy.Nodes.Add(gameObjectNode = new HierarchyNode()
				{
					Tag = gameObject,
					Text = gameObject.Name.ToString(),
					Checked = gameObject.Active,
					ForeColor = EditorStyle.GameObjectNodeForeColor
				});
				gameObjectNode.ForeColor = Color.FromArgb(66, 206, 244);
				AddGameObjectNodeChildren(gameObjectNode);
			}));
		}

		private void StartGame()
		{
			if (gameRunning == false)
			{
				GameViewThread = new Thread(OpenGameView);
				GameViewThread.Name = "GameView Thread";
				GameViewThread.Start();
			}
		}
		private void btn_Play_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < scene.gameObjects.Count; i++)
			{
				for (int j = 0; j < scene.gameObjects[i].Components.Count; j++)
				{
					scene.gameObjects[i].Components[j].Awake();
				}
			}
			//if (gameRunning)
			//{
			//	scene.Exit();
			//}
			//else
			//{
			//StartGame();
			//}
			//btn_Play.FlatAppearance.BorderColor = gameRunning ? Color.White : Color.Aqua;
			//gameRunning = !gameRunning;
		}
		private void Timer_WindowAlign_Tick(object sender, EventArgs e)
		{
			if (scene != null)
			{
				this.SetDesktopLocation(scene.Window.Position.X + (Camera.Instance != null ? (int)Camera.Instance.Size.X : 800), this.Location.Y);
			}
		}
		private void InspectorWindow_Enter(object sender, EventArgs e)
		{
			updateEditor = false;
		}
		private void panel1_Click(object sender, EventArgs e)
		{
			updateEditor = true;
			Defocus();
		}
		private void Defocus()
		{
			Panel_Main.Focus();
		}
		void AddComp(Type component, GameObject gameObject = null)
		{
			if ((gameObject = GetSelectedGameObject()) == null)
			{
				gameObject = GetSelectedComponent()?.GameObject;
			}
			if (gameObject == null) return;

			var comp = gameObject.AddComponent(component);
		}
		private void AddGameObjectNodeChildren(HierarchyNode gameObjectNode)
		{
			if (gameObjectNode.GameObject != null)
			{
				gameObjectNode.Nodes.Clear();

				List<HierarchyNode> componentNodes = new List<HierarchyNode>();//we create node for every component


				// add Transform separately, this way it causes no problems
				/*componentNodes.Add(new HierarchyNode()
				{
					  Tag = gameObjectNode.GameObject.transform,
					  Text = gameObjectNode.GameObject.transform.ToString(),
					  Checked = gameObjectNode.GameObject.transform.Enabled
				});*/
				for (int i = 0; i < gameObjectNode.GameObject.Components.Count; i++)
				{

					componentNodes.Add(new HierarchyNode()
					{
						Tag = gameObjectNode.GameObject.Components[i],
						Text = gameObjectNode.GameObject.Components[i].ToString(),
						Checked = gameObjectNode.GameObject.Components[i].Enabled
					});

				}

				gameObjectNode.Nodes.AddRange(componentNodes.ToArray());
				for (int i = 0; i < gameObjectNode.Nodes.Count; i++)
				{
					gameObjectNode.Nodes[i].Tag = gameObjectNode.GameObject.Components[i];
				}

				for (int i = 0; i < gameObjectNode.GameObject.GameObjects.Count; i++)
				{
					var childNode = new HierarchyNode()
					{
						Tag = gameObjectNode.GameObject.GameObjects[i],
						Text = gameObjectNode.GameObject.GameObjects[i].Name.ToString(),
						Checked = gameObjectNode.GameObject.GameObjects[i].Active,
						ForeColor = EditorStyle.GameObjectNodeForeColor
					};
					gameObjectNode.Nodes.Add(childNode);

					AddGameObjectNodeChildren(childNode);
				}


			}

		}
		HierarchyNode draggedGameObjectNode;

		private void Hierarchy_MouseUp(object sender, MouseEventArgs e)
		{
			if (draggedGameObjectNode == null)
			{
				return;
			}
			if (e.Button != MouseButtons.Left) { return; }
			if (Hierarchy.GetNodeAt(e.Location) == null)
			{
				Hierarchy.SelectedNode = null;

				// wtf ?
				/*Hierarchy.Nodes.Remove(draggedGameObjectNode);
				Hierarchy.Nodes.Add(draggedGameObjectNode);

				Hierarchy.SelectedNode = draggedGameObjectNode;

				draggedGameObjectNode = null;*/
			}
			else if (Hierarchy.GetNodeAt(e.Location).Tag is GameObject)
			{
				HierarchyNode destinationNode = (HierarchyNode)Hierarchy.GetNodeAt(e.Location);
				GameObject parent = (GameObject)destinationNode.Tag;
				if (draggedGameObjectNode.Tag != parent)
				{
					EditorConsole.Log(draggedGameObjectNode.GameObject.Name + " added to " + parent.Name);
					parent.GameObjects.Add(draggedGameObjectNode.GameObject);

					draggedGameObjectNode.GameObject.SetParent(parent);

					AddNodeToNewParent(childGameObject: draggedGameObjectNode.GameObject, parent: destinationNode);
					AddGameObjectNodeChildren(destinationNode);
					Hierarchy.Nodes.Remove(draggedGameObjectNode);

					Hierarchy.SelectedNode = draggedGameObjectNode;
					draggedGameObjectNode = null;
				}
			}
			void AddNodeToNewParent(GameObject childGameObject, HierarchyNode parent)
			{
				HierarchyNode gameObjectNode;//we create GameObject node which will hold other componentNodes

				parent.Nodes.Add(gameObjectNode = new HierarchyNode()
				{
					Tag = childGameObject,
					Text = childGameObject.Name.ToString(),
					Checked = childGameObject.Active
				});

				HierarchyNode[] childNodes = new HierarchyNode[childGameObject.GameObjects.Count];//we create node for every component

				for (int i = 0; i < childGameObject.GameObjects.Count; i++)
				{
					childNodes[i] = new HierarchyNode()
					{
						Tag = childGameObject.GameObjects[i],
						Text = childGameObject.GameObjects[i].ToString(),
						Checked = childGameObject.GameObjects[i].Active
					};
				}

				gameObjectNode.Nodes.AddRange(childNodes);
			}
		}
		private void Hierarchy_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				if (Hierarchy.GetNodeAt(e.Location)?.Tag is GameObject)
				{
					draggedGameObjectNode = (HierarchyNode)Hierarchy.GetNodeAt(e.Location);
				}
			}
			if (e.Button == MouseButtons.Right)
			{
				// Submenu stuff
				/*ToolStripMenuItem componentsMenu = new ToolStripMenuItem("Components");
				ToolStripMenuItem createComponentItem = new ToolStripMenuItem("Create component");

				componentsMenu.DropDownItems.Add(createComponentItem);
				menu.Items.Add(componentsMenu);
				*/
				ContextMenuStrip menu = new ContextMenuStrip();
				menu.Items.Add(
				   "======== Create Object",
				   null,
				   delegate
				   {
					   var go = GameObject.Create(name: "GameObject");
					   go.Awake();
				   });


				var components = typeof(Component)
					.Assembly.GetTypes()
					.Where(t => t.IsSubclassOf(typeof(Component)) && !t.IsAbstract).ToList();


				ToolStripMenuItem menuToolStripItem = (ToolStripMenuItem)menu.Items.Add("Components");


				for (int i = 0; i < components.Count; i++)
				{

					Type typ = components[i].BaseType;
					do
					{
						if (typ == typeof(Component)) break;
						typ = typ.BaseType;
					}
					while (typ.BaseType != null);

					var tmp = i;
					var txt = components[i].ToString();

					menuToolStripItem.DropDownItems.Add(components[i].Name,
				null, (obj, EventArgs) =>
				{
					AddComp(components[tmp]);
				}
				);

				}
				if (InspectorWindow.SelectedObject is GameObject)
				{
					menu.Items.Add(
						"Save Prefab",
						null,
						delegate
						{
							using (SaveFileDialog sfd = new SaveFileDialog())
							{
								sfd.InitialDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Prefabs");

								sfd.AddExtension = true;
								sfd.DefaultExt = ".prefab";
								sfd.Title = "Save prefab";
								sfd.Filter = "GameObject prefab|*.prefab";
								sfd.FilterIndex = 0;
								if (sfd.ShowDialog() == DialogResult.OK)
								{
									Serializer.GetInstance().SaveGameObjects(new SceneFile()
									{
										GameObjects = new List<GameObject>() { InspectorWindow.SelectedObject as GameObject },
										Components = (InspectorWindow.SelectedObject as GameObject).Components
									}, sfd.FileName, isPrefab: true);
								}
							}
						});
				}
				menu.Items.Add(
								"Load Prefab",
								null,
								delegate
								{
									using (OpenFileDialog ofd = new OpenFileDialog())
									{
										ofd.InitialDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Prefabs");
										if (ofd.ShowDialog() == DialogResult.OK)
										{

											SceneFile sf = Serializer.GetInstance().LoadGameObjects(ofd.FileName);

											// sort out new IDs
											sf.GameObjects[0].ID = IDsManager.gameObjectNextID;
											IDsManager.gameObjectNextID++;
											for (int i = 0; i < sf.Components.Count; i++)
											{
												sf.Components[i].gameObjectID = sf.GameObjects[0].ID;
											}
											Serializer.GetInstance().ConnectGameObjectsWithComponents(sf);

											Scene.Instance.OnGameObjectCreated(sf.GameObjects[0]);


											sf.GameObjects[0].Awake();
											//scene.gameObjects.Add(sf.GameObjects[0]);

										}
									}
								});
				menu.Show(sender as Control, e.Location);
			}
		}
		private void Hierarchy_AfterSelect(object sender, TreeViewEventArgs e)
		{
			/*if (e.Node.Tag is ComponentData)
			{*/
			if (ignoreHierarchySelect) { return; }

			var attrib = typeof(ShowInEditor).GetCustomAttribute<ShowInEditor>();

			InspectorWindow.SelectedObject = e.Node.Tag;

			InspectorWindow.BrowsableAttributes = new AttributeCollection(attrib);

			if (e.Node.Tag is GameObject)// on click in hierarchy, select gameObject in scene
			{
				scene.SelectGameObject((e.Node.Tag as GameObject));
			}
			else if (e.Node.Tag is Component)
			{
				scene.SelectGameObject((e.Node.Tag as Component).GameObject);
			}
			//}
		}
		private void InspectorWindow_Leave(object sender, EventArgs e)
		{
			updateEditor = true;
		}

		private void PhysicsCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (CheckBox_Physics.Checked)
			{
				Physics.StartPhysics();
			}
			else
			{
				Physics.StopPhysics();
			}
			CheckBox_Physics.FlatAppearance.BorderColor = CheckBox_Physics.Checked ? Color.Aqua : Color.Tomato;

		}
		private void DeleteUnusedCachedAssets()
		{
			string[] files = Directory.GetFiles("AssetsInUse");
			for (int i = 0; i < scene.gameObjects.Count; i++)
			{
				Scripts.SpriteRenderer spriteRenderer;
				if ((spriteRenderer = scene.gameObjects[i].GetComponent<Scripts.SpriteRenderer>()) != null)
				{
					for (int j = 0; j < files.Length; j++)
					{
						if (files[j] == spriteRenderer.texturePath)
						{
							files[j] = "";
						}
					}
				}
			}
			for (int i = 0; i < files.Length; i++)
			{
				if (files[i] != "")
				{
					File.Delete(files[i]);

				}
			}
		}
		private void EditorForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			DeleteUnusedCachedAssets();
			scene.Exit();
		}
		private HierarchyNode GetNode(object obj)
		{
			for (int i = 0; i < Hierarchy.Nodes.Count; i++)
			{
				if ((Hierarchy.Nodes[i] as HierarchyNode).GameObject == obj ||
					(Hierarchy.Nodes[i] as HierarchyNode).Component == obj) { return (Hierarchy.Nodes[i] as HierarchyNode); }
			}
			return null;
		}
		private void btn_ClearConsole_Click(object sender, EventArgs e)
		{
			EditorConsole.Clear();
		}

		/*private void ListView1_ItemDrag(object sender, ItemDragEventArgs e)
		{
		   DoDragDrop(e.Item, DragDropEffects.Link);

		}*/

		private void InspectorWindow_DragDrop(object sender, DragEventArgs e)
		{
		}

		private void InspectorWindow_GiveFeedback(object sender, GiveFeedbackEventArgs e)
		{

		}

		private void Editor_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && e.KeyCode == Keys.S)
			{
				SaveSceneClicked(this, null);
			}

		}
		private void SaveCurrentScene()
		{
			Serializer.GetInstance().SaveGameObjects(scene.GetSceneFile(), scene.scenePath);

		}

		private bool MouseIsOverControl(System.Windows.Forms.Button btn)
		{
			if (btn.ClientRectangle.Contains(btn.PointToClient(Cursor.Position)))
			{
				return true;
			}
			return false;
		}
	}
}
