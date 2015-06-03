/**
 * FH Joanneum Point-of-Interest App
 * IMS14 / SS2015 / Cloud Computing
 * Team: Florian Maderbacher, Robert Muehlberger, Robert Puerer
 * Version: 1.0
 */

using System;

namespace FHPoiApp
{
	public class PointOfInterest
	{
		public Int32 ID;
		public String Name;
		public Double Latitude;
		public Double Longitude;
		public String Creator;
		public String Description;
		public String Category;

		public PointOfInterest (Int32 ID, String Name, Double Latitude, Double Longitude, String Creator, String Description, String Category)
		{
			this.ID = ID;
			this.Name = Name;
			this.Latitude = Latitude;
			this.Longitude = Longitude;
			this.Creator = Creator;
			this.Description = Description;
			this.Category = Category;
		}
	}
}

