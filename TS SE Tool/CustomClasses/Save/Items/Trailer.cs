using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TS_SE_Tool.Utilities;
using TS_SE_Tool.Save.DataFormat;
using TS_SE_Tool.CustomClasses.Global;

namespace TS_SE_Tool.Save.Items
{
    class Trailer
    {
        #region variables
        internal SCS_Float  cargo_mass          { get; set; } = 0;
        internal SCS_Float  cargo_damage        { get; set; } = 0;

        internal SCS_Float  trailer_body_wear   { get; set; } = 0;
        internal SCS_Float  chassis_wear        { get; set; } = 0;
        internal List<SCS_Float> wheels_wear { get; set; } = new List<SCS_Float>();

        internal List<string>   accessories     { get; set; } = new List<string>();

        internal SCS_String     license_plate       { get; set; } = "";

        internal string     trailer_definition  { get; set; } = "";

        internal string     slave_trailer       { get; set; } = "";

        internal bool       is_private          { get; set; } = false;

        internal bool       oversize            { get; set; } = false;

        internal SCS_Float  virtual_rear_wheels_offset { get; set; } = 0;

        internal uint       odometer            { get; set; } = 0;
        internal SCS_Float  odometer_float_part { get; set; } = 0;


        internal uint       trip_fuel_l         { get; set; } = 0;
        internal SCS_Float  trip_fuel           { get; set; } = 0;

        internal uint       trip_distance_km    { get; set; } = 0;
        internal SCS_Float  trip_distance       { get; set; } = 0;

        internal uint       trip_time_min       { get; set; } = 0;
        internal SCS_Float  trip_time           { get; set; } = 0;

        #endregion

        internal Trailer()
        {

        }

        internal Trailer(string[] _input)
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

                        case "trailer_definition":
                            trailer_definition = dataLine;
                            break;

                        case "oversize":
                            oversize = bool.Parse(dataLine);
                            break;

                        case "cargo_mass":
                            cargo_mass = dataLine;
                            break;

                        case "cargo_damage":
                            cargo_damage = dataLine;
                            break;

                        case "virtual_rear_wheels_offset":
                            virtual_rear_wheels_offset = dataLine;
                            break;

                        case "slave_trailer":
                            slave_trailer = dataLine;
                            break;

                        case "is_private":
                            is_private = bool.Parse(dataLine);
                            break;

                        case "trailer_body_wear":
                            trailer_body_wear = dataLine;
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
                            odometer_float_part = dataLine;
                            break;

                        case "trip_fuel_l":
                            trip_fuel_l = uint.Parse(dataLine);
                            break;

                        case "trip_fuel":
                            trip_fuel = dataLine;
                            break;

                        case "trip_distance_km":
                            trip_distance_km = uint.Parse(dataLine);
                            break;

                        case "trip_distance":
                            trip_distance = dataLine;
                            break;

                        case "trip_time_min":
                            trip_time_min = uint.Parse(dataLine);
                            break;

                        case "trip_time":
                            trip_time = dataLine;
                            break;

                        case "license_plate":
                            license_plate = dataLine;
                            break;

                        case "chassis_wear":
                            chassis_wear = dataLine;
                            break;

                        case "wheels_wear":
                            wheels_wear.Capacity = int.Parse(dataLine);
                            break;

                        case var s when s.StartsWith("wheels_wear["):
                            wheels_wear.Add(dataLine);
                            break;
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

            returnSB.AppendLine("trailer : " + _nameless + " {");

            returnSB.AppendLine(" trailer_definition: " + trailer_definition);

            returnSB.AppendLine(" oversize: " + oversize.ToString().ToLower());

            returnSB.AppendLine(" cargo_mass: " + cargo_mass.ToString());
            returnSB.AppendLine(" cargo_damage: " + cargo_damage.ToString());

            returnSB.AppendLine(" virtual_rear_wheels_offset: " + virtual_rear_wheels_offset.ToString());

            returnSB.AppendLine(" slave_trailer: " + slave_trailer);
            returnSB.AppendLine(" is_private: " + is_private.ToString().ToLower());

            returnSB.AppendLine(" trailer_body_wear: " + trailer_body_wear.ToString());

            returnSB.AppendLine(" accessories: " + accessories.Count);
            for (int i = 0; i < accessories.Count; i++)
                returnSB.AppendLine(" accessories[" + i + "]: " + accessories[i]);

            returnSB.AppendLine(" odometer: " + odometer);
            returnSB.AppendLine(" odometer_float_part: " +odometer_float_part.ToString());

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

            return returnString;
        }

    }
}
