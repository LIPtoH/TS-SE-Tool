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
using System.Reflection;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Mime;
using System.Diagnostics;
using System.Security.Cryptography;

namespace TS_SE_Tool
{
    public partial class FormSplash : Form
    {
        string[] NewVersion = { "", "" };
        bool NVavailible = false, CheckComplete = false;
        FormMain MainForm = Application.OpenForms.OfType<FormMain>().Single();

        bool CheckForUpdates = true;

        public FormSplash()
        {
            InitializeComponent();

            MainForm.HelpTranslateFormMethod(this, MainForm.ResourceManagerMain, Thread.CurrentThread.CurrentUICulture);

            labelTSSE.Text = Utilities.AssemblyData.AssemblyProduct;

            string translatedString = MainForm.ResourceManagerMain.GetString(labelVersion.Name, Thread.CurrentThread.CurrentUICulture);
            if (translatedString != null)
                labelVersion.Text = String.Format(translatedString, Utilities.AssemblyData.AssemblyVersion);
            else
                labelVersion.Text = String.Format("{0} (alpha)", Utilities.AssemblyData.AssemblyVersion);
        }

        private void FormSplash_Load(object sender, EventArgs e)
        {
            try
            {
                CheckForUpdates = Properties.Settings.Default.CheckUpdatesOnStartup;
            }
            catch
            { }

            CheckLatestVersion();
        }

        private void FormSplash_Shown(object sender, EventArgs e)
        {
            try
            {
                CheckForUpdates = Properties.Settings.Default.CheckUpdatesOnStartup;
            }
            catch
            {
                MessageBox.Show("Please update manually in order to fix it.", "Installation corrupted", MessageBoxButtons.OK, MessageBoxIcon.Error);
                buttonOK.Click -= new EventHandler(this.button1_Click);
                buttonOK.Text = "Close application";
                buttonOK.Click += new EventHandler(this.button1_ClickCloseApp);
            }
        }

        //Actions
        private void linkLabelNewVersion_Click(object sender, EventArgs e)
        {
            buttonOK.Enabled = false;

            FormCheckUpdates FormWindow = new FormCheckUpdates("download");
            FormWindow.NewVersion = NewVersion;
            DialogResult t = FormWindow.ShowDialog();

            buttonOK.Enabled = true;
            if(t == DialogResult.OK)
            {
                linkLabelNewVersion.Text = String.Format("You are using latest version!\r\n(Repair)");
                linkLabelNewVersion.DisabledLinkColor = this.ForeColor;
            }
            else if (t == DialogResult.Abort)
            {
                linkLabelNewVersion.Text = String.Format("Something gone wrong. Please use links below for manual update.");
                linkLabelNewVersion.DisabledLinkColor = Color.Red;
            }

            linkLabelNewVersion.LinkBehavior = LinkBehavior.NeverUnderline;
            linkLabelNewVersion.Links[0].Enabled = false;
        }
        //Links
        private void linkFirst_Click(object sender, EventArgs e)
        {
            string url = "https://forum.scssoft.com/viewtopic.php?f=34&t=266092";
            Process.Start(url);
        }

        private void linkSecond_Click(object sender, EventArgs e)
        {
            string url = "https://forum.truckersmp.com/index.php?/topic/79561-ts-saveeditor-tool";
            Process.Start(url);
        }

        private void linkLabelGitHub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "https://github.com/LIPtoH/TS-SE-Tool/releases/latest";
            Process.Start(url);
        }

        private void linkLabelHelpLocalPDF_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string file = "HowTo.pdf";
            if(File.Exists(file))
                Process.Start(file);
            else
            {
                MessageBox.Show("Missing manual. Try to repair via update", "HowTo.pdf not found");
            }
        }

        private void linkLabelHelpYouTube_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "https://liptoh.now.im/TS-SET-Tutorial";
            Process.Start(url);
        }
        //Main button
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_ClickCloseApp(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //Extra        
        private async void CheckLatestVersion()
        {
            if (CheckForUpdates)
            {
                linkLabelNewVersion.LinkBehavior = LinkBehavior.NeverUnderline;
                linkLabelNewVersion.Links[0].Enabled = false;
                linkLabelNewVersion.DisabledLinkColor = this.ForeColor;

                linkLabelNewVersion.Text = "Checking ...";
                buttonOK.Text = "Checking for updates";
                await Task.Run(() => Check());

                if (CheckComplete && NVavailible)
                {
                    bool betterVersion = false;
                    string[] numArr = NewVersion[0].Split(new char[] { '.' });
                    string[] currArr = Utilities.AssemblyData.AssemblyVersion.Split(new char[] { '.' });

                    for (byte i = 0; i < numArr.Length; i++)
                    {
                        if (byte.Parse(numArr[i]) > byte.Parse(currArr[i]))
                        {
                            betterVersion = true;
                            break;
                        }
                    }

                    if (betterVersion)
                    {
                        linkLabelNewVersion.Text = String.Format("New version {0} available!\r\n(Download)", NewVersion[0]);
                    }
                    else
                    {
                        linkLabelNewVersion.Text = String.Format("You are using latest version!\r\n(Repair)");
                    }

                    linkLabelNewVersion.LinkBehavior = LinkBehavior.AlwaysUnderline;
                    linkLabelNewVersion.Links[0].Enabled = true;
                    linkLabelNewVersion.LinkColor = Color.LimeGreen;
                    linkLabelNewVersion.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 204);

                    linkLabelNewVersion.Click += new EventHandler(linkLabelNewVersion_Click);
                }
                else
                {
                    tableLayoutPanel2.RowStyles[3] = new RowStyle(SizeType.Absolute, 30F);
                    linkLabelNewVersion.Text = String.Format("Cannot check for updates!", NewVersion[0]);
                    linkLabelNewVersion.LinkBehavior = LinkBehavior.NeverUnderline;
                    linkLabelNewVersion.Links[0].Enabled = false;
                    linkLabelNewVersion.DisabledLinkColor = Color.Crimson;
                    linkLabelNewVersion.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 204);
                }
            }
            else
                tableLayoutPanel2.RowStyles[3] = new RowStyle(SizeType.Absolute, 0F);

            buttonOK.Click += new EventHandler(this.button1_Click);
            buttonOK.Text = "OK";
        }

        private void Check()
        {
            string newversionData = GetLatestVersionData("https://liptoh.now.im/TS-SET-CheckVersion");

            if (newversionData != null)
            {
                CheckComplete = true;
                NewVersion = newversionData.Split(new char[] { '\t' });

                if (NewVersion[0] != null && NewVersion[0].Contains("TS.SE.Tool"))
                {
                    NVavailible = true;
                    NewVersion[0] = NewVersion[0].Replace("TS.SE.Tool.", "").Replace(".zip", "");
                }
            }
        }

        public string GetLatestVersionData(string url)
        {
            string result = null;

            using (WebClient client = new WebClient())
            {
                try
                {
                    result = client.DownloadString(url);
                }                  
                catch { }

                client.Dispose();
            }
            return result;
        }
    }
}

