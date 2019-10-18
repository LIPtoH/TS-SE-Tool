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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
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
            this.statusStripCCpositions = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusMessages = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
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
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonExportImport, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.buttonMoveSaves, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 1);
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
            //this.listBox2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listBox2_MouseDown);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Existing saves";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(403, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Exporting";
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
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
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
    }
}