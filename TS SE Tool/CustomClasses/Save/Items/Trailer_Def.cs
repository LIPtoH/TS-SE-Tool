using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TS_SE_Tool.Save.DataFormat;

namespace TS_SE_Tool.Save.Items
{
    class Trailer_Def : SiiNBlockCore
    {
        internal SCS_String trailer { get; set; } = "";

        internal int gross_trailer_weight_limit { get; set; } = 0;

        internal SCS_Float chassis_mass { get; set; } = 0;
        internal SCS_Float body_mass { get; set; } = 0;

        internal int axles { get; set; } = 0;

        internal SCS_Float volume { get; set; } = 0;

        internal string body_type { get; set; } = "";
        internal string chain_type { get; set; } = "";

        internal List<string> country_validity { get; set; } = new List<string>();

        internal List<SCS_Float> mass_ratio { get; set; } = new List<SCS_Float>();

        internal SCS_Float length { get; set; } = 0;

        internal SCS_String source_name { get; set; } = "";


        internal Trailer_Def()
        { }

        internal Trailer_Def(string[] _input)
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

                        case "trailer":
                            {
                                trailer = dataLine;
                                break;
                            }

                        case "gross_trailer_weight_limit":
                            {
                                gross_trailer_weight_limit = int.Parse(dataLine);
                                break;
                            }

                        case "chassis_mass":
                            {
                                chassis_mass = dataLine;
                                break;
                            }

                        case "body_mass":
                            {
                                body_mass = dataLine;
                                break;
                            }

                        case "axles":
                            {
                                axles = int.Parse(dataLine);
                                break;
                            }

                        case "volume":
                            {
                                volume = dataLine;
                                break;
                            }

                        case "body_type":
                            {
                                body_type = dataLine;
                                break;
                            }

                        case "chain_type":
                            {
                                chain_type = dataLine;
                                break;
                            }

                        case "country_validity":
                            {
                                country_validity.Capacity = int.Parse(dataLine);
                                break;
                            }

                        case var s when s.StartsWith("country_validity["):
                            {
                                country_validity.Add(dataLine);
                                break;
                            }

                        case "mass_ratio":
                            {
                                mass_ratio.Capacity = int.Parse(dataLine);
                                break;
                            }

                        case var s when s.StartsWith("mass_ratio["):
                            {
                                mass_ratio.Add(dataLine);
                                break;
                            }

                        case "length":
                            {
                                length = dataLine;
                                break;
                            }

                        case "source_name":
                            {
                                source_name = dataLine;
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

            returnSB.AppendLine("trailer_def : " + _nameless + " {");

            returnSB.AppendLine(" trailer: " + trailer.ToString());

            returnSB.AppendLine(" gross_trailer_weight_limit: " + gross_trailer_weight_limit.ToString());

            returnSB.AppendLine(" chassis_mass: " + chassis_mass.ToString());
            returnSB.AppendLine(" body_mass: " + body_mass.ToString());

            returnSB.AppendLine(" axles: " + axles.ToString());

            returnSB.AppendLine(" volume: " + volume.ToString());

            returnSB.AppendLine(" body_type: " + body_type);
            returnSB.AppendLine(" chain_type: " + chain_type);

            returnSB.AppendLine(" country_validity: " + country_validity.Count);
            for (int i = 0; i < country_validity.Count; i++)
                returnSB.AppendLine(" country_validity[" + i + "]: " + country_validity[i]);

            returnSB.AppendLine(" mass_ratio: " + mass_ratio.Count);
            for (int i = 0; i < mass_ratio.Count; i++)
                returnSB.AppendLine(" mass_ratio[" + i + "]: " + mass_ratio[i].ToString());

            returnSB.AppendLine(" length: " + length.ToString());

            returnSB.AppendLine(" source_name: " + source_name);

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            this.removeWritenBlock(_nameless);

            return returnString;
        }
    }
}