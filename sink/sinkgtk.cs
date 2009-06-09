
using System;
using System.IO;

namespace sink
{
	
	
	public class sinkgtk : Gtk.Window
	{
		private world world_start;
		protected Gtk.Action NewSimulation, StopSimulation;
		protected Gtk.ToggleAction PauseSimulation;
		protected Gtk.Statusbar statusbar1;
		private Gdk.GC gc;
		protected Gtk.DrawingArea drawingarea1;
		private bool pause = false;
		private int Mark = 1;
		private int Size; 
		private int limit = 600;
		private int cicle = 0;
		private int max_cicle = 100000;
		private double[] conf;
		private StreamWriter fwr; 
		
		public sinkgtk() :
				base("")
		{
			Stetic.Gui.Build(this, typeof(sink.sinkgtk));			
		}
		
		public void init()
		{
			this.Resize(this.limit,this.limit+50); 
			this.conf = new double[10];	
		}
		
		//"New Simulation" MenuButton is activated:				
				protected virtual void OnNewSimulationActivated(object sender, System.EventArgs e)
				{
					Console.WriteLine("Creating world....");
										
					//Open the configuration's file to write 
					//in the "this.conf[]" the valor of the variables.					
					this.open_var(); 
					
					//Open the "exel" file
					DateTime today=DateTime.Now;
					string filename = (today.Day).ToString()+ (today.Month).ToString() + (today.Year).ToString() + "-" + (today.Hour).ToString() + (today.Minute).ToString()+ (today.Second).ToString()+".xls";
   					FileStream fs = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write);
					fwr = new StreamWriter(fs);
					fwr.WriteLine("Size;"+(this.conf[4]).ToString()+";Population;"+(this.conf[1]).ToString()+";Mortality;"+(this.conf[2]).ToString()+";Patch;"+(this.conf[3]).ToString()+";World internal var;"+(this.conf[5]).ToString()+";"+(this.conf[6]).ToString()+";"+(this.conf[7]).ToString()+";"+(this.conf[8]).ToString());
					fwr.WriteLine("Cicles;Population;Density;Neighborhood");
					
					//Set the maxim number of cicles value
					this.max_cicle = (int)this.conf[0]; 
					
					//Set the Size value
					this.Size = (int)this.conf[4];
					
					int [] dates = new int[8];
					for(int i = 0; i < 8; i++){
						dates[i] = (int)this.conf[i+1];
					}					
					//Start the "world" simulation
					world_start = new world(dates); 
					this.cicle = 0;
					this.pause = false;	
					this.update_world();
					
					//Menu-buttons sensitive
					this.NewSimulation.Sensitive = false;
					this.StopSimulation.Sensitive = true;
					this.PauseSimulation.Sensitive= true;
					
				}
			
		//"Pause Simulation" MenuButton is activated:
				protected virtual void OnPauseSimulationActivated(object sender, System.EventArgs e)
				{
					pause_world();
				}
				
		//"Stop Simulation" MenuButton is activated:	
				protected virtual void OnStopSimulationActivated(object sender, System.EventArgs e)
				{
					Console.WriteLine("Deleting world...");
					
					//Stop simulation
					Mark = 1;
					this.pause = false;
					//Paint white rectangle
					this.drawingarea1.QueueDraw();	
					//Close the "excel" file
					fwr.Close();
					
					//Menu-buttons sensitive
					this.NewSimulation.Sensitive = true;
					this.StopSimulation.Sensitive = false;
					this.PauseSimulation.Sensitive= false;
					this.PauseSimulation.Active = false;
					
				}
		//"Exit Simulation" MenuButton is activated:	
				protected virtual void OnExitActivated(object sender, System.EventArgs e)
				{
					Gtk.Application.Quit();
				}
				
				public void open_var()
				{
					StreamReader fp;
					try {
						fp=File.OpenText("config.txt");
						for(int i = 0; i < 10; i++){
							this.conf[i] = Convert.ToDouble(fp.ReadLine());
						}	
						fp.Close();						
					}catch (Exception o){
						//The default settings
						this.conf[0] = 999000.00;
						this.conf[1] = 100.00;
						this.conf[2] = 10.00;
						this.conf[3] = 100.00;
						this.conf[4] = 60.00;
						this.conf[5] = 8.00;
						this.conf[6] = 3.00;
						this.conf[7] = 2.00;
						this.conf[8] = 3.00;
						
						//Write the error
						Console.WriteLine("\"config.txt\" doesn't exist");
						Console.WriteLine(o.ToString());
						
						//Create a new configure file if it doesn't exist
						StreamWriter fw;
  						fw=File.CreateText("config.txt");
  						for(int i = 0; i < 10; i++){
  							fw.WriteLine((this.conf[i]).ToString());
  						}
  						fw.Close();
					}
				}
		
		//"Intro Variables" MenuButton is activated:	
				protected virtual void OnIntroVariablesActivated(object sender, System.EventArgs e)
				{
					this.open_var();
					configuration conf = new configuration();
					conf.var_default(this.conf);
					conf.Run();
					conf.Destroy();
				}

		//Drawing area:
				protected virtual void OnDrawingarea1ExposeEvent(object o, Gtk.ExposeEventArgs args)
				{
					Gtk.Widget DrawingArea = (Gtk.Widget)o;
					Gdk.Color color =  new Gdk.Color((byte)255,(byte)255,(byte)255);
					this.gc = new Gdk.GC(DrawingArea.GdkWindow);
					this.gc.RgbFgColor = color;
					DrawingArea.GdkWindow.DrawRectangle(this.gc,true,
                                             0,
                                             0,
                                             this.limit*2,
                                             this.limit*2);
                                             
                    //If the simulation isn't stopped (Mark = 0): 
                   	if(Mark == 0){   						
						int[,] population = this.world_start.get_population_array();
						int[,] habitat = this.world_start.get_habitat_array();
						
	                    for (int x = 0; x < this.Size; x++) {
	                    	for (int y = 0; y < this.Size; y++) {
	                    		int n_x = ((this.limit*x)/this.Size);
	                		  	int n_y = ((this.limit*y)/this.Size);
	                    		if(habitat[x,y] == 1)
	                    		{
	                       			color = new Gdk.Color((byte)0,(byte)255,(byte)0);
									this.gc.RgbFgColor = color;
									DrawingArea.GdkWindow.DrawRectangle(this.gc,true,
                                             n_x,
                                             n_y,
                                             this.limit/this.Size,
                                             this.limit/this.Size);
                                             
                                    color = new Gdk.Color((byte)0,(byte)0,(byte)0);
									this.gc.RgbFgColor = color;         
                                    DrawingArea.GdkWindow.DrawRectangle(this.gc,false,
                                             n_x,
                                             n_y,
                                             this.limit/this.Size,
                                             this.limit/this.Size);   
	                    		}
	                    		if(population[x,y] == 1){
	                    			color = new Gdk.Color((byte)255,(byte)0,(byte)0);
									this.gc.RgbFgColor = color;
									DrawingArea.GdkWindow.DrawRectangle(this.gc,true,
                                             n_x,
                                             n_y,
                                             (this.limit/this.Size)-1,
                                             (this.limit/this.Size)-1);  
	                    		}
	                    	}
						}	
						update_world();   
					}
				}
				
				public int normalize(int coord, int limit)
				{
					return limit/coord;
				}
				
				public bool update_world()
				{
					Mark = 0;
					
					if(pause==false){
						//If cicle >= maxim number of cicles, simulation is stopped with Mark = 1
						if(cicle >= this.max_cicle){
								this.Mark= 1;	
								fwr.Close(); //Close the "excel" file
								//Menu-buttons sensitive
								this.NewSimulation.Sensitive = true;
								this.StopSimulation.Sensitive = false;
								this.PauseSimulation.Sensitive= false;
						}else{
							//New cicle starts
							this.cicle++;
							this.world_start.cicle();
							
							//Dates of "the world"
							float population = this.world_start.get_population();
							float density = this.world_start.get_density();
							float proximity = this.world_start.get_proximity();
							
							//Write the statusbar
							this.statusbar1.Push(1, "Cicle: "+this.cicle.ToString() +"            Population: "+population.ToString()+"    Density: "+density.ToString()+"     Neighborhood: "+proximity.ToString());
							
							//Write the "exel" file every cicle
							string info = (this.cicle).ToString()+";"+population.ToString()+";"+density.ToString()+";"+proximity.ToString();
							fwr.WriteLine(info);
						}
						this.drawingarea1.QueueDraw();
					} 
					return false;
				}
				
				public void pause_world()
				{
					if(this.pause){
						this.pause = false;
					}else{
						this.pause = true;						
					}	
				}

		//Windows exit-button is activated:	
				protected virtual void OnRemoved(object o, Gtk.RemovedArgs args)
				{
					Gtk.Application.Quit();
				}

				protected virtual void OnHelp1Activated(object sender, System.EventArgs e)
				{
					help helpuser = new help();
					helpuser.Run();
					helpuser.Destroy();
				}

				protected virtual void OnAboutAuthorsActivated(object sender, System.EventArgs e)
				{
					authors authormenu = new authors();
					authormenu.Run();
					authormenu.Destroy();
				}
	
				
	}
}
