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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using TS_SE_Tool.CustomClasses;
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
            this.Icon = Properties.Resources.MainIco;

            this.SuspendLayout();

            try
            {
                string translatedString = MainForm.ResourceManagerMain.GetString(this.Name, Thread.CurrentThread.CurrentUICulture);
                if (translatedString != null)
                    this.Text = translatedString;
            }
            catch
            {
            }

            MainForm.HelpTranslateFormMethod(this, MainForm.ResourceManagerMain, Thread.CurrentThread.CurrentUICulture);

            CorrectControlsPositions();
            this.ResumeLayout();

            PopulateControls();
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
            combDT = new DataTable();
            combDT.Columns.Add("ID");
            combDT.Columns.Add("CurrencyDisplayName");

            foreach (KeyValuePair<string, double> tempitem in MainForm.CurrencyDictR)
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

            comboBoxSettingCurrencySelect.ValueMember = "ID";
            comboBoxSettingCurrencySelect.DisplayMember = "CurrencyDisplayName";
            comboBoxSettingCurrencySelect.DataSource = combDT;
            comboBoxSettingCurrencySelect.SelectedValue = MainForm.ProgSettingsV.CurrencyMes;

            if (comboBoxSettingCurrencySelect.SelectedValue == null)
            {
                _sgw = true;
                comboBoxSettingCurrencySelect.SelectedIndex = 0;
                MainForm.ProgSettingsV.CurrencyMes = comboBoxSettingCurrencySelect.SelectedValue.ToString();
            }

            //Pickup time intervals
            numericUpDownSettingPickTimeD.Value = Math.Floor((decimal)(MainForm.ProgSettingsV.JobPickupTime / 24));
            numericUpDownSettingPickTimeH.Value = MainForm.ProgSettingsV.JobPickupTime - numericUpDownSettingPickTimeD.Value * 24;

            //Loop width
            numericUpDownSettingLoopCitys.Value = MainForm.ProgSettingsV.LoopEvery;

            if (_sgw)
                PrepareSettingsAndSave();
        }

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
        
        private void numericUpDownSettingPickTimeD_ValueChanged(object sender, EventArgs e)
        {
            numericUpDownSettingPickTimeH.Value = 0;
        }

        private void buttonSettingSave_Click(object sender, EventArgs e)
        {
            PrepareSettingsAndSave();
        }

        private void PrepareSettingsAndSave()
        {
            MainForm.DistanceMultiplier = MainForm.DistanceMultipliers[comboBoxSettingDistanceMesSelect.SelectedValue.ToString()];
            MainForm.ProgSettingsV.DistanceMes = comboBoxSettingDistanceMesSelect.SelectedValue.ToString();
            MainForm.ProgSettingsV.CurrencyMes = comboBoxSettingCurrencySelect.SelectedValue.ToString();
            MainForm.ProgSettingsV.JobPickupTime = (short)(numericUpDownSettingPickTimeH.Value + numericUpDownSettingPickTimeD.Value * 24);

            MainForm.WriteConfig();

            MainForm.FillAccountMoneyTB();
        }

        private void buttonSettingCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void numericUpDownSettingLoopCitys_ValueChanged(object sender, EventArgs e)
        {
            MainForm.ProgSettingsV.LoopEvery = Convert.ToByte(numericUpDownSettingLoopCitys.Value);
        }

        private void CorrectControlsPositions()
        {
            //Longest setting string
            Control[] labellist = { labelJobPickupTime, labelLoopEvery, labelDistance, labelCurrency };
            int longeststr = 0, margin = 6;

            foreach (Control c in labellist)
            {
                Label temp = c as Label;
                if (c.Width > longeststr)
                    longeststr = c.Width;
            }

            Control[][] Controllist = new Control[4][];
            Controllist[0] = new Control[] { numericUpDownSettingPickTimeD, labelDayShort, numericUpDownSettingPickTimeH, labelHourShort };
            Controllist[1] = new Control[] { numericUpDownSettingLoopCitys, labelCity };
            Controllist[2] = new Control[] { comboBoxSettingDistanceMesSelect };
            Controllist[3] = new Control[] { comboBoxSettingCurrencySelect };

            foreach (Control[] cc in Controllist)
            {
                int margincount = 2, startX = longeststr;
                
                foreach (Control c in cc)
                {
                    c.Location = new Point(startX + margin * margincount, c.Location.Y);
                    startX += c.Width;
                    margincount++;
                }
            }
        }
    }
}
