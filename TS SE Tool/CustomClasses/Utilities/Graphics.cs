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
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace TS_SE_Tool.Utilities
{

    internal class TS_Graphics
    {
        static FormMain MainForm = Application.OpenForms.OfType<FormMain>().Single();

        internal enum LPtype : byte
        {
            Truck = 0,
            Trailer = 1
        }

        internal static Image LicensePlateRender(string _lpText, string _country)
        {
            return LicensePlateRender(_lpText, _country, LPtype.Truck);
        }

        internal static Image LicensePlateRender(string _lpText, string _country, LPtype _type)
        {
            //Game type
            string gametype = MainForm.GameType;

            //BG path
            string[] platePriority;
            if (_type == 0)
                platePriority = new string[] { "front", "rear" };
            else
                platePriority = new string[] { "rear", "front" };

            string folderPath = @"img\" + gametype + @"\lp\" + _country + @"\", IMGpath = "";

            foreach (string plate in platePriority)
            {

                if (File.Exists(folderPath + plate + @".dds"))
                {
                    IMGpath = folderPath + plate + @".dds";
                    break;
                }                    
            }

            if (IMGpath == "")
                return null;
            
            //Load BG image
            Image BGImg = MainForm.ExtImgLoader(new string[] { IMGpath })[0];

            //Load Font data
            string fontDataPath = @"img\" + gametype + @"\lpFont\" + _country + @".font";

            //Img Font map

            Dictionary<UInt16, ScsFontLetter> FontMap = new Dictionary<UInt16, ScsFontLetter>();
            string fontimg = ""; //mu

            //Read
            try
            {
                string[] tempFile = File.ReadAllLines(fontDataPath); // Directory.GetCurrentDirectory() +

                for (int i = 0; i < tempFile.Length; i++)
                {
                    string line = tempFile[i];

                    //Font image to load
                    if (line.StartsWith("image:"))
                    {
                        string info = line.Split(new char[] { ':' }, 2)[1];

                        fontimg = info.Split(new char[] { ',' }, 3)[0].Split(new char[] { '/' }).Last().Split(new char[] { '.' }, 2)[0];
                    }

                    //Letters map
                    if (line.StartsWith("x"))
                    {
                        string info = line.Split(new char[] { '#' }, 2)[0];

                        string[] dataparts = info.Remove(0, 1).Split(new char[] { ',' }, 9);

                        ScsFontLetter letter = new ScsFontLetter(dataparts[1], dataparts[2], dataparts[3], dataparts[4], dataparts[5], dataparts[6], dataparts[7]);

                        FontMap.Add(Convert.ToUInt16(dataparts[0], 16), letter);
                    }

                    //implement kerning

                }
            }
            catch
            {
                FormMain.LogWriter("Font file is missing");
            }

            //Load Font 
            IMGpath = @"img\" + gametype + @"\lpFont\" + fontimg + @".dds";
            Image FontImg = MainForm.ExtImgLoader(new string[] { IMGpath })[0];

            //Create font map

            foreach (KeyValuePair<ushort, ScsFontLetter> letter in FontMap)
            {
                if (letter.Value.Width == 0 || letter.Value.Height == 0)
                {
                }
                else
                {
                    Rectangle rect = new Rectangle(letter.Value.P_x, letter.Value.P_y, letter.Value.Width, letter.Value.Height);

                    Bitmap tmpLetter = new Bitmap(FontImg).Clone(rect, PixelFormat.Format32bppArgb);

                    FontMap[letter.Key].LetterImage = tmpLetter;
                }
            }

            //Draw text

            Bitmap LPtextBitmap = new Bitmap(256, 36);

            int LeftOffset = 15, RightOffset = 5;
            int TopOffset = 5;

            int lpLength = _lpText.Length; int cursorPos = 0, hshift = 0, vshift = 0;

            for (int i = 0; i < lpLength; i++)
            {
                string letter = _lpText.Substring(i, 1);

                if (letter == "<")
                {
                    string tagtext = _lpText.Substring(i + 1, _lpText.IndexOf('>', i + 1) - i - 1);

                    //Detect tag type
                    int taglength = _lpText.IndexOf(' ', i) - i - 1;
                    string tag = _lpText.Substring(i + 1, taglength);

                    switch (tag)
                    {
                        case "offset":
                            {
                                int datapos = tagtext.IndexOf("hshift");
                                int eqpos = tagtext.IndexOf('=', datapos) + 1;
                                int spacepos = tagtext.IndexOf(' ', datapos);

                                if (spacepos == -1)
                                    spacepos = tagtext.Length;

                                cursorPos += Convert.ToInt32(tagtext.Substring(eqpos, spacepos - eqpos));

                                datapos = tagtext.IndexOf("vshift");
                                eqpos = tagtext.IndexOf('=', datapos) + 1;
                                spacepos = tagtext.IndexOf(' ', datapos);

                                if(spacepos == -1)
                                    spacepos = tagtext.Length;

                                vshift += Convert.ToInt32(tagtext.Substring(eqpos, spacepos - eqpos));

                                break;
                            }                            

                        case "img":
                            {
                                //img source
                                int datapos = tagtext.IndexOf("src");
                                int eqpos = tagtext.IndexOf('=', datapos) + 1;
                                int spacepos = tagtext.IndexOf(' ', datapos);

                                if (spacepos == -1)
                                    spacepos = tagtext.Length;

                                string imgsource = tagtext.Substring(eqpos, spacepos - eqpos);

                                string imgToLoadmat = imgsource.Split(new char[] { '/' }).Last();

                                string imgToLoad = imgToLoadmat.Substring(0, imgToLoadmat.LastIndexOf('.'));

                                //Resize

                                //Load img
                                string tmpImgPath = @"img\" + gametype + @"\lp\" + _country + "\\" + imgToLoad + @".dds";                                
                                Image tmpImg = MainForm.ExtImgLoader(new string[] { tmpImgPath })[0];

                                //Draw img
                                if (tmpImg != null)
                                {
                                    using (var canvas = Graphics.FromImage(LPtextBitmap))
                                    {
                                        canvas.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                                        canvas.DrawImage(tmpImg, cursorPos, TopOffset + vshift);
                                        canvas.Save();
                                    }

                                    cursorPos = cursorPos + tmpImg.Width;
                                }

                                break;
                            }
                            
                            /*
                        case "font":
                            break;

                        case "align":
                            break;

                        case "ret":
                            break;
                            */                     
                    }

                    //skip to ">"
                    int skip = _lpText.IndexOf('>', i + 1) - i;
                    i = i + skip;
                    continue;
                }

                byte[] utf8bytes = Encoding.UTF8.GetBytes(letter);                
                ushort utf8number = utf8bytes[0];

                if (FontMap[utf8number].LetterImage != null)
                    using (var canvas = Graphics.FromImage(LPtextBitmap))
                    {
                        canvas.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        canvas.DrawImage(FontMap[utf8number].LetterImage, cursorPos + FontMap[utf8number].Left_offset, TopOffset + FontMap[utf8number].Top_offset + vshift);
                        canvas.Save();
                    }

                cursorPos = cursorPos + FontMap[utf8number].Advance;
            }

            //Trim lptextImg
            Rectangle textbounds = new Rectangle(0, 0, cursorPos, LPtextBitmap.Height);
            Image LPtextImg = LPtextBitmap.Clone(textbounds, PixelFormat.Format32bppArgb);
            
            using (var canvas = Graphics.FromImage(BGImg))
            {
                canvas.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                int newWidth = BGImg.Width - LeftOffset - RightOffset;

                if (newWidth < LPtextImg.Width)
                {
                    int newheight = (int)Math.Floor(LPtextImg.Height * ((decimal)newWidth / LPtextImg.Width));

                    if (newheight > 32)
                    {
                        newWidth = (int)Math.Floor(LPtextImg.Width * (28.0m / LPtextImg.Height));
                        canvas.DrawImage(LPtextImg, LeftOffset + Math.Abs(newWidth - LPtextImg.Width) / 2, (TopOffset + BGImg.Height - newheight) / 2, newWidth, 28);

                    }
                    else
                        canvas.DrawImage(LPtextImg, LeftOffset, (TopOffset + BGImg.Height - newheight) /  2, newWidth, newheight);
                }
                else
                {
                    newWidth = (int)Math.Floor(LPtextImg.Width * (32.0m / LPtextImg.Height));

                    canvas.DrawImage(LPtextImg, LeftOffset + (BGImg.Width - newWidth - LeftOffset - RightOffset) / 2, (TopOffset + BGImg.Height - 32) / 2, newWidth, 32);
                }
                
                canvas.Save();
            }

            return BGImg;
        }

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
