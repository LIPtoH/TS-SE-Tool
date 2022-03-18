using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TS_SE_Tool.Utilities;

namespace TS_SE_Tool.CustomClasses.Global
{
    public class FloatList : List<float>
    {
        new public string this[int index]
        {
            get
            {
                return NumericUtilities.SingleFloatToString(base[index]);
            }

            set
            {
                base[index] = NumericUtilities.HexFloatToSingleFloat(value);
            }
        }

        public void Add(string item)
        {
            base.Add(NumericUtilities.HexFloatToSingleFloat(item));
        }
    }
}
