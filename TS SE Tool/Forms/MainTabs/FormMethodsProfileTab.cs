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
using System.Drawing;
using System.Windows.Forms;

namespace TS_SE_Tool
{
    public partial class FormMain
    {
        //Profile tab
        private void CreateProfilePanelControls()
        {
            int pSkillsNameHeight = 64, pSkillsNameWidth = 64, pSkillsNameOffset = 5, pSkillsNamelOffset = 12;

            string[] toolskillimgtooltip = new string[] { "ADR", "Long Distance", "High Value Cargo", "Fragile Cargo", "Just-In-Time Delivery", "Ecodriving" };

            for (int i = 0; i < 6; i++)
            {
                Panel Ppanel = new Panel();
                groupBoxProfileSkill.Controls.Add(Ppanel);

                Ppanel.Parent = groupBoxProfileSkill;
                Ppanel.Location = new Point(pSkillsNamelOffset, 17 + (pSkillsNameHeight + pSkillsNameOffset) * i);
                Ppanel.BorderStyle = BorderStyle.None;
                Ppanel.Size = new Size(pSkillsNameWidth, pSkillsNameHeight);
                Ppanel.Name = "profileSkillsPanel" + i.ToString();
                toolTipMain.SetToolTip(Ppanel, toolskillimgtooltip[i]);

                Bitmap bgimg = new Bitmap(SkillImgS[i], pSkillsNameHeight, pSkillsNameWidth);
                Ppanel.BackgroundImage = bgimg;

                Label slabel = new Label();
                groupBoxProfileSkill.Controls.Add(slabel);
                slabel.Name = "labelProfileSkill" + i.ToString() + "Name";
                slabel.Location = new Point(pSkillsNamelOffset * 2 + pSkillsNameWidth, 17 + (pSkillsNameHeight + pSkillsNameOffset) * i);
                slabel.Text = toolskillimgtooltip[i];
                slabel.AutoSize = true;
            }

            int bADRHeight = 48, bADRWidth = 48, pOffset = 6, lOffset = pSkillsNameWidth + pSkillsNamelOffset * 2;

            for (int i = 0; i < 6; i++)
            {
                CheckBox Ppanel = new CheckBox();
                groupBoxProfileSkill.Controls.Add(Ppanel);

                Ppanel.Parent = groupBoxProfileSkill;

                Ppanel.Location = new Point(lOffset + (bADRWidth + pOffset) * i, 17 + 14);
                Ppanel.Appearance = Appearance.Button;
                Ppanel.FlatStyle = FlatStyle.Flat;
                Ppanel.Size = new Size(bADRWidth, bADRHeight);
                Ppanel.Name = "buttonADR" + i.ToString();
                Ppanel.Checked = false;
                Ppanel.Padding = new Padding(0, 0, 1, 2);
                Ppanel.BackgroundImageLayout = ImageLayout.Stretch;


                Ppanel.BackgroundImage = SkillImgSBG[0];
                Ppanel.Image = ADRImgSGrey[i];
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
                    groupBoxProfileSkill.Controls.Add(Ppanel);

                    Ppanel.Parent = groupBoxProfileSkill;

                    Ppanel.Location = new Point(lOffset + (bADRWidth + pOffset) * j, 17 + 14 + (pSkillsNameHeight + pSkillsNameOffset) * (i + 1));
                    Ppanel.Appearance = Appearance.Button;
                    Ppanel.FlatStyle = FlatStyle.Flat;
                    Ppanel.Size = new Size(bADRWidth, bADRHeight);
                    Ppanel.Name = "buttonSkill" + i.ToString() + j.ToString();
                    Ppanel.Checked = false;
                    Ppanel.Padding = new Padding(0, 0, 1, 2);
                    Ppanel.BackgroundImageLayout = ImageLayout.Zoom;

                    Ppanel.BackgroundImage = SkillImgSBG[0];
                    Ppanel.FlatAppearance.BorderSize = 0;

                    Ppanel.MouseEnter += new EventHandler(Skillbutton_MouseEnter);
                    Ppanel.MouseLeave += new EventHandler(Skillbutton_MouseLeave);
                    Ppanel.Click += new EventHandler(Skillbutton_Click);
                    Ppanel.CheckedChanged += new EventHandler(Skillbutton_CheckedChanged);

                    SkillButtonArray[i, j] = Ppanel;
                }
            }

            CreateUserColorsButtons();
        }

        private void FillFormProfileControls()
        {
            FormUpdatePlayerLevel();

            char[] ADR = Convert.ToString(PlayerDataData.PlayerSkills[0], 2).PadLeft(6, '0').ToCharArray();

            for (int i = 0; i < ADR.Length; i++)
            {
                if (ADR[i] == '1')
                    ADRbuttonArray[i].Checked = true;
            }

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < PlayerDataData.PlayerSkills[i + 1]; j++)
                {
                    SkillButtonArray[i, j].Checked = true;
                }
            }

            UpdateUserColorsButtons();
        }

        private void FormUpdatePlayerLevel()
        {
            int playerlvl = PlayerDataData.getPlayerLvl()[0];
            labelPlayerLevelNumber.Text = playerlvl.ToString();

            for (int i = PlayerLevelNames.Count - 1; i >= 0; i--)
                if (PlayerLevelNames[i].LevelLimit <= playerlvl)
                {
                    labelPlayerLevelName.Text = PlayerLevelNames[i].LevelName;
                    panelPlayerLevel.BackColor = PlayerLevelNames[i].NameColor;
                    break;
                }

            labelPlayerExperience.Text = PlayerDataData.ExperiencePoints.ToString();
            labelExperienceNxtLvlThreshhold.Text = "/   " + PlayerDataData.getPlayerLvl()[1].ToString();
        }

        private void CreateUserColorsButtons()
        {
            int padding = 6, width = 72, height = 48, colorcount = 8;
            int usableSpace = groupBoxProfileUserColors.Bounds.Width;

            for (int i = 0; i < colorcount; i++)
            {
                Button rb = new Button();
                rb.Name = "buttonUC" + i.ToString();
                rb.Text = null;
                rb.Location = new Point((usableSpace - width) / 2, 32 + (padding + height) * i);
                rb.Size = new Size(width, height);
                rb.FlatStyle = FlatStyle.Flat;
                rb.Enabled = false;

                rb.Click += new EventHandler(SelectColor);

                groupBoxProfileUserColors.Controls.Add(rb);
            }
        }

        private void buttonProfileShareColors_Click(object sender, EventArgs e)
        {
            FormShareUserColors FormWindow = new FormShareUserColors();
            FormWindow.ShowDialog();
            UpdateUserColorsButtons();
        }

        private void UpdateUserColorsButtons()
        {
            int padding = 6, width = 23;

            for (int i = 0; i < UserColorsList.Count; i++)
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
                    btn.Click += new EventHandler(SelectColor);

                    groupBoxProfileUserColors.Controls.Add(btn);
                }

                if (btn != null)
                {
                    btn.Enabled = true;
                    if (UserColorsList[i].A == 0)
                    {
                        btn.Text = "X";
                        btn.BackColor = Color.FromName("Control");
                    }
                    else
                    {
                        btn.Text = "";
                        btn.BackColor = UserColorsList[i];
                    }
                }
            }
        }

        internal void SelectColor(object sender, EventArgs e)
        {
            Button obj = sender as Button;
            Color BC = obj.BackColor;
            if (obj.Text != "")
                BC = Color.White;

            OpenPainter.ColorPicker.FormColorPicker frm = new OpenPainter.ColorPicker.FormColorPicker(BC);
            frm.Font = SystemFonts.DialogFont;

            if (frm.ShowDialog() == DialogResult.OK)
            {
                int index = int.Parse(obj.Name.Substring(8, 1));

                UserColorsList[index] = frm.PrimaryColor;

                if (frm.PrimaryColor.A != 0)
                {
                    obj.Text = "";
                    obj.BackColor = frm.PrimaryColor;
                }
                else
                {
                    obj.Text = "X";
                    obj.BackColor = Color.FromName("Control");
                }
            }
        }

        //Profile buttons
        private void buttonPlayerLvlPlus01_Click(object sender, EventArgs e)
        {
            PlayerDataData.getPlayerExp(int.Parse(labelPlayerLevelNumber.Text) + 1);

            FormUpdatePlayerLevel();
        }

        private void buttonPlayerLvlPlus10_Click(object sender, EventArgs e)
        {
            PlayerDataData.getPlayerExp(int.Parse(labelPlayerLevelNumber.Text) + 10);

            FormUpdatePlayerLevel();
        }

        private void buttonPlayerLvlMax_Click(object sender, EventArgs e)
        {
            PlayerDataData.getPlayerExp(150);

            FormUpdatePlayerLevel();
        }

        private void buttonPlayerLvlMinus01_Click(object sender, EventArgs e)
        {
            PlayerDataData.getPlayerExp(int.Parse(labelPlayerLevelNumber.Text) - 1);

            FormUpdatePlayerLevel();
        }

        private void buttonPlayerLvlMinus10_Click(object sender, EventArgs e)
        {
            PlayerDataData.getPlayerExp(int.Parse(labelPlayerLevelNumber.Text) - 10);

            FormUpdatePlayerLevel();
        }

        private void buttonPlayerLvlMin_Click(object sender, EventArgs e)
        {
            PlayerDataData.getPlayerExp(0);

            FormUpdatePlayerLevel();
        }
        // end profile buttons

        //Skill buttons
        private void Skillbutton_Click(object sender, EventArgs e)
        {
            CheckBox thisbutton = sender as CheckBox;

            int skillIndex = int.Parse(thisbutton.Name.Substring(11, 1));
            byte buttonIndex = byte.Parse(thisbutton.Name.Substring(12, 1));
            if (thisbutton.Checked)
            {
                for (int j = 0; j < buttonIndex; j++)
                {
                    SkillButtonArray[skillIndex, j].Checked = true;
                }
                PlayerDataData.PlayerSkills[++skillIndex] = ++buttonIndex;
            }
            else
            {
                for (int j = 5; j >= int.Parse(thisbutton.Name.Substring(12, 1)); j--)
                {
                    SkillButtonArray[skillIndex, j].Checked = false;
                }
                PlayerDataData.PlayerSkills[++skillIndex] = buttonIndex;
            }
        }

        private void Skillbutton_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox thisbutton = sender as CheckBox;

            if (thisbutton.Checked)
            {
                thisbutton.BackgroundImage = SkillImgSBG[3];
            }
            else
            {
                thisbutton.BackgroundImage = SkillImgSBG[0];
            }
        }

        private void Skillbutton_MouseEnter(object sender, EventArgs e)
        {
            CheckBox thisbutton = sender as CheckBox;

            int i = int.Parse(thisbutton.Name.Substring(11, 1));

            for (int j = 0; j <= int.Parse(thisbutton.Name.Substring(12, 1)); j++)
            {
                if (!SkillButtonArray[i, j].Checked)
                {
                    SkillButtonArray[i, j].BackgroundImage = SkillImgSBG[1];
                }

            }

            for (int j = int.Parse(thisbutton.Name.Substring(12, 1)); j < 6; j++)
            {
                if (SkillButtonArray[i, j].Checked)
                {
                    SkillButtonArray[i, j].BackgroundImage = SkillImgSBG[0];
                }

            }
            //thisbutton.BackgroundImage = SkillImgSBG[1];
        }

        private void Skillbutton_MouseLeave(object sender, EventArgs e)
        {
            CheckBox thisbutton = sender as CheckBox;
            int i = int.Parse(thisbutton.Name.Substring(11, 1));

            for (int j = 0; j <= int.Parse(thisbutton.Name.Substring(12, 1)); j++)
            {
                if (!SkillButtonArray[i, j].Checked)
                {
                    SkillButtonArray[i, j].BackgroundImage = SkillImgSBG[0];
                }
            }

            for (int j = int.Parse(thisbutton.Name.Substring(12, 1)); j < 6; j++)
            {
                if (SkillButtonArray[i, j].Checked)
                {
                    SkillButtonArray[i, j].BackgroundImage = SkillImgSBG[3];
                }

            }
        }

        private void ADRbutton_Click(object sender, EventArgs e)
        {
            CheckBox thisbutton = sender as CheckBox;

            if (thisbutton.Checked)
            {
                char[] ADR = Convert.ToString(PlayerDataData.PlayerSkills[0], 2).PadLeft(6, '0').ToCharArray();
                ADR[byte.Parse(thisbutton.Name.Substring(9, 1))] = '1';

                PlayerDataData.PlayerSkills[0] = Convert.ToByte(new string(ADR), 2);
                thisbutton.BackgroundImage = SkillImgSBG[1];
            }
            else
            {
                char[] ADR = Convert.ToString(PlayerDataData.PlayerSkills[0], 2).PadLeft(6, '0').ToCharArray();
                ADR[byte.Parse(thisbutton.Name.Substring(9, 1))] = '0';
                string temp = new string(ADR);
                PlayerDataData.PlayerSkills[0] = Convert.ToByte(temp.PadLeft(8, '0'), 2);
                thisbutton.BackgroundImage = SkillImgSBG[1];
            }
        }

        private void ADRbutton_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox thisbutton = sender as CheckBox;

            if (thisbutton.Checked)
            {
                thisbutton.BackgroundImage = SkillImgSBG[3];
                thisbutton.Image = ADRImgS[int.Parse(thisbutton.Name.Substring(9))];
            }
            else
            {
                thisbutton.BackgroundImage = SkillImgSBG[0];
                thisbutton.Image = ADRImgSGrey[int.Parse(thisbutton.Name.Substring(9))];
            }
        }

        private void ADRbutton_MouseEnter(object sender, EventArgs e)
        {
            CheckBox thisbutton = sender as CheckBox;

            thisbutton.BackgroundImage = SkillImgSBG[1];
        }

        private void ADRbutton_MouseHover(object sender, EventArgs e)
        {
            CheckBox thisbutton = sender as CheckBox;

            if (thisbutton.Checked)
            {
                thisbutton.BackgroundImage = SkillImgSBG[1];
                thisbutton.Image = ADRImgS[int.Parse(thisbutton.Name.Substring(9))];
            }
            else
            {
                thisbutton.BackgroundImage = SkillImgSBG[1];
                thisbutton.Image = ADRImgSGrey[int.Parse(thisbutton.Name.Substring(9))];
            }
        }

        private void ADRbutton_MouseLeave(object sender, EventArgs e)
        {
            CheckBox thisbutton = sender as CheckBox;
            if (thisbutton.Checked)
                thisbutton.BackgroundImage = SkillImgSBG[3];
            else
                thisbutton.BackgroundImage = SkillImgSBG[0];
        }

        //end Skill buttons
        //end Profile tab
    }
}