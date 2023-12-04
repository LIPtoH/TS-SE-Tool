using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS_SE_Tool.Utilities;

namespace TS_SE_Tool.Save.Items
{
    class Ferry_log_Entry : SiiNBlockCore
    {
        internal string ferry { get; set; } = "";

        internal string connection { get; set; } = "";

        internal uint last_visit { get; set; } = 0;

        internal uint use_count { get; set; } = 0;

        internal Ferry_log_Entry()
        { }

        internal Ferry_log_Entry(string[] _input)
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
                        case "ferry_log_entry":
                        case "}":
                            {
                                break;
                            }

                        case "ferry":
                            {
                                ferry = dataLine;
                                break;
                            }

                        case "connection":
                            {
                                connection = dataLine;
                                break;
                            }

                        case "last_visit":
                            {
                                last_visit = uint.Parse(dataLine);
                                break;
                            }

                        case "use_count":
                            {
                                use_count = uint.Parse(dataLine);
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

            returnSB.AppendLine("ferry_log_entry : " + _nameless + " {");

            returnSB.AppendLine(" ferry: " + ferry);
            returnSB.AppendLine(" connection: " + connection);

            returnSB.AppendLine(" last_visit: " + last_visit.ToString());
            returnSB.AppendLine(" use_count: " + use_count.ToString());

            WriteUnidentifiedLines();

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            this.removeWritenBlock(_nameless);

            return returnString;
        }
    }
}