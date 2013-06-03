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

//	#region Paging
//
//	// A skeleton implementation of a page view.
//	// This view simply implements the required properties of NIPagingScrollViewPage.
//	// @interface NIPageView : NIRecyclableView <NIPagingScrollViewPage>
//	[BaseType(typeof(UIView))] // TODO: typeof(NIRecyclableView
//	public partial interface NIPageView : NIPagingScrollViewPage {
//
//		// TODO
//		//@property (nonatomic, readwrite, assign) NSInteger pageIndex;
//		//		[Export ("pageIndex")]
//		//		int PageIndex { get; set; }
//		//
//		//		// numberOfPages will be this value until reloadData is called.
//		//		[Field ("NIPagingScrollViewUnknownNumberOfPages")]
//		//		int NIPagingScrollViewUnknownNumberOfPages { get; }
//		//
//		// 		// The default number of pixels on the side of each page.
//		// 		// Value: 10
//		//		[Field ("NIPagingScrollViewDefaultPageMargin")]
//		//		float NIPagingScrollViewDefaultPageMargin { get; }
//	}
//
// 	// A paged scroll view that shows a series of pages.
//	// @interface NIPagingScrollView : UIView <UIScrollViewDelegate>
//	// TODO: Inherit from UIScrollViewDelegate
//	[BaseType (typeof (UIView))]
//	public partial interface NIPagingScrollView  {
//
//		// - (void)reloadData;
//		[Export ("reloadData")]
//		void ReloadData ();
//
//		// @property (nonatomic, NI_WEAK) id<NIPagingScrollViewDataSource> dataSource;
//		[Export ("dataSource"), NullAllowed]
//		NSObject WeakDataSource { get; set; }
//
//		[Wrap ("WeakDataSource")]
//		NIPagingScrollViewDataSource DataSource { get; set; }
//
//		// @property (nonatomic, NI_WEAK) id<NIPagingScrollViewDelegate> delegate;
//		[Export ("delegate"), NullAllowed]
//		NSObject WeakDelegate { get; set; }
//
//		[Wrap("WeakDelegate")]
//		UIScrollViewDelegate Delegate { get; set; }
//
//		// - (UIView<NIPagingScrollViewPage> *)dequeueReusablePageWithIdentifier:(NSString *)identifier;
//		[Export ("dequeueReusablePageWithIdentifier:")]
//		UIView DequeueReusablePageWithIdentifier (string identifier);
//
//		// - (UIView<NIPagingScrollViewPage> *)centerPageView;
//		[Export ("centerPageView")]
//		IntPtr CenterPageView { get; }
//
//		// @property (nonatomic, assign) NSInteger centerPageIndex; 
//		// Use moveToPageAtIndex:animated: to animate to a given page.
//		[Export ("centerPageIndex")]
//		int CenterPageIndex { get; set; }
//
//		// @property (nonatomic, readonly, assign) NSInteger numberOfPages;
//		[Export ("numberOfPages")]
//		int NumberOfPages { get; }
//
//		// @property (nonatomic, assign) CGFloat pageMargin;
//		[Export ("pageMargin")]
//		float PageMargin { get; set; }
//
//		// @property (nonatomic, assign) NIPagingScrollViewType type; 
//		// Default: NIPagingScrollViewHorizontal
//		[Export ("type")]
//		NIPagingScrollViewType Type { get; set; }
//
//		/// <summary>
//		/// - (BOOL)hasNext;
//		/// </summary>
//		/// <value><c>true</c> if this instance has next; otherwise, <c>false</c>.</value>
//		[Export ("hasNext")]
//		bool HasNext { get; }
//
//		/// <summary>
//		/// - (BOOL)hasPrevious;
//		/// </summary>
//		[Export("hasPrevious")]
//		bool HasPrevious { get; }
//
//		/// <summary>
//		/// - (void)moveToNextAnimated:(BOOL)animated;
//		/// </summary>
//		/// <param name="animated">If set to <c>true</c> animated.</param>
//		[Export ("moveToNextAnimated:")]
//		void MoveToNextAnimated (bool animated);
//
//		/// <summary>
//		/// - (void)moveToPreviousAnimated:(BOOL)animated;
//		/// </summary>
//		/// <param name="animated">If set to <c>true</c> animated.</param>
//		[Export ("moveToPreviousAnimated:")]
//		void MoveToPreviousAnimated (bool animated);
//
//		/// <summary>
//		/// - (BOOL)moveToPageAtIndex:(NSInteger)pageIndex animated:(BOOL)animated;
//		/// </summary>
//		/// <returns><c>true</c>, if to page at index was moved, <c>false</c> otherwise.</returns>
//		/// <param name="pageIndex">Page index.</param>
//		/// <param name="animated">If set to <c>true</c> animated.</param>
//		[Export ("moveToPageAtIndex:animated:")]
//		bool MoveToPageAtIndex (int pageIndex, bool animated);
//
//		/// <summary>
//		/// - (void)willRotateToInterfaceOrientation:(UIInterfaceOrientation)toInterfaceOrientation duration:(NSTimeInterval)duration;
//		/// </summary>
//		/// <param name="toInterfaceOrientation">To interface orientation.</param>
//		/// <param name="duration">Duration.</param>
//		[Export ("willRotateToInterfaceOrientation:duration:")]
//		void WillRotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation, double duration);
//
//		/// <summary>
//		/// - (void)willAnimateRotationToInterfaceOrientation:(UIInterfaceOrientation)toInterfaceOrientation duration:(NSTimeInterval)duration;
//		/// </summary>
//		/// <param name="toInterfaceOrientation">To interface orientation.</param>
//		/// <param name="duration">Duration.</param>
//		[Export ("willAnimateRotationToInterfaceOrientation:duration:")]
//		void WillAnimateRotationToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation, double duration);
//
//		/// <summary>
//		/// @property (nonatomic, readonly, NI_STRONG) UIScrollView* pagingScrollView;
//		/// </summary>
//		/// <value>The paging scroll view.</value>
//		[Export ("pagingScrollView")]
//		UIScrollView PagingScrollView { get; }
//
//		/// <summary>
//		/// @property (nonatomic, readonly, copy) NSMutableSet* visiblePages;
//		/// Set of UIView<NIPagingScrollViewPage>*
//		/// </summary>
//		/// <value>The visible pages.</value>
//		[Export ("visiblePages")]
//		NSMutableSet VisiblePages { get; }
//	}
//
//	// @interface NIPagingScrollView (Subclassing)
//	[Category, BaseType (typeof(NIPagingScrollView))]
//	public partial interface NIPagingScrollViewSubclassing {
//
//		/// <summary>
//		/// - (void)willDisplayPage:(UIView<NIPagingScrollViewPage> *)pageView;
//		/// </summary>
//		/// <param name="pageView">Page view.</param>
//		[Export ("willDisplayPage:")]
//		void WillDisplayPage (UIView pageView);
//
//		/// <summary>
//		/// - (void)didRecyclePage:(UIView<NIPagingScrollViewPage> *)pageView;
//		/// </summary>
//		/// <param name="pageView">Page view.</param>
//		[Export ("didRecyclePage:")]
//		void DidRecyclePage (UIView pageView);
//
//		/// <summary>
//		/// - (void)didReloadNumberOfPages;
//		/// </summary>
//		[Export ("didReloadNumberOfPages")]
//		void DidReloadNumberOfPages ();
//
//		/// <summary>
//		/// - (void)didChangeCenterPageIndexFrom:(NSInteger)from to:(NSInteger)to;
//		/// </summary>
//		/// <param name="from">From.</param>
//		/// <param name="to">To.</param>
//		[Export ("didChangeCenterPageIndexFrom:to:")]
//		void DidChangeCenterPageIndexFrom (int from, int to);
//
//		/// <summary>
//		/// - (UIView<NIPagingScrollViewPage> *)loadPageAtIndex:(NSInteger)pageIndex;
//		/// </summary>
//		/// <returns>The page at index.</returns>
//		/// <param name="pageIndex">Page index.</param>
//		[Export ("loadPageAtIndex:")]
//		UIView LoadPageAtIndex (int pageIndex);
//	}
//
//	// TODO
//	//	// @interface NIPagingScrollView (ProtectedMethods)
//	//	[Category, BaseType (typeof (NIPagingScrollView))]
//	//	public partial interface NIPagingScrollViewProtectedMethods {
//	//		/// <summary>
//	//		/// - (void)setCenterPageIndexIvar:(NSInteger)centerPageIndex;
//	//		/// </summary>
//	//		/// <value>The center page index ivar.</value>
//	//		[Export ("centerPageIndexIvar")]
//	//		int CenterPageIndexIvar { set; }
//	//
//	//		/// <summary>
//	//		/// - (void)recyclePageAtIndex:(NSInteger)pageIndex;
//	//		/// </summary>
//	//		/// <param name="pageIndex">Page index.</param>
//	//		[Export ("recyclePageAtIndex:")]
//	//		void RecyclePageAtIndex (int pageIndex);
//	//
//	//		/// <summary>
//	//		/// - (void)displayPageAtIndex:(NSInteger)pageIndex;
//	//		/// </summary>
//	//		/// <param name="pageIndex">Page index.</param>
//	//		[Export ("displayPageAtIndex:")]
//	//		void DisplayPageAtIndex (int pageIndex);
//	//
//	//		/// <summary>
//	//		/// - (CGFloat)pageScrollableDimension;
//	//		/// </summary>
//	//		/// <value>The page scrollable dimension.</value>
//	//		[Export ("pageScrollableDimension")]
//	//		float PageScrollableDimension { get; }
//	//
//	//		/// <summary>
//	//		/// - (void)layoutVisiblePages;
//	//		/// </summary>
//	//		[Export ("layoutVisiblePages")]
//	//		void LayoutVisiblePages ();
//	//	}
//
//	// The data source for NIPagingScrollView.
//	// @protocol NIPagingScrollViewDataSource <NSObject>
//	[Model]
//	[BaseType(typeof(NSObject))]
//	public partial interface NIPagingScrollViewDataSource {
//
//		/// <summary>
//		/// Fetches the total number of pages in the scroll view.
//		/// The value returned in this method will be cached by the scroll view until reloadData
//		/// is called again.
//		/// - (NSInteger)numberOfPagesInPagingScrollView:(NIPagingScrollView *)pagingScrollView;
//		/// </summary>
//		/// <returns>The of pages in paging scroll view.</returns>
//		/// <param name="pagingScrollView">Paging scroll view.</param>
//		[Export ("numberOfPagesInPagingScrollView:")]
//		int NumberOfPagesInPagingScrollView (NIPagingScrollView pagingScrollView);
//
//		/// <summary>
//		/// Fetches a page that will be displayed at the given page index.
//		/// You should always try to reuse pages by calling dequeueReusablePageWithIdentifier: on the 
//		/// paging scroll view before allocating a new page.
//		/// - (UIView<NIPagingScrollViewPage> *)pagingScrollView:(NIPagingScrollView *)pagingScrollView pageViewForIndex:(NSInteger)pageIndex;
//		/// </summary>
//		/// <returns>The scroll view.</returns>
//		/// <param name="pagingScrollView">Paging scroll view.</param>
//		/// <param name="pageIndex">Page index.</param>
//		[Export ("pagingScrollView:pageViewForIndex:")]
//		NSObject PagingScrollView (NIPagingScrollView pagingScrollView, int pageIndex);
//	}
//
//	// @protocol NIPagingScrollViewDelegate <UIScrollViewDelegate>
//	// TODO: Inherit from the UIScrollViewDelegate
//	[Model]
//	[BaseType(typeof(NSObject))]
//	public partial interface NIPagingScrollViewDelegate {
//
// 		// The user is scrolling between two photos.
//		// - (void)pagingScrollViewDidScroll:(NIPagingScrollView *)pagingScrollView;
//		[Export ("pagingScrollViewDidScroll:")]
//		void PagingScrollViewDidScroll (NIPagingScrollView pagingScrollView);
//
//  		// The current page will change.
// 		// pagingScrollView.centerPageIndex will reflect the old page index, not the new
// 		// page index.
//		// - (void)pagingScrollViewWillChangePages:(NIPagingScrollView *)pagingScrollView;
//		[Export ("pagingScrollViewWillChangePages:")]
//		void PagingScrollViewWillChangePages (NIPagingScrollView pagingScrollView);
//
// 		// The current page has changed.
// 		// pagingScrollView.centerPageIndex will reflect the changed page index.
//		// - (void)pagingScrollViewDidChangePages:(NIPagingScrollView *)pagingScrollView;
//		[Export ("pagingScrollViewDidChangePages:")]
//		void PagingScrollViewDidChangePages (NIPagingScrollView pagingScrollView);
//	}
//
//	// @protocol NIPagingScrollViewPage <NIRecyclableView>
//	// TODO: Inherit from the recyclable view protocol
//	[Model]
//	public partial interface NIPagingScrollViewPage {
//
//		[Export ("pageIndex")]
//		int PageIndex { get; set; }
//
//		[Export ("pageDidDisappear")]
//		void PageDidDisappear ();
//
//		[Export ("frameAndMaintainState")]
//		NSObject FrameAndMaintainState { set; }
//	}
//
//	#endregion

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

