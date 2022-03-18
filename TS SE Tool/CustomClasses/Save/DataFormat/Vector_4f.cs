using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS_SE_Tool.Utilities;

namespace TS_SE_Tool.Save.DataFormat
{
    class Vector_4f
    {
        float W { get; set; } = 1;

        float X { get; set; } = 0;
        float Y { get; set; } = 0;
        float Z { get; set; } = 0;

        internal Vector_4f()
        { }

        internal Vector_4f(float _w, float _x, float _y, float _z)
        {
            W = _w;
            X = _x;
            Y = _y;
            Z = _z;
        }

        internal Vector_4f(string _input)
        {
            string[] parts = _input.Split(new char[] { '(', ')', ';', ',' }, 5, StringSplitOptions.RemoveEmptyEntries);

            W = NumericUtilities.HexFloatToSingleFloat(parts[0].Trim());
            X = NumericUtilities.HexFloatToSingleFloat(parts[1].Trim());
            Y = NumericUtilities.HexFloatToSingleFloat(parts[2].Trim());
            Z = NumericUtilities.HexFloatToSingleFloat(parts[3].Trim());
        }

        internal void ToVector(string _input)
        {
            string[] parts = _input.Split(new char[] { '(', ')', ';', ',' }, 5, StringSplitOptions.RemoveEmptyEntries);

            W = NumericUtilities.HexFloatToSingleFloat(parts[0].Trim());
            X = NumericUtilities.HexFloatToSingleFloat(parts[1].Trim());
            Y = NumericUtilities.HexFloatToSingleFloat(parts[2].Trim());
            Z = NumericUtilities.HexFloatToSingleFloat(parts[3].Trim());
        }

        override public string ToString()
        {
            // (&3f7f126f; &bd85bf17, &bd5ecfd4, &bb69a963)
            return "(" + NumericUtilities.SingleFloatToString(W) + "; " +
                NumericUtilities.SingleFloatToString(X) + ", " + NumericUtilities.SingleFloatToString(Y) + ", " + NumericUtilities.SingleFloatToString(Z) + ")";
        }
    }
}
