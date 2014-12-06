using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotspotShare.HostedNetwork
{
	class DefaultThreadedEvents : IThreadedEvents
	{
		public IAsyncResult BeginInvoke(Delegate method)
		{
			method.Method.Invoke(method.Target, null);
			return null;
		}

		public IAsyncResult BeginInvoke(Delegate method, params object[] args)
		{
			method.Method.Invoke(method.Target, args);
			return null;
		}

		public IAsyncResult BeginInvoke(Action method)
		{
			return method.BeginInvoke(null, null);
		}

		public IAsyncResult BeginInvoke(Action method, object @object)
		{
			return method.BeginInvoke(null, @object);
		}
	}
}
