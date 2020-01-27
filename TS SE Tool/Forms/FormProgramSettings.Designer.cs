namespace TS_SE_Tool
{
    partial class FormProgramSettings
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
            this.labelShowSplashOnStartup = new System.Windows.Forms.Label();
            this.labelCheckUpdatesOnStartup = new System.Windows.Forms.Label();
            this.checkBoxShowSplashOnStartup = new System.Windows.Forms.CheckBox();
            this.checkBoxCheckUpdatesOnStartup = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 211F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.buttonSave, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.labelShowSplashOnStartup, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelCheckUpdatesOnStartup, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.checkBoxShowSplashOnStartup, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.checkBoxCheckUpdatesOnStartup, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(264, 219);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // buttonSave
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.buttonSave, 2);
            this.buttonSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.buttonSave.Location = new System.Drawing.Point(3, 182);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(258, 34);
            this.buttonSave.TabIndex = 2;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // labelShowSplashOnStartup
            // 
            this.labelShowSplashOnStartup.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelShowSplashOnStartup.AutoSize = true;
            this.labelShowSplashOnStartup.Location = new System.Drawing.Point(3, 38);
            this.labelShowSplashOnStartup.Name = "labelShowSplashOnStartup";
            this.labelShowSplashOnStartup.Size = new System.Drawing.Size(152, 13);
            this.labelShowSplashOnStartup.TabIndex = 3;
            this.labelShowSplashOnStartup.Text = "Show splash screen on startup";
            // 
            // labelCheckUpdatesOnStartup
            // 
            this.labelCheckUpdatesOnStartup.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelCheckUpdatesOnStartup.AutoSize = true;
            this.labelCheckUpdatesOnStartup.Location = new System.Drawing.Point(3, 68);
            this.labelCheckUpdatesOnStartup.Name = "labelCheckUpdatesOnStartup";
            this.labelCheckUpdatesOnStartup.Size = new System.Drawing.Size(129, 13);
            this.labelCheckUpdatesOnStartup.TabIndex = 0;
            this.labelCheckUpdatesOnStartup.Text = "Check updates on startup";
            // 
            // checkBoxShowSplashOnStartup
            // 
            this.checkBoxShowSplashOnStartup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxShowSplashOnStartup.AutoSize = true;
            this.checkBoxShowSplashOnStartup.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxShowSplashOnStartup.Location = new System.Drawing.Point(214, 38);
            this.checkBoxShowSplashOnStartup.Name = "checkBoxShowSplashOnStartup";
            this.checkBoxShowSplashOnStartup.Size = new System.Drawing.Size(47, 14);
            this.checkBoxShowSplashOnStartup.TabIndex = 5;
            this.checkBoxShowSplashOnStartup.UseVisualStyleBackColor = true;
            // 
            // checkBoxCheckUpdatesOnStartup
            // 
            this.checkBoxCheckUpdatesOnStartup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxCheckUpdatesOnStartup.AutoSize = true;
            this.checkBoxCheckUpdatesOnStartup.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxCheckUpdatesOnStartup.Location = new System.Drawing.Point(214, 68);
            this.checkBoxCheckUpdatesOnStartup.Name = "checkBoxCheckUpdatesOnStartup";
            this.checkBoxCheckUpdatesOnStartup.Size = new System.Drawing.Size(47, 14);
            this.checkBoxCheckUpdatesOnStartup.TabIndex = 1;
            this.checkBoxCheckUpdatesOnStartup.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxCheckUpdatesOnStartup.UseVisualStyleBackColor = true;
            this.checkBoxCheckUpdatesOnStartup.CheckedChanged += new System.EventHandler(this.checkBoxCheckUpdatesOnStartup_CheckedChanged);
            // 
            // FormProgramSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(264, 219);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormProgramSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Program Settings";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label labelCheckUpdatesOnStartup;
        private System.Windows.Forms.CheckBox checkBoxCheckUpdatesOnStartup;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Label labelShowSplashOnStartup;
        private System.Windows.Forms.CheckBox checkBoxShowSplashOnStartup;
    }
}