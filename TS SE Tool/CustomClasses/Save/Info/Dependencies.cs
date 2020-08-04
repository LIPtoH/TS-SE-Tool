/*
   Copyright 2016-2020 LIPtoH <liptoh.codebase@gmail.com>

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
    class Dependency
    {
        internal string Raw;

        internal string RawDepType;
        internal string DepType;

        internal string RawDepID;
        internal string DepLoadID;

        internal string DepName;

        public Dependency (string _RawText)
        {
            Raw = _RawText;

            string[] DepParts = Raw.Split(new char[] { '|' }, 3);

            RawDepType = DepParts[0];

            if (RawDepType == "dlc" || RawDepType == "rdlc")
            {
                DepType = "dlc";
                DepLoadID = DepType + '_' + DepParts[1].Split(new char[] { '_' }, 2)[1];
            }
            else if (RawDepType == "mod")
            {
                DepType = "mod";

                if (DepParts[1].StartsWith("mod_"))                
                    DepLoadID = DepType + '_' + DepParts[1].Split(new char[] { '_' }, 2)[1];                
                else                
                    DepLoadID = "mod_" + DepParts[1].Replace(' ', '_');                
            }

            RawDepID = DepParts[1];
            DepName = DepParts[2];
        }
    }
}
