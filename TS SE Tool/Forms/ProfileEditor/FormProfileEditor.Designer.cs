namespace TS_SE_Tool
{
    partial class FormProfileEditor
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
            this.tableLayoutPanelProfileName = new System.Windows.Forms.TableLayoutPanel();
            this.labelProfileName = new System.Windows.Forms.Label();
            this.labelProfileNameValue = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonRenameProfile = new System.Windows.Forms.Button();
            this.buttonCloneProfile = new System.Windows.Forms.Button();
            this.buttonExportSettings = new System.Windows.Forms.Button();
            this.buttonImportSettings = new System.Windows.Forms.Button();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.labelSettings = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanelProfileName.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanelProfileName, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(310, 361);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanelProfileName
            // 
            this.tableLayoutPanelProfileName.ColumnCount = 2;
            this.tableLayoutPanelProfileName.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 84F));
            this.tableLayoutPanelProfileName.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelProfileName.Controls.Add(this.labelProfileName, 0, 0);
            this.tableLayoutPanelProfileName.Controls.Add(this.labelProfileNameValue, 1, 0);
            this.tableLayoutPanelProfileName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelProfileName.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelProfileName.Name = "tableLayoutPanelProfileName";
            this.tableLayoutPanelProfileName.RowCount = 1;
            this.tableLayoutPanelProfileName.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelProfileName.Size = new System.Drawing.Size(304, 34);
            this.tableLayoutPanelProfileName.TabIndex = 0;
            // 
            // labelProfileName
            // 
            this.labelProfileName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProfileName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelProfileName.Location = new System.Drawing.Point(3, 0);
            this.labelProfileName.Name = "labelProfileName";
            this.labelProfileName.Padding = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.labelProfileName.Size = new System.Drawing.Size(78, 34);
            this.labelProfileName.TabIndex = 3;
            this.labelProfileName.Text = "Profile Name";
            this.labelProfileName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelProfileNameValue
            // 
            this.labelProfileNameValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.labelProfileNameValue.AutoSize = true;
            this.labelProfileNameValue.Location = new System.Drawing.Point(87, 0);
            this.labelProfileNameValue.Name = "labelProfileNameValue";
            this.labelProfileNameValue.Size = new System.Drawing.Size(35, 34);
            this.labelProfileNameValue.TabIndex = 4;
            this.labelProfileNameValue.Text = "Name";
            this.labelProfileNameValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.buttonRenameProfile, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.buttonCloneProfile, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.buttonExportSettings, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.buttonImportSettings, 0, 4);
            this.tableLayoutPanel3.Controls.Add(this.labelSettings, 0, 2);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 43);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 6;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(304, 275);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // buttonRenameProfile
            // 
            this.buttonRenameProfile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonRenameProfile.Location = new System.Drawing.Point(3, 3);
            this.buttonRenameProfile.Name = "buttonRenameProfile";
            this.buttonRenameProfile.Size = new System.Drawing.Size(298, 24);
            this.buttonRenameProfile.TabIndex = 1;
            this.buttonRenameProfile.Text = "Rename Profile";
            this.buttonRenameProfile.UseVisualStyleBackColor = true;
            this.buttonRenameProfile.Click += new System.EventHandler(this.buttonRenameProfile_Click);
            // 
            // buttonCloneProfile
            // 
            this.buttonCloneProfile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCloneProfile.Location = new System.Drawing.Point(3, 33);
            this.buttonCloneProfile.Name = "buttonCloneProfile";
            this.buttonCloneProfile.Size = new System.Drawing.Size(298, 24);
            this.buttonCloneProfile.TabIndex = 2;
            this.buttonCloneProfile.Text = "Clone Profile";
            this.buttonCloneProfile.UseVisualStyleBackColor = true;
            this.buttonCloneProfile.Click += new System.EventHandler(this.buttonCloneProfile_Click);
            // 
            // buttonExportSettings
            // 
            this.buttonExportSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonExportSettings.Location = new System.Drawing.Point(3, 93);
            this.buttonExportSettings.Name = "buttonExportSettings";
            this.buttonExportSettings.Size = new System.Drawing.Size(298, 24);
            this.buttonExportSettings.TabIndex = 3;
            this.buttonExportSettings.Text = "Export";
            this.buttonExportSettings.UseVisualStyleBackColor = true;
            this.buttonExportSettings.Click += new System.EventHandler(this.buttonExportSettings_Click);
            // 
            // buttonImportSettings
            // 
            this.buttonImportSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonImportSettings.Location = new System.Drawing.Point(3, 123);
            this.buttonImportSettings.Name = "buttonImportSettings";
            this.buttonImportSettings.Size = new System.Drawing.Size(298, 24);
            this.buttonImportSettings.TabIndex = 4;
            this.buttonImportSettings.Text = "Import";
            this.buttonImportSettings.UseVisualStyleBackColor = true;
            this.buttonImportSettings.Click += new System.EventHandler(this.buttonImportSettings_Click);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.buttonSave, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.buttonCancel, 1, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 324);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(304, 34);
            this.tableLayoutPanel4.TabIndex = 2;
            // 
            // buttonSave
            // 
            this.buttonSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSave.Location = new System.Drawing.Point(3, 3);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(146, 28);
            this.buttonSave.TabIndex = 0;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCancel.Location = new System.Drawing.Point(155, 3);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(146, 28);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // labelSettings
            // 
            this.labelSettings.AutoSize = true;
            this.labelSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSettings.Location = new System.Drawing.Point(3, 60);
            this.labelSettings.Name = "labelSettings";
            this.labelSettings.Size = new System.Drawing.Size(298, 30);
            this.labelSettings.TabIndex = 5;
            this.labelSettings.Text = "Settings";
            this.labelSettings.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormProfileEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(310, 361);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(326, 400);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(326, 400);
            this.Name = "FormProfileEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Profile Manager";
            this.Load += new System.EventHandler(this.FormProfileEditor_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanelProfileName.ResumeLayout(false);
            this.tableLayoutPanelProfileName.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label labelProfileName;
        private System.Windows.Forms.Button buttonRenameProfile;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button buttonCloneProfile;
        private System.Windows.Forms.Button buttonImportSettings;
        private System.Windows.Forms.Button buttonExportSettings;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelProfileName;
        private System.Windows.Forms.Label labelProfileNameValue;
        private System.Windows.Forms.Label labelSettings;
    }
}