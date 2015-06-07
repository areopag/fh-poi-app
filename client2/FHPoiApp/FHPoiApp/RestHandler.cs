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
using System.Xml;
using Newtonsoft.Json;

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

		private SortedList<Int32, PointOfInterest> getPOIsFromJson(String jsonRaw)
		{
			SortedList<Int32, PointOfInterest> pois = new SortedList<int, PointOfInterest> ();

			// Parse the raw JSON output
			jsonRaw = jsonRaw.Trim ();
			Console.WriteLine ("jsonRaw: " + jsonRaw);

			if (jsonRaw.Length > 0) {
				if (jsonRaw.Substring (0, 1) == "[") {
					String[] rawArray = jsonRaw.Substring (1, jsonRaw.Length - 2).Split(',');
					foreach (String rawObj in rawArray) {
						if (rawObj.Trim() != "") {
							PointOfInterest poi = JsonConvert.DeserializeObject<PointOfInterest> (jsonRaw);

						}

					}
				} else if (jsonRaw == "null") {

				} else {
					//XmlNode doc = JsonConvert.DeserializeXmlNode (jsonRaw);

					PointOfInterest poi = JsonConvert.DeserializeObject<PointOfInterest> (jsonRaw);
				}
			}

			return pois;
		}

		private Dictionary<String, String> getPostDataFromPoi(PointOfInterest poi) {
			Dictionary<String, String> data = new Dictionary<string, string> ();

			data.Add ("id", poi.ID.ToString());
			data.Add ("name", poi.Name);
			data.Add ("latitude", poi.Latitude.ToString());
			data.Add ("longitude", poi.Longitude.ToString());
			data.Add ("creator", poi.Creator);
			data.Add ("description", poi.Description);
			data.Add ("category", poi.Category);

			return data;
		}

		private String getRawWebData(String Url, HttpMethod Method, Dictionary<String, String> postParameters)
		{
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create (Url);
			request.Method = Method.ToString();

			if (Method == HttpMethod.POST) {
				string postData = "";

				// prepare the post data to be sent as byte stream
				foreach (string key in postParameters.Keys) {
					postData += HttpUtility.UrlEncode (key) + "="
						+ HttpUtility.UrlEncode (postParameters [key]) + "&";
				}

				byte[] data = Encoding.ASCII.GetBytes (postData);

				request.ContentType = "application/x-www-form-urlencoded";
				request.ContentLength = data.Length;

				// send the data to the server
				Stream dataStream = request.GetRequestStream ();
				dataStream.Write (data, 0, data.Length);
				dataStream.Close ();
			}

			// request the response from the server
			HttpWebResponse response = (HttpWebResponse)request.GetResponse ();

			Console.WriteLine ("StatusCode: " + response.StatusCode.ToString ());

			// read the response data from the server
			Stream responseStream = response.GetResponseStream ();
			StreamReader reader = new StreamReader (responseStream);
			String rawData = reader.ReadToEnd ();

			return rawData;
		}

		public SortedList<Int32, PointOfInterest> getAllPOIs()
		{
			String Url = ServerUrl;
			logger.add ("Quering all POIs...", Url);

			String rawResponse = getRawWebData (Url, HttpMethod.GET, new Dictionary<String, String>());

			SortedList<Int32, PointOfInterest> pois = getPOIsFromJson(rawResponse);
			return pois;
		}

		public SortedList<Int32, PointOfInterest> getPOIsWithAttribute(String name, String value)
		{
			String Url = ServerUrl + "?" + name + "=" + value;
			logger.add ("Quering POIs with attribute: " + name + "=" + value, Url);

			String rawResponse = getRawWebData (Url, HttpMethod.GET, new Dictionary<String, String>());

			SortedList<Int32, PointOfInterest> pois = getPOIsFromJson(rawResponse);
			return pois;
		}

		public SortedList<Int32, PointOfInterest> getPOIwithId(Int32 id)
		{
			String Url = ServerUrl + "/" + id.ToString();
			logger.add ("Quering POIs with ID: " + id.ToString(), Url);

			String rawResponse = getRawWebData (Url, HttpMethod.GET, new Dictionary<String, String>());

			SortedList<Int32, PointOfInterest> pois = getPOIsFromJson(rawResponse);
			return pois;
		}

		public Int32 insertPOI(PointOfInterest poi)
		{
			String Url = ServerUrl;
			logger.add ("Insert a new POI", Url);

			// construct the json
			//String json = JsonConvert.SerializeObject (poi);

			Dictionary<String, String> data = getPostDataFromPoi (poi);
			String rawResponse = getRawWebData (Url, HttpMethod.POST, data);


			return 0;
		}

		public void updatePOI(PointOfInterest poi)
		{
			String Url = ServerUrl + "/" + poi.ID;
			logger.add ("Update a POI with ID: " + poi.ID.ToString(), Url);

			Dictionary<String, String> data = getPostDataFromPoi (poi);
			String rawResponse = getRawWebData (Url, HttpMethod.POST, data);
		}

		public void deletePOI(PointOfInterest poi)
		{
			String Url = ServerUrl + "/" + poi.ID;
			logger.add ("Delete a POI with ID: " + poi.ID.ToString(), "url");

			String rawResponse = getRawWebData (Url, HttpMethod.DELETE, new Dictionary<String, String>());
		}
	}
}

