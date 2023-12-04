using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TS_SE_Tool.Utilities;
using TS_SE_Tool.Save.DataFormat;
using TS_SE_Tool.CustomClasses.Global;

namespace TS_SE_Tool.Save.Items
{
    class Bank_Loan : SiiNBlockCore
    {
        internal int amount { get; set; } = 0;

        internal int original_amount { get; set; } = 0;

        internal int time_stamp { get; set; } = 0;

        internal SCS_Float interest_rate { get; set; } = 0;

        internal int duration { get; set; } = 0;

        internal Bank_Loan()
        { }

        internal Bank_Loan(string[] _input)
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
                        case "bank_loan":
                        case "}":
                            {
                                break;
                            }

                        case "amount":
                            {
                                amount = int.Parse(dataLine);
                                break;
                            }

                        case "original_amount":
                            {
                                original_amount = int.Parse(dataLine);
                                break;
                            }

                        case "time_stamp":
                            {
                                time_stamp = int.Parse(dataLine);
                                break;
                            }

                        case "interest_rate":
                            {
                                interest_rate = dataLine;
                                break;
                            }

                        case "duration":
                            {
                                duration = int.Parse(dataLine);
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

            returnSB.AppendLine("bank_loan : " + _nameless + " {");

            returnSB.AppendLine(" amount: " + amount);
            returnSB.AppendLine(" original_amount: " + original_amount);
            returnSB.AppendLine(" time_stamp: " + time_stamp);
            returnSB.AppendLine(" interest_rate: " + interest_rate.ToString());
            returnSB.AppendLine(" duration: " + duration);

            WriteUnidentifiedLines();

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            this.removeWritenBlock(_nameless);

            return returnString;
        }
    }
}
