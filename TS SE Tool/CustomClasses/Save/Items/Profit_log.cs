using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS_SE_Tool.Save.Items
{
    class Profit_log
    {
        internal List<string> stats_data { get; set; } = new List<string>();

        internal int acc_distance_free { get; set; } = 0;
        internal int acc_distance_on_job { get; set; } = 0;
        internal int? history_age { get; set; } = null;

        internal Profit_log()
        { }

        internal Profit_log(string[] _input)
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

                        case "stats_data":
                            {
                                stats_data.Capacity = int.Parse(dataLine);
                                break;
                            }

                        case var s when s.StartsWith("stats_data["):
                            {
                                stats_data.Add(dataLine);
                                break;
                            }

                        case "acc_distance_free":
                            {
                                acc_distance_free = int.Parse(dataLine);
                                break;
                            }

                        case "acc_distance_on_job":
                            {
                                acc_distance_on_job = int.Parse(dataLine);
                                break;
                            }

                        case "history_age":
                            {
                                history_age = dataLine == "nil" ? (int?)null : int.Parse(dataLine);
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

            returnSB.AppendLine("profit_log : " + _nameless + " {");

            returnSB.AppendLine(" stats_data: " + stats_data.Count);
            for (int i = 0; i < stats_data.Count; i++)
                returnSB.AppendLine(" stats_data[" + i + "]: " + stats_data[i]);

            returnSB.AppendLine(" acc_distance_free: " + acc_distance_free.ToString());
            returnSB.AppendLine(" acc_distance_on_job: " + acc_distance_on_job.ToString());

            returnSB.AppendLine(" history_age: " + (history_age == null ? "nil" : history_age.ToString()));

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            return returnString;
        }
    }
}