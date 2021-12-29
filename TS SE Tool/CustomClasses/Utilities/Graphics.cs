/*
   Copyright 2016-2022 LIPtoH <liptoh.codebase@gmail.com>

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
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace TS_SE_Tool.Utilities
{

    internal class TS_Graphics
    {
        static FormMain MainForm = Application.OpenForms.OfType<FormMain>().Single();
        
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

        public static Image ResizeImage(Image _inputImage, int _newWidth, int _newHeight)
        {
            Image newImage = new Bitmap(_newWidth, _newHeight);

            using (var canvas = Graphics.FromImage(newImage))
            {
                canvas.InterpolationMode = InterpolationMode.Bicubic;
                canvas.SmoothingMode = SmoothingMode.HighQuality;

                var attributes = new ImageAttributes();
                attributes.SetWrapMode(WrapMode.TileFlipXY);

                var destination = new Rectangle(0, 0, _newWidth, _newHeight);

                canvas.DrawImage(_inputImage, destination, 0, 0, _inputImage.Width, _inputImage.Height, GraphicsUnit.Pixel, attributes);
                canvas.Save();
            }

            return newImage;
        }

        public static Image SimpleResizeImage(Image _inputImage, int _newWidth, int _newHeight)
        {
            Image newImage = new Bitmap(_inputImage, _newWidth, _newHeight);
            return newImage;
        }

        private Image ReColorMonochrome(Image _inpulLetter, Color _newColor)
        {
            Bitmap tmp = new Bitmap(_inpulLetter);

            for (int y = 0; y < _inpulLetter.Height; y++)
            {
                for (int x = 0; x < _inpulLetter.Width; x++)
                {
                    tmp.SetPixel(x, y, Color.FromArgb(tmp.GetPixel(x, y).A, _newColor.R, _newColor.G, _newColor.B));
                }
            }

            return tmp;
        }
    }
}
