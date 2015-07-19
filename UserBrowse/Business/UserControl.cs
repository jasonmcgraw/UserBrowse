using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace UserBrowse
{
	public static class UserControl
	{
		public static async Task<List<UserListModel>> GetUserListAsync()
		{
			string result = await WebControl.MakeGetRequestAsync (WebConstants.UserListPath);

			List<UserListModel> model = JsonConvert.DeserializeObject<List<UserListModel>> (result);

			return model;
		}
		public static async Task<UserModel> GetUserAsync(int userId)
		{
			string path = string.Format (WebConstants.ProfilePath, userId);
			string result = await WebControl.MakeGetRequestAsync (path);

			UserModel model = JsonConvert.DeserializeObject<UserModel> (result);

			return model;
		}
	}
}

