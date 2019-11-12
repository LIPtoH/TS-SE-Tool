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
using System.Reflection;
using System.IO;
using System.Threading;

namespace TS_SE_Tool
{
    public partial class FormSplash : Form
    {
        FormMain MainForm = Application.OpenForms.OfType<FormMain>().Single();
        public FormSplash()
        {
            InitializeComponent();

            MainForm.HelpTranslateFormMethod(this, MainForm.ResourceManagerMain, Thread.CurrentThread.CurrentUICulture);

            labelTSSE.Text = AssemblyProduct;

            string translatedString = MainForm.ResourceManagerMain.GetString(labelVersion.Name, Thread.CurrentThread.CurrentUICulture);
            if (translatedString != null)
                labelVersion.Text = String.Format(translatedString, AssemblyVersion);
            else
                labelVersion.Text = String.Format("{0} (alpha)", AssemblyVersion);

        }

        private void FormSplash_Load(object sender, EventArgs e)
        {

        }

        private void linkFirst_Click(object sender, EventArgs e)
        {
            string url = "https://forum.scssoft.com/viewtopic.php?f=34&t=266092";
            System.Diagnostics.Process.Start(url);
        }

        private void linkSecond_Click(object sender, EventArgs e)
        {
            string url = "https://forum.truckersmp.com/index.php?/topic/79561-ts-saveeditor-tool";
            System.Diagnostics.Process.Start(url);
        }


        private void linkLabelHelpLocalPDF_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string file = "HowTo.pdf";
            if(File.Exists(file))
                System.Diagnostics.Process.Start(file);
            else
            {
                MessageBox.Show("Missing manual", "HowTo.pdf not found");
            }
        }

        private void linkLabelHelpYouTube_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "https://rebrand.ly/TS-SET-Tutorial";
            System.Diagnostics.Process.Start(url);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
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
    }
}
