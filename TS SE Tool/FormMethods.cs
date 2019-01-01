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
using System.Windows.Forms;
using System.Reflection;
using System.Resources;
using System.Windows;
using TS_SE_Tool.CustomClasses;

namespace TS_SE_Tool
{
    public partial class FormMain
    {
        private void ShowStatusMessages(string _status, string _message)
        {

            toolStripStatusMessages.Text = GetranslatedString(_message);
            if (_status == "e")
            {
                toolStripStatusMessages.ForeColor = Color.Red;
            }
            if (_status == "i")
            {
                toolStripStatusMessages.ForeColor = Color.Black;
            }
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
                ProgSettingsV = new ProgSettings(0.1, "Default", false, 72, 0, 1.0, "km");

                ProgSettingsV.ProgramVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductMajorPart +
                    (FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductMinorPart / 10.0);

                SavefileVersion = 0;
                SupportedSavefileVersionETS2 = 39; //Supported save version
                SupportedGameVersionETS2 = "1.33.x"; //Last game version Tested on
                //SupportedSavefileVersionATS;
                SupportedGameVersionATS = "1.33.x"; //Last game version Tested on

                ProfileETS2 = @"\Euro Truck Simulator 2";
                ProfileATS = @"\American Truck Simulator";

                comboBoxPrevProfiles.FlatStyle =
                comboBoxProfiles.FlatStyle =
                comboBoxSaves.FlatStyle = FlatStyle.Flat;

                dictionaryProfiles = new Dictionary<string, string>();
                dictionaryProfiles.Add("ETS2", ProfileETS2);
                dictionaryProfiles.Add("ATS", ProfileATS);

                UserCompanyAssignedTruck = "";

                CompaniesLngDict = new Dictionary<string, string>();
                CitiesLngDict = new Dictionary<string, string>();

                Globals.CurrentGame = dictionaryProfiles["ETS2"];
                GameType = "ETS";

                DBconnection = new System.Data.SqlServerCe.SqlCeConnection("Data Source = Database.sdf");
                CreateDatabase();

                DistancesTable = new DataTable();
                DistancesTable.Columns.Add("SourceCity", typeof(string));
                DistancesTable.Columns.Add("SourceCompany", typeof(string));
                DistancesTable.Columns.Add("DestinationCity", typeof(string));
                DistancesTable.Columns.Add("DestinationCompany", typeof(string));
                DistancesTable.Columns.Add("Distance", typeof(int));
                DistancesTable.Columns.Add("FerryTime", typeof(int));
                DistancesTable.Columns.Add("FerryPrice", typeof(int));

                //comboBoxUrgency
                DataTable combDT = new DataTable();
                combDT.Columns.Add("ID");
                combDT.Columns.Add("UrgencyDisplayName");

                combDT.Rows.Add(new object[] { 0, "Standard" });
                combDT.Rows.Add(new object[] { 1, "Important" });
                combDT.Rows.Add(new object[] { 2, "Urgent" });

                comboBoxUrgency.ValueMember = "ID";
                comboBoxUrgency.DisplayMember = "UrgencyDisplayName";
                comboBoxUrgency.DataSource = combDT;
                comboBoxUrgency.SelectedIndex = -1;

                CountryDictionary = new CountryDictionary();

                Globals.PlayerLevelUps = new int[] {200, 500, 700, 900, 1000, 1100, 1300, 1600, 1700, 1900, 2100, 2300, 2600, 2700,
                    2900, 3000, 3100, 3400, 3700, 4000, 4300, 4600, 4700, 4900, 5200, 5700, 5900, 6000, 6200, 6600, 6900};

                PlayerLevelNames = new List<LevelNames>();

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

                DistanceMultipliers = new Dictionary<string, double>();
                DistanceMultipliers.Add("km", 1);
                DistanceMultipliers.Add("mi", km_to_mileconvert);

                ADRImgS = new Image[6];
                ADRImgSGrey = new Image[6];
                SkillImgSBG = new Image[5];
                SkillImgS = new Image[6];
                ADRbuttonArray = new CheckBox[6];
                GaragesImg = new Image[4];
                CitiesImg = new Image[2];
                UrgencyImg = new Image[3];
                CargoTypeImg = new Image[3];
                GameIconeImg = new Image[2];
                TruckPartsImg = new Image[5];
                TrailerPartsImg = new Image[4];

                SkillButtonArray = new CheckBox[5, 6];

                TabpagesImages = new ImageList();

                buttonGameETS.Image = GameIconeImg[0];
                buttonGameATS.Image = GameIconeImg[1];
            }

            FileDecoded = false;
            SavefilePath = "";

            tempInfoFileInMemory = null;
            tempSavefileInMemory = null;
            tempProfileFileInMemory = null;

            PlayerProfileData = new PlayerProfile("", 0, new byte[] { 0, 0, 0, 0, 0, 0 }, 0);

            CompaniesList = new List<string>();
            CitiesList = new List<City>();

            CountriesList = new List<string>();
            CargoesList = new List<Cargo>();
            HeavyCargoList = new List<string>();
            CompanyTruckList = new List<CompanyTruck>();
            CompanyTruckListDB = new List<CompanyTruck>();
            CompanyTruckListDiff = new List<CompanyTruck>();

            UserColorsList = new List<Color>();
            GaragesList = new List<Garages>();
            //UserTruckList = new Dictionary<string, UserCompanyTruck>();
            UserTruckDictionary = new Dictionary<string, UserCompanyTruckData>();

            VisitedCities = new List<VisitedCity>();

            CargoesListDB = new List<Cargo>();
            CitiesListDB = new List<string>();
            CompaniesListDB = new List<string>();
            CargoesListDiff = new List<Cargo>();
            CitiesListDiff = new List<string>();
            CompaniesListDiff = new List<string>();

            EconomyEventQueueList = new string[0];
            EconomyEventsTable = new string[0, 0];
            EconomyEventUnitLinkStringList = new string[0];

            JobsAmountAdded = 0;
            LastVisitedCity = "";
            InGameTime = 0;
            RandomValue = new Random();
            CitiesListAddedToCompare = new string[1];

            JobsListAdded = new string[0];
            LastModifiedTimestamp = new DateTime();
            ListSavefileCompanysString = new string[0];

            //game = "ETS";
            JobsTotalDistance = 0;
            LoopStartCity = "";
            LoopStartCompany = "";
            ProgPrevVersion = 0f;

            RouteList = new Routes();
            DistancesTable.Clear();

            components = null;
        }

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

        private Color GetProgressbarColor(decimal value)
        {
            return ProgressBarGradient.GetPixel(Convert.ToInt32((1 - value) * 100) - 1, 0);
        }

        private Bitmap ImageFromDDS(string _path)
        {
            Bitmap bitmap = null;

            if (File.Exists(_path))
            {
                FileStream fsimage = new FileStream(_path, FileMode.Open);

                S16.Drawing.DDSImage asd = new S16.Drawing.DDSImage(fsimage);

                bitmap = asd.BitmapImage;

                return bitmap;
            }
            else
                return bitmap;
        }

        private void PopulateFormControlsk()
        {
            buttonDecryptSave.Enabled = false;
            buttonWriteSave.Enabled = true;

            string t1 = "Trucking since:\n\r" + DateTimeOffset.FromUnixTimeSeconds(PlayerProfileData.CreationTime).DateTime.ToLocalTime().ToString();
            toolTipMain.SetToolTip(pictureBoxProfileAvatar, t1);

            FillFormProfileControls();
            UpdateUserColorsButtons();
            FillFormCompanyControls();

            FillUserCompanyTrucksList();

            FillcomboBoxCargoList();
            FillcomboBoxCountries();
            FillcomboBoxCompanies();
            FillcomboBoxSourceCityDestinationCity();
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
                string templine = UserTruck.Value.Parts.Find(x => x.PartType == "truckbrandname").PartData.Find(xline => xline.StartsWith(" data_path:"));
                string truckname = templine.Split(new char[] { '"' })[1].Split(new char[] { '/' })[4];

                combDT.Rows.Add(UserTruck.Key, truckname); ////.TruckName);
            }
            /*
            foreach (KeyValuePair<string, UserCompanyTruck> UserTruck in UserTruckList)
            {
                combDT.Rows.Add(UserTruck.Key, UserTruck.Value.TruckName);
            }
            */
            //combDT.DefaultView.Sort = "UserTruckName ASC";
            comboBoxCompanyTrucks.ValueMember = "UserTruckNameless";
            comboBoxCompanyTrucks.DisplayMember = "UserTruckName";

            comboBoxCompanyTrucks.DataSource = combDT;

            //UserTruckList.TryGetValue(comboBoxCompanyTrucks.SelectedValue.ToString(), out UserCompanyTruck SelectedUserCompanyTruck);

            comboBoxCompanyTrucks.SelectedValue = UserCompanyAssignedTruck;
        }

        private void comboBoxCompanyTrucks_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmbbx = sender as ComboBox;

            if (cmbbx.SelectedIndex != -1)
            {
                UpdateTruckPanelProgressBars();
                //UpdateTruckPanelProgressTitles();
            }
        }

        private void CreateProfilePanelControls()
        {
            tabControlMain.ImageList = TabpagesImages;

            for (int i = 0; i < 6; i++)
            {
                tabControlMain.TabPages[i].ImageIndex = i;
            }

            int pSkillsNameHeight = 56, pSkillsNameWidth = 56, pSkillsNameOffset = 5, pSkillsNamelOffset = 12;

            string[] toolskillimgtooltip = new string[] { "ADR", "Long Distance", "High Value Cargo", "Fragile Cargo", "Just-In-Time Delivery", "Ecodriving" };

            for (int i = 0; i < 6; i++)
            {
                Panel Ppanel = new Panel();
                groupBoxProfileSkill.Controls.Add(Ppanel);

                Ppanel.Parent = groupBoxProfileSkill;
                Ppanel.Location = new Point(pSkillsNamelOffset, 11 + (pSkillsNameHeight + pSkillsNameOffset) * i);
                Ppanel.BorderStyle = BorderStyle.None;
                Ppanel.Size = new Size(pSkillsNameWidth, pSkillsNameHeight);
                Ppanel.Name = "profileSkillsPanel" + i.ToString();
                toolTipMain.SetToolTip(Ppanel, toolskillimgtooltip[i]);

                Bitmap bgimg = new Bitmap(SkillImgS[i], pSkillsNameHeight, pSkillsNameWidth);
                Ppanel.BackgroundImage = bgimg;

                Label slabel = new Label();
                groupBoxProfileSkill.Controls.Add(slabel);
                slabel.Name = "profileSkillName" + i.ToString();
                slabel.Location = new Point(pSkillsNamelOffset * 2 + pSkillsNameWidth, 11 + (pSkillsNameHeight + pSkillsNameOffset) * i);
                slabel.Text = toolskillimgtooltip[i];
                slabel.AutoSize = true;
            }

            int bADRHeight = 40, bADRWidth = 40, pOffset = 6, lOffset = pSkillsNameWidth + pSkillsNamelOffset * 2;

            for (int i = 0; i < 6; i++)
            {
                CheckBox Ppanel = new CheckBox();
                groupBoxProfileSkill.Controls.Add(Ppanel);

                Ppanel.Parent = groupBoxProfileSkill;

                Ppanel.Location = new Point(lOffset + (bADRWidth + pOffset) * i, 11 + 14);
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

                    Ppanel.Location = new Point(lOffset + (bADRWidth + pOffset) * j, 11 + 14 + (pSkillsNameHeight + pSkillsNameOffset) * (i + 1));
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

        private void ClearProfilePage()
        {
            foreach (CheckBox temp in ADRbuttonArray)
                temp.Checked = false;

            foreach (CheckBox temp in SkillButtonArray)
                temp.Checked = false;
        }

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
                PlayerProfileData.PlayerSkills[++skillIndex] = ++buttonIndex;
            }
            else
            {
                for (int j = 5; j >= int.Parse(thisbutton.Name.Substring(12, 1)); j--)
                {
                    SkillButtonArray[skillIndex, j].Checked = false;
                }
                PlayerProfileData.PlayerSkills[++skillIndex] = buttonIndex;
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
                char[] ADR = Convert.ToString(PlayerProfileData.PlayerSkills[0], 2).PadLeft(6, '0').ToCharArray();
                ADR[byte.Parse(thisbutton.Name.Substring(9, 1))] = '1';

                PlayerProfileData.PlayerSkills[0] = Convert.ToByte(new string(ADR), 2);
                thisbutton.BackgroundImage = SkillImgSBG[1];
            }
            else
            {
                char[] ADR = Convert.ToString(PlayerProfileData.PlayerSkills[0], 2).PadLeft(6, '0').ToCharArray();
                ADR[byte.Parse(thisbutton.Name.Substring(9, 1))] = '0';
                string temp = new string(ADR);
                PlayerProfileData.PlayerSkills[0] = Convert.ToByte(temp.PadLeft(8, '0'), 2);
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

        private void CreateTruckPanelControls()
        {
            CreateTruckPanelProgressBars();
        }

        private void CreateTruckPanelProgressBars()
        {
            int pHeight = RepairImg.Height, pOffset = 5, lOffset = 60, pSizeW = 300;
            int pSkillsNameHeight = 32, pSkillsNameWidth = 32, pSkillsNameOffset = 5, pSkillsNamelOffset = 12;

            string[] toolskillimgtooltip = new string[] { "Engine", "Transmission", "Chassis", "Cabin", "Wheels" };
            Label slabel;
            Panel Ppanel;

            for (int i = 0; i < 5; i++)
            {
                slabel = new Label();
                groupBoxTruckDetails.Controls.Add(slabel);
                slabel.Name = "labelTruckPartName" + toolskillimgtooltip[i];//.ToString();
                slabel.Location = new Point(pSkillsNamelOffset, 23 + (pSkillsNameHeight + pSkillsNameOffset * 3) * i);
                slabel.Text = toolskillimgtooltip[i];
                slabel.AutoSize = true;

                Panel imgpanel = new Panel();
                groupBoxTruckDetails.Controls.Add(imgpanel);

                imgpanel.Parent = groupBoxTruckDetails;
                imgpanel.Location = new Point(pSkillsNamelOffset, 23 + 14 + (pSkillsNameHeight + pSkillsNameOffset * 3) * i);
                imgpanel.BorderStyle = BorderStyle.None;
                imgpanel.Size = new Size(pSkillsNameWidth, pSkillsNameHeight);
                imgpanel.Name = "TruckPartImg" + i.ToString();
                //toolTipMain.SetToolTip(Ppanel, toolskillimgtooltip[i]);

                Bitmap bgimg = new Bitmap(TruckPartsImg[i], pSkillsNameHeight, pSkillsNameWidth);
                imgpanel.BackgroundImage = bgimg;

                //Panel 
                Ppanel = new Panel();
                groupBoxTruckDetails.Controls.Add(Ppanel);

                Ppanel.Parent = groupBoxTruckDetails;
                Ppanel.Location = new Point(lOffset, imgpanel.Location.Y);
                Ppanel.BorderStyle = BorderStyle.FixedSingle;
                Ppanel.Size = new Size(pSizeW, RepairImg.Height);
                Ppanel.Name = "progressbarTruckPart" + i.ToString();

                Button button = new Button();
                groupBoxTruckDetails.Controls.Add(button);

                button.Parent = groupBoxTruckDetails;
                button.Location = new Point(Ppanel.Location.X + Ppanel.Width + pOffset, Ppanel.Location.Y);
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
            groupBoxTruckDetails.Controls.Add(Ppanelf);
            Ppanelf.Parent = groupBoxTruckDetails;
            Ppanelf.Location = new Point(lOffset + pSizeW + pOffset * 2 + RepairImg.Width, 23 + 14);
            Ppanelf.BorderStyle = BorderStyle.FixedSingle;
            Ppanelf.Size = new Size(50, 220);
            Ppanelf.Name = "progressbarTruckFuel";

            slabel = new Label();
            groupBoxTruckDetails.Controls.Add(slabel);
            slabel.Name = "labelTruckDetailsFuel";
            slabel.Text = "Fuel";
            slabel.AutoSize = true;
            slabel.Location = new Point(Ppanelf.Location.X + (Ppanelf.Width - slabel.Width) / 2, Ppanelf.Location.Y + Ppanelf.Height + 10);

            CreateTruckPanelButtons();

        }

        private void UpdateTruckPanelProgressBars()
        {
            UserTruckDictionary.TryGetValue(comboBoxCompanyTrucks.SelectedValue.ToString(), out UserCompanyTruckData SelectedUserCompanyTruck);

            for (int i = 0; i < 5; i++)
            {
                Panel pnl = null;
                string pnlname = "progressbarTruckPart" + i.ToString();
                if (groupBoxTruckDetails.Controls.ContainsKey(pnlname))
                {
                    pnl = groupBoxTruckDetails.Controls[pnlname] as Panel;
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

                    string wear = TruckDataPart.Find(xl => xl.StartsWith(" wear:")).Split(new char[] { ' ' })[2];//TruckPart.PartWear;
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

            Panel pnlfuel = null;
            string pnlnamefuel = "progressbarTruckFuel";
            if (groupBoxTruckDetails.Controls.ContainsKey(pnlnamefuel))
            {
                pnlfuel = groupBoxTruckDetails.Controls[pnlnamefuel] as Panel;
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
                    FontFamily.GenericSansSerif,  // or any other font family
                    (int)FontStyle.Regular,      // font style (bold, italic, etc.)
                    g.DpiY * fontSize / 72,       // em size
                    new Rectangle(0, 0, pnlfuel.Width, pnlfuel.Height),              // location where to draw text
                    sf);          // set options here (e.g. center alignment)
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.FillPath(Brushes.Black, p);
                g.DrawPath(Pens.Black, p);

                pnlfuel.BackgroundImage = progress;
            }

            string lctxt = "";
            labelLicensePlate.Text = "";
            int stindex = 0, endindex = 0;

            string LicensePlate = SelectedUserCompanyTruck.Parts.Find(xp => xp.PartType == "truckdata").PartData.Find(xl => xl.StartsWith(" license_plate:")).Split(new char[] { '"' })[1];

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
                labelLicensePlate.Text = lctxt.Split(new char[] { '|' })[0] + " Country " + lctxt.Split(new char[] { '|' })[1];
            else
                labelLicensePlate.Text = lctxt.Split(new char[] { '|' })[0];
        }

        private void UpdateTruckPanelProgressTitles()
        {

        }

        private void CreateTruckPanelButtons()
        {
            //int lOffset = 100, pSizeW = 200;
            int pHeight = RepairImg.Height, pOffset = 5, tOffset = comboBoxCompanyTrucks.Location.Y;
            int topbutoffset = comboBoxCompanyTrucks.Location.X + comboBoxCompanyTrucks.Width + pOffset;// = lOffset + pSizeW + pOffset * 2 + RepairImg.Width + groupBoxTruckDetails.Location.X;
            //int elembuttonoffset = lOffset + pSizeW + pOffset;

            Button buttonR = new Button();
            tabPageTruck.Controls.Add(buttonR);

            buttonR.Location = new Point(topbutoffset, tOffset);
            buttonR.FlatStyle = FlatStyle.Flat;
            buttonR.Size = new Size(RepairImg.Height, RepairImg.Height);
            buttonR.Name = "buttonTruckRepair";
            buttonR.BackgroundImage = RepairImg;
            buttonR.BackgroundImageLayout = ImageLayout.Zoom;
            buttonR.Text = "";
            buttonR.FlatAppearance.BorderSize = 0;
            buttonR.Click += new EventHandler(buttonTruckRepair_Click);

            Button buttonF = new Button();
            tabPageTruck.Controls.Add(buttonF);

            buttonF.Location = new Point(topbutoffset + RepairImg.Height + pOffset, tOffset);
            buttonF.FlatStyle = FlatStyle.Flat;
            buttonF.Size = new Size(RepairImg.Height, RepairImg.Height);
            buttonF.Name = "buttonTruckReFuel";
            buttonF.BackgroundImage = RefuelImg;
            buttonF.BackgroundImageLayout = ImageLayout.Zoom;
            buttonF.Text = "";
            buttonF.FlatAppearance.BorderSize = 0;
            buttonF.Click += new EventHandler(buttonTruckReFuel_Click);
        }

        public void buttonTruckReFuel_Click(object sender, EventArgs e)
        {
            int i = 0;
            foreach (string temp in UserTruckDictionary[comboBoxCompanyTrucks.SelectedValue.ToString()].Parts.Find(x => x.PartType == "truckdata").PartData)
            {
                if (temp.StartsWith(" fuel_relative:"))
                {
                    UserTruckDictionary[comboBoxCompanyTrucks.SelectedValue.ToString()].Parts.Find(x => x.PartType == "truckdata").PartData[i] = " fuel_relative: 1";
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
                foreach (UserCompanyTruckDataPart temp in UserTruckDictionary[comboBoxCompanyTrucks.SelectedValue.ToString()].Parts.FindAll(x => x.PartType == tempPart))
                {
                    string partNameless = temp.PartNameless;

                    int i = 0;

                    foreach (string temp2 in temp.PartData)
                    {
                        if (temp2.StartsWith(" wear:"))
                        {
                            UserTruckDictionary[comboBoxCompanyTrucks.SelectedValue.ToString()].Parts.Find(x => x.PartNameless == partNameless).PartData[i] = " wear: 0";
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

            foreach (UserCompanyTruckDataPart temp in UserTruckDictionary[comboBoxCompanyTrucks.SelectedValue.ToString()].Parts.FindAll(x => x.PartType == PartList[bi]))
            {
                string partNameless = temp.PartNameless;

                int i = 0;

                foreach (string temp2 in temp.PartData)
                {
                    if (temp2.StartsWith(" wear:"))
                    {
                        UserTruckDictionary[comboBoxCompanyTrucks.SelectedValue.ToString()].Parts.Find(x => x.PartNameless == partNameless).PartData[i] = " wear: 0";
                        break;
                    }
                    i++;
                }
            }

            UpdateTruckPanelProgressBars();
        }
        
        private void buttonTruckPaintCopy_Click(object sender, EventArgs e)
        {
            string tempPaint = "TruckPaint\r\n";

            List<string> paintstr = UserTruckDictionary[comboBoxCompanyTrucks.SelectedValue.ToString()].Parts.Find(xp => xp.PartType == "paintjob").PartData;

            foreach (string temp in paintstr)
            {
                tempPaint+= temp + "\r\n";
            }
            //MessageBox.Show(tempPaint);
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
                /*
                foreach (string line in Lines)
                {
                    MessageBox.Show(line);
                }
                */
                if (Lines[0] == "TruckPaint")
                {
                    List<string> paintstr = new List<string>();
                    for (int i = 1; i < Lines.Length; i++)
                    {
                        paintstr.Add(Lines[i]);
                    }

                    UserTruckDictionary[comboBoxCompanyTrucks.SelectedValue.ToString()].Parts.Find(xp => xp.PartType == "paintjob").PartData = paintstr;

                    MessageBox.Show("Paint data  has been inserted.");
                }
                else
                    MessageBox.Show("Wrong data. Expected Paint data but\r\n"+ Lines[0]+ "\r\nwas found.");
            }
            catch
            {
                MessageBox.Show("Something gone wrong with decoding.");
            }
        }

        private void checkBoxProfileBackups_CheckedChanged(object sender, EventArgs e)
        {
            FillAllProfilesPaths();
        }

        public void FillAllProfilesPaths()
        {
            comboBoxPrevProfiles.Items.Clear();

            string MyDocumentsPath = "";
            MyDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + Globals.CurrentGame;

            if (!Directory.Exists(MyDocumentsPath))
            {
                MessageBox.Show("Standart Game Save folder don't exist");
                return;
            }

            if (checkBoxProfileBackups.Checked)
            {
                foreach (string folder in Directory.GetDirectories(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + Globals.CurrentGame))
                {
                    if (Path.GetFileName(folder).StartsWith("profiles"))
                    {
                        comboBoxPrevProfiles.Items.Add(Path.GetFileName(folder));
                    }
                }
            }
            else
                if(Directory.Exists(MyDocumentsPath + "\\profiles" ))
                    comboBoxPrevProfiles.Items.Add("profiles");

            if(comboBoxPrevProfiles.Items.Count > 0)
            {
                comboBoxPrevProfiles.SelectedIndex = 0;
                buttonOpenSaveFolder.Enabled = true;
                buttonDecryptSave.Enabled = true;
                buttonLoadSave.Enabled = true;
            }                
            else
            {
                MessageBox.Show("No profiles found");

                buttonOpenSaveFolder.Enabled = false;
                buttonDecryptSave.Enabled = false;
                buttonLoadSave.Enabled = false;
            }                
        }

        public void FillProfiles()
        {
            string Profile = "";
            comboBoxProfiles.Items.Clear();
            
            string MyDocumentsPath = "";
            MyDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + Globals.CurrentGame + "\\" + comboBoxPrevProfiles.Items[comboBoxPrevProfiles.SelectedIndex];//@"\profiles";
            /*
            if (!Directory.Exists(MyDocumentsPath))
            {
                MessageBox.Show("Standart Save files don't exist");
                return;
            }
            */
            Globals.ProfilesHex = Directory.GetDirectories(MyDocumentsPath).OrderByDescending(f => new FileInfo(f).LastWriteTime).ToArray();

            if(Globals.ProfilesHex.Length > 0)
            {
                foreach (string profile in Globals.ProfilesHex)
                {
                    Profile = FromHexToString(Path.GetFileName(profile));
                    comboBoxProfiles.Items.Add(Profile);
                }
                comboBoxProfiles.SelectedIndex = 0;
                buttonOpenSaveFolder.Enabled = true;
                buttonDecryptSave.Enabled = true;
                buttonLoadSave.Enabled = true;
            }
            else
            {
                MessageBox.Show("No profiles found");

                buttonOpenSaveFolder.Enabled = false;
                buttonDecryptSave.Enabled = false;
                buttonLoadSave.Enabled = false;
            }
        }

        public void FillProfileSaves()
        {
            comboBoxSaves.Items.Clear();

            string savePath = Globals.ProfilesHex[comboBoxProfiles.SelectedIndex] + @"\save";

            Globals.SavesHex = Directory.GetDirectories(savePath).OrderByDescending(f => new FileInfo(f).LastWriteTime).ToArray();

            if(Globals.SavesHex.Length > 0)
            {
                foreach (string profile in Globals.SavesHex)
                {
                    comboBoxSaves.Items.Add(Path.GetFileName(profile));
                }

                comboBoxSaves.SelectedIndex = 0;
                buttonOpenSaveFolder.Enabled = true;
                buttonDecryptSave.Enabled = true;
                buttonLoadSave.Enabled = true;
            }
            else
            {
                MessageBox.Show("No save file folders found");

                buttonOpenSaveFolder.Enabled = false;
                buttonDecryptSave.Enabled = false;
                buttonLoadSave.Enabled = false;
            }
        }

        public void FillVisitedCities()
        {
            listBoxVisitedCities.Items.Clear();

            foreach(City vc in CitiesList)
            {
                listBoxVisitedCities.Items.Add(vc);
            }

            listBoxVisitedCities.DrawMode = DrawMode.OwnerDrawVariable;
        }

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
            string txt = "";

            CitiesLngDict.TryGetValue(vc.CityName, out string value);
            if (value != null && value != "")
                txt = value;
            else
            {
                txt = vc.CityName + " -n";
            }

            //if (CitiesLngDict.TryGetValue(vc.CityName, out string value))
            //    txt = value;
            e.Graphics.DrawString(txt, Font, br, layout_rect);

            // Draw the focus rectangle if appropriate.
            e.DrawFocusRectangle();
        }

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

            FillVisitedCities();
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

            FillVisitedCities();
        }

        public void FillGaragesList()
        {
            listBoxGarages.Items.Clear();

            foreach (Garages garage in from x in GaragesList where !x.IgnoreStatus select x)
            {
                listBoxGarages.Items.Add(garage);
            }

            listBoxGarages.DrawMode = DrawMode.OwnerDrawVariable;
        }

        private int GarageItemMargin = 3;
        private const float GaragePictureHeight = 32;

        private void listBoxGarages_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            // Get the ListBox and the item.
            e.ItemHeight = (int)(GaragePictureHeight + 2 * GarageItemMargin);
        }

        private void listBoxGarages_DrawItem(object sender, DrawItemEventArgs e)
        {
            // Get the ListBox and the item.
            ListBox lst = sender as ListBox;
            string txt = lst.Items[e.Index].ToString();
            Garages grg = (Garages)lst.Items[e.Index];

            // Draw the background.
            e.DrawBackground();

            Image grgicon = GaragesImg[grg.GarageStatus];

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

            // Find the area in which to put the text.
            float x = e.Bounds.Left + picture_width + 3 * GarageItemMargin;
            float y = e.Bounds.Top + GarageItemMargin * 2;
            float width = e.Bounds.Right - GarageItemMargin - x;
            float height = e.Bounds.Bottom - GarageItemMargin - y;
            RectangleF layout_rect = new RectangleF(x, y, width, height);

            // Draw the text.
            e.Graphics.DrawString(txt, this.Font, br, layout_rect);

            // Draw the focus rectangle if appropriate.
            e.DrawFocusRectangle();
        }

        private void buttonGaragesBuy_Click(object sender, EventArgs e)
        {
            if(listBoxGarages.SelectedItems.Count == 0)
            {
                foreach(Garages garage in listBoxGarages.Items)
                {
                    if (garage.GarageStatus == 0)
                        garage.GarageStatus = 2;
                }
            }
            else
                foreach (Garages garage in listBoxGarages.SelectedItems)
                {
                    if (garage.GarageStatus == 0)
                        garage.GarageStatus = 2;
                }

            FillGaragesList();
        }

        private void buttonGaragesUpgrade_Click(object sender, EventArgs e)
        {
            if (listBoxGarages.SelectedItems.Count == 0)
            {
                foreach (Garages garage in listBoxGarages.Items)
                {
                    if (garage.GarageStatus == 2)
                        garage.GarageStatus = 3;
                }
            }
            else
                foreach (Garages garage in listBoxGarages.SelectedItems)
                {
                    if (garage.GarageStatus == 2)
                        garage.GarageStatus = 3;
                }

            FillGaragesList();
        }

        private void buttonGaragesBuyUpgrade_Click(object sender, EventArgs e)
        {
            if (listBoxGarages.SelectedItems.Count == 0)
            {
                foreach (Garages garage in listBoxGarages.Items)
                {
                        garage.GarageStatus = 3;
                }
            }
            else
                foreach (Garages garage in listBoxGarages.SelectedItems)
                {
                        garage.GarageStatus = 3;
                }

            FillGaragesList();
        }

        private void buttonGaragesSell_Click(object sender, EventArgs e)
        {
            if (listBoxGarages.SelectedItems.Count == 0)
            {
                foreach (Garages garage in listBoxGarages.Items)
                {
                    garage.GarageStatus = 0;
                }
            }
            else
                foreach (Garages garage in listBoxGarages.SelectedItems)
                {
                    garage.GarageStatus = 0;
                }

            FillGaragesList();
        }
        
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
                    //DestinationCompIcon = ExtImgLoader(new string[] { files[0] }, 100, 32, 0, 0)[0];
                    if (files.Length > 0)
                        DestinationCompIcon = ExtImgLoader(new string[] { files[0] }, 100, 32, 0, 0)[0];
                    else
                    {
                        DestinationCompIcon = DrawCompanyText(Job.SourceCompany, 100, 32, br);
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

                dest_rect = new RectangleF(e.Bounds.Left + JobsItemMargin * 2 + SourceCompIcon.Width, e.Bounds.Top + JobsItemMargin * 2 + JobsTextHeigh, 32, 32);
                source_rect = new RectangleF(0, 0, 32, 32);
                e.Graphics.DrawImage(CargoTypeImg[Job.Type], dest_rect, source_rect, GraphicsUnit.Pixel);


                // Draw the Urgency picture
                source_rect = new RectangleF(0, 0, 32, 32);
                dest_rect = new RectangleF((e.Bounds.Right - e.Bounds.Left - UrgencyImg[Job.Urgency].Width) / 2, e.Bounds.Top + JobsItemMargin, UrgencyImg[Job.Urgency].Width, UrgencyImg[Job.Urgency].Height);
                e.Graphics.DrawImage(UrgencyImg[Job.Urgency], dest_rect, source_rect, GraphicsUnit.Pixel);
                ////

                // Draw the text.

                string value = "", SourceCityName="", DestinationCityName="";

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

                //CitiesLngDict.TryGetValue(Job.SourceCity, out string SourceCityName);
                //CitiesLngDict.TryGetValue(Job.DestinationCity, out string DestinationCityName);

                // Find the area in which to put the text.
                float x = e.Bounds.Left + JobsItemMargin;
                float y = e.Bounds.Top - JobsItemMargin + JobsTextHeigh / 2;
                float width = (e.Bounds.Right - e.Bounds.Left - JobsItemMargin * 4 - UrgencyImg[Job.Urgency].Width) / 2;
                float height = JobsTextHeigh;//e.Bounds.Bottom - JobsItemMargin - y;
                RectangleF layout_rect = new RectangleF(x, y, width, height);

                string txt = SourceCityName;
                StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Near;
                e.Graphics.DrawString(txt, this.Font, br, layout_rect, format);

                // Find the area in which to put the text.
                x = e.Bounds.Left + width + 3 * JobsItemMargin + UrgencyImg[Job.Urgency].Width;
                layout_rect = new RectangleF(x, y, width, height);
                format.Alignment = StringAlignment.Far;
                txt = DestinationCityName;
                e.Graphics.DrawString(txt, this.Font, br, layout_rect, format);

                // Find the area in which to put the text.
                x = e.Bounds.Left + picture_width + 3 * JobsItemMargin + 32;
                y = e.Bounds.Top + JobsItemMargin * 2 + UrgencyImg[Job.Urgency].Height;
                width = e.Bounds.Right - JobsItemMargin - x;
                height = e.Bounds.Bottom - JobsItemMargin - y;
                layout_rect = new RectangleF(x, y, width, height);

                txt = Job.Cargo;
                e.Graphics.DrawString(txt, this.Font, br, layout_rect);

                // Find the area in which to put Distance text.
                if (Job.Distance == 11111)
                {
                    txt = "";
                }
                else
                {
                    txt = (Job.Distance * DistanceMultiplier).ToString() + " " + ProgSettingsV.DistanceMes; //km";
                }

                if (Job.Ferrytime > 0)
                {
                    txt = (Job.Distance * DistanceMultiplier).ToString() + " " + ProgSettingsV.DistanceMes + " ( ferry " + Job.Ferrytime.ToString() + "h - " + Job.Ferryprice.ToString() + " €)";
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

            g.DrawString(_companyName, new Font(Font.FontFamily, 16) , _brush, rectf, format);
            g.Flush();

            return bmp;
        }

        public void FillcomboBoxCountries()
        {
            foreach (string str in CountriesList)
            {
                comboBoxCountries.Items.Add(str);
            }
        }

        private void comboBoxCountries_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxDestinationCity.SelectedIndex = -1;
            comboBoxDestinationCompany.SelectedIndex = -1;

            triggerDestinationCitiesUpdate();
        }

        public void FillcomboBoxCompanies()
        {
            //start filtering
            List<string> tempCompList = new List<string>();

            foreach (City city in CitiesList.FindAll(x => !x.Disabled))
            {
                List<Company> source = city.ReturnCompanies();

                foreach (Company company in from x in source.Distinct()
                                            where !x.Excluded
                                            select x)
                {
                    if (!tempCompList.Contains(company.CompanyName))
                    {
                        tempCompList.Add(company.CompanyName);
                    }
                }
            }
            tempCompList = tempCompList.OrderBy(x => x).ToList();
            //end filtering
            
            DataTable combDT = new DataTable();
            DataColumn dc = new DataColumn("Company", typeof(string));
            combDT.Columns.Add(dc);

            dc = new DataColumn("CompanyName", typeof(string));
            combDT.Columns.Add(dc);

            //start filling
            combDT.Rows.Add("All", "All");

            foreach (string tempitem in tempCompList)
                if (CompaniesLngDict.TryGetValue(tempitem, out string value))
                    if (value != null && value != "")
                    {
                        combDT.Rows.Add(tempitem, value);
                    }
                    else
                    {
                        combDT.Rows.Add(tempitem, tempitem);
                    }

            comboBoxCompanies.ValueMember = "Company";
            comboBoxCompanies.DisplayMember = "CompanyName";
            comboBoxCompanies.DataSource = combDT;
            //end filling

            comboBoxCountries.SelectedIndex = comboBoxCountries.FindString("All");
            comboBoxCompanies.SelectedIndex = comboBoxCompanies.FindString("All");
        }

        private void comboBoxCompanies_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxDestinationCity.SelectedIndex = -1;
            comboBoxDestinationCompany.SelectedIndex = -1;

            triggerDestinationCitiesUpdate();
        }

        public void FillcomboBoxSourceCityDestinationCity()
        {
            DataTable combDT = new DataTable();
            DataColumn dc = new DataColumn("City", typeof(string));
            combDT.Columns.Add(dc);

            dc = new DataColumn("CityName", typeof(string));
            combDT.Columns.Add(dc);

            //start filling

            //fill source and destination cities
            foreach (City tempcity in from x in CitiesList
                                      where !x.Disabled
                                      select x)
            {
                //if (CitiesLngDict.TryGetValue(tempcity.CityName, out string value))
                CitiesLngDict.TryGetValue(tempcity.CityName, out string value);
                if (value != null && value != "")
                {
                    combDT.Rows.Add(tempcity.CityName, value);
                }
                else
                {
                    combDT.Rows.Add(tempcity.CityName, tempcity.CityName + " -n");
                }
            }
            combDT.DefaultView.Sort = "CityName ASC";

            comboBoxSourceCity.ValueMember = "City";
            comboBoxSourceCity.DisplayMember = "CityName";
            comboBoxSourceCity.DataSource = combDT;
            //end filling

            comboBoxSourceCity.SelectedValue = LastVisitedCity;
            //end
        }

        public void FillcomboBoxCargoList()
        {
            
            DataTable combDT = new DataTable();
            DataColumn dc = new DataColumn("Cargo", typeof(string));
            combDT.Columns.Add(dc);

            dc = new DataColumn("CargoName", typeof(string));
            combDT.Columns.Add(dc);

            foreach (Cargo tempitem in CargoesList)
            {
                string str = tempitem.CargoName;
                string value = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(str);

                if (tempitem.CargoType == 1)
                    value += " [H]";
                else if (tempitem.CargoType == 2)
                    value += " [D]";

                combDT.Rows.Add(str, value);
            }

            combDT.DefaultView.Sort = "CargoName ASC";

            comboBoxCargoList.ValueMember = "Cargo";
            comboBoxCargoList.DisplayMember = "CargoName";
            comboBoxCargoList.DataSource = combDT;

            comboBoxCargoList.SelectedIndex = RandomValue.Next(comboBoxCargoList.Items.Count);
        }

        private void comboBoxSourceCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            string city = comboBoxSourceCity.SelectedValue.ToString();

            comboBoxSourceCompany.SelectedIndex = -1;

            City ccity = CitiesList.Find(x => x.CityName == city);

            List<Company> list = ccity.ReturnCompanies();
            List<Company> list2 = list.FindAll(x => !x.Excluded);


            DataTable combDT = new DataTable();
            DataColumn dc = new DataColumn("Company", typeof(string));
            combDT.Columns.Add(dc);

            dc = new DataColumn("CompanyName", typeof(string));
            combDT.Columns.Add(dc);

            foreach (Company company in list2)
                if (CompaniesLngDict.TryGetValue(company.CompanyName, out string value))
                    if (value != "")
                    {
                        combDT.Rows.Add(company.CompanyName, value);
                    }
                    else
                    {
                        combDT.Rows.Add(company.CompanyName, company.CompanyName);
                    }

            combDT.DefaultView.Sort = "CompanyName ASC";
            
            comboBoxSourceCompany.ValueMember = "Company";
            comboBoxSourceCompany.DisplayMember = "CompanyName";

            comboBoxSourceCompany.DataSource = combDT;

            if (ProgSettingsV.ProposeRandom && (comboBoxSourceCompany.Items.Count > 0))
            {
                comboBoxSourceCompany.SelectedIndex = RandomValue.Next(comboBoxSourceCompany.Items.Count);
            }
        }

        private void comboBoxSourceCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ProgSettingsV.ProposeRandom)
            {
                comboBoxCargoList.SelectedIndex = RandomValue.Next(comboBoxCargoList.Items.Count);
            }
        }

        private void comboBoxDestinationCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxDestinationCity.SelectedIndex >= 0)
            {
                comboBoxDestinationCompany.SelectedIndex = -1;
                comboBoxDestinationCompany.Text = "";

                triggerDestinationCompaniesUpdate();
            }

            if (ProgSettingsV.ProposeRandom && (comboBoxDestinationCompany.Items.Count > 0))
            {
                comboBoxDestinationCompany.SelectedIndex = RandomValue.Next(comboBoxDestinationCompany.Items.Count);

                if ((comboBoxDestinationCompany.Items.Count != 1) && (comboBoxSourceCity.SelectedValue == comboBoxDestinationCity.SelectedValue) )
                {
                    int rnd = 0;
                    while(true)
                    {
                        rnd = RandomValue.Next(comboBoxDestinationCompany.Items.Count);
                        if (comboBoxSourceCompany.SelectedIndex != rnd)
                        {
                            comboBoxDestinationCompany.SelectedIndex = rnd;
                            break;
                        }
                    }                    
                }
            }
        }

        private void triggerDestinationCitiesUpdate()
        {
            if(comboBoxCompanies.SelectedIndex != -1)
                SetupDestinationCities(!(comboBoxCountries.Text == "All"), !(comboBoxCompanies.SelectedValue.ToString() == "All"));// .Text == "All"));
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
            foreach (City tempcity in from x in CitiesList
                                      where !x.Disabled
                                      select x)
            {
                CitiesLngDict.TryGetValue(tempcity.CityName, out string value);
                if (value != null && value != "")
                    combDT.Rows.Add(tempcity.CityName, value);
                else
                {
                    combDT.Rows.Add(tempcity.CityName, tempcity.CityName + " -n");
                }

                //comboBoxSourceCity.Items.Add(tempcity.CityName); //Source
                //comboBoxDestinationCity.Items.Add(tempcity.CityName); //Destination
            }

            comboBoxHQcity.ValueMember = "City";
            comboBoxHQcity.DisplayMember = "CityName";
            comboBoxHQcity.DataSource = combDT;
        }

        private void SetupDestinationCities(bool _country_selected, bool _company_selected)
        {
            DataTable combDT = new DataTable();
            DataColumn dc = new DataColumn("City", typeof(string));
            combDT.Columns.Add(dc);

            dc = new DataColumn("CityName", typeof(string));
            combDT.Columns.Add(dc);

            //start filling
            List<City> cities = CitiesList;

            if (_country_selected && checkBoxFilterDestination.Checked)
            {
                cities = CitiesList.FindAll(x => !x.Disabled && (x.Country == comboBoxCountries.Text));
            }

            if (_country_selected && !checkBoxFilterDestination.Checked)
            {
                cities = CitiesList.FindAll(x => x.Country == comboBoxCountries.Text);
            }

            if (!(_country_selected || checkBoxFilterDestination.Checked))
            {
                cities = CitiesList;
            }

            if (!(_country_selected || !checkBoxFilterDestination.Checked))
            {
                cities = CitiesList.FindAll(x => !x.Disabled);
            }
            
            foreach (City city in cities)
            {

                List<Company> companyList = city.ReturnCompanies();

                if (!_company_selected)
                {
                }
                else
                if (_company_selected && checkBoxFilterDestination.Checked)
                {
                    companyList = companyList.FindAll(x => (x.CompanyName == comboBoxCompanies.SelectedValue.ToString()) && !x.Excluded);
                }
                else
                if (!(_company_selected || !checkBoxFilterDestination.Checked))
                {
                    companyList = companyList.FindAll(x => !x.Excluded);
                }
                else
                if (_company_selected && !checkBoxFilterDestination.Checked)
                {
                    companyList = companyList.FindAll(x => x.CompanyName == comboBoxCompanies.SelectedValue.ToString());
                }

                if (companyList.Count > 0)
                    if (CitiesLngDict.TryGetValue(city.CityName, out string CityNamevalue))
                        if (CityNamevalue != null && CityNamevalue != "")
                            combDT.Rows.Add(city.CityName, CityNamevalue);
                        else
                        {
                            combDT.Rows.Add(city.CityName, city.CityName + " -n");
                        }
                /*
                foreach (Company company in companyList)
                {
                    if (_company_selected)
                    {
                        if (CitiesLngDict.TryGetValue(city.CityName, out string valueName))
                            combDT.Rows.Add(city.CityName, valueName);
                        else
                        {
                            combDT.Rows.Add(city.CityName, city.CityName);
                        }
                    }
                }
                */
            }

            combDT.DefaultView.Sort = "CityName ASC";

            comboBoxDestinationCity.ValueMember = "City";
            comboBoxDestinationCity.DisplayMember = "CityName";
            comboBoxDestinationCity.DataSource = combDT;
            //end filling

            if (comboBoxDestinationCity.Items.Count == 0)
            {
                ShowStatusMessages("e", "message_no_matching_cities");
            }
            else
            {
                ShowStatusMessages("i", "");
                comboBoxDestinationCity.SelectedIndex = RandomValue.Next(comboBoxDestinationCity.Items.Count);
            }
        }

        private void triggerDestinationCompaniesUpdate()
        {
            SetupDestinationCompanies(!(comboBoxCompanies.SelectedValue.ToString() == "All"));//.Text == "All"));
        }

        private void SetupDestinationCompanies(bool _company_selected)
        {
            List<Company> CityCompanies = CitiesList.Find(x => x.CityName == comboBoxDestinationCity.SelectedValue.ToString()).ReturnCompanies();
            List<Company> RealCompanies = CityCompanies.FindAll(x => !x.Excluded);

            if (_company_selected && checkBoxFilterDestination.Checked)
            {
                RealCompanies = RealCompanies.FindAll(x => (x.CompanyName == comboBoxCompanies.SelectedValue.ToString()));
            }

            #region notused
            /*
            if (!(_company_selected || !checkBoxFilterDestination.Checked))
            {
                //list2 = list.FindAll(x => !x.Excluded);
            }

            if (!(_company_selected || checkBoxFilterDestination.Checked))
            {
                //list2 = list.FindAll(x => !x.Excluded);
            }

            if (_company_selected && !checkBoxFilterDestination.Checked)
            {
                if (predicate2 == null)
                {
                    predicate2 = x => x.CompanyName == comboBoxCompanies.Text;
                }
                list2 = list.FindAll(predicate2);
            }
            */
            #endregion

            DataTable combDT = new DataTable();
            DataColumn dc = new DataColumn("Company", typeof(string));
            combDT.Columns.Add(dc);

            dc = new DataColumn("CompanyName", typeof(string));
            combDT.Columns.Add(dc);

            foreach (Company company in RealCompanies)
            {
                CompaniesLngDict.TryGetValue(company.CompanyName, out string value);
                if (value != null && value != "")
                {
                    combDT.Rows.Add(company.CompanyName, value);
                }                    
                else
                {
                    combDT.Rows.Add(company.CompanyName, company.CompanyName);
                }
            }


            combDT.DefaultView.Sort = "CompanyName ASC";

            comboBoxDestinationCompany.ValueMember = "Company";
            comboBoxDestinationCompany.DisplayMember = "CompanyName";

            comboBoxDestinationCompany.DataSource = combDT;
        }

        private void comboBoxDestinationCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ProgSettingsV.ProposeRandom)
            {
                comboBoxCargoList.SelectedIndex = RandomValue.Next(comboBoxCargoList.Items.Count);
            }
        }

        private void comboBoxCargoList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ProgSettingsV.ProposeRandom)
            {
                comboBoxUrgency.SelectedIndex = RandomValue.Next(comboBoxUrgency.Items.Count);
            }
        }

        private void FillFormProfileControls()
        {
            FormUpdatePlayerLevel();

            char[] ADR = Convert.ToString(PlayerProfileData.PlayerSkills[0], 2).PadLeft(6,'0').ToCharArray();
            
            for (int i = 0; i < ADR.Length; i++)
            {
                if (ADR[i] == '1')
                    ADRbuttonArray[i].Checked = true;
            }

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < PlayerProfileData.PlayerSkills[i+1]; j++)
                {
                    SkillButtonArray[i, j].Checked = true;
                }
            }
        }

        private void FormUpdatePlayerLevel()
        {
            int playerlvl = PlayerProfileData.getPlayerLvl()[0];
            labelPlayerLevelNumber.Text = playerlvl.ToString();

            for (int i = PlayerLevelNames.Count - 1; i >= 0; i--)
                if (PlayerLevelNames[i].LevelLimit <= playerlvl)
                {
                    labelPlayerLevelName.Text = PlayerLevelNames[i].LevelName;
                    panelPlayerLevel.BackColor = PlayerLevelNames[i].NameColor;
                    break;
                }

            textBoxPlayerExperience.Text = PlayerProfileData.ExperiencePoints.ToString();
            labelExperienceNxtLvlThreshhold.Text = "/   " + PlayerProfileData.getPlayerLvl()[1].ToString();

        }

        private void FillFormCompanyControls()
        {
            FillHQcities();

            FillGaragesList();
            FillVisitedCities();

            textBoxMoneyAccount.Text = PlayerProfileData.AccountMoney.ToString();
            comboBoxHQcity.SelectedValue = PlayerProfileData.HQcity;
            textBoxUserCompanyName.Text = PlayerProfileData.CompanyName;
        }

        private void buttonPlayerLvlPlus01_Click(object sender, EventArgs e)
        {
            PlayerProfileData.getPlayerExp(int.Parse(labelPlayerLevelNumber.Text) + 1);

            FormUpdatePlayerLevel();
        }

        private void buttonPlayerLvlPlus10_Click(object sender, EventArgs e)
        {
            PlayerProfileData.getPlayerExp(int.Parse(labelPlayerLevelNumber.Text) + 10);

            FormUpdatePlayerLevel();
        }

        private void buttonPlayerLvlMax_Click(object sender, EventArgs e)
        {
            PlayerProfileData.getPlayerExp(150);

            FormUpdatePlayerLevel();
        }

        private void buttonPlayerLvlMinus01_Click(object sender, EventArgs e)
        {
            PlayerProfileData.getPlayerExp(int.Parse(labelPlayerLevelNumber.Text) - 1);

            FormUpdatePlayerLevel();
        }

        private void buttonPlayerLvlMinus10_Click(object sender, EventArgs e)
        {
            PlayerProfileData.getPlayerExp(int.Parse(labelPlayerLevelNumber.Text) - 10);

            FormUpdatePlayerLevel();
        }

        private void buttonPlayerLvlMin_Click(object sender, EventArgs e)
        {
            PlayerProfileData.getPlayerExp(0);

            FormUpdatePlayerLevel();
        }

        private void ToggleVisibility(bool visible)
        {
            foreach (TabPage tp in tabControlMain.TabPages)
                tp.Enabled = visible;
        }

        public void ToggleGame_Click(object sender, EventArgs e)
        {
            Button gamebutton = sender as Button;

            if (gamebutton.Name == "buttonGameETS")
                ToggleGame("ETS");
            else
                ToggleGame("ATS");

            FillAllProfilesPaths();
        }

        public void ToggleGame(string _game)
        {
            if (_game == "ETS")
            {
                Globals.CurrentGame = dictionaryProfiles["ETS2"];
                buttonGameETS.Enabled = false;
                buttonGameATS.Enabled = true;
                GameType = _game;
                buttonGameETS.BackColor = Color.White;
                buttonGameETS.ForeColor = Color.Black;
                buttonGameATS.BackColor = Color.FromKnownColor(KnownColor.Control);
                buttonGameATS.ForeColor = Color.FromKnownColor(KnownColor.ControlText);
            }
            else
            {
                Globals.CurrentGame = dictionaryProfiles["ATS"];
                buttonGameETS.Enabled = true;
                buttonGameATS.Enabled = false;
                GameType = _game;
                buttonGameATS.BackColor = Color.White;
                buttonGameATS.ForeColor = Color.Black;
                buttonGameETS.BackColor = Color.FromKnownColor(KnownColor.Control);
                buttonGameETS.ForeColor = Color.FromKnownColor(KnownColor.ControlText);
            }

        }

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
            CultureInfo ci = Thread.CurrentThread.CurrentUICulture;

            try
            {
                PlainTXTResourceManager rm = new PlainTXTResourceManager();
                ResourceSet set = rm.GetResourceSet(ci, true, true);

                List<string> keys = new List<string>();

                foreach (DictionaryEntry o in set)
                {
                    keys.Add((string)o.Key);
                }

                foreach (string x in keys)
                {
                    try
                    {
                        Controls.Find(x, true)[0].Text = rm.GetString(x, ci);
                    }
                    catch { }
                }
            }
            catch
            {

            }
            //rm.ReleaseAllResources();
        }

        private string GetranslatedString(string _key)
        {
            CultureInfo ci = Thread.CurrentThread.CurrentUICulture;
            try
            {
                PlainTXTResourceManager rm = new PlainTXTResourceManager();
                string res = rm.GetString(_key, ci);
                if(res != null)
                    return res;
                else
                    return _key;
            }
            catch
            {
                return _key;
            }
        }

        private void GetTranslationFiles()
        {
            if (Directory.Exists(Directory.GetCurrentDirectory() + @"\lang") )
            {
                string[] langfiles = Directory.GetFiles(Directory.GetCurrentDirectory() + @"\lang","??-??.txt");
                string langTag;

                string langNameShort;
                string countryShort;

                string flagpath;

                foreach (string lang in langfiles)
                {
                    langTag = File.ReadAllLines(lang, Encoding.UTF8)[0].Split(new char[] { '[', ']' })[1];

                    langNameShort = langTag.Split('-')[0];
                    countryShort = langTag.Split('-')[1];

                    CultureInfo ci = new CultureInfo(langTag, false);

                    char[] a = ci.NativeName.ToCharArray();
                    a[0] = char.ToUpper(a[0]);
                    string CorrectedNativeName = new string(a);

                    ToolStripItem TSitem = new ToolStripMenuItem();
                    TSitem.Name = langNameShort + "_" + countryShort + "_ToolStripMenuItem";
                    TSitem.Text = CorrectedNativeName;

                    TSitem.Click += new EventHandler(toolstripChangeLanguage);//+= ChangeLanguage(TSitem,);

                    flagpath = Directory.GetCurrentDirectory() + @"\lang\flags\" + countryShort + ".png";

                    if (File.Exists(flagpath))
                    {
                        TSitem.Image = new Bitmap(flagpath);
                        TSitem.ImageScaling = ToolStripItemImageScaling.None;
                    }

                    languageToolStripMenuItem.DropDownItems.Add(TSitem);

                    byte[] bytes = Encoding.UTF8.GetBytes(flagpath);
                }
            }
            else
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\lang");
            }
        }        

        private void ClearJobData()
        {
            JobsTotalDistance = 0;
            JobsAmountAdded = 0;

            Array.Resize(ref JobsListAdded, 0);
            Array.Resize(ref ListSavefileCompanysString, 0);
            Array.Resize(ref EconomyEventUnitLinkStringList, 0);

            listBoxAddedJobs.Items.Clear();
            labelJobsListDistance.Text = "Jobs Distance";
            buttonClearJobList.Enabled = false;
        }

        private void CreateUserColorsButtons()
        {
            int padding = 6, width = 40, colorcount = 8;
            //UserColorsList.Count

            for (int i = 0; i < colorcount; i++)
            {
                Button rb = new Button();
                rb.Name = "buttonUC" + i.ToString();
                rb.Text = null;
                rb.Location = new Point(15, 32 + (padding + width) * (i));
                rb.Size = new Size(width, width);
                rb.FlatStyle = FlatStyle.Flat;
                rb.Enabled = false;

                rb.Click += new EventHandler(SelectColor);

                groupBoxUserColors.Controls.Add(rb);                
            }
        }

        private void buttonProfileShareColors_Click(object sender, EventArgs e)
        {

        }

        private void UpdateUserColorsButtons()
        {
            
            int padding = 6, width = 23;//, colorcount = 8;
            /*
            for (int i = 0; i < 5; i++)
            {
                Panel pnl = null;
                string pnlname = "progressbarTruck" + i.ToString();

                if (groupBoxTruckDetails.Controls.ContainsKey(pnlname))
                {
                    pnl = groupBoxTruckDetails.Controls[pnlname] as Panel;
                }

                if (pnl != null)
                {

                }
            }
            */
            for (int i = 0; i < UserColorsList.Count; i++)
            {
                Button btn = null;
                string btnname = "buttonUC" + i.ToString();

                if (groupBoxUserColors.Controls.ContainsKey(btnname))
                {
                    btn = groupBoxUserColors.Controls[btnname] as Button;
                }
                else
                {
                    //Button rb = new Button();
                    btn.Name = "buttonUC" + i.ToString();
                    btn.Text = null;
                    btn.Location = new Point(6 + (padding + width) * (i), 19);
                    btn.Size = new Size(width, 23);
                    btn.FlatStyle = FlatStyle.Flat;
                    //rb.BackColor = UserColorsList[i];
                    btn.Enabled = false;
                    //if (UserColorsList[i].A == 0)
                    //    rb.Text = "X";

                    btn.Click += new EventHandler(SelectColor);

                    groupBoxUserColors.Controls.Add(btn);
                }

                if (btn != null)
                {
                    btn.BackColor = UserColorsList[i];
                    btn.Enabled = true;
                    if (UserColorsList[i].A == 0)
                        btn.Text = "X";
                }
            }
        }

        private void SelectColor(object sender, EventArgs e)
        {
            Button obj = sender as Button;

            OpenPainter.ColorPicker.frmColorPicker frm = new OpenPainter.ColorPicker.frmColorPicker(obj.BackColor);

            frm.Font = SystemFonts.DialogFont;

            if (frm.ShowDialog() == DialogResult.OK)
            {
                obj.BackColor = frm.PrimaryColor;

                int index = int.Parse( obj.Name.Substring(8, 1));

                UserColorsList[index] = frm.PrimaryColor;

                if (frm.PrimaryColor.A != 0)
                    obj.Text = "";
                else
                    obj.Text = "X";

            }
        }

        private void checkBoxRandomDest_CheckedChanged(object sender, EventArgs e)
        {
            ProgSettingsV.ProposeRandom = checkBoxRandomDest.Checked;
        }

    }
}