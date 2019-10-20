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

namespace TS_SE_Tool
{
    class TrailerDefinition
    {
        public string DefName { get; set; }
        public int CargoType { get; set; } = 0;
        public int UnitsCount { get; set; } = 1;
        public int Volume { get; set; } = 1;
        public int CargoWeight { get; set; } = 1;


        public TrailerDefinition (string _DefName)
        {
            DefName = _DefName;
        }

        public TrailerDefinition(string _DefName, int _CargoType, string _UnitsCount)
        {
            DefName = _DefName;
            CargoType = _CargoType;
            UnitsCount = int.Parse(_UnitsCount);
        }
        /*
        public TrailerDefinition(string _DefName, int _Volume, int _CargoWeight)
        {
            DefName = _DefName;
            Volume = _Volume;
            CargoWeight = _CargoWeight;
        }
        */
    }

    #region Equality

    class TrailerDefinitionComparer : IEqualityComparer<TrailerDefinition>
    {
        public bool Equals(TrailerDefinition obj1, TrailerDefinition obj2)
        {
            if (Object.ReferenceEquals(obj1, null) || Object.ReferenceEquals(obj2, null))
                return false;

            if (ReferenceEquals(obj1, obj2)) return true;

            return Equals2(obj1, obj2);
        }

        public bool Equals2(TrailerDefinition obj1, TrailerDefinition obj2)
        {
            if (obj1 == null && obj2 == null) return true;

            return obj1.DefName == obj2.DefName && obj1.CargoType == obj2.CargoType && obj1.UnitsCount == obj2.UnitsCount;
        }

        public int GetHashCode(TrailerDefinition obj)
        {
            if (Object.ReferenceEquals(obj, null)) return 0;

            unchecked
            {
                var hashCode = 13;
                var myStrHashCode = !string.IsNullOrEmpty(obj.DefName) ? obj.DefName.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ myStrHashCode;

                hashCode = hashCode * Tuple.Create(obj.CargoType, obj.UnitsCount
                    //, obj.Volume, obj.CargoWeight
                    ).GetHashCode();
                return hashCode;
            }
        }
    }
    #endregion    
}
