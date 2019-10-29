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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace TS_SE_Tool
{
    public class Garages
    {
        private FormMain MainForm = Application.OpenForms.OfType<FormMain>().Single();

        public Garages(string _GarageName, int _GarageStatus)
        {
            GarageName = _GarageName;
            GarageStatus = _GarageStatus;
        }

        public Garages(string _GarageName)
        {
            GarageName = _GarageName;
        }

        public string GarageName { get; set; }

        public string GarageNameTranslated { get; set; }

        public int GarageStatus { get; set; } = 0;

        public List<string> Vehicles { get; set; } = new List<string>();

        public List<string> Drivers { get; set; } = new List<string>();

        public List<string> Trailers { get; set; } = new List<string>();

        public bool IgnoreStatus { get; set; } = true;

        /*
        public override string ToString() {
            string output = "", status = "", statusStr = "";

            if (GarageStatus == 0)
            {
                statusStr = "Not owned";
                status = MainForm.ResourceManagerMain.GetString(statusStr, Thread.CurrentThread.CurrentUICulture);
            }
            else if (GarageStatus == 2)
            {
                statusStr = "Small";
                status = MainForm.ResourceManagerMain.GetString(statusStr, Thread.CurrentThread.CurrentUICulture);
            }
            else if (GarageStatus == 3)
            {
                statusStr = "Large";
                status = MainForm.ResourceManagerMain.GetString(statusStr, Thread.CurrentThread.CurrentUICulture);
            }
            else if (GarageStatus == 6)
            {
                statusStr = "Tiny";
                status = MainForm.ResourceManagerMain.GetString(statusStr, Thread.CurrentThread.CurrentUICulture);
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
                trueGarageName = GarageName + " -n";
            }

            output = trueGarageName + "\n" + trueStatus;

            return output;
        }
        */

        public string GetStatusString()
        {
            string output = "", status = "", statusStr = "";

            if (GarageStatus == 0)
            {
                statusStr = "Not owned";
            }
            else if (GarageStatus == 2)
            {
                statusStr = "Small";
            }
            else if (GarageStatus == 3)
            {
                statusStr = "Large";
            }
            else if (GarageStatus == 6)
            {
                statusStr = "Tiny";
            }

            status = MainForm.ResourceManagerMain.GetString(statusStr, Thread.CurrentThread.CurrentUICulture);

            if (status != null && status != "")
                output = status;
            else
            {
                output = statusStr;
            }

            return output;
        }

    }
}
