using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TS_SE_Tool.Save.DataFormat;

namespace TS_SE_Tool.Save.Items
{
    class Oversize_Offer : SiiNBlockCore
    {
        internal string offer_data { get; set; } = "";

        internal SCS_String truck { get; set; } = "";

        internal uint expiration { get; set; } = 0;

        internal SCS_String intro_cutscene { get; set; } = "";
        internal SCS_String outro_cutscene { get; set; } = "";

        internal Oversize_Offer()
        { }

        internal Oversize_Offer(string[] _input)
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

                        case "offer_data":
                            {
                                offer_data = dataLine;
                                break;
                            }

                        case "truck":
                            {
                                truck = dataLine;
                                break;
                            }

                        case "expiration":
                            {
                                expiration = uint.Parse(dataLine);
                                break;
                            }

                        case "intro_cutscene":
                            {
                                intro_cutscene = dataLine;
                                break;
                            }

                        case "outro_cutscene":
                            {
                                outro_cutscene = dataLine;
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

            returnSB.AppendLine("oversize_offer : " + _nameless + " {");

            returnSB.AppendLine(" offer_data: " + offer_data);

            returnSB.AppendLine(" truck: " + truck.ToString());

            returnSB.AppendLine(" expiration: " + expiration.ToString());

            returnSB.AppendLine(" intro_cutscene: " + intro_cutscene.ToString());
            returnSB.AppendLine(" outro_cutscene: " + outro_cutscene.ToString());

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            this.removeWritenBlock(_nameless);

            return returnString;
        }
    }
}