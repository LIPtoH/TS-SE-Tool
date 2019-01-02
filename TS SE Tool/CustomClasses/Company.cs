/*
   Copyright 2016-2018 LIPtoH <liptoh.codebase@gmail.com>

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/
using System.Collections.Generic;

namespace TS_SE_Tool
{
    class Company
    {
        public Company(string _companyName, int _jobs)
        {
            CompanyName = _companyName;
            JobsOffers = _jobs;
        }

        public void AddCargoIn(string _CargoName)
        {
            inCargo.Add(_CargoName);
        }

        public void AddCargoOut(string _CargoName)
        {
            outCargo.Add(_CargoName);
        }

        private List<string> inCargo = new List<string>();
        private List<string> outCargo = new List<string>();
        public int[] CragoSeeds = new int[0];

        public string CompanyName { get; set; }
        public int JobsOffers { get; set; } = 0;
        public bool Excluded { get; set; } = false;
    }
}
