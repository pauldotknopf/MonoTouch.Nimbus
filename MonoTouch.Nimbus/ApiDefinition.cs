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
	public partial interface NIPhotoAlbumScrollViewDelegate {

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

	public delegate void AFHTTPClientRequestSuccess(AFHTTPRequestOperation operation, NSObject responseObject);

	public delegate void AFHTTPClientRequestFailure(AFHTTPRequestOperation operation, NSError error);
	
	public delegate void AFHTTPClientMultipartFormRequestWithMethodBlock(AFMultipartFormData formData);

	public delegate void AFHTTPClientProgress(UINavigationItem numberOfFinishedOperations, uint totalNumberOfOperations);

	public delegate void AFHTTPClientCompletion(NSArray operations);

	[BaseType (typeof (NSObject))]
	public partial interface AFHTTPClient {

		[Export ("baseURL")]
		NSUrl BaseUrl { get; }

		[Export ("stringEncoding")]
		NSStringEncoding StringEncoding { get; set; }

		[Export ("parameterEncoding")]
		AFHTTPClientParameterEncoding ParameterEncoding { get; set; }

		[Export ("operationQueue")]
		NSOperationQueue OperationQueue { get; }

		[Static, Export ("clientWithBaseURL:")]
		AFHTTPClient ClientWithBaseUrl (NSUrl url);

		[Export ("initWithBaseURL:")]
		IntPtr Constructor (NSUrl url);

		[Export ("registerHTTPOperationClass:")]
		bool RegisterHTTPOperationClass (MonoTouch.ObjCRuntime.Class operationClass);

		[Export ("unregisterHTTPOperationClass:")]
		void UnregisterHTTPOperationClass (MonoTouch.ObjCRuntime.Class operationClass);

		[Export ("defaultValueForHeader:")]
		string DefaultValueForHeader (string header);

		[Export ("setDefaultHeader:value:")]
		void SetDefaultHeader (string header, string value);

		[Export ("setAuthorizationHeaderWithUsername:password:")]
		void SetAuthorizationHeaderWithUsername (string username, string password);

		[Export ("authorizationHeaderWithToken")]
		string AuthorizationHeaderWithToken { set; }

		[Export ("clearAuthorizationHeader")]
		void ClearAuthorizationHeader ();

		[Export ("requestWithMethod:path:parameters:")]
		NSMutableUrlRequest RequestWithMethod (string method, string path, NSDictionary parameters);

		[Export ("multipartFormRequestWithMethod:path:parameters:constructingBodyWithBlock:")]
		NSMutableUrlRequest MultipartFormRequestWithMethod (string method, string path, NSDictionary parameters, AFHTTPClientMultipartFormRequestWithMethodBlock block);

		[Export ("HTTPRequestOperationWithRequest:success:failure:")]
		AFHTTPRequestOperation HTTPRequestOperationWithRequest (NSUrlRequest urlRequest, AFHTTPClientRequestSuccess success, AFHTTPClientRequestFailure failure);

		[Export ("enqueueHTTPRequestOperation:")]
		void EnqueueHTTPRequestOperation (AFHTTPRequestOperation operation);

		[Export ("cancelAllHTTPOperationsWithMethod:path:")]
		void CancelAllHttpoPerationsWithMethod (string method, string path);

		[Export ("enqueueBatchOfHTTPRequestOperationsWithRequests:progressBlock:completionBlock:")]
		void EnqueueBatchOfHttprEquestOperationsWithRequests (NSObject [] urlRequests, AFHTTPClientProgress progressBlock, AFHTTPClientCompletion completionBlock);

		[Export ("enqueueBatchOfHTTPRequestOperations:progressBlock:completionBlock:")]
		void EnqueueBatchOfHttprEquestOperations (NSObject [] operations, AFHTTPClientProgress progressBlock, AFHTTPClientCompletion completionBlock);

		[Export ("getPath:parameters:success:failure:")]
		void GetPath (string path, NSDictionary parameters, AFHTTPClientRequestSuccess success, AFHTTPClientRequestFailure failure);

		[Export ("postPath:parameters:success:failure:")]
		void PostPath (string path, NSDictionary parameters, AFHTTPClientRequestSuccess success, AFHTTPClientRequestFailure failure);

		[Export ("putPath:parameters:success:failure:")]
		void PutPath (string path, NSDictionary parameters, AFHTTPClientRequestSuccess success, AFHTTPClientRequestFailure failure);

		[Export ("deletePath:parameters:success:failure:")]
		void DeletePath (string path, NSDictionary parameters, AFHTTPClientRequestSuccess success, AFHTTPClientRequestFailure failure);

		[Export ("patchPath:parameters:success:failure:")]
		void PatchPath (string path, NSDictionary parameters, AFHTTPClientRequestSuccess success, AFHTTPClientRequestFailure failure);

		// TODO:
//		[Field ("kAFUploadStream3GSuggestedPacketSize")]
//		uint kAFUploadStream3GSuggestedPacketSize { get; }
//
//		[Field ("kAFUploadStream3GSuggestedDelay")]
//		double kAFUploadStream3GSuggestedDelay { get; }
	}

	[Model]
	[BaseType(typeof(NSObject))]
	public partial interface AFMultipartFormData {

		[Export ("appendPartWithFileURL:name:error:")]
		bool AppendPartWithFileUrl (NSUrl fileURL, string name, out NSError error);

		[Export ("appendPartWithFileData:name:fileName:mimeType:")]
		void AppendPartWithFileData (NSData data, string name, string fileName, string mimeType);

		[Export ("appendPartWithFormData:name:")]
		void AppendPartWithFormData (NSData data, string name);

		[Export ("appendPartWithHeaders:body:")]
		void AppendPartWithHeaders (NSDictionary headers, NSData body);

		[Export ("throttleBandwidthWithPacketSize:delay:")]
		void ThrottleBandwidthWithPacketSize (uint numberOfBytes, double delay);
	}

	[BaseType (typeof (AFURLConnectionOperation))]
	public partial interface AFHTTPRequestOperation {

		[Export ("response")]
		NSHttpUrlResponse Response { get; }

		[Export ("hasAcceptableStatusCode")]
		bool HasAcceptableStatusCode { get; }

		[Export ("hasAcceptableContentType")]
		bool HasAcceptableContentType { get; }

		[Export ("successCallbackQueue")]
		DispatchQueue SuccessCallbackQueue { get; set; }

		[Export ("failureCallbackQueue")]
		DispatchQueue FailureCallbackQueue { get; set; }

		[Export ("acceptableStatusCodes")]
		NSIndexSet AcceptableStatusCodes { get; }

		[Static, Export ("addAcceptableStatusCodes:")]
		void AddAcceptableStatusCodes (NSIndexSet statusCodes);

		[Export ("acceptableContentTypes")]
		NSSet AcceptableContentTypes { get; }

		[Static, Export ("addAcceptableContentTypes:")]
		void AddAcceptableContentTypes (NSSet contentTypes);

		[Static, Export ("canProcessRequest:")]
		bool CanProcessRequest (NSUrlRequest urlRequest);

		// TODO:
//		[Export ("completionBlockWithSuccess:failure")]
//		Delegate CompletionBlockWithSuccess { set; }
	}

	[BaseType (typeof (AFHTTPRequestOperation))]
	public partial interface AFImageRequestOperation {

		[Export ("responseImage")]
		UIImage ResponseImage { get; }

		[Export ("imageScale")]
		float ImageScale { get; set; }

		[Static, Export ("imageRequestOperationWithRequest:success:")]
		AFImageRequestOperation ImageRequestOperationWithRequest (NSUrlRequest urlRequest, ImageRequestOperationWithRequestSuccess1 success);

		[Static, Export ("imageRequestOperationWithRequest:imageProcessingBlock:success:failure:")]
		AFImageRequestOperation ImageRequestOperationWithRequest (NSUrlRequest urlRequest, [NullAllowed]ImageRequestOperationWithRequestProcessingBlock imageProcessingBlock, ImageRequestOperationWithRequestSuccess2 success, ImageRequestOperationWithRequestFailure failure);
	}

	public delegate void ImageRequestOperationWithRequestSuccess1(UIImage image);
	public delegate void ImageRequestOperationWithRequestSuccess2(NSUrlRequest request, NSHttpUrlResponse response, UIImage image);
	public delegate void ImageRequestOperationWithRequestFailure(NSUrlRequest request, NSHttpUrlResponse response, NSError error);
	public delegate UIImage ImageRequestOperationWithRequestProcessingBlock(UIImage image);
	public delegate void AFJSONRequestOperationJsonRequestOperationWithRequestSuccess(NSUrlRequest request, NSHttpUrlResponse response, NSObject json);
	public delegate void AFJSONRequestOperationJsonRequestOperationWithRequestFailure(NSUrlRequest request, NSHttpUrlResponse response, NSError error, NSObject json);
	[BaseType (typeof (AFHTTPRequestOperation))]
	public partial interface AFJSONRequestOperation {

		[Export ("responseJSON")]
		NSObject ResponseJson { get; }

		[Export ("JSONReadingOptions")]
		NSJsonReadingOptions JsonReadingOptions { get; set; }

		[Static, Export ("JSONRequestOperationWithRequest:success:failure:")]
		AFJSONRequestOperation JsonRequestOperationWithRequest (NSUrlRequest urlRequest, AFJSONRequestOperationJsonRequestOperationWithRequestSuccess success, AFJSONRequestOperationJsonRequestOperationWithRequestFailure failure);
	}

	[BaseType (typeof (NSObject))]
	public partial interface AFNetworkActivityIndicatorManager {

		[Export ("enabled")]
		bool Enabled { [Bind ("isEnabled")] get; set; }

		[Export ("isNetworkActivityIndicatorVisible")]
		bool IsNetworkActivityIndicatorVisible { get; }

		[Export ("sharedManager")]
		AFNetworkActivityIndicatorManager SharedManager { get; }

		[Export ("incrementActivityCount")]
		void IncrementActivityCount ();

		[Export ("decrementActivityCount")]
		void DecrementActivityCount ();
	}

//	+ (AFPropertyListRequestOperation *)propertyListRequestOperationWithRequest:(NSURLRequest *)urlRequest
//		success:(void (^)(NSURLRequest *request, NSHTTPURLResponse *response, id propertyList))success
//			failure:(void (^)(NSURLRequest *request, NSHTTPURLResponse *response, NSError *error, id propertyList))failure;

	public delegate void AFPropertyListRequestOperationSuccess(NSUrlRequest request, NSHttpUrlResponse response, NSObject propertyList);
	public delegate void AFPropertyListRequestOperationFailure(NSUrlRequest request, NSHttpUrlResponse response, NSError error, NSObject propertyList);

	[BaseType (typeof (AFHTTPRequestOperation))]
	public partial interface AFPropertyListRequestOperation {

		[Export ("responsePropertyList")]
		NSObject ResponsePropertyList { get; }

		[Export ("propertyListReadOptions")]
		NSPropertyListReadOptions PropertyListReadOptions { get; set; }

		[Static, Export ("propertyListRequestOperationWithRequest:success:failure:")]
		AFPropertyListRequestOperation PropertyListRequestOperationWithRequest (NSUrlRequest urlRequest, AFPropertyListRequestOperationSuccess success, AFPropertyListRequestOperationFailure failure);
	}

	[BaseType (typeof (NSOperation))]
	public partial interface AFURLConnectionOperation {

		[Export ("runLoopModes")]
		NSSet RunLoopModes { get; set; }

		[Export ("request")]
		NSUrlRequest Request { get; }

		[Export ("response")]
		NSUrlResponse Response { get; }

		[Export ("error")]
		NSError Error { get; }

		[Export ("responseData")]
		NSData ResponseData { get; }

		[Export ("responseString")]
		string ResponseString { get; }

		[Export ("inputStream")]
		NSInputStream InputStream { get; set; }

		[Export ("outputStream")]
		NSOutputStream OutputStream { get; set; }

		[Export ("initWithRequest:")]
		IntPtr Constructor (NSUrlRequest urlRequest);

		[Export ("pause")]
		void Pause ();

		[Export ("isPaused")]
		bool IsPaused { get; }

		[Export ("resume")]
		void Resume ();

		/// TODO:
//		[Export ("shouldExecuteAsBackgroundTaskWithExpirationHandler")]
//		Delegate ShouldExecuteAsBackgroundTaskWithExpirationHandler { set; }
	}

	#endregion
}

