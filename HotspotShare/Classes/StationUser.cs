using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace HotspotShare.Classes
{
	public class StationUser
	{
		public enum UserStatus
		{
			Disconnect,
			Connecting,
			Connected
		}

		/// <summary>
		/// Number of tries
		/// </summary>
		private int _hostNameResolveTry = 5;

		public bool _checked = false;

		public UserStatus Status { get; set; }
		public string MacAddress { get; set; }
		public DateTime JoinDate { get; set; }
		public string IpAddress { get; set; }
		public string HostName { get; set; }
		public string Vendor { get; set; }
		public bool HostNameResolved { get; set; }


		public bool NotifiedConnecting { get; set; }
		public bool NotifiedConnected { get; set; }

		/// <summary>
		/// Is there any trial left?
		/// </summary>
		public bool CanTryResolveHost()
		{
			if (_hostNameResolveTry > 0)
			{
				_hostNameResolveTry--;
				return true;
			}
			return false;
		}

		public bool ResolveHostName()
		{
			if (string.IsNullOrWhiteSpace(IpAddress))
				return false;
			try
			{
				var hostName = Dns.GetHostEntry(IpAddress).HostName;
				if (!string.IsNullOrEmpty(hostName))
				{
					HostName = hostName;
					return true;
				}
			}
			catch { }
			return false;
		}
	}
}
