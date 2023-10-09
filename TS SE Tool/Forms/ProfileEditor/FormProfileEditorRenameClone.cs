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
using System.IO;
using System.IO.Compression;
using System.Threading;

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
        FormMain MainForm = Application.OpenForms.OfType<FormMain>().Single();

        public string InitialName = "";
        public string InitialPath = "";
        private byte NameLengthLimit = 30;

        private bool aboveLimitLength = false;
        private int FormWidthMin = 0;
        private int textBoxNewNameWidthMin = 160;
        private int tbNewNameWidth = 0;
        private int tbNewNameWidthMulty = 0;

        private List<string> existingProfiles = new List<string>();

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
            PrepareForm();
            TranslateForm();

            CorrectControlsPositions();

            if (!indicateCharLimit())
                buttonAccept.Enabled = false;
        }

        //
        private void TranslateForm()
        {
            MainForm.HelpTranslateFormMethod(this);

            string controlsNewNames = "";

            switch (FormMode)
            {
                case "rename":
                    {
                        controlsNewNames = "Renaming";

                        break;
                    }
                case "clone":
                    {
                        controlsNewNames = "Cloning";

                        break;
                    }
            }

            MainForm.HelpTranslateControlDiffName(this, this.Name + controlsNewNames);
            MainForm.HelpTranslateControlDiffName(labelNewName, labelNewName.Name + controlsNewNames);
        }

        //
        private void PrepareForm()
        {
            switch (FormMode)
            {
                case "rename":
                    {
                        textBoxNewName.MaxLength = NameLengthLimit;

                        checkBoxMutiCloning.Visible = false;
                        checkBoxFullCloning.Visible = false;

                        break;
                    }
                case "clone":
                    {
                        checkBoxCreateBackup.Visible = false;

                        checkBoxFullCloning.Location = checkBoxMutiCloning.Location;
                        checkBoxMutiCloning.Location = checkBoxCreateBackup.Location;

                        break;
                    }
            }
        }

        //
        private void CorrectControlsPositions()
        {
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

        //Name textbox events
        private void textBoxNewName_KeyDown(object sender, KeyEventArgs e)
        {
            if (textBoxNewName.Text.Length == 0) return;

            if (e.KeyCode == Keys.Return || e.KeyCode == Keys.Back || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Home || e.KeyCode == Keys.End || e.KeyCode == Keys.PageDown || e.KeyCode == Keys.PageUp || e.KeyCode == Keys.Delete)
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

        private void textBoxNewName_KeyUp(object sender, KeyEventArgs e)
        {
            indicateCharLimit();
        }

        private void textBoxNewName_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Filter
            char[] forbidenChars = { '\\', '|' };

            char tmpChar = e.KeyChar;
            if (forbidenChars.Contains(tmpChar))
            {
                e.Handled = true;
            }

            //charLimit();

            // Check for the flag being set in the KeyDown event
            if (aboveLimitLength == true)
            {
                // Stop the character from being entered into the control
                e.Handled = true;
            }
        }

        private void textBoxNewName_TextChanged(object sender, EventArgs e)
        {
            bool apply = true;

            //Trim on paste
            if (textBoxNewName.Multiline)
            {
                int linecount = textBoxNewName.Lines.Length;

                var tmpLines = textBoxNewName.Lines;

                for (int i = 0; i < linecount; i++)
                {
                    if (textBoxNewName.Lines[i].Length > NameLengthLimit)
                    {
                        tmpLines[i] = textBoxNewName.Lines[i].Substring(0, NameLengthLimit);
                    }

                    textBoxNewName.Lines = tmpLines;
                }

                string[] commonElements = textBoxNewName.Lines.Intersect(existingProfiles).ToArray();

                if (commonElements.Count() > 1)
                {
                    errorProvider.SetError(textBoxNewName, "Profiles already exist: " + Environment.NewLine +
                                                            string.Join(Environment.NewLine, commonElements));
                    apply = false;
                }
                else if (commonElements.Count() > 0)
                {
                    errorProvider.SetError(textBoxNewName, "Profile [ " + commonElements[0] + " ] already exist");
                    apply = false;
                }
                else
                    errorProvider.SetError(textBoxNewName, "");
            }
            else
            {
                if (textBoxNewName.Text.Length > NameLengthLimit)
                    textBoxNewName.Text = textBoxNewName.Text.Substring(0, NameLengthLimit);

                if (existingProfiles.Contains(textBoxNewName.Text))
                {
                    errorProvider.SetError(textBoxNewName, "Profile [ " + textBoxNewName.Text + " ] already exist");
                    apply = false;
                }
                else
                {
                    errorProvider.SetError(textBoxNewName, "");
                }
            }

            //
            calculateTextBoxNewNameSize();
            if (!indicateCharLimit())
                apply = false;

            buttonAccept.Enabled = apply;

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

            calculateTextBoxNewNameSize();
        }

        //Buttons
        private void buttonAccept_Click(object sender, EventArgs e)
        {
            switch (FormMode)
            {
                case "rename":
                    {
                        string NewProfileName = textBoxNewName.Text.Trim(new char[] { ' ' }), NewFolderName = "", NewFolderPath = "";
                        byte progress = 0;


                        if (NewProfileName != InitialName && !existingProfiles.Contains(NewProfileName))
                        {
                            ReturnNewName = NewProfileName;

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
                                ProfileData.ProcessData(profileFile);
                                progress = 5;

                                //New name
                                ProfileData.ProfileName = new Save.DataFormat.SCS_String(NewProfileName);
                                progress = 6;

                                //Write file
                                using (StreamWriter SW = new StreamWriter(NewFolderPath + "\\profile.sii", false))
                                {
                                    ProfileData.WriteToStream(SW);
                                }
                                progress = 7;

                                //Iterate through save folders to edit preview.tobj
                                DirectoryInfo[] dirInfoArray = new DirectoryInfo(NewFolderPath + "\\save").GetDirectories();

                                foreach (DirectoryInfo subdir in dirInfoArray)
                                {
                                    FileInfo[] saveFolderFile = subdir.GetFiles();
                                    foreach (FileInfo fI in saveFolderFile)
                                    {
                                        if (fI.Name == "preview.tobj")
                                        {
                                            //tobj
                                            string pathToTGA = "/home/profiles/" + NewFolderName + "/save/" + subdir.Name + "/preview.tga";

                                            Utilities.IO_Utilities.WritePreviewTOBJ(fI.FullName, pathToTGA);

                                            break;
                                        }
                                    }
                                }
                                progress = 8;

                                //Make backup
                                if (checkBoxCreateBackup.Checked)
                                {
                                    if (File.Exists(InitialPath + ".zip"))
                                        File.Delete(InitialPath + ".zip");

                                    ZipFile.CreateFromDirectory(InitialPath, InitialPath + ".zip");
                                }
                                progress = 9;

                                //Delete old folder
                                Directory.Delete(InitialPath, true);
                                progress = 10;

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
                                        MessageBox.Show("Profile has wrong version/format");
                                        goto delete;
                                    case 5:
                                        MessageBox.Show("Profile name is not applied");
                                        goto delete;
                                    case 6:
                                        MessageBox.Show("Profile write failed");
                                        goto delete;
                                    case 7:
                                        MessageBox.Show("Error during save preview fixing");
                                        break;
                                    case 8:
                                        MessageBox.Show("Profile backup creation failed. Keeping both versions. Delete old profile manually.");
                                        break;
                                    case 9:
                                        MessageBox.Show("Deleting old profile failed. Keeping both versions. Delete old profile manually.");
                                        ZipFile.ExtractToDirectory(InitialPath + ".zip", InitialPath);
                                        File.Delete(InitialPath + ".zip");
                                        break;
                                        delete:
                                        MessageBox.Show("Deleting new Profile.");
                                        Directory.Delete(NewFolderPath);
                                        break;
                                    default:
                                        MessageBox.Show("Unexpected error. Deleting new Profile.");
                                        Directory.Delete(NewFolderPath);
                                        break;
                                }
                            }
                        }
                        else
                        {
                            DialogResult = DialogResult.Abort;
                            Close();
                        }

                        break;
                    }
                case "clone":
                    {
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
                                if (NewProfileName == InitialName || existingProfiles.Contains(NewProfileName))
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
                                List<string> fileList = new List<string>();
                                string[] tmpFilelist;

                                //Avatar
                                fileList.AddRange(Directory.EnumerateFiles(InitialPath, "avatar.png", SearchOption.TopDirectoryOnly).ToList());

                                //Profile
                                fileList.AddRange(Directory.EnumerateFiles(InitialPath, "profile.sii", SearchOption.TopDirectoryOnly).ToList());

                                //Controls
                                fileList.AddRange(Directory.EnumerateFiles(InitialPath, "controls.sii", SearchOption.TopDirectoryOnly).ToList());

                                //CFG
                                tmpFilelist = Directory.EnumerateFiles(InitialPath, "*.cfg", SearchOption.TopDirectoryOnly).ToArray();
                                foreach (string file in tmpFilelist)
                                {
                                    if (Path.GetFileName(file).Equals("config.cfg") || Path.GetFileName(file).Equals("config_local.cfg"))
                                        fileList.Add(file);
                                }

                                //SII gearbox layout
                                tmpFilelist = Directory.EnumerateFiles(InitialPath, "*.sii", SearchOption.TopDirectoryOnly).ToArray();

                                foreach (string file in tmpFilelist)
                                {
                                    if (Path.GetFileName(file).StartsWith("gearbox_layout_"))
                                        fileList.Add(file);
                                }

                                //Iterate files
                                foreach (string file in fileList)
                                {
                                    string temppath = Path.Combine(NewFolderPath, Path.GetFileName(file)); //new file path with name

                                    FileInfo tFI = new FileInfo(file);  //fileinfo
                                    tFI.CopyTo(temppath, false);        //Copy
                                }
                                progress = 3;

                                //Decode profile.sii
                                string[] profileFile = ParentForm.NewDecodeFile(NewFolderPath + "\\profile.sii");
                                progress = 4;

                                SaveFileProfileData ProfileData = new SaveFileProfileData();
                                ProfileData.ProcessData(profileFile);
                                progress = 5;

                                //New name
                                ProfileData.ProfileName = new Save.DataFormat.SCS_String(NewProfileName);
                                ProfileData.CreationTime = Utilities.DateTimeUtilities.DateTimeToUnixTimeStamp();
                                progress = 6;

                                //Write file
                                using (StreamWriter SW = new StreamWriter(NewFolderPath + "\\profile.sii", false))
                                {
                                    ProfileData.WriteToStream(SW);
                                }
                                progress = 7;

                                //Create save folder
                                string NewSaveFolder = NewFolderPath + "\\save";
                                Directory.CreateDirectory(NewSaveFolder);

                                progress = 8;

                                //Copy saves
                                string[] validFileNames = new string[] { "game.sii", "info.sii", "preview.tga", "preview.mat", "preview.tobj" };

                                if (checkBoxFullCloning.Checked)
                                {
                                    Utilities.IO_Utilities.DirectoryCopy(InitialPath + "\\save", NewSaveFolder, true, validFileNames);

                                    //Iterate through save folders to edit preview.tobj
                                    DirectoryInfo[] dirInfoArray = new DirectoryInfo(NewFolderPath + "\\save").GetDirectories();

                                    foreach (DirectoryInfo subdir in dirInfoArray)
                                    {
                                        FileInfo[] saveFolderFile = subdir.GetFiles();
                                        foreach (FileInfo fI in saveFolderFile)
                                        {
                                            if (fI.Name == "preview.tobj")
                                            {
                                                //tobj
                                                string pathToTGA = "/home/profiles/" + NewProfileNameHex + "/save/" + subdir.Name + "/preview.tga";

                                                Utilities.IO_Utilities.WritePreviewTOBJ(fI.FullName, pathToTGA);

                                                break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    Utilities.IO_Utilities.DirectoryCopy(InitialPath + "\\save\\autosave", NewSaveFolder + "\\autosave", false, validFileNames);
                                }

                                progress = 9;

                                //Clone album
                                if (Directory.Exists(InitialPath + "\\album"))
                                    Utilities.IO_Utilities.DirectoryCopy(InitialPath + "\\album", NewFolderPath + "\\album", false);

                                progress = 10;
                                
                                //Cloned folders
                                existingProfiles.Add(NewProfileName);
                                ReturnClonedNames.Add(NewProfileName);
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
                                        MessageBox.Show("Profile has wrong version/format");
                                        goto delete;
                                    case 5:
                                        MessageBox.Show("Profile properties was not applied");
                                        goto delete;
                                    case 6:
                                        MessageBox.Show("Profile write failed");
                                        goto delete;
                                    case 7:
                                        MessageBox.Show("Directory for saves was not created");
                                        goto delete;
                                    case 8:
                                        MessageBox.Show("Directory with saves copy failed");
                                        goto delete;
                                    case 9:
                                        MessageBox.Show("Album Directory copy failed");                                        
                                        break;
                                    default:
                                        MessageBox.Show("Unexpected error. Deleting new Profile.");
                                        delete:
                                        Directory.Delete(NewFolderPath);
                                        break;
                                }
                            }
                        }

                        if (ReturnClonedNames.Count > 0)
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

        //Extra
        private bool indicateCharLimit()
        {
            int txtLength = 0;
            bool apply = true;

            if (textBoxNewName.Multiline)
            {
                if (textBoxNewName.Lines.Length > 0)
                {
                    int currentLine = textBoxNewName.GetLineFromCharIndex(textBoxNewName.SelectionStart);
                    txtLength = textBoxNewName.Lines[currentLine].Length;
                }
                else
                    apply = false;
            }
            else
            {
                txtLength = textBoxNewName.Text.Length;

                if (txtLength == 0)
                    apply = false;
            }

            //===

            if (txtLength == 0)
            {
                labelCharCountLimit.ForeColor = Color.Red;
                labelCharCountLimit.Font = new Font(labelCharCountLimit.Font, FontStyle.Bold);
            }
            else if (txtLength >= NameLengthLimit)
            {
                labelCharCountLimit.ForeColor = Color.Red;
                labelCharCountLimit.Font = new Font(labelCharCountLimit.Font, FontStyle.Bold);
            }
            else
            {
                labelCharCountLimit.ForeColor = Color.DarkGreen;
                labelCharCountLimit.Font = new Font(labelCharCountLimit.Font, FontStyle.Regular);
            }

            labelCharCountLimit.Text = "[" + txtLength.ToString() + " | " + NameLengthLimit.ToString() + "]";

            return apply;
        }

        private void calculateTextBoxNewNameSize()
        {
            int extraM = 25;

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
            }
            else
            {
                Graphics gr = CreateGraphics();
                SizeF tSize = gr.MeasureString(textBoxNewName.Text, textBoxNewName.Font);
                tbNewNameWidth = (int)Math.Ceiling(Convert.ToDouble(tSize.Width));

                if (tbNewNameWidth + extraM >= textBoxNewNameWidthMin)
                    this.Width = FormWidthMin + tbNewNameWidth - textBoxNewNameWidthMin + extraM;
                else
                    this.Width = FormWidthMin;
            }

        }

        private void FormProfileEditorRenameClone_Load(object sender, EventArgs e)
        {
            existingProfiles = Directory.GetDirectories(InitialPath.Remove(InitialPath.LastIndexOf('\\') + 1)).Select(d => new DirectoryInfo(d).Name).ToList();

            existingProfiles = existingProfiles.Where(x => !x.Contains(' ')).ToList();

            List<string> tmp = new List<string>();

            foreach (string name in existingProfiles)
            {
                tmp.Add(Utilities.TextUtilities.FromHexToString(name));
            }

            existingProfiles = tmp.ToList();
        }
    }
}
