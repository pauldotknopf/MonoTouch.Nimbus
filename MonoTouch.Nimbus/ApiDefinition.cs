using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.Drawing;

namespace MonoTouch.Nimbus
{
	#region Core
	
	#endregion

	#region Launcher

	#region NILauncherButtonView

	#endregion

	#region NILauncherPageView

	#endregion

	#region NILauncherView

	[BaseType (typeof (UIView))]
	interface NILauncherView {
		[Export ("contentInsetForPages")]
		UIEdgeInsets ContentInsetForPages { get; set;  }
		
		[Export ("buttonSize")]
		SizeF ButtonSize { get; set;  }
		
		[Export ("numberOfRows")]
		int NumberOfRows { get; set;  }
		
		[Export ("numberOfColumns")]
		int NumberOfColumns { get; set;  }
		
		[Export ("delegate", ArgumentSemantic.Assign)][NullAllowed]
		NSObject WeakDelegate { get; set; }
		
		[Wrap ("WeakDelegate")][NullAllowed]
 		NILauncherDelegate Delegate { get; set; }
		
		[Export ("dataSource")]
		id<NILauncherDataSource> DataSource { get; set;  }
		
		[Export ("reloadData")]
		void ReloadData ();
		
		[Export ("dequeueReusableViewWithIdentifier:")]
		UIView<NILauncherButtonView> DequeueReusableViewWithIdentifier (string identifier);
		
		[Export ("willRotateToInterfaceOrientation:duration:")]
		void WillRotateToInterfaceOrientationduration (UIInterfaceOrientation toInterfaceOrientation, double duration);
		
		[Export ("willAnimateRotationToInterfaceOrientation:duration:")]
		void WillAnimateRotationToInterfaceOrientationduration (UIInterfaceOrientation toInterfaceOrientation, double duration);
		
	}
	
	[BaseType (typeof ())]
	[Model]
	interface NILauncherDataSource {
		[Abstract]
		[Export ("launcherView:numberOfButtonsInPage:")]
		int LauncherViewnumberOfButtonsInPage (NILauncherView launcherView, int page);
		
		[Abstract]
		[Export ("launcherView:buttonViewForPage:atIndex:")]
		 UIView<NILauncherButtonView> LauncherViewbuttonViewForPageatIndex (NILauncherView launcherView, int page, int index);
		
		[Abstract]
		[Export ("numberOfPagesInLauncherView:")]
		int NumberOfPagesInLauncherView (NILauncherView launcherView);
		
		[Abstract]
		[Export ("numberOfRowsPerPageInLauncherView:")]
		int NumberOfRowsPerPageInLauncherView (NILauncherView launcherView);
		
		[Abstract]
		[Export ("numberOfColumnsPerPageInLauncherView:")]
		int NumberOfColumnsPerPageInLauncherView (NILauncherView launcherView);
		
	}
	
	[BaseType (typeof (NSObject))]
	[Model]
	interface NILauncherDelegate {
		[Export ("launcherView:didSelectItemOnPage:atIndex:")]
		void LauncherViewdidSelectItemOnPageatIndex (NILauncherView launcherView, int page, int index);
		
	}
	
	[BaseType (typeof ())]
	[Model]
	interface NILauncherButtonView {
		[Abstract]
		[Export ("button")]
		UIButton Button { get; set;  }
		
	}

	#endregion

	#region NILauncherViewController

	[BaseType (typeof (UIViewController))]
	interface NILauncherViewController {
	}

	#endregion

	#region NILauncherViewModel

	#endregion

	#region NILauncherViewObject

	[BaseType (typeof (NSObject))]
	interface NILauncherViewObject {
		[Export ("initWithTitle:image:")]
		NSObject InitWithTitleimage (string title, UIImage image);
		
		[Static]
		[Export ("objectWithTitle:image:")]
		NSObject ObjectWithTitleimage (string title, UIImage image);
		
	}

	#endregion

	#endregion
}

