using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS_SE_Tool.Utilities;

namespace TS_SE_Tool.Save.DataFormat
{
    public struct SCS_Float
    {
        public SCS_Float(float _input)
        {
            Value = _input;
        }

        internal float Value { get; set; }

        static public implicit operator SCS_Float(float _input)
        {
            return new SCS_Float(_input);
        }

        static public implicit operator SCS_Float(string _input)
        {
            return new SCS_Float(NumericUtilities.HexFloatToSingleFloat(_input));
        }

        static public implicit operator float(SCS_Float _input)
        {
            return _input.Value;
        }

        public override string ToString()
        {
            return NumericUtilities.SingleFloatToHexFloat(Value);
        }
        public string ToHexString()
        {
            return NumericUtilities.SingleFloatToHexFloat(Value, true);
        }
    }

}
