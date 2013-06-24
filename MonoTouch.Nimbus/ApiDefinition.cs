using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.Drawing;
using MonoTouch.CoreGraphics;
using MonoTouch.ObjCRuntime;
using MonoTouch.CoreFoundation;

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

	#region InMemoryCache

	[BaseType (typeof (NSObject))]
	public partial interface NIMemoryCache {

		[Export ("initWithCapacity:")]
		IntPtr Constructor (uint capacity);

		[Export ("count")]
		uint Count { get; }

		[Export ("storeObject:withName:")]
		void StoreObject (NSObject item, string name);

		[Export ("storeObject:withName:expiresAfter:")]
		void StoreObject (NSObject item, string name, NSDate expirationDate);

		[Export ("removeObjectWithName:")]
		void RemoveObjectWithName (string name);

		[Export ("removeAllObjectsWithPrefix:")]
		void RemoveAllObjectsWithPrefix (string prefix);

		[Export ("removeAllObjects")]
		void RemoveAllObjects ();

		[Export ("objectWithName:")]
		NSObject ObjectWithName (string name);

		[Export ("containsObjectWithName:")]
		bool ContainsObjectWithName (string name);

		[Export ("dateOfLastAccessWithName:")]
		NSDate DateOfLastAccessWithName (string name);

		[Export ("nameOfLeastRecentlyUsedObject")]
		string NameOfLeastRecentlyUsedObject { get; }

		[Export ("nameOfMostRecentlyUsedObject")]
		string NameOfMostRecentlyUsedObject { get; }

		[Export ("reduceMemoryUsage")]
		void ReduceMemoryUsage ();

		[Export ("willSetObject:withName:previousObject:")]
		bool WillSetObject (NSObject item, string name, NSObject previousObject);

		[Export ("didSetObject:withName:")]
		void DidSetObject (NSObject item, string name);

		[Export ("willRemoveObject:withName:")]
		void WillRemoveObject (NSObject item, string name);
	}

	[BaseType (typeof (NIMemoryCache))]
	public partial interface NIImageMemoryCache {

		[Export ("numberOfPixels")]
		uint NumberOfPixels { get; }

		[Export ("maxNumberOfPixels")]
		uint MaxNumberOfPixels { get; set; }

		[Export ("maxNumberOfPixelsUnderStress")]
		uint MaxNumberOfPixelsUnderStress { get; set; }
	}

	#endregion

	#endregion

	#region PagingScrollView

	[BaseType (typeof (NIRecyclableView))]
	public partial interface NIPageView : NIPagingScrollViewPage 
	{
		// TODO: Bug somewhere in btouch. This fields were causing a build error. http://stackoverflow.com/questions/16952194/monotouch-binding-project-build-errors
		//[Field ("NIPagingScrollViewUnknownNumberOfPages")]
		//int NIPagingScrollViewUnknownNumberOfPages { get; }

		//[Field ("NIPagingScrollViewDefaultPageMargin")]
		//float NIPagingScrollViewDefaultPageMargin { get; }
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
	[BaseType(typeof(NSObject))]
	public partial interface NIPagingScrollViewDataSource {

		[Export ("numberOfPagesInPagingScrollView:"), Abstract]
		int NumberOfPagesInPagingScrollView (NIPagingScrollView pagingScrollView);

		[Export ("pagingScrollView:pageViewForIndex:"), Abstract]
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
		
		[Export ("imageMemoryCache")]
		NIImageMemoryCache ImageMemoryCache { get; set; }
		
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

	#region Photos

	///@interface NIPhotoAlbumScrollView : NIPagingScrollView <NIPhotoScrollViewDelegate> 
	[BaseType(typeof(NIPagingScrollView))]
	public partial interface NIPhotoAlbumScrollView : NIPhotoScrollViewDelegate {

		//- (UIView<NIPagingScrollViewPage> *)pagingScrollView:(NIPagingScrollView *)pagingScrollView pageViewForIndex:(NSInteger)pageIndex;
		[Export ("pagingScrollView:pageViewForIndex:")]
		UIView PagingScrollView (NSObject pagingScrollView, int pageIndex);

		//@property (nonatomic, readwrite, NI_WEAK) id<NIPhotoAlbumScrollViewDataSource> dataSource;
		[Export ("dataSource"), NullAllowed]
		NSObject WeakDataSource { get; set; }

		[Wrap ("WeakDataSource")]
		NIPhotoAlbumScrollViewDataSource DataSource { get; set; }

		//@property (nonatomic, readwrite, NI_WEAK) id<NIPhotoAlbumScrollViewDelegate> delegate;
		[Export ("delegate"), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap("WeakDelegate")]
		NIPhotoAlbumScrollViewDelegate Delegate { get; set; }

		//@property (nonatomic, readwrite, assign, getter=isZoomingEnabled) BOOL zoomingIsEnabled;
		[Export ("zoomingIsEnabled")]
		bool ZoomingIsEnabled { [Bind ("isZoomingEnabled")] get; set; }

		//@property (nonatomic, readwrite, assign, getter=isZoomingAboveOriginalSizeEnabled) BOOL zoomingAboveOriginalSizeIsEnabled;
		[Export ("zoomingAboveOriginalSizeIsEnabled")]
		bool ZoomingAboveOriginalSizeIsEnabled { [Bind ("isZoomingAboveOriginalSizeEnabled")] get; set; }

		//@property (nonatomic, readwrite, NI_STRONG) UIColor* photoViewBackgroundColor;
		[Export ("photoViewBackgroundColor")]
		UIColor PhotoViewBackgroundColor { get; set; }

		//@property (nonatomic, readwrite, NI_STRONG) UIImage* loadingImage;
		[Export ("loadingImage")]
		UIImage LoadingImage { get; set; }

		//- (void)didLoadPhoto: (UIImage *)image atIndex: (NSInteger)photoIndex photoSize: (NIPhotoScrollViewPhotoSize)photoSize;
		[Export ("didLoadPhoto:atIndex:photoSize:")]
		void DidLoadPhoto (UIImage image, int photoIndex, NIPhotoScrollViewPhotoSize photoSize);
	}

	//@protocol NIPhotoAlbumScrollViewDataSource <NIPagingScrollViewDataSource>
	[Model]
	[BaseType(typeof(NSObject))]
	public partial interface NIPhotoAlbumScrollViewDataSource : NIPagingScrollViewDataSource {

		//- (UIImage *)photoAlbumScrollView: (NIPhotoAlbumScrollView *)photoAlbumScrollView
		//	photoAtIndex: (NSInteger)photoIndex
		//		photoSize: (NIPhotoScrollViewPhotoSize *)photoSize
		//		isLoading: (BOOL *)isLoading
		//		originalPhotoDimensions: (CGSize *)originalPhotoDimensions;
		[Export ("photoAlbumScrollView:photoAtIndex:photoSize:isLoading:originalPhotoDimensions:"), Abstract]
		NSObject PhotoAlbumScrollView (NIPhotoAlbumScrollView photoAlbumScrollView, int photoIndex, out NIPhotoScrollViewPhotoSize photoSize, out bool isLoading, out SizeF originalPhotoDimensions);

		//- (void)photoAlbumScrollView: (NIPhotoAlbumScrollView *)photoAlbumScrollView
		//	stopLoadingPhotoAtIndex: (NSInteger)photoIndex;
		[Export ("photoAlbumScrollView:stopLoadingPhotoAtIndex:")]
		void PhotoAlbumScrollView (NIPhotoAlbumScrollView photoAlbumScrollView, int photoIndex);
	}

	//@protocol NIPhotoAlbumScrollViewDelegate <NIPagingScrollViewDelegate>
	[Model]
	[BaseType(typeof(NSObject))]
	public partial interface NIPhotoAlbumScrollViewDelegate : NIPagingScrollViewDelegate {

		//- (void)photoAlbumScrollView: (NIPhotoAlbumScrollView *)photoAlbumScrollView
		//	didZoomIn: (BOOL)didZoomIn;
		[Export ("photoAlbumScrollView:didZoomIn:")]
		void PhotoAlbumScrollView (NIPhotoAlbumScrollView photoAlbumScrollView, bool didZoomIn);

		//- (void)photoAlbumScrollViewDidLoadNextPhoto:(NIPhotoAlbumScrollView *)photoAlbumScrollView;
		[Export ("photoAlbumScrollViewDidLoadNextPhoto:")]
		void PhotoAlbumScrollViewDidLoadNextPhoto (NIPhotoAlbumScrollView photoAlbumScrollView);

		//- (void)photoAlbumScrollViewDidLoadPreviousPhoto:(NIPhotoAlbumScrollView *)photoAlbumScrollView;
		[Export ("photoAlbumScrollViewDidLoadPreviousPhoto:")]
		void PhotoAlbumScrollViewDidLoadPreviousPhoto (NIPhotoAlbumScrollView photoAlbumScrollView);
	}

	//@interface NIPhotoScrollView : UIView <
	//	UIScrollViewDelegate,
	//	NIPagingScrollViewPage> 
	[BaseType (typeof (UIView))]
	public partial interface NIPhotoScrollView { //: UIScrollViewDelegate {

		//@property (nonatomic, readwrite, assign, getter=isZoomingEnabled) BOOL zoomingIsEnabled; // default: yes
		[Export ("zoomingIsEnabled")]
		bool ZoomingIsEnabled { [Bind ("isZoomingEnabled")] get; set; }

		//@property (nonatomic, readwrite, assign, getter=isZoomingAboveOriginalSizeEnabled) BOOL zoomingAboveOriginalSizeIsEnabled; // default: yes
		[Export ("zoomingAboveOriginalSizeIsEnabled")]
		bool ZoomingAboveOriginalSizeIsEnabled { [Bind ("isZoomingAboveOriginalSizeEnabled")] get; set; }

		//@property (nonatomic, readwrite, assign, getter=isDoubleTapToZoomEnabled) BOOL doubleTapToZoomIsEnabled; // default: yes
		[Export ("doubleTapToZoomIsEnabled")]
		bool DoubleTapToZoomIsEnabled { [Bind ("isDoubleTapToZoomEnabled")] get; set; }

		//@property (nonatomic, readwrite, assign) CGFloat maximumScale; // default: 0 (autocalculate)
		[Export ("maximumScale")]
		float MaximumScale { get; set; }

		//@property (nonatomic, readwrite, NI_WEAK) id<NIPhotoScrollViewDelegate> photoScrollViewDelegate;
		[Export ("photoScrollViewDelegate"), NullAllowed]
		NSObject WeakPhotoScrollViewDelegate { get; set; }

		[Wrap ("WeakPhotoScrollViewDelegate")]
		NIPhotoScrollViewDelegate PhotoScrollViewDelegate { get; set; }

		//- (UIImage *)image;
		[Export ("image")]
		UIImage Image { get; }

		//- (NIPhotoScrollViewPhotoSize)photoSize;
		[Export ("photoSize")]
		NIPhotoScrollViewPhotoSize PhotoSize { get; }

		//- (void)setImage:(UIImage *)image photoSize:(NIPhotoScrollViewPhotoSize)photoSize;
		[Export ("setImage:photoSize:")]
		void SetImage (UIImage image, NIPhotoScrollViewPhotoSize photoSize);

		//@property (nonatomic, assign, getter = isLoading) BOOL loading;
		[Export ("loading")]
		bool Loading { [Bind ("isLoading")] get; set; }

		//@property (nonatomic, readwrite, assign) NSInteger pageIndex;
		[Export ("pageIndex")]
		int PageIndex { get; set; }

		//@property (nonatomic, readwrite, assign) CGSize photoDimensions;
		[Export ("photoDimensions")]
		SizeF PhotoDimensions { get; set; }

		//@property (nonatomic, readonly, NI_STRONG) UITapGestureRecognizer* doubleTapGestureRecognizer;
		[Export ("doubleTapGestureRecognizer")]
		UITapGestureRecognizer DoubleTapGestureRecognizer { get; }
	}

	//@protocol NIPhotoScrollViewDelegate <NSObject>
	[Model]
	[BaseType(typeof(NSObject))]
	public partial interface NIPhotoScrollViewDelegate {

		//- (void)photoScrollViewDidDoubleTapToZoom: (NIPhotoScrollView *)photoScrollView
		//	didZoomIn: (BOOL)didZoomIn;
		[Export ("photoScrollViewDidDoubleTapToZoom:didZoomIn:")]
		void PhotoScrollViewDidDoubleTapToZoom (NIPhotoScrollView photoScrollView, bool didZoomIn);
	}

	//@interface NIPhotoScrubberView : UIView
	[BaseType (typeof (UIView))]
	public partial interface NIPhotoScrubberView {

		//@property (nonatomic, readwrite, NI_WEAK) id<NIPhotoScrubberViewDataSource> dataSource;
		[Export ("dataSource"), NullAllowed]
		NSObject WeakDataSource { get; set; }

		[Wrap ("WeakDataSource")]
		NIPhotoScrubberViewDataSource DataSource { get; set; }

		//@property (nonatomic, readwrite, assign) id<NIPhotoScrubberViewDelegate> delegate;
		[Export ("delegate"), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap("WeakDelegate")]
		NIPhotoScrubberViewDelegate Delegate { get; set; }

		//- (void)reloadData;
		[Export ("reloadData")]
		void ReloadData ();

		//- (void)didLoadThumbnail: (UIImage *)image
		//	atIndex: (NSInteger)photoIndex;
		[Export ("didLoadThumbnail:atIndex:")]
		void DidLoadThumbnail (UIImage image, int photoIndex);

		//@property (nonatomic, readwrite, assign) NSInteger selectedPhotoIndex;
		[Export ("selectedPhotoIndex")]
		int SelectedPhotoIndex { get; set; }

		//- (void)setSelectedPhotoIndex:(NSInteger)photoIndex animated:(BOOL)animated;
		[Export ("setSelectedPhotoIndex:animated:")]
		void SetSelectedPhotoIndex (int photoIndex, bool animated);
	}

	//@protocol NIPhotoScrubberViewDataSource <NSObject>
	[Model]
	[BaseType(typeof(NSObject))]
	public partial interface NIPhotoScrubberViewDataSource {

		//- (NSInteger)numberOfPhotosInScrubberView:(NIPhotoScrubberView *)photoScrubberView;
		[Export ("numberOfPhotosInScrubberView:"), Abstract]
		int NumberOfPhotosInScrubberView (NIPhotoScrubberView photoScrubberView);

		//- (UIImage *)photoScrubberView: (NIPhotoScrubberView *)photoScrubberView
		//	thumbnailAtIndex: (NSInteger)thumbnailIndex;
		[Export ("photoScrubberView:thumbnailAtIndex:"), Abstract]
		UIImage PhotoScrubberView (NIPhotoScrubberView photoScrubberView, int thumbnailIndex);
	}

	//@protocol NIPhotoScrubberViewDelegate <NSObject>
	[Model]
	[BaseType(typeof(NSObject))]
	public partial interface NIPhotoScrubberViewDelegate {

		//- (void)photoScrubberViewDidChangeSelection:(NIPhotoScrubberView *)photoScrubberView;
		[Export ("photoScrubberViewDidChangeSelection:")]
		void PhotoScrubberViewDidChangeSelection (NIPhotoScrubberView photoScrubberView);
	}

	//@interface NIToolbarPhotoViewController : UIViewController <
	//	NIPhotoAlbumScrollViewDelegate,
	//	NIPhotoScrubberViewDelegate > 
	[BaseType (typeof (UIViewController))]
	public partial interface NIToolbarPhotoViewController : NIPhotoAlbumScrollViewDelegate, NIPhotoScrubberViewDelegate {

		//@property (nonatomic, readwrite, assign, getter=isToolbarTranslucent) BOOL toolbarIsTranslucent; // default: yes
		[Export ("toolbarIsTranslucent")]
		bool ToolbarIsTranslucent { [Bind ("isToolbarTranslucent")] get; set; }

		//@property (nonatomic, readwrite, assign) BOOL hidesChromeWhenScrolling; // default: yes
		[Export ("hidesChromeWhenScrolling")]
		bool HidesChromeWhenScrolling { get; set; }

		//@property (nonatomic, readwrite, assign) BOOL chromeCanBeHidden; // default: yes
		[Export ("chromeCanBeHidden")]
		bool ChromeCanBeHidden { get; set; }

		//@property (nonatomic, readwrite, assign) BOOL animateMovingToNextAndPreviousPhotos; // default: no
		[Export ("animateMovingToNextAndPreviousPhotos")]
		bool AnimateMovingToNextAndPreviousPhotos { get; set; }

		//@property (nonatomic, readwrite, assign, getter=isScrubberEnabled) BOOL scrubberIsEnabled; // default: ipad yes - iphone no
		[Export ("scrubberIsEnabled")]
		bool ScrubberIsEnabled { [Bind ("isScrubberEnabled")] get; set; }

		//@property (nonatomic, readonly, NI_STRONG) UIToolbar* toolbar;
		[Export ("toolbar")]
		UIToolbar Toolbar { get; }

		//@property (nonatomic, readonly, NI_STRONG) NIPhotoAlbumScrollView* photoAlbumView;
		[Export ("photoAlbumView")]
		NIPhotoAlbumScrollView PhotoAlbumView { get; }

		//@property (nonatomic, readonly, NI_STRONG) NIPhotoScrubberView* photoScrubberView;
		[Export ("photoScrubberView")]
		NIPhotoScrubberView PhotoScrubberView { get; }

		//- (void)refreshChromeState;
		[Export ("refreshChromeState")]
		void RefreshChromeState ();

		//@property (nonatomic, readonly, NI_STRONG) UIBarButtonItem* nextButton;
		[Export ("nextButton")]
		UIBarButtonItem NextButton { get; }

		//@property (nonatomic, readonly, NI_STRONG) UIBarButtonItem* previousButton;
		[Export ("previousButton")]
		UIBarButtonItem PreviousButton { get; }

		//- (void)setChromeVisibility:(BOOL)isVisible animated:(BOOL)animated;
		[Export ("setChromeVisibility:animated:")]
		void SetChromeVisibility (bool isVisible, bool animated);

		//- (void)setChromeTitle;
		[Export ("setChromeTitle")]
		void SetChromeTitle ();
	}

	#endregion

	#region AFNetworking

	#region Delegates

	//(void (^)(AFHTTPRequestOperation *operation, id responseObject))
	public delegate void AFHTTPClientRequestSuccess(AFHTTPRequestOperation operation, NSObject responseObject);
	//(void (^)(AFHTTPRequestOperation *operation, NSError *error))
	public delegate void AFHTTPClientRequestFailure(AFHTTPRequestOperation operation, NSError error);
	//(void (^)(id <AFMultipartFormData> formData))
	public delegate void AFHTTPClientMultipartFormRequestWithMethodBlock(AFMultipartFormData formData);
	//(void (^)(NSUInteger numberOfFinishedOperations, NSUInteger totalNumberOfOperations))
	public delegate void AFHTTPClientProgress(UINavigationItem numberOfFinishedOperations, uint totalNumberOfOperations);
	//(void (^)(NSArray *operations))
	public delegate void AFHTTPClientCompletion(NSArray operations);
	//(void (^)(UIImage *image))
	public delegate void ImageRequestOperationWithRequestSuccess1(UIImage image);
	//(void (^)(NSURLRequest *request, NSHTTPURLResponse *response, UIImage *image))
	public delegate void ImageRequestOperationWithRequestSuccess2(NSUrlRequest request, NSHttpUrlResponse response, UIImage image);
	//(void (^)(NSURLRequest *request, NSHTTPURLResponse *response, NSError *error))
	public delegate void ImageRequestOperationWithRequestFailure(NSUrlRequest request, NSHttpUrlResponse response, NSError error);
	//(UIImage *(^)(UIImage *image))
	public delegate UIImage ImageRequestOperationWithRequestProcessingBlock(UIImage image);
	//(void (^)(NSURLRequest *request, NSHTTPURLResponse *response, id JSON))
	public delegate void AFJSONRequestOperationJsonRequestOperationWithRequestSuccess(NSUrlRequest request, NSHttpUrlResponse response, NSObject json);
	//(void (^)(NSURLRequest *request, NSHTTPURLResponse *response, NSError *error, id JSON))
	public delegate void AFJSONRequestOperationJsonRequestOperationWithRequestFailure(NSUrlRequest request, NSHttpUrlResponse response, NSError error, NSObject json);
	//(void (^)(NSURLRequest *request, NSHTTPURLResponse *response, id propertyList))
	public delegate void AFPropertyListRequestOperationSuccess(NSUrlRequest request, NSHttpUrlResponse response, NSObject propertyList);
	//(void (^)(NSURLRequest *request, NSHTTPURLResponse *response, NSError *error, id propertyList))
	public delegate void AFPropertyListRequestOperationFailure(NSUrlRequest request, NSHttpUrlResponse response, NSError error, NSObject propertyList);

	#endregion

	//@interface AFHTTPClient : NSObject <NSCoding, NSCopying>
	[BaseType (typeof (NSObject))]
	public partial interface AFHTTPClient {

		//@property (readonly, nonatomic) NSURL *baseURL;
		[Export ("baseURL")]
		NSUrl BaseUrl { get; }

		//@property (nonatomic, assign) NSStringEncoding stringEncoding;
		[Export ("stringEncoding")]
		NSStringEncoding StringEncoding { get; set; }

		//@property (nonatomic, assign) AFHTTPClientParameterEncoding parameterEncoding;
		[Export ("parameterEncoding")]
		AFHTTPClientParameterEncoding ParameterEncoding { get; set; }

		//@property (readonly, nonatomic) NSOperationQueue *operationQueue;
		[Export ("operationQueue")]
		NSOperationQueue OperationQueue { get; }

		//TODO:
		//@property (readonly, nonatomic, assign) AFNetworkReachabilityStatus networkReachabilityStatus;

		//+ (AFHTTPClient *)clientWithBaseURL:(NSURL *)url;
		[Static, Export ("clientWithBaseURL:")]
		AFHTTPClient ClientWithBaseUrl (NSUrl url);

		//- (id)initWithBaseURL:(NSURL *)url;
		[Export ("initWithBaseURL:")]
		IntPtr Constructor (NSUrl url);

		//- (BOOL)registerHTTPOperationClass:(Class)operationClass;
		[Export ("registerHTTPOperationClass:")]
		bool RegisterHttpOperationClass (MonoTouch.ObjCRuntime.Class operationClass);

		//- (void)unregisterHTTPOperationClass:(Class)operationClass;
		[Export ("unregisterHTTPOperationClass:")]
		void UnregisterHttpOperationClass (MonoTouch.ObjCRuntime.Class operationClass);

		//- (NSString *)defaultValueForHeader:(NSString *)header;
		[Export ("defaultValueForHeader:")]
		string DefaultValueForHeader (string header);

		//- (void)setDefaultHeader:(NSString *)header value:(NSString *)value;
		[Export ("setDefaultHeader:value:")]
		void SetDefaultHeader (string header, string value);

		//- (void)setAuthorizationHeaderWithUsername:(NSString *)username password:(NSString *)password;
		[Export ("setAuthorizationHeaderWithUsername:password:")]
		void SetAuthorizationHeaderWithUsername (string username, string password);

		//- (void)setAuthorizationHeaderWithToken:(NSString *)token;
		[Export ("authorizationHeaderWithToken")]
		string AuthorizationHeaderWithToken { set; }

		//- (void)clearAuthorizationHeader;
		[Export ("clearAuthorizationHeader")]
		void ClearAuthorizationHeader ();

		//- (NSMutableURLRequest *)requestWithMethod:(NSString *)method 
		//	path:(NSString *)path 
		//		parameters:(NSDictionary *)parameters;
		[Export ("requestWithMethod:path:parameters:")]
		NSMutableUrlRequest RequestWithMethod (string method, string path, NSDictionary parameters);

		//- (NSMutableURLRequest *)multipartFormRequestWithMethod:(NSString *)method
		//	path:(NSString *)path
		//		parameters:(NSDictionary *)parameters
		//		constructingBodyWithBlock:(void (^)(id <AFMultipartFormData> formData))block;
		[Export ("multipartFormRequestWithMethod:path:parameters:constructingBodyWithBlock:")]
		NSMutableUrlRequest MultipartFormRequestWithMethod (string method, string path, NSDictionary parameters, AFHTTPClientMultipartFormRequestWithMethodBlock block);

		//- (AFHTTPRequestOperation *)HTTPRequestOperationWithRequest:(NSURLRequest *)urlRequest
		//	success:(void (^)(AFHTTPRequestOperation *operation, id responseObject))success
		//		failure:(void (^)(AFHTTPRequestOperation *operation, NSError *error))failure;
		[Export ("HTTPRequestOperationWithRequest:success:failure:")]
		AFHTTPRequestOperation HttpRequestOperationWithRequest (NSUrlRequest urlRequest, AFHTTPClientRequestSuccess success, AFHTTPClientRequestFailure failure);

		//- (void)enqueueHTTPRequestOperation:(AFHTTPRequestOperation *)operation;
		[Export ("enqueueHTTPRequestOperation:")]
		void EnqueueHTTPRequestOperation (AFHTTPRequestOperation operation);

		//- (void)cancelAllHTTPOperationsWithMethod:(NSString *)method path:(NSString *)path;
		[Export ("cancelAllHTTPOperationsWithMethod:path:")]
		void CancelAllHttpoPerationsWithMethod (string method, string path);

		//- (void)enqueueBatchOfHTTPRequestOperationsWithRequests:(NSArray *)urlRequests
		//	progressBlock:(void (^)(NSUInteger numberOfFinishedOperations, NSUInteger totalNumberOfOperations))progressBlock 
		//		completionBlock:(void (^)(NSArray *operations))completionBlock;
		[Export ("enqueueBatchOfHTTPRequestOperationsWithRequests:progressBlock:completionBlock:")]
		void EnqueueBatchOfHttprEquestOperationsWithRequests (NSObject [] urlRequests, AFHTTPClientProgress progressBlock, AFHTTPClientCompletion completionBlock);

		//- (void)enqueueBatchOfHTTPRequestOperations:(NSArray *)operations 
		//	progressBlock:(void (^)(NSUInteger numberOfFinishedOperations, NSUInteger totalNumberOfOperations))progressBlock 
		//		completionBlock:(void (^)(NSArray *operations))completionBlock;
		[Export ("enqueueBatchOfHTTPRequestOperations:progressBlock:completionBlock:")]
		void EnqueueBatchOfHttprEquestOperations (NSObject [] operations, AFHTTPClientProgress progressBlock, AFHTTPClientCompletion completionBlock);

		//- (void)getPath:(NSString *)path
		//	parameters:(NSDictionary *)parameters
		//		success:(void (^)(AFHTTPRequestOperation *operation, id responseObject))success
		//		failure:(void (^)(AFHTTPRequestOperation *operation, NSError *error))failure;
		[Export ("getPath:parameters:success:failure:")]
		void GetPath (string path, NSDictionary parameters, AFHTTPClientRequestSuccess success, AFHTTPClientRequestFailure failure);

		//- (void)postPath:(NSString *)path 
		//	parameters:(NSDictionary *)parameters 
		//		success:(void (^)(AFHTTPRequestOperation *operation, id responseObject))success
		//		failure:(void (^)(AFHTTPRequestOperation *operation, NSError *error))failure;
		[Export ("postPath:parameters:success:failure:")]
		void PostPath (string path, NSDictionary parameters, AFHTTPClientRequestSuccess success, AFHTTPClientRequestFailure failure);

		//- (void)putPath:(NSString *)path 
		//	parameters:(NSDictionary *)parameters 
		//		success:(void (^)(AFHTTPRequestOperation *operation, id responseObject))success
		//		failure:(void (^)(AFHTTPRequestOperation *operation, NSError *error))failure;
		[Export ("putPath:parameters:success:failure:")]
		void PutPath (string path, NSDictionary parameters, AFHTTPClientRequestSuccess success, AFHTTPClientRequestFailure failure);

		//- (void)deletePath:(NSString *)path 
		//	parameters:(NSDictionary *)parameters 
		//		success:(void (^)(AFHTTPRequestOperation *operation, id responseObject))success
		//		failure:(void (^)(AFHTTPRequestOperation *operation, NSError *error))failure;
		[Export ("deletePath:parameters:success:failure:")]
		void DeletePath (string path, NSDictionary parameters, AFHTTPClientRequestSuccess success, AFHTTPClientRequestFailure failure);

		//- (void)patchPath:(NSString *)path
		//	parameters:(NSDictionary *)parameters 
		//		success:(void (^)(AFHTTPRequestOperation *operation, id responseObject))success
		//		failure:(void (^)(AFHTTPRequestOperation *operation, NSError *error))failure;
		[Export ("patchPath:parameters:success:failure:")]
		void PatchPath (string path, NSDictionary parameters, AFHTTPClientRequestSuccess success, AFHTTPClientRequestFailure failure);

		// TODO:
		//[Field ("kAFUploadStream3GSuggestedPacketSize")]
		//uint kAFUploadStream3GSuggestedPacketSize { get; }
		//
		//[Field ("kAFUploadStream3GSuggestedDelay")]
		//double kAFUploadStream3GSuggestedDelay { get; }
	}

	//@protocol AFMultipartFormData
	[Model]
	[BaseType(typeof(NSObject))]
	public partial interface AFMultipartFormData {

		//- (BOOL)appendPartWithFileURL:(NSURL *)fileURL
		//	name:(NSString *)name
		//		error:(NSError * __autoreleasing *)error;
		[Export ("appendPartWithFileURL:name:error:")]
		bool AppendPartWithFileUrl (NSUrl fileURL, string name, out NSError error);

		//- (void)appendPartWithFileData:(NSData *)data
		//	name:(NSString *)name
		//		fileName:(NSString *)fileName
		//		mimeType:(NSString *)mimeType;
		[Export ("appendPartWithFileData:name:fileName:mimeType:")]
		void AppendPartWithFileData (NSData data, string name, string fileName, string mimeType);

		//- (void)appendPartWithFormData:(NSData *)data
		//	name:(NSString *)name;
		[Export ("appendPartWithFormData:name:")]
		void AppendPartWithFormData (NSData data, string name);

		//- (void)appendPartWithHeaders:(NSDictionary *)headers
		//	body:(NSData *)body;
		[Export ("appendPartWithHeaders:body:")]
		void AppendPartWithHeaders (NSDictionary headers, NSData body);

		//- (void)throttleBandwidthWithPacketSize:(NSUInteger)numberOfBytes
		//	delay:(NSTimeInterval)delay;
		[Export ("throttleBandwidthWithPacketSize:delay:")]
		void ThrottleBandwidthWithPacketSize (uint numberOfBytes, double delay);
	}

	//@interface AFHTTPRequestOperation : AFURLConnectionOperation
	[BaseType (typeof (AFURLConnectionOperation))]
	public partial interface AFHTTPRequestOperation {

		//@property (readonly, nonatomic, strong) NSHTTPURLResponse *response;
		[Export ("response")]
		NSHttpUrlResponse Response { get; }

		//@property (readonly) BOOL hasAcceptableStatusCode;
		[Export ("hasAcceptableStatusCode")]
		bool HasAcceptableStatusCode { get; }

		//@property (readonly) BOOL hasAcceptableContentType;
		[Export ("hasAcceptableContentType")]
		bool HasAcceptableContentType { get; }

		//@property (nonatomic, assign) dispatch_queue_t successCallbackQueue;
		[Export ("successCallbackQueue")]
		DispatchQueue SuccessCallbackQueue { get; set; }

		//@property (nonatomic, assign) dispatch_queue_t failureCallbackQueue;
		[Export ("failureCallbackQueue")]
		DispatchQueue FailureCallbackQueue { get; set; }

		//+ (NSIndexSet *)acceptableStatusCodes;
		[Export ("acceptableStatusCodes")]
		NSIndexSet AcceptableStatusCodes { get; }

		//+ (void)addAcceptableStatusCodes:(NSIndexSet *)statusCodes;
		[Static, Export ("addAcceptableStatusCodes:")]
		void AddAcceptableStatusCodes (NSIndexSet statusCodes);

		//+ (NSSet *)acceptableContentTypes;
		[Export ("acceptableContentTypes")]
		NSSet AcceptableContentTypes { get; }

		//+ (void)addAcceptableContentTypes:(NSSet *)contentTypes;
		[Static, Export ("addAcceptableContentTypes:")]
		void AddAcceptableContentTypes (NSSet contentTypes);

		//+ (BOOL)canProcessRequest:(NSURLRequest *)urlRequest;
		[Static, Export ("canProcessRequest:")]
		bool CanProcessRequest (NSUrlRequest urlRequest);

		// TODO:
		//[Export ("completionBlockWithSuccess:failure")]
		//Delegate CompletionBlockWithSuccess { set; }
	}

	//@interface AFImageRequestOperation : AFHTTPRequestOperation
	[BaseType (typeof (AFHTTPRequestOperation))]
	public partial interface AFImageRequestOperation {

		//@property (readonly, nonatomic, strong) UIImage *responseImage;
		[Export ("responseImage")]
		UIImage ResponseImage { get; }

		//@property (nonatomic, assign) CGFloat imageScale;
		[Export ("imageScale")]
		float ImageScale { get; set; }

		//+ (AFImageRequestOperation *)imageRequestOperationWithRequest:(NSURLRequest *)urlRequest                
		//	success:(void (^)(UIImage *image))success;
		[Static, Export ("imageRequestOperationWithRequest:success:")]
		AFImageRequestOperation ImageRequestOperationWithRequest (NSUrlRequest urlRequest, ImageRequestOperationWithRequestSuccess1 success);

		//+ (AFImageRequestOperation *)imageRequestOperationWithRequest:(NSURLRequest *)urlRequest
		//	imageProcessingBlock:(UIImage *(^)(UIImage *image))imageProcessingBlock
		//		success:(void (^)(NSURLRequest *request, NSHTTPURLResponse *response, UIImage *image))success
		//		failure:(void (^)(NSURLRequest *request, NSHTTPURLResponse *response, NSError *error))failure;
		[Static, Export ("imageRequestOperationWithRequest:imageProcessingBlock:success:failure:")]
		AFImageRequestOperation ImageRequestOperationWithRequest (NSUrlRequest urlRequest, [NullAllowed]ImageRequestOperationWithRequestProcessingBlock imageProcessingBlock, ImageRequestOperationWithRequestSuccess2 success, ImageRequestOperationWithRequestFailure failure);
	}

	//@interface AFJSONRequestOperation : AFHTTPRequestOperation
	[BaseType (typeof (AFHTTPRequestOperation))]
	public partial interface AFJSONRequestOperation {

		//@property (readonly, nonatomic, strong) id responseJSON;
		[Export ("responseJSON")]
		NSObject ResponseJson { get; }

		//@property (nonatomic, assign) NSJSONReadingOptions JSONReadingOptions;
		[Export ("JSONReadingOptions")]
		NSJsonReadingOptions JsonReadingOptions { get; set; }

		//+ (AFJSONRequestOperation *)JSONRequestOperationWithRequest:(NSURLRequest *)urlRequest
		//	success:(void (^)(NSURLRequest *request, NSHTTPURLResponse *response, id JSON))success 
		//		failure:(void (^)(NSURLRequest *request, NSHTTPURLResponse *response, NSError *error, id JSON))failure;
		[Static, Export ("JSONRequestOperationWithRequest:success:failure:")]
		AFJSONRequestOperation JsonRequestOperationWithRequest (NSUrlRequest urlRequest, AFJSONRequestOperationJsonRequestOperationWithRequestSuccess success, AFJSONRequestOperationJsonRequestOperationWithRequestFailure failure);
	}

	//@interface AFNetworkActivityIndicatorManager : NSObject
	[BaseType (typeof (NSObject))]
	public partial interface AFNetworkActivityIndicatorManager {

		//@property (nonatomic, assign, getter = isEnabled) BOOL enabled;
		[Export ("enabled")]
		bool Enabled { [Bind ("isEnabled")] get; set; }

		//@property (readonly, nonatomic, assign) BOOL isNetworkActivityIndicatorVisible; 
		[Export ("isNetworkActivityIndicatorVisible")]
		bool IsNetworkActivityIndicatorVisible { get; }

		//+ (AFNetworkActivityIndicatorManager *)sharedManager;
		[Export ("sharedManager"), Static]
		AFNetworkActivityIndicatorManager SharedManager { get; }

		//- (void)incrementActivityCount;
		[Export ("incrementActivityCount")]
		void IncrementActivityCount ();

		//- (void)decrementActivityCount;
		[Export ("decrementActivityCount")]
		void DecrementActivityCount ();
	}

	//@interface AFPropertyListRequestOperation : AFHTTPRequestOperation
	[BaseType (typeof (AFHTTPRequestOperation))]
	public partial interface AFPropertyListRequestOperation {

		//@property (readonly, nonatomic) id responsePropertyList;
		[Export ("responsePropertyList")]
		NSObject ResponsePropertyList { get; }

		//@property (nonatomic, assign) NSPropertyListReadOptions propertyListReadOptions;
		[Export ("propertyListReadOptions")]
		NSPropertyListReadOptions PropertyListReadOptions { get; set; }

		//+ (AFPropertyListRequestOperation *)propertyListRequestOperationWithRequest:(NSURLRequest *)urlRequest
		//	success:(void (^)(NSURLRequest *request, NSHTTPURLResponse *response, id propertyList))success
		//		failure:(void (^)(NSURLRequest *request, NSHTTPURLResponse *response, NSError *error, id propertyList))failure;
		[Static, Export ("propertyListRequestOperationWithRequest:success:failure:")]
		AFPropertyListRequestOperation PropertyListRequestOperationWithRequest (NSUrlRequest urlRequest, AFPropertyListRequestOperationSuccess success, AFPropertyListRequestOperationFailure failure);
	}

	//@interface AFURLConnectionOperation : NSOperation <NSURLConnectionDelegate, NSURLConnectionDataDelegate, NSCoding, NSCopying>
	[BaseType (typeof (NSOperation))]
	public partial interface AFURLConnectionOperation {

		//@property (nonatomic, strong) NSSet *runLoopModes;
		[Export ("runLoopModes")]
		NSSet RunLoopModes { get; set; }

		//@property (readonly, nonatomic, strong) NSURLRequest *request;
		[Export ("request")]
		NSUrlRequest Request { get; }

		//@property (readonly, nonatomic, strong) NSURLResponse *response;
		[Export ("response")]
		NSUrlResponse Response { get; }

		//@property (readonly, nonatomic, strong) NSError *error;
		[Export ("error")]
		NSError Error { get; }

		//@property (readonly, nonatomic, strong) NSData *responseData;
		[Export ("responseData")]
		NSData ResponseData { get; }

		//@property (readonly, nonatomic, copy) NSString *responseString;
		[Export ("responseString")]
		string ResponseString { get; }

		//@property (nonatomic, strong) NSInputStream *inputStream;
		[Export ("inputStream")]
		NSInputStream InputStream { get; set; }

		//@property (nonatomic, strong) NSOutputStream *outputStream;
		[Export ("outputStream")]
		NSOutputStream OutputStream { get; set; }

		//- (id)initWithRequest:(NSURLRequest *)urlRequest;
		[Export ("initWithRequest:")]
		IntPtr Constructor (NSUrlRequest urlRequest);

		//- (void)pause;
		[Export ("pause")]
		void Pause ();

		//- (BOOL)isPaused;
		[Export ("isPaused")]
		bool IsPaused { get; }

		//- (void)resume;
		[Export ("resume")]
		void Resume ();

		/// TODO:
		//[Export ("shouldExecuteAsBackgroundTaskWithExpirationHandler")]
		//Delegate ShouldExecuteAsBackgroundTaskWithExpirationHandler { set; }
	}

	#endregion
}

