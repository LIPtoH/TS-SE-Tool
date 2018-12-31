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

namespace TS_SE_Tool
{
    class Cargo //: IEquatable<Cargo>
    {
        public string CargoName { get; set; }
        public int CargoType { get; set; }
        //public bool[] CargoVariant = new bool[6];
        public Dictionary<string, Dictionary<string, int>> CargoVarDef = new Dictionary<string, Dictionary<string, int>>();
        //public int unitsCount { get; set; }

        public Cargo(string _CargoName, int _CargoType, string _CargoVariant, string _CargoDefinition, string _UnitsCount)//int _CargoVariant)
        {
            CargoName = _CargoName;
            CargoType = _CargoType;
            //CargoVariant[_CargoVariant] = true;
            Dictionary<string, int> temp = new Dictionary<string, int>();
            temp.Add(_CargoVariant, int.Parse(_UnitsCount));

            CargoVarDef.Add(_CargoDefinition, temp);
        }

        public Cargo(string _CargoName, int _CargoType, Dictionary<string, Dictionary<string, int>> _CargoVarDef) //bool[] _CargoVariant)
        {
            CargoName = _CargoName;
            CargoType = _CargoType;
            //CargoVariant = _CargoVariant;
            CargoVarDef = _CargoVarDef;
        }
        /*
        public Cargo(string _CargoName, string _CargoType, string _CargoVariants)
        {
            CargoName = _CargoName;
            CargoType = int.Parse(_CargoType);

            char[] temp = Convert.ToString(int.Parse(_CargoVariants), 2).PadLeft(6, '0').ToCharArray();

            for (int i = temp.Length - 1; i >= 0; i--)
            {
                if (temp[i] == '1')
                    CargoVariant[temp.Length - i - 1] = true;
                else
                    CargoVariant[temp.Length - i - 1] = false;
            }
        }
        */
        public string getOriginalName(string _display_name)
        {
            return _display_name.Split(new string[] { " [" }, 0)[0];
        }

        /*
        public bool Equals(Cargo other)
        {
            // Would still want to check for null etc. first.
            return CargoName == other.CargoName &&
                   CargoType == other.CargoType &&
                   CargoVariant == other.CargoVariant;
        }
        */
    }
        
    #region Equality

    class CargoComparer : IEqualityComparer<Cargo>
    {
        public bool Equals(Cargo obj1, Cargo obj2)
        {
            if (Object.ReferenceEquals(obj1, null) || Object.ReferenceEquals(obj2, null))
                return false;
            //if (ReferenceEquals(null, obj2)) return false;

            if (ReferenceEquals(obj1, obj2)) return true;

            //if (obj1.GetType() != obj2.GetType()) return false;

            return Equals2(obj1 , obj2);
        }

        public bool Equals2(Cargo Cargo1, Cargo Cargo2)
        {
            //if (Cargo1 == null || Cargo2 == null) return false;
            //else 
            if (Cargo1 == null && Cargo2 == null) return true;

            return Cargo1.CargoName == Cargo2.CargoName && Cargo1.CargoType == Cargo2.CargoType && (Cargo1.CargoVarDef == Cargo2.CargoVarDef); //Enumerable.SequenceEqual(Cargo1.CargoVariant, Cargo2.CargoVariant);
        }

        public int GetHashCode(Cargo obj)
        {
            if (Object.ReferenceEquals(obj, null)) return 0;

            unchecked
            {
                var hashCode = 13;
                hashCode = (hashCode * 397) ^ obj.CargoType;
                var myStrHashCode = !string.IsNullOrEmpty(obj.CargoName) ? obj.CargoName.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ myStrHashCode;

                for (int i = 0; i < obj.CargoVarDef.Count; i++)
                {
                    hashCode = hashCode * 3 + obj.CargoVarDef.GetHashCode();
                }
                /*
                for (int i = 0; i < obj.CargoVariant.Length; i++)
                {
                    hashCode = hashCode * 3 + obj.CargoVariant[i].GetHashCode();
                }
                */
                //hashCode = (hashCode * 397) ^ CargoVariant.GetHashCode();
                return hashCode;
            }
        }
    }
    #endregion
    
}
