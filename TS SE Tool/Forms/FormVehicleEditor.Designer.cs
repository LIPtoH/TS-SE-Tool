namespace TS_SE_Tool
{
    partial class FormVehicleEditor
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
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxAccessoryData = new System.Windows.Forms.TextBox();
            this.panelEditButtons = new System.Windows.Forms.Panel();
            this.buttonPaste = new System.Windows.Forms.Button();
            this.buttonCopy = new System.Windows.Forms.Button();
            this.buttonEdit = new System.Windows.Forms.Button();
            this.dataGridViewAccessories = new System.Windows.Forms.DataGridView();
            this.nameless = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ico = new System.Windows.Forms.DataGridViewImageColumn();
            this.AccName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Delete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonApply = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panelEditButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAccessories)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(584, 411);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 230F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.textBoxAccessoryData, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.panelEditButtons, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.dataGridViewAccessories, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 33);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(584, 345);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // textBoxAccessoryData
            // 
            this.textBoxAccessoryData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxAccessoryData.Location = new System.Drawing.Point(233, 3);
            this.textBoxAccessoryData.Multiline = true;
            this.textBoxAccessoryData.Name = "textBoxAccessoryData";
            this.textBoxAccessoryData.ReadOnly = true;
            this.textBoxAccessoryData.Size = new System.Drawing.Size(348, 309);
            this.textBoxAccessoryData.TabIndex = 1;
            this.textBoxAccessoryData.WordWrap = false;
            this.textBoxAccessoryData.ReadOnlyChanged += new System.EventHandler(this.textBoxAccessoryData_ReadOnlyChanged);
            // 
            // panelEditButtons
            // 
            this.panelEditButtons.Controls.Add(this.buttonPaste);
            this.panelEditButtons.Controls.Add(this.buttonCopy);
            this.panelEditButtons.Controls.Add(this.buttonEdit);
            this.panelEditButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEditButtons.Location = new System.Drawing.Point(230, 315);
            this.panelEditButtons.Margin = new System.Windows.Forms.Padding(0);
            this.panelEditButtons.Name = "panelEditButtons";
            this.panelEditButtons.Size = new System.Drawing.Size(354, 30);
            this.panelEditButtons.TabIndex = 2;
            // 
            // buttonPaste
            // 
            this.buttonPaste.Location = new System.Drawing.Point(262, 3);
            this.buttonPaste.Name = "buttonPaste";
            this.buttonPaste.Size = new System.Drawing.Size(89, 23);
            this.buttonPaste.TabIndex = 3;
            this.buttonPaste.Text = "Paste";
            this.buttonPaste.UseVisualStyleBackColor = true;
            this.buttonPaste.Click += new System.EventHandler(this.buttonPaste_Click);
            // 
            // buttonCopy
            // 
            this.buttonCopy.Location = new System.Drawing.Point(167, 3);
            this.buttonCopy.Name = "buttonCopy";
            this.buttonCopy.Size = new System.Drawing.Size(89, 23);
            this.buttonCopy.TabIndex = 2;
            this.buttonCopy.Text = "Copy";
            this.buttonCopy.UseVisualStyleBackColor = true;
            this.buttonCopy.Click += new System.EventHandler(this.buttonCopy_Click);
            // 
            // buttonEdit
            // 
            this.buttonEdit.Location = new System.Drawing.Point(4, 3);
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Size = new System.Drawing.Size(121, 23);
            this.buttonEdit.TabIndex = 1;
            this.buttonEdit.Text = "Edit";
            this.buttonEdit.UseVisualStyleBackColor = true;
            this.buttonEdit.Click += new System.EventHandler(this.buttonEditAccessory_Click);
            // 
            // dataGridViewAccessories
            // 
            this.dataGridViewAccessories.AllowUserToAddRows = false;
            this.dataGridViewAccessories.AllowUserToDeleteRows = false;
            this.dataGridViewAccessories.AllowUserToResizeColumns = false;
            this.dataGridViewAccessories.AllowUserToResizeRows = false;
            this.dataGridViewAccessories.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridViewAccessories.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridViewAccessories.ColumnHeadersVisible = false;
            this.dataGridViewAccessories.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameless,
            this.ico,
            this.AccName,
            this.Delete});
            this.dataGridViewAccessories.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewAccessories.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridViewAccessories.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewAccessories.MultiSelect = false;
            this.dataGridViewAccessories.Name = "dataGridViewAccessories";
            this.dataGridViewAccessories.ReadOnly = true;
            this.dataGridViewAccessories.RowHeadersVisible = false;
            this.dataGridViewAccessories.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.tableLayoutPanel2.SetRowSpan(this.dataGridViewAccessories, 2);
            this.dataGridViewAccessories.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridViewAccessories.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewAccessories.Size = new System.Drawing.Size(224, 339);
            this.dataGridViewAccessories.TabIndex = 3;
            this.dataGridViewAccessories.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewAccessories_CellContentClick);
            this.dataGridViewAccessories.SelectionChanged += new System.EventHandler(this.dataGridViewAccessories_SelectionChanged);
            // 
            // nameless
            // 
            this.nameless.HeaderText = "nameless";
            this.nameless.Name = "nameless";
            this.nameless.ReadOnly = true;
            this.nameless.Width = 25;
            this.nameless.Visible = false;
            // 
            // ico
            // 
            this.ico.HeaderText = "ico";
            this.ico.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.ico.Name = "ico";
            this.ico.ReadOnly = true;
            this.ico.Width = 25;
            // 
            // AccName
            // 
            this.AccName.HeaderText = "Accessory";
            this.AccName.Name = "AccName";
            this.AccName.ReadOnly = true;
            this.AccName.Width = 145;
            // 
            // Delete
            // 
            this.Delete.HeaderText = "Delete";
            this.Delete.Name = "Delete";
            this.Delete.ReadOnly = true;
            this.Delete.Width = 30;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.buttonCancel, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.buttonApply, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 378);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(584, 33);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCancel.Location = new System.Drawing.Point(295, 3);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(286, 27);
            this.buttonCancel.TabIndex = 0;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonApply
            // 
            this.buttonApply.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonApply.Location = new System.Drawing.Point(3, 3);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(286, 27);
            this.buttonApply.TabIndex = 1;
            this.buttonApply.Text = "Apply";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // FormVehicleEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 411);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormVehicleEditor";
            this.Text = "FormVehicleEditor";
            this.Load += new System.EventHandler(this.FormVehicleEditor_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.panelEditButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAccessories)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TextBox textBoxAccessoryData;
        private System.Windows.Forms.Panel panelEditButtons;
        private System.Windows.Forms.Button buttonEdit;
        private System.Windows.Forms.Button buttonCopy;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.DataGridView dataGridViewAccessories;
        private System.Windows.Forms.Button buttonPaste;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameless;
        private System.Windows.Forms.DataGridViewImageColumn ico;
        private System.Windows.Forms.DataGridViewTextBoxColumn AccName;
        private System.Windows.Forms.DataGridViewButtonColumn Delete;
    }
}