using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TS_SE_Tool.Utilities;
using TS_SE_Tool.Save.DataFormat;

namespace TS_SE_Tool.Save.Items
{
    class Trailer
    {
        internal uint       cargoMass { get; set; } = 0;
        internal float      cargoDamage { get; set; } = 0;

        internal float      trailerBodyWear { get; set; } = 0;
        internal float      chassisWear { get; set; } = 0;
        internal float[]    wheelsWear { get; set; } = new float[0];

        internal List<string>   accessories { get; set; } = new List<string>();

        internal string     licensePlate { get; set; } = "";

        internal string     trailer_definition { get; set; } = "";

        internal string     slave_trailer { get; set; } = "";

        internal bool       is_private { get; set; } = false;

        internal bool       oversize { get; set; } = false;

        internal float      virtual_rear_wheels_offset { get; set; } = 0;

        internal uint       odometer { get; set; } = 0;
        internal float      odometer_float_part { get; set; } = 0;

        internal uint       trip_fuel_l { get; set; } = 0;
        internal float      trip_fuel { get; set; } = 0;

        internal uint       trip_distance_km { get; set; } = 0;
        internal float      trip_distance { get; set; } = 0;

        internal uint       trip_time_min { get; set; } = 0;
        internal float      trip_time { get; set; } = 0;
        
    }
}
