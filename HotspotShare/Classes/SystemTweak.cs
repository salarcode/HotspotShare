using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using Microsoft.Win32;

namespace HotspotShare.Classes
{
	public static class SystemTweak
	{
		public static void TweakTheSystem()
		{
			//RemoveIcsDomainSuffix();
			StartTheServices(true);
			AllowTheHostedNetwork();
		}
		public static void TweakTheSystemNotForced()
		{
			//RemoveIcsDomainSuffix();
			StartTheServices(false);
			AllowTheHostedNetwork();
		}
		public static void TweakTheSystemAsync()
		{
			new Action(TweakTheSystem).BeginInvoke(null, null);
		}

		private static void RemoveIcsDomainSuffix()
		{
			try
			{
				Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "ICSDomain", "");
			}
			catch (Exception)
			{
			}
		}

		private static void AllowTheHostedNetwork()
		{
			try
			{
				Process.Start(new ProcessStartInfo()
				{
					FileName = "netsh",
					Arguments = "wlan set hostednetwork mode=allow",
					UseShellExecute = false,
					CreateNoWindow = true,
					WindowStyle = ProcessWindowStyle.Hidden,
				});
			}
			catch (Exception ex)
			{
				LogExceptions.Log(ex);
			}
		}

		private static void StartTheServices(bool stopServices)
		{
			try
			{
				// Routing and Remote Access
				using (var svc = new ServiceController())
				{
					svc.ServiceName = "RemoteAccess";

					if (svc.Status != ServiceControllerStatus.Running)
					{
						try
						{
							ServiceHelper.ChangeStartMode(svc, ServiceStartMode.Automatic);
						}
						catch (Exception ex)
						{
							LogExceptions.Log(ex);
						}

						svc.Start();
					}
				}
			}
			catch (Exception ex)
			{
				LogExceptions.Log(ex);
			}

			if (stopServices)
				try
				{
					// Internet Connection Sharing (ICS)
					using (var svc = new ServiceController())
					{
						svc.ServiceName = "SharedAccess";

						if (svc.Status != ServiceControllerStatus.Running)
						{
							try
							{
								ServiceHelper.ChangeStartMode(svc, ServiceStartMode.Manual);
							}
							catch (Exception ex)
							{
								LogExceptions.Log(ex);
							}
						}
						else
						{
							svc.Stop();
						}
					}
				}
				catch (Exception ex)
				{
					LogExceptions.Log(ex);
				}
		}
	}
}
