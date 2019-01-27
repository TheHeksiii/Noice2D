using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Color = System.Drawing.Color;
namespace Engine.Editor
{
    public partial class EditorForm : Form
    {
        public static EditorForm GetInstance()
        {
            return instance;
        }
        public GameObjectNode CurrentGameObjectNode;
        public static EditorForm instance;
        private bool gameRunning = false;
        public EditorSceneView scene;
        private Thread GameViewThread;
        public PropertyGrid GetInspectorWindow()
        {
            return InspectorWindow;
        }
        public EditorForm()
        {

            instance = this;
            InitializeComponent();

            ScriptsManager.LoadAllScripts();
            //ShowInTaskbar = false;

            //InspectorWindow.SelectedObject = testScene;
            MouseWheel += MouseWheelValueChanged;

            InspectorWindow.MouseWheel += MouseWheelValueChanged;
            StartGame();
        }
        protected override void OnDeactivate(EventArgs e)
        {
            Defocus();
            base.OnDeactivate(e);
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
                        (float)InspectorWindow.SelectedGridItem.Value + e.Delta / 100f);
                }
                InspectorWindow.Refresh();
            }
        }

        bool update = true;
        public void OpenGameView()
        {
            scene = new EditorSceneView();
            scene.GameObjectCreated += OnGameObjectCreated;
            scene.SceneUpdated += OnSceneUpdated;
            scene.Run();// this is end point, dont put anything after Run
        }
        private void OnSceneUpdated(object sender, List<GameObject> gameObjects)
        {
            BeginInvoke((Action)(() =>
            {
                if (update)
                    InspectorWindow.Refresh();
            }));
        }
        public void OnGameObjectCreated(object sender, GameObject gameObject)
        {
            BeginInvoke((Action)(() =>
            {
                GameObjectNode node = new GameObjectNode(gameObject);
                GameObjectsTreeView.Nodes.Add(new TreeNode()
                {
                    Tag = node,
                    Text = gameObject.name.ToString()
                });
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
        bool sceneRunning = false;
        private void Timer_WindowAlign_Tick(object sender, EventArgs e)
        {
            if (scene?.IsActive == true || sceneRunning)
            {
                sceneRunning = true;
                GameWindow a = scene.Window;
                this.SetDesktopLocation(a.Position.X + scene.WindowSize.X, this.Location.Y);
            }
        }
        private void InspectorWindow_Enter(object sender, EventArgs e)
        {
            Global.GameRunning = update = false;
        }
        private void panel1_Click(object sender, EventArgs e)
        {
            update = true;
            Defocus();
        }
        private void Defocus()
        {
            Panel_Main.Focus();
        }
        void AddComp(Type component)
        {
            var comp = CurrentGameObjectNode.GameObject.AddComponentFromType(component);
            CurrentGameObjectNode.ComponentNodes.Add(new ComponentNode(CurrentGameObjectNode, comp));
        }
        private void HierarchyWindow_MouseDown(object sender, MouseEventArgs e)
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
                var comps = typeof(Component)
    .Assembly.GetTypes()
    .Where(t => t.IsSubclassOf(typeof(Component)) && !t.IsAbstract).ToList();
                //.Select(t => (Component)Activator.CreateInstance(t));
                for (int i = 0; i < comps.Count; i++)
                {
                    var tmp = i;
                    menu.Items.Add(comps[i].ToString(),
                        null, (obj, EventArgs) => { AddComp(comps[tmp]); });

                }
                var types = ScriptsManager.ScriptsAssembly.GetTypes().Where(t => t.IsSubclassOf(typeof(Component)))
                    .ToList();
                for (int i = 0; i < types.Count; i++)
                {
                    var tmp = i;
                    menu.Items.Add(types[i].ToString(),
                        null, (obj, EventArgs) => { AddComp(types[tmp]); });

                }


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
                        var a = 0;
                        //MessageBox.Show("Work In Progress"); });

                    });
                menu.Show(sender as Control, e.Location);
            }
        }



        private void ComponentsTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            InspectorWindow.SelectedObject = e.Node.Tag;
        }

        private void GameObjectsTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            CurrentGameObjectNode = e.Node.Tag as GameObjectNode;
            PopulateComponentsTreeView(CurrentGameObjectNode);
        }
        private void PopulateComponentsTreeView(GameObjectNode gameObjectNode)
        {
            ComponentsTreeView.Nodes.Clear();
            for (int i = 0; i < gameObjectNode.ComponentNodes.Count; i++)
            {
                ComponentsTreeView.Nodes.Add(new TreeNode()
                {
                    Tag = gameObjectNode.ComponentNodes[i],
                    Text = gameObjectNode.ComponentNodes[i].Name
                });
            }
        }
        private void InspectorWindow_Leave(object sender, EventArgs e)
        {
            Global.GameRunning = update = true;
        }

        private void PhysicsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox_Physics.Checked)
            {
                Physics.GetInstance().StartPhysics();
            }
            else
            {
                Physics.GetInstance().StopPhysics();
            }
            CheckBox_Physics.FlatAppearance.BorderColor = CheckBox_Physics.Checked ? Color.Aqua : Color.Tomato;

        }
        private void EditorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            scene.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ScriptsManager.LoadAllScripts();
        }
    }
}
