using System;
using Xamarin.Forms;
using SQLite;
using UserBrowse;
using UserBrowse.iOS;
using System.IO;

[assembly: Dependency (typeof (SQLite_iOS))]

namespace UserBrowse.iOS
{
	public class SQLite_iOS : ISQLite
	{
		#region ISQLite implementation
		public SQLite.SQLiteConnection GetConnection ()
		{
			var sqliteFilename = "tcdb.db3";
			string documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal); // Documents folder
			string libraryPath = Path.Combine (documentsPath, "..", "Library"); // Library folder
			var path = Path.Combine(libraryPath, sqliteFilename);

			var conn = new SQLite.SQLiteConnection(path);

			// Return the database connection 
			return conn;
		}
		#endregion
	}
}