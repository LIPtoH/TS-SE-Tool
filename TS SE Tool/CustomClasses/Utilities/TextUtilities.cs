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
                return "";
            }
        }
    }
}
