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
using System.Collections.Generic;
using System.Linq;
using System;
using System.Windows.Forms;

namespace TS_SE_Tool
{
    class City
    {
        private FormMain MainForm = Application.OpenForms.OfType<FormMain>().Single();

        public string Country { get; set; }
        public string CityName;
        public string CityNameTranslated;
        public List<Company> Companies = new List<Company>();
        public bool Disabled = false;
        public bool Visited = false;

        public City(string _cityName, string _countryName)
        {
            CityName = _cityName;
            Country = _countryName;
        }

        public void AddCompany(string _companyName, int _jobsoffer)
        {
            Companies.Add(new Company(_companyName, _jobsoffer));
        }

        public void UpdateCompany(string _companyName, int _jobsoffer)
        {
            foreach (Company company in Companies)
            {
                if (company.CompanyName == _companyName)
                {
                    company.JobsOffers = _jobsoffer;
                }
            }
        }

        public void UpdateCompanyCargoOfferCount(string _companyName, int _cargooffers)
        {
            Array.Resize(ref Companies.Find(x => x.CompanyName == _companyName).CragoSeeds, _cargooffers);
        }

        public void ExcludeCompany()
        {
            foreach (Company company in Companies)
            {
                if (company.JobsOffers == 0)
                {
                    company.Excluded = true;
                }

                MainForm.CompaniesLngDict.TryGetValue(company.CompanyName, out string value);

                if (value != "" && value != null)
                {
                    company.CompanyNameTranslated = value;
                }
                else
                {
                    company.CompanyNameTranslated = company.CompanyName + " -nt";
                }
            }

            int num = (from x in Companies where x.Excluded select x).Count();

            if (num >= Companies.Count)
            {
                Disabled = true;
            }
        }

        public List<Company> ReturnCompanies()
        {
            return Companies;
        }
    }
}
