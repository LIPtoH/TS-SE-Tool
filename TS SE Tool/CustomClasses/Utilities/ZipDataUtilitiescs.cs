using System;
using System.IO;
using ICSharpCode.SharpZipLib.GZip;

namespace TS_SE_Tool.Utilities
{
    public class ZipDataUtilitiescs
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

            for (int j = 0; j < pairs.Length; j++)
                bytes[j] = Convert.ToByte(pairs[j], 16);

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
    }
}
