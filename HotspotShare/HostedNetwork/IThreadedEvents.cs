using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotspotShare.HostedNetwork
{
	public interface IThreadedEvents
	{
		/// <summary>
		/// Executes the specified delegate asynchronously with the specified arguments, on the thread that the control's underlying handle was created on.
		/// </summary>
		/// <param name="method">A delegate to a method that takes no parameters.</param>
		/// <returns></returns>
		IAsyncResult BeginInvoke(System.Delegate method);

		/// <summary>
		/// Executes the specified delegate asynchronously with the specified arguments, on the thread that the control's underlying handle was created on.
		/// </summary>
		/// <param name="method">A delegate to a method that takes no parameters.</param>
		/// <param name="args"> An array of objects to pass as arguments to the given method. This can be null if no arguments are needed.</param>
		/// <returns></returns>
		IAsyncResult BeginInvoke(System.Delegate method, params object[] args);
		/// <summary>
		/// Executes the specified delegate asynchronously with the specified arguments, on the thread that the control's underlying handle was created on.
		/// </summary>
		/// <param name="method">A delegate to a method that takes no parameters.</param>
		/// <returns></returns>
		IAsyncResult BeginInvoke(System.Action method);

		/// <summary>
		/// Executes the specified delegate asynchronously with the specified arguments, on the thread that the control's underlying handle was created on.
		/// </summary>
		/// <param name="method">A delegate to a method that takes no parameters.</param>
		/// <param name="@object"> An array of objects to pass as arguments to the given method. This can be null if no arguments are needed.</param>
		/// <returns></returns>
		IAsyncResult BeginInvoke(System.Action method, object @object);
	}
}
