using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS_SE_Tool.Utilities;

namespace TS_SE_Tool.Save.Items
{
    class Bus_stop : SiiNBlockCore
    {
        internal bool discovered { get; set; } = false;

        internal List<string> lines_offer { get; set; } = new List<string>();

        internal Bus_stop()
        { }

        internal Bus_stop(string[] _input)
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
                        case "bus_stop":
                        case "}":
                            {
                                break;
                            }

                        case "discovered":
                            {
                                discovered = bool.Parse(dataLine);
                                break;
                            }

                        case "lines_offer":
                            {
                                lines_offer.Capacity = int.Parse(dataLine);
                                break;
                            }

                        case var s when s.StartsWith("lines_offer["):
                            {
                                lines_offer.Add(dataLine);
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

            returnSB.AppendLine("bus_stop : " + _nameless + " {");
            
            returnSB.AppendLine(" discovered: " + discovered.ToString().ToLower());

            returnSB.AppendLine(" lines_offer: " + lines_offer.Count);
            for (int i = 0; i < lines_offer.Count; i++)
                returnSB.AppendLine(" lines_offer[" + i + "]: " + lines_offer[i]);

            WriteUnidentifiedLines();

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            this.removeWritenBlock(_nameless);

            return returnString;
        }
    }
}