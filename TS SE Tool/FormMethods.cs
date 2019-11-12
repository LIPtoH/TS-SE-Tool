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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Resources;
using System.Windows;
using ICSharpCode.SharpZipLib.GZip;
using Microsoft.Win32;
using TS_SE_Tool.CustomClasses;

namespace TS_SE_Tool
{
    public partial class FormMain
    {
        private void ShowStatusMessages(string _status, string _message)
        {
            if (_status == "e")
            {
                toolStripStatusMessages.ForeColor = Color.Red;
            }
            else
            if (_status == "i")
            {
                toolStripStatusMessages.ForeColor = Color.Black;
            }
            else
            if (_status == "clear")
            {
                toolStripStatusMessages.Text = "";
                return;
            }
            toolStripStatusMessages.Text = GetranslatedString(_message);
        }

        public void ShowStatusMessages(string _status, string _message, Form _senderForm, string _statusStrip, string _targetLabel)
        {
            StatusStrip tssm = (StatusStrip)_senderForm.Controls.Find(_statusStrip, true)[0];

            if (_status == "e")
            {   
                tssm.Items[_targetLabel].ForeColor = Color.Red;
            }
            else
            if (_status == "i")
            {
                tssm.Items[_targetLabel].ForeColor = Color.Black;
            }
            else
            if (_status == "clear")
            {
                tssm.Items[_targetLabel].Text = "";
                return;
            }

            tssm.Items[_targetLabel].Text = GetranslatedString(_message);
        }

        private void ShowStatusMessages(string _status, string _message, string _option)
        {

            toolStripStatusMessages.Text = GetranslatedString(_message) + " (" + _option + ")";
            if (_status == "e")
            {
                toolStripStatusMessages.ForeColor = Color.Red;
            }
            if (_status == "i")
            {
                toolStripStatusMessages.ForeColor = Color.Black;
            }
        }

        public void SetDefaultValues(bool _initial)
        {
            if (_initial)
            {
                ResourceManagerMain = new PlainTXTResourceManager();
                ProgSettingsV = new ProgSettings();

                ProgSettingsV.ProgramVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

                SavefileVersion = 0;
                SupportedSavefileVersionETS2 = new int[] { 39, 40, 41, 42, 43, 44 }; //Supported save version
                SupportedGameVersionETS2 = "1.33.x - 1.36.x"; //Last game version Tested on
                //SupportedSavefileVersionATS;
                SupportedGameVersionATS = "1.33.x - 1.36.x"; //Last game version Tested on

                ProfileETS2 = @"\Euro Truck Simulator 2";
                ProfileATS = @"\American Truck Simulator";

                comboBoxPrevProfiles.FlatStyle =
                comboBoxProfiles.FlatStyle =
                comboBoxSaves.FlatStyle = FlatStyle.Flat;

                dictionaryProfiles = new Dictionary<string, string>();
                dictionaryProfiles.Add("ETS2", ProfileETS2);
                dictionaryProfiles.Add("ATS", ProfileATS);

                CompaniesLngDict = new Dictionary<string, string>();
                CitiesLngDict = new Dictionary<string, string>();
                CountriesLngDict = new Dictionary<string, string>();
                CargoLngDict = new Dictionary<string, string>();
                UrgencyLngDict = new Dictionary<string, string>();
                //CustomStringsDict = new Dictionary<string, string>();
                TruckBrandsLngDict = new Dictionary<string, string>();

                GameType = "ETS2";
                Globals.CurrentGame = dictionaryProfiles[GameType];

                DistancesTable = new DataTable();
                DistancesTable.Columns.Add("SourceCity", typeof(string));
                DistancesTable.Columns.Add("SourceCompany", typeof(string));
                DistancesTable.Columns.Add("DestinationCity", typeof(string));
                DistancesTable.Columns.Add("DestinationCompany", typeof(string));
                DistancesTable.Columns.Add("Distance", typeof(int));
                DistancesTable.Columns.Add("FerryTime", typeof(int));
                DistancesTable.Columns.Add("FerryPrice", typeof(int));

                CountryDictionary = new CountryDictionary();

                PlayerLevelNames = new List<LevelNames>();

                #region Player level names
                LevelNames lvl_name0 = new LevelNames(0, "Newbie", "FFE0E0E0");
                LevelNames lvl_name1 = new LevelNames(5, "Enthusiast", "FF45C294");
                LevelNames lvl_name2 = new LevelNames(10, "Workhorse", "FF75BAEA");
                LevelNames lvl_name3 = new LevelNames(15, "Entrepeneur", "FF3A88F4");
                LevelNames lvl_name4 = new LevelNames(20, "Master", "FF5847F0");
                LevelNames lvl_name5 = new LevelNames(25, "Instructor", "FFDA9356");
                LevelNames lvl_name6 = new LevelNames(30, "Elite", "FFF58493");
                LevelNames lvl_name7 = new LevelNames(40, "King of the Road", "FFC99EF2");
                LevelNames lvl_name8 = new LevelNames(50, "Legend", "FFC2F9FF");
                LevelNames lvl_name9 = new LevelNames(100, "Divine Champion", "FFF1DEA5");

                PlayerLevelNames.Add(lvl_name0);
                PlayerLevelNames.Add(lvl_name1);
                PlayerLevelNames.Add(lvl_name2);
                PlayerLevelNames.Add(lvl_name3);
                PlayerLevelNames.Add(lvl_name4);
                PlayerLevelNames.Add(lvl_name5);
                PlayerLevelNames.Add(lvl_name6);
                PlayerLevelNames.Add(lvl_name7);
                PlayerLevelNames.Add(lvl_name8);
                PlayerLevelNames.Add(lvl_name9);
                #endregion

                CurrencyDict = new Dictionary<string, List<string>>();
                CurrencyDictR = new Dictionary<string, double>();

                #region Currency

                string curName = "EUR";
                CurrencyDictR.Add(curName, 1);
                string[] input = new string[] { "", "€", "" };
                List<string> curLst = new List<string>(input);
                CurrencyDict.Add(curName, curLst);
                
                curName = "CHF";
                CurrencyDictR.Add(curName, 1.142);
                input = new string[] { "", "", " CHF" };
                curLst = new List<string>(input);
                CurrencyDict.Add(curName, curLst);

                curName = "CZK";
                CurrencyDictR.Add(curName, 25.88);
                input = new string[] { "", "", " Kč" };
                curLst = new List<string>(input);
                CurrencyDict.Add(curName, curLst);

                curName = "GBP";
                CurrencyDictR.Add(curName, 0.875);
                input = new string[] { "", "£", "" };
                curLst = new List<string>(input);
                CurrencyDict.Add(curName, curLst);

                curName = "PLN";
                CurrencyDictR.Add(curName, 4.317);
                input = new string[] { "", "", " zł" };
                curLst = new List<string>(input);
                CurrencyDict.Add(curName, curLst);

                curName = "HUF";
                CurrencyDictR.Add(curName, 325.3);
                input = new string[] { "", "", " Ft" };
                curLst = new List<string>(input);
                CurrencyDict.Add(curName, curLst);

                curName = "DKK";
                CurrencyDictR.Add(curName, 7.46);
                input = new string[] { "", "", " kr" };
                curLst = new List<string>(input);
                CurrencyDict.Add(curName, curLst);

                curName = "SEK";
                CurrencyDictR.Add(curName, 10.52);
                input = new string[] { "", "", " kr" };
                curLst = new List<string>(input);
                CurrencyDict.Add(curName, curLst);

                curName = "NOK";
                CurrencyDictR.Add(curName, 9.51);
                input = new string[] { "", "", " kr" };
                curLst = new List<string>(input);
                CurrencyDict.Add(curName, curLst);

                curName = "RUB";
                CurrencyDictR.Add(curName, 77.05);
                input = new string[] { "", "₽", "" };
                curLst = new List<string>(input);
                CurrencyDict.Add(curName, curLst);
                #endregion 
                
                //Urgency
                UrgencyArray = new int[] { 0, 1, 2 };

                DistanceMultipliers = new Dictionary<string, double>();
                DistanceMultipliers.Add("km", 1);
                DistanceMultipliers.Add("mi", km_to_mileconvert);

                ADRImgS = new Image[6];
                ADRImgSGrey = new Image[6];
                SkillImgSBG = new Image[5];
                SkillImgS = new Image[6];
                ADRbuttonArray = new CheckBox[6];
                GaragesImg = new Image[1];
                GaragesHQImg = new Image[1];
                CitiesImg = new Image[2];
                UrgencyImg = new Image[3];
                CargoTypeImg = new Image[3];
                CargoType2Img = new Image[3];
                GameIconeImg = new Image[2];
                TruckPartsImg = new Image[5];
                TrailerPartsImg = new Image[4];
                ProgUIImgs = new Image[0];

                SkillButtonArray = new CheckBox[5, 6];

                TabpagesImages = new ImageList();
            }

            unCertainRouteLength = "";
            FileDecoded = false;
            SavefilePath = "";

            tempInfoFileInMemory = null;
            tempSavefileInMemory = null;
            tempProfileFileInMemory = null;

            //string ATSexp = "";

            if (GameType == "ETS2")
                Globals.PlayerLevelUps = new int[] {200, 500, 700, 900, 1000, 1100, 1300, 1600, 1700, 2100, 2300, 2600, 2700,
                    2900, 3000, 3100, 3400, 3700, 4000, 4300, 4600, 4700, 4900, 5200, 5700, 5900, 6000, 6200, 6600, 6800};
            else
                Globals.PlayerLevelUps = new int[] {200, 500, 700, 900, 1100, 1300, 1500, 1700, 1900, 2100, 2300, 2500, 2700,
                    2900, 3100, 3300, 3500, 3700, 4000, 4300, 4600, 4900, 5200, 5500, 5800, 6100, 6400, 6700, 7000, 7300};

            PlayerDataV = new PlayerData("", 0, new byte[] { 0, 0, 0, 0, 0, 0 }, 0);
            ProfileDataV = new ProfileData();

            UserCompanyAssignedTruckPlacementEdited = false;

            InfoDepContinue = false;

            CompaniesList = new List<string>();
            CitiesList = new List<City>();

            CountriesList = new List<string>();
            CargoesList = new List<Cargo>();
            TrailerDefinitionVariants = new Dictionary<string, List<string>>();
            TrailerVariants = new List<string>();

            HeavyCargoList = new List<string>();
            CompanyTruckList = new List<CompanyTruck>();
            CompanyTruckListDB = new List<CompanyTruck>();
            CompanyTruckListDiff = new List<CompanyTruck>();

            UserColorsList = new List<Color>();
            GaragesList = new List<Garages>();
            UserTruckDictionary = new Dictionary<string, UserCompanyTruckData>();
            UserDriverDictionary = new Dictionary<string, UserCompanyDriverData>();
            DriverPool = new List<string>();
            UserTrailerDictionary = new Dictionary<string, UserCompanyTruckData>();
            UserTrailerDefDictionary = new Dictionary<string, List<string>>();

            extraVehicles = new List<string>();
            extraDrivers = new List<string>();

            VisitedCities = new List<VisitedCity>();

            CargoesListDB = new List<Cargo>();
            CitiesListDB = new List<string>();
            CompaniesListDB = new List<string>();
            CargoesListDiff = new List<Cargo>();
            CitiesListDiff = new List<string>();
            CompaniesListDiff = new List<string>();

            DBDependencies = new List<string>();
            SFDependencies = new List<string>();

            ExternalCompanies = new List<ExtCompany>();

            ExtCargoList = new List<ExtCargo>();

            //EconomyEventQueueList = new string[0];
            EconomyEventsTable = new string[0, 0];
            EconomyEventUnitLinkStringList = new string[0];

            JobsAmountAdded = 0;
            LastVisitedCity = "";
            InGameTime = 0;
            RandomValue = new Random();
            //CitiesListAddedToCompare = new string[1];

            //JobsListAdded = new string[0];
            LastModifiedTimestamp = new DateTime();
            //ListSavefileCompanysString = new List<string>();//string[0];

            AddedJobsDictionary = new Dictionary<string, List<JobAdded>>();

            GPSbehind = new Dictionary<string, List<string>>();
            GPSahead = new Dictionary<string, List<string>>();

            GPSbehindOnline = new Dictionary<string, List<string>>();
            GPSaheadOnline = new Dictionary<string, List<string>>();

            namelessList = new List<string>();
            namelessLast = "";
            //JobsTotalDistance = 0;
            LoopStartCity = "";
            LoopStartCompany = "";
            ProgPrevVersion = "0.0.1.0";

            RouteList = new Routes();
            DistancesTable.Clear();

            components = null;

            //Clear elements
            ClearFormControls();
        }

        private void ClearFormControls()
        {
            comboBoxFreightMarketTrailerVariant.SelectedIndex = -1;
            comboBoxFreightMarketTrailerVariant.DataSource = null;
            comboBoxFreightMarketTrailerDef.SelectedIndex = -1;
            comboBoxFreightMarketTrailerDef.DataSource = null;
            comboBoxFreightMarketCargoList.SelectedIndex = -1;
            comboBoxFreightMarketCargoList.DataSource = null;
        }

        private void PopulateFormControlsk()
        {
            buttonMainDecryptSave.Enabled = false;
            buttonMainWriteSave.Enabled = true;

            //string t1 = "Trucking since:\n\r" + DateTimeOffset.FromUnixTimeSeconds(ProfileDataV.CreationTime).DateTime.ToLocalTime().ToString();
            //toolTipMain.SetToolTip(pictureBoxProfileAvatar, t1);
            
            AddTranslationToData();

            FillFormProfileControls();
            UpdateUserColorsButtons();
            FillFormCompanyControls();

            FillUserCompanyTrucksList();
            FillUserCompanyTrailerList();

            FillFormFreightMarketControls();

            FillFormCargoOffersControls();
        }

        //Menu controls
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
        //About
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox aboutWindow = new AboutBox();
            aboutWindow.ShowDialog();
        }

        //How to
        private void localPDFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string pdf_path = Directory.GetCurrentDirectory() + @"\HowTo.pdf";
            if (File.Exists(pdf_path))
                Process.Start(pdf_path);
        }

        private void youTubeVideoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string url = "https://rebrand.ly/TS-SET-Tutorial";
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
            string url = "http://rebrand.ly/TS-SET-Download";
            Process.Start(url);
        }


        //Main part controls
        private void buttonRefreshAll_Click(object sender, EventArgs e)
        {
            buttonMainDecryptSave.Enabled = true;
            buttonMainLoadSave.Enabled = true;

            FillAllProfilesPaths();
        }

        private void buttonOpenSaveFolder_Click(object sender, EventArgs e)
        {
            if(Directory.Exists(Globals.SavesHex[comboBoxSaves.SelectedIndex]))
                Process.Start(Globals.SavesHex[comboBoxSaves.SelectedIndex]);
            //else
        }

        private void buttonMainAddCustomFolder_Click(object sender, EventArgs e)
        {
            FormAddCustomFolder FormWindow = new FormAddCustomFolder();
            FormWindow.ShowDialog();
        }

        private void buttonDecryptSave_Click(object sender, EventArgs e)
        {
            SetDefaultValues(false);

            radioButtonMainGameSwitchETS.Enabled = false;
            radioButtonMainGameSwitchATS.Enabled = false;

            checkBoxProfilesAndSavesProfileBackups.Enabled = false;
            buttonProfilesAndSavesRefreshAll.Enabled = false;
            comboBoxPrevProfiles.Enabled = false;
            comboBoxProfiles.Enabled = false;
            comboBoxSaves.Enabled = false;

            buttonMainDecryptSave.Enabled = false;
            buttonMainLoadSave.Enabled = false;

            SavefilePath = Globals.SavesHex[comboBoxSaves.SelectedIndex];
            string SiiSavePath = SavefilePath + @"\game.sii";

            string[] file = NewDecodeFile(SiiSavePath, this, "statusStripMain", "toolStripStatusMessages");

            if (file != null)
            {
                LogWriter("Backing up file to: " + SavefilePath + @"\game_backup.sii");

                File.Copy(SiiSavePath, SavefilePath + @"\game_backup.sii", true);

                File.WriteAllLines(SiiSavePath, file);

                ShowStatusMessages("i", "");
            }
                
            else
                ShowStatusMessages("e", "error_could_not_decode_file");

            radioButtonMainGameSwitchETS.Enabled = true;
            radioButtonMainGameSwitchATS.Enabled = true;


            checkBoxProfilesAndSavesProfileBackups.Enabled = true;
            buttonProfilesAndSavesRefreshAll.Enabled = true;
            comboBoxPrevProfiles.Enabled = true;
            comboBoxProfiles.Enabled = true;
            comboBoxSaves.Enabled = true;

            buttonMainDecryptSave.Enabled = false;
            buttonMainLoadSave.Enabled = true;


            ToggleGame(GameType);

            //GC
            GC.Collect();
            //GC.WaitForPendingFinalizers();
        }

        private void LoadSaveFile_Click(object sender, EventArgs e)
        {
            radioButtonMainGameSwitchETS.Enabled = false;
            radioButtonMainGameSwitchATS.Enabled = false;

            checkBoxProfilesAndSavesProfileBackups.Enabled = false;
            buttonProfilesAndSavesRefreshAll.Enabled = false;

            comboBoxPrevProfiles.Enabled = false;
            comboBoxProfiles.Enabled = false;
            comboBoxSaves.Enabled = false;

            buttonMainDecryptSave.Enabled = false;
            buttonMainLoadSave.Enabled = false;
            buttonMainWriteSave.Enabled = false;

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

            ToggleVisibility(false);
            buttonMainWriteSave.Enabled = false;

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

            MyDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + Globals.CurrentGame;

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
            {

            }

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
                            combDT.Rows.Add(folder, "[L] " + Path.GetFileName(folder)); //combDT.Rows.Add(index, "[L] " + Path.GetFileName(folder));

                            tempList.Add(folder);
                        }
                        if (Path.GetFileName(folder).StartsWith("steam_profiles")) //Documents
                        {
                            combDT.Rows.Add(folder, "[S] " + Path.GetFileName(folder)); //combDT.Rows.Add(index, "[S] " + Path.GetFileName(folder));

                            tempList.Add(folder);
                        }
                    }

                //string RemoteUserdataDirectory Steam Profiles
                if (Directory.Exists(RemoteUserdataDirectory))
                    foreach (string folder in Directory.GetDirectories(RemoteUserdataDirectory))
                    {
                        if (Path.GetFileName(folder).StartsWith("profiles")) //Steam
                        {
                            combDT.Rows.Add(folder, "[S] " + Path.GetFileName(folder)); //combDT.Rows.Add(index, "[S] " + Path.GetFileName(folder));

                            tempList.Add(folder);
                        }
                    }
            }
            else
            {
                string folder = MyDocumentsPath + @"\profiles";

                if (Directory.Exists(folder))
                {
                    combDT.Rows.Add(folder, "[L] profiles");
                    tempList.Add(folder);
                }

                folder = RemoteUserdataDirectory + @"\profiles";

                if (Directory.Exists(folder))
                {
                    combDT.Rows.Add(folder, "[S] profiles");
                    tempList.Add(folder);
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
            }
            /*
            int index = 0;
            if (ProgSettingsV.CustomPaths.Keys.Contains(GameType))
                foreach (string CustPath in ProgSettingsV.CustomPaths[GameType])
                {
                    index++;
                    if (Directory.Exists( CustPath))
                    {
                        combDT.Rows.Add(CustPath, "[C] Custom path " + index.ToString());
                        tempList.Add(CustPath);
                    }   
                }
            */
            Globals.ProfilesPaths = tempList.ToArray();
            comboBoxPrevProfiles.ValueMember = "ProfileID";
            comboBoxPrevProfiles.DisplayMember = "ProfileName";
            comboBoxPrevProfiles.DataSource = combDT;


            if (comboBoxPrevProfiles.Items.Count > 0)
            {
                comboBoxPrevProfiles.SelectedIndex = 0;
                buttonProfilesAndSavesOpenSaveFolder.Enabled = true;
                buttonMainDecryptSave.Enabled = true;
                buttonMainLoadSave.Enabled = true;
                comboBoxPrevProfiles.Enabled = true;
                comboBoxProfiles.Enabled = true;
                comboBoxSaves.Enabled = true;
            }
            else
            {
                buttonProfilesAndSavesOpenSaveFolder.Enabled = false;
                buttonMainDecryptSave.Enabled = false;
                buttonMainLoadSave.Enabled = false;
                comboBoxPrevProfiles.Enabled = false;
                comboBoxProfiles.Enabled = false;
                comboBoxSaves.Enabled = false;

                MessageBox.Show("No profiles found");
            }
        }

        private void comboBoxPrevProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!Directory.Exists(Globals.ProfilesPaths[comboBoxPrevProfiles.SelectedIndex]))
            {
                return;
            }

            string sv = comboBoxPrevProfiles.SelectedValue.ToString();

            FillProfiles();

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
                        Profile = FromHexToString(Path.GetFileName(profile));
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

                comboBoxProfiles.ValueMember = "ProfilePath";
                comboBoxProfiles.DisplayMember = "ProfileName";
                comboBoxProfiles.DataSource = combDT;

                if (comboBoxProfiles.Items.Count > 0)
                {
                    Globals.ProfilesHex = NewProfileHex;

                    comboBoxProfiles.Enabled = true;
                    comboBoxSaves.Enabled = true;
                    comboBoxProfiles.SelectedIndex = 0;
                    buttonProfilesAndSavesOpenSaveFolder.Enabled = true;
                    buttonMainDecryptSave.Enabled = true;
                    buttonMainLoadSave.Enabled = true;
                }
                else
                {
                    comboBoxProfiles.Enabled = false;
                    comboBoxSaves.Enabled = false;
                    comboBoxProfiles.DataSource = null;
                    comboBoxSaves.DataSource = null;
                    comboBoxProfiles.SelectedIndex = -1;

                    buttonProfilesAndSavesOpenSaveFolder.Enabled = false;
                    buttonMainDecryptSave.Enabled = false;
                    buttonMainLoadSave.Enabled = false;

                    ShowStatusMessages("e", "No valid Profiles was found");
                }
            }
            else
            {
                comboBoxProfiles.Enabled = false;
                comboBoxSaves.Enabled = false;
                buttonProfilesAndSavesOpenSaveFolder.Enabled = false;
                buttonMainDecryptSave.Enabled = false;
                buttonMainLoadSave.Enabled = false;

                MessageBox.Show("No profiles found");
            }
        }

        private void comboBoxProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxProfiles.SelectedIndex > -1)
                FillProfileSaves();

            try
            {
                string AvatarPath = Globals.ProfilesHex[comboBoxProfiles.SelectedIndex] + @"\avatar.png";

                Bitmap Source = new Bitmap(AvatarPath);
                Rectangle SourceRect = new Rectangle(0, 0, 95, 95);
                Bitmap Cropped = Source.Clone(SourceRect, Source.PixelFormat);
                pictureBoxProfileAvatar.Image = Cropped;
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
                //CheckProfileInfoData();

                //Add text to avatar
                string t1 = "Trucking since: " + DateTimeOffset.FromUnixTimeSeconds(ProfileDataV.CreationTime).DateTime.ToLocalTime().ToString();

                int playerlvl = ProfileDataV.getPlayerLvl()[0];
                string playerLvlName = "";
                for (int i = PlayerLevelNames.Count - 1; i >= 0; i--)
                    if (PlayerLevelNames[i].LevelLimit <= playerlvl)
                    {
                        playerLvlName = PlayerLevelNames[i].LevelName;
                        break;
                    }

                t1 += "\r\n" + playerLvlName + " (Level " + playerlvl.ToString() + ")";
                t1 += "\r\nDistance driven: " + ProfileDataV.CachedDistance + " km";
                t1 += "\r\nRoads explored: " + (ProfileDataV.RoadsExplored * 100).ToString("0.00") + "%";
                t1 += "\r\nDeliveries finished: " + ProfileDataV.DeliveriesFinished;
                t1 += "\r\nOwned Garages: small: " + ProfileDataV.OwnedGaradesSmall + ",large: " + ProfileDataV.OwnedGaradesLarge;
                t1 += "\r\nOwned Trucks: " + ProfileDataV.OwnedTrucks;
                t1 += "\r\nOwned Trailers: " + ProfileDataV.OwnedTrailers;
                t1 += "\r\nTotal game time spent: " + ProfileDataV.GameTimeSpent / 1440 + " days " + Math.Floor(((decimal)(ProfileDataV.GameTimeSpent % 1440) / 1440) * 24) + " hour(s)";
                t1 += "\r\nPlaying time: " + ProfileDataV.RealTimeSpent / 60 + "h " + ProfileDataV.RealTimeSpent % 60 + " min";


                //Add to Avatar
                toolTipMain.SetToolTip(pictureBoxProfileAvatar, t1);
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
                        combDT.Rows.Add(profile, GetCustomSaveFilename(profile, this, statusStripMain.Name, "toolStripStatusMessages"));

                    NotANumber = false;
                }

                comboBoxSaves.ValueMember = "savePath";
                comboBoxSaves.DisplayMember = "saveName";

                comboBoxSaves.DataSource = combDT;

                comboBoxSaves.Enabled = true;
                comboBoxSaves.SelectedIndex = 0;

                buttonProfilesAndSavesOpenSaveFolder.Enabled = true;
                buttonMainDecryptSave.Enabled = true;
                buttonMainLoadSave.Enabled = true;

                ShowStatusMessages("i", "");
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

        //Profile tab
        private void CreateProfilePanelControls()
        {
            int pSkillsNameHeight = 64, pSkillsNameWidth = 64, pSkillsNameOffset = 5, pSkillsNamelOffset = 12;

            string[] toolskillimgtooltip = new string[] { "ADR", "Long Distance", "High Value Cargo", "Fragile Cargo", "Just-In-Time Delivery", "Ecodriving" };

            for (int i = 0; i < 6; i++)
            {
                Panel Ppanel = new Panel();
                groupBoxProfileSkill.Controls.Add(Ppanel);

                Ppanel.Parent = groupBoxProfileSkill;
                Ppanel.Location = new Point(pSkillsNamelOffset, 17 + (pSkillsNameHeight + pSkillsNameOffset) * i);
                Ppanel.BorderStyle = BorderStyle.None;
                Ppanel.Size = new Size(pSkillsNameWidth, pSkillsNameHeight);
                Ppanel.Name = "profileSkillsPanel" + i.ToString();
                toolTipMain.SetToolTip(Ppanel, toolskillimgtooltip[i]);

                Bitmap bgimg = new Bitmap(SkillImgS[i], pSkillsNameHeight, pSkillsNameWidth);
                Ppanel.BackgroundImage = bgimg;

                Label slabel = new Label();
                groupBoxProfileSkill.Controls.Add(slabel);
                slabel.Name = "labelProfileSkill" + i.ToString() + "Name";
                slabel.Location = new Point(pSkillsNamelOffset * 2 + pSkillsNameWidth, 17 + (pSkillsNameHeight + pSkillsNameOffset) * i);
                slabel.Text = toolskillimgtooltip[i];
                slabel.AutoSize = true;
            }

            int bADRHeight = 48, bADRWidth = 48, pOffset = 6, lOffset = pSkillsNameWidth + pSkillsNamelOffset * 2;

            for (int i = 0; i < 6; i++)
            {
                CheckBox Ppanel = new CheckBox();
                groupBoxProfileSkill.Controls.Add(Ppanel);

                Ppanel.Parent = groupBoxProfileSkill;

                Ppanel.Location = new Point(lOffset + (bADRWidth + pOffset) * i, 17 + 14);
                Ppanel.Appearance = Appearance.Button;
                Ppanel.FlatStyle = FlatStyle.Flat;
                Ppanel.Size = new Size(bADRWidth, bADRHeight);
                Ppanel.Name = "buttonADR" + i.ToString();
                Ppanel.Checked = false;
                Ppanel.Padding = new Padding(0, 0, 1, 2);
                Ppanel.BackgroundImageLayout = ImageLayout.Stretch;


                Ppanel.BackgroundImage = SkillImgSBG[0];
                Ppanel.Image = ADRImgSGrey[i];
                Ppanel.FlatAppearance.BorderSize = 0;

                Ppanel.MouseEnter += new EventHandler(ADRbutton_MouseEnter);
                Ppanel.MouseLeave += new EventHandler(ADRbutton_MouseLeave);
                Ppanel.Click += new EventHandler(ADRbutton_Click);
                Ppanel.CheckedChanged += new EventHandler(ADRbutton_CheckedChanged);
                Ppanel.MouseHover += new EventHandler(ADRbutton_MouseHover);

                ADRbuttonArray[i] = Ppanel;
            }

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    CheckBox Ppanel = new CheckBox();
                    groupBoxProfileSkill.Controls.Add(Ppanel);

                    Ppanel.Parent = groupBoxProfileSkill;

                    Ppanel.Location = new Point(lOffset + (bADRWidth + pOffset) * j, 17 + 14 + (pSkillsNameHeight + pSkillsNameOffset) * (i + 1));
                    Ppanel.Appearance = Appearance.Button;
                    Ppanel.FlatStyle = FlatStyle.Flat;
                    Ppanel.Size = new Size(bADRWidth, bADRHeight);
                    Ppanel.Name = "buttonSkill" + i.ToString() + j.ToString();
                    Ppanel.Checked = false;
                    Ppanel.Padding = new Padding(0, 0, 1, 2);
                    Ppanel.BackgroundImageLayout = ImageLayout.Zoom;

                    Ppanel.BackgroundImage = SkillImgSBG[0];
                    Ppanel.FlatAppearance.BorderSize = 0;

                    Ppanel.MouseEnter += new EventHandler(Skillbutton_MouseEnter);
                    Ppanel.MouseLeave += new EventHandler(Skillbutton_MouseLeave);
                    Ppanel.Click += new EventHandler(Skillbutton_Click);
                    Ppanel.CheckedChanged += new EventHandler(Skillbutton_CheckedChanged);

                    SkillButtonArray[i, j] = Ppanel;
                }
            }

            CreateUserColorsButtons();
        }

        private void FillFormProfileControls()
        {
            FormUpdatePlayerLevel();

            char[] ADR = Convert.ToString(PlayerDataV.PlayerSkills[0], 2).PadLeft(6, '0').ToCharArray();

            for (int i = 0; i < ADR.Length; i++)
            {
                if (ADR[i] == '1')
                    ADRbuttonArray[i].Checked = true;
            }

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < PlayerDataV.PlayerSkills[i + 1]; j++)
                {
                    SkillButtonArray[i, j].Checked = true;
                }
            }
        }

        private void FormUpdatePlayerLevel()
        {
            int playerlvl = PlayerDataV.getPlayerLvl()[0];
            labelPlayerLevelNumber.Text = playerlvl.ToString();

            for (int i = PlayerLevelNames.Count - 1; i >= 0; i--)
                if (PlayerLevelNames[i].LevelLimit <= playerlvl)
                {
                    labelPlayerLevelName.Text = PlayerLevelNames[i].LevelName;
                    panelPlayerLevel.BackColor = PlayerLevelNames[i].NameColor;
                    break;
                }

            labelPlayerExperience.Text = PlayerDataV.ExperiencePoints.ToString();
            labelExperienceNxtLvlThreshhold.Text = "/   " + PlayerDataV.getPlayerLvl()[1].ToString();
        }

        private void CreateUserColorsButtons()
        {
            int padding = 6, width = 108, height = 46, colorcount = 8;

            for (int i = 0; i < colorcount; i++)
            {
                Button rb = new Button();
                rb.Name = "buttonUC" + i.ToString();
                rb.Text = null;
                rb.Location = new Point(8, 32 + (padding + height) * i);
                rb.Size = new Size(width, height);
                rb.FlatStyle = FlatStyle.Flat;
                rb.Enabled = false;

                rb.Click += new EventHandler(SelectColor);

                groupBoxProfileUserColors.Controls.Add(rb);
            }
        }

        private void buttonProfileShareColors_Click(object sender, EventArgs e)
        {
            FormShareUserColors FormWindow = new FormShareUserColors();
            FormWindow.ShowDialog();
            UpdateUserColorsButtons();
        }

        private void UpdateUserColorsButtons()
        {
            int padding = 6, width = 23;

            for (int i = 0; i < UserColorsList.Count; i++)
            {
                Button btn = null;
                string btnname = "buttonUC" + i.ToString();

                if (groupBoxProfileUserColors.Controls.ContainsKey(btnname))
                {
                    btn = groupBoxProfileUserColors.Controls[btnname] as Button;
                }
                else
                {
                    btn.Name = "buttonUC" + i.ToString();
                    btn.Text = null;
                    btn.Location = new Point(6 + (padding + width) * (i), 19);
                    btn.Size = new Size(width, 23);
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.Enabled = false;
                    btn.Click += new EventHandler(SelectColor);

                    groupBoxProfileUserColors.Controls.Add(btn);
                }

                if (btn != null)
                {
                    btn.Enabled = true;
                    if (UserColorsList[i].A == 0)
                    {
                        btn.Text = "X";
                        btn.BackColor = Color.FromName("Control");
                    }                        
                    else
                    {
                        btn.Text = "";
                        btn.BackColor = UserColorsList[i];
                    }   
                }
            }
        }

        internal void SelectColor(object sender, EventArgs e)
        {
            Button obj = sender as Button;
            Color BC = obj.BackColor;
            if (obj.Text != "")
                BC = Color.White;

            OpenPainter.ColorPicker.FormColorPicker frm = new OpenPainter.ColorPicker.FormColorPicker(BC);
            frm.Font = SystemFonts.DialogFont;

            if (frm.ShowDialog() == DialogResult.OK)
            {
                int index = int.Parse(obj.Name.Substring(8, 1));

                UserColorsList[index] = frm.PrimaryColor;

                if (frm.PrimaryColor.A != 0)
                {
                    obj.Text = "";
                    obj.BackColor = frm.PrimaryColor;
                }
                else
                {
                    obj.Text = "X";
                    obj.BackColor = Color.FromName("Control");
                }                    
            }
        }

        //Profile buttons
        private void buttonPlayerLvlPlus01_Click(object sender, EventArgs e)
        {
            PlayerDataV.getPlayerExp(int.Parse(labelPlayerLevelNumber.Text) + 1);

            FormUpdatePlayerLevel();
        }

        private void buttonPlayerLvlPlus10_Click(object sender, EventArgs e)
        {
            PlayerDataV.getPlayerExp(int.Parse(labelPlayerLevelNumber.Text) + 10);

            FormUpdatePlayerLevel();
        }

        private void buttonPlayerLvlMax_Click(object sender, EventArgs e)
        {
            PlayerDataV.getPlayerExp(150);

            FormUpdatePlayerLevel();
        }

        private void buttonPlayerLvlMinus01_Click(object sender, EventArgs e)
        {
            PlayerDataV.getPlayerExp(int.Parse(labelPlayerLevelNumber.Text) - 1);

            FormUpdatePlayerLevel();
        }

        private void buttonPlayerLvlMinus10_Click(object sender, EventArgs e)
        {
            PlayerDataV.getPlayerExp(int.Parse(labelPlayerLevelNumber.Text) - 10);

            FormUpdatePlayerLevel();
        }

        private void buttonPlayerLvlMin_Click(object sender, EventArgs e)
        {
            PlayerDataV.getPlayerExp(0);

            FormUpdatePlayerLevel();
        }
        // end profile buttons

        //Skill buttons
        private void Skillbutton_Click(object sender, EventArgs e)
        {
            CheckBox thisbutton = sender as CheckBox;

            int skillIndex = int.Parse(thisbutton.Name.Substring(11, 1));
            byte buttonIndex = byte.Parse(thisbutton.Name.Substring(12, 1));
            if (thisbutton.Checked)
            {
                for (int j = 0; j < buttonIndex; j++)
                {
                    SkillButtonArray[skillIndex, j].Checked = true;
                }
                PlayerDataV.PlayerSkills[++skillIndex] = ++buttonIndex;
            }
            else
            {
                for (int j = 5; j >= int.Parse(thisbutton.Name.Substring(12, 1)); j--)
                {
                    SkillButtonArray[skillIndex, j].Checked = false;
                }
                PlayerDataV.PlayerSkills[++skillIndex] = buttonIndex;
            }
        }

        private void Skillbutton_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox thisbutton = sender as CheckBox;

            if (thisbutton.Checked)
            {
                thisbutton.BackgroundImage = SkillImgSBG[3];
            }
            else
            {
                thisbutton.BackgroundImage = SkillImgSBG[0];
            }
        }

        private void Skillbutton_MouseEnter(object sender, EventArgs e)
        {
            CheckBox thisbutton = sender as CheckBox;

            int i = int.Parse(thisbutton.Name.Substring(11, 1));

            for (int j = 0; j <= int.Parse(thisbutton.Name.Substring(12, 1)); j++)
            {
                if (!SkillButtonArray[i, j].Checked)
                {
                    SkillButtonArray[i, j].BackgroundImage = SkillImgSBG[1];
                }

            }

            for (int j = int.Parse(thisbutton.Name.Substring(12, 1)); j < 6; j++)
            {
                if (SkillButtonArray[i, j].Checked)
                {
                    SkillButtonArray[i, j].BackgroundImage = SkillImgSBG[0];
                }

            }
            //thisbutton.BackgroundImage = SkillImgSBG[1];
        }

        private void Skillbutton_MouseLeave(object sender, EventArgs e)
        {
            CheckBox thisbutton = sender as CheckBox;
            int i = int.Parse(thisbutton.Name.Substring(11, 1));

            for (int j = 0; j <= int.Parse(thisbutton.Name.Substring(12, 1)); j++)
            {
                if (!SkillButtonArray[i, j].Checked)
                {
                    SkillButtonArray[i, j].BackgroundImage = SkillImgSBG[0];
                }
            }

            for (int j = int.Parse(thisbutton.Name.Substring(12, 1)); j < 6; j++)
            {
                if (SkillButtonArray[i, j].Checked)
                {
                    SkillButtonArray[i, j].BackgroundImage = SkillImgSBG[3];
                }

            }
        }

        private void ADRbutton_MouseEnter(object sender, EventArgs e)
        {
            CheckBox thisbutton = sender as CheckBox;

            thisbutton.BackgroundImage = SkillImgSBG[1];
        }

        private void ADRbutton_MouseLeave(object sender, EventArgs e)
        {
            CheckBox thisbutton = sender as CheckBox;
            if (thisbutton.Checked)
                thisbutton.BackgroundImage = SkillImgSBG[3];
            else
                thisbutton.BackgroundImage = SkillImgSBG[0];
        }

        private void ADRbutton_Click(object sender, EventArgs e)
        {
            CheckBox thisbutton = sender as CheckBox;

            if (thisbutton.Checked)
            {
                char[] ADR = Convert.ToString(PlayerDataV.PlayerSkills[0], 2).PadLeft(6, '0').ToCharArray();
                ADR[byte.Parse(thisbutton.Name.Substring(9, 1))] = '1';

                PlayerDataV.PlayerSkills[0] = Convert.ToByte(new string(ADR), 2);
                thisbutton.BackgroundImage = SkillImgSBG[1];
            }
            else
            {
                char[] ADR = Convert.ToString(PlayerDataV.PlayerSkills[0], 2).PadLeft(6, '0').ToCharArray();
                ADR[byte.Parse(thisbutton.Name.Substring(9, 1))] = '0';
                string temp = new string(ADR);
                PlayerDataV.PlayerSkills[0] = Convert.ToByte(temp.PadLeft(8, '0'), 2);
                thisbutton.BackgroundImage = SkillImgSBG[1];
            }
        }

        private void ADRbutton_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox thisbutton = sender as CheckBox;

            if (thisbutton.Checked)
            {
                thisbutton.BackgroundImage = SkillImgSBG[3];
                thisbutton.Image = ADRImgS[int.Parse(thisbutton.Name.Substring(9))];
            }
            else
            {
                thisbutton.BackgroundImage = SkillImgSBG[0];
                thisbutton.Image = ADRImgSGrey[int.Parse(thisbutton.Name.Substring(9))];
            }
        }

        private void ADRbutton_MouseHover(object sender, EventArgs e)
        {
            CheckBox thisbutton = sender as CheckBox;

            if (thisbutton.Checked)
            {
                thisbutton.BackgroundImage = SkillImgSBG[1];
                thisbutton.Image = ADRImgS[int.Parse(thisbutton.Name.Substring(9))];
            }
            else
            {
                thisbutton.BackgroundImage = SkillImgSBG[1];
                thisbutton.Image = ADRImgSGrey[int.Parse(thisbutton.Name.Substring(9))];
            }
        }

        private void ClearProfilePage()
        {
            foreach (CheckBox temp in ADRbuttonArray)
                temp.Checked = false;

            foreach (CheckBox temp in SkillButtonArray)
                temp.Checked = false;
        }
        //end Skill buttons
        //end Profile tab

        //User Trucks tab
        private void CreateTruckPanelControls()
        {
            CreateTruckPanelProgressBars();
        }

        private void CreateTruckPanelProgressBars()
        {
            int pHeight = RepairImg.Height, pOffset = 5, lOffset = 60, pSizeW = 300;
            int pSkillsNameHeight = 32, pSkillsNameWidth = 32, pSkillsNameOffset = 5, pSkillsNamelOffset = 12;

            string[] toolskillimgtooltip = new string[] { "Engine", "Transmission", "Chassis", "Cabin", "Wheels" };
            Label slabel, labelpartName;
            Panel Ppanel;

            for (int i = 0; i < 5; i++)
            {
                slabel = new Label();
                groupBoxUserTruckTruckDetails.Controls.Add(slabel);
                slabel.Name = "labelTruckPartName" + toolskillimgtooltip[i];
                slabel.Location = new Point(pSkillsNamelOffset, 23 + (pSkillsNameHeight + pSkillsNameOffset * 3) * i);
                slabel.Text = toolskillimgtooltip[i];
                slabel.AutoSize = true;

                labelpartName = new Label();
                groupBoxUserTruckTruckDetails.Controls.Add(labelpartName);
                labelpartName.Name = "labelTruckPartDataName" + i;
                labelpartName.Location = new Point(lOffset + pSizeW / 2, 23 + (pSkillsNameHeight + pSkillsNameOffset * 3) * i);
                labelpartName.Text = "";
                labelpartName.AutoSize = true;

                Panel imgpanel = new Panel();
                groupBoxUserTruckTruckDetails.Controls.Add(imgpanel);

                imgpanel.Parent = groupBoxUserTruckTruckDetails;
                imgpanel.Location = new Point(pSkillsNamelOffset, 23 + 14 + (pSkillsNameHeight + pSkillsNameOffset * 3) * i);
                imgpanel.BorderStyle = BorderStyle.None;
                imgpanel.Size = new Size(pSkillsNameWidth, pSkillsNameHeight);
                imgpanel.Name = "TruckPartImg" + i.ToString();

                Bitmap bgimg = new Bitmap(TruckPartsImg[i], pSkillsNameHeight, pSkillsNameWidth);
                imgpanel.BackgroundImage = bgimg;

                //Panel 
                Ppanel = new Panel();
                groupBoxUserTruckTruckDetails.Controls.Add(Ppanel);

                Ppanel.Parent = groupBoxUserTruckTruckDetails;
                Ppanel.Location = new Point(lOffset, imgpanel.Location.Y + pOffset);
                Ppanel.BorderStyle = BorderStyle.FixedSingle;
                Ppanel.Size = new Size(pSizeW, 24);
                Ppanel.Name = "progressbarTruckPart" + i.ToString();
                
                Button button = new Button();
                groupBoxUserTruckTruckDetails.Controls.Add(button);

                button.Parent = groupBoxUserTruckTruckDetails;
                button.Location = new Point(Ppanel.Location.X + Ppanel.Width + pOffset, imgpanel.Location.Y);
                button.FlatStyle = FlatStyle.Flat;
                button.Size = new Size(RepairImg.Height, RepairImg.Height);
                button.Name = "buttonTruckElRepair" + i.ToString();
                button.BackgroundImage = RepairImg;
                button.BackgroundImageLayout = ImageLayout.Zoom;
                button.Text = "";
                button.FlatAppearance.BorderSize = 0;
                button.Click += new EventHandler(buttonElRepair_Click);
            }

            Panel Ppanelf = new Panel();
            groupBoxUserTruckTruckDetails.Controls.Add(Ppanelf);
            Ppanelf.Parent = groupBoxUserTruckTruckDetails;
            Ppanelf.Location = new Point(lOffset + pSizeW + pOffset * 2 + RepairImg.Width, 23 + 14);
            Ppanelf.BorderStyle = BorderStyle.FixedSingle;
            Ppanelf.Size = new Size(50, 220);
            Ppanelf.Name = "progressbarTruckFuel";

            slabel = new Label();
            groupBoxUserTruckTruckDetails.Controls.Add(slabel);
            slabel.Name = "labelTruckDetailsFuel";
            slabel.Text = "Fuel";
            slabel.AutoSize = true;
            slabel.Location = new Point(Ppanelf.Location.X + (Ppanelf.Width - slabel.Width) / 2, Ppanelf.Location.Y + Ppanelf.Height + 10);

            CreateTruckPanelButtons();

        }

        private void CreateTruckPanelButtons()
        {
            int pHeight = RepairImg.Height, pOffset = 5, tOffset = comboBoxUserTruckCompanyTrucks.Location.Y;
            int topbutoffset = comboBoxUserTruckCompanyTrucks.Location.X + comboBoxUserTruckCompanyTrucks.Width + pOffset;

            Button buttonInfo = new Button();
            tableLayoutPanel8.Controls.Add(buttonInfo, 0, 1);
            buttonInfo.FlatStyle = FlatStyle.Flat;
            buttonInfo.Size = new Size(CutomizeImg.Width, CutomizeImg.Height);
            buttonInfo.Name = "buttonTruckInfo";
            buttonInfo.BackgroundImage = ConvertBitmapToGrayscale(CutomizeImg);
            buttonInfo.BackgroundImageLayout = ImageLayout.Zoom;
            buttonInfo.Text = "";
            buttonInfo.FlatAppearance.BorderSize = 0;
            buttonInfo.Enabled = false;
            buttonInfo.Dock = DockStyle.Fill;

            Button buttonR = new Button();
            tableLayoutPanel8.Controls.Add(buttonR, 3, 1);            
            buttonR.FlatStyle = FlatStyle.Flat;
            buttonR.Size = new Size(RepairImg.Height, RepairImg.Height);
            buttonR.Name = "buttonTruckRepair";
            buttonR.BackgroundImage = RepairImg;
            buttonR.BackgroundImageLayout = ImageLayout.Zoom;
            buttonR.Text = "";
            buttonR.FlatAppearance.BorderSize = 0;
            buttonR.Click += new EventHandler(buttonTruckRepair_Click);
            buttonR.Dock = DockStyle.Fill;

            Button buttonF = new Button();
            tableLayoutPanel8.Controls.Add(buttonF, 4, 1);
            buttonF.FlatStyle = FlatStyle.Flat;
            buttonF.Size = new Size(RepairImg.Height, RepairImg.Height);
            buttonF.Name = "buttonTruckReFuel";
            buttonF.BackgroundImage = RefuelImg;
            buttonF.BackgroundImageLayout = ImageLayout.Zoom;
            buttonF.Text = "";
            buttonF.FlatAppearance.BorderSize = 0;
            buttonF.Click += new EventHandler(buttonTruckReFuel_Click);
            buttonF.Dock = DockStyle.Fill;

        }

        public void buttonTruckReFuel_Click(object sender, EventArgs e)
        {
            int i = 0;
            foreach (string temp in UserTruckDictionary[comboBoxUserTruckCompanyTrucks.SelectedValue.ToString()].Parts.Find(x => x.PartType == "truckdata").PartData)
            {
                if (temp.StartsWith(" fuel_relative:"))
                {
                    UserTruckDictionary[comboBoxUserTruckCompanyTrucks.SelectedValue.ToString()].Parts.Find(x => x.PartType == "truckdata").PartData[i] = " fuel_relative: 1";
                    break;
                }
                i++;
            }
            UpdateTruckPanelProgressBars();
        }

        public void buttonTruckRepair_Click(object sender, EventArgs e)
        {
            string[] PartList = { "engine", "transmission", "chassis", "cabin", "tire" };

            foreach (string tempPart in PartList)
            {
                foreach (UserCompanyTruckDataPart temp in UserTruckDictionary[comboBoxUserTruckCompanyTrucks.SelectedValue.ToString()].Parts.FindAll(x => x.PartType == tempPart))
                {
                    string partNameless = temp.PartNameless;

                    int i = 0;

                    foreach (string temp2 in temp.PartData)
                    {
                        if (temp2.StartsWith(" wear:"))
                        {
                            UserTruckDictionary[comboBoxUserTruckCompanyTrucks.SelectedValue.ToString()].Parts.Find(x => x.PartNameless == partNameless).PartData[i] = " wear: 0";
                            break;
                        }
                        i++;
                    }
                }
            }

            UpdateTruckPanelProgressBars();
        }

        public void buttonElRepair_Click(object sender, EventArgs e)
        {
            Button curbtn = sender as Button;
            int bi = Convert.ToByte(curbtn.Name.Substring(19));

            string[] PartList = { "engine", "transmission", "chassis", "cabin", "tire" };

            foreach (UserCompanyTruckDataPart temp in UserTruckDictionary[comboBoxUserTruckCompanyTrucks.SelectedValue.ToString()].Parts.FindAll(x => x.PartType == PartList[bi]))
            {
                string partNameless = temp.PartNameless;

                int i = 0;

                foreach (string temp2 in temp.PartData)
                {
                    if (temp2.StartsWith(" wear:"))
                    {
                        UserTruckDictionary[comboBoxUserTruckCompanyTrucks.SelectedValue.ToString()].Parts.Find(x => x.PartNameless == partNameless).PartData[i] = " wear: 0";
                        break;
                    }
                    i++;
                }
            }

            UpdateTruckPanelProgressBars();
        }

        private void UpdateTruckPanelProgressBars()
        {
            UserTruckDictionary.TryGetValue(comboBoxUserTruckCompanyTrucks.SelectedValue.ToString(), out UserCompanyTruckData SelectedUserCompanyTruck);

            for (int i = 0; i < 5; i++)
            {
                Panel pnl = null;
                Label labelPart = null;

                string pnlname = "progressbarTruckPart" + i.ToString(), labelPartName = "labelTruckPartDataName" + i.ToString();

                if (groupBoxUserTruckTruckDetails.Controls.ContainsKey(pnlname))
                {
                    pnl = groupBoxUserTruckTruckDetails.Controls[pnlname] as Panel;
                }

                if (groupBoxUserTruckTruckDetails.Controls.ContainsKey(labelPartName))
                {
                    labelPart = groupBoxUserTruckTruckDetails.Controls[labelPartName] as Label;
                }

                if (pnl != null)
                {
                    List<string> TruckDataPart = null;

                    switch (i)
                    {
                        case 0:
                            TruckDataPart = SelectedUserCompanyTruck.Parts.Find(xp => xp.PartType == "engine").PartData;
                            break;
                        case 1:
                            TruckDataPart = SelectedUserCompanyTruck.Parts.Find(xp => xp.PartType == "transmission").PartData;
                            break;
                        case 2:
                            TruckDataPart = SelectedUserCompanyTruck.Parts.Find(xp => xp.PartType == "chassis").PartData;
                            break;
                        case 3:
                            TruckDataPart = SelectedUserCompanyTruck.Parts.Find(xp => xp.PartType == "cabin").PartData;
                            break;
                        case 4:
                            TruckDataPart = SelectedUserCompanyTruck.Parts.Find(xp => xp.PartType == "tire").PartData;
                            break;
                    }
                    string wear = "0";
                    if (TruckDataPart != null)
                    {
                        if (labelPart != null)
                        {
                            labelPart.Text = TruckDataPart.Find(xl => xl.StartsWith(" data_path:")).Split(new char[] { '"' })[1].Split(new char[] { '/' }).Last().Split(new char[] { '.' })[0];
                        }

                        wear = TruckDataPart.Find(xl => xl.StartsWith(" wear:")).Split(new char[] { ' ' })[2];
                    }
                    else
                    {
                        labelPart.Text = "!! Part not found !!";
                    }

                    labelPart.Location = new Point(pnl.Location.X + (pnl.Width - labelPart.Width) / 2, labelPart.Location.Y);

                    decimal _wear = 0;

                    if (wear != "0" && wear != "1")
                        _wear = HexFloatToDecimalFloat(wear);
                    else
                    if (wear == "1")
                        _wear = 1;

                    SolidBrush ppen = new SolidBrush(GetProgressbarColor(_wear));

                    int x = 0, y = 0, pnlwidth = (int)(pnl.Width * (1 - _wear));

                    Bitmap progress = new Bitmap(pnl.Width, pnl.Height);

                    Graphics g = Graphics.FromImage(progress);
                    g.FillRectangle(ppen, x, y, pnlwidth, pnl.Height);

                    int fontSize = 12;
                    StringFormat sf = new StringFormat();
                    sf.LineAlignment = StringAlignment.Center;
                    sf.Alignment = StringAlignment.Center;

                    GraphicsPath p = new GraphicsPath();
                    p.AddString(
                        ((int)((1 - _wear) * 100)).ToString() + " %",   // text to draw
                        FontFamily.GenericSansSerif,                    // or any other font family
                        (int)FontStyle.Bold,                            // font style (bold, italic, etc.)
                        g.DpiY * fontSize / 72,                         // em size
                        new Rectangle(0, 0, pnl.Width, pnl.Height),     // location where to draw text
                        sf);                                            // set options here (e.g. center alignment)
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.FillPath(Brushes.Black, p);
                    g.DrawPath(Pens.Black, p);

                    pnl.BackgroundImage = progress;
                }
            }

            Panel pnlfuel = null;
            string pnlnamefuel = "progressbarTruckFuel";
            if (groupBoxUserTruckTruckDetails.Controls.ContainsKey(pnlnamefuel))
            {
                pnlfuel = groupBoxUserTruckTruckDetails.Controls[pnlnamefuel] as Panel;
            }

            if (pnlfuel != null)
            {
                string fuel = SelectedUserCompanyTruck.Parts.Find(xp => xp.PartType == "truckdata").PartData.Find(xl => xl.StartsWith(" fuel_relative:")).Split(new char[] { ' ' })[2];//SelectedUserCompanyTruck.Fuel;
                decimal _fuel = 0;

                if (fuel != "0" && fuel != "1")
                    _fuel = HexFloatToDecimalFloat(fuel);
                else
                if (fuel == "1")
                    _fuel = 1;

                SolidBrush ppen = new SolidBrush(GetProgressbarColor(1 - _fuel));
                int pnlheight = (int)(pnlfuel.Height * (_fuel)), x = 0, y = pnlfuel.Height - pnlheight;

                Bitmap progress = new Bitmap(pnlfuel.Width, pnlfuel.Height);

                Graphics g = Graphics.FromImage(progress);
                g.FillRectangle(ppen, x, y, pnlfuel.Width, pnlheight);

                int fontSize = 10;
                StringFormat sf = new StringFormat();
                sf.LineAlignment = StringAlignment.Center;
                sf.Alignment = StringAlignment.Center;

                GraphicsPath p = new GraphicsPath();
                p.AddString(
                    ((int)(_fuel * 100)).ToString() + " %",             // text to draw
                    FontFamily.GenericSansSerif,                        // or any other font family
                    (int)FontStyle.Regular,                             // font style (bold, italic, etc.)
                    g.DpiY * fontSize / 72,                             // em size
                    new Rectangle(0, 0, pnlfuel.Width, pnlfuel.Height), // location where to draw text
                    sf);                                                // set options here (e.g. center alignment)
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.FillPath(Brushes.Black, p);
                g.DrawPath(Pens.Black, p);

                pnlfuel.BackgroundImage = progress;
            }

            string lctxt = "";
            labelLicensePlate.Text = "";
            int stindex = 0, endindex = 0;

            string LicensePlate = SelectedUserCompanyTruck.Parts.Find(xp => xp.PartType == "truckdata").PartData.Find(xl => xl.StartsWith(" license_plate:")).Split(new char[] { '"' })[1];

            for (int i = 0; i < LicensePlate.Length; i++)
            {
                if (LicensePlate[i] == '<')
                {
                    endindex = i;
                    lctxt += LicensePlate.Substring(stindex, endindex - stindex);
                }
                else if (LicensePlate[i] == '>')
                {
                    stindex = i + 1;
                }
                else if (i == LicensePlate.Length - 1)
                {
                    endindex = i + 1;
                    lctxt += LicensePlate.Substring(stindex, endindex - stindex);
                }
            }
            if (lctxt.Split(new char[] { '|' }).Length > 1)
                labelLicensePlate.Text = lctxt.Split(new char[] { '|' })[0] + " Country " + lctxt.Split(new char[] { '|' })[1];
            else
                labelLicensePlate.Text = lctxt.Split(new char[] { '|' })[0];
        }

        private void FillUserCompanyTrucksList()
        {
            DataTable combDT = new DataTable();
            DataColumn dc = new DataColumn("UserTruckNameless", typeof(string));
            combDT.Columns.Add(dc);

            dc = new DataColumn("UserTruckName", typeof(string));
            combDT.Columns.Add(dc);

            foreach (KeyValuePair<string, UserCompanyTruckData> UserTruck in UserTruckDictionary)
            {
                string truckname = "";
                try
                {
                    string templine = UserTruck.Value.Parts.Find(x => x.PartType == "truckbrandname").PartData.Find(xline => xline.StartsWith(" data_path:"));
                    truckname = templine.Split(new char[] { '"' })[1].Split(new char[] { '/' })[4];
                }
                catch { }
                TruckBrandsLngDict.TryGetValue(truckname, out string trucknamevalue);

                string TruckName = "";

                if (UserTruckDictionary[UserTruck.Key].Users)
                    TruckName = "[U] ";
                else
                    TruckName = "[Q] ";

                if (trucknamevalue != null && trucknamevalue != "")
                {
                    TruckName += trucknamevalue;
                    combDT.Rows.Add(UserTruck.Key, TruckName);
                }
                else
                {
                    TruckName += truckname;
                    combDT.Rows.Add(UserTruck.Key, TruckName);
                }
            }
            //combDT.DefaultView.Sort = "UserTruckName ASC";
            comboBoxUserTruckCompanyTrucks.ValueMember = "UserTruckNameless";
            comboBoxUserTruckCompanyTrucks.DisplayMember = "UserTruckName";
            comboBoxUserTruckCompanyTrucks.DataSource = combDT;
            comboBoxUserTruckCompanyTrucks.SelectedValue = PlayerDataV.UserCompanyAssignedTruck;
        }

        private void comboBoxCompanyTrucks_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmbbx = sender as ComboBox;

            if (cmbbx.SelectedIndex != -1)
            {
                UpdateTruckPanelProgressBars();
            }
        }

        private void buttonUserTruckSelectCurrent_Click(object sender, EventArgs e)
        {
            comboBoxUserTruckCompanyTrucks.SelectedValue = PlayerDataV.UserCompanyAssignedTruck;
        }

        private void buttonUserTruckSwitchCurrent_Click(object sender, EventArgs e)
        {
            PlayerDataV.UserCompanyAssignedTruck = comboBoxUserTruckCompanyTrucks.SelectedValue.ToString();
        }

        //Share buttons
        private void buttonTruckPaintCopy_Click(object sender, EventArgs e)
        {
            string tempPaint = "TruckPaint\r\n";

            List<string> paintstr = UserTruckDictionary[comboBoxUserTruckCompanyTrucks.SelectedValue.ToString()].Parts.Find(xp => xp.PartType == "paintjob").PartData;

            foreach (string temp in paintstr)
            {
                tempPaint += temp + "\r\n";
            }

            string asd = BitConverter.ToString(zipText(tempPaint)).Replace("-", "");
            Clipboard.SetText(asd);
            MessageBox.Show("Paint data has been copied.");
        }

        private void buttonTruckPaintPaste_Click(object sender, EventArgs e)
        {
            try
            {
                string inputData = unzipText(Clipboard.GetText());
                string[] Lines = inputData.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                if (Lines[0] == "TruckPaint")
                {
                    List<string> paintstr = new List<string>();
                    for (int i = 1; i < Lines.Length; i++)
                    {
                        paintstr.Add(Lines[i]);
                    }

                    UserTruckDictionary[comboBoxUserTruckCompanyTrucks.SelectedValue.ToString()].Parts.Find(xp => xp.PartType == "paintjob").PartData = paintstr;

                    MessageBox.Show("Paint data  has been inserted.");
                }
                else
                    MessageBox.Show("Wrong data. Expected Paint data but\r\n" + Lines[0] + "\r\nwas found.");
            }
            catch
            {
                MessageBox.Show("Something gone wrong with decoding.");
            }
        }
        //end Share buttons
        //end User Trucks tab

        //User Trailer tab
        private void CreateTrailerPanelControls()
        {
            CreateTrailerPanelProgressBars();
        }

        private void CreateTrailerPanelProgressBars()
        {
            int pHeight = RepairImg.Height, pOffset = 5, lOffset = 60, pSizeW = 300;
            int pSkillsNameHeight = 32, pSkillsNameWidth = 32, pSkillsNameOffset = 5, pSkillsNamelOffset = 12;

            string[] toolskillimgtooltip = new string[] { "Cargo", "Body", "Chassis", "Wheels" };
            Label slabel;
            Panel Ppanel;

            for (int i = 0; i < 4; i++)
            {
                slabel = new Label();
                groupBoxUserTrailerTrailerDetails.Controls.Add(slabel);
                slabel.Name = "labelTrailerPartName" + toolskillimgtooltip[i];
                slabel.Location = new Point(pSkillsNamelOffset, 23 + (pSkillsNameHeight + pSkillsNameOffset * 3) * i);
                slabel.Text = toolskillimgtooltip[i];
                slabel.AutoSize = true;

                Panel imgpanel = new Panel();
                groupBoxUserTrailerTrailerDetails.Controls.Add(imgpanel);

                imgpanel.Parent = groupBoxUserTrailerTrailerDetails;
                imgpanel.Location = new Point(pSkillsNamelOffset, 23 + 14 + (pSkillsNameHeight + pSkillsNameOffset * 3) * i);
                imgpanel.BorderStyle = BorderStyle.None;
                imgpanel.Size = new Size(pSkillsNameWidth, pSkillsNameHeight);
                imgpanel.Name = "TrailerPartImg" + i.ToString();

                Bitmap bgimg = new Bitmap(TrailerPartsImg[i], pSkillsNameHeight, pSkillsNameWidth);
                imgpanel.BackgroundImage = bgimg;

                //Panel 
                Ppanel = new Panel();
                groupBoxUserTrailerTrailerDetails.Controls.Add(Ppanel);

                Ppanel.Parent = groupBoxUserTrailerTrailerDetails;
                Ppanel.Location = new Point(lOffset, imgpanel.Location.Y);
                Ppanel.BorderStyle = BorderStyle.FixedSingle;
                Ppanel.Size = new Size(pSizeW, RepairImg.Height);
                Ppanel.Name = "progressbarTrailerPart" + i.ToString();

                Button button = new Button();
                groupBoxUserTrailerTrailerDetails.Controls.Add(button);

                button.Parent = groupBoxUserTrailerTrailerDetails;
                button.Location = new Point(Ppanel.Location.X + Ppanel.Width + pOffset, Ppanel.Location.Y);
                button.FlatStyle = FlatStyle.Flat;
                button.Size = new Size(RepairImg.Height, RepairImg.Height);
                button.Name = "buttonTrailerElRepair" + i.ToString();
                button.BackgroundImage = RepairImg;
                button.BackgroundImageLayout = ImageLayout.Zoom;
                button.Text = "";
                button.FlatAppearance.BorderSize = 0;
                button.Click += new EventHandler(buttonTrailerElRepair_Click);
            }

            CreateTrailerPanelButtons();
        }

        private void CreateTrailerPanelButtons()
        {
            int pHeight = RepairImg.Height, pOffset = 5, tOffset = comboBoxUserTrailerCompanyTrailers.Location.Y;
            int topbutoffset = comboBoxUserTrailerCompanyTrailers.Location.X + comboBoxUserTrailerCompanyTrailers.Width + pOffset;

            //tableLayoutPanel13

            Button buttonR = new Button();
            //tabPageTrailer.Controls.Add(buttonR);
            tableLayoutPanel13.Controls.Add(buttonR, 3, 1);
            buttonR.Location = new Point(topbutoffset, tOffset);
            buttonR.FlatStyle = FlatStyle.Flat;
            buttonR.Size = new Size(RepairImg.Height, RepairImg.Height);
            buttonR.Name = "buttonTrailerRepair";
            buttonR.BackgroundImage = RepairImg;
            buttonR.BackgroundImageLayout = ImageLayout.Zoom;
            buttonR.Text = "";
            buttonR.FlatAppearance.BorderSize = 0;
            buttonR.Click += new EventHandler(buttonTrailerRepair_Click);
            buttonR.Dock = DockStyle.Fill;

            Button buttonInfo = new Button();
            //tabPageTrailer.Controls.Add(buttonInfo);
            tableLayoutPanel13.Controls.Add(buttonInfo, 0, 1);
            //buttonInfo.Location = new Point(labelUserTrailerTrailer.Location.X + (comboBoxUserTrailerCompanyTrailers.Location.X - labelUserTrailerTrailer.Location.X - CutomizeImg.Width - pOffset) / 2, buttonUserTruckSelectCurrent.Location.Y + pOffset);
            buttonInfo.FlatStyle = FlatStyle.Flat;
            buttonInfo.Size = new Size(CutomizeImg.Width, CutomizeImg.Height);
            buttonInfo.Name = "buttonTruckInfo";
            buttonInfo.BackgroundImage = ConvertBitmapToGrayscale(CutomizeImg);
            buttonInfo.BackgroundImageLayout = ImageLayout.Zoom;
            buttonInfo.Text = "";
            buttonInfo.FlatAppearance.BorderSize = 0;
            buttonInfo.Enabled = false;
            buttonInfo.Dock = DockStyle.Fill;
        }

        public void buttonTrailerRepair_Click(object sender, EventArgs e)
        {
            string[] PartList = { "trailerdata", "body", "chassis", "tire" };
            string trailerNameless = "";

            trailerNameless = comboBoxUserTrailerCompanyTrailers.SelectedValue.ToString();

            StartTrailerParts:

            foreach (string tempPart in PartList)
            {
                foreach (UserCompanyTruckDataPart temp in UserTrailerDictionary[trailerNameless].Parts.FindAll(x => x.PartType == tempPart))
                {
                    string partNameless = temp.PartNameless;

                    int i = 0;

                    foreach (string temp2 in temp.PartData)
                    {
                        if (temp2.StartsWith(" wear:"))
                        {
                            UserTrailerDictionary[trailerNameless].Parts.Find(x => x.PartNameless == partNameless).PartData[i] = " wear: 0";
                            break;
                        }
                        else
                        if (temp2.StartsWith(" cargo_damage:"))
                        {
                            UserTrailerDictionary[trailerNameless].Parts.Find(x => x.PartType == "trailerdata").PartData[i] = " cargo_damage: 0";
                            break;
                        }
                        i++;
                    }
                }
            }

            UserCompanyTruckDataPart slavetrailer = UserTrailerDictionary[trailerNameless].Parts.Find(x => x.PartType == "slavetrailer");

            if (slavetrailer != null)
            {
                trailerNameless = slavetrailer.PartNameless;
                goto StartTrailerParts;
            }

            UpdateTrailerPanelProgressBars();
        }

        public void buttonTrailerElRepair_Click(object sender, EventArgs e)
        {
            Button curbtn = sender as Button;
            int bi = Convert.ToByte(curbtn.Name.Substring(21));

            string[] PartList = { "trailerdata", "body", "chassis", "tire" };
            string trailerNameless = "";

            trailerNameless = comboBoxUserTrailerCompanyTrailers.SelectedValue.ToString();

            StartTrailerParts:

            foreach (UserCompanyTruckDataPart temp in UserTrailerDictionary[trailerNameless].Parts.FindAll(x => x.PartType == PartList[bi]))
            {
                string partNameless = temp.PartNameless;

                int i = 0;

                foreach (string temp2 in temp.PartData)
                {
                    if (temp2.StartsWith(" wear:"))
                    {
                        UserTrailerDictionary[trailerNameless].Parts.Find(x => x.PartNameless == partNameless).PartData[i] = " wear: 0";
                        break;
                    }
                    else
                    if (temp2.StartsWith(" cargo_damage:"))
                    {
                        UserTrailerDictionary[trailerNameless].Parts.Find(x => x.PartType == "trailerdata").PartData[i] = " cargo_damage: 0";
                        break;
                    }
                    i++;
                }
            }

            UserCompanyTruckDataPart slavetrailer = UserTrailerDictionary[trailerNameless].Parts.Find(x => x.PartType == "slavetrailer");
            
            if (slavetrailer != null)
            {
                trailerNameless = slavetrailer.PartNameless;
                goto StartTrailerParts;
            }

            UpdateTrailerPanelProgressBars();
        }

        private void UpdateTrailerPanelProgressBars()
        {
            UserTrailerDictionary.TryGetValue(comboBoxUserTrailerCompanyTrailers.SelectedValue.ToString(), out UserCompanyTruckData SelectedUserCompanyTrailer);

            for (int i = 0; i < 4; i++)
            {
                Panel pnl = null;
                string pnlname = "progressbarTrailerPart" + i.ToString();
                if (groupBoxUserTrailerTrailerDetails.Controls.ContainsKey(pnlname))
                {
                    pnl = groupBoxUserTrailerTrailerDetails.Controls[pnlname] as Panel;
                }

                if (pnl != null)
                {
                    UserCompanyTruckDataPart tempPart = null;

                    switch (i)
                    {
                        case 0:
                            tempPart = SelectedUserCompanyTrailer.Parts.Find(xp => xp.PartType == "trailerdata");
                            break;
                        case 1:
                            tempPart = SelectedUserCompanyTrailer.Parts.Find(xp => xp.PartType == "body");
                            break;
                        case 2:
                            tempPart = SelectedUserCompanyTrailer.Parts.Find(xp => xp.PartType == "chassis");
                            break;
                        case 3:
                            tempPart = SelectedUserCompanyTrailer.Parts.Find(xp => xp.PartType == "tire");
                            break;
                    }

                    string wear = "0";

                    try
                    {
                        List<string> TruckDataPart = tempPart.PartData;
                        wear = TruckDataPart.Find(xl => xl.StartsWith(" wear:") || xl.StartsWith(" cargo_damage:")).Split(new char[] { ' ' })[2];
                    }
                    catch
                    { }
                    
                    decimal _wear = 0;

                    if (wear != "0" && wear != "1")
                        _wear = HexFloatToDecimalFloat(wear);
                    else
                    if (wear == "1")
                        _wear = 1;

                    SolidBrush ppen = new SolidBrush(GetProgressbarColor(_wear));

                    int x = 0, y = 0, pnlwidth = (int)(pnl.Width * (1 - _wear));

                    Bitmap progress = new Bitmap(pnl.Width, pnl.Height);

                    Graphics g = Graphics.FromImage(progress);
                    g.FillRectangle(ppen, x, y, pnlwidth, pnl.Height);

                    int fontSize = 12;
                    StringFormat sf = new StringFormat();
                    sf.LineAlignment = StringAlignment.Center;
                    sf.Alignment = StringAlignment.Center;

                    GraphicsPath p = new GraphicsPath();
                    p.AddString(
                        ((int)((1 - _wear) * 100)).ToString() + " %",             // text to draw
                        FontFamily.GenericSansSerif,  // or any other font family
                        (int)FontStyle.Bold,      // font style (bold, italic, etc.)
                        g.DpiY * fontSize / 72,       // em size
                        new Rectangle(0, 0, pnl.Width, pnl.Height),              // location where to draw text
                        sf);          // set options here (e.g. center alignment)
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.FillPath(Brushes.Black, p);
                    g.DrawPath(Pens.Black, p);

                    pnl.BackgroundImage = progress;
                }
            }

            string lctxt = "";
            labelLicensePlateTr.Text = "";
            int stindex = 0, endindex = 0;

            string LicensePlate = SelectedUserCompanyTrailer.Parts.Find(xp => xp.PartType == "trailerdata").PartData.Find(xl => xl.StartsWith(" license_plate:")).Split(new char[] { '"' })[1];

            for (int i = 0; i < LicensePlate.Length; i++)//SelectedUserCompanyTruck.LicensePlate.Length; i++)
            {
                if (LicensePlate[i] == '<')
                {
                    endindex = i;
                    lctxt += LicensePlate.Substring(stindex, endindex - stindex);
                }
                else if (LicensePlate[i] == '>')
                {
                    stindex = i + 1;
                }
                else if (i == LicensePlate.Length - 1)
                {
                    endindex = i + 1;
                    lctxt += LicensePlate.Substring(stindex, endindex - stindex);
                }
            }
            if (lctxt.Split(new char[] { '|' }).Length > 1)
                labelLicensePlateTr.Text = lctxt.Split(new char[] { '|' })[0] + " Country " + lctxt.Split(new char[] { '|' })[1];
            else
                labelLicensePlateTr.Text = lctxt.Split(new char[] { '|' })[0];
        }

        private void FillUserCompanyTrailerList()
        {
            
            DataTable combDT = new DataTable();
            DataColumn dc = new DataColumn("UserTrailerkNameless", typeof(string));
            combDT.Columns.Add(dc);

            dc = new DataColumn("UserTrailerName", typeof(string));
            combDT.Columns.Add(dc);

            foreach (KeyValuePair<string, UserCompanyTruckData> UserTrailer in UserTrailerDictionary)
            {
                if (UserTrailer.Value.Main)
                {
                    string trailerdef = UserTrailerDictionary[UserTrailer.Key].Parts.Find(x => x.PartType == "trailerdef").PartNameless;

                    string trailername = "";

                    if (UserTrailerDictionary[UserTrailer.Key].Users)
                        trailername = "[U] ";
                    else
                        trailername = "[Q] ";

                    if (trailerdef.Contains("_nameless"))
                        trailername += UserTrailerDefDictionary[trailerdef].Find(x => x.StartsWith(" source_name:")).Split(new char[] { '"' })[1];
                    else
                        trailername += trailerdef;

                    combDT.Rows.Add(UserTrailer.Key, trailername);
                }
            }

            if(combDT.Rows.Count > 0)
            {
                //combDT.DefaultView.Sort = "UserTrailerName ASC";
                comboBoxUserTrailerCompanyTrailers.Enabled = true;
                comboBoxUserTrailerCompanyTrailers.ValueMember = "UserTrailerkNameless";
                comboBoxUserTrailerCompanyTrailers.DisplayMember = "UserTrailerName";
                comboBoxUserTrailerCompanyTrailers.DataSource = combDT;
                comboBoxUserTrailerCompanyTrailers.SelectedValue = PlayerDataV.UserCompanyAssignedTrailer;
            }
            else
            {
                comboBoxUserTrailerCompanyTrailers.Enabled = false;
            }
        }

        private void comboBoxCompanyTrailers_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmbbx = sender as ComboBox;

            if (cmbbx.SelectedIndex != -1)
            {
                UpdateTrailerPanelProgressBars();
            }
        }

        private void buttonUserTrailerSelectCurrent_Click(object sender, EventArgs e)
        {
            comboBoxUserTrailerCompanyTrailers.SelectedValue = PlayerDataV.UserCompanyAssignedTrailer;
        }

        private void buttonUserTrailerSwitchCurrent_Click(object sender, EventArgs e)
        {
            PlayerDataV.UserCompanyAssignedTrailer = comboBoxUserTrailerCompanyTrailers.SelectedValue.ToString();
        }

        //end User Trailer tab

        //User Company tab
        private void FillFormCompanyControls()
        {
            listBoxVisitedCities.DrawMode = DrawMode.OwnerDrawVariable;
            listBoxGarages.DrawMode = DrawMode.OwnerDrawVariable;

            FillHQcities();

            FillGaragesList(0);
            FillVisitedCities(0);
            FillAccountMoneyTB();

            textBoxUserCompanyCompanyName.Text = PlayerDataV.CompanyName;

            //FromStringToHex(PlayerDataV.CompanyName);

            MemoryStream ms = new MemoryStream();

            Bitmap temp = ImageFromDDS(@"img\" + GameType + @"\player_logo\" + PlayerDataV.CompanyLogo + ".dds");
            if(temp != null)
            {
                temp.Clone(new Rectangle(0, 0, 94, 94), temp.PixelFormat).Save(ms, ImageFormat.Png);
                PlayerCompanyLogo = Image.FromStream(ms);
            }
            else
            {
                PlayerCompanyLogo = new Bitmap(94, 94);
            }
            ms.Dispose();

            pictureBoxCompanyLogo.Image = PlayerCompanyLogo;
        }

        public void FillAccountMoneyTB()
        {
            UInt64 valueBefore = (uint)Math.Floor(PlayerDataV.AccountMoney * CurrencyDictR[ProgSettingsV.CurrencyMes]);

            string newtext = "";
            if (CurrencyDict[ProgSettingsV.CurrencyMes][0] != "")
                newtext += CurrencyDict[ProgSettingsV.CurrencyMes][0] + "-";

            newtext += CurrencyDict[ProgSettingsV.CurrencyMes][1] + String.Format(CultureInfo.CurrentCulture, "{0:N0}", valueBefore) + ",-" + CurrencyDict[ProgSettingsV.CurrencyMes][2];

            textBoxUserCompanyMoneyAccount.TextChanged -= textBoxMoneyAccount_TextChanged;
            textBoxUserCompanyMoneyAccount.Text = newtext;
            textBoxUserCompanyMoneyAccount.TextChanged += textBoxMoneyAccount_TextChanged;
        }

        private void FillHQcities()
        {
            DataTable combDT = new DataTable();
            DataColumn dc = new DataColumn("City", typeof(string));
            combDT.Columns.Add(dc);

            dc = new DataColumn("CityName", typeof(string));
            combDT.Columns.Add(dc);

            //start filling

            //fill source and destination cities
            foreach (Garages garage in from x in GaragesList where x.GarageStatus != 0 && !x.IgnoreStatus select x)
            {
                combDT.Rows.Add(garage.GarageName, garage.GarageNameTranslated);
            }

            combDT.DefaultView.Sort = "CityName ASC";
            comboBoxUserCompanyHQcity.SelectedIndexChanged -= comboBoxUserCompanyHQcity_SelectedIndexChanged;
            comboBoxUserCompanyHQcity.ValueMember = "City";
            comboBoxUserCompanyHQcity.DisplayMember = "CityName";
            comboBoxUserCompanyHQcity.BeginUpdate();
            comboBoxUserCompanyHQcity.DataSource = combDT;
            comboBoxUserCompanyHQcity.SelectedValue = PlayerDataV.HQcity;
            comboBoxUserCompanyHQcity.EndUpdate();
            comboBoxUserCompanyHQcity.SelectedIndexChanged += comboBoxUserCompanyHQcity_SelectedIndexChanged;
        }
        
        private void comboBoxUserCompanyHQcity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxUserCompanyHQcity.SelectedValue != null)
                PlayerDataV.HQcity = comboBoxUserCompanyHQcity.SelectedValue.ToString();
        }

        private void textBoxMoneyAccount_TextChanged(object sender, EventArgs e)
        {
            TextBox textBoxAccountMoney = sender as TextBox;

            string newtext = "";

            if (!string.IsNullOrEmpty(textBoxAccountMoney.Text))
            {
                int testV = textBoxAccountMoney.SelectionStart;

                string onlyDigits = new string(textBoxAccountMoney.Text.Where(c => char.IsDigit(c)).ToArray());
                if (!UInt64.TryParse(onlyDigits, NumberStyles.AllowThousands, CultureInfo.CurrentCulture, out ulong valueBefore))
                {
                    valueBefore = UInt64.MaxValue;
                }
                //[sign1] - [sign2] 1.234,- [sign3]
                if (CurrencyDict[ProgSettingsV.CurrencyMes][0] != "")
                    newtext += CurrencyDict[ProgSettingsV.CurrencyMes][0] + "-";

                newtext += CurrencyDict[ProgSettingsV.CurrencyMes][1] + String.Format(CultureInfo.CurrentCulture, "{0:N0}", valueBefore) + ",-" + CurrencyDict[ProgSettingsV.CurrencyMes][2];

                int cSpace1 = textBoxAccountMoney.Text.Substring(0, testV).Count(Char.IsWhiteSpace);

                textBoxAccountMoney.TextChanged -= textBoxMoneyAccount_TextChanged;
                textBoxAccountMoney.Text = newtext;
                textBoxAccountMoney.TextChanged += textBoxMoneyAccount_TextChanged;

                int cSpace2 = textBoxAccountMoney.Text.Substring(0, testV).Count(Char.IsWhiteSpace);

                textBoxAccountMoney.SelectionStart = testV + cSpace2 - cSpace1;

                PlayerDataV.AccountMoney = (uint)Math.Round(valueBefore / CurrencyDictR[ProgSettingsV.CurrencyMes]);
            }
        }

        private void textBoxMoneyAccount_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox textBoxAccountMoney = sender as TextBox;
            UInt64 valueBefore = 0;
            string onlyDigits = new string(textBoxAccountMoney.Text.Where(c => char.IsDigit(c)).ToArray());

            int testV = textBoxAccountMoney.SelectionStart;

            if (!string.IsNullOrEmpty(textBoxAccountMoney.Text))
            {
                if (!Char.IsDigit(e.KeyChar))
                {
                    if(e.KeyChar == (char)Keys.Back)
                    {
                        return;
                    }

                    if(UInt64.TryParse(onlyDigits, NumberStyles.AllowThousands, CultureInfo.CurrentCulture, out valueBefore))
                    {
                        textBoxAccountMoney.Text = valueBefore.ToString();
                        e.Handled = true;
                    }
                    else
                    {
                        valueBefore = UInt64.MaxValue;
                        textBoxAccountMoney.Text = valueBefore.ToString();
                        e.Handled = true;
                    }
                }
            }
        }
        //Visited cities
        //Fill
        public void FillVisitedCities(int _vindex)
        {
            listBoxVisitedCities.BeginUpdate();
            listBoxVisitedCities.Items.Clear();

            if (CitiesList.Count <= 0)
                return;

            foreach (City vc in from x in CitiesList where !x.Disabled select x)
            {
                listBoxVisitedCities.Items.Add(vc);
            }

            listBoxVisitedCities.TopIndex = _vindex;
            listBoxVisitedCities.EndUpdate();
        }
        //Draw
        private int VisitedCitiesItemMargin = 3;
        private const float VisitedCitiesPictureHeight = 32;

        private void listBoxVisitedCities_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            // Get the ListBox and the item.
            e.ItemHeight = (int)(VisitedCitiesPictureHeight + 2 * VisitedCitiesItemMargin);
        }

        private void listBoxVisitedCities_DrawItem(object sender, DrawItemEventArgs e)
        {
            // Get the ListBox and the item.
            ListBox lst = sender as ListBox;
            City vc = (City)lst.Items[e.Index];

            // Draw the background.
            e.DrawBackground();

            int index = 0;
            if (vc.Visited)
                index = 1;

            Image cityicon = CitiesImg[index];

            // Draw the picture.
            float scale = VisitedCitiesPictureHeight / cityicon.Height;
            RectangleF source_rect = new RectangleF(0, 0, cityicon.Width, cityicon.Height);

            float picture_width = scale * cityicon.Width;

            RectangleF dest_rect = new RectangleF(e.Bounds.Left + VisitedCitiesItemMargin, e.Bounds.Top + VisitedCitiesItemMargin, picture_width, VisitedCitiesPictureHeight);
            e.Graphics.DrawImage(cityicon, dest_rect, source_rect, GraphicsUnit.Pixel);
            ////

            // See if the item is selected.
            Brush br;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                br = SystemBrushes.HighlightText;
            else
                br = new SolidBrush(e.ForeColor);

            // Find the area in which to put the text.
            float x = e.Bounds.Left + picture_width + 3 * VisitedCitiesItemMargin;
            float y = e.Bounds.Top + VisitedCitiesItemMargin * 2;
            float width = e.Bounds.Right - VisitedCitiesItemMargin - x;
            float height = e.Bounds.Bottom - VisitedCitiesItemMargin - y;
            RectangleF layout_rect = new RectangleF(x, y, width, height);

            // Draw the text.
            string txt = "";//, DisplayCityName = "";

            //CitiesLngDict.TryGetValue(vc.CityName, out string value);
            /*
            DisplayCityName = vc.CityNameTranslated;

            if (DisplayCityName != null && DisplayCityName != "")
                txt = DisplayCityName;
            else
            {
                txt = vc.CityName + " -nt";
            }
            */
            txt = vc.CityNameTranslated;
            e.Graphics.DrawString(txt, Font, br, layout_rect);

            // Draw the focus rectangle if appropriate.
            e.DrawFocusRectangle();
        }
        //Buttons
        private void buttonCitiesVisit_Click(object sender, EventArgs e)
        {
            if (listBoxVisitedCities.SelectedItems.Count == 0)
            {
                foreach (City city in listBoxVisitedCities.Items)
                {
                    if (!city.Visited)
                        city.Visited = true;
                }
            }
            else
                foreach (City city in listBoxVisitedCities.SelectedItems)
                {
                    if (!city.Visited)
                        city.Visited = true;
                }


            FillVisitedCities(listBoxVisitedCities.TopIndex);
        }

        private void buttonCitiesUnVisit_Click(object sender, EventArgs e)
        {
            if (listBoxVisitedCities.SelectedItems.Count == 0)
            {
                foreach (City city in listBoxVisitedCities.Items)
                {
                    if (city.Visited)
                        city.Visited = false;
                }
            }
            else
                foreach (City city in listBoxVisitedCities.SelectedItems)
                {
                    if (city.Visited)
                        city.Visited = false;
                }

            FillVisitedCities(listBoxVisitedCities.TopIndex);
        }
        //Garages
        //Fill
        public void FillGaragesList(int _vindex)
        {
            listBoxGarages.BeginUpdate();
            listBoxGarages.Items.Clear();

            if (GaragesList.Count <= 0)
                return;

            foreach (Garages garage in from x in GaragesList where !x.IgnoreStatus select x)
            {
                listBoxGarages.Items.Add(garage);
            }

            listBoxGarages.TopIndex = _vindex;
            listBoxGarages.EndUpdate();
        }
        //Draw
        private int GarageItemMargin = 3;
        private const float GaragePictureHeight = 32;

        private void listBoxGarages_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            // Get the ListBox and the item.
            e.ItemHeight = (int)(GaragePictureHeight + 2 * GarageItemMargin);
        }

        private void listBoxGarages_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index == -1)
                return;

            // Get the ListBox and the item.
            ListBox lst = sender as ListBox;
            string txt = "";
            Garages grg = (Garages)lst.Items[e.Index];

            // Draw the background.
            e.DrawBackground();
            Image grgicon;
            if (grg.GarageName != PlayerDataV.HQcity)
                grgicon = GaragesImg[grg.GarageStatus];
            else
                grgicon = GaragesHQImg[grg.GarageStatus];

            // Draw the picture.
            float scale = GaragePictureHeight / grgicon.Height;
            RectangleF source_rect = new RectangleF(0, 0, grgicon.Width, grgicon.Height);

            float picture_width = scale * grgicon.Width;

            RectangleF dest_rect = new RectangleF(e.Bounds.Left + GarageItemMargin, e.Bounds.Top + GarageItemMargin, picture_width, GaragePictureHeight);
            e.Graphics.DrawImage(grgicon, dest_rect, source_rect, GraphicsUnit.Pixel);
            ////

            // See if the item is selected.
            Brush br;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                br = SystemBrushes.HighlightText;
            else
                br = new SolidBrush(e.ForeColor);

            int maxvehdr = 0;

            if (grg.GarageStatus == 0)
                goto skipVehAndDrDraw;//"Not owned";
            else if (grg.GarageStatus == 2)
                maxvehdr = 3;
            else if (grg.GarageStatus == 3)
                maxvehdr = 5;
            else if (grg.GarageStatus == 6)
                maxvehdr = 1;

            //Vehicles & Drivers

            int curVeh = 0, curDr = 0;

            foreach (string temp in grg.Vehicles)
            {
                if (temp != null)
                    curVeh++;
            }
            foreach (string temp in grg.Drivers)
            {
                if (temp != null)
                    curDr++;
            }

            string Vs = "", Ds = "", Ts = "";

            Vs =  ResourceManagerMain.GetString("VehicleShort", Thread.CurrentThread.CurrentUICulture);
            Ds = ResourceManagerMain.GetString("DriverShort", Thread.CurrentThread.CurrentUICulture);
            Ts = ResourceManagerMain.GetString("TrailerShort", Thread.CurrentThread.CurrentUICulture);

            txt = Vs + ": " + curVeh + " / " + maxvehdr + " " + Ds +": " + curDr + " / " + maxvehdr + " " + Ts + ": " + grg.Trailers.Count;

            Size size = TextRenderer.MeasureText(txt, this.Font);

            float x = e.Bounds.Right - size.Width - 3;
            float y = e.Bounds.Top + 18;
            float width = e.Bounds.Right - 100;
            float height = e.Bounds.Bottom - 14;

            RectangleF layout_rect = new RectangleF(x, y, size.Width, height);

            // Draw the text.
            e.Graphics.DrawString(txt, this.Font, br, layout_rect);

            skipVehAndDrDraw:;

            //City and Size
            // Find the area in which to put the text.
            x = e.Bounds.Left + picture_width + 3 * GarageItemMargin;
            y = e.Bounds.Top + GarageItemMargin * 2;
            width = e.Bounds.Right - GarageItemMargin - x;
            height = e.Bounds.Bottom - GarageItemMargin - y;
            layout_rect = new RectangleF(x, y, width, height);

            //txt = lst.Items[e.Index].ToString();
            txt = grg.GarageNameTranslated + "\n" + grg.GetStatusString();
            // Draw the text.
            e.Graphics.DrawString(txt, this.Font, br, layout_rect);

            // Draw the focus rectangle if appropriate.
            e.DrawFocusRectangle();
        }
        //Buttons

        private void buttonUserCompanyGaragesManage_Click(object sender, EventArgs e)
        {
            PrepareGarages();

            FormGaragesSoldContent testDialog = new FormGaragesSoldContent();
            testDialog.ShowDialog(this);
        }

        private void buttonGaragesBuy_Click(object sender, EventArgs e)
        {
            List<Garages> tmp;

            if (listBoxGarages.SelectedItems.Count == 0)
                tmp = listBoxGarages.Items.Cast<Garages>().ToList();
            else
                tmp = listBoxGarages.SelectedItems.Cast<Garages>().ToList();

            foreach (Garages garage in tmp)
            {
                if (garage.GarageStatus == 0)
                    garage.GarageStatus = 2;
            }

            PrepareGarages();

            FillGaragesList(listBoxGarages.TopIndex);
            FillHQcities();
        }

        private void buttonGaragesUpgrade_Click(object sender, EventArgs e)
        {
            List<Garages> tmp;

            if (listBoxGarages.SelectedItems.Count == 0)
                tmp = listBoxGarages.Items.Cast<Garages>().ToList();
            else
                tmp = listBoxGarages.SelectedItems.Cast<Garages>().ToList();

            foreach (Garages garage in tmp)
            {
                if (garage.GarageStatus == 2)
                    garage.GarageStatus = 3;
                else if (garage.GarageStatus == 6)
                    garage.GarageStatus = 2;
            }

            PrepareGarages();

            FillGaragesList(listBoxGarages.TopIndex);
        }

        private void buttonGaragesDowngrade_Click(object sender, EventArgs e)
        {
            List<Garages> tmp;

            if (listBoxGarages.SelectedItems.Count == 0)
                tmp = listBoxGarages.Items.Cast<Garages>().ToList();
            else
                tmp = listBoxGarages.SelectedItems.Cast<Garages>().ToList();

            foreach (Garages garage in tmp)
            {
                if (garage.GarageStatus == 3)
                    garage.GarageStatus = 2;
                else if (garage.GarageName == comboBoxUserCompanyHQcity.SelectedValue.ToString())
                    garage.GarageStatus = 6;
            }

            PrepareGarages();

            FillGaragesList(listBoxGarages.TopIndex);
        }

        private void buttonGaragesSell_Click(object sender, EventArgs e)
        {
            List<Garages> tmp;

            if (listBoxGarages.SelectedItems.Count == 0)
                tmp = listBoxGarages.Items.Cast<Garages>().ToList();
            else
                tmp = listBoxGarages.SelectedItems.Cast<Garages>().ToList();

            foreach (Garages garage in tmp)
            {
                if (garage.GarageName == PlayerDataV.HQcity)
                    garage.GarageStatus = 6;
                else
                    garage.GarageStatus = 0;
            }

            PrepareGarages();

            FillGaragesList(listBoxGarages.TopIndex);
            FillHQcities();
        }
        //end User Company tab

        //Freight market tab
        private void FillFormFreightMarketControls()
        {
            FillcomboBoxCargoList();
            FillcomboBoxCountries();
            FillcomboBoxCompanies();
            FillcomboBoxUrgencyList();
            FillcomboBoxSourceCity();
        }
        //Job list
        private int JobsItemMargin = 3;
        private const float JobsPictureHeight = 32, JobsTextHeigh = 23, JobsItemHeight = 64;

        private void listBoxAddedJobs_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = (int)(JobsItemHeight + 2 * JobsItemMargin);
        }

        private void listBoxAddedJobs_DrawItem(object sender, DrawItemEventArgs e)
        {
            // Get the ListBox and the item.
            ListBox lst = sender as ListBox;

            if (lst.Items.Count > 0)
            {

                JobAdded Job = (JobAdded)lst.Items[e.Index];

                // Draw the background.
                e.DrawBackground();

                // See if the item is selected.
                Brush br;
                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                    br = SystemBrushes.HighlightText;
                else
                    br = new SolidBrush(e.ForeColor);

                Image SourceCompIcon = null, DestinationCompIcon = null;

                if (File.Exists(@"img\" + GameType + @"\companies\" + Job.SourceCompany + ".dds"))
                    SourceCompIcon = ExtImgLoader(new string[] { @"img\" + GameType + @"\companies\" + Job.SourceCompany + ".dds" }, 100, 32, 0, 0)[0];
                else
                {
                    string currentDirName = Directory.GetCurrentDirectory() + @"\img\" + GameType + @"\companies";
                    string searchpattern = Job.SourceCompany.Split(new char[] { '_' })[0] + "*.dds";
                    string[] files = Directory.GetFiles(currentDirName, searchpattern);

                    if (files.Length > 0)
                        SourceCompIcon = ExtImgLoader(new string[] { files[0] }, 100, 32, 0, 0)[0];
                    else
                    {
                        SourceCompIcon = DrawCompanyText(Job.SourceCompany, 100, 32, br);
                    }
                }

                if (File.Exists(@"img\" + GameType + @"\companies\" + Job.DestinationCompany + ".dds"))
                    DestinationCompIcon = ExtImgLoader(new string[] { @"img\" + GameType + @"\companies\" + Job.DestinationCompany + ".dds" }, 100, 32, 0, 0)[0];
                else
                {
                    string currentDirName = Directory.GetCurrentDirectory() + @"\img\" + GameType + @"\companies";
                    string searchpattern = Job.DestinationCompany.Split(new char[] { '_' })[0] + "*.dds";
                    string[] files = Directory.GetFiles(currentDirName, searchpattern);
                    if (files.Length > 0)
                        DestinationCompIcon = ExtImgLoader(new string[] { files[0] }, 100, 32, 0, 0)[0];
                    else
                    {
                        DestinationCompIcon = DrawCompanyText(Job.DestinationCompany, 100, 32, br);
                    }
                }

                // Draw the Source comp. picture
                float scale = JobsPictureHeight / SourceCompIcon.Height;
                RectangleF source_rect = new RectangleF(0, 0, SourceCompIcon.Width, SourceCompIcon.Height);

                float picture_width = scale * SourceCompIcon.Width;

                RectangleF dest_rect = new RectangleF(e.Bounds.Left + JobsItemMargin, e.Bounds.Top + JobsItemMargin * 2 + JobsTextHeigh, picture_width, JobsPictureHeight);
                e.Graphics.DrawImage(SourceCompIcon, dest_rect, source_rect, GraphicsUnit.Pixel);


                // Draw the Destination comp. picture
                dest_rect = new RectangleF(e.Bounds.Right - JobsItemMargin - picture_width, e.Bounds.Top + JobsItemMargin * 2 + JobsTextHeigh, picture_width, JobsPictureHeight);
                e.Graphics.DrawImage(DestinationCompIcon, dest_rect, source_rect, GraphicsUnit.Pixel);
                ////
                // Draw Type picture
                Image[] TypeImgs = new Image[5];
                int indexTypeImgs = 0, CargoMass = 0;
                bool extheavy = false;

                try
                {
                    ExtCargo tempExtCargo = ExtCargoList.Find(z => z.CargoName == Job.Cargo);

                    decimal fragile = tempExtCargo.Fragility;
                    bool valuable = tempExtCargo.Valuable;
                    int ADRclass = tempExtCargo.ADRclass;
                    int trueADR = ADRclass;
                    switch (trueADR)
                    {
                        case 6:
                            {
                                trueADR = 5;
                                break;
                            }
                        case 8:
                            {
                                trueADR = 6;
                                break;
                            }
                    }
                    CargoMass = (int)(tempExtCargo.Mass * Job.UnitsCount);
                    if (CargoMass > 26000)
                        extheavy = true;

                    if (ADRclass > 0)
                    {
                        Bitmap bmp = new Bitmap(32, 32);
                        Graphics graph = Graphics.FromImage(bmp);
                        graph.DrawImage(ADRImgS[trueADR - 1], 2, 2, 28, 28);

                        TypeImgs[indexTypeImgs] = bmp;
                        indexTypeImgs++;
                    }

                    if (fragile == 0 || fragile >= (decimal)0.7)
                    {
                        TypeImgs[indexTypeImgs] = CargoType2Img[0];
                        indexTypeImgs++;
                    }

                    if (valuable)
                    {
                        TypeImgs[indexTypeImgs] = CargoType2Img[1];
                        indexTypeImgs++;
                    }
                }
                catch
                {

                }

                if (extheavy || Job.Type == 1)
                {
                    TypeImgs[indexTypeImgs] = CargoTypeImg[1];
                    indexTypeImgs++;
                }

                if (Job.Type == 2)
                {
                    TypeImgs[indexTypeImgs] = CargoTypeImg[2];
                    indexTypeImgs++;
                }

                TypeImgs[indexTypeImgs] = UrgencyImg[Job.Urgency];

                int xmult = 0, images = 0;

                foreach (Image temp in TypeImgs)
                {
                    if (temp == null)
                    {
                        break;
                    }
                    images++;
                }

                // Draw Cargo type Images
                for (int i = 0; i < 5; i++)
                {
                    if (TypeImgs[i] == null)
                    {
                        break;
                    }

                    source_rect = new RectangleF(0, 0, 32, 32);
                    dest_rect = new RectangleF((e.Bounds.Right - e.Bounds.Left - 32 * images) / 2 + 32 * xmult, e.Bounds.Top + JobsItemMargin, 32, 32);
                    e.Graphics.DrawImage(TypeImgs[i], dest_rect, source_rect, GraphicsUnit.Pixel);

                    xmult++;
                }
                

                // Draw the text.
                string value = "", SourceCityName = "", DestinationCityName = "";

                CitiesLngDict.TryGetValue(Job.SourceCity, out value);
                if (value != null && value != "")
                    SourceCityName = value;
                else
                {
                    SourceCityName = Job.SourceCity + " -n";
                }
                CitiesLngDict.TryGetValue(Job.DestinationCity, out value);
                if (value != null && value != "")
                    DestinationCityName = value;
                else
                {
                    DestinationCityName = Job.DestinationCity + " -n";
                }
                
                //Source City
                // Find the area in which to put the text.
                float x = e.Bounds.Left + JobsItemMargin;
                float y = e.Bounds.Top - JobsItemMargin + JobsTextHeigh / 2;
                float width = (e.Bounds.Right - e.Bounds.Left - JobsItemMargin * 4 - UrgencyImg[Job.Urgency].Width) / 2;
                float height = JobsTextHeigh;//e.Bounds.Bottom - JobsItemMargin - y;
                RectangleF layout_rect = new RectangleF(x, y, width, height);
                
                string txt = "(" + CitiesList.Find(xc => xc.CityName == Job.SourceCity).Country.First() + ") " + SourceCityName;
                StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Near;
                e.Graphics.DrawString(txt, this.Font, br, layout_rect, format);

                //Destination City
                // Find the area in which to put the text.
                x = e.Bounds.Left + width + 3 * JobsItemMargin + UrgencyImg[Job.Urgency].Width;
                layout_rect = new RectangleF(x, y, width, height);
                format.Alignment = StringAlignment.Far;
                txt = DestinationCityName + " (" + CitiesList.Find(xc => xc.CityName == Job.DestinationCity).Country.First() + ")";
                e.Graphics.DrawString(txt, this.Font, br, layout_rect, format);

                //Cargo
                // Find the area in which to put the text.
                x = e.Bounds.Left + picture_width + 4 * JobsItemMargin;// + 32 * 2;
                y = e.Bounds.Top + JobsItemMargin * 2 + UrgencyImg[Job.Urgency].Height;
                width = e.Bounds.Right - JobsItemMargin - x;
                height = e.Bounds.Bottom - JobsItemMargin - y;
                layout_rect = new RectangleF(x, y, width, height);

                if (CargoLngDict.TryGetValue(Job.Cargo, out string CargoName))
                {
                    if (CargoName != null && CargoName != "")
                    {
                        txt = CargoName;
                    }
                    else
                        txt = Job.Cargo;
                }
                else
                    txt = Job.Cargo;

                if (CargoMass > 0)
                    txt += " (" + CargoMass + " kg)";

                e.Graphics.DrawString(txt, this.Font, br, layout_rect);

                // Find the area in which to put Distance text.
                if (Job.Distance == 11111)
                {
                    txt = Math.Floor(5 * DistanceMultiplier).ToString() + "* ";
                }
                else
                {
                    txt = Math.Floor(Job.Distance * DistanceMultiplier).ToString() + " ";
                }

                txt += ProgSettingsV.DistanceMes + " ";

                if (Job.Ferrytime > 0)
                {
                    txt += "(Ferry ";

                    if(Job.Ferrytime < 60)
                        txt += Job.Ferrytime.ToString() + "min - ";
                    else
                        txt += (Job.Ferrytime / 60).ToString() + "h - ";

                    txt += Job.Ferryprice.ToString() + " €)";
                }

                layout_rect = new RectangleF(x, y + 14, width, height);
                e.Graphics.DrawString(txt, this.Font, br, layout_rect);

                // Draw the focus rectangle if appropriate.
                e.DrawFocusRectangle();
            }
        }

        public Bitmap DrawCompanyText(string _companyName, int _width, int _height, Brush _brush)
        {
            Bitmap bmp = new Bitmap(100, 32);
            RectangleF rectf = new RectangleF(5, 5, 90, 22);

            Graphics g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;


            g.DrawString(_companyName, new Font(Font.FontFamily, 12), _brush, rectf, format);
            g.Flush();

            return bmp;
        }
        //Main countries
        public void FillcomboBoxCountries()
        {
            DataTable combDT = new DataTable();
            DataColumn dc = new DataColumn("Country", typeof(string));
            combDT.Columns.Add(dc);

            dc = new DataColumn("CountryName", typeof(string));
            combDT.Columns.Add(dc);

            string value = null;

            foreach (string tempitem in CountriesList)
            {
                value = null;
                CountriesLngDict.TryGetValue(tempitem, out value);

                if (value != null && value != "")
                {
                    combDT.Rows.Add(tempitem, value);
                }
                else
                {
                    string CapName = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(tempitem);
                    combDT.Rows.Add(tempitem, CapName);
                }
            }

            combDT.DefaultView.Sort = "CountryName ASC";

            DataTable dt2 = combDT.DefaultView.ToTable();
            DataRow row1 = dt2.NewRow();
            row1[0] = "All";

            CountriesLngDict.TryGetValue("All", out value);

            if (value != null && value != "")
            {
                row1[1] = value;
            }
            else
            {
                row1[1] = "All";
            }

            dt2.Rows.InsertAt(row1, 0);

            comboBoxFreightMarketCountries.ValueMember = "Country";
            comboBoxFreightMarketCountries.DisplayMember = "CountryName";
            comboBoxFreightMarketCountries.DataSource = dt2;
            comboBoxFreightMarketCountries.SelectedValue = "All";
        }

        private void comboBoxCountries_SelectedIndexChanged(object sender, EventArgs e)
        {
            triggerDestinationCitiesUpdate();
        }
        //Main companies
        public void FillcomboBoxCompanies()
        {
            //start filtering
            List<string> tempCompList = new List<string>();
            Dictionary<string, string> sourceCompList = new Dictionary<string, string>();

            foreach (City city in CitiesList.FindAll(x => !x.Disabled))
            {
                List<Company> source = city.ReturnCompanies();

                foreach (Company company in from x in source where !x.Excluded select x)
                {
                    if(!sourceCompList.ContainsKey(company.CompanyName))
                        sourceCompList.Add(company.CompanyName, company.CompanyNameTranslated);
                }
            }
            //end filtering

            DataTable combDT = new DataTable();
            DataColumn dc = new DataColumn("Company", typeof(string));
            combDT.Columns.Add(dc);

            dc = new DataColumn("CompanyName", typeof(string));
            combDT.Columns.Add(dc);

            foreach(KeyValuePair<string, string> tempitem in sourceCompList)
            {
                combDT.Rows.Add(tempitem.Key, tempitem.Value);
            }

            DataTable sortedDT = combDT.DefaultView.Table.Copy();

            DataView dv = sortedDT.DefaultView;
            dv.Sort = "CompanyName ASC";
            sortedDT = dv.ToTable();
            sortedDT.DefaultView.Sort = "";
            
            DataRow row = sortedDT.NewRow();
            string tvalue;
            CompaniesLngDict.TryGetValue("All", out tvalue);
            row.ItemArray = new object [] { "All", tvalue };

            sortedDT.Rows.InsertAt(row, 0);
            //
            comboBoxFreightMarketCompanies.ValueMember = "Company";
            comboBoxFreightMarketCompanies.DisplayMember = "CompanyName";
            comboBoxFreightMarketCompanies.DataSource = sortedDT;
            //end filling
        }

        private void comboBoxCompanies_SelectedIndexChanged(object sender, EventArgs e)
        {
            triggerDestinationCitiesUpdate();
        }

        public void FillcomboBoxSourceCity()
        {
            DataTable combDT = new DataTable();
            DataColumn dc = new DataColumn("City", typeof(string));
            combDT.Columns.Add(dc);

            dc = new DataColumn("CityName", typeof(string));
            combDT.Columns.Add(dc);

            //start filling

            //fill source and destination cities
            foreach (City tempcity in from x in CitiesList where !x.Disabled select x)
            {
                combDT.Rows.Add(tempcity.CityName, tempcity.CityNameTranslated);
            }
            combDT.DefaultView.Sort = "CityName ASC";

            comboBoxFreightMarketSourceCity.ValueMember = "City";
            comboBoxFreightMarketSourceCity.DisplayMember = "CityName";
            comboBoxFreightMarketSourceCity.DataSource = combDT;
            //end filling

            comboBoxFreightMarketSourceCity.SelectedValue = LastVisitedCity;
            //end
        }
        //Cargo list
        public void FillcomboBoxCargoList()
        {
            DataTable combDT = new DataTable();
            DataColumn dc = new DataColumn("Cargo", typeof(string));
            combDT.Columns.Add(dc);

            dc = new DataColumn("CargoName", typeof(string));
            combDT.Columns.Add(dc);

            foreach (Cargo tempitem in CargoesList)
            {
                if (CargoLngDict.TryGetValue(tempitem.CargoName, out string value))
                {
                    if (value != null && value != "")
                    {
                        string str = tempitem.CargoName;

                        combDT.Rows.Add(str, value);
                    }
                    else
                    {
                        string str = tempitem.CargoName;
                        string CapName = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(str);

                        combDT.Rows.Add(str, CapName);
                    }
                }
                else
                {
                    string str = tempitem.CargoName;
                    string CapName = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(str);

                    combDT.Rows.Add(str, CapName);
                }
            }

            combDT.DefaultView.Sort = "CargoName ASC";

            comboBoxFreightMarketCargoList.ValueMember = "Cargo";
            comboBoxFreightMarketCargoList.DisplayMember = "CargoName";
            comboBoxFreightMarketCargoList.DataSource = combDT;
        }

        private void comboBoxFreightMarketCargoList_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = 26;
        }

        private void comboBoxFreightMarketCargoList_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
                return;

            //if ((e.State & DrawItemState.ComboBoxEdit) == DrawItemState.ComboBoxEdit)
            //    return;

            ComboBox lst = sender as ComboBox;

            // Draw the background of the item.
            e.DrawBackground();

            // See if the item is selected.
            Brush br;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                br = SystemBrushes.HighlightText;
            else
                br = new SolidBrush(e.ForeColor);

            float x = e.Bounds.Left;
            float y = e.Bounds.Top;
            float width = e.Bounds.Width;
            float height = e.Bounds.Height;
            RectangleF layout_rect;

            string CargoName = ((DataRowView)lst.Items[e.Index])[0].ToString(),
                CargoDN = lst.GetItemText(lst.Items[e.Index]);

            if (CargoName.EndsWith("_c"))
                CargoDN += " (Cont)";

            string txt = CargoDN;

            //////
            // Draw Type picture
            Image[] TypeImgs = new Image[5];
            int indexTypeImgs = 0;
            bool extheavy = false;

            try
            {
                ExtCargo tempExtCargo = ExtCargoList.Find(z => z.CargoName == CargoName);

                decimal fragile = tempExtCargo.Fragility;
                bool valuable = tempExtCargo.Valuable;
                bool overveight = tempExtCargo.Overweight;
                int ADRclass = tempExtCargo.ADRclass;
                int trueADR = ADRclass;

                switch (trueADR)
                {
                    case 6:
                        {
                            trueADR = 5;
                            break;
                        }
                    case 8:
                        {
                            trueADR = 6;
                            break;
                        }
                }

                if (ADRclass > 0)
                {
                    Bitmap bmp = new Bitmap(32, 32);
                    Graphics graph = Graphics.FromImage(bmp);
                    graph.DrawImage(ADRImgS[trueADR - 1], 2, 2, 28, 28);

                    TypeImgs[indexTypeImgs] = bmp;
                    indexTypeImgs++;
                }

                if (fragile == 0 || fragile >= (decimal)0.7)
                {
                    TypeImgs[indexTypeImgs] = CargoType2Img[0];
                    indexTypeImgs++;
                }

                if (valuable)
                {
                    TypeImgs[indexTypeImgs] = CargoType2Img[1];
                    indexTypeImgs++;
                }

                if (overveight)
                {
                    extheavy = true;
                }
            }
            catch
            {
            }
            
            if (extheavy)
            {
                TypeImgs[indexTypeImgs] = CargoTypeImg[1];
                indexTypeImgs++;
            }
            /*
            if (CargoType == "2")
            {
                TypeImgs[indexTypeImgs] = CargoTypeImg[2];
                indexTypeImgs++;
            }
            */
            int xmult = 0, images = 0;

            foreach (Image temp in TypeImgs)
            {
                if (temp == null)
                {
                    break;
                }
                images++;
            }

            for (int i = 0; i < 5; i++)
            {
                if (TypeImgs[i] == null)
                {
                    break;
                }

                RectangleF source_rect = new RectangleF(0, 0, 32, 32);
                RectangleF dest_rect = new RectangleF((e.Bounds.Right - 26 * images) + 24 * xmult, e.Bounds.Top + 1, 24, 24);
                e.Graphics.DrawImage(TypeImgs[i], dest_rect, source_rect, GraphicsUnit.Pixel);

                xmult++;
            }
            /////

            // Find the area in which to put the text.
            float fntsize = 8.25f;
            y = e.Bounds.Top + (e.Bounds.Height - 4 - fntsize) / 2;
            layout_rect = new RectangleF(x, y, width, height);
            //format.Alignment = StringAlignment.Far;
            Font textfnt = new Font("Microsoft Sans Serif", fntsize);
            e.Graphics.DrawString(txt, textfnt, br, layout_rect);

            // Draw the focus rectangle if the mouse hovers over an item.
            e.DrawFocusRectangle();
        }

        private void comboBoxCargoList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ProgSettingsV.ProposeRandom && comboBoxFreightMarketUrgency.Items.Count > 0)
            {
                comboBoxFreightMarketUrgency.SelectedIndex = RandomValue.Next(comboBoxFreightMarketUrgency.Items.Count);
            }
            ComboBox temp = sender as ComboBox;
            if (temp.SelectedIndex > -1)
                FillcomboBoxTrailerDefList();
        }
        //Urgency
        public void FillcomboBoxUrgencyList()
        {
            DataTable combDT = new DataTable();
            combDT.Columns.Add("ID");
            combDT.Columns.Add("UrgencyDisplayName");

            foreach (int tempitem in UrgencyArray)
            {
                string str = tempitem.ToString();
                if (UrgencyLngDict.TryGetValue(str, out string value))
                {
                    if (value != null && value != "")
                    {
                        combDT.Rows.Add(str, value);
                    }
                    else
                    {
                        combDT.Rows.Add(str, str);
                    }
                }
                else
                {
                    combDT.Rows.Add(str, str);
                }
            }

            comboBoxFreightMarketUrgency.ValueMember = "ID";
            comboBoxFreightMarketUrgency.DisplayMember = "UrgencyDisplayName";
            comboBoxFreightMarketUrgency.DataSource = combDT;
        }

        private void comboBoxFreightMarketUrgency_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = 26;
        }

        private void comboBoxFreightMarketUrgency_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
                return;

            ComboBox lst = sender as ComboBox;

            e.DrawBackground();

            Brush br;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                br = SystemBrushes.HighlightText;
            else
                br = new SolidBrush(e.ForeColor);

            float x = e.Bounds.Left;
            float y = e.Bounds.Top;
            float width = e.Bounds.Width;
            float height = e.Bounds.Height;
            RectangleF layout_rect;

            int urgIndex = int.Parse((string)((DataRowView)lst.Items[e.Index])[0]);

            string txt = lst.GetItemText(lst.Items[e.Index]);

            //Draw Urgency img
            RectangleF source_rect = new RectangleF(0, 0, 32, 32);
            RectangleF dest_rect = new RectangleF((e.Bounds.Right - 26), e.Bounds.Top + 1, 24, 24);
            e.Graphics.DrawImage(UrgencyImg[urgIndex], dest_rect, source_rect, GraphicsUnit.Pixel);

            //Draw text
            // Find the area in which to put the text.
            float fntsize = 8.25f;
            y = e.Bounds.Top + (e.Bounds.Height - 4 - fntsize) / 2;
            layout_rect = new RectangleF(x, y, width, height);
            //format.Alignment = StringAlignment.Far;

            Font textfnt = new Font("Microsoft Sans Serif", fntsize);

            e.Graphics.DrawString(txt, textfnt, br, layout_rect);

            // Draw the focus rectangle if the mouse hovers over an item.
            e.DrawFocusRectangle();
        }
        //Trailer definition
        public void FillcomboBoxTrailerDefList()
        {
            DataTable combDT = new DataTable();
            DataColumn dc = new DataColumn("Definition", typeof(string));
            combDT.Columns.Add(dc);

            dc = new DataColumn("DefinitionName", typeof(string));
            combDT.Columns.Add(dc);

            dc = new DataColumn("CargoType", typeof(int));
            combDT.Columns.Add(dc);

            dc = new DataColumn("UnitsCount", typeof(int));
            combDT.Columns.Add(dc);

            Cargo TempCargo = CargoesList.Find(x => x.CargoName == comboBoxFreightMarketCargoList.SelectedValue.ToString().Split(new char[] { ',' })[0]);
            
            foreach ( TrailerDefinition tempitem in TempCargo.TrailerDefList)
            {
                string value = null;

                CargoLngDict.TryGetValue(tempitem.DefName, out value);

                if (value != null && value != "")
                {
                    combDT.Rows.Add(tempitem.DefName, value + " (" + tempitem.UnitsCount +"u)", tempitem.CargoType, tempitem.UnitsCount);
                }
                else
                {
                    string CapName = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(tempitem.DefName);
                    string[] CapNameArray =  CapName.Split(new char[] { '.' });

                    CapName = "";
                    for (int i = 1; i < CapNameArray.Length; i++)
                    {
                        CapName += CapNameArray[i] + " ";
                    }


                    combDT.Rows.Add(tempitem.DefName, CapName + "(" + tempitem.UnitsCount + "u)", tempitem.CargoType, tempitem.UnitsCount);
                }
            }

            combDT.DefaultView.Sort = "DefinitionName ASC";

            comboBoxFreightMarketTrailerDef.ValueMember = "Definition";
            comboBoxFreightMarketTrailerDef.DisplayMember = "DefinitionName";
            comboBoxFreightMarketTrailerDef.DataSource = combDT;

            comboBoxFreightMarketTrailerDef.SelectedIndex = RandomValue.Next(comboBoxFreightMarketTrailerDef.Items.Count);
        }

        private void comboBoxFreightMarketTrailerDef_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox temp = sender as ComboBox;
            if (temp.SelectedIndex > -1)
                FillcomboBoxTrailerVariantList();
        }

        private void comboBoxFreightMarketTrailerDef_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = 26;
        }

        private void comboBoxFreightMarketTrailerDef_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
                return;

            ComboBox lst = sender as ComboBox;

            // Draw the background of the item.
            e.DrawBackground();

            // See if the item is selected.
            Brush br;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                br = SystemBrushes.HighlightText;
            else
                br = new SolidBrush(e.ForeColor);

            float x = e.Bounds.Left;
            float y = e.Bounds.Top;
            float height = e.Bounds.Height;
            RectangleF layout_rect;

            string CargoName = ((DataRowView)comboBoxFreightMarketCargoList.SelectedItem)[0].ToString(),
                DefDN = lst.GetItemText(lst.Items[e.Index]), CargoType = ((DataRowView)lst.Items[e.Index])[2].ToString();

            string txt = DefDN;

            int DefUnitsCount = int.Parse(((DataRowView)lst.Items[e.Index])[3].ToString());

            //////
            // Draw Type picture
            Image[] TypeImgs = new Image[5];
            int indexTypeImgs = 0;
            bool extheavy = false;

            try
            {
                decimal TCW = ExtCargoList.Find(xx => xx.CargoName == CargoName).Mass * DefUnitsCount;

                if (TCW > 26000)
                    extheavy = true;
            }
            catch
            { }

            if (extheavy || CargoType == "1")
            {
                TypeImgs[indexTypeImgs] = CargoTypeImg[1];
                indexTypeImgs++;
            }

            if (CargoType == "2")
            {
                TypeImgs[indexTypeImgs] = CargoTypeImg[2];
                indexTypeImgs++;
            }
            
            int xmult = 0, images = 0;

            foreach (Image temp in TypeImgs)
            {
                if (temp == null)
                {
                    break;
                }
                images++;
            }

            int rightOffset = 24;

            float width = comboBoxFreightMarketTrailerDef.Width - 26 * images - rightOffset;

            //e.Graphics.DrawRectangle(new Pen(Color.Red), e.Bounds.X, e.Bounds.Y, width, e.Bounds.Height);

            for (int i = 0; i < 5; i++)
            {
                if (TypeImgs[i] == null)
                {
                    break;
                }

                RectangleF source_rect = new RectangleF(0, 0, 32, 32);
                RectangleF dest_rect = new RectangleF((e.Bounds.Left + width) + 24 * xmult, e.Bounds.Top + 1, 24, 24);
                e.Graphics.DrawImage(TypeImgs[i], dest_rect, source_rect, GraphicsUnit.Pixel);

                xmult++;
            }
            /////

            // Find the area in which to put the text.

            float fntsize = 8.25f;
            Font textfnt = new Font("Microsoft Sans Serif", fntsize);

            Size size = TextRenderer.MeasureText(txt, textfnt);
            if (size.Width >= width - 21)
            {
                fntsize = 7f;
                textfnt = new Font("Microsoft Sans Serif", fntsize);
                y = e.Bounds.Top - 5 + (e.Bounds.Height - 4 - fntsize) / 2;
            }
            else
            {
                y = e.Bounds.Top + (e.Bounds.Height - 4 - fntsize) / 2;
            }

            layout_rect = new RectangleF(x, y, width, height);
            StringFormat format = new StringFormat();
            
            //format.Alignment = StringAlignment.Far;

            e.Graphics.DrawString(txt, textfnt, br, layout_rect);

            // Draw the focus rectangle if the mouse hovers over an item.
            e.DrawFocusRectangle();
        }
        //Trailer variant
        public void FillcomboBoxTrailerVariantList()
        {
            DataTable combDT = new DataTable();
            DataColumn dc = new DataColumn("Variant", typeof(string));
            combDT.Columns.Add(dc);

            dc = new DataColumn("VariantName", typeof(string));
            combDT.Columns.Add(dc);

            List<string> TrailerVariants = TrailerDefinitionVariants[comboBoxFreightMarketTrailerDef.SelectedValue.ToString()];

            foreach (string tempitem in TrailerVariants)
            {
                string value = null;

                //CargoLngDict.TryGetValue(tempitem, out value);

                if (value != null && value != "")
                {
                    combDT.Rows.Add(tempitem, value);
                }
                else
                {
                    string CapName = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(tempitem);
                    combDT.Rows.Add(tempitem, CapName);
                }
            }

            combDT.DefaultView.Sort = "VariantName ASC";

            comboBoxFreightMarketTrailerVariant.ValueMember = "Variant";
            comboBoxFreightMarketTrailerVariant.DisplayMember = "VariantName";
            comboBoxFreightMarketTrailerVariant.DataSource = combDT;

            comboBoxFreightMarketTrailerVariant.SelectedIndex = RandomValue.Next(comboBoxFreightMarketTrailerVariant.Items.Count);
        }

        private void comboBoxFreightMarketTrailerVariant_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = 26;
        }

        private void comboBoxFreightMarketTrailerVariant_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
                return;

            ComboBox lst = sender as ComboBox;

            // Draw the background of the item.
            e.DrawBackground();

            // See if the item is selected.
            Brush br;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                br = SystemBrushes.HighlightText;
            else
                br = new SolidBrush(e.ForeColor);

            float x = e.Bounds.Left;
            float y = e.Bounds.Top;
            float width = e.Bounds.Width - 8;
            float height = e.Bounds.Height;
            RectangleF layout_rect;
            string DefDN = lst.GetItemText(lst.Items[e.Index]);
            string txt = DefDN;

            // Find the area in which to put the text.
            float fntsize = 8.25f;
            Font textfnt = new Font("Microsoft Sans Serif", fntsize);

            Size size = TextRenderer.MeasureText(txt, textfnt);
            if (size.Width > width)
            {
                fntsize = 7f;
                textfnt = new Font("Microsoft Sans Serif", fntsize);
                y = e.Bounds.Top - 5 + (e.Bounds.Height - 4 - fntsize) / 2;
            }
            else
            {
                y = e.Bounds.Top + (e.Bounds.Height - 4 - fntsize) / 2;
            }

            layout_rect = new RectangleF(x, y, width, height);
            //format.Alignment = StringAlignment.Far;
            e.Graphics.DrawString(txt, textfnt, br, layout_rect);

            // Draw the focus rectangle if the mouse hovers over an item.
            e.DrawFocusRectangle();
        }
        //Source city
        private void comboBoxSourceCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            string _sourceCityName = comboBoxFreightMarketSourceCity.SelectedValue.ToString();

            comboBoxFreightMarketSourceCompany.SelectedIndex = -1;

            List<Company> sourceCompaniesList = CitiesList.Find(x => x.CityName == _sourceCityName).ReturnCompanies().FindAll(x => !x.Excluded);

            DataTable combDT = new DataTable();
            DataColumn dc = new DataColumn("Company", typeof(string));
            combDT.Columns.Add(dc);

            dc = new DataColumn("CompanyName", typeof(string));
            combDT.Columns.Add(dc);

            foreach (Company company in sourceCompaniesList)
                combDT.Rows.Add(company.CompanyName, company.CompanyNameTranslated);

            combDT.DefaultView.Sort = "CompanyName ASC";

            comboBoxFreightMarketSourceCompany.ValueMember = "Company";
            comboBoxFreightMarketSourceCompany.DisplayMember = "CompanyName";

            comboBoxFreightMarketSourceCompany.DataSource = combDT;

            if (ProgSettingsV.ProposeRandom && (comboBoxFreightMarketSourceCompany.Items.Count > 0))
            {
                comboBoxFreightMarketSourceCompany.SelectedIndex = RandomValue.Next(comboBoxFreightMarketSourceCompany.Items.Count);
            }
        }

        //Source company
        private void comboBoxSourceCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ProgSettingsV.ProposeRandom)
            {
                comboBoxFreightMarketCargoList.SelectedIndex = RandomValue.Next(comboBoxFreightMarketCargoList.Items.Count);
            }
        }
        //Destination city
        private void comboBoxDestinationCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxFreightMarketDestinationCity.SelectedIndex >= 0)
            {
                comboBoxFreightMarketDestinationCompany.SelectedIndex = -1;
                comboBoxFreightMarketDestinationCompany.Text = "";

                triggerDestinationCompaniesUpdate();

                if (ProgSettingsV.ProposeRandom && (comboBoxFreightMarketDestinationCompany.Items.Count > 0))
                {
                    if ((comboBoxFreightMarketDestinationCompany.Items.Count != 1) && (comboBoxFreightMarketSourceCity.SelectedValue == comboBoxFreightMarketDestinationCity.SelectedValue))
                    {
                        int rnd = 0;
                        while (true)
                        {
                            rnd = RandomValue.Next(comboBoxFreightMarketDestinationCompany.Items.Count);
                            if (comboBoxFreightMarketSourceCompany.SelectedIndex != rnd)
                            {
                                comboBoxFreightMarketDestinationCompany.SelectedIndex = rnd;
                                break;
                            }
                        }
                    }
                    else
                        comboBoxFreightMarketDestinationCompany.SelectedIndex = RandomValue.Next(comboBoxFreightMarketDestinationCompany.Items.Count);
                }
            }
        }

        private void triggerDestinationCitiesUpdate()
        {
            if (comboBoxFreightMarketCompanies.SelectedIndex != -1)
                SetupDestinationCities(!(comboBoxFreightMarketCountries.SelectedValue.ToString() == "All"), !(comboBoxFreightMarketCompanies.SelectedValue.ToString() == "All"));
        }

        private void SetupDestinationCities(bool _country_selected, bool _company_selected)
        {
            DataTable combDT = new DataTable();
            DataColumn dc = new DataColumn("City", typeof(string));
            combDT.Columns.Add(dc);

            dc = new DataColumn("CityName", typeof(string));
            combDT.Columns.Add(dc);

            //start filling
            List<City> cities = CitiesList.FindAll(x => !x.Disabled);

            if (_country_selected && !checkBoxFreightMarketFilterDestination.Checked)
            {
                cities = cities.FindAll(x => x.Country == comboBoxFreightMarketCountries.SelectedValue.ToString());
            }
            else
            if (!(_country_selected || checkBoxFreightMarketFilterDestination.Checked))
            {
            }

            foreach (City city in cities)
            {
                List<Company> companyList = city.ReturnCompanies();

                if (!_company_selected)
                {
                }
                else
                if (_company_selected && checkBoxFreightMarketFilterDestination.Checked)
                {
                    companyList = companyList.FindAll(x => (x.CompanyName == comboBoxFreightMarketCompanies.SelectedValue.ToString()) && !x.Excluded);
                }
                else
                if (!(_company_selected || !checkBoxFreightMarketFilterDestination.Checked))
                {
                    companyList = companyList.FindAll(x => !x.Excluded);
                }
                else
                if (_company_selected && !checkBoxFreightMarketFilterDestination.Checked)
                {
                    companyList = companyList.FindAll(x => x.CompanyName == comboBoxFreightMarketCompanies.SelectedValue.ToString());
                }

                if (companyList.Count > 0)
                {
                    combDT.Rows.Add(city.CityName, city.CityNameTranslated);
                    comboBoxFreightMarketDestinationCity.Enabled = true;
                }
                else
                {
                    comboBoxFreightMarketDestinationCity.Enabled = false;
                }
            }

            combDT.DefaultView.Sort = "CityName ASC";

            comboBoxFreightMarketDestinationCity.ValueMember = "City";
            comboBoxFreightMarketDestinationCity.DisplayMember = "CityName";
            comboBoxFreightMarketDestinationCity.DataSource = combDT;
            //end filling

            if (comboBoxFreightMarketDestinationCity.Items.Count == 0)
            {
                ShowStatusMessages("e", "message_no_matching_cities");
            }
            else
            {
                ShowStatusMessages("i", "");
                comboBoxFreightMarketDestinationCity.SelectedIndex = RandomValue.Next(comboBoxFreightMarketDestinationCity.Items.Count);
            }
        }
        //Destination companies
        private void triggerDestinationCompaniesUpdate()
        {
            SetupDestinationCompanies(!(comboBoxFreightMarketCompanies.SelectedValue.ToString() == "All"));
        }

        private void SetupDestinationCompanies(bool _company_selected)
        {
            List<Company> CityCompanies = CitiesList.Find(x => x.CityName == comboBoxFreightMarketDestinationCity.SelectedValue.ToString()).ReturnCompanies();
            List<Company> RealCompanies = CityCompanies.FindAll(x => !x.Excluded);

            if (_company_selected && checkBoxFreightMarketFilterDestination.Checked)
            {
                RealCompanies = RealCompanies.FindAll(x => (x.CompanyName == comboBoxFreightMarketCompanies.SelectedValue.ToString()));
            }

            DataTable combDT = new DataTable();
            DataColumn dc = new DataColumn("Company", typeof(string));
            combDT.Columns.Add(dc);

            dc = new DataColumn("CompanyName", typeof(string));
            combDT.Columns.Add(dc);

            foreach (Company company in RealCompanies)
            {
                combDT.Rows.Add(company.CompanyName, company.CompanyNameTranslated);
                /*
                CompaniesLngDict.TryGetValue(company.CompanyName, out string value);
                if (value != null && value != "")
                {
                    combDT.Rows.Add(company.CompanyName, value);
                }
                else
                {
                    combDT.Rows.Add(company.CompanyName, company.CompanyName + " -n");
                }
                */
            }
            
            combDT.DefaultView.Sort = "CompanyName ASC";
            comboBoxFreightMarketDestinationCompany.ValueMember = "Company";
            comboBoxFreightMarketDestinationCompany.DisplayMember = "CompanyName";

            comboBoxFreightMarketDestinationCompany.DataSource = combDT;
        }

        private void comboBoxDestinationCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ProgSettingsV.ProposeRandom)
            {
                comboBoxFreightMarketCargoList.SelectedIndex = RandomValue.Next(comboBoxFreightMarketCargoList.Items.Count);
            }
        }
        //Buttons
        private void buttonAddJob_Click(object sender, EventArgs e)
        {
            AddCargo(false);
        }

        private void buttonEditJob_Click(object sender, EventArgs e)
        {
            AddCargo(true);

            buttonFreightMarketCancelJobEdit.Visible = false;
            buttonFreightMarketCancelJobEdit.Enabled = false;
            buttonFreightMarketAddJob.Width = 394;

            buttonFreightMarketAddJob.Text = "Add Job to list";
            buttonFreightMarketAddJob.Click -= buttonEditJob_Click;
            buttonFreightMarketAddJob.Click += buttonAddJob_Click;

            int JobsTotalDistance = 0;

            foreach (JobAdded tmpItem in listBoxFreightMarketAddedJobs.Items)
            {
                JobsTotalDistance += tmpItem.Distance;
            }

            labelFreightMarketDistanceNumbers.Text = Math.Floor(JobsTotalDistance * DistanceMultiplier).ToString() + unCertainRouteLength + " " + ProgSettingsV.DistanceMes;
        }

        private void buttonFreightMarketCancelJobEdit_Click(object sender, EventArgs e)
        {
            listBoxFreightMarketAddedJobs.Enabled = true;
            buttonFreightMarketCancelJobEdit.Visible = false;
            buttonFreightMarketCancelJobEdit.Enabled = false;
            buttonFreightMarketAddJob.Width = 394;

            buttonFreightMarketAddJob.Text = "Add Job to list";
            buttonFreightMarketAddJob.Click -= buttonEditJob_Click;
            buttonFreightMarketAddJob.Click += buttonAddJob_Click;

            comboBoxFreightMarketSourceCity.SelectedValue = ((JobAdded)listBoxFreightMarketAddedJobs.Items[listBoxFreightMarketAddedJobs.Items.Count - 1]).DestinationCity;
            comboBoxFreightMarketSourceCompany.SelectedValue = ((JobAdded)listBoxFreightMarketAddedJobs.Items[listBoxFreightMarketAddedJobs.Items.Count - 1]).DestinationCompany;
        }
        
        private void buttonClearJobList_Click(object sender, EventArgs e)
        {
            ClearJobData();
        }

        private void ClearJobData()
        {
            unCertainRouteLength = "";
            JobsAmountAdded = 0;

            AddedJobsDictionary.Clear();
            Array.Resize(ref EconomyEventUnitLinkStringList, 0);

            listBoxFreightMarketAddedJobs.Items.Clear();
            labelFreightMarketDistanceNumbers.Text = "0 " + ProgSettingsV.DistanceMes;
            buttonFreightMarketClearJobList.Enabled = false;
        }

        private void checkBoxRandomDest_CheckedChanged(object sender, EventArgs e)
        {
            ProgSettingsV.ProposeRandom = checkBoxFreightMarketRandomDest.Checked;
        }

        private void buttonFreightMarketRandomizeCargo_Click(object sender, EventArgs e)
        {
            comboBoxFreightMarketCargoList.SelectedIndex = RandomValue.Next(comboBoxFreightMarketCargoList.Items.Count);
        }

        //contextMenuStrip Freight Market JobList
        private void listBoxFreightMarketAddedJobs_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if( listBoxFreightMarketAddedJobs.Items.Count != 0)
                {
                    Rectangle rect = listBoxFreightMarketAddedJobs.GetItemRectangle(listBoxFreightMarketAddedJobs.Items.Count - 1);

                    if (e.Y < rect.Bottom)
                    {
                        contextMenuStripFreightMarketJobList.Show(listBoxFreightMarketAddedJobs, e.Location);
                        int index = listBoxFreightMarketAddedJobs.IndexFromPoint(e.Location);
                        listBoxFreightMarketAddedJobs.SelectedIndex = index;
                    }                        
                }                
            }

            if (e.Button == MouseButtons.Left)
            {
                if (listBoxFreightMarketAddedJobs.Items.Count != 0)
                {
                    Rectangle rect = listBoxFreightMarketAddedJobs.GetItemRectangle(listBoxFreightMarketAddedJobs.Items.Count - 1);

                    if (e.Y > rect.Bottom)
                    {
                        
                    }
                }
            }
        }

        private void contextMenuStripFreightMarketJobListEdit_Click(object sender, EventArgs e)
        {
            FM_JobList_Edit();
        }

        private void contextMenuStripFreightMarketJobListDelete_Click(object sender, EventArgs e)
        {
            FM_JobList_Delete();
        }

        private void FM_JobList_Delete()
        {
            string companyNameJob = "company : company.volatile." + ((JobAdded)listBoxFreightMarketAddedJobs.SelectedItem).SourceCompany + "." + ((JobAdded)listBoxFreightMarketAddedJobs.SelectedItem).SourceCity + " {";
            AddedJobsDictionary[companyNameJob].Remove((JobAdded)listBoxFreightMarketAddedJobs.SelectedItem);
            if (AddedJobsDictionary[companyNameJob].Count == 0)
                AddedJobsDictionary.Remove(companyNameJob);
            listBoxFreightMarketAddedJobs.Items.Remove(listBoxFreightMarketAddedJobs.SelectedItem);

            //labelFreightMarketDistanceNumbers.Text = "0 " + ProgSettingsV.DistanceMes;
            int JobsTotalDistance = 0;

            foreach (JobAdded tmpItem in listBoxFreightMarketAddedJobs.Items)
            {
                JobsTotalDistance += tmpItem.Distance;
            }

            labelFreightMarketDistanceNumbers.Text = Math.Floor(JobsTotalDistance * DistanceMultiplier).ToString() + unCertainRouteLength + " " + ProgSettingsV.DistanceMes;
        }

        private void FM_JobList_Edit()
        {
            listBoxFreightMarketAddedJobs.Enabled = false;
            buttonFreightMarketAddJob.Width = 238;
            buttonFreightMarketCancelJobEdit.Visible = true;
            buttonFreightMarketCancelJobEdit.Enabled = true;

            comboBoxFreightMarketCountries.SelectedValue = "All";
            comboBoxFreightMarketCompanies.SelectedValue = "All";

            FreightMarketJob = (JobAdded)listBoxFreightMarketAddedJobs.SelectedItem;

            comboBoxFreightMarketSourceCity.SelectedValue = FreightMarketJob.SourceCity;
            comboBoxFreightMarketSourceCompany.SelectedValue = FreightMarketJob.SourceCompany;
            comboBoxFreightMarketDestinationCity.SelectedValue = FreightMarketJob.DestinationCity;
            comboBoxFreightMarketDestinationCompany.SelectedValue = FreightMarketJob.DestinationCompany;

            comboBoxFreightMarketCargoList.SelectedValue = FreightMarketJob.Cargo;
            comboBoxFreightMarketUrgency.SelectedValue = FreightMarketJob.Urgency;
            comboBoxFreightMarketTrailerDef.SelectedValue = FreightMarketJob.TrailerDefinition;
            comboBoxFreightMarketTrailerVariant.SelectedValue = FreightMarketJob.TrailerVariant;

            buttonFreightMarketAddJob.Text = "Edit Job";
            buttonFreightMarketAddJob.Click -= buttonAddJob_Click;
            buttonFreightMarketAddJob.Click += buttonEditJob_Click;
        }

        //end Freight market tab

        //Cargo Market tab
        private void FillFormCargoOffersControls()
        {
            FillCargoMarketCities();
            FillTrailerTypesCM();
        }

        private void FillCargoMarketCities()
        {
            DataTable combDT = new DataTable();
            DataColumn dc = new DataColumn("City", typeof(string));
            combDT.Columns.Add(dc);

            dc = new DataColumn("CityName", typeof(string));
            combDT.Columns.Add(dc);

            //start filling

            //fill source and destination cities
            foreach (City tempcity in from x in CitiesList where !x.Disabled select x)
            {
                combDT.Rows.Add(tempcity.CityName, tempcity.CityNameTranslated);
                /*
                CitiesLngDict.TryGetValue(tempcity.CityName, out string value);
                if (value != null && value != "")
                    combDT.Rows.Add(tempcity.CityName, value);
                else
                {
                    combDT.Rows.Add(tempcity.CityName, tempcity.CityName + " -n");
                }
                */
            }

            combDT.DefaultView.Sort = "CityName ASC";

            comboBoxCargoMarketSourceCity.ValueMember = "City";
            comboBoxCargoMarketSourceCity.DisplayMember = "CityName";
            comboBoxCargoMarketSourceCity.DataSource = combDT;

            comboBoxCargoMarketSourceCity.SelectedValue = LastVisitedCity;
        }

        private void comboBoxSourceCityCM_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxCargoMarketSourceCity.SelectedIndex >= 0)
            {
                comboBoxSourceCargoMarketCompany.SelectedIndex = -1;
                comboBoxSourceCargoMarketCompany.Text = "";

                SetupSourceCompaniesCM();
            }

            if (comboBoxSourceCargoMarketCompany.Items.Count > 0)
            {
                comboBoxSourceCargoMarketCompany.SelectedIndex = RandomValue.Next(comboBoxSourceCargoMarketCompany.Items.Count);
            }
        }

        private void SetupSourceCompaniesCM()
        {
            List<Company> CityCompanies = CitiesList.Find(x => x.CityName == comboBoxCargoMarketSourceCity.SelectedValue.ToString()).ReturnCompanies();
            List<Company> RealCompanies = CityCompanies.FindAll(x => !x.Excluded);

            DataTable combDT = new DataTable();
            DataColumn dc = new DataColumn("Company", typeof(string));
            combDT.Columns.Add(dc);

            dc = new DataColumn("CompanyName", typeof(string));
            combDT.Columns.Add(dc);

            foreach (Company company in RealCompanies)
            {
                combDT.Rows.Add(company.CompanyName, company.CompanyNameTranslated);
                /*
                CompaniesLngDict.TryGetValue(company.CompanyName, out string value);
                if (value != null && value != "")
                {
                    combDT.Rows.Add(company.CompanyName, value);
                }
                else
                {
                    combDT.Rows.Add(company.CompanyName, company.CompanyName + " -n");
                }
                */
            }

            combDT.DefaultView.Sort = "CompanyName ASC";

            comboBoxSourceCargoMarketCompany.ValueMember = "Company";
            comboBoxSourceCargoMarketCompany.DisplayMember = "CompanyName";
            comboBoxSourceCargoMarketCompany.DataSource = combDT;
        }

        private void comboBoxSourceCompanyCM_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxSourceCargoMarketCompany.SelectedValue != null && ExternalCompanies.Count > 0)
            {
                PreparePossibleCargoes();
            }

            PrintCargoSeeds();
        }

        private void comboBoxCMTrailerTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            PreparePossibleCargoes();
        }

        private void PreparePossibleCargoes()
        {
            listBoxCargoMarketCargoListForCompany.Items.Clear();

            ExtCompany t = ExternalCompanies.Find(x => x.CompanyName == comboBoxSourceCargoMarketCompany.SelectedValue.ToString());

            if (t != null)
            {
                List<string> oC = t.outCargo;

                if (oC != null)
                    foreach (string cargo in oC)
                    {
                        if (comboBoxCMTrailerTypes.SelectedValue != null && ExtCargoList.Count > 0)
                        {
                            ExtCargo temp = ExtCargoList.Find(x => x.CargoName == cargo);
                            if (temp != null)
                            {
                                if (temp.BodyTypes.Contains(comboBoxCMTrailerTypes.SelectedValue.ToString()))
                                    listBoxCargoMarketCargoListForCompany.Items.Add(cargo);
                            }
                        }
                    }
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            PrintCargoSeeds();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            PrintCargoSeeds();
        }

        private void PrintCargoSeeds()
        {
            listBoxCargoMarketSourceCargoSeeds.Items.Clear();

            if (comboBoxSourceCargoMarketCompany.SelectedValue != null) //&& ExternalCompanies.Count > 0)
            {
                //List<string> tempOutCargo = ExternalCompanies.Find(x => x.CompanyName == comboBoxSourceCompanyCM.SelectedValue.ToString()).outCargo;

                //int seedindex = 0;
                foreach (int cargoseed in CitiesList.Find(x => x.CityName == comboBoxCargoMarketSourceCity.SelectedValue.ToString()).ReturnCompanies().Find(x => x.CompanyName == comboBoxSourceCargoMarketCompany.SelectedValue.ToString()).CragoSeeds)
                {
                    //int Cargoreminder2 = (cargoseed - InGameTime + (int)numericUpDown2.Value) % (tempOutCargo.Count() - (int)numericUpDown1.Value);
                    //tempOutCargo.Sort();

                    string cargoforseed = "";
                    /*
                    if(listBoxCargoMarketCargoListForCompany.Items.Count > 0)
                    {
                        cargoforseed = "| Cargo ";
                        string[] tempOutCargo = new string[listBoxCargoMarketCargoListForCompany.Items.Count];
                        listBoxCargoMarketCargoListForCompany.Items.CopyTo(tempOutCargo, 0);

                        int Cargoreminder = (int) (cargoseed - InGameTime - seedindex) % tempOutCargo.Count();
                        cargoforseed += tempOutCargo[Cargoreminder];
                    }
                    */
                    listBoxCargoMarketSourceCargoSeeds.Items.Add("" + cargoseed.ToString().PadRight(12, ' ') + " | Time left " + ((cargoseed - InGameTime) / 60).ToString().PadLeft(2) + " h " + 
                        ((cargoseed - InGameTime) % 60).ToString().PadLeft(2) + " m " + cargoforseed);

                    //seedindex++;
                }
            }
        }
        
        private void buttonCargoMarketRandomizeCargoCompany_Click(object sender, EventArgs e)
        {
            List<Company> CityCompanies = CitiesList.Find(x => x.CityName == comboBoxCargoMarketSourceCity.SelectedValue.ToString()).ReturnCompanies();
            List<Company> RealCompanies = CityCompanies.FindAll(x => !x.Excluded);

            int[] tempseeds = new int[10];

            for(int i=0; i< tempseeds.Length; i++)
            {
                tempseeds[i] = InGameTime + RandomValue.Next(180, 1800);
            }

            RealCompanies.Find(x => x.CompanyName == comboBoxSourceCargoMarketCompany.SelectedValue.ToString()).CragoSeeds = tempseeds;

            PrintCargoSeeds();
        }

        private void buttonCargoMarketResetCargoCompany_Click(object sender, EventArgs e)
        {
            List<Company> CityCompanies = CitiesList.Find(x => x.CityName == comboBoxCargoMarketSourceCity.SelectedValue.ToString()).ReturnCompanies();
            List<Company> RealCompanies = CityCompanies.FindAll(x => !x.Excluded);

            RealCompanies.Find(x => x.CompanyName == comboBoxSourceCargoMarketCompany.SelectedValue.ToString()).CragoSeeds = new int[0];

            PrintCargoSeeds();
        }

        private void buttonCargoMarketRandomizeCargoCity_Click(object sender, EventArgs e)
        {
            List<Company> CityCompanies = CitiesList.Find(x => x.CityName == comboBoxCargoMarketSourceCity.SelectedValue.ToString()).ReturnCompanies();
            List<Company> RealCompanies = CityCompanies.FindAll(x => !x.Excluded);

            foreach (Company company in RealCompanies)
            {
                int[] tempseeds = new int[10];

                for (int i = 0; i < tempseeds.Length; i++)
                {
                    tempseeds[i] = InGameTime + RandomValue.Next(180, 1800);
                }

                company.CragoSeeds = tempseeds;
            }

            PrintCargoSeeds();
        }

        private void buttonCargoMarketResetCargoCity_Click(object sender, EventArgs e)
        {
            List<Company> CityCompanies = CitiesList.Find(x => x.CityName == comboBoxCargoMarketSourceCity.SelectedValue.ToString()).ReturnCompanies();
            List<Company> RealCompanies = CityCompanies.FindAll(x => !x.Excluded);

            foreach (Company company in RealCompanies)
            {
                company.CragoSeeds = new int[0];
            }

            PrintCargoSeeds();
        }

        private void FillTrailerTypesCM()
        {
            DataTable combDT = new DataTable();
            DataColumn dc = new DataColumn("TrailerType", typeof(string));
            combDT.Columns.Add(dc);

            dc = new DataColumn("TrailerTypeName", typeof(string));
            combDT.Columns.Add(dc);

            List<string> TrailerTypes = new List<string>();

            foreach(ExtCargo temp in ExtCargoList)
            {
                foreach(string temptype in temp.BodyTypes)
                {
                    if (TrailerTypes.FindIndex(x => x == temptype) == -1)
                        TrailerTypes.Add(temptype);
                }
            }

            foreach (string trailertype in TrailerTypes)
            {
                combDT.Rows.Add(trailertype, CultureInfo.InvariantCulture.TextInfo.ToTitleCase(trailertype));
            }

            combDT.DefaultView.Sort = "TrailerTypeName ASC";

            comboBoxCMTrailerTypes.ValueMember = "TrailerType";
            comboBoxCMTrailerTypes.DisplayMember = "TrailerTypeName";

            comboBoxCMTrailerTypes.DataSource = combDT;
        }
        //end Cargo Market tab

        //Convoy tools tab
        private void buttonGPSCurrentPositionCopy_Click(object sender, EventArgs e)
        {
            string tempString = "GPS_TruckPosition\r\n";

            tempString += PlayerDataV.UserCompanyAssignedTruckPlacement;
            string asd = BitConverter.ToString(zipText(tempString)).Replace("-", "");
            Clipboard.SetText(asd);
            MessageBox.Show("Truck GPS position has been copied.");
        }

        private void buttonGPSCurrentPositionPaste_Click(object sender, EventArgs e)
        {
            //UserCompanyAssignedTruckPlacement
            try
            {
                string inputData = unzipText(Clipboard.GetText());
                string[] Lines = inputData.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                if (Lines[0] == "GPS_TruckPosition")
                {
                    List<string> tempstr = new List<string>();
                    for (int i = 1; i < Lines.Length; i++)
                    {
                        tempstr.Add(Lines[i]);
                    }

                    PlayerDataV.UserCompanyAssignedTruckPlacement = tempstr[0];
                    //PlayerProfileData.UserCompanyAssignedTrailerPlacement = "(0, 0, 0) (1; 0, 0, 0)";

                    MessageBox.Show("Truck GPS position has been inserted.");
                    UserCompanyAssignedTruckPlacementEdited = true;
                }
                else
                    MessageBox.Show("Wrong data. Expected Truck GPS data but\r\n" + Lines[0] + "\r\nwas found.");
            }
            catch
            {
                MessageBox.Show("Something gone wrong with decoding.");
            }
        }

        private void buttonGPSStoredGPSPathCopy_Click(object sender, EventArgs e)
        {
            string tempData = "GPS_Path\r\n";
            //GPS Behind
            if (GPSbehind.Count > 0)
            {
                tempData += "GPSbehind\r\n";
                foreach (KeyValuePair<string, List<string>> temp in GPSbehind)
                {
                    tempData += "waypoint\r\n";
                    foreach (string tempLines in temp.Value)
                    {
                        tempData += tempLines + "\r\n";
                    }
                }
            }
            //GPS Ahead
            tempData += "GPSahead\r\n";
            foreach (KeyValuePair<string, List<string>> temp in GPSahead)
            {
                tempData += "waypoint\r\n";
                foreach (string tempLines in temp.Value)
                {
                    tempData += tempLines + "\r\n";
                }
            }
            //GPS Avoid
            tempData += "GPSavoid\r\n";
            foreach (KeyValuePair<string, List<string>> temp in GPSAvoid)
            {
                tempData += "waypoint\r\n";
                foreach (string tempLines in temp.Value)
                {
                    tempData += tempLines + "\r\n";
                }
            }

            //MessageBox.Show(tempPaint);
            string asd = BitConverter.ToString(zipText(tempData)).Replace("-", "");
            Clipboard.SetText(asd);
            MessageBox.Show("GPS Path data has been copied.");
        }

        private void buttonGPSStoredGPSPathPaste_Click(object sender, EventArgs e)
        {
            try
            {
                string inputData = unzipText(Clipboard.GetText());
                string[] Lines = inputData.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                if (Lines[0] == "GPS_Path")
                {
                    Dictionary<int, List<string>> tempGPSbehind, tempGPSahead, tempGPSavoid;

                    tempGPSbehind = new Dictionary<int, List<string>>();
                    tempGPSahead = new Dictionary<int, List<string>>();
                    tempGPSavoid = new Dictionary<int, List<string>>();

                    bool tagGPSbehind = false, tagGPSahead = false, tagGPSavoid = false;

                    for (int i = 1; i < Lines.Length; i++)
                    {
                        //GPSbehind
                        if (Lines[i].StartsWith("GPSbehind"))
                        {
                            tagGPSbehind = true;
                            continue;
                        }

                        if (tagGPSbehind)
                        {
                            int wp = 0;
                            do
                            {
                                if (Lines[i].StartsWith("waypoint"))
                                {
                                    i++;
                                    List<string> tmpList = new List<string>();

                                    while (!Lines[i].StartsWith("waypoint") && !Lines[i].StartsWith("GPSahead") && Lines[i] != "" && i < Lines.Length)
                                    {
                                        tmpList.Add(Lines[i]);
                                        i++;
                                    }

                                    tempGPSbehind.Add(wp, tmpList);
                                    wp++;
                                }
                            }
                            while (!Lines[i].StartsWith("GPSahead") && Lines[i] != "" && i < Lines.Length);

                            tagGPSbehind = false;
                        }

                        //GPSahead
                        if (Lines[i].StartsWith("GPSahead"))
                        {
                            tagGPSahead = true;
                            continue;
                        }

                        if (tagGPSahead)
                        {
                            int wp = 0;
                            do
                            {
                                if (Lines[i].StartsWith("waypoint"))
                                {
                                    i++;
                                    List<string> tmpList = new List<string>();

                                    while (!Lines[i].StartsWith("waypoint") && Lines[i] != "" && i < Lines.Length)
                                    {
                                        tmpList.Add(Lines[i]);
                                        i++;
                                    }

                                    tempGPSahead.Add(wp, tmpList);
                                    wp++;
                                }
                            }
                            while (i < Lines.Length && Lines[i] != "");

                            tagGPSahead = false;
                        }

                        //GPS Avoid
                        if (Lines[i].StartsWith("GPSavoid"))
                        {
                            tagGPSavoid = true;
                            continue;
                        }

                        if (tagGPSavoid)
                        {
                            int wp = 0;
                            do
                            {
                                if (Lines[i].StartsWith("waypoint"))
                                {
                                    i++;
                                    List<string> tmpList = new List<string>();

                                    while (!Lines[i].StartsWith("waypoint") && !Lines[i].StartsWith("GPSavoid") && Lines[i] != "" && i < Lines.Length)
                                    {
                                        tmpList.Add(Lines[i]);
                                        i++;
                                    }

                                    tempGPSavoid.Add(wp, tmpList);
                                    wp++;
                                }
                            }
                            while (!Lines[i].StartsWith("GPSavoid") && Lines[i] != "" && i < Lines.Length);

                            tagGPSavoid = false;
                        }
                    }

                    //GPSbehind = tempGPSbehind
                    if (tempGPSbehind.Count > 0)
                    {
                        GPSbehind.Clear();
                        foreach (KeyValuePair<int, List<string>> temp in tempGPSbehind)
                        {
                            GPSbehind.Add(GetSpareNameless(), temp.Value);
                        }
                    }

                    //GPSahead = tempGPSahead
                    if (tempGPSahead.Count > 0)
                    {
                        GPSahead.Clear();
                        foreach (KeyValuePair<int, List<string>> temp in tempGPSahead)
                        {
                            GPSahead.Add(GetSpareNameless(), temp.Value);
                        }
                    }

                    //GPSavoid = tempGPSavoid
                    if (tempGPSavoid.Count > 0)
                    {
                        GPSAvoid.Clear();
                        foreach (KeyValuePair<int, List<string>> temp in tempGPSavoid)
                        {
                            GPSAvoid.Add(GetSpareNameless(), temp.Value);
                        }
                    }

                    MessageBox.Show("GPS Path data  has been inserted.");
                }
                else
                    MessageBox.Show("Wrong data. Expected GPS Path data but\r\n" + Lines[0] + "\r\nwas found.");
            }
            catch
            {
                MessageBox.Show("Something gone wrong with decoding.");
            }
        }

        private void buttonConvoyToolsGPSTruckPositionMultySaveCopy_Click(object sender, EventArgs e)
        {
            FormConvoyControlPositions FormWindow = new FormConvoyControlPositions(true);
            FormWindow.ShowDialog();
        }

        private void buttonConvoyToolsGPSTruckPositionMultySavePaste_Click(object sender, EventArgs e)
        {
            FormConvoyControlPositions FormWindow = new FormConvoyControlPositions(false);
            FormWindow.ShowDialog();
        }

        //end Convoy Tools tab

        //Form methods
        private void ToggleVisibility(bool visible)
        {
            foreach (TabPage tp in tabControlMain.TabPages)
            {
                tp.Enabled = visible;
            }

            if (comboBoxUserTruckCompanyTrucks.Items.Count == 0)
            {
                tabControlMain.TabPages["tabPageTruck"].Enabled = false;
            }

            if (comboBoxUserTrailerCompanyTrailers.Items.Count == 0)
            {
                tabControlMain.TabPages["tabPageTrailer"].Enabled = false;
            }

            int pSkillsNameHeight = 64, pSkillsNameWidth = 64;
            for (int i = 0; i <6;i++)
            {
               Control[] tmp =  this.Controls.Find("profileSkillsPanel" + i.ToString(), true);
                if(visible)
                {
                    Bitmap bgimg = new Bitmap(SkillImgS[i], pSkillsNameHeight, pSkillsNameWidth);
                    tmp[0].BackgroundImage = bgimg;
                }
                else
                    tmp[0].BackgroundImage = ConvertBitmapToGrayscale(tmp[0].BackgroundImage);
            }
        }

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
                    ToggleVisibility(false);
                    buttonMainWriteSave.Enabled = false;
                    buttonMainDecryptSave.Enabled = true;
                    SetDefaultValues(false);
                }
            }

            if (_game == "ETS2")
            {
                Globals.CurrentGame = dictionaryProfiles["ETS2"];
                GameType = _game;
            }
            else
            {
                Globals.CurrentGame = dictionaryProfiles["ATS"];
                GameType = _game;
            }
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
        //Master
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

                HelpTranslateFormMethod(this, ResourceManagerMain, ci);
                HelpTranslateMenuStripMethod(menuStripMain, ResourceManagerMain, ci);

                this.ResumeLayout();

                for (int i = 0; i < 6; i++)
                {
                    string translatedString = ResourceManagerMain.GetString("labelProfileSkillName" + i.ToString(), ci);

                    foreach (Control c in groupBoxProfileSkill.Controls)
                    {
                        if(c.Name == "profileSkillsPanel" + i.ToString())
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

                AddTranslationToData();
                RefreshComboboxes();
                CorrectControlsPositions();
            }
            catch
            {
            }            
            //rm.ReleaseAllResources();
        }
        //Help methods for searching controls
        internal void HelpTranslateFormMethod (Control parent, PlainTXTResourceManager _rm, CultureInfo _ci)
        {
            char[] charsToTrim = { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
            foreach (Control c in parent.Controls)
            {
                try
                {

                    string translatedString = _rm.GetString(c.Name.TrimEnd(charsToTrim), _ci);
                    if (translatedString != null)
                        c.Text = translatedString;
                }
                catch
                {   
                }
                HelpTranslateFormMethod(c, _rm, _ci);
            }
        }

        private void HelpTranslateMenuStripMethod(MenuStrip parent, PlainTXTResourceManager _rm, CultureInfo _ci)
        {
            foreach (ToolStripMenuItem c in parent.Items)
            {
                try
                {
                    string translatedString = _rm.GetString(c.Name, _ci);
                    if (translatedString != null)
                        c.Text = translatedString;
                }
                catch
                {
                }
                HelpTranslateMenuStripDDMethod(c, _rm, _ci);
            }
        }

        private void HelpTranslateMenuStripDDMethod(ToolStripDropDownItem parent, PlainTXTResourceManager _rm, CultureInfo _ci)
        {
            try
            {
                foreach (object c in parent.DropDownItems)
                {
                    if (c is ToolStripDropDownItem)
                    {
                        ToolStripDropDownItem thisbutton = c as ToolStripDropDownItem;

                        string translatedString = _rm.GetString(thisbutton.Name, _ci);
                        if (translatedString != null)
                            thisbutton.Text = translatedString;

                        HelpTranslateMenuStripDDMethod(thisbutton, _rm, _ci);
                    }
                }
            }
            catch
            {
            }

        }
        //Correct positions
        private void CorrectControlsPositions()
        {
            //Truck
            Panel pbTruckFuel = (Panel)tabControlMain.TabPages[2].Controls.Find("progressbarTruckFuel",true)[0];
            Label Flabel = (Label)tabControlMain.TabPages[2].Controls.Find("labelTruckDetailsFuel", true)[0];

            Flabel.Location = new Point(pbTruckFuel.Location.X + (pbTruckFuel.Width - Flabel.Width) / 2, pbTruckFuel.Location.Y + pbTruckFuel.Height + 10);

            //Freight Market
            labelFreightMarketDistanceNumbers.Location = new Point( labelFreightMarketDistance.Location.X + labelFreightMarketDistance.Width + 6, labelFreightMarketDistanceNumbers.Location.Y);
        }
        //Translate CB
        private void RefreshComboboxes()
        {
            int savedindex = 0, j = 0;
            string savedvalue = "", ntFormat = " -nt";
            DataTable temptable = new DataTable();
            
            //Countries ComboBoxes
            temptable = comboBoxFreightMarketCountries.DataSource as DataTable;
            if (temptable != null)
            {
                savedindex = comboBoxFreightMarketCountries.SelectedIndex;

                if (savedindex != -1)
                    savedvalue = comboBoxFreightMarketCountries.SelectedValue.ToString();

                comboBoxFreightMarketCountries.SelectedIndexChanged -= comboBoxCountries_SelectedIndexChanged;
                //i = 0;
                foreach (DataRow temp in temptable.Rows)
                {
                    string source = temp[0].ToString();

                    CountriesLngDict.TryGetValue(source, out string value);

                    if (value != null && value != "")
                    {
                        temp[1] = value;
                    }
                    else
                    {
                        string CapName = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(source);
                        temp[1] = CapName;
                    }
                }

                DataTable sortedDT = temptable.DefaultView.Table.Copy();

                DataView dv = sortedDT.DefaultView;
                dv.Sort = "CountryName ASC";
                sortedDT = dv.ToTable();
                sortedDT.DefaultView.Sort = "";

                DataRow sourceRow = sortedDT.Select("Country = 'All'")[0];
                int rowi = sortedDT.Rows.IndexOf(sourceRow);

                DataRow row = sortedDT.NewRow();
                row.ItemArray = sourceRow.ItemArray;

                sortedDT.Rows.RemoveAt(rowi);
                sortedDT.Rows.InsertAt(row, 0);


                comboBoxFreightMarketCountries.DataSource = sortedDT;
                
                if (savedindex != -1)
                    comboBoxFreightMarketCountries.SelectedValue = savedvalue;
                else
                    comboBoxFreightMarketCountries.SelectedValue = "All";

                comboBoxFreightMarketCountries.SelectedIndexChanged += comboBoxCountries_SelectedIndexChanged;
            }
            
            //Companies
            temptable = comboBoxFreightMarketCompanies.DataSource as DataTable;

            if (temptable != null)
            {
                savedindex = comboBoxFreightMarketCompanies.SelectedIndex;

                if (savedindex != -1)
                    savedvalue = comboBoxFreightMarketCompanies.SelectedValue.ToString();

                comboBoxFreightMarketCompanies.SelectedIndexChanged -= comboBoxCompanies_SelectedIndexChanged;
                //i = 0;
                foreach (DataRow temp in temptable.Rows)
                {
                    string source = temp[0].ToString();

                    CompaniesLngDict.TryGetValue(source, out string value);

                    if (value != null && value != "")
                    {
                        temp[1] = value;
                    }
                    else
                    {
                        temp[1] = source + ntFormat;
                    }
                }

                DataTable sortedDT = temptable.DefaultView.Table.Copy();

                DataView dv = sortedDT.DefaultView;
                dv.Sort = "CompanyName ASC";
                sortedDT = dv.ToTable();
                sortedDT.DefaultView.Sort = "";

                DataRow sourceRow = sortedDT.Select("Company = 'All'")[0];
                int rowi = sortedDT.Rows.IndexOf(sourceRow);

                DataRow row = sortedDT.NewRow();
                row.ItemArray = sourceRow.ItemArray;

                sortedDT.Rows.RemoveAt(rowi);
                sortedDT.Rows.InsertAt(row, 0);


                comboBoxFreightMarketCompanies.DataSource = sortedDT;

                if (savedindex != -1)
                    comboBoxFreightMarketCompanies.SelectedValue = savedvalue;
                else
                    comboBoxFreightMarketCompanies.SelectedValue = "All";

                comboBoxFreightMarketCompanies.SelectedIndexChanged += comboBoxCompanies_SelectedIndexChanged;
            }
            
            //////
            //Cities ComboBoxes
            ComboBox[] CitiesCB = { comboBoxFreightMarketSourceCity, comboBoxFreightMarketDestinationCity, comboBoxUserCompanyHQcity, comboBoxCargoMarketSourceCity };
            EventHandler[] CitiesCBeh = { comboBoxSourceCity_SelectedIndexChanged, comboBoxDestinationCity_SelectedIndexChanged, comboBoxUserCompanyHQcity_SelectedIndexChanged, comboBoxSourceCityCM_SelectedIndexChanged };
            j = 0;
            foreach (ComboBox tempCB in CitiesCB)
            {
                temptable = tempCB.DataSource as DataTable;
                if (temptable != null)
                {
                    savedindex = tempCB.SelectedIndex;

                    if (savedindex != -1)
                        savedvalue = tempCB.SelectedValue.ToString();

                    tempCB.SelectedIndexChanged -= CitiesCBeh[j];
                    //i = 0;
                    foreach (DataRow temp in temptable.Rows)
                    {
                        string source = temp[0].ToString();

                        CitiesLngDict.TryGetValue(source, out string value);

                        if (value != null && value != "")
                        {
                            temp[1] = value;
                        }
                        else
                        {
                            temp[1] = source + ntFormat;
                        }
                    }

                    if (savedindex != -1)
                        tempCB.SelectedValue = savedvalue;

                    tempCB.SelectedIndexChanged += CitiesCBeh[j];
                    j++;
                }
            }

            //////
            //Companies ComboBoxes
            ComboBox[] CompaniesCB = { comboBoxFreightMarketSourceCompany, comboBoxFreightMarketDestinationCompany, comboBoxSourceCargoMarketCompany };
            EventHandler[] CompaniesCBeh = { comboBoxSourceCompany_SelectedIndexChanged, comboBoxDestinationCompany_SelectedIndexChanged, comboBoxSourceCompanyCM_SelectedIndexChanged };
            j = 0;
            foreach (ComboBox tempCB in CompaniesCB)
            {
                temptable = tempCB.DataSource as DataTable;
                if (temptable != null)
                {
                    savedindex = tempCB.SelectedIndex;

                    if (savedindex != -1)
                        savedvalue = tempCB.SelectedValue.ToString();

                    tempCB.SelectedIndexChanged -= CompaniesCBeh[j];

                    //i = 0;
                    foreach (DataRow temp in temptable.Rows)
                    {
                        string source = temp[0].ToString();

                        CompaniesLngDict.TryGetValue(source, out string value);

                        if (value != null && value != "")
                        {
                            temp[1] = value;
                        }
                        else
                        {
                            temp[1] = source + ntFormat;
                        }
                    }

                    if (savedindex != -1)
                        tempCB.SelectedValue = savedvalue;

                    tempCB.SelectedIndexChanged += CompaniesCBeh[j];
                    j++;
                    }
            }

            //Freight Market
            //Cargo
            temptable = comboBoxFreightMarketCargoList.DataSource as DataTable;
            if (temptable != null)
            {
                savedindex = comboBoxFreightMarketCargoList.SelectedIndex;

                if (savedindex != -1)
                    savedvalue = comboBoxFreightMarketCargoList.SelectedValue.ToString();

                comboBoxFreightMarketCargoList.SelectedIndexChanged -= comboBoxCargoList_SelectedIndexChanged;

                //i = 0;
                foreach (DataRow temp in temptable.Rows)
                {
                    string source = temp[0].ToString();

                    CargoLngDict.TryGetValue(source, out string value);

                    if (value != null && value != "")
                    {
                        temp[1] = value;
                    }
                    else
                    {
                        temp[1] = source + ntFormat;
                    }
                }

                if (savedindex != -1)
                    comboBoxFreightMarketCargoList.SelectedValue = savedvalue;

                comboBoxFreightMarketCargoList.SelectedIndexChanged += comboBoxCargoList_SelectedIndexChanged;
            }

            //Urgency
            temptable = comboBoxFreightMarketUrgency.DataSource as DataTable;
            if (temptable != null)
            {
                //i = 0;
                foreach (DataRow temp in temptable.Rows)
                {
                    string source = temp[0].ToString();

                    UrgencyLngDict.TryGetValue(source, out string value);

                    if (value != null && value != "")
                    {
                        temp[1] = value;
                    }
                    else
                    {
                        temp[1] = source + ntFormat;
                    }
                }
            }

            //////
            //ListBoxes
            FillVisitedCities(listBoxVisitedCities.TopIndex);
            FillGaragesList(listBoxGarages.TopIndex);

            listBoxFreightMarketAddedJobs.Refresh();
        }
        //Get translation line
        private string GetranslatedString(string _key)
        {
            CultureInfo ci = Thread.CurrentThread.CurrentUICulture;

            try
            {
                PlainTXTResourceManager rm = new PlainTXTResourceManager();

                string resultString = rm.GetString(_key, ci);

                if(resultString != null)
                    return resultString;
                else
                    return _key;
            }
            catch
            {
                return _key;
            }
        }

        private void AddTranslationToData()
        {
            string ntFormat = " -nt";
            //Countries
            /*
            foreach (string tempitem in CountriesList)
            {
                CountriesLngDict.TryGetValue(tempitem, out string value);

                if (value != null && value != "")
                {
                    tempitem.CountryNameTranslated = value;
                }
                else
                {
                    string CapName = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(tempitem);
                    tempitem.CountryNameTranslated = CapName;
                }
            }
            */
            //Cities
            foreach (City _city in from x in CitiesList where !x.Disabled select x)
            {
                CitiesLngDict.TryGetValue(_city.CityName, out string _translated);

                if (_translated != null && _translated != "")
                {
                    _city.CityNameTranslated = _translated;
                }
                else
                {
                    _city.CityNameTranslated = _city.CityName + ntFormat;
                }
            }

            CitiesList = CitiesList.OrderBy(x => x.CityNameTranslated).ToList();

            //Garages
            foreach (Garages _garage in GaragesList)
            {
                CitiesLngDict.TryGetValue(_garage.GarageName, out string _translated);

                if (_translated != null && _translated != "")
                {
                    _garage.GarageNameTranslated = _translated;
                }
                else
                {
                    _garage.GarageNameTranslated = _garage.GarageName + ntFormat;
                }
            }

            GaragesList = GaragesList.OrderBy(x => x.GarageNameTranslated).ToList();

            //Companies


        }
        //Language End

        //IMG
        //Custom PB color gradient
        private void CreateProgressBarBitmap()
        {
            ProgressBarGradient = new Bitmap(100, 1);

            LinearGradientBrush br = new LinearGradientBrush(new RectangleF(0, 0, 100, 1), Color.Black, Color.Black, 0, false);
            ColorBlend cb = new ColorBlend();

            cb.Positions = new[] { 0.0f, 0.5f, 1f };
            cb.Colors = new[] { Color.FromArgb(255, 255, 0, 0), Color.FromArgb(255, 255, 255, 0), Color.FromArgb(255, 0, 255, 0), };

            br.InterpolationColors = cb;

            //puts the gradient scale onto a bitmap which allows for getting a color from pixel
            Graphics g = Graphics.FromImage(ProgressBarGradient);
            g.FillRectangle(br, new RectangleF(0, 0, ProgressBarGradient.Width, ProgressBarGradient.Height));
        }

        private Color GetProgressbarColor(decimal _value)
        {
            if (_value < 0)
                _value = 0;
            else if (_value > 1)
                _value = 1;
            return ProgressBarGradient.GetPixel(Convert.ToInt32((1 - _value) * 99), 0);
        }

        private Bitmap ConvertBitmapToGrayscale(Image _source)
        {
            Bitmap bm = new Bitmap(_source);
            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(bm);

            //create the grayscale ColorMatrix
            ColorMatrix colorMatrix = new ColorMatrix(
               new float[][]
               {
                 new float[] {.299f, .299f, .299f, 0, 0},
                 new float[] {.587f, .587f, .587f, 0, 0},
                 new float[] {.114f, .114f, .114f, 0, 0},
                 new float[] {0, 0, 0, 1, 0},
                 new float[] {0, 0, 0, 0, 1}
               });

            //create some image attributes
            ImageAttributes attributes = new ImageAttributes();

            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);

            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(_source, new Rectangle(0, 0, _source.Width, _source.Height), 0, 0, _source.Width, _source.Height, GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object
            g.Dispose();

            return bm;
        }
        //IMG End

        //ZIP text
        internal byte[] zipText(string _text)
        {
            if (_text == null)
                return null;

            using (Stream memOutput = new MemoryStream())
            {
                using (GZipOutputStream zipOut = new GZipOutputStream(memOutput))
                {
                    using (StreamWriter writer = new StreamWriter(zipOut))
                    {
                        writer.Write(_text);

                        writer.Flush();
                        zipOut.Finish();

                        byte[] bytes = new byte[memOutput.Length];
                        memOutput.Seek(0, SeekOrigin.Begin);
                        memOutput.Read(bytes, 0, bytes.Length);

                        return bytes;
                    }
                }
            }
        }

        internal string unzipText(string _sbytes)
        {
            string[] pairs = new string[_sbytes.Length / 2];
            byte[] bytes;

            for (int i = 0; i < _sbytes.Length / 2; i++)
            {
                pairs[i] = _sbytes.Substring(i * 2, 2);
            }

            bytes = new byte[pairs.Length];

            for (int j = 0; j < pairs.Length; j++)
                bytes[j] = Convert.ToByte(pairs[j], 16);

            if (bytes == null)
                return null;

            using (Stream memInput = new MemoryStream(bytes))
            using (GZipInputStream zipInput = new GZipInputStream(memInput))
            using (StreamReader reader = new StreamReader(zipInput))
            {
                string text = reader.ReadToEnd();

                return text;
            }
        }
        //ZIP End

        //DateTime
        public DateTime UnixTimeStampToDateTime(double _unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(_unixTimeStamp).ToLocalTime();

            return dtDateTime;
        }

        public double DateTimeToUnixTimeStamp(DateTime _dateTime)
        {
            //Data time to utc
            _dateTime = _dateTime.ToUniversalTime();

            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            double unixTimeStamp = _dateTime.Subtract(dtDateTime).TotalSeconds;
            return unixTimeStamp;
        }
        //DateTime End

        //Extra
        //Search index in CB by Value
        private int FindByValue (ComboBox _inputComboBox, string _value)
        {
            DataTable _combDT = new DataTable();
            _combDT = _inputComboBox.DataSource as DataTable;

            string fcol = _combDT.Columns[0].ToString();

            string _searchExpression = fcol + " = '" + _value + @"'";
            DataRow[] _foundRows = _combDT.Select(_searchExpression);

            if (_foundRows.Length > 0)
                return 0;
            else
                return -1;
        }

        static string NullToString(object _value)
        {
            return _value == null ? "null" : _value.ToString();
        }

        //Iterating throught nameless
        private string GetSpareNameless()
        {
            if (namelessLast == "")
            {
                namelessLast = namelessList.Last();
            }

            ushort _incr = 48;

            string[] _namelessNumbers = namelessLast.Split(new char[] { '.' });
            ushort[] _namelessNumArray = new ushort[_namelessNumbers.Length];

            Array.Reverse(_namelessNumbers);
            bool _first = true, _overflow = false;

            for (int i = 0; i < _namelessNumbers.Length; i++)
            {
                _namelessNumArray[i] = UInt16.Parse(_namelessNumbers[i], NumberStyles.HexNumber);

                try
                {
                    if (_first)
                    {
                        _namelessNumArray[i] = checked((ushort)(_namelessNumArray[i] + _incr));
                    }
                    else
                    if (_overflow)
                    {
                        _namelessNumArray[i] = checked((ushort)(_namelessNumArray[i] + 1));
                        _overflow = false;
                    }
                }
                catch (OverflowException)
                {
                    if (_first)
                    {
                        _namelessNumArray[i] = (ushort)(_namelessNumArray[i] + _incr);
                    }
                    else
                    {
                        _namelessNumArray[i] = (ushort)(_namelessNumArray[i] + 1);
                    }
                    _overflow = true;
                }

                if (i == (_namelessNumbers.Length - 1) && _overflow)
                {
                    Array.Resize(ref _namelessNumArray, _namelessNumArray.Length + 1);

                    _namelessNumArray[_namelessNumbers.Length - 1] = 1;
                }

                if (_first)
                    _first = false;
            }

            namelessLast = "";

            for (int i = 0; i < _namelessNumArray.Length; i++)
            {
                if (i < _namelessNumArray.Length - 1)
                {
                    namelessLast = "." + _namelessNumArray[i].ToString("x4") + namelessLast;
                }
                else
                {
                    namelessLast = _namelessNumArray[i].ToString("x") + namelessLast;
                }
            }
            //namelessLast
            return namelessLast;
        }

        //end Form methods
    }
}