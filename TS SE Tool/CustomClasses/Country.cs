using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS_SE_Tool
{
    class Country
    {
        string CountryName { get; set; }
        public string ShortName { get; set; }
        public double FuelPrice { get; set; }

        public Country(string _countryName, string _shortName, string _fuelPrice)
        {
            CountryName = _countryName;
            ShortName = _shortName;
            FuelPrice = Convert.ToDouble(_fuelPrice);
        }
    }
}
