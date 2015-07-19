using System;

namespace UserBrowse
{
	public class PostModel
	{
		public int PostId { get; set; }

		public int UserId { get; set; }

		public string ProfilePicture { get; set; }

		public string Username { get; set; }

		public string Body { get; set; }

		public DateTime PostDate { get; set; }

		public string PostDateText { 
			get { 
				return PostDate.ToLocalTime ().ToString ();
			} 
		}


	}
}

