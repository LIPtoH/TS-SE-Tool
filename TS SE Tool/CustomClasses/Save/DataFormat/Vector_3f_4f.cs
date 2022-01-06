using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS_SE_Tool.Save.DataFormat;

namespace TS_SE_Tool.Save.DataFormat
{
    class Vector_3f_4f
    {
        Vector_3f placement { get; set; } = new Vector_3f();
        Vector_4f direction { get; set; } = new Vector_4f();

        internal Vector_3f_4f()
        { }

        internal Vector_3f_4f(string _input)
        {
            string[] parts = _input.Split(new char[] { '(', ')' }, 5, StringSplitOptions.RemoveEmptyEntries);

            string place = parts[0], direct = parts[2];

            placement = new Vector_3f(place);
            direction = new Vector_4f(direct);
        }

        override public string ToString()
        {
            // (&c75137e5, &40dfd91d, &453d1a51) (&3f59911d; &ba5459ea, &bf06e8dd, &ba03acdd)
            return placement + " " + direction;
        }

    }
}
