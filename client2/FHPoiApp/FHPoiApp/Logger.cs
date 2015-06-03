/**
 * FH Joanneum Point-of-Interest App
 * IMS14 / SS2015 / Cloud Computing
 * Team: Florian Maderbacher, Robert Muehlberger, Robert Puerer
 * Version: 1.0
 */

using System;

namespace FHPoiApp
{
	public class Logger
	{
		private static Logger INSTANCE;

		public static Logger getInstance()
		{
			if (Logger.INSTANCE == null) {
				Logger.INSTANCE = new Logger ();
				Logger.INSTANCE.Init ();
			}
			return Logger.INSTANCE;
		}

		public delegate void OnNewLoggerItemHandler(object sender, LoggerItem item);
		public event OnNewLoggerItemHandler OnNewLoggerItem;

		private void OnNewLoggerItemDelegate(LoggerItem item)
		{
			if (OnNewLoggerItem != null) {
				OnNewLoggerItem (this, item);
			}
		}

		private Logger ()
		{
			// empty constructor
		}

		private void Init()
		{

		}

		public void add(String subject, String details)
		{
			LoggerItem item = new LoggerItem (subject, details);

			String logLine = item.Timestamp.ToString ("yyyy-MM-dd HH:mm:ss.fff") + " - " + item.Subject;
			if (item.Details != "")
				logLine += " - " + item.Details;

			Console.WriteLine (logLine);

			// raise the event
			OnNewLoggerItemDelegate (item);
		}

		public void add(String subject)
		{
			add (subject, "");
		}
	}
}

