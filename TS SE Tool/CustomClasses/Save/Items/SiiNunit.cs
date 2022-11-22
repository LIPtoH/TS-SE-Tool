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

        internal List<string> NamelessIgnoreList = new List<string>();

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

                SiiNitems.Add(nameless, DetectTag(nameless, tagLine, GetLines().ToArray()));

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

        public dynamic DetectTag(string tagLine, string[] _input)
        {
            return DetectTag("", tagLine, _input);
        }

        public dynamic DetectTag(string nameless, string tagLine, string[] _input)
        {
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
                        return new Economy(_input);
                    }

                case "bank":
                    {
                        return new Bank(_input);
                    }

                case "bank_loan":
                    {
                        return new Bank_Loan(_input);
                    }

                case "player":
                    {
                        return new Player(_input);
                    }

                case "trailer":
                    {
                        return new Trailer(_input);
                    }

                case "trailer_utilization_log":
                    {
                        return new Trailer_Utilization_log(_input);
                    }

                case "trailer_utilization_log_entry":
                    {
                        return new Trailer_Utilization_log_Entry(_input);
                    }

                case "trailer_def":
                    {
                        return new Trailer_Def(_input);
                    }

                case "player_job":
                    {
                        return new Player_Job(_input);
                    }

                case "vehicle":
                    {
                        return new Vehicle(_input);
                    }

                case "vehicle_accessory":
                    {
                        return new Vehicle_Accessory(_input);
                    }

                case "vehicle_addon_accessory":
                    {
                        return new Vehicle_Addon_Accessory(_input);
                    }

                case "vehicle_drv_plate_accessory":
                    {
                        return new Vehicle_Drv_plate_Accessory(_input);
                    }

                case "vehicle_wheel_accessory":
                    {
                        return new Vehicle_Wheel_Accessory(_input);
                    }

                case "vehicle_paint_job_accessory":
                    {
                        return new Vehicle_Paint_job_Accessory(_input);
                    }

                case "vehicle_sound_accessory":
                    {
                        return new Vehicle_Sound_Accessory(_input);
                    }

                case "vehicle_cargo_accessory":
                    {
                        return new Vehicle_Cargo_Accessory(_input);
                    }

                case "profit_log":
                    {
                        return new Profit_log(_input);
                    }

                case "profit_log_entry":
                    {
                        return new Profit_log_Entry(_input);
                    }

                case "driver_player":
                    {
                        return new Driver_Player(_input);
                    }

                case "driver_ai":
                    {
                        return new Driver_AI(_input);
                    }

                case "job_info":
                    {
                        return new Job_Info(_input);
                    }

                case "company":
                    {
                        return new Company(_input);
                    }
                case "job_offer_data":
                    {
                        return new Job_offer_Data(_input);
                    }

                case "garage":
                    {
                        return new Garage(_input);
                    }

                case "game_progress":
                    {
                        return new Game_Progress(_input);
                    }

                case "registry":
                    {
                        return new Registry(_input);
                    }

                case "transport_data":
                    {
                        return new Transport_Data(_input);
                    }

                case "economy_event_queue":
                    {
                        return new Economy_event_Queue(_input);
                    }

                case "economy_event":
                    {
                        return new Economy_event(_input);
                    }

                case "mail_ctrl":
                    {
                        return new Mail_Ctrl(_input);
                    }

                case "mail_def":
                    {
                        return new Mail_Def(_input);
                    }

                case "oversize_job_save":
                    {
                        return new Oversize_Job_save(_input);
                    }

                case "trajectory_orders_save":
                    {
                        return new Trajectory_orders_Save(_input);
                    }

                case "oversize_block_rule_save":
                    {
                        return new Oversize_Block_rule_Save(_input);
                    }

                case "police_ctrl":
                    {
                        return new Police_Ctrl(_input);
                    }

                case "oversize_offer_ctrl":
                    {
                        return new Oversize_offer_Ctrl(_input);
                    }

                case "oversize_route_offers":
                    {
                        return new Oversize_Route_offers(_input);
                    }

                case "oversize_offer":
                    {
                        return new Oversize_Offer(_input);
                    }

                case "delivery_log":
                    {
                        return new Delivery_log(_input);
                    }

                case "delivery_log_entry":
                    {
                        return new Delivery_log_Entry(_input);
                    }

                case "ferry_log":
                    {
                        return new Ferry_log(_input);
                    }

                case "ferry_log_entry":
                    {
                        return new Ferry_log_Entry(_input);
                    }

                case "gps_waypoint_storage":
                    {
                        return new GPS_waypoint_Storage(_input);
                    }

                case "map_action":
                    {
                        return new Map_action(_input);
                    }

                case "bus_stop":
                    {
                        return new Bus_stop(_input);
                    }

                case "bus_job_log":
                    {
                        return new Bus_job_Log(_input);
                    }

                default:
                    {
                        List<string> tmpNewBlockLines = _input.ToList();

                        UnidentifiedBlocks.Add(nameless);

                        Utilities.IO_Utilities.ErrorLogWriter("Save | New Data block | " + tagLine + Environment.NewLine +
                            string.Join(Environment.NewLine, tmpNewBlockLines));

                        return new Unidentified(tmpNewBlockLines);
                    }
            }

            return null;
        }

        internal string PrintOut(uint _version)
        {
            string returnString = "";

            StringBuilder returnSB = new StringBuilder();

            //=== Remove Ignored blocks

            if (NamelessIgnoreList.Count > 0)
            {
                foreach (string item in NamelessIgnoreList.Where(x => x != null && x != "null"))
                {
                    Type accType = SiiNitems[item].GetType();

                    switch (accType.Name)
                    {
                        case "Vehicle":
                            {
                                Vehicle tmpItem = (Vehicle)SiiNitems[item];

                                foreach (string acc in tmpItem.accessories)
                                {
                                    NamelessControlList.Remove(acc);
                                }

                                NamelessControlList.Remove(item);
                                break;
                            }

                        case "Trailer":
                            {
                                Trailer tmpItem = (Trailer)SiiNitems[item];

                                foreach (string acc in tmpItem.accessories)
                                {
                                    NamelessControlList.Remove(acc);
                                }

                                NamelessControlList.Remove(item);
                                break;
                            }

                        default:
                            {
                                NamelessControlList.Remove(item);
                                break;
                            }
                    }
                }
            }

            //=== Start row

            returnSB.AppendLine("SiiNunit" + Environment.NewLine + "{");

            //=== Economy

            returnSB.AppendLine(Economy.PrintOut(_version, EconomyNameless));

            //=== Bank

            returnSB.AppendLine(Bank.PrintOut(_version, Economy.bank));

            foreach (string item in Bank.loans.Where(x => x != null && x != "null"))
            {
                returnSB.AppendLine(SiiNitems[item].PrintOut(_version, item));
            }

            //=== Player

            returnSB.AppendLine(Player.PrintOut(_version, Economy.player));

            //=== Trailers

            List<string> tmpAccList = new List<string>();

            foreach (string item in Player.trailers.Where(x => x != null && x != "null"))
            {
                string trailerNameless = item;

                trStart:;

                Trailer Trailer = SiiNitems[trailerNameless];

                returnSB.AppendLine(Trailer.PrintOut(_version, trailerNameless));

                tmpAccList.InsertRange(0, Trailer.accessories);

                if (Trailer.slave_trailer != "null")
                {
                    trailerNameless = Trailer.slave_trailer;
                    goto trStart;
                }

                foreach (string accNameless in tmpAccList.Where(x => x != null && x != "null"))
                {
                    returnSB.AppendLine(SiiNitems[accNameless].PrintOut(_version, accNameless));
                }

                tmpAccList.Clear();
            }

            //--- utilization logs

            foreach (string item in Player.trailer_utilization_logs.Where(x => x != null && x != "null"))
            {
                Trailer_Utilization_log Trailer_Utilization_log = SiiNitems[item];

                returnSB.AppendLine(Trailer_Utilization_log.PrintOut(_version, item));

                foreach (string item2 in Trailer_Utilization_log.entries.Where(x => x != null && x != "null"))
                {
                    returnSB.AppendLine(SiiNitems[item2].PrintOut(_version, item2));
                }
            }

            //--- definitions

            foreach (string item in Player.trailer_defs.Where(x => x != null && x != "null"))
            {
                returnSB.AppendLine(SiiNitems[item].PrintOut(_version, item));
            }

            //=== Current job

            if (Player.current_job != "null")
            {
                Player_Job Player_Job = SiiNitems[Player.current_job];

                returnSB.AppendLine(Player_Job.PrintOut(_version, Player.current_job));

                if (Player_Job.company_truck != "null")
                {
                    Vehicle Vehicle = SiiNitems[Player_Job.company_truck];

                    returnSB.AppendLine(Vehicle.PrintOut(_version, Player_Job.company_truck));

                    foreach (string accNameless in Vehicle.accessories.Where(x => x != null && x != "null"))
                    {
                        returnSB.AppendLine(SiiNitems[accNameless].PrintOut(_version, accNameless));
                    }
                }

                if (Player_Job.company_trailer != "null")
                {
                    string trailerNameless = Player_Job.company_trailer;

                    trStart:;

                    Trailer Trailer = SiiNitems[trailerNameless];

                    returnSB.AppendLine(Trailer.PrintOut(_version, trailerNameless));

                    tmpAccList.InsertRange(0, Trailer.accessories);

                    if (Trailer.slave_trailer != "null")
                    {
                        trailerNameless = Trailer.slave_trailer;
                        goto trStart;
                    }

                    foreach (string accNameless in tmpAccList.Where(x => x != null && x != "null"))
                    {
                        returnSB.AppendLine(SiiNitems[accNameless].PrintOut(_version, accNameless));
                    }

                    tmpAccList.Clear();
                }

                if (Player_Job.special != "null")
                {
                    returnSB.AppendLine(SiiNitems[Player_Job.special].PrintOut(_version, Player_Job.special));
                }
            }

            //=== Selected job

            if (Player.selected_job != "null")
            {
                Job_Info Job_Info = SiiNitems[Player.selected_job];

                returnSB.AppendLine(Job_Info.PrintOut(_version, Player.selected_job));

                if (Job_Info.special != "null")
                {
                    returnSB.AppendLine(SiiNitems[Job_Info.special].PrintOut(_version, Job_Info.special));
                }
            }

            //=== Trucks

            foreach (string item in Player.trucks.Where(x => x != null && x != "null"))
            {
                Vehicle Vehicle = SiiNitems[item];

                returnSB.AppendLine(Vehicle.PrintOut(_version, item));

                foreach (string accNameless in Vehicle.accessories.Where(x => x != null && x != "null"))
                {
                    returnSB.AppendLine(SiiNitems[accNameless].PrintOut(_version, accNameless));
                }
            }

            //--- Profit logs

            foreach (string item in Player.truck_profit_logs.Where(x => x != null && x != "null"))
            {
                Profit_log Profit_log = SiiNitems[item];

                returnSB.AppendLine(Profit_log.PrintOut(_version, item));

                foreach (string item2 in Profit_log.stats_data.Where(x => x != null && x != "null"))
                {
                    returnSB.AppendLine(SiiNitems[item2].PrintOut(_version, item2));
                }
            }

            //=== Drivers

            foreach (string item in Player.drivers.Where(x => x != null && x != "null"))
            {
                returnSB.AppendLine(SiiNitems[item].PrintOut(_version, item));

                if (SiiNitems[item] is Driver_Player)
                {
                    Driver_Player Driver_Player = SiiNitems[item];

                    Profit_log Profit_log = SiiNitems[Driver_Player.profit_log];

                    returnSB.AppendLine(Profit_log.PrintOut(_version, Driver_Player.profit_log));

                    foreach (string item2 in Profit_log.stats_data.Where(x => x != null && x != "null"))
                    {
                        returnSB.AppendLine(SiiNitems[item2].PrintOut(_version, item2));
                    }
                }
                else
                {
                    Driver_AI Driver_AI = SiiNitems[item];

                    returnSB.AppendLine(SiiNitems[Driver_AI.driver_job].PrintOut(_version, Driver_AI.driver_job));

                    Profit_log Profit_log = SiiNitems[Driver_AI.profit_log];

                    returnSB.AppendLine(Profit_log.PrintOut(_version, Driver_AI.profit_log));

                    foreach (string item2 in Profit_log.stats_data.Where(x => x != null && x != "null"))
                    {
                        returnSB.AppendLine(SiiNitems[item2].PrintOut(_version, item2));
                    }
                }
            }

            //=== Companies

            foreach (string item in Economy.companies.Where(x => x != null && x != "null"))
            {
                Company Company = SiiNitems[item];

                returnSB.AppendLine(Company.PrintOut(_version, item));

                //--- Delivered Trailer

                if (Company.delivered_trailer != "null")
                {
                    string trailerNameless = Company.delivered_trailer;

                    trStart:;

                    Trailer Trailer = SiiNitems[trailerNameless];

                    returnSB.AppendLine(Trailer.PrintOut(_version, trailerNameless));

                    tmpAccList.InsertRange(0, Trailer.accessories);

                    if (Trailer.slave_trailer != "null")
                    {
                        trailerNameless = Trailer.slave_trailer;
                        goto trStart;
                    }

                    foreach (string accNameless in tmpAccList.Where(x => x != null && x != "null"))
                    {
                        returnSB.AppendLine(SiiNitems[accNameless].PrintOut(_version, accNameless));
                    }

                    tmpAccList.Clear();
                }

                //--- Job offers

                foreach (string item2 in Company.job_offer.Where(x => x != null && x != "null"))
                {
                    returnSB.AppendLine(SiiNitems[item2].PrintOut(_version, item2));
                }
            }

            //=== Garages

            foreach (string item in Economy.garages.Where(x => x != null && x != "null"))
            {
                Garage Garage = SiiNitems[item];

                returnSB.AppendLine(Garage.PrintOut(_version, item));

                Profit_log Profit_log = SiiNitems[Garage.profit_log];

                returnSB.AppendLine(Profit_log.PrintOut(_version, Garage.profit_log));

                foreach (string item2 in Profit_log.stats_data.Where(x => x != null && x != "null"))
                {
                    returnSB.AppendLine(SiiNitems[item2].PrintOut(_version, item2));
                }
            }

            //=== Game Progress

            Game_Progress Game_Progress = SiiNitems[Economy.game_progress];

            returnSB.AppendLine(Game_Progress.PrintOut(_version, Economy.game_progress));

            //---

            Transport_Data Transport_Data = SiiNitems[Game_Progress.generic_transports];

            returnSB.AppendLine(Transport_Data.PrintOut(_version, Game_Progress.generic_transports));

            //---

            Transport_Data = SiiNitems[Game_Progress.undamaged_transports];

            returnSB.AppendLine(Transport_Data.PrintOut(_version, Game_Progress.undamaged_transports));

            //---

            Transport_Data = SiiNitems[Game_Progress.clean_transports];

            returnSB.AppendLine(Transport_Data.PrintOut(_version, Game_Progress.clean_transports));

            //=== Economy event Queue

            returnSB.AppendLine(Economy_event_Queue.PrintOut(_version, Economy.event_queue));

            foreach (string item in Economy_event_Queue.data.Where(x => x != null && x != "null"))
            {
                returnSB.AppendLine(SiiNitems[item].PrintOut(_version, item));
            }

            //=== Mail Control

            Mail_Ctrl Mail_Ctrl = SiiNitems[Economy.mail_ctrl];

            returnSB.AppendLine(Mail_Ctrl.PrintOut(_version, Economy.mail_ctrl));

            foreach (string item in Mail_Ctrl.inbox.Where(x => x != null && x != "null"))
            {
                returnSB.AppendLine(SiiNitems[item].PrintOut(_version, item));
            }

            foreach (string item in Mail_Ctrl.pending_mails.Where(x => x != null && x != "null"))
            {
                returnSB.AppendLine(SiiNitems[item].PrintOut(_version, item));
            }

            //=== Oversize offer

            Oversize_offer_Ctrl Oversize_offer_Ctrl = SiiNitems[Economy.oversize_offer_ctrl];

            returnSB.AppendLine(Oversize_offer_Ctrl.PrintOut(_version, Economy.oversize_offer_ctrl));

            foreach (string item in Oversize_offer_Ctrl.route_offers.Where(x => x != null && x != "null"))
            {
                Oversize_Route_offers Oversize_Route_offers = SiiNitems[item];

                returnSB.AppendLine(Oversize_Route_offers.PrintOut(_version, item));

                foreach (string item2 in Oversize_Route_offers.offers.Where(x => x != null && x != "null"))
                {
                    returnSB.AppendLine(SiiNitems[item2].PrintOut(_version, item2));
                }
            }

            //=== Delivery log

            Delivery_log Delivery_log = SiiNitems[Economy.delivery_log];

            returnSB.AppendLine(Delivery_log.PrintOut(_version, Economy.delivery_log));

            foreach (string item in Delivery_log.entries.Where(x => x != null && x != "null"))
            {
                returnSB.AppendLine(SiiNitems[item].PrintOut(_version, item));
            }

            //=== Ferry log

            if (SiiNitems.ContainsKey(Economy.ferry_log))
            {
                Ferry_log Ferry_log = SiiNitems[Economy.ferry_log];

                returnSB.AppendLine(Ferry_log.PrintOut(_version, Economy.ferry_log));

                foreach (string item in Ferry_log.entries.Where(x => x != null && x != "null"))
                {
                    returnSB.AppendLine(SiiNitems[item].PrintOut(_version, item));
                }
            }

            //=== GPS Online

            foreach (string item in Economy.stored_online_gps_behind_waypoints.Where(x => x != null && x != "null"))
            {
                returnSB.AppendLine(SiiNitems[item].PrintOut(_version, item));
            }

            foreach (string item in Economy.stored_online_gps_ahead_waypoints.Where(x => x != null && x != "null"))
            {
                returnSB.AppendLine(SiiNitems[item].PrintOut(_version, item));
            }

            foreach (string item in Economy.stored_online_gps_avoid_waypoints.Where(x => x != null && x != "null"))
            {
                returnSB.AppendLine(SiiNitems[item].PrintOut(_version, item));
            }

            //=== Special job

            if (Economy.stored_special_job != "null")
            {
                Oversize_Job_save Oversize_Job_save = SiiNitems[Economy.stored_special_job];

                returnSB.AppendLine(Oversize_Job_save.PrintOut(_version, Economy.stored_special_job));

                foreach (string item in Oversize_Job_save.trajectory_orders.Where(x => x != null && x != "null"))
                {
                    returnSB.AppendLine(SiiNitems[item].PrintOut(_version, item));
                }

                foreach (string item in Oversize_Job_save.active_blocks_rules.Where(x => x != null && x != "null"))
                {
                    returnSB.AppendLine(SiiNitems[item].PrintOut(_version, item));
                }
            }

            //=== Police Control

            returnSB.AppendLine(SiiNitems[Economy.police_ctrl].PrintOut(_version, Economy.police_ctrl));

            //=== GPS

            foreach (string item in Economy.stored_gps_behind_waypoints.Where(x => x != null && x != "null"))
            {
                returnSB.AppendLine(SiiNitems[item].PrintOut(_version, item));
            }

            foreach (string item in Economy.stored_gps_ahead_waypoints.Where(x => x != null && x != "null"))
            {
                returnSB.AppendLine(SiiNitems[item].PrintOut(_version, item));
            }

            foreach (string item in Economy.stored_gps_avoid_waypoints.Where(x => x != null && x != "null"))
            {
                returnSB.AppendLine(SiiNitems[item].PrintOut(_version, item));
            }

            //=== Stored Map actions

            foreach (string item in Economy.stored_map_actions.Where(x => x != null && x != "null"))
            {
                returnSB.AppendLine(SiiNitems[item].PrintOut(_version, item));
            }

            //=== AI Drivers offer

            List<string> AI_Drivers = new List<string>();

            AI_Drivers.AddRange(Economy.drivers_offer);
            AI_Drivers.AddRange(Economy.driver_pool);

            foreach (string item in AI_Drivers.Where(x => x != null && x != "null"))
            {
                Driver_AI Driver_AI = SiiNitems[item];

                returnSB.AppendLine(Driver_AI.PrintOut(_version, item));

                //

                string jobNameless = Driver_AI.driver_job;

                returnSB.AppendLine(SiiNitems[jobNameless].PrintOut(_version, jobNameless));

                //

                string logNameless = Driver_AI.profit_log;

                Profit_log Profit_log = SiiNitems[logNameless];

                returnSB.AppendLine(Profit_log.PrintOut(_version, logNameless));

                foreach (string statNameless in Profit_log.stats_data.Where(x => x != null && x != "null"))
                {
                    returnSB.AppendLine(SiiNitems[statNameless].PrintOut(_version, statNameless));
                }
            }

            //=== Registry

            returnSB.AppendLine(SiiNitems[Economy.registry].PrintOut(_version, Economy.registry));

            //=== Bus stops

            foreach (string item in Economy.bus_stops.Where(x => x != null && x != "null"))
            {
                returnSB.AppendLine(SiiNitems[item].PrintOut(_version, item));
            }

            //=== Bus job Log

            Bus_job_Log Bus_job_Log = SiiNitems[Economy.bus_job_log];

            returnSB.AppendLine(Bus_job_Log.PrintOut(_version, Economy.bus_job_log));

            foreach (string item in Bus_job_Log.entries.Where(x => x != null && x != "null"))
            {
                returnSB.AppendLine(SiiNitems[item].PrintOut(_version, item));
            }

            //=== Unidentified blocks

            foreach (string item in UnidentifiedBlocks)
            {
                returnSB.AppendLine(SiiNitems[item].PrintOut(_version, item));
            }

            //=== Print skipped blocks

            if (NamelessControlList.Count > 0)
            {
                List<string> skippedBlocks = new List<string>();
                skippedBlocks.AddRange(NamelessControlList);

                foreach (string item in skippedBlocks.Where(x => x != null && x != "null"))
                {
                    returnSB.AppendLine(SiiNitems[item].PrintOut(_version, item));
                }
            }
            
            returnSB.Append("}");

            returnString = returnSB.ToString();

            return returnString;
        }

    }
}