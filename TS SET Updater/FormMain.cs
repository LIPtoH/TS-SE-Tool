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

        private void FormMain_Load(object sender, EventArgs e)
        {
            if (updateStatus)
                Updater();
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
                    string zipPath = @".\updater\ts.set.newversion.zip";
                    string extractPath = @".\";
                    string[] dt = Directory.GetDirectories(extractPath);

                    labelStatus.Text = "Extracting update files.";
                    await Task.Run(() => ExtractZipFile(zipPath, extractPath));
                    //ZipFile.ExtractToDirectory(zipPath, extractPath);
                    labelStatus.Text = "All files extracted.";
                    try
                    {
                        if(File.Exists(@".\TS SE Tool.exe"))
                            Process.Start(@".\TS SE Tool.exe");
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
                    MessageBox.Show("Update file failed integrity check. Please update manually of try again.", "File was corrupted");
                }
            }
            else
            {
                MessageBox.Show("Unable to find Update files. Please update manually.", "File not exist");
            }
        }

        public Progress<ZipProgress> _progress;

        private void Report(object sender, ZipProgress zipProgress)
        {
            int persentagComplete = zipProgress.Processed / zipProgress.Total * 100;

            //labelStatus.Text = "Progress " + persentagComplete.ToString() + " %";
            progressBar1.Value = persentagComplete;
        }

        public void ExtractZipFile (string _zipPath, string _extractPath)
        {
            Stream zipReadingStream = File.OpenRead(_zipPath);
            ZipArchive zip = new ZipArchive(zipReadingStream);
            zip.ExtractToDirectory(_extractPath, _progress, true);
        }

        private bool checkFileHash(string _filepath, string _hashtocompare)
        {
            bool SameHash = false;

            using (var hash = SHA512.Create())
            {
                using (var stream = File.OpenRead(_filepath))
                {
                    var fileHash = hash.ComputeHash(stream);
                    string newHash = BitConverter.ToString(fileHash).Replace("-", "").ToLowerInvariant();

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
