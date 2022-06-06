using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TS_SE_Tool.SCSfiles
{
    internal class fileMAT
    {
        internal string type = "";

        internal string texture = "";

        internal string texture_name = "";

        internal fileMAT(string _filepath)
        {
            if (File.Exists(_filepath))
            {
                using (StreamReader reader = new StreamReader(_filepath))
                {
                    string text = reader.ReadToEnd();

                    string[] _input = text.Split(new string[] { Environment.NewLine, "\n", "\r" }, StringSplitOptions.None);

                    processFile(_input);
                }
            }
        }

        internal fileMAT(string[] _input)
        {
            processFile(_input);
        }

        void processFile(string[] _input)
        {
            string tagLine = "", dataLine = "";

            //block decoding
            for (int line = 0; line < _input.Length; line++)
            {
                string currentLine = _input[line];

                if (currentLine.Contains(':'))
                {
                    string[] splittedLine = currentLine.Split(new char[] { ':' }, 2);

                    tagLine = splittedLine[0].Trim();
                    dataLine = splittedLine[1].Trim().Trim(new char[] { '"' });
                }
                else
                {
                    continue;
                }

                switch (tagLine)
                {
                    case "":
                    case "{":
                    case "}":
                        {
                            break;
                        }

                    case "material":
                        {
                            type = dataLine;

                            break;
                        }

                    case "texture":
                        {
                            texture = dataLine;

                            break;
                        }

                    case "texture_name":
                        {
                            texture_name = dataLine;

                            break;
                        }

                }
            }
        }
    }
}