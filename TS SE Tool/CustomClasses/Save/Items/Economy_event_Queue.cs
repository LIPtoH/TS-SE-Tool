using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS_SE_Tool.Save.Items
{
    class Economy_event_Queue : SiiNBlockCore
    {
        internal List<string> data { get; set; } = new List<string>();

        internal Economy_event_Queue()
        { }

        internal Economy_event_Queue(string[] _input)
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

                    case "data":
                        {
                            data.Capacity = int.Parse(dataLine);
                            break;
                        }

                    case var s when s.StartsWith("data["):
                        {
                            data.Add(dataLine);
                            break;
                        }
                }
            }
        }

        internal string PrintOut(uint _version)
        {
            return PrintOut(_version, null);
        }
        internal string PrintOut(uint _version, string _nameless)
        {
            string returnString = "";

            StringBuilder returnSB = new StringBuilder();

            returnSB.AppendLine("economy_event_queue : " + _nameless + " {");

            returnSB.AppendLine(" data: " + data.Count);
            for (int i = 0; i < data.Count; i++)
                returnSB.AppendLine(" data[" + i + "]: " + data[i]);

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            this.removeWritenBlock(_nameless);

            return returnString;
        }
    }
}