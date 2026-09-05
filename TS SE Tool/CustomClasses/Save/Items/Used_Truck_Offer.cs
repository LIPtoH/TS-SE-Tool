/*
   ETS2 / ATS 1.61 compatibility additions
   Copyright 2026 rusplane <https://rusplane.dev>

   Based on TS SE Tool by LIPtoH.
*/

using System;
using System.Text;

namespace TS_SE_Tool.Save.Items
{
    class Used_Truck_Offer : SiiNBlockCore
    {
        internal bool lefthand_traffic { get; set; } = false;
        internal string truck { get; set; } = "null";
        internal int price { get; set; } = 0;
        internal int expiration_game_time { get; set; } = 0;

        internal Used_Truck_Offer() { }

        internal Used_Truck_Offer(string[] input)
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
                        case "lefthand_traffic":
                            lefthand_traffic = bool.Parse(dataLine);
                            break;

                        case "truck":
                            truck = dataLine;
                            break;

                        case "price":
                            price = int.Parse(dataLine);
                            break;

                        case "expiration_game_time":
                            expiration_game_time = int.Parse(dataLine);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Utilities.IO_Utilities.ErrorLogWriter(
                        ex.Message + Environment.NewLine +
                        "used_truck_offer | " +
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

            returnSB.AppendLine("used_truck_offer : " + _nameless + " {");

            returnSB.AppendLine(" lefthand_traffic: " + lefthand_traffic.ToString().ToLower());
            returnSB.AppendLine(" truck: " + truck);
            returnSB.AppendLine(" price: " + price.ToString());
            returnSB.AppendLine(" expiration_game_time: " + expiration_game_time.ToString());

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            this.removeWritenBlock(_nameless);

            return returnString;
        }
    }
}