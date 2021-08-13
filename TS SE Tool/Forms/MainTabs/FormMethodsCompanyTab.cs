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
        private void FillFormCompanyControls()
        {
            textBoxUserCompanyCompanyName.Text = MainSaveFileProfileData.CompanyName;
            //textBoxUserCompanyCompanyName.ReadOnly = false;
            FillAccountMoneyTB();
            FillHQcities();

            listBoxVisitedCities.DrawMode = DrawMode.OwnerDrawVariable;
            listBoxGarages.DrawMode = DrawMode.OwnerDrawVariable;

            FillVisitedCities(0);
            FillGaragesList(0);

            MemoryStream ms = new MemoryStream();

            Bitmap temp = ImageFromDDS(@"img\" + GameType + @"\player_logo\" + MainSaveFileProfileData.Logo + ".dds");
            if (temp != null)
            {
                temp.Clone(new Rectangle(0, 0, 94, 94), temp.PixelFormat).Save(ms, ImageFormat.Png);
                PlayerCompanyLogo = Image.FromStream(ms);
            }
            else
            {
                PlayerCompanyLogo = new Bitmap(94, 94);
            }
            ms.Dispose();

            pictureBoxCompanyLogo.Image = PlayerCompanyLogo;
        }

        public void FillAccountMoneyTB()
        {
            Int64 valueBefore = (long)Math.Floor(PlayerDataData.AccountMoney * CurrencyDictConversion[Globals.CurrencyName]);

            string newtext = "";
            if (CurrencyDictFormat[Globals.CurrencyName][0] != "")
                newtext += CurrencyDictFormat[Globals.CurrencyName][0] + "-";

            newtext += CurrencyDictFormat[Globals.CurrencyName][1] + String.Format(CultureInfo.CurrentCulture, "{0:N0}", valueBefore) + ",-" + CurrencyDictFormat[Globals.CurrencyName][2];

            textBoxUserCompanyMoneyAccount.TextChanged -= textBoxMoneyAccount_TextChanged;
            textBoxUserCompanyMoneyAccount.Text = newtext;
            textBoxUserCompanyMoneyAccount.TextChanged += textBoxMoneyAccount_TextChanged;
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
            comboBoxUserCompanyHQcity.SelectedValue = PlayerDataData.HQcity;
            comboBoxUserCompanyHQcity.EndUpdate();
            comboBoxUserCompanyHQcity.SelectedIndexChanged += comboBoxUserCompanyHQcity_SelectedIndexChanged;
        }

        private void comboBoxUserCompanyHQcity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxUserCompanyHQcity.SelectedValue != null)
                PlayerDataData.HQcity = comboBoxUserCompanyHQcity.SelectedValue.ToString();
        }

        private void textBoxUserCompanyCompanyName_TextChanged(object sender, EventArgs e)
        {
            if (textBoxUserCompanyCompanyName.Text.Length > 20)
            {
                textBoxUserCompanyCompanyName.Text = textBoxUserCompanyCompanyName.Text.Remove(20);
                textBoxUserCompanyCompanyName.Select(20, 0);
            }

            if (textBoxUserCompanyCompanyName.Text.Length == 0)
            {
                labelUserCompanyCompanyName.ForeColor = Color.Red;
                labelCompanyNameSize.ForeColor = Color.Red;
                labelCompanyNameSize.Font = new Font(labelCompanyNameSize.Font, FontStyle.Bold);
            }
            else if (textBoxUserCompanyCompanyName.Text.Length == 20)
            {
                labelUserCompanyCompanyName.ForeColor = Color.FromKnownColor(KnownColor.ControlText);
                labelCompanyNameSize.ForeColor = Color.DarkGreen;
                labelCompanyNameSize.Font = new Font(labelCompanyNameSize.Font, FontStyle.Bold);
            }
            else
            {
                labelUserCompanyCompanyName.ForeColor = Color.FromKnownColor(KnownColor.ControlText);
                labelCompanyNameSize.ForeColor = Color.FromKnownColor(KnownColor.ControlText);
                labelCompanyNameSize.Font = new Font(labelCompanyNameSize.Font, FontStyle.Regular);
            }

            labelCompanyNameSize.Text = textBoxUserCompanyCompanyName.Text.Length.ToString() + " / 20";
        }

        private void textBoxUserCompanyCompanyName_Validating(object sender, CancelEventArgs e)
        {
            TextBox txtbx = sender as TextBox;

            if (txtbx.ReadOnly == false)
                if (txtbx.TextLength == 0)
                {
                    // Cancel the event and select the text to be corrected by the user.
                    MessageBox.Show("Company name empty");
                    e.Cancel = true;
                }
        }

        private void textBoxMoneyAccount_TextChanged(object sender, EventArgs e)
        {
            TextBox textBoxAccountMoney = sender as TextBox;

            string newtext = "";

            if (!string.IsNullOrEmpty(textBoxAccountMoney.Text))
            {
                int testV = textBoxAccountMoney.SelectionStart;

                string onlyDigits = new string(textBoxAccountMoney.Text.Where(c => char.IsDigit(c)).ToArray());
                if (!Int64.TryParse(onlyDigits, NumberStyles.AllowThousands, CultureInfo.CurrentCulture, out long valueBefore))
                {
                    valueBefore = Int64.MaxValue;
                }
                //[sign1] - [sign2] 1.234,- [sign3]
                if (CurrencyDictFormat[Globals.CurrencyName][0] != "")
                    newtext += CurrencyDictFormat[Globals.CurrencyName][0] + "-";

                newtext += CurrencyDictFormat[Globals.CurrencyName][1] + String.Format(CultureInfo.CurrentCulture, "{0:N0}", valueBefore) + ",-" + CurrencyDictFormat[Globals.CurrencyName][2];

                int cSpace1 = textBoxAccountMoney.Text.Substring(0, testV).Count(Char.IsWhiteSpace);

                textBoxAccountMoney.TextChanged -= textBoxMoneyAccount_TextChanged;
                textBoxAccountMoney.Text = newtext;
                textBoxAccountMoney.TextChanged += textBoxMoneyAccount_TextChanged;

                int cSpace2 = textBoxAccountMoney.Text.Substring(0, testV).Count(Char.IsWhiteSpace);

                textBoxAccountMoney.SelectionStart = testV + cSpace2 - cSpace1;

                PlayerDataData.AccountMoney = (long)Math.Round(valueBefore / CurrencyDictConversion[Globals.CurrencyName]);
            }
        }

        private void textBoxMoneyAccount_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox textBoxAccountMoney = sender as TextBox;
            Int64 valueBefore = 0;
            string onlyDigits = new string(textBoxAccountMoney.Text.Where(c => char.IsDigit(c)).ToArray());

            int testV = textBoxAccountMoney.SelectionStart;

            if (!string.IsNullOrEmpty(textBoxAccountMoney.Text))
            {
                if (!Char.IsDigit(e.KeyChar))
                {
                    if (e.KeyChar == (char)Keys.Back)
                    {
                        return;
                    }

                    if (Int64.TryParse(onlyDigits, NumberStyles.AllowThousands, CultureInfo.CurrentCulture, out valueBefore))
                    {
                        textBoxAccountMoney.Text = valueBefore.ToString();
                        e.Handled = true;
                    }
                    else
                    {
                        valueBefore =Int64.MaxValue;
                        textBoxAccountMoney.Text = valueBefore.ToString();
                        e.Handled = true;
                    }
                }
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

            foreach (City vc in from x in CitiesList where !x.Disabled select x)
            {
                listBoxVisitedCities.Items.Add(vc);
            }

            listBoxVisitedCities.TopIndex = _vindex;
            listBoxVisitedCities.EndUpdate();
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

            // Draw the background.
            e.DrawBackground();

            int index = 0;
            if (vc.Visited)
                index = 1;

            Image cityicon = CitiesImg[index];

            // Draw the picture.
            float scale = VisitedCitiesPictureHeight / cityicon.Height;
            RectangleF source_rect = new RectangleF(0, 0, cityicon.Width, cityicon.Height);

            float picture_width = scale * cityicon.Width;

            RectangleF dest_rect = new RectangleF(e.Bounds.Left + VisitedCitiesItemMargin, e.Bounds.Top + VisitedCitiesItemMargin, picture_width, VisitedCitiesPictureHeight);
            e.Graphics.DrawImage(cityicon, dest_rect, source_rect, GraphicsUnit.Pixel);
            ////

            // See if the item is selected.
            Brush br;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                br = SystemBrushes.HighlightText;
            else
                br = new SolidBrush(e.ForeColor);

            // Find the area in which to put the text.
            float x = e.Bounds.Left + picture_width + 3 * VisitedCitiesItemMargin;
            float y = e.Bounds.Top + VisitedCitiesItemMargin * 2;
            float width = e.Bounds.Right - VisitedCitiesItemMargin - x;
            float height = e.Bounds.Bottom - VisitedCitiesItemMargin - y;
            RectangleF layout_rect = new RectangleF(x, y, width, height);

            // Draw the text.
            string txt = "";//, DisplayCityName = "";

            //CitiesLngDict.TryGetValue(vc.CityName, out string value);
            /*
            DisplayCityName = vc.CityNameTranslated;

            if (DisplayCityName != null && DisplayCityName != "")
                txt = DisplayCityName;
            else
            {
                txt = vc.CityName + " -nt";
            }
            */
            txt = vc.CityNameTranslated;
            e.Graphics.DrawString(txt, Font, br, layout_rect);

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

            foreach (Garages garage in from x in GaragesList where !x.IgnoreStatus select x)
            {
                listBoxGarages.Items.Add(garage);
            }

            listBoxGarages.TopIndex = _vindex;
            listBoxGarages.EndUpdate();
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
            string txt = "";
            Garages grg = (Garages)lst.Items[e.Index];

            // Draw the background.
            e.DrawBackground();
            Image grgicon;
            if (grg.GarageName != PlayerDataData.HQcity)
                grgicon = GaragesImg[grg.GarageStatus];
            else
                grgicon = GaragesHQImg[grg.GarageStatus];

            // Draw the picture.
            float scale = GaragePictureHeight / grgicon.Height;
            RectangleF source_rect = new RectangleF(0, 0, grgicon.Width, grgicon.Height);

            float picture_width = scale * grgicon.Width;

            RectangleF dest_rect = new RectangleF(e.Bounds.Left + GarageItemMargin, e.Bounds.Top + GarageItemMargin, picture_width, GaragePictureHeight);
            e.Graphics.DrawImage(grgicon, dest_rect, source_rect, GraphicsUnit.Pixel);
            ////

            // See if the item is selected.
            Brush br;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                br = SystemBrushes.HighlightText;
            else
                br = new SolidBrush(e.ForeColor);

            int maxvehdr = 0;

            if (grg.GarageStatus == 0)
                goto skipVehAndDrDraw;//"Not owned";
            else if (grg.GarageStatus == 2)
                maxvehdr = 3;
            else if (grg.GarageStatus == 3)
                maxvehdr = 5;
            else if (grg.GarageStatus == 6)
                maxvehdr = 1;

            //Vehicles & Drivers

            int curVeh = 0, curDr = 0;

            foreach (string temp in grg.Vehicles)
            {
                if (temp != null)
                    curVeh++;
            }
            foreach (string temp in grg.Drivers)
            {
                if (temp != null)
                    curDr++;
            }

            string Vs = "", Ds = "", Ts = "";

            Vs = ResourceManagerMain.GetString("VehicleShort", Thread.CurrentThread.CurrentUICulture);
            Ds = ResourceManagerMain.GetString("DriverShort", Thread.CurrentThread.CurrentUICulture);
            Ts = ResourceManagerMain.GetString("TrailerShort", Thread.CurrentThread.CurrentUICulture);

            txt = Vs + ": " + curVeh + " / " + maxvehdr + " " + Ds + ": " + curDr + " / " + maxvehdr + " " + Ts + ": " + grg.Trailers.Count;

            Size size = TextRenderer.MeasureText(txt, this.Font);

            float x = e.Bounds.Right - size.Width - 3;
            float y = e.Bounds.Top + 18;
            float width = e.Bounds.Right - 100;
            float height = e.Bounds.Bottom - 14;

            RectangleF layout_rect = new RectangleF(x, y, size.Width, height);

            // Draw the text.
            e.Graphics.DrawString(txt, this.Font, br, layout_rect);

            skipVehAndDrDraw:;

            //City and Size
            // Find the area in which to put the text.
            x = e.Bounds.Left + picture_width + 3 * GarageItemMargin;
            y = e.Bounds.Top + GarageItemMargin * 2;
            width = e.Bounds.Right - GarageItemMargin - x;
            height = e.Bounds.Bottom - GarageItemMargin - y;
            layout_rect = new RectangleF(x, y, width, height);

            //txt = lst.Items[e.Index].ToString();
            txt = grg.GarageNameTranslated + "\n" + grg.GetStatusString();
            // Draw the text.
            e.Graphics.DrawString(txt, this.Font, br, layout_rect);

            // Draw the focus rectangle if appropriate.
            e.DrawFocusRectangle();
        }
        
        //Buttons
        private void buttonUserCompanyGaragesManage_Click(object sender, EventArgs e)
        {
            PrepareGarages();

            FormGaragesSoldContent testDialog = new FormGaragesSoldContent();
            testDialog.ShowDialog(this);
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
                if (garage.GarageName == PlayerDataData.HQcity)
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