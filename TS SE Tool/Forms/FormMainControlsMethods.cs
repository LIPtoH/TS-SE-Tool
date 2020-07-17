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

            textBoxUserCompanyMoneyAccount.Refresh();
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
                LogWriter("Wrong language setting format");
            }

            CultureInfo ci = Thread.CurrentThread.CurrentUICulture;

            try
            {
                this.SuspendLayout();

                HelpTranslateFormMethod(this, toolTipMain, ci);
                HelpTranslateMenuStripMethod(menuStripMain, ResourceManagerMain, ci);

                this.ResumeLayout();

                for (int i = 0; i < 6; i++)
                {
                    string translatedString = ResourceManagerMain.GetString("labelProfileSkillName" + i.ToString(), ci);

                    foreach (Control c in groupBoxProfileSkill.Controls)
                    {
                        if (c.Name == "profileSkillsPanel" + i.ToString())
                        {
                            toolTipMain.SetToolTip(c, translatedString);
                        }
                    }
                }

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
            {
            }
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
            string url = "https://liptoh.now.im/TS-SET-Tutorial";
            Process.Start(url);
        }

        //Downloads
        private void checkGitHubRelesesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string url = "https://github.com/LIPtoH/TS-SE-Tool/releases";
            Process.Start(url);
        }

        private void checkTMPForumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string url = "https://forum.truckersmp.com/index.php?/topic/79561-ts-saveeditor-tool";
            Process.Start(url);
        }

        private void checkSCSForumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string url = "https://forum.scssoft.com/viewtopic.php?f=34&t=266092";
            Process.Start(url);
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
                Bitmap bgimg = new Bitmap(SkillImgS[i], pSkillsNameHeight, pSkillsNameWidth);
                tmp[0].BackgroundImage = bgimg;
                if (!_state)
                    tmp[0].BackgroundImage = ConvertBitmapToGrayscale(tmp[0].BackgroundImage);
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
                DialogResult result = MessageBox.Show("Savefile not saved.\nDo you want to discard changes and switch game type?", "Switching game", MessageBoxButtons.YesNo);

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
            buttonMainDecryptSave.Enabled = true;
            buttonMainLoadSave.Enabled = true;

            FillAllProfilesPaths();
        }

        private void buttonProfilesAndSavesEditProfile_Click(object sender, EventArgs e)
        {
            FormProfileEditor FormWindow = new FormProfileEditor();
            FormWindow.ParentForm = this;
            DialogResult t = FormWindow.ShowDialog();

            if(t != DialogResult.Cancel)
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
                LogWriter("Backing up file to: " + SavefilePath + @"\game_backup.sii");

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

            LoadSaveFile(); //Load save file
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

            LogWriter("Backing up file to: " + SavefilePath + @"\game_backup.sii");
            //File.Copy(SiiSavePath, SiiSavePath + "_backup", true);
            File.Copy(SiiSavePath, SavefilePath + @"\game_backup.sii", true);

            WriteSaveFile(); //Save save file with or without changes

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
            string MyDocumentsPath = "";
            string RemoteUserdataDirectory = "";

            MyDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + dictionaryProfiles[GameType];// Globals.CurrentGame;

            try
            {
                //string SteamInstallPath = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Valve\Steam", "InstallPath", null).ToString();
                string SteamInstallPath = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Valve\Steam", "InstallPath", null).ToString();

                if (SteamInstallPath == null)
                {
                    //unknown steam path
                }
                else
                {
                    string SteamCloudPath = SteamInstallPath + @"\userdata";
                    if (!Directory.Exists(SteamCloudPath))
                    {
                        //no userdata
                    }
                    else
                    {
                        string[] userdatadirectories = Directory.GetDirectories(SteamCloudPath);

                        if (userdatadirectories.Length == 0)
                        {
                            //no steam user directories
                        }
                        else
                        {
                            DateTime lastHigh = DateTime.Now;

                            string CurrentUserDir = Directory.GetDirectories(SteamCloudPath).OrderByDescending(f => new FileInfo(f).LastWriteTime).ToArray()[0];//null;

                            string GameID = "";
                            if (GameType == "ETS2")
                                GameID = @"\227300"; //ETS2
                            else
                                GameID = @"\270880"; //ATS

                            if (!Directory.Exists(MyDocumentsPath) && !Directory.Exists(CurrentUserDir + GameID))
                            {
                                MessageBox.Show("Standart Game Save folders don't exist");
                                return;
                            }

                            RemoteUserdataDirectory = CurrentUserDir + GameID + @"\remote";
                        }
                    }
                }
            }
            catch
            { }

            DataTable combDT = new DataTable();
            DataColumn dc = new DataColumn("ProfileID", typeof(string));
            combDT.Columns.Add(dc);

            dc = new DataColumn("ProfileName", typeof(string));
            combDT.Columns.Add(dc);

            List<string> tempList = new List<string>();

            if (checkBoxProfilesAndSavesProfileBackups.Checked)
            {
                if (Directory.Exists(MyDocumentsPath))
                    foreach (string folder in Directory.GetDirectories(MyDocumentsPath))
                    {
                        if (Path.GetFileName(folder).StartsWith("profiles")) //Documents
                        {
                            if (Directory.Exists(folder) && Directory.GetDirectories(folder).Count() > 0)
                            {
                                combDT.Rows.Add(folder, "[L] " + Path.GetFileName(folder));
                                tempList.Add(folder);
                            }
                        }
                    }

                //string RemoteUserdataDirectory Steam Profiles
                if (Directory.Exists(RemoteUserdataDirectory))
                    foreach (string folder in Directory.GetDirectories(RemoteUserdataDirectory))
                    {
                        if (Path.GetFileName(folder).StartsWith("profiles")) //Steam
                        {
                            if (Directory.Exists(folder) && Directory.GetDirectories(folder).Count() > 0)
                            {
                                combDT.Rows.Add(folder, "[S] " + Path.GetFileName(folder));
                                tempList.Add(folder);
                            }                                
                        }
                    }
            }
            else
            {
                string folder = MyDocumentsPath + @"\profiles";

                if (Directory.Exists(folder) && Directory.GetDirectories(folder).Count() > 0)
                {
                    combDT.Rows.Add(folder, "[L] profiles");
                    tempList.Add(folder);
                }

                folder = RemoteUserdataDirectory + @"\profiles";

                if (Directory.Exists(folder) && Directory.GetDirectories(folder).Count() > 0)
                {
                    combDT.Rows.Add(folder, "[S] profiles");
                    tempList.Add(folder);
                }

            }

            int index = 0;
            if (ProgSettingsV.CustomPaths.Keys.Contains(GameType))
                foreach (string CustPath in ProgSettingsV.CustomPaths[GameType])
                {
                    index++;
                    if (Directory.Exists(CustPath))
                    {
                        if (Directory.Exists(CustPath + @"\profiles"))
                        {
                            combDT.Rows.Add(CustPath + @"\profiles", "[C] Custom path " + index.ToString());
                            tempList.Add(CustPath + @"\profiles");
                        }
                        else
                        {
                            combDT.Rows.Add(CustPath, "[C] Custom path " + index.ToString());
                            tempList.Add(CustPath);
                        }
                    }
                }

            Globals.ProfilesPaths = tempList.ToArray();
            comboBoxPrevProfiles.ValueMember = "ProfileID";
            comboBoxPrevProfiles.DisplayMember = "ProfileName";
            comboBoxPrevProfiles.DataSource = combDT;

            if (comboBoxPrevProfiles.Items.Count > 0)
            {
                comboBoxPrevProfiles.Enabled = true;
                buttonProfilesAndSavesOpenSaveFolder.Enabled = true;
                buttonMainDecryptSave.Enabled = true;
                buttonMainLoadSave.Enabled = true;
            }
            else
            {
                comboBoxPrevProfiles.SelectedIndex = -1;
                comboBoxPrevProfiles.Enabled = false;

                comboBoxProfiles.Enabled = false;
                comboBoxSaves.Enabled = false;

                buttonProfilesAndSavesOpenSaveFolder.Enabled = false;
                buttonMainDecryptSave.Enabled = false;
                buttonMainLoadSave.Enabled = false;

                MessageBox.Show("No profiles found");
            }
        }

        private void comboBoxPrevProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!Directory.Exists(Globals.ProfilesPaths[comboBoxPrevProfiles.SelectedIndex]))
            {
                return;
            }

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
            string sv = comboBoxPrevProfiles.SelectedValue.ToString();

            comboBoxPrevProfiles.SelectedIndexChanged -= comboBoxPrevProfiles_SelectedIndexChanged;
            FillAllProfilesPaths();
            comboBoxPrevProfiles.SelectedIndexChanged += comboBoxPrevProfiles_SelectedIndexChanged;

            int index = FindByValue(comboBoxPrevProfiles, sv);

            if (index > -1)
                comboBoxPrevProfiles.SelectedValue = sv;
            else
                comboBoxPrevProfiles.SelectedIndex = 0;
        }

        public void FillProfiles()
        {
            if (!Directory.Exists(Globals.ProfilesPaths[comboBoxPrevProfiles.SelectedIndex]))
            {
                FillAllProfilesPaths();
                return;
            }

            string Profile = "";
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


            if (Globals.ProfilesHex.Count > 0)
            {
                DataTable combDT = new DataTable();
                DataColumn dc = new DataColumn("ProfilePath", typeof(string));
                combDT.Columns.Add(dc);

                dc = new DataColumn("ProfileName", typeof(string));
                combDT.Columns.Add(dc);

                List<string> NewProfileHex = new List<string>();

                if (!includedFiles.Contains("game.sii"))
                {
                    foreach (string profile in Globals.ProfilesHex)
                    {
                        Profile = Utilities.TextUtilities.FromHexToString(Path.GetFileName(profile));
                        if (Profile != null && Directory.Exists(profile + @"\save"))
                        {
                            combDT.Rows.Add(profile, Profile);
                            NewProfileHex.Add(profile);
                        }
                    }
                }
                else
                {
                    NewProfileHex.Add(SelectedFolder);
                    combDT.Rows.Add(SelectedFolder, "[C] Custom profile");
                }

                Globals.ProfilesHex = NewProfileHex;

                comboBoxProfiles.ValueMember = "ProfilePath";
                comboBoxProfiles.DisplayMember = "ProfileName";
                comboBoxProfiles.DataSource = combDT;

                if (comboBoxProfiles.Items.Count > 0)
                {
                    //comboBoxProfiles.SelectedIndex = 0;
                    comboBoxProfiles.Enabled = true;
                    //comboBoxSaves.Enabled = true;
                    buttonProfilesAndSavesOpenSaveFolder.Enabled = true;
                    buttonMainDecryptSave.Enabled = true;
                    buttonMainLoadSave.Enabled = true;
                }
                else
                {
                    comboBoxProfiles.Enabled = false;
                    comboBoxProfiles.DataSource = null;
                    comboBoxProfiles.SelectedIndex = -1;
                    comboBoxSaves.Enabled = false;
                    comboBoxSaves.DataSource = null;

                    buttonProfilesAndSavesOpenSaveFolder.Enabled = false;
                    buttonMainDecryptSave.Enabled = false;
                    buttonMainLoadSave.Enabled = false;

                    UpdateStatusBarMessage.ShowStatusMessage(SMStatus.Error, "error_No valid Profiles was found");
                }
            }
            else
            {
                comboBoxProfiles.Enabled = false;
                comboBoxSaves.Enabled = false;
                buttonProfilesAndSavesOpenSaveFolder.Enabled = false;
                buttonMainDecryptSave.Enabled = false;
                buttonMainLoadSave.Enabled = false;

                MessageBox.Show("Please select another folder","No valid profiles found");
            }
        }

        private void comboBoxProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxProfiles.SelectedIndex > -1)
                FillProfileSaves();

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
                    pictureBoxProfileAvatar.Image = ExtImgLoader(imgpaths, 95, 95, 0, 0)[0];
                }
            }
            catch
            {
                string[] imgpaths = new string[] { @"img\unknown.dds" };
                pictureBoxProfileAvatar.Image = ExtImgLoader(imgpaths, 95, 95, 0, 0)[0];
            }

            try
            {
                //Read profile data
                string SiiProfilePath = Globals.ProfilesHex[comboBoxProfiles.SelectedIndex] + @"\profile.sii";

                LoadProfileDataFile();

                //Add tooltip to Avatar
                toolTipMain.SetToolTip(pictureBoxProfileAvatar, MainSaveFileProfileData.GetProfileSummary(PlayerLevelNames));
            }
            catch
            { }
        }

        private void comboBoxProfiles_DropDown(object sender, EventArgs e)
        {
            string sv = comboBoxProfiles.SelectedValue.ToString();

            comboBoxProfiles.SelectedIndexChanged -= comboBoxProfiles_SelectedIndexChanged;
            FillProfiles();
            comboBoxProfiles.SelectedIndexChanged += comboBoxProfiles_SelectedIndexChanged;

            int index = FindByValue(comboBoxProfiles, sv);

            if (index > -1)
                comboBoxProfiles.SelectedValue = sv;
            else
                comboBoxProfiles.SelectedIndex = 0;
        }

        public void FillProfileSaves()
        {
            if (!Directory.Exists(Globals.ProfilesHex[comboBoxProfiles.SelectedIndex]))
            {
                FillProfiles();
                return;
            }

            string SelectedFolder = Globals.ProfilesHex[comboBoxProfiles.SelectedIndex];

            List<string> includedFiles = new List<string>();
            string[] t1 = Directory.GetFiles(SelectedFolder);
            List<string> t2 = t1.Select(Path.GetFileName).ToList();
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

            if (Globals.SavesHex.Length > 0)
            {
                DataTable combDT = new DataTable();
                DataColumn dc = new DataColumn("savePath", typeof(string));
                combDT.Columns.Add(dc);

                dc = new DataColumn("saveName", typeof(string));
                combDT.Columns.Add(dc);

                bool NotANumber = false;

                foreach (string profile in Globals.SavesHex)
                {
                    if (!File.Exists(profile + @"\game.sii") || !File.Exists(profile + @"\info.sii"))
                        continue;

                    string[] fold = profile.Split(new string[] { "\\" }, StringSplitOptions.None);

                    foreach (char c in fold[fold.Length - 1])
                    {
                        if (c < '0' || c > '9')
                        {
                            NotANumber = true;
                            break;
                        }
                    }

                    if (NotANumber)
                    {
                        string[] namearr = fold[fold.Length - 1].Split(new char[] { '_' });
                        string ProfileName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(namearr[0]);

                        for (int i = 1; i < namearr.Length; i++)
                        {
                            ProfileName += " " + namearr[i];
                        }

                        combDT.Rows.Add(profile, "- " + ProfileName + " -");
                    }
                    else
                        combDT.Rows.Add(profile, GetCustomSaveFilename(profile));

                    NotANumber = false;
                }

                comboBoxSaves.ValueMember = "savePath";
                comboBoxSaves.DisplayMember = "saveName";

                comboBoxSaves.DataSource = combDT;

                if (comboBoxSaves.Items.Count > 0)
                {
                    comboBoxSaves.Enabled = true;
                    //comboBoxSaves.SelectedIndex = 0;

                    buttonProfilesAndSavesOpenSaveFolder.Enabled = true;
                    buttonMainDecryptSave.Enabled = true;
                    buttonMainLoadSave.Enabled = true;

                    UpdateStatusBarMessage.ShowStatusMessage(SMStatus.Clear);
                }
                else
                {
                    comboBoxSaves.Enabled = false;
                    comboBoxSaves.DataSource = null;
                    comboBoxSaves.SelectedIndex = -1;

                    buttonProfilesAndSavesOpenSaveFolder.Enabled = false;
                    buttonMainDecryptSave.Enabled = false;
                    buttonMainLoadSave.Enabled = false;

                    UpdateStatusBarMessage.ShowStatusMessage(SMStatus.Error, "error_No valid Saves was found");
                }
            }
            else
            {
                comboBoxSaves.Enabled = false;
                buttonProfilesAndSavesOpenSaveFolder.Enabled = false;
                buttonMainDecryptSave.Enabled = false;
                buttonMainLoadSave.Enabled = false;

                MessageBox.Show("No save file folders found");
            }
        }

        private void comboBoxSaves_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonMainDecryptSave.Enabled = true;
            buttonMainLoadSave.Enabled = true;
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