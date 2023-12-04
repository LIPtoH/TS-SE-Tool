using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS_SE_Tool.Utilities;

namespace TS_SE_Tool.Save.Items
{
    class Profit_log_Entry : SiiNBlockCore
    {
        internal int revenue { get; set; } = 0;
        internal int wage { get; set; } = 0;
        internal int maintenance { get; set; } = 0;
        internal int fuel { get; set; } = 0;
        internal int distance { get; set; } = 0;

        internal bool distance_on_job { get; set; } = false;

        internal int cargo_count { get; set; } = 0;

        internal string cargo { get; set; } = "";
        internal string source_city { get; set; } = "";
        internal string source_company { get; set; } = "";
        internal string destination_city { get; set; } = "";
        internal string destination_company { get; set; } = "";

        internal int timestamp_day { get; set; } = 0;

        internal Profit_log_Entry()
        { }

        internal Profit_log_Entry(string[] _input)
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
                        case "profit_log_entry":
                        case "}":
                            {
                                break;
                            }

                        case "revenue":
                            {
                                revenue = int.Parse(dataLine);
                                break;
                            }

                        case "wage":
                            {
                                wage = int.Parse(dataLine);
                                break;
                            }

                        case "maintenance":
                            {
                                maintenance = int.Parse(dataLine);
                                break;
                            }

                        case "fuel":
                            {
                                fuel = int.Parse(dataLine);
                                break;
                            }

                        case "distance":
                            {
                                distance = int.Parse(dataLine);
                                break;
                            }

                        case "distance_on_job":
                            {
                                distance_on_job = bool.Parse(dataLine);
                                break;
                            }

                        case "cargo_count":
                            {
                                cargo_count = int.Parse(dataLine);
                                break;
                            }

                        case "cargo":
                            {
                                cargo = dataLine;
                                break;
                            }

                        case "source_city":
                            {
                                source_city = dataLine;
                                break;
                            }

                        case "source_company":
                            {
                                source_company = dataLine;
                                break;
                            }

                        case "destination_city":
                            {
                                destination_city = dataLine;
                                break;
                            }

                        case "destination_company":
                            {
                                destination_company = dataLine;
                                break;
                            }

                        case "timestamp_day":
                            {
                                timestamp_day = int.Parse(dataLine);
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

        internal string PrintOut(uint _version, string _nameless)
        {
            string returnString = "";

            StringBuilder returnSB = new StringBuilder();

            returnSB.AppendLine("profit_log_entry : " + _nameless + " {");

            returnSB.AppendLine(" revenue: " + revenue.ToString());
            returnSB.AppendLine(" wage: " + wage.ToString());
            returnSB.AppendLine(" maintenance: " + maintenance.ToString());
            returnSB.AppendLine(" fuel: " + fuel.ToString());
            returnSB.AppendLine(" distance: " + distance.ToString());

            returnSB.AppendLine(" distance_on_job: " + distance_on_job.ToString().ToLower());

            returnSB.AppendLine(" cargo_count: " + cargo_count.ToString());

            returnSB.AppendLine(" cargo: " + cargo);
            returnSB.AppendLine(" source_city: " + source_city);
            returnSB.AppendLine(" source_company: " + source_company);
            returnSB.AppendLine(" destination_city: " + destination_city);
            returnSB.AppendLine(" destination_company: " + destination_company);

            returnSB.AppendLine(" timestamp_day: " + timestamp_day.ToString());

            WriteUnidentifiedLines();

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            this.removeWritenBlock(_nameless);

            return returnString;
        }
    }
}