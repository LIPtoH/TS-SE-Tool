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
        public ProgSettings(double _ProgramVersion, string _Language, bool _ProposeRandom, Int16 _JobPickupTime, byte _LoopEvery, double _TimeMultiplier, string _DistanceMes)
        {
            ProgramVersion = _ProgramVersion;
            Language = _Language;
            ProposeRandom = _ProposeRandom;
            JobPickupTime = _JobPickupTime;
            LoopEvery = _LoopEvery;
            TimeMultiplier = _TimeMultiplier;
            DistanceMes = _DistanceMes;
            CustomPaths = new List<string>();
        }

        public double ProgramVersion { get; set; }

        public string Language { get; set; }

        public bool ProposeRandom { get; set; }

        public Int16 JobPickupTime { get; set; }

        public byte LoopEvery { get; set; }

        public double TimeMultiplier { get; set; }

        public string DistanceMes { get; set; }

        public List<string> CustomPaths { get; set; }
    }
}
