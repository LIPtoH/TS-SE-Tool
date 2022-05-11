using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS_SE_Tool.Save.Items
{
    class Job_offer_Data : SiiNBlockCore
    {
        internal string target { get; set; } = "";

        internal uint? expiration_time { get; set; } = null;

        internal int? urgency { get; set; } = 0;

        internal int shortest_distance_km { get; set; } = 0;

        internal int ferry_time { get; set; } = 0;
        internal int ferry_price { get; set; } = 0;

        internal string cargo { get; set; } = "";

        internal string company_truck { get; set; } = "";

        internal string trailer_variant { get; set; } = "";
        internal string trailer_definition { get; set; } = "";

        internal int units_count { get; set; } = 0;

        internal int fill_ratio { get; set; } = 0;

        internal List<DataFormat.SCS_Placement> trailer_place { get; set; } = new List<DataFormat.SCS_Placement>();


        internal Job_offer_Data()
        { }

        internal Job_offer_Data(string[] _input)
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

                        case "target":
                            {
                                target = dataLine;
                                break;
                            }

                        case "expiration_time":
                            {
                                expiration_time = dataLine == "nil" ? (uint?)null : uint.Parse(dataLine);
                                break;
                            }

                        case "urgency":
                            {
                                urgency = dataLine == "nil" ? (int?)null : int.Parse(dataLine);
                                break;
                            }

                        case "shortest_distance_km":
                            {
                                shortest_distance_km = int.Parse(dataLine);
                                break;
                            }

                        case "ferry_time":
                            {
                                ferry_time = int.Parse(dataLine);
                                break;
                            }

                        case "ferry_price":
                            {
                                ferry_price = int.Parse(dataLine);
                                break;
                            }

                        case "cargo":
                            {
                                cargo = dataLine;
                                break;
                            }

                        case "company_truck":
                            {
                                company_truck = dataLine;
                                break;
                            }

                        case "trailer_variant":
                            {
                                trailer_variant = dataLine;
                                break;
                            }

                        case "trailer_definition":
                            {
                                trailer_definition = dataLine;
                                break;
                            }

                        case "units_count":
                            {
                                units_count = int.Parse(dataLine);
                                break;
                            }

                        case "fill_ratio":
                            {
                                fill_ratio = int.Parse(dataLine);
                                break;
                            }

                        case "trailer_place":
                            {
                                trailer_place.Capacity = int.Parse(dataLine);
                                break;
                            }

                        case var s when s.StartsWith("trailer_place["):
                            {
                                trailer_place.Add(new DataFormat.SCS_Placement(dataLine));
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

            returnSB.AppendLine("job_offer_data : " + _nameless + " {");

            returnSB.AppendLine(" target: " + target);

            returnSB.AppendLine(" expiration_time: " + (expiration_time == null ? "nil" : expiration_time.ToString()));
            returnSB.AppendLine(" urgency: " + (urgency == null ? "nil" : urgency.ToString()));
            returnSB.AppendLine(" shortest_distance_km: " + shortest_distance_km.ToString());
            returnSB.AppendLine(" ferry_time: " + ferry_time.ToString());
            returnSB.AppendLine(" ferry_price: " + ferry_price.ToString());

            returnSB.AppendLine(" cargo: " + cargo);

            returnSB.AppendLine(" company_truck: " + company_truck);

            returnSB.AppendLine(" trailer_variant: " + trailer_variant);
            returnSB.AppendLine(" trailer_definition: " + trailer_definition);

            returnSB.AppendLine(" units_count: " + units_count.ToString());
            returnSB.AppendLine(" fill_ratio: " + fill_ratio.ToString());

            returnSB.AppendLine(" trailer_place: " + trailer_place.Count);
            for (int i = 0; i < trailer_place.Count; i++)
                returnSB.AppendLine(" trailer_place[" + i + "]: " + trailer_place[i]);

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            this.removeWritenBlock(_nameless);

            return returnString;
        }
    }
}