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
using System.Windows.Forms;
using System.Linq;
using System.IO;
using System.Data;
using System.Data.SqlServerCe;
using System.Collections.Generic;
using System.Threading;
using System.Drawing;
using System.Globalization;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;

using ErikEJ.SqlCe;

using TS_SE_Tool.Utilities;
using TS_SE_Tool.Save.Items;

namespace TS_SE_Tool
{
    public partial class FormMain : Form
    {
        private void NewPrepareData()
        {
            IO_Utilities.LogWriter("Prepare started");
            UpdateStatusBarMessage.ShowStatusMessage(SMStatus.Info, "message_preparing_data");

            SiiNunitData = new SiiNunit(tempSavefileInMemory);

            ExtraPrepareStuff();

            UpdateStatusBarMessage.ShowStatusMessage(SMStatus.Info, "message_operation_finished");
            IO_Utilities.LogWriter("Prepare ended");
        }

        private void ExtraPrepareStuff()
        {
            workerLoadSaveFile.ReportProgress(80);

            //namelessList.Sort();
            //namelessList = namelessList.Distinct().ToList();

            PreparePlayerDictionariesInitial();
            PrepareCitiesInitial();
            PrepareGaragesInitial();
            PrepareVisitedCitiesInitial();
            PrepareGPSInitial();
            PrepareCargoTrailerDefsVariantsLists();

            //ExportnamelessList();

            //Exclude company from city if no jobs assigned by game
            foreach (City city in CitiesList)
            {
                city.ExcludeCompany();
                if (VisitedCities.Exists(x => x.Name == city.CityName))
                    city.Visited = true;
            }

            //LoadAdditionalCargo(); //REWRITE

            CompaniesList = CompaniesList.Distinct().ToList();//Delete duplicates
            CargoesList = CargoesList.Distinct().ToList(); //Delete duplicates

            CompanyTruckComparer companyTruckComparer = new CompanyTruckComparer();

            CompanyTruckList = CompanyTruckList.Distinct(companyTruckComparer).ToList(); //Delete duplicates
            HeavyCargoList = HeavyCargoList.Distinct().ToList(); //Delete duplicates

            //Set country to city
            foreach (City tempcity in CitiesList)
            {
                string country = CountryDictionary.GetCountry(tempcity.CityName);
                tempcity.Country = country;

                if ((country != null) && (country != ""))
                {
                    CountriesList.Add(country);
                }
            }

            CountriesList = CountriesList.Distinct().ToList();

            CountryDictionary.SaveDictionaryFile(); //Save country-city list to file

            //Filter garages
            foreach (City tempcity in from x in CitiesList where !x.Disabled select x)
            {
                Garages tmpgrg = GaragesList.Find(x => x.GarageName == tempcity.CityName);
                if (tmpgrg != null)
                    tmpgrg.IgnoreStatus = false;
            }

            GaragesList = GaragesList.Distinct().OrderBy(x => x.GarageName).ToList();
            
            PrepareDBdata();

            //GetCompaniesCargoInOut();
            workerLoadSaveFile.ReportProgress(90);

            GetAllDistancesFromDB();

            workerLoadSaveFile.ReportProgress(100);
        }

        private void CheckSaveInfoData()
        {
            MainSaveFileInfoData.ProcessData(tempInfoFileInMemory);            

            if (MainSaveFileInfoData.Version > 0)
            {
                if (MainSaveFileInfoData.Version > SupportedSavefileVersionETS2[1])
                {
                    string dialogCaption = "", dialogText = "";
                    string[] returnValues = HelpTranslateDialog("UnsupportedVersion");

                    dialogText = Regex.Unescape(String.Format(returnValues[1], MainSaveFileInfoData.Version));

                    DialogResult DR = UpdateStatusBarMessage.ShowMessageBox(this, dialogText, returnValues[0], MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (DR == DialogResult.No)
                    {
                        InfoDepContinue = false;
                        return;
                    }
                }

                if (MainSaveFileInfoData.Version < SupportedSavefileVersionETS2[0])
                {
                    string dialogCaption = "", dialogText = "";
                    string[] returnValues = HelpTranslateDialog("NoBackwardCompatibility");

                    dialogText = Regex.Unescape(String.Format(returnValues[1], MainSaveFileInfoData.Version));

                    DialogResult DR = UpdateStatusBarMessage.ShowMessageBox(this, dialogText, returnValues[0], MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    if (DR == DialogResult.OK)
                    {
                        InfoDepContinue = false;
                        return;
                    }
                }
            }
            else if (MainSaveFileInfoData.Version == 0)
            {
                DialogResult result = UpdateStatusBarMessage.ShowMessageBox(this, "Savefile version was not recognised." + Environment.NewLine +"Do you want to continue?", "Version not recognised", MessageBoxButtons.YesNo);
                
                if (result == DialogResult.No)
                {
                    InfoDepContinue = false;
                    return;
                }
            }

            string sql = "UPDATE [DatabaseDetails] SET SaveVersion = " + MainSaveFileInfoData.Version + " WHERE ID_DBline = 1;";
            UpdateDatabase(sql);

            GetDataFromDatabase("Dependencies");

            //Check dependencies
            if (DBDependencies.Count == 0)
            {
                InsertDataIntoDatabase("Dependencies");
                InfoDepContinue = true;
            }
            else
            {
                List<string> tmpSFdep = MainSaveFileInfoData.Dependencies.Where(x => x.RawDepType != "rdlc").Select(x => x.Raw.Value).ToList();

                List<string> dbdep = DBDependencies.Except(tmpSFdep).ToList();
                List<string> sfdep = tmpSFdep.Except(DBDependencies).ToList();

                if (dbdep.Count > 0 || sfdep.Count > 0)
                {
                    string dbdepstr = "", sfdepstr = "";

                    if (dbdep.Count > 0)
                    {
                        dbdepstr += "\r\nDependencies only in Database (" + dbdep.Count.ToString() +  ") will be Deleted:\r\n";
                        int i = 0;
                        foreach (string temp in dbdep)
                        {
                            i++;
                            dbdepstr += i.ToString() + ") " + temp + "\r\n";
                        }
                    }

                    if (sfdep.Count > 0)
                    {
                        sfdepstr += "\r\nDependencies only in Save file (" + sfdep.Count.ToString() + ") will be Added:\r\n";
                        int i = 0;
                        foreach (string temp in sfdep)
                        {
                            i++;
                            sfdepstr += i.ToString() + ") " + temp + "\r\n"; ;
                        }
                    }

                    DialogResult r = UpdateStatusBarMessage.ShowMessageBox(this,
                        "Save file and Database has different Dependencies due to installed\\deleted mods\\dlc's." + Environment.NewLine +
                        "This may result in wrong path and cargo data." + Environment.NewLine + Environment.NewLine +
                        "Do you want to Proceed and Update Dependencies?" + Environment.NewLine +
                        dbdepstr + Environment.NewLine + sfdepstr, "Dependencies conflict",
                        MessageBoxButtons.YesNo);

                    if (r == DialogResult.Yes)
                    {
                        //Update Dependencies
                        InsertDataIntoDatabase("Dependencies");
                        InfoDepContinue = true;
                    }
                    else
                    {
                        //Stop opening save
                        InfoDepContinue = false;
                    }
                }
                else
                {
                    InfoDepContinue = true;
                }
            }

            if (!InfoDepContinue)
                return;

            LoadCachedExternalCargoData("def");

            if (MainSaveFileInfoData.Dependencies.Count > 0)
                foreach(Dependency tDepend in MainSaveFileInfoData.Dependencies)
                {
                    LoadCachedExternalCargoData(tDepend.DepLoadID);
                }

            if (MainSaveFileInfoData.Version == 0)
                UpdateStatusBarMessage.ShowStatusMessage(SMStatus.Error, "error_save_version_not_detected");
        }

        public string GetCustomSaveFilename(string _tempSaveFilePath)
        {
            string chunkOfline;

            string tempSiiInfoPath = _tempSaveFilePath + @"\info.sii";
            string[] tempFile = null;

            if (!File.Exists(tempSiiInfoPath))
            {
                IO_Utilities.LogWriter("File does not exist in " + tempSiiInfoPath);
                UpdateStatusBarMessage.ShowStatusMessage(SMStatus.Error, "error_could_not_find_file");
            }
            else
            {   
                FileDecoded = false;
                try
                {
                    int decodeAttempt = 0;
                    while (decodeAttempt < 5)
                    {
                        tempFile = NewDecodeFile(tempSiiInfoPath, false);

                        if (FileDecoded)
                        {
                            break;
                        }

                        decodeAttempt++;
                    }

                    if (decodeAttempt == 5)
                    {
                        IO_Utilities.LogWriter("Could not decrypt after 5 attempts");
                        UpdateStatusBarMessage.ShowStatusMessage(SMStatus.Error, "error_could_not_decode_file");
                    }
                }
                catch
                {
                    IO_Utilities.LogWriter("Could not read: " + tempSiiInfoPath);
                }

                if ((tempFile == null) || (tempFile[0] != "SiiNunit"))
                {
                    IO_Utilities.LogWriter("Wrongly decoded Info file or wrong file format");
                    UpdateStatusBarMessage.ShowStatusMessage(SMStatus.Error, "error_file_not_decoded");
                }
                else if (tempFile != null)
                {
                    for (int line = 0; line < tempFile.Length; line++)
                    {
                        if (tempFile[line].StartsWith(" name:"))
                        {
                            chunkOfline = tempFile[line];
                            string CustomName = chunkOfline.Split(new char[] { ' ' },3)[2];

                            if (CustomName.StartsWith("\""))
                            {
                                CustomName = CustomName.Substring(1, CustomName.Length - 2);
                            }

                            return CustomName;
                        }
                    }
                }
            }
            //////
            return "<!>Error<!>";
        }

        //Remove broken color sets
        
        private void PrepareUserColors()
        {
            if (MainSaveFileInfoData.Version < 49)
                return;

            int setcount = SiiNunitData.Economy.user_colors.Count() / 4;

            //iterate through sets
            for (int i = 0; i < setcount; i++)
            {
                if (SiiNunitData.Economy.user_colors[4 * i].color.A == 0)
                {
                    //clear color set
                    for (int j = 1; j < 4; j++)
                    {
                        SiiNunitData.Economy.user_colors[4 * i + j] = new Save.DataFormat.SCS_Color(0, 0, 0, 0);
                    }
                    continue;
                }
            }

            RemoveUserColorUnused4slot();
        }
        
        //
        private void PrepareCitiesInitial()
        {
            string[] chunks;

            foreach(string company in SiiNunitData.Economy.companies)
            {
                chunks = company.Split(new char[] { '.' });

                string cityname = chunks[3], companyname = chunks[2];

                if (cityname == null)
                    continue;                

                //Add City to List from companies list
                if (CitiesList.Where(x => x.CityName == cityname).Count() == 0)
                {
                    CitiesList.Add(new City(cityname));
                }

                CompaniesList.Add(companyname); //add company to list

                //Add Company to City from companies list                            
                foreach (City tempcity in CitiesList.FindAll(x => x.CityName == cityname))
                {
                    tempcity.AddCompany(companyname);

                    Save.Items.Company tempcompany = (Save.Items.Company)SiiNunitData.SiiNitems[company];

                    tempcity.UpdateCompanyJobOffersCount(companyname, tempcompany.job_offer.Count);

                    tempcity.UpdateCompanyCargoSeeds(companyname, tempcompany.cargo_offer_seeds.ToArray());
                }
            }
        }

        private void PrepareGaragesInitial()
        {
            foreach (string garage in SiiNunitData.Economy.garages)
            {
                string garageName = garage.Split(new char[] { '.' })[1];

                Save.Items.Garage tmpSiiNGarage = SiiNunitData.SiiNitems[garage];

                GaragesList.Add(new Garages(garageName, tmpSiiNGarage.status));

                Garages tempGarage = GaragesList.Find(x => x.GarageName == garageName);

                tempGarage.Vehicles.AddRange(tmpSiiNGarage.vehicles);
                tempGarage.Drivers.AddRange(tmpSiiNGarage.drivers);
                tempGarage.Trailers.AddRange(tmpSiiNGarage.trailers);
            }
        }

        private void PrepareVisitedCitiesInitial()
        {
            int cityid = 0;
            foreach (string city in SiiNunitData.Economy.visited_cities)
            {
                VisitedCities.Add(new VisitedCity(city, SiiNunitData.Economy.visited_cities_count[cityid], true));
                cityid++;
            }
        }

        private void PreparePlayerDictionariesInitial()
        {
            foreach (string trck in SiiNunitData.Player.trucks)
            {
                UserTruckDictionary.Add(trck, new UserCompanyTruckData());

                UserTruckDictionary[trck].TruckMainData = SiiNunitData.SiiNitems[trck];
            }

            //
            foreach (string trlr in SiiNunitData.Player.trailers)
            {
                UserTrailerDictionary.Add(trlr, new UserCompanyTrailerData());
                UserTrailerDictionary[trlr].TrailerMainData = SiiNunitData.SiiNitems[trlr];
            }

            //
            foreach (string trlrDef in SiiNunitData.Player.trailer_defs)
            {
                UserTrailerDefDictionary.Add(trlrDef, SiiNunitData.SiiNitems[trlrDef]);
            }

            //
            if (SiiNunitData.Player_Job != null)
            {
                string jobtrck = SiiNunitData.Player_Job.company_truck;
                if (jobtrck != "null")
                {
                    UserTruckDictionary.Add(jobtrck, new UserCompanyTruckData());
                    UserTruckDictionary[jobtrck].TruckMainData = SiiNunitData.SiiNitems[jobtrck];
                    UserTruckDictionary[jobtrck].Users = false;
                }

                string jobtrlr = SiiNunitData.Player_Job.company_trailer;
                if (jobtrlr != "null")
                {
                    UserTrailerDictionary.Add(jobtrlr, new UserCompanyTrailerData());
                    UserTrailerDictionary[jobtrlr].TrailerMainData = SiiNunitData.SiiNitems[jobtrlr];
                    UserTrailerDictionary[jobtrlr].Users = false;
                }
            }

            //
            foreach (string drvr in SiiNunitData.Player.drivers)
            {
                UserDriverDictionary.Add(drvr, new UserCompanyDriverData());
            }
        }

        private void PrepareGPSInitial()
        {
            //GPS
            //Online
            foreach(string entry in SiiNunitData.Economy.stored_online_gps_behind_waypoints)
            {
                GPSbehindOnline.Add(entry, new List<string>());
            }

            foreach (string entry in SiiNunitData.Economy.stored_online_gps_ahead_waypoints)
            {
                GPSaheadOnline.Add(entry, new List<string>());
            }

            //Offline
            //Normal
            foreach (string entry in SiiNunitData.Economy.stored_gps_behind_waypoints)
            {
                GPSbehind.Add(entry, new List<string>());
            }

            foreach (string entry in SiiNunitData.Economy.stored_gps_ahead_waypoints)
            {
                GPSahead.Add(entry, new List<string>());
            }
            //Avoid
            foreach (string entry in SiiNunitData.Economy.stored_gps_avoid_waypoints)
            {
                GPSAvoid.Add(entry, new List<string>());
            }
        }

        private void PrepareVisitedCitiesWrite()
        {
            foreach (City city in CitiesList)
            {
                VisitedCity temp = VisitedCities.Find(x => x.Name == city.CityName);

                if (temp != null)
                {
                    if (!city.Visited)
                        VisitedCities[VisitedCities.IndexOf(temp)].VisitCount = 0;
                }
                else
                {
                    if (city.Visited)
                        VisitedCities.Add(new VisitedCity(city.CityName, 1, true));
                    else
                        VisitedCities.Add(new VisitedCity(city.CityName, 0, false));
                }
            }

            foreach (VisitedCity vc in VisitedCities)
            {
                if (vc.Visited)
                {
                    if (!SiiNunitData.Economy.visited_cities.Contains(vc.Name))
                    {
                        SiiNunitData.Economy.visited_cities.Add(vc.Name);
                        SiiNunitData.Economy.visited_cities_count.Add(vc.VisitCount);
                    }
                }
                else
                {
                    if (SiiNunitData.Economy.visited_cities.Contains(vc.Name))
                    {
                        int idx = SiiNunitData.Economy.visited_cities.IndexOf(vc.Name);

                        SiiNunitData.Economy.visited_cities.RemoveAt(idx);
                        SiiNunitData.Economy.visited_cities_count.RemoveAt(idx);
                    }
                }
            }

        }

        private void PrepareCargoTrailerDefsVariantsLists()
        {
            foreach (string company in SiiNunitData.Economy.companies)
            {
                Save.Items.Company tmpCompany = SiiNunitData.SiiNitems[company];

                //
                int cargotype = 0, units_count = 0;
                string cargo = "", trailervariant = "", trailerdefinition = "", company_truck = "";

                foreach (string job_offer in tmpCompany.job_offer)
                {
                    Save.Items.Job_offer_Data tmpJob_offer_Data = SiiNunitData.SiiNitems[job_offer];

                    if (tmpJob_offer_Data.cargo == "null")
                        continue;

                    //===
                    company_truck = tmpJob_offer_Data.company_truck;

                    cargo = tmpJob_offer_Data.cargo.Split(new char[] { '.' })[1];
                    trailervariant = tmpJob_offer_Data.trailer_variant;
                    trailerdefinition = tmpJob_offer_Data.trailer_definition;

                    units_count = tmpJob_offer_Data.units_count;

                    //===

                    cargotype = 0;

                    //===

                    if (company_truck.Contains("\"heavy"))
                    {
                        cargotype = 1;
                    }
                    else if (company_truck.Contains("\"double"))
                    {
                        cargotype = 2;
                    }

                    //===

                    CompanyTruckList.Add(new CompanyTruck(company_truck, cargotype));

                    //===

                    if (!TrailerVariants.Contains(trailervariant))
                        TrailerVariants.Add(trailervariant);

                    //===


                    Cargo tempCargo = CargoesList.Find(x => x.CargoName == cargo);

                    if (tempCargo == null)
                    {
                        CargoesList.Add(new Cargo(cargo, cargotype, trailerdefinition, units_count));
                    }
                    else
                    {
                        if (!tempCargo.TrailerDefList.Exists(x => x.DefName == trailerdefinition && x.CargoType == cargotype && x.UnitsCount == units_count))
                        {
                            CargoesList.Find(x => x.CargoName == cargo).TrailerDefList.Add(new TrailerDefinition(trailerdefinition, cargotype, units_count));
                        }
                    }

                    if (!TrailerDefinitionVariants.ContainsKey(trailerdefinition))
                    {
                        List<string> tmp = new List<string> { trailervariant };
                        TrailerDefinitionVariants.Add(trailerdefinition, tmp);
                    }
                    else
                    {
                        if (!TrailerDefinitionVariants[trailerdefinition].Contains(trailervariant))
                        {
                            TrailerDefinitionVariants[trailerdefinition].Add(trailervariant);
                        }
                    }
                    //===
                }
            }
        }

        private void PrepareDBdata()
        {
            //GetDataFrom Database

            //GetDataFromDatabase("CargoesTable");
            GetDataFromDatabase("CitysTable");
            GetDataFromDatabase("CompaniesTable");
            GetDataFromDatabase("TrucksTable");

            //Compare Data to Database

            //=== Cargo

            if (CargoesListDB.Count() > 0)
            {
                CargoComparer CCaad = new CargoComparer();
                CargoesListDiff = CargoesList.Except(CargoesListDB, CCaad).ToList();
                Predicate<Cargo> tempCargoPred = null;

                foreach (Cargo tempCargo in CargoesListDiff)
                {
                    tempCargoPred = x => x.CargoName == tempCargo.CargoName;

                    int listDBindex = CargoesListDB.FindIndex(tempCargoPred);
                    int listDIFFindex = CargoesListDiff.FindIndex(tempCargoPred);

                    if (listDBindex != -1)
                    {
                        foreach (TrailerDefinition cdef in tempCargo.TrailerDefList)
                        {
                            Dictionary<string, int> tempdef = new Dictionary<string, int>();

                            CargoesListDB[listDBindex].TrailerDefList.Add(cdef);
                        }
                        CargoesListDB[listDBindex].TrailerDefList = CargoesListDB[listDBindex].TrailerDefList.Distinct().ToList();

                        CargoesListDiff[listDIFFindex].TrailerDefList = CargoesListDB[listDBindex].TrailerDefList;
                    }
                    else
                    {
                        CargoesListDB.Add(new Cargo(tempCargo.CargoName, tempCargo.TrailerDefList));
                    }
                }
            }
            else
            {
                CargoesListDB = CargoesList;
                CargoesListDiff = CargoesList;
            }

            //=== Cities

            if (CitiesListDB.Count() > 0)
            {
                foreach (string tempCity in CitiesListDB)
                {
                    if (CitiesList.Where(x => x.CityName == tempCity) == null)
                        CitiesListDiff.Add(tempCity);
                }

                if (CitiesListDiff != null)
                    foreach (string tempCity in CitiesListDiff)
                    {
                        CitiesListDB.Add(tempCity);
                    }
            }
            else
            {
                foreach (City tempCity in CitiesList)
                {
                    CitiesListDB.Add(tempCity.CityName);
                }

                CitiesListDiff = CitiesListDB;
            }

            //=== Companies

            if (CompaniesListDB.Count() > 0)
            {
                foreach (string tempCompany in CompaniesListDB)
                {
                    if (CompaniesList.Where(x => x == tempCompany) == null)
                        CompaniesListDiff.Add(tempCompany);
                }

                if (CompaniesListDiff != null)
                    foreach (string tempCompany in CompaniesListDiff)
                    {
                        CompaniesListDB.Add(tempCompany);
                    }
            }
            else
            {
                foreach (string tempCompany in CompaniesList)
                {
                    CompaniesListDB.Add(tempCompany);
                }

                CompaniesListDiff = CompaniesListDB;
            }

            //=== Trucks

            if (CompanyTruckListDB.Count() > 0)
            {
                CompanyTruckListDiff = CompanyTruckList.Except(CompanyTruckListDB, new CompanyTruckComparer()).ToList();

                foreach (CompanyTruck tempCompany in CompanyTruckListDiff)
                {
                    CompanyTruckListDB.Add(tempCompany);
                }
            }
            else
            {
                CompanyTruckListDB = CompanyTruckList;
                CompanyTruckListDiff = CompanyTruckList;
            }

            //

            SaveCompaniesLng();
            SaveCitiesLng();
            SaveCargoLng();

            //save new data to database

            //InsertDataIntoDatabase("CargoesTable");
            InsertDataIntoDatabase("CitysTable");
            InsertDataIntoDatabase("CompaniesTable");
            InsertDataIntoDatabase("TrucksTable");

            //end save data

            AddDistances_DataTableToDB_Bulk(DistancesTable);
        }

        //Apply new garage size and Copy extra items to temp Lists
        private void PrepareGarages()
        {
            List<string> extraTrailers = new List<string>();

            foreach (Garages tempGarage in GaragesList)
            {
                int capacity = 0;

                if (tempGarage.GarageStatus == 2)
                {
                    capacity = 3;
                }
                else if (tempGarage.GarageStatus == 3)
                {
                    capacity = 5;
                }
                else if (tempGarage.GarageStatus == 6)
                {
                    capacity = 1;
                }

                if (capacity == 0)
                {
                    //Move
                    extraVehicles.AddRange(tempGarage.Vehicles);
                    extraDrivers.AddRange(tempGarage.Drivers);
                    extraTrailers.AddRange(tempGarage.Trailers);
                    //Delete                    
                    tempGarage.Vehicles.Clear();
                    tempGarage.Drivers.Clear();
                    tempGarage.Trailers.Clear();
                }
                else
                {
                    int cur = tempGarage.Vehicles.Count;

                    if (capacity < cur)
                    {
                        extraVehicles.AddRange(tempGarage.Vehicles.GetRange(capacity, cur - capacity));
                        extraDrivers.AddRange(tempGarage.Drivers.GetRange(capacity, cur - capacity));

                        tempGarage.Vehicles.RemoveRange(capacity, cur - capacity);
                        tempGarage.Drivers.RemoveRange(capacity, cur - capacity);
                    }
                    else if (capacity > cur)
                    {
                        string rstr = null;
                        tempGarage.Vehicles.AddRange(Enumerable.Repeat(rstr, capacity - cur));
                        tempGarage.Drivers.AddRange(Enumerable.Repeat(rstr, capacity - cur));
                    }
                }
            }

            if (extraTrailers.Count > 0)
            {
                GaragesList[GaragesList.FindIndex(x => x.GarageName == SiiNunitData.Player.hq_city)].Trailers.AddRange(extraTrailers);
                extraTrailers.Clear();
            }

            int iV = extraDrivers.Count();

            for (int i = iV - 1; i >= 0; i--)
            {
                if(extraVehicles[i] == extraDrivers[i])
                {
                    extraVehicles.RemoveAt(i);
                    extraDrivers.RemoveAt(i);
                }
            }

            if (extraDrivers.Count() > 0)
            {
                if (extraDrivers.Contains(SiiNunitData.Economy.driver_pool[0]))
                {
                    Garages tmpG = new Garages(SiiNunitData.Player.hq_city);

                    int hqIdx = GaragesList.IndexOf(tmpG);
                    int sIdx = 0;

                    int DrvIdx, VhcIdx;

                    while (true)
                    {
                        DrvIdx = GaragesList[hqIdx].Drivers.FindIndex(sIdx, x => x == null);
                        VhcIdx = GaragesList[hqIdx].Vehicles.FindIndex(sIdx, x => x == null);
                        
                        if (DrvIdx > -1 && VhcIdx > -1)
                        {
                            if (DrvIdx == VhcIdx)
                            {
                                break;
                            }
                            else
                            {
                                if (DrvIdx > VhcIdx)
                                    sIdx = DrvIdx;
                                else
                                    sIdx = VhcIdx;
                            }
                        }
                        else
                        {
                            DrvIdx = 0;
                            break;
                        }
                    }

                    extraDrivers.Add(GaragesList[hqIdx].Drivers[DrvIdx]);
                    extraVehicles.Add(GaragesList[hqIdx].Vehicles[DrvIdx]);

                    int tmpIdx = extraDrivers.IndexOf(SiiNunitData.Economy.driver_pool[0]);

                    GaragesList[hqIdx].Drivers[DrvIdx] = extraDrivers[tmpIdx];
                    GaragesList[hqIdx].Vehicles[DrvIdx] = extraVehicles[tmpIdx];

                    extraDrivers.RemoveAt(tmpIdx);
                    extraVehicles.RemoveAt(tmpIdx);
                }
            }
        }

        private void PrepareGaragesWrite()
        {
            foreach(string grg in SiiNunitData.Economy.garages)
            {
                Save.Items.Garage siiGarage = SiiNunitData.SiiNitems[grg];
                Garages prgrGarage = GaragesList.Find(x => x.GarageName == grg.Split(new char[] { '.' })[1]);

                siiGarage.drivers = prgrGarage.Drivers;
                siiGarage.vehicles = prgrGarage.Vehicles;
                siiGarage.trailers = prgrGarage.Trailers;
                siiGarage.status = prgrGarage.GarageStatus;
            }
        }

        private void PrepareCompaniesJobWrite()
        {
            foreach (KeyValuePair<string, List<JobAdded>> cmp in AddedJobsDictionary)
            {
                Save.Items.Company siiCompany = SiiNunitData.SiiNitems[cmp.Key];

                for (int i = 0; i < cmp.Value.Count; i++)
                {
                    JobAdded job = cmp.Value.ElementAt(i);

                    string jobId = siiCompany.job_offer[i];

                    Save.Items.Job_offer_Data siiJob = SiiNunitData.SiiNitems[jobId];

                    siiJob.target = "\"" + job.DestinationCompany + "." + job.DestinationCity + "\"";
                    siiJob.expiration_time = job.ExpirationTime;
                    siiJob.urgency = job.Urgency;
                    siiJob.shortest_distance_km = job.Distance;
                    siiJob.ferry_time = job.Ferrytime;
                    siiJob.ferry_price = job.Ferryprice;
                    siiJob.cargo = "cargo." + job.Cargo;
                    siiJob.company_truck = job.CompanyTruck;
                    siiJob.trailer_variant = job.TrailerVariant;
                    siiJob.trailer_definition = job.TrailerDefinition;
                    siiJob.units_count = job.UnitsCount;
                    //siiJob.fill_ratio = 1;
                }
            }
        }

        //Rearrange extra User Drivers to glogal Driver pool
        private void PrepareDriversTrucksWrite()
        {
            extraDrivers.RemoveAll(x => x == null);

            foreach (string tmp in extraDrivers)
            {
                if (tmp != null)
                {
                    SiiNunitData.Economy.driver_pool.Add(tmp);
                    UserDriverDictionary.Remove(tmp);
                }
            }

            extraVehicles.RemoveAll(x => x == null);
        }

        //Sort events by time
        private void PrepareEvents()
        {
            Dictionary<string, Economy_event> timeList = new Dictionary<string, Economy_event>();

            foreach (string ecEventLink in SiiNunitData.Economy_event_Queue.data)
            {
                Economy_event ecEvent = ((Economy_event)SiiNunitData.SiiNitems[ecEventLink]);
                string cmpLink = ecEvent.unit_link;

                if (AddedJobsDictionary.ContainsKey(cmpLink))
                {
                    if (ecEvent.param < AddedJobsDictionary[cmpLink].Count)
                    {
                        ecEvent.time = AddedJobsDictionary[cmpLink].ElementAt(ecEvent.param).ExpirationTime;
                    }
                }

                timeList.Add(ecEventLink, ecEvent);
            }

            //Sort by time
            var sortedDict = from entry in timeList orderby entry.Value.time ascending select entry;

            List<string> newQueue = new List<string>();
            newQueue.AddRange(sortedDict.Select(x => x.Key));

            SiiNunitData.Economy_event_Queue.data = newQueue;
            
        }

        //Create DB
        private void CreateDatabase(string fileName)
        {
            string connectionString;

            if(!Directory.Exists("dbs"))
            {
                Directory.CreateDirectory("dbs");
            }

            if (!File.Exists(fileName))
            {
                //Create
                UpdateStatusBarMessage.ShowStatusMessage(SMStatus.Error, "message_database_missing_creating_db");

                connectionString = string.Format("DataSource ='{0}';", fileName);

                SqlCeEngine Engine = new SqlCeEngine(connectionString);

                Engine.CreateDatabase();

                UpdateStatusBarMessage.ShowStatusMessage(SMStatus.Info, "message_database_created");

                CreateDatabaseStructure();
            }
            else
            {
                //Update
                UpdateDatabaseVersion();
            }
        }
        //Create DB structure
        private void CreateDatabaseStructure()
        {
            UpdateStatusBarMessage.ShowStatusMessage(SMStatus.Error, "message_database_missing_creating_db_structure");

            string sql = "", DBVersion = "";

            //
            DBVersion = "0.2.6.6";

            string[] splitDBver = DBVersion.Split(new char[] { '.' });

            sql += "CREATE TABLE DatabaseDetails (ID_DBline INT IDENTITY(1,1) PRIMARY KEY, GameName NVARCHAR(8) NOT NULL, SaveVersion INT NOT NULL, ProfileName NVARCHAR(128) NOT NULL, " +
                "V1 numeric(4,0) NOT NULL, V2 numeric(4,0) NOT NULL, V3 numeric(4,0) NOT NULL, V4 numeric(4,0) NOT NULL, ReadableName NVARCHAR(30) NOT NULL);";
            sql += "INSERT INTO [DatabaseDetails] (GameName, SaveVersion, ProfileName, V1, V2, V3, V4, ReadableName) VALUES ('" + GameType + "', 0, '" + Globals.SelectedProfile + "','" +
                splitDBver[0] + "','" + splitDBver[1] + "','" + splitDBver[2] + "','" + splitDBver[3] + "','" + Utilities.TextUtilities.FromHexToString(Globals.SelectedProfile) + "');";

            sql += "CREATE TABLE Dependencies (ID_dep INT IDENTITY(1,1) PRIMARY KEY, Dependency NVARCHAR(256) NOT NULL);";

            sql += "CREATE TABLE CitysTable (ID_city INT IDENTITY(1,1) PRIMARY KEY, CityName NVARCHAR(32) NOT NULL);";

            sql += "CREATE TABLE CompaniesTable (ID_company INT IDENTITY(1,1) PRIMARY KEY, CompanyName NVARCHAR(32) NOT NULL);";

            sql += "CREATE TABLE CargoesTable (ID_cargo INT IDENTITY(1,1) PRIMARY KEY, CargoName NVARCHAR(32) NOT NULL);";

            sql += "CREATE TABLE TrailerDefinitionTable (ID_trailerD INT IDENTITY(1,1) PRIMARY KEY, TrailerDefinitionName NVARCHAR(64) NOT NULL);";
            sql += "CREATE UNIQUE INDEX [Idx_Uniq] ON [TrailerDefinitionTable] ([TrailerDefinitionName]);";

            sql += "CREATE TABLE CargoesToTrailerDefinitionTable (ID_trailerCtD INT IDENTITY(1,1) PRIMARY KEY, CargoID INT NOT NULL, TrailerDefinitionID INT NOT NULL, CargoType INT NOT NULL);";
            sql += "ALTER TABLE CargoesToTrailerDefinitionTable ADD FOREIGN KEY(CargoID) REFERENCES CargoesTable(ID_cargo);";
            sql += "ALTER TABLE CargoesToTrailerDefinitionTable ADD FOREIGN KEY(TrailerDefinitionID) REFERENCES TrailerDefinitionTable(ID_trailerD);";
            sql += "CREATE UNIQUE INDEX [Idx_Uniq] ON [CargoesToTrailerDefinitionTable] ([CargoID],[TrailerDefinitionID]);";

            sql += "CREATE TABLE TrailerVariantTable (ID_trailerV INT IDENTITY(1,1) PRIMARY KEY, TrailerVariantName NVARCHAR(64) NOT NULL);";
            sql += "CREATE UNIQUE INDEX [Idx_Uniq] ON [TrailerVariantTable] ([TrailerVariantName]);";

            sql += "CREATE TABLE TrailerDefinitionToTrailerVariantTable (ID_trailerDtV INT IDENTITY(1,1) PRIMARY KEY, TrailerDefinitionID INT NOT NULL, TrailerVariantID INT NOT NULL);";
            sql += "ALTER TABLE TrailerDefinitionToTrailerVariantTable ADD FOREIGN KEY(TrailerDefinitionID) REFERENCES TrailerDefinitionTable(ID_trailerD);";
            sql += "ALTER TABLE TrailerDefinitionToTrailerVariantTable ADD FOREIGN KEY(TrailerVariantID) REFERENCES TrailerVariantTable(ID_trailerV);";
            sql += "CREATE UNIQUE INDEX [Idx_Uniq] ON [TrailerDefinitionToTrailerVariantTable] ([TrailerDefinitionID],[TrailerVariantID]);";

            sql += "CREATE TABLE TrucksTable (ID_truck INT IDENTITY(1,1) PRIMARY KEY, TruckName NVARCHAR(64) NOT NULL, TruckType TINYINT NOT NULL);";

            sql += "CREATE TABLE CompaniesInCitysTable (ID_CmpnToCt INT IDENTITY(1,1) PRIMARY KEY, CityID INT NOT NULL, CompanyID INT NOT NULL);";
            sql += "ALTER TABLE CompaniesInCitysTable ADD FOREIGN KEY(CityID) REFERENCES CitysTable(ID_city) ON DELETE CASCADE;";
            sql += "ALTER TABLE CompaniesInCitysTable ADD FOREIGN KEY(CompanyID) REFERENCES CompaniesTable(ID_company) ON DELETE CASCADE;";

            sql += "CREATE TABLE CompaniesCargoTable (ID_CmpnCrg INT IDENTITY(1,1) PRIMARY KEY, CompanyID INT NOT NULL, CargoID INT NOT NULL);";
            sql += "ALTER TABLE CompaniesCargoTable ADD FOREIGN KEY(CompanyID) REFERENCES CompaniesTable(ID_company) ON DELETE CASCADE;";
            sql += "ALTER TABLE CompaniesCargoTable ADD FOREIGN KEY(CargoID) REFERENCES CargoesTable(ID_cargo) ON DELETE CASCADE;";

            sql += "CREATE TABLE DistancesTable (ID_Distance INT IDENTITY(1,1) PRIMARY KEY, SourceCityID INT NOT NULL, SourceCompanyID INT NOT NULL, " +
                "DestinationCityID INT NOT NULL, DestinationCompanyID INT NOT NULL, Distance INT NOT NULL, FerryTime INT NOT NULL, FerryPrice INT NOT NULL);";
            sql += "ALTER TABLE DistancesTable ADD FOREIGN KEY(SourceCityID) REFERENCES CitysTable(ID_city);";
            sql += "ALTER TABLE DistancesTable ADD FOREIGN KEY(SourceCompanyID) REFERENCES CompaniesTable(ID_company);";
            sql += "ALTER TABLE DistancesTable ADD FOREIGN KEY(DestinationCityID) REFERENCES CitysTable(ID_city);";
            sql += "ALTER TABLE DistancesTable ADD FOREIGN KEY(DestinationCompanyID) REFERENCES CompaniesTable(ID_company);";
            sql += "CREATE UNIQUE INDEX [Idx_Uniq_path] ON [DistancesTable] ([SourceCityID],[SourceCompanyID],[DestinationCityID],[DestinationCompanyID]);";

            sql += "CREATE TABLE tempBulkDistancesTable (ID_Distance INT IDENTITY(1,1) PRIMARY KEY, SourceCity NVARCHAR(32) NOT NULL, SourceCompany NVARCHAR(32) NOT NULL, " +
                "DestinationCity NVARCHAR(32) NOT NULL, DestinationCompany NVARCHAR(32) NOT NULL, Distance INT NOT NULL, FerryTime INT NOT NULL, FerryPrice INT NOT NULL);";

            sql += "CREATE TABLE tempDistancesTable (ID_Distance INT IDENTITY(1,1) PRIMARY KEY, SourceCityID INT NOT NULL, SourceCompanyID INT NOT NULL, " +
                "DestinationCityID INT NOT NULL, DestinationCompanyID INT NOT NULL, Distance INT NOT NULL, FerryTime INT NOT NULL, FerryPrice INT NOT NULL);";
        

            UpdateDatabase( sql.Split(';') );
        }

        //Update DB version
        private void UpdateDatabaseVersion()
        {
            string DBVersion = "", commandText = "";
            string DBVersionNew = "", sql = "";

            //
            DBVersionNew = "0.2.6.6";

            //Get DB version
            SqlCeDataReader reader = null;

            if (DBconnection.State == ConnectionState.Closed)
                DBconnection.Open();

            bool oldDBversion = false;

            try
            {
                commandText = "SELECT column_name FROM Information_SCHEMA.columns WHERE table_name = 'DatabaseDetails' AND column_name = 'DBVersion';";
                reader = new SqlCeCommand(commandText, DBconnection).ExecuteReader();

                while (reader.Read())
                {
                    if (reader[0].ToString() == "DBVersion")
                        oldDBversion = true;
                }
            }
            catch { }

            if (oldDBversion)
            {
                try
                {
                    commandText = "SELECT DBVersion FROM [DatabaseDetails];";
                    reader = new SqlCeCommand(commandText, DBconnection).ExecuteReader();

                    while (reader.Read())
                        DBVersion = reader["DBVersion"].ToString();

                }
                catch { }
            }
            else
            {
                try
                {
                    commandText = "SELECT V1, V2, V3, V4 FROM [DatabaseDetails];";
                    reader = new SqlCeCommand(commandText, DBconnection).ExecuteReader();

                    while (reader.Read())
                        DBVersion = reader["V1"].ToString() + "." + reader["V2"].ToString() + "." + reader["V3"].ToString() + "." + reader["V4"].ToString();
                }
                catch { }
            }

            DBconnection.Close();
            //

            switch (DBVersion)
            {
                case "0.1.6":
                    {
                        goto label016;
                    }
                case "0.2.0":
                    {
                        goto label020;
                    }
                case "0.2.6":
                    {
                        goto label026;
                    }
                case "0.2.6.2":
                    {
                        goto label0266;
                    }
                case "0.2.6.6":
                    {
                        goto labelskip;
                    }
                default:
                    {
                        return;
                    }
            }

            //0.1.6
            label016:

            sql = "ALTER TABLE [Dependencies] ALTER COLUMN Dependency NVARCHAR(256) NOT NULL;";
            UpdateDatabase(sql);
            //

            //0.2.0
            label020:

            sql = "ALTER TABLE [DatabaseDetails] ALTER COLUMN ProfileName NVARCHAR(128) NOT NULL;";
            UpdateDatabase(sql);

            sql = "UPDATE [DatabaseDetails] SET ProfileName = '" + Globals.SelectedProfile + "' " + "WHERE ID_DBline = 1;";
            UpdateDatabase(sql);
            //

            //0.2.6
            label026:

            UpdateDatabase("DELETE FROM DatabaseDetails WHERE ID_DBline > 1;");
            //

            //0.2.6.6
            label0266:

            sql = "ALTER TABLE DatabaseDetails DROP COLUMN DBVersion;";
            UpdateDatabase(sql);

            sql = "ALTER TABLE DatabaseDetails ADD V1 numeric(4,0) NULL, V2 numeric(4,0) NULL, V3 numeric(4,0) NULL, V4 numeric(4,0) NULL, ReadableName NVARCHAR(30) NULL;";
            UpdateDatabase(sql);

            sql = "UPDATE [DatabaseDetails] SET V1 = '0', V2 = '0', V3 = '0', V4 = '0', ReadableName = '" + Utilities.TextUtilities.FromHexToString(Globals.SelectedProfile) + "' WHERE ID_DBline = 1;";
            UpdateDatabase(sql);

            sql = "ALTER TABLE [DatabaseDetails] ALTER COLUMN [V1] numeric(4,0) NOT NULL;";
            sql += "ALTER TABLE [DatabaseDetails] ALTER COLUMN [V2] numeric(4,0) NOT NULL;";
            sql += "ALTER TABLE [DatabaseDetails] ALTER COLUMN [V3] numeric(4,0) NOT NULL;";
            sql += "ALTER TABLE [DatabaseDetails] ALTER COLUMN [V4] numeric(4,0) NOT NULL;";
            sql += "ALTER TABLE [DatabaseDetails] ALTER COLUMN [ReadableName] NVARCHAR(30) NOT NULL;";

            UpdateDatabase(sql.Split(';'));
            //

            //Set new version
            string[] splitDBver = DBVersionNew.Split(new char[] { '.' });

            sql = "UPDATE [DatabaseDetails] SET V1 = '" + splitDBver[0] + "', V2 = '" + splitDBver[1] + "', V3 = '" + splitDBver[2] + "', V4 = '" + splitDBver[3] + "' WHERE ID_DBline = 1;";
            UpdateDatabase(sql);

            //END
            labelskip:;

        }

        //Help function for DB update
        private void UpdateDatabase(string _sql_string)
        {
            if (DBconnection.State == ConnectionState.Closed)
                DBconnection.Open();
            try
            {
                SqlCeCommand command = DBconnection.CreateCommand();
                command.CommandText = _sql_string;
                command.ExecuteNonQuery();
            }
            catch (SqlCeException sqlexception)
            {
                UpdateStatusBarMessage.ShowStatusMessage(SMStatus.Error, "error_sql_exception");
                MessageBox.Show(sqlexception.Message + "\r\n" + _sql_string, "SQL Exception. Update DB", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                UpdateStatusBarMessage.ShowStatusMessage(SMStatus.Error, "error_exception");
                MessageBox.Show(ex.Message, "Exception.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                DBconnection.Close();
            }
        }

        private void UpdateDatabase(string[] _sql_strings)
        {
            if (DBconnection.State == ConnectionState.Closed)
                DBconnection.Open();

            SqlCeCommand cmd;

            foreach (string sqlline in _sql_strings)
            {
                if (sqlline != "")
                {
                    cmd = new SqlCeCommand(sqlline, DBconnection);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        UpdateStatusBarMessage.ShowStatusMessage(SMStatus.Info, "message_database_created");
                    }
                    catch (SqlCeException sqlexception)
                    {
                        UpdateStatusBarMessage.ShowStatusMessage(SMStatus.Error, "error_sql_exception");
                        MessageBox.Show(sqlexception.Message, "SQL Exception. Create DB", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        UpdateStatusBarMessage.ShowStatusMessage(SMStatus.Error, "error_exception");
                        MessageBox.Show(ex.Message, "Exception.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            DBconnection.Close();
        }
        
        //Load distances from database
        private void GetAllDistancesFromDB()
        {
            try
            {
                RouteList.ClearList(); //Clears existing list in program

                DBconnection.Open();

                string commandText = "SELECT SourceCity.CityName AS SourceCityName, SourceCompany.CompanyName AS SourceCompanyName, DestinationCity.CityName AS DestinationCityName, " +
                    "DestinationCompany.CompanyName AS DestinationCompanyName, Distance, FerryTime, FerryPrice " +
                    "FROM DistancesTable " +
                    "INNER JOIN CompaniesTable AS DestinationCompany ON DistancesTable.DestinationCompanyID = DestinationCompany.ID_company " +
                    "INNER JOIN CitysTable AS DestinationCity ON DistancesTable.DestinationCityID = DestinationCity.ID_city " +
                    "INNER JOIN CompaniesTable AS SourceCompany ON DistancesTable.SourceCompanyID = SourceCompany.ID_company " +
                    "INNER JOIN CitysTable AS SourceCity ON DistancesTable.SourceCityID = SourceCity.ID_city;";

                SqlCeDataReader reader = new SqlCeCommand(commandText, DBconnection).ExecuteReader();

                while (reader.Read())
                {
                    RouteList.AddRoute(reader["SourceCityName"].ToString(), reader["SourceCompanyName"].ToString(), reader["DestinationCityName"].ToString(), reader["DestinationCompanyName"].ToString(),
                        reader["Distance"].ToString(), reader["FerryTime"].ToString(), reader["FerryPrice"].ToString());
                }

                DBconnection.Close();
            }
            catch (SqlCeException sqlexception)
            {
                UpdateStatusBarMessage.ShowStatusMessage(SMStatus.Error, "error_sql_exception");
                MessageBox.Show(sqlexception.Message, "SQL Exception. Load all Distances", MessageBoxButtons.OK, MessageBoxIcon.Error);

                IO_Utilities.LogWriter("Getting Data went wrong");
            }
            catch (Exception ex)
            {
                UpdateStatusBarMessage.ShowStatusMessage(SMStatus.Error, "error_exception");
                MessageBox.Show(ex.Message, "Exception.", MessageBoxButtons.OK, MessageBoxIcon.Error);

                IO_Utilities.LogWriter("Getting Data went wrong");
            }

            IO_Utilities.LogWriter("Loaded " + RouteList.CountItems() + " routes from DataBase");
        }
        //Upload data to DB
        private void AddDistances_DataTableToDB_Bulk(DataTable reader)//(bool keepNulls, DataTable reader)
        {
            using (SqlCeBulkCopy bc = new SqlCeBulkCopy(DBconnection))
            {
                bc.DestinationTableName = "tempBulkDistancesTable";
                bc.WriteToServer(reader);
            }
            DBconnection.Close();
            reader.Clear();

            UpdateDatabase("INSERT INTO tempDistancesTable (SourceCityID, SourceCompanyID, DestinationCityID, DestinationCompanyID, Distance, FerryTime, FerryPrice) " +
                "SELECT DISTINCT SourceCity.ID_city AS SourceCityID, SourceCompany.ID_company AS SourceCompanyID, DestinationCity.ID_city AS DestinationCityID, DestinationCompany.ID_company AS DestinationCompanyID, Distance, FerryTime, FerryPrice " +
                "FROM tempBulkDistancesTable " +
                "INNER JOIN CompaniesTable AS DestinationCompany ON tempBulkDistancesTable.DestinationCompany = DestinationCompany.CompanyName " +
                "INNER JOIN CitysTable AS DestinationCity ON tempBulkDistancesTable.DestinationCity = DestinationCity.CityName " +
                "INNER JOIN CompaniesTable AS SourceCompany ON tempBulkDistancesTable.SourceCompany = SourceCompany.CompanyName " +
                "INNER JOIN CitysTable AS SourceCity ON tempBulkDistancesTable.SourceCity = SourceCity.CityName ");

            string commandText = "SELECT * FROM tempDistancesTable";
            DBconnection.Open();
            SqlCeDataReader sqlreader = new SqlCeCommand(commandText, DBconnection).ExecuteReader();

            int rowsupdate = 0;

            while (sqlreader.Read())
            {
                string updatecommandText = "UPDATE [DistancesTable] SET Distance = '" + sqlreader["Distance"].ToString() + "', " +
                    "FerryTime = '" + sqlreader["FerryTime"].ToString() + "', " +
                    "FerryPrice = '" + sqlreader["FerryPrice"].ToString() + "' " +
                    "WHERE SourceCityID = '" + sqlreader["SourceCityID"].ToString() + "' " +
                    "AND SourceCompanyID = '" + sqlreader["SourceCompanyID"].ToString() + "' " +
                    "AND DestinationCityID = '" + sqlreader["DestinationCityID"].ToString() + "' " +
                    "AND DestinationCompanyID = '" + sqlreader["DestinationCompanyID"].ToString() + "'";

                int rowsupdated = -1;

                try
                {
                    SqlCeCommand command = DBconnection.CreateCommand();
                    command.CommandText = updatecommandText;
                    rowsupdated = command.ExecuteNonQuery();
                }
                catch (SqlCeException sqlexception)
                {
                    MessageBox.Show(sqlexception.Message + " | " + sqlexception.ErrorCode, "SQL Exception.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (rowsupdated == 0)
                {
                    updatecommandText = "INSERT INTO [DistancesTable] (SourceCityID, SourceCompanyID, DestinationCityID, DestinationCompanyID, Distance, FerryTime, FerryPrice) " +
                        "VALUES('" +
                        sqlreader["SourceCityID"].ToString() + "', '" +
                        sqlreader["SourceCompanyID"].ToString() + "', '" +
                        sqlreader["DestinationCityID"].ToString() + "', '" +
                        sqlreader["DestinationCompanyID"].ToString() + "', '" +
                        sqlreader["Distance"].ToString() + "', '" +
                        sqlreader["FerryTime"].ToString() + "', '" +
                        sqlreader["FerryPrice"].ToString() + "');";

                    SqlCeCommand command = DBconnection.CreateCommand();
                    command.CommandText = updatecommandText;
                    command.ExecuteNonQuery();
                }

                rowsupdate++;
            }

            IO_Utilities.LogWriter("Paths checked " + rowsupdate.ToString());
            DBconnection.Close();

            UpdateDatabase("DELETE FROM [tempBulkDistancesTable]");
            UpdateDatabase("DELETE FROM [tempDistancesTable]");

            DistancesTable.Clear();

            SqlCeEngine DBengine = new SqlCeEngine(DBconnection.ConnectionString);
            DBengine.Shrink();
        }
        //Upload to DB
        private void InsertDataIntoDatabase(string _targetTable)
        {
            switch (_targetTable)
            {
                case "Dependencies":
                    {
                        if (MainSaveFileInfoData.Dependencies != null && MainSaveFileInfoData.Dependencies.Count() > 0)
                        {
                            string SQLCommandCMD = "";
                            bool first = true;

                            List<string> gameplayDependencies = MainSaveFileInfoData.Dependencies.Where(x => x.RawDepType != "rdlc").Select(x => x.Raw.Value).ToList();

                            List<string> uniqueDependencies = DBDependencies.Except(gameplayDependencies).ToList();

                            if (uniqueDependencies != null && uniqueDependencies.Count() > 0)
                            {
                                SQLCommandCMD += "DELETE FROM [Dependencies] WHERE Dependency IN (";

                                foreach (string tempitem in uniqueDependencies)
                                {
                                    if (!first)
                                    {
                                        SQLCommandCMD += " , ";
                                    }
                                    else
                                    {
                                        first = false;
                                    }

                                    string sqlstr = tempitem;
                                    int apoIndex = 0;

                                    while (true)
                                    {
                                        apoIndex = sqlstr.IndexOf("'", apoIndex);

                                        if (apoIndex > -1)
                                            sqlstr = sqlstr.Insert(apoIndex, "'");
                                        else
                                            break;

                                        apoIndex = apoIndex + 2;
                                    }

                                    SQLCommandCMD += "'" + sqlstr + "'";
                                }
                                SQLCommandCMD += ")";

                                UpdateDatabase(SQLCommandCMD);
                            }

                            uniqueDependencies = gameplayDependencies.Except(DBDependencies).ToList();

                            if (uniqueDependencies != null && uniqueDependencies.Count() > 0)
                            {
                                SQLCommandCMD = "INSERT INTO [Dependencies] (Dependency) ";
                                first = true;

                                foreach (string tempitem in uniqueDependencies)
                                {
                                    if (!first)
                                    {
                                        SQLCommandCMD += " UNION ALL ";
                                    }
                                    else
                                    {
                                        first = false;
                                    }

                                    string sqlstr = tempitem;
                                    int apoIndex = 0;

                                    while (true)
                                    {
                                        apoIndex = sqlstr.IndexOf("'", apoIndex);

                                        if (apoIndex > -1)
                                            sqlstr = sqlstr.Insert(apoIndex, "'");
                                        else
                                            break;

                                        apoIndex = apoIndex + 2;
                                    }

                                    SQLCommandCMD += "SELECT '" + sqlstr + "'";
                                }
                                UpdateDatabase(SQLCommandCMD);
                            }
                        }

                        break;
                    }

                case "CargoesTable":
                    {
                        int rowsupdated = -1;
                        string SQLCommandCMD = "",  updatecommandText = "";
                        bool first = true;
                        SqlCeCommand command = DBconnection.CreateCommand();

                        /// DEFENITION
                        SQLCommandCMD = "INSERT INTO [TrailerDefinitionTable] (TrailerDefinitionName) ";
                        
                        foreach (KeyValuePair<string, List<string>> tempDefVar in TrailerDefinitionVariants)
                        {
                            if (!first)
                            {
                                SQLCommandCMD += " UNION ALL ";
                            }
                            else
                            {
                                first = false;
                            }

                            SQLCommandCMD += "SELECT '" + tempDefVar.Key + "'";
                        }
                        UpdateDatabase(SQLCommandCMD);
                        ///
                        ///VARIANT
                        SQLCommandCMD = "INSERT INTO [TrailerVariantTable] (TrailerVariantName) ";
                        first = true;

                        foreach (string tempVar in TrailerVariants)
                        {
                            if (!first)
                            {
                                SQLCommandCMD += " UNION ALL ";
                            }
                            else
                            {
                                first = false;
                            }

                            SQLCommandCMD += "SELECT '" + tempVar + "'";
                        }
                        UpdateDatabase(SQLCommandCMD);
                        ///
                        ///// DEFENITION
                        foreach (KeyValuePair<string, List<string>> tempDefVar in TrailerDefinitionVariants)
                        {
                            /*   
                            updatecommandText = "UPDATE [TrailerDefinitionTable] SET TrailerDefinitionName = '" + tempDefVar.Key + "' " +
                           "WHERE TrailerDefinitionName = '" + tempDefVar.Key + "';";

                            rowsupdated = -1;
                            try
                            {
                                command.CommandText = updatecommandText;
                                command.Connection.Open();
                                rowsupdated = command.ExecuteNonQuery();
                            }
                            catch (SqlCeException sqlexception)
                            {
                                MessageBox.Show(sqlexception.Message + " | " + sqlexception.ErrorCode, "SQL Exception. Trailer Def U", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            finally
                            {
                                command.Connection.Close();
                            }

                            if (rowsupdated == 0)
                            {
                                updatecommandText = "INSERT INTO [TrailerDefinitionTable] (TrailerDefinitionName) " +
                                    "VALUES('" + tempDefVar.Key + "');";
                                try
                                {
                                    command.CommandText = updatecommandText;
                                    command.Connection.Open();
                                    command.ExecuteNonQuery();
                                }
                                catch (SqlCeException sqlexception) //when (sqlexception.ErrorCode == )
                                {
                                    MessageBox.Show(sqlexception.Message + " | " + sqlexception.ErrorCode, "SQL Exception. Trailer Def I", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                finally
                                {
                                    command.Connection.Close();
                                }
                            }
                            */
                            SQLCommandCMD = "SELECT ID_trailerD FROM [TrailerDefinitionTable] WHERE TrailerDefinitionName = '" + tempDefVar.Key + "';";
                            command.CommandText = SQLCommandCMD;

                            command.Connection.Open();
                            SqlCeDataReader readerDef = command.ExecuteReader();

                            int DefenitonID = -1;

                            while (readerDef.Read())
                            {
                                DefenitonID = int.Parse(readerDef["ID_trailerD"].ToString());
                            }
                            command.Connection.Close();

                            /////VARIANT
                            foreach (string VariantName in tempDefVar.Value)
                            {
                                /*
                                updatecommandText = "UPDATE [TrailerVariantTable] SET TrailerVariantName = '" + VariantName + "' " +
                                    "WHERE TrailerVariantName = '" + VariantName + "'; ";

                                rowsupdated = -1;
                                try
                                {
                                    command.CommandText = updatecommandText;
                                    command.Connection.Open();
                                    rowsupdated = command.ExecuteNonQuery();
                                }
                                catch (SqlCeException sqlexception)
                                {
                                    MessageBox.Show(sqlexception.Message + " | " + sqlexception.ErrorCode, "SQL Exception. Trailer Var", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                finally
                                {
                                    command.Connection.Close();
                                }

                                if (rowsupdated == 0)
                                {
                                    updatecommandText = "INSERT INTO [TrailerVariantTable] (TrailerVariantName) " +
                                        "VALUES('" + VariantName + "');";

                                    try
                                    {
                                        command.CommandText = updatecommandText;
                                        command.Connection.Open();
                                        command.ExecuteNonQuery();
                                    }
                                    catch (SqlCeException sqlexception) //when (sqlexception.ErrorCode == 2601)
                                    {
                                        MessageBox.Show(sqlexception.Message + " | " + sqlexception.ErrorCode, "SQL Exception. Trailer Var", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    finally
                                    {
                                        command.Connection.Close();
                                    }
                                }
                                */

                                int VariantID = -1;
                                SQLCommandCMD = "SELECT ID_trailerV FROM [TrailerVariantTable] WHERE TrailerVariantName = '" + VariantName + "';";
                                command.CommandText = SQLCommandCMD;

                                command.Connection.Open();
                                SqlCeDataReader readerVar = command.ExecuteReader();


                                while (readerVar.Read())
                                {
                                    VariantID = int.Parse(readerVar["ID_trailerV"].ToString());
                                }
                                command.Connection.Close();

                                if (rowsupdated == 0)
                                {
                                    updatecommandText = "INSERT INTO [TrailerDefinitionToTrailerVariantTable] (TrailerDefinitionID, TrailerVariantID) " +
                                        "VALUES(" + DefenitonID + ", " + VariantID + ");";

                                    try
                                    {
                                        command.CommandText = updatecommandText;
                                        command.Connection.Open();
                                        command.ExecuteNonQuery();
                                    }
                                    catch (SqlCeException sqlexception)
                                    {
                                        MessageBox.Show(sqlexception.Message + " | " + sqlexception.ErrorCode, "SQL Exception.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    finally
                                    {
                                        command.Connection.Close();
                                    }
                                }
                            }
                            /////
                        }
                        /////

                        if (CargoesListDiff != null && CargoesListDiff.Count() > 0)
                        {

                            foreach (Cargo tempitem in CargoesListDiff)
                            {
                                updatecommandText = "UPDATE [CargoesTable] SET CargoName = '" + tempitem.CargoName + "' " +
                                "WHERE CargoName = '" + tempitem.CargoName + "';";

                                try
                                {
                                    command.CommandText = updatecommandText;
                                    command.Connection.Open();
                                    rowsupdated = command.ExecuteNonQuery();
                                }
                                catch (SqlCeException sqlexception)
                                {
                                    MessageBox.Show(sqlexception.Message + " | " + sqlexception.ErrorCode, "SQL Exception. Cargo U", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                finally
                                {
                                    command.Connection.Close();
                                }

                                if (rowsupdated == 0)
                                {
                                    updatecommandText = "INSERT INTO [CargoesTable] (CargoName) " +
                                        "VALUES('" + tempitem.CargoName + "');";

                                    try
                                    {
                                        command.CommandText = updatecommandText;
                                        command.Connection.Open();
                                        command.ExecuteNonQuery();
                                    }
                                    catch (SqlCeException sqlexception)
                                    {
                                        MessageBox.Show(sqlexception.Message + " | " + sqlexception.ErrorCode, "SQL Exception. Cargo I", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    finally
                                    {
                                        command.Connection.Close();
                                    }
                                }

                                SQLCommandCMD = "SELECT ID_cargo FROM [CargoesTable] WHERE CargoName = '" + tempitem.CargoName + "';";
                                command.CommandText = SQLCommandCMD;

                                command.Connection.Open();
                                SqlCeDataReader readerCargo = command.ExecuteReader();

                                int CargoID = -1;

                                while (readerCargo.Read())
                                {
                                    CargoID = int.Parse(readerCargo["ID_cargo"].ToString());
                                }
                                command.Connection.Close();

                                foreach (TrailerDefinition tempDefVar in tempitem.TrailerDefList)
                                {
                                    SQLCommandCMD = "SELECT ID_trailerD FROM [TrailerDefinitionTable] WHERE TrailerDefinitionName = '" + tempDefVar.DefName + "' ";
                                    command.CommandText = SQLCommandCMD;

                                    command.Connection.Open();
                                    SqlCeDataReader readerDef = command.ExecuteReader();

                                    int DefenitonID = -1;

                                    while (readerDef.Read())
                                    {
                                        DefenitonID = int.Parse(readerDef["ID_trailerD"].ToString());
                                    }
                                    command.Connection.Close();

                                    if (rowsupdated == 0)
                                    {
                                        updatecommandText = "INSERT INTO [CargoesToTrailerDefinitionTable] (CargoID, TrailerDefinitionID,  CargoType) " +
                                        "VALUES(" + CargoID + ", " + DefenitonID + ", " + tempDefVar.CargoType + ");";

                                        try
                                        {
                                            command.CommandText = updatecommandText;
                                            command.Connection.Open();
                                            command.ExecuteNonQuery();
                                        }
                                        catch (SqlCeException sqlexception)
                                        {
                                            MessageBox.Show(sqlexception.Message + " | " + sqlexception.ErrorCode + "/r/n" + updatecommandText, "SQL Exception. CtD", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                        finally
                                        {
                                            command.Connection.Close();
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    }

                case "CitysTable":
                    {
                        if (CitiesListDiff != null && CitiesListDiff.Count() > 0)
                        {
                            string SQLCommandCMD = "";
                            SQLCommandCMD += "INSERT INTO [CitysTable] (CityName) ";

                            bool first = true;

                            foreach (string tempcity in CitiesListDiff)
                            {
                                if (!first)
                                {
                                    SQLCommandCMD += " UNION ALL ";
                                }
                                else
                                {
                                    first = false;
                                }

                                SQLCommandCMD += "SELECT '" + tempcity + "'";
                            }
                            UpdateDatabase(SQLCommandCMD);
                        }

                        break;
                    }


                case "CompaniesTable":
                    {
                        if (CitiesListDiff != null && CitiesListDiff.Count() > 0)
                        {
                            string SQLCommandCMD = "";
                            SQLCommandCMD += "INSERT INTO [CompaniesTable] (CompanyName) ";

                            bool first = true;

                            foreach (string tempitem in CompaniesListDiff)
                            {
                                if (!first)
                                {
                                    SQLCommandCMD += " UNION ALL ";
                                }
                                else
                                {
                                    first = false;
                                }

                                SQLCommandCMD += "SELECT '" + tempitem + "'";
                            }
                            UpdateDatabase(SQLCommandCMD);
                        }

                        break;
                    }

                case "TrucksTable":
                    {
                        if (CompanyTruckListDiff != null && CompanyTruckListDiff.Count() > 0)
                        {
                            string SQLCommandCMD = "";
                            SQLCommandCMD += "INSERT INTO [TrucksTable] (TruckName, TruckType) ";

                            bool first = true;

                            foreach (CompanyTruck tempitem in CompanyTruckListDiff)
                            {
                                if (!first)
                                {
                                    SQLCommandCMD += " UNION ALL ";
                                }
                                else
                                {
                                    first = false;
                                }

                                SQLCommandCMD += "SELECT '" + tempitem.TruckName + "', " + tempitem.Type;
                            }
                            UpdateDatabase(SQLCommandCMD);
                        }

                        break;
                    }
            }
        }
        //Load from DB
        private void GetDataFromDatabase(string _targetTable)
        {
            SqlCeDataReader reader = null;

            try
            {
                if (DBconnection.State == ConnectionState.Closed)
                    DBconnection.Open();

                int totalrecord = 0;

                switch (_targetTable)
                {
                    case "Dependencies":
                        {
                            DBDependencies.Clear();

                            string commandText = "SELECT Dependency FROM [Dependencies];";
                            reader = new SqlCeCommand(commandText, DBconnection).ExecuteReader();

                            while (reader.Read())
                            {
                                DBDependencies.Add(reader["Dependency"].ToString());
                            }
                            //DBDependencies

                            totalrecord = DBDependencies.Count();
                            break;
                        }

                    case "CargoesTable":
                        {
                            CargoesListDB.Clear(); //Clears existing list
                            
                            string commandText = "SELECT ID_cargo, CargoName FROM [CargoesTable];";
                            //DBconnection.Open();
                            reader = new SqlCeCommand(commandText, DBconnection).ExecuteReader();

                            List<TrailerDefinition> tempDefVars = new List<TrailerDefinition>();

                            while (reader.Read())
                            {
                                commandText = "SELECT TrailerDefinitionID, CargoType FROM [CargoesToTrailerDefinitionTable] WHERE CargoID = '" + reader["ID_cargo"].ToString() + "';";

                                try
                                {
                                    SqlCeDataReader reader2 = new SqlCeCommand(commandText, DBconnection).ExecuteReader();

                                    Dictionary<string, int> tempVar = new Dictionary<string, int>();

                                    while (reader2.Read())
                                    {
                                        commandText = "SELECT TrailerDefinitionName FROM [TrailerDefinitionTable] WHERE ID_trailerD = '" + reader2["TrailerDefinitionID"].ToString() + "';";
                                        SqlCeDataReader reader3 = new SqlCeCommand(commandText, DBconnection).ExecuteReader();

                                        tempDefVars.Add(new TrailerDefinition(reader3["TrailerDefinitionName"].ToString(), int.Parse(reader2["CargoType"].ToString()), "1"));//reader2["CargoUnitsCount"].ToString()));
                                    }
                                }
                                catch
                                { }

                                CargoesListDB.Add(new Cargo(reader["CargoName"].ToString(), tempDefVars));
                            }

                            totalrecord = CargoesListDB.Count();

                            break;
                        }

                    case "CitysTable":
                        {
                            CitiesListDB.Clear(); //Clears existing list

                            string commandText = "SELECT CityName FROM [CitysTable];";

                            reader = new SqlCeCommand(commandText, DBconnection).ExecuteReader();

                            while (reader.Read())
                            {
                                CitiesListDB.Add(reader["CityName"].ToString());
                            }

                            totalrecord = CitiesListDB.Count();

                            break;
                        }

                    case "CompaniesTable":
                        {
                            CompaniesListDB.Clear(); //Clears existing list

                            string commandText = "SELECT CompanyName FROM [CompaniesTable];";

                            reader = new SqlCeCommand(commandText, DBconnection).ExecuteReader();

                            while (reader.Read())
                            {
                                CompaniesListDB.Add(reader["CompanyName"].ToString());
                            }

                            totalrecord = CompaniesListDB.Count();

                            break;
                        }
                    case "TrucksTable":
                        {
                            CompanyTruckListDB.Clear(); //Clears existing list

                            string commandText = "SELECT TruckName, TruckType FROM [TrucksTable];";

                            reader = new SqlCeCommand(commandText, DBconnection).ExecuteReader();

                            while (reader.Read())
                            {
                                CompanyTruckListDB.Add(new CompanyTruck(reader["TruckName"].ToString(), int.Parse(reader["TruckType"].ToString())));
                            }

                            totalrecord = CompanyTruckListDB.Count();

                            break;
                        }
                }

                IO_Utilities.LogWriter("Loaded " + totalrecord + " entries from " + _targetTable + " table.");
            }
            catch
            {
                IO_Utilities.LogWriter("Missing " + DBconnection.DataSource + " file");
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                DBconnection.Close();
            }

        }

        //External Data

        private void ExtDataCreateDatabase(string _dbname)
        {
            string connectionString;

            string fileName = _dbname;

            int index = _dbname.LastIndexOf("\\");
            string first = _dbname.Substring(0, index);
            string second = _dbname.Substring(index + 1);

            if (!Directory.Exists(first))
            {
                Directory.CreateDirectory(first);
            }

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            connectionString = string.Format("DataSource ='{0}';", fileName);

            SqlCeEngine Engine = new SqlCeEngine(connectionString);
            Engine.CreateDatabase();

            ExtDataCreateDatabaseStructure(fileName);
        }

        private void ExtDataCreateDatabaseStructure(string _fileName)
        {
            SqlCeConnection tDBconnection;
            tDBconnection = new SqlCeConnection("Data Source = " + _fileName + ";");

            if (tDBconnection.State == ConnectionState.Closed)
            {
                tDBconnection.Open();
            }
            
            SqlCeCommand cmd;

            string sql = "";

            sql += "CREATE TABLE BodyTypesTable (ID_bodytype INT IDENTITY(1,1) PRIMARY KEY, BodyTypeName NVARCHAR(32) NOT NULL);";

            sql += "CREATE TABLE CargoesTable (ID_cargo INT IDENTITY(1,1) PRIMARY KEY, CargoName NVARCHAR(32) NOT NULL, ADRclass INT NOT NULL, Fragility DECIMAL(4,3) NOT NULL, Mass DECIMAL(10,3) NOT NULL, UnitRewardpPerKM DECIMAL(12,5) NOT NULL, Valuable BIT NOT NULL, Overweight BIT NOT NULL);";

            sql += "CREATE TABLE BodyTypesToCargoTable (ID_BodyToCrg INT IDENTITY(1,1) PRIMARY KEY, CargoID INT NOT NULL, BodyTypeID INT NOT NULL);";
            sql += "ALTER TABLE BodyTypesToCargoTable ADD FOREIGN KEY(CargoID) REFERENCES CargoesTable(ID_cargo);";
            sql += "ALTER TABLE BodyTypesToCargoTable ADD FOREIGN KEY(BodyTypeID) REFERENCES BodyTypesTable(ID_bodytype) ON DELETE CASCADE;";

            sql += "CREATE TABLE CompaniesTable (ID_company INT IDENTITY(1,1) PRIMARY KEY, CompanyName NVARCHAR(32) NOT NULL);";

            sql += "CREATE TABLE AllCargoesTable (ID_cargo INT IDENTITY(1,1) PRIMARY KEY, CargoName NVARCHAR(32) NOT NULL);";

            sql += "CREATE TABLE CompaniesCargoesInTable (ID_CargoIn INT IDENTITY(1,1) PRIMARY KEY, CompanyID INT NOT NULL, CargoID INT NOT NULL);";
            sql += "ALTER TABLE CompaniesCargoesInTable ADD FOREIGN KEY(CompanyID) REFERENCES CompaniesTable(ID_company) ON DELETE CASCADE;";
            sql += "ALTER TABLE CompaniesCargoesInTable ADD FOREIGN KEY(CargoID) REFERENCES AllCargoesTable(ID_cargo) ON DELETE CASCADE;";

            sql += "CREATE TABLE CompaniesCargoesOutTable (ID_CargoOut INT IDENTITY(1,1) PRIMARY KEY, CompanyID INT NOT NULL, CargoID INT NOT NULL);";
            sql += "ALTER TABLE CompaniesCargoesOutTable ADD FOREIGN KEY(CompanyID) REFERENCES CompaniesTable(ID_company) ON DELETE CASCADE;";
            sql += "ALTER TABLE CompaniesCargoesOutTable ADD FOREIGN KEY(CargoID) REFERENCES AllCargoesTable(ID_cargo) ON DELETE CASCADE;";

            string[] linesArray = sql.Split(';');

            foreach (string sqlline in linesArray)
            {
                if (sqlline != "")
                {
                    cmd = new SqlCeCommand(sqlline, tDBconnection);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        UpdateStatusBarMessage.ShowStatusMessage(SMStatus.Info, "message_database_created");
                    }
                    catch (SqlCeException sqlexception)
                    {
                        UpdateStatusBarMessage.ShowStatusMessage(SMStatus.Error, "error_sql_exception");
                        MessageBox.Show(sqlexception.Message, "SQL Exception. Ext Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        UpdateStatusBarMessage.ShowStatusMessage(SMStatus.Error, "error_exception");
                        MessageBox.Show(ex.Message, "Exception.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            tDBconnection.Close();
        }

        private void ExtDataInsertDataIntoDatabase(string _dbname, string _targetTable, object _data)
        {
            switch (_targetTable)
            {
                case "CargoesTable":
                    {
                        List<ExtCargo> extCargolist = _data as List<ExtCargo>;

                        SqlCeConnection tDBconnection;
                        string _fileName = _dbname;//Directory.GetCurrentDirectory() + @"\gameref\ETS\cache\" + _dbname + ".sdf";
                        tDBconnection = new SqlCeConnection("Data Source = " + _fileName + ";");

                        if (tDBconnection.State == ConnectionState.Closed)
                        {
                            tDBconnection.Open();
                        }

                        SqlCeCommand command = tDBconnection.CreateCommand();
                        string updatecommandText = "";

                        List<string> tempBodyTypes = new List<string>();

                        foreach (ExtCargo tempcargo in extCargolist)
                        {
                            tempBodyTypes.AddRange(tempcargo.BodyTypes);
                        }

                        tempBodyTypes = tempBodyTypes.Distinct().ToList();

                        foreach (string tempBody in tempBodyTypes)
                        {
                            updatecommandText = "INSERT INTO [BodyTypesTable] (BodyTypeName) " +
                                                "VALUES('" +
                                                tempBody + "');";

                            //SqlCeCommand command = DBconnection.CreateCommand();
                            try
                            {
                                command.CommandText = updatecommandText;
                                command.ExecuteNonQuery();
                            }
                            catch (SqlCeException sqlexception)
                            {
                                MessageBox.Show(sqlexception.Message + " | " + sqlexception.ErrorCode, "SQL Exception.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }

                        int CargoID = 1;

                        foreach (ExtCargo tempcargo in extCargolist)
                        {
                            byte valuable = 0, overweight = 0;
                            if (tempcargo.Valuable)
                                valuable = 1;

                            if (tempcargo.Overweight)
                                overweight = 1;

                            updatecommandText = "INSERT INTO [CargoesTable] (CargoName, ADRclass, Fragility, Mass, UnitRewardpPerKM, Valuable, Overweight) " +
                                                "VALUES('" +
                                                tempcargo.CargoName + "', " +
                                                tempcargo.ADRclass + ", " +
                                                tempcargo.Fragility.ToString(CultureInfo.InvariantCulture) + ", " +
                                                tempcargo.Mass.ToString(CultureInfo.InvariantCulture) + ", " +
                                                tempcargo.UnitRewardpPerKM.ToString(CultureInfo.InvariantCulture) + ", " +
                                                valuable + ", " +
                                                overweight + ");";

                            //SqlCeCommand command = DBconnection.CreateCommand();
                            try
                            {
                                command.CommandText = updatecommandText;
                                //command.Connection.Open();
                                command.ExecuteNonQuery();
                            }
                            catch (SqlCeException sqlexception)
                            {
                                MessageBox.Show(sqlexception.Message + " | " + sqlexception.ErrorCode, "SQL Exception.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

                            foreach (string body_types in tempcargo.BodyTypes)
                            {
                                updatecommandText = "SELECT ID_bodytype FROM [BodyTypesTable] WHERE BodyTypeName = '" + body_types + "' ";
                                command.CommandText = updatecommandText;

                                //command.Connection.Open();
                                SqlCeDataReader readerDef = command.ExecuteReader();

                                int BodyTypeID = -1;

                                while (readerDef.Read())
                                {
                                    BodyTypeID = int.Parse(readerDef["ID_bodytype"].ToString());
                                }
                                //command.Connection.Close();

                                updatecommandText = "INSERT INTO [BodyTypesToCargoTable] (CargoID, BodyTypeID) " +
                                                "VALUES(" +
                                                CargoID + ", " +
                                                BodyTypeID + ");";

                                try
                                {
                                    command.CommandText = updatecommandText;
                                    //command.Connection.Open();
                                    command.ExecuteNonQuery();
                                }
                                catch (SqlCeException sqlexception)
                                {
                                    MessageBox.Show(sqlexception.Message + " | " + sqlexception.ErrorCode, "SQL Exception.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }

                            CargoID++;
                        }

                        tDBconnection.Close();

                        break;
                    }

                case "CompaniesTable":
                    {
                        List<ExtCompany> extCompanylist = _data as List<ExtCompany>;

                        SqlCeConnection tDBconnection;
                        string _fileName = _dbname;//Directory.GetCurrentDirectory() + @"\gameref\ETS\cache\" + _dbname + ".sdf";
                        tDBconnection = new SqlCeConnection("Data Source = " + _fileName + ";");

                        if (tDBconnection.State == ConnectionState.Closed)
                        {
                            tDBconnection.Open();
                        }

                        SqlCeCommand command = tDBconnection.CreateCommand();
                        string updatecommandText = "";

                        List<string> tempCompanies = new List<string>();


                        foreach (ExtCompany tempCompany in extCompanylist)
                        {
                            tempCompanies.AddRange(tempCompany.inCargo);
                            tempCompanies.AddRange(tempCompany.outCargo);
                        }

                        tempCompanies = tempCompanies.Distinct().ToList();

                        //updatecommandText = "INSERT INTO [AllCargoesTable] (CargoName) ";
                        updatecommandText = "INSERT INTO [AllCargoesTable] (CargoName) VALUES(@inputText)";
                        command.CommandText = updatecommandText;
                        command.Parameters.Add("@inputText", SqlDbType.NVarChar);

                        foreach (string tempCompany in tempCompanies)
                        {
                            //updatecommandText += "VALUES('" + tempCompany + "') ";
                            try
                            {
                                command.Parameters[0].Value = tempCompany;
                                command.ExecuteNonQuery();
                                //command.CommandText = updatecommandText;
                                //command.ExecuteNonQuery();
                            }
                            catch (SqlCeException sqlexception)
                            {
                                MessageBox.Show(sqlexception.Message + " | " + sqlexception.ErrorCode, "SQL Exception.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }

                        foreach (ExtCompany tempCompany in extCompanylist)
                        {
                            updatecommandText = "INSERT INTO [CompaniesTable] (CompanyName) " +
                                                "VALUES('" +
                                                tempCompany.CompanyName + "');";

                            //SqlCeCommand command = DBconnection.CreateCommand();
                            try
                            {
                                command.CommandText = updatecommandText;
                                command.ExecuteNonQuery();
                            }
                            catch (SqlCeException sqlexception)
                            {
                                MessageBox.Show(sqlexception.Message + " | " + sqlexception.ErrorCode, "SQL Exception.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

                            updatecommandText = "SELECT ID_company FROM [CompaniesTable] WHERE CompanyName = '" + tempCompany.CompanyName + "' ";
                            command.CommandText = updatecommandText;

                            //command.Connection.Open();
                            SqlCeDataReader readerDef = command.ExecuteReader();

                            int CompanyID = -1;

                            while (readerDef.Read())
                            {
                                CompanyID = int.Parse(readerDef["ID_company"].ToString());
                            }

                            foreach (string tempcargo in tempCompany.inCargo)
                            {
                                updatecommandText = "SELECT ID_cargo FROM [AllCargoesTable] WHERE CargoName = '" + tempcargo + "' ";
                                command.CommandText = updatecommandText;

                                //command.Connection.Open();
                                int CargoID = -1;
                                try
                                {
                                    SqlCeDataReader readerCargo = command.ExecuteReader();

                                    while (readerCargo.Read())
                                    {
                                        CargoID = int.Parse(readerCargo["ID_cargo"].ToString());
                                    }
                                    if (CargoID != -1)
                                    {
                                        updatecommandText = "INSERT INTO [CompaniesCargoesInTable] (CompanyID, CargoID) " +
                                                            "VALUES(" +
                                                            CompanyID + ", " +
                                                            CargoID + ");";
                                        try
                                        {
                                            command.CommandText = updatecommandText;
                                            //command.Connection.Open();
                                            command.ExecuteNonQuery();
                                        }
                                        catch (SqlCeException sqlexception)
                                        {
                                            MessageBox.Show(sqlexception.Message + " | " + sqlexception.ErrorCode, "SQL Exception.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    }

                                }
                                catch (SqlCeException sqlexception)
                                {
                                    MessageBox.Show(sqlexception.Message + " | " + sqlexception.ErrorCode, "SQL Exception.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }


                            }

                            foreach (string tempcargo in tempCompany.outCargo)
                            {
                                updatecommandText = "SELECT ID_cargo FROM [AllCargoesTable] WHERE CargoName = '" + tempcargo + "' ";
                                command.CommandText = updatecommandText;

                                //command.Connection.Open();
                                int CargoID = -1;
                                try
                                {
                                    SqlCeDataReader readerCargo = command.ExecuteReader();

                                    while (readerCargo.Read())
                                    {
                                        CargoID = int.Parse(readerCargo["ID_cargo"].ToString());
                                    }
                                    if (CargoID != -1)
                                    {
                                        updatecommandText = "INSERT INTO [CompaniesCargoesOutTable] (CompanyID, CargoID) " +
                                                            "VALUES(" +
                                                            CompanyID + ", " +
                                                            CargoID + ");";
                                        try
                                        {
                                            command.CommandText = updatecommandText;
                                            //command.Connection.Open();
                                            command.ExecuteNonQuery();
                                        }
                                        catch (SqlCeException sqlexception)
                                        {
                                            MessageBox.Show(sqlexception.Message + " | " + sqlexception.ErrorCode, "SQL Exception.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    }

                                }
                                catch (SqlCeException sqlexception)
                                {
                                    MessageBox.Show(sqlexception.Message + " | " + sqlexception.ErrorCode, "SQL Exception.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }


                            }

                        }

                        tDBconnection.Close();

                        break;
                    }
            }
        }

        private void LoadCachedExternalCargoData(string _dbname)
        {
            SqlCeDataReader reader = null, reader2 = null;

            try
            {
                SqlCeConnection tDBconnection;
                string _fileName = Directory.GetCurrentDirectory() + @"\gameref\cache\" + GameType + "\\" + _dbname + ".sdf";

                if (!File.Exists(_fileName))
                    return;

                tDBconnection = new SqlCeConnection("Data Source = " + _fileName + ";");

                if (tDBconnection.State == ConnectionState.Closed)
                {
                    tDBconnection.Open();
                }

                string commandText = "SELECT ID_cargo, CargoName, ADRclass, Fragility, Mass, UnitRewardpPerKM, Valuable, Overweight FROM [CargoesTable]";

                reader = new SqlCeCommand(commandText, tDBconnection).ExecuteReader();

                while (reader.Read())
                {
                    ExtCargo tempExtCargo = new ExtCargo(reader["CargoName"].ToString());

                    tempExtCargo.Fragility = decimal.Parse(reader["Fragility"].ToString());

                    tempExtCargo.ADRclass = int.Parse(reader["ADRclass"].ToString());

                    tempExtCargo.Mass = decimal.Parse(reader["Mass"].ToString());

                    tempExtCargo.UnitRewardpPerKM = decimal.Parse(reader["UnitRewardpPerKM"].ToString());

                    //tempExtCargo.Groups.Add(reader["CargoName"].ToString());

                    //tempExtCargo.MaxDistance = int.Parse(reader["CargoName"].ToString());

                    //tempExtCargo.Volume = decimal.Parse(reader["CargoName"].ToString());

                    tempExtCargo.Valuable = bool.Parse(reader["Valuable"].ToString());

                    tempExtCargo.Overweight = bool.Parse(reader["Overweight"].ToString());

                    commandText = "SELECT BodyTypesTable.BodyTypeName FROM [BodyTypesToCargoTable] INNER JOIN [BodyTypesTable] ON BodyTypesTable.ID_bodytype = BodyTypesToCargoTable.BodyTypeID WHERE BodyTypesToCargoTable.CargoID = '" + reader["ID_cargo"].ToString() + "';";

                    reader2 = new SqlCeCommand(commandText, tDBconnection).ExecuteReader();

                    while (reader2.Read())
                    {
                        tempExtCargo.BodyTypes.Add(reader2["BodyTypeName"].ToString());
                    }

                    ExtCargoList.Add(tempExtCargo);
                }

                commandText = "SELECT ID_company, CompanyName FROM [CompaniesTable]";

                reader = new SqlCeCommand(commandText, tDBconnection).ExecuteReader();

                while (reader.Read())
                {
                    int compindex = ExternalCompanies.FindIndex(x => x.CompanyName == reader["CompanyName"].ToString());

                    if (compindex == -1)
                    {
                        ExtCompany tempExtCompany = new ExtCompany(reader["CompanyName"].ToString());

                        commandText = "SELECT AllCargoesTable.CargoName FROM [CompaniesCargoesOutTable] INNER JOIN [AllCargoesTable] ON AllCargoesTable.ID_cargo = CompaniesCargoesOutTable.CargoID WHERE CompaniesCargoesOutTable.CompanyID = '" + reader["ID_company"].ToString() + "';";

                        reader2 = new SqlCeCommand(commandText, tDBconnection).ExecuteReader();

                        while (reader2.Read())
                        {
                            tempExtCompany.outCargo.Add(reader2["CargoName"].ToString());
                        }

                        ExternalCompanies.Add(tempExtCompany);
                    }
                    else
                    {
                        commandText = "SELECT AllCargoesTable.CargoName FROM [CompaniesCargoesOutTable] INNER JOIN [AllCargoesTable] ON AllCargoesTable.ID_cargo = CompaniesCargoesOutTable.CargoID WHERE CompaniesCargoesOutTable.CompanyID = '" + reader["ID_company"].ToString() + "';";

                        reader2 = new SqlCeCommand(commandText, tDBconnection).ExecuteReader();

                        while (reader2.Read())
                        {
                            ExternalCompanies[compindex].outCargo.Add(reader2["CargoName"].ToString());
                        }
                    }
                }

                tDBconnection.Close();
            }
            catch
            { }
        }
    }
}
