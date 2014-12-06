using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace HotspotShare.Classes
{
	public static class AdapterVendors
	{
		public static string GetVendor(string macAddress)
		{
			if (string.IsNullOrEmpty(macAddress))
				return macAddress;

			var mac = macAddress.Trim().Replace(":", "").Replace("-", "");
			if (mac.Length > 6)
				mac = mac.Substring(0, 6);
			try
			{
				var regEx = new Regex("" + mac + "\\s(?<Name>.*)", RegexOptions.IgnoreCase);
				var list = Language.nmap_mac_prefixes;
				var match = regEx.Match(list);

				if (match != null && match.Success)
				{
					return match.Groups["Name"].Value;
				}
			}
			catch (Exception)
			{
				return macAddress;
			}
			return mac;
		}

	}
}
