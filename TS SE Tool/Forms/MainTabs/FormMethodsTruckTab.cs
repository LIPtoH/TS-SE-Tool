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
            CreateTruckPanelButtons();
            CreateTruckPanelProgressBars();
        }

        private void CreateTruckPanelProgressBars()
        {
            int pHeight = RepairImg.Height, pOffset = 5, lOffset = 60, pSizeW = 300;
            int pSkillsNameHeight = 32, pSkillsNameWidth = 32, pSkillsNameOffset = 5, pSkillsNamelOffset = 12;

            string[] toolskillimgtooltip = new string[] { "Engine", "Transmission", "Chassis", "Cabin", "Wheels" };
            Label slabel, labelpartName;
            Panel Ppanel;

            for (int i = 0; i < 5; i++)
            {
                slabel = new Label();
                groupBoxUserTruckTruckDetails.Controls.Add(slabel);
                slabel.Name = "labelTruckPartName" + toolskillimgtooltip[i];
                slabel.Location = new Point(pSkillsNamelOffset, 23 + (pSkillsNameHeight + pSkillsNameOffset * 3) * i);
                slabel.Text = toolskillimgtooltip[i];
                slabel.AutoSize = true;

                labelpartName = new Label();
                groupBoxUserTruckTruckDetails.Controls.Add(labelpartName);
                labelpartName.Name = "labelTruckPartDataName" + i;
                labelpartName.Location = new Point(lOffset + pSizeW / 2, 23 + (pSkillsNameHeight + pSkillsNameOffset * 3) * i);
                labelpartName.Text = "";
                labelpartName.AutoSize = true;

                Panel imgpanel = new Panel();
                groupBoxUserTruckTruckDetails.Controls.Add(imgpanel);

                imgpanel.Parent = groupBoxUserTruckTruckDetails;
                imgpanel.Location = new Point(pSkillsNamelOffset, 23 + 14 + (pSkillsNameHeight + pSkillsNameOffset * 3) * i);
                imgpanel.BorderStyle = BorderStyle.None;
                imgpanel.Size = new Size(pSkillsNameWidth, pSkillsNameHeight);
                imgpanel.Name = "TruckPartImg" + i.ToString();

                Bitmap bgimg = new Bitmap(TruckPartsImg[i], pSkillsNameHeight, pSkillsNameWidth);
                imgpanel.BackgroundImage = bgimg;

                //Panel 
                Ppanel = new Panel();
                groupBoxUserTruckTruckDetails.Controls.Add(Ppanel);

                Ppanel.Parent = groupBoxUserTruckTruckDetails;
                Ppanel.Location = new Point(lOffset, imgpanel.Location.Y + pOffset);
                Ppanel.BorderStyle = BorderStyle.FixedSingle;
                Ppanel.Size = new Size(pSizeW, 24);
                Ppanel.Name = "progressbarTruckPart" + i.ToString();

                Button button = new Button();
                groupBoxUserTruckTruckDetails.Controls.Add(button);

                button.Parent = groupBoxUserTruckTruckDetails;
                button.Location = new Point(Ppanel.Location.X + Ppanel.Width + pOffset, imgpanel.Location.Y);
                button.FlatStyle = FlatStyle.Flat;
                button.Size = new Size(RepairImg.Height, RepairImg.Height);
                button.Name = "buttonTruckElRepair" + i.ToString();
                button.BackgroundImage = RepairImg;
                button.BackgroundImageLayout = ImageLayout.Zoom;
                button.Text = "";
                button.FlatAppearance.BorderSize = 0;
                button.Click += new EventHandler(buttonElRepair_Click);
            }

            Panel Ppanelf = new Panel();
            groupBoxUserTruckTruckDetails.Controls.Add(Ppanelf);
            Ppanelf.Parent = groupBoxUserTruckTruckDetails;
            Ppanelf.Location = new Point(lOffset + pSizeW + pOffset * 2 + RepairImg.Width, 23 + 14);
            Ppanelf.BorderStyle = BorderStyle.FixedSingle;
            Ppanelf.Size = new Size(50, 220);
            Ppanelf.Name = "progressbarTruckFuel";

            slabel = new Label();
            groupBoxUserTruckTruckDetails.Controls.Add(slabel);
            slabel.Name = "labelTruckDetailsFuel";
            slabel.Text = "Fuel";
            slabel.AutoSize = true;
            slabel.Location = new Point(Ppanelf.Location.X + (Ppanelf.Width - slabel.Width) / 2, Ppanelf.Location.Y + Ppanelf.Height + 10);
        }

        private void CreateTruckPanelButtons()
        {
            int pHeight = RepairImg.Height, pOffset = 5, tOffset = comboBoxUserTruckCompanyTrucks.Location.Y;
            int topbutoffset = comboBoxUserTruckCompanyTrucks.Location.X + comboBoxUserTruckCompanyTrucks.Width + pOffset;

            Button buttonInfo = new Button();
            tableLayoutPanel8.Controls.Add(buttonInfo, 0, 1);
            buttonInfo.FlatStyle = FlatStyle.Flat;
            buttonInfo.Size = new Size(CutomizeImg.Width, CutomizeImg.Height);
            buttonInfo.Name = "buttonTruckInfo";
            buttonInfo.BackgroundImage = ConvertBitmapToGrayscale(CutomizeImg);
            buttonInfo.BackgroundImageLayout = ImageLayout.Zoom;
            buttonInfo.Text = "";
            buttonInfo.FlatAppearance.BorderSize = 0;
            buttonInfo.Enabled = false;
            buttonInfo.Dock = DockStyle.Fill;

            Button buttonR = new Button();
            tableLayoutPanel8.Controls.Add(buttonR, 3, 1);
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
            tableLayoutPanel8.Controls.Add(buttonF, 4, 1);
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
            //combDT.DefaultView.Sort = "UserTruckName ASC";
            comboBoxUserTruckCompanyTrucks.ValueMember = "UserTruckNameless";
            comboBoxUserTruckCompanyTrucks.DisplayMember = "UserTruckName";
            comboBoxUserTruckCompanyTrucks.DataSource = combDT;
            comboBoxUserTruckCompanyTrucks.SelectedValue = PlayerDataV.UserCompanyAssignedTruck;
        }

        private void UpdateTruckPanelProgressBars()
        {
            UserTruckDictionary.TryGetValue(comboBoxUserTruckCompanyTrucks.SelectedValue.ToString(), out UserCompanyTruckData SelectedUserCompanyTruck);

            for (int i = 0; i < 5; i++)
            {
                Panel pnl = null;
                Label labelPart = null;

                string pnlname = "progressbarTruckPart" + i.ToString(), labelPartName = "labelTruckPartDataName" + i.ToString();

                if (groupBoxUserTruckTruckDetails.Controls.ContainsKey(pnlname))
                {
                    pnl = groupBoxUserTruckTruckDetails.Controls[pnlname] as Panel;
                }

                if (groupBoxUserTruckTruckDetails.Controls.ContainsKey(labelPartName))
                {
                    labelPart = groupBoxUserTruckTruckDetails.Controls[labelPartName] as Label;
                }

                if (pnl != null)
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
                        if (labelPart != null)
                        {
                            labelPart.Text = TruckDataPart.Find(xl => xl.StartsWith(" data_path:")).Split(new char[] { '"' })[1].Split(new char[] { '/' }).Last().Split(new char[] { '.' })[0];
                        }

                        wear = TruckDataPart.Find(xl => xl.StartsWith(" wear:")).Split(new char[] { ' ' })[2];
                    }
                    else
                    {
                        labelPart.Text = "!! Part not found !!";
                    }

                    labelPart.Location = new Point(pnl.Location.X + (pnl.Width - labelPart.Width) / 2, labelPart.Location.Y);

                    decimal _wear = 0;

                    if (wear != "0" && wear != "1")
                        _wear = Utilities.NumericUtilities.HexFloatToDecimalFloat(wear);
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

            Panel pnlfuel = null;
            string pnlnamefuel = "progressbarTruckFuel";
            if (groupBoxUserTruckTruckDetails.Controls.ContainsKey(pnlnamefuel))
            {
                pnlfuel = groupBoxUserTruckTruckDetails.Controls[pnlnamefuel] as Panel;
            }

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

            string lctxt = "";
            labelLicensePlate.Text = "";
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
            if (lctxt.Split(new char[] { '|' }).Length > 1)
                labelLicensePlate.Text = lctxt.Split(new char[] { '|' })[0] + " Country " + lctxt.Split(new char[] { '|' })[1];
            else
                labelLicensePlate.Text = lctxt.Split(new char[] { '|' })[0];
        }

        //Events
        private void comboBoxCompanyTrucks_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmbbx = sender as ComboBox;

            if (cmbbx.SelectedIndex != -1)
            {
                UpdateTruckPanelProgressBars();
            }
        }

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
            comboBoxUserTruckCompanyTrucks.SelectedValue = PlayerDataV.UserCompanyAssignedTruck;
        }

        private void buttonUserTruckSwitchCurrent_Click(object sender, EventArgs e)
        {
            PlayerDataV.UserCompanyAssignedTruck = comboBoxUserTruckCompanyTrucks.SelectedValue.ToString();
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