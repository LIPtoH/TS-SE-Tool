using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS_SE_Tool.Save.Items
{
    class Oversize_offer_Ctrl
    {
        internal List<string> route_offers { get; set; } = new List<string>();
        
        internal Oversize_offer_Ctrl()
        { }

        internal Oversize_offer_Ctrl(string[] _input)
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

                switch (tagLine)
                {
                    case "":
                        {
                            break;
                        }

                    case "route_offers":
                        {
                            route_offers.Capacity = int.Parse(dataLine);
                            break;
                        }

                    case var s when s.StartsWith("route_offers["):
                        {
                            route_offers.Add(dataLine);
                            break;
                        }

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

            returnSB.AppendLine("oversize_offer_ctrl : " + _nameless + " {");

            returnSB.AppendLine(" route_offers: " + route_offers.Count);
            for (int i = 0; i < route_offers.Count; i++)
                returnSB.AppendLine(" route_offers[" + i + "]: " + route_offers[i]);

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            return returnString;
        }
    }
}