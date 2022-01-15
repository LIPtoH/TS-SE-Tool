using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TS_SE_Tool.Utilities;
using TS_SE_Tool.Save.DataFormat;

namespace TS_SE_Tool.Save.Items
{
    class Trailer_Utilization_log
    {
        internal List<string> entries { get; set; } = new List<string>();

        internal int total_driven_distance_km { get; set; } = 0;
        internal int total_transported_cargoes { get; set; } = 0;

        internal SCS_Float total_transported_weight { get; set; } = 0;

        internal Trailer_Utilization_log()
        { }

        internal Trailer_Utilization_log(string[] _input)
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

                    case "entries":
                        {
                            entries.Capacity = int.Parse(dataLine);
                            break;
                        }

                    case var s when s.StartsWith("entries["):
                        {
                            entries.Add(dataLine);
                            break;
                        }

                    case "total_driven_distance_km":
                        {
                            total_driven_distance_km = int.Parse(dataLine);
                            break;
                        }

                    case "total_transported_cargoes":
                        {
                            total_transported_cargoes = int.Parse(dataLine);
                            break;
                        }

                    case "total_transported_weight":
                        {
                            total_transported_weight = dataLine;
                            break;
                        }

                }
            }
        }

        internal string PrintOut(uint _version, string _nameless)
        {
            string returnString = "";

            StringBuilder returnSB = new StringBuilder();

            returnSB.AppendLine("trailer_utilization_log : " + _nameless + " {");

            returnSB.AppendLine(" entries: " + entries.Count);
            for (int i = 0; i < entries.Count; i++)
                returnSB.AppendLine(" entries[" + i + "]: " + entries[i]);

            returnSB.AppendLine(" total_driven_distance_km: " + total_driven_distance_km.ToString());
            returnSB.AppendLine(" total_transported_cargoes: " + total_transported_cargoes.ToString());
            returnSB.AppendLine(" total_transported_weight: " + total_transported_weight.ToString());

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            return returnString;
        }
    }
}
