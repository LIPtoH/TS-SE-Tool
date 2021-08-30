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
        public static decimal HexFloatToDecimalFloat(string _hexFloat)
        {
            if (_hexFloat.Contains('&'))
            {
                string binarystring = String.Join(String.Empty, _hexFloat.Substring(1).Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));

                short sign = Convert.ToInt16(binarystring.Substring(0, 1), 2);
                byte exp = Convert.ToByte(binarystring.Substring(1, 8), 2);
                uint mantis = Convert.ToUInt32(binarystring.Substring(9, 23), 2);

                string decstrign = sign.ToString() + " " + exp.ToString() + " " + mantis.ToString();

                decimal decformat = (decimal)(Math.Pow(-1, sign) * Math.Pow(2, (exp - 127)) * (1 + (mantis / Math.Pow(2, 23))));

                return decformat;
            }
            else
            {
                return Convert.ToDecimal(_hexFloat);
            }
        }

        public static string DecimalFloatToHexFloat(decimal _decimalFloat)
        {
            if (_decimalFloat != 0 && _decimalFloat != 1)
            {
                string _hexFloat = "&";

                //Sign
                short sign = 0; // 0 - positive 1 - negative

                if (Math.Sign(_decimalFloat) == -1)
                    sign = 1;
                //

                //Exponent
                sbyte exp = 0;

                decimal tmpDecimal = _decimalFloat, tmpDecimalCycle = 0;
                int expPower = 0;

                bool positive = true;

                if (tmpDecimal >= 1)
                    positive = true;
                else
                    positive = false;

                while (true)
                {
                    tmpDecimalCycle = tmpDecimal / (decimal)Math.Pow(2, expPower);

                    if (tmpDecimalCycle >= 1 && tmpDecimalCycle < 2)
                        break;

                    if (positive)
                        ++expPower;
                    else
                        --expPower;
                }

                exp = (sbyte)(127 + expPower);
                //

                //Mantis
                string strMantis = "";

                decimal tmpMantis = tmpDecimalCycle - Math.Truncate(tmpDecimalCycle);
                byte mantisBin = 0;

                while (true)
                {
                    tmpMantis = tmpMantis * 2;

                    mantisBin = Convert.ToByte(Math.Truncate(tmpMantis));

                    strMantis += mantisBin;

                    if (tmpMantis == 1 || strMantis.Length == 23)
                        break;

                    tmpMantis = tmpMantis - mantisBin;
                }
                //

                //To binary
                string binarystring = sign.ToString() + Convert.ToString(exp, 2).PadLeft(8, '0') + strMantis.PadLeft(23, '0');

                //Convert
                _hexFloat += Convert.ToInt32(binarystring, 2).ToString("x4");

                //Result
                return _hexFloat;
            }
            else
            {
                return _decimalFloat.ToString();
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
