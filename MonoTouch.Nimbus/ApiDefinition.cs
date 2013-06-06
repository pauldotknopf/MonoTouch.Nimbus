using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.Drawing;
using MonoTouch.CoreGraphics;
using MonoTouch.ObjCRuntime;

namespace MonoTouch.Nimbus
{
	#region Core

	//typedef void (^NIOperationBlock)(NIOperation* operation);
	public delegate void NIOperationBlock(NIOperation operation);
	//typedef void (^NIOperationDidFailBlock)(NIOperation* operation, NSError* error);
	public delegate void NIOperationDidFailBlock(NIOperation operation, NSError error);

	[BaseType (typeof (NSOperation))]
	public partial interface NIOperation {
		
		[Export ("delegate"), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap("WeakDelegate")]
		NIOperationDelegate Delegate { get; set; }

		[Export ("lastError")]
		NSError LastError { get; }
		
		[Export ("tag")]
		int Tag { get; set; }
		
		[Export ("didStartBlock")]
		NIOperationBlock DidStartBlock { get; set; }
		
		[Export ("didFinishBlock")]
		NIOperationBlock DidFinishBlock { get; set; }
		
		[Export ("didFailWithErrorBlock")]
		NIOperationDidFailBlock DidFailWithErrorBlock { get; set; }
		
		[Export ("willFinishBlock")]
		NIOperationBlock WillFinishBlock { get; set; }
		
		[Export ("didStart")]
		void DidStart ();
		
		[Export ("didFinish")]
		void DidFinish ();
		
		[Export ("didFailWithError:")]
		void DidFailWithError (NSError error);
		
		[Export ("willFinish")]
		void WillFinish ();
	}
	
	[Model]
	[BaseType(typeof(NSObject))]
	public partial interface NIOperationDelegate {
		
		[Export ("nimbusOperationDidStart:")]
		void NimbusOperationDidStart (NIOperation operation);
		
		[Export ("nimbusOperationWillFinish:")]
		void NimbusOperationWillFinish (NIOperation operation);
		
		[Export ("nimbusOperationDidFinish:")]
		void NimbusOperationDidFinish (NIOperation operation);
		
		[Export ("nimbusOperationDidFail:withError:")]
		void NimbusOperationDidFail (NIOperation operation, NSError error);
	}

	#region View Recycling

 	// An object for efficiently reusing views by recycling and dequeuing them from a pool of views.
 	// This sort of object is what UITableView and NIPagingScrollView use to recycle their views.
	// @interface NIViewRecycler : NSObject
	[BaseType (typeof (NSObject))]
	public partial interface NIViewRecycler {
		// - (UIView<NIRecyclableView> *)dequeueReusableViewWithIdentifier:(NSString *)reuseIdentifier;
		[Export ("dequeueReusableViewWithIdentifier:")]
		UIView DequeueReusableViewWithIdentifier (string reuseIdentifier);

		// - (void)recycleView:(UIView<NIRecyclableView> *)view;
		[Export ("recycleView:")]
		void RecycleView (UIView view);

		// - (void)removeAllViews;
		[Export ("removeAllViews")]
		void RemoveAllViews ();
	}

	// The protocol for a recyclable view.
	// @protocol NIRecyclableView <NSObject>
	[BaseType(typeof(NSObject), Name="NIRecyclableView")]
	[Model]
	public partial interface NIRecyclableViewProtocol {

 		// The identifier used to categorize views into buckets for reuse.
 		// Views will be reused when a new view is requested with a matching identifier.
 		// If the reuseIdentifier is nil then the class name will be used.
		// @property (nonatomic, readwrite, copy) NSString* reuseIdentifier;
		[Export ("reuseIdentifier")]
		string ReuseIdentifier { get; set; }

 		// Called immediately after the view has been dequeued from the recycled view pool.
		// - (void)prepareForReuse;
		[Export ("prepareForReuse")]
		void PrepareForReuse ();
	}

 	// A simple view implementation of the NIRecyclableView protocol.
 	// This view class can easily be used with a NIViewRecycler.
	// @interface NIRecyclableView : UIView <NIRecyclableView>
	[BaseType (typeof (UIView))]
	public partial interface NIRecyclableView : NIRecyclableViewProtocol {

		// Designated initializer.
		// - (id)initWithReuseIdentifier:(NSString *)reuseIdentifier;
		[Export ("initWithReuseIdentifier:")]
		IntPtr Constructor (string reuseIdentifier);
	}

	#endregion

	#endregion

	#region Paging

	[BaseType (typeof (NIRecyclableView))]
	public partial interface NIPageView : NIPagingScrollViewPage 
	{
		[Field ("NIPagingScrollViewUnknownNumberOfPages")]
		int NIPagingScrollViewUnknownNumberOfPages { get; }

		[Field ("NIPagingScrollViewDefaultPageMargin")]
		float NIPagingScrollViewDefaultPageMargin { get; }
	}

	[BaseType (typeof (UIView))]
	public partial interface NIPagingScrollView {

		[Export ("reloadData")]
		void ReloadData ();

		[Export ("dataSource"), NullAllowed]
		NSObject WeakDataSource { get; set; }

		[Wrap ("WeakDataSource")]
		NIPagingScrollViewDataSource DataSource { get; set; }

		[Export ("delegate"), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap("WeakDelegate")]
		UIScrollViewDelegate Delegate { get; set; }

		[Export ("dequeueReusablePageWithIdentifier:")]
		UIView DequeueReusablePageWithIdentifier (string identifier);

		[Export ("centerPageView")]
		UIView CenterPageView { get; }

		[Export ("centerPageIndex")]
		int CenterPageIndex { get; set; }

		[Export ("numberOfPages")]
		int NumberOfPages { get; }

		[Export ("pageMargin")]
		float PageMargin { get; set; }

		[Export ("type")]
		NIPagingScrollViewType Type { get; set; }

		[Export ("hasNext")]
		bool HasNext { get; }

		[Export ("hasPrevious")]
		bool HasPrevious { get; }

		[Export ("moveToNextAnimated:")]
		void MoveToNextAnimated (bool animated);

		[Export ("moveToPreviousAnimated:")]
		void MoveToPreviousAnimated (bool animated);

		[Export ("moveToPageAtIndex:animated:")]
		bool MoveToPageAtIndex (int pageIndex, bool animated);

		[Export ("willRotateToInterfaceOrientation:duration:")]
		void WillRotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation, double duration);

		[Export ("willAnimateRotationToInterfaceOrientation:duration:")]
		void WillAnimateRotationToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation, double duration);

		[Export ("pagingScrollView")]
		UIScrollView PagingScrollView { get; }

		[Export ("visiblePages")]
		NSMutableSet VisiblePages { get; }
	}

	[Category, BaseType (typeof (NIPagingScrollView))]
	public partial interface NIPagingScrollViewSubclassing {

		[Export ("willDisplayPage:")]
		void WillDisplayPage (UIView pageView);

		[Export ("didRecyclePage:")]
		void DidRecyclePage (UIView pageView);

		[Export ("didReloadNumberOfPages")]
		void DidReloadNumberOfPages ();

		[Export ("didChangeCenterPageIndexFrom:to:")]
		void DidChangeCenterPageIndexFrom (int from, int to);

		[Export ("loadPageAtIndex:")]
		UIView LoadPageAtIndex (int pageIndex);
	}

	[Model]
	public partial interface NIPagingScrollViewDataSource {

		[Export ("numberOfPagesInPagingScrollView:")]
		int NumberOfPagesInPagingScrollView (NIPagingScrollView pagingScrollView);

		[Export ("pagingScrollView:pageViewForIndex:")]
		NSObject PagingScrollView (NIPagingScrollView pagingScrollView, int pageIndex);
	}

	[Model]
	public partial interface NIPagingScrollViewDelegate {

		[Export ("pagingScrollViewDidScroll:")]
		void PagingScrollViewDidScroll (NIPagingScrollView pagingScrollView);

		[Export ("pagingScrollViewWillChangePages:")]
		void PagingScrollViewWillChangePages (NIPagingScrollView pagingScrollView);

		[Export ("pagingScrollViewDidChangePages:")]
		void PagingScrollViewDidChangePages (NIPagingScrollView pagingScrollView);
	}

	[Model]
	public partial interface NIPagingScrollViewPage {

		[Export ("pageIndex")]
		int PageIndex { get; set; }

		[Export ("pageDidDisappear")]
		void PageDidDisappear ();

		[Export ("frameAndMaintainState")]
		NSObject FrameAndMaintainState { set; }
	}

   	#endregion

	#region NetworkImage

	[Model]
	[BaseType(typeof(NSObject))]
	public partial interface NINetworkImageOperation {
		
		[Export ("cacheIdentifier")]
		string CacheIdentifier { get; }
		
		[Export ("imageCropRect")]
		RectangleF ImageCropRect { get; set; }
		
		[Export ("imageDisplaySize")]
		SizeF ImageDisplaySize { get; set; }
		
		[Export ("scaleOptions")]
		NINetworkImageViewScaleOptions ScaleOptions { get; set; }
		
		[Export ("interpolationQuality")]
		CGInterpolationQuality InterpolationQuality { get; set; }
		
		[Export ("imageContentMode")]
		UIViewContentMode ImageContentMode { get; set; }
		
		[Export ("imageCroppedAndSizedForDisplay")]
		UIImage ImageCroppedAndSizedForDisplay { get; set; }
	}

	[BaseType (typeof (UIImageView))]
	public partial interface NINetworkImageView : NIOperationDelegate {

		[Export ("delegate"), NullAllowed]
		NSObject WeakDelegate { get; set; }
		
		[Wrap("WeakDelegate")]
		NINetworkImageViewDelegate Delegate { get; set; }

		[Export ("initWithImage:")]
		IntPtr Constructor (UIImage image);
		
		[Export ("initialImage")]
		UIImage InitialImage { get; set; }
		
		[Export ("sizeForDisplay")]
		bool SizeForDisplay { get; set; }
		
		[Export ("scaleOptions")]
		NINetworkImageViewScaleOptions ScaleOptions { get; set; }
		
		[Export ("interpolationQuality")]
		CGInterpolationQuality InterpolationQuality { get; set; }
		
//		[Export ("imageMemoryCache")]
//		NIImageMemoryCache ImageMemoryCache { get; set; }
		
		[Export ("networkOperationQueue")]
		NSOperationQueue NetworkOperationQueue { get; set; }
		
		[Export ("maxAge")]
		double MaxAge { get; set; }
		
		[Export ("pathToNetworkImage")]
		string PathToNetworkImage { set; }
		
		[Export ("setPathToNetworkImage:forDisplaySize:")]
		void SetPathToNetworkImage (string pathToNetworkImage, SizeF displaySize);

		[Export ("setPathToNetworkImage:forDisplaySize:contentMode:")]
		void SetPathToNetworkImage (string pathToNetworkImage, SizeF displaySize, UIViewContentMode contentMode);
		
		[Export ("setPathToNetworkImage:forDisplaySize:contentMode:cropRect:")]
		void SetPathToNetworkImage (string pathToNetworkImage, SizeF displaySize, UIViewContentMode contentMode, RectangleF cropRect);
		
		[Export ("setPathToNetworkImage:cropRect:")]
		void SetPathToNetworkImage (string pathToNetworkImage, RectangleF cropRect);
		
		[Export ("setPathToNetworkImage:contentMode:")]
		void SetPathToNetworkImage (string pathToNetworkImage, UIViewContentMode contentMode);
		
		[Export ("setNetworkImageOperation:forDisplaySize:contentMode:cropRect:")]
		void SetNetworkImageOperation (NINetworkImageOperation operation, SizeF displaySize, UIViewContentMode contentMode, RectangleF cropRect);
		
		[Export ("loading")]
		bool Loading { [Bind ("isLoading")] get; }
		
		[Export ("prepareForReuse")]
		void PrepareForReuse ();
		
		[Export ("networkImageViewDidStartLoading")]
		void NetworkImageViewDidStartLoading ();
		
		[Export ("networkImageViewDidLoadImage:")]
		void NetworkImageViewDidLoadImage (UIImage image);
		
		[Export ("networkImageViewDidFailWithError:")]
		void NetworkImageViewDidFailWithError (NSError error);
	}

	[Model]
	[BaseType(typeof(NSObject))]
	public partial interface NINetworkImageViewDelegate  {
		
		[Export ("networkImageViewDidStartLoad:")]
		void NetworkImageViewDidStartLoad (NINetworkImageView imageView);
		
		[Export ("networkImageView:didLoadImage:")]
		void NetworkImageViewDidLoadImage (NINetworkImageView imageView, UIImage image);
		
		[Export ("networkImageView:didFailWithError:")]
		void NetworkImageViewDidFailWithError (NINetworkImageView imageView, NSError error);
		
		[Export ("networkImageView:readBytes:totalBytes:")]
		void NetworkImageViewReadBytes (NINetworkImageView imageView, long readBytes, long totalBytes);
	}

	#endregion
}

