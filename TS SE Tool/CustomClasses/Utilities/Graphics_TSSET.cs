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
    internal class Graphics_TSSET
    {
        internal static Icon IconFromImage(Image _inputImage)
        {
            return IconFromImage(_inputImage, 0);
        }

        internal static Icon IconFromImage(Image _inputImage, byte _offset)
        {
            Bitmap bmpIcon = new Bitmap(_inputImage, _inputImage.Width - _offset * 2, _inputImage.Height - _offset * 2);

            Bitmap bmpCanvas = new Bitmap(_inputImage.Width, _inputImage.Height);

            using (var canvas = System.Drawing.Graphics.FromImage(bmpCanvas))
            {   
                canvas.InterpolationMode = InterpolationMode.HighQualityBicubic;
                canvas.DrawImage(bmpIcon, _offset, _offset);
                canvas.Save();
            }

            return Icon.FromHandle(bmpCanvas.GetHicon());
        }

        public static Image ResizeImage(Image _inputImage, int _newWidth, int _newHeight)
        {
            Image newImage = new Bitmap(_newWidth, _newHeight);

            using (var canvas = System.Drawing.Graphics.FromImage(newImage))
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

        public static Image[] ImgFromFileLoader(string[] _filenamesarray)
        {
            Image[] tempImgarray = new Image[_filenamesarray.Length];

            for (int i = 0; i < _filenamesarray.Length; i++)
                if (File.Exists(_filenamesarray[i]))
                    tempImgarray[i] = Bitmap.FromFile(_filenamesarray[i]);
                else
                    tempImgarray[i] = null;

            return tempImgarray;
        }

        public static Image[] ddsImgLoader(string[] _filenamesarray)
        {
            return ddsImgLoader(_filenamesarray, -1, -1, 0, 0, -1, -1);
        }

        public static Image[] ddsImgLoader(string[] _filenamesarray, int _width, int _height)
        {
            return ddsImgLoader(_filenamesarray, _width, _height, 0, 0, _width, _height);
        }

        public static Image[] ddsImgLoader(string[] _filenamesarray, int _width, int _height, int _x, int _y)
        {
            return ddsImgLoader(_filenamesarray, _width, _height, _x, _y, _width, _height);
        }

        public static Image[] ddsImgLoader(string[] _filenamesarray, int _width, int _height, int _x, int _y, int _newWidth, int _newHeight)
        {
            Image[] tempImgarray = new Image[_filenamesarray.Length];

            Bitmap ddsImg;

            for (int i = 0; i < _filenamesarray.Length; i++)
            {
                try
                {
                    if (File.Exists(_filenamesarray[i]))
                    {
                        ddsImg = ImageFromDDS(_filenamesarray[i]);

                        if (_width == -1)
                        {
                            _width = ddsImg.Width;
                            _height = ddsImg.Height;
                        }

                        ddsImg = ddsImg.Clone(new Rectangle(_x, _y, _width, _height), ddsImg.PixelFormat);

                        if (_newWidth == -1)
                        {
                            _newWidth = ddsImg.Width;
                            _newHeight = ddsImg.Height;
                        }

                        tempImgarray[i] = new Bitmap(ddsImg, _newWidth, _newHeight);

                    }
                    else
                        tempImgarray[i] = new Bitmap(1, 1);
                }
                catch
                {
                    tempImgarray[i] = new Bitmap(1, 1);
                }
            }

            return tempImgarray;
        }

        internal static Bitmap ImageFromDDS(string _path)
        {
            Bitmap bitmap = null;

            if (File.Exists(_path))
            {
                S16.Drawing.DDSImage ddsImg;
                using (FileStream fsimage = new FileStream(_path, FileMode.Open))
                    ddsImg = new S16.Drawing.DDSImage(fsimage);

                bitmap = ddsImg.BitmapImage;

                return bitmap;
            }
            else
                return bitmap;
        }

        // Progressbar color gradient

        static Bitmap ProgressBarGradient;

        internal static void CreateProgressBarBitmap()
        {
            Bitmap ProgressBarGradientThis = new Bitmap(100, 1);

            LinearGradientBrush br = new LinearGradientBrush(new RectangleF(0, 0, 100, 1), Color.Black, Color.Black, 0, false);
            ColorBlend cb = new ColorBlend();

            cb.Positions = new[] { 0.0f, 0.5f, 1f };
            cb.Colors = new[] { Color.FromArgb(255, 255, 0, 0), Color.FromArgb(255, 255, 255, 0), Color.FromArgb(255, 0, 255, 0), };

            br.InterpolationColors = cb;

            //puts the gradient scale onto a bitmap which allows for getting a color from pixel
            Graphics g = Graphics.FromImage(ProgressBarGradientThis);
            g.FillRectangle(br, new RectangleF(0, 0, ProgressBarGradientThis.Width, ProgressBarGradientThis.Height));

            ProgressBarGradient = ProgressBarGradientThis;

            return;
        }

        internal static Color GetProgressbarColor(float _value)
        {
            if (_value < 0)
                _value = 0;
            else if (_value > 1)
                _value = 1;

            return ProgressBarGradient.GetPixel(Convert.ToInt32((1 - _value) * 99), 0);
        }

        // Get Grayscale
        internal static Bitmap ConvertBitmapToGrayscale(Image _source)
        {
            Bitmap bm = new Bitmap(_source);

            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(bm);

            //create the grayscale ColorMatrix
            ColorMatrix colorMatrix = new ColorMatrix(
               new float[][]
               {
                 new float[] {.299f, .299f, .299f, 0, 0},
                 new float[] {.587f, .587f, .587f, 0, 0},
                 new float[] {.114f, .114f, .114f, 0, 0},
                 new float[] {0, 0, 0, 1, 0},
                 new float[] {0, 0, 0, 0, 1}
               });

            //create some image attributes
            ImageAttributes attributes = new ImageAttributes();

            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);

            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(_source, new Rectangle(0, 0, _source.Width, _source.Height), 0, 0, _source.Width, _source.Height, GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object
            g.Dispose();

            return bm;
        }
        
    }
}
