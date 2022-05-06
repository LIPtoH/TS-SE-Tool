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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace TS_SE_Tool
{
    public partial class FormMain
    {
        //User Company tab

        private void CreateCompanyPanelControls()
        {
            buttonUserCompanyGaragesManage.Text = "";
            buttonUserCompanyGaragesManage.BackgroundImage = CustomizeImg;
            buttonUserCompanyGaragesManage.BackgroundImageLayout = ImageLayout.Center;

            listBoxVisitedCities.DrawMode = DrawMode.OwnerDrawVariable;
            listBoxGarages.DrawMode = DrawMode.OwnerDrawVariable;
        }

        private void tableLayoutPanel2_EnabledChanged(object sender, EventArgs e)
        {
            ToggleVisualUserCompanyControls(tableLayoutPanel2.Enabled);
        }

        private void ToggleVisualUserCompanyControls(bool _state)
        {
            Control tmpControl;

            string[] buttons = { "buttonUserCompanyGaragesManage" };
            Image[] images = { CustomizeImg };

            for (int i = 0; i < buttons.Count(); i++)
            {
                try
                {
                    tmpControl = tabControlMain.TabPages["tabPageCompany"].Controls.Find(buttons[i], true)[0];
                }
                catch
                {
                    break;
                }

                if (_state && tmpControl.Enabled)
                    tmpControl.BackgroundImage = images[i];
                else
                    tmpControl.BackgroundImage = Utilities.Graphics_TSSET.ConvertBitmapToGrayscale(images[i]);
            }
        }

        private void FillFormCompanyControls()
        {
            pictureBoxCompanyLogo.Image = Utilities.Graphics_TSSET.ddsImgLoader(new string[] { @"img\" + GameType + @"\player_logo\" + MainSaveFileProfileData.Logo + ".dds" }, 94, 94, 0, 0)[0];

            textBoxUserCompanyCompanyName.Text = MainSaveFileProfileData.CompanyName.Value;

            FillAccountMoneyTB();

            FillHQcities();

            FillVisitedCities(0);
            FillGaragesList(0);
        }

        public void FillAccountMoneyTB()
        {
            //
            Int64 valueBefore = (long)Math.Floor(SiiNunitData.Bank.money_account * CurrencyDictConversion[Globals.CurrencyName]);

            textBoxUserCompanyMoneyAccount.Text = String.Format(CultureInfo.CurrentCulture, "{0:N0}", valueBefore);

            //
            this.ActiveControl = textBoxUserCompanyMoneyAccount;
            this.ActiveControl = null;
        }

        private void FillHQcities()
        {
            DataTable combDT = new DataTable();
            DataColumn dc = new DataColumn("City", typeof(string));
            combDT.Columns.Add(dc);

            dc = new DataColumn("CityName", typeof(string));
            combDT.Columns.Add(dc);

            //start filling

            //fill source and destination cities
            foreach (Garages garage in from x in GaragesList where x.GarageStatus != 0 && !x.IgnoreStatus select x)
            {
                combDT.Rows.Add(garage.GarageName, garage.GarageNameTranslated);
            }

            combDT.DefaultView.Sort = "CityName ASC";
            comboBoxUserCompanyHQcity.SelectedIndexChanged -= comboBoxUserCompanyHQcity_SelectedIndexChanged;
            comboBoxUserCompanyHQcity.ValueMember = "City";
            comboBoxUserCompanyHQcity.DisplayMember = "CityName";
            comboBoxUserCompanyHQcity.BeginUpdate();
            comboBoxUserCompanyHQcity.DataSource = combDT;
            comboBoxUserCompanyHQcity.SelectedValue = SiiNunitData.Player.hq_city;
            comboBoxUserCompanyHQcity.EndUpdate();
            comboBoxUserCompanyHQcity.SelectedIndexChanged += comboBoxUserCompanyHQcity_SelectedIndexChanged;
        }

        private void comboBoxUserCompanyHQcity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxUserCompanyHQcity.SelectedValue != null)
            {
                string prevHQ = SiiNunitData.Player.hq_city, newHQ = comboBoxUserCompanyHQcity.SelectedValue.ToString();

                Garages prevGarage = GaragesList.Where(x => x.GarageName == prevHQ).First(),
                        newGarage = GaragesList.Where(x => x.GarageName == newHQ).First();

                string playerDriver = SiiNunitData.Player.drivers[0];
                int prevSlotIdx = prevGarage.Drivers.IndexOf(playerDriver);

                string tmpDrvr = newGarage.Drivers[0],
                       tmpVhcl = newGarage.Vehicles[0];

                newGarage.Drivers[0] = prevGarage.Drivers[prevSlotIdx];
                newGarage.Vehicles[0] = prevGarage.Vehicles[prevSlotIdx];

                //Check for spare slots
                int spareSlots = -1;

                for (int i = 0; i < newGarage.Drivers.Count; i++)
                {
                    if (newGarage.Drivers[i] == newGarage.Vehicles[i])
                    {
                        spareSlots = i;
                        break;
                    }
                }

                if (spareSlots > -1)
                {
                    //Move
                    newGarage.Drivers[spareSlots] = tmpDrvr;
                    newGarage.Vehicles[spareSlots] = tmpVhcl;

                    prevGarage.Drivers[prevSlotIdx] = null;
                    prevGarage.Vehicles[prevSlotIdx] = null;
                }
                else
                {
                    //Swap
                    prevGarage.Drivers[prevSlotIdx] = tmpDrvr;
                    prevGarage.Vehicles[prevSlotIdx] = tmpVhcl;
                }

                //Set HQ
                SiiNunitData.Player.hq_city = newHQ;

                FillGaragesList(listBoxGarages.TopIndex);
            }
        }

        private void textBoxUserCompanyCompanyName_TextChanged(object sender, EventArgs e)
        {
            if (textBoxUserCompanyCompanyName.Text.Length > 20)
            {
                textBoxUserCompanyCompanyName.Text = textBoxUserCompanyCompanyName.Text.Remove(20);
                textBoxUserCompanyCompanyName.Select(20, 0);
            }

            int txtLength = textBoxUserCompanyCompanyName.Text.Length;

            switch (txtLength)
            {
                case 0:
                    {
                        labelUserCompanyCompanyName.ForeColor = Color.Red;

                        labelCompanyNameSize.ForeColor = Color.Red;
                        labelCompanyNameSize.Font = new Font(labelCompanyNameSize.Font, FontStyle.Bold);

                        break;
                    }

                case 20:
                    {
                        labelUserCompanyCompanyName.ForeColor = Color.FromKnownColor(KnownColor.ControlText);

                        labelCompanyNameSize.ForeColor = Color.DarkGreen;
                        labelCompanyNameSize.Font = new Font(labelCompanyNameSize.Font, FontStyle.Bold);

                        break;
                    }

                default:
                    {
                        labelUserCompanyCompanyName.ForeColor = Color.FromKnownColor(KnownColor.ControlText);

                        labelCompanyNameSize.ForeColor = Color.FromKnownColor(KnownColor.ControlText);
                        labelCompanyNameSize.Font = new Font(labelCompanyNameSize.Font, FontStyle.Regular);

                        break;
                    }
            }

            labelCompanyNameSize.Text = textBoxUserCompanyCompanyName.Text.Length.ToString() + " / 20";

            MainSaveFileProfileData.CompanyName = new Save.DataFormat.SCS_String(textBoxUserCompanyCompanyName.Text);
        }

        private void textBoxUserCompanyCompanyName_Validating(object sender, CancelEventArgs e)
        {
            TextBox txtbx = sender as TextBox;

            if (txtbx.ReadOnly == false)
                if (txtbx.TextLength == 0)
                {
                    // Cancel the event and select the text to be corrected by the user.
                    MessageBox.Show("Company name is empty." + Environment.NewLine + "It must contain at least 1 letter. ", "Company name",MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true;
                }
        }
        
        private void textBoxUserCompanyMoneyAccount_Enter(object sender, EventArgs e)
        {
            Int64 valueBefore = (long)Math.Floor(SiiNunitData.Bank.money_account * CurrencyDictConversion[Globals.CurrencyName]);

            textBoxUserCompanyMoneyAccount.Text = String.Format(CultureInfo.CurrentCulture, "{0:N0}", valueBefore);

            //
            textBoxUserCompanyMoneyAccount.KeyPress += textBoxMoneyAccount_KeyPress;
            textBoxUserCompanyMoneyAccount.TextChanged += textBoxMoneyAccount_TextChanged;

            MoneyAccountEntered = true;
        }

        bool MoneyAccountEntered = false;

        private void textBoxUserCompanyMoneyAccount_MouseClick(object sender, MouseEventArgs e)
        {
            if (MoneyAccountEntered)
            {
                int selbefore = textBoxUserCompanyMoneyAccount.SelectionStart;

                var charIndex = textBoxUserCompanyMoneyAccount.GetCharIndexFromPosition(e.Location);
                var charPosition = textBoxUserCompanyMoneyAccount.GetPositionFromCharIndex(charIndex);

                string charChar = textBoxUserCompanyMoneyAccount.Text[charIndex].ToString();

                Graphics g = Graphics.FromImage(new Bitmap(1, 1));
                var charSize = g.MeasureString(charChar, textBoxUserCompanyMoneyAccount.Font);

                if (e.Location.X > charPosition.X + charSize.Width) selbefore++;

                string newtext = "";

                if (CurrencyDictFormat[Globals.CurrencyName][0] != "")
                    newtext += CurrencyDictFormat[Globals.CurrencyName][0] + "-";

                newtext += CurrencyDictFormat[Globals.CurrencyName][1];

                textBoxUserCompanyMoneyAccount.SelectionStart = selbefore - newtext.Length;

                MoneyAccountEntered = false;
            }
        }

        private void textBoxUserCompanyMoneyAccount_Validating(object sender, CancelEventArgs e)
        {

        }

        private void textBoxUserCompanyMoneyAccount_Leave(object sender, EventArgs e)
        {
            textBoxUserCompanyMoneyAccount.KeyPress -= textBoxMoneyAccount_KeyPress;
            textBoxUserCompanyMoneyAccount.TextChanged -= textBoxMoneyAccount_TextChanged;
            
            //
            if (!Int64.TryParse(textBoxUserCompanyMoneyAccount.Text, NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign, CultureInfo.CurrentCulture, out long newValue))
                return;

            SiiNunitData.Bank.money_account = (long)Math.Round(newValue / CurrencyDictConversion[Globals.CurrencyName]);

            //[sign1] - [sign2] 1.234,- [sign3]
            string newtext = "";
            if (CurrencyDictFormat[Globals.CurrencyName][0] != "")
                newtext += CurrencyDictFormat[Globals.CurrencyName][0] + "-";

            newtext += CurrencyDictFormat[Globals.CurrencyName][1] + String.Format(CultureInfo.CurrentCulture, "{0:N0}", newValue) + ",-" + CurrencyDictFormat[Globals.CurrencyName][2];
            //

            textBoxUserCompanyMoneyAccount.Text = newtext;
        }

        private void textBoxMoneyAccount_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox textBoxAccountMoney = sender as TextBox;

            if (!string.IsNullOrEmpty(textBoxAccountMoney.Text))
            {
                if (!Char.IsDigit(e.KeyChar))
                {
                    if (e.KeyChar == (char)Keys.Enter)
                    {
                        this.ActiveControl = null;
                        return;
                    }

                    if (e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Delete || e.KeyChar == (char)'-')
                    {
                        return;
                    }

                    e.Handled = true;
                }
            }
        }

        private void textBoxMoneyAccount_TextChanged(object sender, EventArgs e)
        {
            TextBox textBoxAccountMoney = sender as TextBox;

            string newtext = "";

            if (!string.IsNullOrEmpty(textBoxAccountMoney.Text))
            {
                int selectionStart = textBoxAccountMoney.SelectionStart;

                string onlyDigits = textBoxAccountMoney.Text;

                if (!Int64.TryParse(onlyDigits, NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign, CultureInfo.CurrentCulture, out long valueBefore))
                {
                    valueBefore = 0;
                }

                newtext = String.Format(CultureInfo.CurrentCulture, "{0:N0}", valueBefore);

                int cSpace1 = textBoxAccountMoney.Text.Substring(0, selectionStart).Count(Char.IsWhiteSpace);
                string txtBefore = textBoxAccountMoney.Text;

                //
                textBoxUserCompanyMoneyAccount.TextChanged -= textBoxMoneyAccount_TextChanged;
                textBoxAccountMoney.Text = newtext;
                textBoxUserCompanyMoneyAccount.TextChanged += textBoxMoneyAccount_TextChanged;

                //
                if (selectionStart > textBoxAccountMoney.Text.Length)
                    selectionStart = textBoxAccountMoney.Text.Length;

                int cSpace2 = textBoxAccountMoney.Text.Substring(0, selectionStart).Count(Char.IsWhiteSpace);
                string txtAfter = textBoxAccountMoney.Text;

                int cSpaceDiff = 0, txtDiff = txtBefore.Length - txtAfter.Length;

                if (txtDiff <= 0)
                {
                    if (cSpace1 >= cSpace2)
                        cSpaceDiff = cSpace1 - cSpace2;
                    else
                        cSpaceDiff = cSpace2 - cSpace1;
                }
                else
                {
                    if (cSpace1 <= cSpace2)
                        cSpaceDiff = cSpace1 - cSpace2;
                    else
                        cSpaceDiff = cSpace2 - cSpace1;
                }

                textBoxAccountMoney.SelectionStart = selectionStart + cSpaceDiff;
            }
            else
            {
                textBoxAccountMoney.Text = "0";
            }
        }

        //Visited cities
        //Fill
        public void FillVisitedCities(int _vindex)
        {
            listBoxVisitedCities.BeginUpdate();
            listBoxVisitedCities.Items.Clear();

            if (CitiesList.Count <= 0)
                return;

            int vicited = 0;
            foreach (City vc in from x in CitiesList where !x.Disabled select x)
            {
                if (vc.Visited)
                    vicited++;

                listBoxVisitedCities.Items.Add(vc);
            }

            listBoxVisitedCities.TopIndex = _vindex;
            listBoxVisitedCities.EndUpdate();

            labelUserCompanyVisitedCitiesCurrent.Text = vicited.ToString();
            labelUserCompanyVisitedCitiesTotal.Text = listBoxVisitedCities.Items.Count.ToString();
        }

        //Draw
        private int VisitedCitiesItemMargin = 3;
        private const float VisitedCitiesPictureHeight = 32;

        private void listBoxVisitedCities_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            // Get the ListBox and the item.
            e.ItemHeight = (int)(VisitedCitiesPictureHeight + 2 * VisitedCitiesItemMargin);
        }

        private void listBoxVisitedCities_DrawItem(object sender, DrawItemEventArgs e)
        {
            // Get the ListBox and the item.
            ListBox lst = sender as ListBox;

            City vc = (City)lst.Items[e.Index];

            string txt = "",
                   countryName = CitiesList.Find(xc => xc.CityName == vc.CityName).Country;

            StringFormat format = new StringFormat();

            Brush br;
            Font RegularFont = new Font(this.Font.FontFamily, 9f), 
                 BoldFont = new Font(this.Font, FontStyle.Bold);

            Image cityicon;
            float scale, picture_width;

            float x, y, width, height;
            RectangleF layout_rect, source_rect, dest_rect;

            SizeF itemSize;

            // Draw the background.
            e.DrawBackground();

            // See if the item is selected.
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                br = SystemBrushes.HighlightText;
            else
                br = new SolidBrush(e.ForeColor);

            // Icon

            if (vc.Visited) 
                cityicon = CitiesImg[1];
            else
                cityicon = CitiesImg[0];

            source_rect = new RectangleF(0, 0, cityicon.Width, cityicon.Height);

            scale = VisitedCitiesPictureHeight / cityicon.Height;
            picture_width = scale * cityicon.Width;

            dest_rect = new RectangleF(e.Bounds.Left + VisitedCitiesItemMargin, e.Bounds.Top + VisitedCitiesItemMargin, picture_width, VisitedCitiesPictureHeight);

            // Draw
            e.Graphics.DrawImage(cityicon, dest_rect, source_rect, GraphicsUnit.Pixel);

            //===

            // City
            txt = vc.CityNameTranslated;

            itemSize = e.Graphics.MeasureString(txt, RegularFont);

            x = e.Bounds.Left + picture_width + 3 * VisitedCitiesItemMargin;
            y = e.Bounds.Top + (e.Bounds.Bottom - e.Bounds.Top - itemSize.Height) / 2;

            layout_rect = new RectangleF(x, y, itemSize.Width, itemSize.Height);

            // Draw the text
            e.Graphics.DrawString(txt, RegularFont, br, layout_rect);

            //=== Country

            if (!string.IsNullOrEmpty(countryName))
            {
                txt = "[ ";

                if (CountriesDataList.ContainsKey(countryName))
                    txt += CountriesDataList[countryName].ShortName;
                else
                    txt += countryName.First();

                txt += " ]";
            }
            else
            {
                txt = "[ - - ]";
            }

            itemSize = e.Graphics.MeasureString(txt, BoldFont);

            x = e.Bounds.Right - itemSize.Width - GarageItemMargin;
            y = e.Bounds.Top + (e.Bounds.Bottom - e.Bounds.Top - itemSize.Height) / 2 + 1; 

            layout_rect = new RectangleF(x, y, itemSize.Width, itemSize.Height);

            format.Alignment = StringAlignment.Far;

            // Draw
            e.Graphics.DrawString(txt, BoldFont, br, layout_rect, format);
            

            // Draw the focus rectangle if appropriate.
            e.DrawFocusRectangle();
        }
        //Buttons
        private void buttonCitiesVisit_Click(object sender, EventArgs e)
        {
            if (listBoxVisitedCities.SelectedItems.Count == 0)
            {
                foreach (City city in listBoxVisitedCities.Items)
                {
                    if (!city.Visited)
                        city.Visited = true;
                }
            }
            else
                foreach (City city in listBoxVisitedCities.SelectedItems)
                {
                    if (!city.Visited)
                        city.Visited = true;
                }


            FillVisitedCities(listBoxVisitedCities.TopIndex);
        }

        private void buttonCitiesUnVisit_Click(object sender, EventArgs e)
        {
            if (listBoxVisitedCities.SelectedItems.Count == 0)
            {
                foreach (City city in listBoxVisitedCities.Items)
                {
                    if (city.Visited)
                        city.Visited = false;
                }
            }
            else
                foreach (City city in listBoxVisitedCities.SelectedItems)
                {
                    if (city.Visited)
                        city.Visited = false;
                }

            FillVisitedCities(listBoxVisitedCities.TopIndex);
        }
        
        //Garages
        //Fill
        public void FillGaragesList(int _vindex)
        {
            listBoxGarages.BeginUpdate();
            listBoxGarages.Items.Clear();

            if (GaragesList.Count <= 0)
                return;

            int grgs = 0;
            foreach (Garages garage in from x in GaragesList where !x.IgnoreStatus select x)
            {
                listBoxGarages.Items.Add(garage);

                if (garage.GarageStatus != 0)
                    grgs++;
            }

            listBoxGarages.TopIndex = _vindex;
            listBoxGarages.EndUpdate();

            labelUserCompanyGaragesCurrent.Text = grgs.ToString();
            labelUserCompanyGaragesTotal.Text = listBoxGarages.Items.Count.ToString();
        }

        //Draw
        private int GarageItemMargin = 3;
        private const float GaragePictureHeight = 32;

        private void listBoxGarages_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            // Get the ListBox and the item.
            e.ItemHeight = (int)(GaragePictureHeight + 2 * GarageItemMargin);
        }

        private void listBoxGarages_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index == -1)
                return;

            // Get the ListBox and the item.
            ListBox lst = sender as ListBox;

            Garages grg = (Garages)lst.Items[e.Index];

            string txt = "",
                   countryName = CitiesList.Find(xc => xc.CityName == grg.GarageName).Country;

            CultureInfo ci = Thread.CurrentThread.CurrentUICulture;

            StringFormat format = new StringFormat();

            Brush brush;
            Font RegularFontSized = new Font(this.Font.FontFamily, 10f), 
                 BoldFont = new Font(this.Font, FontStyle.Bold);

            Image grgicon;
            float scale, picture_width;

            SizeF itemSize;

            float x, y, width, height;
            RectangleF layout_rect, source_rect, dest_rect;

            // Brush if the item is selected

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                brush = SystemBrushes.HighlightText;
            else
                brush = new SolidBrush(e.ForeColor);

            // Draw the background
            e.DrawBackground();

            //=== Icon status

            if (grg.GarageName != SiiNunitData.Player.hq_city)
                grgicon = GaragesImg[grg.GarageStatus];
            else
                grgicon = GaragesHQImg[grg.GarageStatus];

            source_rect = new RectangleF(0, 0, grgicon.Width, grgicon.Height);

            scale = GaragePictureHeight / grgicon.Height;
            picture_width = scale * grgicon.Width;

            dest_rect = new RectangleF(e.Bounds.Left + GarageItemMargin, e.Bounds.Top + GarageItemMargin, picture_width, GaragePictureHeight);

            // Draw
            e.Graphics.DrawImage(grgicon, dest_rect, source_rect, GraphicsUnit.Pixel);

            //===

            //=== City

            txt = grg.GarageNameTranslated;

            itemSize = e.Graphics.MeasureString(txt, BoldFont);

            x = e.Bounds.Left + picture_width + GarageItemMargin * 3;
            y = e.Bounds.Top + GarageItemMargin * 2;

            layout_rect = new RectangleF(x, y, itemSize.Width, itemSize.Height);

            // Draw the text.
            e.Graphics.DrawString(txt, BoldFont, brush, layout_rect);

            //===

            //=== Country

            if (!string.IsNullOrEmpty(countryName))
            {
                txt = "[ ";

                if (CountriesDataList.ContainsKey(countryName))
                    txt += CountriesDataList[countryName].ShortName;
                else
                    txt += countryName.First();

                txt += " ]";

            }
            else
            {
                txt = "[ - - ]";
            }    

            itemSize = e.Graphics.MeasureString(txt, BoldFont);

            x = e.Bounds.Right - itemSize.Width - GarageItemMargin;

            layout_rect = new RectangleF(x, y, itemSize.Width, itemSize.Height);

            format.Alignment = StringAlignment.Far;

            // Draw
            e.Graphics.DrawString(txt, BoldFont, brush, layout_rect, format);

            //===

            // Garage status

            txt = grg.GetStatusString();

            itemSize = e.Graphics.MeasureString(txt, this.Font);

            x = e.Bounds.Left + picture_width + GarageItemMargin * 3;
            y = e.Bounds.Bottom - itemSize.Height - GarageItemMargin * 1;

            layout_rect = new RectangleF(x, y, itemSize.Width, itemSize.Height);

            // Draw the text.
            e.Graphics.DrawString(txt, this.Font, brush, layout_rect);

            //===

            //=== Vehicles & Drivers

            if (grg.GarageStatus != 0)
            {
                int curVeh = 0, curDr = 0;

                foreach (string temp in grg.Vehicles)
                    if (temp != null)
                        curVeh++;

                foreach (string temp in grg.Drivers)
                    if (temp != null)
                        curDr++;

                string stringV = "", stringD = "", stringT = "";

                stringV = ResourceManagerMain.GetPlainString("VehicleShort", ci);
                stringD = ResourceManagerMain.GetPlainString("DriverShort", ci);
                stringT = ResourceManagerMain.GetPlainString("TrailerShort", ci);

                txt = String.Format("{0}: {1} / {2} {3}: {4} / {5} {6}: {7}", stringV, curVeh, grg.Vehicles.Count, stringD, curDr, grg.Drivers.Count, stringT, grg.Trailers.Count);

                itemSize = e.Graphics.MeasureString(txt, this.Font);

                x = e.Bounds.Right - itemSize.Width - GarageItemMargin * 1;

                layout_rect = new RectangleF(x, y, itemSize.Width, itemSize.Height);

                format.Alignment = StringAlignment.Far;

                // Draw
                e.Graphics.DrawString(txt, this.Font, brush, layout_rect, format);
            }

            //===

            // Draw the focus rectangle if appropriate.
            e.DrawFocusRectangle();
        }
        
        //Buttons
        private void buttonUserCompanyGaragesManage_Click(object sender, EventArgs e)
        {
            PrepareGarages();

            FormGaragesSoldContent testDialog = new FormGaragesSoldContent();
            testDialog.ShowDialog(this);

            FillGaragesList(listBoxGarages.TopIndex);
        }

        private void buttonGaragesBuy_Click(object sender, EventArgs e)
        {
            List<Garages> tmp;

            if (listBoxGarages.SelectedItems.Count == 0)
                tmp = listBoxGarages.Items.Cast<Garages>().ToList();
            else
                tmp = listBoxGarages.SelectedItems.Cast<Garages>().ToList();

            foreach (Garages garage in tmp)
            {
                if (garage.GarageStatus == 0)
                    garage.GarageStatus = 2;
            }

            PrepareGarages();

            FillGaragesList(listBoxGarages.TopIndex);
            FillHQcities();
        }

        private void buttonGaragesUpgrade_Click(object sender, EventArgs e)
        {
            List<Garages> tmp;

            if (listBoxGarages.SelectedItems.Count == 0)
                tmp = listBoxGarages.Items.Cast<Garages>().ToList();
            else
                tmp = listBoxGarages.SelectedItems.Cast<Garages>().ToList();

            foreach (Garages garage in tmp)
            {
                if (garage.GarageStatus == 2)
                    garage.GarageStatus = 3;
                else if (garage.GarageStatus == 6)
                    garage.GarageStatus = 2;
            }

            PrepareGarages();

            FillGaragesList(listBoxGarages.TopIndex);
        }

        private void buttonGaragesDowngrade_Click(object sender, EventArgs e)
        {
            List<Garages> tmp;

            if (listBoxGarages.SelectedItems.Count == 0)
                tmp = listBoxGarages.Items.Cast<Garages>().ToList();
            else
                tmp = listBoxGarages.SelectedItems.Cast<Garages>().ToList();

            foreach (Garages garage in tmp)
            {
                if (garage.GarageStatus == 3)
                    garage.GarageStatus = 2;
                else if (garage.GarageName == comboBoxUserCompanyHQcity.SelectedValue.ToString())
                    garage.GarageStatus = 6;
            }

            PrepareGarages();

            FillGaragesList(listBoxGarages.TopIndex);
        }

        private void buttonGaragesSell_Click(object sender, EventArgs e)
        {
            List<Garages> tmp;

            if (listBoxGarages.SelectedItems.Count == 0)
                tmp = listBoxGarages.Items.Cast<Garages>().ToList();
            else
                tmp = listBoxGarages.SelectedItems.Cast<Garages>().ToList();

            foreach (Garages garage in tmp)
            {
                if (garage.GarageName == SiiNunitData.Player.hq_city)
                    garage.GarageStatus = 6;
                else
                    garage.GarageStatus = 0;
            }

            PrepareGarages();

            FillGaragesList(listBoxGarages.TopIndex);
            FillHQcities();
        }
        //end User Company tab
    }
}