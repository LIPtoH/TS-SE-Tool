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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace TS_SE_Tool
{
    public class Garages
    {
        public string GarageName { get; set; } = "";
        public string GarageNameTranslated { get; set; } = "";

        public int GarageStatus { get; set; } = 0;

        public List<string> Vehicles { get; set; } = new List<string>();
        public List<string> Drivers { get; set; } = new List<string>();
        public List<string> Trailers { get; set; } = new List<string>();

        public bool IgnoreStatus { get; set; } = true;

        private FormMain MainForm = Application.OpenForms.OfType<FormMain>().Single();

        public Garages(string _GarageName)
        {
            GarageName = _GarageName;
        }

        public Garages(string _GarageName, int _GarageStatus)
        {
            GarageName = _GarageName;
            GarageStatus = _GarageStatus;
        }

        public string GetStatusString()
        {
            string status = "", statusStr = "";

            switch (GarageStatus)
            {
                case 0:
                    {
                        statusStr = "Not owned";
                        break;
                    }
                case 2:
                    {
                        statusStr = "Small";
                        break;
                    }
                case 3:
                    {
                        statusStr = "Large";
                        break;
                    }
                case 6:
                    {
                        statusStr = "Tiny";
                        break;
                    }
            }

            status = MainForm.ResourceManagerMain.GetPlainString(statusStr, Thread.CurrentThread.CurrentUICulture);

            if (string.IsNullOrEmpty(status))
                status = statusStr;     

            return status;
        }


        public override string ToString()
        {
            return GarageName + " | D:" + Drivers.Count + "| V:" + Vehicles.Count;
        }
    }
}
