/*
   Copyright 2016-2019 LIPtoH <liptoh.codebase@gmail.com>

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
using System.Drawing;

namespace TS_SE_Tool
{
    class LevelNames
    {
        public LevelNames (int _LevelLimit, string _LevelName, string _NameColor)
        {
            LevelLimit = _LevelLimit;
            LevelName = _LevelName;
            NameColor = Color.FromArgb(Convert.ToUInt16(_NameColor.Substring(0, 2),16), Convert.ToUInt16(_NameColor.Substring(6, 2), 16), Convert.ToUInt16(_NameColor.Substring(4, 2), 16), Convert.ToUInt16(_NameColor.Substring(2, 2), 16));
        }

        public string getName(int _Level)
        {
            return LevelName;
        }

        public int LevelLimit { get; set; }

        public string LevelName { get; set; }

        public Color NameColor { get; set; }
    }
}
