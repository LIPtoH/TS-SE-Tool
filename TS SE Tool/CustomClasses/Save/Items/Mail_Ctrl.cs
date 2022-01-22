using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS_SE_Tool.Save.Items
{
    class Mail_Ctrl
    {
        internal List<string> inbox { get; set; } = new List<string>();

        internal int last_id { get; set; } = 0;

        internal int unread_count { get; set; } = 0;

        internal int pending_mails { get; set; } = 0;

        internal int pmail_timers { get; set; } = 0;

        internal Mail_Ctrl()
        { }

        internal Mail_Ctrl(string[] _input)
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

                        case "inbox":
                            {
                                inbox.Capacity = int.Parse(dataLine);
                                break;
                            }

                        case var s when s.StartsWith("inbox["):
                            {
                                inbox.Add(dataLine);
                                break;
                            }

                        case "last_id":
                            {
                                last_id = int.Parse(dataLine);
                                break;
                            }

                        case "unread_count":
                            {
                                unread_count = int.Parse(dataLine);
                                break;
                            }

                        case "pending_mails":
                            {
                                pending_mails = int.Parse(dataLine);
                                break;
                            }

                        case "pmail_timers":
                            {
                                pmail_timers = int.Parse(dataLine);
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

            returnSB.AppendLine("mail_ctrl : " + _nameless + " {");

            returnSB.AppendLine(" inbox: " + inbox.Count);
            for (int i = 0; i < inbox.Count; i++)
                returnSB.AppendLine(" inbox[" + i + "]: " + inbox[i]);

            returnSB.AppendLine(" last_id: " + last_id.ToString());

            returnSB.AppendLine(" unread_count: " + unread_count.ToString());

            returnSB.AppendLine(" pending_mails: " + pending_mails.ToString());

            returnSB.AppendLine(" pmail_timers: " + pmail_timers.ToString());

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            return returnString;
        }
    }
}