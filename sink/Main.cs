// project created on 19/04/2007 at 13:47
using System;
using Gtk;

namespace sink
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Application.Init ();
			sinkgtk world = new sinkgtk();
			world.init();
			Application.Run ();
		}
	}
}