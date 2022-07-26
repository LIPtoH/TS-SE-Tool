namespace TS_SE_Tool
{
    partial class FormAIDriverEditor
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
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labelDriverName = new System.Windows.Forms.Label();
            this.groupBoxDriverSkill = new System.Windows.Forms.GroupBox();
            this.labelDriverNameText = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.toolTipAIDriverEditor = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.97235F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 82.02765F));
            this.tableLayoutPanel1.Controls.Add(this.labelDriverName, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBoxDriverSkill, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelDriverNameText, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(429, 549);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // labelDriverName
            // 
            this.labelDriverName.AutoSize = true;
            this.labelDriverName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDriverName.Location = new System.Drawing.Point(3, 0);
            this.labelDriverName.Name = "labelDriverName";
            this.labelDriverName.Size = new System.Drawing.Size(71, 45);
            this.labelDriverName.TabIndex = 37;
            this.labelDriverName.Text = "Driver name:";
            this.labelDriverName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBoxDriverSkill
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.groupBoxDriverSkill, 2);
            this.groupBoxDriverSkill.Location = new System.Drawing.Point(3, 48);
            this.groupBoxDriverSkill.Name = "groupBoxDriverSkill";
            this.groupBoxDriverSkill.Size = new System.Drawing.Size(420, 435);
            this.groupBoxDriverSkill.TabIndex = 35;
            this.groupBoxDriverSkill.TabStop = false;
            this.groupBoxDriverSkill.Text = "Skills";
            // 
            // labelDriverNameText
            // 
            this.labelDriverNameText.AutoSize = true;
            this.labelDriverNameText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDriverNameText.Location = new System.Drawing.Point(80, 0);
            this.labelDriverNameText.Name = "labelDriverNameText";
            this.labelDriverNameText.Size = new System.Drawing.Size(346, 45);
            this.labelDriverNameText.TabIndex = 38;
            this.labelDriverNameText.Text = "name";
            this.labelDriverNameText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.panel1, 2);
            this.panel1.Controls.Add(this.buttonCancel);
            this.panel1.Controls.Add(this.buttonSave);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 486);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(429, 63);
            this.panel1.TabIndex = 39;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(252, 3);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(165, 55);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(12, 3);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(165, 55);
            this.buttonSave.TabIndex = 0;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            // 
            // FormAIDriverEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(429, 549);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FormAIDriverEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormAIDriverEditor";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label labelDriverName;
        private System.Windows.Forms.GroupBox groupBoxDriverSkill;
        private System.Windows.Forms.Label labelDriverNameText;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.ToolTip toolTipAIDriverEditor;
    }
}