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
using System.IO;
using System.Globalization;
using System.Windows.Forms;
using System.Data.SqlServerCe;
using System.Threading;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.ComponentModel;
using System.Reflection;
using System.Diagnostics;
using System.Text;

namespace TS_SE_Tool
{
    public partial class FormMain : Form
    {

        private BackgroundWorker worker;

        private void LoadConfig()
        {
            try
            {
                PropertyInfo[] properties = ProgSettingsV.GetType().GetProperties();
                foreach (string line in File.ReadAllLines(Directory.GetCurrentDirectory() + @"\config.cfg"))
                {
                    switch (line.Split(new char[] { '=' })[0])
                    {
                        case "ProgramVersion":
                            {
                                ProgPrevVersion = double.Parse(line.Split(new char[] { '=' })[1]);
                                //ProgSettingsV.ProgramVersion = double.Parse( line.Split(new char[] { '=' })[1]);
                                break;
                            }

                        case "Language":
                            {
                                ProgSettingsV.Language = line.Split(new char[] { '=' })[1];
                                if (ProgSettingsV.Language == "Default")
                                    Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
                                break;
                            }
                        case "JobPickupTime":
                            {
                                ProgSettingsV.JobPickupTime = short.Parse(line.Split(new char[] { '=' })[1]);
                                break;
                            }
                        case "LoopEvery":
                            {
                                ProgSettingsV.LoopEvery = byte.Parse(line.Split(new char[] { '=' })[1]);
                                break;
                            }

                        case "ProposeRandom":
                            {
                                ProgSettingsV.ProposeRandom = bool.Parse( line.Split(new char[] { '=' })[1]);
                                break;
                            }
                        case "TimeMultiplier":
                            {
                                ProgSettingsV.TimeMultiplier = short.Parse(line.Split(new char[] { '=' })[1]);
                                if (ProgSettingsV.TimeMultiplier > 7.0)
                                {
                                    ProgSettingsV.TimeMultiplier = 7.0;
                                }
                                else if (ProgSettingsV.TimeMultiplier < 0.1)
                                {
                                    ProgSettingsV.TimeMultiplier = 0.1;
                                }
                                break;
                            }
                    }
                }

                checkBoxRandomDest.Checked = ProgSettingsV.ProposeRandom;

            }
            catch
            {
                LogWriter("Config.cfg file not found or have wrong format");
                WriteConfig();
            }
        }

        internal void WriteConfig()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(Directory.GetCurrentDirectory() + @"\config.cfg", false))
                {
                    PropertyInfo[] properties = ProgSettingsV.GetType().GetProperties();
                    foreach (PropertyInfo pi in properties)
                    {
                        writer.WriteLine(pi.Name + "=" + pi.GetValue(ProgSettingsV).ToString());
                    }
                }
            }
            catch
            {
                LogWriter("Could not write to " + Directory.GetCurrentDirectory());
                ShowStatusMessages("e", "error_could_not_write_to_file", Directory.GetCurrentDirectory());
            }
        }

        private void LoadExtCountries()
        {
            try
            {
                CountryDictionaryFile = File.ReadAllLines(Directory.GetCurrentDirectory() + @"\lang\CityToCountry.csv");

                for (int i = 0; i < CountryDictionaryFile.Length; i++)
                {
                    CountryDictionary.AddCountry(CountryDictionaryFile[i].Split(new char[] { ';' })[0], CountryDictionaryFile[i].Split(new char[] { ';' })[1]);
                }
            }
            catch
            {
                LogWriter("CityToCountry.csv file is missing in lang directory");
            }
        }

        private void LoadExtCargoes()
        {
            string[] strArray;

            try
            {
                strArray = File.ReadAllLines(Directory.GetCurrentDirectory() + @"\heavy_cargoes.csv");

                for (int i = 0; i < strArray.Length; i++)
                {
                    HeavyCargoList.Add(strArray[i]);
                }
            }
            catch
            {
                strArray = new string[] {
                    "asph_miller", "cable_reel", "concr_beams", "dozer", "locomotive", "metal_center", "mobile_crane", "transformat",
                    "case600", "cat627", "coil", "kalmar240", "kalmar240_s", "komatsu155", "terex3160", "transformer", "wirtgen250"
                    };

                File.WriteAllLines(Directory.GetCurrentDirectory() + @"\heavy_cargoes.csv", strArray);

                LogWriter("Default heavy_cargoes.csv created");

                for (int i = 0; i < strArray.Length; i++)
                {
                    HeavyCargoList.Add(strArray[i]);
                }
            }
        }

        private void LoadCompaniesLng()
        {
            try
            {
                string[] tempFile = File.ReadAllLines(Directory.GetCurrentDirectory() + @"\lang\companies_translate.txt");

                for (int i = 0; i < tempFile.Length; i++)
                {
                    CompaniesLngDict.Add(tempFile[i].Split(new char[] { ';' })[0], tempFile[i].Split(new char[] { ';' })[1]);
                }
            }
            catch
            {
                LogWriter("companies_translate.txt file is missing");
            }
        }

        private void LoadCitiesLng()
        {
            try
            {
                string[] tempFile = File.ReadAllLines(Directory.GetCurrentDirectory() + @"\lang\cities_translate.txt");

                for (int i = 0; i < tempFile.Length; i++)
                {
                    string[] tmp = tempFile[i].Split(new char[] { ';' });
                    if (tmp[0] != "")
                        CitiesLngDict.Add(tmp[0], tmp[1]);
                }
            }
            catch
            {
                LogWriter("cities_translate.txt file is missing");
            }
        }

        private void LoadExtImages()
        {
            MemoryStream ms = new MemoryStream();
            ImageFromDDS(@"img\service_ico.dds").Save(ms, ImageFormat.Png);
            RepairImg = Image.FromStream(ms);
            ms.Dispose();

            ms = new MemoryStream();
            ImageFromDDS(@"img\gas_ico.dds").Save(ms, ImageFormat.Png);
            RefuelImg = Image.FromStream(ms);
            ms.Dispose();

            string[] imgpaths;
            imgpaths = new string[] { @"img\" + GameType + @"\adr_1.dds", @"img\" + GameType + @"\adr_2.dds", @"img\" + GameType + @"\adr_3.dds", @"img\" + GameType + @"\adr_4.dds", @"img\" + GameType + @"\adr_6.dds", @"img\" + GameType + @"\adr_8.dds" };
            ADRImgS = ExtImgLoader(imgpaths, 46, 46, 9, 9, 35, 35);

            imgpaths = new string[] { @"img\" + GameType + @"\adr_1_grey.dds", @"img\" + GameType + @"\adr_2_grey.dds", @"img\" + GameType + @"\adr_3_grey.dds", @"img\" + GameType + @"\adr_4_grey.dds", @"img\" + GameType + @"\adr_6_grey.dds", @"img\" + GameType + @"\adr_8_grey.dds" };
            ADRImgSGrey = ExtImgLoader(imgpaths, 46, 46, 9, 9, 35, 35);

            imgpaths = new string[] { @"img\skill_bar_s.dds", @"img\skill_bar_s2.dds", @"img\skill_bar1.dds", @"img\skill_bar2.dds", @"img\skill_bar3.dds" };
            int y = 9;
            for (int i = 0; i < 5; i++)
            {
                if (i == 2)
                    y = 8;
                ms = new MemoryStream();
                Bitmap temp = ImageFromDDS(imgpaths[i]);
                temp.Clone(new Rectangle(9, y, 46, 46), temp.PixelFormat).Save(ms, ImageFormat.Png);
                SkillImgSBG[i] = Image.FromStream(ms);
                ms.Dispose();
            }

            imgpaths = new string[] { @"img\skill_adr.dds", @"img\skill_distance.dds", @"img\skill_heavy.dds", @"img\skill_fragile.dds", @"img\skill_jit.dds", @"img\skill_mechanical.dds" };
            SkillImgS = ExtImgLoader(imgpaths, 64, 64, 64, 0);

            imgpaths = new string[] { @"img\profiles.dds", @"img\comp_man.dds", @"img\truck_service.dds", @"img\trailers.dds", @"img\company_job.dds", @"img\cargo_market.dds", @"img\maps.dds" };
            TabpagesImages.Images.AddRange (ExtImgLoader(imgpaths, 64, 64, 64, 0));

            imgpaths = new string[] { @"img\garage_free_ico.dds", @"img\garage_free_ico.dds", @"img\garage_small_ico.dds", @"img\garage_large_ico.dds", @"img\garage_free_ico.dds", @"img\garage_free_ico.dds", @"img\garage_tiny_ico.dds" };
            GaragesImg = ExtImgLoader(imgpaths, 32, 32, 0, 0);

            imgpaths = new string[] { @"img\city_pin_0.dds", @"img\city_pin_1.dds"};
            CitiesImg = ExtImgLoader(imgpaths, 32, 32, 0, 0);

            imgpaths = new string[] { @"img\easy.dds", @"img\normal.dds", @"img\hard.dds" };
            UrgencyImg = ExtImgLoader(imgpaths, 32, 32, 0, 0);

            imgpaths = new string[] { @"img\none_32.dds", @"img\heavy.dds", @"img\articulated.dds" };
            CargoTypeImg =  ExtImgLoader(imgpaths, 32, 32, 0, 0);

            imgpaths = new string[] { @"img\" + GameType + @"\engine.dds", @"img\" + GameType + @"\transmission.dds", @"img\" + GameType + @"\chassis.dds", @"img\" + GameType + @"\cabin.dds", @"img\" + GameType + @"\tyres.dds" };
            TruckPartsImg = ExtImgLoader(imgpaths, 64, 64, 0, 0);

            imgpaths = new string[] { @"img\engine.dds", @"img\transmission.dds", @"img\chassis.dds",  @"img\tyres.dds" };
            TrailerPartsImg = ExtImgLoader(imgpaths, 64, 64, 0, 0);

            imgpaths = new string[] { @"img\ETS\game_n.dds", @"img\ATS\game_n.dds" };
            GameIconeImg = ExtImgLoader(imgpaths, 32, 32, 0, 0);
        }

        private Image[] ExtImgLoader(string[] _filenamesarray, int _width, int _height, int _x, int _y )
        {
            MemoryStream ms;
            Image[] tempImgarray = new Image[_filenamesarray.Length];

            for (int i = 0; i < _filenamesarray.Length; i++)
            {
                try
                {
                    ms = new MemoryStream();
                    Bitmap temp = ImageFromDDS(_filenamesarray[i]);
                    temp.Clone(new Rectangle(_x, _y, _width, _height), temp.PixelFormat).Save(ms, ImageFormat.Png);
                    tempImgarray[i] = Image.FromStream(ms);
                    ms.Dispose();
                }
                catch
                {
                    tempImgarray[i] = new Bitmap(_width, _height);
                }
            }

            return tempImgarray;
        }

        private Image[] ExtImgLoader(string[] _filenamesarray, int _width, int _height, int _x, int _y, int _newwidth, int _newheight)
        {
            MemoryStream ms;
            Image[] tempImgarray = new Image[_filenamesarray.Length];

            for (int i = 0; i < _filenamesarray.Length; i++)
            {
                try
                {
                    ms = new MemoryStream();
                    Bitmap temp = ImageFromDDS(_filenamesarray[i]);
                    temp.Clone(new Rectangle(_x, _y, _width, _height), temp.PixelFormat).Save(ms, ImageFormat.Png);
                    tempImgarray[i] = new Bitmap(Image.FromStream(ms), new Size(_newwidth, _newheight));
                    ms.Dispose();
                }
                catch
                {
                    tempImgarray[i] = new Bitmap(_width, _height);
                }
            }

            return tempImgarray;
        }

        private void SaveCompaniesLng()
        {
            CompaniesList = CompaniesList.Distinct().OrderBy(x => x).ToList();

            List<string> newCompanies = new List<string>();

            foreach (string tempitem in CompaniesList )
            {
                if (!CompaniesLngDict.TryGetValue(tempitem, out string value))
                {
                    newCompanies.Add(tempitem);
                }
            }

            try
            {
                using (StreamWriter writer = new StreamWriter(Directory.GetCurrentDirectory() + @"\lang\companies_translate.txt", true))
                {
                    foreach (string str in newCompanies)
                    {
                        writer.WriteLine(str + ";");
                    }
                }
            }
            catch
            {
                LogWriter("companies_translate.txt file is missing");
            }
        }

        private void SaveCitiesLng()
        {
            CitiesList = CitiesList.Distinct().OrderBy(x => x.CityName).ToList();

            List<string> newCompanies = new List<string>();

            foreach (City tempitem in CitiesList)
            {
                if (!CitiesLngDict.TryGetValue(tempitem.CityName, out string value))
                {
                    newCompanies.Add(tempitem.CityName);
                }
            }

            try
            {
                using (StreamWriter writer = new StreamWriter(Directory.GetCurrentDirectory() + @"\lang\cities_translate.txt", true))
                {
                    foreach (string str in newCompanies)
                    {
                        writer.WriteLine(str + ";");
                    }
                }
            }
            catch
            {
                LogWriter("companies_translate.txt file is missing");
            }
        }

        private void ExportFormControlstoLanguageFile()
        {
            string filename = Directory.GetCurrentDirectory() + @"\lang\base_lngfile.txt";

            if (!File.Exists(filename))
            {
                File.CreateText(filename);
            }

            try
            {
                using (StreamWriter writer = new StreamWriter(filename, true))
                {
                    foreach (Control x in Controls)
                    {
                        if (x.Text != "" && (x is Label || x is CheckBox || x is Button || x is GroupBox))
                        {
                            if (x.Text.Any(z => char.IsLetter(z)))
                                writer.WriteLine(x.Name + "=" + x.Text);
                        }

                        if (x is GroupBox)
                        {
                            foreach (Control xc in x.Controls)
                            {
                                if (xc.Text != "" && (xc is Label || xc is CheckBox || xc is Button || xc is GroupBox))
                                {
                                    if (xc.Text.Any(z => char.IsLetter(z)))
                                        writer.WriteLine(xc.Name + "=" + xc.Text);
                                }
                            }
                        }

                        if (x is TabControl)
                        {
                            TabControl pages = x as TabControl;
                            
                            foreach (TabPage page in pages.TabPages)
                            {
                                foreach (Control y in page.Controls)
                                {
                                    if (y.Text != "" && (y is Label || y is CheckBox || y is Button || y is GroupBox))
                                    {
                                        if (y.Text.Any(z => char.IsLetter(z)))
                                            writer.WriteLine(y.Name + "=" + y.Text);
                                    }

                                    if (y is GroupBox)
                                    {
                                        foreach (Control yc in y.Controls)
                                        {
                                            if (yc.Text != "" && (yc is Label || yc is CheckBox || yc is Button || yc is GroupBox))
                                            {
                                                if (yc.Text.Any(z => char.IsLetter(z)))
                                                    writer.WriteLine(yc.Name + "=" + yc.Text);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        
                    }
               }
            }
            catch
            {
                LogWriter("base_lngfile file is missing");
            }
        }

        public static void LogWriter(string _error)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(Directory.GetCurrentDirectory() + @"\log.log", true))
                {
                    writer.WriteLine(DateTime.Now + ": " + _error);
                }
            }
            catch
            {
            }
        }

        private byte[] LoadFileToMemory(string _filePath)
        {
            byte[] _buffer;
            try
            {
                _buffer = File.ReadAllBytes(_filePath);
            }
            catch
            {
                LogWriter("Could not find file in: " + _filePath);
                ShowStatusMessages("e", "error_could_not_find_file");

                FileDecoded = false;
                return null;
            }
            return _buffer;
        }

        private void LoadSaveFile_Click(object sender, EventArgs e)
        {
            //SetDefaultValues(false);

            buttonDecryptSave.Enabled = false;
            buttonLoadSave.Enabled = false;
            buttonGameETS.Enabled = false;
            buttonGameATS.Enabled = false;

            LoadSaveFile(); //Load save file

            //GC
            GC.Collect();
            //GC.WaitForPendingFinalizers();
        }

        private void LoadSaveFile()
        {
            //this.config_file[0] = this.textField_enter_directory.Text;
            
            WriteConfig();
            ToggleVisibility(false);
            SetDefaultValues(false);

            ClearProfilePage();
            ClearJobData();

            ShowStatusMessages("i", "message_decoding_save_file");

            SavefilePath = Globals.SavesHex[comboBoxSaves.SelectedIndex];

            string SiiInfoPath = SavefilePath + @"\info.sii";

            string SiiSavePath = SavefilePath + @"\game.sii";

            string SiiProfilePath = Globals.ProfilesHex[comboBoxProfiles.SelectedIndex] + @"\profile.sii";

            //Profile Info
            if (!File.Exists(SiiProfilePath))
            {
                LogWriter("File does not exist in " + SiiProfilePath);
                ShowStatusMessages("e", "error_could_not_find_file");
            }
            else
            {
                FileDecoded = false;
                try
                {
                    int decodeAttempt = 0;
                    while (decodeAttempt < 5)
                    {
                        tempProfileFileInMemory = DecodeFile(SiiProfilePath);

                        if (FileDecoded)
                        {
                            break;
                        }

                        decodeAttempt++;
                    }

                    if (decodeAttempt == 5)
                    {
                        ShowStatusMessages("e", "error_could_not_decode_file");
                        LogWriter("Could not decrypt after 5 attempts");
                    }
                }
                catch
                {
                    LogWriter("Could not read: " + SiiProfilePath);
                }

                if ((tempProfileFileInMemory == null) || (tempProfileFileInMemory[0] != "SiiNunit"))
                {
                    LogWriter("Wrongly decoded Profile file or wrong file format");
                    ShowStatusMessages("e", "error_file_not_decoded");
                }
                else if (tempProfileFileInMemory != null)
                {
                    //LastModifiedTimestamp = File.GetLastWriteTime(SiiInfoPath);
                    //textBoxDynamic.Text += LastModifiedTimestamp.ToString();

                    CheckProfileInfoData();
                }
            }

            tempProfileFileInMemory = null; //clearmemory
            //End Profile Info

            //Save info
            if (!File.Exists(SiiInfoPath))
            {
                LogWriter("File does not exist in " + SiiInfoPath);
                ShowStatusMessages("e", "error_could_not_find_file");
            }
            else
            {
                FileDecoded = false;
                try
                {
                    int decodeAttempt = 0;
                    while (decodeAttempt < 5)
                    {
                        tempInfoFileInMemory = DecodeFile(SiiInfoPath);

                        if (FileDecoded)
                        {
                            break;
                        }

                        decodeAttempt++;
                    }

                    if (decodeAttempt == 5)
                    {
                        ShowStatusMessages("e", "error_could_not_decode_file");
                        LogWriter("Could not decrypt after 5 attempts");
                    }
                }
                catch
                {
                    LogWriter("Could not read: " + SiiInfoPath);
                }

                if ((tempInfoFileInMemory == null) || (tempInfoFileInMemory[0] != "SiiNunit"))
                {
                    LogWriter("Wrongly decoded Info file or wrong file format");
                    ShowStatusMessages("e", "error_file_not_decoded");
                }
                else if (tempInfoFileInMemory != null)
                {
                    //LastModifiedTimestamp = File.GetLastWriteTime(SiiInfoPath);
                    //textBoxDynamic.Text += LastModifiedTimestamp.ToString();

                    CheckSaveInfoData();
                }
            }

            tempInfoFileInMemory = null; //clearmemory

            //endinfo

            if (SavefileVersion > 0 && SavefileVersion != SupportedSavefileVersionETS2)
            {
                MessageBox.Show("Savefile version don't supported.\nYou cann't edit file with this version, but you can try to decode it.", "Wrong version");
                return;
                //goto LoadSavefileEnd
            }
            else if (SavefileVersion == 0)
            {
                DialogResult result = MessageBox.Show("Savefile version was not recognised.\nDo you want to continue?", "Version not recognised", MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                    return;
            }

            if (!File.Exists(SiiSavePath))
            {
                LogWriter("File does not exist in " + SavefilePath);
                ShowStatusMessages("e", "error_could_not_find_file");
            }
            else
            {
                //Game type for custom path
                if (SavefilePath.Contains("Euro Truck"))
                {
                    GameType = "ETS";
                }
                else
                {
                    GameType = "ATS";
                }

                /*Profile name for custom path
                 * 
                string[] strArray = Globals.SavesHex[comboBoxSaves.SelectedIndex].Split(new char[] { '\\' });
                string hexValue = strArray[strArray.Length - 3];
                hexValue = FromHexToString(hexValue);
                Label_Profile.Text = hexValue;
                */
                FileDecoded = false;
                try
                {
                    int decodeAttempt = 0;
                    while (decodeAttempt < 5)
                    {
                        tempSavefileInMemory = DecodeFile(SiiSavePath);

                        if (FileDecoded)
                        {
                            break;
                        }

                        decodeAttempt++;
                    }

                    if (decodeAttempt == 5)
                    {
                        ShowStatusMessages("e", "error_could_not_decode_file");
                        LogWriter("Could not decrypt after 5 attempts");
                    }

                }
                catch
                {
                    LogWriter("Could not read: " + SiiSavePath);
                }

                if ((tempSavefileInMemory == null) || (tempSavefileInMemory[0] != "SiiNunit"))
                {
                    LogWriter("Wrongly decoded Save file or wrong file format");
                    ShowStatusMessages("e", "error_file_not_decoded");
                }
                else if (tempSavefileInMemory != null)
                {
                    LastModifiedTimestamp = File.GetLastWriteTime(SiiSavePath);

                    InsertDataIntoDatabase("GameTypesTable");

                    comboBoxCountries.Items.Add("All");

                    worker = new BackgroundWorker();
                    worker.WorkerReportsProgress = true;
                    worker.DoWork += PrepareData;//Start;
                    worker.ProgressChanged += worker_ProgressChanged;
                    worker.RunWorkerCompleted += worker_RunWorkerCompleted;

                    worker.RunWorkerAsync();
                }
            }
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            toolStripProgressBarMain.Value = e.ProgressPercentage;
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            toolStripProgressBarMain.Value = 0;
            PopulateFormControlsk();

            buttonDecryptSave.Enabled = false;
            buttonLoadSave.Enabled = true;

            ToggleGame(GameType);
            ToggleVisibility(true);

        }

        //button_save_file
        private void buttonWriteSave_Click(object sender, EventArgs e)
        {
            ToggleVisibility(false);
            buttonWriteSave.Enabled = false;
            WriteSaveFile(); //Save save file with or without changes

            MessageBox.Show("File saved", "Saving", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void WriteSaveFile()
        {
            string SiiSavePath = SavefilePath + @"\game.sii";

            ShowStatusMessages("i", "message_saving_file");

            if (File.GetLastWriteTime(SiiSavePath) > LastModifiedTimestamp)
            {
                ShowStatusMessages("e", "error_file_was_modified");
                LogWriter("Save game was modified - reload file to prevent progress loss");
            }
            else
            {
                PrepareEvents();
                PrepareVisitedCities();

                File.WriteAllText(SiiSavePath, tempSavefileInMemory[0] + "\n");

                using (StreamWriter writer = new StreamWriter(SiiSavePath, true))
                {

                    bool EconomySection = false, PlayerSection = false;
                    bool editedcompany = false, visitedcitycompany = false, insidecompany = false, editedtruck = false, insidetruck = false;
                    int JobIndex = 0, truckaccCount = 0;
                    string trucknameless = "";


                    for (int line = 1; line < tempSavefileInMemory.Length; line++)
                    {
                        string SaveInMemLine = tempSavefileInMemory[line];

                        if (SaveInMemLine.StartsWith("economy : _nameless"))
                        {
                            EconomySection = true;
                            goto EndWrite;
                        }

                        if (EconomySection && SaveInMemLine.StartsWith("}"))
                        {
                            EconomySection = false;
                            goto EndWrite;
                        }

                        if (EconomySection)
                        {
                            //Experience points
                            if (SaveInMemLine.StartsWith(" experience_points"))
                            {
                                writer.WriteLine(" experience_points: " + PlayerProfileData.ExperiencePoints.ToString());
                                continue;
                            }

                            //Skills
                            if (SaveInMemLine.StartsWith(" adr:"))
                            {
                                char[] ADR = Convert.ToString(PlayerProfileData.PlayerSkills[0], 2).PadLeft(6, '0').ToCharArray();
                                Array.Reverse(ADR);
                                
                                writer.WriteLine(" adr: " + Convert.ToByte(new string(ADR), 2));
                                continue;
                            }
                            if (SaveInMemLine.StartsWith(" long_dist:"))
                            {
                                writer.WriteLine(" long_dist: " + PlayerProfileData.PlayerSkills[1].ToString());
                                continue;
                            }
                            if (SaveInMemLine.StartsWith(" heavy:"))
                            {
                                writer.WriteLine(" heavy: " + PlayerProfileData.PlayerSkills[2].ToString());
                                continue;
                            }
                            if (SaveInMemLine.StartsWith(" fragile:"))
                            {
                                writer.WriteLine(" fragile: " + PlayerProfileData.PlayerSkills[3].ToString());
                                continue;
                            }
                            if (SaveInMemLine.StartsWith(" urgent:"))
                            {
                                writer.WriteLine(" urgent: " + PlayerProfileData.PlayerSkills[4].ToString());
                                continue;
                            }
                            if (SaveInMemLine.StartsWith(" mechanical:"))
                            {
                                writer.WriteLine(" mechanical: " + PlayerProfileData.PlayerSkills[5].ToString());
                                continue;
                            }

                            //User Colors
                            if (SaveInMemLine.StartsWith(" user_colors["))
                            {
                                string userColor;
                                int userColorID = int.Parse(SaveInMemLine.Split(new char[] { '[', ']' })[1]);

                                if (UserColorsList[userColorID] == Color.FromArgb(0, 0, 0, 0))
                                {
                                    userColor = "0";
                                }
                                else if (UserColorsList[userColorID] == Color.FromArgb(255, 255, 255, 255))
                                {
                                    userColor = "nil";
                                }
                                else
                                {
                                    Byte[] bytes = new Byte[] { UserColorsList[userColorID].R, UserColorsList[userColorID].G, UserColorsList[userColorID].B, 255};
                                    uint temp = BitConverter.ToUInt32(bytes, 0);

                                    userColor = temp.ToString();
                                }

                                writer.WriteLine(" user_colors[" + userColorID.ToString() + "]: " + userColor);
                                //line++;
                                continue;
                            }


                            //Visited cities
                            if (SaveInMemLine.StartsWith(" visited_cities:"))
                            {
                                int visitedcitiesbefore = int.Parse( SaveInMemLine.Split(new char[] { ':' })[1]);
                                line += visitedcitiesbefore;

                                List<VisitedCity> newvc = VisitedCities.FindAll(x => x.Visited && x.VisitCount > 0);

                                int visitedcitiesafter = newvc.Count;

                                writer.WriteLine(" visited_cities: " + visitedcitiesafter);

                                int vcindex = 0;
                                foreach (VisitedCity vc in newvc)
                                {
                                    writer.WriteLine(" visited_cities[" + vcindex + "]: " + vc.Name);
                                    vcindex++;
                                }

                                continue;
                            }

                            if (SaveInMemLine.StartsWith(" visited_cities_count:"))
                            {
                                int visitedcitiesbefore = int.Parse(SaveInMemLine.Split(new char[] { ':' })[1]);
                                line += visitedcitiesbefore;

                                List<VisitedCity> newvc = VisitedCities.FindAll(x => x.Visited && x.VisitCount > 0);

                                int visitedcitiesafter = newvc.Count;

                                writer.WriteLine(" visited_cities_count: " + visitedcitiesafter);

                                int vcindex = 0;
                                foreach (VisitedCity vc in newvc)
                                {
                                    writer.WriteLine(" visited_cities_count[" + vcindex + "]: " + vc.VisitCount);
                                    vcindex++;
                                }

                                continue;
                            }
                        }

                        //Account Money
                        if (tempSavefileInMemory[line].StartsWith(" money_account:"))
                        {
                            writer.WriteLine(" money_account: " + PlayerProfileData.AccountMoney.ToString());
                            continue;
                        }

                        //Player section
                        if (tempSavefileInMemory[line].StartsWith("player :"))
                        {
                            PlayerSection = true;
                            goto EndWrite;
                        }

                        if (PlayerSection && tempSavefileInMemory[line].StartsWith("}"))
                        {
                            PlayerSection = false;
                            goto EndWrite;
                        }
                        if (PlayerSection)
                        {
                            //HQ city
                            if (SaveInMemLine.StartsWith(" hq_city:"))
                            {
                                writer.WriteLine(" hq_city: " + PlayerProfileData.HQcity);
                                continue;
                            }
                        }

                        //fill queue
                        if (SaveInMemLine.StartsWith("economy_event_queue :"))
                        {
                            writer.WriteLine(SaveInMemLine);
                            line++;
                            writer.WriteLine(tempSavefileInMemory[line]);
                            int EconomyEventQueueNumber = 0;

                            while (EconomyEventQueueNumber < EconomyEventsTable.GetLength(0))
                            {
                                writer.WriteLine(string.Concat(new object[] { " data[", EconomyEventQueueNumber, "]: ", EconomyEventsTable[EconomyEventQueueNumber, 0] }));
                                EconomyEventQueueNumber++;
                            }

                            line += EconomyEventQueueNumber;
                            writer.WriteLine("}");
                            writer.WriteLine("");
                            line += 2;

                            for (int k = 0; k < EconomyEventsTable.GetLength(0); k++)
                            {
                                for (int m = 1; m < EconomyEventsTable.GetLength(1); m++)
                                {
                                    if (m == 2)
                                    {
                                        writer.WriteLine(" time: " + EconomyEventsTable[k, m]);
                                    }
                                    else
                                    {
                                        writer.WriteLine(EconomyEventsTable[k, m]);
                                    }
                                }

                                writer.WriteLine("}");
                                writer.WriteLine("");
                                line += 6;
                            }

                            continue;
                        }

                        if (insidecompany && SaveInMemLine.StartsWith("}"))
                        {
                            insidecompany = false;
                        }

                        if (SaveInMemLine.StartsWith("company :"))
                        {
                            insidecompany = true;
                            string companycity = SaveInMemLine.Split(new char[] { '.' })[3].Split(new char[] { ' ' })[0];

                            //if (VisitedCities.Find(x => x.Name == companycity && x.Visited == true) != null)
                            //    visitedcitycompany = true;

                            visitedcitycompany = VisitedCities.Find(x => x.Name == companycity).Visited;

                            /*
                            if(Array.Find(ListSavefileCompanysString, x => x.Equals(SaveInMemLine)) != null)
                            {
                                editedcompany = true;
                                JobIndex = j;

                                continue;
                            }
                            */
                            for (int j = 0; j < ListSavefileCompanysString.Length; j++)
                            {
                                //find edited company
                                if (SaveInMemLine == ListSavefileCompanysString[j])
                                {
                                    editedcompany = true;
                                    JobIndex = j;

                                    continue;
                                }
                            }
                        }

                        //fill new job data
                        if (editedcompany && SaveInMemLine.StartsWith("job_offer_data : "))
                        {
                            writer.WriteLine(SaveInMemLine);
                            writer.WriteLine(JobsListAdded[JobIndex]);
                            line += 11;
                            editedcompany = false;

                            continue;
                        }

                        if (insidecompany && SaveInMemLine.StartsWith(" discovered:"))
                        {
                            writer.WriteLine(" discovered: " + visitedcitycompany);
                            visitedcitycompany = false;

                            continue;
                        }

                        
                        //find vehicle
                        if (SaveInMemLine.StartsWith("vehicle :"))
                        {
                            trucknameless = SaveInMemLine.Split(new char[] { ' ' })[2];

                            if (UserTruckDictionary.ContainsKey(trucknameless))
                            {
                                editedtruck = true;
                                insidetruck = true;                                
                            }

                            //continue;
                        }

                        if (insidetruck && SaveInMemLine.StartsWith("}"))
                        {
                            insidetruck = false;
                        }

                        if (insidetruck && SaveInMemLine.StartsWith(" fuel_relative:"))
                        {
                            List<string> temp = UserTruckDictionary[trucknameless].Parts.Find(x => x.PartType == "truckdata").PartData;

                            writer.WriteLine(temp.Find(x => x.StartsWith(" fuel_relative:")));
                            continue;
                        }

                        if (insidetruck && SaveInMemLine.StartsWith(" accessories:"))
                        {
                            truckaccCount = int.Parse(SaveInMemLine.Split(new char[] { ':' })[1]);
                        }

                        //edit vehicle accessory
                        if (editedtruck && SaveInMemLine.StartsWith("vehicle_"))
                        {
                            string partnameless = SaveInMemLine.Split(new char[] { ' ' })[2];
                            writer.WriteLine(SaveInMemLine);
                            line++;

                            List<string> temp = UserTruckDictionary[trucknameless].Parts.Find(x => x.PartNameless == partnameless).PartData;

                            foreach (string tempdataline in temp)
                            {
                                writer.WriteLine(tempdataline);
                            }
                            
                            while (tempSavefileInMemory[line] != "}")
                            {
                                line++;
                            }
                            
                            truckaccCount--;
                            writer.WriteLine(tempSavefileInMemory[line]);
                            continue;
                        }                       

                        EndWrite:
                        writer.WriteLine(SaveInMemLine);
                    }
                }
            }

            ShowStatusMessages("i", "message_file_saved");
            LastModifiedTimestamp = File.GetLastWriteTime(SiiSavePath);

            //dispose attempt
            SetDefaultValues(false);

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public void ExportDB()
        {
            GetAllDistancesFromDB();

            using (StreamWriter writer = new StreamWriter(Directory.GetCurrentDirectory() + @"\Database.txt", false))
            {
                foreach (string[] strArray in RouteList.GetRoutes())
                {
                    writer.WriteLine(strArray[0] + ";" + strArray[1] + ";" + strArray[2] + ";" + strArray[3] + ";" + strArray[4] + ";" + strArray[5] + ";" + strArray[6]);
                }
            }
        }

        public void ImportDB()
        {
            LogWriter("Import started");
            ShowStatusMessages("i", "message_import_database");

            try
            {
                string[] DatabaseTXTArray = File.ReadAllLines(Directory.GetCurrentDirectory() + @"\Database.txt");

                int Line = 0;

                while (Line < DatabaseTXTArray.Length)
                {
                    string[] TxtDBline = DatabaseTXTArray[Line].Split(new char[] { ';' });
                    
                    DistancesTable.Rows.Add(TxtDBline[0], TxtDBline[1], TxtDBline[2], TxtDBline[3], int.Parse(TxtDBline[4]), int.Parse(TxtDBline[5]), int.Parse(TxtDBline[6]));

                    Line++;
                }

                AddDistances_DataTableToDB_Bulk(DistancesTable);
            }
            catch
            {
                LogWriter("Import failed, wrong file format or missing file");
                ShowStatusMessages("e", "error_during_importing_db");
            }

            LogWriter("Import finished");
        }

        private void GetTranslationFiles()
        {
            if (Directory.Exists(Directory.GetCurrentDirectory() + @"\lang"))
            {
                string[] langfiles = Directory.GetFiles(Directory.GetCurrentDirectory() + @"\lang", "??-??.txt");
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

        private void GetCompaniesCargoInOut()
        {
            if (Directory.Exists(Directory.GetCurrentDirectory() + @"\gameref"))
            {
                string[] dlcFolders = Directory.GetDirectories(Directory.GetCurrentDirectory() + @"\gameref\" + GameType);

                foreach(string dlcFolder in dlcFolders)
                {
                    if (Directory.Exists(dlcFolder + @"\def\company"))
                    {
                        string[] companyFolders = Directory.GetDirectories(dlcFolder + @"\def\company");

                        foreach (string companyFolder in companyFolders)
                        {
                            if (Directory.Exists(companyFolder + @"\out"))
                            {
                                string company = companyFolder.Split(new string[] { "\\" }, StringSplitOptions.None).Last();

                                string[] cargoes = Directory.GetFiles(companyFolder + @"\out", "*.sii");
                                List<string> tempOutCargo = new List<string>();

                                foreach (string cargo in cargoes)
                                {
                                    string tempcargo = cargo.Split(new string[] { "\\" }, StringSplitOptions.None).Last().Split(new string[] { ".sii" }, StringSplitOptions.None)[0];

                                    tempOutCargo.Add(tempcargo);
                                }

                                if(!ExternalCompanies.Exists(x => x.CompanyName == company))
                                {
                                    ExternalCompanies.Add(new ExtCompany(company));
                                }

                                ExternalCompanies.Find(x => x.CompanyName == company).AddCargoOut(tempOutCargo);
                            }
                        }
                    }
                }
            }
            else
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\gameref");
            }
        }
    }
}