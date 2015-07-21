using System;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace UserBrowse
{
	public class UserDatabase 
	{
		static object locker = new object ();

		SQLiteConnection db;

		public UserDatabase()
		{
			db = DependencyService.Get<ISQLite> ().GetConnection ();
			db.CreateTable<UserModel>();
		}

		public int SaveUser(UserModel model)
		{
			lock (locker) {
				return db.InsertOrReplace (model);
			}
		}
		public UserModel GetUser(int userId)
		{
			lock (locker) {
				return db.Table<UserModel> ().FirstOrDefault (x => x.UserId == userId);
			}
		}
	}
}