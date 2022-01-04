using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TS_SE_Tool.Utilities;
using TS_SE_Tool.CustomClasses.Global;

namespace TS_SE_Tool.Save.Items
{
    class Trailer
    {
        internal float      cargo_mass          { get; set; } = 0;
        internal string      _cargo_mass
        {
            get
            {
                return NumericUtilities.SingleFloatToHexFloat(cargo_mass);
            }
            set
            {
                cargo_mass = NumericUtilities.HexFloatToSingleFloat(value);
            }
        }

        internal float      cargo_damage        { get; set; } = 0;
        internal string     _cargo_damage
        {
            get
            {
                return NumericUtilities.SingleFloatToHexFloat(cargo_damage);
            }
            set
            {
                cargo_damage = NumericUtilities.HexFloatToSingleFloat(value);
            }
        }

        internal float      trailer_body_wear   { get; set; } = 0;
        internal string     _trailer_body_wear
        {
            get
            {
                return NumericUtilities.SingleFloatToHexFloat(trailer_body_wear);
            }
            set
            {
                trailer_body_wear = NumericUtilities.HexFloatToSingleFloat(value);
            }
        }

        internal float      chassis_wear        { get; set; } = 0;
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

        internal FloatList    wheels_wear     { get; set; } = new FloatList(); //List<float>();

        internal List<string>   accessories     { get; set; } = new List<string>();

        internal string     license_plate       { get; set; } = "";
        internal string     _license_plate
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

        internal string     trailer_definition  { get; set; } = "";

        internal string     slave_trailer       { get; set; } = "";

        internal bool       is_private          { get; set; } = false;

        internal bool       oversize            { get; set; } = false;

        internal float      virtual_rear_wheels_offset { get; set; } = 0;
        internal string     _virtual_rear_wheels_offset
        {
            get
            {
                return NumericUtilities.SingleFloatToHexFloat(virtual_rear_wheels_offset);
            }
            set
            {
                virtual_rear_wheels_offset = NumericUtilities.HexFloatToSingleFloat(value);
            }
        }

        internal uint       odometer            { get; set; } = 0;

        internal float      odometer_float_part { get; set; } = 0;
        internal string     _odometer_float_part
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


        internal uint       trip_fuel_l         { get; set; } = 0;

        internal float      trip_fuel           { get; set; } = 0;
        internal string     _trip_fuel
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

        internal uint       trip_distance_km    { get; set; } = 0;

        internal float      trip_distance       { get; set; } = 0;
        internal string     _trip_distance
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

        internal uint       trip_time_min       { get; set; } = 0;

        internal float      trip_time           { get; set; } = 0;
        internal string     _trip_time
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

        internal Trailer()
        {

        }

        internal Trailer(string[] _input)
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

                    case "trailer_definition":
                        trailer_definition = dataLine;
                        break;

                    case "oversize":
                        oversize = bool.Parse(dataLine);
                        break;

                    case "cargo_mass":
                        _cargo_mass = dataLine;
                        break;

                    case "cargo_damage":
                        _cargo_damage = dataLine;
                        break;

                    case "virtual_rear_wheels_offset":
                        _virtual_rear_wheels_offset = dataLine;
                        break;

                    case "slave_trailer":
                        slave_trailer = dataLine;
                        break;

                    case "is_private":
                        is_private = bool.Parse(dataLine);
                        break;

                    case "trailer_body_wear":
                        _trailer_body_wear = dataLine;
                        break;

                    case "accessories":
                        accessories.Capacity = int.Parse(dataLine);
                        break;

                    case var s when s.StartsWith("accessories["):
                        accessories.Add(dataLine);
                        break;

                    case "odometer":
                        odometer = uint.Parse(dataLine);
                        break;

                    case "odometer_float_part":
                        _odometer_float_part = dataLine;
                        break;

                    case "trip_fuel_l":
                        trip_fuel_l = uint.Parse(dataLine);
                        break;

                    case "trip_fuel":
                        _trip_fuel = dataLine;
                        break;

                    case "trip_distance_km":
                        trip_distance_km = uint.Parse(dataLine);
                        break;

                    case "trip_distance":
                        _trip_distance = dataLine;
                        break;

                    case "trip_time_min":
                        trip_time_min = uint.Parse(dataLine);
                        break;

                    case "trip_time":
                        _trip_time = dataLine;
                        break;

                    case "license_plate":
                        _license_plate = dataLine;
                        break;

                    case "chassis_wear":
                        _chassis_wear = dataLine;
                        break;

                    case "wheels_wear":
                        wheels_wear.Capacity = int.Parse(dataLine);
                        break;

                    case var s when s.StartsWith("wheels_wear["):
                        wheels_wear.Add(dataLine);
                        break;

                } //switch end
            } //loop end
        }

        internal string PrintOut(uint _version, string _nameless)
        {
            string returnString = "";

            StringBuilder returnSB = new StringBuilder();

            returnSB.AppendLine("trailer : " + _nameless + " {");

            returnSB.AppendLine(" trailer_definition: " + trailer_definition);

            returnSB.AppendLine(" oversize: " + oversize.ToString().ToLower());

            returnSB.AppendLine(" cargo_mass: " + _cargo_mass);
            returnSB.AppendLine(" cargo_damage: " + _cargo_damage);

            returnSB.AppendLine(" virtual_rear_wheels_offset: " + _virtual_rear_wheels_offset);

            returnSB.AppendLine(" slave_trailer: " + slave_trailer);
            returnSB.AppendLine(" is_private: " + is_private.ToString().ToLower());

            returnSB.AppendLine(" trailer_body_wear: " + _trailer_body_wear);

            returnSB.AppendLine(" accessories: " + accessories.Count);
            for (int i = 0; i < accessories.Count; i++)
                returnSB.AppendLine(" accessories[" + i + "]: " + accessories[i]);

            returnSB.AppendLine(" odometer: " + odometer);
            returnSB.AppendLine(" odometer_float_part: " +_odometer_float_part);

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
