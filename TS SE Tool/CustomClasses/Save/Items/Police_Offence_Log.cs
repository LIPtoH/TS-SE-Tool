/*
   ETS2 / ATS 1.61 compatibility additions
   Copyright 2026 rusplane <https://rusplane.dev>

   Based on TS SE Tool by LIPtoH.
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace TS_SE_Tool.Save.Items
{
    class Police_Offence_Log : SiiNBlockCore
    {
        internal List<string> detailed_history_entries { get; set; } = new List<string>();
        internal List<int> offence_total_counts { get; set; } = new List<int>();
        internal List<int> offence_total_fines { get; set; } = new List<int>();

        internal Police_Offence_Log() { }

        internal Police_Offence_Log(string[] input)
        {
            foreach (string currentLine in input)
            {
                if (!currentLine.Contains(":"))
                    continue;

                string[] split = currentLine.Split(new[] { ':' }, 2);
                string tagLine = split[0].Trim();
                string dataLine = split[1].Trim();

                try
                {
                    switch (tagLine)
                    {
                        case "detailed_history_entries":
                            detailed_history_entries.Capacity = int.Parse(dataLine);
                            break;

                        case var s when s.StartsWith("detailed_history_entries["):
                            detailed_history_entries.Add(dataLine);
                            break;

                        case "offence_total_counts":
                            offence_total_counts.Capacity = int.Parse(dataLine);
                            break;

                        case var s when s.StartsWith("offence_total_counts["):
                            offence_total_counts.Add(int.Parse(dataLine));
                            break;

                        case "offence_total_fines":
                            offence_total_fines.Capacity = int.Parse(dataLine);
                            break;

                        case var s when s.StartsWith("offence_total_fines["):
                            offence_total_fines.Add(int.Parse(dataLine));
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Utilities.IO_Utilities.ErrorLogWriter(
                        ex.Message + Environment.NewLine +
                        "police_offence_log | " +
                        tagLine + " = " + dataLine);
                }
            }
        }

        internal string PrintOut(uint version)
        {
            return PrintOut(version, null);
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
                returnSB.AppendLine(" offence_total_fines[" + i + "]: " + offence_total_fines[i].ToString());

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            this.removeWritenBlock(_nameless);

            return returnString;
        }
    }
}