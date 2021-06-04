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
using System.Threading;

namespace TS_SE_Tool
{
    public partial class FormProgramSettings : Form
    {
        FormMain MainForm = Application.OpenForms.OfType<FormMain>().Single();
        public FormProgramSettings()
        {
            InitializeComponent();

            this.Icon = Utilities.TS_Graphics.IconFromImage(MainForm.ProgUIImgsDict["ProgramSettings"]);

            this.SuspendLayout();

            MainForm.HelpTranslateControl(this);

            MainForm.HelpTranslateFormMethod(this);

            this.ResumeLayout();

            LoadSettings();
        }

        private void LoadSettings()
        {
            //Loading settings Setting checkboxes
            checkBoxShowSplashOnStartup.Checked = Properties.Settings.Default.ShowSplashOnStartup;
            checkBoxCheckUpdatesOnStartup.Checked = Properties.Settings.Default.CheckUpdatesOnStartup;
        }

        private void SaveSettings()
        {
            Properties.Settings.Default.ShowSplashOnStartup = checkBoxShowSplashOnStartup.Checked;
            Properties.Settings.Default.CheckUpdatesOnStartup = checkBoxCheckUpdatesOnStartup.Checked;
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void checkBoxCheckUpdatesOnStartup_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxCheckUpdatesOnStartup.Checked)
            {
                checkBoxShowSplashOnStartup.Checked = true;
                checkBoxShowSplashOnStartup.Enabled = false;
            }
            else
            {
                checkBoxShowSplashOnStartup.Enabled = true;
            }
        }
    }
}
