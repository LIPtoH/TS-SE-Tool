using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TS_SE_Tool.Save.DataFormat
{
    class SCS_Color
    {
        internal Color color { get; set; } = Color.FromArgb(0, 0, 0, 0);

        internal SCS_Color()
        { }

        public SCS_Color Clone()
        {
            return (SCS_Color) MemberwiseClone();
        }

        internal SCS_Color(int colorInt)
        {
            color = Color.FromArgb(colorInt);
        }

        internal SCS_Color(int alpha, int red, int green, int blue)
        {
            color = Color.FromArgb(alpha, red, green, blue);
        }

        internal SCS_Color(string _input)
        {
            if (_input != "0")
            {
                if (_input == "nil")
                    _input = "4294967295";

                _input = Utilities.NumericUtilities.IntegerToHexString(Convert.ToUInt32(_input));

                int[] hexColorParts = Utilities.NumericUtilities.SplitNConvertSSCHexColor(_input, 2).ToArray();

                //Alpha Red Green Blue
                color = Color.FromArgb(hexColorParts[0], hexColorParts[3], hexColorParts[2], hexColorParts[1]);
            }
        }

        override public string ToString()
        {
            string outColorS = "0";

            if (color != Color.FromArgb(0, 0, 0, 0))
                if (color == Color.FromArgb(255, 255, 255, 255))
                    outColorS = "nil";
                else
                {
                    Byte[] bytes = new Byte[] { color.R, color.G, color.B, 255 };
                    uint temp = BitConverter.ToUInt32(bytes, 0);

                    outColorS = temp.ToString();
                }

            return outColorS;
        }
    }
}
