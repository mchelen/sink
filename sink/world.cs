
using System;

namespace sink
{
	
	
	public class world
	{
		private int[,] population;
		private int[,] proximity;
		private int[,] habitat;
		private int x_axis;
		private int y_axis;
		private int n_population;
		private int mortality;
		private int patch;
		int [] dates;
		
		//Constructor. "dates" is an array with the config values
		public world(int [] dates)
		{
			this.dates = new int[10];
			this.dates = dates;
			this.x_axis = dates[3];
			this.y_axis = dates[3];
			
			this.population = new int[this.x_axis, this.y_axis];	
			this.proximity = new int[this.x_axis, this.y_axis];
			this.habitat = new int[this.x_axis, this.y_axis];
			
			for(int i = 0; i < this.x_axis; i++){
				for(int e = 0; e < this.y_axis; e++){
					this.population[i,e] = 0;
					this.proximity[i,e] = 0;
					this.habitat[i,e] = 0;
				}
			}
			
			this.n_population = dates[0];
			this.mortality = dates[1];
			this.patch = dates[2];			
			
			this.init();
		}
		
	
		//Start World's values
		public void init()
		{
			int x = 0, y = 0;
			Random oRand = new Random(unchecked((int) DateTime.Now.Ticks));
			//Init the "habitat" array
			for(int i = 0; i < this.patch; i++){
				x = (oRand.Next() % (this.x_axis - 1));
				y = (oRand.Next() % (this.y_axis - 1));				
			
				for(int j = y-1; j < y+2; j++){
					for(int e = x-1; e < x+2; e++){
						this.habitat[this.normalize(e,this.x_axis), this.normalize(j,this.y_axis)] = 1;
					}
				}				
			}
			
			//Init the "population" array
			for(int i = 0; i < this.n_population; i++){
				x = (oRand.Next() % (this.x_axis - 1));
				y = (oRand.Next() % (this.y_axis - 1));
				this.population[x,y] = 1;	
			}
			
		}
		
		
		public int normalize(int coord, int limit){
			if (coord < 0)
				return (limit - Math.Abs(coord)) % limit;
			else if (coord >= limit)
				return coord - limit;
			else
				return coord;
		}
		
				
		//Add the neighbours of every square at "proximity" array
		public void density_calculus()
		{
			for(int x = 0; x < this.x_axis; x++){
				for(int y = 0; y < this.y_axis; y++){
					this.proximity[x,y] = 0;					
						for(int j = x-1; j < x+2; j++){	
							for(int e = y-1; e < y+2; e++){
							if(j != x || e != y)								
								this.proximity[x,y] += this.population[this.normalize(j,this.x_axis),this.normalize(e,this.y_axis)];								
						}
					}
				}			
			}
			
		}
		
		
		public void update()
		{
			int x = 0, y = 0;
			Random oRand = new Random(unchecked((int) DateTime.Now.Ticks));
			for(x = 0; x < this.x_axis; x++){
				for(y = 0; y < this.y_axis; y++){				
					if(this.population[x,y] == 1){
						if((this.proximity[x,y] >= this.dates[4] && this.habitat[x,y] == 1)||(this.proximity[x,y] >= this.dates[5] && this.habitat[x,y] == 0)){
							this.population[x,y] = 0;
						}						
					}else{
						if((this.proximity[x,y] >= this.dates[6] && this.habitat[x,y] == 1)||(this.proximity[x,y] >= this.dates[7] && this.habitat[x,y] == 0)){
							this.population[x,y] = 1;
						}
					}					
				}
			}	
			
			//Mortality
			for(int i= 0; i < this.mortality; i++){
				x = (oRand.Next() % (this.x_axis - 1));
				y = (oRand.Next() % (this.y_axis - 1));
				this.population[x,y] = 0;	
			}			
		}
		
			
		public void cicle()
		{
			this.density_calculus();
			this.update();
		}
		
		
		public float get_population()
		{
			float population = 0;
			for(int x = 0; x < this.x_axis; x++){
				for(int y = 0; y < this.y_axis; y++){
					population += this.population[x,y];
				}
			}
			return population;
		}
		
		
		public float get_density()
		{
			float density = this.get_population()/(this.x_axis * this.y_axis);
			return density;
		}
		
		
		public float get_proximity()
		{
			float proximity = this.get_population()/((this.x_axis*this.y_axis)/8);
			return proximity;
		}
		
		
		public int[,] get_population_array()
		{
			return this.population;
		}
		
		
		public int[,] get_habitat_array()
		{
			return this.habitat;
		}
		
		
	}
}
