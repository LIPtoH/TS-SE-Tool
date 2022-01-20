using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TS_SE_Tool.Save.DataFormat;

namespace TS_SE_Tool.Save.Items
{
    class Mail_Def
    {
        internal int id { get; set; } = 0;

        internal SCS_String mail_text_ref { get; set; } = "";

        internal List<SCS_String> param_keys { get; set; } = new List<SCS_String>();
        internal List<SCS_String> param_values { get; set; } = new List<SCS_String>();

        internal bool read { get; set; } = false;
        internal bool accepted { get; set; } = false;
        internal bool expired { get; set; } = false;

        internal int custom_data { get; set; } = 0;

        internal Mail_Def()
        { }

        internal Mail_Def(string[] _input)
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

                switch (tagLine)
                {
                    case "":
                        {
                            break;
                        }

                    case "id":
                        {
                            id = int.Parse(dataLine);
                            break;
                        }

                    case "mail_text_ref":
                        {
                            mail_text_ref = dataLine;
                            break;
                        }

                    case "param_keys":
                        {
                            param_keys.Capacity = int.Parse(dataLine);
                            break;
                        }

                    case var s when s.StartsWith("param_keys["):
                        {
                            param_keys.Add(dataLine);
                            break;
                        }

                    case "param_values":
                        {
                            param_values.Capacity = int.Parse(dataLine);
                            break;
                        }

                    case var s when s.StartsWith("param_values["):
                        {
                            param_values.Add(dataLine);
                            break;
                        }

                    case "read":
                        {
                            read = bool.Parse(dataLine);
                            break;
                        }

                    case "accepted":
                        {
                            accepted = bool.Parse(dataLine);
                            break;
                        }

                    case "expired":
                        {
                            expired = bool.Parse(dataLine);
                            break;
                        }

                    case "custom_data":
                        {
                            custom_data = int.Parse(dataLine);
                            break;
                        }
                }
            }
        }

        internal string PrintOut(uint _version, string _nameless)
        {
            string returnString = "";

            StringBuilder returnSB = new StringBuilder();

            returnSB.AppendLine("mail_def : " + _nameless + " {");

            returnSB.AppendLine(" id: " + id.ToString());

            returnSB.AppendLine(" mail_text_ref: " + mail_text_ref.ToString());

            returnSB.AppendLine(" param_keys: " + param_keys.Count);
            for (int i = 0; i < param_keys.Count; i++)
                returnSB.AppendLine(" param_keys[" + i + "]: " + param_keys[i].ToString());

            returnSB.AppendLine(" param_values: " + param_values.Count);
            for (int i = 0; i < param_values.Count; i++)
                returnSB.AppendLine(" param_values[" + i + "]: " + param_values[i].ToString());

            returnSB.AppendLine(" read: " + read.ToString().ToLower());
            returnSB.AppendLine(" accepted: " + accepted.ToString().ToLower());
            returnSB.AppendLine(" expired: " + expired.ToString().ToLower());

            returnSB.AppendLine(" custom_data: " + custom_data.ToString());

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            return returnString;
        }
    }
}