using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using HotspotShare.Api.WinAPI;
using HotspotShare.HostedNetwork;

namespace HotspotShare.Api
{
	public class WlanManager : IDisposable
	{
		private IntPtr _wlanHandle;
		private uint _serverVersion;

		private wlanapi.WLAN_NOTIFICATION_CALLBACK _notificationCallback;

		private WLAN_HOSTED_NETWORK_STATE _hostedNetworkState;

		protected Dictionary<string, WlanStation> _stations = new Dictionary<string, WlanStation>();

		public WlanManager()
		{
			_notificationCallback = new wlanapi.WLAN_NOTIFICATION_CALLBACK(OnNotification);
			Init();
		}

		private void Init()
		{
			try
			{
				WlanUtils.Throw_On_Win32_Error(wlanapi.WlanOpenHandle(wlanapi.WLAN_CLIENT_VERSION_VISTA, IntPtr.Zero, out _serverVersion, ref _wlanHandle));


				WLAN_NOTIFICATION_SOURCE notifSource;
				WlanUtils.Throw_On_Win32_Error(wlanapi.WlanRegisterNotification(_wlanHandle, WLAN_NOTIFICATION_SOURCE.All, true, _notificationCallback, IntPtr.Zero, IntPtr.Zero, out notifSource));


				WLAN_HOSTED_NETWORK_REASON failReason = InitSettings();
				if (failReason != WLAN_HOSTED_NETWORK_REASON.wlan_hosted_network_reason_success)
				{
					throw new Exception("Init Error WlanHostedNetworkInitSettings: " + failReason.ToString());
				}

			}
			catch
			{
				wlanapi.WlanCloseHandle(_wlanHandle, IntPtr.Zero);
				throw;
			}
		}

		#region "Events"

		public event EventHandler HostedNetworkStarted;
		public event EventHandler HostedNetworkStopped;
		public event EventHandler HostedNetworkAvailable;

		public event EventHandler StationJoin;
		public event EventHandler StationLeave;
		public event EventHandler StationStateChange;

		#endregion

		#region "OnNotification"

		protected void onHostedNetworkStarted()
		{
			_hostedNetworkState = WLAN_HOSTED_NETWORK_STATE.wlan_hosted_network_active;
			if (HostedNetworkStarted != null)
			{
				HostedNetworkStarted(this, EventArgs.Empty);
			}
		}

		protected void OnHostedNetworkStopped()
		{
			_hostedNetworkState = WLAN_HOSTED_NETWORK_STATE.wlan_hosted_network_idle;

			if (HostedNetworkStopped != null)
			{
				HostedNetworkStopped(this, EventArgs.Empty);
			}
		}

		protected void OnHostedNetworkAvailable()
		{
			_hostedNetworkState = WLAN_HOSTED_NETWORK_STATE.wlan_hosted_network_idle;

			if (HostedNetworkAvailable != null)
			{
				HostedNetworkAvailable(this, EventArgs.Empty);
			}
		}

		protected void OnStationJoin(WLAN_HOSTED_NETWORK_PEER_STATE stationState)
		{
			var pStation = new WlanStation(stationState);
			_stations[pStation.MacAddress] = pStation;

			if (StationJoin != null)
			{
				StationJoin(this, EventArgs.Empty);
			}
		}

		protected void OnStationLeave(WLAN_HOSTED_NETWORK_PEER_STATE stationState)
		{
			_stations.Remove(stationState.PeerMacAddress.ConvertToString());
			
			RaiseStationLeave();
		}

		protected void RaiseStationLeave()
		{
			if (StationLeave != null)
			{
				StationLeave(this, EventArgs.Empty);
			}
		}

		protected void OnStationStateChange(WLAN_HOSTED_NETWORK_PEER_STATE stationState)
		{
			if (StationStateChange != null)
			{
				StationStateChange(this, EventArgs.Empty);
			}
		}

		protected void OnNotification(ref WLAN_NOTIFICATION_DATA notifData, IntPtr context)
		{
			switch (notifData.notificationCode)
			{
				case (int)WLAN_HOSTED_NETWORK_NOTIFICATION_CODE.wlan_hosted_network_state_change:

					if (notifData.dataSize > 0 && notifData.dataPtr != IntPtr.Zero)
					{
						WLAN_HOSTED_NETWORK_STATE_CHANGE pStateChange = (WLAN_HOSTED_NETWORK_STATE_CHANGE)Marshal.PtrToStructure(notifData.dataPtr, typeof(WLAN_HOSTED_NETWORK_STATE_CHANGE));

						switch (pStateChange.NewState)
						{
							case WLAN_HOSTED_NETWORK_STATE.wlan_hosted_network_active:
								onHostedNetworkStarted();
								break;

							case WLAN_HOSTED_NETWORK_STATE.wlan_hosted_network_idle:
								if (pStateChange.OldState == WLAN_HOSTED_NETWORK_STATE.wlan_hosted_network_active)
								{
									OnHostedNetworkStopped();
								}
								else
								{
									OnHostedNetworkAvailable();
								}
								break;

							case WLAN_HOSTED_NETWORK_STATE.wlan_hosted_network_unavailable:
								if (pStateChange.OldState == WLAN_HOSTED_NETWORK_STATE.wlan_hosted_network_active)
								{
									OnHostedNetworkStopped();
								}
								OnHostedNetworkAvailable();
								break;
						}
					}

					break;

				case (int)WLAN_HOSTED_NETWORK_NOTIFICATION_CODE.wlan_hosted_network_peer_state_change:

					if (notifData.dataSize > 0 && notifData.dataPtr != IntPtr.Zero)
					{
						WLAN_HOSTED_NETWORK_DATA_PEER_STATE_CHANGE pPeerStateChange = (WLAN_HOSTED_NETWORK_DATA_PEER_STATE_CHANGE)Marshal.PtrToStructure(notifData.dataPtr, typeof(WLAN_HOSTED_NETWORK_DATA_PEER_STATE_CHANGE));

						if (pPeerStateChange.NewState.PeerAuthState == WLAN_HOSTED_NETWORK_PEER_AUTH_STATE.wlan_hosted_network_peer_state_authenticated)
						{
							// Station joined the hosted network
							OnStationJoin(pPeerStateChange.NewState);
						}
						else if (pPeerStateChange.NewState.PeerAuthState == WLAN_HOSTED_NETWORK_PEER_AUTH_STATE.wlan_hosted_network_peer_state_invalid)
						{
							// Station left the hosted network
							OnStationLeave(pPeerStateChange.NewState);
						}
						else
						{
							// Authentication state changed
							OnStationStateChange(pPeerStateChange.NewState);
						}
					}

					break;

				case (int)WLAN_HOSTED_NETWORK_NOTIFICATION_CODE.wlan_hosted_network_radio_state_change:
					if (notifData.dataSize > 0 && notifData.dataPtr != IntPtr.Zero)
					{
						//WLAN_HOSTED_NETWORK_RADIO_STATE pRadioState = (WLAN_HOSTED_NETWORK_RADIO_STATE)Marshal.PtrToStructure(notifData.dataPtr, typeof(WLAN_HOSTED_NETWORK_RADIO_STATE));
						// Do nothing for now
					}
					//else
					//{
					//    // // Shall NOT happen
					//    // _ASSERT(FAILSE);
					//}
					break;
			}

		}

		#endregion

		#region "Public Methods"

		public WLAN_HOSTED_NETWORK_REASON ForceStart()
		{
			WLAN_HOSTED_NETWORK_REASON failReason;
			WlanUtils.Throw_On_Win32_Error(wlanapi.WlanHostedNetworkForceStart(_wlanHandle, out failReason, IntPtr.Zero));

			_hostedNetworkState = WLAN_HOSTED_NETWORK_STATE.wlan_hosted_network_active;

			return failReason;
		}

		public WLAN_HOSTED_NETWORK_REASON ForceStop()
		{
			WLAN_HOSTED_NETWORK_REASON failReason;
			WlanUtils.Throw_On_Win32_Error(wlanapi.WlanHostedNetworkForceStop(_wlanHandle, out failReason, IntPtr.Zero));

			_hostedNetworkState = WLAN_HOSTED_NETWORK_STATE.wlan_hosted_network_idle;

			_stations.Clear();
			RaiseStationLeave();

			return failReason;
		}

		public WLAN_HOSTED_NETWORK_REASON StartUsing()
		{
			WLAN_HOSTED_NETWORK_REASON failReason;
			WlanUtils.Throw_On_Win32_Error(wlanapi.WlanHostedNetworkStartUsing(_wlanHandle, out failReason, IntPtr.Zero));

			_hostedNetworkState = WLAN_HOSTED_NETWORK_STATE.wlan_hosted_network_active;

			return failReason;
		}

		public WLAN_HOSTED_NETWORK_REASON StopUsing()
		{
			WLAN_HOSTED_NETWORK_REASON failReason;
			WlanUtils.Throw_On_Win32_Error(wlanapi.WlanHostedNetworkStopUsing(_wlanHandle, out failReason, IntPtr.Zero));

			_hostedNetworkState = WLAN_HOSTED_NETWORK_STATE.wlan_hosted_network_idle;

			_stations.Clear();
			RaiseStationLeave();

			return failReason;
		}

		public WLAN_HOSTED_NETWORK_REASON InitSettings()
		{
			WLAN_HOSTED_NETWORK_REASON failReason;
			WlanUtils.Throw_On_Win32_Error(wlanapi.WlanHostedNetworkInitSettings(_wlanHandle, out failReason, IntPtr.Zero));
			return failReason;
		}

		public WLAN_HOSTED_NETWORK_REASON QuerySecondaryKey(out string passKey, out bool isPassPhrase, out bool isPersistent)
		{
			WLAN_HOSTED_NETWORK_REASON failReason;
			uint keyLen;
			WlanUtils.Throw_On_Win32_Error(wlanapi.WlanHostedNetworkQuerySecondaryKey(_wlanHandle, out keyLen, out passKey, out isPassPhrase, out isPersistent, out failReason, IntPtr.Zero));
			return failReason;
		}

		public WLAN_HOSTED_NETWORK_REASON SetSecondaryKey(string passKey)
		{
			WLAN_HOSTED_NETWORK_REASON failReason;

			WlanUtils.Throw_On_Win32_Error(wlanapi.WlanHostedNetworkSetSecondaryKey(_wlanHandle, (uint)(passKey.Length + 1), passKey, true, true, out failReason, IntPtr.Zero));

			return failReason;
		}

		public WLAN_HOSTED_NETWORK_STATUS QueryStatus()
		{
			IntPtr pStatus;
			WlanUtils.Throw_On_Win32_Error(wlanapi.WlanHostedNetworkQueryStatus(_wlanHandle, out pStatus, IntPtr.Zero));
			WLAN_HOSTED_NETWORK_STATUS status = (WLAN_HOSTED_NETWORK_STATUS)Marshal.PtrToStructure(pStatus, typeof(WLAN_HOSTED_NETWORK_STATUS));
			return status;
		}

		public WLAN_HOSTED_NETWORK_REASON SetConnectionSettings(string hostedNetworkSSID, int maxNumberOfPeers)
		{
			WLAN_HOSTED_NETWORK_REASON failReason;

			WLAN_HOSTED_NETWORK_CONNECTION_SETTINGS settings = new WLAN_HOSTED_NETWORK_CONNECTION_SETTINGS();
			settings.hostedNetworkSSID = WlanUtils.ConvertStringToDOT11_SSID(hostedNetworkSSID);
			settings.dwMaxNumberOfPeers = (uint)maxNumberOfPeers;

			IntPtr settingsPtr = Marshal.AllocHGlobal(Marshal.SizeOf(settings));
			Marshal.StructureToPtr(settings, settingsPtr, false);

			WlanUtils.Throw_On_Win32_Error(
				wlanapi.WlanHostedNetworkSetProperty(
					_wlanHandle,
					WLAN_HOSTED_NETWORK_OPCODE.wlan_hosted_network_opcode_connection_settings,
					(uint)Marshal.SizeOf(settings), settingsPtr, out failReason, IntPtr.Zero
				)
			);

			return failReason;
		}

		public WLAN_OPCODE_VALUE_TYPE QueryConnectionSettings(out string hostedNetworkSSID, out int maxNumberOfPeers)
		{
			uint dataSize;
			IntPtr dataPtr;
			WLAN_OPCODE_VALUE_TYPE opcode;

			WlanUtils.Throw_On_Win32_Error(
				wlanapi.WlanHostedNetworkQueryProperty(
					_wlanHandle,
					WLAN_HOSTED_NETWORK_OPCODE.wlan_hosted_network_opcode_connection_settings,
					out dataSize, out dataPtr, out opcode, IntPtr.Zero
				)
			);

			var settings = (WLAN_HOSTED_NETWORK_CONNECTION_SETTINGS)Marshal.PtrToStructure(dataPtr, typeof(WLAN_HOSTED_NETWORK_CONNECTION_SETTINGS));
			
			hostedNetworkSSID = settings.hostedNetworkSSID.ConvertToString();

			maxNumberOfPeers = (int)settings.dwMaxNumberOfPeers;

			return opcode;
		}

		public void StartHostedNetwork()
		{
			try
			{
				ForceStop();

				var failReason = StartUsing();
				if (failReason != WLAN_HOSTED_NETWORK_REASON.wlan_hosted_network_reason_success)
				{
					throw new Exception("Could Not Start Hosted Network!\n\n" + failReason.ToString());
				}
			}
			catch
			{
				ForceStop();
				throw;
			}
		}

		public void StopHostedNetwork()
		{
			ForceStop();
		}

		#endregion

		#region "Properties"

		public Guid HostedNetworkInterfaceGuid
		{
			get
			{
				var status = QueryStatus();
				return status.IPDeviceID;
			}
		}

		public HostedNetworkInfo HostedNetworkInfo
		{
			get
			{
				var status = QueryStatus();

				return new HostedNetworkInfo()
				{
					Dot11PhyType = status.dot11PhyType,
					DwNumberOfPeers = status.dwNumberOfPeers,
					HostedNetworkState = status.HostedNetworkState,
					IPDeviceId = status.IPDeviceID,
					UlChannelFrequency = status.ulChannelFrequency,
					WlanHostedNetworkBssid = status.wlanHostedNetworkBSSID
				};
			}
		}

		public WLAN_HOSTED_NETWORK_STATE HostedNetworkState
		{
			get
			{
				return _hostedNetworkState;
			}
		}

		public Dictionary<string, WlanStation> Stations
		{
			get
			{
				return _stations;
			}
		}

		public bool IsHostedNetworkStarted
		{
			get
			{
				return (_hostedNetworkState == WLAN_HOSTED_NETWORK_STATE.wlan_hosted_network_active);
			}
		}


		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			ForceStop();

			if (_wlanHandle != IntPtr.Zero)
			{
				wlanapi.WlanCloseHandle(_wlanHandle, IntPtr.Zero);
			}
		}

		#endregion
	}
}
