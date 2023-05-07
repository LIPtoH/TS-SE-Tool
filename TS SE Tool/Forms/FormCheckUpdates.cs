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
    public partial class FormCheckUpdates : Form
    {
        FormMain MainForm = Application.OpenForms.OfType<FormMain>().Single();

        internal string[] NewVersion = { "", "" };
        string FormMode = "check";

        public FormCheckUpdates(string _formMode)
        {
            InitializeComponent();
            this.Size = new Size(300, 130);
            FormMode = _formMode;
        }

        private void FormCheckUpdates_Load(object sender, EventArgs e)
        {
            buttonDownload.Text = "Download && Update";

            if (FormMode == "check")
                CheckLatestVersion();

            else if(FormMode == "download")
            {
                labelStatus.Text = labelStatus.Text = String.Format("New version {0} available!\r\nClick on Download to update.", NewVersion[0]);

                buttonDownload.Visible = true;
                buttonDownload.Click += new EventHandler(this.buttonDownload_Click);

                this.Size = new Size(300, 180);
            }
        }
        //Buttons
        private void buttonOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonDownload_Click(object sender, EventArgs e)
        {
            this.Size = new Size(300, 225);

            buttonDownload.Enabled = false;
            buttonOK.Enabled = false;

            progressBarDownload.Visible = true;

            bool properFileDownloaded = false;
            byte trysCount = 0;

            do
            {
                trysCount++;
                if (trysCount == 6)
                    break;

                startDownload();

                labelStatus.Text = "Checking file...";
                properFileDownloaded = checkFileHash(Directory.GetCurrentDirectory() + @"\updater\ts.set.newversion.zip", NewVersion[1]);

                if (!properFileDownloaded)
                    labelStatus.Text = "Hash not matching!";

                Thread.Sleep(1000);
            } while (!properFileDownloaded);

            labelStatus.Text = "Ready for update.";
            progressBarDownload.Visible = false;

            if (properFileDownloaded)
            {
                buttonDownload.Click -= new EventHandler(this.buttonDownload_Click);
                //buttonDownload.Text = "Update";
                buttonDownload.Click += new EventHandler(this.buttonUpdate_Click);
                buttonDownload.Enabled = true;
                if (true)
                    buttonDownload.PerformClick();
            }
            else
            {
                MessageBox.Show("Unable to download new version. Try later or download new version manually.", "Download failed");
                this.Close();
                this.DialogResult = DialogResult.Abort;
            }
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            //Close Start updater and TSSET
            buttonDownload.Visible = false;
            labelStatus.Text = "Starting Updater";
            buttonOK.Text = "OK";

            //copy updater
            if (File.Exists(Directory.GetCurrentDirectory() + @"\updater\updater.exe"))
            {
                File.Copy(Directory.GetCurrentDirectory() + @"\updater\updater.exe", Directory.GetCurrentDirectory() + @"\updater.exe", true);
            }
            else
            {
                MessageBox.Show("Unable to find Updater.exe." + Environment.NewLine + "Please update manually." + Environment.NewLine + "New version located in Updater folder.", "Updater not exist. Manual update");
                labelStatus.Text = "Updater.exe doesn't exist";

                Process.Start("updater");

                MainForm.ForseExit = true;
                Application.Exit();
            }

            if (File.Exists(Directory.GetCurrentDirectory() + @"\updater.exe"))
            {
                this.Size = new Size(300, 130);
                Process.Start(Directory.GetCurrentDirectory() + @"\updater.exe", "true " + NewVersion[1] + " " + Process.GetCurrentProcess().Id.ToString());
                labelStatus.Text = "You can now finish your work.\r\nUpdate will start on exit.";

                MainForm.ForseExit = true;
                Application.Exit();
            }
            else
            {
                MessageBox.Show("Unable to find Updater.exe. Please update manually. New version located in Updater folder.", "File not exist");
                labelStatus.Text = "Updater.exe doesn't exist";

                Process.Start("updater");

                MainForm.ForseExit = true;
                Application.Exit();
            }

            buttonOK.Enabled = true;
        }
        //Events
        //Check
        private async void CheckLatestVersion()
        {
            labelStatus.Text = "Checking for updates";

            NewVersion = await Task.Run(() => Web_Utilities.External.CheckNewVersionAvailability(labelStatus));

            if (NewVersion != null && NewVersion[0] != "" && NewVersion[1] != "")
            {
                bool? betterVersion = false;
                betterVersion = Web_Utilities.External.CheckNewVersionStatus(NewVersion);

                if (betterVersion is bool checkBool)
                {
                    this.Size = new Size(300, 170);

                    buttonDownload.Visible = true;
                    buttonDownload.Click += new EventHandler(this.buttonDownload_Click);
                    buttonOK.Text = "Cancel";


                    if (checkBool)
                    {
                        labelStatus.Text = String.Format("New version {0} available!", NewVersion[0]);

                        SetStatusLabelvisual(visualStatus.good);
                    }
                    else
                    {
                        labelStatus.Text = String.Format("You are using latest version!");

                        SetStatusLabelvisual(visualStatus.neutral);

                        buttonDownload.Text = "Download && Repair";
                    }
                }
                else
                {
                    labelStatus.Text = String.Format("Hello developer =)");

                    SetStatusLabelvisual(visualStatus.neutral);
                }
            }
            else
            {
                labelStatus.Text = String.Format("Cannot check for updates!");

                SetStatusLabelvisual(visualStatus.bad);
            }

            buttonOK.Click += new EventHandler(this.buttonOk_Click);
            buttonOK.Enabled = true;
        }

        internal enum visualStatus : SByte
        {
            neutral = 0,
            good = 1,
            bad = -1
        }

        private void SetStatusLabelvisual(visualStatus _status)
        {
            switch (_status)
            {
                case visualStatus.neutral:
                    {
                        labelStatus.ForeColor = this.ForeColor;
                        labelStatus.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 204);

                        break;
                    }

                case visualStatus.good:
                    {
                        labelStatus.ForeColor = Color.LimeGreen;
                        labelStatus.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 204);

                        break;
                    }
                case visualStatus.bad:
                    {
                        labelStatus.ForeColor = Color.Crimson;
                        labelStatus.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 204);

                        break;
                    }
            }
        }

        //
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

        //Download
        private void startDownload() //async 
        {
            labelStatus.ForeColor = this.ForeColor;
            labelStatus.Text = "Downloading...";

            if(!Directory.Exists("updater"))
            {
                Directory.CreateDirectory("updater");
            }    

            Task t = Task.Run(() => 
            {

                var filename = Directory.GetCurrentDirectory() + @"\updater\ts.set.newversion.zip";

                foreach (string url in new[] { Web_Utilities.External.linkDownloadVersion, Web_Utilities.External.linkDownloadVersion2 })
                {
                    bool available =  Web_Utilities.External.RemoteFileExists(url);

                    if (available)
                    {
                        try
                        {
                            using (Web_Utilities.WebClientWithTO webClient = new Web_Utilities.WebClientWithTO())
                            {
                                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                                webClient.DownloadFile(new Uri(url), filename); //DownloadFileTaskAsync
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }

            });
            t.Wait();
        }

        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate {
                double bytesIn = double.Parse(e.BytesReceived.ToString());
                double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
                double percentage = bytesIn / totalBytes * 100;
                progressBarDownload.Value = int.Parse(Math.Truncate(percentage).ToString());
            });
        }

        void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            buttonOK.Enabled = true;
            labelStatus.Text = "Downloaded!";
        }
        //
    }
}
