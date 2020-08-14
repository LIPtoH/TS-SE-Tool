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
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace TS_SE_Tool
{
    public partial class FormMain
    {
        //User Trailer tab
        private void CreateTrailerPanelControls()
        {
            CreateTrailerPanelMainButtons();
            CreateTrailerPanelPartsControls();
        }

        private void CreateTrailerPanelMainButtons()
        {
            int pHeight = RepairImg.Height, pOffset = 5, tOffset = comboBoxUserTrailerCompanyTrailers.Location.Y;
            int topbutoffset = comboBoxUserTrailerCompanyTrailers.Location.X + comboBoxUserTrailerCompanyTrailers.Width + pOffset;

            Button buttonR = new Button();
            tableLayoutPanelUserTrailerControls.Controls.Add(buttonR, 1, 0);
            buttonR.FlatStyle = FlatStyle.Flat;
            buttonR.Size = new Size(RepairImg.Height, RepairImg.Height);
            buttonR.Name = "buttonTrailerRepair";
            buttonR.BackgroundImage = RepairImg;
            buttonR.BackgroundImageLayout = ImageLayout.Zoom;
            buttonR.Text = "";
            buttonR.FlatAppearance.BorderSize = 0;
            buttonR.Dock = DockStyle.Fill;
            buttonR.Click += new EventHandler(buttonTrailerRepair_Click);

            Button buttonInfo = new Button();
            tableLayoutPanelUserTrailerControls.Controls.Add(buttonInfo, 3, 0);
            buttonInfo.FlatStyle = FlatStyle.Flat;
            buttonInfo.Size = new Size(CustomizeImg.Width, CustomizeImg.Height);
            buttonInfo.Name = "buttonTrailerInfo";
            buttonInfo.BackgroundImage = ConvertBitmapToGrayscale(CustomizeImg);
            buttonInfo.BackgroundImageLayout = ImageLayout.Zoom;
            buttonInfo.Text = "";
            buttonInfo.FlatAppearance.BorderSize = 0;
            buttonInfo.Enabled = false;
            buttonInfo.Dock = DockStyle.Fill;
        }

        private void CreateTrailerPanelPartsControls()
        {
            int pSkillsNameHeight = 32, pSkillsNameWidth = 32;

            string[] toolskillimgtooltip = new string[] { "Cargo", "Body", "Chassis", "Wheels" };
            Label partLabel, partnameLabel;
            Panel pbPanel;

            for (int i = 0; i < 4; i++)
            {
                //Create table layout
                TableLayoutPanel tbllPanel = new TableLayoutPanel();
                tableLayoutPanelTrailerDetails.Controls.Add(tbllPanel, 0, i);
                tbllPanel.Dock = DockStyle.Fill;
                tbllPanel.Margin = new Padding(0);
                //
                tbllPanel.Name = "tableLayoutPanelTrailerDetails" + toolskillimgtooltip[i];

                tbllPanel.ColumnCount = 3;
                tbllPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40F));
                tbllPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
                tbllPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40F));
                tbllPanel.RowCount = 2;
                tbllPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 18F));
                tbllPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
                //

                FlowLayoutPanel flowPanel = new FlowLayoutPanel();
                flowPanel.FlowDirection = FlowDirection.LeftToRight;
                flowPanel.Margin = new Padding(0);
                tbllPanel.SetColumnSpan(flowPanel, 2);
                tbllPanel.Controls.Add(flowPanel, 0, 0);
                //
                //Part type
                partLabel = new Label();
                partLabel.Name = "labelTrailerPartName" + toolskillimgtooltip[i];
                partLabel.Text = toolskillimgtooltip[i];
                partLabel.AutoSize = true;
                partLabel.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom;
                partLabel.MinimumSize = new Size(36, partLabel.Height);

                flowPanel.Controls.Add(partLabel);

                //Part name
                partnameLabel = new Label();
                partnameLabel.Name = "labelTrailerPartDataName" + i;
                partnameLabel.Text = "";
                partnameLabel.AutoSize = true;
                partnameLabel.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom;

                flowPanel.Controls.Add(partnameLabel);

                //Part type image
                Panel imgpanel = new Panel();
                imgpanel.BorderStyle = BorderStyle.None;
                imgpanel.Size = new Size(pSkillsNameWidth, pSkillsNameHeight);
                imgpanel.Margin = new Padding(1);
                imgpanel.Name = "TrailerPartImg" + i.ToString();

                Bitmap bgimg = new Bitmap(TrailerPartsImg[i], pSkillsNameHeight, pSkillsNameWidth);
                imgpanel.BackgroundImage = bgimg;
                tbllPanel.Controls.Add(imgpanel, 0, 1);

                //Progress bar panel 
                pbPanel = new Panel();
                pbPanel.BorderStyle = BorderStyle.FixedSingle;
                pbPanel.Name = "progressbarTrailerPart" + i.ToString();
                pbPanel.Dock = DockStyle.Fill;
                tbllPanel.Controls.Add(pbPanel, 1, 1);

                //Repair button
                Button button = new Button();
                button.FlatStyle = FlatStyle.Flat;
                button.Dock = DockStyle.Fill;
                button.Margin = new Padding(1);

                button.Name = "buttonTrailerElRepair" + i.ToString();
                button.BackgroundImage = RepairImg;
                button.BackgroundImageLayout = ImageLayout.Zoom;
                button.Text = "";
                button.FlatAppearance.BorderSize = 0;
                button.Click += new EventHandler(buttonTrailerElRepair_Click);

                tbllPanel.Controls.Add(button, 2, 1);
            }
            //License plate
            Label labelPlate = new Label();
            labelPlate.Name = "labelUserTrailerLicensePlate";
            labelPlate.Text = "License plate";
            labelPlate.Margin = new Padding(0);
            labelPlate.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
            labelPlate.TextAlign = ContentAlignment.MiddleCenter;

            Label lcPlate = new Label();
            lcPlate.Name = "labelLicensePlateTrailer";
            lcPlate.Text = "A 000 AA";
            lcPlate.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom;
            lcPlate.Dock = DockStyle.Fill;
            lcPlate.TextAlign = ContentAlignment.MiddleLeft;

            tableLayoutPanelTrailerLP.Controls.Add(labelPlate, 0, 0);
            tableLayoutPanelTrailerLP.Controls.Add(lcPlate, 1, 0);            
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
                string pnlname = "progressbarTrailerPart" + i.ToString(), labelPartName = "labelTrailerPartDataName" + i.ToString();

                Panel pbPanel = groupBoxUserTrailerTrailerDetails.Controls.Find(pnlname, true).FirstOrDefault() as Panel;

                Label pnLabel = groupBoxUserTrailerTrailerDetails.Controls.Find(labelPartName, true).FirstOrDefault() as Label;

                if (pbPanel != null)
                {
                    List<string> DataPart = null;

                    switch (i)
                    {
                        case 0:
                            DataPart = SelectedUserCompanyTrailer.Parts.Find(xp => xp.PartType == "trailerdata").PartData;
                            break;
                        case 1:
                            DataPart = SelectedUserCompanyTrailer.Parts.Find(xp => xp.PartType == "body").PartData;
                            break;
                        case 2:
                            DataPart = SelectedUserCompanyTrailer.Parts.Find(xp => xp.PartType == "chassis").PartData;
                            break;
                        case 3:
                            DataPart = SelectedUserCompanyTrailer.Parts.Find(xp => xp.PartType == "tire").PartData;
                            break;
                    }

                    string wear = "0";

                    if (DataPart != null)
                    {
                        if (i !=0 && pnLabel != null)
                        {
                            pnLabel.Text = DataPart.Find(xl => xl.StartsWith(" data_path:")).Split(new char[] { '"' })[1].Split(new char[] { '/' }).Last().Split(new char[] { '.' })[0];
                        }

                        wear = DataPart.Find(xl => xl.StartsWith(" wear:") || xl.StartsWith(" cargo_damage:")).Split(new char[] { ' ' })[2];
                    }
                    else
                    {
                        pnLabel.Text = "!! Part not found !!";
                    }

                    decimal _wear = 0;

                    if (wear != "0" && wear != "1")
                        _wear = Utilities.NumericUtilities.HexFloatToDecimalFloat(wear);
                    else
                    if (wear == "1")
                        _wear = 1;

                    SolidBrush ppen = new SolidBrush(GetProgressbarColor(_wear));

                    int x = 0, y = 0, pnlwidth = (int)(pbPanel.Width * (1 - _wear));

                    Bitmap progress = new Bitmap(pbPanel.Width, pbPanel.Height);

                    Graphics g = Graphics.FromImage(progress);
                    g.FillRectangle(ppen, x, y, pnlwidth, pbPanel.Height);

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
                        new Rectangle(0, 0, pbPanel.Width, pbPanel.Height),     // location where to draw text
                        sf);                                            // set options here (e.g. center alignment)
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.FillPath(Brushes.Black, p);
                    g.DrawPath(Pens.Black, p);

                    pbPanel.BackgroundImage = progress;
                }
            }

            string lctxt = "";
            int stindex = 0, endindex = 0;

            string LicensePlate = SelectedUserCompanyTrailer.Parts.Find(xp => xp.PartType == "trailerdata").PartData.Find(xl => xl.StartsWith(" license_plate:")).Split(new char[] { '"' })[1];

            for (int i = 0; i < LicensePlate.Length; i++)
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

            Label lpText = groupBoxUserTrailerTrailerDetails.Controls.Find("labelLicensePlateTrailer", true).FirstOrDefault() as Label;

            if (lctxt.Split(new char[] { '|' }).Length > 1)
                lpText.Text = lctxt.Split(new char[] { '|' })[0] + " Country " + lctxt.Split(new char[] { '|' })[1];
            else
                lpText.Text = lctxt.Split(new char[] { '|' })[0];
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
                                        trailername += " | ";
                                    trailername += String.Format(trailerDefExtra[iCounter], System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(tmp));

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

            if (combDT.Rows.Count > 1)
            {
                //combDT.DefaultView.Sort = "UserTrailerName ASC";
                comboBoxUserTrailerCompanyTrailers.Enabled = true;
            }
            else
            {
                comboBoxUserTrailerCompanyTrailers.Enabled = false;
            }

            comboBoxUserTrailerCompanyTrailers.ValueMember = "UserTrailerkNameless";
            comboBoxUserTrailerCompanyTrailers.DisplayMember = "UserTrailerName";
            comboBoxUserTrailerCompanyTrailers.DataSource = combDT;


            comboBoxUserTrailerCompanyTrailers.SelectedValue = PlayerDataData.UserCompanyAssignedTrailer;
        }

        private void comboBoxCompanyTrailers_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmbbx = sender as ComboBox;

            if (cmbbx.SelectedIndex != -1 && cmbbx.SelectedValue.ToString() != "null")
            {
                UpdateTrailerPanelProgressBars();
                ToggleTrailerPartsCondition(true);

                buttonUserTrailerSelectCurrent.Enabled = true;
                tableLayoutPanelUserTrailerControls.Enabled = true;

                groupBoxUserTrailerTrailerDetails.Enabled = true;
                groupBoxUserTrailerShareTrailerSettings.Enabled = true;

            }
            else
            {
                ToggleTrailerPartsCondition(false);

                if (!comboBoxUserTrailerCompanyTrailers.Enabled)
                    buttonUserTrailerSelectCurrent.Enabled = false;

                tableLayoutPanelUserTrailerControls.Enabled = false;

                groupBoxUserTrailerTrailerDetails.Enabled = false;
                groupBoxUserTrailerShareTrailerSettings.Enabled = false;
            }
        }
        
        private void groupBoxUserTrailerTrailerDetails_EnabledChanged(object sender, EventArgs e)
        {
            ToggleVisualTrailerDetails(groupBoxUserTrailerTrailerDetails.Enabled);
        }

        private void tableLayoutPanelUserTrailerControls_EnabledChanged(object sender, EventArgs e)
        {
            ToggleVisualTrailerControls(tableLayoutPanelUserTrailerControls.Enabled);
        }

        private void ToggleVisualTrailerDetails(bool _state)
        {
            for (int i = 0; i < 4; i++)
            {
                Control[] tmp = tabControlMain.TabPages["tabPageTrailer"].Controls.Find("buttonTrailerElRepair" + i.ToString(), true);
                if (_state)
                    tmp[0].BackgroundImage = RepairImg;
                else
                    tmp[0].BackgroundImage = ConvertBitmapToGrayscale(RepairImg);
            }
        }

        private void ToggleVisualTrailerControls(bool _state)
        {
            Control TMP;

            string[] buttons = { "buttonTrailerRepair", "buttonTrailerInfo" };
            Image[] images = { RepairImg, CustomizeImg };

            for (int i = 0; i < buttons.Count(); i++)
            {
                try
                {
                    TMP = tabControlMain.TabPages["tabPageTrailer"].Controls.Find(buttons[i], true)[0];
                }
                catch
                {
                    break;
                }

                if (_state && TMP.Enabled)
                    TMP.BackgroundImage = images[i];
                else
                    TMP.BackgroundImage = ConvertBitmapToGrayscale(images[i]);
            }
        }

        private void ToggleTrailerPartsCondition(bool _state)
        {
            if (!_state)
            {
                string lblname, pnlname;

                for (int i = 0; i < 4; i++)
                {
                    lblname = "labelTrailerPartDataName" + i.ToString();
                    Label pnLabel = groupBoxUserTrailerTrailerDetails.Controls.Find(lblname, true).FirstOrDefault() as Label;

                    if (pnLabel != null)
                        pnLabel.Text = "";

                    pnlname = "progressbarTrailerPart" + i.ToString();
                    Panel pbPanel = groupBoxUserTrailerTrailerDetails.Controls.Find(pnlname, true).FirstOrDefault() as Panel;

                    if (pbPanel != null)
                        pbPanel.BackgroundImage = null;
                }
                string lblLCname = "labelLicensePlateTrailer";
                Label lblLC = groupBoxUserTrailerTrailerDetails.Controls.Find(lblLCname, true).FirstOrDefault() as Label;

                if (lblLC != null)
                    lblLC.Text = "A 000 AA";
            }
        }
        //Buttons
        private void buttonUserTrailerSelectCurrent_Click(object sender, EventArgs e)
        {
            comboBoxUserTrailerCompanyTrailers.SelectedValue = PlayerDataData.UserCompanyAssignedTrailer;
        }

        private void buttonUserTrailerSwitchCurrent_Click(object sender, EventArgs e)
        {
            PlayerDataData.UserCompanyAssignedTrailer = comboBoxUserTrailerCompanyTrailers.SelectedValue.ToString();
        }

        //end User Trailer tab
    }
}