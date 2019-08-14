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


            PopulateFormControlsk();
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
                Button rb = new Button();
                groupBoxUserColors.Controls.Add(rb);

                rb.Name = "buttonUC" + i.ToString();
                rb.Text = null;
                rb.Location = new Point(15 + (padding + width) * (i), 16);
                rb.Size = new Size(width, width);
                rb.FlatStyle = FlatStyle.Flat;
                rb.Enabled = false;

                rb.Click += new EventHandler(MainForm.SelectColor);
                UserColorsB[i] = rb;

                CheckBox Ppanel = new CheckBox();
                groupBoxUserColors.Controls.Add(Ppanel);
                Ppanel.Parent = groupBoxUserColors;

                //Ppanel.Appearance = Appearance.Button;
                Ppanel.FlatStyle = FlatStyle.Flat;
                Ppanel.Size = new Size(width, width / 4);
                Ppanel.Name = "checkboxUC" + i.ToString();
                Ppanel.Checked = false;
                Ppanel.Text = "";
                Ppanel.AutoSize = true;
                Ppanel.Location = new Point(15 + (48 - Ppanel.Width) / 2 + (padding + width) * (i), 24 + width);

                UserColorsCB[i] = Ppanel;
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

                if (groupBoxUserColors.Controls.ContainsKey(btnname))
                {
                    btn = groupBoxUserColors.Controls[btnname] as Button;
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

                    groupBoxUserColors.Controls.Add(btn);
                }

                if (btn != null)
                {
                    btn.Enabled = true;
                    if (MainForm.UserColorsList[i].A == 0)
                    {
                        btn.Text = "X";
                        btn.BackColor = Color.FromName("Control");
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
            foreach (CheckBox temp in UserColorsCB)
            {
                if (temp.Checked)
                {
                    tempData += "\r\n" + UserColorsB[i].BackColor.ToArgb().ToString();
                    temp.Checked = false;
                }
                i++;
            }

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
            }
        }
    }
}
