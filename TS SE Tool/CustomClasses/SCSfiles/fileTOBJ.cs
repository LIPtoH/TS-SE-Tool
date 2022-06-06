using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TS_SE_Tool.SCSfiles
{
    internal class fileTOBJ
    {
        internal string texture_path = "";

        internal fileTOBJ(string _filepath)
        {
            if (File.Exists(_filepath))
            {
                Int16 stringSize = 0;

                int readerPos = 0x28;
                int readerSize = 2;

                stringSize = BitConverter.ToInt16(reader(), 0);

                readerPos = 0x30;
                readerSize = stringSize;

                texture_path = Encoding.ASCII.GetString(reader());

                byte[] reader()
                {
                    using (BinaryReader b = new BinaryReader(File.Open(_filepath, FileMode.Open)))
                    {
                        // Seek to our required position.
                        b.BaseStream.Seek(readerPos, SeekOrigin.Begin);

                        return b.ReadBytes(readerSize);
                    }
                }

            }
        }
    }
}
