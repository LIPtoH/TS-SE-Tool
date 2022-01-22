using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TS_SE_Tool.Utilities;
using TS_SE_Tool.Save.DataFormat;

namespace TS_SE_Tool.Save.Items
{
    class Vehicle_Accessory
    {
        internal string data_path { get; set; } = "";
        internal int refund { get; set; } = 0;

        internal string accType { get; set; } = "generalpart";

        internal Vehicle_Accessory()
        { }

        internal Vehicle_Accessory(string[] _input)
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

                                //Type
                                string pathString = data_path.Split(new char[] { '"' })[1];

                                switch (pathString)
                                {
                                    case var s when s.Contains("/data.sii"):
                                        accType = "basepart";
                                        break;

                                    case var s when s.Contains("chassis"):
                                        accType = "chassis";
                                        break;

                                    case var s when s.Contains("body"):
                                        accType = "body";
                                        break;

                                    case var s when s.Contains("cabin"):
                                        accType = "cabin";
                                        break;

                                    case var s when s.Contains("engine"):
                                        accType = "engine";
                                        break;

                                    case var s when s.Contains("transmission"):
                                        accType = "transmission";
                                        break;
                                }

                                break;
                            }

                        case "refund":
                            {
                                refund = int.Parse(dataLine);
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

            returnSB.AppendLine("vehicle_accessory : " + _nameless + " {");

            returnSB.AppendLine(" data_path: " + data_path);
            returnSB.AppendLine(" refund: " + refund.ToString());

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            return returnString;
        }
    }
}