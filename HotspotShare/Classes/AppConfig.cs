using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace HotspotShare.Classes
{
	[Serializable]
	public class AppConfig
	{
		public const string AutoStartupKey = "-startup";
		public const string HelpNoInternetAccess = "help\\no-internet-access.html";

		static AppConfig()
		{
			Instance = LoadConfig();
		}

		public AppConfig()
		{
			UiLanguage = "en";
			AutoDetectInternet = true;
			NotifyNewUser = true;
			AutoStartWithWindows = true;
		}

		private static string ConfigFile
		{
			get { return Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "settings.config"); }
		}

		public static AppConfig Instance { get; private set; }
		public static bool AppStartedFromStartup { get; set; }

		public string NetworkSsid { get; set; }
		public string Password { get; set; }
		public string ConnectionShare { get; set; }
		public Guid? InternetNetwork { get; set; }
		public bool AutoDetectInternet { get; set; }
		public bool NotifyNewUser { get; set; }
		public bool NotifyUserConnecting { get; set; }

		public bool AutoStartWithWindows { get; set; }
		public string UiLanguage { get; set; }

		public bool IsFarsi
		{
			get { return UiLanguage == "fa"; }
		}

		public MessageBoxOptions MessageBoxOptions
		{
			get
			{
				if (IsFarsi)
				{
					return MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign;
				}
				return new MessageBoxOptions();
			}
		}


		public static AppConfig LoadConfig()
		{
			if (!File.Exists(ConfigFile))
				return new AppConfig();
			try
			{
				var xml = new XmlSerializer(typeof(AppConfig));
				using (var f = File.OpenRead(ConfigFile))
				{
					return (AppConfig)xml.Deserialize(f);
				}
			}
			catch (Exception)
			{
				return new AppConfig();
			}
		}

		public void SaveConfig()
		{
			try
			{
				var xml = new XmlSerializer(typeof(AppConfig));
				using (var f = File.Create(ConfigFile))
				{
					xml.Serialize(f, this);
				}
			}
			catch (Exception)
			{
			}
		}
	}
}
