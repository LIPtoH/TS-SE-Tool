/*
   Copyright 2016-2020 LIPtoH <liptoh.codebase@gmail.com>

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace TS_SE_Tool
{
    class ScsFontLetter
    {
        internal UInt16 P_x { get; set; } = 0;
        internal UInt16 P_y { get; set; } = 0;
        internal byte Width { get; set; } = 16;
        internal byte Height { get; set; } = 22;

        internal Int16 Left_offset { get; set; } = 0;
        internal Int16 Top_offset { get; set; } = 0;

        internal byte Advance { get; set; } = 0;

        internal byte ImageIndex { get; set; } = 0;

        internal Bitmap LetterImage { get; set; }

        public ScsFontLetter(UInt16 _PositionX, UInt16 _PositionY, byte _Width, byte _Height, Int16 _Left_offset, Int16 _Top_offset, byte _Advance)
        {
            P_x = _PositionX;
            P_y = _PositionY;

            Width = _Width;
            Height  = _Height;

            Left_offset  = _Left_offset;
            Top_offset  = _Top_offset;

            Advance = _Advance;
        }

        public ScsFontLetter(string _PositionX, string _PositionY, string _Width, string _Height, string _Left_offset, string _Top_offset, string _Advance)
        {
            P_x = Convert.ToUInt16(_PositionX);
            P_y = Convert.ToUInt16(_PositionY);

            Width = Convert.ToByte(_Width);
            Height = Convert.ToByte(_Height);

            Left_offset = Convert.ToInt16(_Left_offset);
            Top_offset = Convert.ToInt16(_Top_offset);

            Advance = Convert.ToByte(_Advance);
        }
    }
}
