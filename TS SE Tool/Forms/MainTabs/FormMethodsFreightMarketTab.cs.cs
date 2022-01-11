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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace TS_SE_Tool
{
    public partial class FormMain
    {
        //Freight market tab
        private void FillFormFreightMarketControls()
        {
            FillcomboBoxCargoList();
            FillcomboBoxCountries();
            FillcomboBoxCompanies();
            FillcomboBoxUrgencyList();
            FillcomboBoxSourceCity();
        }
        
        //Job list
        private int JobsItemMargin = 3;
        private const float JobsPictureHeight = 32, JobsTextHeigh = 23, JobsItemHeight = 64;

        private void listBoxAddedJobs_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = (int)(JobsItemHeight + 2 * JobsItemMargin);
        }

        private void listBoxAddedJobs_DrawItem(object sender, DrawItemEventArgs e)
        {
            try
            {
                // Get the ListBox and the item.
                ListBox lst = sender as ListBox;

                if (lst.Items.Count > 0)
                {

                    JobAdded Job = (JobAdded)lst.Items[e.Index];

                    // Draw the background.
                    e.DrawBackground();

                    // See if the item is selected.
                    Brush br;
                    if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                        br = SystemBrushes.HighlightText;
                    else
                        br = new SolidBrush(e.ForeColor);

                    Image SourceCompIcon = null, DestinationCompIcon = null;

                    if (File.Exists(@"img\" + GameType + @"\companies\" + Job.SourceCompany + ".dds"))
                        SourceCompIcon = ExtImgLoader(new string[] { @"img\" + GameType + @"\companies\" + Job.SourceCompany + ".dds" }, 100, 32, 0, 0)[0];
                    else
                    {
                        string currentDirName = Directory.GetCurrentDirectory() + @"\img\" + GameType + @"\companies";
                        string searchpattern = Job.SourceCompany.Split(new char[] { '_' })[0] + "*.dds";
                        string[] files = Directory.GetFiles(currentDirName, searchpattern);

                        if (files.Length > 0)
                            SourceCompIcon = ExtImgLoader(new string[] { files[0] }, 100, 32, 0, 0)[0];
                        else
                        {
                            SourceCompIcon = DrawCompanyText2(Job.SourceCompany, 100, 32, br, 12);
                        }
                    }

                    if (File.Exists(@"img\" + GameType + @"\companies\" + Job.DestinationCompany + ".dds"))
                        DestinationCompIcon = ExtImgLoader(new string[] { @"img\" + GameType + @"\companies\" + Job.DestinationCompany + ".dds" }, 100, 32, 0, 0)[0];
                    else
                    {
                        string currentDirName = Directory.GetCurrentDirectory() + @"\img\" + GameType + @"\companies";
                        string searchpattern = Job.DestinationCompany.Split(new char[] { '_' })[0] + "*.dds";
                        string[] files = Directory.GetFiles(currentDirName, searchpattern);

                        if (files.Length > 0)
                            DestinationCompIcon = ExtImgLoader(new string[] { files[0] }, 100, 32, 0, 0)[0];
                        else
                        {
                            DestinationCompIcon = DrawCompanyText2(Job.DestinationCompany, 100, 32, br, 12);
                        }
                    }

                    // Draw the Source comp. picture
                    float scale = JobsPictureHeight / SourceCompIcon.Height;
                    RectangleF source_rect = new RectangleF(0, 0, SourceCompIcon.Width, SourceCompIcon.Height);

                    float picture_width = scale * SourceCompIcon.Width;

                    RectangleF dest_rect = new RectangleF(e.Bounds.Left + JobsItemMargin, e.Bounds.Top + JobsItemMargin * 2 + JobsTextHeigh, picture_width, JobsPictureHeight);
                    e.Graphics.DrawImage(SourceCompIcon, dest_rect, source_rect, GraphicsUnit.Pixel);


                    // Draw the Destination comp. picture
                    dest_rect = new RectangleF(e.Bounds.Right - JobsItemMargin - picture_width, e.Bounds.Top + JobsItemMargin * 2 + JobsTextHeigh, picture_width, JobsPictureHeight);
                    e.Graphics.DrawImage(DestinationCompIcon, dest_rect, source_rect, GraphicsUnit.Pixel);
                    ////
                    // Draw Type picture
                    Image[] TypeImgs = new Image[5];
                    int indexTypeImgs = 0, CargoMass = 0;
                    bool extheavy = false;

                    try
                    {
                        ExtCargo tempExtCargo = ExtCargoList.Find(z => z.CargoName == Job.Cargo);

                        if (tempExtCargo != null)
                        {
                            decimal fragile = tempExtCargo.Fragility;
                            bool valuable = tempExtCargo.Valuable;
                            int trueADR = tempExtCargo.ADRclass;

                            switch (trueADR)
                            {
                                case 6:
                                    {
                                        trueADR = 5;
                                        break;
                                    }
                                case 8:
                                    {
                                        trueADR = 6;
                                        break;
                                    }
                            }

                            CargoMass = (int)(tempExtCargo.Mass * Job.UnitsCount);
                            if (CargoMass > 26000)
                                extheavy = true;

                            if (trueADR > 0)
                            {
                                Bitmap bmp = new Bitmap(32, 32);
                                Graphics graph = Graphics.FromImage(bmp);
                                graph.DrawImage(ADRImgS[trueADR - 1], 2, 2, 28, 28);

                                TypeImgs[indexTypeImgs] = bmp;
                                indexTypeImgs++;
                            }

                            if (fragile == 0 || fragile >= (decimal)0.7)
                            {
                                TypeImgs[indexTypeImgs] = CargoType2Img[0];
                                indexTypeImgs++;
                            }

                            if (valuable)
                            {
                                TypeImgs[indexTypeImgs] = CargoType2Img[1];
                                indexTypeImgs++;
                            }
                        }
                    }
                    catch
                    { }

                    if (extheavy || Job.Type == 1)
                    {
                        TypeImgs[indexTypeImgs] = CargoTypeImg[1];
                        indexTypeImgs++;
                    }

                    if (Job.Type == 2)
                    {
                        TypeImgs[indexTypeImgs] = CargoTypeImg[2];
                        indexTypeImgs++;
                    }

                    TypeImgs[indexTypeImgs] = UrgencyImg[Job.Urgency];

                    int xmult = 0, images = 0;

                    foreach (Image temp in TypeImgs)
                    {
                        if (temp == null)
                        {
                            break;
                        }
                        images++;
                    }

                    // Draw Cargo type Images
                    for (int i = 0; i < images; i++)
                    {
                        if (TypeImgs[i] == null)
                        {
                            break;
                        }

                        source_rect = new RectangleF(0, 0, 32, 32);
                        dest_rect = new RectangleF((e.Bounds.Right - e.Bounds.Left - 32 * images) / 2 + 32 * xmult, e.Bounds.Top + JobsItemMargin, 32, 32);
                        e.Graphics.DrawImage(TypeImgs[i], dest_rect, source_rect, GraphicsUnit.Pixel);

                        xmult++;
                    }


                    // Draw the text.
                    string value = "", SourceCityName = "", DestinationCityName = "";

                    CitiesLngDict.TryGetValue(Job.SourceCity, out value);
                    if (value != null && value != "")
                        SourceCityName = value;
                    else
                    {
                        SourceCityName = Job.SourceCity + " -nt";
                    }

                    CitiesLngDict.TryGetValue(Job.DestinationCity, out value);
                    if (value != null && value != "")
                        DestinationCityName = value;
                    else
                    {
                        DestinationCityName = Job.DestinationCity + " -nt";
                    }

                    // Find the area in which to put the text.
                    float x = e.Bounds.Left + JobsItemMargin;
                    float y = e.Bounds.Top - JobsItemMargin + JobsTextHeigh / 2;
                    float width = (e.Bounds.Right - e.Bounds.Left - JobsItemMargin * 4 - UrgencyImg[Job.Urgency].Width) / 2;
                    float height = JobsTextHeigh;

                    RectangleF layout_rect = new RectangleF(x, y, width, height);

                    StringFormat format = new StringFormat();
                    string txt = "";

                    //Extra fonts
                    Font BoldFont = new Font(this.Font, FontStyle.Bold);

                    //Source City
                    string CountryName = CitiesList.Find(xc => xc.CityName == Job.SourceCity).Country;
                    int CountryCodeLength = 0;

                    if (CountryName != "")
                    {
                        if (CountriesDataList.ContainsKey(CountryName))
                        {
                            txt = "(" + CountriesDataList[CountryName].ShortName + ") ";
                        }
                        else
                        {
                            txt = "(" + CitiesList.Find(xc => xc.CityName == Job.SourceCity).Country.First() + ") ";
                        }

                        CountryCodeLength = Convert.ToInt32(e.Graphics.MeasureString(txt, BoldFont).Width);

                        format.Alignment = StringAlignment.Near;
                        layout_rect.Width = CountryCodeLength + JobsItemMargin;

                        e.Graphics.DrawString(txt, BoldFont, br, layout_rect, format);
                    }

                    format = new StringFormat();
                    format.Alignment = StringAlignment.Near;
                    layout_rect.X += +CountryCodeLength + JobsItemMargin;
                    layout_rect.Width = width - CountryCodeLength - JobsItemMargin;

                    txt = SourceCityName;

                    //Draw
                    e.Graphics.DrawString(txt, this.Font, br, layout_rect, format);

                    //Destination City
                    // Find the area in which to put the text.

                    CountryName = CitiesList.Find(xc => xc.CityName == Job.DestinationCity).Country;
                    CountryCodeLength = 0;

                    format.Alignment = StringAlignment.Far;

                    if (CountryName != "")
                    {
                        if (CountriesDataList.ContainsKey(CountryName))
                        {
                            txt = " (" + CountriesDataList[CountryName].ShortName + ")";
                        }
                        else
                        {
                            txt = " (" + CitiesList.Find(xc => xc.CityName == Job.DestinationCity).Country.First() + ")";
                        }
                        CountryCodeLength = Convert.ToInt32(e.Graphics.MeasureString(txt, BoldFont).Width);

                        layout_rect.Width = CountryCodeLength + JobsItemMargin;
                        layout_rect.X = e.Bounds.Right - CountryCodeLength - JobsItemMargin * 2;
                        e.Graphics.DrawString(txt, BoldFont, br, layout_rect, format);
                    }

                    x = e.Bounds.Left + width + 3 * JobsItemMargin + UrgencyImg[Job.Urgency].Width;
                    layout_rect.X = x;
                    layout_rect.Width = width - CountryCodeLength;

                    txt = DestinationCityName;

                    //Draw
                    e.Graphics.DrawString(txt, this.Font, br, layout_rect, format);

                    //Cargo
                    // Find the area in which to put the text.
                    x = e.Bounds.Left + picture_width + 4 * JobsItemMargin;
                    y = e.Bounds.Top + JobsItemMargin * 2 + UrgencyImg[Job.Urgency].Height;
                    width = e.Bounds.Right - JobsItemMargin - x;
                    height = e.Bounds.Bottom - JobsItemMargin - y;
                    layout_rect = new RectangleF(x, y, width, height);

                    if (CargoLngDict.TryGetValue(Job.Cargo, out string CargoName))
                    {
                        if (CargoName != null && CargoName != "")
                        {
                            txt = CargoName;
                        }
                        else
                            txt = Job.Cargo;
                    }
                    else
                        txt = Job.Cargo;

                    if (CargoMass > 0)
                        txt += " (" + Math.Floor(CargoMass * WeightMultiplier).ToString() + " " + ProgSettingsV.WeightMes + ")";

                    e.Graphics.DrawString(txt, this.Font, br, layout_rect);

                    // Find the area in which to put Distance text.
                    if (Job.Distance == 11111)
                    {
                        txt = Math.Floor(5 * DistanceMultiplier).ToString() + "* ";
                    }
                    else
                    {
                        txt = Math.Floor(Job.Distance * DistanceMultiplier).ToString() + " ";
                    }

                    txt += ProgSettingsV.DistanceMes + " ";

                    if (Job.Ferrytime > 0)
                    {
                        txt += "(Ferry ";

                        if (Job.Ferrytime < 60)
                            txt += Job.Ferrytime.ToString() + "min - ";
                        else
                            txt += (Job.Ferrytime / 60).ToString() + "h - ";

                        //Currency
                        string currencyText = "";
                        long newValue = (long)Math.Floor(Job.Ferryprice * CurrencyDictConversion[Globals.CurrencyName]);

                        if (CurrencyDictFormat[Globals.CurrencyName][0] != "")
                            currencyText += CurrencyDictFormat[Globals.CurrencyName][0] + "-";

                        currencyText += CurrencyDictFormat[Globals.CurrencyName][1] + String.Format(CultureInfo.CurrentCulture, "{0:N0}", newValue) + ",-" + CurrencyDictFormat[Globals.CurrencyName][2];
                        
                        txt += currencyText + ")";
                        ///
                    }

                    layout_rect = new RectangleF(x, y + 14, width, height);
                    e.Graphics.DrawString(txt, this.Font, br, layout_rect);

                    // Draw the focus rectangle if appropriate.
                    e.DrawFocusRectangle();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source);
            }
        }

        public Bitmap DrawCompanyText(string _companyName, int _width, int _height, Brush _brush)
        {
            Bitmap bmp = new Bitmap(100, 32);
            RectangleF rectf = new RectangleF(5, 5, 90, 22);

            Graphics g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            g.DrawString(_companyName, new Font(Font.FontFamily, 12), _brush, rectf, format);
            g.Flush();

            return bmp;
        }

        public Bitmap DrawCompanyText2(string _companyName, int _width, int _height, Brush _brush, int _fontsize)
        {
            Bitmap bmp = new Bitmap(_width, _height);
            RectangleF rectf = new RectangleF(5, 5, 90, 22);

            Graphics g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            GraphicsPath p = new GraphicsPath();
            p.AddString(_companyName, Font.FontFamily, (int)FontStyle.Regular, g.DpiY * _fontsize / 72, rectf, format);

            g.DrawPath(new Pen(_brush), p);
            g.FillPath(_brush, p);

            g.Flush();
            //

            return bmp;
        }

        //Main countries
        public void FillcomboBoxCountries()
        {
            DataTable combDT = new DataTable();
            DataColumn dc = new DataColumn("Country", typeof(string));
            combDT.Columns.Add(dc);

            dc = new DataColumn("CountryName", typeof(string));
            combDT.Columns.Add(dc);

            string value = null;

            foreach (string tempitem in CountriesList)
            {
                value = null;
                CountriesLngDict.TryGetValue(tempitem, out value);

                if (value != null && value != "")
                {
                    combDT.Rows.Add(tempitem, value);
                }
                else
                {
                    string CapName = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(tempitem);
                    combDT.Rows.Add(tempitem, CapName);
                }
            }

            combDT.DefaultView.Sort = "CountryName ASC";
            //All item
            DataTable dt2 = combDT.DefaultView.ToTable();
            DataRow row1 = dt2.NewRow();
            row1[0] = "+all";

            CountriesLngDict.TryGetValue("+all", out value);

            if (value != null && value != "")
            {
                row1[1] = value;
            }
            else
            {
                row1[1] = "All";
            }

            dt2.Rows.InsertAt(row1, 0);

            List<City> cities = CitiesList.FindAll(x => !x.Disabled && x.Country == "");

            if (cities.Count > 0)
            {
                //Unsorted item
                row1 = dt2.NewRow();
                row1[0] = "+unsorted";

                CountriesLngDict.TryGetValue("+unsorted", out value);

                if (value != null && value != "")
                {
                    row1[1] = value;
                }
                else
                {
                    row1[1] = "Unsorted";
                }

                dt2.Rows.InsertAt(row1, 1);
            }

            //Finish
            comboBoxFreightMarketCountries.ValueMember = "Country";
            comboBoxFreightMarketCountries.DisplayMember = "CountryName";
            comboBoxFreightMarketCountries.DataSource = dt2;
            comboBoxFreightMarketCountries.SelectedValue = "+all";
        }

        private void comboBoxCountries_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxFreightMarketCountries.SelectedIndex != -1)
                triggerDestinationCitiesUpdate();
        }

        //Main companies
        public void FillcomboBoxCompanies()
        {
            //start filtering
            List<string> tempCompList = new List<string>();
            Dictionary<string, string> sourceCompList = new Dictionary<string, string>();

            foreach (City city in CitiesList.FindAll(x => !x.Disabled))
            {
                List<Company> source = city.ReturnCompanies();

                foreach (Company company in from x in source where !x.Excluded select x)
                {
                    if (!sourceCompList.ContainsKey(company.CompanyName))
                        sourceCompList.Add(company.CompanyName, company.CompanyNameTranslated);
                }
            }
            //end filtering

            DataTable combDT = new DataTable();
            DataColumn dc = new DataColumn("Company", typeof(string));
            combDT.Columns.Add(dc);

            dc = new DataColumn("CompanyName", typeof(string));
            combDT.Columns.Add(dc);

            foreach (KeyValuePair<string, string> tempitem in sourceCompList)
            {
                combDT.Rows.Add(tempitem.Key, tempitem.Value);
            }

            DataTable sortedDT = combDT.DefaultView.Table.Copy();

            DataView dv = sortedDT.DefaultView;
            dv.Sort = "CompanyName ASC";
            sortedDT = dv.ToTable();
            sortedDT.DefaultView.Sort = "";

            DataRow row = sortedDT.NewRow();
            string tvalue = "All";
            CompaniesLngDict.TryGetValue("+all", out tvalue);
            row.ItemArray = new object[] { "+all", tvalue };

            sortedDT.Rows.InsertAt(row, 0);
            //
            comboBoxFreightMarketCompanies.ValueMember = "Company";
            comboBoxFreightMarketCompanies.DisplayMember = "CompanyName";
            comboBoxFreightMarketCompanies.DataSource = sortedDT;
            //end filling
        }

        private void comboBoxCompanies_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxFreightMarketCompanies.SelectedIndex != -1)
                triggerDestinationCitiesUpdate();
        }
        
        //Source city
        public void FillcomboBoxSourceCity()
        {
            DataTable combDT = new DataTable();
            DataColumn dc = new DataColumn("City", typeof(string));
            combDT.Columns.Add(dc);

            var keys = new DataColumn[1];
            keys[0] = dc;
            // Set the PrimaryKeys property to the array.
            combDT.PrimaryKey = keys;

            dc = new DataColumn("CityName", typeof(string));
            combDT.Columns.Add(dc);

            //start filling

            //fill source and destination cities
            foreach (City tempcity in from x in CitiesList where !x.Disabled select x)
            {
                combDT.Rows.Add(tempcity.CityName, tempcity.CityNameTranslated);
            }
            combDT.DefaultView.Sort = "CityName ASC";

            comboBoxFreightMarketSourceCity.ValueMember = "City";
            comboBoxFreightMarketSourceCity.DisplayMember = "CityName";
            comboBoxFreightMarketSourceCity.DataSource = combDT;
            //end filling

            DataRow foundRow = combDT.Rows.Find(new object[1] { Economy.last_visited_city });

            if (combDT.Rows.Find(new object[1] { Economy.last_visited_city }) != null)
                comboBoxFreightMarketSourceCity.SelectedValue = Economy.last_visited_city;
            //end
        }

        private void comboBoxSourceCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxFreightMarketSourceCity.SelectedValue == null)
                return;

            string _sourceCityName = comboBoxFreightMarketSourceCity.SelectedValue.ToString();

            comboBoxFreightMarketSourceCompany.SelectedIndex = -1;

            List<Company> sourceCompaniesList = CitiesList.Find(x => x.CityName == _sourceCityName).ReturnCompanies().FindAll(x => !x.Excluded);

            DataTable combDT = new DataTable();
            DataColumn dc = new DataColumn("Company", typeof(string));
            combDT.Columns.Add(dc);

            dc = new DataColumn("CompanyName", typeof(string));
            combDT.Columns.Add(dc);

            foreach (Company company in sourceCompaniesList)
                combDT.Rows.Add(company.CompanyName, company.CompanyNameTranslated);

            combDT.DefaultView.Sort = "CompanyName ASC";

            comboBoxFreightMarketSourceCompany.ValueMember = "Company";
            comboBoxFreightMarketSourceCompany.DisplayMember = "CompanyName";

            comboBoxFreightMarketSourceCompany.DataSource = combDT;

            if (ProgSettingsV.ProposeRandom && (comboBoxFreightMarketSourceCompany.Items.Count > 0))
            {
                comboBoxFreightMarketSourceCompany.SelectedIndex = RandomValue.Next(comboBoxFreightMarketSourceCompany.Items.Count);
            }
        }
        
        //Source company
        private void comboBoxSourceCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxFreightMarketSourceCompany.SelectedIndex != -1)
                if (ProgSettingsV.ProposeRandom)
                {
                    comboBoxFreightMarketCargoList.SelectedIndex = RandomValue.Next(comboBoxFreightMarketCargoList.Items.Count);
                }
        }
        
        //Destination city
        private void comboBoxDestinationCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxFreightMarketDestinationCity.SelectedIndex >= 0)
            {
                comboBoxFreightMarketDestinationCompany.SelectedIndex = -1;
                comboBoxFreightMarketDestinationCompany.Text = "";

                triggerDestinationCompaniesUpdate();

                if (comboBoxFreightMarketCompanies.SelectedValue.ToString() != "+all")
                {
                    comboBoxFreightMarketDestinationCompany.SelectedValue = comboBoxFreightMarketCompanies.SelectedValue;
                }
                else if (ProgSettingsV.ProposeRandom && (comboBoxFreightMarketDestinationCompany.Items.Count > 0))
                {
                    if ((comboBoxFreightMarketDestinationCompany.Items.Count != 1) && (comboBoxFreightMarketSourceCity.SelectedValue == comboBoxFreightMarketDestinationCity.SelectedValue))
                    {
                        int rnd = 0;
                        while (true)
                        {
                            rnd = RandomValue.Next(comboBoxFreightMarketDestinationCompany.Items.Count);
                            if (comboBoxFreightMarketSourceCompany.SelectedIndex != rnd)
                            {
                                comboBoxFreightMarketDestinationCompany.SelectedIndex = rnd;
                                break;
                            }
                        }
                    }
                    else
                        comboBoxFreightMarketDestinationCompany.SelectedIndex = RandomValue.Next(comboBoxFreightMarketDestinationCompany.Items.Count);
                }
            }
        }

        private void triggerDestinationCitiesUpdate()
        {
            if (comboBoxFreightMarketCountries.SelectedIndex != -1 && comboBoxFreightMarketCompanies.SelectedIndex != -1)
                SetupDestinationCities(!(comboBoxFreightMarketCountries.SelectedValue.ToString() == "+all"), !(comboBoxFreightMarketCompanies.SelectedValue.ToString() == "+all"));
        }

        private void SetupDestinationCities(bool _country_selected, bool _company_selected)
        {
            DataTable combDT = new DataTable();
            DataColumn dc = new DataColumn("City", typeof(string));
            combDT.Columns.Add(dc);

            dc = new DataColumn("CityName", typeof(string));
            combDT.Columns.Add(dc);

            //start filling
            List<City> cities = CitiesList.FindAll(x => !x.Disabled);

            if (_country_selected)// && !checkBoxFreightMarketFilterDestination.Checked)
            {
                if (comboBoxFreightMarketCountries.SelectedValue.ToString() == "+unsorted")
                {
                    cities = cities.FindAll(x => x.Country == "");
                }
                else
                    cities = cities.FindAll(x => x.Country == comboBoxFreightMarketCountries.SelectedValue.ToString());
            }

            if (cities.Count > 0)
            {
                comboBoxFreightMarketDestinationCity.Enabled = false;
                comboBoxFreightMarketDestinationCompany.Enabled = false;

                foreach (City city in cities)
                {
                    List<Company> companyList = city.ReturnCompanies();

                    if (!_company_selected)
                    {
                    }
                    else
                    if (_company_selected && checkBoxFreightMarketFilterDestination.Checked)
                    {
                        companyList = companyList.FindAll(x => (x.CompanyName == comboBoxFreightMarketCompanies.SelectedValue.ToString()) && !x.Excluded);
                    }
                    else
                    if (!(_company_selected || !checkBoxFreightMarketFilterDestination.Checked))
                    {
                        companyList = companyList.FindAll(x => !x.Excluded);
                    }
                    else
                    if (_company_selected && !checkBoxFreightMarketFilterDestination.Checked)
                    {
                        companyList = companyList.FindAll(x => x.CompanyName == comboBoxFreightMarketCompanies.SelectedValue.ToString());
                    }

                    if (companyList.Count > 0)
                    {
                        combDT.Rows.Add(city.CityName, city.CityNameTranslated);
                        comboBoxFreightMarketDestinationCity.Enabled = true;
                        comboBoxFreightMarketDestinationCompany.Enabled = true;
                    }
                }
            }
            else
            {
                comboBoxFreightMarketDestinationCity.Text = "";
                comboBoxFreightMarketDestinationCity.Enabled = false;
                comboBoxFreightMarketDestinationCompany.Text = "";
                comboBoxFreightMarketDestinationCompany.Enabled = false;
            }

            combDT.DefaultView.Sort = "CityName ASC";

            comboBoxFreightMarketDestinationCity.ValueMember = "City";
            comboBoxFreightMarketDestinationCity.DisplayMember = "CityName";
            comboBoxFreightMarketDestinationCity.SelectedIndexChanged -= comboBoxDestinationCity_SelectedIndexChanged;

            string prevValue = null;
            if (comboBoxFreightMarketDestinationCity.SelectedIndex > -1)
                prevValue = comboBoxFreightMarketDestinationCity.SelectedValue.ToString();
            comboBoxFreightMarketDestinationCity.DataSource = combDT;

            comboBoxFreightMarketDestinationCity.SelectedIndexChanged += comboBoxDestinationCity_SelectedIndexChanged;
            //end filling

            if (comboBoxFreightMarketDestinationCity.Items.Count == 0)
            {
                comboBoxFreightMarketDestinationCity.SelectedIndex = -1;
                comboBoxFreightMarketDestinationCity.Text = "";
                comboBoxFreightMarketDestinationCompany.Text = "";

                UpdateStatusBarMessage.ShowStatusMessage(SMStatus.Error, "message_no_matching_cities");
            }
            else
            {
                if (prevValue == null || FindByValue(comboBoxFreightMarketDestinationCity, prevValue) == -1)
                    comboBoxFreightMarketDestinationCity.SelectedIndex = GetRandomCBindex(comboBoxFreightMarketDestinationCity.SelectedIndex, comboBoxFreightMarketDestinationCity.Items.Count);
                else
                    comboBoxFreightMarketDestinationCity.SelectedValue = prevValue;

                UpdateStatusBarMessage.ShowStatusMessage(SMStatus.Clear);
            }

        }
        
        //Destination companies
        private void triggerDestinationCompaniesUpdate()
        {
            SetupDestinationCompanies(!(comboBoxFreightMarketCompanies.SelectedValue.ToString() == "+all"));
        }

        private void SetupDestinationCompanies(bool _company_selected)
        {
            List<Company> CityCompanies = CitiesList.Find(x => x.CityName == comboBoxFreightMarketDestinationCity.SelectedValue.ToString()).ReturnCompanies();
            List<Company> RealCompanies = CityCompanies.FindAll(x => !x.Excluded);

            if (_company_selected && checkBoxFreightMarketFilterDestination.Checked)
            {
                RealCompanies = RealCompanies.FindAll(x => (x.CompanyName == comboBoxFreightMarketCompanies.SelectedValue.ToString()));
            }

            DataTable combDT = new DataTable();
            DataColumn dc = new DataColumn("Company", typeof(string));
            combDT.Columns.Add(dc);

            dc = new DataColumn("CompanyName", typeof(string));
            combDT.Columns.Add(dc);

            foreach (Company company in RealCompanies)
            {
                combDT.Rows.Add(company.CompanyName, company.CompanyNameTranslated);
            }

            combDT.DefaultView.Sort = "CompanyName ASC";
            comboBoxFreightMarketDestinationCompany.ValueMember = "Company";
            comboBoxFreightMarketDestinationCompany.DisplayMember = "CompanyName";

            comboBoxFreightMarketDestinationCompany.DataSource = combDT;
        }

        private void comboBoxDestinationCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxFreightMarketDestinationCompany.SelectedIndex != -1)
                if (ProgSettingsV.ProposeRandom)
                {
                    comboBoxFreightMarketCargoList.SelectedIndex = RandomValue.Next(comboBoxFreightMarketCargoList.Items.Count);
                }
        }
        
        //Cargo list
        public void FillcomboBoxCargoList()
        {
            DataTable combDT = new DataTable();
            DataColumn dc = new DataColumn("Cargo", typeof(string));
            combDT.Columns.Add(dc);

            dc = new DataColumn("CargoName", typeof(string));
            combDT.Columns.Add(dc);

            foreach (Cargo tempitem in CargoesList)
            {
                if (CargoLngDict.TryGetValue(tempitem.CargoName, out string value))
                {
                    if (value != null && value != "")
                    {
                        string str = tempitem.CargoName;

                        combDT.Rows.Add(str, value);
                    }
                    else
                    {
                        string str = tempitem.CargoName;
                        string CapName = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(str);

                        combDT.Rows.Add(str, CapName);
                    }
                }
                else
                {
                    string str = tempitem.CargoName;
                    string CapName = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(str);

                    combDT.Rows.Add(str, CapName);
                }
            }

            combDT.DefaultView.Sort = "CargoName ASC";

            comboBoxFreightMarketCargoList.ValueMember = "Cargo";
            comboBoxFreightMarketCargoList.DisplayMember = "CargoName";
            comboBoxFreightMarketCargoList.DataSource = combDT;
        }

        private void comboBoxFreightMarketCargoList_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = 26;
        }

        private void comboBoxFreightMarketCargoList_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
                return;

            //if ((e.State & DrawItemState.ComboBoxEdit) == DrawItemState.ComboBoxEdit)
            //    return;

            ComboBox lst = sender as ComboBox;

            // Draw the background of the item.
            e.DrawBackground();

            // See if the item is selected.
            Brush br;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                br = SystemBrushes.HighlightText;
            else
                br = new SolidBrush(e.ForeColor);

            float x = e.Bounds.Left;
            float y = e.Bounds.Top;
            float width = e.Bounds.Width;
            float height = e.Bounds.Height;
            RectangleF layout_rect;

            string CargoName = ((DataRowView)lst.Items[e.Index])[0].ToString(),
                CargoDN = lst.GetItemText(lst.Items[e.Index]);

            if (CargoName.EndsWith("_c"))
                CargoDN += " (Cont)";

            string txt = CargoDN;

            //////
            // Draw Type picture
            Image[] TypeImgs = new Image[5];
            int indexTypeImgs = 0;
            bool extheavy = false;

            try
            {
                ExtCargo tempExtCargo = ExtCargoList.Find(z => z.CargoName == CargoName);

                if (tempExtCargo != null)
                {
                    decimal fragile = tempExtCargo.Fragility;
                    bool valuable = tempExtCargo.Valuable;
                    bool overweight = tempExtCargo.Overweight;
                    int ADRclass = tempExtCargo.ADRclass;
                    int trueADR = ADRclass;

                    switch (ADRclass)
                    {
                        case 6:
                            {
                                trueADR = 5;
                                break;
                            }
                        case 8:
                            {
                                trueADR = 6;
                                break;
                            }
                    }

                    if (trueADR > 0)
                    {
                        Bitmap bmp = new Bitmap(32, 32);
                        Graphics graph = Graphics.FromImage(bmp);
                        graph.DrawImage(ADRImgS[trueADR - 1], 2, 2, 28, 28);

                        TypeImgs[indexTypeImgs] = bmp;
                        indexTypeImgs++;
                    }

                    if (fragile == 0 || fragile >= (decimal)0.7)
                    {
                        TypeImgs[indexTypeImgs] = CargoType2Img[0];
                        indexTypeImgs++;
                    }

                    if (valuable)
                    {
                        TypeImgs[indexTypeImgs] = CargoType2Img[1];
                        indexTypeImgs++;
                    }

                    if (overweight)
                    {
                        extheavy = true;
                    }
                }
            }
            catch
            {
            }

            if (extheavy)
            {
                TypeImgs[indexTypeImgs] = CargoTypeImg[1];
                indexTypeImgs++;
            }
            /*
            if (CargoType == "2")
            {
                TypeImgs[indexTypeImgs] = CargoTypeImg[2];
                indexTypeImgs++;
            }
            */
            int xmult = 0, images = 0;

            foreach (Image temp in TypeImgs)
            {
                if (temp == null)
                {
                    break;
                }
                images++;
            }

            for (int i = 0; i < images; i++)
            {
                if (TypeImgs[i] == null)
                {
                    break;
                }

                RectangleF source_rect = new RectangleF(0, 0, 32, 32);
                RectangleF dest_rect = new RectangleF((e.Bounds.Right - 26 * images) + 24 * xmult, e.Bounds.Top + 1, 24, 24);
                e.Graphics.DrawImage(TypeImgs[i], dest_rect, source_rect, GraphicsUnit.Pixel);

                xmult++;
            }
            /////

            // Find the area in which to put the text.
            float fntsize = 8.25f;
            y = e.Bounds.Top + (e.Bounds.Height - 4 - fntsize) / 2;
            layout_rect = new RectangleF(x, y, width, height);
            //format.Alignment = StringAlignment.Far;
            Font textfnt = new Font("Microsoft Sans Serif", fntsize);
            e.Graphics.DrawString(txt, textfnt, br, layout_rect);

            // Draw the focus rectangle if the mouse hovers over an item.
            e.DrawFocusRectangle();
        }

        private void comboBoxCargoList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ProgSettingsV.ProposeRandom && comboBoxFreightMarketUrgency.Items.Count > 0)
            {
                comboBoxFreightMarketUrgency.SelectedIndex = RandomValue.Next(comboBoxFreightMarketUrgency.Items.Count);
            }
            ComboBox temp = sender as ComboBox;
            if (temp.SelectedIndex > -1)
                FillcomboBoxTrailerDefList();
        }
        
        //Urgency
        public void FillcomboBoxUrgencyList()
        {
            DataTable combDT = new DataTable();
            combDT.Columns.Add("ID");
            combDT.Columns.Add("UrgencyDisplayName");

            foreach (int tempitem in UrgencyArray)
            {
                string str = tempitem.ToString();
                if (UrgencyLngDict.TryGetValue(str, out string value))
                {
                    if (value != null && value != "")
                    {
                        combDT.Rows.Add(str, value);
                    }
                    else
                    {
                        combDT.Rows.Add(str, str);
                    }
                }
                else
                {
                    combDT.Rows.Add(str, str);
                }
            }

            comboBoxFreightMarketUrgency.ValueMember = "ID";
            comboBoxFreightMarketUrgency.DisplayMember = "UrgencyDisplayName";
            comboBoxFreightMarketUrgency.DataSource = combDT;
        }

        private void comboBoxFreightMarketUrgency_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = 26;
        }

        private void comboBoxFreightMarketUrgency_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
                return;

            ComboBox lst = sender as ComboBox;

            e.DrawBackground();

            Brush br;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                br = SystemBrushes.HighlightText;
            else
                br = new SolidBrush(e.ForeColor);

            float x = e.Bounds.Left;
            float y = e.Bounds.Top;
            float width = e.Bounds.Width;
            float height = e.Bounds.Height;
            RectangleF layout_rect;

            int urgIndex = int.Parse((string)((DataRowView)lst.Items[e.Index])[0]);

            string txt = lst.GetItemText(lst.Items[e.Index]);

            //Draw Urgency img
            RectangleF source_rect = new RectangleF(0, 0, 32, 32);
            RectangleF dest_rect = new RectangleF((e.Bounds.Right - 26), e.Bounds.Top + 1, 24, 24);
            e.Graphics.DrawImage(UrgencyImg[urgIndex], dest_rect, source_rect, GraphicsUnit.Pixel);

            //Draw text
            // Find the area in which to put the text.
            float fntsize = 8.25f;
            y = e.Bounds.Top + (e.Bounds.Height - 4 - fntsize) / 2;
            layout_rect = new RectangleF(x, y, width, height);
            //format.Alignment = StringAlignment.Far;

            Font textfnt = new Font("Microsoft Sans Serif", fntsize);

            e.Graphics.DrawString(txt, textfnt, br, layout_rect);

            // Draw the focus rectangle if the mouse hovers over an item.
            e.DrawFocusRectangle();
        }
        
        //Trailer definition
        public void FillcomboBoxTrailerDefList()
        {
            DataTable combDT = new DataTable();
            DataColumn dc = new DataColumn("Definition", typeof(string));
            combDT.Columns.Add(dc);

            dc = new DataColumn("DefinitionName", typeof(string));
            combDT.Columns.Add(dc);

            dc = new DataColumn("CargoType", typeof(int));
            combDT.Columns.Add(dc);

            dc = new DataColumn("UnitsCount", typeof(int));
            combDT.Columns.Add(dc);

            Cargo TempCargo = CargoesList.Find(x => x.CargoName == comboBoxFreightMarketCargoList.SelectedValue.ToString().Split(new char[] { ',' })[0]);

            foreach (TrailerDefinition tempitem in TempCargo.TrailerDefList)
            {
                string value = null;

                CargoLngDict.TryGetValue(tempitem.DefName, out value);

                if (value != null && value != "")
                {
                    combDT.Rows.Add(tempitem.DefName, value + " (" + tempitem.UnitsCount + "u)", tempitem.CargoType, tempitem.UnitsCount);
                }
                else
                {
                    string CapName = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(tempitem.DefName);
                    string[] CapNameArray = CapName.Split(new char[] { '.' });

                    CapName = "";
                    for (int i = 1; i < CapNameArray.Length; i++)
                    {
                        CapName += CapNameArray[i] + " ";
                    }


                    combDT.Rows.Add(tempitem.DefName, CapName + "(" + tempitem.UnitsCount + "u)", tempitem.CargoType, tempitem.UnitsCount);
                }
            }

            combDT.DefaultView.Sort = "DefinitionName ASC";

            comboBoxFreightMarketTrailerDef.ValueMember = "Definition";
            comboBoxFreightMarketTrailerDef.DisplayMember = "DefinitionName";
            comboBoxFreightMarketTrailerDef.DataSource = combDT;

            comboBoxFreightMarketTrailerDef.SelectedIndex = RandomValue.Next(comboBoxFreightMarketTrailerDef.Items.Count);
        }

        private void comboBoxFreightMarketTrailerDef_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox temp = sender as ComboBox;
            if (temp.SelectedIndex > -1)
                FillcomboBoxTrailerVariantList();
        }

        private void comboBoxFreightMarketTrailerDef_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = 26;
        }

        private void comboBoxFreightMarketTrailerDef_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
                return;

            ComboBox lst = sender as ComboBox;

            // Draw the background of the item.
            e.DrawBackground();

            // See if the item is selected.
            Brush br;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                br = SystemBrushes.HighlightText;
            else
                br = new SolidBrush(e.ForeColor);

            float x = e.Bounds.Left;
            float y = e.Bounds.Top;
            float height = e.Bounds.Height;
            RectangleF layout_rect;

            string CargoName = ((DataRowView)comboBoxFreightMarketCargoList.SelectedItem)[0].ToString(),
                DefDN = lst.GetItemText(lst.Items[e.Index]), CargoType = ((DataRowView)lst.Items[e.Index])[2].ToString();

            string txt = DefDN;

            int DefUnitsCount = int.Parse(((DataRowView)lst.Items[e.Index])[3].ToString());

            //////
            Image[] TypeImgs = new Image[5];
            int indexTypeImgs = 0;
            bool extheavy = false;

            try
            {
                ExtCargo tmp = ExtCargoList.Find(xx => xx.CargoName == CargoName);

                if (tmp != null)
                {
                    decimal TCW = tmp.Mass * DefUnitsCount;

                    if (TCW > 26000)
                        extheavy = true;
                }
            }
            catch
            { }

            if (extheavy || CargoType == "1")
            {
                TypeImgs[indexTypeImgs] = CargoTypeImg[1];
                indexTypeImgs++;
            }

            if (CargoType == "2")
            {
                TypeImgs[indexTypeImgs] = CargoTypeImg[2];
                indexTypeImgs++;
            }

            int xmult = 0, images = 0;

            foreach (Image temp in TypeImgs)
            {
                if (temp == null)
                {
                    break;
                }
                images++;
            }

            int rightOffset = 24;

            float width = comboBoxFreightMarketTrailerDef.Width - 26 * images - rightOffset;

            //e.Graphics.DrawRectangle(new Pen(Color.Red), e.Bounds.X, e.Bounds.Y, width, e.Bounds.Height);

            // Draw Type picture
            for (int i = 0; i < images; i++)
            {
                if (TypeImgs[i] == null)
                {
                    break;
                }

                RectangleF source_rect = new RectangleF(0, 0, 32, 32);
                RectangleF dest_rect = new RectangleF((e.Bounds.Left + width) + 24 * xmult, e.Bounds.Top + 1, 24, 24);
                e.Graphics.DrawImage(TypeImgs[i], dest_rect, source_rect, GraphicsUnit.Pixel);

                xmult++;
            }
            /////

            // Find the area in which to put the text.

            float fntsize = 8.25f;
            Font textfnt = new Font("Microsoft Sans Serif", fntsize);

            Size size = TextRenderer.MeasureText(txt, textfnt);
            if (size.Width >= width - 21)
            {
                fntsize = 7f;
                textfnt = new Font("Microsoft Sans Serif", fntsize);
                y = e.Bounds.Top - 5 + (e.Bounds.Height - 4 - fntsize) / 2;
            }
            else
            {
                y = e.Bounds.Top + (e.Bounds.Height - 4 - fntsize) / 2;
            }

            layout_rect = new RectangleF(x, y, width, height);
            StringFormat format = new StringFormat();

            //format.Alignment = StringAlignment.Far;

            e.Graphics.DrawString(txt, textfnt, br, layout_rect);

            // Draw the focus rectangle if the mouse hovers over an item.
            e.DrawFocusRectangle();
        }
        
        //Trailer variant
        public void FillcomboBoxTrailerVariantList()
        {
            DataTable combDT = new DataTable();
            DataColumn dc = new DataColumn("Variant", typeof(string));
            combDT.Columns.Add(dc);

            dc = new DataColumn("VariantName", typeof(string));
            combDT.Columns.Add(dc);

            List<string> TrailerVariants = TrailerDefinitionVariants[comboBoxFreightMarketTrailerDef.SelectedValue.ToString()];

            foreach (string tempitem in TrailerVariants)
            {
                string value = null;

                //CargoLngDict.TryGetValue(tempitem, out value);

                if (value != null && value != "")
                {
                    combDT.Rows.Add(tempitem, value);
                }
                else
                {
                    string CapName = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(tempitem);
                    combDT.Rows.Add(tempitem, CapName);
                }
            }

            combDT.DefaultView.Sort = "VariantName ASC";

            comboBoxFreightMarketTrailerVariant.ValueMember = "Variant";
            comboBoxFreightMarketTrailerVariant.DisplayMember = "VariantName";
            comboBoxFreightMarketTrailerVariant.DataSource = combDT;

            comboBoxFreightMarketTrailerVariant.SelectedIndex = RandomValue.Next(comboBoxFreightMarketTrailerVariant.Items.Count);
        }

        private void comboBoxFreightMarketTrailerVariant_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = 26;
        }

        private void comboBoxFreightMarketTrailerVariant_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
                return;

            ComboBox lst = sender as ComboBox;

            // Draw the background of the item.
            e.DrawBackground();

            // See if the item is selected.
            Brush br;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                br = SystemBrushes.HighlightText;
            else
                br = new SolidBrush(e.ForeColor);

            float x = e.Bounds.Left;
            float y = e.Bounds.Top;
            float width = e.Bounds.Width - 8;
            float height = e.Bounds.Height;
            RectangleF layout_rect;
            string DefDN = lst.GetItemText(lst.Items[e.Index]);
            string txt = DefDN;

            // Find the area in which to put the text.
            float fntsize = 8.25f;
            Font textfnt = new Font("Microsoft Sans Serif", fntsize);

            Size size = TextRenderer.MeasureText(txt, textfnt);
            if (size.Width > width)
            {
                fntsize = 7f;
                textfnt = new Font("Microsoft Sans Serif", fntsize);
                y = e.Bounds.Top - 5 + (e.Bounds.Height - 4 - fntsize) / 2;
            }
            else
            {
                y = e.Bounds.Top + (e.Bounds.Height - 4 - fntsize) / 2;
            }

            layout_rect = new RectangleF(x, y, width, height);
            //format.Alignment = StringAlignment.Far;
            e.Graphics.DrawString(txt, textfnt, br, layout_rect);

            // Draw the focus rectangle if the mouse hovers over an item.
            e.DrawFocusRectangle();
        }
        
        //Buttons
        private void buttonAddJob_Click(object sender, EventArgs e)
        {
            AddCargo(false);

            buttonFreightMarketClearJobList.Enabled = true;
        }

        private void buttonEditJob_Click(object sender, EventArgs e)
        {
            AddCargo(true);

            buttonFreightMarketCancelJobEdit.Visible = false;
            buttonFreightMarketCancelJobEdit.Enabled = false;
            buttonFreightMarketAddJob.Width = 394;

            buttonFreightMarketAddJob.Text = "Add Job to list";
            buttonFreightMarketAddJob.Click -= buttonEditJob_Click;
            buttonFreightMarketAddJob.Click += buttonAddJob_Click;
        }

        private void buttonFreightMarketCancelJobEdit_Click(object sender, EventArgs e)
        {
            listBoxFreightMarketAddedJobs.Enabled = true;
            buttonFreightMarketCancelJobEdit.Visible = false;
            buttonFreightMarketCancelJobEdit.Enabled = false;
            buttonFreightMarketAddJob.Width = 394;

            buttonFreightMarketAddJob.Text = "Add Job to list";
            buttonFreightMarketAddJob.Click -= buttonEditJob_Click;
            buttonFreightMarketAddJob.Click += buttonAddJob_Click;

            comboBoxFreightMarketSourceCity.SelectedValue = ((JobAdded)listBoxFreightMarketAddedJobs.Items[listBoxFreightMarketAddedJobs.Items.Count - 1]).DestinationCity;
            comboBoxFreightMarketSourceCompany.SelectedValue = ((JobAdded)listBoxFreightMarketAddedJobs.Items[listBoxFreightMarketAddedJobs.Items.Count - 1]).DestinationCompany;
        }

        private void buttonClearJobList_Click(object sender, EventArgs e)
        {
            ClearJobData();
        }

        private void ClearJobData()
        {
            unCertainRouteLength = "";
            JobsAmountAdded = 0;

            AddedJobsDictionary.Clear();
            Array.Resize(ref EconomyEventUnitLinkStringList, 0);

            listBoxFreightMarketAddedJobs.Items.Clear();
            labelFreightMarketDistanceNumbers.Text = " - ";// + ProgSettingsV.DistanceMes;
            buttonFreightMarketClearJobList.Enabled = false;
        }

        private void checkBoxRandomDest_CheckedChanged(object sender, EventArgs e)
        {
            ProgSettingsV.ProposeRandom = checkBoxFreightMarketRandomDest.Checked;
        }

        private void buttonFreightMarketRandomizeCargo_Click(object sender, EventArgs e)
        {
            comboBoxFreightMarketCargoList.SelectedIndex = RandomValue.Next(comboBoxFreightMarketCargoList.Items.Count);
        }

        //contextMenuStrip Freight Market JobList
        private void listBoxFreightMarketAddedJobs_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (listBoxFreightMarketAddedJobs.Items.Count != 0)
                {
                    Rectangle rect = listBoxFreightMarketAddedJobs.GetItemRectangle(listBoxFreightMarketAddedJobs.Items.Count - 1);

                    if (e.Y < rect.Bottom)
                    {
                        contextMenuStripFreightMarketJobList.Show(listBoxFreightMarketAddedJobs, e.Location);
                        int index = listBoxFreightMarketAddedJobs.IndexFromPoint(e.Location);
                        listBoxFreightMarketAddedJobs.SelectedIndex = index;
                    }
                }
            }

            if (e.Button == MouseButtons.Left)
            {
                if (listBoxFreightMarketAddedJobs.Items.Count != 0)
                {
                    Rectangle rect = listBoxFreightMarketAddedJobs.GetItemRectangle(listBoxFreightMarketAddedJobs.Items.Count - 1);

                    if (e.Y > rect.Bottom)
                    {

                    }
                }
            }
        }

        private void contextMenuStripFreightMarketJobListEdit_Click(object sender, EventArgs e)
        {
            FM_JobList_Edit();
        }

        private void contextMenuStripFreightMarketJobListDelete_Click(object sender, EventArgs e)
        {
            FM_JobList_Delete();
        }

        private void FM_JobList_Delete()
        {
            string companyNameJob = "company : company.volatile." + ((JobAdded)listBoxFreightMarketAddedJobs.SelectedItem).SourceCompany + "." + ((JobAdded)listBoxFreightMarketAddedJobs.SelectedItem).SourceCity + " {";

            AddedJobsDictionary[companyNameJob].Remove((JobAdded)listBoxFreightMarketAddedJobs.SelectedItem);
            if (AddedJobsDictionary[companyNameJob].Count == 0)
                AddedJobsDictionary.Remove(companyNameJob);

            AddedJobsList.Remove((JobAdded)listBoxFreightMarketAddedJobs.SelectedItem);

            listBoxFreightMarketAddedJobs.Items.Remove(listBoxFreightMarketAddedJobs.SelectedItem);

            RefreshFreightMarketDistance();
        }

        private void FM_JobList_Edit()
        {
            listBoxFreightMarketAddedJobs.Enabled = false;
            buttonFreightMarketAddJob.Width = 238;
            buttonFreightMarketCancelJobEdit.Visible = true;
            buttonFreightMarketCancelJobEdit.Enabled = true;

            comboBoxFreightMarketCountries.SelectedValue = "+all";
            comboBoxFreightMarketCompanies.SelectedValue = "+all";

            FreightMarketJob = (JobAdded)listBoxFreightMarketAddedJobs.SelectedItem;

            comboBoxFreightMarketSourceCity.SelectedValue = FreightMarketJob.SourceCity;
            comboBoxFreightMarketSourceCompany.SelectedValue = FreightMarketJob.SourceCompany;
            comboBoxFreightMarketDestinationCity.SelectedValue = FreightMarketJob.DestinationCity;
            comboBoxFreightMarketDestinationCompany.SelectedValue = FreightMarketJob.DestinationCompany;

            comboBoxFreightMarketCargoList.SelectedValue = FreightMarketJob.Cargo;
            comboBoxFreightMarketUrgency.SelectedValue = FreightMarketJob.Urgency;
            comboBoxFreightMarketTrailerDef.SelectedValue = FreightMarketJob.TrailerDefinition;
            comboBoxFreightMarketTrailerVariant.SelectedValue = FreightMarketJob.TrailerVariant;

            buttonFreightMarketAddJob.Text = "Edit Job";
            buttonFreightMarketAddJob.Click -= buttonAddJob_Click;
            buttonFreightMarketAddJob.Click += buttonEditJob_Click;
        }

        private void RefreshFreightMarketDistance()
        {
            if (listBoxFreightMarketAddedJobs.Items.Count > 0)
            {
                int JobsTotalDistance = 0;

                foreach (JobAdded tmpItem in listBoxFreightMarketAddedJobs.Items)
                {
                    JobsTotalDistance += tmpItem.Distance;
                }

                labelFreightMarketDistanceNumbers.Text = Math.Floor(JobsTotalDistance * DistanceMultiplier).ToString() + unCertainRouteLength + " " + ProgSettingsV.DistanceMes;
            }
            else
            {
                labelFreightMarketDistanceNumbers.Text = " - ";
                buttonFreightMarketClearJobList.Enabled = false;
            }
        }

        //end Freight market tab
    }
}