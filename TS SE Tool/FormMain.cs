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

using TS_SE_Tool.Utilities;
using JR.Utils.GUI.Forms;
using Narod.SteamGameFinder;
using TS_SE_Tool.Forms;
using TS_SE_Tool.CustomClasses.Program;

namespace TS_SE_Tool {
    public partial class FormMain : Form {
        #region  Accesslevels
        internal SupportedGame SelectedGame;

        private int JobsAmountAdded;//process result

        private int[] UrgencyArray;//Program

        public bool FileDecoded;  //+

        private string LoopStartCity;//Program
        private string LoopStartCompany;//Program
        private string unCertainRouteLength;//Program

        private bool InfoDepContinue;//Program

        private string ProfileETS2;//Program
        private string ProfileATS;//Program

        //Raw data in memory
        private string[] tempProfileFileInMemory; //Program
        private string[] tempInfoFileInMemory;//Program
        private string[] tempSavefileInMemory; //+
        //
        private Dictionary<string, List<JobAdded>> AddedJobsDictionary;//Program
        private List<JobAdded> AddedJobsList;
        private JobAdded FreightMarketJob;//Program

        public List<LevelNames> PlayerLevelNames;//Program

        private List<City> CitiesList;//+
        private List<string> CitiesListDB;//Program

        private List<Cargo> CargoesList; //+
        private List<Cargo> CargoesListDB;//Program

        private Dictionary<string, List<string>> TrailerDefinitionVariants;//Program
        private List<string> TrailerVariants;//Program

        private Dictionary<string, List<string>> TrailerDefinitionVariantsDB;
        private List<string> TrailerDefinitionListDB;
        private List<string> TrailerVariantsListDB;

        private List<string> HeavyCargoList;

        private List<string> CompaniesList; //+
        private List<string> CompaniesListDB;//Program

        private List<string> CountriesList;//Program

        private List<string> DBDependencies;//Program DB

        public List<Garages> GaragesList; //+
        public List<string> extraVehicles;//process result
        public List<string> extraDrivers;//process result

        private List<VisitedCity> VisitedCities; //+

        private List<CompanyTruck> CompanyTruckList;//Program
        private List<CompanyTruck> CompanyTruckListDB;//Program

        private List<ExtCompany> ExternalCompanies;//Program cache
        private List<ExtCargo> ExtCargoList;//Program cache
        private SqlCeConnection DBconnection;//Program

        private DateTime LastModifiedTimestamp; //+

        internal Save.Items.SiiNunit SiiNunitData;

        internal SaveFileProfileData MainSaveFileProfileData;
        internal SaveFileInfoData MainSaveFileInfoData;

        //
        internal ProgSettings ProgSettingsV;//Program

        private Random RandomValue;//Program

        private CountryDictionary CountryDictionary;//Program
        private Dictionary<string, Country> CountriesDataList;

        private Routes RouteList;//Program DB

        public PlainTXTResourceManager ResourceManagerMain;

        internal Dictionary<string, string> dictionaryProfiles;
        public Dictionary<string, string> CompaniesLngDict, CargoLngDict, TruckBrandsLngDict, CountriesLngDict, UrgencyLngDict, DriverNames;

        public static Dictionary<string, string> CitiesLngDict;//, CustomStringsDict;
        public Dictionary<string, UserCompanyTruckData> UserTruckDictionary; //+

        public Dictionary<string, UserCompanyDriverData> UserDriverDictionary; //+
        private Dictionary<string, UserCompanyTrailerData> UserTrailerDictionary; //+
        private Dictionary<string, Save.Items.Trailer_Def> UserTrailerDefDictionary; //+

        //private List<string> namelessList;//Program
        private string namelessLast;//Program

        private Dictionary<string, List<string>> GPSbehind, GPSahead, GPSAvoid, GPSbehindOnline, GPSaheadOnline; //+

        internal Dictionary<string, double> DistanceMultipliers; //Program
        internal Dictionary<string, double> WeightMultipliers; //Program

        //internal static Bitmap ProgressBarGradient; //Program

        private Image RepairImg, RefuelImg, CustomizeImg; //Program

        internal Image[] MainIcons, ADRImgS, ADRImgSGrey, SkillImgSBG, SkillImgS, GaragesImg, GaragesHQImg, CitiesImg, UrgencyImg, CargoTypeImg, CargoType2Img,
            TruckPartsImg, TrailerPartsImg, VehicleIntegrityPBImg, GameIconeImg, AccessoriesImg; //Program

        private void modsToolStripMenuItem_Click(object sender, EventArgs e) {
            new FormModManager(SelectedGame).Show();
        }

        private void gameToolStripMenuItem_Click(object sender, EventArgs e) {
            new FormPluginManager(SelectedGame).Show();
        }

        internal Dictionary<string, Image> ProgUIImgsDict;

        private ImageList TabpagesImages; //Program

        private CheckBox[,] SkillButtonArray; //Program
        private CheckBox[] ADRbuttonArray; //Program

        internal double DistanceMultiplier = 1; //Program
        const double km_to_mile = 0.621371; //Program

        internal double WeightMultiplier = 1; //Program
        const double kg_to_lb = 2.20462262185; //Program

        internal Dictionary<string, Dictionary<UInt16, SCS.SCSFontLetter>> GlobalFontMap;

        internal bool TssetFoldersExist = false;
        internal bool ForseExit = false;

        #endregion

        public FormMain() {
            IO_Utilities.LogWriter("Initializing form...");
            InitializeComponent();
            IO_Utilities.LogWriter("Form initialized.");

            //Program
            SteamSelectedTimer.Tick += SteamSelectedTimer_Tick;
            SteamSelectedTimer.Interval = 800;

            UpdateStatusBarMessage.OnNewStatusMessage += UpdateStatusBarMessage_OnNewStatusMessage;
            UpdateStatusBarMessage.OnNewMessageBox += ShowMessageBox_OnNewMessageBox;
            this.Icon = Properties.Resources.MainIco;
            this.Text += " [ " + AssemblyData.AssemblyVersion + " ]";

            IO_Utilities.LogWriter("Getting Supported Games...");
            Globals.Initialize(); //Globals.SteamDir = new SteamGameLocator().getSteamInstallLocation().Cast<DirectoryInfo>().FirstOrDefault();
            IO_Utilities.LogWriter($"Done: {Globals.SupportedGames.ToJson()}");

            //IO_Utilities.LogWriter("Getting Game Paths...");
            //Globals.SteamDir = new SteamGameLocator().getSteamInstallLocation().Cast<DirectoryInfo>().FirstOrDefault();
            //IO_Utilities.LogWriter("Done.");

            SetDefaultValues(true);
            IO_Utilities.LogWriter("Loading config...");
            ProgSettingsV.LoadConfigFromFile();
            CheckTssetFoldersExist();
            ApplySettings();
            IO_Utilities.LogWriter("Config loaded.");

            IO_Utilities.LogWriter("Loading resources...");
            LoadExtCountries();
            LoadExtImages();
            IO_Utilities.LogWriter("Resources loaded.");
            AddImagesToControls();

            //Create page controls
            IO_Utilities.LogWriter("Creating form elements...");
            CreateProfilePanelControls();
            CreateCompanyPanelControls();
            Graphics_TSSET.CreateProgressBarBitmap();
            CreateTruckPanelControls();
            CreateTrailerPanelControls();
            IO_Utilities.LogWriter("Done.");

            //Clear elements
            IO_Utilities.LogWriter("Prepare form...");
            ClearFormControls(true);

            ToggleControlsAccess(false);
            IO_Utilities.LogWriter("Done.");

            //Language
            IO_Utilities.LogWriter("Loading translation...");
            GetTranslationFiles();
            ChangeLanguage();
            IO_Utilities.LogWriter("Done.");

            //Non program task
            IO_Utilities.LogWriter("Caching game data...");
            CacheGameData();
            IO_Utilities.LogWriter("Caching finished.");
        }

        private void FormMain_Shown(object sender, EventArgs e) {
            IO_Utilities.LogWriter("Opening form...");
            try {
                IO_Utilities.LogWriter("Done.");

                if (Properties.Settings.Default.ShowSplashOnStartup || Properties.Settings.Default.CheckUpdatesOnStartup)
                    OpenSplashScreen();
            } catch {
                IO_Utilities.LogWriter("Done. Settings error.");

                OpenSplashScreen();
            }

            DetectGame();

            if (Globals.SupportedGames.Get("ETS2").Installed) radioButtonMainGameSwitchETS.Enabled = true;
            if (Globals.SupportedGames.Get("ATS").Installed) radioButtonMainGameSwitchATS.Enabled = true;

            void OpenSplashScreen() {
                FormSplash WindowSplash = new FormSplash();
                WindowSplash.ShowDialog();
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e) {
            DialogResult exitDR;

            if (this.ForseExit)
                return;

            if (AddedJobsDictionary != null && AddedJobsDictionary.Count > 0)
                exitDR = FlexibleMessageBox.Show(this, "You have unsaved changes." + Environment.NewLine + "Do you really want to close down application?", "Close Application without saving changes", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
            else
                exitDR = FlexibleMessageBox.Show(this, "Do you really want to close down application?", "Close Application", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (exitDR == DialogResult.Yes) {
                ProgSettingsV.WriteConfigToFile();
            } else {
                e.Cancel = true;
                Activate();
            }
        }

        private void makeToolStripMenuItem_Click(object sender, EventArgs e) {
            //Set default culture
            string sysCI = CultureInfo.InstalledUICulture.Name;

            string folderPath = Directory.GetCurrentDirectory() + @"\lang\" + sysCI;

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            Process.Start(folderPath);

            //Copy default files

        }
    }

    public static class Globals {
        public static List<SupportedGame> SupportedGames = new();
        //-----
        public static List<DirectoryInfo> ProfileDirs = new();
        public static List<string> ProfilesHex = new();
        //
        public static string SelectedProfile = "";
        public static string SelectedProfilePath = "";
        public static string SelectedProfileName = "";
        //----
        public static string[] SavesHex = new string[0];
        //
        public static string SelectedSave = "";
        public static string SelectedSavePath = "";
        public static string SelectedSaveName = "";
        //----
        public static string CurrencyName = "";
        //
        public static SteamGameLocator SteamGameLocator = new SteamGameLocator();
        public static bool SteamInstalled { get => SteamGameLocator.getIsSteamInstalled(); }
        public static DirectoryInfo SteamDir { get => new DirectoryInfo(SteamGameLocator.getSteamInstallLocation()); }
        public static DirectoryInfo GetLatestSteamUserDataDir() => SteamDir.Combine("userdata").GetDirectories().OrderByDescending(d => d.LastWriteTimeUtc).First(d => d.Name.All(char.IsDigit));
        public static DirectoryInfo DocumentsDir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)); // todo: make dynamic

        public static void Initialize() {
            SupportedGames.Clear();
            SupportedGames.Add(
                new SupportedGame() {
                    SteamAppId = 227300,
                    Type = "ETS2",
                    Name = "Euro Truck Simulator 2",
                    ExecutableName = "eurotrucks2.exe",
                    MinSupportedGameVersion = new Version(1, 43),
                    MaxSupportedGameVersion = new Version(1, 49),
                    SupportedSaveFileVersions = new() { 61, 74 },
                    LicensePlateWidth = 128,
                    PlayerLevelUps = new() {200, 500, 700, 900, 1000, 1100, 1300, 1600, 1700, 2100, 2300, 2600, 2700,
                    2900, 3000, 3100, 3400, 3700, 4000, 4300, 4600, 4700, 4900, 5200, 5700, 5900, 6000, 6200, 6600, 6800},
                    Currencies = new() {
                         new Currency("EUR", 1, new() { "", "€", "" }),
                         new Currency("CHF", 1.142, new() { "", "", " CHF" }),
                         new Currency("CZK", 25.88, new() { "", "", " Kč" }),
                         new Currency("GBP", 0.875, new() { "", "£", "" }),
                         new Currency("PLN", 4.317, new() { "", "", " zł" }),
                         new Currency("HUF", 325.3, new() { "", "", " Ft" }),
                         new Currency("DKK", 7.46, new() { "", "", " kr" }),
                         new Currency("SEK", 10.52, new() { "", "", " kr" }),
                          new Currency("NOK", 9.51, new() { "", "", " kr" }),
                         new Currency("RUB", 77.05, new() { "", "₽", "" })
                    }
                });
            SupportedGames.Add(
                new SupportedGame() {
                    SteamAppId = 270880,
                    Type = "ATS",
                    Name = "American Truck Simulator",
                    ExecutableName = "amtrucks.exe",
                    MinSupportedGameVersion = new Version(1, 43),
                    MaxSupportedGameVersion = new Version(1, 49),
                    LicensePlateWidth = 64,
                    PlayerLevelUps = new() {200, 500, 700, 900, 1100, 1300, 1500, 1700, 1900, 2100, 2300, 2500, 2700,
                    2900, 3100, 3300, 3500, 3700, 4000, 4300, 4600, 4900, 5200, 5500, 5800, 6100, 6400, 6700, 7000, 7300},
                    Currencies = new() {
                           new Currency("USD", 1, new() { "", "$", "" }),
                           new Currency("CAD", 1.3, new() { "", "$", "" }),
                           new Currency("MXN", 18.69, new() { "", "$", "" }),
                           new Currency("EUR", 0.856, new() { "", "€", "" })
                    }
                });
        }
    }



}
