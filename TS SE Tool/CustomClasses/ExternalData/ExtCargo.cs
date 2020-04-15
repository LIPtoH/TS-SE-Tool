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
