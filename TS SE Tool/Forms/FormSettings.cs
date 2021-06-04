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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using System.Threading;

namespace TS_SE_Tool
{
    public partial class FormSettings : Form
    {
        FormMain MainForm = Application.OpenForms.OfType<FormMain>().Single();

        public FormSettings()
        {
            InitializeComponent();

            this.Icon = Utilities.TS_Graphics.IconFromImage(MainForm.ProgUIImgsDict["Settings"]);            

            this.SuspendLayout();

            try
            {
                string translatedString = MainForm.ResourceManagerMain.GetString(this.Name, Thread.CurrentThread.CurrentUICulture);
                if (translatedString != null)
                    this.Text = translatedString;
            }
            catch
            { }

            MainForm.HelpTranslateFormMethod(this, toolTip);

            CorrectControlsPositions();
            this.ResumeLayout();

            MainForm.LoadConfig();
            PopulateControls();
        }

        private void CorrectControlsPositions()
        {
            //Longest setting string
            int longeststr = 0, margin = 6;

            Control[] labellist = new Control[] { labelJobPickupTime, labelLoopEvery, labelDistance, labelCurrency, labelCurrency };

            longeststr = CorrectControlsPositionsLoongest(labellist);

            Control[][] Controllist = new Control[5][];
            Controllist[0] = new Control[] { numericUpDownSettingPickTimeD, labelDayShort, numericUpDownSettingPickTimeH, labelHourShort };
            Controllist[1] = new Control[] { numericUpDownSettingLoopCitys, labelCity };
            Controllist[2] = new Control[] { comboBoxSettingDistanceMesSelect };
            Controllist[3] = new Control[] { labelCurrencyETS2 };
            Controllist[4] = new Control[] { labelCurrencyATS };

            CorrectControlsPositionsMover(Controllist, longeststr, margin);

            //
            labellist = new Control[] { labelCurrencyETS2, labelCurrencyATS };
            longeststr = CorrectControlsPositionsLoongest(labellist);

            Controllist = new Control[2][];
            Controllist[0] = new Control[] { comboBoxSettingCurrencySelectETS2 };
            Controllist[1] = new Control[] { comboBoxSettingCurrencySelectATS };

            CorrectControlsPositionsMover(Controllist, longeststr, margin);
        }

        private int CorrectControlsPositionsLoongest(Control[] _inputList)
        {
            int longeststr = 0;

            foreach (Control c in _inputList)
            {
                Label temp = c as Label;
                if (c.Width > longeststr)
                    longeststr = c.Width + c.Location.X;
            }

            return longeststr;
        }

        private void CorrectControlsPositionsMover(Control[][] _Controllist, int _longeststr, int _margin)
        {
            foreach (Control[] cc in _Controllist)
            {
                int margincount = 2, startX = _longeststr;

                foreach (Control c in cc)
                {
                    c.Location = new Point(startX + _margin * margincount, c.Location.Y);
                    startX += c.Width;
                    margincount++;
                }
            }
        }

        private void PopulateControls()
        {
            bool _sgw = false;

            //Distances choice
            DataTable combDT = new DataTable();
            combDT.Columns.Add("ID");
            combDT.Columns.Add("DistDisplayName");

            Dictionary<string, string> DistanceMesNames = new Dictionary<string, string> { { "km", "Kilometers" }, { "mi", "Miles" } };

            foreach (KeyValuePair<string, string> tempitem in DistanceMesNames)
            {
                string value = MainForm.ResourceManagerMain.GetString(tempitem.Value, Thread.CurrentThread.CurrentUICulture);

                if (value != null && value != "")
                {
                    combDT.Rows.Add(tempitem.Key, value);
                }
                else
                {
                    combDT.Rows.Add(tempitem.Key, tempitem.Value);
                }
            }

            comboBoxSettingDistanceMesSelect.ValueMember = "ID";
            comboBoxSettingDistanceMesSelect.DisplayMember = "DistDisplayName";
            comboBoxSettingDistanceMesSelect.DataSource = combDT;
            comboBoxSettingDistanceMesSelect.SelectedValue = MainForm.ProgSettingsV.DistanceMes;

            if (comboBoxSettingDistanceMesSelect.SelectedValue == null)
            {
                _sgw = true;
                comboBoxSettingDistanceMesSelect.SelectedIndex = 0;
            }


            //Currency choise
            //ETS2
            combDT = new DataTable();
            combDT.Columns.Add("ID");
            combDT.Columns.Add("CurrencyDisplayName");

            foreach (KeyValuePair<string, double> tempitem in MainForm.CurrencyDictConversionETS2)
            {
                string value = MainForm.ResourceManagerMain.GetString(tempitem.Key, Thread.CurrentThread.CurrentUICulture);

                if (value != null && value != "")
                {
                    combDT.Rows.Add(tempitem.Key, value);
                }
                else
                {
                    combDT.Rows.Add(tempitem.Key, tempitem.Key);
                }
            }

            comboBoxSettingCurrencySelectETS2.ValueMember = "ID";
            comboBoxSettingCurrencySelectETS2.DisplayMember = "CurrencyDisplayName";
            comboBoxSettingCurrencySelectETS2.DataSource = combDT;
            comboBoxSettingCurrencySelectETS2.SelectedValue = MainForm.ProgSettingsV.CurrencyMesETS2;

            if (comboBoxSettingCurrencySelectETS2.SelectedValue == null)
            {
                _sgw = true;
                comboBoxSettingCurrencySelectETS2.SelectedIndex = 0;
                MainForm.ProgSettingsV.CurrencyMesETS2 = comboBoxSettingCurrencySelectETS2.SelectedValue.ToString();
            }
            //ATS
            combDT = new DataTable();
            combDT.Columns.Add("ID");
            combDT.Columns.Add("CurrencyDisplayName");

            foreach (KeyValuePair<string, double> tempitem in MainForm.CurrencyDictConversionATS)
            {
                string value = MainForm.ResourceManagerMain.GetString(tempitem.Key, Thread.CurrentThread.CurrentUICulture);

                if (value != null && value != "")
                {
                    combDT.Rows.Add(tempitem.Key, value);
                }
                else
                {
                    combDT.Rows.Add(tempitem.Key, tempitem.Key);
                }
            }

            comboBoxSettingCurrencySelectATS.ValueMember = "ID";
            comboBoxSettingCurrencySelectATS.DisplayMember = "CurrencyDisplayName";
            comboBoxSettingCurrencySelectATS.DataSource = combDT;
            comboBoxSettingCurrencySelectATS.SelectedValue = MainForm.ProgSettingsV.CurrencyMesATS;

            if (comboBoxSettingCurrencySelectATS.SelectedValue == null)
            {
                _sgw = true;
                comboBoxSettingCurrencySelectATS.SelectedIndex = 0;
                MainForm.ProgSettingsV.CurrencyMesATS = comboBoxSettingCurrencySelectATS.SelectedValue.ToString();
            }


            //Pickup time intervals
            numericUpDownSettingPickTimeD.Value = Math.Floor((decimal)(MainForm.ProgSettingsV.JobPickupTime / 24));
            numericUpDownSettingPickTimeH.Value = MainForm.ProgSettingsV.JobPickupTime - numericUpDownSettingPickTimeD.Value * 24;

            //Loop width
            numericUpDownSettingLoopCitys.Value = MainForm.ProgSettingsV.LoopEvery;

            if (_sgw)
                PrepareSettingsAndSave();
        }
        
        //Events
        //DB
        private void buttonSettingDBClear_Click(object sender, EventArgs e)
        {
            MainForm.ClearDatabase();
        }

        private void buttonSettingDBExport_Click(object sender, EventArgs e)
        {
            MainForm.ExportDB();
        }

        private void buttonSettingDBImport_Click(object sender, EventArgs e)
        {
            MainForm.ImportDB();
        }
        
        //Pickup window
        private void numericUpDownSettingPickTimeD_ValueChanged(object sender, EventArgs e)
        {
            numericUpDownSettingPickTimeH.Value = 0;
        }

        private void numericUpDownSettingPickTimeH_ValueChanged(object sender, EventArgs e)
        {

            if (numericUpDownSettingPickTimeH.Value == 24)
            {
                numericUpDownSettingPickTimeD.Value++;
                numericUpDownSettingPickTimeH.Value = 0;
            }
            else if (numericUpDownSettingPickTimeH.Value == -1)
            {
                if(numericUpDownSettingPickTimeD.Value > 0)
                {
                    numericUpDownSettingPickTimeD.Value--;
                    numericUpDownSettingPickTimeH.Value = 0;
                }
                else
                {
                    numericUpDownSettingPickTimeH.Value = 0;
                }
            }
        }

        //
        private void numericUpDownSettingLoopCitys_ValueChanged(object sender, EventArgs e)
        {
            MainForm.ProgSettingsV.LoopEvery = Convert.ToByte(numericUpDownSettingLoopCitys.Value);
        }

        //Buttons
        private void buttonSettingSave_Click(object sender, EventArgs e)
        {
            PrepareSettingsAndSave();
        }

        private void PrepareSettingsAndSave()
        {
            MainForm.DistanceMultiplier = MainForm.DistanceMultipliers[comboBoxSettingDistanceMesSelect.SelectedValue.ToString()];
            MainForm.ProgSettingsV.DistanceMes = comboBoxSettingDistanceMesSelect.SelectedValue.ToString();
            MainForm.ProgSettingsV.CurrencyMesETS2 = comboBoxSettingCurrencySelectETS2.SelectedValue.ToString();
            MainForm.ProgSettingsV.CurrencyMesATS = comboBoxSettingCurrencySelectATS.SelectedValue.ToString();
            MainForm.ProgSettingsV.JobPickupTime = (short)(numericUpDownSettingPickTimeH.Value + numericUpDownSettingPickTimeD.Value * 24);

            MainForm.WriteConfig();

            if (MainForm.GameType == "ETS2")
            {
                Globals.CurrencyName = MainForm.ProgSettingsV.CurrencyMesETS2;
            }
            else
            {
                Globals.CurrencyName = MainForm.ProgSettingsV.CurrencyMesATS;
            }

            MainForm.FillAccountMoneyTB();
        }

        private void buttonSettingCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
