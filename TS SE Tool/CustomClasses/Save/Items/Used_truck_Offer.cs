using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TS_SE_Tool.Utilities;
using TS_SE_Tool.Save.DataFormat;

namespace TS_SE_Tool.Save.Items
{
    internal class Used_truck_Offer : SiiNBlockCore
    {
        //v1.49

        #region variables

        internal bool lefthand_traffic { get; set; } = false;

        internal string truck { get; set; } = "";

        internal uint price { get; set; } = 0;

        internal uint expiration_game_time { get; set; } = 0;

        #endregion

        internal Used_truck_Offer()
        {

        }

        internal Used_truck_Offer(string[] _input)
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
                        case "used_truck_offer":
                        case "}":
                            {
                                break;
                            }

                        case "lefthand_traffic":
                            {
                                lefthand_traffic = bool.Parse(dataLine);
                                break;
                            }

                        case "truck":
                            {
                                truck = dataLine;
                                break;
                            }

                        case "price":
                            {
                                price = uint.Parse(dataLine);
                                break;
                            }

                        case "expiration_game_time":
                            {
                                expiration_game_time = uint.Parse(dataLine);
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

            returnSB.AppendLine("used_truck_offer : " + _nameless + " {");

            returnSB.AppendLine(" lefthand_traffic: " + lefthand_traffic.ToString().ToLower());
            returnSB.AppendLine(" truck: " + truck);
            returnSB.AppendLine(" price: " + price.ToString());
            returnSB.AppendLine(" expiration_game_time: " + expiration_game_time.ToString());

            WriteUnidentifiedLines();

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            this.removeWritenBlock(_nameless);

            return returnString;
        }
    }
}
