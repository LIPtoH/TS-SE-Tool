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
            this.groupBoxImportedColors = new System.Windows.Forms.GroupBox();
            this.buttonImport = new System.Windows.Forms.Button();
            this.buttonExport = new System.Windows.Forms.Button();
            this.buttonReplaceColors = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // groupBoxProfileUserColors
            // 
            this.groupBoxProfileUserColors.Location = new System.Drawing.Point(12, 12);
            this.groupBoxProfileUserColors.Name = "groupBoxProfileUserColors";
            this.groupBoxProfileUserColors.Size = new System.Drawing.Size(472, 100);
            this.groupBoxProfileUserColors.TabIndex = 0;
            this.groupBoxProfileUserColors.TabStop = false;
            this.groupBoxProfileUserColors.Text = "User colors";
            // 
            // groupBoxImportedColors
            // 
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
            // FormShareUserColors
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(496, 284);
            this.Controls.Add(this.buttonReplaceColors);
            this.Controls.Add(this.buttonExport);
            this.Controls.Add(this.buttonImport);
            this.Controls.Add(this.groupBoxImportedColors);
            this.Controls.Add(this.groupBoxProfileUserColors);
            this.Name = "FormShareUserColors";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Share User Colors";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxProfileUserColors;
        private System.Windows.Forms.GroupBox groupBoxImportedColors;
        private System.Windows.Forms.Button buttonImport;
        private System.Windows.Forms.Button buttonExport;
        private System.Windows.Forms.Button buttonReplaceColors;
    }
}