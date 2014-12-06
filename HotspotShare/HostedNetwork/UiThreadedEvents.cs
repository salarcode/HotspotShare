using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HotspotShare.HostedNetwork
{
	/// <summary>
	/// Raises events thread safe
	/// </summary>
	public class UiThreadedEvents : IThreadedEvents
	{
		private readonly Control _handleHolderControl;

		public UiThreadedEvents(Control handleHolderControl)
		{
			_handleHolderControl = handleHolderControl;
		}

		public IAsyncResult BeginInvoke(Delegate method)
		{
			return _handleHolderControl.BeginInvoke(method);
		}

		public IAsyncResult BeginInvoke(Delegate method, params object[] args)
		{
			return _handleHolderControl.BeginInvoke(method, args);
		}

		public IAsyncResult BeginInvoke(Action method)
		{
			return _handleHolderControl.BeginInvoke(method);
		}

		public IAsyncResult BeginInvoke(Action method, object @object)
		{
			return _handleHolderControl.BeginInvoke(method, @object);
		}
	}
}
