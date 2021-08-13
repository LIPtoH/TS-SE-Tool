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

namespace TS_SE_Tool
{
    public class SaveFileInfoData
    {
        private string SaveContainerNameless { get; set; } = "";
        public string Name { get; set; } = "\"\"";  //Save name
        public uint Time { get; set; } = 0;         //IngameTime
        public uint FileTime { get; set; } = 0;
        public ushort Version { get; set; } = 0;

        private byte InfoVersion { get; set; } = 0;
        private uint InfoPlayersExperience { get; set; } = 0;
        private ushort InfoUnlockedRecruitments { get; set; } = 0;
        private ushort InfoUnlockedDealers { get; set; } = 0;
        private ushort InfoVisitedCities { get; set; } = 0;
        private long InfoMoneyAccount { get; set; } = 0;
        private decimal InfoExploredRatio { get; set; } = 0.0M;

        internal List<Dependency> Dependencies { get; set; }

        public string GetDataText()
        {
            string OutputText = "SiiNunit\r\n{\r\n";

            OutputText += "save_container : " + SaveContainerNameless + " {\r\n";
            OutputText += " name: " + Name + "\r\n";
            OutputText += " time: " + Time.ToString() + "\r\n";
            OutputText += " file_time: " + FileTime.ToString() + "\r\n";
            OutputText += " version: " + Version.ToString() + "\r\n";

            if (InfoVersion > 0)
            {
                OutputText += " info_version: " + InfoVersion.ToString() + "\r\n";
            }                

            OutputText += " dependencies: " + Dependencies.Count.ToString() + "\r\n";

            byte DepCount = 0;
            foreach(Dependency tDep in Dependencies)
            {
                OutputText += " dependencies[" + DepCount + "]: \"" + tDep.Raw + "\"\r\n";
                DepCount++;
            }

            if (InfoVersion > 0)
            {
                OutputText += " info_players_experience: " + InfoPlayersExperience.ToString() + "\r\n";
                OutputText += " info_unlocked_recruitments: " + InfoUnlockedRecruitments.ToString() + "\r\n";
                OutputText += " info_unlocked_dealers: " + InfoUnlockedDealers.ToString() + "\r\n";
                OutputText += " info_visited_cities: " + InfoVisitedCities.ToString() + "\r\n";
                OutputText += " info_money_account: " + InfoMoneyAccount.ToString() + "\r\n";
                OutputText += " info_explored_ratio: " + Utilities.NumericUtilities.DecimalFloatToHexFloat(InfoExploredRatio) + "\r\n";
            }

            OutputText += "}\r\n\r\n}";

            return OutputText;
        }

        public void WriteToStream (StreamWriter _streamWriter)
        {
            _streamWriter.Write(GetDataText());
        }

        public void Prepare(string[] _FileLines)
        {
            string[] chunkOfline;

            for (int line = 0; line < _FileLines.Length; line++)
            {
                if (_FileLines[line].StartsWith("save_container :"))
                {
                    chunkOfline = _FileLines[line].Split(new char[] { ' ' });
                    SaveContainerNameless = chunkOfline[2];
                    continue;
                }

                if (_FileLines[line].StartsWith(" name:"))
                {
                    chunkOfline = _FileLines[line].Split(new char[] { ' ' });
                    Name = chunkOfline[2];
                    continue;
                }

                if (_FileLines[line].StartsWith(" time:"))
                {
                    chunkOfline = _FileLines[line].Split(new char[] { ' ' });
                    Time = uint.Parse(chunkOfline[2]);
                    continue;
                }

                if (_FileLines[line].StartsWith(" file_time:"))
                {
                    chunkOfline = _FileLines[line].Split(new char[] { ' ' });
                    FileTime = uint.Parse(chunkOfline[2]);
                    continue;
                }

                if (_FileLines[line].StartsWith(" version:"))
                {
                    chunkOfline = _FileLines[line].Split(new char[] { ' ' });
                    Version = ushort.Parse(chunkOfline[2]);
                    continue;
                }

                if (_FileLines[line].StartsWith(" dependencies:"))
                {
                    chunkOfline = _FileLines[line].Split(new char[] { ' ' });

                    Dependencies = new List<Dependency>();
                    Dependencies.Capacity = short.Parse(chunkOfline[2]);
                    continue;
                }

                if (_FileLines[line].StartsWith(" dependencies["))
                {
                    chunkOfline = _FileLines[line].Split(new char[] { ':' }, 2);

                    string tmpDep = chunkOfline[1].Trim(new char[] { ' ' });
                    tmpDep = tmpDep.Substring(1, tmpDep.Length - 2);

                    Dependencies.Add(new Dependency(tmpDep));
                    continue;
                }

                //Info data
                if (_FileLines[line].StartsWith(" info_version:"))
                {
                    chunkOfline = _FileLines[line].Split(new char[] { ' ' });
                    InfoVersion = byte.Parse(chunkOfline[2]);
                    continue;
                }

                if (_FileLines[line].StartsWith(" info_players_experience:"))
                {
                    chunkOfline = _FileLines[line].Split(new char[] { ' ' });
                    InfoPlayersExperience = uint.Parse(chunkOfline[2]);
                    continue;
                }

                if (_FileLines[line].StartsWith(" info_unlocked_recruitments:"))
                {
                    chunkOfline = _FileLines[line].Split(new char[] { ' ' });
                    InfoUnlockedRecruitments = ushort.Parse(chunkOfline[2]);
                    continue;
                }

                if (_FileLines[line].StartsWith(" info_unlocked_dealers:"))
                {
                    chunkOfline = _FileLines[line].Split(new char[] { ' ' });
                    InfoUnlockedDealers = ushort.Parse(chunkOfline[2]);
                    continue;
                }

                if (_FileLines[line].StartsWith(" info_visited_cities:"))
                {
                    chunkOfline = _FileLines[line].Split(new char[] { ' ' });
                    InfoVisitedCities = ushort.Parse(chunkOfline[2]);
                    continue;
                }

                if (_FileLines[line].StartsWith(" info_money_account:"))
                {
                    chunkOfline = _FileLines[line].Split(new char[] { ' ' });
                    InfoMoneyAccount = long.Parse(chunkOfline[2]);
                    continue;
                }

                if (_FileLines[line].StartsWith(" info_explored_ratio:"))
                {
                    chunkOfline = _FileLines[line].Split(new char[] { ' ' });
                    InfoExploredRatio = Utilities.NumericUtilities.HexFloatToDecimalFloat(chunkOfline[2]);
                    continue;
                }
                //

                if (_FileLines[line].StartsWith("}"))
                {
                    break;
                }
            }
        }
    }
}
