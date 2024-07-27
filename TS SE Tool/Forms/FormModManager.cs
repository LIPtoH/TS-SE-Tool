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
    public partial class FormModManager : Form {
        //FormMain MainForm = Application.OpenForms.OfType<FormMain>().Single();
        public SupportedGame Game { get; private set; }

        public BindingList<GameMod> Mods = new BindingList<GameMod>();


        public FormModManager(SupportedGame game) {
            Game = game;
            InitializeComponent();
        }

        public HashSet<GameMod> FindMatchingmods(DirectoryInfo gameDir) {
            var mods = new HashSet<GameMod>();
            var filesToProcess = Game.ModsDir.GetFiles("*.scs").Concat(Game.ModsDir.GetFiles("*.disabled")).ToList();
            while (filesToProcess.Count > 0) {
                var file = filesToProcess[0];
                filesToProcess.RemoveAt(0);
                var mod = new GameMod() { Enabled = file.Extension == ".disabled", File = file, Name = file.FileNameWithoutExtension() };
                mods.Add(mod);
            }

            // Convert to a distinct HashSet to remove duplicates
            mods = new HashSet<GameMod>(mods.Distinct());

            IO_Utilities.LogWriter($"Found {mods.Count} distinct mod pairs in {gameDir}");

            return mods;
        }


        private void Populatemods() {
            //tableMods.Rows.Clear();
            Mods.Clear();
            FindMatchingmods(Game.ModsDir).ForEach(p => Mods.Add(p));
            //foreach (var mod in mods) {
            //    tableMods.Rows.Add(mod.Enabled, mod.Name, mod.InstallDate, mod.File32bit != null, mod.File64bit != null);
            //}
        }

        private void FormModManager_Load(object sender, EventArgs e) {
            Text = $"Manage {Game.Name} mods";
            tableMods.Columns.Clear();
            tableMods.Rows.Clear();
            tableMods.DataSource = Mods;
            tableMods.RowHeadersVisible = false;
            tableMods.AllowUserToAddRows = false;
            tableMods.AllowUserToDeleteRows = false;
            tableMods.AllowUserToOrderColumns = true;
            tableMods.MultiSelect = true;
            tableMods.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            tableMods.CellContentClick += tableMods_CellContentClick;
            tableMods.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            Populatemods();
            //foreach (var i in new[] { 0, 3, 4 })
            //    tableMods.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            //mods.ListChanged += mods_ListChanged;
            //mods.AddingNew += mods_ListChanged;
        }

        private void mods_ListChanged(object sender, object e) {
            //throw new NotImplementedException();
        }

        private void tableMods_CellContentClick(object sender, DataGridViewCellEventArgs e) {
            tableMods.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void openModsDirectoryToolStripMenuItem_Click(object sender, EventArgs e) {
            Game.ModsDir.OpenInExplorer();
        }

        private GameMod GetmodFromRow(int rowIndex, DataGridView table = null) {
            table ??= tableMods;
            if (table is null || rowIndex < 0 || rowIndex > table.RowCount) return null;
            return table.Rows[rowIndex].DataBoundItem as GameMod;
        }

        private List<GameMod> GetModsFromSelected() {
            var list = new List<GameMod>();
            foreach (DataGridViewRow row in tableMods.SelectedRows) {
                list.Add(row.DataBoundItem as GameMod);
            }
            return list;
            //MyType selectedItem = (MyType)list[0].DataBoundItem; //[0] ---> first item
        }

        //private ContextMenuStrip strip;
        private void tableMods_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e) {
            //var table = sender as DataGridView;
            //if (sender is null || table is null || e is null || e.RowIndex < 0 || e.RowIndex > table.RowCount) {
            //    MessageBox.Show("fail");
            //    return;
            //}
            var mods = GetModsFromSelected(); //var mod = GetmodFromRow(e.RowIndex, table);
            if (mods.Count == 1) {
                modContextMenu.Items[0].Text = mods.First().Enabled ? "Disable" : "Enable";
            } else {
                modContextMenu.Items[0].Text = "Toggle";
            }
            e.ContextMenuStrip = modContextMenu;
        }

        private DialogResult AskUser(string action, MessageBoxIcon icons = MessageBoxIcon.Question) {
            var mods = GetModsFromSelected();
            if (mods is null || mods.Count == 0) return DialogResult.Cancel;
            var sb = new StringBuilder("Are you sure you want to " + action + " the following files:\n\n");
            foreach (var mod in mods) {
                //var files = mod.Files;
                //if (files.Count > 0) sb.AppendLine(string.Join(", ", files.Select(f => f.Name.Quote())));
            }
            return MessageBox.Show(sb.ToString(), action + " files?", MessageBoxButtons.YesNo, icons);
        }

        private void toggleToolStripMenuItem_Click(object sender, EventArgs e) {
            //var menuItem = sender as ToolStripMenuItem;
            if (AskUser("toggle") != DialogResult.Yes) return;
            GetModsFromSelected().ForEach(p => p.Enabled = !p.Enabled);
            Populatemods();
        }

        private void openFoldersToolStripMenuItem_Click(object sender, EventArgs e) {
            //var menuItem = sender as ToolStripMenuItem;
            var mods = GetModsFromSelected();
            var folders = new List<DirectoryInfo>();
            foreach (var mod in mods) {
                //if (mod.x86) folders.Add(mod.File32bit.Directory);
                //if (mod.x64) folders.Add(mod.File64bit.Directory);
            }
            foreach (var folder in folders.DistinctBy(x => x.FullName).ToList()) {
                folder.OpenInExplorer();
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e) {
            //var menuItem = sender as ToolStripMenuItem;
            if (AskUser("delete", MessageBoxIcon.Warning) != DialogResult.Yes) return;
            foreach (var mod in GetModsFromSelected()) {
                mod.Delete();
            }
            Populatemods();
        }

        private void tableMods_MouseHover(object sender, EventArgs e) {
            //var p = tableMods.PointToClient(Cursor.Position);
            //var info = tableMods.HitTest(p.X, p.Y);
            //var mod = GetmodFromRow(info.RowIndex, tableMods);
            //tableMods.Rows[0].Cells
        }
        private void tableMods_CellMouseEnter(object sender, DataGridViewCellEventArgs e) {

        }

        private void showDebugToolStripMenuItem_Click(object sender, EventArgs e) {
            MessageBox.Show(GetModsFromSelected().ToJson(true));// string.Join("\n", GetmodsFromSelected().Select(p => p.ToJson())));
        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e) {
            Populatemods();
        }

        //private void tableMods_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
        //    UpdateDataGridViewSite();
        //}
    }
}
