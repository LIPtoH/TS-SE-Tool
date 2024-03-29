﻿/*
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
namespace TS_SE_Tool
{
    partial class FormSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelCurrencyATS = new System.Windows.Forms.Label();
            this.comboBoxSettingCurrencySelectATS = new System.Windows.Forms.ComboBox();
            this.labelCurrencyETS2 = new System.Windows.Forms.Label();
            this.labelDistance = new System.Windows.Forms.Label();
            this.labelJobPickupTime = new System.Windows.Forms.Label();
            this.comboBoxSettingDistanceMesSelect = new System.Windows.Forms.ComboBox();
            this.numericUpDownSettingPickTimeH = new System.Windows.Forms.NumericUpDown();
            this.comboBoxSettingCurrencySelectETS2 = new System.Windows.Forms.ComboBox();
            this.labelLoopEvery = new System.Windows.Forms.Label();
            this.labelCurrency = new System.Windows.Forms.Label();
            this.numericUpDownSettingLoopCitys = new System.Windows.Forms.NumericUpDown();
            this.labelCity = new System.Windows.Forms.Label();
            this.labelDayShort = new System.Windows.Forms.Label();
            this.numericUpDownSettingPickTimeD = new System.Windows.Forms.NumericUpDown();
            this.labelHourShort = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.labelWeight = new System.Windows.Forms.Label();
            this.comboBoxWeightMesSelect = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSettingPickTimeH)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSettingLoopCitys)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSettingPickTimeD)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.buttonSave, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonCancel, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(364, 261);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // buttonSave
            // 
            this.buttonSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSave.Location = new System.Drawing.Point(3, 224);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(176, 34);
            this.buttonSave.TabIndex = 0;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSettingSave_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCancel.Location = new System.Drawing.Point(185, 224);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(176, 34);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonSettingCancel_Click);
            // 
            // panel1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.panel1, 2);
            this.panel1.Controls.Add(this.comboBoxWeightMesSelect);
            this.panel1.Controls.Add(this.labelWeight);
            this.panel1.Controls.Add(this.labelCurrencyATS);
            this.panel1.Controls.Add(this.comboBoxSettingCurrencySelectATS);
            this.panel1.Controls.Add(this.labelCurrencyETS2);
            this.panel1.Controls.Add(this.labelDistance);
            this.panel1.Controls.Add(this.labelJobPickupTime);
            this.panel1.Controls.Add(this.comboBoxSettingDistanceMesSelect);
            this.panel1.Controls.Add(this.numericUpDownSettingPickTimeH);
            this.panel1.Controls.Add(this.comboBoxSettingCurrencySelectETS2);
            this.panel1.Controls.Add(this.labelLoopEvery);
            this.panel1.Controls.Add(this.labelCurrency);
            this.panel1.Controls.Add(this.numericUpDownSettingLoopCitys);
            this.panel1.Controls.Add(this.labelCity);
            this.panel1.Controls.Add(this.labelDayShort);
            this.panel1.Controls.Add(this.numericUpDownSettingPickTimeD);
            this.panel1.Controls.Add(this.labelHourShort);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(358, 215);
            this.panel1.TabIndex = 2;
            // 
            // labelCurrencyATS
            // 
            this.labelCurrencyATS.AutoSize = true;
            this.labelCurrencyATS.Location = new System.Drawing.Point(171, 151);
            this.labelCurrencyATS.Name = "labelCurrencyATS";
            this.labelCurrencyATS.Size = new System.Drawing.Size(28, 13);
            this.labelCurrencyATS.TabIndex = 14;
            this.labelCurrencyATS.Text = "ATS";
            // 
            // comboBoxSettingCurrencySelectATS
            // 
            this.comboBoxSettingCurrencySelectATS.FormattingEnabled = true;
            this.comboBoxSettingCurrencySelectATS.Location = new System.Drawing.Point(214, 148);
            this.comboBoxSettingCurrencySelectATS.Name = "comboBoxSettingCurrencySelectATS";
            this.comboBoxSettingCurrencySelectATS.Size = new System.Drawing.Size(70, 21);
            this.comboBoxSettingCurrencySelectATS.TabIndex = 13;
            // 
            // labelCurrencyETS2
            // 
            this.labelCurrencyETS2.AutoSize = true;
            this.labelCurrencyETS2.Location = new System.Drawing.Point(171, 122);
            this.labelCurrencyETS2.Name = "labelCurrencyETS2";
            this.labelCurrencyETS2.Size = new System.Drawing.Size(37, 13);
            this.labelCurrencyETS2.TabIndex = 12;
            this.labelCurrencyETS2.Text = "ETS 2";
            // 
            // labelDistance
            // 
            this.labelDistance.AutoSize = true;
            this.labelDistance.Location = new System.Drawing.Point(9, 68);
            this.labelDistance.MaximumSize = new System.Drawing.Size(205, 0);
            this.labelDistance.Name = "labelDistance";
            this.labelDistance.Size = new System.Drawing.Size(49, 13);
            this.labelDistance.TabIndex = 11;
            this.labelDistance.Text = "Distance";
            // 
            // labelJobPickupTime
            // 
            this.labelJobPickupTime.AutoSize = true;
            this.labelJobPickupTime.Location = new System.Drawing.Point(9, 15);
            this.labelJobPickupTime.MaximumSize = new System.Drawing.Size(205, 0);
            this.labelJobPickupTime.MinimumSize = new System.Drawing.Size(170, 0);
            this.labelJobPickupTime.Name = "labelJobPickupTime";
            this.labelJobPickupTime.Size = new System.Drawing.Size(170, 13);
            this.labelJobPickupTime.TabIndex = 0;
            this.labelJobPickupTime.Text = "Cargo relevance time";
            // 
            // comboBoxSettingDistanceMesSelect
            // 
            this.comboBoxSettingDistanceMesSelect.FormattingEnabled = true;
            this.comboBoxSettingDistanceMesSelect.Location = new System.Drawing.Point(214, 65);
            this.comboBoxSettingDistanceMesSelect.Name = "comboBoxSettingDistanceMesSelect";
            this.comboBoxSettingDistanceMesSelect.Size = new System.Drawing.Size(121, 21);
            this.comboBoxSettingDistanceMesSelect.TabIndex = 10;
            // 
            // numericUpDownSettingPickTimeH
            // 
            this.numericUpDownSettingPickTimeH.Location = new System.Drawing.Point(282, 13);
            this.numericUpDownSettingPickTimeH.Maximum = new decimal(new int[] {
            24,
            0,
            0,
            0});
            this.numericUpDownSettingPickTimeH.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.numericUpDownSettingPickTimeH.Name = "numericUpDownSettingPickTimeH";
            this.numericUpDownSettingPickTimeH.Size = new System.Drawing.Size(40, 20);
            this.numericUpDownSettingPickTimeH.TabIndex = 1;
            this.numericUpDownSettingPickTimeH.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // comboBoxSettingCurrencySelectETS2
            // 
            this.comboBoxSettingCurrencySelectETS2.FormattingEnabled = true;
            this.comboBoxSettingCurrencySelectETS2.Location = new System.Drawing.Point(214, 119);
            this.comboBoxSettingCurrencySelectETS2.Name = "comboBoxSettingCurrencySelectETS2";
            this.comboBoxSettingCurrencySelectETS2.Size = new System.Drawing.Size(70, 21);
            this.comboBoxSettingCurrencySelectETS2.TabIndex = 9;
            // 
            // labelLoopEvery
            // 
            this.labelLoopEvery.AutoSize = true;
            this.labelLoopEvery.Location = new System.Drawing.Point(9, 41);
            this.labelLoopEvery.MaximumSize = new System.Drawing.Size(205, 0);
            this.labelLoopEvery.Name = "labelLoopEvery";
            this.labelLoopEvery.Size = new System.Drawing.Size(61, 13);
            this.labelLoopEvery.TabIndex = 0;
            this.labelLoopEvery.Text = "Loop Every";
            // 
            // labelCurrency
            // 
            this.labelCurrency.AutoSize = true;
            this.labelCurrency.Location = new System.Drawing.Point(9, 122);
            this.labelCurrency.MaximumSize = new System.Drawing.Size(205, 0);
            this.labelCurrency.Name = "labelCurrency";
            this.labelCurrency.Size = new System.Drawing.Size(49, 13);
            this.labelCurrency.TabIndex = 8;
            this.labelCurrency.Text = "Currency";
            // 
            // numericUpDownSettingLoopCitys
            // 
            this.numericUpDownSettingLoopCitys.Location = new System.Drawing.Point(214, 39);
            this.numericUpDownSettingLoopCitys.Name = "numericUpDownSettingLoopCitys";
            this.numericUpDownSettingLoopCitys.Size = new System.Drawing.Size(40, 20);
            this.numericUpDownSettingLoopCitys.TabIndex = 1;
            this.numericUpDownSettingLoopCitys.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // labelCity
            // 
            this.labelCity.AutoSize = true;
            this.labelCity.Location = new System.Drawing.Point(261, 41);
            this.labelCity.Name = "labelCity";
            this.labelCity.Size = new System.Drawing.Size(23, 13);
            this.labelCity.TabIndex = 7;
            this.labelCity.Text = "city";
            // 
            // labelDayShort
            // 
            this.labelDayShort.AutoSize = true;
            this.labelDayShort.Location = new System.Drawing.Point(261, 15);
            this.labelDayShort.Name = "labelDayShort";
            this.labelDayShort.Size = new System.Drawing.Size(15, 13);
            this.labelDayShort.TabIndex = 6;
            this.labelDayShort.Text = "D";
            // 
            // numericUpDownSettingPickTimeD
            // 
            this.numericUpDownSettingPickTimeD.Location = new System.Drawing.Point(214, 13);
            this.numericUpDownSettingPickTimeD.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.numericUpDownSettingPickTimeD.Name = "numericUpDownSettingPickTimeD";
            this.numericUpDownSettingPickTimeD.Size = new System.Drawing.Size(41, 20);
            this.numericUpDownSettingPickTimeD.TabIndex = 4;
            this.numericUpDownSettingPickTimeD.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // labelHourShort
            // 
            this.labelHourShort.AutoSize = true;
            this.labelHourShort.Location = new System.Drawing.Point(328, 15);
            this.labelHourShort.Name = "labelHourShort";
            this.labelHourShort.Size = new System.Drawing.Size(13, 13);
            this.labelHourShort.TabIndex = 5;
            this.labelHourShort.Text = "h";
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 15000;
            this.toolTip.InitialDelay = 500;
            this.toolTip.ReshowDelay = 100;
            // 
            // labelWeight
            // 
            this.labelWeight.AutoSize = true;
            this.labelWeight.Location = new System.Drawing.Point(9, 95);
            this.labelWeight.Name = "labelWeight";
            this.labelWeight.Size = new System.Drawing.Size(41, 13);
            this.labelWeight.TabIndex = 15;
            this.labelWeight.Text = "Weight";
            // 
            // comboBoxWeightMesSelect
            // 
            this.comboBoxWeightMesSelect.FormattingEnabled = true;
            this.comboBoxWeightMesSelect.Location = new System.Drawing.Point(214, 92);
            this.comboBoxWeightMesSelect.Name = "comboBoxWeightMesSelect";
            this.comboBoxWeightMesSelect.Size = new System.Drawing.Size(121, 21);
            this.comboBoxWeightMesSelect.TabIndex = 16;
            // 
            // FormSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(364, 261);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(380, 300);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(380, 300);
            this.Name = "FormSettings";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSettingPickTimeH)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSettingLoopCitys)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSettingPickTimeD)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelDistance;
        private System.Windows.Forms.Label labelJobPickupTime;
        private System.Windows.Forms.ComboBox comboBoxSettingDistanceMesSelect;
        private System.Windows.Forms.NumericUpDown numericUpDownSettingPickTimeH;
        private System.Windows.Forms.ComboBox comboBoxSettingCurrencySelectETS2;
        private System.Windows.Forms.Label labelLoopEvery;
        private System.Windows.Forms.Label labelCurrency;
        private System.Windows.Forms.NumericUpDown numericUpDownSettingLoopCitys;
        private System.Windows.Forms.Label labelCity;
        private System.Windows.Forms.Label labelDayShort;
        private System.Windows.Forms.NumericUpDown numericUpDownSettingPickTimeD;
        private System.Windows.Forms.Label labelHourShort;
        private System.Windows.Forms.Label labelCurrencyATS;
        private System.Windows.Forms.ComboBox comboBoxSettingCurrencySelectATS;
        private System.Windows.Forms.Label labelCurrencyETS2;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ComboBox comboBoxWeightMesSelect;
        private System.Windows.Forms.Label labelWeight;
    }
}