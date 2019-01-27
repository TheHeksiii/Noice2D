namespace Editor
{
    partial class EditorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditorForm));
            this.InspectorWindow = new System.Windows.Forms.PropertyGrid();
            this.ComponentsHeaderPanel = new System.Windows.Forms.Panel();
            this.Label_GameObjectsTitle = new System.Windows.Forms.Label();
            this.btn_Play = new System.Windows.Forms.Button();
            this.Timer_WindowAlign = new System.Windows.Forms.Timer(this.components);
            this.Panel_Main = new System.Windows.Forms.Panel();
            this.btn_ClearConsole = new System.Windows.Forms.Button();
            this.txt_Console = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Label_Tool = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.CheckBox_Physics = new System.Windows.Forms.CheckBox();
            this.Panel_GameObjects = new System.Windows.Forms.Panel();
            this.Hierarchy = new System.Windows.Forms.TreeView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.saveSceneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openSceneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.customizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.indexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ComponentsHeaderPanel.SuspendLayout();
            this.Panel_Main.SuspendLayout();
            this.panel1.SuspendLayout();
            this.Panel_GameObjects.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // InspectorWindow
            // 
            this.InspectorWindow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.InspectorWindow.CategoryForeColor = System.Drawing.Color.White;
            this.InspectorWindow.CategorySplitterColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.InspectorWindow.CommandsBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.InspectorWindow.CommandsDisabledLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(133)))), ((int)(((byte)(133)))), ((int)(((byte)(133)))));
            this.InspectorWindow.CommandsLinkColor = System.Drawing.Color.Blue;
            this.InspectorWindow.DisabledItemForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.InspectorWindow.Dock = System.Windows.Forms.DockStyle.Right;
            this.InspectorWindow.Font = new System.Drawing.Font("Bahnschrift SemiBold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InspectorWindow.HelpBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.InspectorWindow.HelpBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.InspectorWindow.HelpForeColor = System.Drawing.Color.White;
            this.InspectorWindow.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(200)))), ((int)(((byte)(100)))));
            this.InspectorWindow.Location = new System.Drawing.Point(466, 24);
            this.InspectorWindow.Name = "InspectorWindow";
            this.InspectorWindow.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.InspectorWindow.SelectedItemWithFocusForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.InspectorWindow.Size = new System.Drawing.Size(281, 465);
            this.InspectorWindow.TabIndex = 0;
            this.InspectorWindow.TabStop = false;
            this.InspectorWindow.ToolbarVisible = false;
            this.InspectorWindow.ViewBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.InspectorWindow.ViewBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.InspectorWindow.ViewForeColor = System.Drawing.Color.White;
            this.InspectorWindow.Enter += new System.EventHandler(this.InspectorWindow_Enter);
            this.InspectorWindow.Leave += new System.EventHandler(this.InspectorWindow_Leave);
            // 
            // ComponentsHeaderPanel
            // 
            this.ComponentsHeaderPanel.BackColor = System.Drawing.Color.Green;
            this.ComponentsHeaderPanel.Controls.Add(this.Label_GameObjectsTitle);
            this.ComponentsHeaderPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.ComponentsHeaderPanel.Location = new System.Drawing.Point(0, 0);
            this.ComponentsHeaderPanel.Name = "ComponentsHeaderPanel";
            this.ComponentsHeaderPanel.Size = new System.Drawing.Size(202, 41);
            this.ComponentsHeaderPanel.TabIndex = 7;
            // 
            // Label_GameObjectsTitle
            // 
            this.Label_GameObjectsTitle.BackColor = System.Drawing.Color.Gray;
            this.Label_GameObjectsTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.Label_GameObjectsTitle.Font = new System.Drawing.Font("Bahnschrift SemiBold", 13F, System.Drawing.FontStyle.Bold);
            this.Label_GameObjectsTitle.Location = new System.Drawing.Point(0, 0);
            this.Label_GameObjectsTitle.Name = "Label_GameObjectsTitle";
            this.Label_GameObjectsTitle.Size = new System.Drawing.Size(202, 41);
            this.Label_GameObjectsTitle.TabIndex = 6;
            this.Label_GameObjectsTitle.Text = "GAMEOBJECTS";
            this.Label_GameObjectsTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_Play
            // 
            this.btn_Play.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_Play.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btn_Play.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btn_Play.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Play.Font = new System.Drawing.Font("Bahnschrift SemiBold", 18F, System.Drawing.FontStyle.Bold);
            this.btn_Play.ForeColor = System.Drawing.Color.White;
            this.btn_Play.Location = new System.Drawing.Point(3, 68);
            this.btn_Play.Name = "btn_Play";
            this.btn_Play.Size = new System.Drawing.Size(154, 46);
            this.btn_Play.TabIndex = 1;
            this.btn_Play.Text = "Play";
            this.btn_Play.UseVisualStyleBackColor = false;
            this.btn_Play.Visible = false;
            this.btn_Play.Click += new System.EventHandler(this.btn_Play_Click);
            // 
            // Timer_WindowAlign
            // 
            this.Timer_WindowAlign.Enabled = true;
            this.Timer_WindowAlign.Interval = 10;
            this.Timer_WindowAlign.Tick += new System.EventHandler(this.Timer_WindowAlign_Tick);
            // 
            // Panel_Main
            // 
            this.Panel_Main.Controls.Add(this.btn_ClearConsole);
            this.Panel_Main.Controls.Add(this.txt_Console);
            this.Panel_Main.Controls.Add(this.panel1);
            this.Panel_Main.Controls.Add(this.button1);
            this.Panel_Main.Controls.Add(this.CheckBox_Physics);
            this.Panel_Main.Controls.Add(this.btn_Play);
            this.Panel_Main.Controls.Add(this.Panel_GameObjects);
            this.Panel_Main.Controls.Add(this.InspectorWindow);
            this.Panel_Main.Controls.Add(this.menuStrip1);
            this.Panel_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Panel_Main.Location = new System.Drawing.Point(0, 0);
            this.Panel_Main.Name = "Panel_Main";
            this.Panel_Main.Size = new System.Drawing.Size(747, 489);
            this.Panel_Main.TabIndex = 5;
            this.Panel_Main.TabStop = true;
            this.Panel_Main.Click += new System.EventHandler(this.panel1_Click);
            // 
            // btn_ClearConsole
            // 
            this.btn_ClearConsole.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.btn_ClearConsole.Location = new System.Drawing.Point(183, 265);
            this.btn_ClearConsole.Name = "btn_ClearConsole";
            this.btn_ClearConsole.Size = new System.Drawing.Size(75, 23);
            this.btn_ClearConsole.TabIndex = 12;
            this.btn_ClearConsole.Text = "Clear";
            this.btn_ClearConsole.UseVisualStyleBackColor = true;
            this.btn_ClearConsole.Click += new System.EventHandler(this.btn_ClearConsole_Click);
            // 
            // txt_Console
            // 
            this.txt_Console.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.txt_Console.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txt_Console.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.txt_Console.ForeColor = System.Drawing.Color.MintCream;
            this.txt_Console.Location = new System.Drawing.Point(3, 265);
            this.txt_Console.Multiline = true;
            this.txt_Console.Name = "txt_Console";
            this.txt_Console.ReadOnly = true;
            this.txt_Console.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_Console.Size = new System.Drawing.Size(255, 221);
            this.txt_Console.TabIndex = 11;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.Label_Tool);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(264, 41);
            this.panel1.TabIndex = 10;
            // 
            // Label_Tool
            // 
            this.Label_Tool.AutoSize = true;
            this.Label_Tool.Font = new System.Drawing.Font("Bahnschrift SemiBold", 12F, System.Drawing.FontStyle.Bold);
            this.Label_Tool.Location = new System.Drawing.Point(12, 11);
            this.Label_Tool.Name = "Label_Tool";
            this.Label_Tool.Size = new System.Drawing.Size(83, 19);
            this.Label_Tool.TabIndex = 0;
            this.Label_Tool.Text = "Tool Label";
            this.Label_Tool.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Bahnschrift SemiBold", 14F, System.Drawing.FontStyle.Bold);
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(3, 172);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(154, 46);
            this.button1.TabIndex = 9;
            this.button1.Text = "Reload scripts";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_ClickAsync);
            // 
            // CheckBox_Physics
            // 
            this.CheckBox_Physics.Appearance = System.Windows.Forms.Appearance.Button;
            this.CheckBox_Physics.BackColor = System.Drawing.Color.White;
            this.CheckBox_Physics.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.CheckBox_Physics.FlatAppearance.BorderColor = System.Drawing.Color.Tomato;
            this.CheckBox_Physics.FlatAppearance.BorderSize = 2;
            this.CheckBox_Physics.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.CheckBox_Physics.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CheckBox_Physics.Font = new System.Drawing.Font("Bahnschrift SemiBold", 13F, System.Drawing.FontStyle.Bold);
            this.CheckBox_Physics.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.CheckBox_Physics.Location = new System.Drawing.Point(3, 120);
            this.CheckBox_Physics.Name = "CheckBox_Physics";
            this.CheckBox_Physics.Size = new System.Drawing.Size(154, 46);
            this.CheckBox_Physics.TabIndex = 7;
            this.CheckBox_Physics.Text = "Physics ";
            this.CheckBox_Physics.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.CheckBox_Physics.UseVisualStyleBackColor = false;
            this.CheckBox_Physics.CheckedChanged += new System.EventHandler(this.PhysicsCheckBox_CheckedChanged);
            // 
            // Panel_GameObjects
            // 
            this.Panel_GameObjects.Controls.Add(this.ComponentsHeaderPanel);
            this.Panel_GameObjects.Controls.Add(this.Hierarchy);
            this.Panel_GameObjects.Dock = System.Windows.Forms.DockStyle.Right;
            this.Panel_GameObjects.Location = new System.Drawing.Point(264, 24);
            this.Panel_GameObjects.Name = "Panel_GameObjects";
            this.Panel_GameObjects.Size = new System.Drawing.Size(202, 465);
            this.Panel_GameObjects.TabIndex = 6;
            // 
            // Hierarchy
            // 
            this.Hierarchy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.Hierarchy.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Hierarchy.CheckBoxes = true;
            this.Hierarchy.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Hierarchy.ForeColor = System.Drawing.Color.White;
            this.Hierarchy.FullRowSelect = true;
            this.Hierarchy.HideSelection = false;
            this.Hierarchy.HotTracking = true;
            this.Hierarchy.Indent = 10;
            this.Hierarchy.LabelEdit = true;
            this.Hierarchy.LineColor = System.Drawing.Color.White;
            this.Hierarchy.Location = new System.Drawing.Point(0, 44);
            this.Hierarchy.Name = "Hierarchy";
            this.Hierarchy.Size = new System.Drawing.Size(202, 421);
            this.Hierarchy.TabIndex = 5;
            this.Hierarchy.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.Hierarchy_AfterSelect);
            this.Hierarchy.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Hierarchy_MouseDown);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(747, 24);
            this.menuStrip1.TabIndex = 8;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.toolStripSeparator,
            this.saveSceneToolStripMenuItem,
            this.openSceneToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripMenuItem.Image")));
            this.newToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.newToolStripMenuItem.Text = "&New";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripMenuItem.Image")));
            this.openToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openToolStripMenuItem.Text = "&Open";
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(177, 6);
            // 
            // saveSceneToolStripMenuItem
            // 
            this.saveSceneToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveSceneToolStripMenuItem.Image")));
            this.saveSceneToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveSceneToolStripMenuItem.Name = "saveSceneToolStripMenuItem";
            this.saveSceneToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveSceneToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveSceneToolStripMenuItem.Text = "&Save Scene";
            // 
            // openSceneToolStripMenuItem
            // 
            this.openSceneToolStripMenuItem.Name = "openSceneToolStripMenuItem";
            this.openSceneToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openSceneToolStripMenuItem.Text = "Open Scene";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(177, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem,
            this.toolStripSeparator3,
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.toolStripSeparator4,
            this.selectAllToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "&Edit";
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.undoToolStripMenuItem.Text = "&Undo";
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.redoToolStripMenuItem.Text = "&Redo";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(141, 6);
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("cutToolStripMenuItem.Image")));
            this.cutToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.cutToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.cutToolStripMenuItem.Text = "Cu&t";
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("copyToolStripMenuItem.Image")));
            this.copyToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.copyToolStripMenuItem.Text = "&Copy";
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pasteToolStripMenuItem.Image")));
            this.pasteToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.pasteToolStripMenuItem.Text = "&Paste";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(141, 6);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.selectAllToolStripMenuItem.Text = "Select &All";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.customizeToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // customizeToolStripMenuItem
            // 
            this.customizeToolStripMenuItem.Name = "customizeToolStripMenuItem";
            this.customizeToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.customizeToolStripMenuItem.Text = "&Customize";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.optionsToolStripMenuItem.Text = "&Options";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contentsToolStripMenuItem,
            this.indexToolStripMenuItem,
            this.searchToolStripMenuItem,
            this.toolStripSeparator5,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // contentsToolStripMenuItem
            // 
            this.contentsToolStripMenuItem.Name = "contentsToolStripMenuItem";
            this.contentsToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.contentsToolStripMenuItem.Text = "&Contents";
            // 
            // indexToolStripMenuItem
            // 
            this.indexToolStripMenuItem.Name = "indexToolStripMenuItem";
            this.indexToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.indexToolStripMenuItem.Text = "&Index";
            // 
            // searchToolStripMenuItem
            // 
            this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
            this.searchToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.searchToolStripMenuItem.Text = "&Search";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(119, 6);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.aboutToolStripMenuItem.Text = "&About...";
            // 
            // EditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.ClientSize = new System.Drawing.Size(747, 489);
            this.Controls.Add(this.Panel_Main);
            this.Font = new System.Drawing.Font("Bahnschrift SemiBold", 9.75F, System.Drawing.FontStyle.Bold);
            this.ForeColor = System.Drawing.Color.White;
            this.KeyPreview = true;
            this.Name = "EditorForm";
            this.ShowIcon = false;
            this.Text = "EditorForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EditorForm_FormClosing);
            this.ComponentsHeaderPanel.ResumeLayout(false);
            this.Panel_Main.ResumeLayout(false);
            this.Panel_Main.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.Panel_GameObjects.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid InspectorWindow;
        private System.Windows.Forms.Button btn_Play;
        private System.Windows.Forms.Timer Timer_WindowAlign;
        private System.Windows.Forms.Panel Panel_Main;
        private System.Windows.Forms.Panel Panel_GameObjects;
        private System.Windows.Forms.TreeView Hierarchy;
        private System.Windows.Forms.Label Label_GameObjectsTitle;
        private System.Windows.Forms.Panel ComponentsHeaderPanel;
        private System.Windows.Forms.CheckBox CheckBox_Physics;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem saveSceneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openSceneToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem customizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem contentsToolStripMenuItem;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label Label_Tool;
        private System.Windows.Forms.ToolStripMenuItem indexToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.TextBox txt_Console;
        private System.Windows.Forms.Button btn_ClearConsole;
    }
}