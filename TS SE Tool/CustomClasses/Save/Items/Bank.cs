using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TS_SE_Tool.Utilities;
using TS_SE_Tool.Save.DataFormat;

namespace TS_SE_Tool.Save.Items
{
    class Bank
    {
        internal Int64 money_account { get; set; } = 0;

        internal int coinsurance_fixed { get; set; } = 0;

        internal SCS_Float coinsurance_ratio { get; set; } = 0;

        internal SCS_Float accident_severity { get; set; } = 0;

        internal List<string> loans { get; set; } = new List<string>();

        internal bool app_enabled { get; set; } = false;

        internal int loan_limit { get; set; } = 0;

        internal SCS_Float payment_timer { get; set; } = 0;

        internal bool overdraft { get; set; } = false;

        internal int overdraft_timer { get; set; } = 0;

        internal int overdraft_warn_count { get; set; } = 0;

        internal bool sell_players_truck_later { get; set; } = false;

        internal bool sell_players_trailer_later { get; set; } = false;

        internal Bank()
        { }

        internal Bank(string[] _input)
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

                        case "money_account":
                            {
                                money_account = Int64.Parse(dataLine);
                                break;
                            }

                        case "coinsurance_fixed":
                            {
                                coinsurance_fixed = int.Parse(dataLine);
                                break;
                            }

                        case "coinsurance_ratio":
                            {
                                coinsurance_ratio = dataLine;
                                break;
                            }

                        case "accident_severity":
                            {
                                accident_severity = dataLine;
                                break;
                            }

                        case "loans":
                            {
                                loans.Capacity = int.Parse(dataLine);
                                break;
                            }

                        case var s when s.StartsWith("loans["):
                            {
                                loans.Add(dataLine);
                                break;
                            }

                        case "app_enabled":
                            {
                                app_enabled = bool.Parse(dataLine);
                                break;
                            }

                        case "loan_limit":
                            {
                                loan_limit = int.Parse(dataLine);
                                break;
                            }

                        case "payment_timer":
                            {
                                payment_timer = dataLine;
                                break;
                            }

                        case "overdraft":
                            {
                                overdraft = bool.Parse(dataLine);
                                break;
                            }

                        case "overdraft_timer":
                            {
                                overdraft_timer = int.Parse(dataLine);
                                break;
                            }

                        case "overdraft_warn_count":
                            {
                                overdraft_warn_count = int.Parse(dataLine);
                                break;
                            }

                        case "sell_players_truck_later":
                            {
                                sell_players_truck_later = bool.Parse(dataLine);
                                break;
                            }

                        case "sell_players_trailer_later":
                            {
                                sell_players_trailer_later = bool.Parse(dataLine);
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

            returnSB.AppendLine("bank : " + _nameless + " {");
            returnSB.AppendLine(" money_account: " + money_account);
            returnSB.AppendLine(" coinsurance_fixed: " + coinsurance_fixed);
            returnSB.AppendLine(" coinsurance_ratio: " + coinsurance_ratio.ToString());
            returnSB.AppendLine(" accident_severity: " + accident_severity.ToString());

            returnSB.AppendLine(" loans: " + loans.Count);
            for (int i = 0; i < loans.Count; i++)
                returnSB.AppendLine(" loans[" + i + "]: " + loans[i]);

            returnSB.AppendLine(" app_enabled: " + app_enabled.ToString().ToLower());
            returnSB.AppendLine(" loan_limit: " + loan_limit);
            returnSB.AppendLine(" payment_timer: " + payment_timer.ToString());
            returnSB.AppendLine(" overdraft: " + overdraft.ToString().ToLower());
            returnSB.AppendLine(" overdraft_timer: " + overdraft_timer.ToString());
            returnSB.AppendLine(" overdraft_warn_count: " + overdraft_warn_count.ToString());
            returnSB.AppendLine(" sell_players_truck_later: " + sell_players_truck_later.ToString().ToLower());
            returnSB.AppendLine(" sell_players_trailer_later: " + sell_players_trailer_later.ToString().ToLower());

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            return returnString;
        }
    }
}
