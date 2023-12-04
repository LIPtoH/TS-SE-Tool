using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS_SE_Tool.Utilities;

namespace TS_SE_Tool.Save.Items
{
    internal class Oversize_Block_rule_Save : SiiNBlockCore
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
                        case "oversize_block_rule_save":
                        case "}":
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

            WriteUnidentifiedLines();

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            this.removeWritenBlock(_nameless);

            return returnString;
        }
    }
}
