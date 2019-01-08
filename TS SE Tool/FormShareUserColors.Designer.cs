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
            this.groupBoxUserColors = new System.Windows.Forms.GroupBox();
            this.groupBoxImportedColors = new System.Windows.Forms.GroupBox();
            this.buttonImportColors = new System.Windows.Forms.Button();
            this.buttonExportColors = new System.Windows.Forms.Button();
            this.buttonReplaceColors = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // groupBoxUserColors
            // 
            this.groupBoxUserColors.Location = new System.Drawing.Point(12, 12);
            this.groupBoxUserColors.Name = "groupBoxUserColors";
            this.groupBoxUserColors.Size = new System.Drawing.Size(472, 100);
            this.groupBoxUserColors.TabIndex = 0;
            this.groupBoxUserColors.TabStop = false;
            this.groupBoxUserColors.Text = "User colors";
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
            // buttonImportColors
            // 
            this.buttonImportColors.Location = new System.Drawing.Point(188, 118);
            this.buttonImportColors.Name = "buttonImportColors";
            this.buttonImportColors.Size = new System.Drawing.Size(170, 46);
            this.buttonImportColors.TabIndex = 2;
            this.buttonImportColors.Text = "Import";
            this.buttonImportColors.UseVisualStyleBackColor = true;
            this.buttonImportColors.Click += new System.EventHandler(this.buttonImportColors_Click);
            // 
            // buttonExportColors
            // 
            this.buttonExportColors.Location = new System.Drawing.Point(12, 118);
            this.buttonExportColors.Name = "buttonExportColors";
            this.buttonExportColors.Size = new System.Drawing.Size(170, 46);
            this.buttonExportColors.TabIndex = 3;
            this.buttonExportColors.Text = "Export";
            this.buttonExportColors.UseVisualStyleBackColor = true;
            this.buttonExportColors.Click += new System.EventHandler(this.buttonExportColors_Click);
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
            this.Controls.Add(this.buttonExportColors);
            this.Controls.Add(this.buttonImportColors);
            this.Controls.Add(this.groupBoxImportedColors);
            this.Controls.Add(this.groupBoxUserColors);
            this.Name = "FormShareUserColors";
            this.Text = "Share User Colors";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxUserColors;
        private System.Windows.Forms.GroupBox groupBoxImportedColors;
        private System.Windows.Forms.Button buttonImportColors;
        private System.Windows.Forms.Button buttonExportColors;
        private System.Windows.Forms.Button buttonReplaceColors;
    }
}