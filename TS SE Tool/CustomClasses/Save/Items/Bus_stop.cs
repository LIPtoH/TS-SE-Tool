using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS_SE_Tool.Save.Items
{
    class Bus_stop
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
                    }
                }
                catch (Exception ex)
                {
                    Utilities.IO_Utilities.ErrorLogWriter(ex.Message + Environment.NewLine + this.GetType().Name.ToLower() + " | " + tagLine + " = " + dataLine);
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

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            return returnString;
        }
    }
}