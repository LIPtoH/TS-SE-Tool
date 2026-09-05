/*
   ETS2 / ATS 1.61 compatibility additions
   Copyright 2026 rusplane <https://rusplane.dev>

   Based on TS SE Tool by LIPtoH.
*/

using System;
using System.Text;

namespace TS_SE_Tool.Save.Items
{
    class Police_Offence_Log_Entry : SiiNBlockCore
    {
        internal int game_time { get; set; } = 0;
        internal int type { get; set; } = 0;
        internal int fine { get; set; } = 0;

        internal Police_Offence_Log_Entry() { }

        internal Police_Offence_Log_Entry(string[] input)
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
                        case "game_time":
                            game_time = int.Parse(dataLine);
                            break;

                        case "type":
                            type = int.Parse(dataLine);
                            break;

                        case "fine":
                            fine = int.Parse(dataLine);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Utilities.IO_Utilities.ErrorLogWriter(
                        ex.Message + Environment.NewLine +
                        "police_offence_log_entry | " +
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

            returnSB.AppendLine("police_offence_log_entry : " + _nameless + " {");

            returnSB.AppendLine(" game_time: " + game_time.ToString());
            returnSB.AppendLine(" type: " + type.ToString());
            returnSB.AppendLine(" fine: " + fine.ToString());

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            this.removeWritenBlock(_nameless);

            return returnString;
        }
    }
}