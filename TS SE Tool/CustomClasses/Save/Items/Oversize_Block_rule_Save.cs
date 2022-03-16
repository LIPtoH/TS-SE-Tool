using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS_SE_Tool.Save.Items
{
    internal class Oversize_Block_rule_Save
    {
        internal int escort_char_type = 0;

        internal UInt64 parent_trajectory_uid = 0;

        internal int parent_trajectory_idx = 0;

        internal Oversize_Block_rule_Save()
        { }

        internal Oversize_Block_rule_Save(string[] _input)
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

                        case "escort_char_type":
                            {
                                escort_char_type = int.Parse(dataLine);
                                break;
                            }

                        case "parent_trajectory_uid":
                            {
                                parent_trajectory_uid = UInt64.Parse(dataLine);
                                break;
                            }

                        case "parent_trajectory_idx":
                            {
                                parent_trajectory_idx = int.Parse(dataLine);
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

            returnSB.AppendLine("oversize_block_rule_save : " + _nameless + " {");

            returnSB.AppendLine(" escort_char_type: " + escort_char_type.ToString());
            returnSB.AppendLine(" parent_trajectory_uid: " + parent_trajectory_uid.ToString());
            returnSB.AppendLine(" parent_trajectory_idx: " + parent_trajectory_idx.ToString());

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            return returnString;
        }
    }
}
