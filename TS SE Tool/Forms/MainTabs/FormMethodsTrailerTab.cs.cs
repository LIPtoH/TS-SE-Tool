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
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TS_SE_Tool
{
    public partial class FormMain
    {
        //User Trailer tab
        private void CreateTrailerPanelControls()
        {
            CreateTrailerPanelProgressBars();
            CreateTrailerPanelButtons();
        }

        private void CreateTrailerPanelProgressBars()
        {
            int pHeight = RepairImg.Height, pOffset = 5, lOffset = 60, pSizeW = 300;
            int pSkillsNameHeight = 32, pSkillsNameWidth = 32, pSkillsNameOffset = 5, pSkillsNamelOffset = 12;

            string[] toolskillimgtooltip = new string[] { "Cargo", "Body", "Chassis", "Wheels" };
            Label slabel;
            Panel Ppanel;

            for (int i = 0; i < 4; i++)
            {
                slabel = new Label();
                groupBoxUserTrailerTrailerDetails.Controls.Add(slabel);
                slabel.Name = "labelTrailerPartName" + toolskillimgtooltip[i];
                slabel.Location = new Point(pSkillsNamelOffset, 23 + (pSkillsNameHeight + pSkillsNameOffset * 3) * i);
                slabel.Text = toolskillimgtooltip[i];
                slabel.AutoSize = true;

                Panel imgpanel = new Panel();
                groupBoxUserTrailerTrailerDetails.Controls.Add(imgpanel);

                imgpanel.Parent = groupBoxUserTrailerTrailerDetails;
                imgpanel.Location = new Point(pSkillsNamelOffset, 23 + 14 + (pSkillsNameHeight + pSkillsNameOffset * 3) * i);
                imgpanel.BorderStyle = BorderStyle.None;
                imgpanel.Size = new Size(pSkillsNameWidth, pSkillsNameHeight);
                imgpanel.Name = "TrailerPartImg" + i.ToString();

                Bitmap bgimg = new Bitmap(TrailerPartsImg[i], pSkillsNameHeight, pSkillsNameWidth);
                imgpanel.BackgroundImage = bgimg;

                //Panel 
                Ppanel = new Panel();
                groupBoxUserTrailerTrailerDetails.Controls.Add(Ppanel);

                Ppanel.Parent = groupBoxUserTrailerTrailerDetails;
                Ppanel.Location = new Point(lOffset, imgpanel.Location.Y);
                Ppanel.BorderStyle = BorderStyle.FixedSingle;
                Ppanel.Size = new Size(pSizeW, RepairImg.Height);
                Ppanel.Name = "progressbarTrailerPart" + i.ToString();

                Button button = new Button();
                groupBoxUserTrailerTrailerDetails.Controls.Add(button);

                button.Parent = groupBoxUserTrailerTrailerDetails;
                button.Location = new Point(Ppanel.Location.X + Ppanel.Width + pOffset, Ppanel.Location.Y);
                button.FlatStyle = FlatStyle.Flat;
                button.Size = new Size(RepairImg.Height, RepairImg.Height);
                button.Name = "buttonTrailerElRepair" + i.ToString();
                button.BackgroundImage = RepairImg;
                button.BackgroundImageLayout = ImageLayout.Zoom;
                button.Text = "";
                button.FlatAppearance.BorderSize = 0;
                button.Click += new EventHandler(buttonTrailerElRepair_Click);
            }
        }

        private void CreateTrailerPanelButtons()
        {
            int pHeight = RepairImg.Height, pOffset = 5, tOffset = comboBoxUserTrailerCompanyTrailers.Location.Y;
            int topbutoffset = comboBoxUserTrailerCompanyTrailers.Location.X + comboBoxUserTrailerCompanyTrailers.Width + pOffset;

            //tableLayoutPanel13

            Button buttonR = new Button();
            //tabPageTrailer.Controls.Add(buttonR);
            tableLayoutPanelUserTruckTrailer.Controls.Add(buttonR, 3, 1);
            buttonR.Location = new Point(topbutoffset, tOffset);
            buttonR.FlatStyle = FlatStyle.Flat;
            buttonR.Size = new Size(RepairImg.Height, RepairImg.Height);
            buttonR.Name = "buttonTrailerRepair";
            buttonR.BackgroundImage = RepairImg;
            buttonR.BackgroundImageLayout = ImageLayout.Zoom;
            buttonR.Text = "";
            buttonR.FlatAppearance.BorderSize = 0;
            buttonR.Click += new EventHandler(buttonTrailerRepair_Click);
            buttonR.Dock = DockStyle.Fill;

            Button buttonInfo = new Button();
            //tabPageTrailer.Controls.Add(buttonInfo);
            tableLayoutPanelUserTruckTrailer.Controls.Add(buttonInfo, 0, 1);
            //buttonInfo.Location = new Point(labelUserTrailerTrailer.Location.X + (comboBoxUserTrailerCompanyTrailers.Location.X - labelUserTrailerTrailer.Location.X - CutomizeImg.Width - pOffset) / 2, buttonUserTruckSelectCurrent.Location.Y + pOffset);
            buttonInfo.FlatStyle = FlatStyle.Flat;
            buttonInfo.Size = new Size(CutomizeImg.Width, CutomizeImg.Height);
            buttonInfo.Name = "buttonTrailerInfo";
            buttonInfo.BackgroundImage = ConvertBitmapToGrayscale(CutomizeImg);
            buttonInfo.BackgroundImageLayout = ImageLayout.Zoom;
            buttonInfo.Text = "";
            buttonInfo.FlatAppearance.BorderSize = 0;
            buttonInfo.Enabled = false;
            buttonInfo.Dock = DockStyle.Fill;
        }

        public void buttonTrailerRepair_Click(object sender, EventArgs e)
        {
            string[] PartList = { "trailerdata", "body", "chassis", "tire" };
            string trailerNameless = "";

            trailerNameless = comboBoxUserTrailerCompanyTrailers.SelectedValue.ToString();

            StartTrailerParts:

            foreach (string tempPart in PartList)
            {
                foreach (UserCompanyTruckDataPart temp in UserTrailerDictionary[trailerNameless].Parts.FindAll(x => x.PartType == tempPart))
                {
                    string partNameless = temp.PartNameless;

                    int i = 0;

                    foreach (string temp2 in temp.PartData)
                    {
                        if (temp2.StartsWith(" wear:"))
                        {
                            UserTrailerDictionary[trailerNameless].Parts.Find(x => x.PartNameless == partNameless).PartData[i] = " wear: 0";
                            break;
                        }
                        else
                        if (temp2.StartsWith(" cargo_damage:"))
                        {
                            UserTrailerDictionary[trailerNameless].Parts.Find(x => x.PartType == "trailerdata").PartData[i] = " cargo_damage: 0";
                            break;
                        }
                        i++;
                    }
                }
            }

            UserCompanyTruckDataPart slavetrailer = UserTrailerDictionary[trailerNameless].Parts.Find(x => x.PartType == "slavetrailer");

            if (slavetrailer != null)
            {
                trailerNameless = slavetrailer.PartNameless;
                goto StartTrailerParts;
            }

            UpdateTrailerPanelProgressBars();
        }

        public void buttonTrailerElRepair_Click(object sender, EventArgs e)
        {
            Button curbtn = sender as Button;
            int bi = Convert.ToByte(curbtn.Name.Substring(21));

            string[] PartList = { "trailerdata", "body", "chassis", "tire" };
            string trailerNameless = "";

            trailerNameless = comboBoxUserTrailerCompanyTrailers.SelectedValue.ToString();

            StartTrailerParts:

            foreach (UserCompanyTruckDataPart temp in UserTrailerDictionary[trailerNameless].Parts.FindAll(x => x.PartType == PartList[bi]))
            {
                string partNameless = temp.PartNameless;

                int i = 0;

                foreach (string temp2 in temp.PartData)
                {
                    if (temp2.StartsWith(" wear:"))
                    {
                        UserTrailerDictionary[trailerNameless].Parts.Find(x => x.PartNameless == partNameless).PartData[i] = " wear: 0";
                        break;
                    }
                    else
                    if (temp2.StartsWith(" cargo_damage:"))
                    {
                        UserTrailerDictionary[trailerNameless].Parts.Find(x => x.PartType == "trailerdata").PartData[i] = " cargo_damage: 0";
                        break;
                    }
                    i++;
                }
            }

            UserCompanyTruckDataPart slavetrailer = UserTrailerDictionary[trailerNameless].Parts.Find(x => x.PartType == "slavetrailer");

            if (slavetrailer != null)
            {
                trailerNameless = slavetrailer.PartNameless;
                goto StartTrailerParts;
            }

            UpdateTrailerPanelProgressBars();
        }

        private void UpdateTrailerPanelProgressBars()
        {
            UserTrailerDictionary.TryGetValue(comboBoxUserTrailerCompanyTrailers.SelectedValue.ToString(), out UserCompanyTruckData SelectedUserCompanyTrailer);

            if (SelectedUserCompanyTrailer == null)
                return;

            for (int i = 0; i < 4; i++)
            {
                Panel pnl = null;
                string pnlname = "progressbarTrailerPart" + i.ToString();
                if (groupBoxUserTrailerTrailerDetails.Controls.ContainsKey(pnlname))
                {
                    pnl = groupBoxUserTrailerTrailerDetails.Controls[pnlname] as Panel;
                }

                if (pnl != null)
                {
                    UserCompanyTruckDataPart tempPart = null;

                    switch (i)
                    {
                        case 0:
                            tempPart = SelectedUserCompanyTrailer.Parts.Find(xp => xp.PartType == "trailerdata");
                            break;
                        case 1:
                            tempPart = SelectedUserCompanyTrailer.Parts.Find(xp => xp.PartType == "body");
                            break;
                        case 2:
                            tempPart = SelectedUserCompanyTrailer.Parts.Find(xp => xp.PartType == "chassis");
                            break;
                        case 3:
                            tempPart = SelectedUserCompanyTrailer.Parts.Find(xp => xp.PartType == "tire");
                            break;
                    }

                    string wear = "0";
                    decimal _wear = 0;

                    if (tempPart != null)
                        try
                        {
                            List<string> TruckDataPart = tempPart.PartData;
                            wear = TruckDataPart.Find(xl => xl.StartsWith(" wear:") || xl.StartsWith(" cargo_damage:")).Split(new char[] { ' ' })[2];
                        }
                        catch
                        { }

                    if (wear != "0" && wear != "1")
                        try
                        {
                            _wear = HexFloatToDecimalFloat(wear);
                        }
                        catch
                        {
                            _wear = 1;
                        }
                    else
                    if (wear == "1")
                        _wear = 1;

                    SolidBrush ppen = new SolidBrush(GetProgressbarColor(_wear));

                    int x = 0, y = 0, pnlwidth = (int)(pnl.Width * (1 - _wear));

                    Bitmap progress = new Bitmap(pnl.Width, pnl.Height);

                    Graphics g = Graphics.FromImage(progress);
                    g.FillRectangle(ppen, x, y, pnlwidth, pnl.Height);

                    int fontSize = 12;
                    StringFormat sf = new StringFormat();
                    sf.LineAlignment = StringAlignment.Center;
                    sf.Alignment = StringAlignment.Center;

                    GraphicsPath p = new GraphicsPath();
                    p.AddString(
                        ((int)((1 - _wear) * 100)).ToString() + " %",   // text to draw
                        FontFamily.GenericSansSerif,                    // or any other font family
                        (int)FontStyle.Bold,                            // font style (bold, italic, etc.)
                        g.DpiY * fontSize / 72,                         // em size
                        new Rectangle(0, 0, pnl.Width, pnl.Height),     // location where to draw text
                        sf);                                            // set options here (e.g. center alignment)
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.FillPath(Brushes.Black, p);
                    g.DrawPath(Pens.Black, p);

                    pnl.BackgroundImage = progress;
                }
            }

            string lctxt = "";
            labelLicensePlateTr.Text = "";
            int stindex = 0, endindex = 0;

            string LicensePlate = SelectedUserCompanyTrailer.Parts.Find(xp => xp.PartType == "trailerdata").PartData.Find(xl => xl.StartsWith(" license_plate:")).Split(new char[] { '"' })[1];

            for (int i = 0; i < LicensePlate.Length; i++)//SelectedUserCompanyTruck.LicensePlate.Length; i++)
            {
                if (LicensePlate[i] == '<')
                {
                    endindex = i;
                    lctxt += LicensePlate.Substring(stindex, endindex - stindex);
                }
                else if (LicensePlate[i] == '>')
                {
                    stindex = i + 1;
                }
                else if (i == LicensePlate.Length - 1)
                {
                    endindex = i + 1;
                    lctxt += LicensePlate.Substring(stindex, endindex - stindex);
                }
            }
            if (lctxt.Split(new char[] { '|' }).Length > 1)
                labelLicensePlateTr.Text = lctxt.Split(new char[] { '|' })[0] + " Country " + lctxt.Split(new char[] { '|' })[1];
            else
                labelLicensePlateTr.Text = lctxt.Split(new char[] { '|' })[0];
        }

        private void FillUserCompanyTrailerList()
        {

            DataTable combDT = new DataTable();
            DataColumn dc = new DataColumn("UserTrailerkNameless", typeof(string));
            combDT.Columns.Add(dc);

            dc = new DataColumn("UserTrailerName", typeof(string));
            combDT.Columns.Add(dc);


            combDT.Rows.Add("null", "-- NONE --"); //none

            foreach (KeyValuePair<string, UserCompanyTruckData> UserTrailer in UserTrailerDictionary)
            {
                if (UserTrailer.Value.Main)
                {
                    string trailername = "";

                    if (UserTrailerDictionary[UserTrailer.Key].Users)
                        trailername = "[U] ";
                    else
                        trailername = "[Q] ";

                    trailername += UserTrailer.Key;

                    string trailerdef = UserTrailerDictionary[UserTrailer.Key].Parts.Find(x => x.PartType == "trailerdef").PartNameless;

                    /*
                    try
                    {
                        string source_name = UserTrailerDefDictionary[trailerdef].Find(x => x.StartsWith(" source_name:")).Split(':')[1];

                        if (!source_name.Contains("null"))
                        {
                            trailername += source_name.Split(new char[] { '"' })[1].Trim(new char[] { ' ' }) + " | ";
                        }
                    }
                    catch { }
                    */

                    trailername += " [ ";

                    if (UserTrailerDefDictionary.Count > 0)
                    {
                        if (UserTrailerDefDictionary.ContainsKey(trailerdef))
                        {
                            string[] trailerDefPropertys = { "body_type", "axles", "chain_type" };
                            string[] trailerDefExtra = { "{0}", "{0} axles", "{0}" };

                            int iCounter = 0;
                            List<string> CurTrailerDef = UserTrailerDefDictionary[trailerdef];

                            bool wasfound = false;

                            foreach (string Property in trailerDefPropertys)
                            {
                                try
                                {
                                    string tmp = CurTrailerDef.Find(x => x.StartsWith(" " + Property + ":")).Split(':')[1].Trim(new char[] { ' ' });

                                    if (wasfound)
                                        trailername += " ";
                                    trailername += String.Format(trailerDefExtra[iCounter], tmp);

                                    wasfound = true;
                                }
                                catch { wasfound = false; }
                                iCounter++;
                            }
                        }
                        else
                        {
                            trailername += trailerdef;
                        }
                    }
                    else
                    {
                        trailername += trailerdef;
                    }

                    trailername += " ]";

                    combDT.Rows.Add(UserTrailer.Key, trailername);
                }
            }

            if (combDT.Rows.Count > 0)
            {
                //combDT.DefaultView.Sort = "UserTrailerName ASC";
                comboBoxUserTrailerCompanyTrailers.Enabled = true;
                comboBoxUserTrailerCompanyTrailers.ValueMember = "UserTrailerkNameless";
                comboBoxUserTrailerCompanyTrailers.DisplayMember = "UserTrailerName";
                comboBoxUserTrailerCompanyTrailers.DataSource = combDT;
                comboBoxUserTrailerCompanyTrailers.SelectedValue = PlayerDataV.UserCompanyAssignedTrailer;
            }
            else
            {
                comboBoxUserTrailerCompanyTrailers.Enabled = false;
            }
        }

        private void comboBoxCompanyTrailers_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmbbx = sender as ComboBox;

            if (cmbbx.SelectedIndex != -1 && cmbbx.SelectedValue.ToString() != "null")
            {
                UpdateTrailerPanelProgressBars();
                tableLayoutPanelUserTruckTrailer.Controls.Find("buttonTrailerRepair", false)[0].Enabled = true;
                groupBoxUserTrailerTrailerDetails.Enabled = true;
                groupBoxUserTrailerShareTrailerSettings.Enabled = true;

            }
            else
            {
                tableLayoutPanelUserTruckTrailer.Controls.Find("buttonTrailerRepair", false)[0].Enabled = false;
                groupBoxUserTrailerTrailerDetails.Enabled = false;
                groupBoxUserTrailerShareTrailerSettings.Enabled = false;
            }
        }

        private void buttonUserTrailerSelectCurrent_Click(object sender, EventArgs e)
        {
            comboBoxUserTrailerCompanyTrailers.SelectedValue = PlayerDataV.UserCompanyAssignedTrailer;
        }

        private void buttonUserTrailerSwitchCurrent_Click(object sender, EventArgs e)
        {
            PlayerDataV.UserCompanyAssignedTrailer = comboBoxUserTrailerCompanyTrailers.SelectedValue.ToString();
        }

        //end User Trailer tab
    }
}