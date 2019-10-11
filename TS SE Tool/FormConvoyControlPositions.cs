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
using System.IO;
using TGASharpLib;

namespace TS_SE_Tool
{
    public partial class FormConvoyControlPositions : Form
    {
        FormMain MainForm = Application.OpenForms.OfType<FormMain>().Single();
        bool exportState;
        Dictionary<string,string> FoldersToClear = new Dictionary<string, string>();
        Dictionary<string, string> NewSave = new Dictionary<string, string>();
        string BaseSave = "";
        string preview_mat = "material : \"ui.rfx\"\r\n{\r\n	texture : \"preview.tobj\"\r\n	texture_name : \"texture\"\r\n}";
        byte[] preview_tobj = new byte[] { 1, 10, 177, 112, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 2, 0, 3, 3, 2, 0, 2, 2, 2, 1, 0, 0, 0, 1, 0, 0 };
        Image[] Thumbnails = new Image[0];


        public FormConvoyControlPositions(bool _exportState)
        {
            InitializeComponent();
            exportState = _exportState;
            FormComponentsSetup(exportState);
            FillSaveListBox();
        }

        private void FormComponentsSetup(bool _exportState)
        {
            radioButtonNamesOriginal.Checked = true;

            if (_exportState)
            {
                this.Text = "Exporting CC positions";
                buttonExportImport.Text = "Export";
                buttonMoveSaves.Text = "> > >";
                buttonSave.Visible = false;
                buttonExportImport.Enabled = false;

                checkBoxCustomThumbnail.Visible = false;
                buttonSelectCustomThumbnail.Visible = false;
            }
            else
            {
                this.Text = "Importing CC positions";
                buttonExportImport.Text = "Import";
                buttonMoveSaves.Text = "< < <";
                buttonSave.Text = "Save";
                radioButtonNamesNone.Visible = false;

                listBox2.SelectionMode = SelectionMode.MultiSimple;
                listBox1.SelectionMode = SelectionMode.None;

                buttonSave.Enabled = false;
                buttonMoveSaves.Enabled = false;
                buttonSelectCustomThumbnail.Enabled = false;
            }

        }

        private void FillSaveListBox()
        {
            if (Globals.SavesHex.Length > 0)
            {
                DataTable combDT = new DataTable();
                DataColumn dc = new DataColumn("savePath", typeof(string));
                combDT.Columns.Add(dc);

                dc = new DataColumn("saveName", typeof(string));
                combDT.Columns.Add(dc);

                dc = new DataColumn("saveType", typeof(byte));
                combDT.Columns.Add(dc);

                dc = new DataColumn("saveDateTime", typeof(string));
                combDT.Columns.Add(dc);

                bool NotANumber = false;

                foreach (string profile in Globals.SavesHex)
                {
                    if (!File.Exists(profile + @"\game.sii") || !File.Exists(profile + @"\info.sii"))
                        continue;

                    string[] fold = profile.Split(new string[] { "\\" }, StringSplitOptions.None);

                    foreach (char c in fold[fold.Length - 1])
                    {
                        if (c < '0' || c > '9')
                        {
                            NotANumber = true;
                            break;
                        }
                    }

                    if (NotANumber)
                    {
                        string[] namearr = fold[fold.Length - 1].Split(new char[] { '_' });
                        string ProfileName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(namearr[0]);

                        for (int i = 1; i < namearr.Length; i++)
                        {
                            ProfileName += " " + namearr[i];
                        }

                        combDT.Rows.Add(profile, "- " + ProfileName + " -", 0, File.GetLastWriteTime(profile + @"\game.sii").ToString());
                    }
                    else
                        combDT.Rows.Add(profile, MainForm.GetCustomSaveFilename(profile, this, "statusStripCCpositions", "toolStripStatusMessages"), 1, File.GetLastWriteTime(profile + @"\game.sii").ToString());

                    NotANumber = false;
                }

                listBox1.DataSource = combDT;

                listBox1.ValueMember = "savePath";
                listBox1.DisplayMember = "saveName";
            }

            toolStripStatusMessages.Text = "";
        }

        //Middle controls
        private void NamesRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButtonNamesOriginal.Checked)
            {
                textBoxCustomName.Enabled = false;
            }
            else if(radioButtonNamesCustom.Checked)
            {
                textBoxCustomName.Enabled = true;
            }
            else if (radioButtonNamesNone.Checked)
            {
                textBoxCustomName.Enabled = false;
            }

            listBox2.Refresh();
        }

        private void textBoxCustomName_TextChanged(object sender, EventArgs e)
        {
            listBox2.Refresh();
        }

        private void checkBoxCustomThumbnail_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox s = sender as CheckBox;

            buttonSelectCustomThumbnail.Enabled = s.Checked;
        }

        private void buttonSelectCustomThumbnail_Click(object sender, EventArgs e)
        {
            // Set the file dialog to filter for graphics files.
            OpenFileDialog openFileDialogThumbnail = new OpenFileDialog();
            openFileDialogThumbnail.Filter = "Images (*.BMP;*.GIF;*.JPG;*.JPEG;*.PNG;*.TIFF)|*.BMP;*.GIF;*.JPG;*.JPEG;*.PNG;*.TIFF|" + //BMP GIF JPEG PNG TIFF
                "All files (*.*)|*.*";

            // Allow the user to select multiple images.
            openFileDialogThumbnail.Multiselect = true;
            openFileDialogThumbnail.Title = "Browse for Thumbnail (256x128 or aspect ratio 2:1)";

            DialogResult dr = openFileDialogThumbnail.ShowDialog();
            if (dr == DialogResult.OK)
            {
                Array.Resize(ref Thumbnails, openFileDialogThumbnail.FileNames.Length);
                // Read the files

                int thumbCount = 0;
                foreach (String file in openFileDialogThumbnail.FileNames)
                {
                    // Create a PictureBox.
                    try
                    {
                        Image loadedImage = Image.FromFile(file);
                        Bitmap temp = (Bitmap)loadedImage;

                        int _width = 256, _height = 128;
                        //Check resolution
                        if(loadedImage.Width != _width || loadedImage.Height != _height)
                        {
                            //Check aspect ratio
                            double aspect = (double) loadedImage.Width / loadedImage.Height;

                            if (aspect == 2)
                            {
                                //Right aspect
                                temp = new Bitmap(loadedImage, new Size(_width, _height));
                            }
                            else
                            {
                                //Wrong aspect. Set new width and height
                                if(aspect > 2)
                                {
                                    _height = loadedImage.Height;
                                    _width = _height * 2;
                                }
                                else
                                {
                                    _width = loadedImage.Width;
                                    _height = _width * 2;
                                }

                                temp = new Bitmap(temp.Clone(new Rectangle(0, 0, _width, _height), temp.PixelFormat),256,128);
                            }
                        }

                        Thumbnails[thumbCount] = temp;
                        thumbCount++;
                    }
                    catch (Exception ex)
                    {
                        // Could not load the image - probably related to Windows file system permissions.
                        MessageBox.Show("Cannot display the image: " + file.Substring(file.LastIndexOf('\\'))
                            + ". You may not have permission to read the file, or " +
                            "it may be corrupt.\n\nError: " + ex.Message);
                    }
                }
            }
        }

        //Buttons
        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectionMode == SelectionMode.None || listBox1.SelectedIndex == -1)
            {
                listBox1.SelectionMode = SelectionMode.One;
                MessageBox.Show("Please select Base save file for New saves.");

                listBox1.SelectedValue = Globals.SelectedSavePath;
            }
            else
            {
                //Check if selected existing and valid save files 0,1 not 2,4
                DataRowView sI = (DataRowView)listBox1.SelectedItem;

                BaseSave = (string)sI.Row["savePath"];

                if ((byte)sI.Row[2] == 0 || (byte)sI.Row[2] == 1)
                {
                    string SiiSavePath = (string)sI.Row[0] + "\\game.sii"; //bool FileDecoded = false;
                    string[] tempSavefileInMemory = null;

                    //Load Base save file
                    if (!File.Exists(SiiSavePath))
                    {
                        MessageBox.Show("File does not exist in " + SiiSavePath);
                        MainForm.ShowStatusMessages("e", "error_could_not_find_file", this, statusStripCCpositions.Name, statusStripCCpositions.Items[0].Name);
                    }
                    else
                    {
                        MainForm.FileDecoded = false;
                        try
                        {
                            int decodeAttempt = 0;
                            while (decodeAttempt < 5)
                            {
                                tempSavefileInMemory = MainForm.NewDecodeFile(SiiSavePath);

                                if (MainForm.FileDecoded)
                                {
                                    break;
                                }
                                decodeAttempt++;
                            }

                            if (decodeAttempt == 5)
                            {
                                MessageBox.Show("Could not decrypt after 5 attempts");
                                MainForm.ShowStatusMessages("e", "error_could_not_decode_file", this, statusStripCCpositions.Name, statusStripCCpositions.Items[0].Name);
                            }
                        }
                        catch
                        {
                            MessageBox.Show("Could not read: " + SiiSavePath);
                        }

                        if ((tempSavefileInMemory == null) || (tempSavefileInMemory[0] != "SiiNunit"))
                        {
                            MessageBox.Show("Wrongly decoded Save file or wrong file format");
                            MainForm.ShowStatusMessages("e", "error_file_not_decoded", this, statusStripCCpositions.Name, statusStripCCpositions.Items[0].Name);
                        }
                        else
                        {
                            //Delete overwritten savefiles
                            foreach (KeyValuePair<string, string> ftd in FoldersToClear)
                            {
                                Directory.Delete(ftd.Key,true);
                            }

                            Globals.SavesHex = Directory.GetDirectories(Globals.SelectedProfilePath + @"\save").OrderByDescending(f => new FileInfo(f).LastWriteTime).ToArray();

                            List<byte> CustomFolders = new List<byte>();

                            //Find all custom folders
                            foreach (string saveF in Globals.SavesHex)
                            {
                                string saveFname = Path.GetFileName(saveF);
                                bool result = byte.TryParse(saveFname, out byte number);
                                if (result)
                                {
                                    CustomFolders.Add(number);
                                }
                            }

                            List<byte> NewCustomFolders = new List<byte>();

                            byte CustomFoldersCount = 1;
                            while (true)
                            {
                                if (CustomFolders.Exists(x => x == CustomFoldersCount))
                                { }
                                else
                                    NewCustomFolders.Add(CustomFoldersCount);

                                if (NewCustomFolders.Count() == NewSave.Count)
                                    break;

                                CustomFoldersCount++;
                            }

                            //Create new numbered folder and new save files
                            int iSave = 0;
                            foreach (KeyValuePair<string, string> entry in NewSave)
                            {
                                //Create folder
                                string fp = Directory.GetParent(Globals.SavesHex[0]).FullName + "\\" + NewCustomFolders[iSave].ToString();
                                Directory.CreateDirectory(fp);

                                //Copy info file
                                string[] infoSii = MainForm.NewDecodeFile(BaseSave + @"\info.sii");

                                //Prepare data
                                double tDT = MainForm.DateTimeToUnixTimeStamp(DateTime.UtcNow.ToLocalTime());

                                SavefileInfoData infoData = new SavefileInfoData();
                                infoData.SaveName = entry.Key;//[iSave];
                                infoData.FileTime = Convert.ToInt32(Math.Floor(tDT));

                                //Write info file
                                MainForm.WriteInfoFile(infoSii, fp + "\\info.sii", infoData);

                                //Create thumbnail files
                                //mat
                                Encoding utf8WithoutBom = new UTF8Encoding(false);
                                using (StreamWriter writer = new StreamWriter(fp + "\\preview.mat", true, utf8WithoutBom))
                                {
                                    writer.WriteLine(preview_mat);
                                }

                                //tobj
                                using (BinaryWriter binWriter = new BinaryWriter(File.Open(fp + "\\preview.tobj", FileMode.Create)))
                                {
                                    binWriter.Write(preview_tobj);
                                    string pathToTGA = "/home/profiles/" + Globals.SelectedProfile + "/save/" + NewCustomFolders[iSave] + "/preview.tga";
                                    byte filePathLength = (byte)pathToTGA.Length;
                                    binWriter.Write(filePathLength);
                                    binWriter.Write(new byte[] { 0, 0, 0, 0, 0, 0, 0 });
                                    binWriter.Write(Encoding.UTF8.GetBytes(pathToTGA));
                                }

                                //image tga
                                //Default thumbnail
                                TGA tgaImg; Bitmap newbmp = null; int custThumbCount = 0;

                                if (!checkBoxCustomThumbnail.Checked)
                                {
                                    string[] imgpaths = new string[] { @"img\autosave.dds" };
                                    newbmp = new Bitmap(MainForm.ExtImgLoader(imgpaths, 256, 128, 0, 0)[0]);
                                }
                                else if (checkBoxCustomThumbnail.Checked && Thumbnails.Length != 0)
                                {
                                    if(custThumbCount < Thumbnails.Length)
                                    {
                                        newbmp = new Bitmap(Thumbnails[custThumbCount]);
                                        custThumbCount++;
                                    }
                                    else
                                    {
                                        custThumbCount = 0;
                                        newbmp = new Bitmap(Thumbnails[custThumbCount]);
                                    }                                        
                                }

                                tgaImg = (TGA)newbmp;
                                tgaImg.Save(fp + "\\preview.tga");

                                //Create game save file
                                using (StreamWriter writer = new StreamWriter(fp + "\\game.sii", true))
                                {
                                    writer.Write(tempSavefileInMemory[0]);

                                    for (int line = 1; line < tempSavefileInMemory.Length; line++)
                                    {
                                        string SaveInMemLine = tempSavefileInMemory[line];

                                        if (SaveInMemLine.StartsWith(" truck_placement:"))
                                        {
                                            writer.Write("\r\n" + " truck_placement: " + entry.Value);
                                            line++;
                                            writer.Write("\r\n" + " trailer_placement: (0, 0, 0) (1; 0, 0, 0)");
                                            line++;
                                            int slave_trailers = int.Parse(tempSavefileInMemory[line].Split(new char[] { ' ' })[2]);
                                            writer.Write("\r\n" + tempSavefileInMemory[line]);
                                            if (slave_trailers > 0)
                                            {
                                                for (int i = 0; i < slave_trailers; i++)
                                                {
                                                    writer.Write("\r\n" + " slave_trailer_placements[" + i + "]: (0, 0, 0) (1; 0, 0, 0)");
                                                    line++;
                                                }
                                            }
                                            continue;
                                        }

                                        //EndWrite:
                                        writer.Write("\r\n" + SaveInMemLine);
                                    }
                                }

                                iSave++;
                            }

                            //
                            buttonSave.Enabled = false;
                            GC.Collect();
                            MessageBox.Show("Saves are created.\nNow you can close this window.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Not a valid save file for base file.\nPlease select existing save file not marked for deleting.");
                    //return;
                }
            }
        }

        private void buttonMoveSaves_Click(object sender, EventArgs e)
        {
            if (exportState)
            {
                DataTable combDT = new DataTable();

                DataColumn dc = new DataColumn("savePath", typeof(string));
                combDT.Columns.Add(dc);

                dc = new DataColumn("saveName", typeof(string));
                combDT.Columns.Add(dc);

                dc = new DataColumn("NewName", typeof(string));
                combDT.Columns.Add(dc);

                if (listBox2.DataSource != null)
                    combDT = (DataTable) listBox2.DataSource;

                int countSelected = listBox1.SelectedItems.Count;

                for(int i = 0; i < countSelected; i++)
                {                    
                    combDT.Rows.Add(((DataRowView)listBox1.SelectedItems[i]).Row.ItemArray[0], ((DataRowView)listBox1.SelectedItems[i]).Row.ItemArray[1], "");
                }

                listBox2.DataSource = combDT;
                listBox2.ValueMember = "savePath";
                listBox2.DisplayMember = "saveName";
                //listBox2.SelectedIndex = -1;

                listBox1.ClearSelected();

                buttonExportImport.Enabled = true;
            }
            else
            {
                if (listBox2.SelectedItems.Count == 0)
                {
                    //Select all
                    for (int i = 0; i < listBox2.Items.Count; i++)
                        listBox2.SetSelected(i, true);
                }
                else
                {
                    NewSave.Clear();
                    //Check save names
                    Dictionary<string,string> ExistingSave = new Dictionary<string, string>();
                    bool ExistingFlag = false;

                    foreach (DataRowView Dr in listBox1.Items)
                    {
                        if ((byte)Dr.Row.ItemArray[2] == 1)
                            ExistingSave.Add(Dr.Row.ItemArray[0].ToString(),Dr.Row.ItemArray[1].ToString());
                    }

                    foreach (DataRowView Dr in listBox2.SelectedItems)
                    {
                        string SN = Dr.Row.ItemArray[2].ToString();
                        string TP = Dr.Row.ItemArray[0].ToString();

                        NewSave.Add(SN, TP);
                        if (ExistingSave.ContainsValue(SN))                        
                            ExistingFlag = true;
                    }

                    if (ExistingFlag)
                    {
                        DialogResult result = MessageBox.Show("Some save files already exist.\nDo you want to overwrite save files?\nYes - overwrite. No - create saves with same names. Cancel - change selected saves.",
                            "Conflicting save file names", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button3);
                        if (result == DialogResult.Yes)
                        {
                            //Overwriting
                            //Creating list for cleanup
                            FoldersToClear = ExistingSave.Where(x => NewSave.ContainsKey(x.Value)).ToDictionary(x => x.Key, x => x.Value);
                            //Deleting from listbox and adding *new*
                        }
                        else if (result == DialogResult.No)
                        {
                            //Create extra
                        }
                        else if(result == DialogResult.Cancel)
                        {
                            return;
                        }
                    }

                    //Move to existing saves
                    DataTable combDT = ((DataTable)listBox1.DataSource).Copy();

                    foreach (DataRow temp in combDT.Rows)
                    {
                        if(FoldersToClear.ContainsKey(temp["savePath"].ToString()))
                        {
                            temp["saveType"] = 4;
                        }
                    }

                    foreach (KeyValuePair<string,string> tNS in NewSave)
                    {
                        //Add row
                        combDT.Rows.Add(tNS.Value, tNS.Key, 2);
                    }

                    listBox1.DataSource = combDT;

                    listBox2.ClearSelected();
                    listBox2.Enabled = false;
                    buttonExportImport.Enabled = false;
                    buttonMoveSaves.Enabled = false;
                    buttonSave.Enabled = true;
                    panel1.Enabled = false;
                    panel2.Enabled = false;
                }
            }
        }

        private void buttonExportImport_Click(object sender, EventArgs e)
        {
            DataTable combDT = new DataTable();

            if (exportState)
            {
                DataColumn dc = new DataColumn("savePath", typeof(string));
                combDT.Columns.Add(dc);

                dc = new DataColumn("saveName", typeof(string));
                combDT.Columns.Add(dc);

                dc = new DataColumn("NewName", typeof(string));
                combDT.Columns.Add(dc);

                string tempData = "CCpositions";

                if (listBox2.DataSource != null)
                    combDT = (DataTable)listBox2.DataSource;

                int CustomInc = 0; //Save number

                toolStripProgressBar.Maximum = combDT.Rows.Count;

                foreach (DataRow temp in combDT.Rows)
                {
                    //Load and scan save files for truck position
                    string[] tSF = MainForm.NewDecodeFile(temp[0].ToString() + @"\game.sii");

                    string[] chunkOfline;
                    string truckPosition = "";

                    for (int line = 0; line < tSF.Length; line++)
                    {
                        if (tSF[line].StartsWith(" truck_placement:"))
                        {
                            chunkOfline = tSF[line].Split(new char[] { ':' });
                            truckPosition = chunkOfline[1].TrimStart(' ');
                            break;
                        }
                    }
                    //Clear memory
                    tSF = null;
                    GC.Collect();
                    //

                    //Save name
                    string SaveName = "";

                    if (radioButtonNamesOriginal.Checked)
                        SaveName = temp[1].ToString();
                    else if (radioButtonNamesCustom.Checked)
                    {
                        SaveName = temp["NewName"].ToString(); //textBoxCustomName.Text + CustomInc.ToString();
                        //CustomInc++;
                    }
                    else if (radioButtonNamesNone.Checked)
                        SaveName = "";

                    tempData += "\r\nName:" + SaveName + "\r\nPosition:" + truckPosition;

                    CustomInc++;
                    toolStripProgressBar.Value = CustomInc;
                }

                string Converted = BitConverter.ToString(MainForm.zipText(tempData)).Replace("-", "");
                Clipboard.SetText(Converted);
                toolStripProgressBar.Value = 0;
                MessageBox.Show("Positions has been copied.");
            }
            else
            {
                DataColumn dc = new DataColumn("truckPosition", typeof(string));
                combDT.Columns.Add(dc);

                dc = new DataColumn("saveName", typeof(string));
                combDT.Columns.Add(dc);

                dc = new DataColumn("NewName", typeof(string));
                combDT.Columns.Add(dc);

                try
                {
                    string inputData = MainForm.unzipText(Clipboard.GetText());
                    string[] Lines = inputData.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                    if (Lines[0] == "CCpositions")
                    {
                        string Name = "";
                        for (int i = 1; i < Lines.Length; i++)
                        {
                            if (Lines[i].StartsWith("Name"))
                            {
                                Name = Lines[i].Split(new char[] { ':' }, 2)[1];
                            }
                            else if (Lines[i].StartsWith("Position"))
                            {
                                combDT.Rows.Add(Lines[i].Split(new char[] { ':' }, 2)[1], Name);
                            }
                        }


                        listBox2.DataSource = combDT;
                        listBox2.ValueMember = "truckPosition";
                        listBox2.DisplayMember = "saveName";
                        listBox2.SelectedIndex = -1;

                        MessageBox.Show("Position data has been inserted.");
                    }
                    else
                        MessageBox.Show("Wrong data. Expected Position data but\r\n" + Lines[0] + "\r\nwas found.");
                }
                catch
                {
                    MessageBox.Show("Something gone wrong.");
                }

                buttonMoveSaves.Enabled = true;
            }
        }
        
        //Drag N Drop on second listbox
        private void listBox2_MouseDown(object sender, MouseEventArgs e)
        {
            if (listBox2.SelectedItem == null) return;

            listBox2.DoDragDrop(listBox2.SelectedItem, DragDropEffects.Move);
        }

        private void listBox2_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void listBox2_DragDrop(object sender, DragEventArgs e)
        {
            Point point = listBox2.PointToClient(new Point(e.X, e.Y));
            int index = listBox2.IndexFromPoint(point);
            if (index < 0)
                index = listBox2.Items.Count - 1;

            int oldindex = listBox2.Items.IndexOf(e.Data.GetData(typeof(DataRowView)));

            if (index == oldindex)
                return;

            try
            {
                // find the row to move in the datasource
                DataRowView rowToMove = (DataRowView)e.Data.GetData(typeof(DataRowView));
                //clone it
                DataRow oldrow = rowToMove.Row;
                object[] test = oldrow.ItemArray;
                //Remove
                //object t1 = listBox2.SelectedItem;
                ((DataTable)listBox2.DataSource).Rows.Remove(oldrow);
                //Create new datasource
                DataTable combDT = ((DataTable)listBox2.DataSource).Copy();
                //Add row
                DataRow newrow = combDT.NewRow();
                newrow.ItemArray = test;
                combDT.Rows.InsertAt(newrow, index);
                listBox2.DataSource = combDT;
                listBox2.SelectedIndex = index;
            }
            catch(Exception ex)
            {
                string Msg = ex.Message;
            }
            
        }

        //Custom listbox Draw
        private int ItemMargin = 3, ItemHeight = 36;

        private void listBox1_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = (int)(ItemHeight);
            Rectangle t = listBox1.GetItemRectangle(e.Index);
        }

        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index == -1)
                return;

            // Get the ListBox and the item.
            ListBox lst = sender as ListBox;
            string txt = "";

            // Draw the background.
            e.DrawBackground();

            // See if the item is selected.
            Brush br;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected && lst.SelectionMode != SelectionMode.None)
                br = SystemBrushes.HighlightText;
            else
            {
                //test
                e.Graphics.FillRectangle(Brushes.White, e.Bounds);
                br = new SolidBrush(ForeColor);
                //br = new SolidBrush(e.ForeColor);
            }

            //Draw area
            float x = e.Bounds.Left + ItemMargin;
            float y = e.Bounds.Top + ItemMargin;
            float width = e.Bounds.Width - ItemMargin * 2;
            float height = Font.Height;// (e.Bounds.Height - ItemMargin * 2) / 2;

            RectangleF layout_rect = new RectangleF(x, y, width, height);

            DataRowView SaveDR = (DataRowView)lst.Items[e.Index];

            //Original name
            txt = SaveDR["saveName"].ToString();

            FontStyle NameFS = FontStyle.Regular;

            if ((byte)SaveDR["saveType"] == 2)
            {
                NameFS = FontStyle.Bold;
            }
            else if((byte)SaveDR["saveType"] == 4)
            {
                NameFS = FontStyle.Bold | FontStyle.Strikeout;
            }

            Font NameFnt = new Font(Font, NameFS);
            //Size size = TextRenderer.MeasureText(txt, NameFnt);
            ///.ItemHeight = (int)(ItemHeight / 2 + 2 * ItemMargin);            

            // Draw the text.
            e.Graphics.DrawString(txt, NameFnt, br, layout_rect);

            y = y + height + ItemMargin;
            layout_rect = new RectangleF(x, y, width, height);
            NameFnt = new Font(NameFnt.FontFamily, NameFnt.Size - 1);

            e.Graphics.DrawString(SaveDR["saveDateTime"].ToString(), NameFnt, br, layout_rect);

            // Draw the focus rectangle if appropriate.
            e.DrawFocusRectangle();
        }

        private void listBox2_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = (int)(ItemHeight + 2 * ItemMargin);
        }

        private void listBox2_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index == -1)
                return;

            // Get the ListBox and the item.
            ListBox lst = sender as ListBox;
            string txt = "";

            // Draw the background.
            e.DrawBackground();

            // See if the item is selected.
            Brush br;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                br = SystemBrushes.HighlightText;
            else
                br = new SolidBrush(e.ForeColor);
            
            //Draw area
            float x = e.Bounds.Left + ItemMargin;
            float y = e.Bounds.Top + ItemMargin;
            float width = e.Bounds.Width - ItemMargin * 2;
            float height = (e.Bounds.Height - ItemMargin * 2) / 2;

            RectangleF layout_rect = new RectangleF(x, y, width, height);

            DataRowView SaveDR = (DataRowView)lst.Items[e.Index];

            //Original name
            txt = SaveDR["saveName"].ToString();// + "\r\n" ;

            Font OriginalNameFnt = new Font(Font, FontStyle.Regular);
            // Draw the text.
            e.Graphics.DrawString(txt, OriginalNameFnt, br, layout_rect);

            //New name
            string SaveName = "";
            int decimalLength = listBox2.Items.Count.ToString("D").Length;

            if (radioButtonNamesOriginal.Checked)
                SaveName = SaveDR[1].ToString();
            else if (radioButtonNamesCustom.Checked && textBoxCustomName.Text != "")
            {
                SaveName = textBoxCustomName.Text + " " + (e.Index + 1).ToString("D" + decimalLength.ToString());
            }
            else if (radioButtonNamesNone.Checked)
                SaveName = "";

            SaveDR["NewName"] = SaveName;
            //listBox2.DataBindings
            //lst.Items[e.Index]

            txt = SaveName;

            Font NewNameFnt = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);

            y = e.Bounds.Top + (OriginalNameFnt.SizeInPoints + ItemMargin * 4);
            layout_rect = new RectangleF(x, y, width, height);
            // Draw the text.
            e.Graphics.DrawString(txt, NewNameFnt, br, layout_rect);

            // Draw the focus rectangle if appropriate.
            e.DrawFocusRectangle();
        }
    }
}
