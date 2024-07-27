namespace TS_SE_Tool.Forms {
    partial class FormPluginManager {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPluginManager));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.reloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openPluginsDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x64ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x86ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tablePlugins = new System.Windows.Forms.DataGridView();
            this.pluginContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toggleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFoldersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.showDebugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tablePlugins)).BeginInit();
            this.pluginContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reloadToolStripMenuItem,
            this.openPluginsDirectoryToolStripMenuItem,
            this.addToolStripMenuItem});
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Name = "menuStrip1";
            // 
            // reloadToolStripMenuItem
            // 
            this.reloadToolStripMenuItem.Name = "reloadToolStripMenuItem";
            resources.ApplyResources(this.reloadToolStripMenuItem, "reloadToolStripMenuItem");
            this.reloadToolStripMenuItem.Click += new System.EventHandler(this.reloadToolStripMenuItem_Click);
            // 
            // openPluginsDirectoryToolStripMenuItem
            // 
            this.openPluginsDirectoryToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.x64ToolStripMenuItem,
            this.x86ToolStripMenuItem});
            this.openPluginsDirectoryToolStripMenuItem.Name = "openPluginsDirectoryToolStripMenuItem";
            resources.ApplyResources(this.openPluginsDirectoryToolStripMenuItem, "openPluginsDirectoryToolStripMenuItem");
            // 
            // x64ToolStripMenuItem
            // 
            this.x64ToolStripMenuItem.Name = "x64ToolStripMenuItem";
            resources.ApplyResources(this.x64ToolStripMenuItem, "x64ToolStripMenuItem");
            this.x64ToolStripMenuItem.Click += new System.EventHandler(this.openPluginsDirToolStripMenuItem_Click);
            // 
            // x86ToolStripMenuItem
            // 
            this.x86ToolStripMenuItem.Name = "x86ToolStripMenuItem";
            resources.ApplyResources(this.x86ToolStripMenuItem, "x86ToolStripMenuItem");
            this.x86ToolStripMenuItem.Click += new System.EventHandler(this.openPluginsDirToolStripMenuItem_Click);
            // 
            // tablePlugins
            // 
            this.tablePlugins.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.tablePlugins, "tablePlugins");
            this.tablePlugins.Name = "tablePlugins";
            this.tablePlugins.CellContextMenuStripNeeded += new System.Windows.Forms.DataGridViewCellContextMenuStripNeededEventHandler(this.tablePlugins_CellContextMenuStripNeeded);
            // 
            // pluginContextMenu
            // 
            this.pluginContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toggleToolStripMenuItem,
            this.openFoldersToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.toolStripSeparator1,
            this.showDebugToolStripMenuItem});
            this.pluginContextMenu.Name = "pluginContextMenu";
            resources.ApplyResources(this.pluginContextMenu, "pluginContextMenu");
            // 
            // toggleToolStripMenuItem
            // 
            this.toggleToolStripMenuItem.Name = "toggleToolStripMenuItem";
            resources.ApplyResources(this.toggleToolStripMenuItem, "toggleToolStripMenuItem");
            this.toggleToolStripMenuItem.Click += new System.EventHandler(this.toggleToolStripMenuItem_Click);
            // 
            // openFoldersToolStripMenuItem
            // 
            this.openFoldersToolStripMenuItem.Name = "openFoldersToolStripMenuItem";
            resources.ApplyResources(this.openFoldersToolStripMenuItem, "openFoldersToolStripMenuItem");
            this.openFoldersToolStripMenuItem.Click += new System.EventHandler(this.openFoldersToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            resources.ApplyResources(this.deleteToolStripMenuItem, "deleteToolStripMenuItem");
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // showDebugToolStripMenuItem
            // 
            this.showDebugToolStripMenuItem.Name = "showDebugToolStripMenuItem";
            resources.ApplyResources(this.showDebugToolStripMenuItem, "showDebugToolStripMenuItem");
            this.showDebugToolStripMenuItem.Click += new System.EventHandler(this.showDebugToolStripMenuItem_Click);
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            resources.ApplyResources(this.addToolStripMenuItem, "addToolStripMenuItem");
            this.addToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // FormPluginManager
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tablePlugins);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormPluginManager";
            this.Load += new System.EventHandler(this.FormPluginManager_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tablePlugins)).EndInit();
            this.pluginContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem openPluginsDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x64ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x86ToolStripMenuItem;
        private System.Windows.Forms.DataGridView tablePlugins;
        private System.Windows.Forms.ContextMenuStrip pluginContextMenu;
        private System.Windows.Forms.ToolStripMenuItem toggleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFoldersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem showDebugToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reloadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
    }
}