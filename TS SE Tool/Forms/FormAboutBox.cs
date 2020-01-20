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
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace TS_SE_Tool
{
    partial class FormAboutBox : Form
    {
        public FormAboutBox()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.MainIco;

            var MainForm = Application.OpenForms.OfType<FormMain>().Single();
            MainForm.HelpTranslateFormMethod(this, MainForm.ResourceManagerMain, Thread.CurrentThread.CurrentUICulture);
            //MainForm.ClearDatabase();

            Text = String.Format("About {0}", AssemblyTitle);
            labelProductName.Text = AssemblyProduct;
            labelVersion.Text = String.Format("Version {0} (alpha)", AssemblyVersion);
            labelCopyright.Text = AssemblyCopyright;
            textBoxDescription.Text = "This program created by\r\nLIPtoH <liptoh.codebase@gmail.com>\r\nhttps://github.com/LIPtoH/TS-SE-Tool\r\n\r\n" +
                "Tools and projects used in this project:\r\n\r\n" +
                "SII Decrypt\r\nhttps://github.com/ncs-sniper/SII_Decrypt\r\n\r\n" +
                "PsColorPicker\r\nhttps://github.com/exectails/PsColorPicker\r\n\r\n" +
                "SharpZipLib\r\nhttps://github.com/icsharpcode/SharpZipLib\r\n\r\n" +
                "SqlCeBulkCopy\r\nhttps://github.com/ErikEJ/SqlCeBulkCopy\r\n\r\n" +
                "DDSImageParser.cs\r\nhttps://gist.github.com/soeminnminn/e9c4c99867743a717f5b\r\n\r\n" +
                "TGASharpLib\r\nhttps://github.com/ALEXGREENALEX/TGASharpLib";

            labelETS2version.Text = String.Join(", ", MainForm.SupportedSavefileVersionETS2.Select(p => p.ToString()).ToArray()) + " (" + MainForm.SupportedGameVersionETS2 + ")";
            labelATSversion.Text = String.Join(", ", MainForm.SupportedSavefileVersionETS2.Select(p => p.ToString()).ToArray()) + " (" + MainForm.SupportedGameVersionATS + ")";

            try
            {
                string translatedString = MainForm.ResourceManagerMain.GetString(this.Name, Thread.CurrentThread.CurrentUICulture);
                if (translatedString != null)
                    this.Text = String.Format(translatedString, AssemblyTitle);
                else
                    this.Text = String.Format("About {0}", AssemblyTitle);
            }
            catch
            {
            }

            try
            {
                string translatedString = MainForm.ResourceManagerMain.GetString(labelVersion.Name, Thread.CurrentThread.CurrentUICulture);
                if (translatedString != null)
                    labelVersion.Text = String.Format(translatedString, AssemblyVersion);
                else
                    labelVersion.Text = String.Format("Version {0} (alpha)", AssemblyVersion);

            }
            catch
            {
            }

        }

        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            string url = "https://www.paypal.me/LIPtoHCode";
            
            DialogResult result = MessageBox.Show("This will open a web-page.\nDo you want to continue?", "Support developer", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
                System.Diagnostics.Process.Start(url);
        }
    }
}
