using System;
using SQLite;

namespace UserBrowse
{
	public interface ISQLite
	{
		SQLiteConnection GetConnection();
	}
}