using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;
using System.Diagnostics;
using System.Security.Cryptography;

namespace TS_SET_Updater
{
    
    public partial class FormMain : Form
    {
        bool updateStatus = false;
        int ProcessID = -1;

        string fileHASHnew = "Error", fileHASHold = "Error";

        public FormMain(string[] args)
        {
            InitializeComponent();

            if(args.Length > 0)
            {
                updateStatus = bool.Parse(args[0]);

                fileHASHold = args[1];

                if (updateStatus)
                    ProcessID = int.Parse(args[2]);
            }
            else
            {
                Application.Exit();
            }
        }

        private void FormMain_Shown(object sender, EventArgs e)
        {
            if (updateStatus)
            {
                extWinPos windowPosition = new extWinPos();
                Rectangle windowRectangle = windowPosition.GetExtWinRectangle(ProcessID);

                this.Location = new Point(windowRectangle.X + (windowRectangle.Width - this.Width) / 2, windowRectangle.Y + (windowRectangle.Height - this.Height) / 2);

                Updater();
            }
            else
                createDatafile(fileHASHold);
        }

        private async void Updater()
        {
            string SourceFileName = "TS SE Tool.exe";
            try
            {
                Process tsset = Process.GetProcessById(ProcessID);
                SourceFileName = tsset.MainModule.FileName;

                this.Text = "Updater Waiting for " + Path.GetFileName(SourceFileName) + " exit";
                tsset.WaitForExit();
            }
            catch
            { }

            this.Text = "Updater";
            this.WindowState = FormWindowState.Normal;

            string updFilePath = @".\updater\ts.set.newversion.zip";

            if (File.Exists(updFilePath))
            {
                bool goodFile = checkFileHash(updFilePath, fileHASHold);

                if (goodFile)
                {
                    this.Size = new Size(300, 160);

                    string extractPath = @".\";

                    labelStatus.Text = "Extracting update files.";
                    progressBar1.Visible = true;
                    await Task.Run(() => ExtractZipFile(updFilePath, extractPath));

                    labelStatus.Text = "All files extracted.";
                    progressBar1.Visible = false;

                    this.Size = new Size(300, 130);

                    try
                    {
                        if (File.Exists(SourceFileName))
                        {
                            Process.Start(SourceFileName);
                            File.Delete(updFilePath);
                        }                            
                    }
                    catch
                    {
                        MessageBox.Show("EXE file missing.", "File not exist");
                    }
                    finally
                    {
                        Application.Exit();
                    }
                }
                else
                {
                    labelStatus.Text = "Error";
                    MessageBox.Show("Update file failed integrity check. Please update manually of try again." + Environment.NewLine +
                        "Expected Hash: " + fileHASHold + Environment.NewLine + "Calculated Hash: " + fileHASHnew, "File was corrupted");
                    Application.Exit();
                }
            }
            else
            {
                labelStatus.Text = "Error";
                MessageBox.Show("Unable to find Update files. Please update manually.", "File not exist");
                Application.Exit();
            }
        }

        public Progress<ZipProgress> _progress;

        private void Report(object sender, ZipProgress zipProgress)
        {
            int persentagComplete = zipProgress.Processed / zipProgress.Total * 100;

            progressBar1.Value = persentagComplete;
        }

        public void ExtractZipFile (string _zipPath, string _extractPath)
        {
            _progress = new Progress<ZipProgress>();
            _progress.ProgressChanged += Report;

            Stream zipReadingStream = File.OpenRead(_zipPath);
            if (zipReadingStream != null)
            {
                ZipArchive zip = new ZipArchive(zipReadingStream);
                zip.ExtractToDirectory(_extractPath, _progress, true);
                zipReadingStream.Close();
            }
            else
                MessageBox.Show("zipReadingStream null");
        }

        private bool checkFileHash(string _filepath, string _hashtocompare)
        {
            bool SameHash = false;

            using (var hash = SHA512.Create())
            {
                using (var stream = File.OpenRead(_filepath))
                {
                    var fileHash = hash.ComputeHash(stream);
                    fileHASHnew = BitConverter.ToString(fileHash).Replace("-", "").ToUpperInvariant();

                    if (_hashtocompare == fileHASHnew)
                        SameHash = true;

                    return SameHash;
                }
            }
        }

        private void createDatafile(string _filepath)
        {
            string FileName = Path.GetFileName(_filepath).Replace(' ','.');// "";
            string FullPath = Path.GetDirectoryName(_filepath);
            string HashData = "";

            using (var hash = SHA512.Create())
            {
                using (var stream = File.OpenRead(_filepath))
                {
                    var fileHash = hash.ComputeHash(stream);
                    HashData = BitConverter.ToString(fileHash).ToUpperInvariant().Replace("-", "");
                }
            }

            try
            {
                using (StreamWriter writer = new StreamWriter(FullPath + @"\fileData.txt", false))
                {
                    writer.Write(FileName + "\t" + HashData);
                }
            }
            catch
            { }
            finally
            {
                Application.Exit();
            }
        }

    }    
}
