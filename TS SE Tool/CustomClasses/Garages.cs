/*
   Copyright 2016-2018 LIPtoH <liptoh.codebase@gmail.com>

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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS_SE_Tool
{
    class Garages
    {
        public Garages(string _GarageName, int _GarageStatus)
        {
            GarageName = _GarageName;
            GarageStatus = _GarageStatus;
        }

        public string GarageName { get; set; }

        public int GarageStatus { get; set; }

        public List<string> Vehicles { get; set; } = new List<string>();

        public List<string> Drivers { get; set; } = new List<string>();

        public List<string> Trailers { get; set; } = new List<string>();

        public bool IgnoreStatus { get; set; } = true;

        public override string ToString() {
            string output = "", status = "", statusStr = "";

            //FormMain.CustomStringsDict.TryGetValue(GarageName, out status);

            if (GarageStatus == 0)
            {
                statusStr = "Not owned";
                FormMain.CustomStringsDict.TryGetValue(statusStr, out status);
            }
            else if (GarageStatus == 2)
            {
                statusStr = "Small";
                FormMain.CustomStringsDict.TryGetValue(statusStr, out status);
            }
            else if (GarageStatus == 3)
            {
                statusStr = "Large";
                FormMain.CustomStringsDict.TryGetValue(statusStr, out status);
            }
            else if (GarageStatus == 6)
            {
                statusStr = "Tiny";
                FormMain.CustomStringsDict.TryGetValue(statusStr, out status);
            }

            string trueStatus = "";
            if (status != null && status != "")
                trueStatus = status;
            else
            {
                trueStatus = statusStr;
            }

            string trueGarageName = "";

            FormMain.CitiesLngDict.TryGetValue(GarageName, out string value);

            if (value != null && value != "")
                trueGarageName = value;
            else
            {
                trueGarageName = GarageName;
            }

            output = trueGarageName + "\n" + status;

            return output;
        }
    }
}
