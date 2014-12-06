using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotspotShare.Api.WinAPI;

namespace HotspotShare.HostedNetwork
{
	public struct HostedNetworkInfo
	{
		public WLAN_HOSTED_NETWORK_STATE HostedNetworkState;
		public Guid IPDeviceId;
		public DOT11_MAC_ADDRESS WlanHostedNetworkBssid;
		public DOT11_PHY_TYPE Dot11PhyType;
		public uint UlChannelFrequency; // ULONG
		public uint DwNumberOfPeers; // DWORD
		//public WLAN_HOSTED_NETWORK_PEER_STATE PeerList[1];
		//public IntPtr PeerList;
	}
}
