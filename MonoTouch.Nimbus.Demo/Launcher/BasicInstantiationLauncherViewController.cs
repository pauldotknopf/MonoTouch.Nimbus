using System;
using MonoTouch.UIKit;

namespace MonoTouch.Nimbus.Demo.Launcher
{
	public class BasicInstantiationLauncherViewController : NILauncherViewController
	{
		public BasicInstantiationLauncherViewController ()
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			View.BackgroundColor = UIColor.UnderPageBackgroundColor;
		}

		public override int NumberOfPagesInLauncherView (NILauncherView launcherView)
		{
			return 1;
		}

		public override int NumberOfRowsPerPageInLauncherView (NILauncherView launcherView)
		{
			return -1;
		}

		public override int NumberOfColumnsPerPageInLauncherView (NILauncherView launcherView)
		{
			return -1;
		}

		public override int NumberOfButtonsInPage (NILauncherView launcherView, int page)
		{
			return 4;
		}

		public override UIView ButtonViewForPage (NILauncherView launcherView, int page, int index)
		{
			NILauncherButtonView view = launcherView.DequeueReusableViewWithIdentifier ("buttonView") as NILauncherButtonView;

			if (view == null) {
				view = new NILauncherButtonView ("buttonView");
				view.Button.SetImage (UIImage.FromFile ("Images/Icon.png"), UIControlState.Normal);
				view.Label.Text = "Test";
				view.Label.Layer.ShadowColor = UIColor.Black.CGColor;
				view.Label.Layer.ShadowOffset = new System.Drawing.SizeF (0, 1);
				view.Label.Layer.Opacity = 1;
				view.Label.Layer.ShadowRadius = 1;
			}

			return view;
		}

		public override void DidSelectItemOnPage (NILauncherView launcherView, int page, int index)
		{

		}
	}
}

