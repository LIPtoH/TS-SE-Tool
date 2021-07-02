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
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;

namespace TS_SE_Tool
{
    public partial class FormMain
    {
        public unsafe string[] NewDecodeFile(string _savefile_path)
        {
            return NewDecodeFile(_savefile_path, true);
        }

        public unsafe string[] NewDecodeFile(string _savefile_path, bool _verbose)
        {
            if (_verbose)
                UpdateStatusBarMessage.ShowStatusMessage(SMStatus.Info, "message_loading_save_file");
            if(_verbose)
                LogWriter("Loading file into memory: " + _savefile_path);

            //string FileData = "";
            byte[] FileDataB = new byte[10];

            try
            {
                FileDataB = File.ReadAllBytes(_savefile_path);
            }
            catch
            {
                LogWriter("Could not find file in: " + _savefile_path);
                UpdateStatusBarMessage.ShowStatusMessage(SMStatus.Error, "error_could_not_find_file");

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
                        if (_verbose)
                            UpdateStatusBarMessage.ShowStatusMessage(SMStatus.Info, "message_decoding_save_file");
                        if (_verbose)
                            LogWriter("Decoding file: " + _savefile_path);

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
                            if (_verbose)
                                UpdateStatusBarMessage.ShowStatusMessage(SMStatus.Clear);

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
                        if (_verbose)
                            UpdateStatusBarMessage.ShowStatusMessage(SMStatus.Info, "message_decoding_save_file");
                        if (_verbose)
                            LogWriter("Decoding file: " + _savefile_path);

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
                            if (_verbose)
                                UpdateStatusBarMessage.ShowStatusMessage(SMStatus.Clear);

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