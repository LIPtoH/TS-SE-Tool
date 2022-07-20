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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TS_SE_Tool
{
    internal class Driver
    {
        FormMain MainForm = Application.OpenForms.OfType<FormMain>().Single();
        internal string driverNameless { get; set; } = "";

        internal bool isUser { get; set; } = false;

        internal bool isStaff { get; set; } = false;

        internal byte adr { get; set; } = 0;
        internal byte long_dist { get; set; } = 0;
        internal byte heavy { get; set; } = 0;
        internal byte fragile { get; set; } = 0;
        internal byte urgent { get; set; } = 0;
        internal byte mechanical { get; set; } = 0;

        internal string driverNameTranslated
        {
            get
            {
                if (this.isUser)
                    return Utilities.TextUtilities.FromHexToString(Globals.SelectedProfile);
                else
                if (MainForm.DriverNames.ContainsKey(this.driverNameless))
                    return MainForm.DriverNames[this.driverNameless].TrimStart(new char[] { '+' });
                else
                    return this.driverNameless;
            }

            set
            { }
        }

        internal byte[] playerSkills
        {
            get
            {
                return new byte[] { adr, long_dist, heavy, fragile, urgent, mechanical };
            }
            set 
            {
                this.adr = value[0];
                this.long_dist = value[1];
                this.heavy = value[2];
                this.fragile = value[3];
                this.urgent = value[4];
                this.mechanical = value[5];
            }
        }

        internal bool getADRbit(int b)
        {
            return (this.adr & (1 << b)) != 0;
        }

        internal void setADRbit(int b, bool state)
        {
            if (state)
                this.adr = (byte)(this.adr | (1 << b));
            else
                this.adr = (byte)(this.adr ^ (1 << b));
        }

        public Driver()
        { }

        public Driver(string _driverNameless)
        {
            driverNameless = _driverNameless;
        }
        public Driver(string _driverNameless, bool _isUser)
        {
            driverNameless = _driverNameless;
            isUser = _isUser;
        }

        public Driver(string _driverNameless, bool _isUser, bool _isStaff)
        {
            driverNameless = _driverNameless;
            isUser = _isUser;
            isStaff = _isStaff;
        }
    }
}
