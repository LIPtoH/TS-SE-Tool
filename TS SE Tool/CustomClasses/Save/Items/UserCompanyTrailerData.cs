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
using TS_SE_Tool.Save.Items;

namespace TS_SE_Tool
{
    public class UserCompanyTrailerData
    {
        public UserCompanyTrailerData()
        {

        }

        public List<UserCompanyTruckDataPart> Parts = new List<UserCompanyTruckDataPart>();
        public string TrailerType { get; set; } = "fromsave";
        public bool Main { get; set; } = true;
        public bool Users { get; set; } = true;

        internal Trailer TrailerMainData { get; set; } = new Trailer();

        public string ProfitLogs { get; set; } = null;
    }
}
