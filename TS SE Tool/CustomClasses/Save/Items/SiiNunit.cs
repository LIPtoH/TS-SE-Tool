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
        internal Dictionary<string, dynamic> SiiNitems = new Dictionary<string, dynamic>();

        internal string EconomyNameless = "";

        internal List<string> UnidentifiedBlocks = new List<string>();

        internal List<string> NamelessControlList = new List<string>();

        internal Economy Economy
        {
            get => (Economy)SiiNitems[EconomyNameless];
            set => SiiNitems[EconomyNameless] = value;
        }

        internal Bank Bank
        {
            get => (Bank)SiiNitems[Economy.bank];
            set => SiiNitems[Economy.bank] = value;
        }

        internal Player Player
        {
            get => (Player)SiiNitems[Economy.player];
            set => SiiNitems[Economy.player] = value;
        }

        internal Player_Job Player_Job
        {
            get => Player.current_job != "null" ? (Player_Job)SiiNitems[Player.current_job] : null;
            set => SiiNitems[Player.current_job] = value;
        }

        internal Economy_event_Queue Economy_event_Queue
        {
            get => (Economy_event_Queue)SiiNitems[Economy.event_queue];
            set => SiiNitems[Economy.event_queue] = value;
        }

        internal SiiNunit()
        { }

        internal SiiNunit(string[] _input)
        {
            string tagLine = "", dataLine = "", nameless = "";

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

                NamelessControlList.Add(nameless);

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
                            EconomyNameless = nameless;

                            SiiNitems.Add(nameless, new Economy(GetLines().ToArray()));

                            break;
                        }

                    case "bank":
                        {
                            SiiNitems.Add(nameless, new Bank(GetLines().ToArray()));

                            break;
                        }

                    case "bank_loan":
                        {
                            SiiNitems.Add(nameless, new Bank_Loan(GetLines().ToArray()));

                            break;
                        }

                    case "player":
                        {
                            SiiNitems.Add(nameless, new Player(GetLines().ToArray()));

                            break;
                        }

                    case "trailer":
                        {
                            SiiNitems.Add(nameless, new Trailer(GetLines().ToArray()));

                            break;
                        }

                    case "trailer_utilization_log":
                        {
                            SiiNitems.Add(nameless, new Trailer_Utilization_log(GetLines().ToArray()));

                            break;
                        }

                    case "trailer_utilization_log_entry":
                        {
                            SiiNitems.Add(nameless, new Trailer_Utilization_log_Entry(GetLines().ToArray()));

                            break;
                        }

                    case "trailer_def":
                        {
                            SiiNitems.Add(nameless, new Trailer_Def(GetLines().ToArray()));

                            break;
                        }

                    case "player_job":
                        {
                            SiiNitems.Add(nameless, new Player_Job(GetLines().ToArray()));

                            break;
                        }

                    case "vehicle":
                        {
                            SiiNitems.Add(nameless, new Vehicle(GetLines().ToArray()));

                            break;
                        }

                    case "vehicle_accessory":
                        {
                            SiiNitems.Add(nameless, new Vehicle_Accessory(GetLines().ToArray()));

                            break;
                        }

                    case "vehicle_addon_accessory":
                        {
                            SiiNitems.Add(nameless, new Vehicle_Addon_Accessory(GetLines().ToArray()));

                            break;
                        }

                    case "vehicle_drv_plate_accessory":
                        {
                            SiiNitems.Add(nameless, new Vehicle_Drv_plate_Accessory(GetLines().ToArray()));

                            break;
                        }

                    case "vehicle_wheel_accessory":
                        {
                            SiiNitems.Add(nameless, new Vehicle_Wheel_Accessory(GetLines().ToArray()));

                            break;
                        }

                    case "vehicle_paint_job_accessory":
                        {
                            SiiNitems.Add(nameless, new Vehicle_Paint_job_Accessory(GetLines().ToArray()));

                            break;
                        }

                    case "vehicle_sound_accessory":
                        {
                            SiiNitems.Add(nameless, new Vehicle_Sound_Accessory(GetLines().ToArray()));

                            break;
                        }

                    case "vehicle_cargo_accessory":
                        {
                            SiiNitems.Add(nameless, new Vehicle_Cargo_Accessory(GetLines().ToArray()));

                            break;
                        }

                    case "profit_log":
                        {
                            SiiNitems.Add(nameless, new Profit_log(GetLines().ToArray()));

                            break;
                        }

                    case "profit_log_entry":
                        {
                            SiiNitems.Add(nameless, new Profit_log_Entry(GetLines().ToArray()));

                            break;
                        }

                    case "driver_player":
                        {
                            SiiNitems.Add(nameless, new Driver_Player(GetLines().ToArray()));

                            break;
                        }

                    case "driver_ai":
                        {
                            SiiNitems.Add(nameless, new Driver_AI(GetLines().ToArray()));

                            break;
                        }

                    case "job_info":
                        {
                            SiiNitems.Add(nameless, new Job_Info(GetLines().ToArray()));

                            break;
                        }

                    case "company":
                        {
                            SiiNitems.Add(nameless, new Company(GetLines().ToArray()));

                            break;
                        }
                    case "job_offer_data":
                        {
                            SiiNitems.Add(nameless, new Job_offer_Data(GetLines().ToArray()));

                            break;
                        }

                    case "garage":
                        {
                            SiiNitems.Add(nameless, new Garage(GetLines().ToArray()));

                            break;
                        }

                    case "game_progress":
                        {
                            SiiNitems.Add(nameless, new Game_Progress(GetLines().ToArray()));

                            break;
                        }

                    case "registry":
                        {
                            SiiNitems.Add(nameless, new Registry(GetLines().ToArray()));

                            break;
                        }

                    case "transport_data":
                        {
                            SiiNitems.Add(nameless, new Transport_Data(GetLines().ToArray()));

                            break;
                        }

                    case "economy_event_queue":
                        {
                            SiiNitems.Add(nameless, new Economy_event_Queue(GetLines().ToArray()));

                            break;
                        }

                    case "economy_event":
                        {
                            SiiNitems.Add(nameless, new Economy_event(GetLines().ToArray()));

                            break;
                        }

                    case "mail_ctrl":
                        {
                            SiiNitems.Add(nameless, new Mail_Ctrl(GetLines().ToArray()));

                            break;
                        }

                    case "mail_def":
                        {
                            SiiNitems.Add(nameless, new Mail_Def(GetLines().ToArray()));

                            break;
                        }

                    case "oversize_job_save":
                        {
                            SiiNitems.Add(nameless, new Oversize_Job_save(GetLines().ToArray()));

                            break;
                        }

                    case "trajectory_orders_save":
                        {
                            SiiNitems.Add(nameless, new Trajectory_orders_Save(GetLines().ToArray()));

                            break;
                        }

                    case "oversize_block_rule_save":
                        {
                            SiiNitems.Add(nameless, new Oversize_Block_rule_Save(GetLines().ToArray()));

                            break;
                        }

                    case "police_ctrl":
                        {
                            SiiNitems.Add(nameless, new Police_Ctrl(GetLines().ToArray()));

                            break;
                        }

                    case "oversize_offer_ctrl":
                        {
                            SiiNitems.Add(nameless, new Oversize_offer_Ctrl(GetLines().ToArray()));

                            break;
                        }

                    case "oversize_route_offers":
                        {
                            SiiNitems.Add(nameless, new Oversize_Route_offers(GetLines().ToArray()));

                            break;
                        }

                    case "oversize_offer":
                        {
                            SiiNitems.Add(nameless, new Oversize_Offer(GetLines().ToArray()));

                            break;
                        }

                    case "delivery_log":
                        {
                            SiiNitems.Add(nameless, new Delivery_log(GetLines().ToArray()));

                            break;
                        }

                    case "delivery_log_entry":
                        {
                            SiiNitems.Add(nameless, new Delivery_log_Entry(GetLines().ToArray()));

                            break;
                        }

                    case "ferry_log":
                        {
                            SiiNitems.Add(nameless, new Ferry_log(GetLines().ToArray()));

                            break;
                        }

                    case "ferry_log_entry":
                        {
                            SiiNitems.Add(nameless, new Ferry_log_Entry(GetLines().ToArray()));

                            break;
                        }

                    case "gps_waypoint_storage":
                        {
                            SiiNitems.Add(nameless, new GPS_waypoint_Storage(GetLines().ToArray()));

                            break;
                        }

                    case "map_action":
                        {
                            SiiNitems.Add(nameless, new Map_action(GetLines().ToArray()));

                            break;
                        }

                    case "bus_stop":
                        {
                            SiiNitems.Add(nameless, new Bus_stop(GetLines().ToArray()));

                            break;
                        }
                        
                    case "bus_job_log":
                        {
                            SiiNitems.Add(nameless, new Bus_job_Log(GetLines().ToArray()));

                            break;
                        }
                        
                    default:
                        {
                            List<string> tmpNewBlockLines = GetLines();

                            UnidentifiedBlocks.Add(nameless);
                            SiiNitems.Add(nameless, new Unidentified(tmpNewBlockLines));

                            Utilities.IO_Utilities.ErrorLogWriter("Save | New Data block | " + tagLine + Environment.NewLine + 
                                string.Join(Environment.NewLine, tmpNewBlockLines));

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

        internal string PrintOut(uint _version)
        {
            string returnString = "";

            StringBuilder returnSB = new StringBuilder();

            returnSB.AppendLine("SiiNunit" + Environment.NewLine + "{");

            //=== Economy

            returnSB.AppendLine(Economy.PrintOut(0, EconomyNameless));

            //=== Bank

            returnSB.AppendLine(Bank.PrintOut(0, Economy.bank));

            foreach (string item in Bank.loans.Where(x => x != null && x != "null"))
            {
                returnSB.AppendLine(SiiNitems[item].PrintOut(0, item));
            }

            //=== Player

            returnSB.AppendLine(Player.PrintOut(0, Economy.player));

            //=== Trailers

            List<string> tmpAccList = new List<string>();

            foreach (string item in Player.trailers.Where(x => x != null && x != "null"))
            {
                string trailerNameless = item;

                trStart:;

                Trailer Trailer = SiiNitems[trailerNameless];

                returnSB.AppendLine(Trailer.PrintOut(0, trailerNameless));

                tmpAccList.InsertRange(0, Trailer.accessories);

                if (Trailer.slave_trailer != "null")
                {
                    trailerNameless = Trailer.slave_trailer;
                    goto trStart;
                }

                foreach (string accNameless in tmpAccList.Where(x => x != null && x != "null"))
                {
                    returnSB.AppendLine(SiiNitems[accNameless].PrintOut(0, accNameless));
                }

                tmpAccList.Clear();
            }

            foreach (string item in Player.trailer_utilization_logs.Where(x => x != null && x != "null"))
            {
                Trailer_Utilization_log Trailer_Utilization_log = SiiNitems[item];

                returnSB.AppendLine(Trailer_Utilization_log.PrintOut(0, item));

                foreach (string item2 in Trailer_Utilization_log.entries.Where(x => x != null && x != "null"))
                {
                    returnSB.AppendLine(SiiNitems[item2].PrintOut(0, item2));
                }
            }

            foreach (string item in Player.trailer_defs.Where(x => x != null && x != "null"))
            {
                returnSB.AppendLine(SiiNitems[item].PrintOut(0, item));
            }

            //=== Current job

            if (Player.current_job != "null")
            {
                Player_Job Player_Job = SiiNitems[Player.current_job];

                returnSB.AppendLine(Player_Job.PrintOut(0, Player.current_job));

                if (Player_Job.company_truck != "null")
                {
                    Vehicle Vehicle = SiiNitems[Player_Job.company_truck];

                    returnSB.AppendLine(Vehicle.PrintOut(0, Player_Job.company_truck));

                    foreach (string accNameless in Vehicle.accessories.Where(x => x != null && x != "null"))
                    {
                        returnSB.AppendLine(SiiNitems[accNameless].PrintOut(0, accNameless));
                    }
                }

                if (Player_Job.company_trailer != "null")
                {
                    string trailerNameless = Player_Job.company_trailer;

                    trStart:;

                    Trailer Trailer = SiiNitems[trailerNameless];

                    returnSB.AppendLine(Trailer.PrintOut(0, trailerNameless));

                    tmpAccList.InsertRange(0, Trailer.accessories);

                    if (Trailer.slave_trailer != "null")
                    {
                        trailerNameless = Trailer.slave_trailer;
                        goto trStart;
                    }

                    foreach (string accNameless in tmpAccList.Where(x => x != null && x != "null"))
                    {
                        returnSB.AppendLine(SiiNitems[accNameless].PrintOut(0, accNameless));
                    }

                    tmpAccList.Clear();
                }

                if (Player_Job.special != "null")
                {
                    returnSB.AppendLine(SiiNitems[Player_Job.special].PrintOut(0, Player_Job.special));
                }
            }

            //=== Selected job

            if (Player.selected_job != "null")
            {
                Job_Info Job_Info = SiiNitems[Player.selected_job];

                returnSB.AppendLine(Job_Info.PrintOut(0, Player.selected_job));

                if (Job_Info.special != null)
                {
                    returnSB.AppendLine(SiiNitems[Job_Info.special].PrintOut(0, Job_Info.special));
                }
            }

            //=== Trucks

            foreach (string item in Player.trucks.Where(x => x != null && x != "null"))
            {
                Vehicle Vehicle = SiiNitems[item];

                returnSB.AppendLine(Vehicle.PrintOut(0, item));

                foreach (string accNameless in Vehicle.accessories.Where(x => x != null && x != "null"))
                {
                    returnSB.AppendLine(SiiNitems[accNameless].PrintOut(0, accNameless));
                }
            }

            //---

            foreach (string item in Player.truck_profit_logs.Where(x => x != null && x != "null"))
            {
                Profit_log Profit_log = SiiNitems[item];

                returnSB.AppendLine(Profit_log.PrintOut(0, item));

                foreach (string item2 in Profit_log.stats_data.Where(x => x != null && x != "null"))
                {
                    returnSB.AppendLine(SiiNitems[item2].PrintOut(0, item2));
                }
            }

            //=== Drivers

            foreach (string item in Player.drivers.Where(x => x != null && x != "null"))
            {
                returnSB.AppendLine(SiiNitems[item].PrintOut(0, item));

                if (SiiNitems[item] is Driver_Player)
                {
                    Driver_Player Driver_Player = SiiNitems[item];

                    Profit_log Profit_log = SiiNitems[Driver_Player.profit_log];

                    returnSB.AppendLine(Profit_log.PrintOut(0, Driver_Player.profit_log));                    

                    foreach (string item2 in Profit_log.stats_data.Where(x => x != null && x != "null"))
                    {
                        returnSB.AppendLine(SiiNitems[item2].PrintOut(0, item2));
                    }
                }
                else
                {
                    Driver_AI Driver_AI = SiiNitems[item];

                    returnSB.AppendLine(SiiNitems[Driver_AI.driver_job].PrintOut(0, Driver_AI.driver_job));

                    Profit_log Profit_log = SiiNitems[Driver_AI.profit_log];

                    returnSB.AppendLine(Profit_log.PrintOut(0, Driver_AI.profit_log));

                    foreach (string item2 in Profit_log.stats_data.Where(x => x != null && x != "null"))
                    {
                        returnSB.AppendLine(SiiNitems[item2].PrintOut(0, item2));
                    }
                }
            }

            //=== Companies

            foreach (string item in Economy.companies.Where(x => x != null && x != "null"))
            {
                Company Company = SiiNitems[item];

                returnSB.AppendLine(Company.PrintOut(0, item));

                foreach (string item2 in Company.job_offer.Where(x => x != null && x != "null"))
                {
                    returnSB.AppendLine(SiiNitems[item2].PrintOut(0, item2));
                }
            }

            //=== Garages

            foreach (string item in Economy.garages.Where(x => x != null && x != "null"))
            {
                Garage Garage = SiiNitems[item];

                returnSB.AppendLine(Garage.PrintOut(0, item));

                Profit_log Profit_log = SiiNitems[Garage.profit_log];

                returnSB.AppendLine(Profit_log.PrintOut(0, Garage.profit_log));

                foreach (string item2 in Profit_log.stats_data.Where(x => x != null && x != "null"))
                {
                    returnSB.AppendLine(SiiNitems[item2].PrintOut(0, item2));
                }
            }

            //=== Game Progress

            Game_Progress Game_Progress = SiiNitems[Economy.game_progress];

            returnSB.AppendLine(Game_Progress.PrintOut(0, Economy.game_progress));

            //---

            Transport_Data Transport_Data = SiiNitems[Game_Progress.generic_transports];

            returnSB.AppendLine(Transport_Data.PrintOut(0, Game_Progress.generic_transports));

            //---

            Transport_Data = SiiNitems[Game_Progress.undamaged_transports];

            returnSB.AppendLine(Transport_Data.PrintOut(0, Game_Progress.undamaged_transports));

            //---

            Transport_Data = SiiNitems[Game_Progress.clean_transports];

            returnSB.AppendLine(Transport_Data.PrintOut(0, Game_Progress.clean_transports));

            //=== Economy event Queue

            returnSB.AppendLine(Economy_event_Queue.PrintOut(0, Economy.event_queue));

            foreach (string item in Economy_event_Queue.data.Where(x => x != null && x != "null"))
            {
                returnSB.AppendLine(SiiNitems[item].PrintOut(0, item));
            }

            //=== Mail Control

            Mail_Ctrl Mail_Ctrl = SiiNitems[Economy.mail_ctrl];

            returnSB.AppendLine(Mail_Ctrl.PrintOut(0, Economy.mail_ctrl));

            foreach (string item in Mail_Ctrl.inbox.Where(x => x != null && x != "null"))
            {
                returnSB.AppendLine(SiiNitems[item].PrintOut(0, item));
            }

            //=== Oversize offer

            Oversize_offer_Ctrl Oversize_offer_Ctrl = SiiNitems[Economy.oversize_offer_ctrl];

            returnSB.AppendLine(Oversize_offer_Ctrl.PrintOut(0, Economy.oversize_offer_ctrl));

            foreach (string item in Oversize_offer_Ctrl.route_offers.Where(x => x != null && x != "null"))
            {
                Oversize_Route_offers Oversize_Route_offers = SiiNitems[item];

                returnSB.AppendLine(Oversize_Route_offers.PrintOut(0, item));

                foreach (string item2 in Oversize_Route_offers.offers.Where(x => x != null && x != "null"))
                {
                    returnSB.AppendLine(SiiNitems[item2].PrintOut(0, item2));
                }
            }

            //=== Delivery log

            Delivery_log Delivery_log = SiiNitems[Economy.delivery_log];

            returnSB.AppendLine(Delivery_log.PrintOut(0, Economy.delivery_log));

            foreach (string item in Delivery_log.entries.Where(x => x != null && x != "null"))
            {
                returnSB.AppendLine(SiiNitems[item].PrintOut(0, item));
            }

            //=== Ferry log

            if (SiiNitems.ContainsKey(Economy.ferry_log))
            {
                Ferry_log Ferry_log = SiiNitems[Economy.ferry_log];

                returnSB.AppendLine(Ferry_log.PrintOut(0, Economy.ferry_log));

                foreach (string item in Ferry_log.entries.Where(x => x != null && x != "null"))
                {
                    returnSB.AppendLine(SiiNitems[item].PrintOut(0, item));
                }
            }

            //=== GPS Online

            foreach (string item in Economy.stored_online_gps_behind_waypoints.Where(x => x != null && x != "null"))
            {
                returnSB.AppendLine(SiiNitems[item].PrintOut(0, item));
            }

            foreach (string item in Economy.stored_online_gps_ahead_waypoints.Where(x => x != null && x != "null"))
            {
                returnSB.AppendLine(SiiNitems[item].PrintOut(0, item));
            }

            foreach (string item in Economy.stored_online_gps_avoid_waypoints.Where(x => x != null && x != "null"))
            {
                returnSB.AppendLine(SiiNitems[item].PrintOut(0, item));
            }

            //=== Special job

            if (Economy.stored_special_job != "null")
            {
                Oversize_Job_save Oversize_Job_save = SiiNitems[Economy.stored_special_job];

                returnSB.AppendLine(Oversize_Job_save.PrintOut(0, Economy.stored_special_job));

                foreach (string item in Oversize_Job_save.trajectory_orders.Where(x => x != null && x != "null"))
                {
                    returnSB.AppendLine(SiiNitems[item].PrintOut(0, item));
                }

                foreach (string item in Oversize_Job_save.active_blocks_rules.Where(x => x != null && x != "null"))
                {
                    returnSB.AppendLine(SiiNitems[item].PrintOut(0, item));
                }
            }

            //=== Police Control

            returnSB.AppendLine(SiiNitems[Economy.police_ctrl].PrintOut(0, Economy.police_ctrl));

            //=== GPS

            foreach (string item in Economy.stored_gps_behind_waypoints.Where(x => x != null && x != "null"))
            {
                returnSB.AppendLine(SiiNitems[item].PrintOut(0, item));
            }

            foreach (string item in Economy.stored_gps_ahead_waypoints.Where(x => x != null && x != "null"))
            {
                returnSB.AppendLine(SiiNitems[item].PrintOut(0, item));
            }

            foreach (string item in Economy.stored_gps_avoid_waypoints.Where(x => x != null && x != "null"))
            {
                returnSB.AppendLine(SiiNitems[item].PrintOut(0, item));
            }

            //=== Stored Map actions

            foreach (string item in Economy.stored_map_actions.Where(x => x != null && x != "null"))
            {
                returnSB.AppendLine(SiiNitems[item].PrintOut(0, item));
            }

            //=== AI Drivers offer

            List<string> AI_Drivers = new List<string>();

            AI_Drivers.AddRange(Economy.drivers_offer);
            AI_Drivers.AddRange(Economy.driver_pool);

            foreach (string item in AI_Drivers.Where(x => x != null && x != "null"))
            {
                Driver_AI Driver_AI = SiiNitems[item];

                returnSB.AppendLine(Driver_AI.PrintOut(0, item));

                //

                string jobNameless = Driver_AI.driver_job;

                returnSB.AppendLine(SiiNitems[jobNameless].PrintOut(0, jobNameless));

                //

                string logNameless = Driver_AI.profit_log;

                Profit_log Profit_log = SiiNitems[logNameless];

                returnSB.AppendLine(Profit_log.PrintOut(0, logNameless));

                foreach (string statNameless in Profit_log.stats_data.Where(x => x != null && x != "null"))
                {
                    returnSB.AppendLine(SiiNitems[statNameless].PrintOut(0, statNameless));
                }
            }

            //=== Registry

            returnSB.AppendLine(SiiNitems[Economy.registry].PrintOut(0, Economy.registry));

            //=== Bus stops

            foreach (string item in Economy.bus_stops.Where(x => x != null && x != "null"))
            {
                returnSB.AppendLine(SiiNitems[item].PrintOut(0, item));
            }

            //=== Bus job Log

            Bus_job_Log Bus_job_Log = SiiNitems[Economy.bus_job_log];

            returnSB.AppendLine(Bus_job_Log.PrintOut(0, Economy.bus_job_log));

            foreach (string item in Bus_job_Log.entries.Where(x => x != null && x != "null"))
            {
                returnSB.AppendLine(SiiNitems[item].PrintOut(0, item));
            }

            //=== Unidentified blocks

            foreach (string item in UnidentifiedBlocks)
            {
                returnSB.AppendLine(SiiNitems[item].PrintOut(0, item));
            }

            //=== Skipped blocks

            if (NamelessControlList.Count > 0)
            {
                List<string> skippedBlocks = new List<string>();
                skippedBlocks.AddRange(NamelessControlList);

                foreach (string item in skippedBlocks)
                {
                    returnSB.AppendLine(SiiNitems[item].PrintOut(0, item));
                }
            }
            
            returnSB.Append("}");

            returnString = returnSB.ToString();

            return returnString;
        }

    }
}