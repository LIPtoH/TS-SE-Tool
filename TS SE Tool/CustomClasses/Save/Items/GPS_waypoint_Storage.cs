using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TS_SE_Tool.Save.DataFormat;
using TS_SE_Tool.Utilities;

namespace TS_SE_Tool.Save.Items
{
    class GPS_waypoint_Storage : SiiNBlockCore
    {
        Vector_3i nav_node_position { get; set; } = new Vector_3i();

        internal string direction { get; set; } = "";

        internal GPS_waypoint_Storage()
        { }

        internal GPS_waypoint_Storage(string[] _input)
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
                        case "gps_waypoint_storage":
                        case "}":
                            {
                                break;
                            }

                        case "nav_node_position":
                            {
                                nav_node_position = new Vector_3i(dataLine);
                                break;
                            }

                        case "direction":
                            {
                                direction = dataLine;
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

            returnSB.AppendLine("gps_waypoint_storage : " + _nameless + " {");

            returnSB.AppendLine(" nav_node_position: " + nav_node_position.ToString());

            returnSB.AppendLine(" direction: " + direction);

            WriteUnidentifiedLines();

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            this.removeWritenBlock(_nameless);

            return returnString;
        }
    }
}