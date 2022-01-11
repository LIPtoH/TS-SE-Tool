using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS_SE_Tool.Utilities;

namespace TS_SE_Tool.Save.DataFormat
{
    class Vector_2f
    {
        float X { get; set; } = 0;
        float Y { get; set; } = 0;

        internal Vector_2f()
        { }

        internal Vector_2f(float _x, float _y)
        {
            X = _x;
            Y = _y;
        }

        internal Vector_2f(string _input)
        {
            string[] parts = _input.Split(new char[] { '(', ')', ';', ',' }, 3, StringSplitOptions.RemoveEmptyEntries);

            X = NumericUtilities.HexFloatToSingleFloat(parts[0].Trim());
            Y = NumericUtilities.HexFloatToSingleFloat(parts[1].Trim());
        }

        internal void ToVector(string _input)
        {
            string[] parts = _input.Split(new char[] { '(', ')', ';', ',' }, 3, StringSplitOptions.RemoveEmptyEntries);

            X = NumericUtilities.HexFloatToSingleFloat(parts[0].Trim());
            Y = NumericUtilities.HexFloatToSingleFloat(parts[1].Trim());
        }

        override public string ToString()
        {
            // (&bd85bf17, &bd5ecfd4)
            return "(" + NumericUtilities.SingleFloatToHexFloat(X) + ", " + NumericUtilities.SingleFloatToHexFloat(Y) + ")";
        }
    }
}
