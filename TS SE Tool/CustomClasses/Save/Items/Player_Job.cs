using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TS_SE_Tool.Save.DataFormat;

namespace TS_SE_Tool.Save.Items
{
    class Player_Job : SiiNBlockCore
    {
        #region variables
        internal string company_truck { get; set; } = "";
        internal string company_trailer { get; set; } = "";

        internal Vector_3f_4f target_placement { get; set; } = new Vector_3f_4f();
        internal Vector_3f_4f target_placement_medium { get; set; } = new Vector_3f_4f();
        internal Vector_3f_4f target_placement_hard { get; set; } = new Vector_3f_4f();
        internal Vector_3f_4f target_placement_rigid { get; set; } = new Vector_3f_4f();
        internal Vector_3f_4f source_placement { get; set; } = new Vector_3f_4f();

        internal int? selected_target { get; set; } = 0;
        internal int time_lower_limit { get; set; } = 0;
        internal int? time_upper_limit { get; set; } = 0;
        internal SCS_Float job_distance { get; set; } = 0;

        internal SCS_Float fuel_consumed { get; set; } = 0;
        internal SCS_Float last_reported_fuel { get; set; } = 0;

        internal int total_fines { get; set; } = 0;

        internal bool is_trailer_loaded { get; set; } = false;

        internal int? online_job_id { get; set; } = null;

        internal string online_job_trailer_model { get; set; } = "";

        internal bool autoload_used { get; set; } = false;

        internal string cargo { get; set; } = "";
        internal string source_company { get; set; } = "";
        internal string target_company { get; set; } = "";

        internal int cargo_model_index { get; set; } = 0;

        internal bool is_articulated { get; set; } = false;
        internal bool is_cargo_market_job { get; set; } = false;

        internal int start_time { get; set; } = 0;
        internal int planned_distance_km { get; set; } = 0;
        internal int ferry_time { get; set; } = 0;
        internal int ferry_price { get; set; } = 0;
        internal int urgency { get; set; } = 0;

        internal string special { get; set; } = "";

        internal int units_count { get; set; } = 0;
        internal int fill_ratio { get; set; } = 0;

        #endregion
        
        internal Player_Job()
        { }

        internal Player_Job(string[] _input)
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

                        case "company_truck":
                            {
                                company_truck = dataLine;
                                break;
                            }

                        case "company_trailer":
                            {
                                company_trailer = dataLine;
                                break;
                            }

                        case "target_placement":
                            {
                                target_placement = new Vector_3f_4f(dataLine);
                                break;
                            }

                        case "target_placement_medium":
                            {
                                target_placement_medium = new Vector_3f_4f(dataLine);
                                break;
                            }

                        case "target_placement_hard":
                            {
                                target_placement_hard = new Vector_3f_4f(dataLine);
                                break;
                            }

                        case "target_placement_rigid":
                            {
                                target_placement_rigid = new Vector_3f_4f(dataLine);
                                break;
                            }

                        case "source_placement":
                            {
                                source_placement = new Vector_3f_4f(dataLine);
                                break;
                            }

                        case "selected_target":
                            {
                                selected_target = dataLine == "nil" ? (int?)null : int.Parse(dataLine);
                                break;
                            }

                        case "time_lower_limit":
                            {
                                time_lower_limit = int.Parse(dataLine);
                                break;
                            }

                        case "time_upper_limit":
                            {
                                time_upper_limit = dataLine == "nil" ? (int?)null : int.Parse(dataLine);
                                break;
                            }

                        case "job_distance":
                            {
                                job_distance = dataLine;
                                break;
                            }

                        case "fuel_consumed":
                            {
                                fuel_consumed = dataLine;
                                break;
                            }

                        case "last_reported_fuel":
                            {
                                last_reported_fuel = dataLine;
                                break;
                            }

                        case "total_fines":
                            {
                                total_fines = int.Parse(dataLine);
                                break;
                            }

                        case "is_trailer_loaded":
                            {
                                is_trailer_loaded = bool.Parse(dataLine);
                                break;
                            }

                        case "online_job_id":
                            {
                                online_job_id = dataLine == "nil" ? (int?)null : int.Parse(dataLine);
                                break;
                            }

                        case "online_job_trailer_model":
                            {
                                online_job_trailer_model = dataLine;
                                break;
                            }

                        case "autoload_used":
                            {
                                autoload_used = bool.Parse(dataLine);
                                break;
                            }

                        case "cargo":
                            {
                                cargo = dataLine;
                                break;
                            }

                        case "source_company":
                            {
                                source_company = dataLine;
                                break;
                            }

                        case "target_company":
                            {
                                target_company = dataLine;
                                break;
                            }

                        case "cargo_model_index":
                            {
                                cargo_model_index = int.Parse(dataLine);
                                break;
                            }

                        case "is_articulated":
                            {
                                is_articulated = bool.Parse(dataLine);
                                break;
                            }

                        case "is_cargo_market_job":
                            {
                                is_cargo_market_job = bool.Parse(dataLine);
                                break;
                            }

                        case "start_time":
                            {
                                start_time = int.Parse(dataLine);
                                break;
                            }

                        case "planned_distance_km":
                            {
                                planned_distance_km = int.Parse(dataLine);
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

                        case "urgency":
                            {
                                urgency = int.Parse(dataLine);
                                break;
                            }

                        case "special":
                            {
                                special = dataLine;
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

            returnSB.AppendLine("player_job : " + _nameless + " {");

            returnSB.AppendLine(" company_truck: " + company_truck);
            returnSB.AppendLine(" company_trailer: " + company_trailer);

            returnSB.AppendLine(" target_placement: " + target_placement.ToString());
            returnSB.AppendLine(" target_placement_medium: " + target_placement_medium.ToString());
            returnSB.AppendLine(" target_placement_hard: " + target_placement_hard.ToString());
            returnSB.AppendLine(" target_placement_rigid: " + target_placement_rigid.ToString());

            returnSB.AppendLine(" source_placement: " + source_placement.ToString());

            returnSB.AppendLine(" selected_target: " + (selected_target == null ? "nil" : selected_target.ToString()));

            returnSB.AppendLine(" time_lower_limit: " + time_lower_limit.ToString());
            returnSB.AppendLine(" time_upper_limit: " + (time_upper_limit == null ? "nil" : time_upper_limit.ToString()));

            returnSB.AppendLine(" job_distance: " + job_distance.ToString());

            returnSB.AppendLine(" fuel_consumed: " + fuel_consumed.ToString());
            returnSB.AppendLine(" last_reported_fuel: " + last_reported_fuel.ToString());

            returnSB.AppendLine(" total_fines: " + total_fines.ToString());

            returnSB.AppendLine(" is_trailer_loaded: " + is_trailer_loaded.ToString().ToLower());

            returnSB.AppendLine(" online_job_id: " + (online_job_id == null ? "nil" : online_job_id.ToString()));

            returnSB.AppendLine(" online_job_trailer_model: " + online_job_trailer_model);

            returnSB.AppendLine(" autoload_used: " + autoload_used.ToString().ToLower());

            returnSB.AppendLine(" cargo: " + cargo);
            returnSB.AppendLine(" source_company: " + source_company);
            returnSB.AppendLine(" target_company: " + target_company);

            returnSB.AppendLine(" cargo_model_index: " + cargo_model_index.ToString());

            returnSB.AppendLine(" is_articulated: " + is_articulated.ToString().ToLower());
            returnSB.AppendLine(" is_cargo_market_job: " + is_cargo_market_job.ToString().ToLower());

            returnSB.AppendLine(" start_time: " + start_time.ToString());
            returnSB.AppendLine(" planned_distance_km: " + planned_distance_km.ToString());
            returnSB.AppendLine(" ferry_time: " + ferry_time.ToString());
            returnSB.AppendLine(" ferry_price: " + ferry_price.ToString());

            returnSB.AppendLine(" urgency: " + urgency.ToString());

            returnSB.AppendLine(" special: " + special);

            returnSB.AppendLine(" units_count: " + units_count.ToString());
            returnSB.AppendLine(" fill_ratio: " + fill_ratio.ToString());


            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            this.removeWritenBlock(_nameless);

            return returnString;
        }
    }
}