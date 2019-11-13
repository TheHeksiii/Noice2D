using Engine;
using Microsoft.Xna.Framework;
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
    public partial class Editor : Form
    {
        public static Editor GetInstance()
        {
            return instance;
        }
        public static Editor instance;

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

        public Editor()
        {
            //new MetroForm().Show();
            instance = this;
            InitializeComponent();

            EditorConsole.TextBox = txt_Console;

            ScriptsManager.CompileScriptsAssembly();
            //ScriptsManager.CreateComponentDataScripts(); // TODO
            //ShowInTaskbar = false;

            //InspectorWindow.SelectedObject = testScene;
            MouseWheel += MouseWheelValueChanged;

            InspectorWindow.MouseWheel += MouseWheelValueChanged;
            InspectorWindow.AllowDrop = true;

            saveSceneToolStripMenuItem.Click += SaveSceneClicked;
            openSceneToolStripMenuItem.Click += LoadSceneClicked;

            Hierarchy.AfterCheck += HierarchyItemChecked;

            //UpdateFileExplorer();
            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");


            StartGame();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Delete) && Hierarchy.SelectedNode != null && Hierarchy.SelectedNode.Tag is GameObject)
            {
                ((GameObject)Hierarchy.SelectedNode.Tag).Destroy();
                return true;
            }
            if (keyData == (Keys.Delete) && Hierarchy.SelectedNode != null && Hierarchy.SelectedNode.Tag is Component)
            {
                ((Component)Hierarchy.SelectedNode.Tag).GameObject.RemoveComponent(Hierarchy.SelectedNode.Tag.GetType());
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
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.DefaultExt = ".xml";
                saveFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
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
            }));
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
        PerformanceCounter cpuCounter;

        private void OnSceneUpdated(object sender, SceneData sceneData)
        {
            BeginInvoke((Action)(() =>
            {

                label_updateTime.Text = scene.updateTime.ToString() + " ms";
                label_renderTime.Text = scene.renderTime.ToString() + " ms";
                if (updateEditor)
                {
                    InspectorWindow.Refresh();
                    InspectorWindow.Invalidate();
                    for (int i = 0; i < Hierarchy.Nodes.Count; i++)
                    {
                        Hierarchy.Nodes[i].Checked = ((GameObject)Hierarchy.Nodes[i].Tag).Active;
                        for (int j = 0; j < Hierarchy.Nodes[i].Nodes.Count; j++)
                        {
                            if (Hierarchy.Nodes[i].Nodes[j].Tag is Component)
                            {
                                Hierarchy.Nodes[i].Nodes[j].Checked = ((Component)Hierarchy.Nodes[i].Nodes[j].Tag).Enabled;
                            }
                            else if (Hierarchy.Nodes[i].Nodes[j].Tag is GameObject)
                            {
                                Hierarchy.Nodes[i].Nodes[j].Checked = ((GameObject)Hierarchy.Nodes[i].Nodes[j].Tag).Active;
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
            if (gameObject is SilentGameObject) { return; }
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
            if (gameRunning)
            {
                scene.Exit();
            }
            else
            {
                StartGame();
            }
            btn_Play.FlatAppearance.BorderColor = gameRunning ? Color.White : Color.Aqua;
            gameRunning = !gameRunning;
        }
        private void Timer_WindowAlign_Tick(object sender, EventArgs e)
        {
            if (scene != null)
            {
                this.SetDesktopLocation(scene.Window.Position.X + (Camera.GetInstance() != null ? (int)Camera.GetInstance().Size.X : 800), this.Location.Y);
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
            if (gameObject == null)
            {
                gameObject = scene.GetSelectedGameObject();
            }
            if (gameObject == null) return;
            var comp = gameObject.AddComponent(component);
            comp.Awake();

        }
        private void AddGameObjectNodeChildren(HierarchyNode gameObjectNode)
        {
            if (gameObjectNode.GameObject != null)
            {
                gameObjectNode.Nodes.Clear();

                List<HierarchyNode> componentNodes = new List<HierarchyNode>();//we create node for every component


                // add Transform separately, this way it causes no problems
                componentNodes.Add(new HierarchyNode()
                {
                    Tag = gameObjectNode.GameObject.transform,
                    Text = gameObjectNode.GameObject.transform.ToString(),
                    Checked = gameObjectNode.GameObject.transform.Enabled
                });
                for (int i = 0; i < gameObjectNode.GameObject.Components.Count; i++)
                {
                    if (gameObjectNode.GameObject.Components[i] is Scripts.Transform == false)
                    {
                        componentNodes.Add(new HierarchyNode()
                        {
                            Tag = gameObjectNode.GameObject.Components[i],
                            Text = gameObjectNode.GameObject.Components[i].ToString(),
                            Checked = gameObjectNode.GameObject.Components[i].Enabled
                        });
                    }
                }

                gameObjectNode.Nodes.AddRange(componentNodes.ToArray());

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
            if (Hierarchy.GetNodeAt(e.Location) == null)
            {
                EditorConsole.Log(draggedGameObjectNode.GameObject.Name + " added to global group");

                Hierarchy.Nodes.Remove(draggedGameObjectNode);
                Hierarchy.Nodes.Add(draggedGameObjectNode);

                Hierarchy.SelectedNode = draggedGameObjectNode;

                draggedGameObjectNode = null;
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
                       var go = new GameObject(name: "GameObject");
                       go.Awake();
                   });

                menu.Items.Add(
                    "Create Component",
                    null,
                    delegate
                    {
                        var textBox = new TextBox();
                        (sender as Control).Controls.Add(textBox);
                        //this.Controls.Add(textBox);
                        textBox.BringToFront();
                        textBox.BringToFront();
                        textBox.Location = e.Location;
                        textBox.Focus();
                        textBox.LostFocus += delegate { (sender as Control).Controls.Remove(textBox); };
                        textBox.KeyDown += new KeyEventHandler((object obj, KeyEventArgs kea) =>
                        {
                            if (kea.KeyCode == Keys.Enter)
                            {
                                var type = ScriptsManager.CreateScript(textBox.Text);
                                AddComp(type);
                                (sender as Control).Controls.Remove(textBox);
                            }
                        });
                        //Log.Error("Work In Progress"); });

                    });

                var components = typeof(Component)
                    .Assembly.GetTypes()
                    .Where(t => t.IsSubclassOf(typeof(Component)) && !t.IsAbstract).ToList();
                components.AddRange(ScriptsManager.ScriptsAssembly.GetTypes().Where(t => t.IsSubclassOf(typeof(Component))));


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
                null, (obj, EventArgs) => {
                    AddComp(components[tmp]); }
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
                                    }, sfd.FileName);
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
                                        if (ofd.ShowDialog() == DialogResult.OK)
                                        {
                                            SceneFile sf = Serializer.GetInstance().LoadGameObjects(ofd.FileName);
                                            Serializer.GetInstance().ConnectGameObjectsWithComponents(sf);
                                            if (sf.GameObjects.Count > 0)
                                            {
                                                sf.GameObjects[0].Awake();
                                                scene.gameObjects.Add(sf.GameObjects[0]);
                                            }

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
        private void EditorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            scene.Exit();
        }

        private async void button1_ClickAsync(object sender, EventArgs e)
        {
            //SaveScene(this, null);
            ScriptsManager.CompileScriptsAssembly();

            List<Type> componentTypes = ScriptsManager.ScriptsAssembly.GetTypes().Where(t => t.IsSubclassOf(typeof(Component))).ToList();

            var scene = Scene.GetInstance();

            for (int i = 0; i < componentTypes.Count; i++)
            {
                for (int j = 0; j < scene.gameObjects.Count; j++)
                {
                    int num = scene.gameObjects[j].Components.Count;
                    for (int compIndex = 0; compIndex < num; compIndex++)
                    {

                        if (scene.gameObjects[j].Components[compIndex].GetType().Name == componentTypes[i].Name)
                        {
                            GetNode(scene.gameObjects[j])?.GetNode(scene.gameObjects[j].Components[compIndex])?.Remove();

                            scene.gameObjects[j].RemoveComponent(compIndex);

                            scene.gameObjects[j].AddComponent(componentTypes[i]).Awake();
                        }
                    }
                }
            }
            //LoadScene(this, null);

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
                if (scene.scenePath == "")
                {
                    SaveSceneClicked(this, null);
                }
                else
                {
                    SaveCurrentScene();
                }
                EditorConsole.Log("Scene Saved");
            }

        }
        private void SaveCurrentScene()
        {
            Serializer.GetInstance().SaveGameObjects(scene.GetSceneFile(), scene.scenePath);

        }
    }
}
