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
    public partial class FormProfileEditor : Form
    {
        public new FormMain ParentForm;

        private string WorkingProfilePath = "";
        private string WorkingProfileName = "";

        public FormProfileEditor()
        {
            InitializeComponent();
            PrepareForm();
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

        //Rename
        private void buttonRenameProfile_Click(object sender, EventArgs e)
        {
            using (var form = new FormProfileEditorRenameClone("rename"))
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    if (form.ReturnRenamedSuccessful)
                    {
                        MessageBox.Show("New Profile name " + form.ReturnNewName);
                        labelProfileNameValue.Text = form.ReturnNewName;
                    }
                }
            }
        }
        
        //Clone
        private void buttonCloneProfile_Click(object sender, EventArgs e)
        {
            using (var form = new FormProfileEditorRenameClone("clone"))
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    if (form.ReturnCloningSuccessful)
                    {

                        MessageBox.Show("Created new Profiles:\r\n" + string.Join("\r\n", form.ReturnClonedNames));

                    }
                }
            }
        }

        private void buttonExportSettings_Click(object sender, EventArgs e)
        {
            using (var form = new FormProfileEditorSettingsImportExport("export"))
            {
                form.ParentForm = ParentForm;
                DialogResult result = form.ShowDialog();

                if (result == DialogResult.OK)
                {
                    //string val = form.ReturnNewName;

                    this.labelProfileNameValue.Text = "Export";
                }
            }
        }

        private void buttonImportSettings_Click(object sender, EventArgs e)
        {
            using (var form = new FormProfileEditorSettingsImportExport("import"))
            {
                form.ParentForm = ParentForm;
                DialogResult result = form.ShowDialog();

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
