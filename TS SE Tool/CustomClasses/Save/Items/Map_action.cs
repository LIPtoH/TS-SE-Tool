using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TS_SE_Tool.Save.DataFormat;

namespace TS_SE_Tool.Save.Items
{
    class Map_action
    {
        internal List<UInt64> id_params { get; set; } = new List<UInt64>();

        internal SCS_String name { get; set; } = "";

        internal string command { get; set; } = "";

        internal List<SCS_Float> num_params { get; set; } = new List<SCS_Float>();

        internal List<string> str_params { get; set; } = new List<string>();

        internal int target_tags { get; set; } = 0;

        internal int target_range { get; set; } = 0;

        internal string type { get; set; } = "";

        internal Map_action()
        { }

        internal Map_action(string[] _input)
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

                    case "id_params":
                        {
                            id_params.Capacity = int.Parse(dataLine);
                            break;
                        }

                    case var s when s.StartsWith("id_params["):
                        {
                            id_params.Add(UInt64.Parse( dataLine));
                            break;
                        }

                    case "name":
                        {
                            name = dataLine;
                            break;
                        }

                    case "command":
                        {
                            command = dataLine;
                            break;
                        }

                    case "num_params":
                        {
                            num_params.Capacity = int.Parse(dataLine);
                            break;
                        }

                    case var s when s.StartsWith("num_params["):
                        {
                            num_params.Add(dataLine);
                            break;
                        }

                    case "str_params":
                        {
                            str_params.Capacity = int.Parse(dataLine);
                            break;
                        }

                    case var s when s.StartsWith("str_params["):
                        {
                            str_params.Add(dataLine);
                            break;
                        }

                    case "target_tags":
                        {
                            target_tags = int.Parse(dataLine);
                            break;
                        }

                    case "target_range":
                        {
                            target_range = int.Parse(dataLine);
                            break;
                        }

                    case "type":
                        {
                            type = dataLine;
                            break;
                        }

                }
            }
        }

        internal string PrintOut(uint _version, string _nameless)
        {
            string returnString = "";

            StringBuilder returnSB = new StringBuilder();

            returnSB.AppendLine("map_action : " + _nameless + " {");

            returnSB.AppendLine(" id_params: " + id_params.Count);
            for (int i = 0; i < id_params.Count; i++)
                returnSB.AppendLine(" id_params[" + i + "]: " + id_params[i].ToString());

            returnSB.AppendLine(" name: " + name.ToString());

            returnSB.AppendLine(" command: " + command);

            returnSB.AppendLine(" num_params: " + num_params.Count);
            for (int i = 0; i < num_params.Count; i++)
                returnSB.AppendLine(" num_params[" + i + "]: " + num_params[i].ToString());

            returnSB.AppendLine(" str_params: " + str_params.Count);
            for (int i = 0; i < str_params.Count; i++)
                returnSB.AppendLine(" str_params[" + i + "]: " + str_params[i]);

            returnSB.AppendLine(" target_tags: " + target_tags.ToString());
            returnSB.AppendLine(" target_range: " + target_range.ToString());

            returnSB.AppendLine(" type: " + type);

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            return returnString;
        }
    }
}