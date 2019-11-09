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
using System.Globalization;
using System.Threading;

namespace TS_SE_Tool
{
    public partial class FormShareUserColors : Form
    {
        Button[] UserColorsB = new Button[8];
        CheckBox[] UserColorsCB = new CheckBox[8];
        Button[] ImportColorsB = new Button[0];
        CheckBox[] ImportColorsCB = new CheckBox[0];
        List<Color> ImportedColors = new List<Color>();

        FormMain MainForm = Application.OpenForms.OfType<FormMain>().Single();

        public FormShareUserColors()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.MainIco;

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

            PopulateFormControlsk();

            buttonReplaceColors.Enabled = false;
            groupBoxImportedColors.Visible = false;

            this.Size = new Size(this.Size.Width, 209);
        }

        private void PopulateFormControlsk()
        {
            CreateUserColorsButtons();
            UpdateUserColorsButtons();
        }

        private void CreateUserColorsButtons()
        {
            int padding = 6, width = 48, colorcount = 8;
            //UserColorsList.Count

            for (int i = 0; i < colorcount; i++)
            {
                Button colorB = new Button();
                groupBoxProfileUserColors.Controls.Add(colorB);

                colorB.Name = "buttonUC" + i.ToString();
                colorB.Text = null;
                colorB.Location = new Point(15 + (padding + width) * (i), 16);
                colorB.Size = new Size(width, width);
                colorB.FlatStyle = FlatStyle.Flat;
                colorB.Enabled = false;

                //rb.Click += new EventHandler(MainForm.SelectColor);
                UserColorsB[i] = colorB;

                CheckBox colorCB = new CheckBox();
                groupBoxProfileUserColors.Controls.Add(colorCB);
                colorCB.Parent = groupBoxProfileUserColors;

                //Ppanel.Appearance = Appearance.Button;
                colorCB.FlatStyle = FlatStyle.Flat;
                colorCB.Size = new Size(width, width / 4);
                colorCB.Name = "checkboxUC" + i.ToString();
                colorCB.Checked = false;
                colorCB.Text = "";
                colorCB.AutoSize = true;
                colorCB.Location = new Point(15 + (48 - colorCB.Width) / 2 + (padding + width) * (i), 24 + width);

                UserColorsCB[i] = colorCB;
                //Ppanel.Padding = new Padding(0, 0, 1, 2);
                //Ppanel.BackgroundImageLayout = ImageLayout.Stretch;
            }
        }

        private void UpdateUserColorsButtons()
        {
            int padding = 6, width = 23;//, colorcount = 8;

            for (int i = 0; i < MainForm.UserColorsList.Count; i++)
            {
                Button btn = null;
                string btnname = "buttonUC" + i.ToString();

                if (groupBoxProfileUserColors.Controls.ContainsKey(btnname))
                {
                    btn = groupBoxProfileUserColors.Controls[btnname] as Button;
                }
                else
                {
                    btn.Name = "buttonUC" + i.ToString();
                    btn.Text = null;
                    btn.Location = new Point(6 + (padding + width) * (i), 19);
                    btn.Size = new Size(width, 23);
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.Enabled = false;

                    btn.Click += new EventHandler(MainForm.SelectColor);

                    groupBoxProfileUserColors.Controls.Add(btn);
                }

                if (btn != null)
                {
                    btn.Enabled = true;
                    if (MainForm.UserColorsList[i].A == 0)
                    {
                        btn.Text = "X";
                        btn.BackColor = Color.FromName("Control");
                        UserColorsCB[i].Enabled = false;
                    }
                    else
                    {
                        btn.Text = "";
                        btn.BackColor = MainForm.UserColorsList[i];
                    }
                }
            }
        }

        private void CreateImportColorsButtons(int _colorcount)
        {
            int padding = 6, width = 48, colorcount = _colorcount;//8;
            //UserColorsList.Count

            Array.Resize(ref ImportColorsB, colorcount);
            Array.Resize(ref ImportColorsCB, colorcount);

            for (int i = 0; i < colorcount; i++)
            {
                CheckBox Ppanel = new CheckBox();
                groupBoxImportedColors.Controls.Add(Ppanel);
                Ppanel.Parent = groupBoxImportedColors;

                //Ppanel.Appearance = Appearance.Button;
                Ppanel.FlatStyle = FlatStyle.Flat;
                Ppanel.Size = new Size(width, width / 4);
                Ppanel.Name = "checkboxIC" + i.ToString();
                Ppanel.Checked = true;
                Ppanel.Text = "";
                Ppanel.AutoSize = true;
                Ppanel.Location = new Point(15 + (48 - Ppanel.Width) / 2 + (padding + width) * i, 16);

                ImportColorsCB[i] = Ppanel;

                Button rb = new Button();
                groupBoxImportedColors.Controls.Add(rb);

                rb.Name = "buttonIC" + i.ToString();
                rb.Text = null;
                rb.Location = new Point(15 + (padding + width) * (i), 24 + Ppanel.Height);
                rb.Size = new Size(width, width);
                rb.FlatStyle = FlatStyle.Flat;
                rb.Enabled = false;
                rb.BackColor = ImportedColors[i];
                //rb.Click += new EventHandler(MainForm.SelectColor);
                ImportColorsB[i] = rb;

                //Ppanel.Padding = new Padding(0, 0, 1, 2);
                //Ppanel.BackgroundImageLayout = ImageLayout.Stretch;
            }
        }

        private void buttonExportColors_Click(object sender, EventArgs e)
        {
            string tempData = "UserColors";
            int i = 0;
            bool ready = false;

            foreach (CheckBox temp in UserColorsCB)
            {
                if (temp.Checked)
                {
                    tempData += "\r\n" + UserColorsB[i].BackColor.ToArgb().ToString();
                    temp.Checked = false;
                    ready = true;
                }
                i++;
            }

            if (!ready)
                return;

            string asd = BitConverter.ToString(MainForm.zipText(tempData)).Replace("-", "");
            Clipboard.SetText(asd);
            MessageBox.Show("Color data has been copied.");
        }

        private void buttonImportColors_Click(object sender, EventArgs e)
        {
            if(ImportColorsB.Length > 0)
            {
                foreach(Button t in ImportColorsB)
                {
                    t.Dispose();
                }

                foreach (CheckBox t in ImportColorsCB)
                {
                    t.Dispose();
                }
            }
            ImportedColors.Clear();

            try
            {
                string inputData = MainForm.unzipText(Clipboard.GetText());
                string[] Lines = inputData.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                if (Lines[0] == "UserColors")
                {
                    List<string> paintstr = new List<string>();

                    for (int i = 1; i < Lines.Length; i++)
                    {
                        ImportedColors.Add(Color.FromArgb(Int32.Parse( Lines[i])));
                    }

                    int impColors = Lines.Length - 1;
                    CreateImportColorsButtons(impColors);

                    foreach (CheckBox colorCB in UserColorsCB)
                    {
                        colorCB.Checked = false;
                    }

                    int g = 0;

                    foreach (Button t in UserColorsB)
                    {
                        if (MainForm.UserColorsList[g].A == 0 && impColors > 0)
                        {
                            UserColorsCB[g].Checked = true;
                            impColors--;

                            if (impColors == 0)
                                break;
                        }
                        g++;
                    }

                    this.Size = new Size(this.Size.Width, 323);
                    groupBoxImportedColors.Visible = true;
                    buttonReplaceColors.Enabled = true;

                    foreach (CheckBox colorCB in UserColorsCB)
                    {
                        colorCB.Enabled = true;
                    }

                    buttonExport.Enabled = false;

                    MessageBox.Show("Color data  has been inserted.");

                }
                else
                    MessageBox.Show("Wrong data. Expected Color data but\r\n" + Lines[0] + "\r\nwas found.");
            }
            catch
            {
                MessageBox.Show("Something gone wrong.");
            }
        }

        private void buttonReplaceColors_Click(object sender, EventArgs e)
        {
            List<Button> tempBList = new List<Button>();

            int i = 0, j = 0;
            foreach (CheckBox temp in ImportColorsCB)
            {
                if (temp.Checked)
                {
                    tempBList.Add(ImportColorsB[i]);
                    temp.Checked = false;
                }
                i++;
            }

            if (tempBList.Count > 0)
            {
                i = 0;
                foreach (CheckBox temp in UserColorsCB)
                {
                    if (temp.Checked)
                    {
                        if (j < tempBList.Count)
                        {
                            MainForm.UserColorsList[i] = tempBList[j].BackColor;
                            j++;
                        }
                        temp.Checked = false;
                    }
                    i++;
                }

                UpdateUserColorsButtons();

                buttonExport.Enabled = true;
            }
        }

        private void CorrectControlsPositions()
        {

        }
    }
}
