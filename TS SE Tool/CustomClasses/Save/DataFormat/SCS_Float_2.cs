using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS_SE_Tool.Utilities;

namespace TS_SE_Tool.Save.DataFormat
{
    class SCS_Float_2
    {
        float X { get; set; } = 0;
        float Y { get; set; } = 0;

        internal SCS_Float_2()
        { }

        internal SCS_Float_2(float _x, float _y)
        {
            X = _x;
            Y = _y;
        }

        internal SCS_Float_2(string _input)
        {
            string[] parts = _input.Split(new char[] { '(', ')', ';', ',' }, 3, StringSplitOptions.RemoveEmptyEntries);

            X = NumericUtilities.HexFloatToSingleFloat(parts[0].Trim());
            Y = NumericUtilities.HexFloatToSingleFloat(parts[1].Trim());
        }

        override public string ToString()
        {
            // (&bd85bf17, &bd5ecfd4)
            return "(" + NumericUtilities.SingleFloatToString(X) + ", " + NumericUtilities.SingleFloatToString(Y) + ")";
        }
    }
}
