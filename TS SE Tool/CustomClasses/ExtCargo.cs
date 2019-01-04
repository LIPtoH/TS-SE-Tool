using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS_SE_Tool
{
    class ExtCargo
    {
        public ExtCargo(string _CargoName)
        {
            CargoName = _CargoName;
        }

        public string CargoName { get; set; }
        public List<string> Groups { get; set; }
        public List<string> BodyTypes { get; set; }
        public int Fragility { get; set; } = 0;
        public bool Valuable { get; set; } = false;
        public bool overweight { get; set; } = false;
        public int ADRclass { get; set; } = 0;
        public int UnitRewardpPerKM { get; set; } = 0;
    }
}
