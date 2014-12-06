using System;
using System.Windows.Forms;
using HotspotShare.Classes;

namespace HotspotShare
{
	partial class Abort : frmBase
	{
		public Action ChangeLanguageToFa { get; set; }
		public Action ChangeLanguageToEn { get; set; }


		public Abort()
		{
			InitializeComponent();
			DialogResult = DialogResult.None;
		}

		private void btnRestart_Click(object sender, EventArgs e)
		{
			Application.Restart();
		}

		private void btnExit_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void AbortLoad(object sender, EventArgs e)
		{

		}

		private void AbortShown(object sender, EventArgs e)
		{
#if SoftLock
			txtMC.Text = SoftLockCode1.Value();
#endif
		}

		private void lnkLanguageFarsi_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			if (ChangeLanguageToFa != null)
				ChangeLanguageToFa();
		}

		private void lnkLanguageEnglish_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			if (ChangeLanguageToEn != null)
				ChangeLanguageToEn();
		}
	}
}
