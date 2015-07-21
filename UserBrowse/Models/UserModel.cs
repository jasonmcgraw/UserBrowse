using System;
using SQLite;
namespace UserBrowse
{
	public class UserModel
	{
		[PrimaryKey]
		public int UserId { get; set; }

		public string Username { get; set; }

		public string Name { get; set; }

		public string Location { get; set; }

		public string About { get; set; }

		public int Plurs { get; set; }

		public int Likes { get; set; }

		public string ProfileImage { get; set; }

		public string CoverImage { get; set; }
	}
}

