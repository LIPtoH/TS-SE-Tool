using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TS_SE_Tool.Utilities;
using TS_SE_Tool.Save.DataFormat;

namespace TS_SE_Tool.Save.Items
{
    class Accessory_Hookup_data : SiiNBlockCore
    {
        #region variables

        internal SCS_String model { get; set; } = "";
        internal SCS_String coll { get; set; } = "";

        internal string look { get; set; } = "";
        internal string variant { get; set; } = "";
        internal string electric_type { get; set; } = "";

        internal SCS_String name { get; set; } = "";
        internal SCS_String icon { get; set; } = "";
        /*
        internal int info { get; set; } = 0;
        internal uint price { get; set; } = 0;
        internal uint unlock { get; set; } = 0;
        internal List<string> suitable_for { get; set; } = new List<string>();
        internal List<string> conflict_with { get; set; } = new List<string>();
        internal List<string> defaults { get; set; } = new List<string>();
        internal List<string> overrides { get; set; } = new List<string>();
        internal List<string> require { get; set; } = new List<string>();
        */
        internal bool original_part { get; set; } = false;
        internal bool sync_over_network { get; set; } = false;

        internal ulong? steam_inventory_id { get; set; } = null;
        
        #endregion

        internal Accessory_Hookup_data()
        { }

        internal Accessory_Hookup_data(string[] _input)
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
                        case "accessory_hookup_data":
                        case "}":
                            {
                                break;
                            }

                        case "model":
                            {
                                model = dataLine;
                                break;
                            }

                        case "coll":
                            {
                                coll = dataLine;
                                break;
                            }

                        case "look":
                            {
                                look = dataLine;
                                break;
                            }

                        case "variant":
                            {
                                variant = dataLine;
                                break;
                            }

                        case "electric_type":
                            {
                                electric_type = dataLine;
                                break;
                            }

                        case "name":
                            {
                                name = dataLine;
                                break;
                            }

                        case "icon":
                            {
                                icon = dataLine;
                                break;
                            }
                            /*
                        case "info":
                            {
                                info = int.Parse(dataLine);
                                break;
                            }

                        case "price":
                            {
                                price = uint.Parse(dataLine);
                                break;
                            }

                        case "unlock":
                            {
                                unlock = uint.Parse(dataLine);
                                break;
                            }

                        case "suitable_for":
                            {
                                suitable_for.Capacity = int.Parse(dataLine);
                                break;
                            }

                        case var s when s.StartsWith("suitable_for["):
                            {
                                suitable_for.Add(dataLine);
                                break;
                            }

                        case "conflict_with":
                            {
                                conflict_with.Capacity = int.Parse(dataLine);
                                break;
                            }

                        case var s when s.StartsWith("conflict_with["):
                            {
                                conflict_with.Add(dataLine);
                                break;
                            }

                        case "defaults":
                            {
                                defaults.Capacity = int.Parse(dataLine);
                                break;
                            }

                        case var s when s.StartsWith("defaults["):
                            {
                                defaults.Add(dataLine);
                                break;
                            }

                        case "overrides":
                            {
                                overrides.Capacity = int.Parse(dataLine);
                                break;
                            }

                        case var s when s.StartsWith("overrides["):
                            {
                                overrides.Add(dataLine);
                                break;
                            }

                        case "require":
                            {
                                require.Capacity = int.Parse(dataLine);
                                break;
                            }

                        case var s when s.StartsWith("require["):
                            {
                                require.Add(dataLine);
                                break;
                            }
                            */
                        case "original_part":
                            {
                                original_part = bool.Parse(dataLine);
                                break;
                            }

                        case "sync_over_network":
                            {
                                sync_over_network = bool.Parse(dataLine);
                                break;
                            }

                        case "steam_inventory_id":
                            {
                                steam_inventory_id = dataLine == "nil" ? (ulong?)null : ulong.Parse(dataLine);
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

        internal string PrintOut(uint _version)
        {
            return PrintOut(_version, null);
        }

        internal string PrintOut(uint _version, string _nameless)
        {
            string returnString = "";

            StringBuilder returnSB = new StringBuilder();

            returnSB.AppendLine("accessory_hookup_data : " + _nameless + " {");

            returnSB.AppendLine(" model: " + model.ToString());
            returnSB.AppendLine(" coll: " + coll.ToString());

            returnSB.AppendLine(" look: " + look);
            returnSB.AppendLine(" variant: " + variant);
            returnSB.AppendLine(" electric_type: " + electric_type);

            returnSB.AppendLine(" name: " + name.ToString());
            returnSB.AppendLine(" icon: " + icon.ToString());
            /*
            returnSB.AppendLine(" info: " + info.ToString());
            returnSB.AppendLine(" price: " + price.ToString());
            returnSB.AppendLine(" unlock: " + unlock.ToString());

            returnSB.AppendLine(" suitable_for: " + suitable_for.Count);
            for (int i = 0; i < suitable_for.Count; i++)
                returnSB.AppendLine(" suitable_for[" + i + "]: " + suitable_for[i]);

            returnSB.AppendLine(" conflict_with: " + conflict_with.Count);
            for (int i = 0; i < conflict_with.Count; i++)
                returnSB.AppendLine(" conflict_with[" + i + "]: " + conflict_with[i]);

            returnSB.AppendLine(" defaults: " + defaults.Count);
            for (int i = 0; i < defaults.Count; i++)
                returnSB.AppendLine(" defaults[" + i + "]: " + defaults[i]);

            returnSB.AppendLine(" overrides: " + overrides.Count);
            for (int i = 0; i < overrides.Count; i++)
                returnSB.AppendLine(" overrides[" + i + "]: " + overrides[i]);

            returnSB.AppendLine(" require: " + require.Count);
            for (int i = 0; i < require.Count; i++)
                returnSB.AppendLine(" require[" + i + "]: " + require[i]);
            */
            returnSB.AppendLine(" original_part: " + original_part.ToString().ToLower());
            returnSB.AppendLine(" sync_over_network: " + sync_over_network.ToString().ToLower());

            returnSB.AppendLine(" steam_inventory_id: " + (steam_inventory_id == null ? "nil" : steam_inventory_id.ToString()));

            WriteUnidentifiedLines();

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            this.removeWritenBlock(_nameless);

            return returnString;
        }
    }
}
