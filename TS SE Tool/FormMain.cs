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

namespace TS_SE_Tool
{
    public partial class FormMain : Form
    {
        #region  Accesslevels

        internal int[] SupportedSavefileVersionETS2; //Program
        internal string SupportedGameVersionETS2;//Program
        //internal int SupportedSavefileVersionATS;
        internal string SupportedGameVersionATS;//Program

        private int JobsAmountAdded;//process result

        private int[] UrgencyArray;//Program

        public bool FileDecoded;  //+

        internal string GameType;//Program

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

        internal static Save.Items.SiiNunit SiiNunitData;

        public SaveFileProfileData MainSaveFileProfileData;
        internal SaveFileInfoData MainSaveFileInfoData;

        //
        internal ProgSettings ProgSettingsV;//Program

        private Random RandomValue;//Program

        private CountryDictionary CountryDictionary;//Program
        private Dictionary<string,Country> CountriesDataList;

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
            TruckPartsImg, TrailerPartsImg, GameIconeImg, AccessoriesImg; //Program

        internal Dictionary<string, Image> ProgUIImgsDict;

        private ImageList TabpagesImages; //Program

        private CheckBox[,] SkillButtonArray; //Program
        private CheckBox[] ADRbuttonArray; //Program

        internal double DistanceMultiplier = 1; //Program
        const double km_to_mile = 0.621371; //Program

        internal double WeightMultiplier = 1; //Program
        const double kg_to_lb = 2.20462262185; //Program

        public Dictionary<string, List<string>> CurrencyDictFormat;
        public Dictionary<string, double> CurrencyDictConversion;

        public Dictionary<string, List<string>> CurrencyDictFormatETS2 = new Dictionary<string, List<string>>();
        public Dictionary<string, double> CurrencyDictConversionETS2 = new Dictionary<string, double>();

        public Dictionary<string, List<string>> CurrencyDictFormatATS = new Dictionary<string, List<string>>();
        public Dictionary<string, double> CurrencyDictConversionATS = new Dictionary<string, double>();

        internal Dictionary<string, Dictionary<UInt16, SCS.SCSFontLetter>> GlobalFontMap;
        internal Dictionary<string, byte> LicensePlateWidth;

        internal bool TssetFoldersExist = false;
        internal bool ForseExit = false;

        #endregion

        public FormMain()
        {
            IO_Utilities.LogWriter("Initializing form...");
            InitializeComponent();
            IO_Utilities.LogWriter("Form initialized.");

            //Program
            UpdateStatusBarMessage.OnNewStatusMessage += UpdateStatusBarMessage_OnNewStatusMessage;
            UpdateStatusBarMessage.OnNewMessageBox += ShowMessageBox_OnNewMessageBox;
            this.Icon = Properties.Resources.MainIco;
            this.Text += " [ " + AssemblyData.AssemblyVersion + " ]";

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

        private void FormMain_Shown(object sender, EventArgs e)
        {
            IO_Utilities.LogWriter("Opening form...");
            try
            {
                IO_Utilities.LogWriter("Done.");

                if (Properties.Settings.Default.ShowSplashOnStartup || Properties.Settings.Default.CheckUpdatesOnStartup)
                    OpenSplashScreen();                
            }
            catch
            {
                IO_Utilities.LogWriter("Done. Settings error.");

                OpenSplashScreen();
            }

            DetectGame();

            void OpenSplashScreen()
            {
                FormSplash WindowSplash = new FormSplash();
                WindowSplash.ShowDialog();
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult exitDR;

            if (this.ForseExit)
            {
                return;
            }

            if (AddedJobsDictionary != null && AddedJobsDictionary.Count > 0)
                exitDR = MessageBox.Show("You have unsaved changes. Do you really want to close down application?", "Close Application without saving changes", MessageBoxButtons.YesNo);
            else
                exitDR = MessageBox.Show("Do you really want to close down application?", "Close Application", MessageBoxButtons.YesNo);

            if (exitDR == DialogResult.Yes)
            {
                ProgSettingsV.WriteConfigToFile();
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
        public static string[] SavesHex = new string[0];
        //
        public static string SelectedSave;
        public static string SelectedSavePath;
        //----
        public static int[] PlayerLevelUps;
        public static string CurrencyName;
    }

}
