using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace MonoTouch.Nimbus.Demo
{
	public class ViewRecyclingController : UIViewController
	{
		public ViewRecyclingController ()
		{
		}

		public override void ViewDidLoad ()
		{
			var recycler = new Nimbus.NIViewRecycler ();
			var sampleView = new SampleView();
			recycler.RecycleView (sampleView);
			var dequed = recycler.DequeueReusableViewWithIdentifier ("test") as SampleView;
			if (dequed.ReusedNumberOfTimes != 1) {
				throw new Exception ("This shoulf of been reused only once.");
			}
		}

		public class SampleView : NIRecyclableView
		{
			private int _reusedNumberOfTimes = 0;

			public SampleView ()
				:base("test")
			{
				
			}

			[Export("reuseIdentifier:")]
			public virtual string ReuseIdentifier{ get; set;}

			public override void PrepareForReuse ()
			{
				_reusedNumberOfTimes += 1;
			}

			public int ReusedNumberOfTimes
			{
				get
				{
					return _reusedNumberOfTimes;
				}
			}
		}
	}
}

