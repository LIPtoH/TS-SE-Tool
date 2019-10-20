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
using System.IO;
using System.Collections.Generic;

namespace TS_SE_Tool
{
    class CountryDictionary
    {
        public List<string[]> CountryList = new List<string[]>();
        private bool NewCityAdded = false;

        public void AddCountry(string _cityName, string _countryName)
        {
            CountryList.Add(new string[] { _cityName, _countryName });
        }

        public string GetCountry(string _cityName)
        {
            try
            {
                foreach (string[] strArray in CountryList)
                {
                    if (strArray[0] == _cityName)
                    {
                        return strArray[1];
                    }
                }
                AddCountry(_cityName, "");

                FormMain.LogWriter("Added to cities dictionary: " + _cityName);

                NewCityAdded = true;
                return "";
            }
            catch
            {
                FormMain.LogWriter(_cityName);
                return "";
            }
        }

        public void SaveDictionaryFile()
        {
            if (NewCityAdded)
            {
                using (StreamWriter writer = new StreamWriter(Directory.GetCurrentDirectory() + @"\lang\CityToCountry.csv", false))
                {
                    foreach (string[] strArray in CountryList)
                    {
                        writer.WriteLine(strArray[0] + ";" + strArray[1]);
                    }
                }
            }
        }
    }
}
