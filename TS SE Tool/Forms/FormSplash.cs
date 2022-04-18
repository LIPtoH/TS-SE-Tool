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
using System.Reflection;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Mime;
using System.Diagnostics;
using System.Security.Cryptography;
using TS_SE_Tool.Utilities;

namespace TS_SE_Tool
{
    public partial class FormSplash : Form
    {
        string[] NewVersion = { "", "" };
        FormMain MainForm = Application.OpenForms.OfType<FormMain>().Single();

        bool CheckForUpdates = true;

        public FormSplash()
        {
            InitializeComponent();

            MainForm.HelpTranslateFormMethod(this);

            labelTSSE.Text = Utilities.AssemblyData.AssemblyProduct;

            string translatedString = MainForm.ResourceManagerMain.GetString(labelVersion.Name, Thread.CurrentThread.CurrentUICulture);
            if (translatedString != null)
                labelVersion.Text = String.Format(translatedString, Utilities.AssemblyData.AssemblyVersion);
            else
                labelVersion.Text = String.Format("{0} (alpha)", Utilities.AssemblyData.AssemblyVersion);

            labelSupportDeveloper.Visible = false;
            buttonSupportDeveloper.Visible = false;
        }

        private void FormSplash_Load(object sender, EventArgs e)
        {
            try
            {
                CheckForUpdates = Properties.Settings.Default.CheckUpdatesOnStartup;
            }
            catch
            {
                MessageBox.Show("Please update manually in order to fix it.", "Installation corrupted", MessageBoxButtons.OK, MessageBoxIcon.Error);
                buttonOK.Click -= new EventHandler(this.buttonOK_Click);
                buttonOK.Text = "Close application";
                buttonOK.Click += new EventHandler(this.buttonOK_ClickCloseApp);
            }

            CheckLatestVersion();
        }

        private void FormSplash_Shown(object sender, EventArgs e)
        { }

        //Actions
        private void linkLabelNewVersion_Click(object sender, EventArgs e)
        {
            buttonOK.Enabled = false;

            FormCheckUpdates FormWindow = new FormCheckUpdates("download");
            FormWindow.NewVersion = NewVersion;

            DialogResult t = FormWindow.ShowDialog();

            if (t == DialogResult.OK)
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

            buttonOK.Enabled = true;
        }
        //Links
        private void linkFirst_Click(object sender, EventArgs e)
        {
            Process.Start(Utilities.Web_Utilities.External.linkSCSforum);
        }

        private void linkSecond_Click(object sender, EventArgs e)
        {
            Process.Start(Utilities.Web_Utilities.External.linTMPforum);
        }

        private void linkLabelGitHub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(Utilities.Web_Utilities.External.linGithubReleasesLatest);
        }

        private void linkLabelHelpLocalPDF_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string file = "HowTo.pdf";

            if (File.Exists(file))
                Process.Start(file);
            else
                MessageBox.Show("Missing manual. Try to repair via update", "HowTo.pdf not found");
        }

        private void linkLabelHelpYouTube_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(Utilities.Web_Utilities.External.linkYoutubeTutorial);
        }
        //Main button
        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonOK_ClickCloseApp(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void buttonSupportDeveloper_Click(object sender, EventArgs e)
        {
            string url = Utilities.Web_Utilities.External.linkHelpDeveloper;

            DialogResult result = MessageBox.Show("This will open " + url + " web-page.\nDo you want to continue?", "Support developer", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
                System.Diagnostics.Process.Start(url);
        }

        //Extra
        private async void CheckLatestVersion()
        {
            if ( !MainForm.TssetFoldersExist || (CheckForUpdates && Web_Utilities.External.CheckNewVersionTimeElapsed(MainForm.ProgSettingsV.LastUpdateCheck)) )
            {
                if (MainForm.TssetFoldersExist)
                {
                    SetLinkLabelNewVersionvisual(visualStatus.neutral);

                    linkLabelNewVersion.Text = "Checking ";

                    buttonOK.Text = "Checking for updates";
                }
                else
                {
                    SetLinkLabelNewVersionvisual(visualStatus.neutralBold);

                    linkLabelNewVersion.Text = String.Format("Your version lacking important files!\r\nGetting link ");

                    buttonOK.Text = "Getting a link to restore important files";
                }

                NewVersion = await Task.Run(() => Web_Utilities.External.CheckNewVersionAvailability(linkLabelNewVersion));

                if (NewVersion != null && NewVersion[0] != "" && NewVersion[1] != "")
                {
                    bool? betterVersion = false;
                    betterVersion = Web_Utilities.External.CheckNewVersionStatus(NewVersion);

                    if (betterVersion is bool checkBool)
                    {
                        if (checkBool)
                        {
                            linkLabelNewVersion.Text = String.Format("New version {0} available!\r\n(Download)", NewVersion[0]);

                            SetLinkLabelNewVersionvisual(visualStatus.good);
                        }
                        else
                        {
                            if (MainForm.TssetFoldersExist)
                                linkLabelNewVersion.Text = String.Format("You are using latest version!\r\n(Repair)");
                            else
                                linkLabelNewVersion.Text = String.Format("Your version lacking important files!\r\n(Repair)");

                            SetLinkLabelNewVersionvisual(visualStatus.neutralLinked);
                        }

                        linkLabelNewVersion.Click += new EventHandler(linkLabelNewVersion_Click);
                    }
                    else
                    {
                        linkLabelNewVersion.Text = String.Format("Hello developer =)");

                        SetLinkLabelNewVersionvisual(visualStatus.neutral);
                    }
                }
                else
                {
                    tableLayoutPanel2.RowStyles[3] = new RowStyle(SizeType.Absolute, 30F);

                    linkLabelNewVersion.Text = String.Format("Cannot check for updates!");

                    SetLinkLabelNewVersionvisual(visualStatus.bad);
                }

                //Update time whaen checking for updates
                MainForm.ProgSettingsV.LastUpdateCheck = DateTime.Now;
            }
            else
                tableLayoutPanel2.RowStyles[3] = new RowStyle(SizeType.Absolute, 0F);

            if (MainForm.TssetFoldersExist)
            {
                buttonOK.Click += new EventHandler(buttonOK_Click);
                buttonOK.Text = "OK";
            }
            else
            {
                buttonOK.Click += new EventHandler(buttonOK_ClickCloseApp);
                buttonOK.Text = "Close";
            }
        }

        //
        internal enum visualStatus : SByte
        {
            neutral = 0,
            good = 1,
            neutralBold = 2,
            neutralLinked = 3,
            bad = -1
        }

        private void SetLinkLabelNewVersionvisual(visualStatus _status)
        {
            switch (_status)
            {
                case visualStatus.neutral:
                    {
                        linkLabelNewVersion.Links[0].Enabled = false;

                        linkLabelNewVersion.LinkBehavior = LinkBehavior.NeverUnderline;
                        linkLabelNewVersion.LinkColor = this.ForeColor;
                        linkLabelNewVersion.DisabledLinkColor = this.ForeColor;
                        linkLabelNewVersion.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 204);

                        break;
                    }

                case visualStatus.neutralBold:
                    {
                        linkLabelNewVersion.Links[0].Enabled = false;

                        linkLabelNewVersion.LinkBehavior = LinkBehavior.NeverUnderline;
                        linkLabelNewVersion.LinkColor = this.ForeColor;
                        linkLabelNewVersion.DisabledLinkColor = this.ForeColor;
                        linkLabelNewVersion.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 204);

                        break;
                    }

                case visualStatus.neutralLinked:
                    {
                        linkLabelNewVersion.Links[0].Enabled = true;

                        linkLabelNewVersion.LinkBehavior = LinkBehavior.AlwaysUnderline;
                        linkLabelNewVersion.LinkColor = this.ForeColor;
                        linkLabelNewVersion.DisabledLinkColor = this.ForeColor;
                        linkLabelNewVersion.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 204);

                        break;
                    }

                case visualStatus.good:
                    {
                        linkLabelNewVersion.Links[0].Enabled = true;

                        linkLabelNewVersion.LinkBehavior = LinkBehavior.AlwaysUnderline;
                        linkLabelNewVersion.LinkColor = Color.LimeGreen;
                        linkLabelNewVersion.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 204);

                        break;
                    }

                case visualStatus.bad:
                    {
                        linkLabelNewVersion.Links[0].Enabled = false;

                        linkLabelNewVersion.LinkBehavior = LinkBehavior.NeverUnderline;
                        linkLabelNewVersion.DisabledLinkColor = Color.Crimson;
                        linkLabelNewVersion.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 204);

                        break;
                    }
            }
        }

    }
}

