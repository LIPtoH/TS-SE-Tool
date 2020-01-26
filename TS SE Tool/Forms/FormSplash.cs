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

            labelTSSE.Text = AssemblyProduct;

            string translatedString = MainForm.ResourceManagerMain.GetString(labelVersion.Name, Thread.CurrentThread.CurrentUICulture);
            if (translatedString != null)
                labelVersion.Text = String.Format(translatedString, AssemblyVersion);
            else
                labelVersion.Text = String.Format("{0} (alpha)", AssemblyVersion);
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
                button1.Click -= new EventHandler(this.button1_Click);
                button1.Text = "Close application";
                button1.Click += new EventHandler(this.button1_ClickCloseApp);
            }
        }

        //Actions
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

        private void linkLabelNewVersion_Click(object sender, EventArgs e)
        {
            /*
            string url = "https://rebrand.ly/TS-SET-Download";
            System.Diagnostics.Process.Start(url);
            */
            button1.Enabled = false;

            bool properFileDownloaded = false;
            byte trysCount = 0;

            linkLabelNewVersion.Text = String.Format("You are using latest version!\r\n(Repair)");
            linkLabelNewVersion.LinkBehavior = LinkBehavior.NeverUnderline;
            linkLabelNewVersion.Links[0].Enabled = false;
            linkLabelNewVersion.DisabledLinkColor = this.ForeColor;
            do
            {
                trysCount++;
                if (trysCount == 6)
                    break;

                startDownload();

                linkLabelNewVersion.Text = "Checking file...";
                properFileDownloaded = checkFileHash(Directory.GetCurrentDirectory() + @"\updater\ts.set.newversion.zip", NewVersion[1]);

                if (!properFileDownloaded)
                    linkLabelNewVersion.Text = "Hash not matching!";

                Thread.Sleep(1000);
            } while (!properFileDownloaded);

            linkLabelNewVersion.Text = "Ready for update.";

            if (properFileDownloaded)
            {
                //Close Start updater and TSSET
                linkLabelNewVersion.Text = "Starting Updater";
                button1.Text = "OK";
                //copy updater
                if (File.Exists(Directory.GetCurrentDirectory() + @"\updater\updater.exe"))
                {
                    File.Copy(Directory.GetCurrentDirectory() + @"\updater\updater.exe", Directory.GetCurrentDirectory() + @"\updater.exe", true);
                }
                else
                {
                    MessageBox.Show("Unable to find Updater.exe. Please update manually. New version located in Updater folder.", "File not exist");
                    linkLabelNewVersion.Text = "Updater.exe doesn't exist";
                    return;
                }

                if (File.Exists(Directory.GetCurrentDirectory() + @"\updater.exe"))
                {
                    Process.Start(Directory.GetCurrentDirectory() + @"\updater.exe", "true " + NewVersion[1] + " " + Process.GetCurrentProcess().Id.ToString());
                    linkLabelNewVersion.Text = "You can now finish your work.\r\nUpdate will start on exit.";

                    Application.Exit();
                }
                else
                {
                    MessageBox.Show("Unable to find Updater.exe. Please update manually. New version located in Updater folder.", "File not exist");
                    linkLabelNewVersion.Text = "Updater.exe doesn't exist";
                }

                button1.Enabled = true;
            }
            else
            {
                MessageBox.Show("Unable to download new version. Try later or download new version manually.", "Download failed");
            }
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
            string url = "https://rebrand.ly/TS-SET-Tutorial";
            System.Diagnostics.Process.Start(url);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_ClickCloseApp(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //Extra
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

        public string GetFilename(string url)
        {
            string result = null;

            using (WebClient client = new WebClient())
            {
                try
                {
                    client.OpenRead(url);

                    string header_contentDisposition = client.ResponseHeaders["content-disposition"];
                    result = new ContentDisposition(header_contentDisposition).FileName;
                }
                catch { }
            }

            return result;
        }
        
        private async void CheckLatestVersion()
        {
            if (CheckForUpdates)
            {
                linkLabelNewVersion.LinkBehavior = LinkBehavior.NeverUnderline;
                linkLabelNewVersion.Links[0].Enabled = false;
                linkLabelNewVersion.DisabledLinkColor = this.ForeColor;

                linkLabelNewVersion.Text = "Checking ...";
                button1.Text = "Checking for updates";
                await Task.Run(() => Check());

                if (CheckComplete && NVavailible)
                {
                    bool betterVersion = false;
                    string[] numArr = NewVersion[0].Split(new char[] { '.' });
                    string[] currArr = AssemblyVersion.Split(new char[] { '.' });

                    for (byte i = 0; i < numArr.Length; i++)
                    {
                        if (byte.Parse(numArr[i]) > byte.Parse(currArr[i]))
                        {
                            betterVersion = true;
                            break;
                        }
                    }

                    linkLabelNewVersion.LinkBehavior = LinkBehavior.AlwaysUnderline;
                    linkLabelNewVersion.LinkColor = Color.LimeGreen;
                    linkLabelNewVersion.Links[0].Enabled = true;

                    if (betterVersion)
                    {
                        linkLabelNewVersion.Text = String.Format("New version {0} available!\r\n(Download)", NewVersion[0]);
                        //linkLabelNewVersion.LinkColor = Color.LimeGreen;
                        linkLabelNewVersion.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 204);
                    }
                    else
                    {
                        linkLabelNewVersion.Text = String.Format("You are using latest version!\r\n(Repair)");
                        linkLabelNewVersion.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 204);
                    }

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

            button1.Click += new EventHandler(this.button1_Click);
            button1.Text = "OK";
        }

        private void Check()
        {
            string newversionData = GetLatestVersionData("https://rebrand.ly/TS-SET-CheckVersion");

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

        private bool checkFileHash(string _filepath, string _hashtocompare)
        {
            bool SameHash = false;

            using (var hash = SHA512.Create())
            {
                using (var stream = File.OpenRead(_filepath))
                {
                    var fileHash = hash.ComputeHash(stream);
                    string newHash = BitConverter.ToString(fileHash).Replace("-", "").ToUpperInvariant();

                    if (_hashtocompare == newHash)
                        SameHash = true;

                    return SameHash;
                }
            }
        }

        private void startDownload() //async 
        {
            //linkLabelNewVersion.ForeColor = this.ForeColor;
            linkLabelNewVersion.Text = "Downloading...";

            Task t = Task.Run(() => {
                var url = "https://rebrand.ly/TS-SET-Download";
                var filename = Directory.GetCurrentDirectory() + @"\updater\ts.set.newversion.zip";

                try
                {
                    using (WebClient webClient = new WebClient())
                    {
                       // webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                        webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                        webClient.DownloadFile(new Uri(url), filename); //DownloadFileTaskAsync
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
            t.Wait();
        }
        
        void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            button1.Enabled = true;
            linkLabelNewVersion.Text = "Downloaded!";
        }
    }
}

