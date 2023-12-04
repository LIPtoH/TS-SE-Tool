using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS_SE_Tool.Utilities;

namespace TS_SE_Tool.Save.Items
{
    class Economy_event : SiiNBlockCore
    {
        internal uint time { get; set; } = 0;

        internal string unit_link { get; set; } = "";

        internal int param { get; set; } = 0;

        internal Economy_event()
        { }

        internal Economy_event(uint _time, string _unit_link, int _param)
        {
            time = _time;
            unit_link = _unit_link;
            param = _param;
        }

        internal Economy_event(string[] _input)
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
                        case "economy_event":
                        case "}":
                            {
                                break;
                            }

                        case "time":
                            {
                                time = uint.Parse(dataLine);
                                break;
                            }

                        case "unit_link":
                            {
                                unit_link = dataLine;
                                break;
                            }

                        case "param":
                            {
                                param = int.Parse(dataLine);
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

            returnSB.AppendLine("economy_event : " + _nameless + " {");

            returnSB.AppendLine(" time: " + time.ToString());

            returnSB.AppendLine(" unit_link: " + unit_link);

            returnSB.AppendLine(" param: " + param.ToString());

            WriteUnidentifiedLines();

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            this.removeWritenBlock(_nameless);

            return returnString;
        }
    }
}