using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS_SE_Tool.Save.Items
{
    internal class Unidentified : SiiNBlockCore
    {
        internal List<string> lines { get; set; } = new List<string>();

        internal Unidentified()
        { }

        internal Unidentified(List<string> _input)
        {
            lines = _input;
        }

        internal string PrintOut(uint _version)
        {
            return PrintOut(_version, null);
        }

        internal string PrintOut(uint _version, string _nameless)
        {
            string returnString = "";

            StringBuilder returnSB = new StringBuilder();

            returnSB.AppendLine(String.Join(Environment.NewLine, lines));

            returnString = returnSB.ToString();

            this.removeWritenBlock(_nameless);

            return returnString;
        }

    }
}
