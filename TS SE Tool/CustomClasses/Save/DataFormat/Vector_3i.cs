using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS_SE_Tool.Save.DataFormat
{
    class Vector_3i
    {
        int X { get; set; } = 0;
        int Y { get; set; } = 0;
        int Z { get; set; } = 0;

        internal Vector_3i()
        { }

        internal Vector_3i(string _input)
        {
            string[] parts = _input.Split(new char[] { '(', ')', ';', ',' }, 4, StringSplitOptions.RemoveEmptyEntries);

            X = int.Parse(parts[0].Trim());
            Y = int.Parse(parts[1].Trim());
            Z = int.Parse(parts[2].Trim());
        }

        override public string ToString()
        {
            // (2147483647, 2147483647, 2147483647)
            return "(" + X.ToString() + ", " + Y.ToString() + ", " + Z.ToString() + ")";
        }

    }
}
