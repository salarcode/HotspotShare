using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HotspotShare.Classes
{
	public class RememberWindow
	{
		public static void MoveToEdges_BottomRight(Form frm)
		{
			var fw = frm.Width;
			var fh = frm.Height;
			var sw = Screen.PrimaryScreen.WorkingArea.Width;
			var sh = Screen.PrimaryScreen.WorkingArea.Height;

			var x = sw - fw;
			var y = sh - fh;
			frm.Location = new Point(x, y);
		}
	}
}
