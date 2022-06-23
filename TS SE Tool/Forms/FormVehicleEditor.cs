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

            List<string> tmpLST = ((string)item.PrintOut(0, "")).Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();

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

                string dialogText = "Delete " + accName.Replace('_', ' ');

                if (accName == "Vehicle_Accessory")
                {
                    dialogText += " " + item.accType;
                }

                dialogText += " ?";

                DialogResult dr = MessageBox.Show(dialogText, "Deleting item", MessageBoxButtons.YesNo);

                if (dr == DialogResult.Yes)
                {
                    string n = (string)senderGrid.Rows[e.RowIndex].Cells[0].Value;

                    Accessories.Remove(n);
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
            changeTextBoxAccessoryDataReadOnlyState(textBoxAccessoryData.ReadOnly);
            changebuttonEditAccessoryState(textBoxAccessoryData.ReadOnly);

            if (textBoxAccessoryData.ReadOnly)
            {
                int selectedIndex = dataGridViewAccessories.CurrentCell.RowIndex;

                dynamic item = dataGridViewAccessories.Rows[selectedIndex].Cells[2].Value;

                string accName = item.GetType().Name;
                string[] dataLines = textBoxAccessoryData.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                Save.Items.SiiNunit tmp = new Save.Items.SiiNunit();

                dynamic newAccItem = tmp.DetectTag(accName.ToLower(), dataLines);

                dataGridViewAccessories.Rows[selectedIndex].Cells[2].Value = newAccItem;
            }
        }

        private void changeTextBoxAccessoryDataReadOnlyState(bool _state)
        {
            if (_state)
            {
                textBoxAccessoryData.ReadOnly = false;
            }
            else
            {
                textBoxAccessoryData.ReadOnly = true;
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
            string format = "TSSET_VehicleAccessory" + Environment.NewLine;

            // Set data to clipboard
            int selectedIndex = dataGridViewAccessories.CurrentCell.RowIndex;
            dynamic item = dataGridViewAccessories.Rows[selectedIndex].Cells[2].Value;

            string clipText = item.PrintOut(0, "");

            Clipboard.SetText(format + clipText);
        }

        private void buttonPaste_Click(object sender, EventArgs e)
        {
            string format = "TSSET_VehicleAccessory";

            // Get data from clipboard
            string result = null;

            var asd = Clipboard.GetDataObject();
            var asab = Clipboard.GetFileDropList();

            if (Clipboard.ContainsText(TextDataFormat.Text))
                result = Clipboard.GetText(TextDataFormat.Text);
            else
                MessageBox.Show("No valid items");


            if (result != null)
            {
                List<string> lines = result.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();

                if (lines[0] != format && lines.Count() > 1)
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
