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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using TS_SE_Tool.CustomClasses;

namespace TS_SE_Tool
{
    public partial class FormSettings : Form
    {
        FormMain MainForm = Application.OpenForms.OfType<FormMain>().Single();

        public FormSettings()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.MainIco;

            DataTable combDT = new DataTable();
            combDT.Columns.Add("ID");
            combDT.Columns.Add("DistDisplayName");

            combDT.Rows.Add(new object[] { "km", "Kilometers" });
            combDT.Rows.Add(new object[] { "mi", "Miles" });

            comboBoxSettingDistanceMesSelect.ValueMember = "ID";
            comboBoxSettingDistanceMesSelect.DisplayMember = "DistDisplayName";
            comboBoxSettingDistanceMesSelect.DataSource = combDT;
            comboBoxSettingDistanceMesSelect.SelectedValue = MainForm.ProgSettingsV.DistanceMes; // - 1;

            numericUpDownSettingPickTimeD.Value = Math.Floor((decimal)(MainForm.ProgSettingsV.JobPickupTime / 24));
            numericUpDownSettingPickTimeH.Value = MainForm.ProgSettingsV.JobPickupTime - numericUpDownSettingPickTimeD.Value * 24;

            numericUpDownSettingLoopCitys.Value = MainForm.ProgSettingsV.LoopEvery;
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
            if (comboBoxSettingDistanceMesSelect.SelectedValue != null)
            {
                MainForm.DistanceMultiplier = MainForm.DistanceMultipliers[comboBoxSettingDistanceMesSelect.SelectedValue.ToString()];
                MainForm.ProgSettingsV.DistanceMes = comboBoxSettingDistanceMesSelect.SelectedValue.ToString();
            }
                
            MainForm.WriteConfig();
        }

        private void buttonSettingCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void numericUpDownSettingLoopCitys_ValueChanged(object sender, EventArgs e)
        {
            MainForm.ProgSettingsV.LoopEvery = Convert.ToByte(numericUpDownSettingLoopCitys.Value);
        }
    }
}
