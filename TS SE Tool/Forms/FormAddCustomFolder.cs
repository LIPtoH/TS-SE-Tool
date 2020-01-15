/*
   Copyright 2016-2019 LIPtoH <liptoh.codebase@gmail.com>

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/
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
using System.Threading;

namespace TS_SE_Tool
{
    public partial class FormAddCustomFolder : Form
    {
        FormMain MainForm = Application.OpenForms.OfType<FormMain>().Single();

        private string SelectedfolderPath;
        private bool ListOpen = true;
        private Dictionary<string, List<string>> CustomPathsArr;
        private string GameType = "";
        private bool CustomPathChanged = false;

        public FormAddCustomFolder()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.MainIco;

            try
            {
                string translatedString = MainForm.ResourceManagerMain.GetString(this.Name, Thread.CurrentThread.CurrentUICulture);
                if (translatedString != null)
                    this.Text = translatedString;
            }
            catch
            {
            }

            MainForm.HelpTranslateFormMethod(this, MainForm.ResourceManagerMain, Thread.CurrentThread.CurrentUICulture);

            ChangeCustomPathListVisibility();
            CustomPathsArr = new Dictionary<string, List<string>>();//(MainForm.ProgSettingsV.CustomPaths);
            foreach (KeyValuePair<string, List<string>> k1 in MainForm.ProgSettingsV.CustomPaths)
            {
                List<string> tmp = new List<string>();
                foreach (string k2 in k1.Value)
                {
                    tmp.Add(k2);
                }
                CustomPathsArr.Add(k1.Key, tmp);
            }

            radioButtonGameTypeETS2.Checked = true;
        }
        //Buttons
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

                radioButtonRootFolderType.Checked = GameSFrootFolder;
                radioButtonProfileFolderType.Checked = GameSFprofileFolder;
                radioButtonSaveFolderType.Checked = GameSFsaveFolder;

                if (radioButtonRootFolderType.Checked || radioButtonProfileFolderType.Checked || radioButtonSaveFolderType.Checked)
                {
                    buttonAddCustomPath.Enabled = true;
                    groupBoxFolderType.Enabled = true;
                }
            }
        }

        private void buttonEditCPlist_Click(object sender, EventArgs e)
        {
            ChangeCustomPathListVisibility();
        }

        private void buttonAddCustomPath_Click(object sender, EventArgs e)
        {
            if (CustomPathsArr.Keys.Contains(GameType))
            {
                if (CustomPathsArr[GameType].Contains(SelectedfolderPath))
                {
                    MessageBox.Show("Path " + SelectedfolderPath + " already added to the " + GameType + " list", "Path exist in the list", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    CustomPathsArr[GameType].Add(SelectedfolderPath);
                    CustomPathChanged = true;
                    if (ListOpen)
                        UpdatedataGridView();
                    MessageBox.Show("Path " + SelectedfolderPath + " added to the " + GameType + " list", "Custom path", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                List<string> tmp = new List<string>();
                tmp.Add(SelectedfolderPath);
                CustomPathsArr.Add(GameType, tmp);
                CustomPathChanged = true;
                if (ListOpen)
                        UpdatedataGridView();
                MessageBox.Show("Path " + SelectedfolderPath + " added to the " + GameType + " list", "Custom path", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            labelCustomPathDir.Text = "Choose folder...";

            groupBoxFolderType.Enabled = false;
            radioButtonUnknownFolderType.Checked = true;

            buttonAddCustomPath.Enabled = false;
            buttonSave.Enabled = true;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            MainForm.ProgSettingsV.CustomPaths = new Dictionary<string, List<string>>(CustomPathsArr);
            CustomPathChanged = false;
            buttonSave.Enabled = false;
        }
        //Radio button
        private void radioButtonFolderType_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton a = sender as RadioButton;
            if(a.Checked & a.Name != "radioButton4" & (radioButtonGameTypeETS2.Checked || radioButtonGameTypeATS.Checked))
                buttonAddCustomPath.Enabled = true;
            else
                buttonAddCustomPath.Enabled = false;
        }

        private void radioButtonGameType_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonGameTypeETS2.Checked)
                GameType = "ETS2";
            else
                GameType = "ATS";

            if (ListOpen)
                UpdatedataGridView();
        }
        //Methods
        private void ChangeCustomPathListVisibility()
        {
            if (ListOpen)
            {
                tableLayoutPanel1.ColumnStyles[2].Width = 0;
                int w = 390;
                this.MinimumSize = new Size(w, this.Height);
                this.Width = w;
                this.MaximumSize = new Size(this.Width, this.Height);

                int xP = this.Location.X + 150;
                if (xP > Screen.PrimaryScreen.WorkingArea.Width)
                    xP = Screen.PrimaryScreen.WorkingArea.Width - this.Width;
                this.Location = new Point(xP, this.Location.Y);

                ListOpen = false;

                try
                {
                    string translatedString = MainForm.ResourceManagerMain.GetString(buttonEditCPlist.Name, Thread.CurrentThread.CurrentUICulture);
                    if (translatedString != null)
                        buttonEditCPlist.Text = translatedString + " ▶";
                    else
                        buttonEditCPlist.Text = "Edit list" + " ▶";

                }
                catch
                {
                }
            }
            else
            {
                tableLayoutPanel1.ColumnStyles[2].Width = 300;
                int w = 690;
                this.MaximumSize = new Size(w, this.Height);
                this.Width = w;
                this.MinimumSize = new Size(this.Width, this.Height);

                int xP = this.Location.X - 150;
                if (xP < 0)
                    xP = 0;
                this.Location = new Point(xP, this.Location.Y);

                ListOpen = true;

                try
                {
                    string translatedString = MainForm.ResourceManagerMain.GetString(buttonEditCPlist.Name, Thread.CurrentThread.CurrentUICulture);
                    if (translatedString != null)
                        buttonEditCPlist.Text = translatedString + " ◀";
                    else
                        buttonEditCPlist.Text = "Edit list" + " ◀";

                }
                catch
                {
                }

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
                CustomPathChanged = true;
                string tmp = senderGrid[1, e.RowIndex].Value.ToString();
                CustomPathsArr[GameType].Remove(tmp);
                if (CustomPathsArr[GameType].Count == 0)
                    CustomPathsArr.Remove(GameType);

                UpdatedataGridView();
                buttonSave.Enabled = true;
            }
        }

        private void UpdatedataGridView()
        {
            DataTable combDT = new DataTable();
            DataColumn dc = new DataColumn("Path", typeof(string));
            combDT.Columns.Add(dc);

            if (CustomPathsArr.Keys.Contains(GameType))
            {
                foreach (string path in CustomPathsArr[GameType])
                {
                    combDT.Rows.Add(path);
                }
            }

            dataGridView1.DataSource = combDT;
        }

        private void FormAddCustomFolder_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult exitDR = DialogResult.No;

            if (CustomPathChanged)
            {
                exitDR = MessageBox.Show("You have unsaved changes.\r\nDo you really want to close dialogue without saving?", "Dialogue close", MessageBoxButtons.YesNo);

                if (exitDR == DialogResult.Yes)
                {
                }
                else
                {
                    e.Cancel = true;
                    Activate();
                }
            }
        }

        //Extra
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
        
    }
}
