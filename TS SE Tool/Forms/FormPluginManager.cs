using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using TS_SE_Tool.Utilities;
using System.ComponentModel;
using System.Text;
using MoreLinq;
using TS_SE_Tool.CustomClasses.Program;

namespace TS_SE_Tool.Forms {
    public partial class FormPluginManager : Form {
        public SupportedGame Game { get; private set; }
        public BindingList<GamePlugin> Plugins = new BindingList<GamePlugin>();

        public FormPluginManager(SupportedGame game) {
            Game = game;
            InitializeComponent();
        }

        public static DirectoryInfo GetPluginsDir(DirectoryInfo gameDir, string arch = "win_x64") {
            var combined = gameDir.Combine("bin", arch, "plugins");
            if (!combined.Exists) combined.Create();
            return combined;
        }

        public static GamePlugin? FindPluginByPath(IEnumerable<GamePlugin> plugins, FileInfo file) {
            foreach (var plugin in plugins) {
                if (plugin.x86 && plugin.File32bit.FullName == file.FullName) return plugin;
                if (plugin.x64 && plugin.File64bit.FullName == file.FullName) return plugin;
            }
            return null;
        }

        public static List<FileInfo> AskUserImport() {
            // Create a new instance of OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog {
                Title = "Select one or more DLL or ZIP files",
                Filter = "DLL Files (*.dll)|*.dll|ZIP Files (*.zip)|*.zip",
                Multiselect = true,
            };
            List<FileInfo> selectedFiles = new List<FileInfo>();
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                foreach (string filePath in openFileDialog.FileNames) {
                    selectedFiles.Add(new FileInfo(filePath));
                }
            }
            return selectedFiles;
        }

        public static HashSet<GamePlugin> FindMatchingPlugins(DirectoryInfo gameDir) {
            var plugins = new HashSet<GamePlugin>();
            var x86Dir = GetPluginsDir(gameDir, "win_x86");
            var x64Dir = GetPluginsDir(gameDir, "win_x64");
            var filesToProcess = x86Dir.GetFiles("*.dll").Concat(x86Dir.GetFiles("*.disabled").Concat(x64Dir.GetFiles("*.dll")).Concat(x64Dir.GetFiles("*.disabled"))).ToList();

            // While there are files left to process...
            while (filesToProcess.Count > 0) {
                // Take the next file to process
                var file = filesToProcess[0];
                filesToProcess.RemoveAt(0);

                // Try to find a matching plugin for this file
                var plugin = FindPluginByPath(plugins, file); // FindPluginByPath uses FileInfo for comparison
                if (plugin is null) plugin = plugins.GetDirectMatch(file);
                if (plugin is null) plugin = plugins.GetFuzzyMatch(file);
                if (plugin is null) {
                    // If no matching plugin is found, create a new one
                    plugin = new GamePlugin() { };
                    plugins.Add(plugin);
                }
                if (file.Is64BitDll()) plugin.File64bit = file;
                else plugin.File32bit = file;
            }

            // Convert to a distinct HashSet to remove duplicates
            plugins = new HashSet<GamePlugin>(plugins.Distinct());

            IO_Utilities.LogWriter($"Found {plugins.Count} distinct plugin pairs in {gameDir}");

            return plugins;
        }


        private void PopulatePlugins() {
            //tablePlugins.Rows.Clear();
            Plugins.Clear();
            FindMatchingPlugins(Game.GameDir).ForEach(p => Plugins.Add(p));
            //foreach (var plugin in Plugins) {
            //    tablePlugins.Rows.Add(plugin.Enabled, plugin.Name, plugin.InstallDate, plugin.File32bit != null, plugin.File64bit != null);
            //}
        }

        private void FormPluginManager_Load(object sender, EventArgs e) {
            Text = $"Manage {Game.Name} plugins";
            tablePlugins.Columns.Clear();
            tablePlugins.Rows.Clear();
            tablePlugins.DataSource = Plugins;
            tablePlugins.RowHeadersVisible = false;
            tablePlugins.AllowUserToAddRows = false;
            tablePlugins.AllowUserToDeleteRows = false;
            tablePlugins.AllowUserToOrderColumns = true;
            tablePlugins.MultiSelect = true;
            tablePlugins.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            tablePlugins.CellContentClick += tablePlugins_CellContentClick;
            tablePlugins.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            PopulatePlugins();
            foreach (var i in new[] { 0, 3, 4 })
                tablePlugins.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            //Plugins.ListChanged += Plugins_ListChanged;
            //Plugins.AddingNew += Plugins_ListChanged;
        }

        private void Plugins_ListChanged(object sender, object e) {
            //throw new NotImplementedException();
        }

        private void tablePlugins_CellContentClick(object sender, DataGridViewCellEventArgs e) {
            tablePlugins.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void openPluginsDirToolStripMenuItem_Click(object sender, EventArgs e) {
            var arch = ((ToolStripItem)sender).Text;
            GetPluginsDir(Game.GameDir, "win_" + arch).OpenInExplorer();
        }

        private GamePlugin GetPluginFromRow(int rowIndex, DataGridView table = null) {
            table ??= tablePlugins;
            if (table is null || rowIndex < 0 || rowIndex > table.RowCount) return null;
            return table.Rows[rowIndex].DataBoundItem as GamePlugin;
        }

        private List<GamePlugin> GetPluginsFromSelected() {
            var list = new List<GamePlugin>();
            foreach (DataGridViewRow row in tablePlugins.SelectedRows) {
                list.Add(row.DataBoundItem as GamePlugin);
            }
            return list;
            //MyType selectedItem = (MyType)list[0].DataBoundItem; //[0] ---> first item
        }

        //private ContextMenuStrip strip;
        private void tablePlugins_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e) {
            //var table = sender as DataGridView;
            //if (sender is null || table is null || e is null || e.RowIndex < 0 || e.RowIndex > table.RowCount) {
            //    MessageBox.Show("fail");
            //    return;
            //}
            var plugins = GetPluginsFromSelected(); //var plugin = GetPluginFromRow(e.RowIndex, table);
            if (plugins.Count == 1) {
                pluginContextMenu.Items[0].Text = plugins.First().Enabled ? "Disable" : "Enable";
            } else {
                pluginContextMenu.Items[0].Text = "Toggle";
            }
            e.ContextMenuStrip = pluginContextMenu;
        }

        private DialogResult AskUser(string action, MessageBoxIcon icons = MessageBoxIcon.Question) {
            var plugins = GetPluginsFromSelected();
            if (plugins is null || plugins.Count == 0) return DialogResult.Cancel;
            var sb = new StringBuilder("Are you sure you want to " + action + " the following files:\n\n");
            foreach (var plugin in plugins) {
                var files = plugin.Files;
                if (files.Count > 0) sb.AppendLine(string.Join(", ", files.Select(f => f.Name.Quote())));
            }
            return MessageBox.Show(sb.ToString(), action + " files?", MessageBoxButtons.YesNo, icons);
        }

        private void toggleToolStripMenuItem_Click(object sender, EventArgs e) {
            //var menuItem = sender as ToolStripMenuItem;
            if (AskUser("toggle") != DialogResult.Yes) return;
            GetPluginsFromSelected().ForEach(p => p.Enabled = !p.Enabled);
            PopulatePlugins();
        }

        private void openFoldersToolStripMenuItem_Click(object sender, EventArgs e) {
            //var menuItem = sender as ToolStripMenuItem;
            var plugins = GetPluginsFromSelected();
            var folders = new List<DirectoryInfo>();
            foreach (var plugin in plugins) {
                if (plugin.x86) folders.Add(plugin.File32bit.Directory);
                if (plugin.x64) folders.Add(plugin.File64bit.Directory);
            }
            foreach (var folder in folders.DistinctBy(x => x.FullName).ToList()) {
                folder.OpenInExplorer();
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e) {
            //var menuItem = sender as ToolStripMenuItem;
            if (AskUser("delete", MessageBoxIcon.Warning) != DialogResult.Yes) return;
            foreach (var plugin in GetPluginsFromSelected()) {
                plugin.Delete();
            }
            PopulatePlugins();
        }

        private void tablePlugins_MouseHover(object sender, EventArgs e) {
            //var p = tablePlugins.PointToClient(Cursor.Position);
            //var info = tablePlugins.HitTest(p.X, p.Y);
            //var plugin = GetPluginFromRow(info.RowIndex, tablePlugins);
            //tablePlugins.Rows[0].Cells
        }
        private void tablePlugins_CellMouseEnter(object sender, DataGridViewCellEventArgs e) {

        }

        private void showDebugToolStripMenuItem_Click(object sender, EventArgs e) {
            MessageBox.Show(GetPluginsFromSelected().ToJson(true));// string.Join("\n", GetPluginsFromSelected().Select(p => p.ToJson())));
        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e) {
            PopulatePlugins();
        }

        private void ImportDll(FileInfo file) {
            if (!file.Exists) return;
            if (file.Extension.ToLower() == ".dll") {
                file.CopyTo(GetPluginsDir(Game.GameDir, file.Is64BitDll() ? "win_x64" : "win_x86").CombineFile(file.Name));
            }
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e) {
            var selectedFiles = AskUserImport();
            if (selectedFiles.Count < 1) return;
            foreach (var file in selectedFiles) {
                if (file.Extension.ToLower() == ".dll") {
                    ImportDll(file);
                }
            }
            PopulatePlugins();
        }

        //private void tablePlugins_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
        //    UpdateDataGridViewSite();
        //}
    }
}
