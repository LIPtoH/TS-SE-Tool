using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TS_SE_Tool.Save.DataFormat;

namespace TS_SE_Tool.Save.Items
{
    class Police_Ctrl
    {
        internal List<SCS_Float> offence_timer { get; set; } = new List<SCS_Float>();

        internal List<int> offence_counter { get; set; } = new List<int>();

        internal List<bool> offence_valid { get; set; } = new List<bool>();
        
        internal Police_Ctrl()
        { }

        internal Police_Ctrl(string[] _input)
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

                        case "offence_timer":
                            {
                                offence_timer.Capacity = int.Parse(dataLine);
                                break;
                            }

                        case var s when s.StartsWith("offence_timer["):
                            {
                                offence_timer.Add(dataLine);
                                break;
                            }

                        case "offence_counter":
                            {
                                offence_counter.Capacity = int.Parse(dataLine);
                                break;
                            }

                        case var s when s.StartsWith("offence_counter["):
                            {
                                offence_counter.Add(int.Parse(dataLine));
                                break;
                            }

                        case "offence_valid":
                            {
                                offence_valid.Capacity = int.Parse(dataLine);
                                break;
                            }

                        case var s when s.StartsWith("offence_valid["):
                            {
                                offence_valid.Add(bool.Parse(dataLine));
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

        internal string PrintOut(uint _version)
        {
            return PrintOut(_version, null);
        }
        internal string PrintOut(uint _version, string _nameless)
        {
            string returnString = "";

            StringBuilder returnSB = new StringBuilder();

            returnSB.AppendLine("police_ctrl : " + _nameless + " {");

            returnSB.AppendLine(" offence_timer: " + offence_timer.Count);
            for (int i = 0; i < offence_timer.Count; i++)
                returnSB.AppendLine(" offence_timer[" + i + "]: " + offence_timer[i].ToString());

            returnSB.AppendLine(" offence_counter: " + offence_counter.Count);
            for (int i = 0; i < offence_counter.Count; i++)
                returnSB.AppendLine(" offence_counter[" + i + "]: " + offence_counter[i].ToString());

            returnSB.AppendLine(" offence_valid: " + offence_valid.Count);
            for (int i = 0; i < offence_valid.Count; i++)
                returnSB.AppendLine(" offence_valid[" + i + "]: " + offence_valid[i].ToString().ToLower());

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            return returnString;
        }
    }
}