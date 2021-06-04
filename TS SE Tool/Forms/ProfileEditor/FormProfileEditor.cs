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
using System.Threading;

namespace TS_SE_Tool
{
    public partial class FormProfileEditor : Form
    {
        public new FormMain ParentForm;
        FormMain MainForm = Application.OpenForms.OfType<FormMain>().Single();

        private string WorkingProfilePath = "";
        private string WorkingProfileName = "";

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
            //dialog result
            buttonSave.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;
        }

        private void FormProfileEditor_Load(object sender, EventArgs e)
        {
            //Profile name
            WorkingProfilePath = ParentForm.comboBoxProfiles.SelectedValue.ToString();
            WorkingProfileName = Utilities.TextUtilities.FromHexToString(WorkingProfilePath.Split(new string[] { "\\" }, StringSplitOptions.None).Last());
            labelProfileNameValue.Text = WorkingProfileName;
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

        //Settings
        private void buttonExportSettings_Click(object sender, EventArgs e)
        {
            using (var dForm = new FormProfileEditorSettingsImportExport("export"))
            {
                dForm.ParentForm = ParentForm;
                DialogResult result = dForm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    //string val = form.ReturnNewName;

                    this.labelProfileNameValue.Text = "Export";
                }
            }
        }

        private void buttonImportSettings_Click(object sender, EventArgs e)
        {
            using (var dForm = new FormProfileEditorSettingsImportExport("import"))
            {
                dForm.ParentForm = ParentForm;
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
