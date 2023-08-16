/*
   Copyright 2016-2022 LIPtoH <liptoh.codebase@gmail.com>

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
using System.Threading;

namespace TS_SE_Tool
{
    public partial class FormProfileEditor : Form
    {
        public new FormMain ParentForm;
        FormMain MainForm = Application.OpenForms.OfType<FormMain>().Single();

        private string WorkingProfilePath = "";
        private string WorkingProfileName = "";
        private string ProfileType = "";

        public FormProfileEditor()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.MainIco;

            PrepareForm();
            TranslateForm();

            CorrectControlsPositions();
        }

        private void PrepareForm()
        {
            //Profile backup
            string ProfilePathBackup = Globals.SelectedProfilePath + @"\profile_backup.sii";

            if (!File.Exists(ProfilePathBackup))
                buttonRestoreProfileBackup.Enabled = false;

            //dialog result
            buttonSave.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

        }

        private void FormProfileEditor_Load(object sender, EventArgs e)
        {
            //Profile name
            WorkingProfilePath = ParentForm.comboBoxProfiles.SelectedValue.ToString();
            WorkingProfileName = Utilities.TextUtilities.FromHexToString(WorkingProfilePath.Split(new string[] { "\\" }, StringSplitOptions.None).Last());

            //Profile type
            ProfileType = ((DataTable)ParentForm.comboBoxRootFolders.DataSource).Rows[ParentForm.comboBoxRootFolders.SelectedIndex].ItemArray[2].ToString();

            labelProfileNameValue.Text = WorkingProfileName;
        }
        private void FormProfileEditor_Shown(object sender, EventArgs e)
        {
            buttonCancel.Focus();
        }


        private void CorrectControlsPositions()
        {
            int newWidth = labelProfileName.PreferredWidth;
            tableLayoutPanelProfileName.ColumnStyles[0] = new ColumnStyle(SizeType.Absolute, newWidth + 6);
        }

        private void TranslateForm()
        {
            MainForm.HelpTranslateFormMethod(this);

            MainForm.HelpTranslateControl(this);
        }

        //Rename
        private void buttonRenameProfile_Click(object sender, EventArgs e)
        {
            using (var dForm = new FormProfileEditorRenameClone("rename"))
            {
                dForm.ParentForm = ParentForm;
                dForm.InitialName = WorkingProfileName;
                dForm.InitialPath = WorkingProfilePath;

                DialogResult result = dForm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    if (dForm.ReturnRenamedSuccessful)
                    {
                        WorkingProfileName = dForm.ReturnNewName;
                        WorkingProfilePath = WorkingProfilePath.Remove(WorkingProfilePath.LastIndexOf('\\') + 1) + Utilities.TextUtilities.FromStringToHex(WorkingProfileName);

                        labelProfileNameValue.Text = dForm.ReturnNewName;

                        MessageBox.Show("New Profile name - " + dForm.ReturnNewName);
                    }
                }
            }
        }
        
        //Clone
        private void buttonCloneProfile_Click(object sender, EventArgs e)
        {
            using (var dForm = new FormProfileEditorRenameClone("clone"))
            {
                dForm.ParentForm = ParentForm;
                dForm.InitialName = WorkingProfileName;
                dForm.InitialPath = WorkingProfilePath;

                DialogResult result = dForm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    if (dForm.ReturnCloningSuccessful)
                    {
                        MessageBox.Show("Created new Profiles (" + dForm.ReturnClonedNames.Count.ToString() + "):\r\n\r\n" + string.Join("\r\n", dForm.ReturnClonedNames));
                    }
                    else
                        MessageBox.Show("No profiles created due to duplicating names.");
                }
            }
        }

        //Restore Backup
        private void buttonRestoreProfileBackup_Click(object sender, EventArgs e)
        {
            string ProfilePath = Globals.SelectedProfilePath + @"\profile.sii",
                   ProfilePathBackup = Globals.SelectedProfilePath + @"\profile_backup.sii";

            if (File.Exists(ProfilePathBackup))
            {
                DialogResult dr = MessageBox.Show("Restoring from backup file will overwrite existing file." + Environment.NewLine +
                                                  "Select: Yes - Overwrite | No - Swap files | Cancel - Abort restoring.",
                                                  "Restoring Profile from Backup", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                if (dr == DialogResult.Cancel)
                    return;

                if (dr == DialogResult.No)
                {
                    SwapFiles(ProfilePath, ProfilePathBackup);
                }
                else
                {
                    File.Copy(ProfilePathBackup, ProfilePath, true);

                    File.Delete(ProfilePathBackup);
                }

                void SwapFiles(string _firstFile, string _secondFile)
                {
                    string tmpFile = Directory.GetParent(_firstFile).FullName + "\\tmp";

                    File.Copy(_firstFile, tmpFile, true);
                    File.Copy(_secondFile, _firstFile, true);
                    File.Copy(tmpFile, _secondFile, true);

                    File.Delete(tmpFile);
                }
            }
        }

        //Settings
        //Export
        private void buttonExportSettings_Click(object sender, EventArgs e)
        {
            using (var dForm = new FormProfileEditorSettingsImportExport(FormProfileEditorSettingsImportExport.Form4Mode.Export))
            {
                dForm.ParentForm = ParentForm;
                dForm.ProfileType = ProfileType;

                DialogResult result = dForm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    //string val = form.ReturnNewName;

                    this.labelProfileNameValue.Text = "Export";
                }
            }
        }
        //Import
        private void buttonImportSettings_Click(object sender, EventArgs e)
        {
            using (var dForm = new FormProfileEditorSettingsImportExport(FormProfileEditorSettingsImportExport.Form4Mode.Import))
            {
                dForm.ParentForm = ParentForm;
                dForm.ProfileType = ProfileType;

                DialogResult result = dForm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    //string val = form.ReturnNewName;

                    this.labelProfileNameValue.Text = "Import";
                }
            }
        }

        //Main buttons
        private void buttonSave_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
