using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TS_SE_Tool.Save.DataFormat;

namespace TS_SE_Tool.Global
{
    public static partial class EnumerableExt
    {
        public static float Sum(this IEnumerable<SCS_Float> source)
        {
            if (source == null) throw new ArgumentNullException("source");
            float sum = 0;
            foreach (float v in source) sum += v;
            return (float)sum;
        }
    }
}
