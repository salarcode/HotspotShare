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
		private string _hostName;
		private bool _hostNamePrefixApplied;

		public UserStatus Status { get; set; }
		public string MacAddress { get; set; }
		public DateTime JoinDate { get; set; }
		public string IpAddress { get; set; }

		public string HostName
		{
			get { return _hostName; }
			set
			{
				_hostName = value;
				HostNameNoPrefix = value;
				_hostNamePrefixApplied = false;
			}
		}

		public string HostNameNoPrefix { get; set; }
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

		public void SetIcsDomainSuffix(string suffix)
		{
			if (_hostNamePrefixApplied)
				return;
			if (string.IsNullOrEmpty(suffix) || string.IsNullOrWhiteSpace(_hostName))
				return;
			suffix = "." + suffix;
			if (_hostName.EndsWith(suffix, StringComparison.InvariantCultureIgnoreCase))
			{
				var name = _hostName.Remove(_hostName.Length - suffix.Length, suffix.Length);
				if (name.Length > 0)
				{
					_hostNamePrefixApplied = true;
					HostNameNoPrefix = name;
				}
			}
		}
	}
}
