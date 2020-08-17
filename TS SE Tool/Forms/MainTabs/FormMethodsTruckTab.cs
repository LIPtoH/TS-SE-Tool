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
            LPpanel.BackgroundImageLayout = ImageLayout.None;

            tableLayoutPanelTruckLP.Controls.Add(LPpanel, 2, 0);
        }

        private void FillUserCompanyTrucksList()
        {
            DataTable combDT = new DataTable();
            DataColumn dc = new DataColumn("UserTruckNameless", typeof(string));
            combDT.Columns.Add(dc);

            dc = new DataColumn("UserTruckName", typeof(string));
            combDT.Columns.Add(dc);

            foreach (KeyValuePair<string, UserCompanyTruckData> UserTruck in UserTruckDictionary)
            {
                string truckname = "";
                try
                {
                    string templine = UserTruck.Value.Parts.Find(x => x.PartType == "truckbrandname").PartData.Find(xline => xline.StartsWith(" data_path:"));
                    truckname = templine.Split(new char[] { '"' })[1].Split(new char[] { '/' })[4];
                }
                catch { }
                TruckBrandsLngDict.TryGetValue(truckname, out string trucknamevalue);

                string TruckName = "";

                if (UserTruckDictionary[UserTruck.Key].Users)
                    TruckName = "[U] ";
                else
                    TruckName = "[Q] ";

                if (trucknamevalue != null && trucknamevalue != "")
                {
                    TruckName += trucknamevalue;
                    combDT.Rows.Add(UserTruck.Key, TruckName);
                }
                else
                {
                    TruckName += truckname;
                    combDT.Rows.Add(UserTruck.Key, TruckName);
                }
            }

            bool noTrucks = false;

            if (combDT.Rows.Count == 0)
            {
                combDT.Rows.Add("null", "-- NONE --"); //none
                noTrucks = true;
            }

            comboBoxUserTruckCompanyTrucks.ValueMember = "UserTruckNameless";
            comboBoxUserTruckCompanyTrucks.DisplayMember = "UserTruckName";
            comboBoxUserTruckCompanyTrucks.DataSource = combDT;

            if (!noTrucks)
            {
                //combDT.DefaultView.Sort = "UserTruckName ASC";
                comboBoxUserTruckCompanyTrucks.Enabled = true;
                comboBoxUserTruckCompanyTrucks.SelectedValue = PlayerDataData.UserCompanyAssignedTruck;
            }
            else
            {
                comboBoxUserTruckCompanyTrucks.Enabled = false;
                comboBoxUserTruckCompanyTrucks.SelectedValue = "null";
            }
        }

        private void UpdateTruckPanelProgressBars()
        {
            UserTruckDictionary.TryGetValue(comboBoxUserTruckCompanyTrucks.SelectedValue.ToString(), out UserCompanyTruckData SelectedUserCompanyTruck);

            for (int i = 0; i < 5; i++)
            {
                string pnlname = "progressbarTruckPart" + i.ToString(), labelPartName = "labelTruckPartDataName" + i.ToString();

                Panel pbPanel = groupBoxUserTruckTruckDetails.Controls.Find(pnlname, true).FirstOrDefault() as Panel;

                Label pnLabel = groupBoxUserTruckTruckDetails.Controls.Find(labelPartName, true).FirstOrDefault() as Label;

                if (pbPanel != null)
                {
                    List<string> TruckDataPart = null;

                    switch (i)
                    {
                        case 0:
                            TruckDataPart = SelectedUserCompanyTruck.Parts.Find(xp => xp.PartType == "engine").PartData;
                            break;
                        case 1:
                            TruckDataPart = SelectedUserCompanyTruck.Parts.Find(xp => xp.PartType == "transmission").PartData;
                            break;
                        case 2:
                            TruckDataPart = SelectedUserCompanyTruck.Parts.Find(xp => xp.PartType == "chassis").PartData;
                            break;
                        case 3:
                            TruckDataPart = SelectedUserCompanyTruck.Parts.Find(xp => xp.PartType == "cabin").PartData;
                            break;
                        case 4:
                            TruckDataPart = SelectedUserCompanyTruck.Parts.Find(xp => xp.PartType == "tire").PartData;
                            break;
                    }
                    string wear = "0";
                    if (TruckDataPart != null)
                    {
                        if (pnLabel != null)
                        {
                            pnLabel.Text = TruckDataPart.Find(xl => xl.StartsWith(" data_path:")).Split(new char[] { '"' })[1].Split(new char[] { '/' }).Last().Split(new char[] { '.' })[0];
                        }

                        wear = TruckDataPart.Find(xl => xl.StartsWith(" wear:")).Split(new char[] { ' ' })[2];
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

            string pnlnamefuel = "progressbarTruckFuel";
            Panel pnlfuel = groupBoxUserTruckTruckDetails.Controls.Find(pnlnamefuel, true).FirstOrDefault() as Panel;

            if (pnlfuel != null)
            {
                string fuel = SelectedUserCompanyTruck.Parts.Find(xp => xp.PartType == "truckdata").PartData.Find(xl => xl.StartsWith(" fuel_relative:")).Split(new char[] { ' ' })[2];//SelectedUserCompanyTruck.Fuel;
                decimal _fuel = 0;

                if (fuel != "0" && fuel != "1")
                    _fuel = Utilities.NumericUtilities.HexFloatToDecimalFloat(fuel);
                else
                if (fuel == "1")
                    _fuel = 1;

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

            //License plate
            string lctxt = "";            
            int stindex = 0, endindex = 0;

            string LicensePlate = SelectedUserCompanyTruck.Parts.Find(xp => xp.PartType == "truckdata").PartData.Find(xl => xl.StartsWith(" license_plate:")).Split(new char[] { '"' })[1];
            
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

            //Find label control
            Label lpText = groupBoxUserTruckTruckDetails.Controls.Find("labelLicensePlate", true).FirstOrDefault() as Label;
            if(lpText != null)
            {
                if (lctxt.Split(new char[] { '|' }).Length > 1)
                    lpText.Text = lctxt.Split(new char[] { '|' })[0] + " Country " + lctxt.Split(new char[] { '|' })[1];
                else
                    lpText.Text = lctxt.Split(new char[] { '|' })[0];
            }            
            
            //
            Panel lpPanel = groupBoxUserTruckTruckDetails.Controls.Find("TruckLicensePlateIMG", true).FirstOrDefault() as Panel;

            if (lpPanel != null)
            {
                Image tmp = Utilities.TS_Graphics.LicensePlateRender(LicensePlate.Split(new char[] { '|' })[0], LicensePlate.Split(new char[] { '|' })[1].Trim(new char[] { '_', ' ' }));

                Bitmap bgimg = new Bitmap(tmp, 128, 32);
                lpPanel.BackgroundImage = bgimg;
            }
        }

        //Events
        private void comboBoxCompanyTrucks_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmbbx = sender as ComboBox;

            if (cmbbx.SelectedValue != null && cmbbx.SelectedValue.ToString() != "null") //cmbbx.SelectedIndex != -1 && 
            {
                UpdateTruckPanelProgressBars();
                ToggleTruckPartsCondition(true);

                buttonUserTruckSelectCurrent.Enabled = true;
                tableLayoutPanelUserTruckControls.Enabled = true;

                groupBoxUserTruckTruckDetails.Enabled = true;
                groupBoxUserTruckShareTruckSettings.Enabled = true;
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
                Control[] tmp = tabControlMain.TabPages["tabPageTruck"].Controls.Find("buttonTruckElRepair" + i.ToString(), true);
                if (_state)
                    tmp[0].BackgroundImage = RepairImg;
                else
                    tmp[0].BackgroundImage = ConvertBitmapToGrayscale(RepairImg);
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
            }
        }
        //Buttons
        public void buttonTruckReFuel_Click(object sender, EventArgs e)
        {
            int i = 0;
            foreach (string temp in UserTruckDictionary[comboBoxUserTruckCompanyTrucks.SelectedValue.ToString()].Parts.Find(x => x.PartType == "truckdata").PartData)
            {
                if (temp.StartsWith(" fuel_relative:"))
                {
                    UserTruckDictionary[comboBoxUserTruckCompanyTrucks.SelectedValue.ToString()].Parts.Find(x => x.PartType == "truckdata").PartData[i] = " fuel_relative: 1";
                    break;
                }
                i++;
            }
            UpdateTruckPanelProgressBars();
        }

        public void buttonTruckRepair_Click(object sender, EventArgs e)
        {
            string[] PartList = { "engine", "transmission", "chassis", "cabin", "tire" };

            foreach (string tempPart in PartList)
            {
                foreach (UserCompanyTruckDataPart temp in UserTruckDictionary[comboBoxUserTruckCompanyTrucks.SelectedValue.ToString()].Parts.FindAll(x => x.PartType == tempPart))
                {
                    string partNameless = temp.PartNameless;

                    int i = 0;

                    foreach (string temp2 in temp.PartData)
                    {
                        if (temp2.StartsWith(" wear:"))
                        {
                            UserTruckDictionary[comboBoxUserTruckCompanyTrucks.SelectedValue.ToString()].Parts.Find(x => x.PartNameless == partNameless).PartData[i] = " wear: 0";
                            break;
                        }
                        i++;
                    }
                }
            }

            UpdateTruckPanelProgressBars();
        }

        public void buttonElRepair_Click(object sender, EventArgs e)
        {
            Button curbtn = sender as Button;
            int bi = Convert.ToByte(curbtn.Name.Substring(19));

            string[] PartList = { "engine", "transmission", "chassis", "cabin", "tire" };

            foreach (UserCompanyTruckDataPart temp in UserTruckDictionary[comboBoxUserTruckCompanyTrucks.SelectedValue.ToString()].Parts.FindAll(x => x.PartType == PartList[bi]))
            {
                string partNameless = temp.PartNameless;

                int i = 0;

                foreach (string temp2 in temp.PartData)
                {
                    if (temp2.StartsWith(" wear:"))
                    {
                        UserTruckDictionary[comboBoxUserTruckCompanyTrucks.SelectedValue.ToString()].Parts.Find(x => x.PartNameless == partNameless).PartData[i] = " wear: 0";
                        break;
                    }
                    i++;
                }
            }

            UpdateTruckPanelProgressBars();
        }

        private void buttonUserTruckSelectCurrent_Click(object sender, EventArgs e)
        {
            comboBoxUserTruckCompanyTrucks.SelectedValue = PlayerDataData.UserCompanyAssignedTruck;
        }

        private void buttonUserTruckSwitchCurrent_Click(object sender, EventArgs e)
        {
            PlayerDataData.UserCompanyAssignedTruck = comboBoxUserTruckCompanyTrucks.SelectedValue.ToString();
        }

        //Share buttons
        private void buttonTruckPaintCopy_Click(object sender, EventArgs e)
        {
            string tempPaint = "TruckPaint\r\n";

            List<string> paintstr = UserTruckDictionary[comboBoxUserTruckCompanyTrucks.SelectedValue.ToString()].Parts.Find(xp => xp.PartType == "paintjob").PartData;

            foreach (string temp in paintstr)
            {
                tempPaint += temp + "\r\n";
            }

            string asd = BitConverter.ToString(Utilities.ZipDataUtilitiescs.zipText(tempPaint)).Replace("-", "");
            Clipboard.SetText(asd);
            MessageBox.Show("Paint data has been copied.");
        }

        private void buttonTruckPaintPaste_Click(object sender, EventArgs e)
        {
            try
            {
                string inputData = Utilities.ZipDataUtilitiescs.unzipText(Clipboard.GetText());
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