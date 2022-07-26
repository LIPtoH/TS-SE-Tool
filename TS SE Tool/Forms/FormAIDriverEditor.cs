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

namespace TS_SE_Tool
{
    public partial class FormAIDriverEditor : Form
    {
        FormMain MainForm = Application.OpenForms.OfType<FormMain>().Single();

        private CheckBox[] ADRbuttonArray = new CheckBox[6];
        private CheckBox[,] SkillButtonArray = new CheckBox[5, 6];

        internal Driver driverData;

        internal FormAIDriverEditor(Driver _driver)
        {
            driverData = _driver;

            InitializeComponent();

            PrepareForm();
        }

        private void PrepareForm()
        {
            this.Icon = Properties.Resources.MainIco;

            CreateProfilePanelControls();
            FillFormProfileControls();

            MainForm.HelpTranslateControl(this);
            MainForm.HelpTranslateFormMethod(this, toolTipAIDriverEditor);

            labelDriverNameText.Text = driverData.driverNameTranslated;

            //dialog result
            buttonSave.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;
        }

        private void CreateProfilePanelControls()
        {
            int pSkillsNameHeight = 64, pSkillsNameWidth = 64, pSkillsNameOffset = 5, pSkillsNamelOffset = 12;

            for (int i = 0; i < 6; i++)
            {
                Panel Ppanel = new Panel();
                groupBoxDriverSkill.Controls.Add(Ppanel);

                Ppanel.Parent = groupBoxDriverSkill;
                Ppanel.Location = new Point(pSkillsNamelOffset, 17 + (pSkillsNameHeight + pSkillsNameOffset) * i);
                Ppanel.BorderStyle = BorderStyle.None;
                Ppanel.Size = new Size(pSkillsNameWidth, pSkillsNameHeight);
                Ppanel.Name = "profileSkillsPanel" + i.ToString();

                Bitmap bgimg = new Bitmap(MainForm.SkillImgS[i], pSkillsNameHeight, pSkillsNameWidth);
                Ppanel.BackgroundImage = bgimg;

                Label slabel = new Label();
                groupBoxDriverSkill.Controls.Add(slabel);
                slabel.Name = "labelProfileSkill" + i.ToString();
                slabel.Location = new Point(pSkillsNamelOffset * 2 + pSkillsNameWidth, 17 + (pSkillsNameHeight + pSkillsNameOffset) * i);
                slabel.AutoSize = true;
            }

            int bADRHeight = 48, bADRWidth = 48, pOffset = 6, lOffset = pSkillsNameWidth + pSkillsNamelOffset * 2;

            for (int i = 0; i < 6; i++)
            {
                CheckBox Ppanel = new CheckBox();
                groupBoxDriverSkill.Controls.Add(Ppanel);

                Ppanel.Parent = groupBoxDriverSkill;

                Ppanel.Location = new Point(lOffset + (bADRWidth + pOffset) * i, 17 + 14);
                Ppanel.Appearance = Appearance.Button;
                Ppanel.FlatStyle = FlatStyle.Flat;
                Ppanel.Size = new Size(bADRWidth, bADRHeight);
                Ppanel.Name = "buttonADR" + i.ToString();
                Ppanel.Checked = false;
                Ppanel.Padding = new Padding(0, 0, 1, 2);
                Ppanel.BackgroundImageLayout = ImageLayout.Stretch;

                Ppanel.BackgroundImage = MainForm.SkillImgSBG[0];
                Ppanel.Image = MainForm.ADRImgSGrey[i];
                Ppanel.FlatAppearance.BorderSize = 0;

                Ppanel.MouseEnter += new EventHandler(ADRbutton_MouseEnter);
                Ppanel.MouseLeave += new EventHandler(ADRbutton_MouseLeave);
                Ppanel.Click += new EventHandler(ADRbutton_Click);
                Ppanel.CheckedChanged += new EventHandler(ADRbutton_CheckedChanged);
                Ppanel.MouseHover += new EventHandler(ADRbutton_MouseHover);

                ADRbuttonArray[i] = Ppanel;
            }

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    CheckBox Ppanel = new CheckBox();
                    groupBoxDriverSkill.Controls.Add(Ppanel);

                    Ppanel.Parent = groupBoxDriverSkill;

                    Ppanel.Location = new Point(lOffset + (bADRWidth + pOffset) * j, 17 + 14 + (pSkillsNameHeight + pSkillsNameOffset) * (i + 1));
                    Ppanel.Appearance = Appearance.Button;
                    Ppanel.FlatStyle = FlatStyle.Flat;
                    Ppanel.Size = new Size(bADRWidth, bADRHeight);
                    Ppanel.Name = "buttonSkill" + i.ToString() + j.ToString();
                    Ppanel.Checked = false;
                    Ppanel.Padding = new Padding(0, 0, 1, 2);
                    Ppanel.BackgroundImageLayout = ImageLayout.Zoom;

                    Ppanel.BackgroundImage = MainForm.SkillImgSBG[0];
                    Ppanel.FlatAppearance.BorderSize = 0;

                    Ppanel.MouseEnter += new EventHandler(Skillbutton_MouseEnter);
                    Ppanel.MouseLeave += new EventHandler(Skillbutton_MouseLeave);
                    Ppanel.Click += new EventHandler(Skillbutton_Click);
                    Ppanel.CheckedChanged += new EventHandler(Skillbutton_CheckedChanged);

                    SkillButtonArray[i, j] = Ppanel;
                }
            }
        }

        private void FillFormProfileControls()
        {
            char[] ADR = Convert.ToString(driverData.adr, 2).PadLeft(6, '0').ToCharArray();

            for (int i = 0; i < 6; i++)
            {
                if (driverData.getADRbit(i))
                    ADRbuttonArray[i].Checked = true;
            }

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < driverData.playerSkills[i + 1]; j++)
                {
                    SkillButtonArray[i, j].Checked = true;
                }
            }
        }

        //Skill buttons
        private void Skillbutton_Click(object sender, EventArgs e)
        {
            CheckBox thisbutton = sender as CheckBox;

            int skillIndex = int.Parse(thisbutton.Name.Substring(11, 1));
            byte buttonIndex = byte.Parse(thisbutton.Name.Substring(12, 1));

            byte[] tmp = driverData.playerSkills;

            if (thisbutton.Checked)
            {
                for (int j = 0; j < buttonIndex; j++)
                    SkillButtonArray[skillIndex, j].Checked = true;

                tmp[++skillIndex] = ++buttonIndex;
            }
            else
            {
                for (int j = 5; j >= buttonIndex; j--)
                    SkillButtonArray[skillIndex, j].Checked = false;

                tmp[++skillIndex] = buttonIndex;
            }

            driverData.playerSkills = tmp;
        }

        private void Skillbutton_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox thisbutton = sender as CheckBox;

            if (thisbutton.Checked)
                thisbutton.BackgroundImage = MainForm.SkillImgSBG[3];
            else
                thisbutton.BackgroundImage = MainForm.SkillImgSBG[0];
        }

        private void Skillbutton_MouseEnter(object sender, EventArgs e)
        {
            CheckBox thisbutton = sender as CheckBox;

            int skillIndex = int.Parse(thisbutton.Name.Substring(11, 1));
            byte buttonIndex = byte.Parse(thisbutton.Name.Substring(12, 1));

            for (int j = 0; j <= buttonIndex; j++)
            {
                if (!SkillButtonArray[skillIndex, j].Checked)
                    SkillButtonArray[skillIndex, j].BackgroundImage = MainForm.SkillImgSBG[1];
            }

            for (int j = buttonIndex; j < 6; j++)
            {
                if (SkillButtonArray[skillIndex, j].Checked)
                    SkillButtonArray[skillIndex, j].BackgroundImage = MainForm.SkillImgSBG[0];
            }
        }

        private void Skillbutton_MouseLeave(object sender, EventArgs e)
        {
            CheckBox thisbutton = sender as CheckBox;


            int i = int.Parse(thisbutton.Name.Substring(11, 1));

            for (int j = 0; j <= int.Parse(thisbutton.Name.Substring(12, 1)); j++)
            {
                if (!SkillButtonArray[i, j].Checked)
                {
                    SkillButtonArray[i, j].BackgroundImage = MainForm.SkillImgSBG[0];
                }
            }

            for (int j = int.Parse(thisbutton.Name.Substring(12, 1)); j < 6; j++)
            {
                if (SkillButtonArray[i, j].Checked)
                {
                    SkillButtonArray[i, j].BackgroundImage = MainForm.SkillImgSBG[3];
                }

            }
        }

        private void ADRbutton_Click(object sender, EventArgs e)
        {
            CheckBox thisbutton = sender as CheckBox;

            byte adrIndex = byte.Parse(thisbutton.Name.Substring(9, 1));

            if (thisbutton.Checked)
                driverData.setADRbit(adrIndex, true);
            else
                driverData.setADRbit(adrIndex, false);

            thisbutton.BackgroundImage = MainForm.SkillImgSBG[1];
        }

        private void ADRbutton_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox thisbutton = sender as CheckBox;

            byte adrIndex = byte.Parse(thisbutton.Name.Substring(9, 1));

            if (thisbutton.Checked)
            {
                thisbutton.BackgroundImage = MainForm.SkillImgSBG[3];
                thisbutton.Image = MainForm.ADRImgS[adrIndex];
            }
            else
            {
                thisbutton.BackgroundImage = MainForm.SkillImgSBG[0];
                thisbutton.Image = MainForm.ADRImgSGrey[adrIndex];
            }
        }

        private void ADRbutton_MouseEnter(object sender, EventArgs e)
        {
            CheckBox thisbutton = sender as CheckBox;

            thisbutton.BackgroundImage = MainForm.SkillImgSBG[1];
        }

        private void ADRbutton_MouseHover(object sender, EventArgs e)
        {
            CheckBox thisbutton = sender as CheckBox;

            byte adrIndex = byte.Parse(thisbutton.Name.Substring(9, 1));

            if (thisbutton.Checked)
            {
                thisbutton.BackgroundImage = MainForm.SkillImgSBG[1];
                thisbutton.Image = MainForm.ADRImgS[adrIndex];
            }
            else
            {
                thisbutton.BackgroundImage = MainForm.SkillImgSBG[1];
                thisbutton.Image = MainForm.ADRImgSGrey[adrIndex];
            }
        }

        private void ADRbutton_MouseLeave(object sender, EventArgs e)
        {
            CheckBox thisbutton = sender as CheckBox;

            if (thisbutton.Checked)
                thisbutton.BackgroundImage = MainForm.SkillImgSBG[3];
            else
                thisbutton.BackgroundImage = MainForm.SkillImgSBG[0];
        }

        //end Skill buttons
    }
}
