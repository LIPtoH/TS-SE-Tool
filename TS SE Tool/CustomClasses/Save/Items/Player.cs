using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TS_SE_Tool.Utilities;
using TS_SE_Tool.Save.DataFormat;
using TS_SE_Tool.CustomClasses.Global;

namespace TS_SE_Tool.Save.Items
{
    class Player : SiiNBlockCore
    {
        #region variables
        internal string hq_city { get; set; } = "";

        internal List<string> trailers { get; set; } = new List<string>();

        internal List<string> trailer_utilization_logs { get; set; } = new List<string>();

        internal List<string> trailer_defs { get; set; } = new List<string>();

        internal string assigned_truck { get; set; } = "";

        internal string my_truck { get; set; } = "";

        internal SCS_Placement my_truck_placement { get; set; } = new SCS_Placement();

        internal bool my_truck_placement_valid { get; set; } = false;

        internal SCS_Placement my_trailer_placement { get; set; } = new SCS_Placement();

        internal SCS_Float my_slave_trailer_placements { get; set; } = 0;

        internal bool my_trailer_attached { get; set; } = false;

        internal bool my_trailer_used { get; set; } = false;

        internal string assigned_trailer { get; set; } = "";

        internal string my_trailer { get; set; } = "";

        internal bool assigned_trailer_connected { get; set; } = false;

        internal SCS_Placement truck_placement { get; set; } = new SCS_Placement();

        internal SCS_Placement trailer_placement { get; set; } = new SCS_Placement();

        internal SCS_Float slave_trailer_placements { get; set; } = 0;

        internal bool schedule_transfer_to_hq { get; set; } = false;

        internal int flags { get; set; } = 0;

        internal int gas_pump_money_debt { get; set; } = 0;

        internal string current_job { get; set; } = "";

        internal string current_bus_job { get; set; } = "";

        internal string selected_job { get; set; } = "";

        internal int driving_time { get; set; } = 0;

        internal int sleeping_count { get; set; } = 0;

        internal int free_roam_distance { get; set; } = 0;

        internal SCS_Float discovary_distance { get; set; } = 0;

        internal List<string> dismissed_drivers { get; set; } = new List<string>();

        internal List<string> trucks { get; set; } = new List<string>();

        internal List<string> truck_profit_logs { get; set; } = new List<string>();

        internal List<string> drivers { get; set; } = new List<string>();

        internal List<int> driver_readiness_timer { get; set; } = new List<int>();

        internal List<bool> driver_quit_warned { get; set; } = new List<bool>();

        #endregion
        internal Player()
        { }

        internal Player(string[] _input)
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
                            {
                                break;
                            }

                        case "hq_city":
                            {
                                hq_city = dataLine;
                                break;
                            }

                        case "trailers":
                            {
                                trailers.Capacity = int.Parse(dataLine);
                                break;
                            }

                        case var s when s.StartsWith("trailers["):
                            {
                                trailers.Add(dataLine);
                                break;
                            }

                        case "trailer_utilization_logs":
                            {
                                trailer_utilization_logs.Capacity = int.Parse(dataLine);
                                break;
                            }

                        case var s when s.StartsWith("trailer_utilization_logs["):
                            {
                                trailer_utilization_logs.Add(dataLine);
                                break;
                            }

                        case "trailer_defs":
                            {
                                trailer_defs.Capacity = int.Parse(dataLine);
                                break;
                            }

                        case var s when s.StartsWith("trailer_defs["):
                            {
                                trailer_defs.Add(dataLine);
                                break;
                            }

                        case "assigned_truck":
                            {
                                assigned_truck = dataLine;
                                break;
                            }

                        case "my_truck":
                            {
                                my_truck = dataLine;
                                break;
                            }

                        case "my_truck_placement":
                            {
                                my_truck_placement = new SCS_Placement(dataLine);
                                break;
                            }

                        case "my_truck_placement_valid":
                            {
                                my_truck_placement_valid = bool.Parse(dataLine);
                                break;
                            }

                        case "my_trailer_placement":
                            {
                                my_trailer_placement = new SCS_Placement(dataLine);
                                break;
                            }

                        case "my_slave_trailer_placements":
                            {
                                my_slave_trailer_placements = dataLine;
                                break;
                            }

                        case "my_trailer_attached":
                            {
                                my_trailer_attached = bool.Parse(dataLine);
                                break;
                            }

                        case "my_trailer_used":
                            {
                                my_trailer_used = bool.Parse(dataLine);
                                break;
                            }

                        case "assigned_trailer":
                            {
                                assigned_trailer = dataLine;
                                break;
                            }

                        case "my_trailer":
                            {
                                my_trailer = dataLine;
                                break;
                            }

                        case "assigned_trailer_connected":
                            {
                                assigned_trailer_connected = bool.Parse(dataLine);
                                break;
                            }

                        case "truck_placement":
                            {
                                truck_placement = new SCS_Placement(dataLine);
                                break;
                            }

                        case "trailer_placement":
                            {
                                trailer_placement = new SCS_Placement(dataLine);
                                break;
                            }

                        case "slave_trailer_placements":
                            {
                                slave_trailer_placements = dataLine;
                                break;
                            }

                        case "schedule_transfer_to_hq":
                            {
                                schedule_transfer_to_hq = bool.Parse(dataLine);
                                break;
                            }

                        case "flags":
                            {
                                flags = int.Parse(dataLine);
                                break;
                            }

                        case "gas_pump_money_debt":
                            {
                                gas_pump_money_debt = int.Parse(dataLine);
                                break;
                            }

                        case "current_job":
                            {
                                current_job = dataLine;
                                break;
                            }

                        case "current_bus_job":
                            {
                                current_bus_job = dataLine;
                                break;
                            }

                        case "selected_job":
                            {
                                selected_job = dataLine;
                                break;
                            }

                        case "driving_time":
                            {
                                driving_time = int.Parse(dataLine);
                                break;
                            }

                        case "sleeping_count":
                            {
                                sleeping_count = int.Parse(dataLine);
                                break;
                            }

                        case "free_roam_distance":
                            {
                                free_roam_distance = int.Parse(dataLine);
                                break;
                            }

                        case "discovary_distance":
                            {
                                discovary_distance = dataLine;
                                break;
                            }

                        case "dismissed_drivers":
                            {
                                dismissed_drivers.Capacity = int.Parse(dataLine);
                                break;
                            }

                        case var s when s.StartsWith("dismissed_drivers["):
                            {
                                dismissed_drivers.Add(dataLine);
                                break;
                            }

                        case "trucks":
                            {
                                trucks.Capacity = int.Parse(dataLine);
                                break;
                            }

                        case var s when s.StartsWith("trucks["):
                            {
                                trucks.Add(dataLine);
                                break;
                            }

                        case "truck_profit_logs":
                            {
                                truck_profit_logs.Capacity = int.Parse(dataLine);
                                break;
                            }

                        case var s when s.StartsWith("truck_profit_logs["):
                            {
                                truck_profit_logs.Add(dataLine);
                                break;
                            }

                        case "drivers":
                            {
                                drivers.Capacity = int.Parse(dataLine);
                                break;
                            }

                        case var s when s.StartsWith("drivers["):
                            {
                                drivers.Add(dataLine);
                                break;
                            }

                        case "driver_readiness_timer":
                            {
                                driver_readiness_timer.Capacity = int.Parse(dataLine);
                                break;
                            }

                        case var s when s.StartsWith("driver_readiness_timer["):
                            {
                                driver_readiness_timer.Add(int.Parse(dataLine));
                                break;
                            }

                        case "driver_quit_warned":
                            {
                                driver_quit_warned.Capacity = int.Parse(dataLine);
                                break;
                            }

                        case var s when s.StartsWith("driver_quit_warned["):
                            {
                                driver_quit_warned.Add(bool.Parse(dataLine));
                                break;
                            }
                    }
                }
                catch (Exception ex)
                {
                    Utilities.IO_Utilities.ErrorLogWriter(ex.Message + Environment.NewLine + this.GetType().Name.ToLower() + " | " + tagLine + " = " + dataLine);
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

            returnSB.AppendLine("player : " + _nameless + " {");

            returnSB.AppendLine(" hq_city: " + hq_city);

            returnSB.AppendLine(" trailers: " + trailers.Count);
            for (int i = 0; i < trailers.Count; i++)
                returnSB.AppendLine(" trailers[" + i + "]: " + trailers[i]);

            returnSB.AppendLine(" trailer_utilization_logs: " + trailer_utilization_logs.Count);
            for (int i = 0; i < trailer_utilization_logs.Count; i++)
                returnSB.AppendLine(" trailer_utilization_logs[" + i + "]: " + trailer_utilization_logs[i]);

            returnSB.AppendLine(" trailer_defs: " + trailer_defs.Count);
            for (int i = 0; i < trailer_defs.Count; i++)
                returnSB.AppendLine(" trailer_defs[" + i + "]: " + trailer_defs[i]);

            returnSB.AppendLine(" assigned_truck: " + assigned_truck);
            returnSB.AppendLine(" my_truck: " + my_truck);
            returnSB.AppendLine(" my_truck_placement: " + my_truck_placement.ToString());
            returnSB.AppendLine(" my_truck_placement_valid: " + my_truck_placement_valid.ToString().ToLower());
            returnSB.AppendLine(" my_trailer_placement: " + my_trailer_placement.ToString());
            returnSB.AppendLine(" my_slave_trailer_placements: " + my_slave_trailer_placements.ToString());
            returnSB.AppendLine(" my_trailer_attached: " + my_trailer_attached.ToString().ToLower());
            returnSB.AppendLine(" my_trailer_used: " + my_trailer_used.ToString().ToLower());
            returnSB.AppendLine(" assigned_trailer: " + assigned_trailer);
            returnSB.AppendLine(" my_trailer: " + my_trailer);
            returnSB.AppendLine(" assigned_trailer_connected: " + assigned_trailer_connected.ToString().ToLower());
            returnSB.AppendLine(" truck_placement: " + truck_placement.ToString());
            returnSB.AppendLine(" trailer_placement: " + trailer_placement.ToString());
            returnSB.AppendLine(" slave_trailer_placements: " + slave_trailer_placements.ToString());
            returnSB.AppendLine(" schedule_transfer_to_hq: " + schedule_transfer_to_hq.ToString().ToLower());
            returnSB.AppendLine(" flags: " + flags.ToString());
            returnSB.AppendLine(" gas_pump_money_debt: " + gas_pump_money_debt.ToString());
            returnSB.AppendLine(" current_job: " + current_job);
            returnSB.AppendLine(" current_bus_job: " + current_bus_job);
            returnSB.AppendLine(" selected_job: " + selected_job);
            returnSB.AppendLine(" driving_time: " + driving_time.ToString());
            returnSB.AppendLine(" sleeping_count: " + sleeping_count.ToString());
            returnSB.AppendLine(" free_roam_distance: " + free_roam_distance.ToString());
            returnSB.AppendLine(" discovary_distance: " + discovary_distance.ToString());

            returnSB.AppendLine(" dismissed_drivers: " + dismissed_drivers.Count);
            for (int i = 0; i < dismissed_drivers.Count; i++)
                returnSB.AppendLine(" dismissed_drivers[" + i + "]: " + dismissed_drivers[i]);

            returnSB.AppendLine(" trucks: " + trucks.Count);
            for (int i = 0; i < trucks.Count; i++)
                returnSB.AppendLine(" trucks[" + i + "]: " + trucks[i]);

            returnSB.AppendLine(" truck_profit_logs: " + truck_profit_logs.Count);
            for (int i = 0; i < truck_profit_logs.Count; i++)
                returnSB.AppendLine(" truck_profit_logs[" + i + "]: " + truck_profit_logs[i]);

            returnSB.AppendLine(" drivers: " + drivers.Count);
            for (int i = 0; i < drivers.Count; i++)
                returnSB.AppendLine(" drivers[" + i + "]: " + drivers[i]);

            returnSB.AppendLine(" driver_readiness_timer: " + driver_readiness_timer.Count);
            for (int i = 0; i < driver_readiness_timer.Count; i++)
                returnSB.AppendLine(" driver_readiness_timer[" + i + "]: " + driver_readiness_timer[i].ToString());

            returnSB.AppendLine(" driver_quit_warned: " + driver_quit_warned.Count);
            for (int i = 0; i < driver_quit_warned.Count; i++)
                returnSB.AppendLine(" driver_quit_warned[" + i + "]: " + driver_quit_warned[i].ToString().ToLower());

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            this.removeWritenBlock(_nameless);

            return returnString;
        }
    }
}