namespace HotspotShare
{
	partial class frmHotspot
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
			System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "Sample",
            "HTC , 192.168.137.100 , a0:af:bf:cf:df:ef ,  2014/01/02 15:22",
            "2014/1/1"}, 0, System.Drawing.SystemColors.WindowText, System.Drawing.SystemColors.Window, new System.Drawing.Font("Tahoma", 8.25F));
			System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] {
            "Sample2",
            "Galaxy"}, 0, System.Drawing.SystemColors.WindowText, System.Drawing.SystemColors.Window, new System.Drawing.Font("Tahoma", 8.25F));
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmHotspot));
			this.tabMain = new System.Windows.Forms.TabControl();
			this.tabHotspot = new System.Windows.Forms.TabPage();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.label2 = new System.Windows.Forms.Label();
			this.lblStatus = new System.Windows.Forms.Label();
			this.btnStartStop = new System.Windows.Forms.Button();
			this.gpbInternet = new System.Windows.Forms.GroupBox();
			this.cmdSharedConnection = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.chkAutoDetectInternet = new System.Windows.Forms.CheckBox();
			this.gpbSettings = new System.Windows.Forms.GroupBox();
			this.passwordLabel = new System.Windows.Forms.Label();
			this.ssidLabel = new System.Windows.Forms.Label();
			this.txtSSID = new System.Windows.Forms.TextBox();
			this.txtPass = new System.Windows.Forms.TextBox();
			this.gpbxOptions = new System.Windows.Forms.GroupBox();
			this.chkAutoStartWindows = new System.Windows.Forms.CheckBox();
			this.tabUsers = new System.Windows.Forms.TabPage();
			this.gpbxUsers = new System.Windows.Forms.GroupBox();
			this.lstUsers = new System.Windows.Forms.ListView();
			this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colVendor = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colConnectedFrom = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colIP = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colMac = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.mnuUsers = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mnuCopyUserIP = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuCopyUserMACAddress = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuCopyUserHostname = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuCopyUserInfo = new System.Windows.Forms.ToolStripMenuItem();
			this.imgIcons20 = new System.Windows.Forms.ImageList(this.components);
			this.btnRefresh = new System.Windows.Forms.Button();
			this.gpbxUsersSettings = new System.Windows.Forms.GroupBox();
			this.chkUsersNotifyUserConnecting = new System.Windows.Forms.CheckBox();
			this.chkUsersNotifyNewUser = new System.Windows.Forms.CheckBox();
			this.lnkAbout = new System.Windows.Forms.LinkLabel();
			this.sysIcon = new System.Windows.Forms.NotifyIcon(this.components);
			this.mnuSys = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mnuShow = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
			this.tmrStationsList = new System.Windows.Forms.Timer(this.components);
			this.tabMain.SuspendLayout();
			this.tabHotspot.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.gpbInternet.SuspendLayout();
			this.gpbSettings.SuspendLayout();
			this.gpbxOptions.SuspendLayout();
			this.tabUsers.SuspendLayout();
			this.gpbxUsers.SuspendLayout();
			this.mnuUsers.SuspendLayout();
			this.gpbxUsersSettings.SuspendLayout();
			this.mnuSys.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabMain
			// 
			this.tabMain.Controls.Add(this.tabHotspot);
			this.tabMain.Controls.Add(this.tabUsers);
			this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabMain.Location = new System.Drawing.Point(0, 0);
			this.tabMain.Name = "tabMain";
			this.tabMain.SelectedIndex = 0;
			this.tabMain.Size = new System.Drawing.Size(293, 383);
			this.tabMain.TabIndex = 1;
			// 
			// tabHotspot
			// 
			this.tabHotspot.Controls.Add(this.tableLayoutPanel1);
			this.tabHotspot.Controls.Add(this.btnStartStop);
			this.tabHotspot.Controls.Add(this.gpbInternet);
			this.tabHotspot.Controls.Add(this.gpbSettings);
			this.tabHotspot.Controls.Add(this.gpbxOptions);
			this.tabHotspot.Location = new System.Drawing.Point(4, 22);
			this.tabHotspot.Name = "tabHotspot";
			this.tabHotspot.Padding = new System.Windows.Forms.Padding(3);
			this.tabHotspot.Size = new System.Drawing.Size(285, 357);
			this.tabHotspot.TabIndex = 0;
			this.tabHotspot.Text = "Hotspot";
			this.tabHotspot.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 79F));
			this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblStatus, 1, 0);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 329);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(175, 17);
			this.tableLayoutPanel1.TabIndex = 7;
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label2.AutoSize = true;
			this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label2.Location = new System.Drawing.Point(3, 4);
			this.label2.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(42, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Status:";
			// 
			// lblStatus
			// 
			this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblStatus.AutoSize = true;
			this.lblStatus.ForeColor = System.Drawing.Color.Red;
			this.lblStatus.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.lblStatus.Location = new System.Drawing.Point(48, 4);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(47, 13);
			this.lblStatus.TabIndex = 4;
			this.lblStatus.Text = "Stopped";
			// 
			// btnStartStop
			// 
			this.btnStartStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnStartStop.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.btnStartStop.Location = new System.Drawing.Point(187, 319);
			this.btnStartStop.Name = "btnStartStop";
			this.btnStartStop.Size = new System.Drawing.Size(90, 30);
			this.btnStartStop.TabIndex = 2;
			this.btnStartStop.Text = "Start";
			this.btnStartStop.UseVisualStyleBackColor = true;
			this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
			// 
			// gpbInternet
			// 
			this.gpbInternet.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gpbInternet.Controls.Add(this.cmdSharedConnection);
			this.gpbInternet.Controls.Add(this.label1);
			this.gpbInternet.Controls.Add(this.chkAutoDetectInternet);
			this.gpbInternet.Location = new System.Drawing.Point(6, 135);
			this.gpbInternet.Name = "gpbInternet";
			this.gpbInternet.Size = new System.Drawing.Size(273, 102);
			this.gpbInternet.TabIndex = 1;
			this.gpbInternet.TabStop = false;
			this.gpbInternet.Text = "Internet";
			// 
			// cmdSharedConnection
			// 
			this.cmdSharedConnection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdSharedConnection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmdSharedConnection.Enabled = false;
			this.cmdSharedConnection.FormattingEnabled = true;
			this.cmdSharedConnection.Location = new System.Drawing.Point(6, 68);
			this.cmdSharedConnection.Name = "cmdSharedConnection";
			this.cmdSharedConnection.Size = new System.Drawing.Size(261, 21);
			this.cmdSharedConnection.TabIndex = 0;
			this.cmdSharedConnection.SelectedIndexChanged += new System.EventHandler(this.cmdSharedConnection_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label1.Location = new System.Drawing.Point(6, 52);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(261, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Default Connection to share:";
			// 
			// chkAutoDetectInternet
			// 
			this.chkAutoDetectInternet.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.chkAutoDetectInternet.Checked = true;
			this.chkAutoDetectInternet.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkAutoDetectInternet.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.chkAutoDetectInternet.Location = new System.Drawing.Point(6, 25);
			this.chkAutoDetectInternet.Name = "chkAutoDetectInternet";
			this.chkAutoDetectInternet.Size = new System.Drawing.Size(261, 17);
			this.chkAutoDetectInternet.TabIndex = 0;
			this.chkAutoDetectInternet.Text = "Auto detect internet connection";
			this.chkAutoDetectInternet.UseVisualStyleBackColor = true;
			this.chkAutoDetectInternet.CheckedChanged += new System.EventHandler(this.chkAutoDetectInternet_CheckedChanged);
			// 
			// gpbSettings
			// 
			this.gpbSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gpbSettings.Controls.Add(this.passwordLabel);
			this.gpbSettings.Controls.Add(this.ssidLabel);
			this.gpbSettings.Controls.Add(this.txtSSID);
			this.gpbSettings.Controls.Add(this.txtPass);
			this.gpbSettings.Location = new System.Drawing.Point(6, 6);
			this.gpbSettings.Name = "gpbSettings";
			this.gpbSettings.Size = new System.Drawing.Size(273, 123);
			this.gpbSettings.TabIndex = 0;
			this.gpbSettings.TabStop = false;
			this.gpbSettings.Text = "Settings";
			// 
			// passwordLabel
			// 
			this.passwordLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.passwordLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.passwordLabel.Location = new System.Drawing.Point(6, 74);
			this.passwordLabel.Name = "passwordLabel";
			this.passwordLabel.Size = new System.Drawing.Size(261, 13);
			this.passwordLabel.TabIndex = 5;
			this.passwordLabel.Text = "Password: ";
			// 
			// ssidLabel
			// 
			this.ssidLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ssidLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.ssidLabel.Location = new System.Drawing.Point(6, 25);
			this.ssidLabel.Name = "ssidLabel";
			this.ssidLabel.Size = new System.Drawing.Size(261, 13);
			this.ssidLabel.TabIndex = 4;
			this.ssidLabel.Text = "Network Name (SSID): ";
			// 
			// txtSSID
			// 
			this.txtSSID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtSSID.Location = new System.Drawing.Point(6, 41);
			this.txtSSID.MaxLength = 32;
			this.txtSSID.Name = "txtSSID";
			this.txtSSID.Size = new System.Drawing.Size(261, 21);
			this.txtSSID.TabIndex = 0;
			// 
			// txtPass
			// 
			this.txtPass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtPass.Location = new System.Drawing.Point(6, 90);
			this.txtPass.Name = "txtPass";
			this.txtPass.PasswordChar = '●';
			this.txtPass.Size = new System.Drawing.Size(261, 21);
			this.txtPass.TabIndex = 1;
			this.txtPass.Enter += new System.EventHandler(this.txtPass_Enter);
			this.txtPass.Leave += new System.EventHandler(this.txtPass_Leave);
			// 
			// gpbxOptions
			// 
			this.gpbxOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gpbxOptions.Controls.Add(this.chkAutoStartWindows);
			this.gpbxOptions.Location = new System.Drawing.Point(6, 243);
			this.gpbxOptions.Name = "gpbxOptions";
			this.gpbxOptions.Size = new System.Drawing.Size(273, 48);
			this.gpbxOptions.TabIndex = 6;
			this.gpbxOptions.TabStop = false;
			this.gpbxOptions.Text = "Options";
			// 
			// chkAutoStartWindows
			// 
			this.chkAutoStartWindows.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.chkAutoStartWindows.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.chkAutoStartWindows.Location = new System.Drawing.Point(8, 20);
			this.chkAutoStartWindows.Name = "chkAutoStartWindows";
			this.chkAutoStartWindows.Size = new System.Drawing.Size(259, 17);
			this.chkAutoStartWindows.TabIndex = 5;
			this.chkAutoStartWindows.Text = "Autostart with windows";
			this.chkAutoStartWindows.UseVisualStyleBackColor = true;
			this.chkAutoStartWindows.CheckedChanged += new System.EventHandler(this.chkAutoStartWindows_CheckedChanged);
			// 
			// tabUsers
			// 
			this.tabUsers.Controls.Add(this.gpbxUsers);
			this.tabUsers.Controls.Add(this.gpbxUsersSettings);
			this.tabUsers.Location = new System.Drawing.Point(4, 22);
			this.tabUsers.Name = "tabUsers";
			this.tabUsers.Padding = new System.Windows.Forms.Padding(3);
			this.tabUsers.Size = new System.Drawing.Size(285, 357);
			this.tabUsers.TabIndex = 1;
			this.tabUsers.Text = "Users";
			this.tabUsers.UseVisualStyleBackColor = true;
			// 
			// gpbxUsers
			// 
			this.gpbxUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gpbxUsers.Controls.Add(this.lstUsers);
			this.gpbxUsers.Controls.Add(this.btnRefresh);
			this.gpbxUsers.Location = new System.Drawing.Point(6, 78);
			this.gpbxUsers.Name = "gpbxUsers";
			this.gpbxUsers.Size = new System.Drawing.Size(273, 271);
			this.gpbxUsers.TabIndex = 1;
			this.gpbxUsers.TabStop = false;
			this.gpbxUsers.Text = "Users";
			// 
			// lstUsers
			// 
			this.lstUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lstUsers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colVendor,
            this.colConnectedFrom,
            this.colIP,
            this.colMac});
			this.lstUsers.ContextMenuStrip = this.mnuUsers;
			this.lstUsers.FullRowSelect = true;
			this.lstUsers.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2});
			this.lstUsers.LargeImageList = this.imgIcons20;
			this.lstUsers.Location = new System.Drawing.Point(6, 20);
			this.lstUsers.MultiSelect = false;
			this.lstUsers.Name = "lstUsers";
			this.lstUsers.ShowItemToolTips = true;
			this.lstUsers.Size = new System.Drawing.Size(261, 216);
			this.lstUsers.TabIndex = 0;
			this.lstUsers.TileSize = new System.Drawing.Size(350, 35);
			this.lstUsers.UseCompatibleStateImageBehavior = false;
			this.lstUsers.View = System.Windows.Forms.View.Tile;
			// 
			// colName
			// 
			this.colName.Text = "Name";
			this.colName.Width = 150;
			// 
			// colVendor
			// 
			this.colVendor.Text = "Vendor";
			this.colVendor.Width = 150;
			// 
			// colConnectedFrom
			// 
			this.colConnectedFrom.Text = "Joined";
			this.colConnectedFrom.Width = 135;
			// 
			// colIP
			// 
			this.colIP.Text = "IP";
			this.colIP.Width = 100;
			// 
			// colMac
			// 
			this.colMac.Text = "MAC";
			this.colMac.Width = 130;
			// 
			// mnuUsers
			// 
			this.mnuUsers.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuCopyUserInfo,
            this.mnuCopyUserIP,
            this.mnuCopyUserMACAddress,
            this.mnuCopyUserHostname});
			this.mnuUsers.Name = "mnuUsers";
			this.mnuUsers.Size = new System.Drawing.Size(178, 92);
			this.mnuUsers.Opening += new System.ComponentModel.CancelEventHandler(this.mnuUsers_Opening);
			// 
			// mnuCopyUserIP
			// 
			this.mnuCopyUserIP.Name = "mnuCopyUserIP";
			this.mnuCopyUserIP.Size = new System.Drawing.Size(177, 22);
			this.mnuCopyUserIP.Text = "Copy IP Address";
			this.mnuCopyUserIP.Click += new System.EventHandler(this.mnuCopyUserIP_Click);
			// 
			// mnuCopyUserMACAddress
			// 
			this.mnuCopyUserMACAddress.Name = "mnuCopyUserMACAddress";
			this.mnuCopyUserMACAddress.Size = new System.Drawing.Size(177, 22);
			this.mnuCopyUserMACAddress.Text = "Copy MAC Address";
			this.mnuCopyUserMACAddress.Click += new System.EventHandler(this.mnuCopyUserMACAddress_Click);
			// 
			// mnuCopyUserHostname
			// 
			this.mnuCopyUserHostname.Name = "mnuCopyUserHostname";
			this.mnuCopyUserHostname.Size = new System.Drawing.Size(177, 22);
			this.mnuCopyUserHostname.Text = "Copy Hostname";
			this.mnuCopyUserHostname.Click += new System.EventHandler(this.mnuCopyUserHostname_Click);
			// 
			// mnuCopyUserInfo
			// 
			this.mnuCopyUserInfo.Name = "mnuCopyUserInfo";
			this.mnuCopyUserInfo.Size = new System.Drawing.Size(177, 22);
			this.mnuCopyUserInfo.Text = "Copy User Info";
			this.mnuCopyUserInfo.Click += new System.EventHandler(this.mnuCopyUserInfo_Click);
			// 
			// imgIcons20
			// 
			this.imgIcons20.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgIcons20.ImageStream")));
			this.imgIcons20.TransparentColor = System.Drawing.Color.Transparent;
			this.imgIcons20.Images.SetKeyName(0, "station-20.png");
			// 
			// btnRefresh
			// 
			this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnRefresh.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.btnRefresh.Location = new System.Drawing.Point(192, 242);
			this.btnRefresh.Name = "btnRefresh";
			this.btnRefresh.Size = new System.Drawing.Size(75, 23);
			this.btnRefresh.TabIndex = 1;
			this.btnRefresh.Text = "Refresh";
			this.btnRefresh.UseVisualStyleBackColor = true;
			this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
			// 
			// gpbxUsersSettings
			// 
			this.gpbxUsersSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gpbxUsersSettings.Controls.Add(this.chkUsersNotifyUserConnecting);
			this.gpbxUsersSettings.Controls.Add(this.chkUsersNotifyNewUser);
			this.gpbxUsersSettings.Location = new System.Drawing.Point(6, 6);
			this.gpbxUsersSettings.Name = "gpbxUsersSettings";
			this.gpbxUsersSettings.Size = new System.Drawing.Size(273, 66);
			this.gpbxUsersSettings.TabIndex = 0;
			this.gpbxUsersSettings.TabStop = false;
			this.gpbxUsersSettings.Text = "Settings";
			// 
			// chkUsersNotifyUserConnecting
			// 
			this.chkUsersNotifyUserConnecting.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.chkUsersNotifyUserConnecting.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.chkUsersNotifyUserConnecting.Location = new System.Drawing.Point(6, 20);
			this.chkUsersNotifyUserConnecting.Name = "chkUsersNotifyUserConnecting";
			this.chkUsersNotifyUserConnecting.Size = new System.Drawing.Size(261, 17);
			this.chkUsersNotifyUserConnecting.TabIndex = 0;
			this.chkUsersNotifyUserConnecting.Text = "Notify when someone is connecting";
			this.chkUsersNotifyUserConnecting.UseVisualStyleBackColor = true;
			// 
			// chkUsersNotifyNewUser
			// 
			this.chkUsersNotifyNewUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.chkUsersNotifyNewUser.Checked = true;
			this.chkUsersNotifyNewUser.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkUsersNotifyNewUser.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.chkUsersNotifyNewUser.Location = new System.Drawing.Point(6, 43);
			this.chkUsersNotifyNewUser.Name = "chkUsersNotifyNewUser";
			this.chkUsersNotifyNewUser.Size = new System.Drawing.Size(261, 17);
			this.chkUsersNotifyNewUser.TabIndex = 1;
			this.chkUsersNotifyNewUser.Text = "Notify when a new user is connected";
			this.chkUsersNotifyNewUser.UseVisualStyleBackColor = true;
			// 
			// lnkAbout
			// 
			this.lnkAbout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lnkAbout.BackColor = System.Drawing.Color.Transparent;
			this.lnkAbout.Image = ((System.Drawing.Image)(resources.GetObject("lnkAbout.Image")));
			this.lnkAbout.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lnkAbout.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.lnkAbout.Location = new System.Drawing.Point(236, 4);
			this.lnkAbout.Name = "lnkAbout";
			this.lnkAbout.Size = new System.Drawing.Size(53, 13);
			this.lnkAbout.TabIndex = 5;
			this.lnkAbout.TabStop = true;
			this.lnkAbout.Text = "About";
			this.lnkAbout.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.lnkAbout.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkAbout_LinkClicked);
			// 
			// sysIcon
			// 
			this.sysIcon.BalloonTipTitle = "Hotspot Share";
			this.sysIcon.ContextMenuStrip = this.mnuSys;
			this.sysIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("sysIcon.Icon")));
			this.sysIcon.Text = "Hotspot Share";
			this.sysIcon.Visible = true;
			this.sysIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.sysIcon_MouseClick);
			// 
			// mnuSys
			// 
			this.mnuSys.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuShow,
            this.toolStripSeparator1,
            this.mnuExit});
			this.mnuSys.Name = "mnuSys";
			this.mnuSys.Size = new System.Drawing.Size(182, 54);
			// 
			// mnuShow
			// 
			this.mnuShow.Name = "mnuShow";
			this.mnuShow.Size = new System.Drawing.Size(181, 22);
			this.mnuShow.Text = "Show Hotspot Share";
			this.mnuShow.Click += new System.EventHandler(this.mnuShow_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(178, 6);
			// 
			// mnuExit
			// 
			this.mnuExit.Name = "mnuExit";
			this.mnuExit.Size = new System.Drawing.Size(181, 22);
			this.mnuExit.Text = "E&xit";
			this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
			// 
			// tmrStationsList
			// 
			this.tmrStationsList.Interval = 20000;
			this.tmrStationsList.Tick += new System.EventHandler(this.tmrStationsList_Tick);
			// 
			// frmHotspot
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(293, 383);
			this.Controls.Add(this.lnkAbout);
			this.Controls.Add(this.tabMain);
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(500, 700);
			this.MinimumSize = new System.Drawing.Size(295, 410);
			this.Name = "frmHotspot";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Hotspot Share";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmHotspot_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmHotspot_FormClosed);
			this.Load += new System.EventHandler(this.frmHotspot_Load);
			this.VisibleChanged += new System.EventHandler(this.frmHotspot_VisibleChanged);
			this.tabMain.ResumeLayout(false);
			this.tabHotspot.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.gpbInternet.ResumeLayout(false);
			this.gpbSettings.ResumeLayout(false);
			this.gpbSettings.PerformLayout();
			this.gpbxOptions.ResumeLayout(false);
			this.tabUsers.ResumeLayout(false);
			this.gpbxUsers.ResumeLayout(false);
			this.mnuUsers.ResumeLayout(false);
			this.gpbxUsersSettings.ResumeLayout(false);
			this.mnuSys.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabMain;
		private System.Windows.Forms.TabPage tabHotspot;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label lblStatus;
		private System.Windows.Forms.Button btnStartStop;
		private System.Windows.Forms.GroupBox gpbInternet;
		private System.Windows.Forms.ComboBox cmdSharedConnection;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox chkAutoDetectInternet;
		private System.Windows.Forms.GroupBox gpbSettings;
		private System.Windows.Forms.Label passwordLabel;
		private System.Windows.Forms.Label ssidLabel;
		private System.Windows.Forms.TextBox txtSSID;
		private System.Windows.Forms.TextBox txtPass;
		private System.Windows.Forms.GroupBox gpbxOptions;
		private System.Windows.Forms.CheckBox chkAutoStartWindows;
		private System.Windows.Forms.TabPage tabUsers;
		private System.Windows.Forms.GroupBox gpbxUsers;
		private System.Windows.Forms.ListView lstUsers;
		private System.Windows.Forms.ColumnHeader colName;
		private System.Windows.Forms.ColumnHeader colVendor;
		private System.Windows.Forms.ColumnHeader colConnectedFrom;
		private System.Windows.Forms.ColumnHeader colIP;
		private System.Windows.Forms.ColumnHeader colMac;
		private System.Windows.Forms.Button btnRefresh;
		private System.Windows.Forms.GroupBox gpbxUsersSettings;
		private System.Windows.Forms.CheckBox chkUsersNotifyNewUser;
		private System.Windows.Forms.LinkLabel lnkAbout;
		private System.Windows.Forms.ImageList imgIcons20;
		private System.Windows.Forms.NotifyIcon sysIcon;
		private System.Windows.Forms.ContextMenuStrip mnuSys;
		private System.Windows.Forms.ToolStripMenuItem mnuShow;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem mnuExit;
		private System.Windows.Forms.Timer tmrStationsList;
		private System.Windows.Forms.CheckBox chkUsersNotifyUserConnecting;
		private System.Windows.Forms.ContextMenuStrip mnuUsers;
		private System.Windows.Forms.ToolStripMenuItem mnuCopyUserIP;
		private System.Windows.Forms.ToolStripMenuItem mnuCopyUserMACAddress;
		private System.Windows.Forms.ToolStripMenuItem mnuCopyUserHostname;
		private System.Windows.Forms.ToolStripMenuItem mnuCopyUserInfo;
	}
}