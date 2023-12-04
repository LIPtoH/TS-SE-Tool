using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS_SE_Tool.Utilities;

namespace TS_SE_Tool.Save.Items
{
    class Oversize_Route_offers : SiiNBlockCore
    {
        internal List<string> offers { get; set; } = new List<string>();

        internal string route { get; set; } = "";

        internal Oversize_Route_offers()
        { }

        internal Oversize_Route_offers(string[] _input)
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
                        case "oversize_route_offers":
                        case "}":
                            {
                                break;
                            }

                        case "offers":
                            {
                                offers.Capacity = int.Parse(dataLine);
                                break;
                            }

                        case var s when s.StartsWith("offers["):
                            {
                                offers.Add(dataLine);
                                break;
                            }

                        case "route":
                            {
                                route = dataLine;
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

            returnSB.AppendLine("oversize_route_offers : " + _nameless + " {");

            returnSB.AppendLine(" offers: " + offers.Count);
            for (int i = 0; i < offers.Count; i++)
                returnSB.AppendLine(" offers[" + i + "]: " + offers[i]);

            returnSB.AppendLine(" route: " + route);

            WriteUnidentifiedLines();

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            this.removeWritenBlock(_nameless);

            return returnString;
        }
    }
}