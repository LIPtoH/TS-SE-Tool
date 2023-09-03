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
using System.Drawing.Imaging;
using System.Reflection;
using System.Text.RegularExpressions;
using JR.Utils.GUI.Forms;
using TS_SE_Tool.Save.DataFormat;
using TS_SE_Tool.Utilities;

namespace TS_SE_Tool
{
    public partial class FormShareUserColors : Form
    {
        private FormMain MainForm = Application.OpenForms.OfType<FormMain>().Single();

        List<SCS_Color> userColors;

        CheckBox[] UserColorsCB;

        Panel[] ImportColorsB = new Panel[0];
        List<Color> ImportedColors = new List<Color>();

        private string nameUC = "UCpanel", nameUCc = "UCcontainer", nameUCcb = "UCcheckbox",
                       nameIC = "ICpanel", nameICc = "ICcontainer", nameICcb = "ICcheckbox",
                       newSlotName = "NewSlotUserColor";

        private readonly int width = 48, padding = 6,  offsetL = 4, offsetT = 4, lineThickness = 4;

        ushort SaveVersion = 0;

        public FormShareUserColors()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.MainIco;

            MainForm.HelpTranslateControl(this);

            MainForm.HelpTranslateFormMethod(this);

            SaveVersion = MainForm.MainSaveFileInfoData.Version;

            userColors = MainForm.SiiNunitData.Economy.user_colors.Select(x => x.Clone()).ToList();
            int colorcount = userColors.Count;

            if (SaveVersion >= 49)
                colorcount = colorcount / 4;

            UserColorsCB = new CheckBox[colorcount];

            CorrectControlsPositions();

            PopulateFormControlsk();

            buttonApply.Enabled = false;
            groupBoxImportedColors.Visible = false;

            //
            panelProfileUserColors.VerticalScroll.Enabled = false;
            panelProfileUserColors.VerticalScroll.Visible = false;
            panelProfileUserColors.VerticalScroll.Maximum = 0;
            panelProfileUserColors.HorizontalScroll.Enabled = true;
            panelProfileUserColors.AutoScroll = true;
            //
            panelImportedColors.VerticalScroll.Enabled = false;
            panelImportedColors.VerticalScroll.Visible = false;
            panelImportedColors.VerticalScroll.Maximum = 0;
            panelImportedColors.HorizontalScroll.Enabled = true;
            panelImportedColors.AutoScroll = true;
            //

            ChangeFormSize(false);
        }

        //Create controls
        private void PopulateFormControlsk()
        {
            CreateUserColorsButtons(0);
            UpdateUserColorsButtons();
        }

        private void CreateUserColorsButtons(int start)
        {
            int colorcount = userColors.Count;

            if (SaveVersion >= 49)
            {
                int ucc = userColors.Count / 4;
                int btnNumber = start * 4;
                int width2 = width / 2;

                int x = 0, y = 0;

                for (int i = start; i < ucc; i++)
                {
                    //Parent panel
                    Panel groupPanel = new Panel();

                    groupPanel.Name = nameUCc + i.ToString();

                    x = offsetL + (width + padding + offsetL) * i;
                    y = offsetT;

                    groupPanel.Location = new Point(x - panelProfileUserColors.HorizontalScroll.Value, y);
                    groupPanel.Size = new Size(width + offsetL, width + offsetT);

                    groupPanel.BorderStyle = BorderStyle.None;
                    groupPanel.BackColor = Color.Black;

                    groupPanel.Enabled = true;

                    groupPanel.DragEnter += groupPanelImport_DragEnter;
                    groupPanel.DragDrop += groupPanelImport_DragDrop;

                    groupPanel.AllowDrop = true;

                    panelProfileUserColors.Controls.Add(groupPanel);

                    //Checkboxes for export
                    CheckBox colorCB = new CheckBox();
                    colorCB.Name = nameUCcb + i.ToString();

                    colorCB.Parent = panelProfileUserColors;

                    colorCB.Size = new Size(11, 11);

                    x = (width - colorCB.Width + offsetL) / 2;
                    y = width + offsetT * 2;

                    colorCB.Location = new Point(x, y);

                    colorCB.Enabled = false;
                    colorCB.Checked = false;

                    colorCB.BackColor = Color.FromKnownColor(KnownColor.Control);
                    colorCB.FlatStyle = FlatStyle.Flat;
                    colorCB.AutoSize = false;

                    colorCB.Text = null;

                    colorCB.CheckedChanged += new System.EventHandler(this.checkBoxExport_CheckedChanged);

                    groupPanel.Controls.Add(colorCB);

                    groupPanel.Size = new Size(width + offsetL, width + offsetT * 7 / 2 + colorCB.Height);

                    UserColorsCB[i] = colorCB;

                    //Panels for colors
                    for (int j = 0; j < 4; j++)
                    {
                        int multX = 0;
                        if (j == 1 || j == 3)
                            multX = 1;

                        Panel colorP = new Panel();

                        colorP.Name = nameUC + btnNumber.ToString();

                        x = offsetL / 2 + width2 * multX;
                        y = offsetT / 2 + width2 * (j / 2);

                        colorP.Location = new Point(x, y);
                        colorP.Size = new Size(width2, width2);

                        colorP.Enabled = true;
                        colorP.BorderStyle = BorderStyle.None;

                        colorP.DragEnter += panelImport_DragEnter;
                        colorP.DragDrop += panelImport_DragDrop;

                        colorP.AllowDrop = true;

                        groupPanel.Controls.Add(colorP);

                        btnNumber++;
                    }
                }
            }
            else
            {
                for (int i = 0; i < colorcount; i++)
                {
                    Panel colorB = new Panel();

                    colorB.Name = nameUC + i.ToString();

                    colorB.Location = new Point(offsetL + i * (padding + width), offsetT);
                    colorB.Size = new Size(width, width);

                    colorB.BorderStyle = BorderStyle.None;
                    colorB.BackColor = Color.Black;

                    colorB.Enabled = false;

                    panelProfileUserColors.Controls.Add(colorB);

                    //=== CheckBox's
                    CheckBox colorCB = new CheckBox();
                    panelProfileUserColors.Controls.Add(colorCB);
                    colorCB.Parent = panelProfileUserColors;

                    colorCB.FlatStyle = FlatStyle.Flat;
                    colorCB.Size = new Size(width, width / 4);
                    colorCB.Name = nameUCcb + i.ToString();
                    colorCB.Checked = false;
                    colorCB.Text = "";
                    colorCB.AutoSize = true;
                    colorCB.Location = new Point(offsetL + (width - colorCB.Width) / 2 + i * (padding + width), offsetT + padding * 2 + width);

                    UserColorsCB[i] = colorCB;
                }
            }
        }

        private void AddNewSlotUserColors()
        {
            Image crossIMG = new Bitmap(width, width);

            using (var canvas = Graphics.FromImage(crossIMG))
            {
                Pen linePen = new Pen(Color.FromKnownColor(KnownColor.Control), lineThickness);

                canvas.DrawLine(linePen, width / 2, offsetT, width / 2, width - offsetT);
                canvas.DrawLine(linePen, offsetT, width / 2, width - offsetT, width / 2);
            }

            //Parent panel
            Panel groupPanel = new Panel();

            groupPanel.Name = newSlotName;
            int i = 0;

            if (SaveVersion >= 49)
                i = userColors.Count / 4;

            int x = offsetL + (width + padding + offsetL) * i;
            int y = offsetT * 3;

            int xOffset = panelProfileUserColors.AutoScrollPosition.X;

            groupPanel.Location = new Point(x + xOffset, y);
            groupPanel.Size = new Size(width + offsetL, width + offsetT);

            groupPanel.BorderStyle = BorderStyle.None;
            groupPanel.BackColor = Color.Black;
            groupPanel.BackgroundImageLayout = ImageLayout.Center;
            groupPanel.BackgroundImage = crossIMG;

            groupPanel.Enabled = true;

            groupPanel.DragEnter += newSlotPanelImport_DragEnter;
            groupPanel.DragDrop += newSlotPanelImport_DragDrop;

            groupPanel.AllowDrop = true;

            panelProfileUserColors.Controls.Add(groupPanel);
        }

        private void MoveNewSlotUserColors()
        {
            if (SaveVersion >= 49)
            {
                Panel nsp = (Panel)panelProfileUserColors.Controls.Find(newSlotName, true)[0];

                if (userColors.Count() / 4 < 40)
                {
                    //Move
                    nsp.Location = new Point(nsp.Location.X + (width + padding + offsetL), nsp.Location.Y);

                    panelProfileUserColors.AutoScrollPosition = new Point((panelProfileUserColors.HorizontalScroll.Maximum + 1 - panelProfileUserColors.Bounds.Width), 0);
                }
                else
                {
                    //Remove
                    panelProfileUserColors.Controls.Remove(nsp);
                }
            }
        }

        private void UpdateUserColorsButtons()
        {
            Image crossIMG;

            if (SaveVersion >= 49)            
                crossIMG = CreateCrossIMG(width / 2, width / 2, lineThickness / 2, padding);
            else
                crossIMG = CreateCrossIMG(width, width, lineThickness, padding);


            for (int i = 0; i < userColors.Count; i++)
            {
                Panel ctrl = null;
                string ctrlName = nameUC + i.ToString();

                Control[] tmp = panelProfileUserColors.Controls.Find(ctrlName, true);

                if (tmp.Length == 0)
                    continue;

                ctrl = tmp[0] as Panel;

                if (ctrl != null)
                {
                    ctrl.Enabled = true;
                    if (userColors[i].color.A == 0)
                    {
                        ctrl.BackColor = Color.FromKnownColor(KnownColor.Control);

                        ctrl.BackgroundImage = crossIMG;
                    }
                    else
                    {
                        ctrl.BackColor = userColors[i].color;

                        ctrl.BackgroundImage = null;

                        if (SaveVersion >= 49)
                            UserColorsCB[i / 4].Enabled = true;
                        else
                            UserColorsCB[i].Enabled = true;
                    }
                }
            }
        }

        //
        private void CreateImportColorsButtons(int _colorcount)
        {
            int colorcount = _colorcount, lineThickness = 4;

            int ucc = userColors.Count / 4;
            int btnNumber = 0;


            if (SaveVersion >= 49)
                colorcount = colorcount / 4;

            Array.Resize(ref ImportColorsB, colorcount);

            if (SaveVersion >= 49)
            {
                lineThickness = lineThickness / 2;
            }

            Image crossIMG = CreateCrossIMG(width / 2, width / 2, lineThickness, padding);

            int x = 0, y = 0;

            for (int i = 0; i < colorcount; i++)
            {
                //Parent panel
                Panel groupPanel = new Panel();

                groupPanel.Name = nameICc + i.ToString();

                x = offsetL + (width + padding + offsetL) * i;
                y = offsetT;

                groupPanel.Location = new Point(x, y);
                groupPanel.Size = new Size(width + offsetL, width + offsetT * 4);

                groupPanel.BorderStyle = BorderStyle.None;
                groupPanel.BackColor = Color.Black;

                groupPanel.Enabled = true;

                groupPanel.MouseDown += groupPanelImport_MouseDown;
                groupPanel.QueryContinueDrag += panelDragSource_QueryContinueDrag;

                panelImportedColors.Controls.Add(groupPanel);

                ImportColorsB[i] = groupPanel;

                //==
                if (SaveVersion >= 49)
                {
                    int width2 = width / 2;

                    for (int j = 0; j < 4; j++)
                    {
                        int multX = 0;
                        if (j == 1 || j == 3)
                            multX = 1;

                        Panel colorP = new Panel();

                        colorP.Name = nameIC + btnNumber.ToString();

                        x = offsetL / 2 + width2 * multX;
                        y = offsetT / 2 + width2 * (j / 2);

                        colorP.Location = new Point(x, y);
                        colorP.Size = new Size(width2, width2);

                        colorP.BorderStyle = BorderStyle.None;

                        if (ImportedColors[btnNumber].A == 0)
                        {
                            colorP.Enabled = false;

                            colorP.BackColor = Color.FromKnownColor(KnownColor.Control);

                            colorP.BackgroundImage = crossIMG;
                        }
                        else
                        {
                            colorP.Enabled = true;

                            colorP.BackColor = ImportedColors[btnNumber];

                            colorP.MouseDown += panelImport_MouseDown;
                            colorP.QueryContinueDrag += panelDragSource_QueryContinueDrag;
                        }

                        groupPanel.Controls.Add(colorP);

                        btnNumber++;
                    }
                }
                else
                {
                    /*
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
                    */
                }
            }
        }

        //==

        private void panelImport_MouseDown(object sender, MouseEventArgs e)
        {
            (sender as Panel).DoDragDrop((sender as Panel).BackColor, DragDropEffects.Copy | DragDropEffects.Move);
        }

        private void panelImport_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent( typeof(Color) ))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void panelImport_DragDrop(object sender, DragEventArgs e)
        {
            Color newColor = (Color)e.Data.GetData(typeof(Color));

            (sender as Panel).BackColor = newColor;
            (sender as Panel).BackgroundImage = null;

            Panel target = sender as Panel;

            if (TextUtilities.ExtractFirstNumber(target.Name, out int number))
                userColors[number].color = newColor;

            buttonApply.Enabled = true;
        }

        //==

        private void groupPanelImport_MouseDown(object sender, MouseEventArgs e)
        {
            Panel source = sender as Panel;
            List<Panel> pList = source.Controls.OfType<Panel>().ToList();

            List<Color> cList = new List<Color>();

            foreach (Panel cPanel in pList)
            {
                if (cPanel.BackgroundImage == null)
                    cList.Add(cPanel.BackColor);
                else
                    cList.Add(Color.FromArgb(0, 0, 0, 0));
            }

            (sender as Panel).DoDragDrop(cList, DragDropEffects.Copy | DragDropEffects.Move);
        }

        private void groupPanelImport_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(List<Color>)))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void groupPanelImport_DragDrop(object sender, DragEventArgs e)
        {
            List<Color> newColors = (List<Color>)e.Data.GetData(typeof(List<Color>));

            Panel source = sender as Panel;
            List<Panel> pList = source.Controls.OfType<Panel>().ToList();

            int idx = 0;

            foreach (Panel pnl in pList)
            {
                if (!TextUtilities.ExtractFirstNumber(pList[idx].Name, out int number))
                    continue;

                if (newColors[idx].A != 0)
                    userColors[number].color = (Color)newColors[idx];
                else
                    userColors[number].color = Color.FromArgb(0);

                idx++;
            }

            if (idx > 0)
                source.Controls.OfType<CheckBox>().ToList()[0].Enabled = true;

            UpdateUserColorsButtons();

            buttonApply.Enabled = true;
        }

        //==
        private void newSlotPanelImport_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(List<Color>)) || e.Data.GetDataPresent(typeof(Color)) )
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void newSlotPanelImport_DragDrop(object sender, DragEventArgs e)
        {
            //Setup
            int prev = userColors.Count();

            userColors.AddRange(new SCS_Color[4] { new SCS_Color(0), new SCS_Color(0), new SCS_Color(0), new SCS_Color(0) });

            Array.Resize(ref UserColorsCB, UserColorsCB.Count() + 1);

            //Create regular panel
            CreateUserColorsButtons(prev / 4);

            Panel nsp = (Panel)panelProfileUserColors.Controls.Find(nameUCc + prev / 4, false)[0];

            if (e.Data.GetDataPresent(typeof(Color)))
            {
                e.Data.SetData(new List<Color>() { (Color)e.Data.GetData(typeof(Color)), Color.FromArgb(0), Color.FromArgb(0), Color.FromArgb(0) });
                //Add single color
                groupPanelImport_DragDrop(nsp, e);
            }
            else
            {
                //Add colors
                groupPanelImport_DragDrop(nsp, e);
            }

            //Move new slot
            MoveNewSlotUserColors();

            buttonApply.Enabled = true;
        }

        //==

        private void panelDragSource_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            // Cancel the drag if the mouse moves off the form.
            Panel lb = sender as Panel;

            if (lb != null)
            {
                Form f = lb.FindForm();
                // The screenOffset is used to account for any desktop bands 
                // that may be at the top or left side of the screen when 
                // determining when to cancel the drag drop operation.
                Point screenOffset;
                screenOffset = SystemInformation.WorkingArea.Location;

                // Cancel the drag if the mouse moves off the form. The screenOffset
                // takes into account any desktop bands that may be at the top or left
                // side of the screen.
                if (((Control.MousePosition.X - screenOffset.X) < f.DesktopBounds.Left) ||
                    ((Control.MousePosition.X - screenOffset.X) > f.DesktopBounds.Right) ||
                    ((Control.MousePosition.Y - screenOffset.Y) < f.DesktopBounds.Top) ||
                    ((Control.MousePosition.Y - screenOffset.Y) > f.DesktopBounds.Bottom))
                {
                    e.Action = DragAction.Cancel;
                }
            }
        }

        //==
        private void checkBoxExport_CheckedChanged(object sender, EventArgs e)
        {
            foreach (CheckBox chbox in UserColorsCB)
            {
                if (chbox.Checked)
                {
                    buttonExport.Enabled = true;
                    return;
                }
            }

            buttonExport.Enabled = false;
        }

        //==

        private Image CreateCrossIMG(int _width, int _lineThickness, int _padding)
        {
            return CreateCrossIMG(_width, _width, _lineThickness, _padding);
        }

        private Image CreateCrossIMG(int _width, int _height, int _lineThickness, int _padding)
        {
            Image crossIMG = new Bitmap(_width, _height);

            using (var canvas = Graphics.FromImage(crossIMG))
            {
                Pen linePen = new Pen(Color.FromKnownColor(KnownColor.ControlText), _lineThickness);

                canvas.DrawLine(linePen, _padding, _padding, _width - _padding, _width - _padding);
                canvas.DrawLine(linePen, _width - _padding, _padding, _padding, _width - _padding);
            }

            return crossIMG;
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
                            tempData += "\r\n" + userColors[i * 4 + j].color.ToArgb().ToString();
                            temp.Checked = false;
                            ready = true;                            
                        }
                    }
                    else
                    {
                        tempData += "\r\n" + userColors[i].color.ToArgb().ToString();
                        temp.Checked = false;
                        ready = true;
                    }
                }
                i++;
            }

            if (!ready)
                return;

            string exportData = BitConverter.ToString(Utilities.ZipDataUtilities.zipText(tempData)).Replace("-", "");
            Clipboard.SetText(exportData);

            FlexibleMessageBox.Show(this, "Color data has been copied", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        private void buttonImportColors_Click(object sender, EventArgs e)
        {
            //Remove imported prev buttons and chkboxes
            if (ImportColorsB.Length > 0)
            {
                foreach (Panel t in ImportColorsB)
                {
                    t.Dispose();
                }
            }

            ImportedColors.Clear(); //Clear imorted color list
            
            //Start import
            try
            {
                //check for gz file format
                string probGZfile = Clipboard.GetText();

                if (probGZfile.Substring(0, 4).ToUpper() != "1F8B")
                    return;

                //unzip
                string inputData = Utilities.ZipDataUtilities.unzipText(probGZfile); //Get data and unzip

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
                        colorCB.Checked = false;

                    //Show imported colors section
                    ChangeFormSize(true);

                    groupBoxImportedColors.Visible = true;

                    //Enable checkboxes to enable import
                    foreach (CheckBox colorCB in UserColorsCB)
                    {
                        colorCB.Enabled = true;
                    }

                    if (SaveVersion >= 49 && userColors.Count() / 4 < 40)
                    {
                        AddNewSlotUserColors();
                    }

                    FlexibleMessageBox.Show(this, "Color data has been inserted", "Import", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
                else
                    FlexibleMessageBox.Show(this, "Expected Color data but" + Environment.NewLine + Lines[0] + Environment.NewLine + "was found.",
                    "Error. Wrong data.", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
            catch (Exception ex)
            {
                FlexibleMessageBox.Show(this, "Something gone wrong." + Environment.NewLine + Environment.NewLine + ex.Message,
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void buttonApplyChanges_Click(object sender, EventArgs e)
        {
            MainForm.SiiNunitData.Economy.user_colors = userColors;

            buttonApply.Enabled = false;
        }

        //Change form size
        private void ChangeFormSize(bool _big)
        {
            if (_big)
            {
                this.MaximumSize = new Size(this.Size.Width, 360);
                tableLayoutPanelMain.RowStyles[2] = new RowStyle(sizeType: SizeType.Percent, 50);
            }
            else
            {
                this.MaximumSize = new Size(this.Size.Width, 230);
                tableLayoutPanelMain.RowStyles[2] = new RowStyle(sizeType: SizeType.Percent, 0);
            }

            this.Size = new Size(this.Size.Width, this.Size.Height);
            this.MinimumSize = this.MaximumSize;
        }
    }
}