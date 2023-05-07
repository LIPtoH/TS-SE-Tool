using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS_SE_Tool.Save.Items
{
    class Police_offence_log_Entry : SiiNBlockCore
    {

        internal int game_time { get; set; } = 0;

        internal int type { get; set; } = 0;

        internal int fine { get; set; } = 0;

        internal Police_offence_log_Entry()
        { }

        internal Police_offence_log_Entry(string[] _input)
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

                        case "game_time":
                            {
                                game_time = int.Parse(dataLine);
                                break;
                            }

                        case "type":
                            {
                                type = int.Parse(dataLine);
                                break;
                            }

                        case "fine":
                            {
                                fine = int.Parse(dataLine);
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

            returnSB.AppendLine("police_offence_log_entry : " + _nameless + " {");

            returnSB.AppendLine(" game_time: " + game_time);
            returnSB.AppendLine(" type: " + type);
            returnSB.AppendLine(" fine: " + fine);

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            this.removeWritenBlock(_nameless);

            return returnString;
        }
    }
}