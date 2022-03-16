using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TS_SE_Tool.Utilities;
using TS_SE_Tool.Save.DataFormat;

namespace TS_SE_Tool.Save.Items
{
    class Vehicle_Wheel_Accessory : SiiNBlockCore
    {

        internal int offset { get; set; } = 0;
        internal Vector_3f paint_color { get; set; } = new Vector_3f();

        internal string data_path { get; set; } = "";
        internal uint refund { get; set; } = 0;

        internal string accType { get; set; } = "generalpart";

        internal Vehicle_Wheel_Accessory()
        { }

        internal Vehicle_Wheel_Accessory(string[] _input)
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

                        case "offset":
                            {
                                offset = int.Parse(dataLine);
                                break;
                            }

                        case "paint_color":
                            {
                                paint_color = new Vector_3f(dataLine);
                                break;
                            }

                        case "data_path":
                            {
                                data_path = dataLine;

                                //Type
                                string pathString = data_path.Split(new char[] { '"' })[1];

                                switch (pathString)
                                {
                                    case var s when s.Contains("/f_tire/") || s.Contains("/r_tire/") || s.Contains("/f_wheel/") || s.Contains("/r_wheel/") || s.Contains("/t_wheel/"):
                                        accType = "tire";
                                        break;
                                }

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

            returnSB.AppendLine("vehicle_wheel_accessory : " + _nameless + " {");

            returnSB.AppendLine(" offset: " + offset.ToString());
            returnSB.AppendLine(" paint_color: " + paint_color.ToString());

            returnSB.AppendLine(" data_path: " + data_path);
            returnSB.AppendLine(" refund: " + refund.ToString());

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            this.removeWritenBlock(_nameless);

            return returnString;
        }
    }
}