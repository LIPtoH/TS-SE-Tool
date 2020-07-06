namespace TS_SE_Tool
{
    partial class FormShareUserColors
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
            this.groupBoxProfileUserColors = new System.Windows.Forms.GroupBox();
            this.panelProfileUserColors = new System.Windows.Forms.Panel();
            this.groupBoxImportedColors = new System.Windows.Forms.GroupBox();
            this.buttonImport = new System.Windows.Forms.Button();
            this.buttonExport = new System.Windows.Forms.Button();
            this.buttonReplaceColors = new System.Windows.Forms.Button();
            this.panelImportedColors = new System.Windows.Forms.Panel();
            this.groupBoxProfileUserColors.SuspendLayout();
            this.groupBoxImportedColors.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxProfileUserColors
            // 
            this.groupBoxProfileUserColors.Controls.Add(this.panelProfileUserColors);
            this.groupBoxProfileUserColors.Location = new System.Drawing.Point(12, 12);
            this.groupBoxProfileUserColors.Name = "groupBoxProfileUserColors";
            this.groupBoxProfileUserColors.Size = new System.Drawing.Size(472, 100);
            this.groupBoxProfileUserColors.TabIndex = 0;
            this.groupBoxProfileUserColors.TabStop = false;
            this.groupBoxProfileUserColors.Text = "User colors";
            // 
            // panelProfileUserColors
            // 
            this.panelProfileUserColors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelProfileUserColors.Location = new System.Drawing.Point(3, 16);
            this.panelProfileUserColors.Name = "panelProfileUserColors";
            this.panelProfileUserColors.Size = new System.Drawing.Size(466, 81);
            this.panelProfileUserColors.TabIndex = 0;
            // 
            // groupBoxImportedColors
            // 
            this.groupBoxImportedColors.Controls.Add(this.panelImportedColors);
            this.groupBoxImportedColors.Location = new System.Drawing.Point(12, 170);
            this.groupBoxImportedColors.Name = "groupBoxImportedColors";
            this.groupBoxImportedColors.Size = new System.Drawing.Size(472, 100);
            this.groupBoxImportedColors.TabIndex = 1;
            this.groupBoxImportedColors.TabStop = false;
            this.groupBoxImportedColors.Text = "Imported colors";
            // 
            // buttonImport
            // 
            this.buttonImport.Location = new System.Drawing.Point(188, 118);
            this.buttonImport.Name = "buttonImport";
            this.buttonImport.Size = new System.Drawing.Size(170, 46);
            this.buttonImport.TabIndex = 2;
            this.buttonImport.Text = "Import";
            this.buttonImport.UseVisualStyleBackColor = true;
            this.buttonImport.Click += new System.EventHandler(this.buttonImportColors_Click);
            // 
            // buttonExport
            // 
            this.buttonExport.Location = new System.Drawing.Point(12, 118);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(170, 46);
            this.buttonExport.TabIndex = 3;
            this.buttonExport.Text = "Export";
            this.buttonExport.UseVisualStyleBackColor = true;
            this.buttonExport.Click += new System.EventHandler(this.buttonExportColors_Click);
            // 
            // buttonReplaceColors
            // 
            this.buttonReplaceColors.Location = new System.Drawing.Point(364, 118);
            this.buttonReplaceColors.Name = "buttonReplaceColors";
            this.buttonReplaceColors.Size = new System.Drawing.Size(120, 46);
            this.buttonReplaceColors.TabIndex = 4;
            this.buttonReplaceColors.Text = "↑↑↑ Replace ↑↑↑";
            this.buttonReplaceColors.UseVisualStyleBackColor = true;
            this.buttonReplaceColors.Click += new System.EventHandler(this.buttonReplaceColors_Click);
            // 
            // panelImportedColors
            // 
            this.panelImportedColors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelImportedColors.Location = new System.Drawing.Point(3, 16);
            this.panelImportedColors.Name = "panelImportedColors";
            this.panelImportedColors.Size = new System.Drawing.Size(466, 81);
            this.panelImportedColors.TabIndex = 0;
            // 
            // FormShareUserColors
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(496, 281);
            this.Controls.Add(this.buttonReplaceColors);
            this.Controls.Add(this.buttonExport);
            this.Controls.Add(this.buttonImport);
            this.Controls.Add(this.groupBoxImportedColors);
            this.Controls.Add(this.groupBoxProfileUserColors);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(512, 320);
            this.MinimizeBox = false;
            this.Name = "FormShareUserColors";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Share User Colors";
            this.groupBoxProfileUserColors.ResumeLayout(false);
            this.groupBoxImportedColors.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxProfileUserColors;
        private System.Windows.Forms.GroupBox groupBoxImportedColors;
        private System.Windows.Forms.Button buttonImport;
        private System.Windows.Forms.Button buttonExport;
        private System.Windows.Forms.Button buttonReplaceColors;
        private System.Windows.Forms.Panel panelProfileUserColors;
        private System.Windows.Forms.Panel panelImportedColors;
    }
}