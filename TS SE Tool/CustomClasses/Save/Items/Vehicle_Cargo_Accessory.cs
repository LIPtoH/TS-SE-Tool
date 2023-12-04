using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TS_SE_Tool.Utilities;
using TS_SE_Tool.Save.DataFormat;

namespace TS_SE_Tool.Save.Items
{
    class Vehicle_Cargo_Accessory : SiiNBlockCore
    {
        internal string cargo_data { get; set; } = "";
        internal uint model_seed { get; set; } = 0;
        internal string data_path { get; set; } = "";
        internal uint refund { get; set; } = 0;


        internal Vehicle_Cargo_Accessory()
        { }

        internal Vehicle_Cargo_Accessory(string[] _input)
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
                        case "vehicle_cargo_accessory":
                        case "}":
                            {
                                break;
                            }

                        case "cargo_data":
                            {
                                cargo_data = dataLine;
                                break;
                            }

                        case "model_seed":
                            {
                                model_seed = uint.Parse(dataLine);
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

            returnSB.AppendLine("vehicle_cargo_accessory : " + _nameless + " {");

            returnSB.AppendLine(" cargo_data: " + cargo_data);
            returnSB.AppendLine(" model_seed: " + model_seed.ToString());

            returnSB.AppendLine(" data_path: " + data_path);
            returnSB.AppendLine(" refund: " + refund.ToString());

            WriteUnidentifiedLines();

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
