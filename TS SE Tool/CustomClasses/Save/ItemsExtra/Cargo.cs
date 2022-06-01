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
    class Cargo : IEquatable<Cargo>, ICloneable
    {
        public string CargoName { get; set; }

        public string CargoNameTranslated { get; set; }

        public List<TrailerDefinition> TrailerDefList = new List<TrailerDefinition>();

        public Cargo(string _CargoName, int _CargoType, string _TrailerDefinition, string _UnitsCount)
        {
            CargoName = _CargoName;
            TrailerDefList.Add(new TrailerDefinition(_TrailerDefinition, _CargoType, _UnitsCount));
        }
        public Cargo(string _CargoName, int _CargoType, string _TrailerDefinition, int _UnitsCount)
        {
            CargoName = _CargoName;
            TrailerDefList.Add(new TrailerDefinition(_TrailerDefinition, _CargoType, _UnitsCount));
        }


        public Cargo(string _CargoName, List<TrailerDefinition> _CargoDefList)
        {
            CargoName = _CargoName;
            TrailerDefList.AddRange(_CargoDefList);
        }

        //===

        public virtual object Clone() {
            return new Cargo(this.CargoName, this.TrailerDefList);
        }

        public bool Equals(Cargo other)
        {
            if (other is null)
                return false;

            return this.CargoName == other.CargoName && (this.TrailerDefList.Except(other.TrailerDefList).Count() == 0);
        }

        public override bool Equals(object obj) => Equals(obj as Cargo);
        public override int GetHashCode() => (CargoName, TrailerDefList.OrderBy(x => x.DefName)).GetHashCode();

        public override string ToString()
        {
            return CargoName + " | " + TrailerDefList.Count();
        }
    }
        
    #region Equality
    class CargoComparer : IEqualityComparer<Cargo>
    {
        public bool Equals(Cargo obj1, Cargo obj2)
        {
            if (ReferenceEquals(obj1, obj2)) return true;

            if (Object.ReferenceEquals(obj1, null) || Object.ReferenceEquals(obj2, null))
                return false;

            if (obj1 == null && obj2 == null) return true;

            TrailerDefinitionComparer _comparer = new TrailerDefinitionComparer();

            var difList = obj1.TrailerDefList.Except(obj2.TrailerDefList, _comparer).ToList();

            return obj1.CargoName == obj2.CargoName && (difList.Count() == 0);
        }

        
        public int GetHashCode(Cargo obj)
        {
            if (Object.ReferenceEquals(obj, null)) return 0;

            unchecked
            {
                var hashCode = 13;
                var myStrHashCode = !string.IsNullOrEmpty(obj.CargoName) ? obj.CargoName.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ myStrHashCode;

                obj.TrailerDefList.Sort((a, b) => a.DefName.CompareTo(b.DefName));

                for (int i = 0; i < obj.TrailerDefList.Count(); i++)
                {
                    hashCode = hashCode * 3 + obj.TrailerDefList[i].GetHashCode();
                }

                return hashCode;
            }
        }
        
    }
    #endregion    
}
