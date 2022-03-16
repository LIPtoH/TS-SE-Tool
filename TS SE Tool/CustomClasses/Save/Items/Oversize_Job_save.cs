using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TS_SE_Tool.Save.DataFormat;

namespace TS_SE_Tool.Save.Items
{
    class Oversize_Job_save : SiiNBlockCore
    {
        internal Vector_3f front_escort_ws_position { get; set; } = new Vector_3f();
        internal Vector_3f back_escort_ws_position { get; set; } = new Vector_3f();

        
        internal UInt64? front_trajectory_uid { get; set; } = 0;

        internal UInt64? back_trajectory_uid { get; set; } = 0;

        
        internal SCS_Float front_trajectory_position { get; set; } = 0;

        internal SCS_Float back_trajectory_position { get; set; } = 0;

        
        internal Vector_4f front_escort_rotation { get; set; } = new Vector_4f();

        internal Vector_4f back_escort_rotation { get; set; } = new Vector_4f();

        
        internal SCS_Float front_escort_speed { get; set; } = 0;

        internal SCS_Float back_escort_speed { get; set; } = 0;

        
        internal bool spawn_escort_active { get; set; } = false;
        internal List<string> trajectory_orders { get; set; } = new List<string>();
        
        internal string front_char_type { get; set; } = "";
        internal string back_char_type { get; set; } = "";
        
        internal int oversize_manager_state { get; set; } = 0;
        internal int? oversize_manager_current_kdop_idx { get; set; } = 0;

        internal Vector_3f oversize_manager_last_valid_pos { get; set; } = new Vector_3f();

        internal List<string> active_blocks_rules { get; set; } = new List<string>();

        internal int front_type_state { get; set; } = 0;
        internal int back_type_state { get; set; } = 0;

        internal uint front_vehicle_seed { get; set; } = 0;
        internal uint back_vehicle_seed { get; set; } = 0;

        internal List<int> map_route_hash { get; set; } = new List<int>();

        internal string offer { get; set; } = "";

        internal Oversize_Job_save()
        { }

        internal Oversize_Job_save(string[] _input)
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

                        case "front_escort_ws_position":
                            {
                                front_escort_ws_position = new Vector_3f(dataLine);
                                break;
                            }

                        case "back_escort_ws_position":
                            {
                                back_escort_ws_position = new Vector_3f(dataLine);
                                break;
                            }

                        case "front_trajectory_uid":
                            {
                                front_trajectory_uid = dataLine == "nil" ? (UInt64?)null : UInt64.Parse(dataLine);
                                break;
                            }

                        case "back_trajectory_uid":
                            {
                                back_trajectory_uid = dataLine == "nil" ? (UInt64?)null : UInt64.Parse(dataLine);
                                break;
                            }

                        case "front_trajectory_position":
                            {
                                front_trajectory_position = dataLine;
                                break;
                            }

                        case "back_trajectory_position":
                            {
                                back_trajectory_position = dataLine;
                                break;
                            }

                        case "front_escort_rotation":
                            {
                                front_escort_rotation = new Vector_4f(dataLine);
                                break;
                            }

                        case "back_escort_rotation":
                            {
                                back_escort_rotation = new Vector_4f(dataLine);
                                break;
                            }

                        case "front_escort_speed":
                            {
                                front_escort_speed = dataLine;
                                break;
                            }

                        case "back_escort_speed":
                            {
                                back_escort_speed = dataLine;
                                break;
                            }

                        case "spawn_escort_active":
                            {
                                spawn_escort_active = bool.Parse(dataLine);
                                break;
                            }

                        case "trajectory_orders":
                            {
                                trajectory_orders.Capacity = int.Parse(dataLine);
                                break;
                            }

                        case var s when s.StartsWith("trajectory_orders["):
                            {
                                trajectory_orders.Add(dataLine);
                                break;
                            }

                        case "front_char_type":
                            {
                                front_char_type = dataLine;
                                break;
                            }

                        case "back_char_type":
                            {
                                back_char_type = dataLine;
                                break;
                            }

                        case "oversize_manager_state":
                            {
                                oversize_manager_state = int.Parse(dataLine);
                                break;
                            }

                        case "oversize_manager_current_kdop_idx":
                            {
                                oversize_manager_current_kdop_idx = dataLine == "nil" ? (int?)null : int.Parse(dataLine);
                                break;
                            }

                        case "oversize_manager_last_valid_pos":
                            {
                                oversize_manager_last_valid_pos = new Vector_3f(dataLine);
                                break;
                            }

                        case "active_blocks_rules":
                            {
                                active_blocks_rules.Capacity = int.Parse(dataLine);
                                break;
                            }

                        case var s when s.StartsWith("active_blocks_rules["):
                            {
                                active_blocks_rules.Add(dataLine);
                                break;
                            }

                        case "front_type_state":
                            {
                                front_type_state = int.Parse(dataLine);
                                break;
                            }

                        case "back_type_state":
                            {
                                back_type_state = int.Parse(dataLine);
                                break;
                            }

                        case "front_vehicle_seed":
                            {
                                front_vehicle_seed = uint.Parse(dataLine);
                                break;
                            }

                        case "back_vehicle_seed":
                            {
                                back_vehicle_seed = uint.Parse(dataLine);
                                break;
                            }

                        case "map_route_hash":
                            {
                                map_route_hash.Capacity = int.Parse(dataLine);
                                break;
                            }

                        case var s when s.StartsWith("map_route_hash["):
                            {
                                map_route_hash.Add(int.Parse(dataLine));
                                break;
                            }

                        case "offer":
                            {
                                offer = dataLine;
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

            returnSB.AppendLine("oversize_job_save : " + _nameless + " {");

            returnSB.AppendLine(" front_escort_ws_position: " + front_escort_ws_position.ToString());
            returnSB.AppendLine(" back_escort_ws_position: " + back_escort_ws_position.ToString());

            returnSB.AppendLine(" front_trajectory_uid: " + (front_trajectory_uid == null ? "nil" : front_trajectory_uid.ToString()));
            returnSB.AppendLine(" back_trajectory_uid: " + (back_trajectory_uid == null ? "nil" : back_trajectory_uid.ToString()));

            returnSB.AppendLine(" front_trajectory_position: " + front_trajectory_position.ToString());
            returnSB.AppendLine(" back_trajectory_position: " + back_trajectory_position.ToString());

            returnSB.AppendLine(" front_escort_rotation: " + front_escort_rotation.ToString());
            returnSB.AppendLine(" back_escort_rotation: " + back_escort_rotation.ToString());

            returnSB.AppendLine(" front_escort_speed: " + front_escort_speed.ToString());
            returnSB.AppendLine(" back_escort_speed: " + back_escort_speed.ToString());

            returnSB.AppendLine(" spawn_escort_active: " + spawn_escort_active.ToString().ToLower());

            returnSB.AppendLine(" trajectory_orders: " + trajectory_orders.Count);
            for (int i = 0; i < trajectory_orders.Count; i++)
                returnSB.AppendLine(" trajectory_orders[" + i + "]: " + trajectory_orders[i]);

            returnSB.AppendLine(" front_char_type: " + front_char_type);
            returnSB.AppendLine(" back_char_type: " + back_char_type);

            returnSB.AppendLine(" oversize_manager_state: " + oversize_manager_state.ToString());
            returnSB.AppendLine(" oversize_manager_current_kdop_idx: " + (oversize_manager_current_kdop_idx == null ? "nil" : oversize_manager_current_kdop_idx.ToString()));

            returnSB.AppendLine(" oversize_manager_last_valid_pos: " + oversize_manager_last_valid_pos.ToString());

            returnSB.AppendLine(" active_blocks_rules: " + active_blocks_rules.Count);
            for (int i = 0; i < active_blocks_rules.Count; i++)
                returnSB.AppendLine(" active_blocks_rules[" + i + "]: " + active_blocks_rules[i]);

            returnSB.AppendLine(" front_type_state: " + front_type_state.ToString());
            returnSB.AppendLine(" back_type_state: " + back_type_state.ToString());

            returnSB.AppendLine(" front_vehicle_seed: " + front_vehicle_seed.ToString());
            returnSB.AppendLine(" back_vehicle_seed: " + back_vehicle_seed.ToString());

            returnSB.AppendLine(" map_route_hash: " + map_route_hash.Count);
            for (int i = 0; i < map_route_hash.Count; i++)
                returnSB.AppendLine(" map_route_hash[" + i + "]: " + map_route_hash[i].ToString());

            returnSB.AppendLine(" offer: " + offer.ToString());

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            this.removeWritenBlock(_nameless);

            return returnString;
        }
    }
}
