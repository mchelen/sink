
using System;
using System.IO;

namespace sink
{
	
	
	public class configuration : Gtk.Dialog
	{
		protected Gtk.HScale hscale1, hscale2, hscale3, hscale4, hscale5, hscale7, hscale8, hscale9, hscale10;
	
		
		public configuration()
		{	
			Stetic.Gui.Build(this, typeof(sink.configuration));					
		}

		public void var_default(double[] conf){
			hscale1.Value = conf[0];
			hscale2.Value = conf[1];
			hscale3.Value = conf[2];
			hscale4.Value = conf[3];
			hscale5.Value = conf[4];
			hscale7.Value = conf[5];
			hscale8.Value = conf[6];
			hscale9.Value = conf[7];
			hscale10.Value = conf[8];	
		}
		protected virtual void OnButton1Released(object sender, System.EventArgs e)
		{
			this.save_conf();
			this.Hide();
		}
		
		public void save_conf()
		{
			try {
				StreamWriter fw;
  				fw=File.CreateText("config.txt");
  				fw.WriteLine((this.hscale1.Value).ToString());
  				fw.WriteLine((this.hscale2.Value).ToString());
  				fw.WriteLine((this.hscale3.Value).ToString());
  				fw.WriteLine((this.hscale4.Value).ToString());
  				fw.WriteLine((this.hscale5.Value).ToString());
  				//fw.WriteLine((this.hscale6.Value).ToString());
  				fw.WriteLine((this.hscale7.Value).ToString());
  				fw.WriteLine((this.hscale8.Value).ToString());
  				fw.WriteLine((this.hscale9.Value).ToString());
  				fw.WriteLine((this.hscale10.Value).ToString());
  				fw.Close();
  			}catch (Exception e){
  				Console.WriteLine(e.ToString());
				Console.WriteLine("file \"config.txt\" can't be created");				
			}
		}

	}
}
