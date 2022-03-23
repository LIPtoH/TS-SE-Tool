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
using System.Collections.Generic;
using System.Linq;

namespace TS_SE_Tool
{
    class Routes
    {
        private List<string[]> RoutesList = new List<string[]>();
        private string[] SingleRoute;

        public void AddRoute(string _starting_city, string _starting_company, string _destination_city, string _destination_company, string _distance, string _ferry_time, string _ferry_price)
        {
            SingleRoute = new string[] { _starting_city, _starting_company, _destination_city, _destination_company, _distance, _ferry_time, _ferry_price};

            RoutesList.Add(SingleRoute);
        }

        public string[] GetRouteData(string _starting_city, string _starting_company, string _destination_city, string _destination_company)
        {
            string[] CompanyToCompanyTrueRoute = RoutesList.Find(x => x[0] == _starting_city && x[1] == _starting_company && x[2] == _destination_city && x[3] == _destination_company);

            if (CompanyToCompanyTrueRoute != null)
            {
                return CompanyToCompanyTrueRoute;
            }
            else
            {
                // Same City Company to Company
                if (_starting_city == _destination_city)
                    return new string[] { _starting_city, _starting_company, _destination_city, _destination_company, "1", "0", "0" };

                // Searching variants

                // City to City both directions
                List<string[]> CityToCityRouteBiDirectional = RoutesList.FindAll(x => x[0] == _starting_city && x[2] == _destination_city || x[0] == _destination_city && x[2] == _starting_city);

                if (CityToCityRouteBiDirectional.Count > 0)
                {
                    CityToCityRouteBiDirectional = CityToCityRouteBiDirectional.OrderBy(x => x[4]).ToList();

                    string[] CityToCityRouteBiDirectionalShortest = CityToCityRouteBiDirectional.First();

                    return CityToCityRouteBiDirectionalShortest;                    
                }
                else
                {
                    // Search for transit routes
                    List<string[]> FiltereRoutes = new List<string[]>();

                    //---
                    List<string[]> CityToCityRouteSingleEnded1 = RoutesList.FindAll(x => x[0] == _starting_city);

                    List<string> CityList1 = GetList(CityToCityRouteSingleEnded1, 2);
                    CityList1.Sort();
                    //---
                    List<string[]> CityToCityRouteSingleEnded2 = RoutesList.FindAll(x => x[2] == _destination_city);

                    List<string> CityList2 = GetList(CityToCityRouteSingleEnded2, 0);
                    CityList2.Sort();
                    //---
                    List<string[]> CityToCityRouteSingleEnded3 = RoutesList.FindAll(x => x[0] == _destination_city);

                    List<string> CityList3 = GetList(CityToCityRouteSingleEnded3, 2);
                    CityList3.Sort();
                    //---
                    List<string[]> CityToCityRouteSingleEnded4 = RoutesList.FindAll(x => x[2] == _starting_city);

                    List<string> CityList4 = GetList(CityToCityRouteSingleEnded4, 0);
                    CityList4.Sort();
                    //---

                    // Get cities
                    var startToEndTransitCities = CityList1.Intersect(CityList2).ToList();
                    var endToStartTransitCities = CityList3.Intersect(CityList4).ToList();

                    List<string[]> SearchResults = new List<string[]>();

                    // Get routes
                    if (startToEndTransitCities.Count > 0)
                    {
                        foreach (string entry in startToEndTransitCities)
                        {
                            var entryRoute = GetRouteWithTransit(_starting_city, _destination_city, entry);
                            if (entryRoute != null)
                                SearchResults.Add(entryRoute);
                        }
                    }

                    if (endToStartTransitCities.Count > 0)
                    {
                        foreach (string entry in endToStartTransitCities)
                        {
                            var entryRoute = GetRouteWithTransit(_destination_city, _starting_city, entry);
                            if (entryRoute != null)
                                SearchResults.Add(entryRoute);
                        }
                    }

                    // Filter routes
                    SearchResults = SearchResults.OrderBy(x => x[4]).ToList();

                    if (SearchResults.Count > 0)
                        return SearchResults.First();
                    else
                        return new string[] { _starting_city, _starting_company, _destination_city, _destination_company, "11111", "0", "0" };

                    //===
                    List<string> GetList (List<string[]> _input, byte idx)
                    {
                        List<string> Data = new List<string>();

                        foreach (string[] entry in _input)
                        {
                            Data.Add(entry[idx]);
                        }

                        Data = Data.Distinct().ToList();

                        return Data;
                    }

                    //===
                    string[] GetRouteWithTransit(string _start, string _end, string _transit)
                    {
                        List<string[]> startToTransit = RoutesList.FindAll(x => x[0] == _start && x[2] == _transit);

                        if (startToTransit.Count > 0)
                        {
                            List<string[]> transitToEnd = RoutesList.FindAll(x => x[0] == _transit && x[2] == _end);

                            string[] shortestSTT = startToTransit.OrderBy(x => x[4]).ToList().First();
                            string[] shartestTTE = transitToEnd.OrderBy(x => x[4]).ToList().First();

                            return new string[] { _starting_city, _starting_company, _destination_city, _destination_company,
                                                (int.Parse(shortestSTT[4]) + int.Parse(shartestTTE[4])).ToString(),
                                                (int.Parse(shortestSTT[5]) + int.Parse(shartestTTE[5])).ToString(),
                                                (int.Parse(shortestSTT[6]) + int.Parse(shartestTTE[6])).ToString()
                                                };
                        }
                        else
                        {
                            return null;
                        }
                    }
                }    
            }
        }

        public List<string[]> GetRoutes()
        {
            return RoutesList;
        }

        public int CountItems()
        {
            return RoutesList.Count();
        }

        public void ClearList()
        {
            RoutesList.Clear();
        }
    }
}
