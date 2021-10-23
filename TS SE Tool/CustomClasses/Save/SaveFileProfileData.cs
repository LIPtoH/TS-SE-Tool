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
using System.Reflection;
using System.Text;
using TS_SE_Tool.Utilities;

namespace TS_SE_Tool
{
    public class SaveFileProfileData
    {
        internal string  UserProfileNameless    { get; set; } = "";

        //---
        public bool     GenederMale         { get; set; } = false;
        internal ushort Face                { get; set; } = 0;
        internal string Brand               { get; set; } = "";

        public string   Logo                { get; set; } = "";

        internal string CompanyName         { get; set; } = "";
        internal string _CompanyName
        {
            get
            {
                return TextUtilities.FromStringToOutputString(CompanyName);
            }
            set
            {
                CompanyName = TextUtilities.CheckAndClearStringFromQuotes(value);
            }
        }

        //---
        internal string MapPath             { get; set; } = "";

        //---
        public uint     CachedExperiencePoints  { get; set; } = 0;
        public uint     CachedDistance          { get; set; } = 0;

        //---
        #region UserData

        internal uint       UserDataSize            { get; set; } = 0;

        internal uint?      ud0_WoTTime             { get; set; } = null;   //0 WoT profile connection date
        internal string     ud1_WoTLicensePlate     { get; set; } = "";     //1 WoT licenseplate
        internal string     ud2_SomeCheckSum        { get; set; } = "";     //2 ???
        internal byte?      ud3_WoTConnected        { get; set; } = null;   //3 isWoTConnected?
        public decimal      ud4_RoadsExplored       { get; set; } = 0.0M;   //4 Road explored persentage
        public uint         ud5_DeliveriesFinished  { get; set; } = 0;      //5 Finished deliveries
        public uint         ud6_OwnedTrucks         { get; set; } = 0;      //6 Owned trucks count
        public uint         ud7_OwnedGaradesSmall   { get; set; } = 0;      //7 Small garages 
        public uint         ud8_OwnedGaradesLarge   { get; set; } = 0;      //8 Large garages
        public ulong        ud9_GameTimeSpent       { get; set; } = 0;      //9 Game time spent
        public uint         ud10_RealTimeSpent      { get; set; } = 0;      //10 Real time spent
        public string       ud11_CurrentTruck       { get; set; } = "";     //11 Current truck //brand.model
        public List<string> ud12_OwnedTruckList = new List<string>();       //12 Owned trucks //brand.model:count,brand.model:count,...;
        internal string     ud13_SomeUserData       { get; set; } = "";     //13 ???
        internal uint?      ud14_SomeUserData       { get; set; } = null;   //14 ??? //0
        internal string     ud15_SomeUserData       { get; set; } = "";     //15 ??? //production
        public uint         ud16_OwnedTrailers      { get; set; } = 0;      //16 Owned trailers

        #region user data backend

        private string[] user_data_array;

        private string user_data_0
        {
            get {
                return ((ud0_WoTTime != null) ? ud0_WoTTime.ToString() : "\"\"");
            }
            set {
                ud0_WoTTime = (value != "\"\"") ? uint.Parse(value.Trim(charsToTrim)) : (uint?)null;
            }
        }

        private string user_data_1
        {
            get {
                return (!string.IsNullOrEmpty(ud1_WoTLicensePlate) ? "\"" + ud1_WoTLicensePlate + "\"" : "\"\"");
            }
            set {
                ud1_WoTLicensePlate = (value != "\"\"") ? value.Trim(charsToTrim) : "";
            }
        }

        private string user_data_2
        {
            get { return (!string.IsNullOrEmpty(ud2_SomeCheckSum) ? ud2_SomeCheckSum : "\"\""); }
            set
            {
                ud2_SomeCheckSum = (value != "\"\"") ? value.Trim(charsToTrim) : "";
            }
        }

        private string user_data_3
        {
            get { return ((ud3_WoTConnected != null) ? ud3_WoTConnected.ToString() : "\"\""); }
            set
            {
                ud3_WoTConnected = (value != "\"\"") ? byte.Parse(value.Trim(charsToTrim)) : (byte?)null;
            }
        }

        private string user_data_4
        {
            get { return "\"" + ud4_RoadsExplored.ToString(NumberFormatInfo.InvariantInfo) + "\""; }
            set
            {
                ud4_RoadsExplored = decimal.Parse (value.Trim(charsToTrim), CultureInfo.InvariantCulture);
            }
        }

        private string user_data_5
        {
            get { return ud5_DeliveriesFinished.ToString(); }
            set
            {
                ud5_DeliveriesFinished = uint.Parse(value);
            }
        }

        private string user_data_6
        {
            get { return ud6_OwnedTrucks.ToString(); }
            set
            {
                ud6_OwnedTrucks = uint.Parse(value);
            }
        }

        private string user_data_7
        {
            get { return ud7_OwnedGaradesSmall.ToString(); }
            set
            {
                ud7_OwnedGaradesSmall = uint.Parse(value);
            }
        }

        private string user_data_8
        {
            get { return ud8_OwnedGaradesLarge.ToString(); }
            set
            {
                ud8_OwnedGaradesLarge = uint.Parse(value);
            }
        }

        private string user_data_9
        {
            get { return ud9_GameTimeSpent.ToString(); }
            set
            {
                ud9_GameTimeSpent = uint.Parse(value);
            }
        }

        private string user_data_10
        {
            get { return ud10_RealTimeSpent.ToString(); }
            set
            {
                ud10_RealTimeSpent = uint.Parse(value);
            }
        }

        private string user_data_11
        {
            get
            {
                return (!string.IsNullOrEmpty(ud15_SomeUserData) ? "\"" + ud11_CurrentTruck + "\"" : "\"\"");
            }
            set
            {
                ud11_CurrentTruck = (value != "\"\"") ? value.Trim(charsToTrim) : "";
            }
        }

        private string user_data_12
        {
            get { return ((ud12_OwnedTruckList.Count > 0) ? "\"" + String.Join(",", ud12_OwnedTruckList) + "\"" : "\"\""); }
            set
            {
                ud12_OwnedTruckList = value.Trim(charsToTrim).Replace(" ", "").Split(new char[] { ',' }).ToList();
            }
        }

        private string user_data_13
        {
            get
            {
                return (!string.IsNullOrEmpty(ud13_SomeUserData) ? ud13_SomeUserData : "\"\"");
            }
            set
            {
                ud13_SomeUserData = value.Trim(charsToTrim);
            }
        }

        private string user_data_14
        {
            get {
                return ((ud14_SomeUserData != null) ? ud14_SomeUserData.ToString() : "\"\""); }
            set
            {
                ud14_SomeUserData = (value != "\"\"") ? uint.Parse(value.Trim(charsToTrim)) : (uint?)null;
            }
        }

        private string user_data_15
        {
            get {
                return (!string.IsNullOrEmpty(ud15_SomeUserData) ? ud15_SomeUserData : "\"\""); }
            set
            {
                ud15_SomeUserData = value.Trim(charsToTrim);
            }
        }

        private string user_data_16
        {
            get { return ud16_OwnedTrailers.ToString(); }
            set
            {
                ud16_OwnedTrailers = uint.Parse(value);
            }
        }

        #endregion

        #endregion

        internal uint   Customization   { get; set; } = 0;      //in bit flag format

        internal List<string>   ActiveMods;                     //Count

        internal List<ushort>   CachedStats;                    //cached_stats

        internal List<ushort>   CachedDiscovery;                //cached_discovery

        //End
        internal byte   Version         { get; set; } = 0;      //profile data format version

        internal string OnlineUserName  { get; set; } = "";
        internal string _OnlineUserName
        {
            get
            {
                return TextUtilities.FromStringToOutputString(OnlineUserName);
            }
            set
            {
                OnlineUserName = TextUtilities.CheckAndClearStringFromQuotes(value);
            }
        }

        internal string OnlinePassword  { get; set; } = "";
        internal string _OnlinePassword
        {
            get
            {
                return TextUtilities.FromStringToOutputString(OnlinePassword);
            }
            set
            {
                OnlinePassword = TextUtilities.CheckAndClearStringFromQuotes(value);
            }
        }

        internal string ProfileName     { get; set; } = "";
        internal string _ProfileName
        {
            get
            {
                return TextUtilities.FromStringToOutputString(ProfileName);
            }
            set
            {
                ProfileName = TextUtilities.CheckAndClearStringFromQuotes(value);
            }
        }

        public uint     CreationTime    { get; set; } = 0;
        public uint     SaveTime        { get; set; } = 0;

        //====
        Dictionary<string, string> unsortedDataDictionary = new Dictionary<string, string>();

        //====
        private char[] charsToTrim = new char[] { '"' };

        //
        public object this[string propertyName]
        {
            get { return this.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).GetValue(this, null); }
            set { this.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).SetValue(this, value, null); }
        }

        //Methods
        //
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

        public string getPlayerLvlName(List<LevelNames> _playerLevelNames, int _playerlvl)
        {
            for (int i = _playerLevelNames.Count - 1; i >= 0; i--)
                if (_playerLevelNames[i].LevelLimit <= _playerlvl)
                {
                    return _playerLevelNames[i].LevelName;
                }
            return "";
        }

        public string getProfileSummary(List<LevelNames> _playerLevelNames)
        {
            List<string> _newText = new List<string>();

            int _playerlvl = getPlayerLvl()[0];
            string _playerLvlName = getPlayerLvlName(_playerLevelNames, _playerlvl);

            _newText.Add("Trucking since: " + DateTimeOffset.FromUnixTimeSeconds(CreationTime).DateTime.ToLocalTime().ToString());
            _newText.Add(_playerLvlName + " (Level " + _playerlvl.ToString() + ")");
            _newText.Add("Distance driven: " + CachedDistance + " km");
            _newText.Add("Roads explored: " + (ud4_RoadsExplored * 100).ToString("0.00") + "%");
            _newText.Add("Deliveries finished: " + ud5_DeliveriesFinished);
            _newText.Add("Owned Garages: small: " + ud7_OwnedGaradesSmall + ", large: " + ud8_OwnedGaradesLarge);
            _newText.Add("Owned Trucks: " + ud6_OwnedTrucks);
            _newText.Add("Owned Trailers: " + ud16_OwnedTrailers);
            _newText.Add("Total game time spent: " + ud9_GameTimeSpent / 1440 + " day(s) " + Math.Floor(((decimal)(ud9_GameTimeSpent % 1440) / 1440) * 24) + " hour(s)");
            _newText.Add("Playing time: " + ud10_RealTimeSpent / 60 + " h " + ud10_RealTimeSpent % 60 + " min");

            StringBuilder sbResult = new StringBuilder();

            foreach(string _line in _newText)
                sbResult.AppendLine(_line);

            return sbResult.ToString();
        }

        //
        public void ProcessData(string[] _fileLines)
        {
            string[] lineParts;
            string currentLine = "";
            string tagLine = "", dataLine = "";

            byte exitLoopMarker = 2;

            for (int lineNumber = 0; lineNumber < _fileLines.Length; lineNumber++)
            {
                currentLine = _fileLines[lineNumber].Trim();

                if (currentLine.Contains(':'))
                {
                    string[] splittedLine = currentLine.Split(new char[] { ':' }, 2);

                    tagLine = splittedLine[0].Trim();
                    dataLine = splittedLine[1].Trim();
                }
                else
                {
                    tagLine = currentLine.Trim();
                    dataLine = "";
                }

                switch (tagLine)
                {
                    case "SiiNunit":
                    case "":
                        {
                            break;
                        }

                    case "{":
                        {
                            break;
                        }
                    case "}":
                        {
                            --exitLoopMarker;

                            if (exitLoopMarker <= 0)
                                goto endOfProcessData;

                            break;
                        }

                    case "user_profile":
                        {
                            UserProfileNameless = dataLine.Split(new char[] { '{' })[0].Trim();
                            break;
                        }

                    case "face":
                        {
                            Face = ushort.Parse(dataLine);
                            break;
                        }

                    case "brand":
                        {
                            Brand = dataLine;
                            break;
                        }

                    case "map_path":
                        {
                            MapPath = dataLine;
                            break;
                        }

                    case "logo":
                        {
                            Logo = dataLine;
                            break;
                        }

                    case "company_name":
                        {
                            _CompanyName = dataLine;
                            break;
                        }

                    case "male":
                        {
                            GenederMale = bool.Parse(dataLine);
                            break;
                        }

                    case "cached_experience":
                        {
                            CachedExperiencePoints = uint.Parse(dataLine);
                            break;
                        }

                    case "cached_distance":
                        {
                            CachedDistance = uint.Parse(dataLine);
                            break;
                        }

                    case "user_data":
                        {
                            UserDataSize = uint.Parse(dataLine);

                            user_data_array = new string[UserDataSize];

                            for (int i = 0; i < UserDataSize; i++)
                            {
                                lineNumber++;
                                lineParts = _fileLines[lineNumber].Split(new char[] { ':' }, 2);

                                string udNumber = lineParts[0].Split(new char[] { '[', ']' }, 3)[1];
                                string udValue = lineParts[1].Trim();

                                string propertyName = "user_data_" + i.ToString();

                                if (this.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic) != null)
                                    this[propertyName] = udValue;

                                user_data_array[int.Parse(udNumber)] = udValue;
                            }
                            break;
                        }

                    case "active_mods":
                        {
                            ActiveMods = new List<string>(int.Parse(dataLine));

                            for (int x = 0; x < ActiveMods.Capacity; x++)
                            {
                                lineNumber++;
                                lineParts = _fileLines[lineNumber].Split(new char[] { ':' }, 2);
                                ActiveMods.Add(lineParts[1].Trim());
                            }
                            break;
                        }

                    case "customization":
                        {
                            Customization = uint.Parse(dataLine);
                            break;
                        }

                    case "cached_stats":
                        {
                            CachedStats = new List<ushort>(int.Parse(dataLine));

                            for (int x = 0; x < CachedStats.Capacity; x++)
                            {
                                lineNumber++;
                                lineParts = _fileLines[lineNumber].Split(new char[] { ':' });
                                CachedStats.Add(ushort.Parse(lineParts[1].Trim()));
                            }
                            break;
                        }

                    case "cached_discovery":
                        {
                            CachedDiscovery = new List<ushort>(int.Parse(dataLine));

                            for (int x = 0; x < CachedDiscovery.Capacity; x++)
                            {
                                lineNumber++;
                                lineParts = _fileLines[lineNumber].Split(new char[] { ':' });
                                CachedDiscovery.Add(ushort.Parse(lineParts[1].Trim()));
                            }
                            break;
                        }

                    case "version":
                        {

                            Version = byte.Parse(dataLine);
                            break;
                        }
                        
                    case "online_user_name":
                        {
                            _OnlineUserName = dataLine;
                            break;
                        }

                    case "online_password":
                        {
                            _OnlinePassword = dataLine;
                            break;
                        }

                    case "profile_name":
                        {
                            _ProfileName = dataLine;
                            break;
                        }

                    case "creation_time":
                        {
                            CreationTime = uint.Parse(dataLine);
                            break;
                        }

                    case "save_time":
                        {
                            SaveTime = uint.Parse(dataLine);
                            break;
                        }
                        
                    default:
                        {
                            unsortedDataDictionary.Add(tagLine, dataLine);
                            break;
                        }
                }
            }

            endOfProcessData:;
        }

        public string GetTextFileFormat()
        {
            StringBuilder sbResult = new StringBuilder();

            sbResult.AppendLine("SiiNunit");
            sbResult.AppendLine("{");

            sbResult.AppendLine("user_profile : " + UserProfileNameless + " {");
            sbResult.AppendLine(" face: " + Face.ToString());
            sbResult.AppendLine(" brand: " + Brand);
            sbResult.AppendLine(" map_path: " + MapPath);
            sbResult.AppendLine(" logo: " + Logo);
            sbResult.AppendLine(" company_name: " + _CompanyName);
            sbResult.AppendLine(" male: " + GenederMale.ToString().ToLower());
            sbResult.AppendLine(" cached_experience: " + CachedExperiencePoints.ToString());
            sbResult.AppendLine(" cached_distance: " + CachedDistance.ToString());

            bool verCheck4 = (new sbyte[] { 4 }).Any(x => x == Version);
            if (verCheck4)
                sbResult.AppendLine(VerOnline());
            
            sbResult.AppendLine(" user_data: " + UserDataSize.ToString());
                for (int i = 0; i < UserDataSize; i++)
                {
                    string propertyName = "user_data_" + i.ToString();

                    if (this.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic) != null)
                        sbResult.AppendLine(" user_data[" + i.ToString() + "]: " + this[propertyName]);
                    else                    
                        sbResult.AppendLine(user_data_array[i]);                    
                }

            sbResult.AppendLine(" active_mods: " + ActiveMods.Capacity.ToString());
                for (int i = 0; i < ActiveMods.Capacity; i++)
                {
                    sbResult.AppendLine(" active_mods[" + i.ToString() + "]: " + ActiveMods[i].ToString());
                }

            sbResult.AppendLine(" customization: " + Customization.ToString());

            sbResult.AppendLine(" cached_stats: " + CachedStats.Capacity.ToString());
                for (int i = 0; i < CachedStats.Capacity; i++)
                {
                    sbResult.AppendLine(" cached_stats[" + i.ToString() + "]: " + CachedStats[i].ToString());
                }

            sbResult.AppendLine(" cached_discovery: " + CachedDiscovery.Capacity.ToString());
                for (int i = 0; i < CachedDiscovery.Capacity; i++)
                {
                    sbResult.AppendLine(" cached_discovery[" + i.ToString() + "]: " + CachedDiscovery[i].ToString());
                }

            bool verCheck5 = (new sbyte[] { 5, 6 }).Any(x => x == Version);
            if (verCheck5 || !verCheck4)            
                sbResult.AppendLine(VerOnline());

            sbResult.AppendLine(" profile_name: " + _ProfileName);
            sbResult.AppendLine(" creation_time: " + CreationTime.ToString());
            sbResult.AppendLine(" save_time: " + SaveTime.ToString());

            //Add lines with unsorted data
            if (unsortedDataDictionary.Count > 0)
            {
                foreach( KeyValuePair<string, string> record  in unsortedDataDictionary)
                {
                    sbResult.AppendLine(" " + record.Key + ": " + record.Value);
                }
            }
            //===

            sbResult.AppendLine("}");
            sbResult.AppendLine();
            sbResult.Append("}");

            return sbResult.ToString();

            string VerOnline()
            {
                StringBuilder sbVerOnline = new StringBuilder();

                sbVerOnline.AppendLine(" version: " + Version.ToString());
                sbVerOnline.AppendLine(" online_user_name: " + _OnlineUserName);
                sbVerOnline.Append(" online_password: " + _OnlinePassword);

                return sbVerOnline.ToString();
            }
        }

        public void WriteToStream(StreamWriter _streamWriter)
        {
            _streamWriter.Write(GetTextFileFormat());
        }
    }
}
