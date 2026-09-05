/*
   ETS2 / ATS 1.61 compatibility additions
   Copyright 2026 rusplane <https://rusplane.dev>

   Based on TS SE Tool by LIPtoH.
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace TS_SE_Tool.Save.Items
{
    class Car_Job_Log : SiiNBlockCore
    {
        internal int version { get; set; } = 0;
        internal List<string> entries { get; set; } = new List<string>();
        internal int cached_jobs_count { get; set; } = 0;

        internal Car_Job_Log() { }

        internal Car_Job_Log(string[] input)
        {
            foreach (string currentLine in input)
            {
                if (!currentLine.Contains(":"))
                    continue;

                string[] split = currentLine.Split(new[] { ':' }, 2);
                string tagLine = split[0].Trim();
                string dataLine = split[1].Trim();

                try
                {
                    switch (tagLine)
                    {
                        case "version":
                            version = int.Parse(dataLine);
                            break;

                        case "entries":
                            entries.Capacity = int.Parse(dataLine);
                            break;

                        case var s when s.StartsWith("entries["):
                            entries.Add(dataLine);
                            break;

                        case "cached_jobs_count":
                            cached_jobs_count = int.Parse(dataLine);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Utilities.IO_Utilities.ErrorLogWriter(
                        ex.Message + Environment.NewLine +
                        "car_job_log | " + tagLine + " = " + dataLine);
                }
            }
        }

        internal string PrintOut(uint version)
        {
            return PrintOut(version, null);
        }

        internal string PrintOut(uint _version, string _nameless)
        {
            string returnString = "";

            StringBuilder returnSB = new StringBuilder();

            returnSB.AppendLine("car_job_log : " + _nameless + " {");

            returnSB.AppendLine(" version: " + version.ToString());

            returnSB.AppendLine(" entries: " + entries.Count);

            for (int i = 0; i < entries.Count; i++)
                returnSB.AppendLine(" entries[" + i + "]: " + entries[i]);

            returnSB.AppendLine(" cached_jobs_count: " + cached_jobs_count.ToString());

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            this.removeWritenBlock(_nameless);

            return returnString;
        }
    }
}