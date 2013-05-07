using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Nimbus;
using System.Drawing;


namespace MonoTouch.Nimbus.Demo
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// User Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		UIWindow window;

		//
		// This method is invoked when the application has loaded and is ready to run. In this 
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			// create a new window instance based on the screen size
			window = new UIWindow (UIScreen.MainScreen.Bounds);
			
			// If you have defined a root view controller, set it here:
			// window.RootViewController = myViewController;

			var image = UIImage.FromFile("Images/Icon.png");

			var imageView = new NINetworkImageView(image);
			imageView.Delegate = new NetworkDelegate();
			imageView.Frame = window.Frame;
			window.AddSubview(imageView);
			imageView.SetPathToNetworkImage("http://www.google.com/images/srpr/logo4w.png", new SizeF(window.Frame.Width, window.Frame.Height), UIViewContentMode.ScaleAspectFit);
			window.MakeKeyAndVisible ();
			
			return true;
		}

		public class NetworkDelegate : NINetworkImageViewDelegate
		{
			public override void NetworkImageViewDidFailWithError (NINetworkImageView imageView, NSError error)
			{

			}

			public override void NetworkImageViewDidLoadImage (NINetworkImageView imageView, UIImage image)
			{

			}

			public override void NetworkImageViewDidStartLoad (NINetworkImageView imageView)
			{

			}

			public override void NetworkImageViewReadBytes (NINetworkImageView imageView, long readBytes, long totalBytes)
			{

			}
		}
	}
}

