using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS_SE_Tool.Save.DataFormat;

namespace TS_SE_Tool.Save.DataFormat
{
    class SCS_Placement
    {
        SCS_Float_3 placement { get; set; } = new SCS_Float_3();
        SCS_Quaternion direction { get; set; } = new SCS_Quaternion();

        internal SCS_Placement()
        { }

        internal SCS_Placement(string _input)
        {
            string[] parts = _input.Split(new char[] { '(', ')' }, 4, StringSplitOptions.RemoveEmptyEntries);

            string place = parts[0], direct = parts[2];

            placement = new SCS_Float_3(place);
            direction = new SCS_Quaternion(direct);
        }

        override public string ToString()
        {
            // (&c75137e5, &40dfd91d, &453d1a51) (&3f59911d; &ba5459ea, &bf06e8dd, &ba03acdd)
            return placement + " " + direction;
        }
    }
}
