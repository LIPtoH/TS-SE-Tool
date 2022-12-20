using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TS_SE_Tool.Utilities;

namespace TS_SE_Tool.Save.DataFormat
{
    public struct SCS_String
    {
        public SCS_String(string _input)
        {
            Value = _input;
        }

        internal string Value { get; set; }

        static public implicit operator SCS_String(string _input)
        {
            return new SCS_String(TextUtilities.FromOutputStringToString(_input));
        }

        public override string ToString()
        {
            return TextUtilities.FromStringToOutputString(Value);
        }
    }
}
