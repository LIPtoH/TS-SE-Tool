using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TS_SE_Tool.Save.DataFormat;

namespace TS_SE_Tool.Save.Items
{
    class Transport_Data
    {
        internal int distance { get; set; } = 0;
        internal uint time { get; set; } = 0;
        internal int money { get; set; } = 0;

        internal List<int> count_per_adr { get; set; } = new List<int>();

        internal List<SCS_String> docks { get; set; } = new List<SCS_String>();

        internal List<int> count_per_dock { get; set; } = new List<int>();

        internal Transport_Data()
        { }

        internal Transport_Data(string[] _input)
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

                    case "distance":
                        {
                            distance = int.Parse(dataLine);
                            break;
                        }

                    case "time":
                        {
                            time = uint.Parse(dataLine);
                            break;
                        }

                    case "money":
                        {
                            money = int.Parse(dataLine);
                            break;
                        }

                    case "count_per_adr":
                        {
                            count_per_adr.Capacity = int.Parse(dataLine);
                            break;
                        }

                    case var s when s.StartsWith("count_per_adr["):
                        {
                            count_per_adr.Add(int.Parse(dataLine));
                            break;
                        }

                    case "docks":
                        {
                            docks.Capacity = int.Parse(dataLine);
                            break;
                        }

                    case var s when s.StartsWith("docks["):
                        {
                            docks.Add(dataLine);
                            break;
                        }

                    case "count_per_dock":
                        {
                            count_per_dock.Capacity = int.Parse(dataLine);
                            break;
                        }

                    case var s when s.StartsWith("count_per_dock["):
                        {
                            count_per_dock.Add(int.Parse(dataLine));
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

            returnSB.AppendLine("transport_data : " + _nameless + " {");

            returnSB.AppendLine(" distance: " + distance.ToString());
            returnSB.AppendLine(" time: " + time.ToString());
            returnSB.AppendLine(" money: " + money.ToString());

            returnSB.AppendLine(" count_per_adr: " + count_per_adr.Count);
            for (int i = 0; i < count_per_adr.Count; i++)
                returnSB.AppendLine(" count_per_adr[" + i + "]: " + count_per_adr[i].ToString());

            returnSB.AppendLine(" docks: " + docks.Count);
            for (int i = 0; i < docks.Count; i++)
                returnSB.AppendLine(" docks[" + i + "]: " + docks[i].ToString());

            returnSB.AppendLine(" count_per_dock: " + count_per_dock.Count);
            for (int i = 0; i < count_per_dock.Count; i++)
                returnSB.AppendLine(" count_per_dock[" + i + "]: " + count_per_dock[i].ToString());

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            return returnString;
        }
    }
}