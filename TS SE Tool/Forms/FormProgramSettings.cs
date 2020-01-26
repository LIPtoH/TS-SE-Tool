using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TS_SE_Tool
{
    public partial class FormProgramSettings : Form
    {
        public FormProgramSettings()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.MainIco;
            LoadSettings();
        }

        private void LoadSettings()
        {
            //Loading settings Setting checkboxes
            checkBoxShowSplashOnStartup.Checked = Properties.Settings.Default.ShowSplashOnStartup;
            checkBoxCheckUpdatesOnStartup.Checked = Properties.Settings.Default.CheckUpdatesOnStartup;
        }

        private void SaveSettings()
        {
            Properties.Settings.Default.ShowSplashOnStartup = checkBoxShowSplashOnStartup.Checked;
            Properties.Settings.Default.CheckUpdatesOnStartup = checkBoxCheckUpdatesOnStartup.Checked;
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void checkBoxCheckUpdatesOnStartup_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxCheckUpdatesOnStartup.Checked)
            {
                checkBoxShowSplashOnStartup.Checked = true;
                checkBoxShowSplashOnStartup.Enabled = false;
            }
            else
            {
                checkBoxShowSplashOnStartup.Enabled = true;
            }
        }
    }
}
