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
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using ICSharpCode.SharpZipLib.GZip;

namespace TS_SE_Tool.Utilities
{
    public class ZipDataUtilities
    {
        internal static byte[] zipText(string _text)
        {
            if (_text == null)
                return null;

            using (Stream memOutput = new MemoryStream())            
                using (GZipOutputStream zipOut = new GZipOutputStream(memOutput))                
                    using (StreamWriter writer = new StreamWriter(zipOut))
                    {
                        writer.Write(_text);

                        writer.Flush();
                        zipOut.Finish();

                        byte[] bytes = new byte[memOutput.Length];
                        memOutput.Seek(0, SeekOrigin.Begin);
                        memOutput.Read(bytes, 0, bytes.Length);

                        return bytes;
                    }


        }

        internal static string unzipText(string _sbytes)
        {
            string[] pairs = new string[_sbytes.Length / 2];
            byte[] bytes;

            for (int i = 0; i < _sbytes.Length / 2; i++)
            {
                pairs[i] = _sbytes.Substring(i * 2, 2);
            }

            bytes = new byte[pairs.Length];

            try
            {
                for (int j = 0; j < pairs.Length; j++)
                    bytes[j] = Convert.ToByte(pairs[j], 16);
            }
            catch
            {
                return null;
            }

            if (bytes == null)
                return null;

            using (Stream memInput = new MemoryStream(bytes))
                using (GZipInputStream zipInput = new GZipInputStream(memInput))
                    using (StreamReader reader = new StreamReader(zipInput))
                    {
                        string text = reader.ReadToEnd();

                        return text;
                    }
        }

        internal static void makeZipArchive(string _zipFilePath, List<string> _filesToZip)
        {
            using (MemoryStream zipMS = new MemoryStream())
            {
                using (ZipArchive zipArchive = new ZipArchive(zipMS, ZipArchiveMode.Create, true))
                {
                    //loop through files to add
                    foreach (string file in _filesToZip)
                    {
                        //read the file bytes
                        byte[] fileToZipBytes = File.ReadAllBytes(file);

                        //create the entry - this is the zipped filename
                        ZipArchiveEntry zipFileEntry = zipArchive.CreateEntry(Path.GetFileName(file));

                        //add the file contents
                        using (Stream zipEntryStream = zipFileEntry.Open())
                        using (BinaryWriter zipFileBinary = new BinaryWriter(zipEntryStream))
                        {
                            zipFileBinary.Write(fileToZipBytes);
                        }
                    }
                }

                using (FileStream finalZipFileStream = new FileStream(_zipFilePath, FileMode.Create))
                {
                    zipMS.Seek(0, SeekOrigin.Begin);
                    zipMS.CopyTo(finalZipFileStream);
                }
            }
        }

        internal static void extractListZipArchive(string _zipFilePath, string _extractPath, List<string> _filesToExtract)
        {
            using (ZipArchive archive = ZipFile.OpenRead(_zipFilePath))
            {
                foreach(string _tmpFile in _filesToExtract)
                {
                    ZipArchiveEntry entry = archive.GetEntry(_tmpFile);

                    entry.ExtractToFile(_extractPath + "\\" + _tmpFile);
                }
            }
        }
    }
}
