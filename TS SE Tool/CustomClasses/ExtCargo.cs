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
        public List<string> Groups = new List<string>();
        public List<string> BodyTypes = new List<string>();
        public int ADRclass { get; set; } = 0;
        public int MaxDistance { get; set; } = 0;
        public decimal Volume { get; set; } = 0;
        public decimal Fragility { get; set; } = 0;
        public decimal UnitRewardpPerKM { get; set; } = 0;
        public decimal Mass { get; set; } = 0;
        public bool Valuable { get; set; } = false;
        public bool Overweight { get; set; } = false;
    }
}
