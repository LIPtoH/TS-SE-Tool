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
using System.Linq;
using System.Windows.Forms;
using TS_SE_Tool.CustomClasses.Program;
using TS_SE_Tool.Utilities;

namespace TS_SE_Tool {
    partial class FormAboutBox : Form {
        FormMain MainForm = Application.OpenForms.OfType<FormMain>().Single();

        public FormAboutBox() {
            InitializeComponent();

            SetFormVisual();
            PopulateFormControls();
            TranslateForm();
#if DEBUG
            Globals.SupportedGames.ToJson(true).ToFile("SupportedGames.json", true); // todo: remove
#endif
        }

        private void SetFormVisual() {
            this.Icon = Utilities.Graphics_TSSET.IconFromImage(MainForm.ProgUIImgsDict["Info"]);
        }

        private void PopulateFormControls() {
            //buttonSupportDeveloper.Visible = false;

            labelProductName.Text = Utilities.AssemblyData.AssemblyProduct;
            labelCopyright.Text = Utilities.AssemblyData.AssemblyCopyright;

            var ets2 = Globals.SupportedGames.Get("ETS2");
            var ats = Globals.SupportedGames.Get("ATS");

            labelETS2version.Text = ets2.SupportedGameVersionString;
            if (ets2.SupportedSaveFileVersions.Count > 0) labelETS2version.Text += " (" + ets2.SupportedSaveFileVersionString + ")";
            labelATSversion.Text = ats.SupportedGameVersionString;
            if (ats.SupportedSaveFileVersions.Count > 0) labelATSversion.Text += " (" + ats.SupportedSaveFileVersionString + ")";

            //
            string[][] referencies = {
                new string[] {"SII Decrypt", "https://github.com/ncs-sniper/SII_Decrypt"},
                new string[] {"PsColorPicker", "https://github.com/exectails/PsColorPicker"},
                new string[] {"SharpZipLib", "https://github.com/icsharpcode/SharpZipLib"},
                new string[] {"SqlCeBulkCopy", "https://github.com/ErikEJ/SqlCeBulkCopy"},
                new string[] {"DDSImageParser.cs", "https://gist.github.com/soeminnminn/e9c4c99867743a717f5b"},
                new string[] {"TGASharpLib", "https://github.com/ALEXGREENALEX/TGASharpLib"},
                new string[] {"FlexibleMessageBox", "http://www.codeproject.com/Articles/601900/FlexibleMessageBox"},
            };

            string referenciesText = "";

            foreach (string[] tmp in referencies) {
                referenciesText += tmp[0] + Environment.NewLine + tmp[1] + Environment.NewLine + Environment.NewLine;
            }
            //
            textBoxDescription.Text = $"Installed Versions:\r\n\r\n{ets2.Type}: {ets2.Version}\r\n{ats.Type}: {ats.Version}\r\n\r\n";
            textBoxDescription.Text += string.Format(MainForm.HelpTranslateString(this.Name + textBoxDescription.Name),
                                                     Utilities.Web_Utilities.External.linkMailDeveloper, Utilities.Web_Utilities.External.linkGithub);

            textBoxDescription.Text += referenciesText;
            //
        }

        private void TranslateForm() {
            MainForm.HelpTranslateFormMethod(this);

            MainForm.HelpTranslateControlExt(this, Utilities.AssemblyData.AssemblyTitle);
            MainForm.HelpTranslateControlExt(labelVersion, Utilities.AssemblyData.AssemblyVersion);
        }

        private void buttonSupportDeveloper_Click(object sender, EventArgs e) {
            string url = Utilities.Web_Utilities.External.linkHelpDeveloper;

            DialogResult result = MessageBox.Show("This will open " + url + " web-page." + Environment.NewLine + "Do you want to continue?",
                                                  "Support developer", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
                System.Diagnostics.Process.Start(url);
        }
    }
}
