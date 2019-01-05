/*
   Copyright 2016-2018 LIPtoH <liptoh.codebase@gmail.com>

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
using ICSharpCode.SharpZipLib.GZip;


namespace TS_SE_Tool
{
    public partial class FormMain : Form
    {
        #region  Accesslevels

        private int SavefileVersion;
        internal int SupportedSavefileVersionETS2;
        internal string SupportedGameVersionETS2;
        //internal int SupportedSavefileVersionATS;
        internal string SupportedGameVersionATS;

        private int InGameTime;
        private int JobsTotalDistance;
        private int JobsAmountAdded;

        private double ProgPrevVersion;

        private bool FileDecoded;

        private string GameType;
        private string SavefilePath;
        private string LastVisitedCity;
        private string LoopStartCity;
        private string LoopStartCompany;

        private string UserCompanyAssignedTruck;
        private string UserCompanyAssignedTrailer;
        private string UserCompanyAssignedTruckPlacement;

        private string ProfileETS2;
        private string ProfileATS;

        private string[] CountryDictionaryFile;
        private string[] tempInfoFileInMemory;
        private string[] tempSavefileInMemory;
        private string[] tempProfileFileInMemory;
        private string[] JobsListAdded;
        private string[] CitiesListAddedToCompare;
        private string[] ListSavefileCompanysString;
        private string[] EconomyEventUnitLinkStringList;
        private string[] EconomyEventQueueList;

        private List<LevelNames> PlayerLevelNames;

        private string[,] EconomyEventsTable;

        private List<City> CitiesList;
        private List<string> CitiesListDB;
        private List<string> CitiesListDiff;

        private List<Cargo> CargoesList;
        private List<Cargo> CargoesListDB;
        private List<Cargo> CargoesListDiff;

        private List<string> HeavyCargoList;

        private List<string> CompaniesList;
        private List<string> CompaniesListDB;
        private List<string> CompaniesListDiff;

        private List<string> CountriesList;

        private List<Garages> GaragesList;
        private List<VisitedCity> VisitedCities;

        private List<CompanyTruck> CompanyTruckList;
        private List<CompanyTruck> CompanyTruckListDB;
        private List<CompanyTruck> CompanyTruckListDiff;

        private List<ExtCompany> ExternalCompanies;

        private List<Color> UserColorsList;

        private SqlCeConnection DBconnection;

        private DateTime LastModifiedTimestamp;

        private PlayerProfile PlayerProfileData;

        internal ProgSettings ProgSettingsV;

        private Random RandomValue;

        private CountryDictionary CountryDictionary;

        private Routes RouteList;

        private Dictionary<string, string> dictionaryProfiles;
        private Dictionary<string, string> CompaniesLngDict;
        public static Dictionary<string, string> CitiesLngDict;
        //private Dictionary<string, UserCompanyTruck> UserTruckList;
        private Dictionary<string, UserCompanyTruckData> UserTruckDictionary;
        private Dictionary<string, UserCompanyTruckData> UserTrailerDictionary;

        private Dictionary<string, List<string>> GPSbehind, GPSahead;

        internal Dictionary<string, double> DistanceMultipliers;

        private DataTable DistancesTable;

        private Bitmap ProgressBarGradient;
        private Image RepairImg, RefuelImg;
        private Image[] ADRImgS, ADRImgSGrey, SkillImgSBG, SkillImgS, GaragesImg, CitiesImg, UrgencyImg, CargoTypeImg, TruckPartsImg, TrailerPartsImg, GameIconeImg;
        private ImageList TabpagesImages;
        private CheckBox[,] SkillButtonArray;
        private CheckBox[] ADRbuttonArray;

        internal double DistanceMultiplier = 1;
        private double km_to_mileconvert = 0.621371;

        #endregion

        public FormMain()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.MainIco;

            //buttonGameETS.Enabled = true;
            //buttonGameATS.Enabled = false;
            GetTranslationFiles();

            SetDefaultValues(true);
            LoadConfig();
            LoadExtCountries();
            LoadCompaniesLng();
            LoadCitiesLng();
            ChangeLanguage();
            ToggleVisibility(false);

            ToggleGame(GameType);
            LoadExtImages();
            //GetCompaniesCargoInOut();

            CreateProfilePanelControls();
            CreateTruckPanelControls();
            CreateProgressBarBitmap();

            listBoxAddedJobs.DrawMode = DrawMode.OwnerDrawVariable;
        }

        private void FormMain_Shown(object sender, EventArgs e)
        {
            FillAllProfilesPaths();
            FillProfiles();
        }

        private void comboBoxProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
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
                pictureBoxProfileAvatar.Image = ExtImgLoader(imgpaths, 95, 95, 0, 0)[0];// new Bitmap(95, 95);
            }

            //DateTime CreationDate = (new DateTime(1970, 1, 1, 0, 0, 0, 0)).AddSeconds(CreationTime); //trucking since
        }

        private void buttonRefreshAll_Click(object sender, EventArgs e)
        {
            FillAllProfilesPaths();
            FillProfiles();
            FillProfileSaves();
        }

        private void buttonDecryptSave_Click(object sender, EventArgs e)
        {
            //SavefilePath = Globals.SavesHex[comboBoxSaves.SelectedIndex];
            SetDefaultValues(false);

            buttonDecryptSave.Enabled = false;
            buttonLoadSave.Enabled = false;
            buttonGameETS.Enabled = false;
            buttonGameATS.Enabled = false;

            SavefilePath = Globals.SavesHex[comboBoxSaves.SelectedIndex];
            string SiiSavePath = SavefilePath + @"\game.sii";
            DecodeFile(SiiSavePath);
            //LoadSaveFile(); //Load save file

            //GC
            GC.Collect();
            //GC.WaitForPendingFinalizers();
            //WriteSaveFile();
        }

        private void buttonOpenSaveFolder_Click(object sender, EventArgs e)
        {
            Process.Start(Globals.SavesHex[comboBoxSaves.SelectedIndex]);
        }

        private void FormMain_Load(object sender, EventArgs e)
        {

        }

        private void buttonGameCustomPath_Click(object sender, EventArgs e)
        {

        }

        private void AddCustomFolder_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Save files (game.sii)|game.sii";
            openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            DialogResult result = openFileDialog1.ShowDialog();

            comboBoxProfiles.Items.Add("Custom");

            string DirectoryPath = Path.GetDirectoryName(openFileDialog1.FileName);

            string DirectoryName = openFileDialog1.FileName.Substring((DirectoryPath.LastIndexOf('\\') + 1), (DirectoryPath.Length - (DirectoryPath.LastIndexOf('\\')) - 1));

            comboBoxSaves.Items.Add(DirectoryName);

            Globals.SavesHex[0] = DirectoryName;
        }

        private void buttonClearJobList_Click(object sender, EventArgs e)
        {
            ClearJobData();
        }

        private void buttonAddJob_Click(object sender, EventArgs e)
        {
            AddCargo();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void makeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportFormControlstoLanguageFile();
        }

        private byte[] zipText(string text)
        {
            if (text == null)
                return null;

            using (Stream memOutput = new MemoryStream())
            {
                using (GZipOutputStream zipOut = new GZipOutputStream(memOutput))
                {
                    using (StreamWriter writer = new StreamWriter(zipOut))
                    {
                        writer.Write(text);

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

        private string unzipText(string _sbytes)
        {
            string[] pairs = new string[_sbytes.Length / 2];
            byte[] bytes;// = new byte[0];

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

        private void comboBoxPrevProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillProfiles();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox aboutWindow = new AboutBox();
            aboutWindow.ShowDialog();
        }

        private void comboBoxHQcity_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (comboBoxHQcity.SelectedValue != null)
                PlayerProfileData.HQcity = comboBoxHQcity.SelectedValue.ToString();
        }

        private void buttonUserTruckSelectCurrent_Click(object sender, EventArgs e)
        {
            comboBoxCompanyTrucks.SelectedValue = UserCompanyAssignedTruck;
        }

        private void buttonUserTruckSwitchCurrent_Click(object sender, EventArgs e)
        {
            UserCompanyAssignedTruck = comboBoxCompanyTrucks.SelectedValue.ToString();
        }

        private void button11_Click(object sender, EventArgs e)
        {

        }

        private void textBoxMoneyAccount_TextChanged(object sender, EventArgs e)
        {
            TextBox txtcur = sender as TextBox;

            if (!string.IsNullOrEmpty(txtcur.Text))
            {
                UInt64 valueBefore = UInt64.Parse(txtcur.Text, NumberStyles.AllowThousands);
                txtcur.Text = String.Format(CultureInfo.CurrentCulture, "{0:N0}", valueBefore);
                txtcur.Select(txtcur.Text.Length, 0);

                PlayerProfileData.AccountMoney = UInt32.Parse(txtcur.Text, NumberStyles.AllowThousands);
            }
        }

        private void textBoxMoneyAccount_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox txtcur = sender as TextBox;
            UInt64 valueBefore = 0;

            if (!string.IsNullOrEmpty(txtcur.Text))
            {
                valueBefore = UInt64.Parse(txtcur.Text, NumberStyles.AllowThousands);
            }

            if (!Char.IsDigit(e.KeyChar) && !(valueBefore <= 999999999))
            {
                txtcur.Text = valueBefore.ToString();
                e.Handled = true;
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult exitDR = DialogResult.Yes;

            if (JobsListAdded != null && JobsListAdded.Length > 0)
                exitDR = MessageBox.Show("You have unsaved changes. Do you realy want to close down application?", "Close Application without saving changes", MessageBoxButtons.YesNo);
            else
                exitDR = MessageBox.Show("Do you realy want to close down application?", "Close Application", MessageBoxButtons.YesNo);

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

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSettings FormWindow = new FormSettings();
            FormWindow.ShowDialog();
        }
    }

    public class Globals
    {
        public static string[] ProfilesPaths;
        public static string[] ProfilesHex;
        public static string[] SavesHex;
        public static string CurrentGame = "";
        public static string ProfileSii = "";
        public static int[] PlayerLevelUps;
    }

}
