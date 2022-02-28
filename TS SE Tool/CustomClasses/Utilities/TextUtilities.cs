/*
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

        public static string FromStringToOutputString(string _input)
        {
            try
            {
                if (_input == "" || _input == null)
                    return "\"\"";          //Empty

                string byteArrayString = StringToByteArrayStringFull(_input);

                if (CheckStringAlphaNumeric(byteArrayString))
                    return _input;                          //Simple AlphaNumeric string 
                else
                    return "\"" + byteArrayString + "\"";   //Else
            }
            catch
            {
                return null;
            }
        }

        public static string FromOutputStringToString(string _input)
        {
            try
            {
                string byteArrayString = StringToByteArrayStringFull(_input);

                if (!CheckStringAlphaNumeric(byteArrayString))
                    return _input.Substring(1, _input.Length - 2);
                else
                    return _input;
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

        public static string StringToByteArrayStringFull(string _input)
        {
            string output = "";

            foreach(char x in _input)
            {
                byte[] testBytes = Encoding.UTF8.GetBytes(new char[] { x });

                if (testBytes.Length == 1)
                {
                    output += x;
                }
                else
                {
                    foreach (byte xByte in testBytes)                    
                        output += "\\x" + xByte.ToString("x");                    
                }
            }

            return output;
        }

        internal static string CapitalizeWord(string _input)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(_input))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(_input[0]) + _input.Substring(1).ToLower();
        }

        public static bool CheckStringAlphaNumeric(string _input)
        {
            foreach (char xChar in _input)
            {
                if(!char.IsLetterOrDigit(xChar) && !(xChar == '_'))
                    return false;
            }

            return true;
        }

        private bool checkStringContainsUnescape(string _input)
        {
            if (_input.Contains(' ') || _input != System.Text.RegularExpressions.Regex.Unescape(_input))
                return true;
            else
                return false;
        }

        internal static string CheckAndClearStringFromQuotes(string _input)
        {
            string processingResult = "";

            if (_input.StartsWith("\"") && _input.EndsWith("\""))
            {
                string innerData = _input.Substring(1, _input.Length - 2);//.Remove(_input.Length - 1, 1).Remove(0, 1);

                if (innerData == "")
                    return "";

                processingResult = TextUtilities.FromUtfHexToString(innerData);
            }            

            return (processingResult == "") ? _input : processingResult;
        }
    }
}
