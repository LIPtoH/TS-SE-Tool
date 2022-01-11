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

using TS_SE_Tool.Global;

namespace TS_SE_Tool
{
    public partial class FormMain
    {
        //User Trucks tab
        private void CreateTruckPanelControls()
        {
            CreateTruckPanelMainButtons();
            CreateTruckPanelPartsControls();
        }

        private void CreateTruckPanelMainButtons()
        {
            int pHeight = RepairImg.Height, pOffset = 5, tOffset = comboBoxUserTruckCompanyTrucks.Location.Y;
            int topbutoffset = comboBoxUserTruckCompanyTrucks.Location.X + comboBoxUserTruckCompanyTrucks.Width + pOffset;

            Button buttonInfo = new Button();
            tableLayoutPanelUserTruckControls.Controls.Add(buttonInfo, 3, 0);
            buttonInfo.FlatStyle = FlatStyle.Flat;
            buttonInfo.Size = new Size(CustomizeImg.Width, CustomizeImg.Height);
            buttonInfo.Name = "buttonTruckInfo";
            buttonInfo.BackgroundImage = CustomizeImg;
            buttonInfo.BackgroundImageLayout = ImageLayout.Zoom;
            buttonInfo.Text = "";
            buttonInfo.FlatAppearance.BorderSize = 0;
            buttonInfo.Enabled = false;
            buttonInfo.Dock = DockStyle.Fill;

            Button buttonR = new Button();
            tableLayoutPanelUserTruckControls.Controls.Add(buttonR, 1, 0);
            buttonR.FlatStyle = FlatStyle.Flat;
            buttonR.Size = new Size(RepairImg.Height, RepairImg.Height);
            buttonR.Name = "buttonTruckRepair";
            buttonR.BackgroundImage = RepairImg;
            buttonR.BackgroundImageLayout = ImageLayout.Zoom;
            buttonR.Text = "";
            buttonR.FlatAppearance.BorderSize = 0;
            buttonR.Click += new EventHandler(buttonTruckRepair_Click);
            buttonR.EnabledChanged += new EventHandler(buttonElRepair_EnabledChanged);
            buttonR.Dock = DockStyle.Fill;

            Button buttonF = new Button();
            tableLayoutPanelUserTruckControls.Controls.Add(buttonF, 2, 0);
            buttonF.FlatStyle = FlatStyle.Flat;
            buttonF.Size = new Size(RepairImg.Height, RepairImg.Height);
            buttonF.Name = "buttonTruckReFuel";
            buttonF.BackgroundImage = RefuelImg;
            buttonF.BackgroundImageLayout = ImageLayout.Zoom;
            buttonF.Text = "";
            buttonF.FlatAppearance.BorderSize = 0;
            buttonF.Click += new EventHandler(buttonTruckReFuel_Click);
            buttonF.EnabledChanged += new EventHandler(buttonRefuel_EnabledChanged);
            buttonF.Dock = DockStyle.Fill;
        }

        private void CreateTruckPanelPartsControls()
        {
            int pSkillsNameHeight = 32, pSkillsNameWidth = 32;

            string[] toolskillimgtooltip = new string[] { "Engine", "Transmission", "Chassis", "Cabin", "Wheels" };
            Label partLabel, partnameLabel;
            Panel pbPanel;

            for (int i = 0; i < 5; i++)
            {
                //Create table layout
                TableLayoutPanel tbllPanel = new TableLayoutPanel();
                tableLayoutPanelTruckDetails.Controls.Add(tbllPanel, 0, i);
                tbllPanel.Dock = DockStyle.Fill;
                tbllPanel.Margin = new Padding(0);
                //
                tbllPanel.Name = "tableLayoutPanelTruckDetails" + toolskillimgtooltip[i];

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

                //Part type
                partLabel = new Label();
                partLabel.Name = "labelTruckPartName" + toolskillimgtooltip[i];
                partLabel.Text = toolskillimgtooltip[i];
                partLabel.AutoSize = true;
                partLabel.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom;
                partLabel.MinimumSize = new Size(36, partLabel.Height);

                flowPanel.Controls.Add(partLabel);

                //Part name
                partnameLabel = new Label();
                partnameLabel.Name = "labelTruckPartDataName" + i;
                partnameLabel.Text = "";
                partnameLabel.AutoSize = true;
                partnameLabel.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom;

                flowPanel.Controls.Add(partnameLabel);

                //Part type image
                Panel imgpanel = new Panel();
                imgpanel.BorderStyle = BorderStyle.None;
                imgpanel.Size = new Size(pSkillsNameWidth, pSkillsNameHeight);
                imgpanel.Margin = new Padding(1);
                imgpanel.Name = "TruckPartImg" + i.ToString();

                Bitmap bgimg = new Bitmap(TruckPartsImg[i], pSkillsNameHeight, pSkillsNameWidth);
                imgpanel.BackgroundImage = bgimg;
                tbllPanel.Controls.Add(imgpanel, 0, 1);

                //Progress bar panel 
                pbPanel = new Panel();
                pbPanel.BorderStyle = BorderStyle.FixedSingle;
                pbPanel.Name = "progressbarTruckPart" + i.ToString();
                pbPanel.Dock = DockStyle.Fill;
                tbllPanel.Controls.Add(pbPanel, 1, 1);

                //Repair button
                Button button = new Button();
                button.FlatStyle = FlatStyle.Flat;
                button.Dock = DockStyle.Fill;
                button.Margin = new Padding(1);

                button.Name = "buttonTruckElRepair" + i.ToString();
                button.BackgroundImage = RepairImg;
                button.BackgroundImageLayout = ImageLayout.Zoom;
                button.Text = "";
                button.FlatAppearance.BorderSize = 0;
                button.Click += new EventHandler(buttonElRepair_Click);
                button.EnabledChanged += new EventHandler(buttonElRepair_EnabledChanged);

                tbllPanel.Controls.Add(button, 2, 1);
            }

            //Fuel panel
            Panel Ppanelf = new Panel();
            Ppanelf.BorderStyle = BorderStyle.FixedSingle;
            Ppanelf.Dock = DockStyle.Fill;
            Ppanelf.Name = "progressbarTruckFuel";

            //label - Fuel
            Label labelF = new Label();
            labelF.Name = "labelTruckDetailsFuel";
            labelF.Text = "Fuel";
            labelF.AutoSize = true;
            labelF.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
            labelF.TextAlign = ContentAlignment.MiddleCenter;

            tableLayoutPanelTruckFuel.Controls.Add(Ppanelf, 0, 1);
            tableLayoutPanelTruckFuel.Controls.Add(labelF, 0, 0);

            //License plate
            Label labelPlate = new Label();
            labelPlate.Name = "labelUserTruckLicensePlate";
            labelPlate.Text = "License plate";
            labelPlate.Margin = new Padding(0);
            labelPlate.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
            labelPlate.TextAlign = ContentAlignment.MiddleCenter;

            Label lcPlate = new Label();
            lcPlate.Name = "labelLicensePlate";
            lcPlate.Text = "A 000 AA";
            lcPlate.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom;
            lcPlate.Dock = DockStyle.Fill;
            lcPlate.TextAlign = ContentAlignment.MiddleLeft;

            tableLayoutPanelTruckLP.Controls.Add(labelPlate, 0, 0);
            tableLayoutPanelTruckLP.Controls.Add(lcPlate, 1, 0);

            //
            Panel LPpanel = new Panel();
            LPpanel.Dock = DockStyle.Fill;
            LPpanel.Margin = new Padding(0);
            LPpanel.Name = "TruckLicensePlateIMG";
            LPpanel.BackgroundImageLayout = ImageLayout.Center;

            tableLayoutPanelTruckLP.Controls.Add(LPpanel, 2, 0);
        }

        private void FillUserCompanyTrucksList()
        {
            if (UserTruckDictionary == null)
                return;

            DataTable combDT = new DataTable();
            DataColumn dc = new DataColumn("UserTruckNameless", typeof(string));
            combDT.Columns.Add(dc);

            dc = new DataColumn("TruckType", typeof(string));
            combDT.Columns.Add(dc);

            dc = new DataColumn("TruckName", typeof(string));
            combDT.Columns.Add(dc);

            dc = new DataColumn("GarageName", typeof(string));
            combDT.Columns.Add(dc);

            dc = new DataColumn("DriverName", typeof(string));
            combDT.Columns.Add(dc);

            DataColumn dcDisplay = new DataColumn("DisplayMember");
            dcDisplay.Expression = string.Format("IIF(UserTruckNameless <> ''," +
                                                        " '[' + {0} +'] ' + IIF(GarageName <> '', {1} +' || ','') + {2} + IIF(DriverName <> 'null', ' || In use - ' + {3},'')," +
                                                        "'-- NONE --')",
                                                "TruckType", "GarageName", "TruckName", "DriverName");
            combDT.Columns.Add(dcDisplay);
            //

            foreach (KeyValuePair<string, UserCompanyTruckData> UserTruck in UserTruckDictionary)
            {
                if (UserTruck.Value == null)
                    continue;

                string truckname = "undetected", truckNameless = "";
                string tmpTruckType = "", tmpTruckName = "", tmpGarageName = "", tmpDriverName = "";

                try
                {
                    if (UserTruck.Value.Parts.Count > 0)
                    {
                        string templine = UserTruck.Value.Parts.Find(x => x.PartType == "truckbrandname").PartData.Find(xline => xline.StartsWith(" data_path:"));
                        truckname = templine.Split(new char[] { '"' })[1].Split(new char[] { '/' })[4];
                    }
                }
                catch
                {
                    continue;
                }
                //

                truckNameless = UserTruck.Key;

                if (UserTruck.Value.Users)
                {
                    tmpTruckType = "U";

                    tmpGarageName = GaragesList.Find(x => x.Vehicles.Contains(truckNameless)).GarageNameTranslated;
                }
                else
                    tmpTruckType = "Q";
                //

                TruckBrandsLngDict.TryGetValue(truckname, out string trucknamevalue);

                if (trucknamevalue != null && trucknamevalue != "")
                {
                    tmpTruckName = trucknamevalue;
                }
                else
                    tmpTruckName = truckname;
                //

                Garages tmpGrg = GaragesList.Where(tX => tX.Vehicles.Contains(truckNameless))?.SingleOrDefault() ?? null;

                if (tmpGrg != null)
                {
                    tmpDriverName = tmpGrg.Drivers[tmpGrg.Vehicles.IndexOf(truckNameless)];
                }
                else
                {
                    tmpDriverName = UserDriverDictionary.Where(tX => tX.Value.AssignedTruck == truckNameless)?.SingleOrDefault().Key ?? "null";
                }

                if (tmpDriverName != null && tmpDriverName != "null")
                    if (Economy.driver_pool[0] == tmpDriverName)
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

                combDT.Rows.Add(truckNameless, tmpTruckType, tmpTruckName, tmpGarageName, tmpDriverName);
            }

            bool noTrucks = false;

            if (combDT.Rows.Count == 0)
            {
                combDT.Rows.Add("null"); // -- NONE --
                noTrucks = true;
            }

            comboBoxUserTruckCompanyTrucks.ValueMember = "UserTruckNameless";
            comboBoxUserTruckCompanyTrucks.DisplayMember = "DisplayMember";
            comboBoxUserTruckCompanyTrucks.DataSource = combDT;

            comboBoxUserTruckCompanyTrucks.Enabled = !noTrucks;

            if (!noTrucks)
                comboBoxUserTruckCompanyTrucks.SelectedValue = Player.assigned_truck;
            else
                comboBoxUserTruckCompanyTrucks.SelectedValue = "null";
        }
        //

        private void UpdateTruckPanelDetails()
        {
            for (byte i = 0; i < 5; i++)
                UpdateTruckPanelProgressBar(i);

            CheckTruckRepair();

            UpdateTruckPanelFuel();
            UpdateTruckPanelLicensePlate();
        }

        private void UpdateTruckPanelProgressBar(byte _number)
        {
            UserTruckDictionary.TryGetValue(comboBoxUserTruckCompanyTrucks.SelectedValue.ToString(), out UserCompanyTruckData SelectedUserCompanyTruck);

            if (SelectedUserCompanyTruck == null)
                return;

            string pnlname = "progressbarTruckPart" + _number.ToString(), labelPartName = "labelTruckPartDataName" + _number.ToString();

            //Progres bar
            Panel pbPanel = groupBoxUserTruckTruckDetails.Controls.Find(pnlname, true).FirstOrDefault() as Panel;
            
            //Part name
            Label pnLabel = groupBoxUserTruckTruckDetails.Controls.Find(labelPartName, true).FirstOrDefault() as Label;
            
            //Repair button
            Button repairButton = groupBoxUserTruckTruckDetails.Controls.Find("buttonTruckElRepair" + _number, true).FirstOrDefault() as Button;

            if (pbPanel != null)
            {
                float _wear = 0;

                List<UserCompanyTruckDataPart> DataPart = null;

                try
                {
                    switch (_number)
                    {
                        case 0:
                            DataPart = SelectedUserCompanyTruck.Parts.FindAll(xp => xp.PartType == "engine");
                            _wear = SelectedUserCompanyTruck.TruckMainData.engine_wear;
                            break;
                        case 1:
                            DataPart = SelectedUserCompanyTruck.Parts.FindAll(xp => xp.PartType == "transmission");
                            _wear = SelectedUserCompanyTruck.TruckMainData.transmission_wear;
                            break;
                        case 2:
                            DataPart = SelectedUserCompanyTruck.Parts.FindAll(xp => xp.PartType == "chassis");
                            _wear = SelectedUserCompanyTruck.TruckMainData.chassis_wear;
                            break;
                        case 3:
                            DataPart = SelectedUserCompanyTruck.Parts.FindAll(xp => xp.PartType == "cabin");
                            _wear = SelectedUserCompanyTruck.TruckMainData.cabin_wear;
                            break;
                        case 4:
                            DataPart = SelectedUserCompanyTruck.Parts.FindAll(xp => xp.PartType == "tire");
                            if (SelectedUserCompanyTruck.TruckMainData.wheels_wear.Count > 0)
                                _wear = SelectedUserCompanyTruck.TruckMainData.wheels_wear.Sum() / SelectedUserCompanyTruck.TruckMainData.wheels_wear.Count;
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
                        //pnLabel.Text = TruckDataPart[0].PartData.Find(xl => xl.StartsWith(" data_path:")).Split(new char[] { '"' })[1].Split(new char[] { '/' }).Last().Split(new char[] { '.' })[0];
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

        private void CheckTruckRepair()
        {
            bool repairEnabled = false;
            Button repairTruck = tableLayoutPanelUserTruckControls.Controls.Find("buttonTruckRepair", true).FirstOrDefault() as Button;

            for (byte i = 0; i < 5; i++)
            {
                try
                {
                    Button tmp = groupBoxUserTruckTruckDetails.Controls.Find("buttonTruckElRepair" + i, true).FirstOrDefault() as Button;
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

            repairTruck.Enabled = repairEnabled;
        }

        private void UpdateTruckPanelFuel()
        {
            UserTruckDictionary.TryGetValue(comboBoxUserTruckCompanyTrucks.SelectedValue.ToString(), out UserCompanyTruckData SelectedUserCompanyTruck);

            string pnlnamefuel = "progressbarTruckFuel";
            Panel pnlfuel = groupBoxUserTruckTruckDetails.Controls.Find(pnlnamefuel, true).FirstOrDefault() as Panel;

            Button refuelTruck = tableLayoutPanelUserTruckControls.Controls.Find("buttonTruckReFuel", true).FirstOrDefault() as Button;

            if (pnlfuel != null)
            {
                float _fuel = SelectedUserCompanyTruck.TruckMainData.fuel_relative;

                if (_fuel == 1)
                    refuelTruck.Enabled = false;
                else
                    refuelTruck.Enabled = true;


                SolidBrush ppen = new SolidBrush(GetProgressbarColor(1 - _fuel));
                int pnlheight = (int)(pnlfuel.Height * (_fuel)), x = 0, y = pnlfuel.Height - pnlheight;

                Bitmap progress = new Bitmap(pnlfuel.Width, pnlfuel.Height);

                Graphics g = Graphics.FromImage(progress);
                g.FillRectangle(ppen, x, y, pnlfuel.Width, pnlheight);

                int fontSize = 10;
                StringFormat sf = new StringFormat();
                sf.LineAlignment = StringAlignment.Center;
                sf.Alignment = StringAlignment.Center;

                GraphicsPath p = new GraphicsPath();
                p.AddString(
                    ((int)(_fuel * 100)).ToString() + " %",             // text to draw
                    FontFamily.GenericSansSerif,                        // or any other font family
                    (int)FontStyle.Regular,                             // font style (bold, italic, etc.)
                    g.DpiY * fontSize / 72,                             // em size
                    new Rectangle(0, 0, pnlfuel.Width, pnlfuel.Height), // location where to draw text
                    sf);                                                // set options here (e.g. center alignment)
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.FillPath(Brushes.Black, p);
                g.DrawPath(Pens.Black, p);

                pnlfuel.BackgroundImage = progress;
            }
        }
        
        private void UpdateTruckPanelLicensePlate()
        {
            UserTruckDictionary.TryGetValue(comboBoxUserTruckCompanyTrucks.SelectedValue.ToString(), out UserCompanyTruckData SelectedUserCompanyTruck);

            string LicensePlate = SelectedUserCompanyTruck.TruckMainData.license_plate;

            SCS.SCSLicensePlate thisLP = new SCS.SCSLicensePlate(LicensePlate, SCS.SCSLicensePlate.LPtype.Truck);

            //Find label control
            Label lpText = groupBoxUserTruckTruckDetails.Controls.Find("labelLicensePlate", true).FirstOrDefault() as Label;
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
            Panel lpPanel = groupBoxUserTruckTruckDetails.Controls.Find("TruckLicensePlateIMG", true).FirstOrDefault() as Panel;
            if (lpPanel != null)
            {
                lpPanel.BackgroundImage = Utilities.TS_Graphics.ResizeImage(thisLP.LicensePlateIMG, LicensePlateWidth[GameType], 32); //ETS - 128x32 or ATS - 128x64 | 64x32
            }
        }

        //Events
        private void comboBoxCompanyTrucks_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmbbx = sender as ComboBox;

            if (cmbbx.SelectedValue != null && cmbbx.SelectedValue.ToString() != "null") //cmbbx.SelectedIndex != -1 && 
            {
                ToggleTruckPartsCondition(true);

                buttonUserTruckSelectCurrent.Enabled = true;
                tableLayoutPanelUserTruckControls.Enabled = true;

                groupBoxUserTruckTruckDetails.Enabled = true;
                groupBoxUserTruckShareTruckSettings.Enabled = true;

                UpdateTruckPanelDetails();
            }
            else
            {
                ToggleTruckPartsCondition(false);

                buttonUserTruckSelectCurrent.Enabled = false;
                tableLayoutPanelUserTruckControls.Enabled = false;

                groupBoxUserTruckTruckDetails.Enabled = false;
                groupBoxUserTruckShareTruckSettings.Enabled = false;
            }
        }

        private void groupBoxUserTruckTruckDetails_EnabledChanged(object sender, EventArgs e)
        {
            ToggleVisualTruckDetails(groupBoxUserTruckTruckDetails.Enabled);
        }

        private void tableLayoutPanelUserTruckControls_EnabledChanged(object sender, EventArgs e)
        {
            ToggleVisualTruckControls(tableLayoutPanelUserTruckControls.Enabled);
        }

        private void ToggleVisualTruckDetails(bool _state)
        {
            for (int i = 0; i < 5; i++)
            {
                Control tmpButtonRepair = tabControlMain.TabPages["tabPageTruck"].Controls.Find("buttonTruckElRepair" + i.ToString(), true).FirstOrDefault();

                if (tmpButtonRepair == null)
                    continue;

                tmpButtonRepair.Enabled = _state;
                
                if (_state)
                    tmpButtonRepair.BackgroundImage = RepairImg;
                else
                    tmpButtonRepair.BackgroundImage = ConvertBitmapToGrayscale(RepairImg);                
            }
        }

        private void ToggleVisualTruckControls(bool _state)
        {
            Control TMP;

            string[] buttons = { "buttonTruckReFuel", "buttonTruckRepair", "buttonTruckInfo" };
            Image[] images = { RefuelImg, RepairImg, CustomizeImg };

            for (int i = 0; i < buttons.Count(); i++)
            {
                try
                {
                    TMP = tabControlMain.TabPages["tabPageTruck"].Controls.Find(buttons[i], true)[0];
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

        private void ToggleTruckPartsCondition(bool _state)
        {
            if (!_state)
            {
                string lblname, pnlname;

                for (int i = 0; i < 5; i++)
                {
                    lblname = "labelTruckPartDataName" + i.ToString();
                    Label pnLabel = groupBoxUserTruckTruckDetails.Controls.Find(lblname, true).FirstOrDefault() as Label;

                    if (pnLabel != null)
                        pnLabel.Text = "";

                    pnlname = "progressbarTruckPart" + i.ToString();
                    Panel pbPanel = groupBoxUserTruckTruckDetails.Controls.Find(pnlname, true).FirstOrDefault() as Panel;

                    if (pbPanel != null)                    
                        pbPanel.BackgroundImage = null;                    
                }

                string pnlFname =  "progressbarTruckFuel";
                Panel pnlF = groupBoxUserTruckTruckDetails.Controls.Find(pnlFname, true).FirstOrDefault() as Panel;

                if (pnlF != null)                
                    pnlF.BackgroundImage = null;

                string lblLCname = "labelLicensePlate";
                Label lblLC = groupBoxUserTruckTruckDetails.Controls.Find(lblLCname, true).FirstOrDefault() as Label;

                if (lblLC != null)
                    lblLC.Text = "A 000 AA";

                pnlname = "TruckLicensePlateIMG";
                Panel LPPanel = groupBoxUserTruckTruckDetails.Controls.Find(pnlname, true).FirstOrDefault() as Panel;

                if (LPPanel != null)
                    LPPanel.BackgroundImage = null;
            }
        }
        //Buttons
        public void buttonTruckReFuel_Click(object sender, EventArgs e)
        {
            UserTruckDictionary.TryGetValue(comboBoxUserTruckCompanyTrucks.SelectedValue.ToString(), out UserCompanyTruckData SelectedUserCompanyTruck);

            if (SelectedUserCompanyTruck == null)
                return;

            SelectedUserCompanyTruck.TruckMainData.fuel_relative = 1;

            UpdateTruckPanelFuel();
        }

        public void buttonTruckRepair_Click(object sender, EventArgs e)
        {
            UserTruckDictionary.TryGetValue(comboBoxUserTruckCompanyTrucks.SelectedValue.ToString(), out UserCompanyTruckData SelectedUserCompanyTruck);

            if (SelectedUserCompanyTruck == null)
                return;

            SelectedUserCompanyTruck.TruckMainData.engine_wear = 0;
            SelectedUserCompanyTruck.TruckMainData.transmission_wear = 0;
            SelectedUserCompanyTruck.TruckMainData.chassis_wear = 0;
            SelectedUserCompanyTruck.TruckMainData.cabin_wear = 0;
            SelectedUserCompanyTruck.TruckMainData.wheels_wear = new List<Save.DataFormat.SCS_Float>();

            for (byte i = 0; i < 5; i++)
                UpdateTruckPanelProgressBar(i);

            CheckTruckRepair();
        }
        //
        public void buttonElRepair_Click(object sender, EventArgs e)
        {
            Button curbtn = sender as Button;
            byte bi = Convert.ToByte(curbtn.Name.Substring(19));

            UserTruckDictionary.TryGetValue(comboBoxUserTruckCompanyTrucks.SelectedValue.ToString(), out UserCompanyTruckData SelectedUserCompanyTruck);

            if (SelectedUserCompanyTruck == null)
                return;

            switch (bi)
            {
                case 0:
                    SelectedUserCompanyTruck.TruckMainData.engine_wear = 0;
                    break;
                case 1:
                    SelectedUserCompanyTruck.TruckMainData.transmission_wear = 0;
                    break;
                case 2:
                    SelectedUserCompanyTruck.TruckMainData.chassis_wear = 0;
                    break;
                case 3:
                    SelectedUserCompanyTruck.TruckMainData.cabin_wear = 0;
                    break;
                case 4:
                    SelectedUserCompanyTruck.TruckMainData.wheels_wear = new List<Save.DataFormat.SCS_Float>();
                    break;
            }

            UpdateTruckPanelProgressBar(bi);
            CheckTruckRepair();
        }
        //
        public void buttonElRepair_EnabledChanged(object sender, EventArgs e)
        {
            Button tmp = sender as Button;

            if (tmp.Enabled)
                tmp.BackgroundImage = RepairImg;
            else
                tmp.BackgroundImage = ConvertBitmapToGrayscale(RepairImg);
        }

        public void buttonRefuel_EnabledChanged(object sender, EventArgs e)
        {
            Button tmp = sender as Button;

            if (tmp.Enabled)
                tmp.BackgroundImage = RefuelImg;
            else
                tmp.BackgroundImage = ConvertBitmapToGrayscale(RefuelImg);
        }
        //
        private void buttonUserTruckSelectCurrent_Click(object sender, EventArgs e)
        {
            comboBoxUserTruckCompanyTrucks.SelectedValue = Player.assigned_truck;
        }

        private void buttonUserTruckSwitchCurrent_Click(object sender, EventArgs e)
        {
            Player.assigned_truck = comboBoxUserTruckCompanyTrucks.SelectedValue.ToString();
        }
        //
        //Share buttons
        private void buttonTruckPaintCopy_Click(object sender, EventArgs e)
        {
            string tempPaint = "TruckPaint\r\n";

            List<string> paintstr = UserTruckDictionary[comboBoxUserTruckCompanyTrucks.SelectedValue.ToString()].Parts.Find(xp => xp.PartType == "paintjob").PartData;

            foreach (string temp in paintstr)
            {
                tempPaint += temp + "\r\n";
            }

            string tmpString = BitConverter.ToString(Utilities.ZipDataUtilities.zipText(tempPaint)).Replace("-", "");
            Clipboard.SetText(tmpString);
            MessageBox.Show("Paint data has been copied.");
        }

        private void buttonTruckPaintPaste_Click(object sender, EventArgs e)
        {
            try
            {
                string inputData = Utilities.ZipDataUtilities.unzipText(Clipboard.GetText());
                string[] Lines = inputData.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                if (Lines[0] == "TruckPaint")
                {
                    List<string> paintstr = new List<string>();
                    for (int i = 1; i < Lines.Length; i++)
                    {
                        paintstr.Add(Lines[i]);
                    }

                    UserTruckDictionary[comboBoxUserTruckCompanyTrucks.SelectedValue.ToString()].Parts.Find(xp => xp.PartType == "paintjob").PartData = paintstr;

                    MessageBox.Show("Paint data  has been inserted.");
                }
                else
                    MessageBox.Show("Wrong data. Expected Paint data but\r\n" + Lines[0] + "\r\nwas found.");
            }
            catch
            {
                MessageBox.Show("Something gone wrong with decoding.");
            }
        }
        //end Share buttons
        //end User Trucks tab
    }
}