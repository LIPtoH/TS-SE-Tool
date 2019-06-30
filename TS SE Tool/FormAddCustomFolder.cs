using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace TS_SE_Tool
{
    public partial class FormAddCustomFolder : Form
    {
        FormMain MainForm = Application.OpenForms.OfType<FormMain>().Single();

        private string SelectedfolderPath;
        private bool ListOpen = true;
        private List<string> CustomPathsArr;

        public FormAddCustomFolder()
        {
            InitializeComponent();
            //tableLayoutPanel1.ColumnStyles[3].Width = 0;
            //this.Width = 390;
            ChangeCustomPathListVisibility();
            CustomPathsArr = new List<string>(MainForm.ProgSettingsV.CustomPaths);
        }

        private void buttonChooseFolder_Click(object sender, EventArgs e)
        {
            // Show the FolderBrowserDialog.            
            DialogResult result = folderBrowserDialogAddCustomFolder.ShowDialog();
            if (result == DialogResult.OK)
            {
                SelectedfolderPath = folderBrowserDialogAddCustomFolder.SelectedPath;
                labelCustomPathDir.Text = SelectedfolderPath;

                List<string> includedFolders = new List<string>();
                foreach (string tFolder in Directory.GetDirectories(SelectedfolderPath))
                {
                    includedFolders.Add(GetDirectoryName2(tFolder));
                }

                List<string> includedFiles = new List<string>();
                foreach (string tFolder in Directory.GetFiles(SelectedfolderPath))
                    includedFiles.Add(GetDirectoryName2(tFolder));

                bool GameSFrootFolder = false, GameSFprofileFolder = false, GameSFsaveFolder = false;

                //Determinate folder type
                if (includedFolders.Contains("profiles"))
                {
                    GameSFrootFolder = true;
                }
                if (includedFiles.Contains("profile.sii"))
                {
                    GameSFprofileFolder = true;
                }
                if (includedFiles.Contains("game.sii"))
                {
                    GameSFsaveFolder = true;
                }

                radioButton1.Checked = GameSFrootFolder;
                radioButton2.Checked = GameSFprofileFolder;
                radioButton3.Checked = GameSFsaveFolder;

                if (radioButton1.Checked || radioButton2.Checked || radioButton3.Checked)
                    buttonAddCustomPath.Enabled = true;
            }
        }

        private void buttonAddCustomPath_Click(object sender, EventArgs e)
        {
            buttonAddCustomPath.Enabled = false;
            labelCustomPathDir.Text = "Choose folder";
            radioButtonGameTypeETS2.Checked = false;
            radioButtonGameTypeATS.Checked = false;
            radioButton4.Checked = true;

            if (!CustomPathsArr.Contains(SelectedfolderPath))
            {
                CustomPathsArr.Add(SelectedfolderPath);
                if (ListOpen)
                    UpdatedataGridView();
                MessageBox.Show("Path " + SelectedfolderPath + " added to the list", "Custom path", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Path " + SelectedfolderPath + " already added to the list", "Path exist in the list", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        static string GetDirectoryName2(string f)
        {
            try
            {
                return f.Substring(f.LastIndexOf('\\') + 1, f.Length - f.LastIndexOf('\\') - 1);
            }
            catch
            {
                return string.Empty;
            }
        }

        private void radioButtonFolderType_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton a = sender as RadioButton;
            if(a.Checked & a.Name != "radioButton4" & (radioButtonGameTypeETS2.Checked || radioButtonGameTypeATS.Checked))
                buttonAddCustomPath.Enabled = true;
            else
                buttonAddCustomPath.Enabled = false;
        }

        private void buttonEditCPlist_Click(object sender, EventArgs e)
        {
            ChangeCustomPathListVisibility();
        }

        private void ChangeCustomPathListVisibility()
        {
            if (ListOpen)
            {
                tableLayoutPanel1.ColumnStyles[3].Width = 0;
                this.Width = 390;
                ListOpen = false;
                buttonEditCPlist.Text = "Edit list" + " ▶";
            }
            else
            {
                tableLayoutPanel1.ColumnStyles[3].Width = 300;
                this.Width = 690;
                ListOpen = true;
                buttonEditCPlist.Text = "Edit list" + " ◀";

                UpdatedataGridView();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == dataGridView1.NewRowIndex || e.RowIndex < 0)
                return;

            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                CustomPathsArr.RemoveAt(e.RowIndex);
            }

            UpdatedataGridView();
        }

        private void UpdatedataGridView()
        {
            DataTable combDT = new DataTable();
            DataColumn dc = new DataColumn("Path", typeof(string));
            combDT.Columns.Add(dc);

            foreach (string path in CustomPathsArr)
            {
                combDT.Rows.Add(path);
            }

            dataGridView1.DataSource = combDT;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            MainForm.ProgSettingsV.CustomPaths = new List<string>(CustomPathsArr);
        }
    }
}
