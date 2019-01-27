using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Engine;
using Microsoft.Xna.Framework;
using Color = System.Drawing.Color;
using Component = Scripts.Component;

namespace Editor
{
    public partial class EditorForm : Form
    {
        public static EditorForm GetInstance()
        {
            return instance;
        }
        public static EditorForm instance;

        bool ignoreHierarchySelect = false;

        bool updateEditor = true;

        Serializer serializer = new Serializer();
        private bool gameRunning = false;
        public EditorSceneView scene;
        private Thread GameViewThread;
        public PropertyGrid GetInspectorWindow()
        {
            return InspectorWindow;
        }

        public EditorForm()
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

            saveSceneToolStripMenuItem.Click += SaveScene;
            openSceneToolStripMenuItem.Click += LoadScene;

            Hierarchy.AfterCheck += HierarchyItemChecked;

            StartGame();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Delete) && Hierarchy.SelectedNode != null && Hierarchy.SelectedNode.Tag is GameObject)
            {
                ((GameObject)Hierarchy.SelectedNode.Tag).Destroy();
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
        private void SaveScene(object sender, EventArgs e)
        {
            scene.SaveScene();
        }
        private void LoadScene(object sender, EventArgs e)
        {
            InspectorWindow.SelectedObject = null;

            Hierarchy.Nodes.Clear();

            scene.LoadScene();
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
        }

        public void OpenGameView()
        {
            scene = new EditorSceneView();

            Control sceneForm = Form.FromHandle(scene.Window.Handle);

            //scene.Window.Position = Screen.AllScreens[Screen.AllScreens.Length - 1].Bounds.Location.ToGamePoint();
            scene.GameObjectCreated += OnGameObjectCreated;
            scene.GameObjectDestroyed += OnGameObjectDestroyed;
            scene.SceneLoaded += OnSceneLoaded;
            scene.SceneUpdated += OnSceneUpdated;
            scene.Run();// this is end point, dont put anything after Run
        }

        private void OnSceneLoaded(object sender, EventArgs e)
        {
            Hierarchy.Invoke((Action)(() =>
            {
                Hierarchy.Nodes.Clear();
                Hierarchy.Invalidate();
            }));
        }

        private void OnGameObjectDestroyed(object sender, GameObject e)
        {
            for (int i = 0; i < Hierarchy.Nodes.Count; i++)
            {
                if (Hierarchy.Nodes[i].Tag == e)
                {
                    Hierarchy.Nodes.RemoveAt(i);
                    return;
                }
            }
        }
        private void OnSceneUpdated(object sender, SceneData sceneData)
        {
            BeginInvoke((Action)(() =>
            {
                Label_Tool.Text = sceneData.tool.ToString();

                if (updateEditor)
                {
                    InspectorWindow.Refresh();
                    for (int i = 0; i < Hierarchy.Nodes.Count; i++)
                    {
                        Hierarchy.Nodes[i].Checked = ((GameObject)Hierarchy.Nodes[i].Tag).Active;
                        for (int j = 0; j < Hierarchy.Nodes[i].Nodes.Count; j++)
                        {
                            Hierarchy.Nodes[i].Nodes[j].Checked = ((Component)Hierarchy.Nodes[i].Nodes[j].Tag).Enabled;
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
                        for (int j = 0; j < Hierarchy.Nodes[i].Nodes.Count; j++)
                        {
                            if (gameObject.Components.Contains((Component)Hierarchy.Nodes[i].Nodes[j].Tag) == false)
                            {
                                Hierarchy.Nodes[i].Nodes[j].Remove();
                            }
                        }
                        gameObjectNode.Nodes.Add(new TreeNode()
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
            if (gameObject.silentInScene == true) { return; }
            BeginInvoke((Action)(() =>
            {
                gameObject.OnComponentAdded += OnComponentAdded;
                TreeNode[] componentNodes = new TreeNode[gameObject.Components.Count];//we create node for every component

                TreeNode gameObjectNode;//we create GameObject node which will hold other componentNodes


                Hierarchy.Nodes.Add(gameObjectNode = new TreeNode()
                {
                    Tag = gameObject,
                    Text = gameObject.name.ToString(),
                    Checked = gameObject.Active
                });

                for (int i = 0; i < componentNodes.Length; i++)
                {
                    componentNodes[i] = new TreeNode()
                    {
                        Tag = gameObject.Components[i],
                        Text = gameObject.Components[i].ToString(),
                        Checked = gameObject.Components[i].Enabled
                    };
                }

                gameObjectNode.Nodes.AddRange(componentNodes);
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
            if (scene?.IsActive == true)
            {
                GameWindow a = scene.Window;
                this.SetDesktopLocation(a.Position.X + scene.windowSize.X, this.Location.Y);
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
        private void Hierarchy_MouseDown(object sender, MouseEventArgs e)
        {
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
                                ScriptsManager.CreateScript(textBox.Text);
                                (sender as Control).Controls.Remove(textBox);
                            }
                        });
                        //Log.Error("Work In Progress"); });

                    });

                var components = typeof(Component)
                    .Assembly.GetTypes()
                    .Where(t => t.IsSubclassOf(typeof(Component)) && !t.IsAbstract).ToList();
                components.AddRange(ScriptsManager.ScriptsAssembly.GetTypes().Where(t => t.IsSubclassOf(typeof(Component))));


                menu.Items.Add("Components");


                for (int i = 0; i < components.Count; i++)
                {
                    for (int j = 1; j < menu.Items.Count; j++)
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

                        (menu.Items[j] as ToolStripMenuItem).DropDownItems.Add(components[i].Name,
                    null, (obj, EventArgs) => { AddComp(components[tmp]); });
                    }
                }
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
                scene.SelectGameObject((e.Node.Tag as Component).gameObject);
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
            SaveScene(this,null);
            ScriptsManager.CompileScriptsAssembly();

            List<Type> componentTypes = ScriptsManager.ScriptsAssembly.GetTypes().Where(t => t.IsSubclassOf(typeof(Component))).ToList();

            for (int i = 0; i < componentTypes.Count; i++)
            {
                for (int j = 0; j < EditorSceneView.GetInstance().gameObjects.Count; j++)
                {
                    int num = EditorSceneView.GetInstance().gameObjects[j].Components.Count;
                    for (int compIndex = 0; compIndex < num; compIndex++)
                    {

                        if (EditorSceneView.GetInstance().gameObjects[j].Components[compIndex].GetType().ToString() == componentTypes[i].ToString())
                        {
                            var scene = EditorSceneView.GetInstance();

                            scene.gameObjects[j].RemoveComponent(compIndex);

                            scene.gameObjects[j].AddComponent(componentTypes[i]).Awake();
                        }
                    }
                }
            }
            LoadScene(this, null);

        }

        private void btn_ClearConsole_Click(object sender, EventArgs e)
        {
            EditorConsole.Clear();
        }
    }
}
