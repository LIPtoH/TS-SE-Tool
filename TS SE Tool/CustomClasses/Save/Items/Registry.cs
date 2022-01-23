using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS_SE_Tool.Save.Items
{
    class Registry
    {
        internal List<int> data { get; set; } = new List<int>();

        internal List<bool> valid { get; set; } = new List<bool>();

        internal List<int> keys { get; set; } = new List<int>();

        internal List<int> index { get; set; } = new List<int>();


        internal Registry()
        { }

        internal Registry(string[] _input)
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
                try
                {
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
                                data.Add(int.Parse(dataLine));
                                break;
                            }

                        case "valid":
                            {
                                valid.Capacity = int.Parse(dataLine);
                                break;
                            }

                        case var s when s.StartsWith("valid["):
                            {
                                valid.Add(bool.Parse(dataLine));
                                break;
                            }

                        case "keys":
                            {
                                keys.Capacity = int.Parse(dataLine);
                                break;
                            }

                        case var s when s.StartsWith("keys["):
                            {
                                keys.Add(int.Parse(dataLine));
                                break;
                            }

                        case "index":
                            {
                                index.Capacity = int.Parse(dataLine);
                                break;
                            }

                        case var s when s.StartsWith("index["):
                            {
                                index.Add(int.Parse(dataLine));
                                break;
                            }

                    }
                }
                catch (Exception ex)
                {
                    Utilities.IO_Utilities.ErrorLogWriter(ex.Message + Environment.NewLine + this.GetType().Name.ToLower() + " | " + tagLine + " = " + dataLine);
                    break;
                }
            }
        }

        internal string PrintOut(uint _version, string _nameless)
        {
            string returnString = "";

            StringBuilder returnSB = new StringBuilder();

            returnSB.AppendLine("registry : " + _nameless + " {");

            returnSB.AppendLine(" data: " + data.Count);
            for (int i = 0; i < data.Count; i++)
                returnSB.AppendLine(" data[" + i + "]: " + data[i].ToString());

            returnSB.AppendLine(" valid: " + valid.Count);
            for (int i = 0; i < valid.Count; i++)
                returnSB.AppendLine(" valid[" + i + "]: " + valid[i].ToString().ToLower());

            returnSB.AppendLine(" keys: " + keys.Count);
            for (int i = 0; i < keys.Count; i++)
                returnSB.AppendLine(" keys[" + i + "]: " + keys[i].ToString());

            returnSB.AppendLine(" index: " + index.Count);
            for (int i = 0; i < index.Count; i++)
                returnSB.AppendLine(" index[" + i + "]: " + index[i].ToString());

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            return returnString;
        }
    }
}