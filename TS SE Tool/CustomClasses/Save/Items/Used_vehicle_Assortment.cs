using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TS_SE_Tool.Utilities;
using TS_SE_Tool.Save.DataFormat;

namespace TS_SE_Tool.Save.Items
{
    internal class Used_vehicle_Assortment : SiiNBlockCore
    {
        //v1.49

        #region variables

        internal uint next_generation_game_time { get; set; } = 0;

        internal List<string> trucks { get; set; } = new List<string>();

        #endregion

        internal Used_vehicle_Assortment()
        {

        }

        internal Used_vehicle_Assortment(string[] _input)
        {
            string tagLine = "", dataLine = "";

            foreach (string currentLine in _input)
            {
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
                        case "":
                        case "used_vehicle_assortment":
                        case "}":
                            {
                                break;
                            }

                        case "next_generation_game_time":
                            {
                                next_generation_game_time = uint.Parse(dataLine);
                                break;
                            }

                        case "trucks":
                            {
                                trucks.Capacity = int.Parse(dataLine);
                                break;
                            }

                        case var s when s.StartsWith("trucks["):
                            {
                                trucks.Add(dataLine);
                                break;
                            }

                        default:
                            {
                                UnidentifiedLines.Add(dataLine);
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

        internal string PrintOut(uint _version, string _nameless)
        {
            string returnString = "";

            StringBuilder returnSB = new StringBuilder();

            returnSB.AppendLine("used_vehicle_assortment : " + _nameless + " {");

            returnSB.AppendLine(" next_generation_game_time: " + next_generation_game_time.ToString());

            returnSB.AppendLine(" trucks: " + trucks.Count);
            for (int i = 0; i < trucks.Count; i++)
                returnSB.AppendLine(" trucks[" + i + "]: " + trucks[i]);

            WriteUnidentifiedLines();

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            this.removeWritenBlock(_nameless);

            return returnString;
        }
    }
}
