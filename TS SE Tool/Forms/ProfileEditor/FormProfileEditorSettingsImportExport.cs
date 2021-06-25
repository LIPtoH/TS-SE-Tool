/*
   Copyright 2016-2020 LIPtoH <liptoh.codebase@gmail.com>

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
using System.IO.Compression;

namespace TS_SE_Tool
{
    public partial class FormProfileEditorSettingsImportExport : Form
    {
        public new FormMain ParentForm;
        FormMain MainForm = Application.OpenForms.OfType<FormMain>().Single();

        private string InitialName = "";
        private string InitialPath = "";

        private string zipFilePath = "";
        private List<string> zipFiles = new List<string>();

        private string controlsNewNames = "";
        private Form4Mode FormMode;

        internal enum Form4Mode
        {
            Import,
            Export
        }

        internal FormProfileEditorSettingsImportExport(Form4Mode _mode)
        {
            InitializeComponent();

            this.Icon = Properties.Resources.MainIco;
            FormMode = _mode;
        }

        private void FormProfileEditorSettingsImportExport_Load(object sender, EventArgs e)
        {
            PrepareData();
            SetupForm();

            TranslateForm();
        }

        private void PrepareData()
        {
            //Profile name
            InitialPath = ParentForm.comboBoxProfiles.SelectedValue.ToString();
            InitialName = Utilities.TextUtilities.FromHexToString(InitialPath.Split(new string[] { "\\" }, StringSplitOptions.None).Last());

            LoadProfiles();

            buttonApply.Enabled = false;
            buttonCancel.Select();
        }

        private void LoadProfiles()
        {
            //Profiles
            DataTable combDT = new DataTable();
            DataColumn dc = new DataColumn("profilePath", typeof(string));
            combDT.Columns.Add(dc);

            dc = new DataColumn("profileName", typeof(string));
            combDT.Columns.Add(dc);

            foreach (string path in Globals.ProfilesHex)
            {
                if (path != ParentForm.comboBoxProfiles.SelectedValue.ToString())
                    combDT.Rows.Add(path, Utilities.TextUtilities.FromHexToString(path.Split(new string[] { "\\" }, StringSplitOptions.None).Last()));
            }

            listBoxProfileList.DataSource = combDT;

            listBoxProfileList.ValueMember = "profilePath";
            listBoxProfileList.DisplayMember = "profileName";

            listBoxProfileList.SelectedIndex = -1;
        }

        private void SetupForm()
        {
            listBoxSettingsExtra.Visible = true;

            switch (FormMode)
            {
                case Form4Mode.Import:
                    {
                        controlsNewNames = "Importing";

                        prepareVisualsSettings(false);

                        listBoxProfileList.SelectedIndexChanged += new EventHandler(listBoxProfileListImport_SelectedIndexChanged);

                        buttonSelectFile.Click += new EventHandler(buttonSelectFileImport_Click);
                        checkBoxChooseFileOption.CheckedChanged += new EventHandler(ChooseFileOptionImport_CheckedChanged);

                        buttonApply.Click += new EventHandler(buttonSaveImport_Click);

                        //set panels
                        tableLayoutPanelSourceDestination.SetColumn(tableLayoutPanelSource, 1);
                        tableLayoutPanelSourceDestination.SetColumn(tableLayoutPanelDestination, 0);

                        break;
                    }
                case Form4Mode.Export:
                    {
                        controlsNewNames = "Exporting";

                        prepareVisualsSettings(false);
                        listBoxProfileList.SelectionMode = SelectionMode.MultiSimple;

                        checkConfigsExist(InitialPath);

                        listBoxProfileList.SelectedIndexChanged += new EventHandler(listBoxProfileListExport_SelectedIndexChanged);

                        buttonSelectFile.Click += new EventHandler(buttonSelectFileExport_Click);
                        checkBoxChooseFileOption.CheckedChanged += new EventHandler(this.ChooseFileOptionExport_CheckedChanged);

                        buttonApply.Click += new EventHandler(this.buttonSaveExport_Click);

                        //set panels
                        tableLayoutPanelSourceDestination.SetColumn(tableLayoutPanelSource, 0);
                        tableLayoutPanelSourceDestination.SetColumn(tableLayoutPanelDestination, 1);

                        break;
                    }
            }
        }

        private void prepareVisualsSettings(bool _status)
        {
            checkBoxControls.Enabled = _status;
            checkBoxConfig.Enabled = _status;
            checkBoxConfigLocal.Enabled = _status;
            checkBoxShifterLayouts.Enabled = _status;
        }

        private void TranslateForm()
        {
            MainForm.HelpTranslateFormMethod(this);

            MainForm.HelpTranslateControlDiffName(this, this.Name + controlsNewNames, InitialName);
            MainForm.HelpTranslateControlDiffName(groupBoxTargetProfile, groupBoxTargetProfile.Name + controlsNewNames);
            MainForm.HelpTranslateControlDiffName(groupBoxSelectFile, groupBoxSelectFile.Name + controlsNewNames);
        }

        private void resetCheckboxes()
        {
            checkBoxControls.Checked = false;
            checkBoxConfig.Checked = false;
            checkBoxConfigLocal.Checked = false;
            checkBoxShifterLayouts.Checked = false;
        }

        //Events
        private void listBoxProfileListImport_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxProfileList.SelectedIndex != -1)
            {
                prepareVisualsSettings(false);
                resetCheckboxes();

                checkConfigsExist(listBoxProfileList.SelectedValue.ToString());

                buttonApply.Enabled = true;

                checkBoxChooseFileOption.Checked = false;
            }

        }

        private void listBoxProfileListExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxProfileList.SelectedIndex != -1)
            {
                buttonApply.Enabled = true;

                checkBoxChooseFileOption.Checked = false;
            }
            else
            {
                buttonApply.Enabled = false;
            }
        }

        private void checkConfigsExist(string _path)
        {
            List<string> folderFiles = Directory.GetFiles(_path).ToList();

            checkConfigFilesExist(folderFiles);
        }

        private void checkConfigsExistZip(string _fileName)
        {
            //List<string> zipFiles = new List<string>();

            try
            {
                Stream zipReadingStream = File.OpenRead(_fileName);
                if (zipReadingStream != null)
                {
                    ZipArchive zip = new ZipArchive(zipReadingStream);

                    zipFiles = zip.Entries.Select(x => x.Name).ToList();
                }
            }
            catch { }

            checkConfigFilesExist(zipFiles);
        }

        private void checkConfigFilesExist(List<string> _inputList)
        {
            List<string> tmpShifterLayoutNames = new List<string>();

            foreach (string tempEntry in _inputList)
            {
                string tmpFileName = Path.GetFileName(tempEntry);

                switch (tmpFileName)
                {
                    case "config.cfg":
                        {
                            checkBoxConfig.Enabled = true;
                            break;
                        }
                    case "config_local.cfg":
                        {
                            checkBoxConfigLocal.Enabled = true;
                            break;
                        }
                    case "controls.sii":
                        {
                            checkBoxControls.Enabled = true;
                            break;
                        }
                }
            }

            getShifterLayouts(_inputList);
        }

        private void checkBoxShifterLayouts_CheckedChanged(object sender, EventArgs e)
        {
            listBoxSettingsExtra.Enabled = checkBoxShifterLayouts.Checked;
            listBoxSettingsExtra.ClearSelected();
        }

        private void LoadSourceProfileShifterLayouts()
        {
            //Layouts
            DataTable combDT = new DataTable();
            DataColumn dc = new DataColumn("layoutsPath", typeof(string));
            combDT.Columns.Add(dc);

            dc = new DataColumn("layoutName", typeof(string));
            combDT.Columns.Add(dc);

            string[] shifterLayouts = Directory.GetFiles(InitialPath, "gearbox_*.sii");

            foreach (string path in shifterLayouts)
            {
                combDT.Rows.Add(path, path.Split(new string[] { "\\" }, StringSplitOptions.None).Last());
            }

            listBoxSettingsExtra.DataSource = combDT;
            listBoxSettingsExtra.ValueMember = "layoutsPath";
            listBoxSettingsExtra.DisplayMember = "layoutName";
        }

        private void getShifterLayouts(List<string> _files)
        {
            string[] shifterLayouts = _files.Where(x => Path.GetFileName(x).StartsWith("gearbox_") && x.EndsWith(".sii")).ToArray();

            if (shifterLayouts.Count() > 0)
                checkBoxShifterLayouts.Enabled = true;

            //Layouts
            DataTable combDT = new DataTable();
            DataColumn dc = new DataColumn("layoutPath", typeof(string));
            combDT.Columns.Add(dc);

            dc = new DataColumn("layoutName", typeof(string));
            combDT.Columns.Add(dc);

            foreach (string path in shifterLayouts)
            {
                combDT.Rows.Add(path, path.Split(new string[] { "\\" }, StringSplitOptions.None).Last());
            }

            listBoxSettingsExtra.DataSource = combDT;
            listBoxSettingsExtra.ValueMember = "layoutPath";
            listBoxSettingsExtra.DisplayMember = "layoutName";

            listBoxSettingsExtra.ClearSelected();
        }

        //Extra buttons
        private void buttonSelectFileImport_Click(object sender, EventArgs e)
        {
            selectZipFileForImport();
        }

        private void buttonSelectFileExport_Click(object sender, EventArgs e)
        {
            selectZipFileForExport();
        }

        //
        private void selectZipFileForImport()
        {
            //Set the file dialog to filter for graphics files.
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "ZIP archives (*.zip)|*.zip";

            openFileDialog.Title = "Browse for ZIP file";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            DialogResult dr = openFileDialog.ShowDialog();
            if (dr == DialogResult.OK)
            {
                setVisualsBeforeFileSetlect();
                //
                zipFilePath = openFileDialog.FileName;
                checkConfigsExistZip(zipFilePath);
                //
                setVisualsAfterFileSetlect();
            }
        }

        private void selectZipFileForExport()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "ZIP archives (*.zip)|*.zip";

            saveFileDialog.Title = "Save as ZIP file";

            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.OverwritePrompt = true;
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                //Visuals
                listBoxProfileList.SelectedIndex = -1;
                //
                zipFilePath = saveFileDialog.FileName;
                //
                setVisualsAfterFileSetlect();

                if (zipFilePath != "")
                    buttonApply.Enabled = true;
            }
        }

        private void ChooseFileOptionImport_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxChooseFileOption.Checked)
            {
                setVisualsBeforeFileSetlect();

                checkConfigsExistZip(zipFilePath);

                setVisualsAfterFileSetlect();

                buttonApply.Enabled = true;
            }
            else
            {
                if (listBoxProfileList.SelectedIndex == -1)
                {
                    setVisualsBeforeFileSetlect();

                    buttonApply.Enabled = false;
                }
                    
            }
        }

        private void ChooseFileOptionExport_CheckedChanged(object sender, EventArgs e)
        {
            //Visuals
            if(checkBoxChooseFileOption.Checked)
            {
                listBoxProfileList.ClearSelected();

                buttonApply.Enabled = true;
            }
            else
            {
                if (listBoxProfileList.SelectedIndex == -1)
                    buttonApply.Enabled = false;
            }
        }

        private void setVisualsBeforeFileSetlect()
        {
            //Visuals
            listBoxProfileList.ClearSelected();

            //listBoxSettingsExtra.Items.Clear();
            listBoxSettingsExtra.DataSource = null;

            prepareVisualsSettings(false);
            resetCheckboxes();
        }

        private void setVisualsAfterFileSetlect()
        {
            //Visuals
            tableLayoutPanelUseFile.ColumnStyles[0].SizeType = SizeType.Percent;
            tableLayoutPanelUseFile.ColumnStyles[0].Width = 80;
            tableLayoutPanelUseFile.ColumnStyles[1].SizeType = SizeType.Percent;
            tableLayoutPanelUseFile.ColumnStyles[1].Width = 20;
            //
            buttonSelectFile.Text = "Reset";
            checkBoxChooseFileOption.Text = Path.GetFileName(zipFilePath);
            toolTipThis.SetToolTip(checkBoxChooseFileOption, zipFilePath);
            //
            checkBoxChooseFileOption.Checked = true; //.Select();
        }

        //Main buttons
        private void buttonSaveImport_Click(object sender, EventArgs e)
        {
            //prepare file list
            List<string> tmpFileList = new List<string>();

            if (listBoxProfileList.SelectedIndex == -1 && checkBoxChooseFileOption.Checked)
            {
                //ZIP
                if (checkBoxConfig.Checked)
                    tmpFileList.Add(zipFiles.Find(x => x == "config.cfg"));

                if (checkBoxConfigLocal.Checked)
                    tmpFileList.Add(zipFiles.Find(x => x == "config_local.cfg"));

                if (checkBoxControls.Checked)
                    tmpFileList.Add(zipFiles.Find(x => x == "controls.sii"));

                if (checkBoxShifterLayouts.Checked)
                    foreach (object tmp in listBoxSettingsExtra.SelectedItems)
                    {
                        string tmpPath = ((DataRowView)tmp).Row.ItemArray[0].ToString();
                        tmpFileList.Add(zipFiles.Find(x => x == tmpPath));
                    }
                //
                try
                {
                    //Extract zip
                    Utilities.ZipDataUtilities.extractListZipArchive(zipFilePath, InitialPath, tmpFileList);

                    //Message
                    MessageBox.Show("Files was imported.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch { }

                //Reset Zip toggle
                checkBoxChooseFileOption.Checked = false;
            }
            else if (listBoxProfileList.SelectedIndex != -1 && !checkBoxChooseFileOption.Checked)
            {
                //Folder
                string tmpSourcePath = ((DataRowView)listBoxProfileList.SelectedItem).Row.ItemArray[0].ToString();

                if (checkBoxConfig.Checked)
                    tmpFileList.Add(tmpSourcePath + "\\config.cfg");

                if (checkBoxConfigLocal.Checked)
                    tmpFileList.Add(tmpSourcePath + "\\config_local.cfg");

                if (checkBoxControls.Checked)
                    tmpFileList.Add(tmpSourcePath + "\\controls.sii");

                if (checkBoxShifterLayouts.Checked)
                    foreach (object tmp in listBoxSettingsExtra.SelectedItems)
                    {
                        string tmpPath = ((DataRowView)tmp).Row.ItemArray[0].ToString();
                        tmpFileList.Add(tmpPath);
                    }

                try
                {
                    //Copy
                    foreach (string file in tmpFileList)
                    {
                        File.Copy(file, InitialPath + "\\" + Path.GetFileName(file));
                    }

                    //Message
                    MessageBox.Show("Files was exported.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                { }

                //Reset Profiles list
                listBoxProfileList.ClearSelected();
            }
        }

        private void buttonSaveExport_Click(object sender, EventArgs e)
        {
            //prepare file list
            List<string> tmpFileList = new List<string>();

            if (checkBoxConfig.Checked)
                tmpFileList.Add(InitialPath + "\\config.cfg");

            if (checkBoxConfigLocal.Checked)
                tmpFileList.Add(InitialPath + "\\config_local.cfg");

            if (checkBoxControls.Checked)
                tmpFileList.Add(InitialPath + "\\controls.sii");

            if (checkBoxShifterLayouts.Checked)
                foreach (object tmp in listBoxSettingsExtra.SelectedItems)
                {
                    string tmpPath = ((DataRowView)tmp).Row.ItemArray[0].ToString();
                    tmpFileList.Add(tmpPath);
                }
            //
            if (tmpFileList.Count() > 0)
                if (listBoxProfileList.SelectedIndex == -1 && checkBoxChooseFileOption.Checked)
                {
                    //ZIP
                    try
                    {
                        Utilities.ZipDataUtilities.makeZipArchive(zipFilePath, tmpFileList);

                        DialogResult dr = MessageBox.Show("ZIP Archive Created.\r\nOpen destination folder?", "Success", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                        if (dr == DialogResult.Yes)
                            System.Diagnostics.Process.Start(Directory.GetParent(zipFilePath).FullName);
                    }
                    catch { }
                }
                else if (listBoxProfileList.SelectedIndex != -1 && !checkBoxChooseFileOption.Checked)
                {
                    //Profiles
                    List<string> exportPath = new List<string>();

                    //Paths
                    foreach (object tmp in listBoxProfileList.SelectedItems)
                    {
                        string tmpPath = ((DataRowView)tmp).Row.ItemArray[0].ToString();
                        exportPath.Add(tmpPath);
                    }

                    //Copy
                    try
                    {
                        foreach (string path in exportPath)
                            foreach (string file in tmpFileList)
                            {
                                File.Copy(file, path + "\\" + Path.GetFileName(file));
                            }

                        //Message
                        MessageBox.Show("Files was exported.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch { }
                }

            //unselect
            listBoxProfileList.SelectedIndex = -1;
            checkBoxChooseFileOption.Checked = false;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkBoxChooseFileOption_Click(object sender, EventArgs e)
        {

        }
    }
}
