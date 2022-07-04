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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;

namespace TS_SE_Tool
{
    public enum SMStatus : byte
    {
        Clear = 0,
        Info = 1,
        Error = 2
    }
    
    public delegate void AddStatusMessageDelegate(SMStatus _status, string _message, string _option);
    public delegate DialogResult AddStatusMessageBoxDelegate(FormMain _this, string _text, string _caption, MessageBoxButtons _buttons, MessageBoxIcon _icon);

    public static class UpdateStatusBarMessage
    {
        public static FormMain MainForm;

        public static event AddStatusMessageDelegate OnNewStatusMessage;
        public static event AddStatusMessageBoxDelegate OnNewMessageBox;

        public static void ShowStatusMessage(SMStatus _status)
        {
            ThreadSafeStatusMessage(_status, null, null);
        }

        public static void ShowStatusMessage(SMStatus _status, string _message)
        {
            ThreadSafeStatusMessage(_status, _message, null);
        }

        public static void ShowStatusMessage(SMStatus _status, string _message, string _option)
        {
            ThreadSafeStatusMessage(_status, _message, _option);
        }

        private static void ThreadSafeStatusMessage(SMStatus _status, string _message, string _option)
        {
            if (MainForm != null && MainForm.InvokeRequired)        // we are in a different thread to the main window
                MainForm.Invoke(new AddStatusMessageDelegate(ThreadSafeStatusMessage), new object[] { _status, _message, _option });     // call self from main thread
            else
                OnNewStatusMessage(_status, _message, _option);
        }
        
        public static DialogResult ShowMessageBox(FormMain _this, string _text, string _caption, MessageBoxButtons _buttons )
        {
            return ThreadSafeMessageBox(_this, _text, _caption, _buttons);
        }

        public static DialogResult ShowMessageBox(FormMain _this, string _text, string _caption, MessageBoxButtons _buttons, MessageBoxIcon _icon)
        {
            return ThreadSafeMessageBox(_this, _text, _caption, _buttons, _icon);
        }

        private static DialogResult ThreadSafeMessageBox(FormMain _this, string _text, string _caption, MessageBoxButtons _buttons)
        {
            if (MainForm != null && MainForm.InvokeRequired)
                return (DialogResult)MainForm.Invoke(new AddStatusMessageBoxDelegate(ThreadSafeMessageBox), new object[] { _this, _text, _caption, _buttons, MessageBoxIcon.None });
            else
                return (DialogResult)OnNewMessageBox(_this, _text, _caption, _buttons, MessageBoxIcon.None);
        }

        private static DialogResult ThreadSafeMessageBox(FormMain _this, string _text, string _caption, MessageBoxButtons _buttons, MessageBoxIcon _icon)
        {
            if (MainForm != null && MainForm.InvokeRequired)
                return (DialogResult)MainForm.Invoke(new AddStatusMessageBoxDelegate(ThreadSafeMessageBox), new object[] { _this, _text, _caption, _buttons, _icon });
            else
                return (DialogResult)OnNewMessageBox(_this, _text, _caption, _buttons, _icon);
        }
    }

    public partial class FormMain
    {
        void UpdateStatusBarMessage_OnNewStatusMessage(SMStatus _status)
        {
            UpdateStatusBarMessage_OnNewStatusMessage(_status, null, null);
        }

        void UpdateStatusBarMessage_OnNewStatusMessage(SMStatus _status, string _message)
        {
            UpdateStatusBarMessage_OnNewStatusMessage(_status, _message, null);
        }

        void UpdateStatusBarMessage_OnNewStatusMessage(SMStatus _status, string _message, string _option)
        {
            switch (_status)
            {
                case SMStatus.Clear:
                    {
                        toolStripStatusMessages.Text = "";
                        return;
                    }
                case SMStatus.Info:
                    {
                        toolStripStatusMessages.ForeColor = Color.Black;
                        break;
                    }
                case SMStatus.Error:
                    {
                        toolStripStatusMessages.ForeColor = Color.Red;
                        break;
                    }
            }

            if (_message != null)
            {
                string toolTipText = GetranslatedString(_message);

                if (_option != null)
                    toolTipText += " (" + _option + ")";

                toolStripStatusMessages.Text = toolTipText;
            }
        }

        DialogResult ShowMessageBox_OnNewMessageBox(FormMain _this, string _text, string _caption, MessageBoxButtons _buttons, MessageBoxIcon _icon)
        {
            return JR.Utils.GUI.Forms.FlexibleMessageBox.Show(_this, _text, _caption, _buttons, _icon);
        }

        public void SetDefaultValues(bool _initial)
        {
            if (_initial)
            {
                listBoxFreightMarketAddedJobs.DrawMode = DrawMode.OwnerDrawVariable;
                comboBoxFreightMarketCargoList.DrawMode = DrawMode.OwnerDrawVariable;
                comboBoxFreightMarketUrgency.DrawMode = DrawMode.OwnerDrawVariable;
                comboBoxFreightMarketTrailerDef.DrawMode = DrawMode.OwnerDrawVariable;
                comboBoxFreightMarketTrailerVariant.DrawMode = DrawMode.OwnerDrawVariable;
                
                ResourceManagerMain = new PlainTXTResourceManager();
                ProgSettingsV = new ProgSettings();

                ProgSettingsV.ProgramVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

                SupportedSavefileVersionETS2 = new int[] { 61, 62 }; //Supported save version
                SupportedGameVersionETS2 = "1.43.x - 1.44.x"; //Last game version Tested on
                //SupportedSavefileVersionATS;
                SupportedGameVersionATS = "1.43.x - 1.44.x"; //Last game version Tested on

                comboBoxPrevProfiles.FlatStyle =
                comboBoxProfiles.FlatStyle =
                comboBoxSaves.FlatStyle = FlatStyle.Flat;

                ProfileETS2 = @"\Euro Truck Simulator 2";
                ProfileATS = @"\American Truck Simulator";
                dictionaryProfiles = new Dictionary<string, string> { { "ETS2", ProfileETS2 }, { "ATS", ProfileATS } };
                GameType = "ETS2";

                LicensePlateWidth = new Dictionary<string, byte> { { "ETS2", 128 }, { "ATS", 64 } };

                CompaniesLngDict = new Dictionary<string, string>();
                CitiesLngDict = new Dictionary<string, string>();
                CountriesLngDict = new Dictionary<string, string>();
                CargoLngDict = new Dictionary<string, string>();
                UrgencyLngDict = new Dictionary<string, string>();
                TruckBrandsLngDict = new Dictionary<string, string>();
                DriverNames = new Dictionary<string, string>();
                
                CountryDictionary = new CountryDictionary();
                CountriesDataList = new Dictionary<string, Country>();

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

                string curName = ""; string[] input; List<string> curLst = new List<string>();

                #region Currency ETS2

                curName = "EUR";
                CurrencyDictConversionETS2.Add(curName, 1);
                input = new string[] { "", "€", "" };
                curLst = new List<string>(input);
                CurrencyDictFormatETS2.Add(curName, curLst);

                curName = "CHF";
                CurrencyDictConversionETS2.Add(curName, 1.142);
                input = new string[] { "", "", " CHF" };
                curLst = new List<string>(input);
                CurrencyDictFormatETS2.Add(curName, curLst);

                curName = "CZK";
                CurrencyDictConversionETS2.Add(curName, 25.88);
                input = new string[] { "", "", " Kč" };
                curLst = new List<string>(input);
                CurrencyDictFormatETS2.Add(curName, curLst);

                curName = "GBP";
                CurrencyDictConversionETS2.Add(curName, 0.875);
                input = new string[] { "", "£", "" };
                curLst = new List<string>(input);
                CurrencyDictFormatETS2.Add(curName, curLst);

                curName = "PLN";
                CurrencyDictConversionETS2.Add(curName, 4.317);
                input = new string[] { "", "", " zł" };
                curLst = new List<string>(input);
                CurrencyDictFormatETS2.Add(curName, curLst);

                curName = "HUF";
                CurrencyDictConversionETS2.Add(curName, 325.3);
                input = new string[] { "", "", " Ft" };
                curLst = new List<string>(input);
                CurrencyDictFormatETS2.Add(curName, curLst);

                curName = "DKK";
                CurrencyDictConversionETS2.Add(curName, 7.46);
                input = new string[] { "", "", " kr" };
                curLst = new List<string>(input);
                CurrencyDictFormatETS2.Add(curName, curLst);

                curName = "SEK";
                CurrencyDictConversionETS2.Add(curName, 10.52);
                input = new string[] { "", "", " kr" };
                curLst = new List<string>(input);
                CurrencyDictFormatETS2.Add(curName, curLst);

                curName = "NOK";
                CurrencyDictConversionETS2.Add(curName, 9.51);
                input = new string[] { "", "", " kr" };
                curLst = new List<string>(input);
                CurrencyDictFormatETS2.Add(curName, curLst);

                curName = "RUB";
                CurrencyDictConversionETS2.Add(curName, 77.05);
                input = new string[] { "", "₽", "" };
                curLst = new List<string>(input);
                CurrencyDictFormatETS2.Add(curName, curLst);
                #endregion

                #region Currency ATS

                curName = "USD";
                CurrencyDictConversionATS.Add(curName, 1);
                input = new string[] { "", "$", "" };
                curLst = new List<string>(input);
                CurrencyDictFormatATS.Add(curName, curLst);

                curName = "CAD";
                CurrencyDictConversionATS.Add(curName, 1.3);
                input = new string[] { "", "$", "" };
                curLst = new List<string>(input);
                CurrencyDictFormatATS.Add(curName, curLst);

                curName = "MXN";
                CurrencyDictConversionATS.Add(curName, 18.69);
                input = new string[] { "", "$", "" };
                curLst = new List<string>(input);
                CurrencyDictFormatATS.Add(curName, curLst);

                curName = "EUR";
                CurrencyDictConversionATS.Add(curName, 0.856);
                input = new string[] { "", "€", "" };
                curLst = new List<string>(input);
                CurrencyDictFormatATS.Add(curName, curLst);
                #endregion

                //Urgency
                UrgencyArray = new int[] { 0, 1, 2 };

                DistanceMultipliers = new Dictionary<string, double> { { "km", 1 }, { "mi", km_to_mile } };
                WeightMultipliers = new Dictionary<string, double> { { "kg", 1 }, { "lb", kg_to_lb } };

                ADRImgS = new Image[6];
                ADRImgSGrey = new Image[6];
                SkillImgSBG = new Image[5];
                SkillImgS = new Image[6];
                GaragesImg = new Image[1];
                GaragesHQImg = new Image[1];
                CitiesImg = new Image[2];
                UrgencyImg = new Image[3];
                CargoTypeImg = new Image[3];
                CargoType2Img = new Image[3];
                GameIconeImg = new Image[2];
                TruckPartsImg = new Image[5];
                TrailerPartsImg = new Image[4];

                AccessoriesImg = new Image[6];

                ProgUIImgsDict = new Dictionary<string, Image>();

                TabpagesImages = new ImageList();

                ADRbuttonArray = new CheckBox[6];
                SkillButtonArray = new CheckBox[5, 6];
            }

            unCertainRouteLength = "";
            FileDecoded = false;

            tempInfoFileInMemory = null;
            tempSavefileInMemory = null;
            tempProfileFileInMemory = null;

            SiiNunitData = null;

            //Game dependant
            if (GameType == "ETS2")
            {
                Globals.PlayerLevelUps = new int[] {200, 500, 700, 900, 1000, 1100, 1300, 1600, 1700, 2100, 2300, 2600, 2700,
                    2900, 3000, 3100, 3400, 3700, 4000, 4300, 4600, 4700, 4900, 5200, 5700, 5900, 6000, 6200, 6600, 6800};
                //Currency
                CurrencyDictFormat = CurrencyDictFormatETS2;
                CurrencyDictConversion = CurrencyDictConversionETS2;
                Globals.CurrencyName = ProgSettingsV.CurrencyMesETS2;
            }
            else
            {
                Globals.PlayerLevelUps = new int[] {200, 500, 700, 900, 1100, 1300, 1500, 1700, 1900, 2100, 2300, 2500, 2700,
                    2900, 3100, 3300, 3500, 3700, 4000, 4300, 4600, 4900, 5200, 5500, 5800, 6100, 6400, 6700, 7000, 7300};
                //Currency
                CurrencyDictFormat = CurrencyDictFormatATS;
                CurrencyDictConversion = CurrencyDictConversionATS;
                Globals.CurrencyName = ProgSettingsV.CurrencyMesATS;
            }

            MainSaveFileProfileData = new SaveFileProfileData();
            MainSaveFileInfoData = new SaveFileInfoData();

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

            GaragesList = new List<Garages>();
            UserTruckDictionary = new Dictionary<string, UserCompanyTruckData>();
            UserDriverDictionary = new Dictionary<string, UserCompanyDriverData>();
            
            UserTrailerDictionary = new Dictionary<string, UserCompanyTrailerData>();
            UserTrailerDefDictionary = new Dictionary<string, Save.Items.Trailer_Def>();

            extraVehicles = new List<string>();
            extraDrivers = new List<string>();

            VisitedCities = new List<VisitedCity>();

            CargoesListDB = new List<Cargo>();
            CitiesListDB = new List<string>();
            CompaniesListDB = new List<string>();
            TrailerDefinitionVariantsDB = new Dictionary<string, List<string>>();
            TrailerDefinitionListDB = new List<string>();
            TrailerVariantsListDB = new List<string>();

            DBDependencies = new List<string>();

            ExternalCompanies = new List<ExtCompany>();

            ExtCargoList = new List<ExtCargo>();

            JobsAmountAdded = 0;
            RandomValue = new Random();

            LastModifiedTimestamp = new DateTime();

            AddedJobsDictionary = new Dictionary<string, List<JobAdded>>();
            AddedJobsList = new List<JobAdded>();

            GPSbehind = new Dictionary<string, List<string>>();
            GPSahead = new Dictionary<string, List<string>>();
            GPSAvoid = new Dictionary<string, List<string>>();

            GPSbehindOnline = new Dictionary<string, List<string>>();
            GPSaheadOnline = new Dictionary<string, List<string>>();

            //namelessList = new List<string>();
            namelessLast = "";
            LoopStartCity = "";
            LoopStartCompany = "";

            RouteList = new Routes();

            components = null;

           GlobalFontMap = new Dictionary<string, Dictionary<UInt16, SCS.SCSFontLetter>>();
    }

        public void ApplySettings()
        {
            DistanceMultiplier = DistanceMultipliers[ProgSettingsV.DistanceMes];
            WeightMultiplier = WeightMultipliers[ProgSettingsV.WeightMes];
            checkBoxFreightMarketRandomDest.Checked = ProgSettingsV.ProposeRandom;
        }

        public void ApplySettingsUI()
        {
            if (SiiNunitData != null)
            {
                //Company
                FillAccountMoneyTB();
                //Freight
                RefreshFreightMarketDistance();
                listBoxFreightMarketAddedJobs.Refresh();
            }
        }

        private void AddImagesToControls()
        {
            //Main menu
            //Program
            toolStripMenuItemProgram.DropDownItems["toolStripMenuItemProgramSettings"].Image = ProgUIImgsDict["ProgramSettings"];
            toolStripMenuItemProgram.DropDownItems["toolStripMenuItemSettings"].Image = ProgUIImgsDict["Settings"];
            toolStripMenuItemProgram.DropDownItems["toolStripMenuItemExit"].Image = ProgUIImgsDict["Cross"];
            //Language
            menuStripMain.Items["toolStripMenuItemLanguage"].Image = ProgUIImgsDict["Language"];
            //Help
            toolStripMenuItemHelp.DropDownItems["toolStripMenuItemAbout"].Image = ProgUIImgsDict["Info"];
            toolStripMenuItemHelp.DropDownItems["toolStripMenuItemTutorial"].Image = ProgUIImgsDict["Question"];
            toolStripMenuItemHelp.DropDownItems["toolStripMenuItemDownload"].Image = ProgUIImgsDict["Download"];
                //Help - How to
                toolStripMenuItemTutorial.DropDownItems["toolStripMenuItemLocalPDF"].Image = ProgUIImgsDict["PDF"];
                toolStripMenuItemTutorial.DropDownItems["toolStripMenuItemYouTubeVideo"].Image = ProgUIImgsDict["YouTube"];
                //Help - Download
                toolStripMenuItemDownload.DropDownItems["toolStripMenuItemCheckUpdates"].Image = ProgUIImgsDict["NetworkCloud"];
                toolStripMenuItemDownload.DropDownItems["checkSCSForumToolStripMenuItem"].Image = ProgUIImgsDict["SCS"];
                toolStripMenuItemDownload.DropDownItems["checkTMPForumToolStripMenuItem"].Image = ProgUIImgsDict["TMP"];
                toolStripMenuItemDownload.DropDownItems["checkGitHubRelesesToolStripMenuItem"].Image = ProgUIImgsDict["github"];

            //Main controls
            radioButtonMainGameSwitchETS.Image = GameIconeImg[0];
            radioButtonMainGameSwitchATS.Image = GameIconeImg[1];
            //
            buttonProfilesAndSavesRefreshAll.BackgroundImage = ProgUIImgsDict["Reload"];
            buttonProfilesAndSavesEditProfile.BackgroundImage = ProgUIImgsDict["EditList"];
            buttonProfilesAndSavesRestoreBackup.BackgroundImage = ProgUIImgsDict["Reload"];

            //Tab pages
            tabControlMain.ImageList = TabpagesImages;

            for (int i = 0; i < TabpagesImages.Images.Count; i++)
            {
                tabControlMain.TabPages[i].ImageIndex = i;
            }
        }

        private void DetectGame()
        {
            try
            {
                //Searching for ETS2
                Process[] ets2proc = Process.GetProcessesByName("eurotrucks2");

                //Searching for ATS
                Process[] atsproc = Process.GetProcessesByName("amtrucks");

                if (atsproc.Count() > 0)
                    radioButtonMainGameSwitchATS.Checked = true;
                else
                    radioButtonMainGameSwitchETS.Checked = true;
            }
            catch
            {
                radioButtonMainGameSwitchETS.Checked = true;
            }
        }

        private void ClearFormControls(bool _initial)
        {
            this.SuspendLayout();
            //Profile
            //Level
            
            labelPlayerLevelName.Text = "*****";
            panelPlayerLevel.BackColor = Color.Transparent;
            labelPlayerLevelNumber.Text = "";

            labelPlayerExperience.Text = "0";
            labelPlayerExperienceNxtLvlThreshhold.Text = "0";
            
            //Skills
            foreach (CheckBox temp in ADRbuttonArray)
                temp.Checked = false;

            foreach (CheckBox temp in SkillButtonArray)
                temp.Checked = false;

            //User Colors
            tableLayoutPanelUserColors.RowStyles[1].Height = 0; //Hide add slot
            DeleteUserColorsButtons();

            //Company
            pictureBoxCompanyLogo.Image = null;

            textBoxUserCompanyCompanyName.Text = "";
            textBoxUserCompanyMoneyAccount.Text = "";
            comboBoxUserCompanyHQcity.DataSource = null;

            listBoxVisitedCities.Items.Clear();
            listBoxGarages.Items.Clear();

            //Truck
            comboBoxUserTruckCompanyTrucks.DataSource = null;

            //Trailer
            comboBoxUserTrailerCompanyTrailers.DataSource = null;

            //FreightMarket
            comboBoxFreightMarketCountries.DataSource = null;
            comboBoxFreightMarketCompanies.DataSource = null;

            comboBoxFreightMarketSourceCity.DataSource = null;
            comboBoxFreightMarketSourceCompany.DataSource = null;

            comboBoxFreightMarketDestinationCity.DataSource = null;
            comboBoxFreightMarketDestinationCompany.DataSource = null;

            comboBoxFreightMarketCargoList.DataSource = null;
            comboBoxFreightMarketUrgency.DataSource = null;

            comboBoxFreightMarketTrailerDef.DataSource = null;
            comboBoxFreightMarketTrailerVariant.DataSource = null;

            listBoxFreightMarketAddedJobs.Items.Clear();

            //CargoMarket
            comboBoxCargoMarketSourceCity.DataSource = null;
            comboBoxCargoMarketSourceCompany.DataSource = null;

            //
            this.ResumeLayout();
        }

        private void PopulateFormControlsk()
        {
            AddTranslationToData();
            
            FillFormProfileControls();          //Profile
            FillFormCompanyControls();          //Company
            FillUserCompanyTrucksList();        //Truck
            FillUserCompanyTrailerList();       //Trailer
            FillFormFreightMarketControls();    //FreightMarket
            FillFormCargoOffersControls();      //CargoMarket
        }

        //Help methods for searching controls
        private char[] charsToTrimTranslation = { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };

        internal void HelpTranslateFormMethod(Control parent, ToolTip _formTooltip)
        {
            CultureInfo _ci = Thread.CurrentThread.CurrentUICulture;

            foreach (Control cntrl in parent.Controls)
            {
                try
                {
                    string translatedString = ResourceManagerMain.GetString(cntrl.Name, _ci);

                    if (translatedString == null)
                        translatedString = ResourceManagerMain.GetString(cntrl.Name.TrimEnd(charsToTrimTranslation), _ci);

                    if (translatedString != null && translatedString != "")
                    {
                        cntrl.Text = translatedString;
                    }

                    if (_formTooltip != null)
                    {
                        string TolltipString = ResourceManagerMain.GetTooltipString(cntrl.Name, _ci);

                        if (TolltipString == null)                        
                            TolltipString = ResourceManagerMain.GetTooltipString(cntrl.Name.TrimEnd(charsToTrimTranslation), _ci);                        

                        if (TolltipString != null)
                        {
                            TolltipString = TolltipString.Replace("\\r\\n", Environment.NewLine);

                            if (int.TryParse(cntrl.Name.Substring(cntrl.Name.Length - 1), out int number))
                                number++;

                            _formTooltip.SetToolTip(cntrl, String.Format(TolltipString, number));
                        }   
                    }
                }
                catch
                { }

                HelpTranslateFormMethod(cntrl, _formTooltip);
            }
        }

        internal void HelpTranslateFormMethod(Control parent)
        {
            HelpTranslateFormMethod(parent, null);            
        }

        internal void HelpTranslateControl(Control thisControl)
        {
            HelpTranslateControlDiffName(thisControl, thisControl.Name);
        }

        internal void HelpTranslateControlDiffName(Control thisControl, string _newName)
        {
            CultureInfo _ci = Thread.CurrentThread.CurrentUICulture;

            try
            {
                string translatedString = ResourceManagerMain.GetString(_newName, _ci);

                if (translatedString == null)
                    translatedString = ResourceManagerMain.GetString(_newName.TrimEnd(charsToTrimTranslation), _ci);

                if (translatedString != null && translatedString != "")
                {
                    thisControl.Text = translatedString;
                }
            }
            catch
            { }
        }

        internal void HelpTranslateControlDiffName(Control thisControl, string _newName, object parameter)
        {
            CultureInfo _ci = Thread.CurrentThread.CurrentUICulture;

            try
            {
                string translatedString = ResourceManagerMain.GetString(_newName, _ci);

                if (translatedString == null)
                    translatedString = ResourceManagerMain.GetString(_newName.TrimEnd(charsToTrimTranslation), _ci);

                if (translatedString != null && translatedString != "")
                {
                    thisControl.Text = String.Format(translatedString, parameter);
                }
            }
            catch
            { }
        }

        internal void HelpTranslateControlExt(Control thisControl, object parameter)
        {
            CultureInfo _ci = Thread.CurrentThread.CurrentUICulture;

            try
            {
                string translatedString = ResourceManagerMain.GetString(thisControl.Name, _ci);

                if (translatedString == null)
                    translatedString = ResourceManagerMain.GetString(thisControl.Name.TrimEnd(charsToTrimTranslation), _ci);

                if (translatedString != null && translatedString != "")
                {
                    thisControl.Text = String.Format(translatedString, parameter);
                }
            }
            catch
            { }
        }

        private void HelpTranslateMenuStripMethod(MenuStrip parent)
        {
            CultureInfo _ci = Thread.CurrentThread.CurrentUICulture;

            foreach (ToolStripMenuItem tmpTSMI in parent.Items)
            {
                try
                {
                    string translatedString = ResourceManagerMain.GetString(tmpTSMI.Name, _ci);
                    if (translatedString != null)
                        tmpTSMI.Text = translatedString;
                }
                catch
                { }

                HelpTranslateMenuStripDDMethod(tmpTSMI, ResourceManagerMain, _ci);
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
            { }
        }

        private string[] HelpTranslateDialog(string _dialogName)
        {
            string dialogCaption = "", dialogText = "";

            try
            {
                string translatedString = ResourceManagerMain.GetString("dialogCaption" + _dialogName, Thread.CurrentThread.CurrentUICulture);

                if (translatedString == null)
                    translatedString = ResourceManagerMain.GetString("dialogCaption" + _dialogName.TrimEnd(charsToTrimTranslation), Thread.CurrentThread.CurrentUICulture);

                if (translatedString != null)
                    dialogCaption = translatedString;

                translatedString = ResourceManagerMain.GetString("dialogText" + _dialogName, Thread.CurrentThread.CurrentUICulture);

                if (translatedString == null)
                    translatedString = ResourceManagerMain.GetString("dialogText" + _dialogName.TrimEnd(charsToTrimTranslation), Thread.CurrentThread.CurrentUICulture);

                if (translatedString != null)
                    dialogText = translatedString;
            }
            catch { }

            return new string[] { dialogCaption, dialogText };
        }

        //Correct positions
        private void CorrectControlsPositions()
        {
            //Truck
            Label labelPlate = (Label)tabControlMain.TabPages["tabPageTruck"].Controls.Find("labelUserTruckLicensePlate", true).FirstOrDefault();
            if (labelPlate != null)
                tableLayoutPanelTruckLP.ColumnStyles[0] = new ColumnStyle(SizeType.Absolute, labelPlate.PreferredSize.Width);

            //Trailer
            labelPlate = (Label)tabControlMain.TabPages["tabPageTrailer"].Controls.Find("labelUserTrailerLicensePlate", true).FirstOrDefault();
            if (labelPlate != null)
                tableLayoutPanelTrailerLP.ColumnStyles[0] = new ColumnStyle(SizeType.Absolute, labelPlate.PreferredSize.Width);

            //Freight Market
            labelFreightMarketDistanceNumbers.Location = new Point( labelFreightMarketDistance.Location.X + labelFreightMarketDistance.Width + 6, labelFreightMarketDistanceNumbers.Location.Y);
        }
        
        //Translate CB

        private void translateTruckComboBox()
        {
            int savedindex = 0;
            string savedvalue = "";
            DataTable temptable = new DataTable();

            //Truck tab
            temptable = comboBoxUserTruckCompanyTrucks.DataSource as DataTable;

            if (temptable != null)
            {
                savedindex = comboBoxUserTruckCompanyTrucks.SelectedIndex;

                if (savedindex != -1)
                    savedvalue = comboBoxUserTruckCompanyTrucks.SelectedValue.ToString();

                foreach (DataRow temp in temptable.Rows)
                {
                    string source = temp[0].ToString();

                    var grg = GaragesList.Find(x => x.Vehicles.Contains(source));

                    if ((byte)temp["TruckType"] == 1) // Users
                    {
                        if (grg != null) // In garage
                        {
                            temp["GarageName"] = grg.GarageNameTranslated;
                            temp["TruckState"] = 2;
                        }
                        else // Sorting
                        {
                            temp["GarageName"] = "Not In Garage";
                            temp["TruckState"] = 3;
                        }
                    }
                    else
                    {
                        temp["TruckState"] = 1;
                    }

                }

                if (savedindex != -1)
                    comboBoxUserTruckCompanyTrucks.SelectedValue = savedvalue;
            }
        }

        private void translateTrailerComboBox()
        {
            int savedindex = 0;
            string savedvalue = "";
            DataTable temptable = new DataTable();

            //Trailer tab
            temptable = comboBoxUserTrailerCompanyTrailers.DataSource as DataTable;

            if (temptable != null)
            {
                savedindex = comboBoxUserTrailerCompanyTrailers.SelectedIndex;

                if (savedindex != -1)
                    savedvalue = comboBoxUserTrailerCompanyTrailers.SelectedValue.ToString();

                //comboBoxUserTrailerCompanyTrailers.SelectedIndexChanged -= comboBoxCompanyTrailers_SelectedIndexChanged;

                foreach (DataRow temp in temptable.Rows)
                {
                    string source = temp[0].ToString();

                    if (source == "null")
                        continue;

                    string value = GaragesList.Find(x => x.Trailers.Contains(source))?.GarageNameTranslated ?? "";

                    if (value != null && value != "")
                    {
                        temp["GarageName"] = value;
                    }
                    else
                    {
                        temp["GarageName"] = "";
                    }
                }

                if (savedindex != -1)
                    comboBoxUserTrailerCompanyTrailers.SelectedValue = savedvalue;

                //comboBoxUserTrailerCompanyTrailers.SelectedIndexChanged += comboBoxCompanyTrailers_SelectedIndexChanged;
            }
        }

        private void TranslateComboBoxes()
        {
            int savedindex = 0, j = 0;
            string savedvalue = "", ntFormat = " -nt";
            DataTable temptable = new DataTable();

            translateTruckComboBox();
            translateTrailerComboBox();

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

                //Shift All
                DataRow sourceRow = sortedDT.Select("Country = '+all'")[0];
                int rowi = sortedDT.Rows.IndexOf(sourceRow);

                DataRow row = sortedDT.NewRow();
                row.ItemArray = sourceRow.ItemArray;

                sortedDT.Rows.RemoveAt(rowi);
                sortedDT.Rows.InsertAt(row, 0);

                //Shift Unsorted
                try
                {
                    DataRow[] tmpRows = sortedDT.Select("Country = '+unsorted'");
                    if (tmpRows.Count() > 0)
                    {
                        sourceRow = tmpRows[0];
                        rowi = sortedDT.Rows.IndexOf(sourceRow);

                        row = sortedDT.NewRow();
                        row.ItemArray = sourceRow.ItemArray;

                        sortedDT.Rows.RemoveAt(rowi);
                        sortedDT.Rows.InsertAt(row, 1);
                    }
                }
                catch { }

                comboBoxFreightMarketCountries.DataSource = sortedDT;
                
                if (savedindex != -1)
                    comboBoxFreightMarketCountries.SelectedValue = savedvalue;
                else
                    comboBoxFreightMarketCountries.SelectedValue = "+all";

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

                DataRow sourceRow = sortedDT.Select("Company = '+all'")[0];
                int rowi = sortedDT.Rows.IndexOf(sourceRow);

                DataRow row = sortedDT.NewRow();
                row.ItemArray = sourceRow.ItemArray;

                sortedDT.Rows.RemoveAt(rowi);
                sortedDT.Rows.InsertAt(row, 0);


                comboBoxFreightMarketCompanies.DataSource = sortedDT;

                if (savedindex != -1)
                    comboBoxFreightMarketCompanies.SelectedValue = savedvalue;
                else
                    comboBoxFreightMarketCompanies.SelectedValue = "+all";

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
                }

                j++;
            }

            //////
            //Companies ComboBoxes
            ComboBox[] CompaniesCB = { comboBoxFreightMarketSourceCompany, comboBoxFreightMarketDestinationCompany, comboBoxCargoMarketSourceCompany };
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
                }

                j++;
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
            if (_key == "")
                return "";

            CultureInfo ci = Thread.CurrentThread.CurrentUICulture;

            try
            {
                PlainTXTResourceManager rm = new PlainTXTResourceManager();

                string resultString = rm.GetString(_key, ci);

                if (resultString != null)
                    return resultString;
                else
                    return _key.Split(new char[] { '_' }, 2)[1];
            }
            catch
            {
                return _key.Split(new char[] { '_' }, 2)[1];
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
        internal string GetSpareNameless()
        {
            SiiNunitData.NamelessControlList.Sort();
            
            if (namelessLast == "")
            {
                int i = 1;
                do
                {
                    namelessLast = SiiNunitData.NamelessControlList[SiiNunitData.NamelessControlList.Count() - i];

                    i++;

                    if (namelessLast.StartsWith("_nameless."))
                    {
                        namelessLast = namelessLast.Replace("_nameless.", "");
                        break;
                    }
                        

                } while (true);
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
            return "_nameless." + namelessLast;
        }

        private int GetRandomCBindex(int _previous, int _lessthen)
        {
            int result = 0;

            do
            {
                result = RandomValue.Next(_lessthen);
            }
            while (result == _previous);

            return result;
        }
        
        //end Form methods
    }
}