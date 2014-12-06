using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HotspotShare.Classes
{
	public class Common
	{
		public static string GetVersion()
		{
			return Assembly.GetExecutingAssembly().GetName().Version.ToString();
		}
		public static string GetVersionMajor()
		{
			return Assembly.GetExecutingAssembly().GetName().Version.ToString(2);
		}

		public static string JoinList(string seperator, List<string> list)
		{
			var sb = new StringBuilder();
			for (int i = 0; i < list.Count; i++)
			{
				if (i > 0)
					sb.Append(seperator);
				sb.Append(list[i]);
			}
			return sb.ToString();
		}
	}
}
