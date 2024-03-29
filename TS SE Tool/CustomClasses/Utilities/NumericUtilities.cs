﻿/*
   Copyright 2016-2022 LIPtoH <liptoh.codebase@gmail.com>

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
                //Split
                string[] stringByteArray = SplitStringIntoChunks(_input.Substring(1), 2).ToArray();

                //Reverse order
                Array.Reverse(stringByteArray);

                //Get bytes
                byte[] tmpByteArray = BitConverter.GetBytes( uint.Parse(string.Concat(stringByteArray),System.Globalization.NumberStyles.HexNumber) );

                if (BitConverter.IsLittleEndian)                
                    tmpByteArray = tmpByteArray.Reverse().ToArray();

                //Result
                return BitConverter.ToSingle(tmpByteArray, 0);
            }
            else
            {
                return float.Parse(_input, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture);
            }
        }

        static IEnumerable<string> SplitStringIntoChunks(string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize)
                .Select(i => str.Substring(i * chunkSize, chunkSize));
        }

        public static string SingleFloatToString(float _input)
        {
            if (!Single.IsNaN(_input))
            {
                int intFloat = (int)_input;

                if (intFloat - _input != 0 || _input >= 1e7)
                {
                    return "&" + SingleFloatToHexFloat(_input);
                }
                else
                {
                    return _input.ToString("g6", System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            else
            {
                return "&" + SingleFloatToHexFloat(_input);
            }
        }

        public static string SingleFloatToHexFloat(float _input)
        {
            //Get bytes
            byte[] tmpByteArray = BitConverter.GetBytes(_input);

            //Reverse order
            Array.Reverse(tmpByteArray);

            //remove dashes and make it lower case
            string hexFloat = BitConverter.ToString(tmpByteArray).Replace("-", "").ToLower();

            return hexFloat;
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
