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
        string FileHash = "";
        int ProcessID = -1;

        public FormMain(string[] args)
        {
            InitializeComponent();

            if(args.Length > 0)
            {
                updateStatus = bool.Parse(args[0]);
                FileHash = args[1];
                if(updateStatus)
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
                extWinPos wp = new extWinPos();
                Rectangle wpR = wp.GetExtWinRectangle(ProcessID);
                this.Location = new Point(wpR.X + (wpR.Width - this.Width) / 2, wpR.Y + (wpR.Height - this.Height) / 2);

                MessageBox.Show(Location.ToString());

                Updater();
            }
            else
                createDatafile(FileHash);
        }

        private async void Updater()
        {
            Process tsset = Process.GetProcessById(ProcessID);
            
            string SourceFileName = tsset.MainModule.FileName;
            
            tsset.WaitForExit();

            string updFilePath = @".\updater\ts.set.newversion.zip";

            if (File.Exists(updFilePath))
            {
                bool goodFile = checkFileHash(updFilePath, FileHash);

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
                        if(File.Exists(SourceFileName)) // SourceFileName
                        {
                            Process.Start(SourceFileName); // SourceFileName
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
                    MessageBox.Show("Update file failed integrity check. Please update manually of try again.", "File was corrupted");
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
                    string newHash = BitConverter.ToString(fileHash).Replace("-", "").ToUpperInvariant();

                    if (_hashtocompare == newHash)
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
            {
            }
            finally
            {
                Application.Exit();
            }

        }

    }    
}
