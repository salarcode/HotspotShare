using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace HotspotShare.Classes
{
	public static class LogExceptions
	{
		private static string AppPath(string file)
		{
			return Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), file);
		}

		public static void Log(string message)
		{
			try
			{
				var sb = new StringBuilder();
				sb.AppendLine("-------------------");
				sb.AppendLine(DateTime.Now.ToString());
				var th = Thread.CurrentThread;
				sb.AppendFormat("Thread Id: {0}, Name: {1}, ApartmentState: {2}, IsBackground: {3} \n\r",
					th.ManagedThreadId, th.Name ?? "", th.GetApartmentState(), th.IsBackground);
				sb.AppendLine(message);
				File.AppendAllText(AppPath("exceptions.log"), sb.ToString());
			}
			catch (Exception)
			{
			}
		}

		public static void Log(Exception ex)
		{
			if (ex == null)
				return;
			Log(ex.ToString());
		}

		public static void ClearTrace()
		{
			try
			{
				File.WriteAllText(AppPath("trace.log"),"");
			}
			catch (Exception)
			{
			}
		}

		public static void LogTrace(bool addDate = true, params string[] message)
		{
			try
			{
				var sb = new StringBuilder();
				sb.AppendLine("-------------------");
				if (addDate)
				{
					sb.AppendLine(DateTime.Now.ToString());
					var th = Thread.CurrentThread;
					sb.AppendFormat("Thread Id: {0}, Name: {1}, ApartmentState: {2}, IsBackground: {3} \n\r",
						th.ManagedThreadId, th.Name ?? "", th.GetApartmentState(), th.IsBackground);
				}
				sb.AppendLine(string.Join(Environment.NewLine, message));
				File.AppendAllText(AppPath("trace.log"), sb.ToString());

			}
			catch (Exception)
			{
			}
		}
	}
}
