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

namespace TS_SE_Tool
{
    class CompanyTruck
    {
        public string TruckName { get; set; }
        public int Type { get; set; } = 0;

        public CompanyTruck(string _TruckName, int _type)
        {
            TruckName = _TruckName;
            Type = _type;
        }
    }
    #region Equality

    class CompanyTruckComparer : IEqualityComparer<CompanyTruck>
    {
        public bool Equals(CompanyTruck obj1, CompanyTruck obj2)
        {
            if (Object.ReferenceEquals(obj1, null) || Object.ReferenceEquals(obj2, null))
                return false;
            //if (ReferenceEquals(null, obj2)) return false;

            if (ReferenceEquals(obj1, obj2)) return true;

            //if (obj1.GetType() != obj2.GetType()) return false;

            return Equals2(obj1, obj2);
        }

        public bool Equals2(CompanyTruck Cargo1, CompanyTruck Cargo2)
        {
            //if (Cargo1 == null || Cargo2 == null) return false;
            //else 
            if (Cargo1 == null && Cargo2 == null) return true;

            return Cargo1.TruckName == Cargo2.TruckName && Cargo1.Type == Cargo2.Type;
        }

        public int GetHashCode(CompanyTruck obj)
        {
            if (Object.ReferenceEquals(obj, null)) return 0;

            unchecked
            {
                var hashCode = 13;
                hashCode = (hashCode * 397) ^ obj.Type;
                var myStrHashCode = !string.IsNullOrEmpty(obj.TruckName) ? obj.TruckName.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ myStrHashCode;
                
                return hashCode;
            }
        }
    }
    #endregion
}
