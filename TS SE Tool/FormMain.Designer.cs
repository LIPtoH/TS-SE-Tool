namespace TS_SE_Tool
{
    partial class FormMain
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
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItemProgram = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemLanguage = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemCreateTrFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.comboBoxProfiles = new System.Windows.Forms.ComboBox();
            this.buttonProfilesAndSavesRefreshAll = new System.Windows.Forms.Button();
            this.comboBoxSaves = new System.Windows.Forms.ComboBox();
            this.buttonMainDecryptSave = new System.Windows.Forms.Button();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageProfile = new System.Windows.Forms.TabPage();
            this.groupBoxProfilePlayerLevel = new System.Windows.Forms.GroupBox();
            this.labelPlayerExperience = new System.Windows.Forms.Label();
            this.buttonPlayerLevelMinus10 = new System.Windows.Forms.Button();
            this.panelPlayerLevel = new System.Windows.Forms.Panel();
            this.labelPlayerLevelName = new System.Windows.Forms.Label();
            this.labelPlayerLevelNumber = new System.Windows.Forms.Label();
            this.labelExperienceNxtLvlThreshhold = new System.Windows.Forms.Label();
            this.buttonPlayerLevelPlus01 = new System.Windows.Forms.Button();
            this.buttonPlayerLevelPlus10 = new System.Windows.Forms.Button();
            this.buttonPlayerLevelMinus01 = new System.Windows.Forms.Button();
            this.buttonPlayerLevelMaximum = new System.Windows.Forms.Button();
            this.buttonPlayerLevelMinimum = new System.Windows.Forms.Button();
            this.groupBoxProfileSkill = new System.Windows.Forms.GroupBox();
            this.groupBoxProfileUserColors = new System.Windows.Forms.GroupBox();
            this.buttonUserColorsShareColors = new System.Windows.Forms.Button();
            this.tabPageCompany = new System.Windows.Forms.TabPage();
            this.pictureBoxCompanyLogo = new System.Windows.Forms.PictureBox();
            this.buttonUserCompanyGaragesSell = new System.Windows.Forms.Button();
            this.buttonUserCompanyCitiesUnVisit = new System.Windows.Forms.Button();
            this.buttonUserCompanyCitiesVisit = new System.Windows.Forms.Button();
            this.buttonUserCompanyGaragesBuyUpgrade = new System.Windows.Forms.Button();
            this.buttonUserCompanyGaragesUpgrade = new System.Windows.Forms.Button();
            this.buttonUserCompanyGaragesBuy = new System.Windows.Forms.Button();
            this.labelUserCompanyMoneyAccount = new System.Windows.Forms.Label();
            this.textBoxUserCompanyMoneyAccount = new System.Windows.Forms.TextBox();
            this.labelUserCompanyVisitedCities = new System.Windows.Forms.Label();
            this.listBoxVisitedCities = new System.Windows.Forms.ListBox();
            this.labelUserCompanyGarages = new System.Windows.Forms.Label();
            this.listBoxGarages = new System.Windows.Forms.ListBox();
            this.labelUserCompanyCompanyName = new System.Windows.Forms.Label();
            this.textBoxUserCompanyCompanyName = new System.Windows.Forms.TextBox();
            this.comboBoxUserCompanyHQcity = new System.Windows.Forms.ComboBox();
            this.labelUserCompanyHQcity = new System.Windows.Forms.Label();
            this.tabPageTruck = new System.Windows.Forms.TabPage();
            this.groupBoxUserTruckShareTruckSettings = new System.Windows.Forms.GroupBox();
            this.buttonShareTruckTruckTruckPaste = new System.Windows.Forms.Button();
            this.buttonShareTruckTruckDetailsPaste = new System.Windows.Forms.Button();
            this.buttonShareTruckTruckTruckCopy = new System.Windows.Forms.Button();
            this.buttonShareTruckTruckDetailsCopy = new System.Windows.Forms.Button();
            this.buttonShareTruckTruckPaintPaste = new System.Windows.Forms.Button();
            this.buttonShareTruckTruckPaintCopy = new System.Windows.Forms.Button();
            this.buttonUserTruckSelectCurrent = new System.Windows.Forms.Button();
            this.groupBoxUserTruckTruckDetails = new System.Windows.Forms.GroupBox();
            this.labelLicensePlate = new System.Windows.Forms.Label();
            this.labelUserTruckLicensePlate = new System.Windows.Forms.Label();
            this.buttonUserTruckSwitchCurrent = new System.Windows.Forms.Button();
            this.comboBoxUserTruckCompanyTrucks = new System.Windows.Forms.ComboBox();
            this.labelUserTruckTruck = new System.Windows.Forms.Label();
            this.tabPageTrailer = new System.Windows.Forms.TabPage();
            this.label10 = new System.Windows.Forms.Label();
            this.buttonUserTrailerSelectCurrent = new System.Windows.Forms.Button();
            this.groupBoxUserTrailerTrailerDetails = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.labelUserTrailerLicensePlate = new System.Windows.Forms.Label();
            this.buttonUserTrailerSwitchCurrent = new System.Windows.Forms.Button();
            this.comboBoxUserTrailerCompanyTrailers = new System.Windows.Forms.ComboBox();
            this.labelUserTrailerTrailer = new System.Windows.Forms.Label();
            this.tabPageFreightMarket = new System.Windows.Forms.TabPage();
            this.labelFreightMarketDistanceNumbers = new System.Windows.Forms.Label();
            this.buttonFreightMarketRandomizeCargo = new System.Windows.Forms.Button();
            this.labelFreightMarketDistance = new System.Windows.Forms.Label();
            this.listBoxFreightMarketAddedJobs = new System.Windows.Forms.ListBox();
            this.checkBoxFreightMarketRandomDest = new System.Windows.Forms.CheckBox();
            this.checkBoxFreightMarketFilterDestination = new System.Windows.Forms.CheckBox();
            this.checkBoxFreightMarketFilterSource = new System.Windows.Forms.CheckBox();
            this.labelFreightMarketFilterMain = new System.Windows.Forms.Label();
            this.labelFreightMarketCountryF = new System.Windows.Forms.Label();
            this.labelFreightMarketCompanyF = new System.Windows.Forms.Label();
            this.comboBoxFreightMarketCompanies = new System.Windows.Forms.ComboBox();
            this.comboBoxFreightMarketCountries = new System.Windows.Forms.ComboBox();
            this.buttonFreightMarketClearJobList = new System.Windows.Forms.Button();
            this.buttonFreightMarketAddJob = new System.Windows.Forms.Button();
            this.labelFreightMarketUrgency = new System.Windows.Forms.Label();
            this.comboBoxFreightMarketUrgency = new System.Windows.Forms.ComboBox();
            this.labelFreightMarketCargo = new System.Windows.Forms.Label();
            this.comboBoxFreightMarketCargoList = new System.Windows.Forms.ComboBox();
            this.comboBoxFreightMarketDestinationCompany = new System.Windows.Forms.ComboBox();
            this.labelFreightMarketDestination = new System.Windows.Forms.Label();
            this.comboBoxFreightMarketDestinationCity = new System.Windows.Forms.ComboBox();
            this.labelFreightMarketCompany = new System.Windows.Forms.Label();
            this.labelFreightMarketCity = new System.Windows.Forms.Label();
            this.comboBoxFreightMarketSourceCompany = new System.Windows.Forms.ComboBox();
            this.labelFreightMarketSource = new System.Windows.Forms.Label();
            this.comboBoxFreightMarketSourceCity = new System.Windows.Forms.ComboBox();
            this.tabPageCargoMarket = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonCargoMarketRandomizeCargoCompany = new System.Windows.Forms.Button();
            this.buttonCargoMarketRandomizeCargoCity = new System.Windows.Forms.Button();
            this.buttonCargoMarketResetCargoCompany = new System.Windows.Forms.Button();
            this.buttonCargoMarketResetCargoCity = new System.Windows.Forms.Button();
            this.listBoxCargoMarketCargoListForCompany = new System.Windows.Forms.ListBox();
            this.listBoxCargoMarketSourceCargoSeeds = new System.Windows.Forms.ListBox();
            this.labelCargoMarketSource = new System.Windows.Forms.Label();
            this.labelCargoMarketCompany = new System.Windows.Forms.Label();
            this.comboBoxCargoMarketSourceCity = new System.Windows.Forms.ComboBox();
            this.labelCargoMarketCity = new System.Windows.Forms.Label();
            this.comboBoxSourceCargoMarketCompany = new System.Windows.Forms.ComboBox();
            this.tabPageConvoyTools = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonConvoyToolsGPSTruckPositionMultySavePaste = new System.Windows.Forms.Button();
            this.buttonConvoyToolsGPSTruckPositionMultySaveCopy = new System.Windows.Forms.Button();
            this.buttonConvoyToolsGPSCurrentPositionPaste = new System.Windows.Forms.Button();
            this.buttonConvoyToolsGPSCurrentPositionCopy = new System.Windows.Forms.Button();
            this.buttonConvoyToolsGPSStoredGPSPathPaste = new System.Windows.Forms.Button();
            this.buttonConvoyToolsGPSStoredGPSPathCopy = new System.Windows.Forms.Button();
            this.buttonMainWriteSave = new System.Windows.Forms.Button();
            this.buttonProfilesAndSavesOpenSaveFolder = new System.Windows.Forms.Button();
            this.statusStripMain = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusMessages = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBarMain = new System.Windows.Forms.ToolStripProgressBar();
            this.buttonMainLoadSave = new System.Windows.Forms.Button();
            this.pictureBoxProfileAvatar = new System.Windows.Forms.PictureBox();
            this.buttonMainGameSwitchCustomFolder = new System.Windows.Forms.Button();
            this.buttonMainGameSwitchETS = new System.Windows.Forms.Button();
            this.buttonMainGameSwitchATS = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.comboBoxPrevProfiles = new System.Windows.Forms.ComboBox();
            this.checkBoxProfilesAndSavesProfileBackups = new System.Windows.Forms.CheckBox();
            this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
            this.groupBoxMainProfilesAndSaves = new System.Windows.Forms.GroupBox();
            this.menuStripMain.SuspendLayout();
            this.tabControlMain.SuspendLayout();
            this.tabPageProfile.SuspendLayout();
            this.groupBoxProfilePlayerLevel.SuspendLayout();
            this.panelPlayerLevel.SuspendLayout();
            this.groupBoxProfileUserColors.SuspendLayout();
            this.tabPageCompany.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCompanyLogo)).BeginInit();
            this.tabPageTruck.SuspendLayout();
            this.groupBoxUserTruckShareTruckSettings.SuspendLayout();
            this.groupBoxUserTruckTruckDetails.SuspendLayout();
            this.tabPageTrailer.SuspendLayout();
            this.groupBoxUserTrailerTrailerDetails.SuspendLayout();
            this.tabPageFreightMarket.SuspendLayout();
            this.tabPageCargoMarket.SuspendLayout();
            this.tabPageConvoyTools.SuspendLayout();
            this.statusStripMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProfileAvatar)).BeginInit();
            this.groupBoxMainProfilesAndSaves.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStripMain
            // 
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemProgram,
            this.toolStripMenuItemLanguage,
            this.toolStripMenuItemHelp});
            this.menuStripMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Size = new System.Drawing.Size(504, 24);
            this.menuStripMain.TabIndex = 0;
            this.menuStripMain.Text = "menuStrip1";
            // 
            // toolStripMenuItemProgram
            // 
            this.toolStripMenuItemProgram.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemSettings,
            this.toolStripSeparator1,
            this.toolStripMenuItemExit});
            this.toolStripMenuItemProgram.Name = "toolStripMenuItemProgram";
            this.toolStripMenuItemProgram.Size = new System.Drawing.Size(65, 20);
            this.toolStripMenuItemProgram.Text = "Program";
            // 
            // toolStripMenuItemSettings
            // 
            this.toolStripMenuItemSettings.Name = "toolStripMenuItemSettings";
            this.toolStripMenuItemSettings.Size = new System.Drawing.Size(116, 22);
            this.toolStripMenuItemSettings.Text = "Settings";
            this.toolStripMenuItemSettings.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(113, 6);
            // 
            // toolStripMenuItemExit
            // 
            this.toolStripMenuItemExit.Name = "toolStripMenuItemExit";
            this.toolStripMenuItemExit.Size = new System.Drawing.Size(116, 22);
            this.toolStripMenuItemExit.Text = "Exit";
            this.toolStripMenuItemExit.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolStripMenuItemLanguage
            // 
            this.toolStripMenuItemLanguage.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemCreateTrFile,
            this.toolStripSeparator2});
            this.toolStripMenuItemLanguage.Name = "toolStripMenuItemLanguage";
            this.toolStripMenuItemLanguage.Size = new System.Drawing.Size(71, 20);
            this.toolStripMenuItemLanguage.Text = "Language";
            // 
            // toolStripMenuItemCreateTrFile
            // 
            this.toolStripMenuItemCreateTrFile.Name = "toolStripMenuItemCreateTrFile";
            this.toolStripMenuItemCreateTrFile.Size = new System.Drawing.Size(162, 22);
            this.toolStripMenuItemCreateTrFile.Text = "Make translation";
            this.toolStripMenuItemCreateTrFile.Click += new System.EventHandler(this.makeToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(159, 6);
            // 
            // toolStripMenuItemHelp
            // 
            this.toolStripMenuItemHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemAbout});
            this.toolStripMenuItemHelp.Name = "toolStripMenuItemHelp";
            this.toolStripMenuItemHelp.Size = new System.Drawing.Size(44, 20);
            this.toolStripMenuItemHelp.Text = "Help";
            // 
            // toolStripMenuItemAbout
            // 
            this.toolStripMenuItemAbout.Name = "toolStripMenuItemAbout";
            this.toolStripMenuItemAbout.Size = new System.Drawing.Size(107, 22);
            this.toolStripMenuItemAbout.Text = "About";
            this.toolStripMenuItemAbout.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // comboBoxProfiles
            // 
            this.comboBoxProfiles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxProfiles.FormattingEnabled = true;
            this.comboBoxProfiles.Location = new System.Drawing.Point(93, 50);
            this.comboBoxProfiles.Name = "comboBoxProfiles";
            this.comboBoxProfiles.Size = new System.Drawing.Size(160, 21);
            this.comboBoxProfiles.TabIndex = 1;
            this.comboBoxProfiles.SelectedIndexChanged += new System.EventHandler(this.comboBoxProfiles_SelectedIndexChanged);
            // 
            // buttonProfilesAndSavesRefreshAll
            // 
            this.buttonProfilesAndSavesRefreshAll.Location = new System.Drawing.Point(327, 19);
            this.buttonProfilesAndSavesRefreshAll.Name = "buttonProfilesAndSavesRefreshAll";
            this.buttonProfilesAndSavesRefreshAll.Size = new System.Drawing.Size(66, 52);
            this.buttonProfilesAndSavesRefreshAll.TabIndex = 2;
            this.buttonProfilesAndSavesRefreshAll.Text = "Refresh";
            this.buttonProfilesAndSavesRefreshAll.UseVisualStyleBackColor = true;
            this.buttonProfilesAndSavesRefreshAll.Click += new System.EventHandler(this.buttonRefreshAll_Click);
            // 
            // comboBoxSaves
            // 
            this.comboBoxSaves.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSaves.FormattingEnabled = true;
            this.comboBoxSaves.Location = new System.Drawing.Point(93, 79);
            this.comboBoxSaves.Name = "comboBoxSaves";
            this.comboBoxSaves.Size = new System.Drawing.Size(160, 21);
            this.comboBoxSaves.TabIndex = 3;
            this.comboBoxSaves.SelectedIndexChanged += new System.EventHandler(this.comboBoxSaves_SelectedIndexChanged);
            // 
            // buttonMainDecryptSave
            // 
            this.buttonMainDecryptSave.Location = new System.Drawing.Point(420, 61);
            this.buttonMainDecryptSave.Name = "buttonMainDecryptSave";
            this.buttonMainDecryptSave.Size = new System.Drawing.Size(76, 23);
            this.buttonMainDecryptSave.TabIndex = 5;
            this.buttonMainDecryptSave.Text = "Decrypt";
            this.buttonMainDecryptSave.UseVisualStyleBackColor = true;
            this.buttonMainDecryptSave.Click += new System.EventHandler(this.buttonDecryptSave_Click);
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageProfile);
            this.tabControlMain.Controls.Add(this.tabPageCompany);
            this.tabControlMain.Controls.Add(this.tabPageTruck);
            this.tabControlMain.Controls.Add(this.tabPageTrailer);
            this.tabControlMain.Controls.Add(this.tabPageFreightMarket);
            this.tabControlMain.Controls.Add(this.tabPageCargoMarket);
            this.tabControlMain.Controls.Add(this.tabPageConvoyTools);
            this.tabControlMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabControlMain.ItemSize = new System.Drawing.Size(82, 24);
            this.tabControlMain.Location = new System.Drawing.Point(12, 167);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(488, 502);
            this.tabControlMain.TabIndex = 6;
            // 
            // tabPageProfile
            // 
            this.tabPageProfile.Controls.Add(this.groupBoxProfilePlayerLevel);
            this.tabPageProfile.Controls.Add(this.groupBoxProfileSkill);
            this.tabPageProfile.Controls.Add(this.groupBoxProfileUserColors);
            this.tabPageProfile.Location = new System.Drawing.Point(4, 28);
            this.tabPageProfile.Name = "tabPageProfile";
            this.tabPageProfile.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageProfile.Size = new System.Drawing.Size(480, 470);
            this.tabPageProfile.TabIndex = 0;
            this.tabPageProfile.Text = "Profile";
            this.tabPageProfile.UseVisualStyleBackColor = true;
            // 
            // groupBoxProfilePlayerLevel
            // 
            this.groupBoxProfilePlayerLevel.Controls.Add(this.labelPlayerExperience);
            this.groupBoxProfilePlayerLevel.Controls.Add(this.buttonPlayerLevelMinus10);
            this.groupBoxProfilePlayerLevel.Controls.Add(this.panelPlayerLevel);
            this.groupBoxProfilePlayerLevel.Controls.Add(this.labelExperienceNxtLvlThreshhold);
            this.groupBoxProfilePlayerLevel.Controls.Add(this.buttonPlayerLevelPlus01);
            this.groupBoxProfilePlayerLevel.Controls.Add(this.buttonPlayerLevelPlus10);
            this.groupBoxProfilePlayerLevel.Controls.Add(this.buttonPlayerLevelMinus01);
            this.groupBoxProfilePlayerLevel.Controls.Add(this.buttonPlayerLevelMaximum);
            this.groupBoxProfilePlayerLevel.Controls.Add(this.buttonPlayerLevelMinimum);
            this.groupBoxProfilePlayerLevel.Location = new System.Drawing.Point(6, 6);
            this.groupBoxProfilePlayerLevel.Name = "groupBoxProfilePlayerLevel";
            this.groupBoxProfilePlayerLevel.Size = new System.Drawing.Size(392, 83);
            this.groupBoxProfilePlayerLevel.TabIndex = 0;
            this.groupBoxProfilePlayerLevel.TabStop = false;
            this.groupBoxProfilePlayerLevel.Text = "Player level";
            // 
            // labelPlayerExperience
            // 
            this.labelPlayerExperience.Location = new System.Drawing.Point(110, 54);
            this.labelPlayerExperience.Name = "labelPlayerExperience";
            this.labelPlayerExperience.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.labelPlayerExperience.Size = new System.Drawing.Size(74, 13);
            this.labelPlayerExperience.TabIndex = 39;
            this.labelPlayerExperience.Text = "0";
            // 
            // buttonPlayerLevelMinus10
            // 
            this.buttonPlayerLevelMinus10.Location = new System.Drawing.Point(6, 19);
            this.buttonPlayerLevelMinus10.Name = "buttonPlayerLevelMinus10";
            this.buttonPlayerLevelMinus10.Size = new System.Drawing.Size(36, 23);
            this.buttonPlayerLevelMinus10.TabIndex = 37;
            this.buttonPlayerLevelMinus10.Text = "- 10";
            this.buttonPlayerLevelMinus10.UseVisualStyleBackColor = true;
            this.buttonPlayerLevelMinus10.Click += new System.EventHandler(this.buttonPlayerLvlMinus10_Click);
            // 
            // panelPlayerLevel
            // 
            this.panelPlayerLevel.Controls.Add(this.labelPlayerLevelName);
            this.panelPlayerLevel.Controls.Add(this.labelPlayerLevelNumber);
            this.panelPlayerLevel.Location = new System.Drawing.Point(110, 19);
            this.panelPlayerLevel.Name = "panelPlayerLevel";
            this.panelPlayerLevel.Size = new System.Drawing.Size(167, 26);
            this.panelPlayerLevel.TabIndex = 5;
            // 
            // labelPlayerLevelName
            // 
            this.labelPlayerLevelName.AutoSize = true;
            this.labelPlayerLevelName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelPlayerLevelName.Location = new System.Drawing.Point(51, 7);
            this.labelPlayerLevelName.Name = "labelPlayerLevelName";
            this.labelPlayerLevelName.Size = new System.Drawing.Size(54, 16);
            this.labelPlayerLevelName.TabIndex = 16;
            this.labelPlayerLevelName.Text = "Newbie";
            // 
            // labelPlayerLevelNumber
            // 
            this.labelPlayerLevelNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelPlayerLevelNumber.Location = new System.Drawing.Point(0, 1);
            this.labelPlayerLevelNumber.Name = "labelPlayerLevelNumber";
            this.labelPlayerLevelNumber.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.labelPlayerLevelNumber.Size = new System.Drawing.Size(48, 25);
            this.labelPlayerLevelNumber.TabIndex = 11;
            this.labelPlayerLevelNumber.Text = "000";
            this.labelPlayerLevelNumber.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelExperienceNxtLvlThreshhold
            // 
            this.labelExperienceNxtLvlThreshhold.AutoSize = true;
            this.labelExperienceNxtLvlThreshhold.Location = new System.Drawing.Point(190, 54);
            this.labelExperienceNxtLvlThreshhold.Name = "labelExperienceNxtLvlThreshhold";
            this.labelExperienceNxtLvlThreshhold.Size = new System.Drawing.Size(27, 13);
            this.labelExperienceNxtLvlThreshhold.TabIndex = 38;
            this.labelExperienceNxtLvlThreshhold.Text = "/   0";
            // 
            // buttonPlayerLevelPlus01
            // 
            this.buttonPlayerLevelPlus01.Location = new System.Drawing.Point(303, 19);
            this.buttonPlayerLevelPlus01.Name = "buttonPlayerLevelPlus01";
            this.buttonPlayerLevelPlus01.Size = new System.Drawing.Size(36, 23);
            this.buttonPlayerLevelPlus01.TabIndex = 12;
            this.buttonPlayerLevelPlus01.Text = "+ 1";
            this.buttonPlayerLevelPlus01.UseVisualStyleBackColor = true;
            this.buttonPlayerLevelPlus01.Click += new System.EventHandler(this.buttonPlayerLvlPlus01_Click);
            // 
            // buttonPlayerLevelPlus10
            // 
            this.buttonPlayerLevelPlus10.Location = new System.Drawing.Point(345, 19);
            this.buttonPlayerLevelPlus10.Name = "buttonPlayerLevelPlus10";
            this.buttonPlayerLevelPlus10.Size = new System.Drawing.Size(36, 23);
            this.buttonPlayerLevelPlus10.TabIndex = 13;
            this.buttonPlayerLevelPlus10.Text = "+ 10";
            this.buttonPlayerLevelPlus10.UseVisualStyleBackColor = true;
            this.buttonPlayerLevelPlus10.Click += new System.EventHandler(this.buttonPlayerLvlPlus10_Click);
            // 
            // buttonPlayerLevelMinus01
            // 
            this.buttonPlayerLevelMinus01.Location = new System.Drawing.Point(48, 19);
            this.buttonPlayerLevelMinus01.Name = "buttonPlayerLevelMinus01";
            this.buttonPlayerLevelMinus01.Size = new System.Drawing.Size(36, 23);
            this.buttonPlayerLevelMinus01.TabIndex = 36;
            this.buttonPlayerLevelMinus01.Text = "- 1";
            this.buttonPlayerLevelMinus01.UseVisualStyleBackColor = true;
            this.buttonPlayerLevelMinus01.Click += new System.EventHandler(this.buttonPlayerLvlMinus01_Click);
            // 
            // buttonPlayerLevelMaximum
            // 
            this.buttonPlayerLevelMaximum.Location = new System.Drawing.Point(303, 48);
            this.buttonPlayerLevelMaximum.Name = "buttonPlayerLevelMaximum";
            this.buttonPlayerLevelMaximum.Size = new System.Drawing.Size(78, 23);
            this.buttonPlayerLevelMaximum.TabIndex = 14;
            this.buttonPlayerLevelMaximum.Text = "MAX >>";
            this.buttonPlayerLevelMaximum.UseVisualStyleBackColor = true;
            this.buttonPlayerLevelMaximum.Click += new System.EventHandler(this.buttonPlayerLvlMax_Click);
            // 
            // buttonPlayerLevelMinimum
            // 
            this.buttonPlayerLevelMinimum.Location = new System.Drawing.Point(6, 48);
            this.buttonPlayerLevelMinimum.Name = "buttonPlayerLevelMinimum";
            this.buttonPlayerLevelMinimum.Size = new System.Drawing.Size(78, 23);
            this.buttonPlayerLevelMinimum.TabIndex = 35;
            this.buttonPlayerLevelMinimum.Text = "<< MIN";
            this.buttonPlayerLevelMinimum.UseVisualStyleBackColor = true;
            this.buttonPlayerLevelMinimum.Click += new System.EventHandler(this.buttonPlayerLvlMin_Click);
            // 
            // groupBoxProfileSkill
            // 
            this.groupBoxProfileSkill.Location = new System.Drawing.Point(6, 91);
            this.groupBoxProfileSkill.Name = "groupBoxProfileSkill";
            this.groupBoxProfileSkill.Size = new System.Drawing.Size(392, 373);
            this.groupBoxProfileSkill.TabIndex = 34;
            this.groupBoxProfileSkill.TabStop = false;
            this.groupBoxProfileSkill.Text = "Skills";
            // 
            // groupBoxProfileUserColors
            // 
            this.groupBoxProfileUserColors.Controls.Add(this.buttonUserColorsShareColors);
            this.groupBoxProfileUserColors.Location = new System.Drawing.Point(404, 6);
            this.groupBoxProfileUserColors.Name = "groupBoxProfileUserColors";
            this.groupBoxProfileUserColors.Size = new System.Drawing.Size(70, 458);
            this.groupBoxProfileUserColors.TabIndex = 7;
            this.groupBoxProfileUserColors.TabStop = false;
            this.groupBoxProfileUserColors.Text = "User colors";
            // 
            // buttonUserColorsShareColors
            // 
            this.buttonUserColorsShareColors.Location = new System.Drawing.Point(6, 401);
            this.buttonUserColorsShareColors.Name = "buttonUserColorsShareColors";
            this.buttonUserColorsShareColors.Size = new System.Drawing.Size(58, 51);
            this.buttonUserColorsShareColors.TabIndex = 0;
            this.buttonUserColorsShareColors.Text = "Share colors";
            this.buttonUserColorsShareColors.UseVisualStyleBackColor = true;
            this.buttonUserColorsShareColors.Click += new System.EventHandler(this.buttonProfileShareColors_Click);
            // 
            // tabPageCompany
            // 
            this.tabPageCompany.Controls.Add(this.pictureBoxCompanyLogo);
            this.tabPageCompany.Controls.Add(this.buttonUserCompanyGaragesSell);
            this.tabPageCompany.Controls.Add(this.buttonUserCompanyCitiesUnVisit);
            this.tabPageCompany.Controls.Add(this.buttonUserCompanyCitiesVisit);
            this.tabPageCompany.Controls.Add(this.buttonUserCompanyGaragesBuyUpgrade);
            this.tabPageCompany.Controls.Add(this.buttonUserCompanyGaragesUpgrade);
            this.tabPageCompany.Controls.Add(this.buttonUserCompanyGaragesBuy);
            this.tabPageCompany.Controls.Add(this.labelUserCompanyMoneyAccount);
            this.tabPageCompany.Controls.Add(this.textBoxUserCompanyMoneyAccount);
            this.tabPageCompany.Controls.Add(this.labelUserCompanyVisitedCities);
            this.tabPageCompany.Controls.Add(this.listBoxVisitedCities);
            this.tabPageCompany.Controls.Add(this.labelUserCompanyGarages);
            this.tabPageCompany.Controls.Add(this.listBoxGarages);
            this.tabPageCompany.Controls.Add(this.labelUserCompanyCompanyName);
            this.tabPageCompany.Controls.Add(this.textBoxUserCompanyCompanyName);
            this.tabPageCompany.Controls.Add(this.comboBoxUserCompanyHQcity);
            this.tabPageCompany.Controls.Add(this.labelUserCompanyHQcity);
            this.tabPageCompany.Location = new System.Drawing.Point(4, 28);
            this.tabPageCompany.Name = "tabPageCompany";
            this.tabPageCompany.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCompany.Size = new System.Drawing.Size(480, 470);
            this.tabPageCompany.TabIndex = 5;
            this.tabPageCompany.Text = "Company";
            this.tabPageCompany.UseVisualStyleBackColor = true;
            // 
            // pictureBoxCompanyLogo
            // 
            this.pictureBoxCompanyLogo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxCompanyLogo.Location = new System.Drawing.Point(9, 6);
            this.pictureBoxCompanyLogo.Name = "pictureBoxCompanyLogo";
            this.pictureBoxCompanyLogo.Size = new System.Drawing.Size(92, 92);
            this.pictureBoxCompanyLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxCompanyLogo.TabIndex = 21;
            this.pictureBoxCompanyLogo.TabStop = false;
            // 
            // buttonUserCompanyGaragesSell
            // 
            this.buttonUserCompanyGaragesSell.Enabled = false;
            this.buttonUserCompanyGaragesSell.Location = new System.Drawing.Point(394, 397);
            this.buttonUserCompanyGaragesSell.Name = "buttonUserCompanyGaragesSell";
            this.buttonUserCompanyGaragesSell.Size = new System.Drawing.Size(80, 56);
            this.buttonUserCompanyGaragesSell.TabIndex = 28;
            this.buttonUserCompanyGaragesSell.Text = "Sell";
            this.buttonUserCompanyGaragesSell.UseVisualStyleBackColor = true;
            this.buttonUserCompanyGaragesSell.Click += new System.EventHandler(this.buttonGaragesSell_Click);
            // 
            // buttonUserCompanyCitiesUnVisit
            // 
            this.buttonUserCompanyCitiesUnVisit.Location = new System.Drawing.Point(111, 397);
            this.buttonUserCompanyCitiesUnVisit.Name = "buttonUserCompanyCitiesUnVisit";
            this.buttonUserCompanyCitiesUnVisit.Size = new System.Drawing.Size(101, 55);
            this.buttonUserCompanyCitiesUnVisit.TabIndex = 27;
            this.buttonUserCompanyCitiesUnVisit.Text = "Unvisit";
            this.buttonUserCompanyCitiesUnVisit.UseVisualStyleBackColor = true;
            this.buttonUserCompanyCitiesUnVisit.Click += new System.EventHandler(this.buttonCitiesUnVisit_Click);
            // 
            // buttonUserCompanyCitiesVisit
            // 
            this.buttonUserCompanyCitiesVisit.Location = new System.Drawing.Point(9, 397);
            this.buttonUserCompanyCitiesVisit.Name = "buttonUserCompanyCitiesVisit";
            this.buttonUserCompanyCitiesVisit.Size = new System.Drawing.Size(96, 55);
            this.buttonUserCompanyCitiesVisit.TabIndex = 26;
            this.buttonUserCompanyCitiesVisit.Text = "Visit";
            this.buttonUserCompanyCitiesVisit.UseVisualStyleBackColor = true;
            this.buttonUserCompanyCitiesVisit.Click += new System.EventHandler(this.buttonCitiesVisit_Click);
            // 
            // buttonUserCompanyGaragesBuyUpgrade
            // 
            this.buttonUserCompanyGaragesBuyUpgrade.Location = new System.Drawing.Point(218, 426);
            this.buttonUserCompanyGaragesBuyUpgrade.Name = "buttonUserCompanyGaragesBuyUpgrade";
            this.buttonUserCompanyGaragesBuyUpgrade.Size = new System.Drawing.Size(170, 26);
            this.buttonUserCompanyGaragesBuyUpgrade.TabIndex = 25;
            this.buttonUserCompanyGaragesBuyUpgrade.Text = "Buy and Upgrade";
            this.buttonUserCompanyGaragesBuyUpgrade.UseVisualStyleBackColor = true;
            this.buttonUserCompanyGaragesBuyUpgrade.Click += new System.EventHandler(this.buttonGaragesBuyUpgrade_Click);
            // 
            // buttonUserCompanyGaragesUpgrade
            // 
            this.buttonUserCompanyGaragesUpgrade.Location = new System.Drawing.Point(306, 397);
            this.buttonUserCompanyGaragesUpgrade.Name = "buttonUserCompanyGaragesUpgrade";
            this.buttonUserCompanyGaragesUpgrade.Size = new System.Drawing.Size(82, 23);
            this.buttonUserCompanyGaragesUpgrade.TabIndex = 24;
            this.buttonUserCompanyGaragesUpgrade.Text = "Upgrade";
            this.buttonUserCompanyGaragesUpgrade.UseVisualStyleBackColor = true;
            this.buttonUserCompanyGaragesUpgrade.Click += new System.EventHandler(this.buttonGaragesUpgrade_Click);
            // 
            // buttonUserCompanyGaragesBuy
            // 
            this.buttonUserCompanyGaragesBuy.Location = new System.Drawing.Point(218, 397);
            this.buttonUserCompanyGaragesBuy.Name = "buttonUserCompanyGaragesBuy";
            this.buttonUserCompanyGaragesBuy.Size = new System.Drawing.Size(82, 23);
            this.buttonUserCompanyGaragesBuy.TabIndex = 23;
            this.buttonUserCompanyGaragesBuy.Text = "Buy";
            this.buttonUserCompanyGaragesBuy.UseVisualStyleBackColor = true;
            this.buttonUserCompanyGaragesBuy.Click += new System.EventHandler(this.buttonGaragesBuy_Click);
            // 
            // labelUserCompanyMoneyAccount
            // 
            this.labelUserCompanyMoneyAccount.AutoSize = true;
            this.labelUserCompanyMoneyAccount.Location = new System.Drawing.Point(107, 43);
            this.labelUserCompanyMoneyAccount.Name = "labelUserCompanyMoneyAccount";
            this.labelUserCompanyMoneyAccount.Size = new System.Drawing.Size(81, 13);
            this.labelUserCompanyMoneyAccount.TabIndex = 22;
            this.labelUserCompanyMoneyAccount.Text = "Account money";
            // 
            // textBoxUserCompanyMoneyAccount
            // 
            this.textBoxUserCompanyMoneyAccount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxUserCompanyMoneyAccount.Location = new System.Drawing.Point(218, 41);
            this.textBoxUserCompanyMoneyAccount.Name = "textBoxUserCompanyMoneyAccount";
            this.textBoxUserCompanyMoneyAccount.Size = new System.Drawing.Size(138, 20);
            this.textBoxUserCompanyMoneyAccount.TabIndex = 21;
            this.textBoxUserCompanyMoneyAccount.TextChanged += new System.EventHandler(this.textBoxMoneyAccount_TextChanged);
            this.textBoxUserCompanyMoneyAccount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxMoneyAccount_KeyPress);
            // 
            // labelUserCompanyVisitedCities
            // 
            this.labelUserCompanyVisitedCities.AutoSize = true;
            this.labelUserCompanyVisitedCities.Location = new System.Drawing.Point(6, 103);
            this.labelUserCompanyVisitedCities.Name = "labelUserCompanyVisitedCities";
            this.labelUserCompanyVisitedCities.Size = new System.Drawing.Size(65, 13);
            this.labelUserCompanyVisitedCities.TabIndex = 20;
            this.labelUserCompanyVisitedCities.Text = "Visited cities";
            // 
            // listBoxVisitedCities
            // 
            this.listBoxVisitedCities.Font = new System.Drawing.Font("Microsoft Sans Serif", 2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listBoxVisitedCities.FormattingEnabled = true;
            this.listBoxVisitedCities.ItemHeight = 4;
            this.listBoxVisitedCities.Location = new System.Drawing.Point(9, 119);
            this.listBoxVisitedCities.Name = "listBoxVisitedCities";
            this.listBoxVisitedCities.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.listBoxVisitedCities.Size = new System.Drawing.Size(203, 272);
            this.listBoxVisitedCities.TabIndex = 19;
            this.listBoxVisitedCities.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBoxVisitedCities_DrawItem);
            this.listBoxVisitedCities.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.listBoxVisitedCities_MeasureItem);
            // 
            // labelUserCompanyGarages
            // 
            this.labelUserCompanyGarages.AutoSize = true;
            this.labelUserCompanyGarages.Location = new System.Drawing.Point(215, 103);
            this.labelUserCompanyGarages.Name = "labelUserCompanyGarages";
            this.labelUserCompanyGarages.Size = new System.Drawing.Size(47, 13);
            this.labelUserCompanyGarages.TabIndex = 18;
            this.labelUserCompanyGarages.Text = "Garages";
            // 
            // listBoxGarages
            // 
            this.listBoxGarages.Font = new System.Drawing.Font("Microsoft Sans Serif", 2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listBoxGarages.FormattingEnabled = true;
            this.listBoxGarages.ItemHeight = 4;
            this.listBoxGarages.Location = new System.Drawing.Point(218, 119);
            this.listBoxGarages.Name = "listBoxGarages";
            this.listBoxGarages.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.listBoxGarages.Size = new System.Drawing.Size(256, 272);
            this.listBoxGarages.TabIndex = 17;
            this.listBoxGarages.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBoxGarages_DrawItem);
            this.listBoxGarages.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.listBoxGarages_MeasureItem);
            // 
            // labelUserCompanyCompanyName
            // 
            this.labelUserCompanyCompanyName.AutoSize = true;
            this.labelUserCompanyCompanyName.Location = new System.Drawing.Point(107, 18);
            this.labelUserCompanyCompanyName.Name = "labelUserCompanyCompanyName";
            this.labelUserCompanyCompanyName.Size = new System.Drawing.Size(80, 13);
            this.labelUserCompanyCompanyName.TabIndex = 15;
            this.labelUserCompanyCompanyName.Text = "Company name";
            // 
            // textBoxUserCompanyCompanyName
            // 
            this.textBoxUserCompanyCompanyName.Enabled = false;
            this.textBoxUserCompanyCompanyName.Location = new System.Drawing.Point(218, 15);
            this.textBoxUserCompanyCompanyName.Name = "textBoxUserCompanyCompanyName";
            this.textBoxUserCompanyCompanyName.Size = new System.Drawing.Size(138, 20);
            this.textBoxUserCompanyCompanyName.TabIndex = 14;
            // 
            // comboBoxUserCompanyHQcity
            // 
            this.comboBoxUserCompanyHQcity.FormattingEnabled = true;
            this.comboBoxUserCompanyHQcity.Location = new System.Drawing.Point(218, 67);
            this.comboBoxUserCompanyHQcity.Name = "comboBoxUserCompanyHQcity";
            this.comboBoxUserCompanyHQcity.Size = new System.Drawing.Size(138, 21);
            this.comboBoxUserCompanyHQcity.TabIndex = 12;
            this.comboBoxUserCompanyHQcity.SelectionChangeCommitted += new System.EventHandler(this.comboBoxHQcity_SelectionChangeCommitted);
            // 
            // labelUserCompanyHQcity
            // 
            this.labelUserCompanyHQcity.AutoSize = true;
            this.labelUserCompanyHQcity.Location = new System.Drawing.Point(107, 70);
            this.labelUserCompanyHQcity.Name = "labelUserCompanyHQcity";
            this.labelUserCompanyHQcity.Size = new System.Drawing.Size(42, 13);
            this.labelUserCompanyHQcity.TabIndex = 4;
            this.labelUserCompanyHQcity.Text = "HQ city";
            // 
            // tabPageTruck
            // 
            this.tabPageTruck.Controls.Add(this.groupBoxUserTruckShareTruckSettings);
            this.tabPageTruck.Controls.Add(this.buttonUserTruckSelectCurrent);
            this.tabPageTruck.Controls.Add(this.groupBoxUserTruckTruckDetails);
            this.tabPageTruck.Controls.Add(this.buttonUserTruckSwitchCurrent);
            this.tabPageTruck.Controls.Add(this.comboBoxUserTruckCompanyTrucks);
            this.tabPageTruck.Controls.Add(this.labelUserTruckTruck);
            this.tabPageTruck.Location = new System.Drawing.Point(4, 28);
            this.tabPageTruck.Name = "tabPageTruck";
            this.tabPageTruck.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTruck.Size = new System.Drawing.Size(480, 470);
            this.tabPageTruck.TabIndex = 1;
            this.tabPageTruck.Text = "Truck";
            this.tabPageTruck.UseVisualStyleBackColor = true;
            // 
            // groupBoxUserTruckShareTruckSettings
            // 
            this.groupBoxUserTruckShareTruckSettings.Controls.Add(this.buttonShareTruckTruckTruckPaste);
            this.groupBoxUserTruckShareTruckSettings.Controls.Add(this.buttonShareTruckTruckDetailsPaste);
            this.groupBoxUserTruckShareTruckSettings.Controls.Add(this.buttonShareTruckTruckTruckCopy);
            this.groupBoxUserTruckShareTruckSettings.Controls.Add(this.buttonShareTruckTruckDetailsCopy);
            this.groupBoxUserTruckShareTruckSettings.Controls.Add(this.buttonShareTruckTruckPaintPaste);
            this.groupBoxUserTruckShareTruckSettings.Controls.Add(this.buttonShareTruckTruckPaintCopy);
            this.groupBoxUserTruckShareTruckSettings.Location = new System.Drawing.Point(6, 382);
            this.groupBoxUserTruckShareTruckSettings.Name = "groupBoxUserTruckShareTruckSettings";
            this.groupBoxUserTruckShareTruckSettings.Size = new System.Drawing.Size(468, 82);
            this.groupBoxUserTruckShareTruckSettings.TabIndex = 27;
            this.groupBoxUserTruckShareTruckSettings.TabStop = false;
            this.groupBoxUserTruckShareTruckSettings.Text = "Share Truck Settings";
            // 
            // buttonShareTruckTruckTruckPaste
            // 
            this.buttonShareTruckTruckTruckPaste.Enabled = false;
            this.buttonShareTruckTruckTruckPaste.Location = new System.Drawing.Point(315, 48);
            this.buttonShareTruckTruckTruckPaste.Name = "buttonShareTruckTruckTruckPaste";
            this.buttonShareTruckTruckTruckPaste.Size = new System.Drawing.Size(147, 23);
            this.buttonShareTruckTruckTruckPaste.TabIndex = 5;
            this.buttonShareTruckTruckTruckPaste.Text = "Paste All Truck Settings";
            this.buttonShareTruckTruckTruckPaste.UseVisualStyleBackColor = true;
            // 
            // buttonShareTruckTruckDetailsPaste
            // 
            this.buttonShareTruckTruckDetailsPaste.Enabled = false;
            this.buttonShareTruckTruckDetailsPaste.Location = new System.Drawing.Point(162, 48);
            this.buttonShareTruckTruckDetailsPaste.Name = "buttonShareTruckTruckDetailsPaste";
            this.buttonShareTruckTruckDetailsPaste.Size = new System.Drawing.Size(147, 23);
            this.buttonShareTruckTruckDetailsPaste.TabIndex = 4;
            this.buttonShareTruckTruckDetailsPaste.Text = "Paste Truck Datails";
            this.buttonShareTruckTruckDetailsPaste.UseVisualStyleBackColor = true;
            // 
            // buttonShareTruckTruckTruckCopy
            // 
            this.buttonShareTruckTruckTruckCopy.Enabled = false;
            this.buttonShareTruckTruckTruckCopy.Location = new System.Drawing.Point(315, 20);
            this.buttonShareTruckTruckTruckCopy.Name = "buttonShareTruckTruckTruckCopy";
            this.buttonShareTruckTruckTruckCopy.Size = new System.Drawing.Size(147, 23);
            this.buttonShareTruckTruckTruckCopy.TabIndex = 3;
            this.buttonShareTruckTruckTruckCopy.Text = "Copy All Truck Settings";
            this.buttonShareTruckTruckTruckCopy.UseVisualStyleBackColor = true;
            // 
            // buttonShareTruckTruckDetailsCopy
            // 
            this.buttonShareTruckTruckDetailsCopy.Enabled = false;
            this.buttonShareTruckTruckDetailsCopy.Location = new System.Drawing.Point(162, 20);
            this.buttonShareTruckTruckDetailsCopy.Name = "buttonShareTruckTruckDetailsCopy";
            this.buttonShareTruckTruckDetailsCopy.Size = new System.Drawing.Size(147, 23);
            this.buttonShareTruckTruckDetailsCopy.TabIndex = 2;
            this.buttonShareTruckTruckDetailsCopy.Text = "Copy Truck Datails";
            this.buttonShareTruckTruckDetailsCopy.UseVisualStyleBackColor = true;
            // 
            // buttonShareTruckTruckPaintPaste
            // 
            this.buttonShareTruckTruckPaintPaste.Location = new System.Drawing.Point(7, 49);
            this.buttonShareTruckTruckPaintPaste.Name = "buttonShareTruckTruckPaintPaste";
            this.buttonShareTruckTruckPaintPaste.Size = new System.Drawing.Size(147, 23);
            this.buttonShareTruckTruckPaintPaste.TabIndex = 1;
            this.buttonShareTruckTruckPaintPaste.Text = "Paste Paint Settings";
            this.buttonShareTruckTruckPaintPaste.UseVisualStyleBackColor = true;
            this.buttonShareTruckTruckPaintPaste.Click += new System.EventHandler(this.buttonTruckPaintPaste_Click);
            // 
            // buttonShareTruckTruckPaintCopy
            // 
            this.buttonShareTruckTruckPaintCopy.Location = new System.Drawing.Point(7, 20);
            this.buttonShareTruckTruckPaintCopy.Name = "buttonShareTruckTruckPaintCopy";
            this.buttonShareTruckTruckPaintCopy.Size = new System.Drawing.Size(147, 23);
            this.buttonShareTruckTruckPaintCopy.TabIndex = 0;
            this.buttonShareTruckTruckPaintCopy.Text = "Copy Paint Settings";
            this.buttonShareTruckTruckPaintCopy.UseVisualStyleBackColor = true;
            this.buttonShareTruckTruckPaintCopy.Click += new System.EventHandler(this.buttonTruckPaintCopy_Click);
            // 
            // buttonUserTruckSelectCurrent
            // 
            this.buttonUserTruckSelectCurrent.Location = new System.Drawing.Point(65, 33);
            this.buttonUserTruckSelectCurrent.Name = "buttonUserTruckSelectCurrent";
            this.buttonUserTruckSelectCurrent.Size = new System.Drawing.Size(150, 42);
            this.buttonUserTruckSelectCurrent.TabIndex = 26;
            this.buttonUserTruckSelectCurrent.Text = "Select Current Truck";
            this.buttonUserTruckSelectCurrent.UseVisualStyleBackColor = true;
            this.buttonUserTruckSelectCurrent.Click += new System.EventHandler(this.buttonUserTruckSelectCurrent_Click);
            // 
            // groupBoxUserTruckTruckDetails
            // 
            this.groupBoxUserTruckTruckDetails.Controls.Add(this.labelLicensePlate);
            this.groupBoxUserTruckTruckDetails.Controls.Add(this.labelUserTruckLicensePlate);
            this.groupBoxUserTruckTruckDetails.Location = new System.Drawing.Point(6, 81);
            this.groupBoxUserTruckTruckDetails.Name = "groupBoxUserTruckTruckDetails";
            this.groupBoxUserTruckTruckDetails.Size = new System.Drawing.Size(468, 295);
            this.groupBoxUserTruckTruckDetails.TabIndex = 25;
            this.groupBoxUserTruckTruckDetails.TabStop = false;
            this.groupBoxUserTruckTruckDetails.Text = "Details";
            // 
            // labelLicensePlate
            // 
            this.labelLicensePlate.Location = new System.Drawing.Point(103, 268);
            this.labelLicensePlate.Name = "labelLicensePlate";
            this.labelLicensePlate.Size = new System.Drawing.Size(300, 13);
            this.labelLicensePlate.TabIndex = 14;
            this.labelLicensePlate.Text = "0 000 00";
            // 
            // labelUserTruckLicensePlate
            // 
            this.labelUserTruckLicensePlate.AutoSize = true;
            this.labelUserTruckLicensePlate.Location = new System.Drawing.Point(17, 268);
            this.labelUserTruckLicensePlate.Name = "labelUserTruckLicensePlate";
            this.labelUserTruckLicensePlate.Size = new System.Drawing.Size(70, 13);
            this.labelUserTruckLicensePlate.TabIndex = 13;
            this.labelUserTruckLicensePlate.Text = "License plate";
            // 
            // buttonUserTruckSwitchCurrent
            // 
            this.buttonUserTruckSwitchCurrent.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonUserTruckSwitchCurrent.Location = new System.Drawing.Point(221, 33);
            this.buttonUserTruckSwitchCurrent.Name = "buttonUserTruckSwitchCurrent";
            this.buttonUserTruckSwitchCurrent.Size = new System.Drawing.Size(184, 42);
            this.buttonUserTruckSwitchCurrent.TabIndex = 23;
            this.buttonUserTruckSwitchCurrent.Text = "Set as Current Truck";
            this.buttonUserTruckSwitchCurrent.UseVisualStyleBackColor = true;
            this.buttonUserTruckSwitchCurrent.Click += new System.EventHandler(this.buttonUserTruckSwitchCurrent_Click);
            // 
            // comboBoxUserTruckCompanyTrucks
            // 
            this.comboBoxUserTruckCompanyTrucks.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxUserTruckCompanyTrucks.FormattingEnabled = true;
            this.comboBoxUserTruckCompanyTrucks.Location = new System.Drawing.Point(65, 6);
            this.comboBoxUserTruckCompanyTrucks.Name = "comboBoxUserTruckCompanyTrucks";
            this.comboBoxUserTruckCompanyTrucks.Size = new System.Drawing.Size(340, 21);
            this.comboBoxUserTruckCompanyTrucks.TabIndex = 22;
            this.comboBoxUserTruckCompanyTrucks.SelectedIndexChanged += new System.EventHandler(this.comboBoxCompanyTrucks_SelectedIndexChanged);
            // 
            // labelUserTruckTruck
            // 
            this.labelUserTruckTruck.AutoSize = true;
            this.labelUserTruckTruck.Location = new System.Drawing.Point(10, 14);
            this.labelUserTruckTruck.Name = "labelUserTruckTruck";
            this.labelUserTruckTruck.Size = new System.Drawing.Size(35, 13);
            this.labelUserTruckTruck.TabIndex = 11;
            this.labelUserTruckTruck.Text = "Truck";
            // 
            // tabPageTrailer
            // 
            this.tabPageTrailer.Controls.Add(this.label10);
            this.tabPageTrailer.Controls.Add(this.buttonUserTrailerSelectCurrent);
            this.tabPageTrailer.Controls.Add(this.groupBoxUserTrailerTrailerDetails);
            this.tabPageTrailer.Controls.Add(this.buttonUserTrailerSwitchCurrent);
            this.tabPageTrailer.Controls.Add(this.comboBoxUserTrailerCompanyTrailers);
            this.tabPageTrailer.Controls.Add(this.labelUserTrailerTrailer);
            this.tabPageTrailer.Location = new System.Drawing.Point(4, 28);
            this.tabPageTrailer.Name = "tabPageTrailer";
            this.tabPageTrailer.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTrailer.Size = new System.Drawing.Size(480, 470);
            this.tabPageTrailer.TabIndex = 2;
            this.tabPageTrailer.Text = "Trailer";
            this.tabPageTrailer.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label10.Location = new System.Drawing.Point(6, 355);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(468, 91);
            this.label10.TabIndex = 32;
            this.label10.Text = "Work In Progres";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonUserTrailerSelectCurrent
            // 
            this.buttonUserTrailerSelectCurrent.Enabled = false;
            this.buttonUserTrailerSelectCurrent.Location = new System.Drawing.Point(65, 33);
            this.buttonUserTrailerSelectCurrent.Name = "buttonUserTrailerSelectCurrent";
            this.buttonUserTrailerSelectCurrent.Size = new System.Drawing.Size(150, 42);
            this.buttonUserTrailerSelectCurrent.TabIndex = 31;
            this.buttonUserTrailerSelectCurrent.Text = "Select Current Trailer";
            this.buttonUserTrailerSelectCurrent.UseVisualStyleBackColor = true;
            this.buttonUserTrailerSelectCurrent.Click += new System.EventHandler(this.buttonUserTrailerSelectCurrent_Click);
            // 
            // groupBoxUserTrailerTrailerDetails
            // 
            this.groupBoxUserTrailerTrailerDetails.Controls.Add(this.label2);
            this.groupBoxUserTrailerTrailerDetails.Controls.Add(this.labelUserTrailerLicensePlate);
            this.groupBoxUserTrailerTrailerDetails.Location = new System.Drawing.Point(6, 81);
            this.groupBoxUserTrailerTrailerDetails.Name = "groupBoxUserTrailerTrailerDetails";
            this.groupBoxUserTrailerTrailerDetails.Size = new System.Drawing.Size(468, 250);
            this.groupBoxUserTrailerTrailerDetails.TabIndex = 30;
            this.groupBoxUserTrailerTrailerDetails.TabStop = false;
            this.groupBoxUserTrailerTrailerDetails.Text = "Details";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(103, 222);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(300, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "0 000 00";
            // 
            // labelUserTrailerLicensePlate
            // 
            this.labelUserTrailerLicensePlate.AutoSize = true;
            this.labelUserTrailerLicensePlate.Location = new System.Drawing.Point(17, 222);
            this.labelUserTrailerLicensePlate.Name = "labelUserTrailerLicensePlate";
            this.labelUserTrailerLicensePlate.Size = new System.Drawing.Size(70, 13);
            this.labelUserTrailerLicensePlate.TabIndex = 15;
            this.labelUserTrailerLicensePlate.Text = "License plate";
            // 
            // buttonUserTrailerSwitchCurrent
            // 
            this.buttonUserTrailerSwitchCurrent.Enabled = false;
            this.buttonUserTrailerSwitchCurrent.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonUserTrailerSwitchCurrent.Location = new System.Drawing.Point(221, 33);
            this.buttonUserTrailerSwitchCurrent.Name = "buttonUserTrailerSwitchCurrent";
            this.buttonUserTrailerSwitchCurrent.Size = new System.Drawing.Size(184, 42);
            this.buttonUserTrailerSwitchCurrent.TabIndex = 29;
            this.buttonUserTrailerSwitchCurrent.Text = "Set as Current Trailer";
            this.buttonUserTrailerSwitchCurrent.UseVisualStyleBackColor = true;
            this.buttonUserTrailerSwitchCurrent.Click += new System.EventHandler(this.buttonUserTrailerSwitchCurrent_Click);
            // 
            // comboBoxUserTrailerCompanyTrailers
            // 
            this.comboBoxUserTrailerCompanyTrailers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxUserTrailerCompanyTrailers.Enabled = false;
            this.comboBoxUserTrailerCompanyTrailers.FormattingEnabled = true;
            this.comboBoxUserTrailerCompanyTrailers.Location = new System.Drawing.Point(65, 6);
            this.comboBoxUserTrailerCompanyTrailers.Name = "comboBoxUserTrailerCompanyTrailers";
            this.comboBoxUserTrailerCompanyTrailers.Size = new System.Drawing.Size(340, 21);
            this.comboBoxUserTrailerCompanyTrailers.TabIndex = 28;
            this.comboBoxUserTrailerCompanyTrailers.SelectedIndexChanged += new System.EventHandler(this.comboBoxCompanyTrailers_SelectedIndexChanged);
            // 
            // labelUserTrailerTrailer
            // 
            this.labelUserTrailerTrailer.AutoSize = true;
            this.labelUserTrailerTrailer.Location = new System.Drawing.Point(10, 14);
            this.labelUserTrailerTrailer.Name = "labelUserTrailerTrailer";
            this.labelUserTrailerTrailer.Size = new System.Drawing.Size(36, 13);
            this.labelUserTrailerTrailer.TabIndex = 27;
            this.labelUserTrailerTrailer.Text = "Trailer";
            // 
            // tabPageFreightMarket
            // 
            this.tabPageFreightMarket.Controls.Add(this.labelFreightMarketDistanceNumbers);
            this.tabPageFreightMarket.Controls.Add(this.buttonFreightMarketRandomizeCargo);
            this.tabPageFreightMarket.Controls.Add(this.labelFreightMarketDistance);
            this.tabPageFreightMarket.Controls.Add(this.listBoxFreightMarketAddedJobs);
            this.tabPageFreightMarket.Controls.Add(this.checkBoxFreightMarketRandomDest);
            this.tabPageFreightMarket.Controls.Add(this.checkBoxFreightMarketFilterDestination);
            this.tabPageFreightMarket.Controls.Add(this.checkBoxFreightMarketFilterSource);
            this.tabPageFreightMarket.Controls.Add(this.labelFreightMarketFilterMain);
            this.tabPageFreightMarket.Controls.Add(this.labelFreightMarketCountryF);
            this.tabPageFreightMarket.Controls.Add(this.labelFreightMarketCompanyF);
            this.tabPageFreightMarket.Controls.Add(this.comboBoxFreightMarketCompanies);
            this.tabPageFreightMarket.Controls.Add(this.comboBoxFreightMarketCountries);
            this.tabPageFreightMarket.Controls.Add(this.buttonFreightMarketClearJobList);
            this.tabPageFreightMarket.Controls.Add(this.buttonFreightMarketAddJob);
            this.tabPageFreightMarket.Controls.Add(this.labelFreightMarketUrgency);
            this.tabPageFreightMarket.Controls.Add(this.comboBoxFreightMarketUrgency);
            this.tabPageFreightMarket.Controls.Add(this.labelFreightMarketCargo);
            this.tabPageFreightMarket.Controls.Add(this.comboBoxFreightMarketCargoList);
            this.tabPageFreightMarket.Controls.Add(this.comboBoxFreightMarketDestinationCompany);
            this.tabPageFreightMarket.Controls.Add(this.labelFreightMarketDestination);
            this.tabPageFreightMarket.Controls.Add(this.comboBoxFreightMarketDestinationCity);
            this.tabPageFreightMarket.Controls.Add(this.labelFreightMarketCompany);
            this.tabPageFreightMarket.Controls.Add(this.labelFreightMarketCity);
            this.tabPageFreightMarket.Controls.Add(this.comboBoxFreightMarketSourceCompany);
            this.tabPageFreightMarket.Controls.Add(this.labelFreightMarketSource);
            this.tabPageFreightMarket.Controls.Add(this.comboBoxFreightMarketSourceCity);
            this.tabPageFreightMarket.Location = new System.Drawing.Point(4, 28);
            this.tabPageFreightMarket.Name = "tabPageFreightMarket";
            this.tabPageFreightMarket.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFreightMarket.Size = new System.Drawing.Size(480, 470);
            this.tabPageFreightMarket.TabIndex = 3;
            this.tabPageFreightMarket.Text = "FreightMarket";
            this.tabPageFreightMarket.UseVisualStyleBackColor = true;
            // 
            // labelFreightMarketDistanceNumbers
            // 
            this.labelFreightMarketDistanceNumbers.AutoSize = true;
            this.labelFreightMarketDistanceNumbers.Location = new System.Drawing.Point(105, 221);
            this.labelFreightMarketDistanceNumbers.Name = "labelFreightMarketDistanceNumbers";
            this.labelFreightMarketDistanceNumbers.Size = new System.Drawing.Size(13, 13);
            this.labelFreightMarketDistanceNumbers.TabIndex = 25;
            this.labelFreightMarketDistanceNumbers.Text = "0";
            // 
            // buttonFreightMarketRandomizeCargo
            // 
            this.buttonFreightMarketRandomizeCargo.Location = new System.Drawing.Point(406, 125);
            this.buttonFreightMarketRandomizeCargo.Name = "buttonFreightMarketRandomizeCargo";
            this.buttonFreightMarketRandomizeCargo.Size = new System.Drawing.Size(67, 48);
            this.buttonFreightMarketRandomizeCargo.TabIndex = 24;
            this.buttonFreightMarketRandomizeCargo.Text = "Random";
            this.buttonFreightMarketRandomizeCargo.UseVisualStyleBackColor = true;
            // 
            // labelFreightMarketDistance
            // 
            this.labelFreightMarketDistance.AutoSize = true;
            this.labelFreightMarketDistance.Location = new System.Drawing.Point(9, 221);
            this.labelFreightMarketDistance.Name = "labelFreightMarketDistance";
            this.labelFreightMarketDistance.Size = new System.Drawing.Size(90, 13);
            this.labelFreightMarketDistance.TabIndex = 23;
            this.labelFreightMarketDistance.Text = "Total path length:";
            // 
            // listBoxFreightMarketAddedJobs
            // 
            this.listBoxFreightMarketAddedJobs.FormattingEnabled = true;
            this.listBoxFreightMarketAddedJobs.Location = new System.Drawing.Point(6, 237);
            this.listBoxFreightMarketAddedJobs.Name = "listBoxFreightMarketAddedJobs";
            this.listBoxFreightMarketAddedJobs.ScrollAlwaysVisible = true;
            this.listBoxFreightMarketAddedJobs.Size = new System.Drawing.Size(467, 225);
            this.listBoxFreightMarketAddedJobs.TabIndex = 22;
            this.listBoxFreightMarketAddedJobs.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBoxAddedJobs_DrawItem);
            this.listBoxFreightMarketAddedJobs.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.listBoxAddedJobs_MeasureItem);
            // 
            // checkBoxFreightMarketRandomDest
            // 
            this.checkBoxFreightMarketRandomDest.AutoSize = true;
            this.checkBoxFreightMarketRandomDest.Location = new System.Drawing.Point(406, 24);
            this.checkBoxFreightMarketRandomDest.Name = "checkBoxFreightMarketRandomDest";
            this.checkBoxFreightMarketRandomDest.Size = new System.Drawing.Size(50, 17);
            this.checkBoxFreightMarketRandomDest.TabIndex = 21;
            this.checkBoxFreightMarketRandomDest.Text = "RND";
            this.checkBoxFreightMarketRandomDest.UseVisualStyleBackColor = true;
            this.checkBoxFreightMarketRandomDest.CheckedChanged += new System.EventHandler(this.checkBoxRandomDest_CheckedChanged);
            // 
            // checkBoxFreightMarketFilterDestination
            // 
            this.checkBoxFreightMarketFilterDestination.AutoSize = true;
            this.checkBoxFreightMarketFilterDestination.Location = new System.Drawing.Point(406, 91);
            this.checkBoxFreightMarketFilterDestination.Name = "checkBoxFreightMarketFilterDestination";
            this.checkBoxFreightMarketFilterDestination.Size = new System.Drawing.Size(48, 17);
            this.checkBoxFreightMarketFilterDestination.TabIndex = 20;
            this.checkBoxFreightMarketFilterDestination.Text = "Filter";
            this.checkBoxFreightMarketFilterDestination.UseVisualStyleBackColor = true;
            // 
            // checkBoxFreightMarketFilterSource
            // 
            this.checkBoxFreightMarketFilterSource.AutoSize = true;
            this.checkBoxFreightMarketFilterSource.Location = new System.Drawing.Point(406, 64);
            this.checkBoxFreightMarketFilterSource.Name = "checkBoxFreightMarketFilterSource";
            this.checkBoxFreightMarketFilterSource.Size = new System.Drawing.Size(48, 17);
            this.checkBoxFreightMarketFilterSource.TabIndex = 19;
            this.checkBoxFreightMarketFilterSource.Text = "Filter";
            this.checkBoxFreightMarketFilterSource.UseVisualStyleBackColor = true;
            // 
            // labelFreightMarketFilterMain
            // 
            this.labelFreightMarketFilterMain.AutoSize = true;
            this.labelFreightMarketFilterMain.Location = new System.Drawing.Point(6, 25);
            this.labelFreightMarketFilterMain.Name = "labelFreightMarketFilterMain";
            this.labelFreightMarketFilterMain.Size = new System.Drawing.Size(29, 13);
            this.labelFreightMarketFilterMain.TabIndex = 18;
            this.labelFreightMarketFilterMain.Text = "Filter";
            // 
            // labelFreightMarketCountryF
            // 
            this.labelFreightMarketCountryF.AutoSize = true;
            this.labelFreightMarketCountryF.Location = new System.Drawing.Point(72, 6);
            this.labelFreightMarketCountryF.Name = "labelFreightMarketCountryF";
            this.labelFreightMarketCountryF.Size = new System.Drawing.Size(43, 13);
            this.labelFreightMarketCountryF.TabIndex = 17;
            this.labelFreightMarketCountryF.Text = "Country";
            // 
            // labelFreightMarketCompanyF
            // 
            this.labelFreightMarketCompanyF.AutoSize = true;
            this.labelFreightMarketCompanyF.Location = new System.Drawing.Point(239, 6);
            this.labelFreightMarketCompanyF.Name = "labelFreightMarketCompanyF";
            this.labelFreightMarketCompanyF.Size = new System.Drawing.Size(51, 13);
            this.labelFreightMarketCompanyF.TabIndex = 16;
            this.labelFreightMarketCompanyF.Text = "Company";
            // 
            // comboBoxFreightMarketCompanies
            // 
            this.comboBoxFreightMarketCompanies.FormattingEnabled = true;
            this.comboBoxFreightMarketCompanies.Location = new System.Drawing.Point(239, 22);
            this.comboBoxFreightMarketCompanies.Name = "comboBoxFreightMarketCompanies";
            this.comboBoxFreightMarketCompanies.Size = new System.Drawing.Size(161, 21);
            this.comboBoxFreightMarketCompanies.TabIndex = 15;
            this.comboBoxFreightMarketCompanies.SelectedIndexChanged += new System.EventHandler(this.comboBoxCompanies_SelectedIndexChanged);
            // 
            // comboBoxFreightMarketCountries
            // 
            this.comboBoxFreightMarketCountries.FormattingEnabled = true;
            this.comboBoxFreightMarketCountries.Location = new System.Drawing.Point(72, 22);
            this.comboBoxFreightMarketCountries.Name = "comboBoxFreightMarketCountries";
            this.comboBoxFreightMarketCountries.Size = new System.Drawing.Size(161, 21);
            this.comboBoxFreightMarketCountries.TabIndex = 14;
            this.comboBoxFreightMarketCountries.SelectedIndexChanged += new System.EventHandler(this.comboBoxCountries_SelectedIndexChanged);
            // 
            // buttonFreightMarketClearJobList
            // 
            this.buttonFreightMarketClearJobList.Location = new System.Drawing.Point(323, 208);
            this.buttonFreightMarketClearJobList.Name = "buttonFreightMarketClearJobList";
            this.buttonFreightMarketClearJobList.Size = new System.Drawing.Size(150, 23);
            this.buttonFreightMarketClearJobList.TabIndex = 13;
            this.buttonFreightMarketClearJobList.Text = "Clear list";
            this.buttonFreightMarketClearJobList.UseVisualStyleBackColor = true;
            this.buttonFreightMarketClearJobList.Click += new System.EventHandler(this.buttonClearJobList_Click);
            // 
            // buttonFreightMarketAddJob
            // 
            this.buttonFreightMarketAddJob.Location = new System.Drawing.Point(70, 179);
            this.buttonFreightMarketAddJob.Name = "buttonFreightMarketAddJob";
            this.buttonFreightMarketAddJob.Size = new System.Drawing.Size(328, 23);
            this.buttonFreightMarketAddJob.TabIndex = 12;
            this.buttonFreightMarketAddJob.Text = "Add Job to list";
            this.buttonFreightMarketAddJob.UseVisualStyleBackColor = true;
            this.buttonFreightMarketAddJob.Click += new System.EventHandler(this.buttonAddJob_Click);
            // 
            // labelFreightMarketUrgency
            // 
            this.labelFreightMarketUrgency.AutoSize = true;
            this.labelFreightMarketUrgency.Location = new System.Drawing.Point(6, 155);
            this.labelFreightMarketUrgency.Name = "labelFreightMarketUrgency";
            this.labelFreightMarketUrgency.Size = new System.Drawing.Size(47, 13);
            this.labelFreightMarketUrgency.TabIndex = 11;
            this.labelFreightMarketUrgency.Text = "Urgency";
            // 
            // comboBoxFreightMarketUrgency
            // 
            this.comboBoxFreightMarketUrgency.FormattingEnabled = true;
            this.comboBoxFreightMarketUrgency.Location = new System.Drawing.Point(70, 152);
            this.comboBoxFreightMarketUrgency.Name = "comboBoxFreightMarketUrgency";
            this.comboBoxFreightMarketUrgency.Size = new System.Drawing.Size(328, 21);
            this.comboBoxFreightMarketUrgency.TabIndex = 10;
            // 
            // labelFreightMarketCargo
            // 
            this.labelFreightMarketCargo.AutoSize = true;
            this.labelFreightMarketCargo.Location = new System.Drawing.Point(6, 128);
            this.labelFreightMarketCargo.Name = "labelFreightMarketCargo";
            this.labelFreightMarketCargo.Size = new System.Drawing.Size(35, 13);
            this.labelFreightMarketCargo.TabIndex = 9;
            this.labelFreightMarketCargo.Text = "Cargo";
            // 
            // comboBoxFreightMarketCargoList
            // 
            this.comboBoxFreightMarketCargoList.DropDownHeight = 212;
            this.comboBoxFreightMarketCargoList.FormattingEnabled = true;
            this.comboBoxFreightMarketCargoList.IntegralHeight = false;
            this.comboBoxFreightMarketCargoList.Location = new System.Drawing.Point(70, 125);
            this.comboBoxFreightMarketCargoList.Name = "comboBoxFreightMarketCargoList";
            this.comboBoxFreightMarketCargoList.Size = new System.Drawing.Size(328, 21);
            this.comboBoxFreightMarketCargoList.TabIndex = 8;
            this.comboBoxFreightMarketCargoList.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboBoxFreightMarketCargoList_DrawItem);
            this.comboBoxFreightMarketCargoList.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.comboBoxFreightMarketCargoList_MeasureItem);
            this.comboBoxFreightMarketCargoList.SelectedIndexChanged += new System.EventHandler(this.comboBoxCargoList_SelectedIndexChanged);
            // 
            // comboBoxFreightMarketDestinationCompany
            // 
            this.comboBoxFreightMarketDestinationCompany.FormattingEnabled = true;
            this.comboBoxFreightMarketDestinationCompany.Location = new System.Drawing.Point(239, 89);
            this.comboBoxFreightMarketDestinationCompany.Name = "comboBoxFreightMarketDestinationCompany";
            this.comboBoxFreightMarketDestinationCompany.Size = new System.Drawing.Size(161, 21);
            this.comboBoxFreightMarketDestinationCompany.TabIndex = 7;
            this.comboBoxFreightMarketDestinationCompany.SelectedIndexChanged += new System.EventHandler(this.comboBoxDestinationCompany_SelectedIndexChanged);
            // 
            // labelFreightMarketDestination
            // 
            this.labelFreightMarketDestination.AutoSize = true;
            this.labelFreightMarketDestination.Location = new System.Drawing.Point(6, 92);
            this.labelFreightMarketDestination.Name = "labelFreightMarketDestination";
            this.labelFreightMarketDestination.Size = new System.Drawing.Size(60, 13);
            this.labelFreightMarketDestination.TabIndex = 6;
            this.labelFreightMarketDestination.Text = "Destination";
            // 
            // comboBoxFreightMarketDestinationCity
            // 
            this.comboBoxFreightMarketDestinationCity.FormattingEnabled = true;
            this.comboBoxFreightMarketDestinationCity.Location = new System.Drawing.Point(72, 89);
            this.comboBoxFreightMarketDestinationCity.Name = "comboBoxFreightMarketDestinationCity";
            this.comboBoxFreightMarketDestinationCity.Size = new System.Drawing.Size(161, 21);
            this.comboBoxFreightMarketDestinationCity.TabIndex = 5;
            this.comboBoxFreightMarketDestinationCity.SelectedIndexChanged += new System.EventHandler(this.comboBoxDestinationCity_SelectedIndexChanged);
            // 
            // labelFreightMarketCompany
            // 
            this.labelFreightMarketCompany.AutoSize = true;
            this.labelFreightMarketCompany.Location = new System.Drawing.Point(239, 46);
            this.labelFreightMarketCompany.Name = "labelFreightMarketCompany";
            this.labelFreightMarketCompany.Size = new System.Drawing.Size(51, 13);
            this.labelFreightMarketCompany.TabIndex = 4;
            this.labelFreightMarketCompany.Text = "Company";
            // 
            // labelFreightMarketCity
            // 
            this.labelFreightMarketCity.AutoSize = true;
            this.labelFreightMarketCity.Location = new System.Drawing.Point(72, 46);
            this.labelFreightMarketCity.Name = "labelFreightMarketCity";
            this.labelFreightMarketCity.Size = new System.Drawing.Size(24, 13);
            this.labelFreightMarketCity.TabIndex = 3;
            this.labelFreightMarketCity.Text = "City";
            // 
            // comboBoxFreightMarketSourceCompany
            // 
            this.comboBoxFreightMarketSourceCompany.FormattingEnabled = true;
            this.comboBoxFreightMarketSourceCompany.Location = new System.Drawing.Point(239, 62);
            this.comboBoxFreightMarketSourceCompany.Name = "comboBoxFreightMarketSourceCompany";
            this.comboBoxFreightMarketSourceCompany.Size = new System.Drawing.Size(161, 21);
            this.comboBoxFreightMarketSourceCompany.TabIndex = 2;
            this.comboBoxFreightMarketSourceCompany.SelectedIndexChanged += new System.EventHandler(this.comboBoxSourceCompany_SelectedIndexChanged);
            // 
            // labelFreightMarketSource
            // 
            this.labelFreightMarketSource.AutoSize = true;
            this.labelFreightMarketSource.Location = new System.Drawing.Point(6, 65);
            this.labelFreightMarketSource.Name = "labelFreightMarketSource";
            this.labelFreightMarketSource.Size = new System.Drawing.Size(41, 13);
            this.labelFreightMarketSource.TabIndex = 1;
            this.labelFreightMarketSource.Text = "Source";
            // 
            // comboBoxFreightMarketSourceCity
            // 
            this.comboBoxFreightMarketSourceCity.FormattingEnabled = true;
            this.comboBoxFreightMarketSourceCity.Location = new System.Drawing.Point(72, 62);
            this.comboBoxFreightMarketSourceCity.Name = "comboBoxFreightMarketSourceCity";
            this.comboBoxFreightMarketSourceCity.Size = new System.Drawing.Size(161, 21);
            this.comboBoxFreightMarketSourceCity.TabIndex = 0;
            this.comboBoxFreightMarketSourceCity.SelectedIndexChanged += new System.EventHandler(this.comboBoxSourceCity_SelectedIndexChanged);
            // 
            // tabPageCargoMarket
            // 
            this.tabPageCargoMarket.Controls.Add(this.label1);
            this.tabPageCargoMarket.Controls.Add(this.buttonCargoMarketRandomizeCargoCompany);
            this.tabPageCargoMarket.Controls.Add(this.buttonCargoMarketRandomizeCargoCity);
            this.tabPageCargoMarket.Controls.Add(this.buttonCargoMarketResetCargoCompany);
            this.tabPageCargoMarket.Controls.Add(this.buttonCargoMarketResetCargoCity);
            this.tabPageCargoMarket.Controls.Add(this.listBoxCargoMarketCargoListForCompany);
            this.tabPageCargoMarket.Controls.Add(this.listBoxCargoMarketSourceCargoSeeds);
            this.tabPageCargoMarket.Controls.Add(this.labelCargoMarketSource);
            this.tabPageCargoMarket.Controls.Add(this.labelCargoMarketCompany);
            this.tabPageCargoMarket.Controls.Add(this.comboBoxCargoMarketSourceCity);
            this.tabPageCargoMarket.Controls.Add(this.labelCargoMarketCity);
            this.tabPageCargoMarket.Controls.Add(this.comboBoxSourceCargoMarketCompany);
            this.tabPageCargoMarket.Location = new System.Drawing.Point(4, 28);
            this.tabPageCargoMarket.Name = "tabPageCargoMarket";
            this.tabPageCargoMarket.Size = new System.Drawing.Size(480, 470);
            this.tabPageCargoMarket.TabIndex = 6;
            this.tabPageCargoMarket.Text = "CargoMarket";
            this.tabPageCargoMarket.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(3, 258);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(468, 212);
            this.label1.TabIndex = 17;
            this.label1.Text = "Work In Progres";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonCargoMarketRandomizeCargoCompany
            // 
            this.buttonCargoMarketRandomizeCargoCompany.Location = new System.Drawing.Point(239, 79);
            this.buttonCargoMarketRandomizeCargoCompany.Name = "buttonCargoMarketRandomizeCargoCompany";
            this.buttonCargoMarketRandomizeCargoCompany.Size = new System.Drawing.Size(161, 23);
            this.buttonCargoMarketRandomizeCargoCompany.TabIndex = 16;
            this.buttonCargoMarketRandomizeCargoCompany.Text = "Randomize Cargo list";
            this.buttonCargoMarketRandomizeCargoCompany.UseVisualStyleBackColor = true;
            this.buttonCargoMarketRandomizeCargoCompany.Click += new System.EventHandler(this.buttonCargoMarketRandomizeCargoCompany_Click);
            // 
            // buttonCargoMarketRandomizeCargoCity
            // 
            this.buttonCargoMarketRandomizeCargoCity.Location = new System.Drawing.Point(72, 79);
            this.buttonCargoMarketRandomizeCargoCity.Name = "buttonCargoMarketRandomizeCargoCity";
            this.buttonCargoMarketRandomizeCargoCity.Size = new System.Drawing.Size(161, 23);
            this.buttonCargoMarketRandomizeCargoCity.TabIndex = 15;
            this.buttonCargoMarketRandomizeCargoCity.Text = "Randomize Cargo list";
            this.buttonCargoMarketRandomizeCargoCity.UseVisualStyleBackColor = true;
            this.buttonCargoMarketRandomizeCargoCity.Click += new System.EventHandler(this.buttonCargoMarketRandomizeCargoCity_Click);
            // 
            // buttonCargoMarketResetCargoCompany
            // 
            this.buttonCargoMarketResetCargoCompany.Location = new System.Drawing.Point(239, 50);
            this.buttonCargoMarketResetCargoCompany.Name = "buttonCargoMarketResetCargoCompany";
            this.buttonCargoMarketResetCargoCompany.Size = new System.Drawing.Size(161, 23);
            this.buttonCargoMarketResetCargoCompany.TabIndex = 14;
            this.buttonCargoMarketResetCargoCompany.Text = "Reset Cargo list";
            this.buttonCargoMarketResetCargoCompany.UseVisualStyleBackColor = true;
            this.buttonCargoMarketResetCargoCompany.Click += new System.EventHandler(this.buttonCargoMarketResetCargoCompany_Click);
            // 
            // buttonCargoMarketResetCargoCity
            // 
            this.buttonCargoMarketResetCargoCity.Location = new System.Drawing.Point(72, 50);
            this.buttonCargoMarketResetCargoCity.Name = "buttonCargoMarketResetCargoCity";
            this.buttonCargoMarketResetCargoCity.Size = new System.Drawing.Size(161, 23);
            this.buttonCargoMarketResetCargoCity.TabIndex = 13;
            this.buttonCargoMarketResetCargoCity.Text = "Reset Cargo list";
            this.buttonCargoMarketResetCargoCity.UseVisualStyleBackColor = true;
            this.buttonCargoMarketResetCargoCity.Click += new System.EventHandler(this.buttonCargoMarketResetCargoCity_Click);
            // 
            // listBoxCargoMarketCargoListForCompany
            // 
            this.listBoxCargoMarketCargoListForCompany.FormattingEnabled = true;
            this.listBoxCargoMarketCargoListForCompany.Location = new System.Drawing.Point(72, 261);
            this.listBoxCargoMarketCargoListForCompany.Name = "listBoxCargoMarketCargoListForCompany";
            this.listBoxCargoMarketCargoListForCompany.Size = new System.Drawing.Size(161, 199);
            this.listBoxCargoMarketCargoListForCompany.TabIndex = 12;
            this.listBoxCargoMarketCargoListForCompany.Visible = false;
            // 
            // listBoxCargoMarketSourceCargoSeeds
            // 
            this.listBoxCargoMarketSourceCargoSeeds.FormattingEnabled = true;
            this.listBoxCargoMarketSourceCargoSeeds.Location = new System.Drawing.Point(72, 108);
            this.listBoxCargoMarketSourceCargoSeeds.Name = "listBoxCargoMarketSourceCargoSeeds";
            this.listBoxCargoMarketSourceCargoSeeds.Size = new System.Drawing.Size(328, 147);
            this.listBoxCargoMarketSourceCargoSeeds.TabIndex = 7;
            // 
            // labelCargoMarketSource
            // 
            this.labelCargoMarketSource.AutoSize = true;
            this.labelCargoMarketSource.Location = new System.Drawing.Point(3, 28);
            this.labelCargoMarketSource.Name = "labelCargoMarketSource";
            this.labelCargoMarketSource.Size = new System.Drawing.Size(41, 13);
            this.labelCargoMarketSource.TabIndex = 6;
            this.labelCargoMarketSource.Text = "Source";
            // 
            // labelCargoMarketCompany
            // 
            this.labelCargoMarketCompany.AutoSize = true;
            this.labelCargoMarketCompany.Location = new System.Drawing.Point(239, 6);
            this.labelCargoMarketCompany.Name = "labelCargoMarketCompany";
            this.labelCargoMarketCompany.Size = new System.Drawing.Size(51, 13);
            this.labelCargoMarketCompany.TabIndex = 5;
            this.labelCargoMarketCompany.Text = "Company";
            // 
            // comboBoxCargoMarketSourceCity
            // 
            this.comboBoxCargoMarketSourceCity.FormattingEnabled = true;
            this.comboBoxCargoMarketSourceCity.Location = new System.Drawing.Point(72, 22);
            this.comboBoxCargoMarketSourceCity.Name = "comboBoxCargoMarketSourceCity";
            this.comboBoxCargoMarketSourceCity.Size = new System.Drawing.Size(161, 21);
            this.comboBoxCargoMarketSourceCity.TabIndex = 2;
            this.comboBoxCargoMarketSourceCity.SelectedIndexChanged += new System.EventHandler(this.comboBoxSourceCityCM_SelectedIndexChanged);
            // 
            // labelCargoMarketCity
            // 
            this.labelCargoMarketCity.AutoSize = true;
            this.labelCargoMarketCity.Location = new System.Drawing.Point(72, 6);
            this.labelCargoMarketCity.Name = "labelCargoMarketCity";
            this.labelCargoMarketCity.Size = new System.Drawing.Size(24, 13);
            this.labelCargoMarketCity.TabIndex = 1;
            this.labelCargoMarketCity.Text = "City";
            // 
            // comboBoxSourceCargoMarketCompany
            // 
            this.comboBoxSourceCargoMarketCompany.FormattingEnabled = true;
            this.comboBoxSourceCargoMarketCompany.Location = new System.Drawing.Point(239, 22);
            this.comboBoxSourceCargoMarketCompany.Name = "comboBoxSourceCargoMarketCompany";
            this.comboBoxSourceCargoMarketCompany.Size = new System.Drawing.Size(161, 21);
            this.comboBoxSourceCargoMarketCompany.TabIndex = 0;
            this.comboBoxSourceCargoMarketCompany.SelectedIndexChanged += new System.EventHandler(this.comboBoxSourceCompanyCM_SelectedIndexChanged);
            // 
            // tabPageConvoyTools
            // 
            this.tabPageConvoyTools.Controls.Add(this.label5);
            this.tabPageConvoyTools.Controls.Add(this.buttonConvoyToolsGPSTruckPositionMultySavePaste);
            this.tabPageConvoyTools.Controls.Add(this.buttonConvoyToolsGPSTruckPositionMultySaveCopy);
            this.tabPageConvoyTools.Controls.Add(this.buttonConvoyToolsGPSCurrentPositionPaste);
            this.tabPageConvoyTools.Controls.Add(this.buttonConvoyToolsGPSCurrentPositionCopy);
            this.tabPageConvoyTools.Controls.Add(this.buttonConvoyToolsGPSStoredGPSPathPaste);
            this.tabPageConvoyTools.Controls.Add(this.buttonConvoyToolsGPSStoredGPSPathCopy);
            this.tabPageConvoyTools.Location = new System.Drawing.Point(4, 28);
            this.tabPageConvoyTools.Name = "tabPageConvoyTools";
            this.tabPageConvoyTools.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageConvoyTools.Size = new System.Drawing.Size(480, 470);
            this.tabPageConvoyTools.TabIndex = 4;
            this.tabPageConvoyTools.Text = "Convoy Control";
            this.tabPageConvoyTools.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(6, 61);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(468, 247);
            this.label5.TabIndex = 6;
            this.label5.Text = "Work In Progres";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonConvoyToolsGPSTruckPositionMultySavePaste
            // 
            this.buttonConvoyToolsGPSTruckPositionMultySavePaste.Location = new System.Drawing.Point(243, 64);
            this.buttonConvoyToolsGPSTruckPositionMultySavePaste.Name = "buttonConvoyToolsGPSTruckPositionMultySavePaste";
            this.buttonConvoyToolsGPSTruckPositionMultySavePaste.Size = new System.Drawing.Size(231, 52);
            this.buttonConvoyToolsGPSTruckPositionMultySavePaste.TabIndex = 5;
            this.buttonConvoyToolsGPSTruckPositionMultySavePaste.Text = "Create multiple saves with different truck positions";
            this.buttonConvoyToolsGPSTruckPositionMultySavePaste.UseVisualStyleBackColor = true;
            // 
            // buttonConvoyToolsGPSTruckPositionMultySaveCopy
            // 
            this.buttonConvoyToolsGPSTruckPositionMultySaveCopy.Location = new System.Drawing.Point(6, 64);
            this.buttonConvoyToolsGPSTruckPositionMultySaveCopy.Name = "buttonConvoyToolsGPSTruckPositionMultySaveCopy";
            this.buttonConvoyToolsGPSTruckPositionMultySaveCopy.Size = new System.Drawing.Size(231, 52);
            this.buttonConvoyToolsGPSTruckPositionMultySaveCopy.TabIndex = 4;
            this.buttonConvoyToolsGPSTruckPositionMultySaveCopy.Text = "Copy truck position from multiple saves";
            this.buttonConvoyToolsGPSTruckPositionMultySaveCopy.UseVisualStyleBackColor = true;
            // 
            // buttonConvoyToolsGPSCurrentPositionPaste
            // 
            this.buttonConvoyToolsGPSCurrentPositionPaste.Location = new System.Drawing.Point(243, 6);
            this.buttonConvoyToolsGPSCurrentPositionPaste.Name = "buttonConvoyToolsGPSCurrentPositionPaste";
            this.buttonConvoyToolsGPSCurrentPositionPaste.Size = new System.Drawing.Size(231, 23);
            this.buttonConvoyToolsGPSCurrentPositionPaste.TabIndex = 3;
            this.buttonConvoyToolsGPSCurrentPositionPaste.Text = "Paste current position";
            this.buttonConvoyToolsGPSCurrentPositionPaste.UseVisualStyleBackColor = true;
            this.buttonConvoyToolsGPSCurrentPositionPaste.Click += new System.EventHandler(this.buttonGPSCurrentPositionPaste_Click);
            // 
            // buttonConvoyToolsGPSCurrentPositionCopy
            // 
            this.buttonConvoyToolsGPSCurrentPositionCopy.Location = new System.Drawing.Point(6, 6);
            this.buttonConvoyToolsGPSCurrentPositionCopy.Name = "buttonConvoyToolsGPSCurrentPositionCopy";
            this.buttonConvoyToolsGPSCurrentPositionCopy.Size = new System.Drawing.Size(231, 23);
            this.buttonConvoyToolsGPSCurrentPositionCopy.TabIndex = 2;
            this.buttonConvoyToolsGPSCurrentPositionCopy.Text = "Copy current position";
            this.buttonConvoyToolsGPSCurrentPositionCopy.UseVisualStyleBackColor = true;
            this.buttonConvoyToolsGPSCurrentPositionCopy.Click += new System.EventHandler(this.buttonGPSCurrentPositionCopy_Click);
            // 
            // buttonConvoyToolsGPSStoredGPSPathPaste
            // 
            this.buttonConvoyToolsGPSStoredGPSPathPaste.Location = new System.Drawing.Point(243, 35);
            this.buttonConvoyToolsGPSStoredGPSPathPaste.Name = "buttonConvoyToolsGPSStoredGPSPathPaste";
            this.buttonConvoyToolsGPSStoredGPSPathPaste.Size = new System.Drawing.Size(231, 23);
            this.buttonConvoyToolsGPSStoredGPSPathPaste.TabIndex = 1;
            this.buttonConvoyToolsGPSStoredGPSPathPaste.Text = "Paste GPS path";
            this.buttonConvoyToolsGPSStoredGPSPathPaste.UseVisualStyleBackColor = true;
            this.buttonConvoyToolsGPSStoredGPSPathPaste.Click += new System.EventHandler(this.buttonGPSStoredGPSPathPaste_Click);
            // 
            // buttonConvoyToolsGPSStoredGPSPathCopy
            // 
            this.buttonConvoyToolsGPSStoredGPSPathCopy.Location = new System.Drawing.Point(6, 35);
            this.buttonConvoyToolsGPSStoredGPSPathCopy.Name = "buttonConvoyToolsGPSStoredGPSPathCopy";
            this.buttonConvoyToolsGPSStoredGPSPathCopy.Size = new System.Drawing.Size(231, 23);
            this.buttonConvoyToolsGPSStoredGPSPathCopy.TabIndex = 0;
            this.buttonConvoyToolsGPSStoredGPSPathCopy.Text = "Copy GPS path";
            this.buttonConvoyToolsGPSStoredGPSPathCopy.UseVisualStyleBackColor = true;
            this.buttonConvoyToolsGPSStoredGPSPathCopy.Click += new System.EventHandler(this.buttonGPSStoredGPSPathCopy_Click);
            // 
            // buttonMainWriteSave
            // 
            this.buttonMainWriteSave.Enabled = false;
            this.buttonMainWriteSave.Location = new System.Drawing.Point(420, 119);
            this.buttonMainWriteSave.Name = "buttonMainWriteSave";
            this.buttonMainWriteSave.Size = new System.Drawing.Size(76, 45);
            this.buttonMainWriteSave.TabIndex = 7;
            this.buttonMainWriteSave.Text = "Save";
            this.buttonMainWriteSave.UseVisualStyleBackColor = true;
            this.buttonMainWriteSave.Click += new System.EventHandler(this.buttonWriteSave_Click);
            // 
            // buttonProfilesAndSavesOpenSaveFolder
            // 
            this.buttonProfilesAndSavesOpenSaveFolder.Location = new System.Drawing.Point(259, 77);
            this.buttonProfilesAndSavesOpenSaveFolder.Name = "buttonProfilesAndSavesOpenSaveFolder";
            this.buttonProfilesAndSavesOpenSaveFolder.Size = new System.Drawing.Size(134, 23);
            this.buttonProfilesAndSavesOpenSaveFolder.TabIndex = 8;
            this.buttonProfilesAndSavesOpenSaveFolder.Text = "Open Folder";
            this.buttonProfilesAndSavesOpenSaveFolder.UseVisualStyleBackColor = true;
            this.buttonProfilesAndSavesOpenSaveFolder.Click += new System.EventHandler(this.buttonOpenSaveFolder_Click);
            // 
            // statusStripMain
            // 
            this.statusStripMain.AutoSize = false;
            this.statusStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusMessages,
            this.toolStripProgressBarMain});
            this.statusStripMain.Location = new System.Drawing.Point(0, 669);
            this.statusStripMain.Name = "statusStripMain";
            this.statusStripMain.Size = new System.Drawing.Size(504, 22);
            this.statusStripMain.TabIndex = 9;
            this.statusStripMain.Text = "statusStrip1";
            // 
            // toolStripStatusMessages
            // 
            this.toolStripStatusMessages.AutoSize = false;
            this.toolStripStatusMessages.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolStripStatusMessages.Name = "toolStripStatusMessages";
            this.toolStripStatusMessages.Size = new System.Drawing.Size(370, 17);
            this.toolStripStatusMessages.Text = "Not Ready";
            this.toolStripStatusMessages.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripProgressBarMain
            // 
            this.toolStripProgressBarMain.Name = "toolStripProgressBarMain";
            this.toolStripProgressBarMain.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.toolStripProgressBarMain.Size = new System.Drawing.Size(100, 16);
            this.toolStripProgressBarMain.Step = 1;
            this.toolStripProgressBarMain.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // buttonMainLoadSave
            // 
            this.buttonMainLoadSave.Location = new System.Drawing.Point(420, 90);
            this.buttonMainLoadSave.Name = "buttonMainLoadSave";
            this.buttonMainLoadSave.Size = new System.Drawing.Size(76, 23);
            this.buttonMainLoadSave.TabIndex = 11;
            this.buttonMainLoadSave.Text = "Load";
            this.buttonMainLoadSave.UseVisualStyleBackColor = true;
            this.buttonMainLoadSave.Click += new System.EventHandler(this.LoadSaveFile_Click);
            // 
            // pictureBoxProfileAvatar
            // 
            this.pictureBoxProfileAvatar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxProfileAvatar.Location = new System.Drawing.Point(6, 19);
            this.pictureBoxProfileAvatar.Name = "pictureBoxProfileAvatar";
            this.pictureBoxProfileAvatar.Size = new System.Drawing.Size(81, 81);
            this.pictureBoxProfileAvatar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxProfileAvatar.TabIndex = 12;
            this.pictureBoxProfileAvatar.TabStop = false;
            // 
            // buttonMainGameSwitchCustomFolder
            // 
            this.buttonMainGameSwitchCustomFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMainGameSwitchCustomFolder.Enabled = false;
            this.buttonMainGameSwitchCustomFolder.Location = new System.Drawing.Point(346, 27);
            this.buttonMainGameSwitchCustomFolder.Name = "buttonMainGameSwitchCustomFolder";
            this.buttonMainGameSwitchCustomFolder.Size = new System.Drawing.Size(150, 23);
            this.buttonMainGameSwitchCustomFolder.TabIndex = 14;
            this.buttonMainGameSwitchCustomFolder.Text = "Add Custom Folder";
            this.buttonMainGameSwitchCustomFolder.UseVisualStyleBackColor = true;
            this.buttonMainGameSwitchCustomFolder.Click += new System.EventHandler(this.AddCustomFolder_Click);
            // 
            // buttonMainGameSwitchETS
            // 
            this.buttonMainGameSwitchETS.BackColor = System.Drawing.SystemColors.Control;
            this.buttonMainGameSwitchETS.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonMainGameSwitchETS.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonMainGameSwitchETS.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonMainGameSwitchETS.Location = new System.Drawing.Point(12, 27);
            this.buttonMainGameSwitchETS.Name = "buttonMainGameSwitchETS";
            this.buttonMainGameSwitchETS.Size = new System.Drawing.Size(150, 23);
            this.buttonMainGameSwitchETS.TabIndex = 15;
            this.buttonMainGameSwitchETS.Text = "ETS2";
            this.buttonMainGameSwitchETS.UseVisualStyleBackColor = false;
            this.buttonMainGameSwitchETS.Click += new System.EventHandler(this.ToggleGame_Click);
            // 
            // buttonMainGameSwitchATS
            // 
            this.buttonMainGameSwitchATS.Enabled = false;
            this.buttonMainGameSwitchATS.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonMainGameSwitchATS.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonMainGameSwitchATS.Location = new System.Drawing.Point(168, 27);
            this.buttonMainGameSwitchATS.Name = "buttonMainGameSwitchATS";
            this.buttonMainGameSwitchATS.Size = new System.Drawing.Size(150, 23);
            this.buttonMainGameSwitchATS.TabIndex = 16;
            this.buttonMainGameSwitchATS.Text = "ATS";
            this.buttonMainGameSwitchATS.UseVisualStyleBackColor = true;
            this.buttonMainGameSwitchATS.Click += new System.EventHandler(this.ToggleGame_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // comboBoxPrevProfiles
            // 
            this.comboBoxPrevProfiles.BackColor = System.Drawing.SystemColors.Window;
            this.comboBoxPrevProfiles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPrevProfiles.FormattingEnabled = true;
            this.comboBoxPrevProfiles.Location = new System.Drawing.Point(93, 19);
            this.comboBoxPrevProfiles.Name = "comboBoxPrevProfiles";
            this.comboBoxPrevProfiles.Size = new System.Drawing.Size(160, 21);
            this.comboBoxPrevProfiles.TabIndex = 19;
            this.comboBoxPrevProfiles.SelectedIndexChanged += new System.EventHandler(this.comboBoxPrevProfiles_SelectedIndexChanged);
            // 
            // checkBoxProfilesAndSavesProfileBackups
            // 
            this.checkBoxProfilesAndSavesProfileBackups.AutoSize = true;
            this.checkBoxProfilesAndSavesProfileBackups.Location = new System.Drawing.Point(259, 23);
            this.checkBoxProfilesAndSavesProfileBackups.Name = "checkBoxProfilesAndSavesProfileBackups";
            this.checkBoxProfilesAndSavesProfileBackups.Size = new System.Drawing.Size(68, 17);
            this.checkBoxProfilesAndSavesProfileBackups.TabIndex = 20;
            this.checkBoxProfilesAndSavesProfileBackups.Text = "Backups";
            this.checkBoxProfilesAndSavesProfileBackups.UseVisualStyleBackColor = true;
            this.checkBoxProfilesAndSavesProfileBackups.CheckedChanged += new System.EventHandler(this.checkBoxProfileBackups_CheckedChanged);
            // 
            // groupBoxMainProfilesAndSaves
            // 
            this.groupBoxMainProfilesAndSaves.Controls.Add(this.checkBoxProfilesAndSavesProfileBackups);
            this.groupBoxMainProfilesAndSaves.Controls.Add(this.pictureBoxProfileAvatar);
            this.groupBoxMainProfilesAndSaves.Controls.Add(this.comboBoxPrevProfiles);
            this.groupBoxMainProfilesAndSaves.Controls.Add(this.comboBoxProfiles);
            this.groupBoxMainProfilesAndSaves.Controls.Add(this.buttonProfilesAndSavesRefreshAll);
            this.groupBoxMainProfilesAndSaves.Controls.Add(this.comboBoxSaves);
            this.groupBoxMainProfilesAndSaves.Controls.Add(this.buttonProfilesAndSavesOpenSaveFolder);
            this.groupBoxMainProfilesAndSaves.Location = new System.Drawing.Point(12, 56);
            this.groupBoxMainProfilesAndSaves.Name = "groupBoxMainProfilesAndSaves";
            this.groupBoxMainProfilesAndSaves.Size = new System.Drawing.Size(402, 108);
            this.groupBoxMainProfilesAndSaves.TabIndex = 21;
            this.groupBoxMainProfilesAndSaves.TabStop = false;
            this.groupBoxMainProfilesAndSaves.Text = "Profiles And Saves";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 691);
            this.Controls.Add(this.groupBoxMainProfilesAndSaves);
            this.Controls.Add(this.buttonMainGameSwitchATS);
            this.Controls.Add(this.buttonMainGameSwitchETS);
            this.Controls.Add(this.buttonMainLoadSave);
            this.Controls.Add(this.statusStripMain);
            this.Controls.Add(this.buttonMainWriteSave);
            this.Controls.Add(this.buttonMainGameSwitchCustomFolder);
            this.Controls.Add(this.tabControlMain);
            this.Controls.Add(this.buttonMainDecryptSave);
            this.Controls.Add(this.menuStripMain);
            this.MainMenuStrip = this.menuStripMain;
            this.MaximumSize = new System.Drawing.Size(520, 730);
            this.MinimumSize = new System.Drawing.Size(520, 730);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TS SE Tool";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.Shown += new System.EventHandler(this.FormMain_Shown);
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.tabControlMain.ResumeLayout(false);
            this.tabPageProfile.ResumeLayout(false);
            this.groupBoxProfilePlayerLevel.ResumeLayout(false);
            this.groupBoxProfilePlayerLevel.PerformLayout();
            this.panelPlayerLevel.ResumeLayout(false);
            this.panelPlayerLevel.PerformLayout();
            this.groupBoxProfileUserColors.ResumeLayout(false);
            this.tabPageCompany.ResumeLayout(false);
            this.tabPageCompany.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCompanyLogo)).EndInit();
            this.tabPageTruck.ResumeLayout(false);
            this.tabPageTruck.PerformLayout();
            this.groupBoxUserTruckShareTruckSettings.ResumeLayout(false);
            this.groupBoxUserTruckTruckDetails.ResumeLayout(false);
            this.groupBoxUserTruckTruckDetails.PerformLayout();
            this.tabPageTrailer.ResumeLayout(false);
            this.tabPageTrailer.PerformLayout();
            this.groupBoxUserTrailerTrailerDetails.ResumeLayout(false);
            this.groupBoxUserTrailerTrailerDetails.PerformLayout();
            this.tabPageFreightMarket.ResumeLayout(false);
            this.tabPageFreightMarket.PerformLayout();
            this.tabPageCargoMarket.ResumeLayout(false);
            this.tabPageCargoMarket.PerformLayout();
            this.tabPageConvoyTools.ResumeLayout(false);
            this.statusStripMain.ResumeLayout(false);
            this.statusStripMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProfileAvatar)).EndInit();
            this.groupBoxMainProfilesAndSaves.ResumeLayout(false);
            this.groupBoxMainProfilesAndSaves.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStripMain;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemProgram;
        private System.Windows.Forms.ComboBox comboBoxProfiles;
        private System.Windows.Forms.Button buttonProfilesAndSavesRefreshAll;
        private System.Windows.Forms.ComboBox comboBoxSaves;
        private System.Windows.Forms.Button buttonMainDecryptSave;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageProfile;
        private System.Windows.Forms.TabPage tabPageTruck;
        private System.Windows.Forms.Label labelUserTruckTruck;
        private System.Windows.Forms.Button buttonMainWriteSave;
        private System.Windows.Forms.Button buttonProfilesAndSavesOpenSaveFolder;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExit;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemLanguage;
        private System.Windows.Forms.StatusStrip statusStripMain;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusMessages;
        private System.Windows.Forms.Button buttonMainLoadSave;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBarMain;
        private System.Windows.Forms.TabPage tabPageTrailer;
        private System.Windows.Forms.TabPage tabPageFreightMarket;
        private System.Windows.Forms.ComboBox comboBoxFreightMarketDestinationCompany;
        private System.Windows.Forms.Label labelFreightMarketDestination;
        private System.Windows.Forms.ComboBox comboBoxFreightMarketDestinationCity;
        private System.Windows.Forms.Label labelFreightMarketCompany;
        private System.Windows.Forms.Label labelFreightMarketCity;
        private System.Windows.Forms.ComboBox comboBoxFreightMarketSourceCompany;
        private System.Windows.Forms.Label labelFreightMarketSource;
        private System.Windows.Forms.ComboBox comboBoxFreightMarketSourceCity;
        private System.Windows.Forms.PictureBox pictureBoxProfileAvatar;
        private System.Windows.Forms.Button buttonFreightMarketClearJobList;
        private System.Windows.Forms.Button buttonFreightMarketAddJob;
        private System.Windows.Forms.Label labelFreightMarketUrgency;
        private System.Windows.Forms.ComboBox comboBoxFreightMarketUrgency;
        private System.Windows.Forms.Label labelFreightMarketCargo;
        private System.Windows.Forms.ComboBox comboBoxFreightMarketCargoList;
        private System.Windows.Forms.Button buttonMainGameSwitchCustomFolder;
        private System.Windows.Forms.Button buttonMainGameSwitchETS;
        private System.Windows.Forms.Button buttonMainGameSwitchATS;
        private System.Windows.Forms.ComboBox comboBoxFreightMarketCompanies;
        private System.Windows.Forms.ComboBox comboBoxFreightMarketCountries;
        private System.Windows.Forms.Label labelFreightMarketFilterMain;
        private System.Windows.Forms.Label labelFreightMarketCountryF;
        private System.Windows.Forms.Label labelFreightMarketCompanyF;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.CheckBox checkBoxFreightMarketFilterDestination;
        private System.Windows.Forms.CheckBox checkBoxFreightMarketFilterSource;
        private System.Windows.Forms.GroupBox groupBoxProfileUserColors;
        private System.Windows.Forms.CheckBox checkBoxFreightMarketRandomDest;
        private System.Windows.Forms.TabPage tabPageConvoyTools;
        private System.Windows.Forms.ComboBox comboBoxPrevProfiles;
        private System.Windows.Forms.CheckBox checkBoxProfilesAndSavesProfileBackups;
        private System.Windows.Forms.TabPage tabPageCompany;
        private System.Windows.Forms.Label labelUserCompanyCompanyName;
        private System.Windows.Forms.TextBox textBoxUserCompanyCompanyName;
        private System.Windows.Forms.ComboBox comboBoxUserCompanyHQcity;
        private System.Windows.Forms.Label labelUserCompanyHQcity;
        private System.Windows.Forms.Button buttonPlayerLevelMaximum;
        private System.Windows.Forms.Button buttonPlayerLevelPlus10;
        private System.Windows.Forms.Button buttonPlayerLevelPlus01;
        private System.Windows.Forms.Label labelPlayerLevelNumber;
        private System.Windows.Forms.Label labelPlayerLevelName;
        private System.Windows.Forms.Label labelUserCompanyVisitedCities;
        private System.Windows.Forms.ListBox listBoxVisitedCities;
        private System.Windows.Forms.Label labelUserCompanyGarages;
        private System.Windows.Forms.ListBox listBoxGarages;
        private System.Windows.Forms.ToolTip toolTipMain;
        private System.Windows.Forms.GroupBox groupBoxProfileSkill;
        private System.Windows.Forms.Button buttonPlayerLevelMinus10;
        private System.Windows.Forms.Button buttonPlayerLevelMinus01;
        private System.Windows.Forms.Button buttonPlayerLevelMinimum;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSettings;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemCreateTrFile;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemHelp;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemAbout;
        private System.Windows.Forms.Label labelExperienceNxtLvlThreshhold;
        private System.Windows.Forms.ComboBox comboBoxUserTruckCompanyTrucks;
        private System.Windows.Forms.Button buttonUserTruckSwitchCurrent;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.GroupBox groupBoxUserTruckTruckDetails;
        private System.Windows.Forms.Button buttonUserTruckSelectCurrent;
        private System.Windows.Forms.Panel panelPlayerLevel;
        private System.Windows.Forms.ListBox listBoxFreightMarketAddedJobs;
        private System.Windows.Forms.Label labelUserCompanyMoneyAccount;
        private System.Windows.Forms.TextBox textBoxUserCompanyMoneyAccount;
        private System.Windows.Forms.Button buttonUserTrailerSelectCurrent;
        private System.Windows.Forms.GroupBox groupBoxUserTrailerTrailerDetails;
        private System.Windows.Forms.Button buttonUserTrailerSwitchCurrent;
        private System.Windows.Forms.ComboBox comboBoxUserTrailerCompanyTrailers;
        private System.Windows.Forms.Label labelUserTrailerTrailer;
        private System.Windows.Forms.Button buttonUserCompanyGaragesSell;
        private System.Windows.Forms.Button buttonUserCompanyCitiesUnVisit;
        private System.Windows.Forms.Button buttonUserCompanyCitiesVisit;
        private System.Windows.Forms.Button buttonUserCompanyGaragesBuyUpgrade;
        private System.Windows.Forms.Button buttonUserCompanyGaragesUpgrade;
        private System.Windows.Forms.Button buttonUserCompanyGaragesBuy;
        private System.Windows.Forms.GroupBox groupBoxUserTruckShareTruckSettings;
        private System.Windows.Forms.Button buttonShareTruckTruckTruckCopy;
        private System.Windows.Forms.Button buttonShareTruckTruckDetailsCopy;
        private System.Windows.Forms.Button buttonShareTruckTruckPaintPaste;
        private System.Windows.Forms.Button buttonShareTruckTruckPaintCopy;
        private System.Windows.Forms.Label labelFreightMarketDistance;
        private System.Windows.Forms.GroupBox groupBoxProfilePlayerLevel;
        private System.Windows.Forms.Button buttonConvoyToolsGPSTruckPositionMultySavePaste;
        private System.Windows.Forms.Button buttonConvoyToolsGPSTruckPositionMultySaveCopy;
        private System.Windows.Forms.Button buttonConvoyToolsGPSCurrentPositionPaste;
        private System.Windows.Forms.Button buttonConvoyToolsGPSCurrentPositionCopy;
        private System.Windows.Forms.Button buttonConvoyToolsGPSStoredGPSPathPaste;
        private System.Windows.Forms.Button buttonConvoyToolsGPSStoredGPSPathCopy;
        private System.Windows.Forms.Label labelLicensePlate;
        private System.Windows.Forms.Label labelUserTruckLicensePlate;
        private System.Windows.Forms.Button buttonShareTruckTruckTruckPaste;
        private System.Windows.Forms.Button buttonShareTruckTruckDetailsPaste;
        private System.Windows.Forms.GroupBox groupBoxMainProfilesAndSaves;
        private System.Windows.Forms.Button buttonFreightMarketRandomizeCargo;
        private System.Windows.Forms.PictureBox pictureBoxCompanyLogo;
        private System.Windows.Forms.Button buttonUserColorsShareColors;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TabPage tabPageCargoMarket;
        private System.Windows.Forms.Label labelCargoMarketSource;
        private System.Windows.Forms.Label labelCargoMarketCompany;
        private System.Windows.Forms.ComboBox comboBoxCargoMarketSourceCity;
        private System.Windows.Forms.Label labelCargoMarketCity;
        private System.Windows.Forms.ComboBox comboBoxSourceCargoMarketCompany;
        private System.Windows.Forms.ListBox listBoxCargoMarketSourceCargoSeeds;
        private System.Windows.Forms.ListBox listBoxCargoMarketCargoListForCompany;
        private System.Windows.Forms.Button buttonCargoMarketRandomizeCargoCompany;
        private System.Windows.Forms.Button buttonCargoMarketRandomizeCargoCity;
        private System.Windows.Forms.Button buttonCargoMarketResetCargoCompany;
        private System.Windows.Forms.Button buttonCargoMarketResetCargoCity;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelUserTrailerLicensePlate;
        private System.Windows.Forms.Label labelFreightMarketDistanceNumbers;
        private System.Windows.Forms.Label labelPlayerExperience;
        private System.Windows.Forms.Label label1;
    }
}

