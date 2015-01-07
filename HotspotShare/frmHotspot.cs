using Blue.Windows;
using HotspotShare.Api;
using HotspotShare.Classes;
using HotspotShare.HostedNetwork;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace HotspotShare
{
	public partial class frmHotspot : frmBase
	{
		#region [Private Fields]

		private bool _hasStationUsers = false;
		private bool _hasAnyUserShouldReadStatus = false;

		private bool _minimizedWarning = false;
		private bool _isVisibleCore = false;
		private bool _manualStartIsInPrgress = false;
		private StickyWindow _sticky;

		private HostedNetworkManager hostedNetworkManager;
		#endregion

		public frmHotspot()
		{
			ApplyLanguage();

			InitializeComponent();
			_sticky = new StickyWindow(this)
			{
				StickToScreen = true
			};
			RememberWindow.MoveToEdges_BottomRight(this);

			_isVisibleCore = true;
			if (AppConfig.AppStartedFromStartup)
				_isVisibleCore = false;

			InitializeForm();


#if TRACE
			LogExceptions.ClearTrace();
#endif

		}
		protected override void SetVisibleCore(bool value)
		{
			// Preventing the form to display in windows auto-start by ignoring the *value*
			base.SetVisibleCore(_isVisibleCore);
		}

		#region [General Private Methods]

		void InitializeForm()
		{
			lstUsers.Items.Clear();

			//SetAppStatus();

			hostedNetworkManager = new HostedNetworkManager();
			hostedNetworkManager.ThreadedEvents = new UiThreadedEvents(this);
			hostedNetworkManager.OnConnectionsListChanged += HostedNetworkManagerOnConnectionsListChanged;
			hostedNetworkManager.OnSharedConnectionChanged += HostedNetworkManagerOnSharedConnectionChanged;
			hostedNetworkManager.OnWorkingStatusChanged += HostedNetworkManagerOnWorkingStatusChanged;
			hostedNetworkManager.OnUserUpdated += HostedNetworkManagerOnUserUpdated;
			hostedNetworkManager.OnUserConnected += HostedNetworkManagerOnUserConnected;
			hostedNetworkManager.OnUserLeave += HostedNetworkManagerOnUserLeave;
			hostedNetworkManager.OnFailedToEnableSharing += HostedNetworkManagerOnFailedToEnableSharing;

			// reading ICS connections list
			hostedNetworkManager.ReadNetworkConnectionsAsync();

			ReadFormSettings();

			if (AppConfig.AppStartedFromStartup)
			{
				if (ValidateForm(false))
				{
					SaveFormSettings();
					hostedNetworkManager.StartAsync();
				}
			}
		}

		void ApplyLanguage()
		{
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(AppConfig.Instance.UiLanguage);
			Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture;
		}

		void SaveFormSettings()
		{
			AppConfig.Instance.AutoDetectInternet = chkAutoDetectInternet.Checked;
			AppConfig.Instance.NotifyNewUser = chkUsersNotifyNewUser.Checked;
			AppConfig.Instance.NotifyUserConnecting = chkUsersNotifyUserConnecting.Checked;
			AppConfig.Instance.Password = txtPass.Text;
			AppConfig.Instance.NetworkSsid = txtSSID.Text;
			AppConfig.Instance.ConnectionShare = cmdSharedConnection.Text;
			AppConfig.Instance.AutoStartWithWindows = chkAutoStartWindows.Checked;

			var item = cmdSharedConnection.SelectedItem as IcsConnection;
			if (item == null)
			{
				AppConfig.Instance.InternetNetwork = null;
			}
			else
			{
				AppConfig.Instance.InternetNetwork = item.Guid;
			}


			AppConfig.Instance.SaveConfig();

			hostedNetworkManager.ConfigAutoInternet = AppConfig.Instance.AutoDetectInternet;
			hostedNetworkManager.ConfigInternetNetwork = AppConfig.Instance.InternetNetwork;
			hostedNetworkManager.ConfigPassword = AppConfig.Instance.Password;
			hostedNetworkManager.ConfigShareInternet = true;
			hostedNetworkManager.ConfigSsid = AppConfig.Instance.NetworkSsid;
		}

		void ReadFormSettings()
		{
			chkAutoDetectInternet.Checked = AppConfig.Instance.AutoDetectInternet;
			chkUsersNotifyNewUser.Checked = AppConfig.Instance.NotifyNewUser;
			chkUsersNotifyUserConnecting.Checked = AppConfig.Instance.NotifyUserConnecting;
			txtPass.Text = AppConfig.Instance.Password;
			txtSSID.Text = AppConfig.Instance.NetworkSsid;
			if (!string.IsNullOrEmpty(AppConfig.Instance.ConnectionShare))
				cmdSharedConnection.Text = AppConfig.Instance.ConnectionShare;
			chkAutoStartWindows.Checked = AppConfig.Instance.AutoStartWithWindows;


			hostedNetworkManager.ConfigAutoInternet = AppConfig.Instance.AutoDetectInternet;
			hostedNetworkManager.ConfigInternetNetwork = AppConfig.Instance.InternetNetwork;
			hostedNetworkManager.ConfigPassword = AppConfig.Instance.Password;
			hostedNetworkManager.ConfigShareInternet = true;
			hostedNetworkManager.ConfigSsid = AppConfig.Instance.NetworkSsid;
		}

		bool ValidateForm(bool showError = true)
		{
			string msg = "";

			txtSSID.Text = txtSSID.Text.Trim();
			txtPass.Text = txtPass.Text.Trim();

			if (txtSSID.Text.Length <= 0)
			{
				msg += Language.Form_SSIDRequired + "\n";
			}

			if (txtSSID.Text.Length > 32)
			{
				msg += Language.Form_SSIDLong + "\n";
			}

			if (txtPass.Text.Length == 0)
			{
				msg += Language.Form_PassRequired + "\n";
			}
			else if (txtPass.Text.Length < 8)
			{
				msg += Language.Form_PassShort + "\n";
			}

			if (txtPass.Text.Length > 64)
			{
				msg += Language.Form_PassLong + "\n";
			}

			//if (cmbToShareConnection.SelectedIndex < 0)
			//{
			//	msg += Language.Form_ConnectionSelected + "\n";
			//}

			if (msg.Length > 0)
			{
				if (showError)
					MessageBox.Show(msg, Language.Form_InvalidInput, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, AppConfig.Instance.MessageBoxOptions);
				return false;
			}
			return true;
		}
		/// <summary>
		/// Updating cmdSharedConnection
		/// </summary>
		void UiUpdateConnectionComboBox(HostedNetworkManager hostednetwork)
		{
			var connections = hostednetwork.IcsConnectedConnections;
			var shared = hostednetwork.SharedConnection;

			cmdSharedConnection.DisplayMember = "Name";
			cmdSharedConnection.ValueMember = "Guid";
			cmdSharedConnection.DataSource = null;
			cmdSharedConnection.Items.Clear();
			cmdSharedConnection.DataSource = connections;

			if (shared == null)
			{
				var savedConnName = AppConfig.Instance.ConnectionShare;
				if (string.IsNullOrWhiteSpace(savedConnName))
				{
					cmdSharedConnection.SelectedIndex = 0;
				}
				else
				{
					cmdSharedConnection.Text = savedConnName;
				}
			}
			else
			{
				cmdSharedConnection.SelectedItem = shared;

				var newName = cmdSharedConnection.Text;
				if (!string.IsNullOrWhiteSpace(newName))
				{
					AppConfig.Instance.ConnectionShare = newName;
				}
			}
		}

		void UiUpdateFormWorkingStatus(HostedNetworkManager hostednetwork)
		{
			bool isBusy = false;
			bool isStarted = false;
			switch (hostednetwork.Status)
			{
				case HostedNetworkManager.WorkingStatus.Started:
					tabMain.SelectTab(tabUsers);
					lblStatus.Text = Language.Form_StartedStated;
					lblStatus.ForeColor = Color.Green;
					btnStartStop.Text = Language.Form_BtnStoped;
					sysIcon.Text = "Hotspot Share (Started)";
					isBusy = false;
					isStarted = true;
					_manualStartIsInPrgress = false;

					sysIcon.Icon = Properties.Resources.AppiconTray;
					break;

				case HostedNetworkManager.WorkingStatus.Stopped:
					tabMain.SelectTab(tabHotspot);
					lblStatus.Text = Language.Form_StopedStated;
					lblStatus.ForeColor = Color.Red;
					btnStartStop.Text = Language.Form_BtnStarted;
					sysIcon.Text = "Hotspot Share (Stopped)";
					isBusy = false;
					isStarted = false;
					//_manualStartIsInPrgress = false;

					// reload the connections list
					hostedNetworkManager.ReadNetworkConnectionsAsync();

					sysIcon.Icon = Properties.Resources.AppiconTrayDisabled;
					break;


				case HostedNetworkManager.WorkingStatus.Starting:
					lblStatus.Text = "Starting...";
					lblStatus.ForeColor = Color.DarkRed;
					btnStartStop.Text = "Starting...";
					sysIcon.Text = "Hotspot Share (Starting)";
					isBusy = true;
					isStarted = false;
					break;

				case HostedNetworkManager.WorkingStatus.Stopping:
					lblStatus.Text = "Stopping...";
					lblStatus.ForeColor = Color.DarkRed;
					btnStartStop.Text = "Stopping...";
					sysIcon.Text = "Hotspot Share (Stopping)";
					isBusy = true;
					isStarted = false;
					//_manualStartIsInPrgress = false;
					break;

				case HostedNetworkManager.WorkingStatus.StartFailed:
					tabMain.SelectTab(tabHotspot);
					lblStatus.Text = "Startup Failed";
					lblStatus.ForeColor = Color.Red;
					btnStartStop.Text = "Try Start";
					sysIcon.Text = "Hotspot Share (Start Failed)";
					isBusy = false;
					isStarted = false;
					_manualStartIsInPrgress = false;

					// reload the connections list
					hostedNetworkManager.ReadNetworkConnectionsAsync();
					break;

				case HostedNetworkManager.WorkingStatus.StopFailed:
					tabMain.SelectTab(tabHotspot);
					lblStatus.Text = "Stopping Failed";
					lblStatus.ForeColor = Color.Red;
					btnStartStop.Text = "Try Stop";
					sysIcon.Text = "Hotspot Share (Stop Failed)";
					isBusy = false;
					isStarted = true;
					_manualStartIsInPrgress = false;
					break;
			}

			var enableControls = !(isBusy || isStarted);
			gpbSettings.Enabled = enableControls;
			gpbInternet.Enabled = enableControls;

			btnStartStop.Enabled = !isBusy;
		}


		void UiUpdateStationsList(HostedNetworkManager hostednetwork, IList<StationUser> newUsers = null)
		{
			bool isFarsi = AppConfig.Instance.IsFarsi;

			var desc = new List<string>();
			lstUsers.Items.Clear();

			var hasUsers = hostednetwork.StationUsers.Count > 0;
			_hasAnyUserShouldReadStatus = false;

			var usersToNotify = new List<StationUser>();

			foreach (var user in hostednetwork.StationUsers)
			{
				if (user.Status == StationUser.UserStatus.Connecting ||
					user.HostNameResolved == false)
					// this user information is incomplete
					_hasAnyUserShouldReadStatus = true;

				if (user.HostNameResolved && !user.NotifiedConnected)
				{
					usersToNotify.Add(user);
					user.NotifiedConnected = true;
				}

				var listViewItem = new ListViewItem(user.HostName, 0);
				listViewItem.Text = user.HostName;

				if (!string.IsNullOrEmpty(user.Vendor))
					desc.Add(user.Vendor);

				if (!string.IsNullOrEmpty(user.IpAddress))
					desc.Add(user.IpAddress);

				if (!string.IsNullOrEmpty(user.MacAddress))
					desc.Add(user.MacAddress);

				desc.Add(user.JoinDate.ToString("yyyy/MM/dd HH:mm"));

				if (isFarsi)
					desc.Reverse();

				var descText = Common.JoinList(", ", desc);
				if (isFarsi)
					descText = "\u200E" + descText;

				listViewItem.SubItems.Add(descText);
				listViewItem.ToolTipText = user.HostName + "\r\n" + descText;
				listViewItem.Tag = user;

				desc.Clear();

				lstUsers.Items.Add(listViewItem);
			}
			if (_hasStationUsers != hasUsers)
			{
				if (hasUsers)
				{
					sysIcon.Icon = Properties.Resources.AppiconTrayUser;
					Icon = Properties.Resources.AppiconTrayUser;
				}
				else
				{
					sysIcon.Icon = Properties.Resources.AppiconTray;
					Icon = Properties.Resources.AppiconTray;
				}
				_hasStationUsers = hasUsers;
			}
			if (usersToNotify.Count > 0 && chkUsersNotifyNewUser.Checked)
			{
				var message=string.Join("\n", usersToNotify.Select(a => a.HostName + ", " + a.IpAddress.ToString()));

				message = "The following user(s) are connected:\n" + message;
				sysIcon.ShowBalloonTip(1000, Language.App_Name, message, ToolTipIcon.Info);
			}
			else if (newUsers != null && chkUsersNotifyUserConnecting.Checked)
			{
				sysIcon.ShowBalloonTip(1000, Language.App_Name, Language.Balloon_NewUser, ToolTipIcon.Info);
			}

			SetRefreshStationsTimer();
		}

		void SetRefreshStationsTimer()
		{
			if (_hasAnyUserShouldReadStatus)
			{
				tmrStationsList.Enabled = true;
			}
			else
			{
				tmrStationsList.Enabled = false;
			}
		}

		void UiShow()
		{
			_isVisibleCore = true;
			this.Show();
			this.Focus();
			this.Activate();
		}


		private void UiShowNoInternetAccess(HostedNetworkManager hostednetwork, Exception exception)
		{
			// only if user is requested start manually
			//if (!_manualStartIsInPrgress)
			//	return;
			var result = MessageBox.Show(
				"Hotspot is failed to enable internet sharing automatically after many tries.\n" +
				"You have to enable the internet sharing manually.\n" +
				"Do you want to see how?",
				"Failed to enable windows sharing",
				MessageBoxButtons.OKCancel,
				MessageBoxIcon.Error,
				MessageBoxDefaultButton.Button1,
				AppConfig.Instance.MessageBoxOptions);
			if (result == DialogResult.OK)
			{
				try
				{
					var filePath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), AppConfig.HelpNoInternetAccess);
					Process.Start(new ProcessStartInfo
					{
						FileName = filePath,
						UseShellExecute = true
					});
				}
				catch (Exception)
				{
				}
			}
		}

		#endregion

		#region [Hosted Network Events]
		private void HostedNetworkManagerOnUserLeave(HostedNetworkManager hostednetwork)
		{
			UiUpdateStationsList(hostednetwork);
		}

		private void HostedNetworkManagerOnUserConnected(HostedNetworkManager hostednetwork, IList<StationUser> newUsers)
		{
			UiUpdateStationsList(hostednetwork, newUsers);
		}

		private void HostedNetworkManagerOnUserUpdated(HostedNetworkManager hostednetwork)
		{
			UiUpdateStationsList(hostednetwork);
		}

		private void HostedNetworkManagerOnWorkingStatusChanged(HostedNetworkManager hostednetwork)
		{
			UiUpdateFormWorkingStatus(hostednetwork);
		}

		private void HostedNetworkManagerOnSharedConnectionChanged(HostedNetworkManager hostednetwork)
		{
			UiUpdateConnectionComboBox(hostednetwork);
		}

		private void HostedNetworkManagerOnConnectionsListChanged(HostedNetworkManager hostednetwork)
		{
			UiUpdateConnectionComboBox(hostednetwork);
		}

		private void HostedNetworkManagerOnFailedToEnableSharing(HostedNetworkManager hostednetwork, Exception exception)
		{
			UiShowNoInternetAccess(hostednetwork, exception);
		}
		#endregion

		#region [Form Events]
		private void cmdSharedConnection_SelectedIndexChanged(object sender, EventArgs e)
		{
		}

		private void frmHotspot_Load(object sender, EventArgs e)
		{


		}

		private void frmHotspot_FormClosing(object sender, FormClosingEventArgs e)
		{
			SaveFormSettings();
			if (e.CloseReason == CloseReason.UserClosing)
			{
				_isVisibleCore = false;
				this.Hide();

				sysIcon.Visible = true;
				if (!_minimizedWarning)
				{
					_minimizedWarning = true;
					sysIcon.ShowBalloonTip(1000, Language.App_Name, Language.Balloon_Minimized, ToolTipIcon.Info);
				}
				e.Cancel = true;
			}
			else
			{
				hostedNetworkManager.StopSynced();
			}
		}

		private void frmHotspot_FormClosed(object sender, FormClosedEventArgs e)
		{
			SetRefreshStationsTimer();
		}

		private void frmHotspot_VisibleChanged(object sender, EventArgs e)
		{
			if (Visible)
			{
				hostedNetworkManager.ReadStationUsersAsync();
				SetRefreshStationsTimer();
			}
		}

		private void btnStartStop_Click(object sender, EventArgs e)
		{
			SaveFormSettings();

			switch (hostedNetworkManager.Status)
			{
				case HostedNetworkManager.WorkingStatus.Stopping:
				case HostedNetworkManager.WorkingStatus.Stopped:
				case HostedNetworkManager.WorkingStatus.StartFailed:
					_manualStartIsInPrgress = true;
					hostedNetworkManager.ResetFailedToEnableSharingNetwork();
					hostedNetworkManager.StartAsync();
					break;

				case HostedNetworkManager.WorkingStatus.Started:
				case HostedNetworkManager.WorkingStatus.Starting:
				case HostedNetworkManager.WorkingStatus.StopFailed:
					_manualStartIsInPrgress = false;
					hostedNetworkManager.StopAsync();
					break;
				default:
					// Do nothing
					break;
			}
		}

		private void chkAutoDetectInternet_CheckedChanged(object sender, EventArgs e)
		{
			cmdSharedConnection.Enabled = !chkAutoDetectInternet.Checked;
			if (cmdSharedConnection.Enabled)
			{
				hostedNetworkManager.ReadNetworkConnectionsAsync();
			}
		}
		private void txtPass_Enter(object sender, EventArgs e)
		{
			txtPass.PasswordChar = '\0';
		}

		private void txtPass_Leave(object sender, EventArgs e)
		{
			txtPass.PasswordChar = '●';
		}

		private void mnuShow_Click(object sender, EventArgs e)
		{
			UiShow();
		}

		private void mnuExit_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void StartupAppInRegistery(bool enable)
		{
			if (enable)
			{
				Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Run", Application.ProductName,
					string.Format("\"{0}\" " + AppConfig.AutoStartupKey, Application.ExecutablePath));

			}
			else
			{
				Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Run", Application.ProductName, "");
			}
		}

		private void chkAutoStartWindows_CheckedChanged(object sender, EventArgs e)
		{
			var auto = chkAutoStartWindows.Checked;
			try
			{
				var proc = new Process();
				proc.EnableRaisingEvents = true;
				proc.Exited += (s, args) =>
				{
					if (proc.ExitCode == 0)
					{
						StartupAppInRegistery(false);
					}
					else
					{
						StartupAppInRegistery(true);
					}
					proc.Dispose();
				};


				if (auto)
				{
					proc.StartInfo = new ProcessStartInfo
					{
						FileName = "schtasks",
						Arguments =
							string.Format("/create /f /sc onlogon /tn HotspotShare /rl highest /DELAY 0001:00 /tr \"{0} -startup\"",
								Application.ExecutablePath),
						UseShellExecute = true,
						CreateNoWindow = true,
						WindowStyle = ProcessWindowStyle.Hidden,
					};
				}
				else
				{
					proc.StartInfo = (new ProcessStartInfo
					{
						FileName = "schtasks",
						Arguments = "/delete /f /tn HotspotShare",
						UseShellExecute = true,
						CreateNoWindow = true,
						WindowStyle = ProcessWindowStyle.Hidden,
					});
				}
				proc.Start();

			}
			catch { }
		}

		private void lnkAbout_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			using (var frm = new frmAbout())
			{
				frm.ShowDialog();
			}
		}


		private void btnRefresh_Click(object sender, EventArgs e)
		{
			hostedNetworkManager.ReadStationUsersAsync();
		}

		private void sysIcon_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				UiShow();
			}
		}

		private void tmrStationsList_Tick(object sender, EventArgs e)
		{
			hostedNetworkManager.ReadStationUsersAsync();
			SetRefreshStationsTimer();
		}

		private void mnuCopyUserIP_Click(object sender, EventArgs e)
		{
			if (lstUsers.SelectedItems == null || lstUsers.SelectedItems.Count == 0)
				return;
			var user = lstUsers.SelectedItems[0].Tag as StationUser;
			if (user == null)
				return;
			Clipboard.SetText(user.IpAddress??"");
		}

		private void mnuCopyUserMACAddress_Click(object sender, EventArgs e)
		{
			if (lstUsers.SelectedItems == null || lstUsers.SelectedItems.Count == 0)
				return;
			var user = lstUsers.SelectedItems[0].Tag as StationUser;
			if (user == null)
				return;
			Clipboard.SetText(user.MacAddress??"");
		}

		private void mnuCopyUserHostname_Click(object sender, EventArgs e)
		{
			if (lstUsers.SelectedItems == null || lstUsers.SelectedItems.Count == 0)
				return;
			var user = lstUsers.SelectedItems[0].Tag as StationUser;
			if (user == null)
				return;
			Clipboard.SetText(user.HostName ?? "");
		}

		private void mnuCopyUserInfo_Click(object sender, EventArgs e)
		{
			if (lstUsers.SelectedItems == null || lstUsers.SelectedItems.Count == 0)
				return;
			if (lstUsers.SelectedItems == null || lstUsers.SelectedItems.Count == 0)
				return;
			var user = lstUsers.SelectedItems[0];
			if (user == null)
				return;
			Clipboard.SetText(user.ToolTipText ?? "");
		}
		#endregion

		private void mnuUsers_Opening(object sender, CancelEventArgs e)
		{
			if (lstUsers.SelectedItems == null || lstUsers.SelectedItems.Count == 0)
			{
				e.Cancel = true;
			}

		}

	}
}
