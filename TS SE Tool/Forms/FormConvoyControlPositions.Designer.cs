namespace TS_SE_Tool
{
    partial class FormConvoyControlPositions
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonSave = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.labelExistingSaves = new System.Windows.Forms.Label();
            this.buttonExportImport = new System.Windows.Forms.Button();
            this.buttonMoveSaves = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radioButtonNamesNone = new System.Windows.Forms.RadioButton();
            this.radioButtonNamesCustom = new System.Windows.Forms.RadioButton();
            this.radioButtonNamesOriginal = new System.Windows.Forms.RadioButton();
            this.textBoxCustomName = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.buttonSelectCustomThumbnail = new System.Windows.Forms.Button();
            this.checkBoxCustomThumbnail = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.radioButtonSelect = new System.Windows.Forms.RadioButton();
            this.labelExportImport = new System.Windows.Forms.Label();
            this.radioButtonMove = new System.Windows.Forms.RadioButton();
            this.statusStripCCpositions = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusMessages = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.labelThumbnailDescription = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.statusStripCCpositions.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel1.Controls.Add(this.buttonSave, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.listBox1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.listBox2, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelExistingSaves, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonExportImport, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.buttonMoveSaves, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(600, 341);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // buttonSave
            // 
            this.buttonSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSave.Location = new System.Drawing.Point(3, 312);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(194, 26);
            this.buttonSave.TabIndex = 2;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(3, 33);
            this.listBox1.Name = "listBox1";
            this.listBox1.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.listBox1.Size = new System.Drawing.Size(194, 273);
            this.listBox1.TabIndex = 0;
            this.listBox1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBox1_DrawItem);
            this.listBox1.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.listBox1_MeasureItem);
            // 
            // listBox2
            // 
            this.listBox2.AllowDrop = true;
            this.listBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox2.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.listBox2.FormattingEnabled = true;
            this.listBox2.Location = new System.Drawing.Point(403, 33);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(194, 273);
            this.listBox2.TabIndex = 2;
            this.listBox2.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBox2_DrawItem);
            this.listBox2.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.listBox2_MeasureItem);
            this.listBox2.SelectedIndexChanged += new System.EventHandler(this.listBox2_SelectedIndexChanged);
            this.listBox2.DragDrop += new System.Windows.Forms.DragEventHandler(this.listBox2_DragDrop);
            this.listBox2.DragOver += new System.Windows.Forms.DragEventHandler(this.listBox2_DragOver);
            this.listBox2.DragLeave += new System.EventHandler(this.listBox2_DragLeave);
            // 
            // labelExistingSaves
            // 
            this.labelExistingSaves.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelExistingSaves.AutoSize = true;
            this.labelExistingSaves.Location = new System.Drawing.Point(3, 8);
            this.labelExistingSaves.Name = "labelExistingSaves";
            this.labelExistingSaves.Size = new System.Drawing.Size(74, 13);
            this.labelExistingSaves.TabIndex = 3;
            this.labelExistingSaves.Text = "Existing saves";
            // 
            // buttonExportImport
            // 
            this.buttonExportImport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonExportImport.Location = new System.Drawing.Point(403, 312);
            this.buttonExportImport.Name = "buttonExportImport";
            this.buttonExportImport.Size = new System.Drawing.Size(194, 26);
            this.buttonExportImport.TabIndex = 5;
            this.buttonExportImport.Text = "Export";
            this.buttonExportImport.UseVisualStyleBackColor = true;
            this.buttonExportImport.Click += new System.EventHandler(this.buttonExportImport_Click);
            // 
            // buttonMoveSaves
            // 
            this.buttonMoveSaves.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonMoveSaves.Location = new System.Drawing.Point(203, 312);
            this.buttonMoveSaves.Name = "buttonMoveSaves";
            this.buttonMoveSaves.Size = new System.Drawing.Size(194, 26);
            this.buttonMoveSaves.TabIndex = 6;
            this.buttonMoveSaves.Text = "> > >";
            this.buttonMoveSaves.UseVisualStyleBackColor = true;
            this.buttonMoveSaves.Click += new System.EventHandler(this.buttonMoveSaves_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(200, 30);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40.14337F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 59.85663F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(200, 279);
            this.tableLayoutPanel2.TabIndex = 7;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.radioButtonNamesNone);
            this.panel1.Controls.Add(this.radioButtonNamesCustom);
            this.panel1.Controls.Add(this.radioButtonNamesOriginal);
            this.panel1.Controls.Add(this.textBoxCustomName);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(194, 106);
            this.panel1.TabIndex = 1;
            // 
            // radioButtonNamesNone
            // 
            this.radioButtonNamesNone.AutoSize = true;
            this.radioButtonNamesNone.Location = new System.Drawing.Point(6, 76);
            this.radioButtonNamesNone.Name = "radioButtonNamesNone";
            this.radioButtonNamesNone.Size = new System.Drawing.Size(121, 17);
            this.radioButtonNamesNone.TabIndex = 6;
            this.radioButtonNamesNone.Text = "Don\'t include names";
            this.radioButtonNamesNone.UseVisualStyleBackColor = true;
            this.radioButtonNamesNone.Visible = false;
            this.radioButtonNamesNone.CheckedChanged += new System.EventHandler(this.NamesRadioButton_CheckedChanged);
            // 
            // radioButtonNamesCustom
            // 
            this.radioButtonNamesCustom.AutoSize = true;
            this.radioButtonNamesCustom.Location = new System.Drawing.Point(6, 26);
            this.radioButtonNamesCustom.Name = "radioButtonNamesCustom";
            this.radioButtonNamesCustom.Size = new System.Drawing.Size(94, 17);
            this.radioButtonNamesCustom.TabIndex = 5;
            this.radioButtonNamesCustom.Text = "Custom names";
            this.radioButtonNamesCustom.UseVisualStyleBackColor = true;
            this.radioButtonNamesCustom.CheckedChanged += new System.EventHandler(this.NamesRadioButton_CheckedChanged);
            // 
            // radioButtonNamesOriginal
            // 
            this.radioButtonNamesOriginal.AutoSize = true;
            this.radioButtonNamesOriginal.Location = new System.Drawing.Point(6, 3);
            this.radioButtonNamesOriginal.Name = "radioButtonNamesOriginal";
            this.radioButtonNamesOriginal.Size = new System.Drawing.Size(94, 17);
            this.radioButtonNamesOriginal.TabIndex = 4;
            this.radioButtonNamesOriginal.Text = "Original names";
            this.radioButtonNamesOriginal.UseVisualStyleBackColor = true;
            this.radioButtonNamesOriginal.CheckedChanged += new System.EventHandler(this.NamesRadioButton_CheckedChanged);
            // 
            // textBoxCustomName
            // 
            this.textBoxCustomName.Enabled = false;
            this.textBoxCustomName.Location = new System.Drawing.Point(3, 49);
            this.textBoxCustomName.Name = "textBoxCustomName";
            this.textBoxCustomName.Size = new System.Drawing.Size(185, 20);
            this.textBoxCustomName.TabIndex = 3;
            this.textBoxCustomName.TextChanged += new System.EventHandler(this.textBoxCustomName_TextChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.labelThumbnailDescription);
            this.panel2.Controls.Add(this.buttonSelectCustomThumbnail);
            this.panel2.Controls.Add(this.checkBoxCustomThumbnail);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 115);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(194, 161);
            this.panel2.TabIndex = 2;
            // 
            // buttonSelectCustomThumbnail
            // 
            this.buttonSelectCustomThumbnail.Location = new System.Drawing.Point(3, 26);
            this.buttonSelectCustomThumbnail.Name = "buttonSelectCustomThumbnail";
            this.buttonSelectCustomThumbnail.Size = new System.Drawing.Size(188, 23);
            this.buttonSelectCustomThumbnail.TabIndex = 1;
            this.buttonSelectCustomThumbnail.Text = "Select images";
            this.buttonSelectCustomThumbnail.UseVisualStyleBackColor = true;
            this.buttonSelectCustomThumbnail.Click += new System.EventHandler(this.buttonSelectCustomThumbnail_Click);
            // 
            // checkBoxCustomThumbnail
            // 
            this.checkBoxCustomThumbnail.AutoSize = true;
            this.checkBoxCustomThumbnail.Location = new System.Drawing.Point(3, 3);
            this.checkBoxCustomThumbnail.Name = "checkBoxCustomThumbnail";
            this.checkBoxCustomThumbnail.Size = new System.Drawing.Size(139, 17);
            this.checkBoxCustomThumbnail.TabIndex = 0;
            this.checkBoxCustomThumbnail.Text = "Add custom Thumbnails";
            this.checkBoxCustomThumbnail.UseVisualStyleBackColor = true;
            this.checkBoxCustomThumbnail.CheckedChanged += new System.EventHandler(this.checkBoxCustomThumbnail_CheckedChanged);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.radioButtonSelect, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.labelExportImport, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.radioButtonMove, 1, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(400, 0);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(200, 30);
            this.tableLayoutPanel3.TabIndex = 8;
            // 
            // radioButtonSelect
            // 
            this.radioButtonSelect.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButtonSelect.AutoSize = true;
            this.radioButtonSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonSelect.Location = new System.Drawing.Point(133, 3);
            this.radioButtonSelect.Name = "radioButtonSelect";
            this.radioButtonSelect.Size = new System.Drawing.Size(64, 24);
            this.radioButtonSelect.TabIndex = 3;
            this.radioButtonSelect.TabStop = true;
            this.radioButtonSelect.Text = "Select";
            this.radioButtonSelect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButtonSelect.UseVisualStyleBackColor = true;
            this.radioButtonSelect.CheckedChanged += new System.EventHandler(this.radioButtonListBoxState_CheckedChanged);
            // 
            // labelExportImport
            // 
            this.labelExportImport.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelExportImport.AutoSize = true;
            this.labelExportImport.Location = new System.Drawing.Point(3, 8);
            this.labelExportImport.Name = "labelExportImport";
            this.labelExportImport.Size = new System.Drawing.Size(51, 13);
            this.labelExportImport.TabIndex = 4;
            this.labelExportImport.Text = "Exporting";
            // 
            // radioButtonMove
            // 
            this.radioButtonMove.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButtonMove.AutoSize = true;
            this.radioButtonMove.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonMove.Location = new System.Drawing.Point(63, 3);
            this.radioButtonMove.Name = "radioButtonMove";
            this.radioButtonMove.Size = new System.Drawing.Size(64, 24);
            this.radioButtonMove.TabIndex = 2;
            this.radioButtonMove.TabStop = true;
            this.radioButtonMove.Text = "Move";
            this.radioButtonMove.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButtonMove.UseVisualStyleBackColor = true;
            this.radioButtonMove.CheckedChanged += new System.EventHandler(this.radioButtonListBoxState_CheckedChanged);
            // 
            // statusStripCCpositions
            // 
            this.statusStripCCpositions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusMessages,
            this.toolStripProgressBar});
            this.statusStripCCpositions.Location = new System.Drawing.Point(0, 344);
            this.statusStripCCpositions.Name = "statusStripCCpositions";
            this.statusStripCCpositions.Size = new System.Drawing.Size(600, 22);
            this.statusStripCCpositions.SizingGrip = false;
            this.statusStripCCpositions.TabIndex = 1;
            this.statusStripCCpositions.Text = "statusStrip1";
            // 
            // toolStripStatusMessages
            // 
            this.toolStripStatusMessages.AutoSize = false;
            this.toolStripStatusMessages.Name = "toolStripStatusMessages";
            this.toolStripStatusMessages.Size = new System.Drawing.Size(370, 17);
            this.toolStripStatusMessages.Text = "Initializing...";
            this.toolStripStatusMessages.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripProgressBar
            // 
            this.toolStripProgressBar.Name = "toolStripProgressBar";
            this.toolStripProgressBar.Size = new System.Drawing.Size(200, 16);
            // 
            // labelThumbnailDescription
            // 
            this.labelThumbnailDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelThumbnailDescription.Location = new System.Drawing.Point(3, 52);
            this.labelThumbnailDescription.MaximumSize = new System.Drawing.Size(190, 0);
            this.labelThumbnailDescription.Name = "labelThumbnailDescription";
            this.labelThumbnailDescription.Size = new System.Drawing.Size(185, 52);
            this.labelThumbnailDescription.TabIndex = 2;
            this.labelThumbnailDescription.Text = "Image size needs to be at least 256x128 and with aspect ratio of 2:1. Otherwise i" +
    "t will grab part of the image from Top Left corner.";
            this.labelThumbnailDescription.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormConvoyControlPositions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 366);
            this.Controls.Add(this.statusStripCCpositions);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FormConvoyControlPositions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormConvoyControlPositions";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.statusStripCCpositions.ResumeLayout(false);
            this.statusStripCCpositions.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.Label labelExistingSaves;
        private System.Windows.Forms.Label labelExportImport;
        private System.Windows.Forms.Button buttonExportImport;
        private System.Windows.Forms.Button buttonMoveSaves;
        private System.Windows.Forms.TextBox textBoxCustomName;
        private System.Windows.Forms.RadioButton radioButtonNamesNone;
        private System.Windows.Forms.RadioButton radioButtonNamesCustom;
        private System.Windows.Forms.RadioButton radioButtonNamesOriginal;
        private System.Windows.Forms.StatusStrip statusStripCCpositions;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;
        public System.Windows.Forms.ToolStripStatusLabel toolStripStatusMessages;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button buttonSelectCustomThumbnail;
        private System.Windows.Forms.CheckBox checkBoxCustomThumbnail;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.RadioButton radioButtonSelect;
        private System.Windows.Forms.RadioButton radioButtonMove;
        private System.Windows.Forms.Label labelThumbnailDescription;
    }
}