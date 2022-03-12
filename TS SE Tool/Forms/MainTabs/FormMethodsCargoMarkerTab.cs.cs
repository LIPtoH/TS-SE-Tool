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
using System.Globalization;
using System.Linq;

namespace TS_SE_Tool
{
    public partial class FormMain
    {
        //Cargo Market tab
        private void FillFormCargoOffersControls()
        {
            FillCargoMarketCities();
            FillTrailerTypesCM();
        }

        private void FillCargoMarketCities()
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

            comboBoxCargoMarketSourceCity.ValueMember = "City";
            comboBoxCargoMarketSourceCity.DisplayMember = "CityName";
            comboBoxCargoMarketSourceCity.DataSource = combDT;

            DataRow foundRow = combDT.Rows.Find(new object[1] { SiiNunitData.Economy.last_visited_city });
            if (combDT.Rows.Find(new object[1] { SiiNunitData.Economy.last_visited_city }) != null)
                comboBoxCargoMarketSourceCity.SelectedValue = SiiNunitData.Economy.last_visited_city;
        }

        private void comboBoxSourceCityCM_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxCargoMarketSourceCity.SelectedIndex >= 0)
            {
                comboBoxSourceCargoMarketCompany.SelectedIndex = -1;
                comboBoxSourceCargoMarketCompany.Text = "";

                SetupSourceCompaniesCM();
            }

            if (comboBoxSourceCargoMarketCompany.Items.Count > 0)
            {
                comboBoxSourceCargoMarketCompany.SelectedIndex = RandomValue.Next(comboBoxSourceCargoMarketCompany.Items.Count);
            }
        }

        private void SetupSourceCompaniesCM()
        {
            List<Company> CityCompanies = CitiesList.Find(x => x.CityName == comboBoxCargoMarketSourceCity.SelectedValue.ToString()).ReturnCompanies();
            List<Company> RealCompanies = CityCompanies.FindAll(x => !x.Excluded);

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

            comboBoxSourceCargoMarketCompany.ValueMember = "Company";
            comboBoxSourceCargoMarketCompany.DisplayMember = "CompanyName";
            comboBoxSourceCargoMarketCompany.DataSource = combDT;
        }

        private void comboBoxSourceCompanyCM_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxSourceCargoMarketCompany.SelectedValue != null && ExternalCompanies.Count > 0)
            {
                PreparePossibleCargoes();
            }

            PrintCargoSeeds();
        }

        private void comboBoxCMTrailerTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            PreparePossibleCargoes();
        }

        private void PreparePossibleCargoes()
        {
            listBoxCargoMarketCargoListForCompany.Items.Clear();

            if (comboBoxSourceCargoMarketCompany.SelectedValue == null)
                return;

            ExtCompany t = ExternalCompanies.Find(x => x.CompanyName == comboBoxSourceCargoMarketCompany.SelectedValue.ToString());

            if (t != null)
            {
                List<string> oC = t.outCargo;

                if (oC != null)
                    foreach (string cargo in oC)
                    {
                        if (comboBoxCMTrailerTypes.SelectedValue != null && ExtCargoList.Count > 0)
                        {
                            ExtCargo temp = ExtCargoList.Find(x => x.CargoName == cargo);
                            if (temp != null)
                            {
                                if (temp.BodyTypes.Contains(comboBoxCMTrailerTypes.SelectedValue.ToString()))
                                    listBoxCargoMarketCargoListForCompany.Items.Add(cargo);
                            }
                        }
                    }
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            PrintCargoSeeds();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            PrintCargoSeeds();
        }

        private void PrintCargoSeeds()
        {
            listBoxCargoMarketSourceCargoSeeds.Items.Clear();

            if (comboBoxCargoMarketSourceCity.SelectedValue != null && comboBoxSourceCargoMarketCompany.SelectedValue != null) //&& ExternalCompanies.Count > 0)
            {
                foreach (int cargoseed in CitiesList.Find(x => x.CityName == comboBoxCargoMarketSourceCity.SelectedValue.ToString()).ReturnCompanies().Find(x => x.CompanyName == comboBoxSourceCargoMarketCompany.SelectedValue.ToString()).CargoSeeds)
                {
                    string cargoforseed = "";
                    listBoxCargoMarketSourceCargoSeeds.Items.Add("" + cargoseed.ToString().PadRight(12, ' ') + " | Time left " + ((cargoseed - SiiNunitData.Economy.game_time) / 60).ToString().PadLeft(2) + " h " +
                        ((cargoseed - SiiNunitData.Economy.game_time) % 60).ToString().PadLeft(2) + " m " + cargoforseed);
                }
            }
            else
            {

            }
        }

        private void buttonCargoMarketRandomizeCargoCompany_Click(object sender, EventArgs e)
        {
            List<Company> CityCompanies = CitiesList.Find(x => x.CityName == comboBoxCargoMarketSourceCity.SelectedValue.ToString()).ReturnCompanies();
            List<Company> RealCompanies = CityCompanies.FindAll(x => !x.Excluded);

            uint[] tempseeds = new uint[10];

            for (int i = 0; i < tempseeds.Length; i++)
            {
                tempseeds[i] = (uint)SiiNunitData.Economy.game_time + (uint)RandomValue.Next(180, 1800);
            }

            RealCompanies.Find(x => x.CompanyName == comboBoxSourceCargoMarketCompany.SelectedValue.ToString()).CargoSeeds = tempseeds;

            PrintCargoSeeds();
        }

        private void buttonCargoMarketResetCargoCompany_Click(object sender, EventArgs e)
        {
            List<Company> CityCompanies = CitiesList.Find(x => x.CityName == comboBoxCargoMarketSourceCity.SelectedValue.ToString()).ReturnCompanies();
            List<Company> RealCompanies = CityCompanies.FindAll(x => !x.Excluded);

            RealCompanies.Find(x => x.CompanyName == comboBoxSourceCargoMarketCompany.SelectedValue.ToString()).CargoSeeds = new uint[0];

            PrintCargoSeeds();
        }

        private void buttonCargoMarketRandomizeCargoCity_Click(object sender, EventArgs e)
        {
            List<Company> CityCompanies = CitiesList.Find(x => x.CityName == comboBoxCargoMarketSourceCity.SelectedValue.ToString()).ReturnCompanies();
            List<Company> RealCompanies = CityCompanies.FindAll(x => !x.Excluded);

            foreach (Company company in RealCompanies)
            {
                uint[] tempseeds = new uint[10];

                for (int i = 0; i < tempseeds.Length; i++)
                {
                    tempseeds[i] = (uint)SiiNunitData.Economy.game_time + (uint)RandomValue.Next(180, 1800);
                }

                company.CargoSeeds = tempseeds;
            }

            PrintCargoSeeds();
        }

        private void buttonCargoMarketResetCargoCity_Click(object sender, EventArgs e)
        {
            List<Company> CityCompanies = CitiesList.Find(x => x.CityName == comboBoxCargoMarketSourceCity.SelectedValue.ToString()).ReturnCompanies();
            List<Company> RealCompanies = CityCompanies.FindAll(x => !x.Excluded);

            foreach (Company company in RealCompanies)
            {
                company.CargoSeeds = new uint[0];
            }

            PrintCargoSeeds();
        }

        private void FillTrailerTypesCM()
        {
            DataTable combDT = new DataTable();
            DataColumn dc = new DataColumn("TrailerType", typeof(string));
            combDT.Columns.Add(dc);

            dc = new DataColumn("TrailerTypeName", typeof(string));
            combDT.Columns.Add(dc);

            List<string> TrailerTypes = new List<string>();

            foreach (ExtCargo temp in ExtCargoList)
            {
                foreach (string temptype in temp.BodyTypes)
                {
                    if (TrailerTypes.FindIndex(x => x == temptype) == -1)
                        TrailerTypes.Add(temptype);
                }
            }

            foreach (string trailertype in TrailerTypes)
            {
                combDT.Rows.Add(trailertype, CultureInfo.InvariantCulture.TextInfo.ToTitleCase(trailertype));
            }

            combDT.DefaultView.Sort = "TrailerTypeName ASC";

            comboBoxCMTrailerTypes.ValueMember = "TrailerType";
            comboBoxCMTrailerTypes.DisplayMember = "TrailerTypeName";

            comboBoxCMTrailerTypes.DataSource = combDT;
        }
        //end Cargo Market tab
    }
}