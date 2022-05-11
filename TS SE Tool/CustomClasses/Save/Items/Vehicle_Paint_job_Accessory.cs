using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TS_SE_Tool.Utilities;
using TS_SE_Tool.Save.DataFormat;

namespace TS_SE_Tool.Save.Items
{
    class Vehicle_Paint_job_Accessory : SiiNBlockCore
    {
        internal SCS_Float_3 mask_r_color { get; set; } = new SCS_Float_3();
        internal SCS_Float_3 mask_g_color { get; set; } = new SCS_Float_3();
        internal SCS_Float_3 mask_b_color { get; set; } = new SCS_Float_3();

        internal SCS_Float_3 flake_color { get; set; } = new SCS_Float_3();
        internal SCS_Float_3 flip_color { get; set; } = new SCS_Float_3();
        internal SCS_Float_3 base_color { get; set; } = new SCS_Float_3();

        internal string data_path { get; set; } = "";
        internal uint refund { get; set; } = 0;


        internal Vehicle_Paint_job_Accessory()
        { }

        internal Vehicle_Paint_job_Accessory(string[] _input)
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

                        case "mask_r_color":
                            {
                                mask_r_color = new SCS_Float_3(dataLine);
                                break;
                            }

                        case "mask_g_color":
                            {
                                mask_g_color = new SCS_Float_3(dataLine);
                                break;
                            }

                        case "mask_b_color":
                            {
                                mask_b_color = new SCS_Float_3(dataLine);
                                break;
                            }

                        case "flake_color":
                            {
                                flake_color = new SCS_Float_3(dataLine);
                                break;
                            }

                        case "flip_color":
                            {
                                flip_color = new SCS_Float_3(dataLine);
                                break;
                            }

                        case "base_color":
                            {
                                base_color = new SCS_Float_3(dataLine);
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

            returnSB.AppendLine("vehicle_paint_job_accessory : " + _nameless + " {");

            returnSB.AppendLine(" mask_r_color: " + mask_r_color.ToString());
            returnSB.AppendLine(" mask_g_color: " + mask_g_color.ToString());
            returnSB.AppendLine(" mask_b_color: " + mask_b_color.ToString());

            returnSB.AppendLine(" flake_color: " + flake_color.ToString());
            returnSB.AppendLine(" flip_color: " + flip_color.ToString());
            returnSB.AppendLine(" base_color: " + base_color.ToString());

            returnSB.AppendLine(" data_path: " + data_path);
            returnSB.AppendLine(" refund: " + refund.ToString());

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            this.removeWritenBlock(_nameless);

            return returnString;
        }
    }
}