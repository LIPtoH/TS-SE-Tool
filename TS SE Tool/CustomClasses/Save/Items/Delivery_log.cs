using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS_SE_Tool.Utilities;

namespace TS_SE_Tool.Save.Items
{
    class Delivery_log : SiiNBlockCore
    {
        internal int version { get; set; } = 0;

        internal List<string> entries { get; set; } = new List<string>();

        internal int cached_jobs_count { get; set; } = 0;

        internal Delivery_log()
        { }

        internal Delivery_log(string[] _input)
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
                        case "delivery_log":
                        case "}":
                            {
                                break;
                            }

                        case "version":
                            {
                                version = int.Parse(dataLine);
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

                        case "cached_jobs_count":
                            {
                                cached_jobs_count = int.Parse(dataLine);
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

            returnSB.AppendLine("delivery_log : " + _nameless + " {");

            returnSB.AppendLine(" version: " + version.ToString());

            returnSB.AppendLine(" entries: " + entries.Count);
            for (int i = 0; i < entries.Count; i++)
                returnSB.AppendLine(" entries[" + i + "]: " + entries[i]);

            returnSB.AppendLine(" cached_jobs_count: " + cached_jobs_count.ToString());

            WriteUnidentifiedLines();

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            this.removeWritenBlock(_nameless);

            return returnString;
        }
    }
}