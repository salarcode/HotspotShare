using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NETWORKLIST;

namespace HotspotShare.Api
{
	public class WindowsNetwork
	{
		readonly NetworkListManager _netManager = new NetworkListManager();

		public string GetNetworkNameConnectedToInternet()
		{
			if (!_netManager.IsConnectedToInternet)
				return null;
			var connected = _netManager.GetNetworks(NLM_ENUM_NETWORK.NLM_ENUM_NETWORK_CONNECTED);
			foreach (INetwork network in connected)
			{
				if (network.IsConnectedToInternet)
					return network.GetName();
			}
			return null;
		}
		public Guid GetNetworkGuidConnectedToInternet()
		{
			if (!_netManager.IsConnectedToInternet)
				return Guid.Empty;
			var connected = _netManager.GetNetworks(NLM_ENUM_NETWORK.NLM_ENUM_NETWORK_CONNECTED);
			foreach (INetwork network in connected)
			{
				if (network.IsConnectedToInternet)
				{
					foreach (INetworkConnection conn in network.GetNetworkConnections())
					{
						return conn.GetAdapterId();
					}

					return network.GetNetworkId();
				}
			}
			return Guid.Empty;
		}
		public Guid GetNetworkGuidConnectedToInternet(Guid ignoreAdapterId)
		{
			if (!_netManager.IsConnectedToInternet)
				return Guid.Empty;
			var connected = _netManager.GetNetworks(NLM_ENUM_NETWORK.NLM_ENUM_NETWORK_CONNECTED);
			foreach (INetwork network in connected)
			{
				if (network.IsConnectedToInternet)
				{
					foreach (INetworkConnection conn in network.GetNetworkConnections())
					{
						var adapterId = conn.GetAdapterId();
						if (adapterId == ignoreAdapterId)
							continue;
						return adapterId;
					}

					return network.GetNetworkId();
				}
			}
			return Guid.Empty;
		}
	}
}
