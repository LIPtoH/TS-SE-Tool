/*
   Copyright 2016-2022 LIPtoH <liptoh.codebase@gmail.com>

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/
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
            this.panelImportedColors = new System.Windows.Forms.Panel();
            this.buttonImport = new System.Windows.Forms.Button();
            this.buttonExport = new System.Windows.Forms.Button();
            this.buttonApply = new System.Windows.Forms.Button();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.panelShareColorsHelpDragDrop = new System.Windows.Forms.Panel();
            this.groupBoxProfileUserColors.SuspendLayout();
            this.groupBoxImportedColors.SuspendLayout();
            this.tableLayoutPanelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxProfileUserColors
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.groupBoxProfileUserColors, 3);
            this.groupBoxProfileUserColors.Controls.Add(this.panelProfileUserColors);
            this.groupBoxProfileUserColors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxProfileUserColors.Location = new System.Drawing.Point(3, 3);
            this.groupBoxProfileUserColors.Name = "groupBoxProfileUserColors";
            this.groupBoxProfileUserColors.Size = new System.Drawing.Size(490, 117);
            this.groupBoxProfileUserColors.TabIndex = 0;
            this.groupBoxProfileUserColors.TabStop = false;
            this.groupBoxProfileUserColors.Text = "User colors";
            // 
            // panelProfileUserColors
            // 
            this.panelProfileUserColors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelProfileUserColors.Location = new System.Drawing.Point(3, 16);
            this.panelProfileUserColors.Name = "panelProfileUserColors";
            this.panelProfileUserColors.Size = new System.Drawing.Size(484, 98);
            this.panelProfileUserColors.TabIndex = 0;
            // 
            // groupBoxImportedColors
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.groupBoxImportedColors, 3);
            this.groupBoxImportedColors.Controls.Add(this.panelImportedColors);
            this.groupBoxImportedColors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxImportedColors.Location = new System.Drawing.Point(3, 201);
            this.groupBoxImportedColors.Name = "groupBoxImportedColors";
            this.groupBoxImportedColors.Size = new System.Drawing.Size(490, 117);
            this.groupBoxImportedColors.TabIndex = 1;
            this.groupBoxImportedColors.TabStop = false;
            this.groupBoxImportedColors.Text = "Imported colors";
            // 
            // panelImportedColors
            // 
            this.panelImportedColors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelImportedColors.Location = new System.Drawing.Point(3, 16);
            this.panelImportedColors.Name = "panelImportedColors";
            this.panelImportedColors.Size = new System.Drawing.Size(484, 98);
            this.panelImportedColors.TabIndex = 0;
            // 
            // buttonImport
            // 
            this.buttonImport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonImport.Location = new System.Drawing.Point(176, 126);
            this.buttonImport.Name = "buttonImport";
            this.buttonImport.Size = new System.Drawing.Size(167, 44);
            this.buttonImport.TabIndex = 2;
            this.buttonImport.Text = "Import";
            this.buttonImport.UseVisualStyleBackColor = true;
            this.buttonImport.Click += new System.EventHandler(this.buttonImportColors_Click);
            // 
            // buttonExport
            // 
            this.buttonExport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonExport.Enabled = false;
            this.buttonExport.Location = new System.Drawing.Point(3, 126);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(167, 44);
            this.buttonExport.TabIndex = 3;
            this.buttonExport.Text = "Export";
            this.buttonExport.UseVisualStyleBackColor = true;
            this.buttonExport.Click += new System.EventHandler(this.buttonExportColors_Click);
            // 
            // buttonApply
            // 
            this.buttonApply.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonApply.Location = new System.Drawing.Point(349, 126);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(144, 44);
            this.buttonApply.TabIndex = 4;
            this.buttonApply.Text = "Apply";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApplyChanges_Click);
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 3;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanelMain.Controls.Add(this.groupBoxProfileUserColors, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.groupBoxImportedColors, 0, 3);
            this.tableLayoutPanelMain.Controls.Add(this.buttonImport, 1, 1);
            this.tableLayoutPanelMain.Controls.Add(this.buttonApply, 2, 1);
            this.tableLayoutPanelMain.Controls.Add(this.buttonExport, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.panelShareColorsHelpDragDrop, 0, 2);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 4;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(496, 321);
            this.tableLayoutPanelMain.TabIndex = 5;
            // 
            // panelShareColorsHelpDragDrop
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.panelShareColorsHelpDragDrop, 3);
            this.panelShareColorsHelpDragDrop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelShareColorsHelpDragDrop.Location = new System.Drawing.Point(0, 173);
            this.panelShareColorsHelpDragDrop.Margin = new System.Windows.Forms.Padding(0);
            this.panelShareColorsHelpDragDrop.Name = "panelShareColorsHelpDragDrop";
            this.panelShareColorsHelpDragDrop.Size = new System.Drawing.Size(496, 25);
            this.panelShareColorsHelpDragDrop.TabIndex = 5;
            // 
            // FormShareUserColors
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(496, 321);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(512, 360);
            this.MinimizeBox = false;
            this.Name = "FormShareUserColors";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Share User Colors";
            this.groupBoxProfileUserColors.ResumeLayout(false);
            this.groupBoxImportedColors.ResumeLayout(false);
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxProfileUserColors;
        private System.Windows.Forms.GroupBox groupBoxImportedColors;
        private System.Windows.Forms.Button buttonImport;
        private System.Windows.Forms.Button buttonExport;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.Panel panelProfileUserColors;
        private System.Windows.Forms.Panel panelImportedColors;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.Panel panelShareColorsHelpDragDrop;
    }
}