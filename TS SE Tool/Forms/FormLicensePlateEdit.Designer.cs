namespace TS_SE_Tool
{
    partial class FormLicensePlateEdit
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
            this.panelLicensePlatePreview = new System.Windows.Forms.Panel();
            this.textBoxLicensePlateNumber = new System.Windows.Forms.TextBox();
            this.textBoxLicensePlateCountry = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.labelLicensePlate = new System.Windows.Forms.Label();
            this.labelCountry = new System.Windows.Forms.Label();
            this.labelLicensePlateTagsHelp = new System.Windows.Forms.Label();
            this.buttonShowTagHelp = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.labelLicensePlateTagsHelp2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 134F));
            this.tableLayoutPanel1.Controls.Add(this.panelLicensePlatePreview, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.textBoxLicensePlateNumber, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBoxLicensePlateCountry, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.labelLicensePlate, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelCountry, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(460, 213);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panelLicensePlatePreview
            // 
            this.panelLicensePlatePreview.Location = new System.Drawing.Point(329, 49);
            this.panelLicensePlatePreview.Name = "panelLicensePlatePreview";
            this.panelLicensePlatePreview.Size = new System.Drawing.Size(128, 32);
            this.panelLicensePlatePreview.TabIndex = 1;
            // 
            // textBoxLicensePlateNumber
            // 
            this.textBoxLicensePlateNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLicensePlateNumber.Location = new System.Drawing.Point(3, 23);
            this.textBoxLicensePlateNumber.Multiline = true;
            this.textBoxLicensePlateNumber.Name = "textBoxLicensePlateNumber";
            this.tableLayoutPanel1.SetRowSpan(this.textBoxLicensePlateNumber, 2);
            this.textBoxLicensePlateNumber.Size = new System.Drawing.Size(320, 58);
            this.textBoxLicensePlateNumber.TabIndex = 2;
            this.textBoxLicensePlateNumber.TextChanged += new System.EventHandler(this.textBoxLicensePlateNumber_TextChanged);
            // 
            // textBoxLicensePlateCountry
            // 
            this.textBoxLicensePlateCountry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLicensePlateCountry.Location = new System.Drawing.Point(329, 23);
            this.textBoxLicensePlateCountry.Name = "textBoxLicensePlateCountry";
            this.textBoxLicensePlateCountry.Size = new System.Drawing.Size(128, 20);
            this.textBoxLicensePlateCountry.TabIndex = 3;
            this.textBoxLicensePlateCountry.TextChanged += new System.EventHandler(this.textBoxLicensePlateNumber_TextChanged);
            // 
            // panel1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.panel1, 2);
            this.panel1.Controls.Add(this.buttonCancel);
            this.panel1.Controls.Add(this.buttonOk);
            this.panel1.Controls.Add(this.buttonShowTagHelp);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 157);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(460, 36);
            this.panel1.TabIndex = 4;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(373, 3);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(84, 29);
            this.buttonCancel.TabIndex = 0;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOk.Location = new System.Drawing.Point(282, 3);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(85, 29);
            this.buttonOk.TabIndex = 1;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // labelLicensePlate
            // 
            this.labelLicensePlate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelLicensePlate.AutoSize = true;
            this.labelLicensePlate.Location = new System.Drawing.Point(3, 7);
            this.labelLicensePlate.Name = "labelLicensePlate";
            this.labelLicensePlate.Size = new System.Drawing.Size(71, 13);
            this.labelLicensePlate.TabIndex = 5;
            this.labelLicensePlate.Text = "License Plate";
            // 
            // labelCountry
            // 
            this.labelCountry.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelCountry.AutoSize = true;
            this.labelCountry.Location = new System.Drawing.Point(329, 7);
            this.labelCountry.Name = "labelCountry";
            this.labelCountry.Size = new System.Drawing.Size(43, 13);
            this.labelCountry.TabIndex = 6;
            this.labelCountry.Text = "Country";
            // 
            // labelLicensePlateTagsHelp
            // 
            this.labelLicensePlateTagsHelp.AutoSize = true;
            this.labelLicensePlateTagsHelp.BackColor = System.Drawing.Color.Transparent;
            this.labelLicensePlateTagsHelp.Location = new System.Drawing.Point(3, 3);
            this.labelLicensePlateTagsHelp.Name = "labelLicensePlateTagsHelp";
            this.labelLicensePlateTagsHelp.Size = new System.Drawing.Size(31, 13);
            this.labelLicensePlateTagsHelp.TabIndex = 7;
            this.labelLicensePlateTagsHelp.Text = "Tags";
            // 
            // buttonShowTagHelp
            // 
            this.buttonShowTagHelp.Location = new System.Drawing.Point(3, 3);
            this.buttonShowTagHelp.Name = "buttonShowTagHelp";
            this.buttonShowTagHelp.Size = new System.Drawing.Size(128, 29);
            this.buttonShowTagHelp.TabIndex = 8;
            this.buttonShowTagHelp.Text = "Expand help";
            this.buttonShowTagHelp.UseVisualStyleBackColor = true;
            this.buttonShowTagHelp.Click += new System.EventHandler(this.buttonShowTagHelp_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.labelLicensePlateTagsHelp2);
            this.panel2.Controls.Add(this.labelLicensePlateTagsHelp);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 84);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(326, 73);
            this.panel2.TabIndex = 9;
            // 
            // labelLicensePlateTagsHelp2
            // 
            this.labelLicensePlateTagsHelp2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelLicensePlateTagsHelp2.AutoSize = true;
            this.labelLicensePlateTagsHelp2.BackColor = System.Drawing.Color.Transparent;
            this.labelLicensePlateTagsHelp2.Location = new System.Drawing.Point(263, 3);
            this.labelLicensePlateTagsHelp2.Name = "labelLicensePlateTagsHelp2";
            this.labelLicensePlateTagsHelp2.Size = new System.Drawing.Size(60, 13);
            this.labelLicensePlateTagsHelp2.TabIndex = 8;
            this.labelLicensePlateTagsHelp2.Text = "Description";
            this.labelLicensePlateTagsHelp2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // FormLicensePlateEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 213);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(476, 252);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(476, 202);
            this.Name = "FormLicensePlateEdit";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "FormLicensePlateEdit";
            this.Shown += new System.EventHandler(this.FormTruckLicensePlateEdit_Shown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panelLicensePlatePreview;
        private System.Windows.Forms.TextBox textBoxLicensePlateNumber;
        private System.Windows.Forms.TextBox textBoxLicensePlateCountry;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Label labelLicensePlate;
        private System.Windows.Forms.Label labelCountry;
        private System.Windows.Forms.Label labelLicensePlateTagsHelp;
        private System.Windows.Forms.Button buttonShowTagHelp;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label labelLicensePlateTagsHelp2;
    }
}