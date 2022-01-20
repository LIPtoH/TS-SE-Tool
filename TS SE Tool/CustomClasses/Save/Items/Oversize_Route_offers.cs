using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS_SE_Tool.Save.Items
{
    class Oversize_Route_offers
    {
        internal List<string> offers { get; set; } = new List<string>();

        internal string route { get; set; } = "";

        internal Oversize_Route_offers()
        { }

        internal Oversize_Route_offers(string[] _input)
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

                    case "offers":
                        {
                            offers.Capacity = int.Parse(dataLine);
                            break;
                        }

                    case var s when s.StartsWith("offers["):
                        {
                            offers.Add(dataLine);
                            break;
                        }

                    case "route":
                        {
                            route = dataLine;
                            break;
                        }

                }
            }
        }

        internal string PrintOut(uint _version, string _nameless)
        {
            string returnString = "";

            StringBuilder returnSB = new StringBuilder();

            returnSB.AppendLine("oversize_route_offers : " + _nameless + " {");

            returnSB.AppendLine(" offers: " + offers.Count);
            for (int i = 0; i < offers.Count; i++)
                returnSB.AppendLine(" offers[" + i + "]: " + offers[i]);

            returnSB.AppendLine(" route: " + route);

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            return returnString;
        }
    }
}