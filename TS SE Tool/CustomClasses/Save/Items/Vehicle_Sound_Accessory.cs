using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TS_SE_Tool.Utilities;
using TS_SE_Tool.Save.DataFormat;

namespace TS_SE_Tool.Save.Items
{
    class Vehicle_Sound_Accessory : SiiNBlockCore
    {
        internal string data_path { get; set; } = "";
        internal uint refund { get; set; } = 0;

        internal Vehicle_Sound_Accessory()
        { }

        internal Vehicle_Sound_Accessory(string[] _input)
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

                        case "data_path":
                            {
                                data_path = dataLine;
                                break;
                            }

                        case "refund":
                            {
                                refund = uint.Parse(dataLine);
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

            returnSB.AppendLine("vehicle_sound_accessory : " + _nameless + " {");

            returnSB.AppendLine(" data_path: " + data_path);
            returnSB.AppendLine(" refund: " + refund.ToString());

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            this.removeWritenBlock(_nameless);

            return returnString;
        }

        public override string ToString()
        {
            return base.ToString().Split(new char[] { '.' })[3];
        }
    }
}