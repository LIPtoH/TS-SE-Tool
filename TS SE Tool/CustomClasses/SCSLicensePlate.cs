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
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace TS_SE_Tool.SCS
{
    class SCSLicensePlate
    {
        static FormMain MainForm = Application.OpenForms.OfType<FormMain>().Single();
        //
        //
        private static int LicensePlateIMGwidth = 512, LicensePlateIMGheight = 128;
        internal Image LicensePlateIMG = new Bitmap(LicensePlateIMGwidth, LicensePlateIMGheight);

        internal string LicensePlateTXT { get; set; } = "";
        //
        //
        internal string SourceText { get; set; }
        internal bool ValidLPtext { get; set; } = false;
        internal bool ValidLPcountry { get; set; } = false;

        internal string SourceLPText { get; set; }
        internal string SourceLPCountry { get; set; }
        //
        //Fixed variables
        private decimal scaleXmult = 3.2m, scaleYmult = 3.2m;
        private Dictionary<string, int> baseLeftOffset = new Dictionary<string, int> {{ "ETS2", 228 }, { "ATS", 256 }};
        private Dictionary<string, int> baseAllignOffset = new Dictionary<string, int> { { "ETS2", 52 }, { "ATS", 0 } };
        private Dictionary<string, int> baseTopOffset = new Dictionary<string, int> { { "ETS2", -2 }, { "ATS", 8 } };
        //
        private decimal fontXscale = 1.0m, fontYscale = 1.0m;
        private decimal fontSupSubScale = 1.0m;
        private int cursorPos = 0, vshift = 0;
        private int leftOffset = 0, allignOffset = 0, BaseLine = 32;
        //Drawing area
        private int rightMostPos = 0, TopMostPxl = 30, DownMostPxl = 30;
        private Color FontColor = Color.FromArgb(255, Color.Black);
        //buffer
        private Bitmap LPtextBitmap = new Bitmap(LicensePlateIMGwidth, LicensePlateIMGheight + 30);
        //
        LPtype LicensePlateType = LPtype.Truck;

        internal enum LPtype : byte
        {
            Truck = 0,
            Trailer = 1
        }

        public SCSLicensePlate(string _input, LPtype _type)
        {
            SourceText = _input;
            LicensePlateType = _type;

            ValidLPtext = CheckSourceText();

            leftOffset = baseAllignOffset[MainForm.GameType];
            allignOffset = baseLeftOffset[MainForm.GameType];

            //Load Font data
            string fontDataPath = @"img\" + MainForm.GameType + @"\lpFont\" + SourceLPCountry + @".font";

            if (File.Exists(fontDataPath))
                CreateFontMap(fontDataPath);

            //Create license plate
            Create();
        }

        public void Reset()
        {
            ValidLPtext = CheckSourceText();
            Create();
        }

        public void UpdateCustom(string _input)
        {
            ValidLPtext = CheckSourceText();
            Create();
        }

        internal bool CheckSourceText()
        {
            try
            {
                SourceLPText = SourceText.Split(new char[] { '|' })[0];
                SourceLPCountry = SourceText.Split(new char[] { '|' })[1].Trim(new char[] { '_', ' ' });
            }
            catch
            {
                return false;
            }

            return true;
        }

        public void Create()
        {
            if (!ValidLPtext)
            {
                LicensePlateTXT = "NON VALID FORMAT";
                DrawWarning(LicensePlateTXT);
                return;
            }

            string _lpText = SourceLPText;
            int lpLength = _lpText.Length;

            //Background

            //BG path
            string[] platePriority;

            if (LicensePlateType == LPtype.Truck)
                platePriority = new string[] { "truck_front", "front", "rear" };
            else
                platePriority = new string[] { "trailer", "rear", "front" };

            string folderPath = @"img\" + MainForm.GameType + @"\lp\" + SourceLPCountry + @"\", IMGpath = "";

            foreach (string plate in platePriority)
            {
                if (File.Exists(folderPath + plate + @".dds"))
                {
                    IMGpath = folderPath + plate + @".dds";
                    break;
                }
            }

            if (IMGpath != "" && MainForm.GlobalFontMap.ContainsKey(SourceLPCountry))
                ValidLPcountry = true;
            else
                DrawWarning("NON STANDART COUNTRY");

            Dictionary<UInt16, SCSFontLetter> thisFontMap = new Dictionary<ushort, SCSFontLetter>();

            if (ValidLPcountry)
            {
                //Load BG image
                Image BGImg = Utilities.Graphics_TSSET.ddsImgLoader(IMGpath).images[0];

                if (BGImg == null)
                    return;

                //
                LicensePlateIMG = new Bitmap(BGImg.Width * 4, BGImg.Height * 4);
                LicensePlateTXT = "";

                LPtextBitmap = new Bitmap(BGImg.Width * 4, BGImg.Height * 4 + 30);
                //
                //Resize BG
                Image LicensePlateBGImg = SimpleResizeImage(BGImg, BGImg.Width * 4, BGImg.Height * 4);

                //Draw BG
                using (var canvas = Graphics.FromImage(LicensePlateIMG))
                {
                    canvas.DrawImage(LicensePlateBGImg, 0, 0);
                }
                //

                thisFontMap = MainForm.GlobalFontMap[SourceLPCountry];
            }

            UInt16 prevLetterUTF = 0;

            for (int i = 0; i < lpLength; i++)
            {
                string letter = _lpText.Substring(i, 1);
                string tag = "";

                if (letter == "<")
                {
                    int arrowIdxClose = _lpText.IndexOf('>', i + 1),
                        arrowIdxOpen = _lpText.IndexOf('<', i + 1);

                    if (arrowIdxClose != -1 && (arrowIdxOpen == -1 || arrowIdxClose < arrowIdxOpen) )
                    {
                        string tagtext = _lpText.Substring(i + 1, _lpText.IndexOf('>', i + 1) - i - 1).Trim(new char[] { ' ' });

                        //Detect tag type
                        int tagend = tagtext.IndexOf(' ');
                        int blockend = tagtext.Length - 1;

                        if (tagend != -1 && blockend > tagend)
                        {
                            tag = tagtext.Substring(0, tagend);
                        }
                        else if (tagend == -1)
                        {
                            tag = tagtext.Substring(0, ++blockend);
                        }

                        //
                        DetectTag(tag, tagtext);

                        //skip to ">"
                        int skip = _lpText.IndexOf('>', i + 1) - i;
                        i = i + skip;
                        continue;
                    }
                }
                else
                {
                    if (ValidLPcountry)
                    {
                        byte[] utf8bytes = Encoding.UTF8.GetBytes(letter);
                        ushort utf8number = utf8bytes[0];

                        if (thisFontMap.ContainsKey(utf8number))
                        {
                            decimal combXScale = scaleXmult * fontXscale * fontSupSubScale;
                            decimal combYScale = scaleYmult * fontYscale * fontSupSubScale;

                            if (thisFontMap[utf8number].LetterImage != null)
                            {
                                Int16 Kern = 0;
                                if (thisFontMap[utf8number].Kerning != null)
                                    if (thisFontMap[utf8number].Kerning.ContainsKey(prevLetterUTF))
                                        Kern = thisFontMap[utf8number].Kerning[prevLetterUTF];


                                Image tmpLetterImage = thisFontMap[utf8number].LetterImage;

                                int letterWidth = (int)Math.Floor(tmpLetterImage.Width * combXScale);
                                int lettetHeight = (int)Math.Floor(tmpLetterImage.Height * combYScale);
                                int xPoint = cursorPos + (int)Math.Ceiling(thisFontMap[utf8number].Left_offset * combXScale) + Kern;
                                int yPoint = BaseLine + (int)Math.Round(thisFontMap[utf8number].Top_offset * combYScale) + (int)Math.Round(vshift * scaleYmult);

                                if (yPoint < TopMostPxl)
                                    TopMostPxl = yPoint;

                                if (yPoint + lettetHeight > DownMostPxl)
                                    DownMostPxl = yPoint + lettetHeight;

                                //ReСolor
                                if (FontColor != Color.Black)
                                    tmpLetterImage = ReColor(tmpLetterImage, FontColor);
                                //

                                using (var canvas = Graphics.FromImage(LPtextBitmap))
                                {
                                    canvas.InterpolationMode = InterpolationMode.NearestNeighbor;
                                    canvas.SmoothingMode = SmoothingMode.HighQuality;

                                    var attributes = new ImageAttributes();
                                    attributes.SetWrapMode(WrapMode.TileFlipXY);

                                    var destination = new Rectangle(xPoint, yPoint, letterWidth, lettetHeight);

                                    canvas.DrawImage(tmpLetterImage, destination, 0, 0, tmpLetterImage.Width, tmpLetterImage.Height, GraphicsUnit.Pixel, attributes);

                                    canvas.Save();
                                }
                            }

                            cursorPos += (int)Math.Ceiling(thisFontMap[utf8number].Advance * combXScale);

                            if (cursorPos > rightMostPos)
                                rightMostPos = cursorPos;
                        }
                        else if (utf8number == 46)
                        {
                            cursorPos += 4;

                            if (cursorPos > rightMostPos)
                                rightMostPos = cursorPos;
                        }

                        LicensePlateTXT += letter;

                        prevLetterUTF = Encoding.UTF8.GetBytes(letter)[0];
                    }
                    else
                    {
                        //MakeText
                        LicensePlateTXT += letter;
                    }
                    //
                }
            }

            DrawTextToLP();
        }

        private void DrawTextToLP()
        {
            using (var canvas = Graphics.FromImage(LicensePlateIMG))
            {
                canvas.DrawImage(LPtextBitmap, leftOffset + allignOffset - (rightMostPos / 2), baseTopOffset[MainForm.GameType] - BaseLine - (TopMostPxl - BaseLine) + (LicensePlateIMG.Height / 2 - (DownMostPxl - TopMostPxl) / 2)); //Draw from center
            }
        }

        private void DrawWarning(string _input)
        {
            //Draw text
            using (var canvas = Graphics.FromImage(LicensePlateIMG))
            {
                canvas.SmoothingMode = SmoothingMode.HighQuality;

                Point point1 = new Point(0, 0);
                Point point2 = new Point(0, LicensePlateIMG.Height);
                Point point3 = new Point(LicensePlateIMG.Width, LicensePlateIMG.Height);
                Point point4 = new Point(LicensePlateIMG.Width, 0);

                Point[] Points = { point1, point2, point3, point4, };

                canvas.FillPolygon(new SolidBrush(Color.Black), Points);

                Pen linesPen = new Pen(Color.DarkOrange, 10);

                for (int i = 0; i < LicensePlateIMG.Width + LicensePlateIMG.Height; i++)
                {
                    Point p1 = new Point(i, -10);
                    Point p2 = new Point(i - LicensePlateIMG.Height, LicensePlateIMG.Height + 10);

                    canvas.DrawLine(linesPen, p1, p2);

                    i += 39;
                }

                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;

                GraphicsPath p = new GraphicsPath();
                p.AddString( _input, FontFamily.GenericMonospace, (int)FontStyle.Bold, canvas.DpiY * 40 / 72,
                    new RectangleF(0, 0, LicensePlateIMG.Width, LicensePlateIMG.Height), stringFormat);

                Pen outlinePen = new Pen(Color.Black, 10);

                canvas.DrawPath(outlinePen, p);

                canvas.DrawString(_input, new Font(FontFamily.GenericMonospace, 40, FontStyle.Bold), new SolidBrush(Color.White), new RectangleF(0, 0, LicensePlateIMG.Width, LicensePlateIMG.Height), stringFormat); //LicensePlateIMG.Width / 2, LicensePlateIMG.Height / 2
            }
        }

        private void DetectTag(string _tag, string _tagtext)
        {
            int datapos = 0, eqpos = 0, spacepos = 0;

            switch (_tag)
            {
                case "offset":
                    {
                        if (!ValidLPcountry)
                            break;

                        datapos = _tagtext.IndexOf("hshift");

                        if (datapos != -1)                        
                            cursorPos += (int)Math.Round(processData() * 3.2);

                        datapos = _tagtext.IndexOf("vshift");

                        if (datapos != -1)                        
                            vshift += processData();                        

                        int processData()
                        {
                            eqpos = _tagtext.IndexOf('=', datapos) + 1;
                            spacepos = _tagtext.IndexOf(' ', eqpos);

                            if (spacepos == -1)
                                spacepos = _tagtext.Length;

                            string Number = _tagtext.Substring(eqpos, spacepos - eqpos);

                            if (Number != "")
                            {
                                if (int.TryParse(Number, out int res))
                                {
                                    return res;
                                }
                            }

                            return 0;
                        }

                        break;
                    }

                case "img":
                    {
                        LicensePlateTXT += " ";
                        //
                        if (!ValidLPcountry)
                            break;

                        //img source
                        datapos = _tagtext.IndexOf("src");
                        eqpos = _tagtext.IndexOf('=', datapos) + 1;
                        spacepos = _tagtext.IndexOf(' ', eqpos);

                        if (spacepos == -1)
                            spacepos = _tagtext.Length;

                        string imgsource = _tagtext.Substring(eqpos, spacepos - eqpos);

                        if (string.IsNullOrEmpty(imgsource))
                            break;

                        //
                        int imgwidth = 0, imgheight = 0;

                        datapos = _tagtext.IndexOf("width");

                        if (datapos != -1)
                            imgwidth = processData();

                        if (imgwidth < 0)
                            break;
                        
                        //
                        datapos = _tagtext.IndexOf("height");

                        if (datapos != -1)
                            imgheight = processData();

                        if (imgheight < 0)
                            break;

                        int processData()
                        {
                            eqpos = _tagtext.IndexOf('=', datapos) + 1;
                            spacepos = _tagtext.IndexOf(' ', eqpos);

                            if (spacepos == -1)
                                spacepos = _tagtext.Length;

                            string Number = _tagtext.Substring(eqpos, spacepos - eqpos);

                            if (Number != "")
                            {
                                if (int.TryParse(Number, out int res))
                                {
                                    return res;
                                }
                            }

                            return -1;
                        }

                        //MAT
                        string gameFilepathMat = imgsource.Split(new string[] { "/material/ui/lp" }, StringSplitOptions.RemoveEmptyEntries).Last();

                        string imgpath = gameFilepathMat.Substring(0, gameFilepathMat.LastIndexOf('/') + 1);

                        //Side detect
                        if (gameFilepathMat.IndexOf("$SIDE$") != 1)
                        {
                            string side = "front";

                            if (LicensePlateType == LPtype.Trailer)
                                side = "rear";

                            gameFilepathMat = gameFilepathMat.Replace("$SIDE$", side);
                        }

                        //TOBJ
                        string tobjFilepath = "";
                        string imgToLoadMat = @"img\" + MainForm.GameType + @"\lp" + gameFilepathMat;

                        if (File.Exists(imgToLoadMat))
                        {
                            tobjFilepath = new SCSfiles.fileMAT(imgToLoadMat).texture;
                        }
                        else
                            break;

                        //DDS
                        string gameFilepathDds = "";
                        string imgToLoadTobj = @"img\" + MainForm.GameType + @"\lp" + imgpath + tobjFilepath;

                        if (File.Exists(imgToLoadTobj))
                        {
                            gameFilepathDds = new SCSfiles.fileTOBJ(imgToLoadTobj).texture_path;
                        }
                        else
                            break;

                        string ddsFilepath = gameFilepathDds.Split(new string[] { "/material/ui/lp" }, StringSplitOptions.RemoveEmptyEntries).Last();

                        //Load img
                        string tmpImgPath = @"img\" + MainForm.GameType + @"\lp" + ddsFilepath;
                        var tmpTuple = Utilities.Graphics_TSSET.ddsImgLoader(new string[] { tmpImgPath });

                        Image tmpImg;

                        if (tmpTuple.validity[0])
                            tmpImg = tmpTuple.images[0];
                        else
                            tmpImg = null;

                        //Draw img
                        if (tmpImg != null)
                        {
                            int imgSpacing = 0;

                            if (imgwidth == 0)
                            {
                                imgwidth = tmpImg.Width;
                            }

                            if (imgheight == 0)
                            {
                                imgheight = tmpImg.Height;
                            }

                            int letterWidth = (int)Math.Ceiling(imgwidth * scaleXmult);
                            int lettetHeight = (int)Math.Ceiling(imgheight * scaleYmult);
                            int xPoint = cursorPos + (int)Math.Ceiling(imgSpacing * scaleXmult);
                            int yPoint = BaseLine + (int)Math.Ceiling(vshift * scaleYmult);

                            if (yPoint < TopMostPxl)
                                TopMostPxl = yPoint - 4;

                            if (yPoint + lettetHeight > DownMostPxl)
                                DownMostPxl = yPoint + lettetHeight;

                            using (var canvas = Graphics.FromImage(LPtextBitmap))
                            {
                                canvas.InterpolationMode = InterpolationMode.NearestNeighbor;
                                canvas.SmoothingMode = SmoothingMode.HighQuality;

                                var attributes = new ImageAttributes();
                                attributes.SetWrapMode(WrapMode.TileFlipXY);

                                var destination = new Rectangle(xPoint, yPoint, letterWidth, lettetHeight);

                                canvas.DrawImage(tmpImg, destination, 0, 0, tmpImg.Width, tmpImg.Height, GraphicsUnit.Pixel, attributes);
                                canvas.Save();
                            }

                            cursorPos = cursorPos + (int)Math.Ceiling((imgwidth + imgSpacing * 2) * scaleXmult);

                            if (cursorPos > rightMostPos)
                                rightMostPos = cursorPos;
                        }

                        break;
                    }

                case "font":
                    {
                        if (!ValidLPcountry)
                            break;

                        //<font xscale=0.8 yscale=0.8>
                        datapos = _tagtext.IndexOf("xscale");

                        if (datapos != -1)                        
                            fontXscale = processData();                        

                        datapos = _tagtext.IndexOf("yscale");

                        if (datapos != -1)                        
                            fontYscale = processData();                        

                        decimal processData()
                        {
                            eqpos = _tagtext.IndexOf('=', datapos) + 1;
                            spacepos = _tagtext.IndexOf(' ', eqpos);

                            if (spacepos == -1)
                                spacepos = _tagtext.Length;

                            string Number = _tagtext.Substring(eqpos, spacepos - eqpos);

                            if (Number != "")
                            {
                                if (decimal.TryParse(Number, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal res))
                                {
                                    return res;
                                }
                            }

                            return 1;
                        }

                        break;
                    }

                case "/font":
                    {
                        fontXscale = 1;
                        fontYscale = 1;

                        break;
                    }

                case "color":
                    {
                        if (!ValidLPcountry)
                            break;

                        //value = FF0009C5 A B G R
                        datapos = _tagtext.IndexOf("value");
                        if (datapos != -1)
                        {
                            eqpos = _tagtext.IndexOf('=', datapos) + 1;
                            spacepos = _tagtext.IndexOf(' ', datapos);

                            if (spacepos == -1)
                                spacepos = _tagtext.Length;

                            string hexScsColorString = _tagtext.Substring(eqpos, spacepos - eqpos);

                            if (hexScsColorString.Length == 8)
                            {
                                int[] hexColorParts = SplitNConvertSSCHexColor(hexScsColorString, 2).ToArray();

                                int tmpAlpha = hexColorParts[0], tmpRed = hexColorParts[3], tmpGreen = hexColorParts[2], tmpBlue = hexColorParts[1];

                                FontColor = Color.FromArgb(tmpAlpha, tmpRed, tmpGreen, tmpBlue);
                            }
                        }

                        break;
                    }

                case "align":
                    {
                        if (!ValidLPcountry)
                            break;

                        //<align right=96> <align left=104>
                        //choose column (left or right) and set left offset
                        //Set leftoffset
                        //Set internal offset

                        bool left = true;

                        if (_tagtext.IndexOf("right") != -1)
                            left = false;

                        if (left)
                            datapos = _tagtext.IndexOf("left");
                        else
                            datapos = _tagtext.IndexOf("right");

                        if (datapos != -1)
                        {
                            eqpos = _tagtext.IndexOf('=', datapos);

                            if (eqpos != -1 && eqpos >= datapos + 4) 
                            {
                                eqpos++;

                                spacepos = _tagtext.IndexOf(' ', eqpos);

                                if (spacepos == -1)
                                    spacepos = _tagtext.Length;

                                string offsetNumber = _tagtext.Substring(eqpos, spacepos - eqpos);

                                if (offsetNumber != "")
                                {
                                    if (int.TryParse(offsetNumber, out int res))
                                    {
                                        allignOffset = (int)Math.Round(res * 1.6, 0);

                                        if (left)
                                            leftOffset = baseAllignOffset[MainForm.GameType] + 232;
                                    }
                                }
                            }
                        }

                        break;
                    }

                case "/align":
                    {
                        if (!ValidLPcountry)
                            break;

                        //start over
                        DrawTextToLP(); //Draw LP
                                        //make new lptext
                        LPtextBitmap = new Bitmap(LicensePlateIMGwidth, LicensePlateIMGheight + 30);

                        cursorPos = 0;
                        rightMostPos = cursorPos;

                        allignOffset = baseLeftOffset[MainForm.GameType];

                        break;
                    }

                case "ret":
                    {
                        LicensePlateTXT += " ";
                        //
                        if (!ValidLPcountry)
                            break;

                        //start over
                        DrawTextToLP(); //Draw LP
                                        //make new lptext
                        LPtextBitmap = new Bitmap(LicensePlateIMGwidth, LicensePlateIMGheight + 30);
                        cursorPos = 0;
                        rightMostPos = cursorPos;
                        break;
                    }

                case "sup":
                    {
                        // 75% size 44% up
                        fontSupSubScale = 0.6m;
                        vshift += 2;
                        break;
                    }

                case "/sup":
                    {
                        fontSupSubScale = 1m;
                        vshift -= 2;
                        break;
                    }

                case "sub":
                    {
                        // 75% size 16% down
                        fontSupSubScale = 0.75m;
                        vshift -= 5;
                        break;
                    }

                case "/sub":
                    {
                        fontSupSubScale = 1m;
                        vshift += 5;
                        break;
                    }
            }
        }

        private void CreateFontMap(string _fontDataPath)
        {
            if (!File.Exists(_fontDataPath))
                return;

            //Check if already done
            if (MainForm.GlobalFontMap.ContainsKey(SourceLPCountry))
                return;

            //Game type
            string gametype = MainForm.GameType;
            string IMGpath = "";

            //Img Font map
            Dictionary<UInt16, SCSFontLetter> thisFontMap = new Dictionary<UInt16, SCSFontLetter>();

            string fontimg = "";

            //Read
            try
            {
                string[] tempFile = File.ReadAllLines(_fontDataPath);

                for (int i = 0; i < tempFile.Length; i++)
                {
                    string line = tempFile[i];
                    //line_spacing: 0  # suggested number of pixels to put between lines

                    //default_scale: 1.000000  # default font scale in reference resolution

                    //Font image to load
                    //Can be multiple imgs
                    //image:/font/license_plate/netherlands_0.mat, 128, 128
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

                        SCSFontLetter letter = new SCSFontLetter(dataparts[1], dataparts[2], dataparts[3], dataparts[4], dataparts[5], dataparts[6], dataparts[7]);

                        thisFontMap.Add(Convert.ToUInt16(dataparts[0], 16), letter);
                    }

                    //Kerning
                    //kern: x0059, x0042, -2      # 'Y' -> 'B'
                    //kern: x0059, x0054, -1      # 'Y' -> 'T'
                    if (line.StartsWith("kern:"))
                    {
                        string[] dataparts = line.Split(new char[] { ':', '#' })[1].Trim(' ').Split(new char[] { ',' });

                        UInt16 targetL = Convert.ToUInt16(dataparts[1].Trim(' ').Remove(0, 1), 16), prevL = Convert.ToUInt16(dataparts[0].Trim(' ').Remove(0, 1), 16);
                        Int16 Kern = Convert.ToInt16(dataparts[2]);

                        if(thisFontMap[targetL].Kerning == null)
                            thisFontMap[targetL].Kerning = new Dictionary<ushort, short>();

                        thisFontMap[targetL].Kerning.Add(prevL, Kern);
                    }
                }
            }
            catch
            {
                //FormMain.LogWriter("Font file is missing");
            }

            //Load Font 
            IMGpath = @"img\" + gametype + @"\lpFont\" + fontimg + @".dds";
            Image FontImg = null;

            if (File.Exists(IMGpath))
                FontImg = Utilities.Graphics_TSSET.ddsImgLoader(IMGpath).images[0];

            if (FontImg == null)
                return;

            //Create font map
            foreach (KeyValuePair< UInt16, SCSFontLetter> letter in thisFontMap)
            {
                if (letter.Value.Width == 0 || letter.Value.Height == 0)
                { }
                else
                {
                    Rectangle rect = new Rectangle(letter.Value.P_x, letter.Value.P_y, letter.Value.Width, letter.Value.Height);

                    Bitmap tmpLetter = new Bitmap(FontImg).Clone(rect, PixelFormat.Format32bppArgb);

                    letter.Value.LetterImage = tmpLetter;
                }
            }

            MainForm.GlobalFontMap.Add(SourceLPCountry, thisFontMap);
        }
        
        public static Image SimpleResizeImage(Image _inputImage, int _newWidth, int _newHeight)
        {
            //Image newImage = new Bitmap(_inputImage, _newWidth, _newHeight);

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

        static IEnumerable<int> SplitNConvertSSCHexColor(string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize).Select(i => Convert.ToInt32(str.Substring(i * chunkSize, chunkSize), 16));
        }

        private Image ReColor(Image _inpulLetter, Color _newColor )
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
