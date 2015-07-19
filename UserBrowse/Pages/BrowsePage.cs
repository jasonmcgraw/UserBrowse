using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Diagnostics;

namespace UserBrowse
{
	public class BrowsePage : ContentPage
	{
		StackLayout layout;
		ActivityIndicator indicator;
		ListView userListView;

		public BrowsePage ()
		{
			layout = new StackLayout {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
			};
			Content = layout;

			GetContent ();
		}
		public async void GetContent()
		{
			indicator = new ActivityIndicator {
				IsRunning = true,
				Color = Color.Black,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand
			};


			layout.Children.Add (indicator);

			List<UserListModel> userList = await UserControl.GetUserListAsync ();

			layout.Children.Remove (indicator);

			userListView = new ListView {
				ItemsSource = userList,
				ItemTemplate = new DataTemplate (typeof(UserCell)),
				RowHeight = 80
			};
			userListView.ItemTapped += (object sender, ItemTappedEventArgs e) => {
				UserListModel cell = e.Item as UserListModel;
				Navigation.PushAsync(new UserPage(cell.UserId));
				userListView.SelectedItem = null;
			};
		
			layout.Children.Add (userListView);
		}
	}
}

