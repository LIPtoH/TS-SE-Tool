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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS_SE_Tool
{
    class ProgSettings
    {        
        public ProgSettings(string _ProgramVersion, string _Language, bool _ProposeRandom, Int16 _JobPickupTime, byte _LoopEvery, double _TimeMultiplier, string _DistanceMes, string _CurrencyMes)
        {
            ProgramVersion = _ProgramVersion;
            Language = _Language;
            ProposeRandom = _ProposeRandom;
            JobPickupTime = _JobPickupTime;
            LoopEvery = _LoopEvery;
            TimeMultiplier = _TimeMultiplier;
            DistanceMes = _DistanceMes;
            CurrencyMes = _CurrencyMes;
            CustomPaths = new Dictionary<string, List<string>>();
        }

        public ProgSettings()
        {

        }

        //0.1, "Default", false, 72, 0, 1.0, "km", "EUR"
        public string ProgramVersion { get; set; } = "0.0.1.0";

        public string Language { get; set; } = "Default";

        public bool ProposeRandom { get; set; } = false;

        public Int16 JobPickupTime { get; set; } = 72;

        public byte LoopEvery { get; set; } = 0;

        public double TimeMultiplier { get; set; } = 1.0;

        public string DistanceMes { get; set; } = "km";

        public string CurrencyMes { get; set; } = "EUR";

        //public List<string> CustomPaths { get; set; }
        public Dictionary<string, List<string>> CustomPaths { get; set; } = new Dictionary<string, List<string>>();
    }
}
