/**
 * FH Joanneum Point-of-Interest App
 * IMS14 / SS2015 / Cloud Computing
 * Team: Florian Maderbacher, Robert Muehlberger, Robert Puerer
 * Version: 1.0
 */

using System;
using Gtk;
using System.Collections.Generic;
using FHPoiApp;

public partial class MainWindow: Gtk.Window
{	
	private FHPoiApp.Settings settings;
	private FHPoiApp.Logger logger;

	private Gtk.ListStore logStore;
	private Gtk.ListStore poiStore;

	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();

		settings = FHPoiApp.Settings.getInstance ();

		logger = FHPoiApp.Logger.getInstance ();
		logger.OnNewLoggerItem += new Logger.OnNewLoggerItemHandler(OnNewLoggerItem);

		txtSettingUrl.Text = settings.getServerUrl ();

		createLoggingTable ();
		createPoiTable ();
		createAttributesQuery ();

		logger.add ("Application started");

	}

	private void createLoggingTable()
	{
		tvLogging.AppendColumn ("Timestamp", new Gtk.CellRendererText(), "text", 0);
		tvLogging.AppendColumn ("Title", new Gtk.CellRendererText(), "text", 1);
		tvLogging.AppendColumn ("Details", new Gtk.CellRendererText(), "text", 2);

		logStore = new Gtk.ListStore (typeof(String), typeof(String), typeof(String));
		tvLogging.Model = logStore;
	}

	private void createPoiTable()
	{
		tvPOIs.AppendColumn ("ID", new Gtk.CellRendererText(), "text", 0);
		tvPOIs.AppendColumn ("Name", new Gtk.CellRendererText(), "text", 1);
		tvPOIs.AppendColumn ("Latitude", new Gtk.CellRendererText(), "text", 2);
		tvPOIs.AppendColumn ("Longitude", new Gtk.CellRendererText(), "text", 3);
		tvPOIs.AppendColumn ("Creator", new Gtk.CellRendererText(), "text", 4);
		tvPOIs.AppendColumn ("Description", new Gtk.CellRendererText(), "text", 5);
		tvPOIs.AppendColumn ("Category", new Gtk.CellRendererText(), "text", 6);

		poiStore = new Gtk.ListStore (typeof(String), typeof(String), typeof(String), typeof(String), typeof(String), typeof(String), typeof(String));
		tvPOIs.Model = poiStore;
	}

	private void createAttributesQuery()
	{
		Gtk.ListStore attributesStore = new Gtk.ListStore (typeof(String));
		attributesStore.AppendValues ("id");
		attributesStore.AppendValues ("name");
		attributesStore.AppendValues ("latitude");
		attributesStore.AppendValues ("longitude");
		attributesStore.AppendValues ("creator");
		attributesStore.AppendValues ("description");
		attributesStore.AppendValues ("category");

		cmbQueryAttributeName.Model = attributesStore;
	}

	private void OnNewLoggerItem(object sender, LoggerItem item)
	{
		Application.Invoke (delegate {
			logStore.InsertWithValues(0, item.Timestamp.ToString ("yyyy-MM-dd HH:mm:ss"), item.Subject, item.Details);
		});
	}

	private void addPOI(PointOfInterest poi)
	{
		poiStore.AppendValues (poi.ID.ToString (), poi.Name, poi.Latitude.ToString (), poi.Longitude.ToString (), poi.Creator, poi.Description, poi.Category);
	}

	private RestHandler getRestHandler()
	{
		return new RestHandler (txtSettingUrl.Text);
	}

	private void Search()
	{
		poiStore.Clear ();
		SortedList<Int32, PointOfInterest> pois = new SortedList<int, PointOfInterest> (); 

		RestHandler handler = new RestHandler (txtSettingUrl.Text);

		if (radQueryAllPOIs.Active) {
			pois = handler.getAllPOIs ();
		} else if (radQueryWithAttribute.Active) {
			String name = cmbQueryAttributeName.ActiveText;
			String value = txtQueryAttributeValue.Text;
			pois = handler.getPOIsWithAttribute (name, value);
		} else if (radQueryWithID.Active) {
			Int32 id = 0;
			if (Int32.TryParse (txtQueryID.Text, out id)) {
				pois = handler.getPOIwithId (id);
			} else {
				MessageDialog md = new MessageDialog (this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "Please input a valid ID (integer).");
				md.Title = "Error!";
				md.Run ();
				md.Destroy ();
			}
		}

		foreach (PointOfInterest poi in pois.Values) {
			addPOI (poi);
		}
	}
		
	private void InsertPOICommit()
	{
		String name = txtNewName.Text;
		Double latitude = 0;
		Double longitude = 0;
		String creator = txtNewCreator.Text;
		String description = txtNewDescription.Text;
		String category = txtNewCategory.Text;

		if (Double.TryParse (txtNewLatitude.Text, out latitude) && Double.TryParse (txtNewLongitude.Text, out longitude)) {
			PointOfInterest poi = new PointOfInterest (0, name, latitude, longitude, creator, description, category);
			RestHandler handler = getRestHandler ();
			Int32 newId = handler.insertPOI (poi);

			MessageDialog md = new MessageDialog (this, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, "New Point of Interest created.\nID: " + newId.ToString());
			md.Title = "Info!";
			md.Run ();
			md.Destroy ();

			resetNewFormular ();

			// switch to query tab
			nbTabs.CurrentPage = 0;
		} else {
			MessageDialog md = new MessageDialog (this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "Please input a valid longitude and latitude (double).");
			md.Title = "Error!";
			md.Run ();
			md.Destroy ();
		}
	}

	private void resetNewFormular()
	{
		txtNewName.Text = "";
		txtNewLatitude.Text = "";
		txtNewLongitude.Text = "";
		txtNewCreator.Text = "";
		txtNewDescription.Text = "";
		txtNewCategory.Text = "";
	}

	private void UpdatePOICommit() 
	{
		if (txtUpdateId.Text != "") {

			Int32 id = Int32.Parse (txtUpdateId.Text);
			String name = txtUpdateName.Text;
			Double latitude = 0;
			Double longitude = 0;
			String creator = txtUpdateCreator.Text;
			String description = txtUpdateDescription.Text;
			String category = txtUpdateCategory.Text;

			if (Double.TryParse (txtUpdateLatitude.Text, out latitude) && Double.TryParse (txtUpdateLongitude.Text, out longitude)) {
				PointOfInterest poi = new PointOfInterest (id, name, latitude, longitude, creator, description, category);
				RestHandler handler = getRestHandler ();
				handler.updatePOI (poi);

				MessageDialog md = new MessageDialog (this, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, "Point of Interest updated.");
				md.Title = "Info!";
				md.Run ();
				md.Destroy ();

				resetUpdateFormular ();

				// switch to query tab
				nbTabs.CurrentPage = 0;
			} else {
				MessageDialog md = new MessageDialog (this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "Please input a valid longitude and latitude (double).");
				md.Title = "Error!";
				md.Run ();
				md.Destroy ();
			}
		} else {
			MessageDialog md = new MessageDialog (this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "Please select an existing Point of Interest first.");
			md.Title = "Error!";
			md.Run ();
			md.Destroy ();
		}
	}

	private void resetUpdateFormular()
	{
		txtUpdateId.Text = "";
		txtUpdateName.Text = "";
		txtUpdateLatitude.Text = "";
		txtUpdateLongitude.Text = "";
		txtUpdateCreator.Text = "";
		txtUpdateDescription.Text = "";
		txtUpdateCategory.Text = "";
	}

	private void UpdatePOI()
	{
		TreeIter iter;
		tvPOIs.Selection.GetSelected (out iter);
		var id = poiStore.GetValue (iter, 0);

		if (id != null) {
			// prepare the formular
			txtUpdateId.Text = id.ToString ();
			txtUpdateName.Text = poiStore.GetValue (iter, 1).ToString ();
			txtUpdateLatitude.Text = poiStore.GetValue (iter, 2).ToString ();
			txtUpdateLongitude.Text = poiStore.GetValue (iter, 3).ToString ();
			txtUpdateCreator.Text = poiStore.GetValue (iter, 4).ToString ();
			txtUpdateDescription.Text = poiStore.GetValue (iter, 5).ToString ();
			txtUpdateCategory.Text = poiStore.GetValue (iter, 6).ToString ();

			// switch to update tab
			nbTabs.CurrentPage = 2;

		} else {
			MessageDialog md = new MessageDialog (this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "Please select an existing Point of Interest for editing.");
			md.Title = "Error!";
			md.Run ();
			md.Destroy ();
		}
	}

	private void DeletePOI()
	{
		TreeIter iter;
		tvPOIs.Selection.GetSelected (out iter);
		var id = poiStore.GetValue (iter, 0);

		if (id != null) {
			MessageDialog mdQuestion = new MessageDialog (this, DialogFlags.Modal, MessageType.Question, ButtonsType.YesNo, "Are you shure?");
			mdQuestion.Title = "Question!";

			ResponseType response = (ResponseType)mdQuestion.Run ();
			mdQuestion.Destroy ();

			if (response == ResponseType.Yes) {
				mdQuestion.Destroy ();

				Int32 idInt = Int32.Parse (id.ToString());
				PointOfInterest poi = new PointOfInterest (idInt, "", 0, 0, "", "", "");

				RestHandler handler = getRestHandler ();
				handler.deletePOI (poi);

				MessageDialog md = new MessageDialog (this, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, "Point of Interest deleted.");
				md.Title = "Info!";
				md.Run ();
				md.Destroy ();
			}
		} else {
			MessageDialog md = new MessageDialog (this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "Please select an existing Point of Interest for deleting.");
			md.Title = "Error!";
			md.Run ();
			md.Destroy ();
		}
	}

	#region UI-Events
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void OnBtnSearchClicked (object sender, EventArgs e)
	{
		Search ();
	}

	protected void OnBtnInsertClicked (object sender, EventArgs e)
	{
		InsertPOICommit ();
	}

	protected void OnBtnUpdateClicked (object sender, EventArgs e)
	{
		UpdatePOICommit ();
	}

	protected void OnBtnEditClicked (object sender, EventArgs e)
	{
		UpdatePOI ();
	}

	protected void OnBtnDeleteClicked (object sender, EventArgs e)
	{
		DeletePOI ();
	}
	#endregion
}
