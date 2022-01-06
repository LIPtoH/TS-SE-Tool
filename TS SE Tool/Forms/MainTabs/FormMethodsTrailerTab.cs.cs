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
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using System.Globalization;
using System.Text.RegularExpressions;

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
            buttonR.EnabledChanged += new EventHandler(buttonTrailerElRepair_EnabledChanged);

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
                button.EnabledChanged += new EventHandler(buttonTrailerElRepair_EnabledChanged);

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

            //
            Panel LPpanel = new Panel();
            LPpanel.Dock = DockStyle.Fill;
            LPpanel.Margin = new Padding(0);
            LPpanel.Name = "TrailerLicensePlateIMG";
            LPpanel.BackgroundImageLayout = ImageLayout.Center;

            tableLayoutPanelTrailerLP.Controls.Add(LPpanel, 2, 0);
        }

        private void FillUserCompanyTrailerList()
        {
            if (UserTrailerDictionary == null)
                return;

            DataTable combDT = new DataTable();
            DataColumn dc = new DataColumn("UserTrailerNameless", typeof(string));
            combDT.Columns.Add(dc);

            dc = new DataColumn("TrailerType", typeof(string));
            combDT.Columns.Add(dc);

            dc = new DataColumn("TrailerName", typeof(string));
            combDT.Columns.Add(dc);

            dc = new DataColumn("GarageName", typeof(string));
            combDT.Columns.Add(dc);

            dc = new DataColumn("DriverName", typeof(string));
            combDT.Columns.Add(dc);

            DataColumn dcDisplay = new DataColumn("DisplayMember");
            dcDisplay.Expression = string.Format("IIF(UserTrailerNameless <> 'null'," +
                                                        " '[' + {0} +'] ' + IIF(GarageName <> '', {1} +' || ','') + {2} + IIF(DriverName <> 'null', ' || In use - ' + {3},'')," +
                                                        "'-- NONE --')",
                                                "TrailerType", "GarageName", "TrailerName", "DriverName");
            combDT.Columns.Add(dcDisplay);
            //

            combDT.Rows.Add("null"); // -- NONE --
            //

            foreach (KeyValuePair<string, UserCompanyTrailerData> UserTrailer in UserTrailerDictionary)
            {
                if (UserTrailer.Value == null)
                    continue;

                if (UserTrailer.Value.Main)
                {
                    string trailername = "", trailerNameless = "";
                    string tmpTrailerType = "", tmpTrailerkName = "", tmpGarageName = "", tmpDriverName = "";

                    trailerNameless = UserTrailer.Key;
                    //

                    if (UserTrailerDictionary[trailerNameless].Users)
                    {
                        tmpTrailerType = "U";

                        tmpGarageName = GaragesList.Find(x => x.Trailers.Contains(trailerNameless)).GarageNameTranslated;
                    }
                    else
                        tmpTrailerType = "Q";
                    //

                    string trailerdef = UserTrailerDictionary[trailerNameless].TrailerMainData.trailer_definition;

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
                                    string tmp = CurTrailerDef.Find(x => x.StartsWith(" " + Property + ":")).Split(':')[1].Trim(new char[] { ' ' }).Replace('_', ' ');

                                    if (wasfound)
                                        trailername += " | ";
                                    trailername += String.Format(trailerDefExtra[iCounter], CultureInfo.CurrentCulture.TextInfo.ToTitleCase(tmp));

                                    wasfound = true;
                                }
                                catch { wasfound = false; }

                                iCounter++;
                            }

                            tmpTrailerkName = trailername;
                        }
                        else
                        {
                            tmpTrailerkName = trailerdef;
                        }
                    }
                    else
                    {
                        tmpTrailerkName = trailerdef;
                    }
                    //

                    tmpDriverName = UserDriverDictionary.Where(tX => tX.Value.AssignedTrailer == trailerNameless)?.SingleOrDefault().Key ?? "null";
                    
                    if (tmpDriverName != "null")
                        if (EconomyPlayerData.UserDriver == tmpDriverName)
                        {
                            tmpDriverName = "> " + Utilities.TextUtilities.FromHexToString(Globals.SelectedProfile);
                        }
                        else
                        {
                            DriverNames.TryGetValue(tmpDriverName, out string _resultvalue);

                            if (_resultvalue != null && _resultvalue != "")
                            {
                                tmpDriverName = _resultvalue.TrimStart(new char[] { '+' });
                            }
                        }
                    //

                    combDT.Rows.Add(trailerNameless, tmpTrailerType, tmpTrailerkName, tmpGarageName, tmpDriverName);
                }
            }

            if (combDT.Rows.Count > 1)
            {
                comboBoxUserTrailerCompanyTrailers.Enabled = true;
            }
            else
            {
                comboBoxUserTrailerCompanyTrailers.Enabled = false;
            }

            comboBoxUserTrailerCompanyTrailers.ValueMember = "UserTrailerNameless";
            comboBoxUserTrailerCompanyTrailers.DisplayMember = "DisplayMember";
            comboBoxUserTrailerCompanyTrailers.DataSource = combDT;

            comboBoxUserTrailerCompanyTrailers.SelectedValue = Player.assigned_trailer;
        }
        //
        private void UpdateTrailerPanelDetails()
        {
            for (byte i = 0; i < 4; i++)
                UpdateTrailerPanelProgressBar(i);

            CheckTrailerRepair();

            //UpdateTrailerPanelCargo();
            UpdateTrailerPanelLicensePlate();
        }

        private void UpdateTrailerPanelProgressBar(byte _number)
        {
            UserTrailerDictionary.TryGetValue(comboBoxUserTrailerCompanyTrailers.SelectedValue.ToString(), out UserCompanyTrailerData SelectedUserCompanyTrailer);

            if (SelectedUserCompanyTrailer == null)
                return;

            string pnlname = "progressbarTrailerPart" + _number.ToString(), labelPartName = "labelTrailerPartDataName" + _number.ToString();

            //Progres bar
            Panel pbPanel = groupBoxUserTrailerTrailerDetails.Controls.Find(pnlname, true).FirstOrDefault() as Panel;

            //Part name
            Label pnLabel = groupBoxUserTrailerTrailerDetails.Controls.Find(labelPartName, true).FirstOrDefault() as Label;

            //Repair button
            Button repairButton = groupBoxUserTrailerTrailerDetails.Controls.Find("buttonTrailerElRepair" + _number, true).FirstOrDefault() as Button;

            if (pbPanel != null)
            {
                List<UserCompanyTruckDataPart> DataPart = null;
                float _wear = 0;

                try
                {
                    switch (_number)
                    {
                        case 0:                            
                            DataPart = SelectedUserCompanyTrailer.Parts.FindAll(xp => xp.PartType == "trailerdata");
                            _wear = SelectedUserCompanyTrailer.TrailerMainData.cargo_damage;
                            break;                            
                        case 1:
                            DataPart = SelectedUserCompanyTrailer.Parts.FindAll(xp => xp.PartType == "body");
                            _wear = SelectedUserCompanyTrailer.TrailerMainData.trailer_body_wear;
                            break;
                        case 2:
                            DataPart = SelectedUserCompanyTrailer.Parts.FindAll(xp => xp.PartType == "chassis");
                            _wear = SelectedUserCompanyTrailer.TrailerMainData.chassis_wear;
                            break;
                        case 3:
                            DataPart = SelectedUserCompanyTrailer.Parts.FindAll(xp => xp.PartType == "tire");
                            if (SelectedUserCompanyTrailer.TrailerMainData.wheels_wear.Count > 0)
                                _wear = SelectedUserCompanyTrailer.TrailerMainData.wheels_wear.Sum() / SelectedUserCompanyTrailer.TrailerMainData.wheels_wear.Count;
                            break;
                    }
                }
                catch
                {
                    repairButton.Enabled = false;
                    return;
                }


                if (pnLabel != null)
                    if (DataPart != null && DataPart.Count > 0)
                    {
                        /*
                        if (_number != 0)
                        {
                            pnLabel.Text = DataPart[0].PartData.Find(xl => xl.StartsWith(" data_path:")).Split(new char[] { '"' })[1].Split(new char[] { '/' }).Last().Split(new char[] { '.' })[0];
                        }
                        else
                        {
                            var tmp = UserDriverDictionary.Select(tx => tx.Value)
                                    .Where(tX => tX.AssignedTrailer == comboBoxUserTrailerCompanyTrailers.SelectedValue.ToString()).ToList();

                            if (tmp != null && tmp.Count > 0)
                            {
                                string tmpCargo = tmp[0].DriverJob.Cargo;

                                if (CargoLngDict.TryGetValue(tmpCargo, out string value))
                                {
                                    if (value != null && value != "")
                                    {
                                        pnLabel.Text = value;
                                    }
                                    else
                                    {
                                        string CapName = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(value);

                                        pnLabel.Text = CapName;
                                    }
                                }
                            }
                            else
                            {
                                pbPanel.BackgroundImage = null;
                                pnLabel.Text = "";
                                return;
                            }
                        }
                        */
                    }
                    else
                    {
                        pnLabel.Text = "none";
                    }

                if (_wear == 0)
                    repairButton.Enabled = false;
                else
                    repairButton.Enabled = true;

                //
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

        private void CheckTrailerRepair()
        {
            bool repairEnabled = false;
            Button repairTrailer = tableLayoutPanelUserTrailerControls.Controls.Find("buttonTrailerRepair", true).FirstOrDefault() as Button;

            for (byte i = 0; i < 4; i++)
            {
                try
                {
                    Button tmp = groupBoxUserTrailerTrailerDetails.Controls.Find("buttonTrailerElRepair" + i, true).FirstOrDefault() as Button;
                    if (tmp.Enabled)
                    {
                        repairEnabled = true;
                        break;
                    }
                }
                catch
                {
                    continue;
                }
            }

            repairTrailer.Enabled = repairEnabled;
        }

        private void UpdateTrailerPanelCargo()
        {

        }

        private void UpdateTrailerPanelLicensePlate()
        {
            UserTrailerDictionary.TryGetValue(comboBoxUserTrailerCompanyTrailers.SelectedValue.ToString(), out UserCompanyTrailerData SelectedUserCompanyTrailer);

            string LicensePlate = SelectedUserCompanyTrailer.TrailerMainData.license_plate;

            SCS.SCSLicensePlate thisLP = new SCS.SCSLicensePlate(LicensePlate, SCS.SCSLicensePlate.LPtype.Truck);

            //Find label control
            Label lpText = groupBoxUserTrailerTrailerDetails.Controls.Find("labelLicensePlateTrailer", true).FirstOrDefault() as Label;
            if (lpText != null)
            {
                lpText.Text = thisLP.LicensePlateTXT + " | ";

                string value = null;
                CountriesLngDict.TryGetValue(thisLP.SourceLPCountry, out value);

                if (value != null && value != "")
                {
                    lpText.Text += value;
                }
                else
                {
                    string CapName = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(thisLP.SourceLPCountry);
                    lpText.Text += CapName;
                }
            }

            //
            Panel lpPanel = groupBoxUserTrailerTrailerDetails.Controls.Find("TrailerLicensePlateIMG", true).FirstOrDefault() as Panel;
            if (lpPanel != null)
            {
                lpPanel.BackgroundImage = Utilities.TS_Graphics.ResizeImage(thisLP.LicensePlateIMG, LicensePlateWidth[GameType], 32); //ETS - 128x32 or ATS - 128x64 | 64x32
            }
        }

        //Events
        private void comboBoxCompanyTrailers_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmbbx = sender as ComboBox;

            if (cmbbx.SelectedIndex != -1 && cmbbx.SelectedValue.ToString() != "null")
            {
                ToggleTrailerPartsCondition(true);

                buttonUserTrailerSelectCurrent.Enabled = true;
                tableLayoutPanelUserTrailerControls.Enabled = true;

                groupBoxUserTrailerTrailerDetails.Enabled = true;
                groupBoxUserTrailerShareTrailerSettings.Enabled = true;

                UpdateTrailerPanelDetails();
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
                Control tmpButtonRepair = tabControlMain.TabPages["tabPageTrailer"].Controls.Find("buttonTrailerElRepair" + i.ToString(), true).FirstOrDefault();

                if (tmpButtonRepair == null)
                    continue;

                tmpButtonRepair.Enabled = _state;

                if (_state)
                    tmpButtonRepair.BackgroundImage = RepairImg;
                else
                    tmpButtonRepair.BackgroundImage = ConvertBitmapToGrayscale(RepairImg);
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

                pnlname = "TrailerLicensePlateIMG";
                Panel LPPanel = groupBoxUserTrailerTrailerDetails.Controls.Find(pnlname, true).FirstOrDefault() as Panel;

                if (LPPanel != null)
                    LPPanel.BackgroundImage = null;
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    Button repairButton = groupBoxUserTrailerTrailerDetails.Controls.Find("buttonTrailerElRepair" + i, true).FirstOrDefault() as Button;

                    if (repairButton != null && repairButton.Enabled)
                        repairButton.BackgroundImage = RepairImg;
                }
            }
        }
        //Buttons
        //Main
        public void buttonTrailerRepair_Click(object sender, EventArgs e)
        {
            string trailerNameless = "", slaveTrailerNameless = "";

            trailerNameless = comboBoxUserTrailerCompanyTrailers.SelectedValue.ToString();

            StartTrailerParts:

            Save.Items.Trailer selectedTrailerData = UserTrailerDictionary[trailerNameless].TrailerMainData;

            selectedTrailerData.cargo_damage = 0;
            selectedTrailerData.trailer_body_wear = 0;
            selectedTrailerData.chassis_wear = 0;
            selectedTrailerData.wheels_wear = new CustomClasses.Global.FloatList();

            slaveTrailerNameless = UserTrailerDictionary[trailerNameless].TrailerMainData.slave_trailer;

            if (slaveTrailerNameless != "null")
            {
                trailerNameless = slaveTrailerNameless;
                goto StartTrailerParts;
            }

            for (byte i = 0; i < 5; i++)
                UpdateTrailerPanelProgressBar(i);

            CheckTrailerRepair();
        }

        //
        private void buttonUserTrailerSelectCurrent_Click(object sender, EventArgs e)
        {
            comboBoxUserTrailerCompanyTrailers.SelectedValue = Player.assigned_trailer;
        }

        private void buttonUserTrailerSwitchCurrent_Click(object sender, EventArgs e)
        {
            Player.assigned_trailer = comboBoxUserTrailerCompanyTrailers.SelectedValue.ToString();
        }
        //Details
        public void buttonTrailerElRepair_Click(object sender, EventArgs e)
        {
            Button curbtn = sender as Button;

            //Get button index
            Regex reDigits = new Regex(@"\d+");
            Match reMatch = reDigits.Match(curbtn.Name);

            byte buttonIndex = byte.Parse(reMatch.Value);

            //
            string trailerNameless = "", slaveTrailerNameless = "";

            trailerNameless = comboBoxUserTrailerCompanyTrailers.SelectedValue.ToString();

            Save.Items.Trailer selectedTrailerData = UserTrailerDictionary[trailerNameless].TrailerMainData;

            StartTrailerParts:

            switch (buttonIndex)
            {
                case 0:
                    selectedTrailerData.cargo_damage = 0;
                    break;

                case 1:
                    selectedTrailerData.trailer_body_wear = 0;
                    break;

                case 2:
                    selectedTrailerData.chassis_wear = 0;
                    break;

                case 3:
                    selectedTrailerData.wheels_wear = new CustomClasses.Global.FloatList();
                    break;
            }

            slaveTrailerNameless = UserTrailerDictionary[trailerNameless].TrailerMainData.slave_trailer;

            if (slaveTrailerNameless != "null")
            {
                trailerNameless = slaveTrailerNameless;
                goto StartTrailerParts;
            }

            UpdateTrailerPanelProgressBar(buttonIndex);

            CheckTrailerRepair();
        }

        public void buttonTrailerElRepair_EnabledChanged(object sender, EventArgs e)
        {
            Button tmp = sender as Button;

            if (tmp.Enabled)
                tmp.BackgroundImage = RepairImg;
            else
                tmp.BackgroundImage = ConvertBitmapToGrayscale(RepairImg);
        }

        //end User Trailer tab
    }
}