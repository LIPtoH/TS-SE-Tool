namespace TS_SE_Tool.Forms {
    partial class FormModManager {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormModManager));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.reloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openPluginsDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableMods = new System.Windows.Forms.DataGridView();
            this.modContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toggleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.showDebugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modEnabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.modName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tableMods)).BeginInit();
            this.modContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reloadToolStripMenuItem,
            this.openPluginsDirectoryToolStripMenuItem});
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
            this.openPluginsDirectoryToolStripMenuItem.Name = "openPluginsDirectoryToolStripMenuItem";
            resources.ApplyResources(this.openPluginsDirectoryToolStripMenuItem, "openPluginsDirectoryToolStripMenuItem");
            this.openPluginsDirectoryToolStripMenuItem.Click += new System.EventHandler(this.openModsDirectoryToolStripMenuItem_Click);
            // 
            // tableMods
            // 
            this.tableMods.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.tableMods.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tableMods.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.modEnabled,
            this.modName});
            resources.ApplyResources(this.tableMods, "tableMods");
            this.tableMods.Name = "tableMods";
            this.tableMods.RowHeadersVisible = false;
            this.tableMods.CellContextMenuStripNeeded += new System.Windows.Forms.DataGridViewCellContextMenuStripNeededEventHandler(this.tableMods_CellContextMenuStripNeeded);
            // 
            // modContextMenu
            // 
            this.modContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toggleToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.toolStripSeparator1,
            this.showDebugToolStripMenuItem});
            this.modContextMenu.Name = "pluginContextMenu";
            resources.ApplyResources(this.modContextMenu, "modContextMenu");
            // 
            // toggleToolStripMenuItem
            // 
            this.toggleToolStripMenuItem.Name = "toggleToolStripMenuItem";
            resources.ApplyResources(this.toggleToolStripMenuItem, "toggleToolStripMenuItem");
            this.toggleToolStripMenuItem.Click += new System.EventHandler(this.toggleToolStripMenuItem_Click);
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
            // modEnabled
            // 
            resources.ApplyResources(this.modEnabled, "modEnabled");
            this.modEnabled.Name = "modEnabled";
            // 
            // modName
            // 
            resources.ApplyResources(this.modName, "modName");
            this.modName.Name = "modName";
            // 
            // FormModManager
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableMods);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormModManager";
            this.Load += new System.EventHandler(this.FormModManager_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tableMods)).EndInit();
            this.modContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem openPluginsDirectoryToolStripMenuItem;
        private System.Windows.Forms.DataGridView tableMods;
        private System.Windows.Forms.ContextMenuStrip modContextMenu;
        private System.Windows.Forms.ToolStripMenuItem toggleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem showDebugToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reloadToolStripMenuItem;
        private System.Windows.Forms.DataGridViewCheckBoxColumn modEnabled;
        private System.Windows.Forms.DataGridViewTextBoxColumn modName;
    }
}