/**
 * FH Joanneum Point-of-Interest App
 * IMS14 / SS2015 / Cloud Computing
 * Team: Florian Maderbacher, Robert Muehlberger, Robert Puerer
 * Version: 1.0
 */

using System;

namespace FHPoiApp
{
	public class Settings
	{
		private static Settings INSTANCE;

		public static Settings getInstance()
		{
			if (Settings.INSTANCE == null) {
				Settings.INSTANCE = new Settings ();
				Settings.INSTANCE.Init (); // first initialization
			}
			return Settings.INSTANCE;
		}

		private Settings ()
		{
			// empty constructor
		}

		private void Init() 
		{

		}

		public String getServerUrl()
		{
			// TODO
			return "http://fh-poi-app.appspot.com/";
		}

		public void setServerUrl(String Url)
		{
			// TODO
		}
	}
}

