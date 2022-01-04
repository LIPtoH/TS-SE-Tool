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
    class Vehicle
    {
        internal float      engine_wear { get; set; } = 0;
        internal string     _engine_wear
        {
            get
            {
                return NumericUtilities.SingleFloatToHexFloat(engine_wear);
            }
            set
            {
                engine_wear = NumericUtilities.HexFloatToSingleFloat(value);
            }
        }

        internal float      transmission_wear { get; set; } = 0;
        internal string     _transmission_wear
        {
            get
            {
                return NumericUtilities.SingleFloatToHexFloat(transmission_wear);
            }
            set
            {
                transmission_wear = NumericUtilities.HexFloatToSingleFloat(value);
            }
        }

        internal float      cabin_wear { get; set; } = 0;
        internal string     _cabin_wear
        {
            get
            {
                return NumericUtilities.SingleFloatToHexFloat(cabin_wear);
            }
            set
            {
                cabin_wear = NumericUtilities.HexFloatToSingleFloat(value);
            }
        }

        internal float      chassis_wear { get; set; } = 0;
        internal string     _chassis_wear
        {
            get
            {
                return NumericUtilities.SingleFloatToHexFloat(chassis_wear);
            }
            set
            {
                chassis_wear = NumericUtilities.HexFloatToSingleFloat(value);
            }
        }

        internal FloatList  wheels_wear { get; set; } = new FloatList();

        internal List<string> accessories { get; set; } = new List<string>();

        internal string license_plate { get; set; } = "";
        internal string _license_plate
        {
            get
            {
                return TextUtilities.FromStringToOutputString(license_plate);
            }
            set
            {
                string data = value;

                if (value.StartsWith("\"") && value.EndsWith("\""))
                    data = value.Substring(1, value.Length - 2);

                license_plate = data;
            }
        }

        internal float fuel_relative { get; set; } = 1;
        internal string _fuel_relative
        {
            get
            {
                return NumericUtilities.SingleFloatToHexFloat(fuel_relative);
            }
            set
            {
                fuel_relative = NumericUtilities.HexFloatToSingleFloat(value);
            }
        }

        internal uint odometer { get; set; } = 0;
        internal float odometer_float_part { get; set; } = 0;
        internal string _odometer_float_part
        {
            get
            {
                return NumericUtilities.SingleFloatToHexFloat(odometer_float_part);
            }
            set
            {
                odometer_float_part = NumericUtilities.HexFloatToSingleFloat(value);
            }
        }

        internal float rheostat_factor { get; set; } = 0;
        internal string _rheostat_factor
        {
            get
            {
                return NumericUtilities.SingleFloatToHexFloat(rheostat_factor);
            }
            set
            {
                rheostat_factor = NumericUtilities.HexFloatToSingleFloat(value);
            }
        }

        internal List<Vector_4f> user_mirror_rot { get; set; } = new List<Vector_4f>();

        internal Vector_3f user_head_offset { get; set; } = new Vector_3f();

        internal float user_fov { get; set; } = 0;
        internal string _user_fov
        {
            get
            {
                return NumericUtilities.SingleFloatToHexFloat(user_fov);
            }
            set
            {
                user_fov = NumericUtilities.HexFloatToSingleFloat(value);
            }
        }

        internal float user_wheel_up_down { get; set; } = 0;
        internal string _user_wheel_up_down
        {
            get
            {
                return NumericUtilities.SingleFloatToHexFloat(user_wheel_up_down);
            }
            set
            {
                user_wheel_up_down = NumericUtilities.HexFloatToSingleFloat(value);
            }
        }

        internal float user_wheel_front_back { get; set; } = 0;
        internal string _user_wheel_front_back
        {
            get
            {
                return NumericUtilities.SingleFloatToHexFloat(user_wheel_front_back);
            }
            set
            {
                user_wheel_front_back = NumericUtilities.HexFloatToSingleFloat(value);
            }
        }

        internal float user_mouse_left_right_default { get; set; } = 0;
        internal string _user_mouse_left_right_default
        {
            get
            {
                return NumericUtilities.SingleFloatToHexFloat(user_mouse_left_right_default);
            }
            set
            {
                user_mouse_left_right_default = NumericUtilities.HexFloatToSingleFloat(value);
            }
        }
        
        internal float user_mouse_up_down_default { get; set; } = 0;
        internal string _user_mouse_up_down_default
        {
            get
            {
                return NumericUtilities.SingleFloatToHexFloat(user_mouse_up_down_default);
            }
            set
            {
                user_mouse_up_down_default = NumericUtilities.HexFloatToSingleFloat(value);
            }
        }
        
        internal uint trip_fuel_l { get; set; } = 0;

        internal float trip_fuel { get; set; } = 0;
        internal string _trip_fuel
        {
            get
            {
                return NumericUtilities.SingleFloatToHexFloat(trip_fuel);
            }
            set
            {
                trip_fuel = NumericUtilities.HexFloatToSingleFloat(value);
            }
        }

        internal uint trip_distance_km { get; set; } = 0;

        internal float trip_distance { get; set; } = 0;
        internal string _trip_distance
        {
            get
            {
                return NumericUtilities.SingleFloatToHexFloat(trip_distance);
            }
            set
            {
                trip_distance = NumericUtilities.HexFloatToSingleFloat(value);
            }
        }

        internal uint trip_time_min { get; set; } = 0;

        internal float trip_time { get; set; } = 0;
        internal string _trip_time
        {
            get
            {
                return NumericUtilities.SingleFloatToHexFloat(trip_time);
            }
            set
            {
                trip_time = NumericUtilities.HexFloatToSingleFloat(value);
            }
        }

        internal Vehicle()
        {

        }

        internal Vehicle(string[] _input)
        {
            string[] lineParts;
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

                switch (tagLine)
                {
                    case "":
                        {
                            break;
                        }

                    case "engine_wear":
                        {
                            _engine_wear = dataLine;
                            break;
                        }

                    case "transmission_wear":
                        {
                            _transmission_wear = dataLine;
                            break;
                        }

                    case "cabin_wear":
                        {
                            _cabin_wear = dataLine;
                            break;
                        }

                    case "chassis_wear":
                        {
                            _chassis_wear = dataLine;
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
                            _fuel_relative = dataLine;
                            break;
                        }

                    case "license_plate":
                        {
                            _license_plate = dataLine;
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
                            user_mirror_rot.Add(new Vector_4f(dataLine));
                            break;
                        }                        

                    case "user_head_offset":
                        {
                            user_head_offset.ToVector(dataLine);
                            break;
                        }

                    case "user_fov":
                        {
                            _user_fov = dataLine;
                            break;
                        }

                    case "user_wheel_up_down":
                        {
                            _user_wheel_up_down = dataLine;
                            break;
                        }

                    case "user_wheel_front_back":
                        {
                            _user_wheel_front_back = dataLine;
                            break;
                        }

                    case "user_mouse_left_right_default":
                        {
                            _user_mouse_left_right_default = dataLine;
                            break;
                        }

                    case "user_mouse_up_down_default":
                        {
                            _user_mouse_up_down_default = dataLine;
                            break;
                        }

                    case "rheostat_factor":
                        {
                            _rheostat_factor = dataLine;
                            break;
                        }

                    case "odometer":
                        {
                            odometer = uint.Parse(dataLine);
                            break;
                        }

                    case "odometer_float_part":
                        {
                            _odometer_float_part = dataLine;
                            break;
                        }

                    case "trip_fuel_l":
                        {
                            trip_fuel_l = uint.Parse(dataLine);
                            break;
                        }

                    case "trip_fuel":
                        {
                            _trip_fuel = dataLine;
                            break;
                        }

                    case "trip_distance_km":
                        {
                            trip_distance_km = uint.Parse(dataLine);
                            break;
                        }

                    case "trip_distance":
                        {
                            _trip_distance = dataLine;
                            break;
                        }

                    case "trip_time_min":
                        {
                            trip_time_min = uint.Parse(dataLine);
                            break;
                        }

                    case "trip_time":
                        {
                            _trip_time = dataLine;
                            break;
                        }

                }
            }
        }

        internal string PrintOut(uint _version, string _nameless)
        {
            string returnString = "";

            StringBuilder returnSB = new StringBuilder();

            returnSB.AppendLine("vehicle : " + _nameless + " {");

            returnSB.AppendLine(" engine_wear: " + _engine_wear);
            returnSB.AppendLine(" transmission_wear: " + _transmission_wear);
            returnSB.AppendLine(" cabin_wear: " + _cabin_wear);

            returnSB.AppendLine(" fuel_relative: " + _fuel_relative);

            returnSB.AppendLine(" rheostat_factor: " + _rheostat_factor);

            returnSB.AppendLine(" user_mirror_rot: " + user_mirror_rot.Count);
            for (int i = 0; i < user_mirror_rot.Count; i++)
                returnSB.AppendLine(" user_mirror_rot[" + i + "]: " + user_mirror_rot[i].ToString());

            returnSB.AppendLine(" user_head_offset: " + user_head_offset.ToString());
            returnSB.AppendLine(" user_fov: " + _user_fov);

            returnSB.AppendLine(" user_wheel_up_down: " + _user_wheel_up_down);
            returnSB.AppendLine(" user_wheel_front_back: " + _user_wheel_front_back);
            returnSB.AppendLine(" user_mouse_left_right_default: " + _user_mouse_left_right_default);
            returnSB.AppendLine(" user_mouse_up_down_default: " + _user_mouse_up_down_default);

            returnSB.AppendLine(" accessories: " + accessories.Count);
            for (int i = 0; i < accessories.Count; i++)
                returnSB.AppendLine(" accessories[" + i + "]: " + accessories[i]);            

            returnSB.AppendLine(" odometer: " + odometer);
            returnSB.AppendLine(" odometer_float_part: " + _odometer_float_part);
                       
            returnSB.AppendLine(" trip_fuel_l: " + trip_fuel_l);
            returnSB.AppendLine(" trip_fuel: " + _trip_fuel);
            returnSB.AppendLine(" trip_distance_km: " + trip_distance_km);
            returnSB.AppendLine(" trip_distance: " + _trip_distance);
            returnSB.AppendLine(" trip_time_min: " + trip_time_min);
            returnSB.AppendLine(" trip_time: " + _trip_time);

            returnSB.AppendLine(" license_plate: " + _license_plate);

            returnSB.AppendLine(" chassis_wear: " + _chassis_wear);

            returnSB.AppendLine(" wheels_wear: " + wheels_wear.Count);
            for (int i = 0; i < wheels_wear.Count; i++)
                returnSB.AppendLine(" wheels_wear[" + i + "]: " + wheels_wear[i]);


            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            return returnString;
        }
    }

    
}
