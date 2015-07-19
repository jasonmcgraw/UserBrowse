using System;
using Xamarin.Forms;

namespace UserBrowse
{
	public class UserPage : ContentPage
	{
		ActivityIndicator indicator;

		StackLayout mainLayout;
		StackLayout infoLayout;
		StackLayout likeLayout;

		RelativeLayout imageLayout;

		CircleImage profileImage;
		Image coverImage;

		Label aboutLabel;
		Label aboutContentLabel;
		Label usernameLabel;
		Label nameLabel;
		Label locationLabel;
		Label likeLabel;
		Label plurLabel;

		UserModel model;

		string font = "Helvetica";
		string fontBold = "Helvetica-Bold";

		int userId;

		public UserPage (int userId)
		{
			this.userId = userId;

			mainLayout = new StackLayout ();

			Content = mainLayout;

			CreateProfile ();
		}
		public async void CreateProfile()
		{
			indicator = new ActivityIndicator {
				IsRunning = true,
				Color = Color.Black,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand
			};
			mainLayout.Children.Add (indicator);

			model = await UserControl.GetUserAsync (userId);

			mainLayout.Children.Remove (indicator);

			CreateImageLayout ();
			CreateInfoLayout ();

		}
		public void CreateImageLayout ()
		{
			imageLayout = new RelativeLayout {
				HeightRequest = 230,
				VerticalOptions = LayoutOptions.Start,
				HorizontalOptions = LayoutOptions.FillAndExpand,
			};
			coverImage = new Image { 
				Source = model.CoverImage,
				Aspect = Aspect.AspectFill
			};
			profileImage = new CircleImage { 
				Source = model.ProfileImage,
				WidthRequest = 80,
				HeightRequest = 80
			};
			imageLayout.Children.Add(coverImage,
				Constraint.Constant(0),
				Constraint.Constant(0),
				Constraint.RelativeToParent((p) => {
					return p.Width;
				}),
				Constraint.RelativeToParent((p) => {
					return imageLayout.HeightRequest;
				})
			);
			imageLayout.Children.Add(profileImage,
				Constraint.RelativeToParent((p) => {
					return (p.Width - profileImage.WidthRequest) / 2;
				}),
				Constraint.RelativeToParent((p) => {
					return (imageLayout.HeightRequest - profileImage.HeightRequest) + 20;
				})
			);

			mainLayout.Children.Add (imageLayout);
		}
		public void CreateInfoLayout()
		{
			infoLayout = new StackLayout {
				Padding = 20
			};

			usernameLabel = new Label {
				Text = model.Username,
				FontFamily = font,
				FontSize = 19
			};
			nameLabel = new Label {
				Text = model.Name,
				FontFamily = font,
				TextColor = Color.FromRgb(140,140,140),
				FontSize = 14
			};
			locationLabel = new Label {
				Text = model.Location,
				FontFamily = font,
				FontSize = 14,
				XAlign = TextAlignment.End,
				HorizontalOptions = LayoutOptions.FillAndExpand
			};
			aboutLabel = new Label {
				Text = "About",
				FontFamily = fontBold,
				FontSize = 14
			};
			aboutContentLabel = new Label {
				Text = model.About,
				FontFamily = font,
				FontSize = 14
			};
			likeLabel = new Label {
				Text = model.Likes + " Likes",
				FontFamily = font,
				FontSize = 14,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				XAlign = TextAlignment.Center
			};
			plurLabel = new Label {
				Text = model.Plurs + " PLUR",
				FontFamily = font,
				FontSize = 14,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				XAlign = TextAlignment.Center
			};
			likeLayout = new StackLayout {
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Padding = 20,
				Children = { likeLabel, plurLabel }
			};
			infoLayout.Children.Add (usernameLabel);
			infoLayout.Children.Add (nameLabel);
			infoLayout.Children.Add (locationLabel);
			infoLayout.Children.Add (aboutLabel);
			infoLayout.Children.Add (aboutContentLabel);
			infoLayout.Children.Add (likeLayout);

			mainLayout.Children.Add (infoLayout);

		}
	
	}
}

