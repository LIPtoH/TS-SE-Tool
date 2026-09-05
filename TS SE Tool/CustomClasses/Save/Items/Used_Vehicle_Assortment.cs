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
    class Used_Vehicle_Assortment : SiiNBlockCore
    {
        internal int next_generation_game_time { get; set; } = 0;
        internal List<string> trucks { get; set; } = new List<string>();

        internal Used_Vehicle_Assortment() { }

        internal Used_Vehicle_Assortment(string[] input)
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
                        case "next_generation_game_time":
                            next_generation_game_time = int.Parse(dataLine);
                            break;

                        case "trucks":
                            trucks.Capacity = int.Parse(dataLine);
                            break;

                        case var s when s.StartsWith("trucks["):
                            trucks.Add(dataLine);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Utilities.IO_Utilities.ErrorLogWriter(
                        ex.Message + Environment.NewLine +
                        "used_vehicle_assortment | " +
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

            returnSB.AppendLine("used_vehicle_assortment : " + _nameless + " {");

            returnSB.AppendLine(" next_generation_game_time: " + next_generation_game_time.ToString());

            returnSB.AppendLine(" trucks: " + trucks.Count);

            for (int i = 0; i < trucks.Count; i++)
                returnSB.AppendLine(" trucks[" + i + "]: " + trucks[i]);

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            this.removeWritenBlock(_nameless);

            return returnString;
        }
    }
}