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

        internal Dictionary<string, Company> Company = new Dictionary<string, Company>();
        internal Dictionary<string, Job_offer_Data> Job_offer_Data = new Dictionary<string, Job_offer_Data>();

        internal Dictionary<string, Garage> Garage = new Dictionary<string, Garage>();

        internal Game_Progress Game_Progress = new Game_Progress();

        internal Registry Registry = new Registry();

        internal Dictionary<string, Transport_Data> Transport_Data = new Dictionary<string, Transport_Data>();

        internal Economy_event_Queue Economy_event_Queue = new Economy_event_Queue();
        internal Dictionary<string, Economy_event> Economy_event = new Dictionary<string, Economy_event>();

        internal Mail_Ctrl Mail_Ctrl = new Mail_Ctrl();
        internal Dictionary<string, Mail_Def> Mail_Def = new Dictionary<string, Mail_Def>();

        internal Police_Ctrl Police_Ctrl = new Police_Ctrl();

        internal Oversize_offer_Ctrl Oversize_offer_Ctrl = new Oversize_offer_Ctrl();
        internal Dictionary<string, Oversize_Route_offers> Oversize_Route_offers = new Dictionary<string, Oversize_Route_offers>();
        internal Dictionary<string, Oversize_Offer> Oversize_Offer = new Dictionary<string, Oversize_Offer>();

        internal Delivery_log Delivery_log = new Delivery_log();
        internal Dictionary<string, Delivery_log_Entry> Delivery_log_Entry = new Dictionary<string, Delivery_log_Entry>();

        internal Ferry_log Ferry_log = new Ferry_log();
        internal Dictionary<string, Ferry_log_Entry> Ferry_log_Entry = new Dictionary<string, Ferry_log_Entry>();

        internal Dictionary<string, GPS_waypoint_Storage> GPS_waypoint_Storage = new Dictionary<string, GPS_waypoint_Storage>();

        internal Dictionary<string, Map_action> Map_action = new Dictionary<string, Map_action>();

        internal Dictionary<string, Bus_stop> Bus_stop = new Dictionary<string, Bus_stop>();
        internal Bus_job_Log Bus_job_Log = new Bus_job_Log();

        //===
        private Dictionary<string, List<string>> NewBlocks = new Dictionary<string, List<string>>();

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

                    case "vehicle_sound_accessory":
                        {
                            VehicleAccessories.Add(nameless, new Vehicle_Sound_Accessory(GetLines().ToArray()));

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

                    case "company":
                        {
                            Company.Add(nameless, new Company(GetLines().ToArray()));

                            break;
                        }
                    case "job_offer_data":
                        {
                            Job_offer_Data.Add(nameless, new Job_offer_Data(GetLines().ToArray()));

                            break;
                        }

                    case "garage":
                        {
                            Garage.Add(nameless, new Garage(GetLines().ToArray()));

                            break;
                        }

                    case "game_progress":
                        {
                            Game_Progress = new Game_Progress(GetLines().ToArray());

                            break;
                        }

                    case "registry":
                        {
                            Registry = new Registry(GetLines().ToArray());

                            break;
                        }

                    case "transport_data":
                        {
                            Transport_Data.Add(nameless, new Transport_Data(GetLines().ToArray()));

                            break;
                        }

                    case "economy_event_queue":
                        {
                            Economy_event_Queue = new Economy_event_Queue(GetLines().ToArray());

                            break;
                        }

                    case "economy_event":
                        {
                            Economy_event.Add(nameless, new Economy_event(GetLines().ToArray()));

                            break;
                        }

                    case "mail_ctrl":
                        {
                            Mail_Ctrl = new Mail_Ctrl(GetLines().ToArray());

                            break;
                        }

                    case "mail_def":
                        {
                            Mail_Def.Add(nameless, new Mail_Def(GetLines().ToArray()));

                            break;
                        }

                    case "police_ctrl":
                        {
                            Police_Ctrl = new Police_Ctrl(GetLines().ToArray());

                            break;
                        }

                    case "oversize_offer_ctrl":
                        {
                            Oversize_offer_Ctrl = new Oversize_offer_Ctrl(GetLines().ToArray());

                            break;
                        }

                    case "oversize_route_offers":
                        {
                            Oversize_Route_offers.Add(nameless, new Oversize_Route_offers(GetLines().ToArray()));

                            break;
                        }

                    case "oversize_offer":
                        {
                            Oversize_Offer.Add(nameless, new Oversize_Offer(GetLines().ToArray()));

                            break;
                        }

                    case "delivery_log":
                        {
                            Delivery_log = new Delivery_log(GetLines().ToArray());

                            break;
                        }

                    case "delivery_log_entry":
                        {
                            Delivery_log_Entry.Add(nameless, new Delivery_log_Entry(GetLines().ToArray()));

                            break;
                        }

                    case "ferry_log":
                        {
                            Ferry_log = new Ferry_log(GetLines().ToArray());

                            break;
                        }

                    case "ferry_log_entry":
                        {
                            Ferry_log_Entry.Add(nameless, new Ferry_log_Entry(GetLines().ToArray()));

                            break;
                        }

                    case "gps_waypoint_storage":
                        {
                            GPS_waypoint_Storage.Add(nameless, new GPS_waypoint_Storage(GetLines().ToArray()));

                            break;
                        }

                    case "map_action":
                        {
                            Map_action.Add(nameless, new Map_action(GetLines().ToArray()));

                            break;
                        }

                    case "bus_stop":
                        {
                            Bus_stop.Add(nameless, new Bus_stop(GetLines().ToArray()));

                            break;
                        }

                    case "bus_job_log":
                        {
                            Bus_job_Log = new Bus_job_Log(GetLines().ToArray());

                            break;
                        }

                    default:
                        {
                            if (!NewDataBlocks.Contains(tagLine))
                            {
                                NewDataBlocks.Add(tagLine);
                                Utilities.IO_Utilities.ErrorLogWriter("Save | New Data block | " + tagLine);

                                //Add new block data
                                NewBlocks.Add(nameless, GetLines());
                            }

                            break;
                        }
                }

                continue;

                //===
                List<string> GetLines()
                {
                    string workLine = "";
                    List<string> Data = new List<string>();

                    line--;

                    do
                    {
                        line++;
                        workLine = _input[line];
                        Data.Add(workLine);

                    } while (!_input[line].StartsWith("}"));

                    return Data;
                }
                //===
            }
        }

        internal string PrintOut(uint _version, string _nameless)
        {
            string returnString = "";

            StringBuilder returnSB = new StringBuilder();

            returnSB.AppendLine("SiiNunit" + Environment.NewLine + "{");

            returnSB.AppendLine(Economy.PrintOut(0, ""));

            foreach (KeyValuePair<string, List<string>> blockData in NewBlocks)
            {
                foreach (string blockLine in blockData.Value)
                    returnSB.AppendLine(blockLine);
            }

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            return returnString;
        }

    }
}