﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS_SE_Tool.Utilities;

namespace TS_SE_Tool.Save.DataFormat
{
    class Vector_3f
    {    
        float X { get; set; } = 0;
        float Y { get; set; } = 0;
        float Z { get; set; } = 0;

        internal Vector_3f()
        { }

        internal Vector_3f(float _x, float _y, float _z)
        {
            X = _x;
            Y = _y;
            Z = _z;
        }

        internal Vector_3f(string _input)
        {
            string[] parts = _input.Split(new char[] { '(', ')', ';', ',' }, 4, StringSplitOptions.RemoveEmptyEntries);

            X = NumericUtilities.HexFloatToSingleFloat(parts[0].Trim());
            Y = NumericUtilities.HexFloatToSingleFloat(parts[1].Trim());
            Z = NumericUtilities.HexFloatToSingleFloat(parts[2].Trim());
        }

        internal void ToVector(string _input)
        {
            string[] parts = _input.Split(new char[] { '(', ')', ';', ',' }, 4, StringSplitOptions.RemoveEmptyEntries);

            X = NumericUtilities.HexFloatToSingleFloat(parts[0].Trim());
            Y = NumericUtilities.HexFloatToSingleFloat(parts[1].Trim());
            Z = NumericUtilities.HexFloatToSingleFloat(parts[2].Trim());
        }

        override public string ToString()
        {
            // (&bd85bf17, &bd5ecfd4, &bb69a963)
            return "(" + NumericUtilities.SingleFloatToString(X) + ", " + NumericUtilities.SingleFloatToString(Y) + ", " + NumericUtilities.SingleFloatToString(Z) + ")";
        }

    }
}
