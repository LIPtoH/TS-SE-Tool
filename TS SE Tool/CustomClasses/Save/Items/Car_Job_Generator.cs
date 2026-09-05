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
    class Car_Job_Generator : SiiNBlockCore
    {
        internal List<string> car_offers { get; set; } = new List<string>();

        internal Car_Job_Generator() { }

        internal Car_Job_Generator(string[] input)
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
                        case "car_offers":
                            car_offers.Capacity = int.Parse(dataLine);
                            break;

                        case var s when s.StartsWith("car_offers["):
                            car_offers.Add(dataLine);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Utilities.IO_Utilities.ErrorLogWriter(
                        ex.Message + Environment.NewLine +
                        "car_job_generator | " + tagLine + " = " + dataLine);
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

            returnSB.AppendLine("car_job_generator : " + _nameless + " {");

            returnSB.AppendLine(" car_offers: " + car_offers.Count);

            for (int i = 0; i < car_offers.Count; i++)
                returnSB.AppendLine(" car_offers[" + i + "]: " + car_offers[i]);

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            this.removeWritenBlock(_nameless);

            return returnString;
        }
    }
}