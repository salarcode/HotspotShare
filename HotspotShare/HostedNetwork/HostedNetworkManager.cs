using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using HotspotShare.Api;
using HotspotShare.Api.WinAPI;
using HotspotShare.Classes;
using Microsoft.Win32;

namespace HotspotShare.HostedNetwork
{
	public class HostedNetworkManager
	{
		private readonly WlanManager _wlanManager = new WlanManager();
		private readonly IcsManager _icsManager = new IcsManager();
		private WindowsNetwork _windowsNetwork = new WindowsNetwork();
		private readonly List<IcsConnection> _icsConnectedConnections = new List<IcsConnection>();
		private IcsConnection _sharedConnection;
		private Guid _hostedNetworkInterfaceGuid;
		private IList<StationUser> _stationUsers;
		private IThreadedEvents _threadedEvents;

		public enum WorkingStatus
		{
			Stopped,
			Started,
			Starting,
			Stopping,
			StartFailed,
			StopFailed,
		}

		public delegate void GenericEvent(HostedNetworkManager hostedNetworkManager);
		public delegate void UserConnectedEvent(HostedNetworkManager hostedNetworkManager, IList<StationUser> newUsers);
		public delegate void ErrorEvent(HostedNetworkManager hostedNetworkManager, Exception exception);

		public HostedNetworkManager()
		{
			_threadedEvents = new DefaultThreadedEvents();
			NetworkChange.NetworkAvailabilityChanged += NetworkChange_NetworkAvailabilityChanged;
			NetworkChange.NetworkAddressChanged += NetworkChange_NetworkAddressChanged;
			_wlanManager.StationJoin += _wlanManager_StationJoin;
			_wlanManager.StationLeave += _wlanManager_StationLeave;
			_wlanManager.StationStateChange += _wlanManager_StationStateChange;
			_wlanManager.HostedNetworkStopped += _wlanManager_HostedNetworkStopped;
			_wlanManager.HostedNetworkStarted += _wlanManager_HostedNetworkStarted;

			// when resumeing from Sleep or hibernate
			SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;
			Status = WorkingStatus.Stopped;
			ConfigAutoInternet = true;
			ConfigShareInternet = true;
			SharedConnection = null;
			ConfigMaxPeers = 32;
			StationUsers = new List<StationUser>();
		}

		#region Public Properties

		public WorkingStatus Status
		{
			get { return _status; }
			private set
			{
				if (_status != value)
				{
					_status = value;
					RaiseOnWorkingStatusChanged();
				}
			}
		}

		public int ConfigMaxPeers { get; set; }
		public string ConfigSsid { get; set; }
		public string ConfigPassword { get; set; }
		public bool ConfigAutoInternet { get; set; }
		public bool ConfigShareInternet { get; set; }
		public Guid? ConfigInternetNetwork { get; set; }

		public Action<string> LogEvent { get; set; }

		public IcsConnection SharedConnection
		{
			get { return _sharedConnection; }
			private set
			{
				// are they same reference
				if (_sharedConnection != value)
				{
					RaiseOnSharedConnectionChanged();
					_sharedConnection = value;
				}
				else
					// are they different guid?
					if (_sharedConnection != null && _sharedConnection.Guid != value.Guid)
					{
						RaiseOnSharedConnectionChanged();
						_sharedConnection = value;
					}
			}
		}

		/// <summary>
		/// Used to raise event in the 
		/// </summary>
		public IThreadedEvents ThreadedEvents
		{
			get { return _threadedEvents; }
			set { _threadedEvents = value; }
		}

		public event GenericEvent OnConnectionsListChanged;

		public event GenericEvent OnSharedConnectionChanged;

		public event GenericEvent OnWorkingStatusChanged;

		public event UserConnectedEvent OnUserConnected;

		public event GenericEvent OnUserLeave;

		public event GenericEvent OnUserUpdated;

		public event ErrorEvent OnFailedToEnableSharing;


		/// <summary>
		/// List of connected connections
		/// </summary>
		public IList<IcsConnection> IcsConnectedConnections
		{
			get { return _icsConnectedConnections; }
		}

		public IList<StationUser> StationUsers
		{
			get { return _stationUsers; }
			private set { _stationUsers = value; }
		}

		#endregion

		#region Public Methods
		//public void 

		public void StartAsync()
		{
			DoWorkThreaded(StartInternal);
		}

		public void StopAsync()
		{
			DoWorkThreaded(() => StopInternal(true));
		}
		public void StopSynced()
		{
			StopInternal(true);
		}

		public void ReadNetworkConnectionsAsync()
		{
			UpdateIcsConnections();
		}

		public void ReadStationUsersAsync()
		{
			StationsReadRequest();
		}
		/// <summary>
		/// Resets the failed network status, causing to display the warning again.
		/// </summary>
		public void ResetFailedToEnableSharingNetwork()
		{
			_lastFailedToEnableSharingFor = null;
		}
		#endregion

		#region Private Methods

		private void StartInternal()
		{
			try
			{
				StopInternal(false);

				Status = WorkingStatus.Starting;

				// system fixes
				SystemTweak.TweakTheSystemNotForced();

				// setting the options
				_wlanManager.SetConnectionSettings(ConfigSsid, ConfigMaxPeers);
				_wlanManager.SetSecondaryKey(ConfigPassword);

				_wlanManager.StartHostedNetwork();

				// save the network id
				_hostedNetworkInterfaceGuid = _wlanManager.HostedNetworkInterfaceGuid;

				var connection = GetIcsToShare();

				if (connection != null)
				{
					ShareConnection(connection);
				}
				else
				{
					DisableSharing(true);
				}

				Status = WorkingStatus.Started;
#if TRACE
				LogExceptions.LogTrace(true,
					"WLanStart completed, internet is: " + ((connection == null) ? "NULL" : connection.Name));
#endif

			}
			catch (Exception ex)
			{
				LogExceptions.Log(ex);
				Status = WorkingStatus.StartFailed;
			}
		}

		private bool StopInternal(bool verbose)
		{
			try
			{
				Status = WorkingStatus.Stopping;

				DisableSharing(true);

				_wlanManager.StopHostedNetwork();

#if TRACE
				LogExceptions.LogTrace(true, "WLanStop->Done");
#endif
				if (verbose)
					Status = WorkingStatus.Stopped;

				return true;
			}
			catch (Exception ex)
			{
				LogExceptions.Log(ex);

				Status = WorkingStatus.StopFailed;
				return false;
			}
		}

		private void ShareConnection(IcsConnection icsConnection)
		{
			// disable sharing first
			DisableSharing(false);

			if (icsConnection == null)
				return;

#if TRACE
			LogExceptions.LogTrace(true, "ShareConnection -> " + icsConnection.Name);
#endif
			Exception failedToSharException = null;

			// then enabled the sharing
			for (int i = 1; i < 10; i++)
			{
				try
				{
					if (_wlanManager.HostedNetworkState != WLAN_HOSTED_NETWORK_STATE.wlan_hosted_network_active)
					{
						Thread.Sleep(500);
						continue;
					}

					var privateConnectionGuid = _wlanManager.HostedNetworkInterfaceGuid;

					_icsManager.EnableIcs(icsConnection.Guid, privateConnectionGuid);

					// the connected network guid to internet
					SharedConnection = icsConnection;

					failedToSharException = null;
#if TRACE
					LogExceptions.LogTrace(true, "ShareConnection.EnableIcs on try no:" + i);
#endif
					return;
				}
				catch (Exception ex)
				{
#if DEBUG
					LogExceptions.Log(ex);
#endif
					failedToSharException = ex;
					Thread.Sleep(500);
				}
			}

			// if we have reached here that means the connection is failed to share with many tries
			if (failedToSharException != null)
			{
#if TRACE
				LogExceptions.LogTrace(true, "ShareConnection is failed -> " + icsConnection.Name,
					failedToSharException.ToString());
#endif
				RaiseOnFailedToEnableSharing(icsConnection, failedToSharException);
			}
			else
			{
#if TRACE
				LogExceptions.LogTrace(true, "ShareConnection is failed -> " + icsConnection.Name);
#endif
			}
		}

		private void DisableSharing(bool force = false)
		{
			if (!force)
			{
				if (_sharedConnection == null)
				{
#if TRACE
					LogExceptions.LogTrace(true, "WLanDisableSharing,NotShared,forced?=" + force);
#endif
					return;
				}
#if TRACE
				LogExceptions.LogTrace(true, "WLanDisableSharing,Shared,forced?=" + force);
#endif
			}
			else
			{
#if TRACE
				LogExceptions.LogTrace(true, "WLanDisableSharing,NoState,forced?=" + force);
#endif
			}
			if (_icsManager.SharingInstalled)
				_icsManager.DisableIcsOnAll();
			SharedConnection = null;
		}

		private void AutoUpdatedSharedConnection()
		{
			if (_status != WorkingStatus.Started)
				// only when the app is started this should update the shared connection!
				return;

			if (!ConfigShareInternet)
				return;
			if (!ConfigAutoInternet)
				return;

			var internetConnected = GetIcsToShare();
			if (internetConnected == null)
				return;

			// connection is changed
			if (_sharedConnection == null || internetConnected.Guid != _sharedConnection.Guid)
			{
#if TRACE
				LogExceptions.LogTrace(true, "AutoUpdatedSharedConnection -> " + internetConnected.Name);
#endif

				ShareConnection(internetConnected);
			}
		}

		private IcsConnection GetIcsToShare()
		{
			UpdateIcsConnections();
			if (!ConfigShareInternet)
				return null;

			if (ConfigAutoInternet)
			{
				var guid = _windowsNetwork.GetNetworkGuidConnectedToInternet(_hostedNetworkInterfaceGuid);
				if (guid == Guid.Empty)
					return null;

				return GetConnection(guid);
			}
			else
			{
				if (ConfigInternetNetwork == null)
				{
					var guid = _windowsNetwork.GetNetworkGuidConnectedToInternet(_hostedNetworkInterfaceGuid);
					if (guid == Guid.Empty)
						return null;

					return GetConnection(guid);
				}
				else
				{
					return GetConnection(ConfigInternetNetwork.Value);
				}
			}
		}

		private IcsConnection GetConnection(Guid icsGuid)
		{
			for (int i = 0; i < _icsConnectedConnections.Count; i++)
			{
				var ics = _icsConnectedConnections[i];
				if (ics.Guid == icsGuid)
					return ics;
			}
			return null;
		}

		private void UpdateIcsConnections()
		{
			if (_icsManager == null)
				return;

			var icsConn = _icsManager.Connections;
			lock (_icsConnectedConnections)
			{
				_icsConnectedConnections.Clear();

				for (int i = icsConn.Count - 1; i >= 0; i--)
				{
					var ics = icsConn[i];

					// do not add the self connection
					if (ics.Guid == _hostedNetworkInterfaceGuid)
						continue;

					if (ics.IsSupported && ics.IsConnected)
					{
						_icsConnectedConnections.Add(ics);
					}
				}
			}

			RaiseOnConnectionsListChanged();
		}

		private static void DoWorkThreaded(ThreadStart threadedAction)
		{
			var th = new Thread(threadedAction)
			{
				IsBackground = true,
				Name = "WHostedNetwork_" + DateTime.Now.Ticks
			};
			th.Start();
		}
		private static void DoWorkThreadPooled(WaitCallback threadedAction)
		{
			ThreadPool.QueueUserWorkItem(threadedAction);
		}

		private volatile bool _readStationsRequest = false;
		private volatile bool _readStationsIsBusy = false;
		private WorkingStatus _status;

		private void StationsReadRequest()
		{
			// set the flag
			_readStationsRequest = true;

			// check the status
			if (_readStationsIsBusy)
				return;

			// call the read stations method
			DoWorkThreadPooled(InternalQueueStationsRead);
		}

		private void InternalQueueStationsRead(object state)
		{
			if (!_readStationsRequest)
				return;
			if (_readStationsIsBusy)
				return;

			_readStationsIsBusy = true;
			try
			{
				do
				{
					// turn off the flag
					_readStationsRequest = false;

					var readAction = new Action(InternalStationsRead);

					// invoking the read in seperate thread
					var readAsync = readAction.BeginInvoke(null, null);

					// waiting to finished
					readAsync.AsyncWaitHandle.WaitOne();

					// continue if the flag is still on
				}
				while (_readStationsRequest);
			}
			finally
			{
				_readStationsIsBusy = false;
			}
		}

		private void InternalStationsRead()
		{
			// do shit here!
			// 
			List<IPInfo> ipInfoDataList = null;
			var getInfoList = new Func<List<IPInfo>>(() =>
			{
				if (ipInfoDataList == null)
				{
					ipInfoDataList = IPInfo.GetIPInfo();
				}
				return ipInfoDataList;
			});

			// they are not checked yet
			for (int i = 0; i < _stationUsers.Count; i++)
			{
				_stationUsers[0]._checked = false;
			}

			var newUsers = new List<StationUser>();
			bool userAdded = false,
				userRemoved = false,
				userUpdated = false;

			foreach (var station in _wlanManager.Stations)
			{
				var stationMac = station.Value.MacAddress;

				var stationUser = _stationUsers.FirstOrDefault(a => a.MacAddress == stationMac);
				if (stationUser == null)
				{
					stationUser = new StationUser
					{
						Status = StationUser.UserStatus.Connecting,
						HostName = "",
						IpAddress = "",
						JoinDate = DateTime.Now,
						MacAddress = stationMac,
						Vendor = AdapterVendors.GetVendor(stationMac)
					};
					_stationUsers.Add(stationUser);
					newUsers.Add(stationUser);
					userAdded = true;
				}
				stationUser._checked = true;

				// user is not connected yet!
				if (string.IsNullOrEmpty(stationUser.IpAddress) ||
					string.IsNullOrEmpty(stationUser.HostName) ||
					stationUser.HostName.Length == 0)
				{
					var stationMacCompare = station.Value.MacAddress.ToLowerInvariant().Replace(':', '-');
					var ipInfo = getInfoList().FirstOrDefault(a => a.MacAddress.ToLowerInvariant() == stationMacCompare);

					if (ipInfo != null)
					{
						stationUser.IpAddress = ipInfo.IPAddress;
						stationUser.HostName = ipInfo.HostName;
						if (stationUser.HostName == null)
						{
							stationUser.HostName = stationMac;
							stationUser.HostNameResolved = false;
						}
						else
						{
							stationUser.HostNameResolved = true;
						}
						userUpdated = true;
						stationUser.Status = StationUser.UserStatus.Connected;
					}
					else
					{
						stationUser.HostName = stationMac;
						stationUser.IpAddress = "";
						stationUser.HostNameResolved = false;
					}
				}
				else if (stationUser.HostNameResolved == false)
				{
					if (stationUser.CanTryResolveHost())
					{
						if (stationUser.ResolveHostName())
						{
							userUpdated = true;
						}
					}
					else
					{
						// do not read the host name again
						stationUser.HostNameResolved = true;
					}
				}
			}

			for (int i = _stationUsers.Count - 1; i >= 0; i--)
			{
				var user = _stationUsers[i];
				if (!user._checked)
				{
					userRemoved = true;
					_stationUsers[i].Status = StationUser.UserStatus.Disconnect;
					_stationUsers.RemoveAt(i);
				}
				user._checked = false;
			}

			if (userAdded)
				RaiseOnUserConnected(newUsers);
			if (userUpdated)
				RaiseOnUserUpdated();
			if (userRemoved)
				RaiseOnUserLeave();
		}

		private void RaiseLog(string msg)
		{
			if (LogEvent != null && msg != null)
				LogEvent(msg);
		}

		protected virtual void RaiseOnWorkingStatusChanged()
		{
			if (OnWorkingStatusChanged != null)
				_threadedEvents.BeginInvoke(OnWorkingStatusChanged, this);
		}

		protected virtual void RaiseOnSharedConnectionChanged()
		{
			if (OnSharedConnectionChanged != null)
				_threadedEvents.BeginInvoke(OnSharedConnectionChanged, this);
		}

		protected virtual void RaiseOnConnectionsListChanged()
		{
			if (OnConnectionsListChanged != null)
				_threadedEvents.BeginInvoke(OnConnectionsListChanged, this);
		}
		protected virtual void RaiseOnUserConnected(IList<StationUser> newUsers)
		{
			if (OnUserConnected != null)
				_threadedEvents.BeginInvoke(OnUserConnected, this, newUsers);
		}
		protected virtual void RaiseOnUserLeave()
		{
			if (OnUserLeave != null)
				_threadedEvents.BeginInvoke(OnUserLeave, this);
		}
		protected virtual void RaiseOnUserUpdated()
		{
			if (OnUserUpdated != null)
				_threadedEvents.BeginInvoke(OnUserUpdated, this);
		}

		/// <summary>
		/// Saveing the last error network .preventing error message for same network
		/// </summary>
		private IcsConnection _lastFailedToEnableSharingFor = null;

		protected virtual void RaiseOnFailedToEnableSharing(IcsConnection failedToEnableSharingFor, Exception exception)
		{
			if (OnFailedToEnableSharing != null &&
				_lastFailedToEnableSharingFor != failedToEnableSharingFor)
			{
				if (_lastFailedToEnableSharingFor != null && _lastFailedToEnableSharingFor.Guid == failedToEnableSharingFor.Guid)
					return;

				_lastFailedToEnableSharingFor = failedToEnableSharingFor;

				_threadedEvents.BeginInvoke(OnFailedToEnableSharing, this, exception);

				// preventing error message for same network
			}
		}

		#endregion

		#region Windows Virtual Network Events
		private void _wlanManager_HostedNetworkStarted(object sender, EventArgs e)
		{
			StationsReadRequest();
		}

		private void _wlanManager_HostedNetworkStopped(object sender, EventArgs e)
		{
			StationsReadRequest();
			if (Status != WorkingStatus.StopFailed)
				Status = WorkingStatus.Stopped;
		}

		private void _wlanManager_StationStateChange(object sender, EventArgs e)
		{
			StationsReadRequest();
		}

		private void _wlanManager_StationLeave(object sender, EventArgs e)
		{
			StationsReadRequest();
		}

		private void _wlanManager_StationJoin(object sender, EventArgs e)
		{
			StationsReadRequest();
		}
		#endregion

		#region Windows System Events
		private void NetworkChange_NetworkAddressChanged(object sender, EventArgs e)
		{
#if TRACE
			LogExceptions.LogTrace(true, "NetworkChange_NetworkAddressChanged");
#endif
			AutoUpdatedSharedConnection();
		}

		private void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
		{
#if TRACE
			LogExceptions.LogTrace(true, "NetworkChange_NetworkAvailabilityChanged>" + e.IsAvailable);
#endif
			AutoUpdatedSharedConnection();
		}

		private void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
		{
			// when resuming from hibernate or sleep mode
			if (e.Mode == PowerModes.Resume)
			{
				// checking the previous state
				switch (Status)
				{
					case WorkingStatus.Started:
					case WorkingStatus.Starting:
					case WorkingStatus.StartFailed:
						StartAsync();
						break;
				}
			}
		}

		#endregion

	}
}
