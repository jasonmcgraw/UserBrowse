using System;
using Xamarin.Forms;

namespace UserBrowse
{
	public class UserCell : ViewCell
	{
		StackLayout layout;
		Image profileImage;
		Label usernameLabel;

		public UserCell ()
		{
		    layout = new StackLayout {
				Orientation = StackOrientation.Horizontal,
				Padding = 5
			};

			profileImage = new Image { 
				HorizontalOptions = LayoutOptions.Start ,
			};
			profileImage.SetBinding (Image.SourceProperty, new Binding ("ProfileImage"));

			usernameLabel = new Label {
				FontFamily = "AvenirNext-Regular",
				FontSize = 14,
				HorizontalOptions = LayoutOptions.Start,
				VerticalOptions = LayoutOptions.CenterAndExpand
			};
			usernameLabel.SetBinding (Label.TextProperty, "Username");

			layout.Children.Add (profileImage);
			layout.Children.Add (usernameLabel);

			View = layout;
		}


	}
}

