using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows.Forms;
using HotspotShare.Classes;

namespace HotspotShare
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			Application.ThreadException += Application_ThreadException;
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			ApplyLanguage();

			if (!WindowsCheck.IsWiressDeviceAvailable())
			{
				MessageBox.Show(Language.App_NoWireless, Language.App_Name,
					MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, AppConfig.Instance.MessageBoxOptions);
				return;
			}

			if (WindowsCheck.CheckWindowsVersion() && !IsTheAppAlreadyRunning())
			{
				bool tweakAsync = true;
				if (args != null && args.Length > 0)
				{
					if (args[0] == AppConfig.AutoStartupKey)
					{
						AppConfig.AppStartedFromStartup = true;

						// tweak the system rightaway
						SystemTweak.TweakTheSystem();
						tweakAsync = false;
					}
				}

				if (tweakAsync)
					SystemTweak.TweakTheSystemAsync();

				Application.Run(new frmHotspot());
			}
		}

		static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
		{
			LogExceptions.Log(e.Exception);
		}

		private static Mutex _mutex;
		private static string MutexName = "Hotspot-Share";
		static bool IsTheAppAlreadyRunning()
		{
			// code to ensure that only one copy of the software is running.
			try
			{
				_mutex = Mutex.OpenExisting(MutexName);

				//since it hasn’t thrown an exception, then we already have one copy of the app open.
				MessageBox.Show(Language.App_OneInstance, Language.App_Name,
					MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, AppConfig.Instance.MessageBoxOptions);

				Environment.Exit(0);
				return true;
			}
			catch
			{
				//since we didn’t find a mutex with that name, create one
				_mutex = new Mutex(true, MutexName);
			}
			return false;
		}
		private static void ApplyLanguage()
		{
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(AppConfig.Instance.UiLanguage);
			Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture;
		}

	}
}
