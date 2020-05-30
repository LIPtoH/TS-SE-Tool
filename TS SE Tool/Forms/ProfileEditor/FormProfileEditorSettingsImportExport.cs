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
        private string InitialName = "";
        private string InitialPath = "";

        private string FormMode;

        public FormProfileEditorSettingsImportExport(string _mode)
        {
            InitializeComponent();
            this.Icon = Properties.Resources.MainIco;
            FormMode = _mode;
        }

        private void FormProfileEditorSettingsImportExport_Load(object sender, EventArgs e)
        {
            PrepareData();
            SetupForm(FormMode);
        }

        private void PrepareData()
        {
            //Profile name
            InitialPath = ParentForm.comboBoxProfiles.SelectedValue.ToString();
            InitialName = Utilities.TextUtilities.FromHexToString(InitialPath.Split(new string[] { "\\" }, StringSplitOptions.None).Last());

            LoadProfiles();
        }

        private void SetupForm(string _mode)
        {
            //Profile name
            //InitialPath = ParentForm.comboBoxProfiles.SelectedValue.ToString();
            InitialName = Utilities.TextUtilities.FromHexToString(InitialPath.Split(new string[] { "\\" }, StringSplitOptions.None).Last());


            switch (_mode)
            {
                case "import":
                    {
                        this.Text = "Importing to ";

                        break;
                    }
                case "export":
                    {
                        this.Text = "Exporting from ";

                        break;
                    }
            }
            this.Text += InitialName;
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
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
