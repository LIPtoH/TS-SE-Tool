/*
   Copyright 2016-2019 LIPtoH <liptoh.codebase@gmail.com>

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
    class ExtCompany
    {
        public string CompanyName { get; set; }
        public List<string> inCargo = new List<string>();
        public List<string> outCargo = new List<string>();

        public ExtCompany(string _companyName)
        {
            CompanyName = _companyName;
        }

        public void AddCargoIn(string _CargoName)
        {
            inCargo.Add(_CargoName);
        }

        public void AddCargoOut(string _CargoName)
        {
            outCargo.Add(_CargoName);
        }

        public void AddCargoIn(List<string> _CargoName)
        {
            inCargo.AddRange(_CargoName);
        }

        public void AddCargoOut(List<string> _CargoName)
        {
            outCargo.AddRange(_CargoName);
        }

    }
}
