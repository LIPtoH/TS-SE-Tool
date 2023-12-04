using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS_SE_Tool.Utilities;

namespace TS_SE_Tool.Save.Items
{
    class Trajectory_orders_Save : SiiNBlockCore
    {
        internal List<bool> handled_array { get; set; } = new List<bool>();

        internal List<int> stage_array { get; set; } = new List<int>();

        internal UInt64 trajectory_uid { get; set; } = 0;


        internal Trajectory_orders_Save()
        { }

        internal Trajectory_orders_Save(string[] _input)
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
                        case "trajectory_orders_save":
                        case "}":
                            {
                                break;
                            }

                        case "handled_array":
                            {
                                handled_array.Capacity = int.Parse(dataLine);
                                break;
                            }

                        case var s when s.StartsWith("handled_array["):
                            {
                                handled_array.Add(bool.Parse(dataLine));
                                break;
                            }

                        case "stage_array":
                            {
                                stage_array.Capacity = int.Parse(dataLine);
                                break;
                            }

                        case var s when s.StartsWith("stage_array["):
                            {
                                stage_array.Add(int.Parse(dataLine));
                                break;
                            }

                        case "trajectory_uid":
                            {
                                trajectory_uid = UInt64.Parse(dataLine);
                                break;
                            }

                        default:
                            {
                                UnidentifiedLines.Add(dataLine);
                                IO_Utilities.ErrorLogWriter(WriteErrorMsg(tagLine, dataLine));
                                break;
                            }
                    }
                }
                catch (Exception ex)
                {
                    IO_Utilities.ErrorLogWriter(WriteErrorMsg(ex.Message, tagLine, dataLine));
                    break;
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

            returnSB.AppendLine("trajectory_orders_save : " + _nameless + " {");

            returnSB.AppendLine(" handled_array: " + handled_array.Count);
            for (int i = 0; i < handled_array.Count; i++)
                returnSB.AppendLine(" handled_array[" + i + "]: " + handled_array[i].ToString().ToLower());

            returnSB.AppendLine(" stage_array: " + stage_array.Count);
            for (int i = 0; i < stage_array.Count; i++)
                returnSB.AppendLine(" stage_array[" + i + "]: " + stage_array[i]);

            returnSB.AppendLine(" trajectory_uid: " + trajectory_uid.ToString());

            WriteUnidentifiedLines();

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            this.removeWritenBlock(_nameless);

            return returnString;
        }
    }
}
