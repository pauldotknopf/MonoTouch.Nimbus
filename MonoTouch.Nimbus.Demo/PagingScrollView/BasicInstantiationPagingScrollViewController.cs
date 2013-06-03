using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace MonoTouch.Nimbus.Demo
{
	public class BasicInstantiationPagingScrollViewController : UIViewController
	{
		string pageReuseIdentifier = "SamplePageIdentifier";

		NIPagingScrollView _pagingScrollView = null;

		public BasicInstantiationPagingScrollViewController ()
		{

		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			_pagingScrollView = new NIPagingScrollView ();
			_pagingScrollView.Frame = View.Frame;
			_pagingScrollView.AutoresizingMask = UIViewAutoresizing.All;
			_pagingScrollView.WeakDataSource = this;

			View.AddSubview (_pagingScrollView);

			_pagingScrollView.ReloadData ();
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			_pagingScrollView = null;
		}

		public override void WillRotate (UIInterfaceOrientation toInterfaceOrientation, double duration)
		{
			base.WillRotate (toInterfaceOrientation, duration);
			_pagingScrollView.WillRotateToInterfaceOrientation (toInterfaceOrientation, duration);
		}

		public override void WillAnimateRotation (UIInterfaceOrientation toInterfaceOrientation, double duration)
		{
			base.WillAnimateRotation (toInterfaceOrientation, duration);
			_pagingScrollView.WillAnimateRotationToInterfaceOrientation (toInterfaceOrientation, duration);
		}

		[Export ("pagingScrollView:pageViewForIndex:")]
		public int NumberOfPagesInPagingScrollView (NIPagingScrollView pagingScrollView)
		{
			return 10;
		}

		SamplePageView _testView = new SamplePageView("test");
		[Export ("numberOfPagesInPagingScrollView:")]
		public UIView PagingScrollView (NIPagingScrollView pagingScrollView, int pageIndex)
		{
			return _testView;
//			var pageView = _pagingScrollView.DequeueReusablePageWithIdentifier (pageReuseIdentifier);
//			if (pageView == null) {
//				pageView = new SamplePageView (pageReuseIdentifier);
//			}
//			return pageView;
		}

		public class SamplePageView : NIRecyclableView
		{
			// Designated initializer.
			//- (id)initWithReuseIdentifier:(NSString *)reuseIdentifier;


			public SamplePageView (string reuseIdentifier)
				:base(reuseIdentifier)
			{
			}

//			string reuseIdentifier;
//			[Export ("reuseIdentifier")]
//			public string ReuseIdentifier {
//				get {
//					return reuseIdentifier;
//				}
//				set {
//					reuseIdentifier = value;
//				}
//			}
		}
	}
}

