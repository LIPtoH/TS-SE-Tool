using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS_SE_Tool.Save.Items
{
    class Trailer_Utilization_log_Entry : SiiNBlockCore
    {
        internal int economy_day { get; set; } = 0;
        internal int use_time { get; set; } = 0;

        internal Trailer_Utilization_log_Entry()
        { }

        internal Trailer_Utilization_log_Entry(string[] _input)
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

                        case "economy_day":
                            {
                                economy_day = int.Parse(dataLine);
                                break;
                            }

                        case "use_time":
                            {
                                use_time = int.Parse(dataLine);
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

            returnSB.AppendLine("trailer_utilization_log_entry : " + _nameless + " {");

            returnSB.AppendLine(" economy_day: " + economy_day.ToString());
            returnSB.AppendLine(" use_time: " + use_time.ToString());

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            this.removeWritenBlock(_nameless);

            return returnString;
        }
    }
}
