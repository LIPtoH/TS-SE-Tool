using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TS_SE_Tool.Utilities;
using TS_SE_Tool.Save.DataFormat;

namespace TS_SE_Tool.Save.Items
{
    class Vehicle_Drv_plate_Accessory
    {
        internal string text { get; set; } = "";

        internal List<string> slot_name { get; set; } = new List<string>();
        internal List<string> slot_hookup { get; set; } = new List<string>();

        internal string data_path { get; set; } = "";
        internal int refund { get; set; } = 0;


        internal Vehicle_Drv_plate_Accessory()
        { }

        internal Vehicle_Drv_plate_Accessory(string[] _input)
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

                    case "slot_name":
                        {
                            slot_name.Capacity = int.Parse(dataLine);
                            break;
                        }

                    case var s when s.StartsWith("slot_name["):
                        {
                            slot_name.Add(dataLine);
                            break;
                        }

                    case "slot_hookup":
                        {
                            slot_hookup.Capacity = int.Parse(dataLine);
                            break;
                        }

                    case var s when s.StartsWith("slot_hookup["):
                        {
                            slot_hookup.Add(dataLine);
                            break;
                        }

                    case "data_path":
                        {
                            data_path = dataLine;
                            break;
                        }

                    case "text":
                        {
                            text = dataLine;
                            break;
                        }

                    case "refund":
                        {
                            refund = int.Parse(dataLine);
                            break;
                        }
                }
            }
        }

        internal string PrintOut(uint _version, string _nameless)
        {
            string returnString = "";

            StringBuilder returnSB = new StringBuilder();

            returnSB.AppendLine("vehicle_drv_plate_accessory : " + _nameless + " {");

            returnSB.AppendLine(" text: " + text);

            returnSB.AppendLine(" slot_name: " + slot_name.ToString());
            returnSB.AppendLine(" slot_hookup: " + slot_hookup.ToString());

            returnSB.AppendLine(" data_path: " + data_path);
            returnSB.AppendLine(" refund: " + refund.ToString());

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            return returnString;
        }
    }
}