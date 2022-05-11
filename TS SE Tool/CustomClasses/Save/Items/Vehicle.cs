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
    class Vehicle : SiiNBlockCore
    {
        #region variables
        internal SCS_Float      engine_wear { get; set; } = 0;

        internal SCS_Float      transmission_wear { get; set; } = 0;

        internal SCS_Float      cabin_wear { get; set; } = 0;

        internal SCS_Float      chassis_wear { get; set; } = 0;

        internal List<SCS_Float> wheels_wear { get; set; } = new List<SCS_Float>();

        internal List<string> accessories { get; set; } = new List<string>();

        internal SCS_String license_plate { get; set; } = "";

        internal SCS_Float fuel_relative { get; set; } = 1;

        internal uint odometer { get; set; } = 0;
        internal SCS_Float odometer_float_part { get; set; } = 0;

        internal SCS_Float rheostat_factor { get; set; } = 0;

        internal List<SCS_Quaternion> user_mirror_rot { get; set; } = new List<SCS_Quaternion>();

        internal SCS_Float_3 user_head_offset { get; set; } = new SCS_Float_3();

        internal SCS_Float user_fov { get; set; } = 0;

        internal SCS_Float user_wheel_up_down { get; set; } = 0;

        internal SCS_Float user_wheel_front_back { get; set; } = 0;

        internal SCS_Float user_mouse_left_right_default { get; set; } = 0;
        
        internal SCS_Float user_mouse_up_down_default { get; set; } = 0;
        
        internal uint trip_fuel_l { get; set; } = 0;

        internal SCS_Float trip_fuel { get; set; } = 0;

        internal uint trip_distance_km { get; set; } = 0;

        internal SCS_Float trip_distance { get; set; } = 0;

        internal uint trip_time_min { get; set; } = 0;

        internal SCS_Float trip_time { get; set; } = 0;

        #endregion
        internal Vehicle()
        {

        }

        internal Vehicle(string[] _input)
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

                        case "engine_wear":
                            {
                                engine_wear = dataLine;
                                break;
                            }

                        case "transmission_wear":
                            {
                                transmission_wear = dataLine;
                                break;
                            }

                        case "cabin_wear":
                            {
                                cabin_wear = dataLine;
                                break;
                            }

                        case "chassis_wear":
                            {
                                chassis_wear = dataLine;
                                break;
                            }

                        case "wheels_wear":
                            {
                                wheels_wear.Capacity = int.Parse(dataLine);
                                break;
                            }

                        case var s when s.StartsWith("wheels_wear["):
                            {
                                wheels_wear.Add(dataLine);
                                break;
                            }

                        case "fuel_relative":
                            {
                                fuel_relative = dataLine;
                                break;
                            }

                        case "license_plate":
                            {
                                license_plate = dataLine;
                                break;
                            }

                        case "accessories":
                            {
                                accessories.Capacity = int.Parse(dataLine);
                                break;
                            }

                        case var s when s.StartsWith("accessories["):
                            {
                                accessories.Add(dataLine);
                                break;
                            }

                        case "user_mirror_rot":
                            {
                                user_mirror_rot.Capacity = int.Parse(dataLine);
                                break;
                            }

                        case var s when s.StartsWith("user_mirror_rot["):
                            {
                                user_mirror_rot.Add(new SCS_Quaternion(dataLine));
                                break;
                            }

                        case "user_head_offset":
                            {
                                user_head_offset = new SCS_Float_3(dataLine);
                                break;
                            }

                        case "user_fov":
                            {
                                user_fov = dataLine;
                                break;
                            }

                        case "user_wheel_up_down":
                            {
                                user_wheel_up_down = dataLine;
                                break;
                            }

                        case "user_wheel_front_back":
                            {
                                user_wheel_front_back = dataLine;
                                break;
                            }

                        case "user_mouse_left_right_default":
                            {
                                user_mouse_left_right_default = dataLine;
                                break;
                            }

                        case "user_mouse_up_down_default":
                            {
                                user_mouse_up_down_default = dataLine;
                                break;
                            }

                        case "rheostat_factor":
                            {
                                rheostat_factor = dataLine;
                                break;
                            }

                        case "odometer":
                            {
                                odometer = uint.Parse(dataLine);
                                break;
                            }

                        case "odometer_float_part":
                            {
                                odometer_float_part = dataLine;
                                break;
                            }

                        case "trip_fuel_l":
                            {
                                trip_fuel_l = uint.Parse(dataLine);
                                break;
                            }

                        case "trip_fuel":
                            {
                                trip_fuel = dataLine;
                                break;
                            }

                        case "trip_distance_km":
                            {
                                trip_distance_km = uint.Parse(dataLine);
                                break;
                            }

                        case "trip_distance":
                            {
                                trip_distance = dataLine;
                                break;
                            }

                        case "trip_time_min":
                            {
                                trip_time_min = uint.Parse(dataLine);
                                break;
                            }

                        case "trip_time":
                            {
                                trip_time = dataLine;
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

        internal string PrintOut(uint _version, string _nameless)
        {
            string returnString = "";

            StringBuilder returnSB = new StringBuilder();

            returnSB.AppendLine("vehicle : " + _nameless + " {");

            returnSB.AppendLine(" engine_wear: " + engine_wear.ToString());
            returnSB.AppendLine(" transmission_wear: " + transmission_wear.ToString());
            returnSB.AppendLine(" cabin_wear: " + cabin_wear.ToString());

            returnSB.AppendLine(" fuel_relative: " + fuel_relative.ToString());

            returnSB.AppendLine(" rheostat_factor: " + rheostat_factor.ToString());

            returnSB.AppendLine(" user_mirror_rot: " + user_mirror_rot.Count);
            for (int i = 0; i < user_mirror_rot.Count; i++)
                returnSB.AppendLine(" user_mirror_rot[" + i + "]: " + user_mirror_rot[i].ToString());

            returnSB.AppendLine(" user_head_offset: " + user_head_offset.ToString());
            returnSB.AppendLine(" user_fov: " + user_fov.ToString());

            returnSB.AppendLine(" user_wheel_up_down: " + user_wheel_up_down.ToString());
            returnSB.AppendLine(" user_wheel_front_back: " + user_wheel_front_back.ToString());
            returnSB.AppendLine(" user_mouse_left_right_default: " + user_mouse_left_right_default.ToString());
            returnSB.AppendLine(" user_mouse_up_down_default: " + user_mouse_up_down_default.ToString());

            returnSB.AppendLine(" accessories: " + accessories.Count);
            for (int i = 0; i < accessories.Count; i++)
                returnSB.AppendLine(" accessories[" + i + "]: " + accessories[i]);            

            returnSB.AppendLine(" odometer: " + odometer);
            returnSB.AppendLine(" odometer_float_part: " + odometer_float_part.ToString());
                       
            returnSB.AppendLine(" trip_fuel_l: " + trip_fuel_l);
            returnSB.AppendLine(" trip_fuel: " + trip_fuel.ToString());
            returnSB.AppendLine(" trip_distance_km: " + trip_distance_km);
            returnSB.AppendLine(" trip_distance: " + trip_distance.ToString());
            returnSB.AppendLine(" trip_time_min: " + trip_time_min);
            returnSB.AppendLine(" trip_time: " + trip_time.ToString());

            returnSB.AppendLine(" license_plate: " + license_plate.ToString());

            returnSB.AppendLine(" chassis_wear: " + chassis_wear.ToString());

            returnSB.AppendLine(" wheels_wear: " + wheels_wear.Count);
            for (int i = 0; i < wheels_wear.Count; i++)
                returnSB.AppendLine(" wheels_wear[" + i + "]: " + wheels_wear[i].ToString());


            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            this.removeWritenBlock(_nameless);

            return returnString;
        }
    }

    
}
