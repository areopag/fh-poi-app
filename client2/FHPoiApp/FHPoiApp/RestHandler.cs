/**
 * FH Joanneum Point-of-Interest App
 * IMS14 / SS2015 / Cloud Computing
 * Team: Florian Maderbacher, Robert Muehlberger, Robert Puerer
 * Version: 1.0
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace FHPoiApp
{
	public class RestHandler
	{
		private String ServerUrl = "";
		private Logger logger;

		public RestHandler (String ServerUrl)
		{
			logger = Logger.getInstance ();
			this.ServerUrl = ServerUrl;
		}

		public SortedList<Int32, PointOfInterest> getAllPOIs()
		{
			logger.add ("Quering all POIs...", "url");

			SortedList<Int32, PointOfInterest> pois = new SortedList<int, PointOfInterest> ();

			for(int i = 1; i <= 10; i++)
				pois.Add(i, new PointOfInterest(i, "FH-Graz", 47.0693127, 15.4079899, "john", "FH Joanneum Graz", "Fachhochschule"));



			// TESTS:
			//getRawWebData (ServerUrl, new Dictionary<String, String>());

			return pois;
		}

		private String getRawWebData(String Url, Dictionary<String, String> postParameters)
		{
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create (Url);
			request.Method = "POST";

			string postData = "";

			foreach (string key in postParameters.Keys)
			{
				postData += HttpUtility.UrlEncode(key) + "="
					+ HttpUtility.UrlEncode(postParameters[key]) + "&";
			}

			byte[] data = Encoding.ASCII.GetBytes(postData);

			request.ContentType = "application/x-www-form-urlencoded";
			request.ContentLength = data.Length;

			HttpWebResponse response = (HttpWebResponse)request.GetResponse ();

			Console.WriteLine ("StatusCode: " + response.StatusCode.ToString ());

			Stream responseStream = response.GetResponseStream ();
			StreamReader reader = new StreamReader (responseStream);
			String rawData = reader.ReadToEnd ();

			Console.WriteLine ("rawData: " + rawData);

			return rawData;
		}

		public SortedList<Int32, PointOfInterest> getPOIsWithAttribute(String name, String value)
		{
			logger.add ("Quering POIs with attribute: " + name + "=" + value, "url");

			SortedList<Int32, PointOfInterest> pois = new SortedList<int, PointOfInterest> ();


			return pois;
		}

		public SortedList<Int32, PointOfInterest> getPOIwithId(Int32 id)
		{
			logger.add ("Quering POIs with ID: " + id.ToString(), "url");

			SortedList<Int32, PointOfInterest> pois = new SortedList<int, PointOfInterest> ();


			return pois;
		}

		public Int32 insertPOI(PointOfInterest poi)
		{
			logger.add ("Insert a new POI", "url");

			return 0;
		}

		public void updatePOI(PointOfInterest poi)
		{
			logger.add ("Update a POI", "url");
		}

		public void deletePOI(PointOfInterest poi)
		{
			logger.add ("Delete a POI", "url");
		}
	}
}

