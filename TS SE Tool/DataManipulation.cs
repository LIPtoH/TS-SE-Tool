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
using TS_SE_Tool.CustomClasses;
using ErikEJ.SqlCe;
using System.ComponentModel;

namespace TS_SE_Tool
{
    public partial class FormMain : Form
    {
        private void PrepareData(object sender, DoWorkEventArgs e)
        {
            string[] chunkOfline;

            LogWriter("Prepare started");
            ShowStatusMessages("i", "message_preparing_data");

            int economyEventQueueIndex = 0;
            int EconomyEventline = 0;

            //scan through save file
            bool EconomySection = false, PlayerSection = false;

            int workerprogressmult = (int)Math.Floor((decimal)(tempSavefileInMemory.Length / 80)), workerprogress = 0;


            for (int line = 0; line < tempSavefileInMemory.Length; line++)
            {
                try
                {
                    if ((int)Math.Floor((decimal)(line / workerprogressmult)) > workerprogress)
                    {
                        workerprogress++;
                        worker.ReportProgress(workerprogress);
                    }

                    int EconomyEventColumn;
                    string destinationcity;
                    string destinationcompany;


                    if (tempSavefileInMemory[line].Contains("_nameless"))
                    {
                        string asd = tempSavefileInMemory[line].Substring(tempSavefileInMemory[line].IndexOf("_nameless"));
                        int dsa = asd.IndexOf(" ");
                        string nameless;
                        if (dsa == -1)
                            nameless = asd.Substring(10);
                        else
                            nameless = asd.Substring(10, dsa - 10);
                        namelessList.Add(nameless);
                        //EconomySection = true;
                    }

                    if (tempSavefileInMemory[line].StartsWith("economy : _nameless"))
                    {
                        EconomySection = true;
                        continue;
                    }

                    if (EconomySection && tempSavefileInMemory[line].StartsWith("}"))
                    {
                        EconomySection = false;
                        continue;
                    }

                    if (tempSavefileInMemory[line].StartsWith(" game_time: "))
                    {
                        chunkOfline = tempSavefileInMemory[line].Split(new char[] { ' ' });
                        InGameTime = int.Parse(chunkOfline[2]);
                        continue;
                    }

                    //Find garages status
                    if (tempSavefileInMemory[line].StartsWith("garage : garage."))
                    {
                        //bool garageinfoended = false;

                        chunkOfline = tempSavefileInMemory[line].Split(new char[] { '.', '{' });
                        Garages tempGarage = GaragesList.Find(x => x.GarageName == chunkOfline[1].TrimEnd(new char[] { ' ' }));

                        line++;

                        while (true)//(!garageinfoended)
                        {
                            if (tempSavefileInMemory[line].StartsWith(" vehicles["))
                                tempGarage.Vehicles.Add(tempSavefileInMemory[line].Split(new char[] { ' ' })[2]);
                            else if (tempSavefileInMemory[line].StartsWith(" drivers["))
                                tempGarage.Drivers.Add(tempSavefileInMemory[line].Split(new char[] { ' ' })[2]);
                            else if (tempSavefileInMemory[line].StartsWith(" status:"))
                                tempGarage.GarageStatus = int.Parse(tempSavefileInMemory[line].Split(new char[] { ':' })[1]);
                            else if (tempSavefileInMemory[line].StartsWith("}"))
                                break;//garageinfoended = true;

                            line++;
                        }

                        continue;
                    }

                    //Economy Section
                    if (EconomySection)
                    {
                        //search through companies.city list
                        if (tempSavefileInMemory[line].StartsWith(" companies["))// && (tempSavefileInMemory[line] != "null"))
                        {
                            chunkOfline = tempSavefileInMemory[line].Split(new char[] { '.' });

                            if (chunkOfline[3] == null)
                            {
                                continue;
                            }
                            //Add City to List from companies list
                            if (CitiesList.Where(x => x.CityName == chunkOfline[3]).Count() == 0)
                            {
                                CitiesList.Add(new City(chunkOfline[3], ""));
                            }

                            CompaniesList.Add(chunkOfline[2]); //add company to list

                            //Add Company to City from companies list
                            foreach (City tempcity in CitiesList.FindAll(x => x.CityName == chunkOfline[3]))
                            {
                                tempcity.AddCompany(chunkOfline[2], 0);
                            }

                            continue;
                        }

                        //Find garages
                        if (tempSavefileInMemory[line].StartsWith(" garages["))
                        {
                            chunkOfline = tempSavefileInMemory[line].Split(new char[] { ':' });
                            Garages tg = new Garages(chunkOfline[1].Split(new char[] { '.' })[1], 0);
                            GaragesList.Add(tg);
                            continue;
                        }
                        //Find visited cities
                        if (tempSavefileInMemory[line].StartsWith(" visited_cities["))
                        {
                            chunkOfline = tempSavefileInMemory[line].Split(new char[] { ':' });
                            VisitedCities.Add(new VisitedCity(chunkOfline[1].Replace(" ", string.Empty), 0, true));
                            continue;
                        }

                        if (tempSavefileInMemory[line].StartsWith(" visited_cities_count["))
                        {
                            int cityid = int.Parse(tempSavefileInMemory[line].Split(new char[] { '[', ']' })[1]);
                            chunkOfline = tempSavefileInMemory[line].Split(new char[] { ':' });
                            VisitedCities[cityid].VisitCount = int.Parse(chunkOfline[1].Replace(" ", string.Empty));
                            continue;
                        }
                        //Experience points
                        if (tempSavefileInMemory[line].StartsWith(" experience_points"))
                        {
                            chunkOfline = tempSavefileInMemory[line].Split(new char[] { ' ' });
                            PlayerProfileData.ExperiencePoints = uint.Parse(chunkOfline[2]);
                            continue;
                        }

                        //Skills
                        if (tempSavefileInMemory[line].StartsWith(" adr:"))
                        {
                            string[] LineArray = tempSavefileInMemory[line].Split(new char[] { ' ' });
                            
                            char[] ADR = Convert.ToString(byte.Parse(LineArray[2]), 2).PadLeft(6, '0').ToCharArray();
                            Array.Reverse(ADR);
                            PlayerProfileData.PlayerSkills[0] = Convert.ToByte(new string(ADR), 2);
                            //PlayerProfileData.PlayerSkills[0] = byte.Parse(LineArray[2]);
                            continue;
                        }
                        if (tempSavefileInMemory[line].StartsWith(" long_dist:"))
                        {
                            string[] LineArray = tempSavefileInMemory[line].Split(new char[] { ' ' });
                            PlayerProfileData.PlayerSkills[1] = byte.Parse(LineArray[2]);
                            continue;
                        }
                        if (tempSavefileInMemory[line].StartsWith(" heavy:"))
                        {
                            string[] LineArray = tempSavefileInMemory[line].Split(new char[] { ' ' });
                            PlayerProfileData.PlayerSkills[2] = byte.Parse(LineArray[2]);
                            continue;
                        }
                        if (tempSavefileInMemory[line].StartsWith(" fragile:"))
                        {
                            string[] LineArray = tempSavefileInMemory[line].Split(new char[] { ' ' });
                            PlayerProfileData.PlayerSkills[3] = byte.Parse(LineArray[2]);
                            continue;
                        }
                        if (tempSavefileInMemory[line].StartsWith(" urgent:"))
                        {
                            string[] LineArray = tempSavefileInMemory[line].Split(new char[] { ' ' });
                            PlayerProfileData.PlayerSkills[4] = byte.Parse(LineArray[2]);
                            continue;
                        }
                        if (tempSavefileInMemory[line].StartsWith(" mechanical:"))
                        {
                            string[] LineArray = tempSavefileInMemory[line].Split(new char[] { ' ' });
                            PlayerProfileData.PlayerSkills[5] = byte.Parse(LineArray[2]);
                            continue;
                        }

                        //User Colors
                        if (tempSavefileInMemory[line].StartsWith(" user_colors["))
                        {
                            string userColorStr = tempSavefileInMemory[line].Split(new string[] { ": " }, 0)[1];
                            Color userColor;

                            if (userColorStr != "0")
                            {
                                if (userColorStr == "nil")
                                {
                                    userColorStr = "4294967295";
                                }

                                userColorStr = IntegerToHexString(Convert.ToUInt32(userColorStr));
                                userColorStr = userColorStr.Substring(2);

                                int _B = int.Parse(userColorStr.Substring(0, 2), NumberStyles.HexNumber);
                                int _G = int.Parse(userColorStr.Substring(2, 2), NumberStyles.HexNumber);
                                int _R = int.Parse(userColorStr.Substring(4, 2), NumberStyles.HexNumber);

                                userColor = Color.FromArgb(255, _R, _G, _B);
                                UserColorsList.Add(userColor);
                            }
                            else
                            {
                                userColor = Color.FromArgb(0, 0, 0, 0);
                                UserColorsList.Add(userColor);
                            }
                            continue;
                        }

                        //GPS
                        /*
                        if (tempSavefileInMemory[line].StartsWith(" stored_nav_start_pos:"))
                        {
                            //string[] LineArray = tempSavefileInMemory[line].Split(new char[] { ' ' });
                            //PlayerProfileData.PlayerSkills[5] = byte.Parse(LineArray[2]);
                            continue;
                        }

                        if (tempSavefileInMemory[line].StartsWith(" stored_nav_start_pos:"))
                        {
                            //string[] LineArray = tempSavefileInMemory[line].Split(new char[] { ' ' });
                            //PlayerProfileData.PlayerSkills[5] = byte.Parse(LineArray[2]);
                            continue;
                        }
                        */
                        //Online
                        if (tempSavefileInMemory[line].StartsWith(" stored_online_gps_behind_waypoints:"))
                        {
                            string[] LineArray = tempSavefileInMemory[line].Split(new char[] { ' ' });
                            int gpscount = int.Parse(LineArray[2]);

                            for (int i = 0; i < gpscount; i++)
                            {
                                line++;
                                GPSbehindOnline.Add(tempSavefileInMemory[line].Split(new char[] { ' ' })[2], new List<string>());
                            }
                            continue;
                        }
                        if (tempSavefileInMemory[line].StartsWith(" stored_online_gps_ahead_waypoints:"))
                        {
                            string[] LineArray = tempSavefileInMemory[line].Split(new char[] { ' ' });
                            int gpscount = int.Parse(LineArray[2]);

                            for (int i = 0; i < gpscount; i++)
                            {
                                line++;
                                GPSaheadOnline.Add(tempSavefileInMemory[line].Split(new char[] { ' ' })[2], new List<string>());
                            }
                            continue;
                        }

                        //Offline
                        if (tempSavefileInMemory[line].StartsWith(" stored_gps_behind_waypoints:"))
                        {
                            string[] LineArray = tempSavefileInMemory[line].Split(new char[] { ' ' });
                            int gpscount = int.Parse(LineArray[2]);

                            for (int i = 0; i < gpscount; i++)
                            {
                                line++;
                                GPSbehind.Add(tempSavefileInMemory[line].Split(new char[] { ' ' })[2], new List<string>());
                            }
                            continue;
                        }
                        if (tempSavefileInMemory[line].StartsWith(" stored_gps_ahead_waypoints:"))
                        {
                            string[] LineArray = tempSavefileInMemory[line].Split(new char[] { ' ' });
                            int gpscount = int.Parse(LineArray[2]);

                            for (int i = 0; i < gpscount; i++)
                            {
                                line++;
                                GPSahead.Add(tempSavefileInMemory[line].Split(new char[] { ' ' })[2], new List<string>());
                            }
                            continue;
                        }

                        //Find last visited city
                        if (tempSavefileInMemory[line].StartsWith(" last_visited_city:"))
                        {
                            string[] LineArray = tempSavefileInMemory[line].Split(new char[] { ' ' });
                            LastVisitedCity = LineArray[2];
                            continue;
                        }

                        if (tempSavefileInMemory[line].StartsWith(" discovered_items:"))
                        {
                            string[] LineArray = tempSavefileInMemory[line].Split(new char[] { ' ' });
                            line += int.Parse(LineArray[2]);
                            continue;
                        }
                    }

                    //Account Money
                    if (tempSavefileInMemory[line].StartsWith(" money_account:"))
                    {
                        chunkOfline = tempSavefileInMemory[line].Split(new char[] { ' ' });
                        PlayerProfileData.AccountMoney = uint.Parse(chunkOfline[2]);
                        continue;
                    }

                    //Player section
                    if (tempSavefileInMemory[line].StartsWith("player :"))
                    {
                        PlayerSection = true;
                        continue;
                    }

                    if (PlayerSection && tempSavefileInMemory[line].StartsWith("}"))
                    {
                        PlayerSection = false;
                        continue;
                    }
                    if (PlayerSection)
                    {
                        //Find HQ city
                        if (tempSavefileInMemory[line].StartsWith(" hq_city:"))
                        {
                            chunkOfline = tempSavefileInMemory[line].Split(new char[] { ' ' });
                            PlayerProfileData.HQcity = chunkOfline[2];
                            continue;
                        }

                        if (tempSavefileInMemory[line].StartsWith(" assigned_truck:"))
                        {
                            chunkOfline = tempSavefileInMemory[line].Split(new char[] { ' ' });
                            UserCompanyAssignedTruck = chunkOfline[2];
                            continue;
                        }

                        if (tempSavefileInMemory[line].StartsWith(" drivers[0]:"))
                        {
                            chunkOfline = tempSavefileInMemory[line].Split(new char[] { ' ' });
                            PlayerProfileData.UserDriver = chunkOfline[2];
                            continue;
                        }

                        if (tempSavefileInMemory[line].StartsWith(" trucks["))
                        {
                            chunkOfline = tempSavefileInMemory[line].Split(new char[] { ' ' });
                            UserTruckDictionary.Add(chunkOfline[2], new UserCompanyTruckData());
                            continue;
                        }

                        if(tempSavefileInMemory[line].StartsWith(" assigned_trailer:"))
                        {
                            chunkOfline = tempSavefileInMemory[line].Split(new char[] { ' ' });
                            UserCompanyAssignedTrailer = chunkOfline[2];
                            continue;
                        }

                        if (tempSavefileInMemory[line].StartsWith(" truck_placement:"))
                        {
                            //chunkOfline = tempSavefileInMemory[line].Split(new char[] { ':' });
                            UserCompanyAssignedTruckPlacement = tempSavefileInMemory[line];//chunkOfline[1];
                            continue;
                        }

                        if (tempSavefileInMemory[line].StartsWith(" trailers["))
                        {
                            chunkOfline = tempSavefileInMemory[line].Split(new char[] { ' ' });
                            //UserTruckList.Add(chunkOfline[2], new UserCompanyTruck());

                            UserTrailerDictionary.Add(chunkOfline[2], new UserCompanyTruckData());
                            continue;
                        }
                    }
                    //Populate GPS
                    if (tempSavefileInMemory[line].StartsWith("gps_waypoint_storage"))
                    {
                        string nameless = tempSavefileInMemory[line].Split(new char[] { ' ' })[2];

                        if( GPSbehind.ContainsKey(nameless))
                        {
                            line++;
                            while (!tempSavefileInMemory[line].StartsWith("}"))
                            {
                                GPSbehind[nameless].Add(tempSavefileInMemory[line]);
                                line++;
                            }
                            
                        }
                        else
                        if (GPSahead.ContainsKey(nameless))
                        {
                                line++;
                                while (!tempSavefileInMemory[line].StartsWith("}"))
                                {
                                    GPSahead[nameless].Add(tempSavefileInMemory[line]);
                                    line++;
                                }
                        }
                        else
                        if (GPSbehindOnline.ContainsKey(nameless))
                        {
                            line++;
                            while (!tempSavefileInMemory[line].StartsWith("}"))
                            {
                                GPSbehindOnline[nameless].Add(tempSavefileInMemory[line]);
                                line++;
                            }

                        }
                        else
                        if (GPSaheadOnline.ContainsKey(nameless))
                        {
                            line++;
                            while (!tempSavefileInMemory[line].StartsWith("}"))
                            {
                                GPSaheadOnline[nameless].Add(tempSavefileInMemory[line]);
                                line++;
                            }
                        }
                    }

                    //find vehicles Truck
                    if (tempSavefileInMemory[line].StartsWith("vehicle :"))
                    {
                        chunkOfline = tempSavefileInMemory[line].Split(new char[] { ' ' });
                        string vehiclenameless = chunkOfline[2];

                        if (UserTruckDictionary.ContainsKey(vehiclenameless))//UserTruckList.ContainsKey(vehiclenameless))
                        {
                            UserTruckDictionary[vehiclenameless].Parts.Add(new UserCompanyTruckDataPart("truckdata"));
                            line++;

                            int accessoriescount = 0;
                            //string[] accessoriespool = null;

                            while (!tempSavefileInMemory[line].StartsWith("}"))
                            {
                                if (tempSavefileInMemory[line].StartsWith(" fuel_relative:"))
                                {
                                    UserTruckDictionary[vehiclenameless].Parts.Find(x => x.PartType == "truckdata").PartData.Add(tempSavefileInMemory[line]);
                                }
                                else

                                if (tempSavefileInMemory[line].StartsWith(" license_plate:"))
                                {
                                    UserTruckDictionary[vehiclenameless].Parts.Find(x => x.PartType == "truckdata").PartData.Add(tempSavefileInMemory[line]);
                                }
                                else

                                if (tempSavefileInMemory[line].StartsWith(" accessories:"))
                                {
                                    accessoriescount = int.Parse(tempSavefileInMemory[line].Split(new char[] { ' ' })[2]);

                                    //accessoriespool = new string[accessoriescount];
                                    line++;
                                    /*
                                    for (int i = 0; i < accessoriescount; i++)
                                    {
                                        accessoriespool[i] = tempSavefileInMemory[line].Split(new char[] { ' ' })[2];
                                        line++;
                                    }
                                    */
                                }

                                line++;
                            }

                            while (accessoriescount > 0)//(accessoriespool.Length > 0)
                            {
                                if (tempSavefileInMemory[line].StartsWith("vehicle_"))
                                {
                                    string accessorynameless = tempSavefileInMemory[line].Split(new char[] { ' ' })[2];
                                    bool paintadded = false;
                                    if (tempSavefileInMemory[line].StartsWith("vehicle_paint_job_accessory :"))
                                    {
                                        UserTruckDictionary[vehiclenameless].Parts.Add(new UserCompanyTruckDataPart("paintjob"));
                                        paintadded = true;
                                    }
                                    line++;

                                    List<string> tempPartData = new List<string>();

                                    while (!tempSavefileInMemory[line].StartsWith("}"))
                                    {
                                        tempPartData.Add(tempSavefileInMemory[line]);

                                        if (!paintadded)
                                            if (tempSavefileInMemory[line].StartsWith(" data_path:"))
                                            {
                                                chunkOfline = tempSavefileInMemory[line].Split(new char[] { ' ' });
                                                string truckpart = chunkOfline[2].Split(new char[] { '"' })[1];

                                                if (truckpart.Contains("data.sii"))
                                                    UserTruckDictionary[vehiclenameless].Parts.Add(new UserCompanyTruckDataPart("truckbrandname"));
                                                else if (truckpart.Contains("/chassis/"))
                                                {
                                                    UserTruckDictionary[vehiclenameless].Parts.Add(new UserCompanyTruckDataPart("chassis"));
                                                }
                                                else if (truckpart.Contains("/cabin/"))
                                                {
                                                    UserTruckDictionary[vehiclenameless].Parts.Add(new UserCompanyTruckDataPart("cabin"));
                                                }
                                                else if (truckpart.Contains("/engine/"))
                                                {
                                                    UserTruckDictionary[vehiclenameless].Parts.Add(new UserCompanyTruckDataPart("engine"));
                                                }
                                                else if (truckpart.Contains("/transmission/"))
                                                {
                                                    UserTruckDictionary[vehiclenameless].Parts.Add(new UserCompanyTruckDataPart("transmission"));
                                                }
                                                else if (truckpart.Contains("/f_tire/") || truckpart.Contains("/r_tire/"))
                                                {
                                                    UserTruckDictionary[vehiclenameless].Parts.Add(new UserCompanyTruckDataPart("tire"));
                                                }
                                                else
                                                    UserTruckDictionary[vehiclenameless].Parts.Add(new UserCompanyTruckDataPart("generalpart"));
                                            }
                                        line++;
                                    }

                                    if (paintadded)
                                        paintadded = false;
                                    UserTruckDictionary[vehiclenameless].Parts.Last().PartNameless = accessorynameless;
                                    UserTruckDictionary[vehiclenameless].Parts.Last().PartData = tempPartData;
                                    accessoriescount--;
                                }

                                //accessoriespool = accessoriespool.Where(val => val != accessorynameless).ToArray();
                                line++;
                            }
                        }
                        continue;
                    }

                    //find vehicles Trailer
                    /*
                    TrailerSearchStart:
                    if (tempSavefileInMemory[line].StartsWith("trailer :"))
                    {
                        chunkOfline = tempSavefileInMemory[line].Split(new char[] { ' ' });
                        string vehiclenameless = chunkOfline[2];

                        if (UserTrailerDictionary.ContainsKey(vehiclenameless))//UserTruckList.ContainsKey(vehiclenameless))
                        {
                            UserTrailerDictionary[vehiclenameless].Parts.Add(new UserCompanyTruckDataPart("trailerdata"));
                            line++;

                            int[] accessoriescount = new int[1];
                            string[] trailernamelessArray = new string[1];
                            int slavetrailerscount = 0;
                            //string[] accessoriespool = null;

                            while (!tempSavefileInMemory[line].StartsWith("}"))
                            {
                                if (tempSavefileInMemory[line].StartsWith(" cargo_mass:"))
                                {
                                    UserTrailerDictionary[vehiclenameless].Parts.Find(x => x.PartType == "trailerdata").PartData.Add(tempSavefileInMemory[line]);
                                }
                                else
                                if (tempSavefileInMemory[line].StartsWith(" cargo_damage:"))
                                {
                                    UserTrailerDictionary[vehiclenameless].Parts.Find(x => x.PartType == "trailerdata").PartData.Add(tempSavefileInMemory[line]);
                                }
                                else
                                if (tempSavefileInMemory[line].StartsWith(" license_plate:"))
                                {
                                    UserTrailerDictionary[vehiclenameless].Parts.Find(x => x.PartType == "trailerdata").PartData.Add(tempSavefileInMemory[line]);
                                }
                                else
                                if (tempSavefileInMemory[line].StartsWith(" slave_trailer:"))
                                {
                                    //UserTrailerDictionary[vehiclenameless].Parts.Find(x => x.PartType == "trailerdata").PartData.Add(tempSavefileInMemory[line]);
                                    if (tempSavefileInMemory[line].Contains("_nameless"))
                                    {
                                        slavetrailerscount++;
                                        Array.Resize(ref accessoriescount, slavetrailerscount);
                                        Array.Resize(ref trailernamelessArray, slavetrailerscount);

                                        UserTrailerDictionary[vehiclenameless].Parts.Add(new UserCompanyTruckDataPart("slavetrailer"));
                                        UserTrailerDictionary[vehiclenameless].Parts.Last().PartNameless = tempSavefileInMemory[line].Split(new char[] { ' ' })[2];
                                    }
                                    else
                                    {
                                        slavetrailerscount++;
                                        Array.Resize(ref accessoriescount, slavetrailerscount);
                                        Array.Resize(ref trailernamelessArray, slavetrailerscount);


                                        trailernamelessArray[slavetrailerscount - 1] = vehiclenameless;
                                    }
                                }
                                else
                                if (tempSavefileInMemory[line].StartsWith(" accessories:"))
                                {
                                    accessoriescount[slavetrailerscount]  = int.Parse(tempSavefileInMemory[line].Split(new char[] { ' ' })[2]);
                                    //accessoriespool = new string[accessoriescount];
                                    line++;
                                    /*
                                    for (int i = 0; i < accessoriescount; i++)
                                    {
                                        accessoriespool[i] = tempSavefileInMemory[line].Split(new char[] { ' ' })[2];
                                        line++;
                                    }
                                    /////
                                }
                                line++;
                            }

                            if (UserTrailerDictionary[vehiclenameless].Parts.Exists(x => x.PartType == "slavetrailer"))
                            {
                                while (!tempSavefileInMemory[line].StartsWith("trailer :"))
                                {
                                    line++;
                                }
                                goto TrailerSearchStart;
                            }
                            else
                            {
                                int trailerindex = accessoriescount.Length - 1;

                                while (accessoriescount[trailerindex] > 0)//(accessoriespool.Length > 0)
                                {
                                    if (tempSavefileInMemory[line].StartsWith("vehicle_"))
                                    {
                                        string accessorynameless = tempSavefileInMemory[line].Split(new char[] { ' ' })[2];
                                        bool paintadded = false;
                                        if (tempSavefileInMemory[line].StartsWith("vehicle_paint_job_accessory :"))
                                        {
                                            UserTrailerDictionary[trailernamelessArray[trailerindex]].Parts.Add(new UserCompanyTruckDataPart("paintjob"));
                                            paintadded = true;
                                        }
                                        line++;

                                        List<string> tempPartData = new List<string>();

                                        while (!tempSavefileInMemory[line].StartsWith("}"))
                                        {
                                            tempPartData.Add(tempSavefileInMemory[line]);

                                            if (!paintadded)
                                                if (tempSavefileInMemory[line].StartsWith(" data_path:"))
                                                {
                                                    chunkOfline = tempSavefileInMemory[line].Split(new char[] { ' ' });
                                                    string truckpart = chunkOfline[2].Split(new char[] { '"' })[1];

                                                    if (truckpart.Contains("data.sii"))
                                                        UserTrailerDictionary[trailernamelessArray[trailerindex]].Parts.Add(new UserCompanyTruckDataPart("trailerchassistype"));
                                                    else if (truckpart.Contains("/body/"))
                                                    {
                                                        UserTrailerDictionary[trailernamelessArray[trailerindex]].Parts.Add(new UserCompanyTruckDataPart("body"));
                                                    }
                                                    else if (truckpart.Contains("/chassis/"))
                                                    {
                                                        UserTrailerDictionary[trailernamelessArray[trailerindex]].Parts.Add(new UserCompanyTruckDataPart("chassis"));
                                                    }
                                                    else if (truckpart.Contains("/r_tire/"))
                                                    {
                                                        UserTrailerDictionary[trailernamelessArray[trailerindex]].Parts.Add(new UserCompanyTruckDataPart("tire"));
                                                    }
                                                    else
                                                        UserTrailerDictionary[trailernamelessArray[trailerindex]].Parts.Add(new UserCompanyTruckDataPart("generalpart"));
                                                }
                                            line++;
                                        }

                                        if (paintadded)
                                            paintadded = false;

                                        UserTrailerDictionary[trailernamelessArray[trailerindex]].Parts.Last().PartNameless = accessorynameless;
                                        UserTrailerDictionary[trailernamelessArray[trailerindex]].Parts.Last().PartData = tempPartData;
                                        accessoriescount[0]--;
                                    }

                                    //accessoriespool = accessoriespool.Where(val => val != accessorynameless).ToArray();
                                    line++;
                                    if (trailerindex != 0)
                                        trailerindex--;
                                }
                            }
                        }
                        continue;
                    }
                    */

                    //find existing jobs
                    if (tempSavefileInMemory[line].StartsWith("company : company.volatile."))
                    {
                        string sourcecity = tempSavefileInMemory[line].Split(new char[] { '.' })[3].Split(new char[] { ' ' })[0]; //Source city
                        string sourcecompany = tempSavefileInMemory[line].Split(new char[] { '.' })[2].Split(new char[] { ' ' })[0]; //Source company

                        int index = line + 1;
                        int numOfJobOffers = 0, numOfCargoOffers = 0, cargoseed = 0 ;
                        List<int> cargoseeds = new List<int>();
                        string deliveredTrailer = "";
                        bool CompanyJobStructureEnded = false;


                        while (!CompanyJobStructureEnded)
                        {
                            if (tempSavefileInMemory[index].StartsWith(" delivered_trailer:"))
                            {
                                deliveredTrailer = tempSavefileInMemory[index].Split(new char[] { ' ' })[2];
                            }
                            else
                            if (tempSavefileInMemory[index].StartsWith(" job_offer:")) //number of jobs in company
                            {
                                numOfJobOffers = int.Parse(tempSavefileInMemory[index].Split(new char[] { ' ' })[2]);

                                //update Company Max jobs in City
                                foreach (City tempcity in CitiesList.FindAll(x => x.CityName == sourcecity))
                                {
                                    tempcity.UpdateCompany(sourcecompany, numOfJobOffers);
                                }
                            }
                            else
                            if (tempSavefileInMemory[index].StartsWith(" cargo_offer_seeds:")) //number of cargo offers in company
                            {
                                numOfCargoOffers = int.Parse(tempSavefileInMemory[index].Split(new char[] { ' ' })[2]);

                                //Array.Resize(ref cargoseeds, numOfCargoOffers);
                                CitiesList.Find(x => x.CityName == sourcecity).UpdateCompanyCargoOfferCount(sourcecompany, numOfCargoOffers);
                            }
                            else
                            if (tempSavefileInMemory[index].StartsWith(" cargo_offer_seeds[")) //number of cargo offers in company
                            {
                                cargoseed = int.Parse(tempSavefileInMemory[index].Split(new char[] { ' ' })[2]);

                                cargoseeds.Add(cargoseed);
                            }
                            else
                            if (tempSavefileInMemory[index].StartsWith("}"))
                            {
                                CitiesList.Find(x => x.CityName == sourcecity).Companies.Find(x => x.CompanyName == sourcecompany).CragoSeeds = cargoseeds.ToArray();
                                CompanyJobStructureEnded = true;
                            }
                            index++;
                        }

                        destinationcity = null;
                        destinationcompany = null;
                        string distance = null;
                        string ferrytime = null;
                        string ferryprice = null;
                        string[] strArrayTarget = null;

                        if ((numOfJobOffers != 0) && (deliveredTrailer == "null"))
                        {
                            int cargotype = 0, units_count = 0;
                            string cargo = "", trailervariant = "", trailerdefinition = "";
                            index++;

                            for (int i = 0; i < numOfJobOffers; i++)
                            {
                                bool JobOfferDataEnded = false;

                                searforstart:
                                //Find Job offer data
                                if (tempSavefileInMemory[index].StartsWith("job_offer_data :"))//&& tempSavefileInMemory[line + 1].StartsWith(" cargo: cargo"))
                                {
                                    index++;

                                    if (!tempSavefileInMemory[index].StartsWith(" target: \"\""))
                                    {
                                        while (!JobOfferDataEnded)
                                        {

                                            if (tempSavefileInMemory[index].StartsWith(" target:")) //Destination city
                                            {
                                                strArrayTarget = tempSavefileInMemory[index].Split(new char[] { '"' });
                                            }
                                            else
                                            if (tempSavefileInMemory[index].StartsWith(" shortest_distance_km:")) //Distance
                                            {
                                                distance = tempSavefileInMemory[index].Split(new char[] { ' ' })[2];
                                            }
                                            else
                                            if (tempSavefileInMemory[index].StartsWith(" ferry_time:")) //Ferry time
                                            {
                                                ferrytime = tempSavefileInMemory[index].Split(new char[] { ' ' })[2];
                                            }
                                            else
                                            if (tempSavefileInMemory[index].StartsWith(" ferry_price:")) //ferry price
                                            {
                                                ferryprice = tempSavefileInMemory[index].Split(new char[] { ' ' })[2];
                                            }
                                            else
                                            if (tempSavefileInMemory[index].StartsWith(" cargo: cargo."))
                                            {
                                                //Getting Cargo name
                                                cargo = tempSavefileInMemory[index].Split(new char[] { '.' })[1];//""; 
                                            }
                                            else
                                            //cargotype
                                            //Company Truck
                                            if (tempSavefileInMemory[index].StartsWith(" company_truck:"))
                                            {
                                                string[] LineArray = tempSavefileInMemory[index].Split(new char[] { ' ' });

                                                if (tempSavefileInMemory[index].Contains("\"heavy"))
                                                {
                                                    cargotype = 1;
                                                }
                                                else if (tempSavefileInMemory[index].Contains("\"double"))
                                                {
                                                    cargotype = 2;
                                                }

                                                CompanyTruckList.Add(new CompanyTruck(LineArray[2], cargotype));

                                                //Normal, Heavy or Double cargo?
                                                if ((cargotype == 0) || (cargotype == 2)) //Normal or Double
                                                {
                                                    if (CargoesList.Exists(x => (x.CargoName == (cargo)) && (x.CargoType == 1)))
                                                    {
                                                        CargoesList.RemoveAt(CargoesList.FindIndex(x => (x.CargoName == (cargo)) && (x.CargoType == 1))); //Remove Heavy from cargo list
                                                    }
                                                }
                                                else if (cargotype == 1) //Heavy
                                                {
                                                    if (CargoesList.Exists(x => (x.CargoName == cargo) && ((x.CargoType == 0) || (x.CargoType == 2))))
                                                    {
                                                        CargoesList.RemoveAt(CargoesList.FindIndex(x => (x.CargoName == (cargo)) && ((x.CargoType == 0) || (x.CargoType == 2)))); //Remove from normal cargo
                                                    }
                                                }
                                            }
                                            else
                                            //Find cargo trailer variant
                                            if (tempSavefileInMemory[index].StartsWith(" trailer_variant:"))
                                            {
                                                trailervariant = tempSavefileInMemory[index].Split(new char[] { ' ' })[2];
                                                /*
                                                //Fill Cargo list
                                                //Add 
                                                if (CargoesList.Where(x => (x.CargoName == cargo && x.CargoType == cargotype)).Count() == 0)
                                                {
                                                    CargoesList.Add(new Cargo(cargo, cargotype, variant));
                                                }
                                                else
                                                {
                                                    CargoesList[CargoesList.FindIndex(x => x.CargoName == cargo && x.CargoType == cargotype)].CargoVariant[variant] = true;
                                                }
                                                //END
                                                */
                                            }
                                            else
                                            //Find cargo trailer definition
                                            if (tempSavefileInMemory[index].StartsWith(" trailer_definition:"))
                                            {
                                                //variant = int.Parse(tempSavefileInMemory[index].Split(new char[] { ' ' })[2]);
                                                trailerdefinition = tempSavefileInMemory[index].Split(new char[] { ' ' })[2];
                                                //END
                                            }
                                            else
                                            if(tempSavefileInMemory[index].StartsWith(" units_count:"))
                                            {
                                                units_count = int.Parse(tempSavefileInMemory[index].Split(new char[] { ' ' })[2]);
                                            }
                                            /*
                                            //Find cargo variant
                                            if (tempSavefileInMemory[index].StartsWith(" variant:"))
                                            {
                                                variant = int.Parse(tempSavefileInMemory[index].Split(new char[] { ' ' })[2]);

                                                //Fill Cargo list
                                                //Add 
                                                if (CargoesList.Where(x => (x.CargoName == cargo && x.CargoType == cargotype)).Count() == 0)
                                                {
                                                    CargoesList.Add(new Cargo(cargo, cargotype, variant));
                                                }
                                                else
                                                {
                                                    CargoesList[CargoesList.FindIndex(x => x.CargoName == cargo && x.CargoType == cargotype)].CargoVariant[variant] = true;
                                                }
                                                //END
                                            }
                                            */
                                            else
                                            if (tempSavefileInMemory[index].StartsWith("}"))
                                            {
                                                //Dictionary<string, Dictionary<string, int>> tempDefVar = new Dictionary<string, Dictionary<string, int>>();
                                                Cargo tempCargo = CargoesList.Find(x => x.CargoName == cargo && x.CargoType == cargotype);
                                                //Dictionary<string, Dictionary<string, int>> tempDefVar = new Dictionary<string, Dictionary<string, int>>();


                                                if (tempCargo == null)
                                                {
                                                    Dictionary<string, Dictionary<string, int>> DefVar = new Dictionary<string, Dictionary<string, int>>();
                                                    Dictionary<string, int> Variant = new Dictionary<string, int>();

                                                    Variant.Add(trailervariant, units_count);
                                                    DefVar.Add(trailerdefinition, Variant);

                                                    CargoesList.Add(new Cargo(cargo, cargotype, DefVar));
                                                }
                                                else
                                                {
                                                    Dictionary<string, Dictionary<string, int>> tempDefVar = tempCargo.CargoVarDef;
                                                    if (!tempDefVar.ContainsKey(trailerdefinition))
                                                    {
                                                        Dictionary<string, int> Variant = new Dictionary<string, int>();
                                                        Variant.Add(trailervariant, units_count);

                                                        CargoesList.Find(x => x.CargoName == cargo && x.CargoType == cargotype).CargoVarDef.Add(trailerdefinition, Variant);
                                                        Dictionary<string, int> tempVar = CargoesList.Find(x => x.CargoName == cargo && x.CargoType == cargotype).CargoVarDef[trailerdefinition];
                                                    }
                                                    else
                                                    {
                                                        if (!tempDefVar[trailerdefinition].ContainsKey(trailervariant))
                                                        {
                                                            CargoesList.Find(x => x.CargoName == cargo && x.CargoType == cargotype).CargoVarDef[trailerdefinition].Add(trailervariant, units_count);
                                                            //CargoesList.Add(new Cargo(cargo, cargotype, tempDefVar));
                                                        }
                                                        else
                                                        {
                                                            CargoesList.Find(x => x.CargoName == cargo && x.CargoType == cargotype).CargoVarDef[trailerdefinition][trailervariant] = units_count;
                                                        }
                                                    }
                                                }
                                                //tempDefVar = tempCargo.CargoVarDef;//[trailerdefinition];
                                                //Dictionary<string, int> tempVar = new Dictionary<string, int>();
                                                //tempVar.Add(trailervariant, units_count);



                                                //if (!tempDefVar.ContainsKey(trailerdefinition))
                                                //{
                                                //    CargoesList.Add(new Cargo(cargo, cargotype, DefVar));
                                                //}
                                                //else


                                                //tempDefVar.Add(trailerdefinition, tempVar);

                                                //Fill Cargo list
                                                //Add 
                                                /*
                                                if (CargoesList.Where(x => (x.CargoName == cargo && x.CargoType == cargotype)).Count() == 0)
                                                {
                                                    //CargoesList.Add(new Cargo(cargo, cargotype, variant));
                                                    CargoesList.Add(new Cargo(cargo, cargotype, tempDefVar));
                                                }
                                                else
                                                {
                                                    //CargoesList[CargoesList.FindIndex(x => x.CargoName == cargo && x.CargoType == cargotype)].CargoVariant[variant] = true;
                                                }
                                                */
                                                //index++;
                                                JobOfferDataEnded = true;
                                            }

                                            index++;
                                        }
                                    }
                                    else
                                    {
                                        index = index + 13;
                                    }
                                }
                                else
                                {
                                    index++;
                                    goto searforstart;
                                }

                                if (((strArrayTarget != null) && (strArrayTarget.Length == 3)) && (strArrayTarget[1] != ""))
                                {
                                    destinationcity = strArrayTarget[1].Split(new char[] { '.' })[1];
                                    destinationcompany = strArrayTarget[1].Split(new char[] { '.' })[0];

                                    DistancesTable.Rows.Add(sourcecity, sourcecompany, destinationcity, destinationcompany, int.Parse(distance), int.Parse(ferrytime), int.Parse(ferryprice));
                                }
                            }
                        }

                        continue;
                    }

                    if (tempSavefileInMemory[line].StartsWith("economy_event_queue :"))
                    {
                        int newSize = int.Parse(tempSavefileInMemory[line + 1].Split(new char[] { ' ' })[2]); // queue size
                        Array.Resize(ref EconomyEventQueueList, newSize);
                        EconomyEventsTable = new string[newSize, 5];

                        continue;
                    }

                    if (tempSavefileInMemory[line].StartsWith(" data[") && tempSavefileInMemory[line].Contains("nameless"))
                    {
                        EconomyEventsTable[economyEventQueueIndex, 0] = tempSavefileInMemory[line].Split(new char[] { ' ' })[2]; //save nameless string full
                        economyEventQueueIndex++;

                        continue;
                    }

                    //Economy events
                    if (tempSavefileInMemory[line].StartsWith("economy_event : "))
                    {
                        for (EconomyEventColumn = 1; EconomyEventColumn < 5; EconomyEventColumn++)
                        {
                            EconomyEventsTable[EconomyEventline, EconomyEventColumn] = tempSavefileInMemory[line + EconomyEventColumn - 1];

                            if (EconomyEventColumn == 2)
                            {
                                EconomyEventsTable[EconomyEventline, EconomyEventColumn] = EconomyEventsTable[EconomyEventline, EconomyEventColumn].Split(new char[] { ' ' })[2]; //time
                            }
                        }
                        EconomyEventline++;

                        continue;
                    }
                }
                catch (Exception ex)
                {
                    ShowStatusMessages("i", "error_exception");
                    MessageBox.Show(ex.Message, "Exception.\r\n" + line.ToString() + " | " + tempSavefileInMemory[line], MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            //end scan

            namelessList.Sort();
            namelessList = namelessList.Distinct().ToList();
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

            //GetDataFrom Database

            //GetDataFromDatabase("CargoesTable", GameType);
            GetDataFromDatabase("CitysTable", GameType);
            GetDataFromDatabase("CompaniesTable", GameType);
            GetDataFromDatabase("TrucksTable", GameType);

            //Compare Data to Database
            if (CargoesListDB.Count() > 0)
            {

                CargoComparer CCaad = new CargoComparer();
                CargoesListDiff = CargoesList.Except(CargoesListDB, CCaad).ToList();
                Predicate<Cargo> tempCargoPred = null;

                foreach (Cargo tempCargo in CargoesListDiff)
                {
                    tempCargoPred = x => x.CargoName == tempCargo.CargoName && x.CargoType == tempCargo.CargoType;

                    int listDBindex = CargoesListDB.FindIndex(tempCargoPred);
                    int listDIFFindex = CargoesListDiff.FindIndex(tempCargoPred);

                    if (listDBindex != -1)
                    {
                        foreach(KeyValuePair<string, Dictionary<string, int>> cdef in tempCargo.CargoVarDef)
                        {
                            Dictionary<string, int> tempdef = new Dictionary<string, int>();
                            foreach(KeyValuePair < string,int> cvar in cdef.Value)
                            {
                                tempdef.Add(cvar.Key, cvar.Value);
                            }
                            CargoesListDB[listDBindex].CargoVarDef.Add(cdef.Key, tempdef);
                        }
                        /*
                        int i = -1;
                        foreach (bool var in tempCargo.CargoVariant)
                        {
                            i++;
                            if (var)
                            {
                                CargoesListDB[listDBindex].CargoVariant[i] = var;
                            }
                        }
                        */
                        //CargoesListDiff[listDIFFindex].CargoVariant = CargoesListDB[listDBindex].CargoVariant;
                        CargoesListDiff[listDIFFindex].CargoVarDef = CargoesListDB[listDBindex].CargoVarDef;
                    }
                    else
                    {
                        //CargoesListDB.Add(new Cargo(tempCargo.CargoName, tempCargo.CargoType, tempCargo.CargoVariant));
                        CargoesListDB.Add(new Cargo(tempCargo.CargoName, tempCargo.CargoType, tempCargo.CargoVarDef));
                    }
                }

            }
            else
            {
                CargoesListDB = CargoesList;
                CargoesListDiff = CargoesList;
            }

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
            //GetCompaniesCargoInOut();
            worker.ReportProgress(90);
            GetAllDistancesFromDB();
            worker.ReportProgress(100);

            //ToggleVisibility(true); //set controls to visible

            ShowStatusMessages("i", "message_operation_finished");
            LogWriter("Prepare ended");
        }

        private void CheckSaveInfoData()
        {
            string[] chunkOfline;

            for (int line = 0; line < tempInfoFileInMemory.Length; line++)
            {
                if (tempInfoFileInMemory[line].StartsWith(" version:"))
                {
                    chunkOfline = tempInfoFileInMemory[line].Split(new char[] { ' ' });
                    SavefileVersion = int.Parse(chunkOfline[2]);
                    continue; //searching only version
                }

                if (tempInfoFileInMemory[line].StartsWith(" dependencies["))
                {
                    chunkOfline = tempInfoFileInMemory[line].Split(new char[] { '"' });
                    //SavefileDependencies.Add(chunkOfline[1]);
                    continue;
                }
            }

            if (SavefileVersion == 0)
                ShowStatusMessages("e", "error_save_version_not_detected");
        }

        private string GetCustomSaveFilename(string _tempSaveFilePath)
        {
            string chunkOfline;

            string tempSiiInfoPath = _tempSaveFilePath + @"\info.sii";
            string[] tempFile = null;
            //////
            if (!File.Exists(tempSiiInfoPath))
            {
                LogWriter("File does not exist in " + tempSiiInfoPath);
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
                        tempFile = DecodeFile(tempSiiInfoPath);

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
                    LogWriter("Could not read: " + tempSiiInfoPath);
                }

                if ((tempFile == null) || (tempFile[0] != "SiiNunit"))
                {
                    LogWriter("Wrongly decoded Info file or wrong file format");
                    ShowStatusMessages("e", "error_file_not_decoded");
                }
                else if (tempFile != null)
                {
                    for (int line = 0; line < tempFile.Length; line++)
                    {
                        if (tempFile[line].StartsWith(" name:"))
                        {
                            chunkOfline = tempFile[line];
                            //SavefileVersion = int.Parse(chunkOfline[2]);
                            return chunkOfline.Substring(7);
                        }
                    }
                }
            }
            //////
            return "<!>Error<!>";
        }

        private void CheckProfileInfoData()
        {
            string[] chunkOfline;

            for (int line = 0; line < tempProfileFileInMemory.Length; line++)
            {
                if (tempProfileFileInMemory[line].StartsWith(" company_name:"))
                {
                    chunkOfline = tempProfileFileInMemory[line].Split(new char[] { ' ' });
                    PlayerProfileData.CompanyName = chunkOfline[2];
                    continue; //searching
                }

                if (tempProfileFileInMemory[line].StartsWith(" cached_discovery:"))
                {
                    chunkOfline = tempProfileFileInMemory[line].Split(new char[] { ' ' });
                    line = +int.Parse(chunkOfline[2]);
                    continue; //searching
                }

                if (tempProfileFileInMemory[line].StartsWith(" creation_time:"))
                {
                    chunkOfline = tempProfileFileInMemory[line].Split(new char[] { ' ' });
                    PlayerProfileData.CreationTime = int.Parse(chunkOfline[2]);
                    continue; //searching
                }
            }
        }

        private void PrepareVisitedCities()
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
                
        }

        private void PrepareEvents()
        {
            for (int i = 0; i < EconomyEventUnitLinkStringList.Length; i++)
            {
                int j = 0;
                while (j < EconomyEventsTable.GetLength(0))
                {
                    if ((EconomyEventUnitLinkStringList[i] == EconomyEventsTable[j, 3]) && (EconomyEventsTable[j, 4] == " param: 0"))
                    {
                        EconomyEventsTable[j, 2] = (InGameTime + (((i + 1) * ProgSettingsV.JobPickupTime) * 60)).ToString(); //time
                    }
                    j++;
                }
            }

            string[,] tempArray = new string[1, 5];

            //Sort by time
            for (int i = 1; i < EconomyEventsTable.GetLength(0); i++)
            {
                int k = 0;
                while (k < 5)
                {
                    tempArray[0, k] = EconomyEventsTable[i, k];
                    k++;
                }
                for (int j = i; j > 0; j--)
                {
                    if (int.Parse(tempArray[0, 2]) < int.Parse(EconomyEventsTable[j - 1, 2]))
                    {
                        for (k = 0; k < 5; k++)
                        {
                            EconomyEventsTable[j, k] = EconomyEventsTable[j - 1, k];
                            EconomyEventsTable[j - 1, k] = tempArray[0, k];
                        }
                    }
                }
            }
        }

        private void AddCargo()
        {
            if ((((comboBoxSourceCity.SelectedIndex < 0) || (comboBoxSourceCompany.SelectedIndex < 0)) || ((comboBoxDestinationCity.SelectedIndex < 0) || (comboBoxDestinationCompany.SelectedIndex < 0))) || (comboBoxCargoList.SelectedIndex < 0) || comboBoxUrgency.SelectedIndex < 0)
            {
                LogWriter("Missing selection of Source, Destination or Cargo settings");
                ShowStatusMessages("e", "error_job_parameters_not_filled");
            }
            else
            {
                ShowStatusMessages("i", "");

                string SourceCity = comboBoxSourceCity.SelectedValue.ToString();
                string SourceCompany = comboBoxSourceCompany.SelectedValue.ToString();
                string DestinationCity = comboBoxDestinationCity.SelectedValue.ToString();
                string DestinationCompany = comboBoxDestinationCompany.SelectedValue.ToString();
                string Cargo = comboBoxCargoList.SelectedValue.ToString();

                string SourceCityName = comboBoxSourceCity.Text;
                string SourceCompanyName = comboBoxSourceCompany.Text;
                string DestinationCityName = comboBoxDestinationCity.Text;
                string DestinationCompanyName = comboBoxDestinationCompany.Text;
                string CargoName = comboBoxCargoList.Text;

                //comboBoxCargoList

                string[] route = RouteList.GetRouteData(SourceCity, SourceCompany, DestinationCity, DestinationCompany);
                string distance = route[4];
                string FerryTime = route[5];
                string FerryPrice = route[6];

                if (FerryTime == "-1")
                {
                    FerryTime = "0";
                }

                if (JobsAmountAdded == 0)
                {
                    LoopStartCity = SourceCity;
                    LoopStartCompany = SourceCompany;
                }

                JobsAmountAdded++;

                Array.Resize(ref JobsListAdded, JobsAmountAdded);
                Array.Resize(ref ListSavefileCompanysString, JobsAmountAdded);
                Array.Resize(ref EconomyEventUnitLinkStringList, JobsAmountAdded);

                string TruckName = "";

                int CargoType = -1;

                if (comboBoxCargoList.Text.Contains("[H]"))
                    CargoType = 1;
                else if (comboBoxCargoList.Text.Contains("[D]"))
                    CargoType = 2;
                else
                    CargoType = 0;
                
                List<CompanyTruck> CompanyTruckType = CompanyTruckListDB.Where(x => x.Type == CargoType).ToList();
                TruckName = CompanyTruckType[RandomValue.Next(CompanyTruckType.Count())].TruckName;

                //int variant = 0;

                int TrueDistance = (int)(int.Parse(distance) * ProgSettingsV.TimeMultiplier);
                
                Cargo cargo = CargoesList.Find(x => x.CargoName == Cargo && x.CargoType == CargoType);

                /*
                int randvar = 0;
                do
                {
                    randvar = RandomValue.Next(5);
                    if (cargo.CargoVariant[randvar])
                    {
                        variant = randvar;
                        break;
                    }
                }
                while (true);
                */
                KeyValuePair<string, int> TrailerVariant = new KeyValuePair<string, int>();
                string TrailerDefinition = "";
                //string UnitsCount = "";

                int randvar = 0;
                randvar = RandomValue.Next(cargo.CargoVarDef.Count());

                KeyValuePair<string, Dictionary<string,int>> tempDefVar = cargo.CargoVarDef.ElementAt(randvar);
                //List<string> tempDefVar = RandomValues(cargo.CargoVarDef).Take(1) as List<string>;

                TrailerDefinition = tempDefVar.Key;
                randvar = RandomValue.Next(tempDefVar.Value.Count());
                TrailerVariant = tempDefVar.Value.ElementAt(randvar);//.Value[randvar];

                string Urgency = comboBoxUrgency.SelectedValue.ToString();

                ListSavefileCompanysString[JobsAmountAdded - 1] = "company : company.volatile." + SourceCompany + "." + SourceCity + " {";
                EconomyEventUnitLinkStringList[JobsAmountAdded - 1] = " unit_link: company.volatile." + SourceCompany + "." + SourceCity;

                int ExpirationTime = InGameTime + (JobsAmountAdded * (ProgSettingsV.JobPickupTime * 60));

                JobsListAdded[JobsAmountAdded - 1] = string.Concat(new object[] {
                    " target: \"", DestinationCompany, ".", DestinationCity, "\"",
                    "\n expiration_time: ", ExpirationTime.ToString(),
                    "\n urgency: ", Urgency,
                    "\n shortest_distance_km: ", TrueDistance,
                    "\n ferry_time: ", FerryTime,
                    "\n ferry_price: ", FerryPrice,
                    "\n cargo: cargo.", Cargo,
                    "\n company_truck: ", TruckName,
                    "\n trailer_variant: ", TrailerVariant.Key,
                    "\n trailer_definition: ", TrailerDefinition,
                    "\n units_count: ", TrailerVariant.Value
                 });

                LogWriter("Job from:" + SourceCityName + " | " + SourceCompanyName+ " To " + DestinationCityName + " | " + DestinationCompanyName + "\n-----------\n" + JobsListAdded[JobsAmountAdded - 1] + "\n-----------");

                buttonWriteSave.Enabled = true;
                buttonClearJobList.Enabled = true;

                listBoxAddedJobs.Items.Add(new JobAdded(SourceCity, SourceCompany, DestinationCity, DestinationCompany, Cargo, int.Parse(Urgency), CargoType, TrueDistance, int.Parse(FerryTime), int.Parse(FerryPrice)));

                if (distance != "11111")
                {
                    JobsTotalDistance += int.Parse(distance);
                }

                labelFreightMarketDistanceNumbers.Text = (JobsTotalDistance * DistanceMultiplier).ToString() + " " + ProgSettingsV.DistanceMes; //km";

                comboBoxSourceCity.SelectedValue = comboBoxDestinationCity.SelectedValue;
                comboBoxSourceCompany.SelectedValue = comboBoxDestinationCompany.SelectedValue;


                int looptest;

                if (JobsAmountAdded == 1)
                {
                    looptest = 1;
                }
                else
                {
                    looptest = JobsAmountAdded + 1;
                }

                if (ProgSettingsV.LoopEvery != 0 && (looptest % ProgSettingsV.LoopEvery) == 0)
                {
                    try
                    {
                        comboBoxDestinationCity.SelectedValue = LoopStartCity; //.SelectedIndex = comboBoxDestinationCity.Items.IndexOf(LoopStartCity);
                        comboBoxDestinationCompany.SelectedValue = LoopStartCompany;//.SelectedIndex = comboBoxDestinationCompany.Items.IndexOf(LoopStartCompany);

                        //DestinationCity = LoopStartCity;
                        //DestinationCompany = LoopStartCompany;
                    }
                    catch
                    {
                        ShowStatusMessages("e", "error_could_not_complete_jobs_loop");
                    }
                }
                else
                {
                    triggerDestinationCitiesUpdate();
                }
            }
        }

        public IEnumerable<TValue> RandomValues<TKey, TValue>(IDictionary<TKey, TValue> dict)
        {
            Random rand = new Random();
            List<TValue> values = Enumerable.ToList(dict.Values);
            int size = dict.Count;
            while (true)
            {
                yield return values[rand.Next(size)];
            }
        }

        private void LoadAdditionalCargo()
        {
            //NEED REWRITE
            try
            {
                string[] strArray = File.ReadAllLines(Directory.GetCurrentDirectory() + "/extra_cargo.txt");
                for (int i = 0; i < strArray.Length; i++)
                {
                    if (strArray[i].StartsWith("cargo."))
                    {
                        string str = "";
                        string[] strArray2 = strArray[i].Split(new char[] { '.' });
                        for (int j = 1; j < strArray2.Length; j++)
                        {
                            if (j > 1)
                            {
                                str = str + ".";
                            }
                            str = str + strArray2[j];
                        }
                        //CargoesList.Add(new Cargo(str, 0, 0));
                    }
                }
            }
            catch
            {
            }
        }

        private void CreateDatabase()
        {
            string connectionString;

            string fileName = "Database.sdf";

            if (!File.Exists(fileName))
            {
                ShowStatusMessages("e", "message_database_missing_creating_db");

                connectionString = string.Format("DataSource ='{0}';", fileName);

                SqlCeEngine Engine = new SqlCeEngine(connectionString);

                Engine.CreateDatabase();

                ShowStatusMessages("i", "message_database_created");

                CreateDatabaseStructure();
            }
        }

        private void CreateDatabaseStructure()
        {
            if (DBconnection.State == ConnectionState.Closed)
            {
                DBconnection.Open();
            }

            ShowStatusMessages("e", "message_database_missing_creating_db_structure");

            SqlCeCommand cmd;

            string sql = "CREATE TABLE GameTypesTable (ID_gameType INT IDENTITY(1,1) PRIMARY KEY, GameTypeName NVARCHAR(32) NOT NULL);";

            sql += "CREATE TABLE CitysTable (ID_city INT IDENTITY(1,1) PRIMARY KEY, CityName NVARCHAR(32) NOT NULL, GameTypeID INT NOT NULL);";
            sql += "ALTER TABLE CitysTable ADD FOREIGN KEY(GameTypeID) REFERENCES GameTypesTable(ID_gameType);";

            sql += "CREATE TABLE CompaniesTable (ID_company INT IDENTITY(1,1) PRIMARY KEY, CompanyName NVARCHAR(32) NOT NULL, GameTypeID INT NOT NULL);";
            sql += "ALTER TABLE CompaniesTable ADD FOREIGN KEY(GameTypeID) REFERENCES GameTypesTable(ID_gameType);";

            sql += "CREATE TABLE CargoesTable (ID_cargo INT IDENTITY(1,1) PRIMARY KEY, CargoName NVARCHAR(32) NOT NULL, CargoType INT NOT NULL, GameTypeID INT NOT NULL);";
            sql += "ALTER TABLE CargoesTable ADD FOREIGN KEY(GameTypeID) REFERENCES GameTypesTable(ID_gameType);";

            sql += "CREATE TABLE TrailerDefinitionTable (ID_trailerD INT IDENTITY(1,1) PRIMARY KEY, TrailerDefinitionName NVARCHAR(64) NOT NULL);";
            //sql += "ALTER TABLE TrailerDefinitionTable ADD FOREIGN KEY(CargoID) REFERENCES CargoesTable(ID_cargo);";

            sql += "CREATE TABLE CargoesToTrailerDefinitionTable (ID_trailerCtD INT IDENTITY(1,1) PRIMARY KEY, CargoID INT NOT NULL, TrailerDefinitionID INT NOT NULL);";
            sql += "ALTER TABLE CargoesToTrailerDefinitionTable ADD FOREIGN KEY(CargoID) REFERENCES CargoesTable(ID_cargo);";
            sql += "ALTER TABLE CargoesToTrailerDefinitionTable ADD FOREIGN KEY(TrailerDefinitionID) REFERENCES TrailerDefinitionTable(ID_trailerD);";

            sql += "CREATE TABLE TrailerVariantTable (ID_trailerV INT IDENTITY(1,1) PRIMARY KEY, TrailerVariantName NVARCHAR(64) NOT NULL, CargoUnitsCount INT NOT NULL);";
            //sql += "ALTER TABLE TrailerVariantTable ADD FOREIGN KEY(TrailerDefinitionID) REFERENCES TrailerDefinitionTable(ID_trailerD);";

            sql += "CREATE TABLE TrailerDefinitionToTrailerVariantTable (ID_trailerDtV INT IDENTITY(1,1) PRIMARY KEY, TrailerDefinitionID INT NOT NULL, TrailerVariantID INT NOT NULL);";
            sql += "ALTER TABLE TrailerDefinitionToTrailerVariantTable ADD FOREIGN KEY(TrailerDefinitionID) REFERENCES TrailerDefinitionTable(ID_trailerD);";
            sql += "ALTER TABLE TrailerDefinitionToTrailerVariantTable ADD FOREIGN KEY(TrailerVariantID) REFERENCES TrailerVariantTable(ID_trailerV);";

            sql += "CREATE TABLE TrucksTable (ID_truck INT IDENTITY(1,1) PRIMARY KEY, TruckName NVARCHAR(64) NOT NULL, TruckType TINYINT NOT NULL, GameTypeID INT NOT NULL);";
            sql += "ALTER TABLE TrucksTable ADD FOREIGN KEY(GameTypeID) REFERENCES GameTypesTable(ID_gameType);";

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


            sql += "CREATE TABLE tempBulkDistancesTable (ID_Distance INT IDENTITY(1,1) PRIMARY KEY, SourceCity NVARCHAR(32) NOT NULL, SourceCompany NVARCHAR(32) NOT NULL, " +
                "DestinationCity NVARCHAR(32) NOT NULL, DestinationCompany NVARCHAR(32) NOT NULL, Distance INT NOT NULL, FerryTime INT NOT NULL, FerryPrice INT NOT NULL);";

            sql += "CREATE TABLE tempDistancesTable (ID_Distance INT IDENTITY(1,1) PRIMARY KEY, SourceCityID INT NOT NULL, SourceCompanyID INT NOT NULL, " +
                "DestinationCityID INT NOT NULL, DestinationCompanyID INT NOT NULL, Distance INT NOT NULL, FerryTime INT NOT NULL, FerryPrice INT NOT NULL);";
            string[] linesArray = sql.Split(';');

            foreach (string sqlline in linesArray)
            {
                if (sqlline != "")
                {
                    cmd = new SqlCeCommand(sqlline, DBconnection);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        ShowStatusMessages("i", "message_database_created");
                    }
                    catch (SqlCeException sqlexception)
                    {
                        ShowStatusMessages("i", "error_sql_exception");
                        MessageBox.Show(sqlexception.Message, "SQL Exception.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        ShowStatusMessages("i", "error_exception");
                        MessageBox.Show(ex.Message, "Exception.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            DBconnection.Close();
        }

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
                ShowStatusMessages("i", "error_sql_exception");
                MessageBox.Show(sqlexception.Message, "SQL Exception.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                ShowStatusMessages("i", "error_exception");
                MessageBox.Show(ex.Message, "Exception.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                DBconnection.Close();
            }
        }

        internal void ClearDatabase()
        {
            string[] deletearray = { "DistancesTable", "CompaniesCargoTable", "CompaniesInCitysTable", "TrucksTable", "CargoesTable", "TrailerVariantTable", "TrailerDefinitionTable", "CompaniesTable", "CitysTable", "GameTypesTable" };

            foreach (string droptable in deletearray)
            {
                UpdateDatabase("DELETE FROM " + droptable);
            }

            LogWriter("Database Cleared");

            CreateDatabaseStructure();
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
                ShowStatusMessages("i", "error_sql_exception");
                MessageBox.Show(sqlexception.Message, "SQL Exception.", MessageBoxButtons.OK, MessageBoxIcon.Error);

                LogWriter("Getting Data went wrong");
            }
            catch (Exception ex)
            {
                ShowStatusMessages("i", "error_exception");
                MessageBox.Show(ex.Message, "Exception.", MessageBoxButtons.OK, MessageBoxIcon.Error);

                LogWriter("Getting Data went wrong");
            }

            LogWriter("Loaded " + RouteList.CountItems() + " routes from DataBase");
        }

        private void AddDistances_DataTableToDB_Bulk (DataTable reader)//(bool keepNulls, DataTable reader)
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

            while (sqlreader.Read())
            {
                string updatecommandText = "UPDATE [DistancesTable] SET Distance = '" + sqlreader["Distance"].ToString() + "', " +
                    "FerryTime = '" + sqlreader["FerryTime"].ToString() + "', " +
                    "FerryPrice = '" + sqlreader["FerryPrice"].ToString() + "' " +
                    "WHERE SourceCityID = '" + sqlreader["SourceCityID"].ToString() + "' "+
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
                catch(SqlCeException sqlexception)
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
            }
            DBconnection.Close();

            UpdateDatabase("DELETE FROM [tempBulkDistancesTable]");
            UpdateDatabase("DELETE FROM [tempDistancesTable]");

            DistancesTable.Clear();

            SqlCeEngine DBengine = new SqlCeEngine(DBconnection.ConnectionString);
            DBengine.Shrink();
        }

        private void InsertDataIntoDatabase(string _targetTable)
        {
            if (_targetTable == "GameTypesTable")
            {
                string SQLCommandCMD = "UPDATE [GameTypesTable] SET [GameTypesTable].GameTypeName = '" + GameType + "' WHERE [GameTypesTable].GameTypeName = '" + GameType + "'";

                DBconnection.Open();

                SqlCeCommand command = DBconnection.CreateCommand();
                command.CommandText = SQLCommandCMD;

                int numberOfRecords = command.ExecuteNonQuery();

                if (numberOfRecords == 0)
                {
                    command.CommandText = "INSERT INTO [GameTypesTable] (GameTypeName) VALUES ('" + GameType + "')";
                    command.ExecuteNonQuery();
                }

                DBconnection.Close();
            }
            else
            {
                string SQLCommandGameType = "SELECT ID_gameType FROM GameTypesTable WHERE GameTypeName = '" + GameType + "'";

                DBconnection.Open();
                SqlCeDataReader reader = new SqlCeCommand(SQLCommandGameType, DBconnection).ExecuteReader();
                int GameTypeID = 0;

                while (reader.Read())
                {
                    GameTypeID = (int)reader.GetValue(0);
                }

                reader.Close();
                DBconnection.Close();

                switch (_targetTable)
                {
                    case "CargoesTable":
                        {
                            if (CargoesListDiff != null && CargoesListDiff.Count() > 0)
                            {
                                string SQLCommandCMD = "";

                                foreach (Cargo tempitem in CargoesListDiff)
                                {
                                    SqlCeCommand command = DBconnection.CreateCommand();

                                    string updatecommandText = "UPDATE [CargoesTable] SET CargoName = '" + tempitem.CargoName + "', " +
                                        "CargoType = " + tempitem.CargoType + " " +
                                    "WHERE GameTypeID = '" + GameTypeID + "' " +
                                    "AND CargoName = '" + tempitem.CargoName + "' " +
                                    "AND CargoType = '" + tempitem.CargoType + "';";

                                    int rowsupdated = -1;
                                    try
                                    {
                                        command.CommandText = updatecommandText;
                                        command.Connection.Open();
                                        rowsupdated = command.ExecuteNonQuery();
                                    }
                                    catch (SqlCeException sqlexception)
                                    {
                                        MessageBox.Show(sqlexception.Message + " | " + sqlexception.ErrorCode, "SQL Exception.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    finally
                                    {
                                        command.Connection.Close();
                                    }

                                    if (rowsupdated == 0)
                                    {
                                        updatecommandText = "INSERT INTO [CargoesTable] (CargoName, CargoType, GameTypeID) " +
                                            "VALUES('" +
                                            tempitem.CargoName + "', '" +
                                            tempitem.CargoType + "', '" +
                                            GameTypeID + "');";

                                        //SqlCeCommand command = DBconnection.CreateCommand();
                                        try
                                        {
                                            command.CommandText = updatecommandText;
                                            command.Connection.Open();
                                            command.ExecuteNonQuery();//rowsupdated = 
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

                                    SQLCommandCMD = "SELECT ID_cargo FROM [CargoesTable] WHERE CargoName = '" + tempitem.CargoName +
                                        "' AND CargoType = " + tempitem.CargoType + " AND GameTypeID = '" + GameTypeID + "' ";
                                    //SqlCeCommand command = DBconnection.CreateCommand();
                                    command.CommandText = SQLCommandCMD;

                                    command.Connection.Open();
                                    SqlCeDataReader readerCargo = command.ExecuteReader();

                                    int CargoID = -1;

                                    while (readerCargo.Read())
                                    {
                                        CargoID = int.Parse(readerCargo["ID_cargo"].ToString());
                                    }
                                    command.Connection.Close();
                                                                        
                                    foreach (KeyValuePair<string, Dictionary<string, int>> tempDefVar in tempitem.CargoVarDef )
                                    {
                                        ///// DEFENITION
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
                                            MessageBox.Show(sqlexception.Message + " | " + sqlexception.ErrorCode, "SQL Exception.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                        finally
                                        {
                                            command.Connection.Close();
                                        }

                                        if (rowsupdated == 0)
                                        {
                                            //sql += "CREATE TABLE CargoesToTrailerDefinitionTable (ID_trailerCtD INT IDENTITY(1,1) PRIMARY KEY, CargoID INT NOT NULL, TrailerDefinitionID INT NOT NULL);";
                                            updatecommandText = "INSERT INTO [TrailerDefinitionTable] (TrailerDefinitionName) " +
                                                "VALUES('" + tempDefVar.Key + "');";

                                            //SqlCeCommand command = DBconnection.CreateCommand();
                                            try
                                            {
                                                command.CommandText = updatecommandText;
                                                command.Connection.Open();
                                                command.ExecuteNonQuery(); //rowsupdated = 
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

                                        SQLCommandCMD = "SELECT ID_trailerD FROM [TrailerDefinitionTable] WHERE TrailerDefinitionName = '" + tempDefVar.Key + "' ";
                                        //SqlCeCommand command = DBconnection.CreateCommand();
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
                                            updatecommandText = "INSERT INTO [CargoesToTrailerDefinitionTable] (CargoID, TrailerDefinitionID) " +
                                            "VALUES('" + CargoID + "', '" + DefenitonID + "');";

                                            //SqlCeCommand command = DBconnection.CreateCommand();
                                            try
                                            {
                                                command.CommandText = updatecommandText;
                                                command.Connection.Open();
                                                command.ExecuteNonQuery();//rowsupdated = 
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

                                        /////
                                        foreach (KeyValuePair<string, int> tempVar in tempDefVar.Value)
                                        {
                                            /////VARIANT

                                            updatecommandText = "UPDATE [TrailerVariantTable] SET TrailerVariantName = '" + tempVar.Key + "', " +
                                                "CargoUnitsCount = '" + tempVar.Value + "' " +
                                                "WHERE TrailerVariantName = '" + tempVar.Key + "'; ";

                                            rowsupdated = -1;
                                            try
                                            {
                                                command.CommandText = updatecommandText;
                                                command.Connection.Open();
                                                rowsupdated = command.ExecuteNonQuery();
                                            }
                                            catch (SqlCeException sqlexception)
                                            {
                                                MessageBox.Show(sqlexception.Message + " | " + sqlexception.ErrorCode, "SQL Exception.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            }
                                            finally
                                            {
                                                command.Connection.Close();
                                            }

                                            if (rowsupdated == 0)
                                            {
                                                //sql += "CREATE TABLE TrailerDefinitionToTrailerVariantTable (ID_trailerDtV INT IDENTITY(1,1) PRIMARY KEY, TrailerDefinitionID INT NOT NULL, TrailerVariantID INT NOT NULL);";
                                                updatecommandText = "INSERT INTO [TrailerVariantTable] (TrailerVariantName, CargoUnitsCount) " +
                                                    "VALUES('" + tempVar.Key + "', '" + tempVar.Value + "');";

                                                //SqlCeCommand command = DBconnection.CreateCommand();
                                                try
                                                {
                                                    command.CommandText = updatecommandText;
                                                    command.Connection.Open();
                                                    command.ExecuteNonQuery();//rowsupdated = 
                                                }
                                                catch (SqlCeException sqlexception)
                                                {
                                                    MessageBox.Show(sqlexception.Message + " | " + sqlexception.ErrorCode, "SQL Exception.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                }
                                                finally
                                                {
                                                    command.Connection.Close();
                                                }


                                                SQLCommandCMD = "SELECT ID_trailerV FROM [TrailerVariantTable] WHERE TrailerVariantName = '" + tempVar.Key + "' AND CargoUnitsCount = '" + tempVar.Value + "';";
                                                //SqlCeCommand command = DBconnection.CreateCommand();
                                                command.CommandText = SQLCommandCMD;

                                                command.Connection.Open();
                                                SqlCeDataReader readerVar = command.ExecuteReader();

                                                int VariantID = -1;

                                                while (readerVar.Read())
                                                {
                                                    VariantID = int.Parse(readerVar["ID_trailerV"].ToString());
                                                }
                                                command.Connection.Close();

                                                if (rowsupdated == 0)
                                                {
                                                    updatecommandText = "INSERT INTO [TrailerDefinitionToTrailerVariantTable] (TrailerDefinitionID, TrailerVariantID) " +
                                                        "VALUES('" + DefenitonID + "', '"  + VariantID + "');";

                                                    //SqlCeCommand command = DBconnection.CreateCommand();
                                                    try
                                                    {
                                                        command.CommandText = updatecommandText;
                                                        command.Connection.Open();
                                                        command.ExecuteNonQuery();//rowsupdated = 
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
                                    }
                                    /*
                                    if (!first)
                                    {
                                        SQLCommandCMD += " UNION ALL ";
                                    }
                                    else
                                    {
                                        first = false;
                                    }

                                    SQLCommandCMD += "SELECT '" + tempitem.CargoName + "', " + tempitem.CargoType + ", " + GameTypeID;//cargovariants + ", " + GameTypeID;
                                    */
                                }
                                UpdateDatabase(SQLCommandCMD);
                            }
                            break;
                        }

                    case "CitysTable":
                        {
                            if (CitiesListDiff != null && CitiesListDiff.Count() > 0)
                            {
                                string SQLCommandCMD = "";
                                SQLCommandCMD += "INSERT INTO [CitysTable] (CityName, GameTypeID) ";

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

                                    SQLCommandCMD += "SELECT '" + tempcity + "', " + GameTypeID;
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
                                SQLCommandCMD += "INSERT INTO [CompaniesTable] (CompanyName, GameTypeID) ";

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

                                    SQLCommandCMD += "SELECT '" + tempitem + "', " + GameTypeID;
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
                                SQLCommandCMD += "INSERT INTO [TrucksTable] (TruckName, TruckType, GameTypeID) ";

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

                                    SQLCommandCMD += "SELECT '" + tempitem.TruckName + "', " + tempitem.Type + ", " + GameTypeID;
                                }
                                UpdateDatabase(SQLCommandCMD);
                            }

                            break;
                        }
                }

            }
            /*
            else if (_targetTable == "CompaniesInCitysTable")
                SQLCommand = new SqlCeCommand(@"INSERT INTO [CompaniesInCitysTable] (CityID, CompanyID) " +
                "(SELECT ID_city FROM CitysTable WHERE CityName = '" + _valueArray[0] + "'), " +
                "(SELECT ID_company FROM CompaniesTable WHERE CompanyName = '" + _valueArray[1] + "')"
                , DBconnection);

            else if (_targetTable == "CompaniesCargoTable")
                SQLCommand = new SqlCeCommand(@"INSERT INTO [CompaniesCargoTable] (CompanyID, CargoID) " +
                "(SELECT ID_company FROM CompaniesTable WHERE CompanyName = '" + _valueArray[0] + "'), " +
                "(SELECT ID_cargo FROM CargoesTable WHERE CargoName = '" + _valueArray[1] + "')"
                , DBconnection);
                */
        }

        private void GetDataFromDatabase(string _targetTable, string _gametype)
        {
            SqlCeDataReader reader = null;

            try
            {
                if(DBconnection.State == ConnectionState.Closed)
                    DBconnection.Open();

                int totalrecord = 0;

                switch (_targetTable)
                {
                    case "CargoesTable":
                        {
                            CargoesListDB.Clear(); //Clears existing list
                            /*
                            sql += "CREATE TABLE CargoesTable (ID_cargo INT IDENTITY(1,1) PRIMARY KEY, CargoName NVARCHAR(32) NOT NULL, CargoType INT NOT NULL, GameTypeID INT NOT NULL);";
                            sql += "ALTER TABLE CargoesTable ADD FOREIGN KEY(GameTypeID) REFERENCES GameTypesTable(ID_gameType);";
                            
                            sql += "CREATE TABLE TrailerDefinitionTable (ID_trailerD INT IDENTITY(1,1) PRIMARY KEY, TrailerDefinitionName NVARCHAR(32) NOT NULL, CargoID INT NOT NULL);";
                            sql += "ALTER TABLE CargoesTable ADD FOREIGN KEY(CargoID) REFERENCES CargoesTable(ID_cargo);";

                            sql += "CREATE TABLE TrailerVariantTable (ID_trailerV INT IDENTITY(1,1) PRIMARY KEY, TrailerVariantName NVARCHAR(32) NOT NULL, CargoUnitsCount INT NOT NULL, TrailerDefinitionID INT NOT NULL);";
                            sql += "ALTER TABLE CargoesTable ADD FOREIGN KEY(TrailerDefinitionID) REFERENCES TrailerDefinitionTable(ID_trailerD);";

                            */
                            //string commandText = "SELECT CargoName, CargoType, CargoVariants FROM [CargoesTable] INNER JOIN GameTypesTable ON CargoesTable.GameTypeID = GameTypesTable.ID_gameType WHERE GameTypeName = '" + _gametype + "';";
                            string commandText = "SELECT ID_cargo, CargoName, CargoType FROM [CargoesTable] INNER JOIN GameTypesTable ON CargoesTable.GameTypeID = GameTypesTable.ID_gameType WHERE GameTypeName = '" + _gametype + "';";
                            //DBconnection.Open();
                            reader = new SqlCeCommand(commandText, DBconnection).ExecuteReader();

                            Dictionary<string, Dictionary<string,int>> tempVarDef = new Dictionary<string, Dictionary<string, int>>();

                            while (reader.Read())
                            {
                                //CargoesListDB.Add(new Cargo(reader["CargoName"].ToString(), reader["CargoType"].ToString(), reader["CargoVariants"].ToString()));
                                commandText = "SELECT ID_trailerD, TrailerDefinitionName FROM [TrailerDefinitionTable] INNER JOIN CargoesTable ON CargoesTable.ID_cargo = TrailerDefinitionTable.CargoID WHERE CargoID = '" + reader["ID_cargo"].ToString() + "';";
                                SqlCeDataReader reader2 = new SqlCeCommand(commandText, DBconnection).ExecuteReader();

                                Dictionary<string, int> tempVar = new Dictionary<string, int>();
                                
                                while (reader2.Read())
                                {

                                    commandText = "SELECT TrailerVariantName, CargoUnitsCount FROM [TrailerVariantTable] INNER JOIN TrailerDefinitionTable ON TrailerDefinitionTable.ID_trailerD = TrailerVariantTable.TrailerDefinitionID WHERE TrailerDefinitionID = '" + reader2["ID_trailerD"].ToString() + "';";

                                    SqlCeDataReader reader3 = new SqlCeCommand(commandText, DBconnection).ExecuteReader();
                                    while (reader3.Read())
                                    {
                                        tempVar.Add(reader3["TrailerVariantName"].ToString(), int.Parse(reader3["CargoUnitsCount"].ToString()));
                                    }

                                    tempVarDef.Add(reader2["TrailerDefinitionName"].ToString(), tempVar);
                                }
                                
                                CargoesListDB.Add(new Cargo(reader["CargoName"].ToString(), int.Parse(reader["CargoType"].ToString()), tempVarDef));//reader["CargoType"].ToString(), reader["CargoType"].ToString()));
                            }
                            //DBconnection.Close();

                            totalrecord = CargoesListDB.Count();

                            break;
                        }

                    case "CitysTable":
                        {
                            CitiesListDB.Clear(); //Clears existing list

                            string commandText = "SELECT CityName FROM [CitysTable] INNER JOIN GameTypesTable ON CitysTable.GameTypeID = GameTypesTable.ID_gameType WHERE GameTypeName = '" + _gametype + "';";

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

                            string commandText = "SELECT CompanyName FROM [CompaniesTable] INNER JOIN GameTypesTable ON CompaniesTable.GameTypeID = GameTypesTable.ID_gameType WHERE GameTypeName = '" + _gametype + "';";

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

                            string commandText = "SELECT TruckName, TruckType FROM [TrucksTable] INNER JOIN GameTypesTable ON TrucksTable.GameTypeID = GameTypesTable.ID_gameType WHERE GameTypeName = '" + _gametype + "';";

                            reader = new SqlCeCommand(commandText, DBconnection).ExecuteReader();

                            while (reader.Read())
                            {
                                CompanyTruckListDB.Add(new CompanyTruck(reader["TruckName"].ToString(), int.Parse(reader["TruckType"].ToString()) ) );
                            }

                            totalrecord = CompanyTruckListDB.Count();

                            break;
                        }
                }

                LogWriter("Loaded " + totalrecord + " entries from " + _targetTable + " table.");
            }
            catch
            {
                LogWriter("Missing Database.sdf file");
            }
            finally
            {
                reader.Close();
                DBconnection.Close();
            }

        }
    }
}
