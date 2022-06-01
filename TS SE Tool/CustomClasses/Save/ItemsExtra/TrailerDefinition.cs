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

namespace TS_SE_Tool
{
    class TrailerDefinition : IEquatable<TrailerDefinition>
    {
        public string DefName { get; set; } = "";
        
        public int CargoType { get; set; } = 0;

        public List<CargoLoadVariants> CargoLoadVariants { get; set; } = new List<CargoLoadVariants>();

        public TrailerDefinition (string _DefName)
        {
            DefName = _DefName;
        }

        public TrailerDefinition(string _DefName, int _CargoType, string _UnitsCount)
        {
            DefName = _DefName;
            CargoType = _CargoType;
            int uc = int.Parse(_UnitsCount);

            if (!CargoLoadVariants.Exists(x => x.UnitsCount == uc))
                CargoLoadVariants.Add(new CargoLoadVariants(uc));
        }

        public TrailerDefinition(string _DefName, int _CargoType, int _UnitsCount)
        {
            DefName = _DefName;
            CargoType = _CargoType;

            if (!CargoLoadVariants.Exists(x => x.UnitsCount == _UnitsCount))
                CargoLoadVariants.Add(new CargoLoadVariants(_UnitsCount));
        }

        public override string ToString()
        {
            return DefName + " | " + CargoType.ToString() + " | " + String.Join(", ", CargoLoadVariants);
        }

        public bool Equals(TrailerDefinition other)
        {
            if (other is null)
                return false;

            return this.DefName == other.DefName && this.CargoType == other.CargoType;
        }

        public override bool Equals(object obj) => Equals(obj as TrailerDefinition);

        public override int GetHashCode() => (DefName, CargoType).GetHashCode();
    }

    #region Equality

    class TrailerDefinitionComparer : IEqualityComparer<TrailerDefinition>
    {
        public bool Equals(TrailerDefinition obj1, TrailerDefinition obj2)
        {
            if (ReferenceEquals(obj1, obj2)) return true;

            if (Object.ReferenceEquals(obj1, null) || Object.ReferenceEquals(obj2, null))
                return false;

            return obj1.DefName == obj2.DefName && obj1.CargoType == obj2.CargoType;
        }

        public int GetHashCode(TrailerDefinition obj)
        {
            if (Object.ReferenceEquals(obj, null)) return 0;

            unchecked
            {
                var hashCode = 13;
                var myStrHashCode = !string.IsNullOrEmpty(obj.DefName) ? obj.DefName.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ myStrHashCode;

                hashCode = hashCode * obj.CargoType.GetHashCode();
                return hashCode;
            }
        }
    }
    #endregion    
}
