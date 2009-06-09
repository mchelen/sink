
using System;

namespace sink
{
	
	
	public class authors : Gtk.AboutDialog
	{
		
		public authors()
		{
			string[] authorname = new string[2];
			authorname[0] = "Sandra Castillo Priego";
			this.Authors = authorname;
			Gdk.Pixbuf imagesink = new Gdk.Pixbuf("sink.bmp");
			this.Logo= imagesink;
			this.Name = "SINK";
			
		}
		
	}
}

