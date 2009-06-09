
using System;

namespace sink
{
	
	
	public class help : Gtk.Dialog
	{
		
		public help()
		{
			Stetic.Gui.Build(this, typeof(sink.help));
			
		}

		protected virtual void OnButton3Released(object sender, System.EventArgs e)
		{
			this.Hide();
		}
	}
}
