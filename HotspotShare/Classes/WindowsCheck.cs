using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows.Forms;

namespace HotspotShare.Classes
{
	public class WindowsCheck
	{
		public static bool CheckWindowsVersion(bool showError = true)
		{
			if (Environment.OSVersion.Version < new Version(6, 1, 0))
			{
				if (showError)
					MessageBox.Show(
						"The application cannot start.\nThe hotspot functionality is only available in windows 7, 8, 10 or later.\nPlease consider upgrading your system.",
						"Hotspot Share", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
			return true;
		}

		public static bool IsWiressDeviceAvailable()
		{
			try
			{
				var networks = NetworkInterface.GetAllNetworkInterfaces();
				for (int i = 0; i < networks.Length; i++)
				{
					var network = networks[i];
					if (network.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
						return true;
				}
				return false;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
