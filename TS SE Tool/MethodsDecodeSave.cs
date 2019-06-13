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
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace TS_SE_Tool
{
    public partial class FormMain
    {
        private string[] DecodeFile(string _savefile_path)
        {
            LogWriter("Backing up file to: " + _savefile_path + "_backup");

            File.Copy(_savefile_path, _savefile_path + "_backup", true);

            ShowStatusMessages("i", "message_loading_save_file");

            LogWriter("Loading file into memory");

            byte[] bytes = LoadFileToMemory(_savefile_path);

            if (bytes == null)
            {
                return null;
            }
            if (Encoding.UTF8.GetString(bytes, 0, 8) == "SiiNunit")
            {
                LogWriter("File already decoded");

                FileDecoded = true;
                return File.ReadAllLines(_savefile_path);
            }

            ShowStatusMessages("i", "message_decoding_save_file");

            LogWriter("Decoding file");

            byte[] buffer = new byte[bytes.Length - 0x38];

            byte[] keyBytes = new byte[] {
                0x2a, 0x5f, 0xcb, 0x17, 0x91, 210, 0x2f, 0xb6, 2, 0x45, 0xb3, 0xd8, 0x36, 0x9e, 0xd0, 0xb2,
                0xc2, 0x73, 0x71, 0x56, 0x3f, 0xbf, 0x1f, 60, 0x9e, 0xdf, 0x6b, 0x11, 130, 90, 0x5d, 10
                };

            byte[] destinationArray = new byte[0x10];

            Array.Copy(bytes, 0x38, buffer, 0, buffer.Length);
            Array.Copy(bytes, 0x24, destinationArray, 0, destinationArray.Length);

            try
            {
                byte[] bytebuffer = AESDecrypt(buffer, keyBytes, destinationArray);
                string input = "";

                using (MemoryStream stream = new MemoryStream(bytebuffer))
                {
                    using (InflaterInputStream stream2 = new InflaterInputStream(stream))
                    {
                        using (StreamReader reader = new StreamReader(stream2))
                        {
                            input = reader.ReadToEnd();
                        }
                    }
                }

                LogWriter("File decoded. Checking file format");

                if (input.StartsWith("BSII"))
                {
                    string arguments = "\"" + _savefile_path + "\" \"" + _savefile_path + "\"";

                    LogWriter("Starting SiiDecrypt");
                    Process SiiDecryptProcess = Process.Start(@"libs\SII_Decrypt.exe", arguments);

                    LogWriter("SiiDecrypt started with parameters: " + arguments);
                    SiiDecryptProcess.WaitForExit();
                    FileDecoded = true;

                    return File.ReadAllLines(_savefile_path);
                }

                LogWriter("Save file was decrypted properly");
                FileDecoded = true;

                return Regex.Split(input, "\r\n|\r|\n");
            }
            catch
            {
                LogWriter("Could not decode file: " + _savefile_path);
                ShowStatusMessages("e", "error_could_not_decode_file");

                return null;
            }
        }

        public static string FromHexToString(string hex)
        {
            try
            {
                byte[] raw = new byte[hex.Length / 2];
                for (int i = 0; i < raw.Length; i++)
                {
                    raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
                }

                return Encoding.UTF8.GetString(raw); //ASCII
            }
            catch
            {
                return null;
            }
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

        private static byte[] AESDecrypt(byte[] _encryptedData, byte[] _keyBytes, byte[] _iv)
        {
            RijndaelManaged managed = new RijndaelManaged
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.None,
                IV = _iv,
                KeySize = 0x80,
                BlockSize = 0x80,
                Key = _keyBytes
            };
            return managed.CreateDecryptor().TransformFinalBlock(_encryptedData, 0, _encryptedData.Length);
        }

    }
}