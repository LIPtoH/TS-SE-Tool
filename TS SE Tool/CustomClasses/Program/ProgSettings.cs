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
using System.IO;
using System.Globalization;
using System.Threading;
using System.Reflection;
using System.Windows.Forms;
using System.Text;
using System.Threading.Tasks;
using TS_SE_Tool.Utilities;

namespace TS_SE_Tool
{
    class ProgSettings
    {
        public ProgSettings()
        { }

        public string   ProgramVersion  { get; set; } = "0.1.0.0";

        public string   ProgPrevVersion { get; set; } = "";

        public string   Language        { get; set; } = "Default";

        public bool     ProposeRandom   { get; set; } = false;

        public Int16    JobPickupTime   { get; set; } = 72;

        public byte     LoopEvery       { get; set; } = 0;

        public double   TimeMultiplier  { get; set; } = 1.0;

        public string   DistanceMes     { get; set; } = "km";

        public string   WeightMes       { get; set; } = "kg";

        public string   CurrencyMesETS2 { get; set; } = "EUR";

        public string   CurrencyMesATS  { get; set; } = "USD";

        public DateTime LastUpdateCheck { get; set; } = DateTime.Now;
        public Dictionary<string, List<string>> CustomPaths { get; set; } = new Dictionary<string, List<string>>();

        public void LoadConfigFromFile()
        {
            try
            {
                string GameType = "";

                foreach (string line in File.ReadAllLines(Directory.GetCurrentDirectory() + @"\config.cfg"))
                {
                    string tag = line.Split(new char[] { '=' })[0], 
                        data = line.Split(new char[] { '=' })[1];

                    switch (tag)
                    {
                        case "ProgramVersion":
                            {
                                ProgPrevVersion = data;
                                break;
                            }

                        case "Language":
                            {
                                Language = data;

                                if (Language == "Default")
                                    Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");

                                break;
                            }

                        case "JobPickupTime":
                            {
                                JobPickupTime = short.Parse(data);
                                break;
                            }

                        case "LoopEvery":
                            {
                                LoopEvery = byte.Parse(data);
                                break;
                            }

                        case "ProposeRandom":
                            {
                                ProposeRandom = bool.Parse(data);
                                break;
                            }

                        case "TimeMultiplier":
                            {
                                TimeMultiplier = short.Parse(data);

                                if (TimeMultiplier > 7.0)
                                {
                                    TimeMultiplier = 7.0;
                                }
                                else if (TimeMultiplier < 0.1)
                                {
                                    TimeMultiplier = 0.1;
                                }

                                break;
                            }

                        case "DistanceMes":
                            {
                                DistanceMes = data;
                                break;
                            }

                        case "WeightMes":
                            {
                                WeightMes = data;
                                break;
                            }

                        case "CurrencyMesETS2":
                            {
                                CurrencyMesETS2 = data;
                                break;
                            }

                        case "CurrencyMesATS":
                            {
                                CurrencyMesATS = data;
                                break;
                            }

                        case "CustomPathGame":
                            {
                                GameType = data;
                                break;
                            }

                        case "CustomPath":
                            {
                                if (GameType == "" || GameType == null)
                                    break;

                                if (CustomPaths.ContainsKey(GameType))
                                {
                                    CustomPaths[GameType].Add(data);
                                }
                                else
                                {
                                    List<string> tmp = new List<string>();
                                    tmp.Add(data);

                                    CustomPaths.Add(GameType, tmp);
                                }

                                break;
                            }

                        case "LastUpdateCheck":
                            {
                                LastUpdateCheck = DateTime.FromFileTimeUtc(long.Parse(data)).ToLocalTime();
                                break;
                            }


                        default:
                            {
                                break;
                            }
                    }
                }

                CustomPaths = CustomPaths.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            }
            catch
            {
                IO_Utilities.LogWriter("Config.cfg file not found or have wrong format. Restoring default");
                WriteConfigToFile();
            }
        }

        public void WriteConfigToFile()
        {
            string[] ExcludeList = new string[] { "CustomPaths", "ProgPrevVersion", "LastUpdateCheck" };

            try
            {
                using (StreamWriter writer = new StreamWriter(Directory.GetCurrentDirectory() + @"\config.cfg", false))
                {
                    PropertyInfo[] properties = this.GetType().GetProperties();

                    foreach (PropertyInfo property in properties)
                    {
                        if(!ExcludeList.Contains(property.Name))                        
                            writer.WriteLine(property.Name + "=" + property.GetValue(this).ToString());
                    }

                    //LastUpdateCheck
                    writer.WriteLine("LastUpdateCheck=" + LastUpdateCheck.ToFileTimeUtc().ToString());

                    //Write Custom paths
                    string GameType = "";

                    foreach (KeyValuePair<string, List<string>> gameCustomPath in CustomPaths)
                    {
                        if (GameType != gameCustomPath.Key)
                        {
                            GameType = gameCustomPath.Key;
                            writer.WriteLine("CustomPathGame=" + gameCustomPath.Key);
                        }
                        foreach (string customPath in gameCustomPath.Value)
                        {
                            writer.WriteLine("CustomPath=" + customPath);
                        }
                    }
                }
            }
            catch
            {
                IO_Utilities.LogWriter("Could not write to " + Directory.GetCurrentDirectory());
                UpdateStatusBarMessage.ShowStatusMessage(SMStatus.Error, "error_could_not_write_to_file", Directory.GetCurrentDirectory() + @"\config.cfg");
            }
        }

    }
}
