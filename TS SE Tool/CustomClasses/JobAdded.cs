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
    class JobAdded
    {
        public string SourceCity { get; set; }
        public string SourceCompany { get; set; }
        public string DestinationCity { get; set; }
        public string DestinationCompany { get; set; }
        public string Cargo { get; set; }
        public int Urgency { get; set; }
        public int Type { get; set; }
        public int Distance { get; set; }
        public int Ferrytime { get; set; }
        public int Ferryprice { get; set; }
        public int UnitsCount { get; set; }
        public int ExpirationTime { get; set; }
        public string CompanyTruck { get; set; }
        public string TrailerVariant { get; set; }
        public string TrailerDefinition { get; set; }


        public JobAdded(string _SourceCity, string _SourceCompany, string _DestinationCity, string _DestinationCompany, string _Cargo, int _Urgency, int _Type, 
            int _UnitsCount, int _Distance, int _Ferrytime, int _Ferryprice, int _ExpirationTime, string _CompanyTruck, string _TrailerVariant, string _TrailerDefinition)
        {
            SourceCity = _SourceCity;
            SourceCompany = _SourceCompany;
            DestinationCity = _DestinationCity;
            DestinationCompany = _DestinationCompany;
            Cargo = _Cargo;
            Urgency = _Urgency;
            Type = _Type;
            Distance = _Distance;
            Ferrytime = _Ferrytime;
            Ferryprice = _Ferryprice;
            UnitsCount = _UnitsCount;
            ExpirationTime = _ExpirationTime;
            CompanyTruck = _CompanyTruck;
            TrailerVariant = _TrailerVariant;
            TrailerDefinition = _TrailerDefinition;
        }

    }
}
