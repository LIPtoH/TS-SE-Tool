using System;
using System.Collections.Generic;
using System.Linq;

namespace TS_SE_Tool
{
    class CargoLoadVariants
    {
        public int UnitsCount { get; set; } = 1;
        
        /*
        public int Volume { get; set; } = 1;

        public int CargoWeight { get; set; } = 1;
        */

        public CargoLoadVariants(int _UnitsCount)
        {
            UnitsCount = _UnitsCount;
        }

        public override string ToString()
        {
            return UnitsCount.ToString();
        }
    }
}
