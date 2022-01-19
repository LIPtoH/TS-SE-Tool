using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TS_SE_Tool.Save.DataFormat;
using TS_SE_Tool.Save.Items;

namespace TS_SE_Tool.Save.Items
{
    class SiiNunit
    {
        internal Economy Economy = new Economy();

        internal Bank Bank = new Bank();
        internal Dictionary<string, Bank_Loan> Bank_Loan = new Dictionary<string, Bank_Loan>();

        internal Player Player = new Player();

        internal Player_Job Player_Job = new Player_Job();

        internal Dictionary<string, Trailer> Trailer = new Dictionary<string, Trailer>();

        internal Dictionary<string, Trailer_Def> Trailer_Def = new Dictionary<string, Trailer_Def>();
        internal Dictionary<string, Trailer_Utilization_log> Trailer_Utilization_log = new Dictionary<string, Trailer_Utilization_log>();
        internal Dictionary<string, Trailer_Utilization_log_Entry> Trailer_Utilization_log_Entry = new Dictionary<string, Trailer_Utilization_log_Entry>();

        internal Dictionary<string, Vehicle> Vehicle = new Dictionary<string, Vehicle>();

        internal Dictionary<string, dynamic> VehicleAccessories = new Dictionary<string, dynamic>();

        internal Dictionary<string, Profit_log> Profit_log = new Dictionary<string, Profit_log>();
        internal Dictionary<string, Profit_log_Entry> Profit_log_Entry = new Dictionary<string, Profit_log_Entry>();

        internal Driver_Player Driver_Player = new Driver_Player();

        internal Dictionary<string, Driver_AI> Driver_AI = new Dictionary<string, Driver_AI>();

        internal Dictionary<string, Job_Info> Job_Info = new Dictionary<string, Job_Info>();

        internal SiiNunit()
        { }

        internal SiiNunit(string[] _input)
        {
            string tagLine = "", dataLine = "", nameless = "";

            List<string> NewDataBlocks = new List<string>();

            //block decoding
            for (int line = 0; line < _input.Length; line++)
            {
                string currentLine = _input[line];

                if (currentLine.Contains(':') && currentLine.Contains('{'))
                {
                    string[] splittedLine = currentLine.Split(new char[] { ':', '{' }, 3);

                    tagLine = splittedLine[0].Trim();
                    dataLine = splittedLine[1].Trim();
                }
                else
                {
                    continue;
                }

                nameless = dataLine;

                switch (tagLine)
                {
                    case "":
                        {
                            break;
                        }

                    case "}":
                        {
                            break;
                        }

                    case "economy":
                        {
                            Economy = new Economy(GetLines().ToArray());

                            break;
                        }

                    case "bank":
                        {
                            Bank = new Bank(GetLines().ToArray());

                            break;
                        }

                    case "bank_loan":
                        {
                            Bank_Loan.Add(nameless, new Bank_Loan(GetLines().ToArray()));

                            break;
                        }

                    case "player":
                        {
                            Player = new Player(GetLines().ToArray());

                            break;
                        }

                    case "player_job":
                        {
                            Player_Job = new Player_Job(GetLines().ToArray());

                            break;
                        }

                    case "trailer":
                        {
                            Trailer.Add(nameless, new Trailer(GetLines().ToArray()));

                            break;
                        }

                    case "trailer_def":
                        {
                            Trailer_Def.Add(nameless, new Trailer_Def(GetLines().ToArray()));

                            break;
                        }

                    case "trailer_utilization_log":
                        {
                            Trailer_Utilization_log.Add(nameless, new Trailer_Utilization_log(GetLines().ToArray()));

                            break;
                        }

                    case "trailer_utilization_log_entry":
                        {
                            Trailer_Utilization_log_Entry.Add(nameless, new Trailer_Utilization_log_Entry(GetLines().ToArray()));

                            break;
                        }

                    case "vehicle":
                        {
                            Vehicle.Add(nameless, new Vehicle(GetLines().ToArray()));

                            break;
                        }

                    case "vehicle_accessory":
                        {
                            VehicleAccessories.Add(nameless, new Vehicle_Accessory(GetLines().ToArray()));

                            break;
                        }

                    case "vehicle_addon_accessory":
                        {
                            VehicleAccessories.Add(nameless, new Vehicle_Addon_Accessory(GetLines().ToArray()));

                            break;
                        }

                    case "vehicle_drv_plate_accessory":
                        {
                            VehicleAccessories.Add(nameless, new Vehicle_Drv_plate_Accessory(GetLines().ToArray()));

                            break;
                        }

                    case "vehicle_wheel_accessory":
                        {
                            VehicleAccessories.Add(nameless, new Vehicle_Wheel_Accessory(GetLines().ToArray()));

                            break;
                        }

                    case "vehicle_paint_job_accessory":
                        {
                            VehicleAccessories.Add(nameless, new Vehicle_Paint_job_Accessory(GetLines().ToArray()));

                            break;
                        }

                    case "profit_log":
                        {
                            Profit_log.Add(nameless, new Profit_log(GetLines().ToArray()));

                            break;
                        }

                    case "profit_log_entry":
                        {
                            Profit_log_Entry.Add(nameless, new Profit_log_Entry(GetLines().ToArray()));

                            break;
                        }

                    case "driver_player":
                        {
                            Driver_Player = new Driver_Player(GetLines().ToArray());

                            break;
                        }

                    case "driver_ai":
                        {
                            Driver_AI.Add(nameless, new Driver_AI(GetLines().ToArray()));

                            break;
                        }

                    case "job_info":
                        {
                            Job_Info.Add(nameless, new Job_Info(GetLines().ToArray()));

                            break;
                        }



                    default:
                        {
                            break;
                        }

                    /*
                    default:
                        {
                            if (!NewDataBlocks.Contains(tagLine))
                            {
                                NewDataBlocks.Add(tagLine);
                                Utilities.IO_Utilities.ErrorLogWriter("Save | New Data block | " + tagLine);
                            }

                            break;
                        }
                    */
                }

                continue;

                List<string> GetLines()
                {
                    string workLine = "";
                    List<string> Data = new List<string>();

                    while (!_input[line].StartsWith("}"))
                    {
                        workLine = _input[line];
                        Data.Add(workLine);

                        line++;
                    }

                    return Data;
                }

            }
        }


        internal string PrintOut(uint _version, string _nameless)
        {
            string returnString = "";

            StringBuilder returnSB = new StringBuilder();

            returnSB.AppendLine("SiiNunit" + Environment.NewLine + "{");

            returnSB.AppendLine(Economy.PrintOut(0, ""));


            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            return returnString;
        }

    }
}