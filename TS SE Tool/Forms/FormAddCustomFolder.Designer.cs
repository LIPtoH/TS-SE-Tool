namespace TS_SE_Tool
{
    partial class FormAddCustomFolder
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.labelCustomPathDir = new System.Windows.Forms.Label();
            this.buttonChooseFolder = new System.Windows.Forms.Button();
            this.groupBoxFolderType = new System.Windows.Forms.GroupBox();
            this.radioButtonUnknownFolderType = new System.Windows.Forms.RadioButton();
            this.radioButtonSaveFolderType = new System.Windows.Forms.RadioButton();
            this.radioButtonProfileFolderType = new System.Windows.Forms.RadioButton();
            this.radioButtonRootFolderType = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonAddCustomPath = new System.Windows.Forms.Button();
            this.groupBoxGameType = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.radioButtonGameTypeETS2 = new System.Windows.Forms.RadioButton();
            this.radioButtonGameTypeATS = new System.Windows.Forms.RadioButton();
            this.buttonEditCPlist = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Path = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Del = new System.Windows.Forms.DataGridViewButtonColumn();
            this.buttonSave = new System.Windows.Forms.Button();
            this.folderBrowserDialogAddCustomFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBoxFolderType.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBoxGameType.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // labelCustomPathDir
            // 
            this.labelCustomPathDir.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelCustomPathDir.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.labelCustomPathDir, 2);
            this.labelCustomPathDir.Location = new System.Drawing.Point(3, 38);
            this.labelCustomPathDir.Name = "labelCustomPathDir";
            this.labelCustomPathDir.Size = new System.Drawing.Size(72, 13);
            this.labelCustomPathDir.TabIndex = 0;
            this.labelCustomPathDir.Text = "Choose folder";
            // 
            // buttonChooseFolder
            // 
            this.buttonChooseFolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonChooseFolder.Location = new System.Drawing.Point(3, 3);
            this.buttonChooseFolder.Name = "buttonChooseFolder";
            this.buttonChooseFolder.Size = new System.Drawing.Size(180, 24);
            this.buttonChooseFolder.TabIndex = 1;
            this.buttonChooseFolder.Text = "Choose folder";
            this.buttonChooseFolder.UseVisualStyleBackColor = true;
            this.buttonChooseFolder.Click += new System.EventHandler(this.buttonChooseFolder_Click);
            // 
            // groupBoxFolderType
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.groupBoxFolderType, 2);
            this.groupBoxFolderType.Controls.Add(this.radioButtonUnknownFolderType);
            this.groupBoxFolderType.Controls.Add(this.radioButtonSaveFolderType);
            this.groupBoxFolderType.Controls.Add(this.radioButtonProfileFolderType);
            this.groupBoxFolderType.Controls.Add(this.radioButtonRootFolderType);
            this.groupBoxFolderType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxFolderType.Enabled = false;
            this.groupBoxFolderType.Location = new System.Drawing.Point(3, 63);
            this.groupBoxFolderType.Name = "groupBoxFolderType";
            this.groupBoxFolderType.Size = new System.Drawing.Size(366, 119);
            this.groupBoxFolderType.TabIndex = 2;
            this.groupBoxFolderType.TabStop = false;
            this.groupBoxFolderType.Text = "Folder type";
            // 
            // radioButtonUnknownFolderType
            // 
            this.radioButtonUnknownFolderType.AutoSize = true;
            this.radioButtonUnknownFolderType.Checked = true;
            this.radioButtonUnknownFolderType.Location = new System.Drawing.Point(7, 20);
            this.radioButtonUnknownFolderType.Name = "radioButtonUnknownFolderType";
            this.radioButtonUnknownFolderType.Size = new System.Drawing.Size(71, 17);
            this.radioButtonUnknownFolderType.TabIndex = 3;
            this.radioButtonUnknownFolderType.TabStop = true;
            this.radioButtonUnknownFolderType.Text = "Unknown";
            this.radioButtonUnknownFolderType.UseVisualStyleBackColor = true;
            this.radioButtonUnknownFolderType.CheckedChanged += new System.EventHandler(this.radioButtonFolderType_CheckedChanged);
            // 
            // radioButtonSaveFolderType
            // 
            this.radioButtonSaveFolderType.AutoSize = true;
            this.radioButtonSaveFolderType.Location = new System.Drawing.Point(7, 91);
            this.radioButtonSaveFolderType.Name = "radioButtonSaveFolderType";
            this.radioButtonSaveFolderType.Size = new System.Drawing.Size(80, 17);
            this.radioButtonSaveFolderType.TabIndex = 2;
            this.radioButtonSaveFolderType.Text = "save Folder";
            this.radioButtonSaveFolderType.UseVisualStyleBackColor = true;
            this.radioButtonSaveFolderType.CheckedChanged += new System.EventHandler(this.radioButtonFolderType_CheckedChanged);
            // 
            // radioButtonProfileFolderType
            // 
            this.radioButtonProfileFolderType.AutoSize = true;
            this.radioButtonProfileFolderType.Location = new System.Drawing.Point(7, 67);
            this.radioButtonProfileFolderType.Name = "radioButtonProfileFolderType";
            this.radioButtonProfileFolderType.Size = new System.Drawing.Size(85, 17);
            this.radioButtonProfileFolderType.TabIndex = 1;
            this.radioButtonProfileFolderType.Text = "profile Folder";
            this.radioButtonProfileFolderType.UseVisualStyleBackColor = true;
            this.radioButtonProfileFolderType.CheckedChanged += new System.EventHandler(this.radioButtonFolderType_CheckedChanged);
            // 
            // radioButtonRootFolderType
            // 
            this.radioButtonRootFolderType.AutoSize = true;
            this.radioButtonRootFolderType.Location = new System.Drawing.Point(7, 43);
            this.radioButtonRootFolderType.Name = "radioButtonRootFolderType";
            this.radioButtonRootFolderType.Size = new System.Drawing.Size(75, 17);
            this.radioButtonRootFolderType.TabIndex = 0;
            this.radioButtonRootFolderType.Text = "root Folder";
            this.radioButtonRootFolderType.UseVisualStyleBackColor = true;
            this.radioButtonRootFolderType.CheckedChanged += new System.EventHandler(this.radioButtonFolderType_CheckedChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 302F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.buttonChooseFolder, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBoxFolderType, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelCustomPathDir, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonAddCustomPath, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.groupBoxGameType, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.buttonEditCPlist, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.dataGridView1, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonSave, 1, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 66F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(674, 281);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // buttonAddCustomPath
            // 
            this.buttonAddCustomPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonAddCustomPath.Enabled = false;
            this.buttonAddCustomPath.Location = new System.Drawing.Point(3, 254);
            this.buttonAddCustomPath.Name = "buttonAddCustomPath";
            this.buttonAddCustomPath.Size = new System.Drawing.Size(180, 24);
            this.buttonAddCustomPath.TabIndex = 3;
            this.buttonAddCustomPath.Text = "ADD";
            this.buttonAddCustomPath.UseVisualStyleBackColor = true;
            this.buttonAddCustomPath.Click += new System.EventHandler(this.buttonAddCustomPath_Click);
            // 
            // groupBoxGameType
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.groupBoxGameType, 2);
            this.groupBoxGameType.Controls.Add(this.tableLayoutPanel2);
            this.groupBoxGameType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxGameType.Location = new System.Drawing.Point(3, 188);
            this.groupBoxGameType.Name = "groupBoxGameType";
            this.groupBoxGameType.Size = new System.Drawing.Size(366, 60);
            this.groupBoxGameType.TabIndex = 4;
            this.groupBoxGameType.TabStop = false;
            this.groupBoxGameType.Text = "Game type";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.radioButtonGameTypeETS2, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.radioButtonGameTypeATS, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(360, 41);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // radioButtonGameTypeETS2
            // 
            this.radioButtonGameTypeETS2.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButtonGameTypeETS2.AutoSize = true;
            this.radioButtonGameTypeETS2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonGameTypeETS2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.radioButtonGameTypeETS2.Location = new System.Drawing.Point(3, 3);
            this.radioButtonGameTypeETS2.Name = "radioButtonGameTypeETS2";
            this.radioButtonGameTypeETS2.Size = new System.Drawing.Size(174, 35);
            this.radioButtonGameTypeETS2.TabIndex = 0;
            this.radioButtonGameTypeETS2.TabStop = true;
            this.radioButtonGameTypeETS2.Text = "ETS 2";
            this.radioButtonGameTypeETS2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButtonGameTypeETS2.UseVisualStyleBackColor = true;
            this.radioButtonGameTypeETS2.CheckedChanged += new System.EventHandler(this.radioButtonGameType_CheckedChanged);
            // 
            // radioButtonGameTypeATS
            // 
            this.radioButtonGameTypeATS.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButtonGameTypeATS.AutoSize = true;
            this.radioButtonGameTypeATS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonGameTypeATS.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.radioButtonGameTypeATS.Location = new System.Drawing.Point(183, 3);
            this.radioButtonGameTypeATS.Name = "radioButtonGameTypeATS";
            this.radioButtonGameTypeATS.Size = new System.Drawing.Size(174, 35);
            this.radioButtonGameTypeATS.TabIndex = 1;
            this.radioButtonGameTypeATS.TabStop = true;
            this.radioButtonGameTypeATS.Text = "ATS";
            this.radioButtonGameTypeATS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButtonGameTypeATS.UseVisualStyleBackColor = true;
            // 
            // buttonEditCPlist
            // 
            this.buttonEditCPlist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonEditCPlist.Location = new System.Drawing.Point(189, 3);
            this.buttonEditCPlist.Name = "buttonEditCPlist";
            this.buttonEditCPlist.Size = new System.Drawing.Size(180, 24);
            this.buttonEditCPlist.TabIndex = 5;
            this.buttonEditCPlist.Text = "Edit list";
            this.buttonEditCPlist.UseVisualStyleBackColor = true;
            this.buttonEditCPlist.Click += new System.EventHandler(this.buttonEditCPlist_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Path,
            this.Del});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(375, 3);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.tableLayoutPanel1.SetRowSpan(this.dataGridView1, 4);
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(296, 245);
            this.dataGridView1.TabIndex = 6;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // Path
            // 
            this.Path.DataPropertyName = "Path";
            this.Path.HeaderText = "Path";
            this.Path.MinimumWidth = 200;
            this.Path.Name = "Path";
            this.Path.ReadOnly = true;
            this.Path.Width = 200;
            // 
            // Del
            // 
            this.Del.HeaderText = "Delete";
            this.Del.MinimumWidth = 50;
            this.Del.Name = "Del";
            this.Del.ReadOnly = true;
            this.Del.Text = "Delete";
            this.Del.UseColumnTextForButtonValue = true;
            this.Del.Width = 50;
            // 
            // buttonSave
            // 
            this.buttonSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSave.Enabled = false;
            this.buttonSave.Location = new System.Drawing.Point(189, 254);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(180, 24);
            this.buttonSave.TabIndex = 7;
            this.buttonSave.Text = "SAVE";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // FormAddCustomFolder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(674, 281);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAddCustomFolder";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Custom folders";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormAddCustomFolder_FormClosing);
            this.groupBoxFolderType.ResumeLayout(false);
            this.groupBoxFolderType.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBoxGameType.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelCustomPathDir;
        private System.Windows.Forms.Button buttonChooseFolder;
        private System.Windows.Forms.GroupBox groupBoxFolderType;
        private System.Windows.Forms.RadioButton radioButtonSaveFolderType;
        private System.Windows.Forms.RadioButton radioButtonProfileFolderType;
        private System.Windows.Forms.RadioButton radioButtonRootFolderType;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.RadioButton radioButtonUnknownFolderType;
        private System.Windows.Forms.Button buttonAddCustomPath;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogAddCustomFolder;
        private System.Windows.Forms.GroupBox groupBoxGameType;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.RadioButton radioButtonGameTypeETS2;
        private System.Windows.Forms.RadioButton radioButtonGameTypeATS;
        private System.Windows.Forms.Button buttonEditCPlist;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Path;
        private System.Windows.Forms.DataGridViewButtonColumn Del;
        private System.Windows.Forms.Button buttonSave;
    }
}