using System;
using System.Diagnostics;
using System.Windows.Forms;
using HotspotShare.Classes;

namespace HotspotShare
{
	partial class frmAbout : frmBase
	{
		public frmAbout()
		{
			InitializeComponent();
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void frmAbout_Load(object sender, EventArgs e)
		{
			lblVersion.Text = Common.GetVersion();
			this.Icon = Application.OpenForms[0].Icon;
		}

		private void lnkUpdate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			var start = new ProcessStartInfo(lnkUpdate.Text);
			try
			{
				start.UseShellExecute = true;
				Process.Start(start);
			}
			catch { }
		}

		private void lnkWebSite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			var start = new ProcessStartInfo(lnkWebSite.Text);
			try
			{
				start.UseShellExecute = true;
				Process.Start(start);
			}
			catch { }
		}
	}
}
