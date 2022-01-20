using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TS_SE_Tool.Save.DataFormat;

namespace TS_SE_Tool.Save.Items
{
    class Garage
    {
        internal List<string> vehicles { get; set; } = new List<string>();

        internal List<string> drivers { get; set; } = new List<string>();

        internal List<string> trailers { get; set; } = new List<string>();

        internal int status { get; set; } = 0;

        internal string profit_log { get; set; } = "";

        internal SCS_Float productivity { get; set; } = 0;

        internal Garage()
        { }

        internal Garage(string[] _input)
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

                switch (tagLine)
                {
                    case "":
                        {
                            break;
                        }


                    case "vehicles":
                        {
                            vehicles.Capacity = int.Parse(dataLine);
                            break;
                        }

                    case var s when s.StartsWith("vehicles["):
                        {
                            vehicles.Add(dataLine);
                            break;
                        }

                    case "drivers":
                        {
                            drivers.Capacity = int.Parse(dataLine);
                            break;
                        }

                    case var s when s.StartsWith("drivers["):
                        {
                            drivers.Add(dataLine);
                            break;
                        }

                    case "trailers":
                        {
                            trailers.Capacity = int.Parse(dataLine);
                            break;
                        }

                    case var s when s.StartsWith("trailers["):
                        {
                            trailers.Add(dataLine);
                            break;
                        }

                    case "status":
                        {
                            status = int.Parse(dataLine);
                            break;
                        }

                    case "profit_log":
                        {
                            profit_log = dataLine;
                            break;
                        }

                    case "productivity":
                        {
                            productivity = dataLine;
                            break;
                        }
                }
            }
        }

        internal string PrintOut(uint _version, string _nameless)
        {
            string returnString = "";

            StringBuilder returnSB = new StringBuilder();

            returnSB.AppendLine("garage : " + _nameless + " {");

            returnSB.AppendLine(" vehicles: " + vehicles.Count);
            for (int i = 0; i < vehicles.Count; i++)
                returnSB.AppendLine(" vehicles[" + i + "]: " + vehicles[i]);

            returnSB.AppendLine(" drivers: " + drivers.Count);
            for (int i = 0; i < drivers.Count; i++)
                returnSB.AppendLine(" drivers[" + i + "]: " + drivers[i]);

            returnSB.AppendLine(" trailers: " + trailers.Count);
            for (int i = 0; i < trailers.Count; i++)
                returnSB.AppendLine(" trailers[" + i + "]: " + trailers[i]);

            returnSB.AppendLine(" status: " + status);
            returnSB.AppendLine(" profit_log: " + profit_log.ToString());
            returnSB.AppendLine(" productivity: " + productivity.ToString());

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            return returnString;
        }
    }
}