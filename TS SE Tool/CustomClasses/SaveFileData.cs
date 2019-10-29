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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace TS_SE_Tool
{
    class SaveFileData
    {
        FormMain MainForm = Application.OpenForms.OfType<FormMain>().Single();

        string SavefilePath { get; set; } //Path to savefile
        DateTime SavefileModifiedTimestamp { get; set; } //Savefile timestamp

        bool FileDecoded { get; set; } //Status of savefile
        int SavefileVersion { get; set; } //Savefile version

        string[] SavefileInMemory { get; set; } //Savefile RAW text data

        //Variables
        //Economy
        List<City> CitiesList { get; set; } //Cities with companies
        List<string> CompaniesList { get; set; } //Companies list
        List<Garages> GaragesList { get; set; } //Garagies
        int InGameTime { get; set; } //Ingame timestamp (hours)
        PlayerData PlayerProfileData { get; set; } //Player profile Data
        List<Color> UserColorsList; //User stored colors
        Dictionary<string, List<string>> GPSbehind, GPSahead, GPSbehindOnline, GPSaheadOnline; //GPS markers
        List<string> DriverPool; //Driver pool
        List<VisitedCity> VisitedCities; //Visited cities
        string LastVisitedCity { get; set; } //Last visited city

        //Player
        Dictionary<string, UserCompanyTruckData> UserTruckDictionary;
        Dictionary<string, UserCompanyDriverData> UserDriverDictionary;
        Dictionary<string, UserCompanyTruckData> UserTrailerDictionary;
        Dictionary<string, List<string>> UserTrailerDefDictionary;

        //Jobs
        List<Cargo> CargoesList; //Cargoes data

        //Events
        private string[] EconomyEventUnitLinkStringList;//List
        private string[,] EconomyEventsTable;//Table

        //Sort Economy events
        private void PrepareEvents()
        {
            for (int i = 0; i < EconomyEventUnitLinkStringList.Length; i++)
            {
                int j = 0;
                while (j < EconomyEventsTable.GetLength(0))
                {
                    if ((EconomyEventUnitLinkStringList[i] == EconomyEventsTable[j, 3]) && (EconomyEventsTable[j, 4] == " param: 0"))
                    {
                        EconomyEventsTable[j, 2] = (InGameTime + (((i + 1) * MainForm.ProgSettingsV.JobPickupTime) * 60)).ToString(); //time
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
    }
}
