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
        FormMain MainForm = Application.OpenForms.OfType<FormMain>().Single();

        Button[] UserColorsB;
        CheckBox[] UserColorsCB;
        Button[] ImportColorsB = new Button[0];
        CheckBox[] ImportColorsCB = new CheckBox[0];
        List<Color> ImportedColors = new List<Color>();

        ushort SaveVersion = 0;

        public FormShareUserColors()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.MainIco;

            MainForm.HelpTranslateControl(this);

            MainForm.HelpTranslateFormMethod(this);

            SaveVersion = MainForm.MainSaveFileInfoData.Version;

            int colorcount = FormMain.SiiNunitData.Economy.user_colors.Count;

            UserColorsB = new Button[colorcount];

            if (SaveVersion >= 49)
                colorcount = colorcount / 4;

            UserColorsCB = new CheckBox[colorcount];

            CorrectControlsPositions();

            PopulateFormControlsk();

            buttonReplaceColors.Enabled = false;
            groupBoxImportedColors.Visible = false;

            ChangeFormSize(false);
        }

        //Create controls
        private void PopulateFormControlsk()
        {
            CreateUserColorsButtons();
            UpdateUserColorsButtons();
        }

        private void CreateUserColorsButtons()
        {
            int padding = 6, width = 48, offsetL = 4, offsetT = 4, colorcount = FormMain.SiiNunitData.Economy.user_colors.Count;
            //UserColorsList.Count

            if (SaveVersion >= 49)
            {
                int ucc = FormMain.SiiNunitData.Economy.user_colors.Count / 4;
                int btnNumber = 0;
                width = 24;

                for (int i = 0; i < ucc; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        int multX = 0;
                        if (j == 1 || j == 3)
                            multX = 1;

                        Button colorB = new Button();
                        panelProfileUserColors.Controls.Add(colorB);

                        colorB.Name = "buttonUC" + btnNumber.ToString();
                        colorB.Text = null;

                        int x = offsetL + width * multX + i * (width * 2 + padding);
                        int y = offsetT + width * (j / 2);

                        colorB.Location = new Point(x, y);
                        colorB.Size = new Size(width, width);
                        colorB.FlatStyle = FlatStyle.Flat;
                        colorB.Enabled = false;

                        UserColorsB[btnNumber] = colorB;

                        btnNumber++;
                    }

                    CheckBox colorCB = new CheckBox();
                    panelProfileUserColors.Controls.Add(colorCB);
                    colorCB.Parent = panelProfileUserColors;

                    colorCB.FlatStyle = FlatStyle.Flat;
                    colorCB.Size = new Size(width * 2, width / 2);
                    colorCB.Name = "checkboxUC" + i.ToString();
                    colorCB.Checked = false;
                    colorCB.Text = "";
                    colorCB.AutoSize = true;
                    colorCB.Location = new Point(offsetL + (width * 2 - colorCB.Width) / 2 + (width * 2 + padding) * i, offsetT + padding + width * 2);
                    colorCB.Enabled = false;

                    UserColorsCB[i] = colorCB;
                }

                panelProfileUserColors.HorizontalScroll.Maximum = 2500; //Virtual width
                panelProfileUserColors.MouseWheel += new MouseEventHandler(this.panelProfileUserColors_MouseWheel);
            }
            else
            {
                for (int i = 0; i < colorcount; i++)
                {
                    Button colorB = new Button();
                    panelProfileUserColors.Controls.Add(colorB);

                    colorB.Name = "buttonUC" + i.ToString();
                    colorB.Text = null;
                    colorB.Location = new Point(offsetL + (padding + width) * (i), offsetT);
                    colorB.Size = new Size(width, width);
                    colorB.FlatStyle = FlatStyle.Flat;
                    colorB.Enabled = false;

                    UserColorsB[i] = colorB;

                    CheckBox colorCB = new CheckBox();
                    panelProfileUserColors.Controls.Add(colorCB);
                    colorCB.Parent = panelProfileUserColors;

                    colorCB.FlatStyle = FlatStyle.Flat;
                    colorCB.Size = new Size(width, width / 4);
                    colorCB.Name = "checkboxUC" + i.ToString();
                    colorCB.Checked = false;
                    colorCB.Text = "";
                    colorCB.AutoSize = true;
                    colorCB.Location = new Point(offsetL + (width - colorCB.Width) / 2 + (padding + width) * (i), offsetT + padding + width);

                    UserColorsCB[i] = colorCB;
                }
            }
        }

        private void UpdateUserColorsButtons()
        {
            int padding = 6, width = 23;//, colorcount = 8;

            for (int i = 0; i < FormMain.SiiNunitData.Economy.user_colors.Count; i++)
            {
                Button btn = null;
                string btnname = "buttonUC" + i.ToString();

                if (panelProfileUserColors.Controls.ContainsKey(btnname))
                {
                    btn = panelProfileUserColors.Controls[btnname] as Button;
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

                    panelProfileUserColors.Controls.Add(btn);
                }

                if (btn != null)
                {
                    btn.Enabled = true;
                    if (FormMain.SiiNunitData.Economy.user_colors[i].color.A == 0)
                    {
                        btn.Text = "X";
                        btn.BackColor = Color.FromName("Control");
                    }
                    else
                    {
                        btn.Text = "";
                        btn.BackColor = FormMain.SiiNunitData.Economy.user_colors[i].color;

                        if (SaveVersion >= 49)
                            UserColorsCB[i / 4].Enabled = true;
                        else
                            UserColorsCB[i].Enabled = true;

                    }
                }
            }
        }

        private void panelProfileUserColors_MouseWheel(object sender, MouseEventArgs e)
        {
            Panel senderPanel = sender as Panel;

            if (e.Delta != 0)
            {
                int scroll = 48;
                int location = Math.Abs(senderPanel.AutoScrollPosition.X);

                if (e.Delta < 0)
                {
                    if (location + scroll < senderPanel.HorizontalScroll.Maximum)
                    {
                        location += scroll;
                        senderPanel.HorizontalScroll.Value = location;
                    }
                    else
                    {
                        location = senderPanel.HorizontalScroll.Maximum;
                        senderPanel.AutoScrollPosition = new Point(location, 0);
                    }
                }
                else
                {
                    if (location - scroll > 0)
                    {
                        location -= scroll;
                        senderPanel.HorizontalScroll.Value = location;
                    }
                    else
                    {
                        location = 0;
                        senderPanel.AutoScrollPosition = new Point(location, 0);
                    }
                }
            }
        }
        //
        private void CreateImportColorsButtons(int _colorcount)
        {
            int padding = 6, width = 48, offsetL = 4, offsetT = 4, colorcount = _colorcount;//8;
            //UserColorsList.Count
            int ucc = FormMain.SiiNunitData.Economy.user_colors.Count / 4;
            int btnNumber = 0;

            Array.Resize(ref ImportColorsB, colorcount);

            if (SaveVersion >= 49)
                colorcount = colorcount / 4;

            Array.Resize(ref ImportColorsCB, colorcount);


            panelImportedColors.HorizontalScroll.Maximum = 2500; //Virtual width
            panelImportedColors.MouseWheel += new MouseEventHandler(this.panelProfileUserColors_MouseWheel);

            for (int i = 0; i < colorcount; i++)
            {
                CheckBox chkbox = new CheckBox();
                panelImportedColors.Controls.Add(chkbox);
                chkbox.Parent = panelImportedColors;

                chkbox.FlatStyle = FlatStyle.Flat;
                chkbox.Size = new Size(width, width / 4);
                chkbox.Name = "checkboxIC" + i.ToString();
                chkbox.Checked = true;
                chkbox.Text = "";
                chkbox.AutoSize = true;                
                chkbox.Location = new Point(offsetL + (width - chkbox.Width) / 2 + (padding + width) * i, offsetT);

                ImportColorsCB[i] = chkbox;

                if (SaveVersion >= 49)
                {
                    int widthB = width / 2;

                    for (int j = 0; j < 4; j++)
                    {
                        int multX = 0;
                        if (j == 1 || j == 3)
                            multX = 1;

                        Button bttn = new Button();
                        panelImportedColors.Controls.Add(bttn);

                        bttn.Name = "buttonIC" + btnNumber.ToString();

                        int x = offsetL + widthB * multX + i * (widthB * 2 + padding);
                        int y = offsetT + widthB * (j / 2) + chkbox.Height + padding;

                        bttn.Location = new Point(x, y);
                        bttn.Size = new Size(widthB, widthB);
                        bttn.FlatStyle = FlatStyle.Flat;
                        bttn.Enabled = false;
                        bttn.BackColor = ImportedColors[btnNumber];

                        if (bttn.BackColor.A != 0)
                            bttn.Text = null;
                        else
                            bttn.Text = "X";

                        ImportColorsB[btnNumber] = bttn;

                        btnNumber++;
                    }
                }
                else
                {
                    Button bttn = new Button();
                    panelImportedColors.Controls.Add(bttn);

                    bttn.Name = "buttonIC" + i.ToString();
                    bttn.Location = new Point(offsetL + (padding + width) * i, offsetT + padding + chkbox.Height);
                    bttn.Size = new Size(width, width);
                    bttn.FlatStyle = FlatStyle.Flat;
                    bttn.Enabled = false;
                    bttn.BackColor = ImportedColors[i];

                    if (bttn.BackColor.A != 0)
                        bttn.Text = null;
                    else
                        bttn.Text = "X";

                    ImportColorsB[i] = bttn;
                }
            }
        }

        private void CorrectControlsPositions()
        {

        }
        //Buttons
        private void buttonExportColors_Click(object sender, EventArgs e)
        {
            string tempData = "UserColors";
            int i = 0;
            bool ready = false;

            foreach (CheckBox temp in UserColorsCB)
            {
                if (temp.Checked)
                {
                    if (SaveVersion >= 49)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            tempData += "\r\n" + FormMain.SiiNunitData.Economy.user_colors[i * 4 + j].color.ToArgb().ToString();
                            temp.Checked = false;
                            ready = true;                            
                        }
                    }
                    else
                    {
                        tempData += "\r\n" + UserColorsB[i].BackColor.ToArgb().ToString();
                        temp.Checked = false;
                        ready = true;
                    }
                }
                i++;
            }

            if (!ready)
                return;

            string asd = BitConverter.ToString(Utilities.ZipDataUtilities.zipText(tempData)).Replace("-", "");
            Clipboard.SetText(asd);
            MessageBox.Show("Color data has been copied.");
        }

        private void buttonImportColors_Click(object sender, EventArgs e)
        {
            //Remove imported prev buttons and chkboxes
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

            ImportedColors.Clear(); //Clear imorted color list
            
            //Start import
            try
            {
                string inputData = Utilities.ZipDataUtilities.unzipText(Clipboard.GetText()); //Get data and unzip

                if (inputData == null)
                    return;

                string[] Lines = inputData.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries); //Split

                if (Lines[0] == "UserColors") //Check if it is color data
                {
                    //Populate color list
                    for (int i = 1; i < Lines.Length; i++)
                    {
                        ImportedColors.Add(Color.FromArgb(Int32.Parse( Lines[i])));
                    }

                    //Create color represantation buttons with chkboxes
                    int impColors = Lines.Length - 1;
                    CreateImportColorsButtons(impColors);

                    //Uncheck all in existing color list
                    foreach (CheckBox colorCB in UserColorsCB)
                    {
                        colorCB.Checked = false;
                    }

                    //Show imopted colors section
                    ChangeFormSize(true);                    

                    groupBoxImportedColors.Visible = true;
                    buttonReplaceColors.Enabled = true;

                    //Enable checkboxes to enable import
                    foreach (CheckBox colorCB in UserColorsCB)
                    {
                        colorCB.Enabled = true;
                    }

                    buttonExport.Enabled = false; //Disable export button

                    MessageBox.Show("Color data  has been inserted.");
                }
                else
                    MessageBox.Show("Wrong data. Expected Color data but\r\n" + Lines[0] + "\r\nwas found.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Something gone wrong.");
            }
        }

        private void buttonReplaceColors_Click(object sender, EventArgs e)
        {
            if (SaveVersion >= 49)
            {
                List<Button> tempBList = new List<Button>();
                int i = 0, j = 0;

                foreach (CheckBox temp in ImportColorsCB)
                {
                    if (temp.Checked)
                    {
                        temp.Checked = false;
                        for(int k = 0; k < 4; k++)
                        {
                            tempBList.Add(ImportColorsB[i]);
                            i++;
                        }
                    }
                    else
                        i = i + 4;
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
                                for (int k = 0; k < 4; k++)
                                {
                                    FormMain.SiiNunitData.Economy.user_colors[i].color = tempBList[j].BackColor;

                                    i++;
                                    j++;
                                }
                            }
                            temp.Checked = false;
                        }
                        else
                            i = i + 4;
                    }
                }
            }
            else
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
                                FormMain.SiiNunitData.Economy.user_colors[i].color = tempBList[j].BackColor;
                                j++;
                            }
                            temp.Checked = false;
                        }
                        i++;
                    }
                }
            }

            UpdateUserColorsButtons();

            buttonExport.Enabled = true;
        }

        //Change form size
        private void ChangeFormSize(bool _big)
        {
            if (_big)
            {
                this.MaximumSize = new Size(this.Size.Width, 323);
            }
            else
            {
                this.MaximumSize = new Size(this.Size.Width, 209);
            }

            this.Size = new Size(this.Size.Width, this.Size.Height);
            this.MinimumSize = this.MaximumSize;
        }
    }
}