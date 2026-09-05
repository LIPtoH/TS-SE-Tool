/*
   ETS2 / ATS 1.61 compatibility additions
   Copyright 2026 rusplane <https://rusplane.dev>

   Based on TS SE Tool by LIPtoH.
*/

using System;
using System.Text;
using TS_SE_Tool.Save.DataFormat;

namespace TS_SE_Tool.Save.Items
{
    class Player_Vehicles : SiiNBlockCore
    {
        internal string vehicle { get; set; } = "null";
        internal Vector_3f_4f stored_vehicle_placement { get; set; } = new Vector_3f_4f();
        internal string stored_vehicle_city { get; set; } = "null";
        internal string stored_vehicle_country { get; set; } = "null";
        internal string trailer { get; set; } = "null";
        internal int stored_trailer_placements { get; set; } = 0;
        internal bool stored_trailer_attached { get; set; } = false;

        internal Player_Vehicles() { }

        internal Player_Vehicles(string[] input)
        {
            foreach (string currentLine in input)
            {
                string tagLine;
                string dataLine;

                if (currentLine.Contains(":"))
                {
                    string[] split = currentLine.Split(new[] { ':' }, 2);
                    tagLine = split[0].Trim();
                    dataLine = split[1].Trim();
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
                        case "vehicle":
                            vehicle = dataLine;
                            break;

                        case "stored_vehicle_placement":
                            stored_vehicle_placement = new Vector_3f_4f(dataLine);
                            break;

                        case "stored_vehicle_city":
                            stored_vehicle_city = dataLine;
                            break;

                        case "stored_vehicle_country":
                            stored_vehicle_country = dataLine;
                            break;

                        case "trailer":
                            trailer = dataLine;
                            break;

                        case "stored_trailer_placements":
                            stored_trailer_placements = int.Parse(dataLine);
                            break;

                        case "stored_trailer_attached":
                            stored_trailer_attached = bool.Parse(dataLine);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Utilities.IO_Utilities.ErrorLogWriter(
                        ex.Message + Environment.NewLine +
                        "player_vehicles | " + tagLine + " = " + dataLine);
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

            returnSB.AppendLine("player_vehicles : " + _nameless + " {");

            returnSB.AppendLine(" vehicle: " + vehicle);
            returnSB.AppendLine(" stored_vehicle_placement: " + stored_vehicle_placement.ToString());
            returnSB.AppendLine(" stored_vehicle_city: " + stored_vehicle_city);
            returnSB.AppendLine(" stored_vehicle_country: " + stored_vehicle_country);
            returnSB.AppendLine(" trailer: " + trailer);
            returnSB.AppendLine(" stored_trailer_placements: " + stored_trailer_placements.ToString());
            returnSB.AppendLine(" stored_trailer_attached: " + stored_trailer_attached.ToString().ToLower());

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            this.removeWritenBlock(_nameless);

            return returnString;
        }
    }
}