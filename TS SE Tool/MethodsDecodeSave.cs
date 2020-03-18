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
using System.Windows.Forms;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System.Runtime.InteropServices;

namespace TS_SE_Tool
{
    public partial class FormMain
    {
        public unsafe string[] NewDecodeFile(string _savefile_path, Form _senderForm, string _statusStrip, string _targetLabel)
        {
            ShowStatusMessages("i", "message_loading_save_file", _senderForm, _statusStrip, _targetLabel);
            LogWriter("Loading file into memory");

            //string FileData = "";
            byte[] FileDataB = new byte[10];

            try
            {
                //FileDate = 
                FileDataB = File.ReadAllBytes(_savefile_path);
            }
            catch
            {
                LogWriter("Could not find file in: " + _savefile_path);
                ShowStatusMessages("e", "error_could_not_find_file", _senderForm, _statusStrip, _targetLabel);

                FileDecoded = false;
                return null;
            }

            int MemFileFrm = -1;
            UInt32 buff = (UInt32)FileDataB.Length;

            fixed (byte* ptr = FileDataB)
            {
                MemFileFrm = SIIGetMemoryFormat(ptr, buff);
            }

            switch (MemFileFrm)
            {
                case 1:
                    // "SIIDEC_RESULT_FORMAT_PLAINTEXT";
                    {
                        FileDecoded = true;
                        string BigS = Encoding.UTF8.GetString(FileDataB);
                        return BigS.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                    }
                case 2:
                    // "SIIDEC_RESULT_FORMAT_ENCRYPTED";
                    {
                        ShowStatusMessages("i", "message_decoding_save_file", _senderForm, _statusStrip, _targetLabel);
                        LogWriter("Decoding file");

                        int result = -1;
                        uint newbuff = 0;
                        uint* newbuffP = &newbuff;

                        fixed (byte* ptr = FileDataB)
                        {
                            result = SIIDecryptAndDecodeMemory(ptr, buff, null, newbuffP);
                        }

                        if (result == 0)
                        {
                            byte[] newFileData = new byte[(int)newbuff];

                            fixed (byte* ptr = FileDataB)
                            {
                                fixed (byte* ptr2 = newFileData)
                                    result = SIIDecryptAndDecodeMemory(ptr, buff, ptr2, newbuffP);
                            }

                            FileDecoded = true;
                            string BigS = Encoding.UTF8.GetString(newFileData);
                            return BigS.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                        }

                        return null;
                    }
                case 3:
                // "SIIDEC_RESULT_FORMAT_BINARY";
                case 4:
                    // "SIIDEC_RESULT_FORMAT_3NK";
                    {
                        ShowStatusMessages("i", "message_decoding_save_file", _senderForm, _statusStrip, _targetLabel);
                        LogWriter("Decoding file");

                        int result = -1;
                        uint newbuff = 0;
                        uint* newbuffP = &newbuff;

                        fixed (byte* ptr = FileDataB)
                        {
                            result = SIIDecodeMemory(ptr, buff, null, newbuffP);
                        }

                        if (result == 0)
                        {
                            byte[] newFileData = new byte[(int)newbuff];

                            fixed (byte* ptr = FileDataB)
                            {
                                fixed (byte* ptr2 = newFileData)
                                    result = SIIDecodeMemory(ptr, buff, ptr2, newbuffP);
                            }

                            FileDecoded = true;
                            string BigS = Encoding.UTF8.GetString(newFileData);
                            return BigS.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                        }
                        return null;
                    }
                case -1:
                // "SIIDEC_RESULT_GENERIC_ERROR";
                case 10:
                // "SIIDEC_RESULT_FORMAT_UNKNOWN";
                case 11:
                // "SIIDEC_RESULT_TOO_FEW_DATA";
                default:
                    // "UNEXPECTED_ERROR";
                    return null;
            }
        }

        public string FromHexToString(string _hex)
        {
            try
            {
                byte[] raw = new byte[_hex.Length / 2];
                for (int i = 0; i < raw.Length; i++)
                {
                    raw[i] = Convert.ToByte(_hex.Substring(i * 2, 2), 16);
                }

                return Encoding.UTF8.GetString(raw); //UTF8
            }
            catch
            {
                return null;
            }
        }

        public static string FromStringToHex(string _sourceString)
        {
            string result = "";
            for (int i = 0; i < _sourceString.Length; i++)
            {
                char tempChar = _sourceString[i];

                if ((tempChar >= 'a' && tempChar <= 'z') || (tempChar >= 'A' && tempChar <= 'Z') || Char.IsDigit(tempChar) || Char.IsWhiteSpace(tempChar))
                {
                    result += tempChar;
                }
                else
                {
                    byte[] tarray = Encoding.UTF8.GetBytes(new char[] { tempChar });

                    if (tarray.Length > 1)
                        foreach (byte tb in tarray)
                            result += "\\x" + tb.ToString("x");
                    else
                        if(tempChar == '"')
                            result += "\\\"";
                        else
                            result += tempChar;
                }
            }
            return result;
        }

        public static decimal HexFloatToDecimalFloat(string _hexFloat)
        {
            string binarystring = String.Join(String.Empty, _hexFloat.Substring(1).Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));

            short sign = Convert.ToInt16(binarystring.Substring(0, 1), 2);
            int exp = Convert.ToInt32(binarystring.Substring(1, 8), 2);
            int mantis = Convert.ToInt32(binarystring.Substring(9, 23), 2);

            string decstrign = sign.ToString() + " " + exp.ToString() + " " + mantis.ToString();

            decimal decformat = (decimal)(Math.Pow(-1, sign) * Math.Pow(2, (exp - 127)) * (1 + (mantis / Math.Pow(2, 23))));

            return decformat;
        }

        public static string IntegerToHexString(uint _integer)
        {
            return _integer.ToString("X2");
        }

        public static string IntegerToBinString(int _integer)
        {
            return Convert.ToString(_integer, 2);
        }

        //SII decrypt
        [DllImport(@"libs/SII_Decrypt.dll", EntryPoint = "GetFileFormat")]
        public static extern Int32 SIIGetFileFormat(string FilePath);

        //unsafe
        [DllImport(@"libs/SII_Decrypt.dll", EntryPoint = "GetMemoryFormat")]
        public static extern unsafe Int32 SIIGetMemoryFormat(byte* InputMS, uint InputMSSize);

        [DllImport(@"libs/SII_Decrypt.dll", EntryPoint = "DecryptAndDecodeMemory")]
        public static extern unsafe Int32 SIIDecryptAndDecodeMemory(byte* InputMS, uint InputMSSize, byte* OutputMS, uint* OutputMSSize);

        [DllImport(@"libs/SII_Decrypt.dll", EntryPoint = "DecodeMemory")]
        public static extern unsafe Int32 SIIDecodeMemory(byte* InputMS, uint InputMSSize, byte* OutputMS, uint* OutputMSSize);

        private string SIIresultDecode (int inputR)
        {
            switch (inputR)
            {
                case -1:
                    return "SIIDEC_RESULT_GENERIC_ERROR";
                case 0:
                    return "SIIDEC_RESULT_SUCCESS";
                case 1:
                    return "SIIDEC_RESULT_FORMAT_PLAINTEXT";
                case 2:
                    return "SIIDEC_RESULT_FORMAT_ENCRYPTED";
                case 3:
                    return "SIIDEC_RESULT_FORMAT_BINARY";
                case 4:
                    return "SIIDEC_RESULT_FORMAT_3NK";
                case 10:
                    return "SIIDEC_RESULT_FORMAT_UNKNOWN";
                case 11:
                    return "SIIDEC_RESULT_TOO_FEW_DATA";
                case 12:
                    return "SIIDEC_RESULT_BUFFER_TOO_SMALL";
                default:
                    return "UNEXPECTED_ERROR";
            }
        }
    }
}