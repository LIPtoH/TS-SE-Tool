/*
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
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Globalization;

namespace TS_SE_Tool
{
    public class SaveFileProfileData
    {
        private string UserProfileNameless { get; set; } = "";
        //---
        private ushort Face { get; set; } = 0;
        private string Brand { get; set; } = "";
        private string MapPath { get; set; } = "";
        public string Logo { get; set; } = "";
        public string CompanyName { get; set; } = "";
        public bool GenederMale { get; set; } = false;
        public uint CachedExperiencePoints { get; set; } = 0;
        public uint CachedDistance { get; set; } = 0;
        #region UserData
        //user_data
        private uint? SomeTimeUD0 { get; set; } = null;         //0 WoT profile connection date
        private string LicensePlateUD1 { get; set; } = null;    //1 WoT licenseplate
        private string SomeCheckSumUD2 { get; set; } = null;    //2 ???
        private byte? WoTConnectedUD3 { get; set; } = null;     //3 isWoTConnected?
        public decimal RoadsExploredUD4 { get; set; } = 0.0M;   //4 Road explored persentage
        public uint DeliveriesFinishedUD5 { get; set; } = 0;    //5 Finished deliveries
        public uint OwnedTrucksUD6 { get; set; } = 0;           //6 Owned trucks count
        public uint OwnedGaradesSmallUD7 { get; set; } = 0;     //7 Small garages 
        public uint OwnedGaradesLargeUD8 { get; set; } = 0;     //8 Large garages
        public ulong GameTimeSpentUD9 { get; set; } = 0;        //9 Game time spent
        public uint RealTimeSpentUD10 { get; set; } = 0;        //10 Real time spent
        public string CurrentTruckUD11 { get; set; } = "";      //11 Current truck //brand.model
        public List<string> OwnedTruckListUD12;                 //12 Owned trucks //brand.model:count;
        private string SomeUserDataUD13 { get; set; } = "";     //13 ???
        private uint? SomeUserDataUD14 { get; set; } = null;    //14 ??? //0
        private string SomeUserDataUD15 { get; set; } = "";     //15 ??? //production
        public uint OwnedTrailersUD16 { get; set; } = 0;        //16 Owned trailers
        #endregion 
        private List<string> ActiveMods;                        //Count
        private uint Customization { get; set; } = 0;           //FlagFormat
        //cached_stats
        private List<ushort> CachedStats;                       //Some stats ???
        //cached_discovery
        private List<ushort> CachedDiscovery;                   //Some discoveries

        //End
        private byte Version { get; set; } = 0;
        private string OnlineUserName { get; set; } = "";
        private string OnlinePassword { get; set; } = "";
        public string ProfileName { get; set; } = "";
        public uint CreationTime { get; set; } = 0;
        public uint SaveTime { get; set; } = 0;

        public int[] getPlayerLvl()
        {
            int CurrentLVL = 0, lvlthreshhold = 0;
            int[] Result;

            foreach (int lvlstep in Globals.PlayerLevelUps)
            {
                lvlthreshhold += lvlstep;

                if (CachedExperiencePoints < lvlthreshhold)                
                    return Result = new int[] { CurrentLVL, lvlthreshhold};
                                   
                else                
                    CurrentLVL++;                
            }
            
            int finalthreshhold = Globals.PlayerLevelUps[Globals.PlayerLevelUps.Length - 1];

            do
            {
                lvlthreshhold += finalthreshhold;

                if (CachedExperiencePoints < lvlthreshhold)
                    return Result = new int[] { CurrentLVL, lvlthreshhold };
                else
                    CurrentLVL++;
            } while (true);
        }

        public string GetProfileSummary(List<LevelNames> _PlayerLevelNames)
        {
            string SummaryText = "Trucking since: " + DateTimeOffset.FromUnixTimeSeconds(CreationTime).DateTime.ToLocalTime().ToString();

            int playerlvl = getPlayerLvl()[0];
            string playerLvlName = "";

            for (int i = _PlayerLevelNames.Count - 1; i >= 0; i--)
                if (_PlayerLevelNames[i].LevelLimit <= playerlvl)
                {
                    playerLvlName = _PlayerLevelNames[i].LevelName;
                    break;
                }

            SummaryText += "\r\n" + playerLvlName + " (Level " + playerlvl.ToString() + ")";
            SummaryText += "\r\nDistance driven: " + CachedDistance + " km";
            SummaryText += "\r\nRoads explored: " + (RoadsExploredUD4 * 100).ToString("0.00") + "%";
            SummaryText += "\r\nDeliveries finished: " + DeliveriesFinishedUD5;
            SummaryText += "\r\nOwned Garages: small: " + OwnedGaradesSmallUD7 + ",large: " + OwnedGaradesLargeUD8;
            SummaryText += "\r\nOwned Trucks: " + OwnedTrucksUD6;
            SummaryText += "\r\nOwned Trailers: " + OwnedTrailersUD16;
            SummaryText += "\r\nTotal game time spent: " + GameTimeSpentUD9 / 1440 + " days " + Math.Floor(((decimal)(GameTimeSpentUD9 % 1440) / 1440) * 24) + " hour(s)";
            SummaryText += "\r\nPlaying time: " + RealTimeSpentUD10 / 60 + "h " + RealTimeSpentUD10 % 60 + " min";

            return SummaryText;
        }

        public void Prepare(string[] _FileLines)
        {
            string[] chunkOfline;
            char[] CharsToTrim = new char[] { '"' };

            for (int line = 0; line < _FileLines.Length; line++)
            {
                if (_FileLines[line].StartsWith("user_profile :"))
                {
                    chunkOfline = _FileLines[line].Split(new char[] { ' ' });
                    UserProfileNameless = chunkOfline[2];
                    continue;
                }

                if (_FileLines[line].StartsWith(" face:"))
                {
                    chunkOfline = _FileLines[line].Split(new char[] { ' ' });
                    Face = ushort.Parse(chunkOfline[2]);
                    continue;
                }

                if (_FileLines[line].StartsWith(" brand:"))
                {
                    chunkOfline = _FileLines[line].Split(new char[] { ' ' });
                    Brand = chunkOfline[2];
                    continue;
                }

                if (_FileLines[line].StartsWith(" map_path:"))
                {
                    chunkOfline = _FileLines[line].Split(new char[] { ' ' });
                    MapPath = chunkOfline[2];
                    continue;
                }

                if (_FileLines[line].StartsWith(" logo:"))
                {
                    chunkOfline = _FileLines[line].Split(new char[] { ' ' });
                    Logo = chunkOfline[2];
                    continue;
                }

                if (_FileLines[line].StartsWith(" company_name:"))
                {
                    chunkOfline = _FileLines[line].Split(new char[] { ' ' }, 3);
                    string result = "";

                    if (chunkOfline[2].StartsWith("\"") && chunkOfline[2].EndsWith("\""))
                    {
                        string compNameH = chunkOfline[2].Remove(chunkOfline[2].Length - 1, 1).Remove(0, 1);

                        result = Utilities.TextUtilities.FromUtfHexToString(compNameH);
                    }

                    CompanyName = (result == "") ? chunkOfline[2] : result;

                    continue;
                }

                if (_FileLines[line].StartsWith(" male:"))
                {
                    chunkOfline = _FileLines[line].Split(new char[] { ' ' });
                    GenederMale = bool.Parse(chunkOfline[2]);
                    continue;
                }

                if (_FileLines[line].StartsWith(" cached_experience:"))
                {
                    chunkOfline = _FileLines[line].Split(new char[] { ' ' });
                    CachedExperiencePoints = uint.Parse(chunkOfline[2]);
                    continue;
                }

                if (_FileLines[line].StartsWith(" cached_distance:"))
                {
                    chunkOfline = _FileLines[line].Split(new char[] { ' ' });
                    CachedDistance = uint.Parse(chunkOfline[2]);
                    continue;
                }

                if (_FileLines[line].StartsWith(" user_data[0]:"))
                {
                    chunkOfline = _FileLines[line].Split(new char[] { ' ' });
                    SomeTimeUD0 = (chunkOfline[2] != "\"\"") ? uint.Parse(chunkOfline[2].Trim(CharsToTrim)) : (uint?)null;
                    continue;
                }

                if (_FileLines[line].StartsWith(" user_data[1]:"))
                {
                    chunkOfline = _FileLines[line].Split(new char[] { ' ' });
                    LicensePlateUD1 = (chunkOfline[2] != "\"\"") ? chunkOfline[2].Trim(CharsToTrim) : null;
                    continue;
                }

                if (_FileLines[line].StartsWith(" user_data[2]:"))
                {
                    chunkOfline = _FileLines[line].Split(new char[] { ' ' });
                    SomeCheckSumUD2 = (chunkOfline[2] != "\"\"") ? chunkOfline[2].Trim(CharsToTrim) : null;
                    continue;
                }

                if (_FileLines[line].StartsWith(" user_data[3]:"))
                {
                    chunkOfline = _FileLines[line].Split(new char[] { ' ' });
                    WoTConnectedUD3 = (chunkOfline[2] != "\"\"") ? byte.Parse(chunkOfline[2].Trim(CharsToTrim)) : (byte?)null;
                    continue;
                }

                if (_FileLines[line].StartsWith(" user_data[4]:"))
                {
                    chunkOfline = _FileLines[line].Split(new char[] { ' ' });
                    RoadsExploredUD4 = decimal.Parse(chunkOfline[2].Trim(CharsToTrim), System.Globalization.CultureInfo.InvariantCulture);
                    continue;
                }

                if (_FileLines[line].StartsWith(" user_data[5]:"))
                {
                    chunkOfline = _FileLines[line].Split(new char[] { ' ' });
                    DeliveriesFinishedUD5 = uint.Parse(chunkOfline[2]);
                    continue;
                }

                if (_FileLines[line].StartsWith(" user_data[6]:"))
                {
                    chunkOfline = _FileLines[line].Split(new char[] { ' ' });
                    OwnedTrucksUD6 = uint.Parse(chunkOfline[2]);
                    continue;
                }

                if (_FileLines[line].StartsWith(" user_data[7]:"))
                {
                    chunkOfline = _FileLines[line].Split(new char[] { ' ' });
                    OwnedGaradesSmallUD7 = uint.Parse(chunkOfline[2]);
                    continue;
                }

                if (_FileLines[line].StartsWith(" user_data[8]:"))
                {
                    chunkOfline = _FileLines[line].Split(new char[] { ' ' });
                    OwnedGaradesLargeUD8 = uint.Parse(chunkOfline[2]);
                    continue;
                }

                if (_FileLines[line].StartsWith(" user_data[9]:"))
                {
                    chunkOfline = _FileLines[line].Split(new char[] { ' ' });
                    GameTimeSpentUD9 = uint.Parse(chunkOfline[2]);
                    continue;
                }

                if (_FileLines[line].StartsWith(" user_data[10]:"))
                {
                    chunkOfline = _FileLines[line].Split(new char[] { ' ' });
                    RealTimeSpentUD10 = uint.Parse(chunkOfline[2]);
                    continue;
                }

                if (_FileLines[line].StartsWith(" user_data[11]:"))
                {
                    chunkOfline = _FileLines[line].Split(new char[] { ' ' });
                    CurrentTruckUD11 = chunkOfline[2].Trim(CharsToTrim);
                    continue;
                }

                if (_FileLines[line].StartsWith(" user_data[12]:"))
                {
                    chunkOfline = _FileLines[line].Split(new char[] { ' ' });
                    OwnedTruckListUD12 = chunkOfline[2].Trim(CharsToTrim).Split(new char[] { ',' }).ToList();
                    continue;
                }

                if (_FileLines[line].StartsWith(" user_data[13]:"))
                {
                    chunkOfline = _FileLines[line].Split(new char[] { ' ' });
                    SomeUserDataUD13 = chunkOfline[2].Trim(CharsToTrim);
                    continue;
                }

                if (_FileLines[line].StartsWith(" user_data[14]:"))
                {
                    chunkOfline = _FileLines[line].Split(new char[] { ' ' });
                    SomeUserDataUD14 = (chunkOfline[2] != "\"\"") ? uint.Parse(chunkOfline[2].Trim(CharsToTrim)) : (uint?)null;
                    continue;
                }

                if (_FileLines[line].StartsWith(" user_data[15]:"))
                {
                    chunkOfline = _FileLines[line].Split(new char[] { ' ' });
                    SomeUserDataUD15 = chunkOfline[2].Trim(CharsToTrim);
                    continue;
                }

                if (_FileLines[line].StartsWith(" user_data[16]:"))
                {
                    chunkOfline = _FileLines[line].Split(new char[] { ' ' });
                    OwnedTrailersUD16 = uint.Parse(chunkOfline[2]);
                    continue;
                }

                if (_FileLines[line].StartsWith(" active_mods:"))
                {
                    chunkOfline = _FileLines[line].Split(new char[] { ' ' });
                    ActiveMods = new List<string>(int.Parse(chunkOfline[2]));

                    for (int x = 0; x < ActiveMods.Capacity; x++)
                    {
                        line++;
                        chunkOfline = _FileLines[line].Split(new char[] { ' ' }, 3);
                        ActiveMods.Add(chunkOfline[2]);
                    }
                    continue;
                }

                if (_FileLines[line].StartsWith(" customization:"))
                {
                    chunkOfline = _FileLines[line].Split(new char[] { ' ' });
                    Customization = uint.Parse(chunkOfline[2]);
                    continue;
                }

                if (_FileLines[line].StartsWith(" cached_stats:"))
                {
                    chunkOfline = _FileLines[line].Split(new char[] { ' ' });
                    CachedStats = new List<ushort>(int.Parse(chunkOfline[2]));

                    for (int x = 0; x < CachedStats.Capacity; x++)
                    {
                        line++;
                        chunkOfline = _FileLines[line].Split(new char[] { ' ' });
                        CachedStats.Add(ushort.Parse(chunkOfline[2]));
                    }
                    continue;
                }

                if (_FileLines[line].StartsWith(" cached_discovery:"))
                {
                    chunkOfline = _FileLines[line].Split(new char[] { ' ' });
                    CachedDiscovery = new List<ushort>(int.Parse(chunkOfline[2]));

                    for (int x = 0; x < CachedDiscovery.Capacity; x++)
                    {
                        line++;
                        chunkOfline = _FileLines[line].Split(new char[] { ' ' });
                        CachedDiscovery.Add(ushort.Parse(chunkOfline[2]));
                    }
                    continue;
                }

                if (_FileLines[line].StartsWith(" version:"))
                {
                    chunkOfline = _FileLines[line].Split(new char[] { ' ' });
                    Version = byte.Parse(chunkOfline[2]);
                    continue;
                }

                if (_FileLines[line].StartsWith(" online_user_name:"))
                {
                    chunkOfline = _FileLines[line].Split(new char[] { ' ' });
                    OnlineUserName = chunkOfline[2].Trim(CharsToTrim);
                    continue;
                }

                if (_FileLines[line].StartsWith(" online_password:"))
                {
                    chunkOfline = _FileLines[line].Split(new char[] { ' ' });
                    OnlinePassword = chunkOfline[2].Trim(CharsToTrim);
                    continue;
                }

                if (_FileLines[line].StartsWith(" profile_name:"))
                {
                    chunkOfline = _FileLines[line].Split(new char[] { ' ' }, 3);

                    string result = null;

                    if (chunkOfline[2].StartsWith("\"") && chunkOfline[2].EndsWith("\""))
                    {
                        string tmp = chunkOfline[2].Remove(chunkOfline[2].Length - 1, 1).Remove(0, 1);

                        result = Utilities.TextUtilities.FromUtfHexToString(tmp);
                    }

                    ProfileName = (result == "") ? chunkOfline[2] : result;

                    continue;                    
                }

                if (_FileLines[line].StartsWith(" creation_time:"))
                {
                    chunkOfline = _FileLines[line].Split(new char[] { ' ' });
                    CreationTime = uint.Parse(chunkOfline[2]);
                    continue;
                }

                if (_FileLines[line].StartsWith(" save_time:"))
                {
                    chunkOfline = _FileLines[line].Split(new char[] { ' ' });
                    SaveTime = uint.Parse(chunkOfline[2]);
                    continue;
                }

                if (_FileLines[line].StartsWith("}"))
                {
                    break;
                }
            }
        }

        public string GetDataText()
        {
            string OutputText = "SiiNunit\r\n{\r\n";

            OutputText += "user_profile : " + UserProfileNameless + " {\r\n";
            OutputText += " face: " + Face.ToString() + "\r\n";
            OutputText += " brand: " + Brand + "\r\n";
            OutputText += " map_path: " + MapPath + "\r\n";
            OutputText += " logo: " + Logo + "\r\n";
            OutputText += " company_name: " + (!CheckStringContainsUnescape(CompanyName) ? CompanyName : "\"" + CompanyName + "\"") + "\r\n";
            OutputText += " male: " + GenederMale.ToString().ToLower() + "\r\n";
            OutputText += " cached_experience: " + CachedExperiencePoints.ToString() + "\r\n";
            OutputText += " cached_distance: " + CachedDistance.ToString() + "\r\n";

            if (Version == 4)
            {
                OutputText += " version: " + Version.ToString() + "\r\n";
                OutputText += " online_user_name: " + (!string.IsNullOrEmpty(OnlineUserName) ? OnlineUserName : "\"\"") + "\r\n";
                OutputText += " online_password: " + (!string.IsNullOrEmpty(OnlinePassword) ? OnlinePassword : "\"\"") + "\r\n";
            }

            OutputText += " user_data: 17\r\n";
            OutputText += " user_data[0]: " + ((SomeTimeUD0 != null) ? SomeTimeUD0.ToString() : "\"\"") + "\r\n";
            OutputText += " user_data[1]: " + ((LicensePlateUD1 != null) ? "\"" + LicensePlateUD1 + "\"" : "\"\"") + "\r\n";
            OutputText += " user_data[2]: " + ((SomeCheckSumUD2 != null) ? SomeCheckSumUD2 : "\"\"") + "\r\n";
            OutputText += " user_data[3]: " + ((WoTConnectedUD3 != null) ? WoTConnectedUD3.ToString() : "\"\"") + "\r\n";
            OutputText += " user_data[4]: \"" + RoadsExploredUD4.ToString(CultureInfo.InvariantCulture) + "\"\r\n";
            OutputText += " user_data[5]: " + DeliveriesFinishedUD5.ToString() + "\r\n";
            OutputText += " user_data[6]: " + OwnedTrucksUD6.ToString() + "\r\n";
            OutputText += " user_data[7]: " + OwnedGaradesSmallUD7.ToString() + "\r\n";
            OutputText += " user_data[8]: " + OwnedGaradesLargeUD8.ToString() + "\r\n";
            OutputText += " user_data[9]: " + GameTimeSpentUD9.ToString() + "\r\n";
            OutputText += " user_data[10]: " + RealTimeSpentUD10.ToString() + "\r\n";
            OutputText += " user_data[11]: \"" + CurrentTruckUD11 + "\"" + "\r\n";
            OutputText += " user_data[12]: \"" + string.Join(",", OwnedTruckListUD12) + "\"" + "\r\n";
            OutputText += " user_data[13]: " + (!string.IsNullOrEmpty(SomeUserDataUD13) ? SomeUserDataUD13 : "\"\"") + "\r\n";
            OutputText += " user_data[14]: " + ((SomeUserDataUD14 != null) ? SomeUserDataUD14.ToString() : "\"\"") + "\r\n";
            OutputText += " user_data[15]: " + (!string.IsNullOrEmpty(SomeUserDataUD15) ? SomeUserDataUD15 : "\"\"") + "\r\n";
            OutputText += " user_data[16]: " + OwnedTrailersUD16.ToString() + "\r\n";

            OutputText += " active_mods: " + ActiveMods.Capacity.ToString() + "\r\n";
            for (int i = 0; i < ActiveMods.Capacity; i++)
            {
                OutputText += " active_mods[" + i.ToString() + "]: " + ActiveMods[i].ToString() + "\r\n";
            }

            OutputText += " customization: " + Customization.ToString() + "\r\n";

            OutputText += " cached_stats: " + CachedStats.Capacity.ToString() + "\r\n";
            for (int i = 0; i < CachedStats.Capacity; i++)
            {
                OutputText += " cached_stats[" + i.ToString() + "]: " + CachedStats[i].ToString() + "\r\n";
            }

            OutputText += " cached_discovery: " + CachedDiscovery.Capacity.ToString() + "\r\n";
            for (int i = 0; i < CachedDiscovery.Capacity; i++)
            {
                OutputText += " cached_discovery[" + i.ToString() + "]: " + CachedDiscovery[i].ToString() + "\r\n";
            }

            if (Version == 5)
            {
                OutputText += " version: " + Version.ToString() + "\r\n";
                OutputText += " online_user_name: " + (!string.IsNullOrEmpty(OnlineUserName) ? OnlineUserName : "\"\"") + "\r\n"; 
                OutputText += " online_password: " + (!string.IsNullOrEmpty(OnlinePassword) ? OnlinePassword : "\"\"") + "\r\n"; 
            }

            OutputText += " profile_name: " + (!CheckStringContainsUnescape(ProfileName) ? ProfileName : "\"" + ProfileName + "\"") + "\r\n";
            OutputText += " creation_time: " + CreationTime.ToString() + "\r\n";
            OutputText += " save_time: " + SaveTime.ToString() + "\r\n";

            OutputText += "}\r\n\r\n}";

            return OutputText;
        }

        private bool CheckStringContainsUnescape(string _input)
        {
            if (_input.Contains(' ') || _input != System.Text.RegularExpressions.Regex.Unescape(_input))
                return true;
            else
                return false;
        }

        public void WriteToStream(StreamWriter _streamWriter)
        {
            _streamWriter.Write(GetDataText());
        }
    }
}
