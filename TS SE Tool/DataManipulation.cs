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
using System.Reflection;
using System.Text.RegularExpressions;

namespace TS_SE_Tool
{
    public partial class FormMain : Form
    {
        private void PrepareData(object sender, DoWorkEventArgs e)
        {
            string[] chunkOfline;

            LogWriter("Prepare started");
            ShowStatusMessages("i", "message_preparing_data", this, statusStripMain.Name, "toolStripStatusMessages");

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
                            if (tempSavefileInMemory[line].StartsWith("}"))
                                break;

                            string res = "";
                            try
                            {
                                res = tempSavefileInMemory[line].Split(new char[] { ' ' })[2];

                                if (res == "null")
                                    res = null;
                            }
                            catch { }                            

                            if (tempSavefileInMemory[line].StartsWith(" vehicles["))
                                tempGarage.Vehicles.Add(res);
                            else if (tempSavefileInMemory[line].StartsWith(" drivers["))
                                tempGarage.Drivers.Add(res);
                            else if (tempSavefileInMemory[line].StartsWith(" trailers["))
                                tempGarage.Trailers.Add(res);
                            else if (tempSavefileInMemory[line].StartsWith(" status:"))
                                tempGarage.GarageStatus = int.Parse(tempSavefileInMemory[line].Split(new char[] { ':' })[1]);                            

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
                            PlayerDataV.ExperiencePoints = uint.Parse(chunkOfline[2]);
                            continue;
                        }

                        //Skills
                        if (tempSavefileInMemory[line].StartsWith(" adr:"))
                        {
                            string[] LineArray = tempSavefileInMemory[line].Split(new char[] { ' ' });

                            char[] ADR = Convert.ToString(byte.Parse(LineArray[2]), 2).PadLeft(6, '0').ToCharArray();
                            Array.Reverse(ADR);
                            PlayerDataV.PlayerSkills[0] = Convert.ToByte(new string(ADR), 2);
                            //PlayerProfileData.PlayerSkills[0] = byte.Parse(LineArray[2]);
                            continue;
                        }
                        if (tempSavefileInMemory[line].StartsWith(" long_dist:"))
                        {
                            string[] LineArray = tempSavefileInMemory[line].Split(new char[] { ' ' });
                            PlayerDataV.PlayerSkills[1] = byte.Parse(LineArray[2]);
                            continue;
                        }
                        if (tempSavefileInMemory[line].StartsWith(" heavy:"))
                        {
                            string[] LineArray = tempSavefileInMemory[line].Split(new char[] { ' ' });
                            PlayerDataV.PlayerSkills[2] = byte.Parse(LineArray[2]);
                            continue;
                        }
                        if (tempSavefileInMemory[line].StartsWith(" fragile:"))
                        {
                            string[] LineArray = tempSavefileInMemory[line].Split(new char[] { ' ' });
                            PlayerDataV.PlayerSkills[3] = byte.Parse(LineArray[2]);
                            continue;
                        }
                        if (tempSavefileInMemory[line].StartsWith(" urgent:"))
                        {
                            string[] LineArray = tempSavefileInMemory[line].Split(new char[] { ' ' });
                            PlayerDataV.PlayerSkills[4] = byte.Parse(LineArray[2]);
                            continue;
                        }
                        if (tempSavefileInMemory[line].StartsWith(" mechanical:"))
                        {
                            string[] LineArray = tempSavefileInMemory[line].Split(new char[] { ' ' });
                            PlayerDataV.PlayerSkills[5] = byte.Parse(LineArray[2]);
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
                        //Online
                        if (tempSavefileInMemory[line].StartsWith(" stored_online_gps_behind_waypoints:"))
                        {
                            string[] LineArray = tempSavefileInMemory[line].Split(new char[] { ' ' });
                            int gpscount = int.Parse(LineArray[2]);

                            for (int i = 0; i < gpscount; i++)
                            {
                                line++;
                                GPSbehindOnline.Add(tempSavefileInMemory[line].Split(new char[] { ' ' })[2].Split(new char[] { '.' }, 2)[1], new List<string>());
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
                                GPSaheadOnline.Add(tempSavefileInMemory[line].Split(new char[] { ' ' })[2].Split(new char[] { '.' }, 2)[1], new List<string>());
                            }
                            continue;
                        }

                        //Offline
                        //Normal
                        if (tempSavefileInMemory[line].StartsWith(" stored_gps_behind_waypoints:"))
                        {
                            string[] LineArray = tempSavefileInMemory[line].Split(new char[] { ' ' });
                            int gpscount = int.Parse(LineArray[2]);

                            for (int i = 0; i < gpscount; i++)
                            {
                                line++;
                                GPSbehind.Add(tempSavefileInMemory[line].Split(new char[] { ' ' })[2].Split(new char[] { '.' }, 2)[1], new List<string>());
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
                                GPSahead.Add(tempSavefileInMemory[line].Split(new char[] { ' ' })[2].Split(new char[] { '.' }, 2)[1], new List<string>());
                            }
                            continue;
                        }
                        //Avoid
                        if (tempSavefileInMemory[line].StartsWith(" stored_gps_avoid_waypoints:"))
                        {
                            string[] LineArray = tempSavefileInMemory[line].Split(new char[] { ' ' });
                            int gpscount = int.Parse(LineArray[2]);

                            for (int i = 0; i < gpscount; i++)
                            {
                                line++;
                                GPSAvoid.Add(tempSavefileInMemory[line].Split(new char[] { ' ' })[2].Split(new char[] { '.' }, 2)[1], new List<string>());
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

                        if (tempSavefileInMemory[line].StartsWith(" driver_pool["))
                        {
                            chunkOfline = tempSavefileInMemory[line].Split(new char[] { ' ' });
                            DriverPool.Add(chunkOfline[2]);
                            continue;
                        }
                    }

                    //Account Money
                    if (tempSavefileInMemory[line].StartsWith(" money_account:"))
                    {
                        chunkOfline = tempSavefileInMemory[line].Split(new char[] { ' ' });
                        PlayerDataV.AccountMoney = uint.Parse(chunkOfline[2]);
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
                            PlayerDataV.HQcity = chunkOfline[2];
                            continue;
                        }

                        if (tempSavefileInMemory[line].StartsWith(" assigned_truck:"))
                        {
                            chunkOfline = tempSavefileInMemory[line].Split(new char[] { ' ' });
                            PlayerDataV.UserCompanyAssignedTruck = chunkOfline[2];
                            continue;
                        }

                        if (tempSavefileInMemory[line].StartsWith(" truck_placement:"))
                        {
                            chunkOfline = tempSavefileInMemory[line].Split(new char[] { ':' });
                            PlayerDataV.UserCompanyAssignedTruckPlacement = chunkOfline[1].TrimStart(' ');
                            continue;
                        }

                        if (tempSavefileInMemory[line].StartsWith(" trucks["))
                        {
                            chunkOfline = tempSavefileInMemory[line].Split(new char[] { ' ' });
                            UserTruckDictionary.Add(chunkOfline[2], new UserCompanyTruckData());
                            continue;
                        }

                        if (tempSavefileInMemory[line].StartsWith(" truck_profit_logs["))
                        {
                            chunkOfline = tempSavefileInMemory[line].Split(new char[] { '[', ']' });
                            string tKey = UserTruckDictionary.Keys.ElementAt(int.Parse(chunkOfline[1]));
                            chunkOfline = tempSavefileInMemory[line].Split(new char[] { ' ' });
                            UserTruckDictionary[tKey].TruckProfitLogs = chunkOfline[2];
                            continue;
                        }

                        if (tempSavefileInMemory[line].StartsWith(" drivers["))
                        {
                            chunkOfline = tempSavefileInMemory[line].Split(new char[] { ' ' });
                            string temp = chunkOfline[2];
                            UserDriverDictionary.Add(temp, new UserCompanyDriverData());
                            chunkOfline = tempSavefileInMemory[line].Split(new char[] { '[', ']' });
                            if (int.Parse(chunkOfline[1]) == 0)
                                PlayerDataV.UserDriver = temp;
                            continue;
                        }

                        if (tempSavefileInMemory[line].StartsWith(" driver_readiness_timer["))
                        {
                            chunkOfline = tempSavefileInMemory[line].Split(new char[] { '[', ']' });
                            string tKey = UserDriverDictionary.Keys.ElementAt(int.Parse(chunkOfline[1]));
                            chunkOfline = tempSavefileInMemory[line].Split(new char[] { ' ' });
                            UserDriverDictionary[tKey].DriverReadiness = int.Parse(chunkOfline[2]);
                            continue;
                        }

                        if (tempSavefileInMemory[line].StartsWith(" driver_quit_warned["))
                        {
                            chunkOfline = tempSavefileInMemory[line].Split(new char[] { '[', ']' });
                            string tKey = UserDriverDictionary.Keys.ElementAt(int.Parse(chunkOfline[1]));
                            chunkOfline = tempSavefileInMemory[line].Split(new char[] { ' ' });
                            UserDriverDictionary[tKey].DriverQuitWarned = bool.Parse(chunkOfline[2]);
                            continue;
                        }

                        if (tempSavefileInMemory[line].StartsWith(" assigned_trailer:"))
                        {
                            chunkOfline = tempSavefileInMemory[line].Split(new char[] { ' ' });
                            PlayerDataV.UserCompanyAssignedTrailer = chunkOfline[2];
                            continue;
                        }

                        if (tempSavefileInMemory[line].StartsWith(" trailers["))
                        {
                            chunkOfline = tempSavefileInMemory[line].Split(new char[] { ' ' });

                            UserTrailerDictionary.Add(chunkOfline[2], new UserCompanyTruckData());
                            continue;
                        }

                        if (tempSavefileInMemory[line].StartsWith(" trailer_defs["))
                        {
                            chunkOfline = tempSavefileInMemory[line].Split(new char[] { ' ' });

                            UserTrailerDefDictionary.Add(chunkOfline[2], new List<string>());
                            continue;
                        }

                        if (tempSavefileInMemory[line].StartsWith(" current_job:"))
                        {
                            chunkOfline = tempSavefileInMemory[line].Split(new char[] { ':' });
                            PlayerDataV.CurrentJob = chunkOfline[1].TrimStart(' ');
                            continue;
                        }
                    }

                    //Populate GPS
                    if (tempSavefileInMemory[line].StartsWith("gps_waypoint_storage"))
                    {
                        string nameless = tempSavefileInMemory[line].Split(new char[] { ' ' })[2].Split(new char[] { '.' },2)[1];

                        if (GPSbehind.ContainsKey(nameless))
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
                        else
                        if (GPSAvoid.ContainsKey(nameless))
                        {
                            line++;
                            while (!tempSavefileInMemory[line].StartsWith("}"))
                            {
                                GPSAvoid[nameless].Add(tempSavefileInMemory[line]);
                                line++;
                            }
                        }
                    }

                    if (tempSavefileInMemory[line].StartsWith("player_job :"))
                    {
                        do
                        {
                            line++;
                            if (tempSavefileInMemory[line].Contains("_nameless") && tempSavefileInMemory[line].StartsWith(" company_truck:"))
                            {
                                chunkOfline = tempSavefileInMemory[line].Split(new char[] { ' ' });
                                UserTruckDictionary.Add(chunkOfline[2], new UserCompanyTruckData());
                                UserTruckDictionary[chunkOfline[2]].Users = false;
                                continue;
                            }

                            if (tempSavefileInMemory[line].Contains("_nameless") && tempSavefileInMemory[line].StartsWith(" company_trailer:"))
                            {
                                chunkOfline = tempSavefileInMemory[line].Split(new char[] { ' ' });
                                UserTrailerDictionary.Add(chunkOfline[2], new UserCompanyTruckData());
                                UserTrailerDictionary[chunkOfline[2]].Users = false;
                                continue;
                            }
                        } while (!tempSavefileInMemory[line].StartsWith("}"));

                        continue;
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

                                    line++;
                                }

                                line++;
                            }

                            while (accessoriescount > 0)
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
                                                else if (truckpart.Contains("/f_tire/") || truckpart.Contains("/r_tire/") || truckpart.Contains("/f_wheel/") || truckpart.Contains("/r_wheel/"))
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
                                line++;
                            }
                        }
                        continue;
                    }

                    //find vehicles Trailer
                    int[] traileraccessoriescount = new int[1];
                    string[] trailernamelessArray = new string[1];
                    int slavetrailerscount = 0;

                    TrailerSearchStart:
                    if (tempSavefileInMemory[line].StartsWith("trailer :"))
                    {
                        chunkOfline = tempSavefileInMemory[line].Split(new char[] { ' ' });
                        string vehiclenameless = chunkOfline[2];

                        if (UserTrailerDictionary.ContainsKey(vehiclenameless))
                        {
                            UserTrailerDictionary[vehiclenameless].Parts.Add(new UserCompanyTruckDataPart("trailerdata"));
                            line++;

                            while (!tempSavefileInMemory[line].StartsWith("}"))
                            {
                                if (tempSavefileInMemory[line].StartsWith(" trailer_definition:"))
                                {
                                    string nameless = tempSavefileInMemory[line].Split(new char[] { ' ' })[2];
                                    UserTrailerDictionary[vehiclenameless].Parts.Add(new UserCompanyTruckDataPart("trailerdef", nameless));
                                }
                                else
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
                                    if (tempSavefileInMemory[line].Contains("_nameless"))
                                    {
                                        slavetrailerscount++;
                                        Array.Resize(ref traileraccessoriescount, slavetrailerscount);
                                        Array.Resize(ref trailernamelessArray, slavetrailerscount);

                                        string trailernameless = tempSavefileInMemory[line].Split(new char[] { ' ' })[2];

                                        trailernamelessArray[slavetrailerscount - 1] = vehiclenameless;

                                        UserTrailerDictionary[vehiclenameless].Parts.Add(new UserCompanyTruckDataPart("slavetrailer", trailernameless));

                                        UserTrailerDictionary.Add(trailernameless, new UserCompanyTruckData());
                                        UserTrailerDictionary.Last().Value.Main = false;
                                    }
                                    else
                                    {
                                        slavetrailerscount++;
                                        Array.Resize(ref traileraccessoriescount, slavetrailerscount);
                                        Array.Resize(ref trailernamelessArray, slavetrailerscount);

                                        trailernamelessArray[slavetrailerscount - 1] = vehiclenameless;
                                    }
                                }
                                else
                                if (tempSavefileInMemory[line].StartsWith(" accessories:"))
                                {
                                    traileraccessoriescount[slavetrailerscount - 1] = int.Parse(tempSavefileInMemory[line].Split(new char[] { ' ' })[2]);
                                    line++;
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
                                int trailerindex = traileraccessoriescount.Length - 1;

                                TrailerAccSearchStart:
                                while (traileraccessoriescount[trailerindex] > 0)
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
                                                    else if (truckpart.Contains("chassis"))
                                                    {
                                                        UserTrailerDictionary[trailernamelessArray[trailerindex]].Parts.Add(new UserCompanyTruckDataPart("chassis"));
                                                    }
                                                    else if (truckpart.Contains("/r_tire/") || truckpart.Contains("/t_wheel/"))
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
                                        traileraccessoriescount[trailerindex]--;
                                    }

                                    line++;
                                }

                                if (trailerindex != 0)
                                {
                                    trailerindex--;
                                    goto TrailerAccSearchStart;
                                }
                            }
                        }
                        continue;
                    }

                    if (tempSavefileInMemory[line].StartsWith("trailer_def :"))
                    {
                        chunkOfline = tempSavefileInMemory[line].Split(new char[] { ' ' });
                        string nameless = chunkOfline[2];

                        line++;

                        while (!tempSavefileInMemory[line].StartsWith("}"))
                        {
                            UserTrailerDefDictionary[nameless].Add(tempSavefileInMemory[line]);
                            line++;
                        }

                        continue;
                    }

                    //find existing jobs
                    if (tempSavefileInMemory[line].StartsWith("company : company.volatile."))
                    {
                        string sourcecity = tempSavefileInMemory[line].Split(new char[] { '.' })[3].Split(new char[] { ' ' })[0]; //Source city
                        string sourcecompany = tempSavefileInMemory[line].Split(new char[] { '.' })[2].Split(new char[] { ' ' })[0]; //Source company

                        int index = line + 1;
                        int numOfJobOffers = 0, numOfCargoOffers = 0, cargoseed = 0;
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
                            int cargotype = 0;
                            string cargo = "", trailervariant = "", trailerdefinition = "", units_count = "";
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
                                                cargotype = 0;

                                                if (tempSavefileInMemory[index].Contains("\"heavy"))
                                                {
                                                    cargotype = 1;
                                                }
                                                else if (tempSavefileInMemory[index].Contains("\"double"))
                                                {
                                                    cargotype = 2;
                                                }

                                                CompanyTruckList.Add(new CompanyTruck(LineArray[2], cargotype));
                                            }
                                            else
                                            //Find cargo trailer variant
                                            if (tempSavefileInMemory[index].StartsWith(" trailer_variant:"))
                                            {
                                                trailervariant = tempSavefileInMemory[index].Split(new char[] { ' ' })[2];
                                                if (!TrailerVariants.Contains(trailervariant))
                                                    TrailerVariants.Add(trailervariant);
                                            }
                                            else
                                            //Find cargo trailer definition
                                            if (tempSavefileInMemory[index].StartsWith(" trailer_definition:"))
                                            {
                                                trailerdefinition = tempSavefileInMemory[index].Split(new char[] { ' ' })[2];
                                            }
                                            else
                                            if (tempSavefileInMemory[index].StartsWith(" units_count:"))
                                            {
                                                units_count = tempSavefileInMemory[index].Split(new char[] { ' ' })[2];
                                            }
                                            else
                                            if (tempSavefileInMemory[index].StartsWith("}"))
                                            {
                                                Cargo tempCargo = CargoesList.Find(x => x.CargoName == cargo);

                                                if (tempCargo == null)
                                                {
                                                    CargoesList.Add(new Cargo(cargo, cargotype, trailerdefinition, units_count));
                                                }
                                                else
                                                {
                                                    if (!tempCargo.TrailerDefList.Exists(x => x.DefName == trailerdefinition && x.CargoType == cargotype && x.UnitsCount == int.Parse(units_count)))
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
                        //Array.Resize(ref EconomyEventQueueList, newSize);
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

            //GetDataFrom Database

            //GetDataFromDatabase("CargoesTable");
            GetDataFromDatabase("CitysTable");
            GetDataFromDatabase("CompaniesTable");
            GetDataFromDatabase("TrucksTable");

            //Compare Data to Database
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
            List<string> SavefileDependencies = new List<string>();

            for (int line = 0; line < tempInfoFileInMemory.Length; line++)
            {
                if (tempInfoFileInMemory[line].StartsWith(" version:"))
                {
                    chunkOfline = tempInfoFileInMemory[line].Split(new char[] { ' ' });
                    SavefileVersion = int.Parse(chunkOfline[2]);
                    continue;
                }
                
                if (tempInfoFileInMemory[line].StartsWith(" dependencies["))
                {
                    chunkOfline = tempInfoFileInMemory[line].Split(new char[] { '"' });
                    SFDependencies.Add(chunkOfline[1]);

                    string[] depstring = chunkOfline[1].Split(new char[] { '|' });

                    string type = depstring[0];
                    string depcode = "";
                    if (type == "dlc" || type == "rdlc")
                    {
                        depcode = depstring[1].Split(new char[] { '_' }, 2)[1];
                        depcode = "dlc_" + depcode;
                    }
                    if (type == "mod")
                    {
                        depcode = depstring[1];
                        depcode = "mod_" + depcode;
                    }

                    SavefileDependencies.Add(depcode);
                    continue;
                }
            }

            string sql = "UPDATE [DatabaseDetails] SET SaveVersion = " + SavefileVersion + " WHERE ID_DBline = 1;";
            UpdateDatabase(sql);

            GetDataFromDatabase("Dependencies");

            //Check dependencies
            if (DBDependencies.Capacity == 0)
            {
                InsertDataIntoDatabase("Dependencies");
                InfoDepContinue = true;
            }
            else
            {
                List<string> dbdep = DBDependencies.Except(SFDependencies).ToList();
                List<string> sfdep = SFDependencies.Except(DBDependencies).ToList();

                if (dbdep.Capacity > 0 || sfdep.Capacity > 0)
                {
                    string dbdepstr = "", sfdepstr = "";

                    if(dbdep.Capacity > 0)
                    {
                        dbdepstr += "\r\nDependencies only in Database (" + dbdep.Count.ToString() +  "):\r\n";
                        int i = 0;
                        foreach (string temp in dbdep)
                        {
                            i++;
                            dbdepstr += i.ToString() + ") " + temp + "\r\n";
                        }
                    }

                    if(sfdep.Capacity > 0)
                    {
                        sfdepstr += "\r\nDependencies only in Save file (" + sfdep.Count.ToString() + "):\r\n";
                        int i = 0;
                        foreach (string temp in sfdep)
                        {
                            i++;
                            sfdepstr += i.ToString() + ") " + temp + "\r\n"; ;
                        }
                    }

                    DialogResult r = MessageBox.Show("Save file and Database has different Dependencies due to installed\\deleted mods\\dlc's.\r\n" +
                        "This may result in wrong path and cargo data.\r\n" +
                        "Do you want to proceed and Update Dependencies?\r\n" + dbdepstr + "\r\n" + sfdepstr, "Dependencies conflict", MessageBoxButtons.YesNo);

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

            if (SavefileDependencies.Count > 0)
                foreach(string tDepend in SavefileDependencies)
                {
                    LoadCachedExternalCargoData(tDepend);
                }

            if (SavefileVersion == 0)
                ShowStatusMessages("e", "error_save_version_not_detected");
        }

        public string GetCustomSaveFilename(string _tempSaveFilePath, Form _senderForm, string _statusStrip, string _targetLabel)
        {
            string chunkOfline;

            string tempSiiInfoPath = _tempSaveFilePath + @"\info.sii";
            string[] tempFile = null;
            //////
            if (!File.Exists(tempSiiInfoPath))
            {
                LogWriter("File does not exist in " + tempSiiInfoPath);
                ShowStatusMessages("e", "error_could_not_find_file", _senderForm, _statusStrip, _targetLabel);
            }
            else
            {   
                FileDecoded = false;
                try
                {
                    int decodeAttempt = 0;
                    while (decodeAttempt < 5)
                    {
                        tempFile = NewDecodeFile(tempSiiInfoPath, _senderForm, _statusStrip, _targetLabel);

                        if (FileDecoded)
                        {
                            break;
                        }

                        decodeAttempt++;
                    }

                    if (decodeAttempt == 5)
                    {
                        LogWriter("Could not decrypt after 5 attempts");
                        ShowStatusMessages("e", "error_could_not_decode_file", _senderForm, _statusStrip, _targetLabel);
                    }
                }
                catch
                {
                    LogWriter("Could not read: " + tempSiiInfoPath);
                }

                if ((tempFile == null) || (tempFile[0] != "SiiNunit"))
                {
                    LogWriter("Wrongly decoded Info file or wrong file format");
                    ShowStatusMessages("e", "error_file_not_decoded", _senderForm, _statusStrip, _targetLabel);
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

        private void CheckProfileInfoData()
        {
            string[] chunkOfline;

            for (int line = 0; line < tempProfileFileInMemory.Length; line++)
            {
                if (tempProfileFileInMemory[line].StartsWith(" logo:"))
                {
                    chunkOfline = tempProfileFileInMemory[line].Split(new char[] { ' ' });
                    PlayerDataV.CompanyLogo = chunkOfline[2];
                    continue; //searching
                }

                if (tempProfileFileInMemory[line].StartsWith(" company_name:"))
                {
                    chunkOfline = tempProfileFileInMemory[line].Split(new char[] { ' ' },3);
                    string result = "";

                    if (chunkOfline[2].StartsWith("\"") && chunkOfline[2].EndsWith("\""))
                    {
                        string compNameH = chunkOfline[2].Trim('"');
                        //try find \x

                        for (int i=0; i < compNameH.Length; i++)
                        {
                            if (compNameH[i] == '\\')
                            {
                                char t = compNameH[i];
                                string temp = compNameH.Substring(i + 1, 1);
                                
                                if(temp == "\\")
                                {
                                    result += temp;
                                    i += 1;
                                }
                                else if (temp == "x")
                                {
                                    string tempChar = "";
                                    checkForChar:

                                    tempChar += compNameH.Substring(i + 2, 2);
                                    string r = FromHexToString(tempChar);
                                    char tChar = Convert.ToChar(r);

                                    if (Char.IsControl(tChar) || tChar != 65533)
                                    {
                                        result += r;
                                        i += 3;
                                    }
                                    else
                                    {
                                        i += 4;

                                        goto checkForChar;
                                    }
                                    
                                }
                                else
                                {
                                    result += temp;
                                    i += 1;
                                }
                                
                            }
                            else
                                result += compNameH[i];
                        }
                    }

                    if (result == "")
                    {
                        PlayerDataV.CompanyName = chunkOfline[2];
                        ProfileDataV.CompanyName = chunkOfline[2];
                    }   
                    else
                    {
                        PlayerDataV.CompanyName = result;
                        ProfileDataV.CompanyName = result;
                    }                        

                    continue; //searching
                }

                if (tempProfileFileInMemory[line].StartsWith(" cached_experience:"))
                {
                    chunkOfline = tempProfileFileInMemory[line].Split(new char[] { ' ' });
                    ProfileDataV.CachedExperiencePoints = uint.Parse(chunkOfline[2]);
                    continue; //searching
                }

                if (tempProfileFileInMemory[line].StartsWith(" cached_distance:"))
                {
                    chunkOfline = tempProfileFileInMemory[line].Split(new char[] { ' ' });
                    ProfileDataV.CachedDistance = uint.Parse(chunkOfline[2]);
                    continue; //searching
                }

                //User data
                if (tempProfileFileInMemory[line].StartsWith(" user_data[4]:"))
                {
                    chunkOfline = tempProfileFileInMemory[line].Split(new char[] { ' ' });

                    //CultureInfo culture = 
                    NumberFormatInfo nfi = new CultureInfo(CultureInfo.CurrentCulture.Name, false).NumberFormat;
                    nfi.NumberDecimalSeparator = ".";

                    double.TryParse(chunkOfline[2].Trim('"'), NumberStyles.AllowDecimalPoint, nfi, out double res);
                    
                    ProfileDataV.RoadsExplored = res;                   

                    continue; //searching
                }

                if (tempProfileFileInMemory[line].StartsWith(" user_data[5]:"))
                {
                    chunkOfline = tempProfileFileInMemory[line].Split(new char[] { ' ' });
                    ProfileDataV.DeliveriesFinished = uint.Parse(chunkOfline[2]);
                    continue; //searching
                }

                if (tempProfileFileInMemory[line].StartsWith(" user_data[7]:"))
                {
                    chunkOfline = tempProfileFileInMemory[line].Split(new char[] { ' ' });
                    ProfileDataV.OwnedGaradesSmall = uint.Parse(chunkOfline[2]);
                    continue; //searching
                }

                if (tempProfileFileInMemory[line].StartsWith(" user_data[8]:"))
                {
                    chunkOfline = tempProfileFileInMemory[line].Split(new char[] { ' ' });
                    ProfileDataV.OwnedGaradesLarge = uint.Parse(chunkOfline[2]);
                    continue; //searching
                }

                if (tempProfileFileInMemory[line].StartsWith(" user_data[9]:"))
                {
                    chunkOfline = tempProfileFileInMemory[line].Split(new char[] { ' ' });
                    ProfileDataV.GameTimeSpent = ulong.Parse(chunkOfline[2]);
                    continue; //searching
                }

                if (tempProfileFileInMemory[line].StartsWith(" user_data[10]:"))
                {
                    chunkOfline = tempProfileFileInMemory[line].Split(new char[] { ' ' });
                    ProfileDataV.RealTimeSpent = uint.Parse(chunkOfline[2]);
                    continue; //searching
                }

                if (tempProfileFileInMemory[line].StartsWith(" user_data[6]:"))
                {
                    chunkOfline = tempProfileFileInMemory[line].Split(new char[] { ' ' });
                    ProfileDataV.OwnedTrucks = uint.Parse(chunkOfline[2].Trim('"'));
                    continue; //searching
                }

                if (tempProfileFileInMemory[line].StartsWith(" user_data[16]:"))
                {
                    chunkOfline = tempProfileFileInMemory[line].Split(new char[] { ' ' });
                    ProfileDataV.OwnedTrailers = uint.Parse(chunkOfline[2]);
                    continue; //searching
                }


                //User data end

                if (tempProfileFileInMemory[line].StartsWith(" cached_discovery:"))
                {
                    chunkOfline = tempProfileFileInMemory[line].Split(new char[] { ' ' });
                    line = +int.Parse(chunkOfline[2]);
                    continue; //searching
                }

                if (tempProfileFileInMemory[line].StartsWith(" profile_name:"))
                {
                    chunkOfline = tempProfileFileInMemory[line].Split(new char[] { ' ' }, 3);
                    string result = null;

                    if (chunkOfline[2].StartsWith("\"") && chunkOfline[2].EndsWith("\""))
                    {
                        string compNameH = chunkOfline[2].Trim('"');
                        if(compNameH.Contains("\\x"))
                        {
                            string compNameH2 = string.Join("", compNameH.Split(new string[] { "\\x" }, StringSplitOptions.RemoveEmptyEntries));
                            result = FromHexToString(compNameH2);
                        }

                        if (result == null)
                        {
                            result = Regex.Unescape(chunkOfline[2]);
                            result = result.Trim('"');//.Remove(result.Length - 1, 1).Remove(0, 1);
                        }
                    }

                    if (result == "")
                    {
                        ProfileDataV.ProfileName = chunkOfline[2];
                    }
                    else
                    {
                        ProfileDataV.ProfileName = result;
                    }

                    continue; //searching
                }

                if (tempProfileFileInMemory[line].StartsWith(" creation_time:"))
                {
                    chunkOfline = tempProfileFileInMemory[line].Split(new char[] { ' ' });
                    ProfileDataV.CreationTime = uint.Parse(chunkOfline[2]);
                    continue; //searching
                }

                if (tempProfileFileInMemory[line].StartsWith(" save_time:"))
                {
                    chunkOfline = tempProfileFileInMemory[line].Split(new char[] { ' ' });
                    ProfileDataV.SaveTime = uint.Parse(chunkOfline[2]);
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
                GaragesList[GaragesList.FindIndex(x => x.GarageName == PlayerDataV.HQcity)].Trailers.AddRange(extraTrailers);
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
                if (extraDrivers.Contains(PlayerDataV.UserDriver))
                {
                    Garages tmpG = new Garages(PlayerDataV.HQcity);

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

                    int tmpIdx = extraDrivers.IndexOf(PlayerDataV.UserDriver);

                    GaragesList[hqIdx].Drivers[DrvIdx] = extraDrivers[tmpIdx];
                    GaragesList[hqIdx].Vehicles[DrvIdx] = extraVehicles[tmpIdx];

                    extraDrivers.RemoveAt(tmpIdx);
                    extraVehicles.RemoveAt(tmpIdx);
                }
            }
        }

        //Rearrange extra User Drivers to glogal Driver pool
        private void PrepareDriversTrucks()
        {
            extraDrivers.RemoveAll(x => x == null);

            foreach (string tmp in extraDrivers)
            {
                if (tmp != null)
                {
                    DriverPool.Add(tmp);
                    UserDriverDictionary.Remove(tmp);
                }
            }

            extraVehicles.RemoveAll(x => x == null);
        }
        //Sort events by time
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

        private void AddCargo(bool JobEditing )
        {
            if (comboBoxFreightMarketSourceCity.SelectedIndex < 0 || comboBoxFreightMarketSourceCompany.SelectedIndex < 0 || comboBoxFreightMarketDestinationCity.SelectedIndex < 0 || comboBoxFreightMarketDestinationCompany.SelectedIndex < 0 || comboBoxFreightMarketCargoList.SelectedIndex < 0 || comboBoxFreightMarketUrgency.SelectedIndex < 0 || comboBoxFreightMarketTrailerDef.SelectedIndex < 0 || comboBoxFreightMarketTrailerVariant.SelectedIndex < 0)
            {
                LogWriter("Missing selection of Source, Destination or Cargo settings");
                ShowStatusMessages("e", "error_job_parameters_not_filled");
            }
            else
            {
                ShowStatusMessages("i", "");

                #region SetupVariables
                //Getting data from form controls
                string SourceCity = comboBoxFreightMarketSourceCity.SelectedValue.ToString();
                string SourceCompany = comboBoxFreightMarketSourceCompany.SelectedValue.ToString();
                string DestinationCity = comboBoxFreightMarketDestinationCity.SelectedValue.ToString();
                string DestinationCompany = comboBoxFreightMarketDestinationCompany.SelectedValue.ToString();
                string Cargo = comboBoxFreightMarketCargoList.SelectedValue.ToString().Split(new char[] { ',' })[0];
                string Urgency = comboBoxFreightMarketUrgency.SelectedValue.ToString();
                string TrailerDefinition = comboBoxFreightMarketTrailerDef.SelectedValue.ToString();
                string TrailerVariant = comboBoxFreightMarketTrailerVariant.SelectedValue.ToString();

                string SourceCityName = comboBoxFreightMarketSourceCity.Text;
                string SourceCompanyName = comboBoxFreightMarketSourceCompany.Text;
                string DestinationCityName = comboBoxFreightMarketDestinationCity.Text;
                string DestinationCompanyName = comboBoxFreightMarketDestinationCompany.Text;
                string CargoName = comboBoxFreightMarketCargoList.Text;
                
                //Trying to get route data from database
                string[] route = RouteList.GetRouteData(SourceCity, SourceCompany, DestinationCity, DestinationCompany);
                string distance = route[4];
                string FerryTime = route[5];
                string FerryPrice = route[6];
                //Setting proper ferry time
                if (FerryTime == "-1")
                {
                    FerryTime = "0";
                }

                //local variables
                int CargoType = -1, UnitsCount = 1;
                string TruckName = "";

                //Local Trailer Def DT
                DataTable TrailerDefDTs = ((DataTable)comboBoxFreightMarketTrailerDef.DataSource).DefaultView.ToTable();
                CargoType = int.Parse(TrailerDefDTs.Rows[comboBoxFreightMarketTrailerDef.SelectedIndex]["CargoType"].ToString());
                UnitsCount = int.Parse(TrailerDefDTs.Rows[comboBoxFreightMarketTrailerDef.SelectedIndex]["UnitsCount"].ToString());
                //Company truck for QJ
                List <CompanyTruck> CompanyTruckType = CompanyTruckListDB.Where(x => x.Type == CargoType).ToList();
                TruckName = CompanyTruckType[RandomValue.Next(CompanyTruckType.Count())].TruckName;
                
                //True Distance
                int TrueDistance = (int)(int.Parse(distance) * ProgSettingsV.TimeMultiplier);

                if (distance == "11111")
                {
                    TrueDistance = (int)(5 * ProgSettingsV.TimeMultiplier);
                    unCertainRouteLength = "*";
                }
                //Time untill job expires
                int ExpirationTime = InGameTime + RandomValue.Next(180, 1800) + (JobsAmountAdded * ProgSettingsV.JobPickupTime * 60);
                #endregion

                //Creating Job data
                JobAdded tempJobData = new JobAdded(SourceCity, SourceCompany, DestinationCity, DestinationCompany, Cargo, int.Parse(Urgency), CargoType, 
                    UnitsCount, TrueDistance, int.Parse(FerryTime), int.Parse(FerryPrice), ExpirationTime, TruckName, TrailerVariant, TrailerDefinition);
                
                //Settign start point for loopback route
                if (JobsAmountAdded == 0)
                {
                    LoopStartCity = SourceCity;
                    LoopStartCompany = SourceCompany;
                }

                //Add Job data to program storage
                string companyNameJob = "company : company.volatile." + SourceCompany + "." + SourceCity + " {";

                if (!JobEditing)
                {
                    //Adding new Job
                    //Tracking total amount of jobs added
                    JobsAmountAdded++;

                    // Prepairing structures
                    Array.Resize(ref EconomyEventUnitLinkStringList, JobsAmountAdded);
                    EconomyEventUnitLinkStringList[JobsAmountAdded - 1] = " unit_link: company.volatile." + SourceCompany + "." + SourceCity;

                    //Add Job data to Listbox
                    listBoxFreightMarketAddedJobs.Items.Add(tempJobData);

                    comboBoxFreightMarketSourceCity.SelectedValue = comboBoxFreightMarketDestinationCity.SelectedValue;
                    comboBoxFreightMarketSourceCompany.SelectedValue = comboBoxFreightMarketDestinationCompany.SelectedValue;

                    //Check and set for loopback route
                    int looptest;

                    if (JobsAmountAdded == 1)
                        looptest = 1;
                    else
                        looptest = JobsAmountAdded + 1;

                    if (ProgSettingsV.LoopEvery != 0 && (looptest % ProgSettingsV.LoopEvery) == 0)
                    {
                        try
                        {
                            comboBoxFreightMarketDestinationCity.SelectedValue = LoopStartCity;
                            comboBoxFreightMarketDestinationCompany.SelectedValue = LoopStartCompany;
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
                else
                {
                    //Editin Job
                    AddedJobsDictionary[companyNameJob].Remove(FreightMarketJob);
                    if (AddedJobsDictionary[companyNameJob].Count == 0)
                        AddedJobsDictionary.Remove(companyNameJob);

                    AddedJobsList.Remove(FreightMarketJob);

                    //Add Job data to Listbox
                    listBoxFreightMarketAddedJobs.Items[listBoxFreightMarketAddedJobs.SelectedIndex] = tempJobData;
                    listBoxFreightMarketAddedJobs.Enabled = true;
                }

                //Adding Job to strucktures
                if (AddedJobsDictionary.ContainsKey(companyNameJob))
                    AddedJobsDictionary[companyNameJob].Add(tempJobData);
                else
                    AddedJobsDictionary.Add(companyNameJob, new List<JobAdded> { tempJobData });

                AddedJobsList.Add(tempJobData);
                //

                //Total distance for Form label
                int JobsTotalDistance = 0;

                foreach (JobAdded tmpItem in listBoxFreightMarketAddedJobs.Items)
                {
                    JobsTotalDistance += tmpItem.Distance;
                }

                labelFreightMarketDistanceNumbers.Text = Math.Floor(JobsTotalDistance * DistanceMultiplier).ToString() + unCertainRouteLength + " " + ProgSettingsV.DistanceMes;
            }
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
                ShowStatusMessages("e", "message_database_missing_creating_db");

                connectionString = string.Format("DataSource ='{0}';", fileName);

                SqlCeEngine Engine = new SqlCeEngine(connectionString);

                Engine.CreateDatabase();

                ShowStatusMessages("i", "message_database_created");

                CreateDatabaseStructure();
            }
            else
            {
                //Edit DB
                SqlCeDataReader reader = null;

                if (DBconnection.State == ConnectionState.Closed)
                    DBconnection.Open();

                string commandText = "SELECT DBVersion FROM [DatabaseDetails];";
                reader = new SqlCeCommand(commandText, DBconnection).ExecuteReader();
                string DBVersion = "";
                while (reader.Read())
                {
                    DBVersion = reader["DBVersion"].ToString();
                }

                string sql = "";

                if (DBVersion == "0.1.6")
                {
                    goto label016;
                }
                else
                if (DBVersion == "0.2.0")
                    return;


                label016://0.1.6
                sql = "ALTER TABLE [Dependencies] ALTER COLUMN Dependency NVARCHAR(256) NOT NULL;";
                UpdateDatabase(sql);

                //string DBVersion1 = Assembly.GetExecutingAssembly().GetName().Version.Major.ToString();
                //string DBVersion2 = Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString();
                //string DBVersion3 = Assembly.GetExecutingAssembly().GetName().Version.Build.ToString();
                //string DBVersion4 = Assembly.GetExecutingAssembly().GetName().Version.Revision.ToString();

                string DBVersionNew = "0.2.0";//DBVersion1 + DBVersion2 + DBVersion3; //Assembly.GetExecutingAssembly().GetName().Version.ToString();
                sql = "UPDATE [DatabaseDetails] SET DBVersion = '" + DBVersionNew + "' " +
                        "WHERE ID_DBline = 1;";
                UpdateDatabase(sql);
            }
        }
        //Create DB structure
        private void CreateDatabaseStructure()
        {
            if (DBconnection.State == ConnectionState.Closed)
            {
                DBconnection.Open();
            }

            ShowStatusMessages("e", "message_database_missing_creating_db_structure");

            SqlCeCommand cmd;

            string sql = "";

            sql += "CREATE TABLE DatabaseDetails (ID_DBline INT IDENTITY(1,1) PRIMARY KEY, DBVersion NVARCHAR(8) NOT NULL, GameName NVARCHAR(8) NOT NULL, SaveVersion INT NOT NULL, ProfileName NVARCHAR(64) NOT NULL);";
            sql += "INSERT INTO [DatabaseDetails] (DBVersion, GameName, SaveVersion, ProfileName) VALUES ('0.2.0','" + GameType +  "', 0, '" + Path.GetFileName(Globals.ProfilesHex[comboBoxProfiles.SelectedIndex]) + "');";

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
                        MessageBox.Show(sqlexception.Message, "SQL Exception. Create DB", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                ShowStatusMessages("i", "error_sql_exception");
                MessageBox.Show(sqlexception.Message + "\r\n" + _sql_string, "SQL Exception. Update DB", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        //Clear DB
        internal void ClearDatabase()
        {
            string[] deletearray = { "DistancesTable", "CompaniesCargoTable", "CompaniesInCitysTable", "TrucksTable", "CargoesTable", "TrailerVariantTable", "TrailerDefinitionTable", "CompaniesTable", "CitysTable"};

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
                MessageBox.Show(sqlexception.Message, "SQL Exception. Load all Distances", MessageBoxButtons.OK, MessageBoxIcon.Error);

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

            LogWriter("Paths checked " + rowsupdate.ToString());
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
                        if (SFDependencies != null && SFDependencies.Count() > 0)
                        {
                            List<string> temp = DBDependencies.Except(SFDependencies).ToList();

                            string SQLCommandCMD = "";
                            bool first = true;

                            if (temp != null && temp.Count() > 0)
                            {
                                SQLCommandCMD += "DELETE FROM [Dependencies] WHERE Dependency IN (";

                                foreach (string tempitem in temp)
                                {
                                    if (!first)
                                    {
                                        SQLCommandCMD += " , ";
                                    }
                                    else
                                    {
                                        first = false;
                                    }

                                    int apoIndex = tempitem.IndexOf("'");
                                    string sqlstr = "";

                                    if (apoIndex > -1)
                                        sqlstr = tempitem.Insert(apoIndex, "'");
                                    else
                                        sqlstr = tempitem;

                                    SQLCommandCMD += "'" + sqlstr + "'";
                                }
                                SQLCommandCMD += ")";

                                UpdateDatabase(SQLCommandCMD);
                            }

                            SQLCommandCMD = "INSERT INTO [Dependencies] (Dependency) ";

                            first = true;

                            temp = SFDependencies.Except(DBDependencies).ToList();

                            foreach (string tempitem in temp)
                            {
                                if (!first)
                                {
                                    SQLCommandCMD += " UNION ALL ";
                                }
                                else
                                {
                                    first = false;
                                }
                                
                                int apoIndex = tempitem.IndexOf("'");
                                string sqlstr = "";

                                if (apoIndex > -1)
                                    sqlstr = tempitem.Insert(apoIndex, "'");
                                else
                                    sqlstr = tempitem;

                                SQLCommandCMD += "SELECT '" + sqlstr + "'";
                            }
                            UpdateDatabase(SQLCommandCMD);
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

                LogWriter("Loaded " + totalrecord + " entries from " + _targetTable + " table.");
            }
            catch
            {
                LogWriter("Missing Database.sdf file");
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
                        //ShowStatusMessages("i", "message_database_created");
                    }
                    catch (SqlCeException sqlexception)
                    {
                        //ShowStatusMessages("i", "error_sql_exception");
                        MessageBox.Show(sqlexception.Message, "SQL Exception. Ext Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        //ShowStatusMessages("i", "error_exception");
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

        //
        
        /*
        //Get random number
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
        */
        
        /*
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
        */

    }
}
