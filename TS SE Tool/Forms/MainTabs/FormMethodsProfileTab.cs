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
            
            for (int i = 0; i < 6; i++)
            {
                Panel Ppanel = new Panel();
                groupBoxProfileSkill.Controls.Add(Ppanel);

                Ppanel.Parent = groupBoxProfileSkill;
                Ppanel.Location = new Point(pSkillsNamelOffset, 17 + (pSkillsNameHeight + pSkillsNameOffset) * i);
                Ppanel.BorderStyle = BorderStyle.None;
                Ppanel.Size = new Size(pSkillsNameWidth, pSkillsNameHeight);
                Ppanel.Name = "profileSkillsPanel" + i.ToString();

                Bitmap bgimg = new Bitmap(SkillImgS[i], pSkillsNameHeight, pSkillsNameWidth);
                Ppanel.BackgroundImage = bgimg;

                Label slabel = new Label();
                groupBoxProfileSkill.Controls.Add(slabel);
                slabel.Name = "labelProfileSkill" + i.ToString();
                slabel.Location = new Point(pSkillsNamelOffset * 2 + pSkillsNameWidth, 17 + (pSkillsNameHeight + pSkillsNameOffset) * i);
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
        }

        private void FillFormProfileControls()
        {
            FormUpdatePlayerLevel();

            char[] ADR = Convert.ToString(Economy.adr, 2).PadLeft(6, '0').ToCharArray();

            for (int i = 0; i < ADR.Length; i++)
            {
                if (ADR[i] == '1')
                    ADRbuttonArray[i].Checked = true;
            }

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < Economy.playerSkills[i + 1]; j++)
                {
                    SkillButtonArray[i, j].Checked = true;
                }
            }

            panelProfileUserColors.VerticalScroll.Value = 0;
            panelProfileUserColors.AutoScrollPosition = new Point(0, 0);

            CreateUserColorsButtons();

            UpdateUserColorsButtons();
        }

        private void FormUpdatePlayerLevel()
        {
            int[] playerLvlPlus = Economy.getPlayerLvl();
            int playerlvl = playerLvlPlus[0];

            for (int i = PlayerLevelNames.Count - 1; i >= 0; i--)
                if (PlayerLevelNames[i].LevelLimit <= playerlvl)
                {
                    labelPlayerLevelName.Text = PlayerLevelNames[i].LevelName;
                    panelPlayerLevel.BackColor = PlayerLevelNames[i].NameColor;
                    break;
                }

            labelPlayerLevelNumber.Text = playerlvl.ToString();

            labelPlayerExperience.Text = Economy.experience_points.ToString();
            labelExperienceNxtLvlThreshhold.Text = "/   " + playerLvlPlus[1].ToString();
        }

        private void CreateUserColorsButtons()
        {
            int padding = 3, width = 24, height = 24, spacing = 4;
            int usableSpace = groupBoxProfileUserColors.Bounds.Width;
                        
            if (MainSaveFileInfoData.Version >= 49)
            {
                int ucc = Economy.user_colors.Count / 4;
                int btnNumber = 0;

                for (int i = 0; i < ucc; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        Button rb = new Button();
                        rb.Name = "buttonUC" + btnNumber.ToString();
                        rb.Text = null;

                        int x = padding + j * width + spacing * j;
                        int y = (spacing + height) * (i);

                        rb.Location = new Point(padding + j * width + spacing * j, (spacing + height) * i);
                        rb.Size = new Size(width, height);
                        rb.FlatStyle = FlatStyle.Flat;
                        rb.Enabled = false;

                        rb.Click += new EventHandler(SelectColor);

                        panelProfileUserColors.Controls.Add(rb);
                        btnNumber++;
                    }
                }

                tableLayoutPanelUserColors.RowStyles[1].Height = 50;

                if (ucc >= 40)
                    buttonAddUserColor.Enabled = false;
                else
                    buttonAddUserColor.Enabled = true;

                panelProfileUserColors.VerticalScroll.Maximum = 1500; //Virtual height
                panelProfileUserColors.MouseWheel += new MouseEventHandler(this.panelProfileUserColors_MouseWheel);
            }
            else
                for (int i = 0; i < Economy.user_colors.Count; i++)
                {
                    Button rb = new Button();
                    rb.Name = "buttonUC" + i.ToString();
                    rb.Text = null;
                    rb.Location = new Point((usableSpace - width * 3) / 2, 8 + (padding * 2 + height * 2) * i);
                    rb.Size = new Size(width * 3, height * 2);
                    rb.FlatStyle = FlatStyle.Flat;
                    rb.Enabled = false;

                    rb.Click += new EventHandler(SelectColor);

                    panelProfileUserColors.Controls.Add(rb);
                }
        }

        private void panelProfileUserColors_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta != 0)
            {
                int scroll = 28;
                int location = Math.Abs(panelProfileUserColors.AutoScrollPosition.Y);

                if (e.Delta < 0)
                {
                    if (location + scroll < panelProfileUserColors.VerticalScroll.Maximum)
                    {
                        location += scroll;
                        panelProfileUserColors.VerticalScroll.Value = location;
                    }
                    else
                    {
                        location = panelProfileUserColors.VerticalScroll.Maximum;
                        panelProfileUserColors.AutoScrollPosition = new Point(0, location);
                    }
                }
                else
                {
                    if (location - scroll > 0)
                    {
                        location -= scroll;
                        panelProfileUserColors.VerticalScroll.Value = location;
                    }
                    else
                    {
                        location = 0;
                        panelProfileUserColors.AutoScrollPosition = new Point(0, location);
                    }
                }
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
            if (MainSaveFileInfoData.Version >= 49)
            {
                int padding = 3, width = 24, height = 24, spacing = 4;

                panelProfileUserColors.VerticalScroll.Maximum = (height + spacing) * Economy.user_colors.Count / 4;

                for (int i = 0; i < Economy.user_colors.Count; i++)
                {
                    Button btn = null;
                    string btnname = "buttonUC" + i.ToString();

                    if (panelProfileUserColors.Controls.ContainsKey(btnname))
                    {
                        btn = panelProfileUserColors.Controls[btnname] as Button;

                        btn.Enabled = true;

                        if (Economy.user_colors[i].color.A == 0)
                        {
                            btn.Text = "X";
                            btn.BackColor = Color.FromName("Control");
                        }
                        else
                        {
                            btn.Text = "";
                            btn.BackColor = Economy.user_colors[i].color;
                        }
                    }
                    else
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            btn = new Button();
                            btn.Name = "buttonUC" + i.ToString();
                            btn.Text = null;

                            int x = padding + j * width + spacing * j;
                            int y = (spacing + height) * (i / 4);

                            btn.Location = new Point(x, y);
                            btn.Size = new Size(width, height);
                            btn.FlatStyle = FlatStyle.Flat;
                            btn.Enabled = false;

                            btn.Click += new EventHandler(SelectColor);

                            panelProfileUserColors.Controls.Add(btn);

                            btn.Enabled = true;

                            if (Economy.user_colors[i].color.A == 0)
                            {
                                btn.Text = "X";
                                btn.BackColor = Color.FromName("Control");
                            }
                            else
                            {
                                btn.Text = "";
                                btn.BackColor = Economy.user_colors[i].color;
                            }
                            i++;
                        }
                        i--;
                    }
                }                
            }
            else
            {
                int padding = 6, width = 23;

                for (int i = 0; i < Economy.user_colors.Count; i++)
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
                        btn.Click += new EventHandler(SelectColor);

                        panelProfileUserColors.Controls.Add(btn);
                    }

                    if (btn != null)
                    {
                        btn.Enabled = true;
                        if (Economy.user_colors[i].color.A == 0)
                        {
                            btn.Text = "X";
                            btn.BackColor = Color.FromName("Control");
                        }
                        else
                        {
                            btn.Text = "";
                            btn.BackColor = Economy.user_colors[i].color;
                        }
                    }
                }
            }
        }

        private void DeleteUserColorsButtons()
        {
            short counter = 0;
            do
            {
                try
                {
                    Control[] tempArray = panelProfileUserColors.Controls.Find("buttonUC" + counter, false);

                    if (tempArray.Length > 0)
                    {
                        panelProfileUserColors.Controls.Remove(tempArray[0]);
                        counter++;
                    }
                    else
                    {
                        break;
                    }
                }
                catch
                {
                    break;
                }

            } while (true);
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
                int index = int.Parse(obj.Name.Substring(8));

                Economy.user_colors[index].color = frm.PrimaryColor;

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

            if (MainSaveFileInfoData.Version >= 49)
                RemoveUserColorUnused4slot();
        }

        private void buttonAddUserColor_Click(object sender, EventArgs e)
        {
            AddUserColor4slot();
        }

        internal void AddUserColor4slot()
        {
            for (int i = 0; i < 4; i++)
            {
                Economy.user_colors.Add(new Save.DataFormat.SCS_Color(0, 0, 0, 0));
            }

            //Scroll panel to the top to properly add buttons
            panelProfileUserColors.AutoScrollPosition = new Point(0, 0);

            UpdateUserColorsButtons();


            //Return to top position
            int location = panelProfileUserColors.VerticalScroll.Maximum - panelProfileUserColors.Height;

            if (location > 0)
            {
                panelProfileUserColors.AutoScrollPosition = new Point(0, location);
                panelProfileUserColors.VerticalScroll.Value = location;
            }
        }

        internal void RemoveUserColorUnused4slot()
        {
            int counter = Economy.user_colors.Count - 1;
            int slotCount = Economy.user_colors.Count / 4;

            try
            {
                for (int iCol = 0; iCol < slotCount; iCol++)
                {
                    bool delete = false;

                    for (int i = 0; i < 4; i++)
                    {
                        if (Economy.user_colors[counter - i].color.A == 0)
                        {
                            delete = true;
                        }
                        else
                        {
                            delete = false;
                            break;
                        }
                    }

                    counter = counter - 4;

                    if (delete)
                    {
                        for (int i = 4; i > 0; i--)
                        {
                            int btnNumber = counter + i;

                            Control[] tempArray = panelProfileUserColors.Controls.Find("buttonUC" + btnNumber.ToString(), false);

                            if (tempArray.Length > 0)
                            {
                                panelProfileUserColors.Controls.Remove(tempArray[0]);
                                Economy.user_colors.RemoveAt(btnNumber);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                if (Economy.user_colors.Count / 4 >= 40)
                    buttonAddUserColor.Enabled = false;
                else
                    buttonAddUserColor.Enabled = true;

            }
            catch
            { }

            //Scroll panel to the top to properly add buttons
            panelProfileUserColors.AutoScrollPosition = new Point(0, 0);

            UpdateUserColorsButtons();

            //Return to top position
            int location = panelProfileUserColors.VerticalScroll.Maximum - panelProfileUserColors.Height;

            if (location > 0)
            {
                panelProfileUserColors.AutoScrollPosition = new Point(0, location);
                panelProfileUserColors.VerticalScroll.Value = location;
            }
        }

        //Profile buttons
        private void buttonPlayerLvlPlus01_Click(object sender, EventArgs e)
        {
            Economy.setPlayerExp(int.Parse(labelPlayerLevelNumber.Text) + 1);

            FormUpdatePlayerLevel();
        }

        private void buttonPlayerLvlPlus10_Click(object sender, EventArgs e)
        {
            Economy.setPlayerExp(int.Parse(labelPlayerLevelNumber.Text) + 10);

            FormUpdatePlayerLevel();
        }

        private void buttonPlayerLvlMax_Click(object sender, EventArgs e)
        {
            Economy.setPlayerExp(150);

            FormUpdatePlayerLevel();
        }

        private void buttonPlayerLvlMinus01_Click(object sender, EventArgs e)
        {
            Economy.setPlayerExp(int.Parse(labelPlayerLevelNumber.Text) - 1);

            FormUpdatePlayerLevel();
        }

        private void buttonPlayerLvlMinus10_Click(object sender, EventArgs e)
        {
            Economy.setPlayerExp(int.Parse(labelPlayerLevelNumber.Text) - 10);

            FormUpdatePlayerLevel();
        }

        private void buttonPlayerLvlMin_Click(object sender, EventArgs e)
        {
            Economy.setPlayerExp(0);

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
                    SkillButtonArray[skillIndex, j].Checked = true;
                
                Economy.playerSkills[++skillIndex] = ++buttonIndex;
            }
            else
            {
                for (int j = 5; j >= buttonIndex; j--)                
                    SkillButtonArray[skillIndex, j].Checked = false;
                
                Economy.playerSkills[++skillIndex] = buttonIndex;
            }
        }

        private void Skillbutton_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox thisbutton = sender as CheckBox;

            if (thisbutton.Checked)            
                thisbutton.BackgroundImage = SkillImgSBG[3];            
            else            
                thisbutton.BackgroundImage = SkillImgSBG[0];
        }

        private void Skillbutton_MouseEnter(object sender, EventArgs e)
        {
            CheckBox thisbutton = sender as CheckBox;

            int skillIndex = int.Parse(thisbutton.Name.Substring(11, 1));
            byte buttonIndex = byte.Parse(thisbutton.Name.Substring(12, 1));

            for (int j = 0; j <= buttonIndex; j++)
            {
                if (!SkillButtonArray[skillIndex, j].Checked)                
                    SkillButtonArray[skillIndex, j].BackgroundImage = SkillImgSBG[1];
            }

            for (int j = buttonIndex; j < 6; j++)
            {
                if (SkillButtonArray[skillIndex, j].Checked)                
                    SkillButtonArray[skillIndex, j].BackgroundImage = SkillImgSBG[0];                
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

            byte adrIndex = byte.Parse(thisbutton.Name.Substring(9, 1));
            char[] ADR = Convert.ToString(Economy.playerSkills[0], 2).PadLeft(6, '0').ToCharArray();

            if (thisbutton.Checked)
            {
                ADR[adrIndex] = '1';

                Economy.playerSkills[0] = Convert.ToByte(new string(ADR), 2);
            }
            else
            {
                ADR[adrIndex] = '0';

                string temp = new string(ADR);

                Economy.playerSkills[0] = Convert.ToByte(temp.PadLeft(6, '0'), 2);
            }

            thisbutton.BackgroundImage = SkillImgSBG[1];
        }

        private void ADRbutton_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox thisbutton = sender as CheckBox;

            byte adrIndex = byte.Parse(thisbutton.Name.Substring(9, 1));

            if (thisbutton.Checked)
            {
                thisbutton.BackgroundImage = SkillImgSBG[3];
                thisbutton.Image = ADRImgS[adrIndex];
            }
            else
            {
                thisbutton.BackgroundImage = SkillImgSBG[0];
                thisbutton.Image = ADRImgSGrey[adrIndex];
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

            byte adrIndex = byte.Parse(thisbutton.Name.Substring(9, 1));

            if (thisbutton.Checked)
            {
                thisbutton.BackgroundImage = SkillImgSBG[1];
                thisbutton.Image = ADRImgS[adrIndex];
            }
            else
            {
                thisbutton.BackgroundImage = SkillImgSBG[1];
                thisbutton.Image = ADRImgSGrey[adrIndex];
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