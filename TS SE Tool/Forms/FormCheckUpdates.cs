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
    public partial class FormCheckUpdates : Form
    {
        bool NVavailible = false, CheckComplete = false;
        string[] NewVersion = { "", "" };

        public FormCheckUpdates()
        {
            InitializeComponent();
            this.Size = new Size(300, 130);
        }

        private void FormCheckUpdates_Load(object sender, EventArgs e)
        {
            CheckLatestVersion();
            buttonDownload.Text = "Download && Update";
        }
        //Buttons
        private void buttonOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonDownload_Click(object sender, EventArgs e)
        {
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
                MessageBox.Show("Unable to find Updater.exe. Please update manually. New version located in Updater folder.", "File not exist");
                labelStatus.Text = "Updater.exe doesn't exist";
                return;
            }

            if (File.Exists(Directory.GetCurrentDirectory() + @"\updater.exe"))
            {
                this.Size = new Size(300, 130);
                Process.Start(Directory.GetCurrentDirectory() + @"\updater.exe", "true " + NewVersion[1] + " " + Process.GetCurrentProcess().Id.ToString());
                labelStatus.Text = "You can now finish your work.\r\nUpdate will start on exit.";

                Application.Exit();
            }
            else
            {
                MessageBox.Show("Unable to find Updater.exe. Please update manually. New version located in Updater folder.", "File not exist");
                labelStatus.Text = "Updater.exe doesn't exist";
            }

            buttonOK.Enabled = true;
        }
        //Events
        private async void CheckLatestVersion()
        {
            labelStatus.Text = "Checking for updates";
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

                if (betterVersion)
                {
                    labelStatus.Text = String.Format("New version {0} available!", NewVersion[0]);
                    labelStatus.ForeColor = Color.LimeGreen;
                    labelStatus.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 204);

                    buttonDownload.Visible = true;
                    buttonDownload.Click += new EventHandler(this.buttonDownload_Click);

                    this.Size = new Size( 300, 225 );

                    buttonOK.Text = "Cancel";
                }
                else
                {
                    labelStatus.Text = String.Format("You are using latest version!");
                    labelStatus.ForeColor = Color.LimeGreen;
                    labelStatus.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 204);

                    buttonDownload.Visible = true;
                    buttonDownload.Click += new EventHandler(this.buttonDownload_Click);
                    buttonDownload.Text = "Download && Repair";

                    this.Size = new Size(300, 225);

                    buttonOK.Text = "Cancel";
                }
            }
            else
            {
                labelStatus.Text = String.Format("Cannot check for updates!");
                labelStatus.ForeColor = Color.Crimson;
                labelStatus.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 204);
            }

            buttonOK.Click += new EventHandler(this.buttonOk_Click);
            buttonOK.Enabled = true;
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

        private void startDownload() //async 
        {
            labelStatus.ForeColor = this.ForeColor;
            labelStatus.Text = "Downloading...";

            Task t = Task.Run( () => {
                var url = "https://rebrand.ly/TS-SET-Download";
                var filename = Directory.GetCurrentDirectory() + @"\updater\ts.set.newversion.zip";

                try
                {
                    using (WebClient webClient = new WebClient())
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
            });
            t.Wait();
        }

        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate {
                double bytesIn = double.Parse(e.BytesReceived.ToString());
                double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
                double percentage = bytesIn / totalBytes * 100;
                //label2.Text = "Downloaded " + e.BytesReceived + " of " + e.TotalBytesToReceive;
                progressBarDownload.Value = int.Parse(Math.Truncate(percentage).ToString());
            });
        }

        void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            /*
            this.BeginInvoke((MethodInvoker)delegate {
                label2.Text = "Completed";
            });
            */
            buttonOK.Enabled = true;
            labelStatus.Text = "Downloaded!";
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
