using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS_SE_Tool.Save.DataFormat;

namespace TS_SE_Tool.Save.Items
{
    class Mail_Ctrl : SiiNBlockCore
    {
        internal List<string> inbox { get; set; } = new List<string>();

        internal int last_id { get; set; } = 0;

        internal int unread_count { get; set; } = 0;

        internal List<string> pending_mails { get; set; } = new List<string>();

        internal List<SCS_Float> pmail_timers { get; set; } = new List<SCS_Float>();

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
                                pending_mails.Capacity = int.Parse(dataLine);
                                break;
                            }

                        case var s when s.StartsWith("pending_mails["):
                            {
                                pending_mails.Add(dataLine);
                                break;
                            }

                        case "pmail_timers":
                            {
                                pmail_timers.Capacity = int.Parse(dataLine);
                                break;
                            }

                        case var s when s.StartsWith("pmail_timers["):
                            {
                                pmail_timers.Add(dataLine);
                                break;
                            }
                    }
                }
                catch (Exception ex)
                {
                    UnidentifiedLines.Add(currentLine);
                    Utilities.IO_Utilities.ErrorLogWriter(ex.Message + Environment.NewLine + this.GetType().Name.ToLower() + " | " + tagLine + " = " + dataLine);
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

            returnSB.AppendLine("mail_ctrl : " + _nameless + " {");

            returnSB.AppendLine(" inbox: " + inbox.Count);
            for (int i = 0; i < inbox.Count; i++)
                returnSB.AppendLine(" inbox[" + i + "]: " + inbox[i]);

            returnSB.AppendLine(" last_id: " + last_id.ToString());

            returnSB.AppendLine(" unread_count: " + unread_count.ToString());

            returnSB.AppendLine(" pending_mails: " + pending_mails.Count);
            for (int i = 0; i < pending_mails.Count; i++)
                returnSB.AppendLine(" pending_mails[" + i + "]: " + pending_mails[i]);

            returnSB.AppendLine(" pmail_timers: " + pmail_timers.Count);
            for (int i = 0; i < pmail_timers.Count; i++)
                returnSB.AppendLine(" pmail_timers[" + i + "]: " + pmail_timers[i].ToString());

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            this.removeWritenBlock(_nameless);

            return returnString;
        }
    }
}