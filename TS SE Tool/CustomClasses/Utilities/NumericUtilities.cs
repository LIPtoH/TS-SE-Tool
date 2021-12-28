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
using System.Linq;
using System.Collections.Generic;

namespace TS_SE_Tool.Utilities
{
    public class NumericUtilities
    {
        public static float HexFloatToSingleFloat(string _input)
        {
            if (_input.Contains('&'))
            {
                string binarystring = String.Join(String.Empty, _input.Substring(1).Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));

                short sign = Convert.ToInt16(binarystring.Substring(0, 1), 2);
                byte exp = Convert.ToByte(binarystring.Substring(1, 8), 2);
                uint mantis = Convert.ToUInt32(binarystring.Substring(9, 23), 2);

                string decstrign = sign.ToString() + " " + exp.ToString() + " " + mantis.ToString();

                float decformat = (float)(Math.Pow(-1, sign) * Math.Pow(2, (exp - 127)) * (1 + (mantis / Math.Pow(2, 23))));

                return decformat;
            }
            else
            {
                return Convert.ToSingle(_input);
            }
        }

        public static string SingleFloatToHexFloat(float _input)
        {
            if (_input != 0 && _input != 1)
            {
                //Get bytes
                byte[] tmpByteArray = BitConverter.GetBytes(_input);
                
                //Reverse order
                Array.Reverse(tmpByteArray);

                //remove dashes and make it lower case
                string hexFloat = BitConverter.ToString(tmpByteArray).Replace("-", "").ToLower();
                
                //Result
                return "&" + hexFloat;
            }
            else
            {
                return _input.ToString();
            }
        }

        public static string IntegerToHexString(uint _integer)
        {
            return _integer.ToString("X2");
        }

        public static string IntegerToBinString(int _integer)
        {
            return Convert.ToString(_integer, 2);
        }

        public static IEnumerable<int> SplitNConvertSSCHexColor(string _inputStr, int chunkSize)
        {
            return Enumerable.Range(0, _inputStr.Length / chunkSize).Select(i => Convert.ToInt32(_inputStr.Substring(i * chunkSize, chunkSize), 16));
        }
    }
}
