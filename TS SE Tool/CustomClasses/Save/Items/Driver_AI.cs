using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TS_SE_Tool.Save.DataFormat;

namespace TS_SE_Tool.Save.Items
{
    class Driver_AI : SiiNBlockCore
    {
        #region variables
        internal byte adr { get; set; } = 0;
        internal byte long_dist { get; set; } = 0;
        internal byte heavy { get; set; } = 0;
        internal byte fragile { get; set; } = 0;
        internal byte urgent { get; set; } = 0;
        internal byte mechanical { get; set; } = 0;

        internal SCS_String hometown { get; set; } = "";
        internal SCS_String current_city { get; set; } = "";

        internal uint state { get; set; } = 0;
        internal int on_duty_timer { get; set; } = 0;
        internal int extra_maintenance { get; set; } = 0;

        internal string driver_job { get; set; } = "";

        internal int experience_points { get; set; } = 0;
        internal int training_policy { get; set; } = 0;

        internal string adopted_truck { get; set; } = "";
        internal string assigned_truck { get; set; } = "";

        internal SCS_Float assigned_truck_efficiency { get; set; } = 0;
        internal int assigned_truck_axle_count { get; set; } = 0;
        internal SCS_Float assigned_truck_mass { get; set; } = 0;

        internal SCS_Float slot_truck_efficiency { get; set; } = 0;
        internal int slot_truck_axle_count { get; set; } = 0;
        internal SCS_Float slot_truck_mass { get; set; } = 0;

        internal string adopted_trailer { get; set; } = "";
        internal string assigned_trailer { get; set; } = "";

        internal SCS_String old_hometown { get; set; } = "";

        internal string profit_log { get; set; } = "";

        #endregion

        internal Driver_AI()
        { }

        internal Driver_AI(string[] _input)
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

                        case "adr":
                            {
                                adr = byte.Parse(dataLine);
                                break;
                            }

                        case "long_dist":
                            {
                                long_dist = byte.Parse(dataLine);
                                break;
                            }

                        case "heavy":
                            {
                                heavy = byte.Parse(dataLine);
                                break;
                            }

                        case "fragile":
                            {
                                fragile = byte.Parse(dataLine);
                                break;
                            }

                        case "urgent":
                            {
                                urgent = byte.Parse(dataLine);
                                break;
                            }

                        case "mechanical":
                            {
                                mechanical = byte.Parse(dataLine);
                                break;
                            }

                        case "hometown":
                            {
                                hometown = dataLine;
                                break;
                            }

                        case "current_city":
                            {
                                current_city = dataLine;
                                break;
                            }

                        case "state":
                            {
                                state = uint.Parse(dataLine);
                                break;
                            }

                        case "on_duty_timer":
                            {
                                on_duty_timer = int.Parse(dataLine);
                                break;
                            }

                        case "extra_maintenance":
                            {
                                extra_maintenance = int.Parse(dataLine);
                                break;
                            }

                        case "driver_job":
                            {
                                driver_job = dataLine;
                                break;
                            }

                        case "experience_points":
                            {
                                experience_points = int.Parse(dataLine);
                                break;
                            }

                        case "training_policy":
                            {
                                training_policy = int.Parse(dataLine);
                                break;
                            }

                        case "adopted_truck":
                            {
                                adopted_truck = dataLine;
                                break;
                            }

                        case "assigned_truck":
                            {
                                assigned_truck = dataLine;
                                break;
                            }

                        case "assigned_truck_efficiency":
                            {
                                assigned_truck_efficiency = dataLine;
                                break;
                            }

                        case "assigned_truck_axle_count":
                            {
                                assigned_truck_axle_count = int.Parse(dataLine);
                                break;
                            }

                        case "assigned_truck_mass":
                            {
                                assigned_truck_mass = dataLine;
                                break;
                            }

                        case "slot_truck_efficiency":
                            {
                                slot_truck_efficiency = dataLine;
                                break;
                            }

                        case "slot_truck_axle_count":
                            {
                                slot_truck_axle_count = int.Parse(dataLine);
                                break;
                            }

                        case "slot_truck_mass":
                            {
                                slot_truck_mass = dataLine;
                                break;
                            }

                        case "adopted_trailer":
                            {
                                adopted_trailer = dataLine;
                                break;
                            }

                        case "assigned_trailer":
                            {
                                assigned_trailer = dataLine;
                                break;
                            }

                        case "old_hometown":
                            {
                                old_hometown = dataLine;
                                break;
                            }

                        case "profit_log":
                            {
                                profit_log = dataLine;
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

            returnSB.AppendLine("driver_ai : " + _nameless + " {");

            returnSB.AppendLine(" adr: " + adr.ToString());
            returnSB.AppendLine(" long_dist: " + long_dist.ToString());
            returnSB.AppendLine(" heavy: " + heavy.ToString());
            returnSB.AppendLine(" fragile: " + fragile.ToString());
            returnSB.AppendLine(" urgent: " + urgent.ToString());
            returnSB.AppendLine(" mechanical: " + mechanical.ToString());

            returnSB.AppendLine(" hometown: " + hometown.ToString());
            returnSB.AppendLine(" current_city: " + current_city.ToString());

            returnSB.AppendLine(" state: " + state.ToString());
            returnSB.AppendLine(" on_duty_timer: " + on_duty_timer.ToString());
            returnSB.AppendLine(" extra_maintenance: " + extra_maintenance.ToString());

            returnSB.AppendLine(" driver_job: " + driver_job);

            returnSB.AppendLine(" experience_points: " + experience_points.ToString());
            returnSB.AppendLine(" training_policy: " + training_policy.ToString());

            returnSB.AppendLine(" adopted_truck: " + adopted_truck);
            returnSB.AppendLine(" assigned_truck: " + assigned_truck);

            returnSB.AppendLine(" assigned_truck_efficiency: " + assigned_truck_efficiency.ToString());
            returnSB.AppendLine(" assigned_truck_axle_count: " + assigned_truck_axle_count.ToString());
            returnSB.AppendLine(" assigned_truck_mass: " + assigned_truck_mass.ToString());

            returnSB.AppendLine(" slot_truck_efficiency: " + slot_truck_efficiency.ToString());
            returnSB.AppendLine(" slot_truck_axle_count: " + slot_truck_axle_count.ToString());
            returnSB.AppendLine(" slot_truck_mass: " + slot_truck_mass.ToString());

            returnSB.AppendLine(" adopted_trailer: " + adopted_trailer);
            returnSB.AppendLine(" assigned_trailer: " + assigned_trailer);

            returnSB.AppendLine(" old_hometown: " + old_hometown.ToString());
            returnSB.AppendLine(" profit_log: " + profit_log);

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            this.removeWritenBlock(_nameless);

            return returnString;
        }
    
        internal void SetForDriverPool()
        {
            hometown = "";
            current_city = "";

            state = 1;

            on_duty_timer = 0;
            extra_maintenance = 0;

            training_policy = 0;

            adopted_truck = "null";
            assigned_truck = "null";

            assigned_truck_efficiency = 1;
            assigned_truck_axle_count = 2;
            assigned_truck_mass = 10000;

            slot_truck_efficiency = 1;
            slot_truck_axle_count = 2;
            slot_truck_mass = 10000;

            adopted_trailer = "null";
            assigned_truck = "null";

            old_hometown = "";
        }
    }
}