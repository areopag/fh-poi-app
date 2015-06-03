/**
 * FH Joanneum Point-of-Interest App
 * IMS14 / SS2015 / Cloud Computing
 * Team: Florian Maderbacher, Robert Muehlberger, Robert Puerer
 * Version: 1.0
 */

using System;

namespace FHPoiApp
{
	public class LoggerItem
	{
		public DateTime Timestamp;
		public String Subject;
		public String Details;

		public LoggerItem (String Subject, String Details)
		{
			this.Timestamp = DateTime.Now;
			this.Subject = Subject;
			this.Details = Details;
		}
	}
}

