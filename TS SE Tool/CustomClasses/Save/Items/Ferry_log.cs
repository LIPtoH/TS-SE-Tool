using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS_SE_Tool.Save.Items
{
    class Ferry_log
    {
        internal List<string> entries { get; set; } = new List<string>();

        internal Ferry_log()
        { }

        internal Ferry_log(string[] _input)
        {
            string tagLine = "", dataLine = "";

            foreach (string currentLine in _input)
            {
                if (currentLine.Contains(':'))
                {
                    string[] splittedLine = currentLine.Split(new char[] { ':' }, 2);

                    tagLine = splittedLine[0].Trim();
                    dataLine = splittedLine[1].Trim();
                }
                else
                {
                    tagLine = currentLine.Trim();
                    dataLine = "";
                }

                switch (tagLine)
                {
                    case "":
                        {
                            break;
                        }

                    case "entries":
                        {
                            entries.Capacity = int.Parse(dataLine);
                            break;
                        }

                    case var s when s.StartsWith("entries["):
                        {
                            entries.Add(dataLine);
                            break;
                        }

                }
            }
        }

        internal string PrintOut(uint _version, string _nameless)
        {
            string returnString = "";

            StringBuilder returnSB = new StringBuilder();

            returnSB.AppendLine("ferry_log : " + _nameless + " {");

            returnSB.AppendLine(" entries: " + entries.Count);
            for (int i = 0; i < entries.Count; i++)
                returnSB.AppendLine(" entries[" + i + "]: " + entries[i]);

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            return returnString;
        }
    }
}