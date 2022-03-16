using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TS_SE_Tool.Save.DataFormat;

namespace TS_SE_Tool.Save.Items
{
    class Game_Progress : SiiNBlockCore
    {
        internal string generic_transports { get; set; } = "";
        internal string undamaged_transports { get; set; } = "";
        internal string clean_transports { get; set; } = "";

        internal List<SCS_String> owned_trucks { get; set; } = new List<SCS_String>();

        internal Game_Progress()
        { }

        internal Game_Progress(string[] _input)
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

                    case "generic_transports":
                        {
                            generic_transports = dataLine;
                            break;
                        }

                    case "undamaged_transports":
                        {
                            undamaged_transports = dataLine;
                            break;
                        }

                    case "clean_transports":
                        {
                            clean_transports = dataLine;
                            break;
                        }

                    case "owned_trucks":
                        {
                            owned_trucks.Capacity = int.Parse(dataLine);
                            break;
                        }

                    case var s when s.StartsWith("owned_trucks["):
                        {
                            owned_trucks.Add(dataLine);
                            break;
                        }

                }
            }
        }

        internal string PrintOut(uint _version)
        {
            return PrintOut(_version, null);
        }

        internal string PrintOut(uint _version, string _nameless)
        {
            string returnString = "";

            StringBuilder returnSB = new StringBuilder();

            returnSB.AppendLine("game_progress : " + _nameless + " {");

            returnSB.AppendLine(" generic_transports: " + generic_transports);
            returnSB.AppendLine(" undamaged_transports: " + undamaged_transports);
            returnSB.AppendLine(" clean_transports: " + clean_transports);

            returnSB.AppendLine(" owned_trucks: " + owned_trucks.Count);
            for (int i = 0; i < owned_trucks.Count; i++)
                returnSB.AppendLine(" owned_trucks[" + i + "]: " + owned_trucks[i]);

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            this.removeWritenBlock(_nameless);

            return returnString;
        }
    }
}