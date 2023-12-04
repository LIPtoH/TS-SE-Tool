using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TS_SE_Tool.Utilities;
using TS_SE_Tool.Save.DataFormat;

namespace TS_SE_Tool.Save.Items
{
    class Flare_blink : SiiNBlockCore
    {
        #region variables
        /*
        internal SCS_Float blink_desync { get; set; } = 0;
        internal SCS_String blink_pattern { get; set; } = "";

        internal SCS_Float blink_step_length { get; set; } = 0;
        internal SCS_Float fake_interior_offset { get; set; } = 0;

        internal bool night_only { get; set; } = false;

        internal SCS_String model_light_source { get; set; } = "";

        internal string light_type { get; set; } = "";
        internal string type { get; set; } = "";
        internal string setup { get; set; } = "";

        internal Vector_3f ambient_color { get; set; } = new Vector_3f();
        internal Vector_3f diffuse_color { get; set; } = new Vector_3f();
        internal Vector_3f specular_color { get; set; } = new Vector_3f();

        internal SCS_Float range { get; set; } = 0;
        internal SCS_Float cut_range { get; set; } = 0;

        internal string cut_direction { get; set; } = "";

        internal SCS_Float attenuation_start_range { get; set; } = 0;

        internal bool forward_distance { get; set; } = false;

        internal SCS_Float inner_angle { get; set; } = 0;
        internal SCS_Float outer_angle { get; set; } = 0;
        internal SCS_Float fade_distance { get; set; } = 0;
        internal SCS_Float fade_span { get; set; } = 0;

        internal SCS_Float yaw_offset { get; set; } = 0;
        internal SCS_Float pitch_offset { get; set; } = 0;
        internal SCS_Float roll_offset { get; set; } = 0;

        internal bool always_on { get; set; } = false;

        internal string dir_type { get; set; } = "";

        internal SCS_Float flare_inner_angle { get; set; } = 0;
        internal SCS_Float flare_outer_angle { get; set; } = 0;

        internal SCS_Float scaling_start_distance { get; set; } = 0;
        internal SCS_Float scaling_end_distance { get; set; } = 0;

        internal SCS_Float scale_factor { get; set; } = 0;
        internal SCS_Float default_scale { get; set; } = 0;

        internal SCS_String model { get; set; } = "";

        internal SCS_Float visual_offset { get; set; } = 0;

        internal SCS_Float state_change_duration { get; set; } = 0;

        internal bool editor { get; set; } = false;

        internal Vector_3f editor_min { get; set; } = new Vector_3f();
        internal Vector_3f editor_max { get; set; } = new Vector_3f();
        */
        #endregion

        internal Flare_blink()
        { }

        internal Flare_blink(string[] _input)
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
                        case "flare_blink":
                        case "}":
                            {
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

            returnSB.AppendLine("flare_blink : " + _nameless + " {");

            WriteUnidentifiedLines();

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            this.removeWritenBlock(_nameless);

            return returnString;
        }
    }
}