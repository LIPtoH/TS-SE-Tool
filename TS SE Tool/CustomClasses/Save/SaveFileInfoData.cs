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
using TS_SE_Tool.Save.DataFormat;
using TS_SE_Tool.Save.Items;

namespace TS_SE_Tool
{
    class SaveFileInfoData : SiiNBlockCore
    {
        private string SaveContainerNameless { get; set; } = "";

        //Data
        internal SCS_String Name { get; set; } = "";

        public uint Time { get; set; } = 0;
        public uint FileTime { get; set; } = 0;
        public ushort Version { get; set; } = 0;

        //Info 
        private byte InfoVersion { get; set; } = 0;
        private uint InfoPlayersExperience { get; set; } = 0;
        private ushort InfoUnlockedRecruitments { get; set; } = 0;
        private ushort InfoUnlockedDealers { get; set; } = 0;
        private ushort InfoVisitedCities { get; set; } = 0;
        private long InfoMoneyAccount { get; set; } = 0;
        private SCS_Float InfoExploredRatio { get; set; } = 0.0F;

        //
        internal List<Dependency> Dependencies { get; set; } = new List<Dependency>();

        //
        int unsortedOrder = 0;

        Dictionary<int, List<string>> unsortedDataDict = new Dictionary<int, List<string>>();

        //Methods
        public void ProcessData(string[] _fileLines)
        {
            string currentLine = "";
            string tagLine = "", dataLine = "";

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
                try
                {
                    switch (tagLine)
                    {
                        case "SiiNunit":
                            {
                                unsortedDataDict.Add(unsortedOrder, new List<string>());
                                break;
                            }
                        case "":
                        case "{":
                            {
                                break;
                            }
                        case "}":
                            {
                                unsortedOrder++;
                                unsortedDataDict.Add(unsortedOrder, new List<string>());
                                break;
                            }

                        case "save_container":
                            {
                                unsortedOrder++;
                                unsortedDataDict.Add(unsortedOrder, new List<string>());

                                SaveContainerNameless = dataLine.Split(new char[] { '{' })[0].Trim();
                                break;
                            }

                        case "name":
                            {
                                Name = dataLine;
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
                                Dependencies.Capacity = int.Parse(dataLine);
                                break;
                            }

                        case var s when s.StartsWith("dependencies["):
                            {
                                Dependencies.Add(new Dependency(dataLine));
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
                                InfoExploredRatio = NumericUtilities.HexFloatToSingleFloat(dataLine);
                                break;
                            }

                        default:
                            {
                                unsortedDataDict[unsortedOrder].Add(currentLine);

                                IO_Utilities.ErrorLogWriter(WriteErrorMsg(tagLine, dataLine));
                                break;
                            }
                    }
                }
                catch (Exception ex)
                {
                    IO_Utilities.ErrorLogWriter(WriteErrorMsg(ex.Message, tagLine, dataLine));
                    break;
                }

            }
        }

        public string PrintOut()
        {
            int unsortedOrder = 0;

            bool InfoExist55 = false;
            if (Version >= 55 && this.InfoVersion > 0)
                InfoExist55 = true;

            StringBuilder sbResult = new StringBuilder();

            sbResult.AppendLine("SiiNunit");
            sbResult.AppendLine("{");

            writeUnsortedLines();

            sbResult.AppendLine("save_container : " + SaveContainerNameless + " {");
            sbResult.AppendLine(" name: " + Name);
            sbResult.AppendLine(" time: " + Time.ToString());
            sbResult.AppendLine(" file_time: " + FileTime.ToString());
            sbResult.AppendLine(" version: " + Version.ToString());

            if (InfoExist55)
                sbResult.AppendLine(" info_version: " + this.InfoVersion.ToString());

            sbResult.AppendLine(" dependencies: " + Dependencies.Count);
            for (int i = 0; i < Dependencies.Count; i++)
                sbResult.AppendLine(" dependencies[" + i + "]: " + Dependencies[i].Raw.ToString());

            if (InfoExist55)
            {
                sbResult.AppendLine(" info_players_experience: " + InfoPlayersExperience.ToString());
                sbResult.AppendLine(" info_unlocked_recruitments: " + InfoUnlockedRecruitments.ToString());
                sbResult.AppendLine(" info_unlocked_dealers: " + InfoUnlockedDealers.ToString());
                sbResult.AppendLine(" info_visited_cities: " + InfoVisitedCities.ToString());
                sbResult.AppendLine(" info_money_account: " + InfoMoneyAccount.ToString());
                sbResult.AppendLine(" info_explored_ratio: " + InfoExploredRatio.ToString());
            }

            writeUnsortedLines();

            sbResult.AppendLine("}");

            writeUnsortedLines();

            sbResult.AppendLine();
            sbResult.Append("}");

            return sbResult.ToString();

            //=== Help methods

            void writeUnsortedLines()
            {
                if (unsortedDataDict[unsortedOrder].Count > 0)
                {
                    foreach (string line in unsortedDataDict[unsortedOrder])
                        sbResult.AppendLine(line);
                }
                unsortedOrder++;
            }
        }

        public void WriteToStream(StreamWriter _streamWriter)
        {
            _streamWriter.Write(PrintOut());
        }

    }
}
