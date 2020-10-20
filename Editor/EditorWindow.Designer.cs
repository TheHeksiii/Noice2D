namespace Editor
{
    partial class EditorWindow
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
                  System.Windows.Forms.Label label_titleCPUTime;
                  System.Windows.Forms.Label label2;
                  System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditorWindow));
                  System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
                  this.InspectorWindow = new System.Windows.Forms.PropertyGrid();
                  this.ComponentsHeaderPanel = new System.Windows.Forms.Panel();
                  this.Label_GameObjectsTitle = new System.Windows.Forms.Label();
                  this.btn_Play = new System.Windows.Forms.Button();
                  this.Timer_WindowAlign = new System.Windows.Forms.Timer(this.components);
                  this.Panel_Main = new System.Windows.Forms.Panel();
                  this.button1 = new System.Windows.Forms.Button();
                  this.CheckBox_Physics = new System.Windows.Forms.CheckBox();
                  this.Panel_GameObjects = new System.Windows.Forms.Panel();
                  this.Hierarchy = new System.Windows.Forms.TreeView();
                  this.menuStrip1 = new System.Windows.Forms.MenuStrip();
                  this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                  this.saveSceneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                  this.saveSceneAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                  this.openSceneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                  this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                  this.btn_ClearConsole = new System.Windows.Forms.Button();
                  this.txt_Console = new System.Windows.Forms.TextBox();
                  this.label_updateTime = new System.Windows.Forms.Label();
                  this.label_renderTime = new System.Windows.Forms.Label();
                  label_titleCPUTime = new System.Windows.Forms.Label();
                  label2 = new System.Windows.Forms.Label();
                  toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
                  this.ComponentsHeaderPanel.SuspendLayout();
                  this.Panel_Main.SuspendLayout();
                  this.Panel_GameObjects.SuspendLayout();
                  this.menuStrip1.SuspendLayout();
                  this.SuspendLayout();
                  // 
                  // label_titleCPUTime
                  // 
                  label_titleCPUTime.AutoSize = true;
                  label_titleCPUTime.Location = new System.Drawing.Point(12, 539);
                  label_titleCPUTime.Name = "label_titleCPUTime";
                  label_titleCPUTime.Size = new System.Drawing.Size(49, 16);
                  label_titleCPUTime.TabIndex = 6;
                  label_titleCPUTime.Text = "Update";
                  // 
                  // label2
                  // 
                  label2.AutoSize = true;
                  label2.Location = new System.Drawing.Point(12, 564);
                  label2.Name = "label2";
                  label2.Size = new System.Drawing.Size(50, 16);
                  label2.TabIndex = 8;
                  label2.Text = "Render";
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
                  this.InspectorWindow.Size = new System.Drawing.Size(281, 488);
                  this.InspectorWindow.TabIndex = 0;
                  this.InspectorWindow.TabStop = false;
                  this.InspectorWindow.ToolbarVisible = false;
                  this.InspectorWindow.ViewBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
                  this.InspectorWindow.ViewBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
                  this.InspectorWindow.ViewForeColor = System.Drawing.Color.White;
                  this.InspectorWindow.DragDrop += new System.Windows.Forms.DragEventHandler(this.InspectorWindow_DragDrop);
                  this.InspectorWindow.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.InspectorWindow_GiveFeedback);
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
                  this.Label_GameObjectsTitle.Font = new System.Drawing.Font("Bahnschrift SemiBold", 15F, System.Drawing.FontStyle.Bold);
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
                  this.btn_Play.Text = "Setup";
                  this.btn_Play.UseVisualStyleBackColor = false;
                  //this.btn_Play.Visible = false;
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
                  this.Panel_Main.Controls.Add(this.button1);
                  this.Panel_Main.Controls.Add(this.CheckBox_Physics);
                  this.Panel_Main.Controls.Add(this.btn_Play);
                  this.Panel_Main.Controls.Add(this.Panel_GameObjects);
                  this.Panel_Main.Controls.Add(this.InspectorWindow);
                  this.Panel_Main.Controls.Add(this.menuStrip1);
                  this.Panel_Main.Dock = System.Windows.Forms.DockStyle.Top;
                  this.Panel_Main.Location = new System.Drawing.Point(0, 0);
                  this.Panel_Main.Name = "Panel_Main";
                  this.Panel_Main.Size = new System.Drawing.Size(747, 512);
                  this.Panel_Main.TabIndex = 5;
                  this.Panel_Main.TabStop = true;
                  this.Panel_Main.Click += new System.EventHandler(this.panel1_Click);
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
                  this.Panel_GameObjects.Size = new System.Drawing.Size(202, 488);
                  this.Panel_GameObjects.TabIndex = 6;
                  // 
                  // Hierarchy
                  // 
                  this.Hierarchy.AllowDrop = true;
                  this.Hierarchy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
                  this.Hierarchy.BorderStyle = System.Windows.Forms.BorderStyle.None;
                  this.Hierarchy.CheckBoxes = true;
                  this.Hierarchy.Dock = System.Windows.Forms.DockStyle.Bottom;
                  this.Hierarchy.ForeColor = System.Drawing.Color.White;
                  this.Hierarchy.FullRowSelect = true;
                  this.Hierarchy.HideSelection = false;
                  this.Hierarchy.HotTracking = true;
                  this.Hierarchy.Indent = 30;
                  this.Hierarchy.LabelEdit = true;
                  this.Hierarchy.LineColor = System.Drawing.Color.White;
                  this.Hierarchy.Location = new System.Drawing.Point(0, 44);
                  this.Hierarchy.Name = "Hierarchy";
                  this.Hierarchy.Size = new System.Drawing.Size(202, 444);
                  this.Hierarchy.TabIndex = 5;
                  this.Hierarchy.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.Hierarchy_AfterSelect);
                  this.Hierarchy.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Hierarchy_MouseDown);
                  this.Hierarchy.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Hierarchy_MouseUp);
                  // 
                  // menuStrip1
                  // 
                  this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
                  this.menuStrip1.Font = new System.Drawing.Font("Bahnschrift SemiBold", 9.75F, System.Drawing.FontStyle.Bold);
                  this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
                  this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
                  this.menuStrip1.Location = new System.Drawing.Point(0, 0);
                  this.menuStrip1.Name = "menuStrip1";
                  this.menuStrip1.Size = new System.Drawing.Size(747, 24);
                  this.menuStrip1.TabIndex = 8;
                  this.menuStrip1.Text = "menuStrip1";
                  // 
                  // fileToolStripMenuItem
                  // 
                  this.fileToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
                  this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveSceneToolStripMenuItem,
            this.saveSceneAsToolStripMenuItem,
            toolStripMenuItem1,
            this.openSceneToolStripMenuItem,
            this.exitToolStripMenuItem});
                  this.fileToolStripMenuItem.ForeColor = System.Drawing.Color.White;
                  this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
                  this.fileToolStripMenuItem.ShortcutKeyDisplayString = "";
                  this.fileToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
                  this.fileToolStripMenuItem.Text = "File";
                  // 
                  // saveSceneToolStripMenuItem
                  // 
                  this.saveSceneToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
                  this.saveSceneToolStripMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
                  this.saveSceneToolStripMenuItem.ForeColor = System.Drawing.Color.White;
                  this.saveSceneToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveSceneToolStripMenuItem.Image")));
                  this.saveSceneToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
                  this.saveSceneToolStripMenuItem.Name = "saveSceneToolStripMenuItem";
                  this.saveSceneToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
                  this.saveSceneToolStripMenuItem.Size = new System.Drawing.Size(192, 26);
                  this.saveSceneToolStripMenuItem.Text = "&Save Scene";
                  // 
                  // saveSceneAsToolStripMenuItem
                  // 
                  this.saveSceneAsToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
                  this.saveSceneAsToolStripMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
                  this.saveSceneAsToolStripMenuItem.ForeColor = System.Drawing.Color.White;
                  this.saveSceneAsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveSceneAsToolStripMenuItem.Image")));
                  this.saveSceneAsToolStripMenuItem.Name = "saveSceneAsToolStripMenuItem";
                  this.saveSceneAsToolStripMenuItem.Size = new System.Drawing.Size(192, 26);
                  this.saveSceneAsToolStripMenuItem.Text = "Save Scene as";
                  // 
                  // openSceneToolStripMenuItem
                  // 
                  this.openSceneToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
                  this.openSceneToolStripMenuItem.ForeColor = System.Drawing.Color.White;
                  this.openSceneToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openSceneToolStripMenuItem.Image")));
                  this.openSceneToolStripMenuItem.Name = "openSceneToolStripMenuItem";
                  this.openSceneToolStripMenuItem.Size = new System.Drawing.Size(192, 26);
                  this.openSceneToolStripMenuItem.Text = "Open Scene";
                  // 
                  // exitToolStripMenuItem
                  // 
                  this.exitToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
                  this.exitToolStripMenuItem.ForeColor = System.Drawing.Color.White;
                  this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
                  this.exitToolStripMenuItem.Size = new System.Drawing.Size(192, 26);
                  this.exitToolStripMenuItem.Text = "E&xit";
                  // 
                  // btn_ClearConsole
                  // 
                  this.btn_ClearConsole.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
                  this.btn_ClearConsole.Location = new System.Drawing.Point(63, 664);
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
                  this.txt_Console.Location = new System.Drawing.Point(144, 524);
                  this.txt_Console.Multiline = true;
                  this.txt_Console.Name = "txt_Console";
                  this.txt_Console.ReadOnly = true;
                  this.txt_Console.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
                  this.txt_Console.Size = new System.Drawing.Size(591, 163);
                  this.txt_Console.TabIndex = 11;
                  // 
                  // label_updateTime
                  // 
                  this.label_updateTime.AutoSize = true;
                  this.label_updateTime.ForeColor = System.Drawing.Color.SandyBrown;
                  this.label_updateTime.Location = new System.Drawing.Point(67, 539);
                  this.label_updateTime.Name = "label_updateTime";
                  this.label_updateTime.Size = new System.Drawing.Size(42, 16);
                  this.label_updateTime.TabIndex = 7;
                  this.label_updateTime.Text = "-2 ms";
                  // 
                  // label_renderTime
                  // 
                  this.label_renderTime.AutoSize = true;
                  this.label_renderTime.ForeColor = System.Drawing.Color.SandyBrown;
                  this.label_renderTime.Location = new System.Drawing.Point(67, 564);
                  this.label_renderTime.Name = "label_renderTime";
                  this.label_renderTime.Size = new System.Drawing.Size(39, 16);
                  this.label_renderTime.TabIndex = 9;
                  this.label_renderTime.Text = "-1 ms";
                  // 
                  // toolStripMenuItem1
                  // 
                  toolStripMenuItem1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
                  toolStripMenuItem1.Enabled = false;
                  toolStripMenuItem1.ForeColor = System.Drawing.Color.White;
                  toolStripMenuItem1.Name = "toolStripMenuItem1";
                  toolStripMenuItem1.Size = new System.Drawing.Size(192, 26);
                  toolStripMenuItem1.Text = "─────────────────";
                  // 
                  // EditorWindow
                  // 
                  this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
                  this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
                  this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
                  this.ClientSize = new System.Drawing.Size(747, 699);
                  this.Controls.Add(this.btn_ClearConsole);
                  this.Controls.Add(this.label_renderTime);
                  this.Controls.Add(this.txt_Console);
                  this.Controls.Add(label2);
                  this.Controls.Add(this.label_updateTime);
                  this.Controls.Add(label_titleCPUTime);
                  this.Controls.Add(this.Panel_Main);
                  this.Font = new System.Drawing.Font("Bahnschrift SemiBold", 9.75F, System.Drawing.FontStyle.Bold);
                  this.ForeColor = System.Drawing.Color.White;
                  this.KeyPreview = true;
                  this.Name = "EditorWindow";
                  this.ShowIcon = false;
                  this.Text = "EditorForm";
                  this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EditorForm_FormClosing);
                  this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Editor_KeyDown);
                  this.ComponentsHeaderPanel.ResumeLayout(false);
                  this.Panel_Main.ResumeLayout(false);
                  this.Panel_Main.PerformLayout();
                  this.Panel_GameObjects.ResumeLayout(false);
                  this.menuStrip1.ResumeLayout(false);
                  this.menuStrip1.PerformLayout();
                  this.ResumeLayout(false);
                  this.PerformLayout();

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
        private System.Windows.Forms.ToolStripMenuItem saveSceneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openSceneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txt_Console;
        private System.Windows.Forms.Button btn_ClearConsole;
        private System.Windows.Forms.Label label_updateTime;
        private System.Windows.Forms.Label label_renderTime;
        private System.Windows.Forms.ToolStripMenuItem saveSceneAsToolStripMenuItem;
      }
}