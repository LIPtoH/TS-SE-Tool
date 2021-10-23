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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using TS_SE_Tool.Utilities;

namespace TS_SE_Tool
{
    public class SaveFileInfoData
    {
        private string      SaveContainerNameless       { get; set; } = "";

        //Data
        internal string     Name                       { get; set; } = "";     //Save name
        internal string     _Name
        {
            get
            {
                return TextUtilities.FromStringToOutputString(Name);
            }
            set
            {
                Name = TextUtilities.CheckAndClearStringFromQuotes(value);
            }
        }

        public uint         Time                        { get; set; } = 0;          //IngameTime
        public uint         FileTime                    { get; set; } = 0;
        public ushort       Version                     { get; set; } = 0;

        //Info 
        private byte        InfoVersion                 { get; set; } = 0;
        private uint        InfoPlayersExperience       { get; set; } = 0;
        private ushort      InfoUnlockedRecruitments    { get; set; } = 0;
        private ushort      InfoUnlockedDealers         { get; set; } = 0;
        private ushort      InfoVisitedCities           { get; set; } = 0;
        private long        InfoMoneyAccount            { get; set; } = 0;
        private decimal     InfoExploredRatio           { get; set; } = 0.0M;

        //
        internal List<Dependency> Dependencies { get; set; }

        //====
        Dictionary<string, string> unsortedDataDictionary = new Dictionary<string, string>();

        //Methods

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


                    case "save_container":
                        {
                            SaveContainerNameless = dataLine.Split(new char[] { '{' })[0].Trim();
                            break;
                        }

                    case "name":
                        {
                            _Name = dataLine;
                            break;
                        }

                    case "time":
                        {
                            Time = uint.Parse(dataLine);
                            break;
                        }

                    case "file_time":
                        {
                            FileTime = uint.Parse(dataLine);
                            break;
                        }

                    case "version":
                        {
                            Version = ushort.Parse(dataLine);                            
                            break;
                        }

                    case "dependencies":
                        {
                            Dependencies = new List<Dependency>(int.Parse(dataLine));

                            for (int x = 0; x < Dependencies.Capacity; x++)
                            {
                                lineNumber++;
                                lineParts = _fileLines[lineNumber].Split(new char[] { ':' }, 2);

                                string tmpDep = lineParts[1].Trim();
                                tmpDep = tmpDep.Substring(1, tmpDep.Length - 2);

                                Dependencies.Add(new Dependency(tmpDep));
                            }

                            break;
                        }

                    case "info_version":
                        {
                            InfoVersion = byte.Parse(dataLine);
                            break;
                        }

                    case "info_players_experience":
                        {
                            InfoPlayersExperience = uint.Parse(dataLine);
                            break;
                        }

                    case "info_unlocked_recruitments":
                        {
                            InfoUnlockedRecruitments = ushort.Parse(dataLine);
                            break;
                        }

                    case "info_unlocked_dealers":
                        {
                            InfoUnlockedDealers = ushort.Parse(dataLine);
                            break;
                        }

                    case "info_visited_cities":
                        {
                            InfoVisitedCities = ushort.Parse(dataLine);
                            break;
                        }

                    case "info_money_account":
                        {
                            InfoMoneyAccount = long.Parse(dataLine);
                            break;
                        }

                    case "info_explored_ratio":
                        {
                            InfoExploredRatio = NumericUtilities.HexFloatToDecimalFloat(dataLine);
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

        public string GetDataText()
        {
            bool InfoExist55 = false;
            if (Version >= 55 && this.InfoVersion > 0)
                InfoExist55 = true;

            StringBuilder sbResult = new StringBuilder();

            sbResult.AppendLine("SiiNunit");
            sbResult.AppendLine("{");

            sbResult.AppendLine("save_container : " + SaveContainerNameless + " {");
            sbResult.AppendLine(" name: " + _Name);
            sbResult.AppendLine(" time: " + Time.ToString());
            sbResult.AppendLine(" file_time: " + FileTime.ToString());
            sbResult.AppendLine(" version: " + Version.ToString());

            if (InfoExist55)
                sbResult.AppendLine(" info_version: " + this.InfoVersion.ToString());

            sbResult.AppendLine(" dependencies: " + Dependencies.Count.ToString());
                for (int i = 0; i < Dependencies.Capacity; i++)
                {
                    sbResult.AppendLine(" dependencies[" + i.ToString() + "]: \"" + Dependencies[i].Raw + "\"");
                }

            if (InfoExist55)
            {
                sbResult.AppendLine(" info_players_experience: " + InfoPlayersExperience.ToString());
                sbResult.AppendLine(" info_unlocked_recruitments: " + InfoUnlockedRecruitments.ToString());
                sbResult.AppendLine(" info_unlocked_dealers: " + InfoUnlockedDealers.ToString());
                sbResult.AppendLine(" info_visited_cities: " + InfoVisitedCities.ToString());
                sbResult.AppendLine(" info_money_account: " + InfoMoneyAccount.ToString());
                sbResult.AppendLine(" info_explored_ratio: " + NumericUtilities.DecimalFloatToHexFloat(InfoExploredRatio));
            }

            //Add lines with unsorted data
            if (unsortedDataDictionary.Count > 0)
            {
                foreach (KeyValuePair<string, string> record in unsortedDataDictionary)
                {
                    sbResult.AppendLine(" " + record.Key + ": " + record.Value);
                }
            }
            //===

            sbResult.AppendLine("}");
            sbResult.AppendLine();
            sbResult.Append("}");

            return sbResult.ToString();
        }

        public void WriteToStream (StreamWriter _streamWriter)
        {
            _streamWriter.Write(GetDataText());
        }

    }
}
