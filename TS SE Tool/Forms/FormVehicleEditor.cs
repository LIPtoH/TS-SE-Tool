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
    public partial class FormVehicleEditor : Form
    {
        FormMain MainForm = Application.OpenForms.OfType<FormMain>().Single();

        Save.Items.Vehicle VehicleData;
        internal Dictionary<string, dynamic> Accessories;
        internal string exportFormatString;

        internal FormVehicleEditor(Save.Items.Vehicle _data, Dictionary<string, dynamic> _accessories)
        {
            InitializeComponent();

            VehicleData = _data;
            Accessories = _accessories;

            TranslateForm();
        }

        private void TranslateForm()
        {
            MainForm.HelpTranslateControl(this);
            MainForm.HelpTranslateFormMethod(this);
        }

        private void FormVehicleEditor_Load(object sender, EventArgs e)
        {
            PopulateDataView();
            dataGridViewAccessories.Rows[0].Selected = true;
            exportFormatString = "TSSET_VehicleAccessory";
        }

        private void PopulateDataView()
        {
            foreach (KeyValuePair<string, dynamic> item in Accessories)
            {
                string accName = item.Value.GetType().Name;

                dataGridViewAccessories.Rows.Add(item.Key, GetImageForAccessoryByType(accName), item.Value, "X");
            }
        }

        private Image GetImageForAccessoryByType(string accName)
        {
            Image tmpIcon = new Bitmap(1, 1);

            switch (accName.ToLower())
            {
                case "vehicle_accessory":
                    {
                        tmpIcon = MainForm.AccessoriesImg[0];
                        break;
                    }
                case "vehicle_addon_accessory":
                    {
                        tmpIcon = MainForm.AccessoriesImg[1];
                        break;
                    }
                case "vehicle_wheel_accessory":
                    {
                        tmpIcon = MainForm.AccessoriesImg[2];
                        break;
                    }
                case "vehicle_paint_job_accessory":
                    {
                        tmpIcon = MainForm.AccessoriesImg[3];
                        break;
                    }
                case "vehicle_sound_accessory":
                    {
                        tmpIcon = MainForm.AccessoriesImg[4];
                        break;
                    }
                case "vehicle_drv_plate_accessory":
                    {
                        tmpIcon = MainForm.AccessoriesImg[5];
                        break;
                    }
                case "vehicle_cargo_accessory":
                    {
                        tmpIcon = MainForm.TrailerPartsImg[0];
                        break;
                    }
            }

            return tmpIcon;
        }

        //Accessories list
        private void dataGridViewAccessories_SelectionChanged(object sender, EventArgs e)
        {
            int selectedIndex = dataGridViewAccessories.CurrentCell.RowIndex;

            dynamic item = dataGridViewAccessories.Rows[selectedIndex].Cells[2].Value;

            List<string> tmpLST = ((string)((dynamic)dataGridViewAccessories.Rows[selectedIndex].Cells[2].Value)
                                        .PrintOut(MainForm.MainSaveFileInfoData.Version, ""))
                                  .Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();

            tmpLST.RemoveAt(0);
            tmpLST.RemoveAt(tmpLST.Count() - 1);

            textBoxAccessoryData.ReadOnly = true;
            textBoxAccessoryData.Text = string.Join(Environment.NewLine, tmpLST);
        }

        private void dataGridViewAccessories_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            //Delete button
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.ColumnIndex == 3 && e.RowIndex >= 0)
            {
                dynamic item = senderGrid.Rows[e.RowIndex].Cells[2].Value;

                string accName = item.GetType().Name;

                string dialogText = "Delete " + Utilities.TextUtilities.CapitalizeWord(accName.Replace('_', ' '));

                if (accName == "Vehicle_Accessory")
                {
                    dialogText += " " + Utilities.TextUtilities.CapitalizeWord(item.accType);
                }

                dialogText += " ?";

                DialogResult dr = MessageBox.Show(dialogText, "Deleting item", MessageBoxButtons.YesNo);

                if (dr == DialogResult.Yes)
                {
                    Accessories.Remove((string)senderGrid.Rows[e.RowIndex].Cells[0].Value);
                    senderGrid.Rows.RemoveAt(e.RowIndex);
                }   
            }
        }

        //Accessory data
        private void textBoxAccessoryData_ReadOnlyChanged(object sender, EventArgs e)
        {
            changebuttonEditAccessoryState(textBoxAccessoryData.ReadOnly);
        }

        //Edit button
        private void buttonEditAccessory_Click(object sender, EventArgs e)
        {
            textBoxAccessoryData.ReadOnly = !textBoxAccessoryData.ReadOnly;

            changebuttonEditAccessoryState(textBoxAccessoryData.ReadOnly);

            if (textBoxAccessoryData.ReadOnly)
            {
                Save.Items.SiiNunit tmp = new Save.Items.SiiNunit();

                int selectedIndex = dataGridViewAccessories.CurrentCell.RowIndex;

                string accName = dataGridViewAccessories.Rows[selectedIndex].Cells[2].Value.GetType().Name;
                string[] dataLines = textBoxAccessoryData.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                dynamic newAccItem = tmp.DetectTag(accName.ToLower(), dataLines);

                dataGridViewAccessories.Rows[selectedIndex].Cells[2].Value = newAccItem;
            }
        }

        private void changebuttonEditAccessoryState(bool _state)
        {
            if (_state)
            {
                MainForm.HelpTranslateControlDiffName(buttonEdit, "buttonEdit");
            }
            else
            {
                MainForm.HelpTranslateControlDiffName(buttonEdit, "buttonSave");
            }
        }

        //Main buttons        
        private void buttonApply_Click(object sender, EventArgs e)
        {
            //Close
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            //Close
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();

            string format = exportFormatString + Environment.NewLine;

            // Set data to clipboard
            int selectedIndex = dataGridViewAccessories.CurrentCell.RowIndex;
            dynamic item = dataGridViewAccessories.Rows[selectedIndex].Cells[2].Value;

            string clipText = item.PrintOut(MainForm.MainSaveFileInfoData.Version, "");

            Clipboard.SetText(format + clipText);
        }

        private void buttonPaste_Click(object sender, EventArgs e)
        {
            // Get data from clipboard
            string result = null;

            if (Clipboard.ContainsText(TextDataFormat.Text))
                result = Clipboard.GetText(TextDataFormat.Text);
            else
                MessageBox.Show("No valid items");

            if (result != null)
            {
                List<string> lines = result.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();

                if (lines[0] != exportFormatString || lines.Count() < 4)
                {
                    MessageBox.Show("Non valid item");
                    return;
                }

                lines.RemoveAt(0);

                string[] splittedLine = lines[0].Split(new char[] { ':', '{' }, 3);
                string tagLine = splittedLine[0].Trim();

                Save.Items.SiiNunit tmp = new Save.Items.SiiNunit();
                dynamic newAccItem = tmp.DetectTag(tagLine, lines.ToArray());

                string nameless = MainForm.GetSpareNameless();

                Accessories.Add(nameless, newAccItem);

                dataGridViewAccessories.Rows.Add(nameless, GetImageForAccessoryByType(tagLine), newAccItem, "X");
            }
        }
    }
}
