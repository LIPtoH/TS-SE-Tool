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
using System.IO;
using System.IO.Compression;

namespace TS_SE_Tool
{
    public partial class FormProfileEditorRenameClone : Form
    {
        public string ReturnNewName { get; set; }
        public List<string> ReturnClonedNames { get; set; } = new List<string>();
        public bool ReturnRenamedSuccessful { get; set; } = false;
        public bool ReturnCloningSuccessful { get; set; } = false;

        private string FormMode;

        public new FormMain ParentForm;
        public string InitialName = "";
        public string InitialPath = "";
        private byte NameLengthLimit = 30;

        private bool aboveLimitLength = false;
        private int FormWidthMin = 0;
        private int textBoxNewNameWidthMin = 160;
        private int tbNewNameWidth = 0;
        private int tbNewNameWidthMulty = 0;

        public FormProfileEditorRenameClone(string _mode)
        {
            InitializeComponent();
            this.Icon = Properties.Resources.MainIco;

            FormMode = _mode;
            //Translate();
            SetupForm();
        }

        private void SetupForm()
        {
            //

            switch (FormMode)
            {
                case "rename":
                    {
                        this.Text = "Renaming";

                        labelNewName.Text = "New profile name:";
                        textBoxNewName.MaxLength = NameLengthLimit;

                        checkBoxMutiCloning.Visible = false;
                        checkBoxFullCloning.Visible = false;

                        break;
                    }
                case "clone":
                    {
                        this.Text = "Cloning";

                        labelNewName.Text = "Name for cloned profile(s):";
                        checkBoxCreateBackup.Visible = false;

                        checkBoxFullCloning.Location = checkBoxMutiCloning.Location;
                        checkBoxMutiCloning.Location = checkBoxCreateBackup.Location;

                        break;
                    }
            }
            //Group box and label width
            tableLayoutPanelControls.ColumnStyles[0].Width = 6 + ((labelNewName.PreferredWidth > groupBoxOptions.PreferredSize.Width) ? labelNewName.PreferredWidth : groupBoxOptions.PreferredSize.Width);
            
            //Name textbox width
            int tcolwidth = tableLayoutPanelControls.GetColumnWidths()[1];
            //
            if (tcolwidth - 6 >= textBoxNewNameWidthMin)
                textBoxNewName.Width = tcolwidth;
            else
            {
                this.Width += textBoxNewNameWidthMin - tcolwidth + 6;
                textBoxNewName.Width = textBoxNewNameWidthMin;
            }

            textBoxNewNameWidthMin = textBoxNewName.Width;

            FormWidthMin = this.Width;
        }

        //Nmae textbox
        private void textBoxNewName_KeyDown(object sender, KeyEventArgs e)
        {
            if (textBoxNewName.Text.Length == 0) return;

            if (e.KeyCode == Keys.Return || e.KeyCode == Keys.Back || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right || e.KeyCode == Keys.Home || e.KeyCode == Keys.End || e.KeyCode == Keys.Delete)
            {
                e.Handled = false;
                aboveLimitLength = false;
                return;
            }

            int index = textBoxNewName.GetLineFromCharIndex(textBoxNewName.SelectionStart);

            string temp = textBoxNewName.Lines[index];

            Graphics gr = CreateGraphics();
            SizeF tSize = gr.MeasureString(textBoxNewName.Text, textBoxNewName.Font);

            tbNewNameWidth = (int)Math.Ceiling(Convert.ToDouble(tSize.Width));

            aboveLimitLength = (temp.Length < NameLengthLimit) ? false : true;
        }

        private void textBoxNewName_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Check for the flag being set in the KeyDown event
            if (aboveLimitLength == true)
            {
                // Stop the character from being entered into the control
                e.Handled = true;
            }
        }

        private void textBoxNewName_TextChanged(object sender, EventArgs e)
        {
            int extraM = textBoxNewName.Font.Height * 2;

            if (textBoxNewName.Multiline)
            {

                int tNewheight = textBoxNewName.Font.Height * (textBoxNewName.Lines.Count() + 1) + 7;
                int tminHeight = textBoxNewName.Font.Height * 3 + 7;

                if (tminHeight < tNewheight)
                    textBoxNewName.Height = tNewheight;
                else
                    textBoxNewName.Height = tminHeight;

                textBoxNewName.ScrollToCaret();

                //Search for longest line
                Graphics gr = CreateGraphics();

                tbNewNameWidthMulty = 0;

                foreach (string newfile in textBoxNewName.Lines)
                {
                    if (newfile.Length == 0 || newfile.Trim(new char[] { ' ' }) == "")
                        continue;

                    SizeF tSize = gr.MeasureString(newfile, textBoxNewName.Font);

                    int tWidth = (int)Math.Ceiling(Convert.ToDouble(tSize.Width));

                    if (tWidth > tbNewNameWidthMulty)
                        tbNewNameWidthMulty = tWidth;
                }

                if (tbNewNameWidthMulty + extraM >= textBoxNewNameWidthMin)
                    this.Width = FormWidthMin + tbNewNameWidthMulty - textBoxNewNameWidthMin + extraM + SystemInformation.VerticalScrollBarWidth;
                else
                    this.Width = FormWidthMin + SystemInformation.VerticalScrollBarWidth;

                textBoxNewName.Width = tableLayoutPanelControls.GetColumnWidths()[1];
            }
            else
            {
                tableLayoutPanelControls.SetRowSpan(textBoxNewName, 1);

                //
                Graphics gr = CreateGraphics();
                SizeF tSize = gr.MeasureString(textBoxNewName.Text, textBoxNewName.Font);
                tbNewNameWidth = (int)Math.Ceiling(Convert.ToDouble(tSize.Width));


                if (tbNewNameWidth + extraM >= textBoxNewNameWidthMin)
                    this.Width = FormWidthMin + tbNewNameWidth - textBoxNewNameWidthMin + extraM;
                else
                    this.Width = FormWidthMin;

                textBoxNewName.Width = tableLayoutPanelControls.GetColumnWidths()[1];
            }
        }
        
        //Checkboxes
        private void checkBoxMutiCloning_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxMutiCloning.Checked)
            {
                textBoxNewName.Multiline = true;
                textBoxNewName.ScrollBars = ScrollBars.Vertical;

                tableLayoutPanelControls.SetRowSpan(textBoxNewName, 2);

                int tNewheight = textBoxNewName.Font.Height * (textBoxNewName.Lines.Count() + 1) + 7;
                int tminHeight = textBoxNewName.Font.Height * 3 + 7;

                if (tminHeight < tNewheight)
                    textBoxNewName.Height = tNewheight;
                else
                    textBoxNewName.Height = tminHeight;
            }
            else
            {
                textBoxNewName.Multiline = false;
                textBoxNewName.ScrollBars = ScrollBars.None;
                textBoxNewName.Text = (textBoxNewName.Lines.Count() != 0) ? textBoxNewName.Lines[0] : "";
            }
        }
        
        //Buttons
        private void buttonAccept_Click(object sender, EventArgs e)
        {
            ReturnNewName = textBoxNewName.Text;

            switch (FormMode)
            {
                case "rename":
                    {
                        string NewProfileName = NewProfileName = textBoxNewName.Text.Trim(new char[] { ' ' }) , NewFolderName = "", NewFolderPath = "";
                        byte progress = 0;

                        //Get existing folders
                        string[] existingDirs = Directory.GetDirectories(InitialPath.Remove(InitialPath.LastIndexOf('\\') + 1));

                        List<string> existingDirList = new List<string>();

                        foreach (string dir in existingDirs)
                        {
                            //Get name 
                            existingDirList.Add(Utilities.TextUtilities.FromHexToString(Path.GetFileName(dir)));
                        }

                        if (NewProfileName != InitialName || existingDirList.Contains(NewProfileName))
                        {
                            try
                            {
                                //New folder name                                
                                NewFolderName = Utilities.TextUtilities.FromStringToHex(NewProfileName);
                                NewFolderPath = InitialPath.Remove(InitialPath.LastIndexOf('\\') + 1) + NewFolderName;

                                progress = 1;

                                //Create folder and copy
                                Directory.CreateDirectory(NewFolderPath);
                                progress = 2;
                                Utilities.IO_Utilities.DirectoryCopy(InitialPath, NewFolderPath, true);
                                progress = 3;

                                //Decode profile.sii
                                FormMain tF = new FormMain();
                                string[] profileFile = tF.NewDecodeFile(NewFolderPath + "\\profile.sii");
                                progress = 4;

                                SaveFileProfileData ProfileData = new SaveFileProfileData();
                                ProfileData.Prepare(profileFile);
                                progress = 5;

                                //New name
                                ProfileData.ProfileName = textBoxNewName.Text;
                                progress = 6;

                                //Write file
                                using (StreamWriter SW = new StreamWriter(NewFolderPath + "\\profile.sii", false))
                                {
                                    ProfileData.WriteToStream(SW);
                                }
                                progress = 7;

                                //Make backup
                                if (checkBoxCreateBackup.Checked)
                                    ZipFile.CreateFromDirectory(InitialPath, InitialPath + ".zip");
                                progress = 8;

                                //Delete old folder
                                Directory.Delete(InitialPath, true);
                                progress = 9;

                                ReturnNewName = NewProfileName;
                                ReturnRenamedSuccessful = true;
                            }
                            catch
                            {
                                switch (progress)
                                {
                                    case 0:
                                        MessageBox.Show("Create new folder name failed");
                                        break;
                                    case 1:
                                        MessageBox.Show("Directory was not created");
                                        break;
                                    case 2:
                                        MessageBox.Show("Directory copy failed");
                                        goto delete;
                                    case 3:
                                        MessageBox.Show("Profile not decoded");
                                        goto delete;
                                    case 4:
                                        MessageBox.Show("Profile hase wrong version/format");
                                        goto delete;
                                    case 5:
                                        MessageBox.Show("Profile name is not applied");
                                        goto delete;
                                    case 6:
                                        MessageBox.Show("Profile write failed");
                                        goto delete;
                                    case 7:
                                        MessageBox.Show("Profile backup creation failed. Keeping both versions. Delete old profile manually.");
                                        break;
                                    case 8:
                                        MessageBox.Show("Deleting old profile failed. Keeping both versions. Delete old profile manually.");
                                        ZipFile.ExtractToDirectory(InitialPath + ".zip", InitialPath);
                                        File.Delete(InitialPath + ".zip");
                                        break;
                                    delete:
                                        Directory.Delete(NewFolderPath);
                                        break;
                                    default:
                                        MessageBox.Show("Unexpected error. Deleting new Profile.");
                                        Directory.Delete(NewFolderPath);
                                        break;
                                }
                            }
                        }

                        break;
                    }
                case "clone":
                    {
                        //Get existing folders
                        string[] existingDirs = Directory.GetDirectories(InitialPath.Remove(InitialPath.LastIndexOf('\\') + 1));

                        List<string> existingDirList = new List<string>();

                        foreach (string dir in existingDirs)
                        {
                            //Get name 
                            existingDirList.Add(Utilities.TextUtilities.FromHexToString(Path.GetFileName(dir)));
                        }

                        foreach (string newfile in textBoxNewName.Lines)
                        {
                            string NewProfileName = "", NewFolderPath = "";
                            byte progress = 0;

                            try
                            {
                                //Check empty lines
                                if (newfile.Length == 0 || newfile.Trim(new char[] { ' ' }).Length == 0)
                                    continue;

                                NewProfileName = newfile.Trim(new char[] { ' ' });
                                //Check existing folders
                                if (NewProfileName == InitialName || existingDirList.Contains(NewProfileName))
                                    continue;

                                //New folder
                                string NewProfileNameHex = Utilities.TextUtilities.FromStringToHex(NewProfileName);
                                NewFolderPath = InitialPath.Remove(InitialPath.LastIndexOf('\\') + 1) + NewProfileNameHex;

                                progress = 1;

                                //Validate If exist skip
                                if (Directory.Exists(NewFolderPath))
                                    continue;

                                Directory.CreateDirectory(NewFolderPath);

                                progress = 2;

                                //Copy profile files .cfg .sii .png
                                //Get the files in the initial directory and copy them to the new location.
                                var files = Directory.EnumerateFiles(InitialPath, "*.*", SearchOption.TopDirectoryOnly).Where(s => s.EndsWith(".cfg") || s.EndsWith(".sii") || s.EndsWith(".png")).ToArray();

                                //Iterate files
                                foreach (string file in files)
                                {
                                    string temppath = Path.Combine(NewFolderPath, Path.GetFileName(file)); //new file path with name

                                    FileInfo tFI = new FileInfo(file); //fileinfo
                                    tFI.CopyTo(temppath, false); //Copy
                                }

                                progress = 3;

                                //Create save folder
                                string NewSaveFolder = NewFolderPath + "\\save";
                                Directory.CreateDirectory(NewSaveFolder);

                                progress = 4;

                                //Copy saves
                                if (checkBoxFullCloning.Checked)
                                {
                                    Utilities.IO_Utilities.DirectoryCopy(InitialPath + "\\save", NewSaveFolder, true);
                                }
                                else
                                {
                                    Utilities.IO_Utilities.DirectoryCopy(InitialPath + "\\save\\autosave", NewSaveFolder + "\\autosave", false);
                                }

                                progress = 5;

                                //Decode profile.sii
                                string[] profileFile = ParentForm.NewDecodeFile(NewFolderPath + "\\profile.sii");
                                progress = 6;

                                SaveFileProfileData ProfileData = new SaveFileProfileData();
                                ProfileData.Prepare(profileFile);
                                progress = 7;

                                //New name
                                ProfileData.ProfileName = NewProfileName;
                                ProfileData.CreationTime = Utilities.DateTimeUtilities.DateTimeToUnixTimeStamp();
                                progress = 8;

                                //Write file
                                using (StreamWriter SW = new StreamWriter(NewFolderPath + "\\profile.sii", false))
                                {
                                    ProfileData.WriteToStream(SW);
                                }
                                progress = 9;

                                //Cloned folders
                                existingDirList.Add(NewProfileName);
                                ReturnClonedNames.Add(NewProfileName);
                            }
                            catch {

                                switch (progress)
                                {
                                    case 0:
                                        MessageBox.Show("Create new folder name failed");
                                        break;
                                    case 1:
                                        MessageBox.Show("Directory was not created");
                                        break;
                                    case 2:
                                        MessageBox.Show("Directory copy failed");
                                        goto delete;
                                    case 3:
                                        MessageBox.Show("Directory for saves was not created");
                                        goto delete;
                                    case 4:
                                        MessageBox.Show("Directory with saves copy failed");
                                        goto delete;
                                    case 5:
                                        MessageBox.Show("Profile not decoded");
                                        goto delete;
                                    case 6:
                                        MessageBox.Show("Profile hase wrong version/format");
                                        goto delete;
                                    case 7:
                                        MessageBox.Show("Profile properties was not applied");
                                        goto delete;
                                    case 8:
                                        MessageBox.Show("Profile write failed");
                                        goto delete;
                                    delete:
                                        Directory.Delete(NewFolderPath);
                                        break;
                                    default:
                                        MessageBox.Show("Unexpected error. Deleting new Profile.");
                                        Directory.Delete(NewFolderPath);
                                        break;
                                }
                            }
                        }

                        if(ReturnClonedNames.Count > 0)
                            ReturnCloningSuccessful = true;

                        break;
                    }
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
