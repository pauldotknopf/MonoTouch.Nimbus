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

		[Export ("numberOfPagesInPagingScrollView:")]
		public int NumberOfPagesInPagingScrollView (NIPagingScrollView pagingScrollView)
		{
			return 10;
		}

		[Export ("pagingScrollView:pageViewForIndex:")]
		public UIView PagingScrollView (NIPagingScrollView pagingScrollView, int pageIndex)
		{
			var pageView = _pagingScrollView.DequeueReusablePageWithIdentifier (pageReuseIdentifier);
			if (pageView == null) {
				pageView = new SamplePageView (pageReuseIdentifier);
			}
			return pageView;
		}

		public class SamplePageView : NIPageView
		{
			UILabel _label;

			public SamplePageView (string identifier)
				:base()
			{
				ReuseIdentifier = identifier;
				_label = new UILabel(Frame);
				_label.AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;
				_label.Font = UIFont.SystemFontOfSize(26);
				_label.TextAlignment = UITextAlignment.Center;
				_label.BackgroundColor = UIColor.Clear;
				AddSubview(_label);
			}

			public override int PageIndex {
				get {
					return base.PageIndex;
				}
				set {
					_label.Text = "This is page " + value;
					UIColor bgColor;
					UIColor textColor;
					switch(value % 4)
					{
					case 0:
						bgColor = UIColor.Red;
						textColor = UIColor.White;
						break;
					case 1:
						bgColor = UIColor.Blue;
						textColor = UIColor.White;
						break;
					case 2:
						bgColor = UIColor.Yellow;
						textColor = UIColor.Black;
						break;
					default:
						bgColor = UIColor.Green;
						textColor = UIColor.Black;
						break;
					}
					BackgroundColor = bgColor;
					_label.TextColor = textColor;
					SetNeedsLayout ();

					base.PageIndex = value;
				}
			}

			public override void PageDidDisappear ()
			{
			}

			public override NSObject FrameAndMaintainState {
				set {

				}
			}

			public override void PrepareForReuse ()
			{
			}
		}
	}
}

