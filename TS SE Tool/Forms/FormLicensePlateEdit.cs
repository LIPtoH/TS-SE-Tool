using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace TS_SE_Tool
{
    public partial class FormLicensePlateEdit : Form
    {
        TS_SE_Tool.FormMain MainForm = Application.OpenForms.OfType<TS_SE_Tool.FormMain>().Single();
        public string licenseplatetext = "";
        private bool WindowsSizeState = false;

        public FormLicensePlateEdit(string _licenseplatetext)
        {
            InitializeComponent();

            this.Icon = Utilities.Graphics_TSSET.IconFromImage(MainForm.ProgUIImgsDict["Settings"]);

            try
            {
                string translatedString = MainForm.ResourceManagerMain.GetString(this.Name, Thread.CurrentThread.CurrentUICulture);
                if (translatedString != null)
                    this.Text = translatedString;
            }
            catch
            { }

            this.SuspendLayout();

            CreateControls();
            ToggleFormSize();
            ToggleTagHelpText();

            MainForm.HelpTranslateFormMethod(this);

            licenseplatetext = _licenseplatetext;

            string[] lpParts = licenseplatetext.Split(new char[] { '|' });

            textBoxLicensePlateNumber.Text = lpParts[0];
            textBoxLicensePlateCountry.Text = lpParts[1];
            
            this.ResumeLayout();
        }
        private void FormTruckLicensePlateEdit_Shown(object sender, EventArgs e)
        {
            buttonCancel.Focus();
        }

        private void CreateControls()
        {
            this.AcceptButton = buttonOk;
            this.CancelButton = buttonCancel;

            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            SetTagHelpText();
        }

        private void ToggleFormSize()
        {
            Size WinSizeMin = new Size(676, 212),
                 WinSizeMax = new Size(676, 282);

            if (WindowsSizeState)
            {
                this.MaximumSize = WinSizeMax;
                this.MinimumSize = WinSizeMax;
            }
            else
            {
                this.MinimumSize = WinSizeMin;
                this.MaximumSize = WinSizeMin;
            }

            WindowsSizeState = !WindowsSizeState;
        }
        private void buttonShowTagHelp_Click(object sender, EventArgs e)
        {
            ToggleFormSize();
            ToggleTagHelpText();
        }

        private void SetTagHelpText()
        {
            labelLicensePlateTagsHelp.Text = "";
            labelLicensePlateTagsHelp2.Text = "";

            StringFormat format = new StringFormat();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("~ <color value=FFBBGGRR>");
            sb.AppendLine("~ <font xscale=0.5 yscale=0.5> </font>");
            sb.AppendLine("~ <offset hshift=5 vshift=-7>");
            sb.AppendLine("~ <img src=/material/ui/lp/COUNTRY/NAME.mat width=10 height=15>");
            sb.AppendLine("~ <align left=23> <align right=23> </align>");
            sb.AppendLine("~ <sup> </sup> <sub> </sub>");
            sb.AppendLine("~ <ret>");

            string leftText = sb.ToString();

            sb = new StringBuilder();
            sb.AppendLine("Color in HEX Format ABGR");
            sb.AppendLine("Font scale");
            sb.AppendLine("Offset Next symbols & images");
            sb.AppendLine("Add Image with possible Scaling");
            sb.AppendLine("Place Symbols in columns with offset");
            sb.AppendLine("Upper & Lower index");
            sb.AppendLine("Start over to Change column (align) or make Multilayer plate");

            string rightText = sb.ToString();

            panel2.Paint += new PaintEventHandler((sender, e) =>
            {
                format.Alignment = StringAlignment.Near;

                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
                e.Graphics.DrawString(leftText, labelLicensePlateTagsHelp.Font, SystemBrushes.ControlText, 0, 0, format);

                int sWidth = Convert.ToInt32(e.Graphics.MeasureString(rightText, labelLicensePlateTagsHelp.Font).Width),
                    panelWidth = ((Panel)sender).Width;

                format.Alignment = StringAlignment.Far;

                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
                e.Graphics.DrawString(rightText, labelLicensePlateTagsHelp.Font, SystemBrushes.ControlText, panelWidth, 0, format);
            });

        }

        private void ToggleTagHelpText()
        {
            if (WindowsSizeState)
                buttonShowTagHelp.Text = "Expand help";
            else
                buttonShowTagHelp.Text = "Collapse help";
        }

        private void textBoxLicensePlateNumber_TextChanged(object sender, EventArgs e)
        {
            licenseplatetext = textBoxLicensePlateNumber.Text + '|' + textBoxLicensePlateCountry.Text;

            SCS.SCSLicensePlate thisLP = new SCS.SCSLicensePlate(licenseplatetext, SCS.SCSLicensePlate.LPtype.Truck);

            panelLicensePlatePreview.BackgroundImage = Utilities.Graphics_TSSET.ResizeImage(thisLP.LicensePlateIMG, MainForm.LicensePlateWidth[MainForm.GameType], 32); //ETS - 128x32 or ATS - 128x64
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
