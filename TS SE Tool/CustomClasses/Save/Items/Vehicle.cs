using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TS_SE_Tool.Utilities;
using TS_SE_Tool.Save.DataFormat;

namespace TS_SE_Tool.Save.Items
{
    class Vehicle
    {
        internal float      engineWear { get; set; } = 0;
        internal float      transmissionWear { get; set; } = 0;
        internal float      cabinWear { get; set; } = 0;
        internal float      chassisWear { get; set; } = 0;
        internal float[]    wheelsWear { get; set; } = new float[0];

        internal List<string> accessories { get; set; } = new List<string>();

        internal string     licensePlate { get; set; } = "";

        internal float      fuelRelative { get; set; } = 1;

        internal uint       odometer { get; set; } = 0;
        internal float      odometer_float_part { get; set; } = 0;

        internal float      rheostat_factor { get; set; } = 0;

        internal List<Vector_4f>    user_mirror_rot { get; set; } = new List<Vector_4f>();

        internal Vector_3f          user_head_offset { get; set; } = new Vector_3f();

        internal int    user_fov { get; set; } = 0;

        internal int    user_wheel_up_down { get; set; } = 0;
        internal int    user_wheel_front_back { get; set; } = 0;
        internal int    user_mouse_left_right_default { get; set; } = 0;
        internal int    user_mouse_up_down_default { get; set; } = 0;

        internal uint   trip_fuel_l { get; set; } = 0;
        internal float  trip_fuel { get; set; } = 0;

        internal uint   trip_distance_km { get; set; } = 0;
        internal float  trip_distance { get; set; } = 0;

        internal uint   trip_time_min { get; set; } = 0;
        internal float  trip_time { get; set; } = 0;

        internal string PrintOut(uint _version, string _nameless)
        {
            string returnString = "";

            StringBuilder returnSB = new StringBuilder();

            returnSB.AppendLine("vehicle : " + _nameless + " {");

            returnSB.AppendLine(" engine_wear: " + NumericUtilities.SingleFloatToHexFloat(engineWear));
            returnSB.AppendLine(" transmission_wear: " + NumericUtilities.SingleFloatToHexFloat(transmissionWear));
            returnSB.AppendLine(" cabin_wear: " + NumericUtilities.SingleFloatToHexFloat(cabinWear));

            returnSB.AppendLine(" fuel_relative: " + NumericUtilities.SingleFloatToHexFloat(fuelRelative));

            returnSB.AppendLine(" rheostat_factor: " + NumericUtilities.SingleFloatToHexFloat(rheostat_factor));

            returnSB.AppendLine(" user_mirror_rot: " + user_mirror_rot.Count);
            for (int i = 0; i < user_mirror_rot.Count; i++)
                returnSB.AppendLine(" user_mirror_rot[" + i + "]: " + user_mirror_rot[i].ToString());

            returnSB.AppendLine(" user_head_offset: " + user_head_offset.ToString());
            returnSB.AppendLine(" user_fov: " + user_fov);

            returnSB.AppendLine(" user_wheel_up_down: " + user_wheel_up_down);
            returnSB.AppendLine(" user_wheel_front_back: " + user_wheel_front_back);
            returnSB.AppendLine(" user_mouse_left_right_default: " + user_mouse_left_right_default);
            returnSB.AppendLine(" user_mouse_up_down_default: " + user_mouse_up_down_default);

            returnSB.AppendLine(" accessories: " + accessories.Count);
            for (int i = 0; i < accessories.Count; i++)
                returnSB.AppendLine(" accessories[" + i + "]: " + accessories[i]);            

            returnSB.AppendLine(" odometer: " + odometer);
            returnSB.AppendLine(" odometer_float_part: " + NumericUtilities.SingleFloatToHexFloat(odometer_float_part));
                       
            returnSB.AppendLine(" trip_fuel_l: " + trip_fuel_l);
            returnSB.AppendLine(" trip_fuel: " + NumericUtilities.SingleFloatToHexFloat(trip_fuel));
            returnSB.AppendLine(" trip_distance_km: " + trip_distance_km);
            returnSB.AppendLine(" trip_distance: " + NumericUtilities.SingleFloatToHexFloat(trip_distance));
            returnSB.AppendLine(" trip_time_min: " + trip_time_min);
            returnSB.AppendLine(" trip_time: " + NumericUtilities.SingleFloatToHexFloat(trip_time));

            returnSB.AppendLine(" license_plate: " + TextUtilities.FromStringToOutputString(licensePlate));

            returnSB.AppendLine(" chassis_wear: " + NumericUtilities.SingleFloatToHexFloat(chassisWear));

            returnSB.AppendLine(" wheels_wear: " + wheelsWear.Length);
            for (int i = 0; i < wheelsWear.Length; i++)
                returnSB.AppendLine(" wheels_wear[" + i + "]: " + NumericUtilities.SingleFloatToHexFloat(wheelsWear[i]));


            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            return returnString;
        }
    }

    
}
