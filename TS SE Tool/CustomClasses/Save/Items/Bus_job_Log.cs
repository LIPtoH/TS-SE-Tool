using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS_SE_Tool.Save.Items
{
    class Bus_job_Log : SiiNBlockCore
    {
        internal int version { get; set; } = 0;

        internal List<string> entries { get; set; } = new List<string>();

        internal Bus_job_Log()
        { }

        internal Bus_job_Log(string[] _input)
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
                    }
                }
                catch (Exception ex)
                {
                    Utilities.IO_Utilities.ErrorLogWriter(ex.Message + Environment.NewLine + this.GetType().Name.ToLower() + " | " + tagLine + " = " + dataLine);
                    break;
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

            returnSB.AppendLine("bus_job_log : " + _nameless + " {");

            returnSB.AppendLine(" version: " + version.ToString());

            returnSB.AppendLine(" entries: " + entries.Count);
            for (int i = 0; i < entries.Count; i++)
                returnSB.AppendLine(" entries[" + i + "]: " + entries[i]);

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            this.removeWritenBlock(_nameless);

            return returnString;
        }
    }
}