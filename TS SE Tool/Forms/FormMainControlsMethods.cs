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
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Reflection;
using ICSharpCode.SharpZipLib.GZip;
using Microsoft.Win32;
using TS_SE_Tool.Utilities;

namespace TS_SE_Tool
{
    public partial class FormMain
    {
        //Menu controls
        private void programSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormProgramSettings FormWindow = new FormProgramSettings();
            FormWindow.ShowDialog();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSettings FormWindow = new FormSettings();
            FormWindow.ShowDialog();

            ApplySettings();
            ApplySettingsUI();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Language
        //tool strip click
        private void toolstripChangeLanguage(object sender, EventArgs e)
        {
            ToolStripItem obj = sender as ToolStripItem;
            string _objname = obj.Name;

            string[] cult = _objname.ToString().Split('_');
            string ButtonCultureInfo = cult[0] + "-" + cult[1];

            ProgSettingsV.Language = ButtonCultureInfo;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(ButtonCultureInfo);

            ChangeLanguage();
        }

        private void ChangeLanguage()
        {
            try
            {
                if (ProgSettingsV.Language != "Default")
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(ProgSettingsV.Language);//CultureInfo.GetCultureInfo("en-US");
            }
            catch
            {
                IO_Utilities.LogWriter("Wrong language setting format");
            }

            CultureInfo ci = Thread.CurrentThread.CurrentUICulture;

            try
            {
                this.SuspendLayout();

                HelpTranslateFormMethod(this, toolTipMain);
                HelpTranslateMenuStripMethod(menuStripMain);

                this.ResumeLayout();

                LngFileLoader("countries_translate.txt", CountriesLngDict, ProgSettingsV.Language);
                LngFileLoader("cities_translate.txt", CitiesLngDict, ProgSettingsV.Language);
                LngFileLoader("companies_translate.txt", CompaniesLngDict, ProgSettingsV.Language);
                LngFileLoader("cargo_translate.txt", CargoLngDict, ProgSettingsV.Language);
                LngFileLoader("urgency_translate.txt", UrgencyLngDict, ProgSettingsV.Language);
                //LngFileLoader("custom_strings.txt", CustomStringsDict, ProgSettingsV.Language);

                LoadTruckBrandsLng();
                LoadDriverNamesLng();

                AddTranslationToData();
                RefreshComboboxes();
                CorrectControlsPositions();
            }
            catch
            { }

            //rm.ReleaseAllResources();
        }

        //About
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAboutBox aboutWindow = new FormAboutBox();
            aboutWindow.ShowDialog();
        }

        //How to
        private void localPDFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string pdf_path = Directory.GetCurrentDirectory() + @"\HowTo.pdf";

            if (File.Exists(pdf_path))
                Process.Start(pdf_path);
            else
                MessageBox.Show("Missing manual. Try to repair via update", "HowTo.pdf not found");
        }

        private void youTubeVideoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(Utilities.Web_Utilities.External.linkYoutubeTutorial);
        }

        //Downloads
        private void checkGitHubRelesesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(Utilities.Web_Utilities.External.linGithubReleases);
        }

        private void checkTMPForumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(Utilities.Web_Utilities.External.linTMPforum);
        }

        private void checkSCSForumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(Utilities.Web_Utilities.External.linkSCSforum);
        }

        private void latestStableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCheckUpdates FormWindow = new FormCheckUpdates("check");
            FormWindow.ShowDialog();
        }
        //Menu controls End

        //Form methods
        private void ToggleControlsAccess(bool _state)
        {
            buttonMainWriteSave.Enabled = _state;
            buttonMainWriteSave.Visible = _state;

            foreach (TabPage tp in tabControlMain.TabPages)
            {
                tp.Enabled = _state;
            }

            //Profile
            int pSkillsNameHeight = 64, pSkillsNameWidth = 64;
            for (int i = 0; i < 6; i++)
            {
                Control[] tmp = this.Controls.Find("profileSkillsPanel" + i.ToString(), true);
                if(tmp[0] != null)
                {
                    Bitmap bgimg = new Bitmap(SkillImgS[i], pSkillsNameHeight, pSkillsNameWidth);
                    tmp[0].BackgroundImage = bgimg;
                    if (!_state)
                        tmp[0].BackgroundImage = ConvertBitmapToGrayscale(tmp[0].BackgroundImage);
                }
            }
        }

        private void ToggleMainControlsAccess(bool _state)
        {
            radioButtonMainGameSwitchETS.Enabled = _state;
            radioButtonMainGameSwitchATS.Enabled = _state;

            checkBoxProfilesAndSavesProfileBackups.Enabled = _state;
            buttonProfilesAndSavesRefreshAll.Enabled = _state;
            buttonProfilesAndSavesEditProfile.Enabled = _state;

            comboBoxPrevProfiles.Enabled = _state;
            comboBoxProfiles.Enabled = _state;
            comboBoxSaves.Enabled = _state;

            buttonMainDecryptSave.Enabled = _state;
            buttonMainLoadSave.Enabled = _state;

            buttonMainWriteSave.Enabled = _state;
        }

        //Main part controls
        
        //Game select
        public void ToggleGame_Click(object sender, EventArgs e)
        {
            if (radioButtonMainGameSwitchETS.Checked)
                ToggleGame("ETS2");
            else
                ToggleGame("ATS");

            FillAllProfilesPaths();
        }

        public void ToggleGame(string _game)
        {
            if (tempSavefileInMemory != null)
            {
                DialogResult result = MessageBox.Show("Savefile not saved.\nDo you want to discard changes and switch game type?", "Switching game", 
                    MessageBoxButtons.YesNo);

                if (result == DialogResult.No)
                    return;
                else
                {
                    buttonMainDecryptSave.Enabled = true;

                    ToggleControlsAccess(false);
                    SetDefaultValues(false);
                    ClearFormControls(true);
                }
            }

            if (_game == "ETS2")
                GameType = _game;
            else
                GameType = _game;
        }

        private void buttonMainAddCustomFolder_Click(object sender, EventArgs e)
        {
            FormAddCustomFolder FormWindow = new FormAddCustomFolder();
            FormWindow.ShowDialog();
        }
        
        //Profile list
        private void buttonRefreshAll_Click(object sender, EventArgs e)
        {
            FillAllProfilesPaths();
        }

        private void buttonProfilesAndSavesEditProfile_Click(object sender, EventArgs e)
        {
            FormProfileEditor FormWindow = new FormProfileEditor();
            FormWindow.ParentForm = this;
            DialogResult t = FormWindow.ShowDialog();

            if (t != DialogResult.Cancel)
            {
                //Refresh
                buttonMainDecryptSave.Enabled = true;
                buttonMainLoadSave.Enabled = true;

                FillAllProfilesPaths();
            }
        }
        
        //Buttons
        private void buttonDecryptSave_Click(object sender, EventArgs e)
        {
            SetDefaultValues(false);
            ClearFormControls(true);

            ToggleMainControlsAccess(false);

            SavefilePath = Globals.SavesHex[comboBoxSaves.SelectedIndex];
            string SiiSavePath = SavefilePath + @"\game.sii";

            string[] file = NewDecodeFile(SiiSavePath);

            if (file != null)
            {
                IO_Utilities.LogWriter("Backing up file to: " + SavefilePath + @"\game_backup.sii");

                File.Copy(SiiSavePath, SavefilePath + @"\game_backup.sii", true);

                File.WriteAllLines(SiiSavePath, file);

                UpdateStatusBarMessage.ShowStatusMessage(SMStatus.Clear);
            }
            else
                UpdateStatusBarMessage.ShowStatusMessage(SMStatus.Error, "error_could_not_decode_file");

            ToggleMainControlsAccess(true);
            buttonMainDecryptSave.Enabled = false;

            ToggleGame(GameType);

            //GC
            GC.Collect();
            //GC.WaitForPendingFinalizers();
        }

        private void buttonOpenSaveFolder_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(Globals.SavesHex[comboBoxSaves.SelectedIndex]))
                Process.Start(Globals.SavesHex[comboBoxSaves.SelectedIndex]);
            //else
        }

        private void LoadSaveFile_Click(object sender, EventArgs e)
        {
            ToggleMainControlsAccess(false);

            ToggleControlsAccess(false);

            //Load save file
            LoadSaveFile();

            //GC
            GC.Collect();
            //GC.WaitForPendingFinalizers();
        }

        private void buttonWriteSave_Click(object sender, EventArgs e)
        {
            if (extraDrivers.Count() > 0 || extraVehicles.Count() > 0)
            {
                DialogResult res = MessageBox.Show("Do you want to save Drivers and Trucks from sold garages?", "Attention! Loosing content", MessageBoxButtons.YesNo);
                if (res == DialogResult.Yes)
                {
                    FormGaragesSoldContent testDialog = new FormGaragesSoldContent();
                    testDialog.ShowDialog(this);
                }
            }

            ToggleControlsAccess(false);

            string SiiSavePath = SavefilePath + @"\game.sii";

            IO_Utilities.LogWriter("Backing up file to: " + SavefilePath + @"\game_backup.sii");
            //File.Copy(SiiSavePath, SiiSavePath + "_backup", true);
            File.Copy(SiiSavePath, SavefilePath + @"\game_backup.sii", true);

            //Write
            NewWrireSaveFile();

            buttonMainDecryptSave.Enabled = true;
            MessageBox.Show("File saved", "Saving", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //Profile and Saves groupbox
        private void checkBoxProfileBackups_CheckedChanged(object sender, EventArgs e)
        {
            string sv = comboBoxPrevProfiles.SelectedValue.ToString();

            FillAllProfilesPaths();

            //if (checkBoxProfilesAndSavesProfileBackups.Checked)
            //{
            int index = FindByValue(comboBoxPrevProfiles, sv);

            if (index > -1)
                comboBoxPrevProfiles.SelectedValue = sv;
            else
                comboBoxPrevProfiles.SelectedIndex = 0;
            //}
        }

        public void FillAllProfilesPaths()
        {
            try
            {
                string MyDocumentsPath = "";
                string RemoteUserdataDirectory = "";

                string SteamError = "", MyDocError = "";
                bool SteamFolderEx = false, MyDocFolderEx = true;

                try
                {
                    //string SteamInstallPath = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Valve\Steam", "InstallPath", null).ToString();
                    string SteamInstallPath = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Valve\Steam", "InstallPath", null).ToString();

                    if (SteamInstallPath == null)
                    {
                        //unknown steam path
                        SteamError = "Can not detect Steam install folder.";
                    }
                    else
                    {
                        string SteamCloudPath = SteamInstallPath + @"\userdata";
                        if (!Directory.Exists(SteamCloudPath))
                        {
                            //no userdata
                            SteamError = "No userdata in Steam folder.";
                        }
                        else
                        {
                            string[] userdatadirectories = Directory.GetDirectories(SteamCloudPath);

                            if (userdatadirectories.Length == 0)
                            {
                                //no steam user directories
                                SteamError = "No user folders found in Steam folder.";
                            }
                            else
                            {
                                //DateTime lastHigh = DateTime.Now;

                                string[] CurrentUserDirs = Directory.GetDirectories(SteamCloudPath).OrderByDescending(f => new FileInfo(f).LastWriteTime).ToArray();
                                string CurrentUserDir = "";

                                foreach (string tmpDir in CurrentUserDirs)
                                {
                                    string tmp = Path.GetFileName(tmpDir);

                                    if (tmp.All(Char.IsDigit))
                                    {
                                        CurrentUserDir = tmpDir;
                                        break;
                                    }
                                }

                                string GameID = "";
                                if (GameType == "ETS2")
                                    GameID = @"\227300"; //ETS2
                                else
                                    GameID = @"\270880"; //ATS

                                if (!Directory.Exists(CurrentUserDir + GameID))
                                {
                                    SteamError = "Game folder for this game - " + GameType + " in Steam folder does not exist.";
                                }
                                else
                                {
                                    RemoteUserdataDirectory = CurrentUserDir + GameID + @"\remote";
                                    SteamFolderEx = true;
                                }
                            }
                        }
                    }
                }
                catch
                { }

                if (!SteamFolderEx)
                    IO_Utilities.LogWriter(SteamError);
                //

                MyDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + dictionaryProfiles[GameType];

                if (!Directory.Exists(MyDocumentsPath))
                {
                    MyDocError = "Folder in \"My documents\" for this game - " + GameType + " does not exist.";
                    MyDocFolderEx = false;
                    IO_Utilities.LogWriter(MyDocError);
                }
                //

                DataTable combDT = new DataTable();
                DataColumn dc = new DataColumn("ProfileID", typeof(string));
                combDT.Columns.Add(dc);

                dc = new DataColumn("ProfileName", typeof(string));
                combDT.Columns.Add(dc);

                dc = new DataColumn("ProfileType", typeof(string));
                combDT.Columns.Add(dc);

                List<string> tempList = new List<string>();

                if (MyDocFolderEx || SteamFolderEx)
                    if (checkBoxProfilesAndSavesProfileBackups.Checked)
                    {
                        if (MyDocFolderEx)
                            foreach (string folder in Directory.GetDirectories(MyDocumentsPath))
                            {
                                if (Path.GetFileName(folder).StartsWith("profiles")) //Documents
                                {
                                    if (Directory.Exists(folder) && Directory.GetDirectories(folder).Count() > 0)
                                    {
                                        combDT.Rows.Add(folder, "[L] " + Path.GetFileName(folder), "local");
                                        tempList.Add(folder);
                                    }
                                }
                            }

                        //string RemoteUserdataDirectory Steam Profiles
                        if (SteamFolderEx)
                            foreach (string folder in Directory.GetDirectories(RemoteUserdataDirectory))
                            {
                                if (Path.GetFileName(folder).StartsWith("profiles")) //Steam
                                {
                                    if (Directory.Exists(folder) && Directory.GetDirectories(folder).Count() > 0)
                                    {
                                        combDT.Rows.Add(folder, "[S] " + Path.GetFileName(folder), "steam");
                                        tempList.Add(folder);
                                    }
                                }
                            }
                    }
                    else
                    {
                        string folder = "";
                        if (MyDocFolderEx)
                        {
                            folder = MyDocumentsPath + @"\profiles";

                            if (Directory.Exists(folder) && Directory.GetDirectories(folder).Count() > 0)
                            {
                                combDT.Rows.Add(folder, "[L] profiles", "local");
                                tempList.Add(folder);
                            }
                        }
                        if (SteamFolderEx)
                        {
                            folder = RemoteUserdataDirectory + @"\profiles";

                            if (Directory.Exists(folder) && Directory.GetDirectories(folder).Count() > 0)
                            {
                                combDT.Rows.Add(folder, "[S] profiles", "steam");
                                tempList.Add(folder);
                            }
                        }
                    }

                int cpIndex = 0;
                if (ProgSettingsV.CustomPaths.Keys.Contains(GameType))
                    foreach (string CustPath in ProgSettingsV.CustomPaths[GameType])
                    {
                        cpIndex++;
                        if (Directory.Exists(CustPath))
                        {
                            if (Directory.Exists(CustPath + @"\profiles"))
                            {
                                combDT.Rows.Add(CustPath + @"\profiles", "[C] Custom path " + cpIndex.ToString(), "custom");
                                tempList.Add(CustPath + @"\profiles");
                            }
                            else
                            {
                                combDT.Rows.Add(CustPath, "[C] Custom path " + cpIndex.ToString(), "custom");
                                tempList.Add(CustPath);
                            }
                        }
                    }

                if (!MyDocFolderEx && !SteamFolderEx)
                {
                    IO_Utilities.LogWriter("Standart Save folders does not exist for this game - " + GameType + ". " + MyDocError + " " + SteamError +
                        " Check installation. Start game first (Steam).");
                }

                Globals.ProfilesPaths = tempList.ToArray();

                comboBoxPrevProfiles.ValueMember = "ProfileID";
                comboBoxPrevProfiles.DisplayMember = "ProfileName";
                comboBoxPrevProfiles.DataSource = combDT;

                if (comboBoxPrevProfiles.Items.Count > 0)
                {
                    comboBoxPrevProfiles.Enabled = true;
                }
                else
                {
                    comboBoxPrevProfiles.SelectedIndex = -1;
                    comboBoxPrevProfiles.Enabled = false;

                    comboBoxProfiles.Enabled = false;
                    comboBoxSaves.Enabled = false;

                    MessageBox.Show("Standart Save folders does not exist for this game - " + GameType + ".\r\n" + MyDocError + "\r\n" + SteamError +
                        "\r\nCheck installation, start game and update list or Add Custom paths.");
                }
            }
            catch
            {
                IO_Utilities.ErrorLogWriter("Populating Root Profiles failed");
            }
        }

        private void comboBoxPrevProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!Directory.Exists(Globals.ProfilesPaths[comboBoxPrevProfiles.SelectedIndex]))            
                return;            

            buttonProfilesAndSavesEditProfile.Enabled = false;
            buttonMainDecryptSave.Enabled = false;
            buttonProfilesAndSavesOpenSaveFolder.Enabled = false;
            buttonMainLoadSave.Enabled = false;

            FillProfiles();

            string sv = comboBoxPrevProfiles.SelectedValue.ToString();

            int index = FindByValue(comboBoxPrevProfiles, sv);

            if (index > -1)
                comboBoxPrevProfiles.SelectedValue = sv;
            else
                comboBoxPrevProfiles.SelectedIndex = 0;
        }

        private void comboBoxPrevProfiles_DropDown(object sender, EventArgs e)
        {
            comboBoxPrevProfiles.SelectedIndexChanged -= comboBoxPrevProfiles_SelectedIndexChanged;

            string sv = comboBoxPrevProfiles.SelectedValue.ToString();

            FillAllProfilesPaths();

            int index = FindByValue(comboBoxPrevProfiles, sv);

            if (index > -1)
                comboBoxPrevProfiles.SelectedValue = sv;
            else
                comboBoxPrevProfiles.SelectedIndex = 0;

            comboBoxPrevProfiles.SelectedIndexChanged += comboBoxPrevProfiles_SelectedIndexChanged;
        }

        public void FillProfiles()
        {
            try
            {
                if (!Directory.Exists(Globals.ProfilesPaths[comboBoxPrevProfiles.SelectedIndex]))
                {
                    FillAllProfilesPaths();
                    return;
                }

                comboBoxProfiles.SelectedIndexChanged -= new EventHandler(comboBoxProfiles_SelectedIndexChanged);

                string ProfileName = "";
                string SelectedFolder = "";
                SelectedFolder = comboBoxPrevProfiles.SelectedValue.ToString();

                List<string> includedFiles = new List<string>();
                includedFiles = Directory.GetFiles(SelectedFolder).Select(Path.GetFileName).ToList();

                if (includedFiles.Contains("profile.sii") || includedFiles.Contains("game.sii"))
                {
                    Globals.ProfilesHex.Clear();
                    Globals.ProfilesHex.Add(SelectedFolder);
                }
                else
                    Globals.ProfilesHex = Directory.GetDirectories(SelectedFolder).OrderByDescending(f => new FileInfo(f).LastWriteTime).ToList();


                DataTable combDT = new DataTable();
                DataColumn dc = new DataColumn("ProfilePath", typeof(string));
                combDT.Columns.Add(dc);

                dc = new DataColumn("ProfileName", typeof(string));
                combDT.Columns.Add(dc);

                DataColumn dcDisplay = new DataColumn("DisplayMember");
                dcDisplay.Expression = string.Format("IIF(ProfilePath <> 'null', {1}, '-- not found --')", "ProfilePath", "ProfileName");
                combDT.Columns.Add(dcDisplay);

                if (Globals.ProfilesHex.Count > 0)
                {
                    List<string> NewProfileHex = new List<string>();

                    if (!includedFiles.Contains("game.sii"))
                    {
                        foreach (string profile in Globals.ProfilesHex)
                        {
                            if (Directory.Exists(profile + @"\save"))
                            {
                                ProfileName = Utilities.TextUtilities.FromHexToString(Path.GetFileName(profile));

                                if (ProfileName != null)
                                {
                                    combDT.Rows.Add(profile, ProfileName);
                                    NewProfileHex.Add(profile);
                                }
                            }                                
                        }
                    }
                    else
                    {
                        NewProfileHex.Add(SelectedFolder);
                        combDT.Rows.Add(SelectedFolder, "[C] Custom profile", "custom");
                    }

                    Globals.ProfilesHex = NewProfileHex;

                    //
                    bool isFoundSaves = false;

                    if (combDT.Rows.Count > 0)
                        isFoundSaves = true;

                    if (isFoundSaves)
                    {
                        comboBoxProfiles.Enabled = true;
                    }
                    else
                    {
                        combDT.Rows.Add("null");
                        comboBoxProfiles.Enabled = false;
                        comboBoxSaves.Enabled = false;
                    }

                    comboBoxProfiles.ValueMember = "ProfilePath";
                    comboBoxProfiles.DisplayMember = "DisplayMember";
                    //

                    if (isFoundSaves)
                    {

                        buttonProfilesAndSavesEditProfile.Enabled = true;

                        UpdateStatusBarMessage.ShowStatusMessage(SMStatus.Clear);
                    }
                    else
                    {
                        buttonProfilesAndSavesEditProfile.Enabled = false;

                        UpdateStatusBarMessage.ShowStatusMessage(SMStatus.Error, "error_No valid Saves was found");
                    }

                    comboBoxProfiles.SelectedIndexChanged += new EventHandler(comboBoxProfiles_SelectedIndexChanged);
                    comboBoxProfiles.DataSource = combDT;
                }
                else
                {
                    comboBoxProfiles.Enabled = false;
                    comboBoxSaves.Enabled = false;
                    buttonProfilesAndSavesOpenSaveFolder.Enabled = false;
                    buttonMainDecryptSave.Enabled = false;
                    //buttonMainLoadSave.Enabled = false;

                    MessageBox.Show("Please select another folder", "No valid profiles found");
                }
            }
            catch
            {
                IO_Utilities.ErrorLogWriter("Populating Profiles list failed");
            }
        }

        private void comboBoxProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Globals.ProfilesHex.Count != 0)
            {
                try
                {
                    string AvatarPath = Globals.ProfilesHex[comboBoxProfiles.SelectedIndex] + @"\avatar.png";

                    if (File.Exists(AvatarPath))
                    {
                        Bitmap Source = new Bitmap(AvatarPath);
                        Rectangle SourceRect = new Rectangle(0, 0, 95, 95);
                        Bitmap Cropped = Source.Clone(SourceRect, Source.PixelFormat);
                        pictureBoxProfileAvatar.Image = Cropped;
                    }
                    else
                    {
                        string[] imgpaths = new string[] { @"img\unknown.dds" };
                        pictureBoxProfileAvatar.Image = Utilities.Graphics.ddsImgLoader(imgpaths, 95, 95)[0];
                    }
                }
                catch
                {
                    string[] imgpaths = new string[] { @"img\unknown.dds" };
                    pictureBoxProfileAvatar.Image = Utilities.Graphics.ddsImgLoader(imgpaths, 95, 95)[0];
                }

                try
                {
                    //Read profile data
                    string SiiProfilePath = Globals.ProfilesHex[comboBoxProfiles.SelectedIndex] + @"\profile.sii";

                    LoadProfileDataFile();

                    //Add tooltip to Avatar
                    toolTipMain.SetToolTip(pictureBoxProfileAvatar, MainSaveFileProfileData.getProfileSummary(PlayerLevelNames));
                }
                catch
                { }
            }
            else
            {
                pictureBoxProfileAvatar.Image = null; 
                //Add tooltip to Avatar
                toolTipMain.SetToolTip(pictureBoxProfileAvatar, "");
            }

            if (comboBoxProfiles.SelectedIndex > -1)
                FillProfileSaves();
        }

        private void comboBoxProfiles_DropDown(object sender, EventArgs e)
        {
            comboBoxProfiles.SelectedIndexChanged -= comboBoxProfiles_SelectedIndexChanged;

            string sv = comboBoxProfiles.SelectedValue.ToString();

            FillProfiles();

            int index = FindByValue(comboBoxProfiles, sv);

            if (index > -1)
                comboBoxProfiles.SelectedValue = sv;
            else
                comboBoxProfiles.SelectedIndex = 0;

            comboBoxProfiles.SelectedIndexChanged += comboBoxProfiles_SelectedIndexChanged;
        }

        public void FillProfileSaves()
        {
            try
            {
                if (Globals.ProfilesHex.Count != 0 && !Directory.Exists(Globals.ProfilesHex[comboBoxProfiles.SelectedIndex]))
                {
                    FillProfiles();
                    return;
                }

                comboBoxSaves.SelectedIndexChanged -= new EventHandler(comboBoxSaves_SelectedIndexChanged);

                Globals.SavesHex = new string[0];

                if (Globals.ProfilesHex.Count != 0)
                {
                    string SelectedFolder = Globals.ProfilesHex[comboBoxProfiles.SelectedIndex];

                    List<string> includedFiles = new List<string>();

                    includedFiles = Directory.GetFiles(SelectedFolder).Select(Path.GetFileName).ToList();

                    if (includedFiles.Contains("game.sii"))
                    {
                        Globals.SavesHex = new string[1];
                        Globals.SavesHex[0] = SelectedFolder;
                    }
                    else
                    {
                        SelectedFolder = Globals.ProfilesHex[comboBoxProfiles.SelectedIndex] + @"\save";
                        Globals.SavesHex = Directory.GetDirectories(SelectedFolder).OrderByDescending(f => new FileInfo(f).LastWriteTime).ToArray();
                    }
                }

                DataTable combDT = new DataTable();
                DataColumn dc = new DataColumn("savePath", typeof(string));
                combDT.Columns.Add(dc);

                dc = new DataColumn("saveName", typeof(string));
                combDT.Columns.Add(dc);

                DataColumn dcDisplay = new DataColumn("DisplayMember");
                dcDisplay.Expression = string.Format("IIF(savePath <> 'null', {1}, '-- not found --')", "savePath", "saveName");
                combDT.Columns.Add(dcDisplay);

                if (Globals.SavesHex.Length > 0)
                {

                    bool NotANumber = false;

                    foreach (string saveFolder in Globals.SavesHex)
                    {
                        if (!File.Exists(saveFolder + @"\game.sii") || !File.Exists(saveFolder + @"\info.sii"))
                            continue;

                        string[] folders = saveFolder.Split(new string[] { "\\" }, StringSplitOptions.None);

                        if (folders.Last().Contains(' '))
                        {
                            string tmpName = GetCustomSaveFilename(saveFolder);

                            if (tmpName != "")
                                combDT.Rows.Add(saveFolder, tmpName);
                            else
                                combDT.Rows.Add(saveFolder, "NoName ( " + folders.Last() + " )");
                        }
                        else
                        {
                            foreach (char c in folders.Last())
                            {
                                if (c < '0' || c > '9')
                                {
                                    NotANumber = true;
                                    break;
                                }
                            }

                            if (NotANumber)
                            {
                                string[] namearr = folders.Last().Split(new char[] { '_' });
                                string ProfileName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(namearr[0]);

                                for (int i = 1; i < namearr.Length; i++)
                                {
                                    ProfileName += " " + namearr[i];
                                }

                                combDT.Rows.Add(saveFolder, "- " + ProfileName + " -");
                            }
                            else
                            {
                                string tmpName = GetCustomSaveFilename(saveFolder);

                                if (tmpName != "")
                                    combDT.Rows.Add(saveFolder, tmpName);
                                else
                                    combDT.Rows.Add(saveFolder, "NoName ( " + folders.Last() + " )");
                            }

                            NotANumber = false;
                        }

                    }

                    bool isFoundSaves = false;

                    if (combDT.Rows.Count > 0)
                        isFoundSaves = true;

                    if (isFoundSaves)
                    {
                        comboBoxSaves.Enabled = true;
                    }
                    else
                    {
                        combDT.Rows.Add("null");
                        comboBoxSaves.Enabled = false;
                    }

                    comboBoxSaves.ValueMember = "savePath";
                    comboBoxSaves.DisplayMember = "DisplayMember"; //"saveName";

                    if (isFoundSaves)
                    {
                        comboBoxSaves.SelectedIndexChanged += new EventHandler(comboBoxSaves_SelectedIndexChanged);

                        buttonProfilesAndSavesOpenSaveFolder.Enabled = true;
                        buttonMainDecryptSave.Enabled = true;

                        UpdateStatusBarMessage.ShowStatusMessage(SMStatus.Clear);
                    }
                    else
                    {
                        buttonProfilesAndSavesOpenSaveFolder.Enabled = false;
                        buttonMainDecryptSave.Enabled = false;
                        buttonMainLoadSave.Enabled = false;

                        UpdateStatusBarMessage.ShowStatusMessage(SMStatus.Error, "error_No valid Saves was found");
                    }

                    comboBoxSaves.DataSource = combDT;
                }
                else
                {
                    combDT.Rows.Add("null");
                    comboBoxSaves.ValueMember = "savePath";
                    comboBoxSaves.DisplayMember = "DisplayMember";

                    comboBoxSaves.DataSource = combDT;

                    comboBoxSaves.Enabled = false;

                    comboBoxSaves.Enabled = false;
                    buttonProfilesAndSavesOpenSaveFolder.Enabled = false;
                    buttonMainDecryptSave.Enabled = false;
                    buttonMainLoadSave.Enabled = false;

                    UpdateStatusBarMessage.ShowStatusMessage(SMStatus.Error, "error_No Save file folders found");
                    //MessageBox.Show("No Save file folders found");
                }
            }
            catch
            {
                IO_Utilities.ErrorLogWriter("Populating Saves list failed");
            }
        }

        private void comboBoxSaves_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonMainDecryptSave.Enabled = true;

            DataRowView drv = (DataRowView)comboBoxPrevProfiles.SelectedItem;

            if (drv["ProfileType"].ToString() == "steam")
            {
                buttonMainLoadSave.Enabled = false;
                buttonMainLoadSave.Text = "Disable Steam Cloud";

                buttonMainLoadSave.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 204);
            }
            else
            {
                buttonMainLoadSave.Enabled = true;
                buttonMainLoadSave.Text = "Load";

                buttonMainLoadSave.Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Bold, GraphicsUnit.Point, 204);
            }                
        }

        private void comboBoxSaves_DropDown(object sender, EventArgs e)
        {
            string sv = comboBoxSaves.SelectedValue.ToString();

            FillProfileSaves();

            int index = FindByValue(comboBoxSaves, sv);

            if (index > -1)
                comboBoxSaves.SelectedValue = sv;
            else
                comboBoxSaves.SelectedIndex = 0;
        }
        //end Profile and Saves groupbox
        //end Main part controls
    }
}