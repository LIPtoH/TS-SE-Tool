using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS_SE_Tool.Save.Items
{
    class Job_Info
    {
        internal string cargo { get; set; } = "";
        internal string source_company { get; set; } = "";
        internal string target_company { get; set; } = "";

        internal int cargo_model_index { get; set; } = 0;

        internal bool is_articulated { get; set; } = false;
        internal bool is_cargo_market_job { get; set; } = false;

        internal int start_time { get; set; } = 0;
        internal int planned_distance_km { get; set; } = 0;
        internal int ferry_time { get; set; } = 0;
        internal int ferry_price { get; set; } = 0;
        internal int? urgency { get; set; } = 0;

        internal string special { get; set; } = "";

        internal int units_count { get; set; } = 0;
        internal int fill_ratio { get; set; } = 0;

        internal Job_Info()
        { }

        internal Job_Info(string[] _input)
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

                        case "cargo":
                            {
                                cargo = dataLine;
                                break;
                            }

                        case "source_company":
                            {
                                source_company = dataLine;
                                break;
                            }

                        case "target_company":
                            {
                                target_company = dataLine;
                                break;
                            }

                        case "cargo_model_index":
                            {
                                cargo_model_index = int.Parse(dataLine);
                                break;
                            }

                        case "is_articulated":
                            {
                                is_articulated = bool.Parse(dataLine);
                                break;
                            }

                        case "is_cargo_market_job":
                            {
                                is_cargo_market_job = bool.Parse(dataLine);
                                break;
                            }

                        case "start_time":
                            {
                                start_time = int.Parse(dataLine);
                                break;
                            }

                        case "planned_distance_km":
                            {
                                planned_distance_km = int.Parse(dataLine);
                                break;
                            }

                        case "ferry_time":
                            {
                                ferry_time = int.Parse(dataLine);
                                break;
                            }

                        case "ferry_price":
                            {
                                ferry_price = int.Parse(dataLine);
                                break;
                            }

                        case "urgency":
                            {
                                urgency = dataLine == "nil" ? (int?)null : int.Parse(dataLine);
                                break;
                            }

                        case "special":
                            {
                                special = dataLine;
                                break;
                            }

                        case "units_count":
                            {
                                units_count = int.Parse(dataLine);
                                break;
                            }

                        case "fill_ratio":
                            {
                                fill_ratio = int.Parse(dataLine);
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

            returnSB.AppendLine("job_info : " + _nameless + " {");

            returnSB.AppendLine(" cargo: " + cargo);
            returnSB.AppendLine(" source_company: " + source_company);
            returnSB.AppendLine(" target_company: " + target_company);

            returnSB.AppendLine(" cargo_model_index: " + cargo_model_index.ToString());

            returnSB.AppendLine(" is_articulated: " + is_articulated.ToString().ToLower());
            returnSB.AppendLine(" is_cargo_market_job: " + is_cargo_market_job.ToString().ToLower());

            returnSB.AppendLine(" start_time: " + start_time.ToString());
            returnSB.AppendLine(" planned_distance_km: " + planned_distance_km.ToString());
            returnSB.AppendLine(" ferry_time: " + ferry_time.ToString());
            returnSB.AppendLine(" ferry_price: " + ferry_price.ToString());
            returnSB.AppendLine(" urgency: " + (urgency == null ? "nil" : urgency.ToString()));

            returnSB.AppendLine(" special: " + special);

            returnSB.AppendLine(" units_count: " + units_count.ToString());
            returnSB.AppendLine(" fill_ratio: " + fill_ratio.ToString());

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            return returnString;
        }
    }
}