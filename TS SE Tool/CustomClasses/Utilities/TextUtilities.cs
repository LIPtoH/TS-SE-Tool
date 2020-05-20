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

namespace TS_SE_Tool.Utilities
{
    public class TextUtilities
    {
        public static string FromHexToString(string _input)
        {
            try
            {
                byte[] raw = new byte[_input.Length / 2];
                for (int i = 0; i < raw.Length; i++)
                {
                    raw[i] = Convert.ToByte(_input.Substring(i * 2, 2), 16);
                }

                return Encoding.UTF8.GetString(raw); //UTF8
            }
            catch
            {
                return null;
            }
        }

        public static string FromUtfHexToString(string _input)
        {
            try
            {
                string result = "";

                for (int i = 0; i < _input.Length; i++)
                {
                    if (i < _input.Length - 2 && _input.Substring(i, 2) == "\\x")
                    {
                        string temp = "";
                        nextHD:

                        temp += _input.Substring(i + 2, 2);
                        i = i + 4;

                        if (i < _input.Length - 2 && _input.Substring(i, 2) == "\\x")
                            goto nextHD;
                        else
                        {
                            result += FromHexToString(temp);
                            i--;
                        }
                    }
                    else                    
                        result += _input[i];                    
                }

                return result;
            }
            catch
            {
                return null;
            }
        }

        public static string FromStringToHex(string _input)
        {
            try
            {
                return ByteArrayToString(Encoding.UTF8.GetBytes(_input.ToCharArray()));
            }
            catch
            {
                return null;
            }
        }

        public static string ByteArrayToString(byte[] _ba)
        {
            return BitConverter.ToString(_ba).Replace("-", "");
        }
    }
}
