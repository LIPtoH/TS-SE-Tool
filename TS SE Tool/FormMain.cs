﻿/*
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
using System.Data.SqlServerCe;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Deployment.Application;
using System.Threading;


namespace TS_SE_Tool
{
    public partial class FormMain : Form
    {
        #region  Accesslevels

        internal int[] SupportedSavefileVersionETS2; //Program
        internal string SupportedGameVersionETS2;//Program
        //internal int SupportedSavefileVersionATS;
        internal string SupportedGameVersionATS;//Program

        private int InGameTime; //+
        //private int JobsTotalDistance;
        private int JobsAmountAdded;//process result

        private int[] UrgencyArray;//Program

        private string ProgPrevVersion;//Program

        public bool FileDecoded;  //+

        internal string GameType;//Program
        private string SavefilePath; //+
        private string LastVisitedCity; //+
        private string LoopStartCity;//Program
        private string LoopStartCompany;//Program
        private string unCertainRouteLength;//Program

        private bool UserCompanyAssignedTruckPlacementEdited;//Program

        private bool InfoDepContinue;//Program

        private string ProfileETS2;//Program
        private string ProfileATS;//Program

        //Raw data in memory
        private string[] tempProfileFileInMemory; //Program
        private string[] tempInfoFileInMemory;//Program
        private string[] tempSavefileInMemory; //+
        //
        //private string[] CitiesListAddedToCompare;
        //private string[] JobsListAdded;
        //private List<string>  ListSavefileCompanysString;
        private Dictionary<string, List<JobAdded>> AddedJobsDictionary;//Program
        private List<JobAdded> AddedJobsList;
        private JobAdded FreightMarketJob;//Program
        private string[] EconomyEventUnitLinkStringList;//Program
        //private string[] EconomyEventQueueList;

        public List<LevelNames> PlayerLevelNames;//Program

        private string[,] EconomyEventsTable;//Program

        private List<City> CitiesList;//+
        private List<string> CitiesListDB;//Program
        private List<string> CitiesListDiff;//Program

        private List<Cargo> CargoesList; //+
        private List<Cargo> CargoesListDB;//Program
        private List<Cargo> CargoesListDiff;//Program

        private Dictionary<string, List<string>> TrailerDefinitionVariants;//Program
        private List<string> TrailerVariants;//Program

        private List<string> HeavyCargoList;

        private List<string> CompaniesList; //+
        private List<string> CompaniesListDB;//Program
        private List<string> CompaniesListDiff;//Program

        private List<string> CountriesList;//Program

        private List<string> DBDependencies;//Program DB
        //private List<string> SFDependencies;//Program Info

        public List<Garages> GaragesList; //+
        public List<string> extraVehicles;//process result
        public List<string> extraDrivers;//process result

        private List<VisitedCity> VisitedCities; //+

        private List<CompanyTruck> CompanyTruckList;//Program
        private List<CompanyTruck> CompanyTruckListDB;//Program
        private List<CompanyTruck> CompanyTruckListDiff;//Program

        private List<ExtCompany> ExternalCompanies;//Program cache
        private List<ExtCargo> ExtCargoList;//Program cache

        internal List<Color> UserColorsList; //+

        private SqlCeConnection DBconnection;//Program

        private DateTime LastModifiedTimestamp; //+

        public PlayerData PlayerDataData; //+
        public SaveFileProfileData MainSaveFileProfileData;
        internal SaveFileInfoData MainSaveFileInfoData;

        internal ProgSettings ProgSettingsV;//Program

        private Random RandomValue;//Program

        private CountryDictionary CountryDictionary;//Program
        private Dictionary<string,Country> CountriesDataList;

        private Routes RouteList;//Program DB

        public PlainTXTResourceManager ResourceManagerMain;

        private Dictionary<string, string> dictionaryProfiles;
        public Dictionary<string, string> CompaniesLngDict, CargoLngDict, TruckBrandsLngDict, CountriesLngDict, UrgencyLngDict, DriverNames;

        public static Dictionary<string, string> CitiesLngDict;//, CustomStringsDict;
        public Dictionary<string, UserCompanyTruckData> UserTruckDictionary; //+
        private List<string> DriverPool; //+
        public Dictionary<string, UserCompanyDriverData> UserDriverDictionary; //+
        private Dictionary<string, UserCompanyTruckData> UserTrailerDictionary; //+
        private Dictionary<string, List<string>> UserTrailerDefDictionary; //+

        private List<string> namelessList;//Program
        private string namelessLast;//Program

        private Dictionary<string, List<string>> GPSbehind, GPSahead, GPSAvoid, GPSbehindOnline, GPSaheadOnline; //+

        internal Dictionary<string, double> DistanceMultipliers; //Program

        private DataTable DistancesTable; //Program

        private Bitmap ProgressBarGradient; //Program
        private Image RepairImg, RefuelImg, CustomizeImg, PlayerCompanyLogo; //Program

        private Image[] ADRImgS, ADRImgSGrey, SkillImgSBG, SkillImgS, GaragesImg, GaragesHQImg, CitiesImg, UrgencyImg, CargoTypeImg, CargoType2Img, 
            TruckPartsImg, TrailerPartsImg, GameIconeImg; //Program

        internal Dictionary<string, Image> ProgUIImgsDict;

        private ImageList TabpagesImages; //Program

        private CheckBox[,] SkillButtonArray; //Program
        private CheckBox[] ADRbuttonArray; //Program

        internal double DistanceMultiplier = 1; //Program
        private double km_to_mileconvert = 0.621371; //Program

        public Dictionary<string, List<string>> CurrencyDictFormat;
        public Dictionary<string, double> CurrencyDictConversion;

        public Dictionary<string, List<string>> CurrencyDictFormatETS2 = new Dictionary<string, List<string>>();
        public Dictionary<string, double> CurrencyDictConversionETS2 = new Dictionary<string, double>();

        public Dictionary<string, List<string>> CurrencyDictFormatATS = new Dictionary<string, List<string>>();
        public Dictionary<string, double> CurrencyDictConversionATS = new Dictionary<string, double>();
        #endregion

        public FormMain()
        {
            LogWriter("Initializing form...");
            InitializeComponent();
            LogWriter("Form initialized.");
            //Non program task
            LogWriter("Caching game data...");
            CacheGameData();
            LogWriter("Caching finished.");

            //Program
            UpdateStatusBarMessage.OnNewStatusMessage += UpdateStatusBarMessage_OnNewStatusMessage;
            this.Icon = Properties.Resources.MainIco;

            SetDefaultValues(true);
            LogWriter("Loading config...");
            LoadConfig();
            LogWriter("Config loaded.");

            LogWriter("Loading resources...");
            LoadExtCountries();
            LoadExtImages();
            LogWriter("Resources loaded.");
            AddImagesToControls();

            //Create page controls
            LogWriter("Creating form elements...");
            CreateProfilePanelControls();
            CreateProgressBarBitmap();
            CreateTruckPanelControls();
            CreateTrailerPanelControls();
            LogWriter("Done.");

            //Clear elements
            LogWriter("Prepare form...");
            ClearFormControls(true);

            ToggleControlsAccess(false);
            LogWriter("Done.");

            //Language
            LogWriter("Loading translation...");
            GetTranslationFiles();
            ChangeLanguage();
            LogWriter("Done.");

            DetectGame();
        }

        private void FormMain_Shown(object sender, EventArgs e)
        {
            LogWriter("Opening form...");
            try
            {
                if (Properties.Settings.Default.ShowSplashOnStartup || Properties.Settings.Default.CheckUpdatesOnStartup)
                {
                    LogWriter("Done.");
                    FormSplash WindowSplash = new FormSplash();
                    WindowSplash.ShowDialog();
                }
            }
            catch
            {
                LogWriter("Done. Settings error.");
                FormSplash WindowSplash = new FormSplash();
                WindowSplash.ShowDialog();
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult exitDR = DialogResult.Yes;

            if (AddedJobsDictionary != null && AddedJobsDictionary.Count > 0)
                exitDR = MessageBox.Show("You have unsaved changes. Do you really want to close down application?", "Close Application without saving changes", MessageBoxButtons.YesNo);
            else
                exitDR = MessageBox.Show("Do you really want to close down application?", "Close Application", MessageBoxButtons.YesNo);

            if (exitDR == DialogResult.Yes)
            {
                WriteConfig();
            }
            else
            {
                e.Cancel = true;
                Activate();
            }
        }

        private void makeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Set default culture
            string sysCI = CultureInfo.InstalledUICulture.Name;

            string folderPath = Directory.GetCurrentDirectory() + @"\lang\" + sysCI;

            if(!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            Process.Start(folderPath);

            //Copy default files

        }
    }

    public class Globals
    {
        //-----
        public static string[] ProfilesPaths;
        public static List<string> ProfilesHex;
        //
        public static string SelectedProfile;
        public static string SelectedProfilePath;
        //----
        public static string[] SavesHex;
        //
        public static string SelectedSave;
        public static string SelectedSavePath;
        //----
        public static int[] PlayerLevelUps;
        public static string CurrencyName;
    }

}
