using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TS_SE_Tool.Save.DataFormat;
using TS_SE_Tool.Utilities;

namespace TS_SE_Tool.Save.Items
{
    class Police_offence_Log : SiiNBlockCore
    {
        internal List<string> detailed_history_entries { get; set; } = new List<string>();

        internal List<uint> offence_total_counts { get; set; } = new List<uint>();

        internal List<uint> offence_total_fines { get; set; } = new List<uint>();
        
        internal Police_offence_Log()
        { }

        internal Police_offence_Log(string[] _input)
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
                        case "police_offence_log":
                        case "}":
                            {
                                break;
                            }

                        case "detailed_history_entries":
                            {
                                detailed_history_entries.Capacity = int.Parse(dataLine);
                                break;
                            }

                        case var s when s.StartsWith("detailed_history_entries["):
                            {
                                detailed_history_entries.Add(dataLine);
                                break;
                            }

                        case "offence_total_counts":
                            {
                                offence_total_counts.Capacity = int.Parse(dataLine);
                                break;
                            }

                        case var s when s.StartsWith("offence_total_counts["):
                            {
                                offence_total_counts.Add(uint.Parse(dataLine));
                                break;
                            }

                        case "offence_total_fines":
                            {
                                offence_total_fines.Capacity = int.Parse(dataLine);
                                break;
                            }

                        case var s when s.StartsWith("offence_total_fines["):
                            {
                                offence_total_fines.Add(uint.Parse(dataLine));
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

        internal string PrintOut(uint _version)
        {
            return PrintOut(_version, null);
        }
        internal string PrintOut(uint _version, string _nameless)
        {
            string returnString = "";

            StringBuilder returnSB = new StringBuilder();

            returnSB.AppendLine("police_offence_log : " + _nameless + " {");

            returnSB.AppendLine(" detailed_history_entries: " + detailed_history_entries.Count);
            for (int i = 0; i < detailed_history_entries.Count; i++)
                returnSB.AppendLine(" detailed_history_entries[" + i + "]: " + detailed_history_entries[i]);

            returnSB.AppendLine(" offence_total_counts: " + offence_total_counts.Count);
            for (int i = 0; i < offence_total_counts.Count; i++)
                returnSB.AppendLine(" offence_total_counts[" + i + "]: " + offence_total_counts[i].ToString());

            returnSB.AppendLine(" offence_total_fines: " + offence_total_fines.Count);
            for (int i = 0; i < offence_total_fines.Count; i++)
                returnSB.AppendLine(" offence_total_fines[" + i + "]: " + offence_total_fines[i].ToString().ToLower());

            WriteUnidentifiedLines();

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            this.removeWritenBlock(_nameless);

            return returnString;
        }
    }
}