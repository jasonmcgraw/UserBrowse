using System;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using UserBrowse;
using UserBrowse.iOS;
using UIKit;


[assembly: ExportRenderer(typeof(CircleImage), typeof(CircleImageRenderer))]
namespace UserBrowse.iOS
{
	public class CircleImageRenderer : ImageRenderer
	{

		protected override void OnElementChanged (ElementChangedEventArgs<Image> e)
		{
			base.OnElementChanged(e);
			Control.ClipsToBounds = true;
			Control.Layer.BorderColor = UIColor.White.CGColor;
			Control.Layer.BorderWidth = 2;
			Control.Layer.CornerRadius = 40;
		}


	}
}
