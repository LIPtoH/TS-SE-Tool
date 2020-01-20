// PsColorPicker.cs
// 
// Copyright (c) 2007-2010, OpenPainter.org, and based on the work of
//               2005 Danny Blanchard (scrabcakes@gmail.com)
// 
// The contents of this file are subject to the Mozilla Public License
// Version 1.1 (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// 
// http://www.mozilla.org/MPL/
// 
// Software distributed under the License is distributed on an "AS IS"
// basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See
// the License for the specific language governing rights and limitations
// under the License.
// 
// The Original Code is OpenPainter.
// 
// The Initial Developer of the Original Code is OpenPainter.org.
// All Rights Reserved.

//---------------------------------------------------------------------------
// Project:           Adobe Color Picker Clone 1
// Filename:          PsColorPicker.cs
// Original Author:   Danny Blanchard
//                    - scrabcakes@gmail.com
// Updates:
//  3/28/2005 - Initial Version : Danny Blanchard
//  July 2010 - Updated by OpenPainter.org
//---------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

namespace OpenPainter.ColorPicker
{
	public class FormColorPicker : Form
	{
		private AdobeColors.HSB _hsl;
		private Color _rgb;
        private Button buttonClearColor;
        private TableLayoutPanel tableLayoutPanel1;
        private Panel panel1;
        private AdobeColors.CMYK _cmyk;
        private TableLayoutPanel tableLayoutPanel2;
        private Button buttonApply;
        TS_SE_Tool.FormMain MainForm = Application.OpenForms.OfType<TS_SE_Tool.FormMain>().Single();

        public FormColorPicker(Color starting_color)
		{
			InitializeComponent();

            this.Icon = TS_SE_Tool.Properties.Resources.MainIco;

            this.SuspendLayout();

            try
            {
                string translatedString = MainForm.ResourceManagerMain.GetString(this.Name, Thread.CurrentThread.CurrentUICulture);
                if (translatedString != null)
                    this.Text = translatedString;
            }
            catch
            {
            }

            MainForm.HelpTranslateFormMethod(this, MainForm.ResourceManagerMain, Thread.CurrentThread.CurrentUICulture);
            //CorrectControlsPositions();
            this.ResumeLayout();

            _rgb = starting_color;
			_hsl = AdobeColors.RGB_to_HSB(_rgb);
			_cmyk = AdobeColors.RGB_to_CMYK(_rgb);

			UpdateTextBoxes();

			m_ctrl_BigBox.HSB = _hsl;
			m_ctrl_ThinBox.HSB = _hsl;

			m_lbl_Primary_Color.BackColor = starting_color;
			m_lbl_Secondary_Color.BackColor = starting_color;

            m_rbtn_Hue.Checked = true;
		}

		/// <summary>
		/// Gets or sets the picker's color.
		/// </summary>
		public Color PrimaryColor
        {
			get
			{
				return _rgb;
			}
			set
			{
				_rgb = value;
				_hsl = AdobeColors.RGB_to_HSB(_rgb);

				UpdateTextBoxes();

				m_ctrl_BigBox.HSB = _hsl;
				m_ctrl_ThinBox.HSB = _hsl;

				m_lbl_Primary_Color.BackColor = _rgb;
			}
		}

		public ColorComponent DrawStyle
		{
			get
			{
				if (m_rbtn_Hue.Checked)
					return ColorComponent.Hue;
				else if (m_rbtn_Sat.Checked)
					return ColorComponent.Saturation;
				else if (m_rbtn_Black.Checked)
					return ColorComponent.Brightness;
				else if (m_rbtn_Red.Checked)
					return ColorComponent.Red;
				else if (m_rbtn_Green.Checked)
					return ColorComponent.Green;
				else if (m_rbtn_Blue.Checked)
					return ColorComponent.Blue;
				else
					return ColorComponent.Hue;
			}
			set
			{
				switch (value)
				{
					case ColorComponent.Hue:
						m_rbtn_Hue.Checked = true;
						break;
					case ColorComponent.Saturation:
						m_rbtn_Sat.Checked = true;
						break;
					case ColorComponent.Brightness:
						m_rbtn_Black.Checked = true;
						break;
					case ColorComponent.Red:
						m_rbtn_Red.Checked = true;
						break;
					case ColorComponent.Green:
						m_rbtn_Green.Checked = true;
						break;
					case ColorComponent.Blue:
						m_rbtn_Blue.Checked = true;
						break;
					default:
						m_rbtn_Hue.Checked = true;
						break;
				}
			}
		}

		#region Designer Generated Variables
		private System.Windows.Forms.Button m_cmd_OK;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.TextBox m_txt_Hue;
		private System.Windows.Forms.TextBox m_txt_Sat;
		private System.Windows.Forms.TextBox m_txt_Black;
		private System.Windows.Forms.TextBox m_txt_Red;
		private System.Windows.Forms.TextBox m_txt_Green;
		private System.Windows.Forms.TextBox m_txt_Blue;
		private System.Windows.Forms.TextBox m_txt_Cyan;
		private System.Windows.Forms.TextBox m_txt_Magenta;
		private System.Windows.Forms.TextBox m_txt_Yellow;
		private System.Windows.Forms.TextBox m_txt_K;
		private System.Windows.Forms.TextBox m_txt_Hex;
		private System.Windows.Forms.RadioButton m_rbtn_Hue;
		private System.Windows.Forms.RadioButton m_rbtn_Sat;
		private System.Windows.Forms.RadioButton m_rbtn_Black;
		private System.Windows.Forms.RadioButton m_rbtn_Red;
		private System.Windows.Forms.RadioButton m_rbtn_Green;
		private System.Windows.Forms.RadioButton m_rbtn_Blue;
		private System.Windows.Forms.Label m_lbl_HexPound;
		private System.Windows.Forms.Label m_lbl_Cyan;
		private System.Windows.Forms.Label m_lbl_Magenta;
		private System.Windows.Forms.Label m_lbl_Yellow;
		private System.Windows.Forms.Label m_lbl_K;
		private System.Windows.Forms.Label m_lbl_Primary_Color;
		private System.Windows.Forms.Label m_lbl_Secondary_Color;
		private ctrlVerticalColorSlider m_ctrl_ThinBox;
		private ctrl2DColorBox m_ctrl_BigBox;
		private System.Windows.Forms.Label m_lbl_Hue_Symbol;
		private System.Windows.Forms.Label m_lbl_Saturation_Symbol;
		private System.Windows.Forms.Label m_lbl_Black_Symbol;
		private System.Windows.Forms.Label m_lbl_Cyan_Symbol;
		private System.Windows.Forms.Label m_lbl_Magenta_Symbol;
		private System.Windows.Forms.Label m_lbl_Yellow_Symbol;
		private Label label1;
		private Label labelnew;
		private Label labelold;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            OpenPainter.ColorPicker.AdobeColors.HSB hsb1 = new OpenPainter.ColorPicker.AdobeColors.HSB();
            OpenPainter.ColorPicker.AdobeColors.HSB hsb2 = new OpenPainter.ColorPicker.AdobeColors.HSB();
            this.m_cmd_OK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.m_txt_Hue = new System.Windows.Forms.TextBox();
            this.m_txt_Sat = new System.Windows.Forms.TextBox();
            this.m_txt_Black = new System.Windows.Forms.TextBox();
            this.m_txt_Red = new System.Windows.Forms.TextBox();
            this.m_txt_Green = new System.Windows.Forms.TextBox();
            this.m_txt_Blue = new System.Windows.Forms.TextBox();
            this.m_txt_Cyan = new System.Windows.Forms.TextBox();
            this.m_txt_Magenta = new System.Windows.Forms.TextBox();
            this.m_txt_Yellow = new System.Windows.Forms.TextBox();
            this.m_txt_K = new System.Windows.Forms.TextBox();
            this.m_txt_Hex = new System.Windows.Forms.TextBox();
            this.m_rbtn_Hue = new System.Windows.Forms.RadioButton();
            this.m_rbtn_Sat = new System.Windows.Forms.RadioButton();
            this.m_rbtn_Black = new System.Windows.Forms.RadioButton();
            this.m_rbtn_Red = new System.Windows.Forms.RadioButton();
            this.m_rbtn_Green = new System.Windows.Forms.RadioButton();
            this.m_rbtn_Blue = new System.Windows.Forms.RadioButton();
            this.m_lbl_HexPound = new System.Windows.Forms.Label();
            this.m_lbl_Cyan = new System.Windows.Forms.Label();
            this.m_lbl_Magenta = new System.Windows.Forms.Label();
            this.m_lbl_Yellow = new System.Windows.Forms.Label();
            this.m_lbl_K = new System.Windows.Forms.Label();
            this.m_lbl_Primary_Color = new System.Windows.Forms.Label();
            this.m_lbl_Secondary_Color = new System.Windows.Forms.Label();
            this.m_lbl_Hue_Symbol = new System.Windows.Forms.Label();
            this.m_lbl_Saturation_Symbol = new System.Windows.Forms.Label();
            this.m_lbl_Black_Symbol = new System.Windows.Forms.Label();
            this.m_lbl_Cyan_Symbol = new System.Windows.Forms.Label();
            this.m_lbl_Magenta_Symbol = new System.Windows.Forms.Label();
            this.m_lbl_Yellow_Symbol = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.labelnew = new System.Windows.Forms.Label();
            this.labelold = new System.Windows.Forms.Label();
            this.m_ctrl_BigBox = new OpenPainter.ColorPicker.ctrl2DColorBox();
            this.m_ctrl_ThinBox = new OpenPainter.ColorPicker.ctrlVerticalColorSlider();
            this.buttonClearColor = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonApply = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_cmd_OK
            // 
            this.m_cmd_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.m_cmd_OK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_cmd_OK.Location = new System.Drawing.Point(3, 279);
            this.m_cmd_OK.Name = "m_cmd_OK";
            this.m_cmd_OK.Size = new System.Drawing.Size(257, 24);
            this.m_cmd_OK.TabIndex = 4;
            this.m_cmd_OK.Text = "OK";
            this.m_cmd_OK.Click += new System.EventHandler(this.m_cmd_OK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCancel.Location = new System.Drawing.Point(266, 279);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(258, 24);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.Click += new System.EventHandler(this.m_cmd_Cancel_Click);
            // 
            // m_txt_Hue
            // 
            this.m_txt_Hue.Location = new System.Drawing.Point(367, 197);
            this.m_txt_Hue.Name = "m_txt_Hue";
            this.m_txt_Hue.Size = new System.Drawing.Size(33, 20);
            this.m_txt_Hue.TabIndex = 6;
            this.m_txt_Hue.Leave += new System.EventHandler(this.m_txt_Hue_Leave);
            // 
            // m_txt_Sat
            // 
            this.m_txt_Sat.Location = new System.Drawing.Point(367, 222);
            this.m_txt_Sat.Name = "m_txt_Sat";
            this.m_txt_Sat.Size = new System.Drawing.Size(33, 20);
            this.m_txt_Sat.TabIndex = 7;
            this.m_txt_Sat.Leave += new System.EventHandler(this.m_txt_Sat_Leave);
            // 
            // m_txt_Black
            // 
            this.m_txt_Black.Location = new System.Drawing.Point(367, 247);
            this.m_txt_Black.Name = "m_txt_Black";
            this.m_txt_Black.Size = new System.Drawing.Size(33, 20);
            this.m_txt_Black.TabIndex = 8;
            this.m_txt_Black.Leave += new System.EventHandler(this.m_txt_Black_Leave);
            // 
            // m_txt_Red
            // 
            this.m_txt_Red.Location = new System.Drawing.Point(367, 104);
            this.m_txt_Red.Name = "m_txt_Red";
            this.m_txt_Red.Size = new System.Drawing.Size(33, 20);
            this.m_txt_Red.TabIndex = 9;
            this.m_txt_Red.Leave += new System.EventHandler(this.m_txt_Red_Leave);
            // 
            // m_txt_Green
            // 
            this.m_txt_Green.Location = new System.Drawing.Point(367, 130);
            this.m_txt_Green.Name = "m_txt_Green";
            this.m_txt_Green.Size = new System.Drawing.Size(33, 20);
            this.m_txt_Green.TabIndex = 10;
            this.m_txt_Green.Leave += new System.EventHandler(this.m_txt_Green_Leave);
            // 
            // m_txt_Blue
            // 
            this.m_txt_Blue.Location = new System.Drawing.Point(367, 156);
            this.m_txt_Blue.Name = "m_txt_Blue";
            this.m_txt_Blue.Size = new System.Drawing.Size(33, 20);
            this.m_txt_Blue.TabIndex = 11;
            this.m_txt_Blue.Leave += new System.EventHandler(this.m_txt_Blue_Leave);
            // 
            // m_txt_Cyan
            // 
            this.m_txt_Cyan.Location = new System.Drawing.Point(458, 169);
            this.m_txt_Cyan.Name = "m_txt_Cyan";
            this.m_txt_Cyan.Size = new System.Drawing.Size(33, 20);
            this.m_txt_Cyan.TabIndex = 15;
            this.m_txt_Cyan.Leave += new System.EventHandler(this.m_txt_Cyan_Leave);
            // 
            // m_txt_Magenta
            // 
            this.m_txt_Magenta.Location = new System.Drawing.Point(458, 195);
            this.m_txt_Magenta.Name = "m_txt_Magenta";
            this.m_txt_Magenta.Size = new System.Drawing.Size(33, 20);
            this.m_txt_Magenta.TabIndex = 16;
            this.m_txt_Magenta.Leave += new System.EventHandler(this.m_txt_Magenta_Leave);
            // 
            // m_txt_Yellow
            // 
            this.m_txt_Yellow.Location = new System.Drawing.Point(458, 221);
            this.m_txt_Yellow.Name = "m_txt_Yellow";
            this.m_txt_Yellow.Size = new System.Drawing.Size(33, 20);
            this.m_txt_Yellow.TabIndex = 17;
            this.m_txt_Yellow.Leave += new System.EventHandler(this.m_txt_Yellow_Leave);
            // 
            // m_txt_K
            // 
            this.m_txt_K.Location = new System.Drawing.Point(458, 247);
            this.m_txt_K.Name = "m_txt_K";
            this.m_txt_K.Size = new System.Drawing.Size(33, 20);
            this.m_txt_K.TabIndex = 18;
            this.m_txt_K.Leave += new System.EventHandler(this.m_txt_K_Leave);
            // 
            // m_txt_Hex
            // 
            this.m_txt_Hex.Location = new System.Drawing.Point(458, 105);
            this.m_txt_Hex.Name = "m_txt_Hex";
            this.m_txt_Hex.Size = new System.Drawing.Size(57, 20);
            this.m_txt_Hex.TabIndex = 19;
            this.m_txt_Hex.Leave += new System.EventHandler(this.m_txt_Hex_Leave);
            this.m_txt_Hex.Validated += new System.EventHandler(this.m_txt_Hex_Leave);
            // 
            // m_rbtn_Hue
            // 
            this.m_rbtn_Hue.AutoSize = true;
            this.m_rbtn_Hue.Location = new System.Drawing.Point(325, 198);
            this.m_rbtn_Hue.Name = "m_rbtn_Hue";
            this.m_rbtn_Hue.Size = new System.Drawing.Size(36, 17);
            this.m_rbtn_Hue.TabIndex = 20;
            this.m_rbtn_Hue.Text = "H:";
            this.m_rbtn_Hue.CheckedChanged += new System.EventHandler(this.m_rbtn_Hue_CheckedChanged);
            // 
            // m_rbtn_Sat
            // 
            this.m_rbtn_Sat.AutoSize = true;
            this.m_rbtn_Sat.Location = new System.Drawing.Point(326, 224);
            this.m_rbtn_Sat.Name = "m_rbtn_Sat";
            this.m_rbtn_Sat.Size = new System.Drawing.Size(35, 17);
            this.m_rbtn_Sat.TabIndex = 21;
            this.m_rbtn_Sat.Text = "S:";
            this.m_rbtn_Sat.CheckedChanged += new System.EventHandler(this.m_rbtn_Sat_CheckedChanged);
            // 
            // m_rbtn_Black
            // 
            this.m_rbtn_Black.AutoSize = true;
            this.m_rbtn_Black.Location = new System.Drawing.Point(326, 248);
            this.m_rbtn_Black.Name = "m_rbtn_Black";
            this.m_rbtn_Black.Size = new System.Drawing.Size(35, 17);
            this.m_rbtn_Black.TabIndex = 22;
            this.m_rbtn_Black.Text = "B:";
            this.m_rbtn_Black.CheckedChanged += new System.EventHandler(this.m_rbtn_Black_CheckedChanged);
            // 
            // m_rbtn_Red
            // 
            this.m_rbtn_Red.AutoSize = true;
            this.m_rbtn_Red.Location = new System.Drawing.Point(325, 105);
            this.m_rbtn_Red.Name = "m_rbtn_Red";
            this.m_rbtn_Red.Size = new System.Drawing.Size(36, 17);
            this.m_rbtn_Red.TabIndex = 23;
            this.m_rbtn_Red.Text = "R:";
            this.m_rbtn_Red.CheckedChanged += new System.EventHandler(this.m_rbtn_Red_CheckedChanged);
            // 
            // m_rbtn_Green
            // 
            this.m_rbtn_Green.AutoSize = true;
            this.m_rbtn_Green.Location = new System.Drawing.Point(325, 131);
            this.m_rbtn_Green.Name = "m_rbtn_Green";
            this.m_rbtn_Green.Size = new System.Drawing.Size(36, 17);
            this.m_rbtn_Green.TabIndex = 24;
            this.m_rbtn_Green.Text = "G:";
            this.m_rbtn_Green.CheckedChanged += new System.EventHandler(this.m_rbtn_Green_CheckedChanged);
            // 
            // m_rbtn_Blue
            // 
            this.m_rbtn_Blue.AutoSize = true;
            this.m_rbtn_Blue.Location = new System.Drawing.Point(326, 157);
            this.m_rbtn_Blue.Name = "m_rbtn_Blue";
            this.m_rbtn_Blue.Size = new System.Drawing.Size(35, 17);
            this.m_rbtn_Blue.TabIndex = 25;
            this.m_rbtn_Blue.Text = "B:";
            this.m_rbtn_Blue.CheckedChanged += new System.EventHandler(this.m_rbtn_Blue_CheckedChanged);
            // 
            // m_lbl_HexPound
            // 
            this.m_lbl_HexPound.Location = new System.Drawing.Point(436, 107);
            this.m_lbl_HexPound.Margin = new System.Windows.Forms.Padding(0);
            this.m_lbl_HexPound.Name = "m_lbl_HexPound";
            this.m_lbl_HexPound.Size = new System.Drawing.Size(19, 15);
            this.m_lbl_HexPound.TabIndex = 27;
            this.m_lbl_HexPound.Text = "#";
            this.m_lbl_HexPound.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // m_lbl_Cyan
            // 
            this.m_lbl_Cyan.Location = new System.Drawing.Point(425, 172);
            this.m_lbl_Cyan.Margin = new System.Windows.Forms.Padding(0);
            this.m_lbl_Cyan.Name = "m_lbl_Cyan";
            this.m_lbl_Cyan.Size = new System.Drawing.Size(30, 18);
            this.m_lbl_Cyan.TabIndex = 31;
            this.m_lbl_Cyan.Text = "C:";
            this.m_lbl_Cyan.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // m_lbl_Magenta
            // 
            this.m_lbl_Magenta.Location = new System.Drawing.Point(425, 198);
            this.m_lbl_Magenta.Margin = new System.Windows.Forms.Padding(0);
            this.m_lbl_Magenta.Name = "m_lbl_Magenta";
            this.m_lbl_Magenta.Size = new System.Drawing.Size(30, 18);
            this.m_lbl_Magenta.TabIndex = 32;
            this.m_lbl_Magenta.Text = "M:";
            this.m_lbl_Magenta.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // m_lbl_Yellow
            // 
            this.m_lbl_Yellow.Location = new System.Drawing.Point(425, 224);
            this.m_lbl_Yellow.Margin = new System.Windows.Forms.Padding(0);
            this.m_lbl_Yellow.Name = "m_lbl_Yellow";
            this.m_lbl_Yellow.Size = new System.Drawing.Size(30, 18);
            this.m_lbl_Yellow.TabIndex = 33;
            this.m_lbl_Yellow.Text = "Y:";
            this.m_lbl_Yellow.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // m_lbl_K
            // 
            this.m_lbl_K.Location = new System.Drawing.Point(425, 250);
            this.m_lbl_K.Margin = new System.Windows.Forms.Padding(0);
            this.m_lbl_K.Name = "m_lbl_K";
            this.m_lbl_K.Size = new System.Drawing.Size(30, 18);
            this.m_lbl_K.TabIndex = 34;
            this.m_lbl_K.Text = "K:";
            this.m_lbl_K.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // m_lbl_Primary_Color
            // 
            this.m_lbl_Primary_Color.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.m_lbl_Primary_Color.BackColor = System.Drawing.Color.White;
            this.m_lbl_Primary_Color.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.m_lbl_Primary_Color.Location = new System.Drawing.Point(29, 14);
            this.m_lbl_Primary_Color.Name = "m_lbl_Primary_Color";
            this.m_lbl_Primary_Color.Size = new System.Drawing.Size(58, 25);
            this.m_lbl_Primary_Color.TabIndex = 36;
            this.m_lbl_Primary_Color.Click += new System.EventHandler(this.m_lbl_Primary_Color_Click);
            // 
            // m_lbl_Secondary_Color
            // 
            this.m_lbl_Secondary_Color.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.m_lbl_Secondary_Color.BackColor = System.Drawing.Color.Silver;
            this.m_lbl_Secondary_Color.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.m_lbl_Secondary_Color.Location = new System.Drawing.Point(29, 39);
            this.m_lbl_Secondary_Color.Name = "m_lbl_Secondary_Color";
            this.m_lbl_Secondary_Color.Size = new System.Drawing.Size(58, 25);
            this.m_lbl_Secondary_Color.TabIndex = 37;
            this.m_lbl_Secondary_Color.Click += new System.EventHandler(this.m_lbl_Secondary_Color_Click);
            // 
            // m_lbl_Hue_Symbol
            // 
            this.m_lbl_Hue_Symbol.AutoSize = true;
            this.m_lbl_Hue_Symbol.Location = new System.Drawing.Point(403, 200);
            this.m_lbl_Hue_Symbol.Margin = new System.Windows.Forms.Padding(0);
            this.m_lbl_Hue_Symbol.Name = "m_lbl_Hue_Symbol";
            this.m_lbl_Hue_Symbol.Size = new System.Drawing.Size(12, 13);
            this.m_lbl_Hue_Symbol.TabIndex = 40;
            this.m_lbl_Hue_Symbol.Text = "°";
            // 
            // m_lbl_Saturation_Symbol
            // 
            this.m_lbl_Saturation_Symbol.AutoSize = true;
            this.m_lbl_Saturation_Symbol.Location = new System.Drawing.Point(403, 225);
            this.m_lbl_Saturation_Symbol.Name = "m_lbl_Saturation_Symbol";
            this.m_lbl_Saturation_Symbol.Size = new System.Drawing.Size(18, 13);
            this.m_lbl_Saturation_Symbol.TabIndex = 41;
            this.m_lbl_Saturation_Symbol.Text = "%";
            // 
            // m_lbl_Black_Symbol
            // 
            this.m_lbl_Black_Symbol.AutoSize = true;
            this.m_lbl_Black_Symbol.Location = new System.Drawing.Point(403, 250);
            this.m_lbl_Black_Symbol.Name = "m_lbl_Black_Symbol";
            this.m_lbl_Black_Symbol.Size = new System.Drawing.Size(18, 13);
            this.m_lbl_Black_Symbol.TabIndex = 42;
            this.m_lbl_Black_Symbol.Text = "%";
            // 
            // m_lbl_Cyan_Symbol
            // 
            this.m_lbl_Cyan_Symbol.AutoSize = true;
            this.m_lbl_Cyan_Symbol.Location = new System.Drawing.Point(494, 172);
            this.m_lbl_Cyan_Symbol.Margin = new System.Windows.Forms.Padding(0);
            this.m_lbl_Cyan_Symbol.Name = "m_lbl_Cyan_Symbol";
            this.m_lbl_Cyan_Symbol.Size = new System.Drawing.Size(18, 13);
            this.m_lbl_Cyan_Symbol.TabIndex = 43;
            this.m_lbl_Cyan_Symbol.Text = "%";
            // 
            // m_lbl_Magenta_Symbol
            // 
            this.m_lbl_Magenta_Symbol.AutoSize = true;
            this.m_lbl_Magenta_Symbol.Location = new System.Drawing.Point(494, 198);
            this.m_lbl_Magenta_Symbol.Name = "m_lbl_Magenta_Symbol";
            this.m_lbl_Magenta_Symbol.Size = new System.Drawing.Size(18, 13);
            this.m_lbl_Magenta_Symbol.TabIndex = 44;
            this.m_lbl_Magenta_Symbol.Text = "%";
            // 
            // m_lbl_Yellow_Symbol
            // 
            this.m_lbl_Yellow_Symbol.AutoSize = true;
            this.m_lbl_Yellow_Symbol.Location = new System.Drawing.Point(494, 224);
            this.m_lbl_Yellow_Symbol.Name = "m_lbl_Yellow_Symbol";
            this.m_lbl_Yellow_Symbol.Size = new System.Drawing.Size(18, 13);
            this.m_lbl_Yellow_Symbol.TabIndex = 45;
            this.m_lbl_Yellow_Symbol.Text = "%";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(494, 250);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(18, 13);
            this.label1.TabIndex = 45;
            this.label1.Text = "%";
            // 
            // labelnew
            // 
            this.labelnew.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelnew.Location = new System.Drawing.Point(0, 0);
            this.labelnew.Margin = new System.Windows.Forms.Padding(0);
            this.labelnew.Name = "labelnew";
            this.labelnew.Size = new System.Drawing.Size(116, 13);
            this.labelnew.TabIndex = 46;
            this.labelnew.Text = "new";
            this.labelnew.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelold
            // 
            this.labelold.AutoSize = true;
            this.labelold.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelold.Location = new System.Drawing.Point(0, 65);
            this.labelold.Margin = new System.Windows.Forms.Padding(0);
            this.labelold.Name = "labelold";
            this.labelold.Size = new System.Drawing.Size(116, 13);
            this.labelold.TabIndex = 46;
            this.labelold.Text = "old";
            this.labelold.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // m_ctrl_BigBox
            // 
            this.m_ctrl_BigBox.BaseColorComponent = OpenPainter.ColorPicker.ColorComponent.Hue;
            hsb1.B = 1D;
            hsb1.H = 0D;
            hsb1.S = 1D;
            this.m_ctrl_BigBox.HSB = hsb1;
            this.m_ctrl_BigBox.Location = new System.Drawing.Point(3, 3);
            this.m_ctrl_BigBox.Name = "m_ctrl_BigBox";
            this.m_ctrl_BigBox.RGB = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.m_ctrl_BigBox.Size = new System.Drawing.Size(260, 260);
            this.m_ctrl_BigBox.TabIndex = 39;
            this.m_ctrl_BigBox.WebSafeColorsOnly = false;
            this.m_ctrl_BigBox.SelectionChanged += new System.EventHandler(this.m_ctrl_BigBox_SelectionChanged);
            // 
            // m_ctrl_ThinBox
            // 
            this.m_ctrl_ThinBox.BaseColorComponent = OpenPainter.ColorPicker.ColorComponent.Hue;
            hsb2.B = 1D;
            hsb2.H = 0D;
            hsb2.S = 1D;
            this.m_ctrl_ThinBox.HSB = hsb2;
            this.m_ctrl_ThinBox.Location = new System.Drawing.Point(275, 3);
            this.m_ctrl_ThinBox.Name = "m_ctrl_ThinBox";
            this.m_ctrl_ThinBox.RGB = System.Drawing.Color.Red;
            this.m_ctrl_ThinBox.Size = new System.Drawing.Size(39, 264);
            this.m_ctrl_ThinBox.TabIndex = 38;
            this.m_ctrl_ThinBox.WebSafeColorsOnly = false;
            this.m_ctrl_ThinBox.SelectionChanged += new System.EventHandler(this.m_ctrl_ThinBox_SelectionChanged);
            // 
            // buttonClearColor
            // 
            this.buttonClearColor.Location = new System.Drawing.Point(439, 12);
            this.buttonClearColor.Name = "buttonClearColor";
            this.buttonClearColor.Size = new System.Drawing.Size(79, 34);
            this.buttonClearColor.TabIndex = 47;
            this.buttonClearColor.Text = "Clear";
            this.buttonClearColor.Click += new System.EventHandler(this.buttonClearColor_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.m_cmd_OK, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonCancel, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(527, 306);
            this.tableLayoutPanel1.TabIndex = 48;
            // 
            // panel1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.panel1, 2);
            this.panel1.Controls.Add(this.buttonApply);
            this.panel1.Controls.Add(this.tableLayoutPanel2);
            this.panel1.Controls.Add(this.m_ctrl_BigBox);
            this.panel1.Controls.Add(this.buttonClearColor);
            this.panel1.Controls.Add(this.m_lbl_Cyan_Symbol);
            this.panel1.Controls.Add(this.m_lbl_Magenta_Symbol);
            this.panel1.Controls.Add(this.m_lbl_Yellow_Symbol);
            this.panel1.Controls.Add(this.m_txt_Hex);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.m_lbl_Hue_Symbol);
            this.panel1.Controls.Add(this.m_ctrl_ThinBox);
            this.panel1.Controls.Add(this.m_lbl_Saturation_Symbol);
            this.panel1.Controls.Add(this.m_lbl_Black_Symbol);
            this.panel1.Controls.Add(this.m_lbl_K);
            this.panel1.Controls.Add(this.m_txt_Hue);
            this.panel1.Controls.Add(this.m_lbl_Yellow);
            this.panel1.Controls.Add(this.m_txt_Sat);
            this.panel1.Controls.Add(this.m_lbl_Magenta);
            this.panel1.Controls.Add(this.m_txt_Black);
            this.panel1.Controls.Add(this.m_lbl_Cyan);
            this.panel1.Controls.Add(this.m_txt_Red);
            this.panel1.Controls.Add(this.m_lbl_HexPound);
            this.panel1.Controls.Add(this.m_txt_Green);
            this.panel1.Controls.Add(this.m_rbtn_Blue);
            this.panel1.Controls.Add(this.m_txt_Blue);
            this.panel1.Controls.Add(this.m_rbtn_Green);
            this.panel1.Controls.Add(this.m_txt_Cyan);
            this.panel1.Controls.Add(this.m_rbtn_Red);
            this.panel1.Controls.Add(this.m_txt_Magenta);
            this.panel1.Controls.Add(this.m_rbtn_Black);
            this.panel1.Controls.Add(this.m_txt_Yellow);
            this.panel1.Controls.Add(this.m_rbtn_Sat);
            this.panel1.Controls.Add(this.m_txt_K);
            this.panel1.Controls.Add(this.m_rbtn_Hue);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(527, 276);
            this.panel1.TabIndex = 6;
            // 
            // buttonApply
            // 
            this.buttonApply.Location = new System.Drawing.Point(439, 53);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(79, 23);
            this.buttonApply.TabIndex = 49;
            this.buttonApply.Text = "Apply";
            this.buttonApply.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.labelold, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.labelnew, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.m_lbl_Secondary_Color, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.m_lbl_Primary_Color, 0, 1);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(317, 3);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 13F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 13F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(116, 78);
            this.tableLayoutPanel2.TabIndex = 48;
            // 
            // FormColorPicker
            // 
            this.AcceptButton = this.m_cmd_OK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(527, 306);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormColorPicker";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Color Picker";
            this.Load += new System.EventHandler(this.frmColorPicker_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

		}


		#endregion

		#region Events Handlers

		#region General Events

		private void frmColorPicker_Load(object sender, System.EventArgs e)
		{
            
		}


		private void m_cmd_OK_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}


		private void m_cmd_Cancel_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

        private void buttonClearColor_Click(object sender, EventArgs e)
        {
            _rgb = Color.FromArgb(0, 0, 0, 0);
            _hsl = AdobeColors.RGB_to_HSB(_rgb);
            _cmyk = AdobeColors.RGB_to_CMYK(_rgb);

            UpdateTextBoxes();

            m_ctrl_BigBox.HSB = _hsl;
            m_ctrl_ThinBox.HSB = _hsl;

            m_lbl_Primary_Color.BackColor = _rgb;
        }

        #endregion

        #region Primary Picture Box (m_ctrl_BigBox)

        private void m_ctrl_BigBox_SelectionChanged(object sender, System.EventArgs e)
		{
			_hsl = m_ctrl_BigBox.HSB;
			_rgb = AdobeColors.HSB_to_RGB(_hsl);
			_cmyk = AdobeColors.RGB_to_CMYK(_rgb);

			UpdateTextBoxes();

			m_ctrl_ThinBox.HSB = _hsl;

			m_lbl_Primary_Color.BackColor = _rgb;
			m_lbl_Primary_Color.Update();
		}

		#endregion

		#region Secondary Picture Box (m_ctrl_ThinBox)

		private void m_ctrl_ThinBox_SelectionChanged(object sender, System.EventArgs e)
		{
			_hsl = m_ctrl_ThinBox.HSB;
			_rgb = AdobeColors.HSB_to_RGB(_hsl);
			_cmyk = AdobeColors.RGB_to_CMYK(_rgb);

			UpdateTextBoxes();

			m_ctrl_BigBox.HSB = _hsl;

			m_lbl_Primary_Color.BackColor = _rgb;
			m_lbl_Primary_Color.Update();
		}

		#endregion

		#region Hex Box (m_txt_Hex)

		private void m_txt_Hex_Leave(object sender, System.EventArgs e)
		{
			var text = m_txt_Hex.Text.ToUpper();
			var has_illegal_chars = false;

            text = text.TrimStart(new char[] { '#', ' ' }).TrimEnd(new char[] { ' ' });

			if (text.Length <= 0)
				has_illegal_chars = true;

			foreach (char letter in text)
			{
				if (!char.IsNumber(letter))
				{
					if (letter >= 'A' && letter <= 'F')
						continue;
					has_illegal_chars = true;
					break;
				}
			}

			if (has_illegal_chars)
			{
				MessageBox.Show("Hex must be a hex value between 0x000000 and 0xFFFFFF");
				WriteHexData(_rgb);
				return;
			}

			_rgb = ParseHexData(text);
			_hsl = AdobeColors.RGB_to_HSB(_rgb);
			_cmyk = AdobeColors.RGB_to_CMYK(_rgb);

			m_ctrl_BigBox.HSB = _hsl;
			m_ctrl_ThinBox.HSB = _hsl;
			m_lbl_Primary_Color.BackColor = _rgb;

			UpdateTextBoxes();
		}
        
        #endregion

        #region Color Boxes

        private void m_lbl_Primary_Color_Click(object sender, System.EventArgs e)
		{
			_rgb = m_lbl_Primary_Color.BackColor;
			_hsl = AdobeColors.RGB_to_HSB(_rgb);

			m_ctrl_BigBox.HSB = _hsl;
			m_ctrl_ThinBox.HSB = _hsl;

			_cmyk = AdobeColors.RGB_to_CMYK(_rgb);

			UpdateTextBoxes();
		}

		private void m_lbl_Secondary_Color_Click(object sender, System.EventArgs e)
		{
			_rgb = m_lbl_Secondary_Color.BackColor;
			_hsl = AdobeColors.RGB_to_HSB(_rgb);

			m_ctrl_BigBox.HSB = _hsl;
			m_ctrl_ThinBox.HSB = _hsl;

			m_lbl_Primary_Color.BackColor = _rgb;
			m_lbl_Primary_Color.Update();

			_cmyk = AdobeColors.RGB_to_CMYK(_rgb);

			UpdateTextBoxes();
		}

		#endregion

		#region Radio Buttons

		private void m_rbtn_Hue_CheckedChanged(object sender, System.EventArgs e)
		{
			if (m_rbtn_Hue.Checked)
			{
				m_ctrl_ThinBox.BaseColorComponent = ColorComponent.Hue;
				m_ctrl_BigBox.BaseColorComponent = ColorComponent.Hue;
			}
		}

		private void m_rbtn_Sat_CheckedChanged(object sender, System.EventArgs e)
		{
			if (m_rbtn_Sat.Checked)
			{
				m_ctrl_ThinBox.BaseColorComponent = ColorComponent.Saturation;
				m_ctrl_BigBox.BaseColorComponent = ColorComponent.Saturation;
			}
		}

		private void m_rbtn_Black_CheckedChanged(object sender, System.EventArgs e)
		{
			if (m_rbtn_Black.Checked)
			{
				m_ctrl_ThinBox.BaseColorComponent = ColorComponent.Brightness;
				m_ctrl_BigBox.BaseColorComponent = ColorComponent.Brightness;
			}
		}

		private void m_rbtn_Red_CheckedChanged(object sender, System.EventArgs e)
		{
			if (m_rbtn_Red.Checked)
			{
				m_ctrl_ThinBox.BaseColorComponent = ColorComponent.Red;
				m_ctrl_BigBox.BaseColorComponent = ColorComponent.Red;
			}
		}

		private void m_rbtn_Green_CheckedChanged(object sender, System.EventArgs e)
		{
			if (m_rbtn_Green.Checked)
			{
				m_ctrl_ThinBox.BaseColorComponent = ColorComponent.Green;
				m_ctrl_BigBox.BaseColorComponent = ColorComponent.Green;
			}
		}

		private void m_rbtn_Blue_CheckedChanged(object sender, System.EventArgs e)
		{
			if (m_rbtn_Blue.Checked)
			{
				m_ctrl_ThinBox.BaseColorComponent = ColorComponent.Blue;
				m_ctrl_BigBox.BaseColorComponent = ColorComponent.Blue;
			}
		}

		#endregion

		#region Text Boxes

		private void m_txt_Hue_Leave(object sender, System.EventArgs e)
		{
			string text = m_txt_Hue.Text;
			bool has_illegal_chars = false;

			if (text.Length <= 0)
				has_illegal_chars = true;
			else
				foreach (char letter in text)
				{
					if (!char.IsNumber(letter))
					{
						has_illegal_chars = true;
						break;
					}
				}

			if (has_illegal_chars)
			{
				MessageBox.Show("Hue must be a number value between 0 and 360");
				UpdateTextBoxes();
				return;
			}

			int hue = int.Parse(text);

			if (hue < 0)
			{
				MessageBox.Show("An integer between 0 and 360 is required.\nClosest value inserted.");
				m_txt_Hue.Text = "0";
				_hsl.H = 0.0;
			}
			else if (hue > 360)
			{
				MessageBox.Show("An integer between 0 and 360 is required.\nClosest value inserted.");
				m_txt_Hue.Text = "360";
				_hsl.H = 1.0;
			}
			else
			{
				_hsl.H = (double)hue / 360;
			}

			_rgb = AdobeColors.HSB_to_RGB(_hsl);
			_cmyk = AdobeColors.RGB_to_CMYK(_rgb);
			m_ctrl_BigBox.HSB = _hsl;
			m_ctrl_ThinBox.HSB = _hsl;
			m_lbl_Primary_Color.BackColor = _rgb;

			UpdateTextBoxes();
		}

		private void m_txt_Sat_Leave(object sender, System.EventArgs e)
		{
			string text = m_txt_Sat.Text;
			bool has_illegal_chars = false;

			if (text.Length <= 0)
				has_illegal_chars = true;
			else
				foreach (char letter in text)
				{
					if (!char.IsNumber(letter))
					{
						has_illegal_chars = true;
						break;
					}
				}

			if (has_illegal_chars)
			{
				MessageBox.Show("Saturation must be a number value between 0 and 100");
				UpdateTextBoxes();
				return;
			}

			int sat = int.Parse(text);

			if (sat < 0)
			{
				MessageBox.Show("An integer between 0 and 100 is required.\nClosest value inserted.");
				m_txt_Sat.Text = "0";
				_hsl.S = 0.0;
			}
			else if (sat > 100)
			{
				MessageBox.Show("An integer between 0 and 100 is required.\nClosest value inserted.");
				m_txt_Sat.Text = "100";
				_hsl.S = 1.0;
			}
			else
			{
				_hsl.S = (double)sat / 100;
			}

			_rgb = AdobeColors.HSB_to_RGB(_hsl);
			_cmyk = AdobeColors.RGB_to_CMYK(_rgb);
			m_ctrl_BigBox.HSB = _hsl;
			m_ctrl_ThinBox.HSB = _hsl;
			m_lbl_Primary_Color.BackColor = _rgb;

			UpdateTextBoxes();
		}

		private void m_txt_Black_Leave(object sender, System.EventArgs e)
		{
			string text = m_txt_Black.Text;
			bool has_illegal_chars = false;

			if (text.Length <= 0)
				has_illegal_chars = true;
			else
				foreach (char letter in text)
				{
					if (!char.IsNumber(letter))
					{
						has_illegal_chars = true;
						break;
					}
				}

			if (has_illegal_chars)
			{
				MessageBox.Show("Brightness must be a number value between 0 and 100.");
				UpdateTextBoxes();
				return;
			}

			int lum = int.Parse(text);

			if (lum < 0)
			{
				MessageBox.Show("An integer between 0 and 100 is required.\nClosest value inserted.");
				m_txt_Black.Text = "0";
				_hsl.B = 0.0;
			}
			else if (lum > 100)
			{
				MessageBox.Show("An integer between 0 and 100 is required.\nClosest value inserted.");
				m_txt_Black.Text = "100";
				_hsl.B = 1.0;
			}
			else
			{
				_hsl.B = (double)lum / 100;
			}

			_rgb = AdobeColors.HSB_to_RGB(_hsl);
			_cmyk = AdobeColors.RGB_to_CMYK(_rgb);
			m_ctrl_BigBox.HSB = _hsl;
			m_ctrl_ThinBox.HSB = _hsl;
			m_lbl_Primary_Color.BackColor = _rgb;

			UpdateTextBoxes();
		}

		private void m_txt_Red_Leave(object sender, System.EventArgs e)
		{
			string text = m_txt_Red.Text;
			bool has_illegal_chars = false;

			if (text.Length <= 0)
				has_illegal_chars = true;
			else
				foreach (char letter in text)
				{
					if (!char.IsNumber(letter))
					{
						has_illegal_chars = true;
						break;
					}
				}

			if (has_illegal_chars)
			{
				MessageBox.Show("Red must be a number value between 0 and 255");
				UpdateTextBoxes();
				return;
			}

			int red = int.Parse(text);

			if (red < 0)
			{
				MessageBox.Show("An integer between 0 and 255 is required.\nClosest value inserted.");
				m_txt_Sat.Text = "0";
				_rgb = Color.FromArgb(0, _rgb.G, _rgb.B);
			}
			else if (red > 255)
			{
				MessageBox.Show("An integer between 0 and 255 is required.\nClosest value inserted.");
				m_txt_Sat.Text = "255";
				_rgb = Color.FromArgb(255, _rgb.G, _rgb.B);
			}
			else
			{
				_rgb = Color.FromArgb(red, _rgb.G, _rgb.B);
			}

			_hsl = AdobeColors.RGB_to_HSB(_rgb);
			_cmyk = AdobeColors.RGB_to_CMYK(_rgb);
			m_ctrl_BigBox.HSB = _hsl;
			m_ctrl_ThinBox.HSB = _hsl;
			m_lbl_Primary_Color.BackColor = _rgb;

			UpdateTextBoxes();
		}

		private void m_txt_Green_Leave(object sender, System.EventArgs e)
		{
			string text = m_txt_Green.Text;
			bool has_illegal_chars = false;

			if (text.Length <= 0)
				has_illegal_chars = true;
			else
				foreach (char letter in text)
				{
					if (!char.IsNumber(letter))
					{
						has_illegal_chars = true;
						break;
					}
				}

			if (has_illegal_chars)
			{
				MessageBox.Show("Green must be a number value between 0 and 255");
				UpdateTextBoxes();
				return;
			}

			int green = int.Parse(text);

			if (green < 0)
			{
				MessageBox.Show("An integer between 0 and 255 is required.\nClosest value inserted.");
				m_txt_Green.Text = "0";
				_rgb = Color.FromArgb(_rgb.R, 0, _rgb.B);
			}
			else if (green > 255)
			{
				MessageBox.Show("An integer between 0 and 255 is required.\nClosest value inserted.");
				m_txt_Green.Text = "255";
				_rgb = Color.FromArgb(_rgb.R, 255, _rgb.B);
			}
			else
			{
				_rgb = Color.FromArgb(_rgb.R, green, _rgb.B);
			}

			_hsl = AdobeColors.RGB_to_HSB(_rgb);
			_cmyk = AdobeColors.RGB_to_CMYK(_rgb);
			m_ctrl_BigBox.HSB = _hsl;
			m_ctrl_ThinBox.HSB = _hsl;
			m_lbl_Primary_Color.BackColor = _rgb;

			UpdateTextBoxes();
		}

		private void m_txt_Blue_Leave(object sender, System.EventArgs e)
		{
			string text = m_txt_Blue.Text;
			bool has_illegal_chars = false;

			if (text.Length <= 0)
				has_illegal_chars = true;
			else
				foreach (char letter in text)
				{
					if (!char.IsNumber(letter))
					{
						has_illegal_chars = true;
						break;
					}
				}

			if (has_illegal_chars)
			{
				MessageBox.Show("Blue must be a number value between 0 and 255");
				UpdateTextBoxes();
				return;
			}

			int blue = int.Parse(text);

			if (blue < 0)
			{
				MessageBox.Show("An integer between 0 and 255 is required.\nClosest value inserted.");
				m_txt_Blue.Text = "0";
				_rgb = Color.FromArgb(_rgb.R, _rgb.G, 0);
			}
			else if (blue > 255)
			{
				MessageBox.Show("An integer between 0 and 255 is required.\nClosest value inserted.");
				m_txt_Blue.Text = "255";
				_rgb = Color.FromArgb(_rgb.R, _rgb.G, 255);
			}
			else
			{
				_rgb = Color.FromArgb(_rgb.R, _rgb.G, blue);
			}

			_hsl = AdobeColors.RGB_to_HSB(_rgb);
			_cmyk = AdobeColors.RGB_to_CMYK(_rgb);
			m_ctrl_BigBox.HSB = _hsl;
			m_ctrl_ThinBox.HSB = _hsl;
			m_lbl_Primary_Color.BackColor = _rgb;

			UpdateTextBoxes();
		}

		private void m_txt_Cyan_Leave(object sender, System.EventArgs e)
		{
			string text = m_txt_Cyan.Text;
			bool has_illegal_chars = false;

			if (text.Length <= 0)
				has_illegal_chars = true;
			else
				foreach (char letter in text)
				{
					if (!char.IsNumber(letter))
					{
						has_illegal_chars = true;
						break;
					}
				}

			if (has_illegal_chars)
			{
				MessageBox.Show("Cyan must be a number value between 0 and 100");
				UpdateTextBoxes();
				return;
			}

			int cyan = int.Parse(text);

			if (cyan < 0)
			{
				MessageBox.Show("An integer between 0 and 100 is required.\nClosest value inserted.");
				_cmyk.C = 0.0;
			}
			else if (cyan > 100)
			{
				MessageBox.Show("An integer between 0 and 100 is required.\nClosest value inserted.");
				_cmyk.C = 1.0;
			}
			else
			{
				_cmyk.C = (double)cyan / 100;
			}

			_rgb = AdobeColors.CmykToRgb(_cmyk);
			_hsl = AdobeColors.RGB_to_HSB(_rgb);
			m_ctrl_BigBox.HSB = _hsl;
			m_ctrl_ThinBox.HSB = _hsl;
			m_lbl_Primary_Color.BackColor = _rgb;

			UpdateTextBoxes();
		}

		private void m_txt_Magenta_Leave(object sender, System.EventArgs e)
		{
			string text = m_txt_Magenta.Text;
			bool has_illegal_chars = false;

			if (text.Length <= 0)
				has_illegal_chars = true;
			else
				foreach (char letter in text)
				{
					if (!char.IsNumber(letter))
					{
						has_illegal_chars = true;
						break;
					}
				}

			if (has_illegal_chars)
			{
				MessageBox.Show("Magenta must be a number value between 0 and 100");
				UpdateTextBoxes();
				return;
			}

			int magenta = int.Parse(text);

			if (magenta < 0)
			{
				MessageBox.Show("An integer between 0 and 100 is required.\nClosest value inserted.");
				m_txt_Magenta.Text = "0";
				_cmyk.M = 0.0;
			}
			else if (magenta > 100)
			{
				MessageBox.Show("An integer between 0 and 100 is required.\nClosest value inserted.");
				m_txt_Magenta.Text = "100";
				_cmyk.M = 1.0;
			}
			else
			{
				_cmyk.M = (double)magenta / 100;
			}

			_rgb = AdobeColors.CmykToRgb(_cmyk);
			_hsl = AdobeColors.RGB_to_HSB(_rgb);
			m_ctrl_BigBox.HSB = _hsl;
			m_ctrl_ThinBox.HSB = _hsl;
			m_lbl_Primary_Color.BackColor = _rgb;

			UpdateTextBoxes();
		}

		private void m_txt_Yellow_Leave(object sender, System.EventArgs e)
		{
			string text = m_txt_Yellow.Text;
			bool has_illegal_chars = false;

			if (text.Length <= 0)
				has_illegal_chars = true;
			else
				foreach (char letter in text)
				{
					if (!char.IsNumber(letter))
					{
						has_illegal_chars = true;
						break;
					}
				}

			if (has_illegal_chars)
			{
				MessageBox.Show("Yellow must be a number value between 0 and 100");
				UpdateTextBoxes();
				return;
			}

			int yellow = int.Parse(text);

			if (yellow < 0)
			{
				MessageBox.Show("An integer between 0 and 100 is required.\nClosest value inserted.");
				m_txt_Yellow.Text = "0";
				_cmyk.Y = 0.0;
			}
			else if (yellow > 100)
			{
				MessageBox.Show("An integer between 0 and 100 is required.\nClosest value inserted.");
				m_txt_Yellow.Text = "100";
				_cmyk.Y = 1.0;
			}
			else
			{
				_cmyk.Y = (double)yellow / 100;
			}

			_rgb = AdobeColors.CmykToRgb(_cmyk);
			_hsl = AdobeColors.RGB_to_HSB(_rgb);
			m_ctrl_BigBox.HSB = _hsl;
			m_ctrl_ThinBox.HSB = _hsl;
			m_lbl_Primary_Color.BackColor = _rgb;

			UpdateTextBoxes();
		}

		private void m_txt_K_Leave(object sender, System.EventArgs e)
		{
			string text = m_txt_K.Text;
			bool has_illegal_chars = false;

			if (text.Length <= 0)
				has_illegal_chars = true;
			else
				foreach (char letter in text)
				{
					if (!char.IsNumber(letter))
					{
						has_illegal_chars = true;
						break;
					}
				}

			if (has_illegal_chars)
			{
				MessageBox.Show("Key must be a number value between 0 and 100");
				UpdateTextBoxes();
				return;
			}

			int key = int.Parse(text);

			if (key < 0)
			{
				MessageBox.Show("An integer between 0 and 100 is required.\nClosest value inserted.");
				m_txt_K.Text = "0";
				_cmyk.K = 0.0;
			}
			else if (key > 100)
			{
				MessageBox.Show("An integer between 0 and 100 is required.\nClosest value inserted.");
				m_txt_K.Text = "100";
				_cmyk.K = 1.0;
			}
			else
			{
				_cmyk.K = (double)key / 100;
			}

			_rgb = AdobeColors.CmykToRgb(_cmyk);
			_hsl = AdobeColors.RGB_to_HSB(_rgb);
			m_ctrl_BigBox.HSB = _hsl;
			m_ctrl_ThinBox.HSB = _hsl;
			m_lbl_Primary_Color.BackColor = _rgb;

			UpdateTextBoxes();
		}
        
        #endregion

        #endregion

        #region Private Functions

        private void WriteHexData(Color rgb)
		{
			string red = Convert.ToString(rgb.R, 16);
			if (red.Length < 2) red = "0" + red;
			string green = Convert.ToString(rgb.G, 16);
			if (green.Length < 2) green = "0" + green;
			string blue = Convert.ToString(rgb.B, 16);
			if (blue.Length < 2) blue = "0" + blue;

			m_txt_Hex.Text = red.ToUpper() + green.ToUpper() + blue.ToUpper();
			m_txt_Hex.Update();
		}

		private Color ParseHexData(string hex_data)
		{
			hex_data = "000000" + hex_data;
			hex_data = hex_data.Remove(0, hex_data.Length - 6);

			string r_text, g_text, b_text;
			int r, g, b;

			r_text = hex_data.Substring(0, 2);
			g_text = hex_data.Substring(2, 2);
			b_text = hex_data.Substring(4, 2);

			r = int.Parse(r_text, System.Globalization.NumberStyles.HexNumber);
			g = int.Parse(g_text, System.Globalization.NumberStyles.HexNumber);
			b = int.Parse(b_text, System.Globalization.NumberStyles.HexNumber);

			return Color.FromArgb(r, g, b);
		}

		private void UpdateTextBoxes()
		{
			m_txt_Hue.Text = ((int)Math.Round(_hsl.H * 360)).ToString();
			m_txt_Sat.Text = ((int)Math.Round(_hsl.S * 100)).ToString();
			m_txt_Black.Text = ((int)Math.Round(_hsl.B * 100)).ToString();

			m_txt_Red.Text = _rgb.R.ToString();
			m_txt_Green.Text = _rgb.G.ToString();
			m_txt_Blue.Text = _rgb.B.ToString();

			m_txt_Cyan.Text = ((int)Math.Round(_cmyk.C * 100)).ToString();
			m_txt_Magenta.Text = ((int)Math.Round(_cmyk.M * 100)).ToString();
			m_txt_Yellow.Text = ((int)Math.Round(_cmyk.Y * 100)).ToString();
			m_txt_K.Text = ((int)Math.Round(_cmyk.K * 100)).ToString();

			m_txt_Hue.Update();
			m_txt_Sat.Update();
			m_txt_Black.Update();

			m_txt_Red.Update();
			m_txt_Green.Update();
			m_txt_Blue.Update();

			m_txt_Cyan.Update();
			m_txt_Magenta.Update();
			m_txt_Yellow.Update();
			m_txt_K.Update();

			WriteHexData(_rgb);
		}

		#endregion

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}

			base.Dispose(disposing);
		}
    }
}
