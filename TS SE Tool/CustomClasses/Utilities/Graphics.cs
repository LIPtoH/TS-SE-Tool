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
using System.Drawing;

namespace TS_SE_Tool.Utilities
{
    internal class TS_Graphics
    {
        internal static Icon IconFromImage(Image _inputImage)
        {
            return IconFromImage(_inputImage, 0);
        }

        internal static Icon IconFromImage(Image _inputImage, byte _offset)
        {
            Bitmap bmpIcon = new Bitmap(_inputImage, _inputImage.Width - _offset * 2, _inputImage.Height - _offset * 2);

            //bmpIcon

            Bitmap bmpCanvas = new Bitmap(_inputImage.Width, _inputImage.Height);
            /*
            if (_offset > 0)
                using (Graphics gfx = Graphics.FromImage(bmpCanvas))
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(0xff, 0xff, 0xff)))
                {
                    gfx.FillRectangle(brush, 0, 0, bmpCanvas.Width, bmpCanvas.Height);
                }
            */
            using (var canvas = Graphics.FromImage(bmpCanvas))
            {   
                canvas.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                canvas.DrawImage(bmpIcon, _offset, _offset);
                canvas.Save();
            }

            return Icon.FromHandle(bmpCanvas.GetHicon());
        }
    }
}
