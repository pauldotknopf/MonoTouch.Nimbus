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

	#region Delegates

	//void (^NIOperationBlock)(NIOperation* operation);
	public delegate void NIOperationBlock(NIOperation operation);
	//void (^NIOperationDidFailBlock)(NIOperation* operation, NSError* error);
	public delegate void NIOperationDidFailBlock(NIOperation operation, NSError error);

	#endregion

	#region Operations

	//@interface NIOperation : NSOperation
	[BaseType (typeof (NSOperation))]
	public partial interface NIOperation {

		//@property (readwrite, NI_WEAK) id<NIOperationDelegate> delegate;
		[Export ("delegate"), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap("WeakDelegate")]
		NIOperationDelegate Delegate { get; set; }

		//@property (readonly,  NI_STRONG) NSError* lastError;
		[Export ("lastError")]
		NSError LastError { get; }

		//@property (readwrite, assign) NSInteger tag;
		[Export ("tag")]
		int Tag { get; set; }

		//@property (readwrite, copy) NIOperationBlock didStartBlock;
		[Export ("didStartBlock")]
		NIOperationBlock DidStartBlock { get; set; }

		//@property (readwrite, copy) NIOperationBlock didFinishBlock;
		[Export ("didFinishBlock")]
		NIOperationBlock DidFinishBlock { get; set; }

		//@property (readwrite, copy) NIOperationDidFailBlock didFailWithErrorBlock;
		[Export ("didFailWithErrorBlock")]
		NIOperationDidFailBlock DidFailWithErrorBlock { get; set; }

		//@property (readwrite, copy) NIOperationBlock willFinishBlock;
		[Export ("willFinishBlock")]
		NIOperationBlock WillFinishBlock { get; set; }

		//- (void)didStart;
		[Export ("didStart")]
		void DidStart ();

		//- (void)didFinish;
		[Export ("didFinish")]
		void DidFinish ();

		//- (void)didFailWithError:(NSError *)error;
		[Export ("didFailWithError:")]
		void DidFailWithError (NSError error);

		//- (void)willFinish;
		[Export ("willFinish")]
		void WillFinish ();
	}

	//@protocol NIOperationDelegate <NSObject>
	[Model]
	[BaseType(typeof(NSObject))]
	public partial interface NIOperationDelegate {

		//- (void)nimbusOperationDidStart:(NIOperation *)operation;
		[Export ("nimbusOperationDidStart:")]
		void NimbusOperationDidStart (NIOperation operation);

		//- (void)nimbusOperationWillFinish:(NIOperation *)operation;
		[Export ("nimbusOperationWillFinish:")]
		void NimbusOperationWillFinish (NIOperation operation);

		//- (void)nimbusOperationDidFinish:(NIOperation *)operation;
		[Export ("nimbusOperationDidFinish:")]
		void NimbusOperationDidFinish (NIOperation operation);

		//- (void)nimbusOperationDidFail:(NIOperation *)operation withError:(NSError *)error;
		[Export ("nimbusOperationDidFail:withError:")]
		void NimbusOperationDidFail (NIOperation operation, NSError error);
	}

	#endregion

	#region View Recycling

 	//@interface NIViewRecycler : NSObject
	[BaseType (typeof (NSObject))]
	public partial interface NIViewRecycler {

		//- (UIView<NIRecyclableView> *)dequeueReusableViewWithIdentifier:(NSString *)reuseIdentifier;
		[Export ("dequeueReusableViewWithIdentifier:")]
		UIView DequeueReusableViewWithIdentifier (string reuseIdentifier);

		//- (void)recycleView:(UIView<NIRecyclableView> *)view;
		[Export ("recycleView:")]
		void RecycleView (UIView view);

		//- (void)removeAllViews;
		[Export ("removeAllViews")]
		void RemoveAllViews ();
	}

	//@protocol NIRecyclableView <NSObject>
	[BaseType(typeof(NSObject))]
	[Model]
	public partial interface NIRecyclableViewProtocol {

 		//@property (nonatomic, readwrite, copy) NSString* reuseIdentifier;
		[Export ("reuseIdentifier")]
		string ReuseIdentifier { get; set; }

 		//- (void)prepareForReuse;
		[Export ("prepareForReuse")]
		void PrepareForReuse ();
	}

 	//@interface NIRecyclableView : UIView <NIRecyclableView>
	[BaseType (typeof (UIView))]
	public partial interface NIRecyclableView : NIRecyclableViewProtocol {

		//- (id)initWithReuseIdentifier:(NSString *)reuseIdentifier;
		[Export ("initWithReuseIdentifier:")]
		IntPtr Constructor (string reuseIdentifier);
	}

	#endregion

	#region InMemoryCache

	//@interface NIMemoryCache : NSObject
	[BaseType (typeof (NSObject))]
	public partial interface NIMemoryCache {

		//- (id)initWithCapacity:(NSUInteger)capacity;
		[Export ("initWithCapacity:")]
		IntPtr Constructor (uint capacity);

		//- (NSUInteger)count;
		[Export ("count")]
		uint Count { get; }

		//- (void)storeObject:(id)object withName:(NSString *)name;
		[Export ("storeObject:withName:")]
		void StoreObject (NSObject item, string name);

		//- (void)storeObject:(id)object withName:(NSString *)name expiresAfter:(NSDate *)expirationDate;
		[Export ("storeObject:withName:expiresAfter:")]
		void StoreObject (NSObject item, string name, NSDate expirationDate);

		//- (void)removeObjectWithName:(NSString *)name;
		[Export ("removeObjectWithName:")]
		void RemoveObjectWithName (string name);

		//- (void)removeAllObjectsWithPrefix:(NSString *)prefix;
		[Export ("removeAllObjectsWithPrefix:")]
		void RemoveAllObjectsWithPrefix (string prefix);

		//- (void)removeAllObjects;
		[Export ("removeAllObjects")]
		void RemoveAllObjects ();

		//- (id)objectWithName:(NSString *)name;
		[Export ("objectWithName:")]
		NSObject ObjectWithName (string name);

		//- (BOOL)containsObjectWithName:(NSString *)name;
		[Export ("containsObjectWithName:")]
		bool ContainsObjectWithName (string name);

		//- (NSDate *)dateOfLastAccessWithName:(NSString *)name;
		[Export ("dateOfLastAccessWithName:")]
		NSDate DateOfLastAccessWithName (string name);

		//- (NSString *)nameOfLeastRecentlyUsedObject;
		[Export ("nameOfLeastRecentlyUsedObject")]
		string NameOfLeastRecentlyUsedObject { get; }

		//- (NSString *)nameOfMostRecentlyUsedObject;
		[Export ("nameOfMostRecentlyUsedObject")]
		string NameOfMostRecentlyUsedObject { get; }

		//- (void)reduceMemoryUsage;
		[Export ("reduceMemoryUsage")]
		void ReduceMemoryUsage ();

		//- (BOOL)willSetObject:(id)object withName:(NSString *)name previousObject:(id)previousObject;
		[Export ("willSetObject:withName:previousObject:")]
		bool WillSetObject (NSObject item, string name, NSObject previousObject);

		//- (void)didSetObject:(id)object withName:(NSString *)name;
		[Export ("didSetObject:withName:")]
		void DidSetObject (NSObject item, string name);

		//- (void)willRemoveObject:(id)object withName:(NSString *)name;
		[Export ("willRemoveObject:withName:")]
		void WillRemoveObject (NSObject item, string name);
	}

	//@interface NIImageMemoryCache : NIMemoryCache
	[BaseType (typeof (NIMemoryCache))]
	public partial interface NIImageMemoryCache {

		//@property (nonatomic, readonly, assign) NSUInteger numberOfPixels;
		[Export ("numberOfPixels")]
		uint NumberOfPixels { get; }

		//@property (nonatomic, readwrite, assign) NSUInteger maxNumberOfPixels;
		[Export ("maxNumberOfPixels")]
		uint MaxNumberOfPixels { get; set; }

		//@property (nonatomic, readwrite, assign) NSUInteger maxNumberOfPixelsUnderStress;
		[Export ("maxNumberOfPixelsUnderStress")]
		uint MaxNumberOfPixelsUnderStress { get; set; }
	}

	#endregion

	#endregion

	#region PagingScrollView

	//@interface NIPageView : NIRecyclableView <NIPagingScrollViewPage>
	[BaseType (typeof (NIRecyclableView))]
	public partial interface NIPageView : NIPagingScrollViewPage 
	{
	}

	//@interface NIPagingScrollView : UIView <UIScrollViewDelegate>
	[BaseType (typeof (UIView))]
	public partial interface NIPagingScrollView {

		//- (void)reloadData;
		[Export ("reloadData")]
		void ReloadData ();

		//@property (nonatomic, NI_WEAK) id<NIPagingScrollViewDataSource> dataSource;
		[Export ("dataSource"), NullAllowed]
		NSObject WeakDataSource { get; set; }

		[Wrap ("WeakDataSource")]
		NIPagingScrollViewDataSource DataSource { get; set; }

		//@property (nonatomic, NI_WEAK) id<NIPagingScrollViewDelegate> delegate;
		[Export ("delegate"), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap("WeakDelegate")]
		UIScrollViewDelegate Delegate { get; set; }

		//- (UIView<NIPagingScrollViewPage> *)dequeueReusablePageWithIdentifier:(NSString *)identifier;
		[Export ("dequeueReusablePageWithIdentifier:")]
		UIView DequeueReusablePageWithIdentifier (string identifier);

		//- (UIView<NIPagingScrollViewPage> *)centerPageView;
		[Export ("centerPageView")]
		UIView CenterPageView { get; }

		//@property (nonatomic, assign) NSInteger centerPageIndex;
		[Export ("centerPageIndex")]
		int CenterPageIndex { get; set; }

		//@property (nonatomic, readonly, assign) NSInteger numberOfPages;
		[Export ("numberOfPages")]
		int NumberOfPages { get; }

		//@property (nonatomic, assign) CGFloat pageMargin;
		[Export ("pageMargin")]
		float PageMargin { get; set; }

		//@property (nonatomic, assign) NIPagingScrollViewType type;
		[Export ("type")]
		NIPagingScrollViewType Type { get; set; }

		//- (BOOL)hasNext;
		[Export ("hasNext")]
		bool HasNext { get; }

		//- (BOOL)hasPrevious;
		[Export ("hasPrevious")]
		bool HasPrevious { get; }

		//- (void)moveToNextAnimated:(BOOL)animated;
		[Export ("moveToNextAnimated:")]
		void MoveToNextAnimated (bool animated);

		//- (void)moveToPreviousAnimated:(BOOL)animated;
		[Export ("moveToPreviousAnimated:")]
		void MoveToPreviousAnimated (bool animated);

		//- (BOOL)moveToPageAtIndex:(NSInteger)pageIndex animated:(BOOL)animated;
		[Export ("moveToPageAtIndex:animated:")]
		bool MoveToPageAtIndex (int pageIndex, bool animated);

		//- (void)willRotateToInterfaceOrientation:(UIInterfaceOrientation)toInterfaceOrientation duration:(NSTimeInterval)duration;
		[Export ("willRotateToInterfaceOrientation:duration:")]
		void WillRotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation, double duration);

		//- (void)willAnimateRotationToInterfaceOrientation:(UIInterfaceOrientation)toInterfaceOrientation duration:(NSTimeInterval)duration;
		[Export ("willAnimateRotationToInterfaceOrientation:duration:")]
		void WillAnimateRotationToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation, double duration);

		//@property (nonatomic, readonly, NI_STRONG) UIScrollView* pagingScrollView;
		[Export ("pagingScrollView")]
		UIScrollView PagingScrollView { get; }

		//@property (nonatomic, readonly, copy) NSMutableSet* visiblePages;
		[Export ("visiblePages")]
		NSMutableSet VisiblePages { get; }
	}

	//@interface NIPagingScrollView (Subclassing)
	[Category, BaseType (typeof (NIPagingScrollView))]
	public partial interface NIPagingScrollViewSubclassing {

		//- (void)willDisplayPage:(UIView<NIPagingScrollViewPage> *)pageView;
		[Export ("willDisplayPage:")]
		void WillDisplayPage (UIView pageView);

		//- (void)didRecyclePage:(UIView<NIPagingScrollViewPage> *)pageView;
		[Export ("didRecyclePage:")]
		void DidRecyclePage (UIView pageView);

		//- (void)didReloadNumberOfPages;
		[Export ("didReloadNumberOfPages")]
		void DidReloadNumberOfPages ();

		//- (void)didChangeCenterPageIndexFrom:(NSInteger)from to:(NSInteger)to;
		[Export ("didChangeCenterPageIndexFrom:to:")]
		void DidChangeCenterPageIndexFrom (int from, int to);

		//- (UIView<NIPagingScrollViewPage> *)loadPageAtIndex:(NSInteger)pageIndex;
		[Export ("loadPageAtIndex:")]
		UIView LoadPageAtIndex (int pageIndex);
	}

	//@protocol NIPagingScrollViewDataSource <NSObject>
	[Model]
	[BaseType(typeof(NSObject))]
	public partial interface NIPagingScrollViewDataSource {

		//- (NSInteger)numberOfPagesInPagingScrollView:(NIPagingScrollView *)pagingScrollView;
		[Export ("numberOfPagesInPagingScrollView:"), Abstract]
		int NumberOfPagesInPagingScrollView (NIPagingScrollView pagingScrollView);

		//- (UIView<NIPagingScrollViewPage> *)pagingScrollView:(NIPagingScrollView *)pagingScrollView pageViewForIndex:(NSInteger)pageIndex;
		[Export ("pagingScrollView:pageViewForIndex:"), Abstract]
		NSObject PageViewForIndex (NIPagingScrollView pagingScrollView, int pageIndex);
	}

	//@protocol NIPagingScrollViewDelegate <UIScrollViewDelegate>
	[Model]
	public partial interface NIPagingScrollViewDelegate {

		//- (void)pagingScrollViewDidScroll:(NIPagingScrollView *)pagingScrollView;
		[Export ("pagingScrollViewDidScroll:")]
		void PagingScrollViewDidScroll (NIPagingScrollView pagingScrollView);

		//- (void)pagingScrollViewWillChangePages:(NIPagingScrollView *)pagingScrollView;
		[Export ("pagingScrollViewWillChangePages:")]
		void PagingScrollViewWillChangePages (NIPagingScrollView pagingScrollView);

		//- (void)pagingScrollViewDidChangePages:(NIPagingScrollView *)pagingScrollView;
		[Export ("pagingScrollViewDidChangePages:")]
		void PagingScrollViewDidChangePages (NIPagingScrollView pagingScrollView);
	}

	//@protocol NIPagingScrollViewPage <NIRecyclableView>
	[Model]
	public partial interface NIPagingScrollViewPage : NIRecyclableView {

		//@property (nonatomic, readwrite, assign) NSInteger pageIndex;
		[Export ("pageIndex"), Abstract]
		int PageIndex { get; set; }

		//- (void)pageDidDisappear;
		[Export ("pageDidDisappear")]
		void PageDidDisappear ();

		//- (void)setFrameAndMaintainState:(CGRect)frame;
		[Export ("frameAndMaintainState")]
		NSObject FrameAndMaintainState { set; }
	}

	#endregion

	#region NetworkImage

	//@protocol NINetworkImageOperation <NSObject>
	[Model]
	[BaseType(typeof(NSObject))]
	public partial interface NINetworkImageOperation {

		//@property (readonly, copy) NSString* cacheIdentifier;
		[Export ("cacheIdentifier"), Abstract]
		string CacheIdentifier { get; }

		//@property (readwrite, assign) CGRect imageCropRect;
		[Export ("imageCropRect"), Abstract]
		RectangleF ImageCropRect { get; set; }

		//@property (readwrite, assign) CGSize imageDisplaySize;
		[Export ("imageDisplaySize"), Abstract]
		SizeF ImageDisplaySize { get; set; }

		//@property (readwrite, assign) NINetworkImageViewScaleOptions scaleOptions;
		[Export ("scaleOptions"), Abstract]
		NINetworkImageViewScaleOptions ScaleOptions { get; set; }

		//@property (readwrite, assign) CGInterpolationQuality interpolationQuality;
		[Export ("interpolationQuality"), Abstract]
		CGInterpolationQuality InterpolationQuality { get; set; }

		//@property (readwrite, assign) UIViewContentMode imageContentMode;
		[Export ("imageContentMode"), Abstract]
		UIViewContentMode ImageContentMode { get; set; }

		//@property (readwrite, NI_STRONG) UIImage* imageCroppedAndSizedForDisplay;
		[Export ("imageCroppedAndSizedForDisplay"), Abstract]
		UIImage ImageCroppedAndSizedForDisplay { get; set; }
	}

	//@interface NINetworkImageView : UIImageView <NIOperationDelegate>
	[BaseType (typeof (UIImageView))]
	public partial interface NINetworkImageView : NIOperationDelegate {

		//@property (nonatomic, readwrite, NI_WEAK) id<NINetworkImageViewDelegate> delegate;
		[Export ("delegate"), NullAllowed]
		NSObject WeakDelegate { get; set; }
		
		[Wrap("WeakDelegate")]
		NINetworkImageViewDelegate Delegate { get; set; }

		//- (id)initWithImage:(UIImage *)image;
		[Export ("initWithImage:")]
		IntPtr Constructor (UIImage image);

		//@property (nonatomic, readwrite, NI_STRONG) UIImage* initialImage; 
		[Export ("initialImage")]
		UIImage InitialImage { get; set; }

		//@property (nonatomic, readwrite, assign) BOOL sizeForDisplay; 
		[Export ("sizeForDisplay")]
		bool SizeForDisplay { get; set; }

		//@property (nonatomic, readwrite, assign) NINetworkImageViewScaleOptions scaleOptions;
		[Export ("scaleOptions")]
		NINetworkImageViewScaleOptions ScaleOptions { get; set; }

		//@property (nonatomic, readwrite, assign) CGInterpolationQuality interpolationQuality;
		[Export ("interpolationQuality")]
		CGInterpolationQuality InterpolationQuality { get; set; }

		//@property (nonatomic, readwrite, NI_STRONG) NIImageMemoryCache* imageMemoryCache; 
		[Export ("imageMemoryCache")]
		NIImageMemoryCache ImageMemoryCache { get; set; }

		//@property (nonatomic, readwrite, NI_STRONG) NSOperationQueue* networkOperationQueue;
		[Export ("networkOperationQueue")]
		NSOperationQueue NetworkOperationQueue { get; set; }

		//@property (nonatomic, readwrite, assign) NSTimeInterval maxAge;
		[Export ("maxAge")]
		double MaxAge { get; set; }

		//- (void)setPathToNetworkImage:(NSString *)pathToNetworkImage;
		[Export ("setPathToNetworkImage:")]
		void SetPathToNetworkImage (string pathToNetworkImage);

		//- (void)setPathToNetworkImage:(NSString *)pathToNetworkImage forDisplaySize:(CGSize)displaySize;
		[Export ("setPathToNetworkImage:forDisplaySize:")]
		void SetPathToNetworkImage (string pathToNetworkImage, SizeF displaySize);

		//- (void)setPathToNetworkImage:(NSString *)pathToNetworkImage forDisplaySize:(CGSize)displaySize contentMode:(UIViewContentMode)contentMode;
		[Export ("setPathToNetworkImage:forDisplaySize:contentMode:")]
		void SetPathToNetworkImage (string pathToNetworkImage, SizeF displaySize, UIViewContentMode contentMode);

		//- (void)setPathToNetworkImage:(NSString *)pathToNetworkImage forDisplaySize:(CGSize)displaySize contentMode:(UIViewContentMode)contentMode cropRect:(CGRect)cropRect;
		[Export ("setPathToNetworkImage:forDisplaySize:contentMode:cropRect:")]
		void SetPathToNetworkImage (string pathToNetworkImage, SizeF displaySize, UIViewContentMode contentMode, RectangleF cropRect);

		//- (void)setPathToNetworkImage:(NSString *)pathToNetworkImage cropRect:(CGRect)cropRect;
		[Export ("setPathToNetworkImage:cropRect:")]
		void SetPathToNetworkImage (string pathToNetworkImage, RectangleF cropRect);

		//- (void)setPathToNetworkImage:(NSString *)pathToNetworkImage contentMode:(UIViewContentMode)contentMode;
		[Export ("setPathToNetworkImage:contentMode:")]
		void SetPathToNetworkImage (string pathToNetworkImage, UIViewContentMode contentMode);

		//- (void)setNetworkImageOperation:(NIOperation<NINetworkImageOperation> *)operation forDisplaySize:(CGSize)displaySize contentMode:(UIViewContentMode)contentMode cropRect:(CGRect)cropRect;
		[Export ("setNetworkImageOperation:forDisplaySize:contentMode:cropRect:")]
		void SetNetworkImageOperation (NINetworkImageOperation operation, SizeF displaySize, UIViewContentMode contentMode, RectangleF cropRect);

		//@property (nonatomic, readonly, assign, getter=isLoading) BOOL loading;
		[Export ("loading")]
		bool Loading { [Bind ("isLoading")] get; }

		//- (void)prepareForReuse;
		[Export ("prepareForReuse")]
		void PrepareForReuse ();

		//- (void)networkImageViewDidStartLoading;
		[Export ("networkImageViewDidStartLoading")]
		void NetworkImageViewDidStartLoading ();

		//- (void)networkImageViewDidLoadImage:(UIImage *)image;
		[Export ("networkImageViewDidLoadImage:")]
		void NetworkImageViewDidLoadImage (UIImage image);

		//- (void)networkImageViewDidFailWithError:(NSError *)error;
		[Export ("networkImageViewDidFailWithError:")]
		void NetworkImageViewDidFailWithError (NSError error);
	}

	//@protocol NINetworkImageViewDelegate <NSObject>
	[Model]
	[BaseType(typeof(NSObject))]
	public partial interface NINetworkImageViewDelegate  {

		//- (void)networkImageViewDidStartLoad:(NINetworkImageView *)imageView;
		[Export ("networkImageViewDidStartLoad:")]
		void NetworkImageViewDidStartLoad (NINetworkImageView imageView);

		//- (void)networkImageView:(NINetworkImageView *)imageView didLoadImage:(UIImage *)image;
		[Export ("networkImageView:didLoadImage:")]
		void NetworkImageViewDidLoadImage (NINetworkImageView imageView, UIImage image);

		//- (void)networkImageView:(NINetworkImageView *)imageView didFailWithError:(NSError *)error;
		[Export ("networkImageView:didFailWithError:")]
		void NetworkImageViewDidFailWithError (NINetworkImageView imageView, NSError error);

		//- (void)networkImageView:(NINetworkImageView *)imageView readBytes:(long long)readBytes totalBytes:(long long)totalBytes;
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
		UIView PageViewForIndex (NSObject pagingScrollView, int pageIndex);

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
		NSObject LoadPhotoAtIndex (NIPhotoAlbumScrollView photoAlbumScrollView, int photoIndex, out NIPhotoScrollViewPhotoSize photoSize, out bool isLoading, out SizeF originalPhotoDimensions);

		//- (void)photoAlbumScrollView: (NIPhotoAlbumScrollView *)photoAlbumScrollView
		//	stopLoadingPhotoAtIndex: (NSInteger)photoIndex;
		[Export ("photoAlbumScrollView:stopLoadingPhotoAtIndex:")]
		void StopLoadingPhotoAtIndex (NIPhotoAlbumScrollView photoAlbumScrollView, int photoIndex);
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
	[BaseType (typeof (NIRecyclableView))]
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
		void SetImage ([NullAllowed]UIImage image, NIPhotoScrollViewPhotoSize photoSize);

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

	#region Launcher

	//@protocol NILauncherButtonView <NIRecyclableView>
	[BaseType(typeof(NSObject))]
	[Model]
	public partial interface NILauncherButtonViewProtocol : NIRecyclableViewProtocol
	{
		//@property (nonatomic, readwrite, NI_STRONG) UIButton* button;
		[Export("button")]
		UIButton Button { get; set; }
	}

	// @interface NILauncherButtonView : NIRecyclableView <NILauncherButtonView, NILauncherViewObjectView>
	[BaseType(typeof(NIRecyclableView))]
	public partial interface NILauncherButtonView : NILauncherButtonViewProtocol
	{
		//- (id)initWithReuseIdentifier:(NSString *)reuseIdentifier;
		[Export ("initWithReuseIdentifier:")]
		IntPtr Constructor (string reuseIdentifier);

		//@property (nonatomic, readwrite, copy) UILabel* label;
		[Export("label", ArgumentSemantic.Copy)]
		UILabel Label { get; set; }

		//@property (nonatomic, readwrite, assign) UIEdgeInsets contentInset;
		[Export("contentInset", ArgumentSemantic.Assign)]
		UIEdgeInsets ContentInset { get; set; }
	}

	[BaseType(typeof(NSObject))]
	[Model]
	public partial interface NILauncherViewObjectProtocol
	{
		//@property (nonatomic, readwrite, copy) NSString* title;
		[Export("title", ArgumentSemantic.Copy)]
		string Title { get; set; }

		//@property (nonatomic, readwrite, NI_STRONG) UIImage* image;
		[Export("image")]
		UIImage Image { get; set; }

		//@property (nonatomic, readwrite, NI_STRONG) UIImage* image;
		//- (Class)buttonViewClass;
	}

	//@interface NILauncherViewObject : NSObject <NILauncherViewObject, NSCoding>
	[BaseType(typeof(NSObject))]
	public partial interface NILauncherViewObject : NILauncherViewObjectProtocol
	{
		//- (id)initWithTitle:(NSString *)title image:(UIImage *)image;
		[Export ("initWithTitle:image:")]
		IntPtr Constructor (string title, UIImage image);

		//+ (id)objectWithTitle:(NSString *)title image:(UIImage *)image;
		[Export ("objectWithTitle:image:"), Static]
		NILauncherViewObject ObjectWithTitle (string title, UIImage image);
	}

	[BaseType(typeof(NSObject))]
	[Model]
	public partial interface NILauncherViewObjectView
	{
		//- (void)shouldUpdateViewWithObject:(id)object;
		[Export("shouldUpdateViewWithObject:")]
	 	void ShowUpdateViewWithObject (NSObject o);
	}

	//@interface NILauncherViewController : UIViewController <NILauncherDelegate, NILauncherDataSource>
	[BaseType(typeof(UIViewController))]
	public partial interface NILauncherViewController : NILauncherDelegate, NILauncherDataSource
	{
		//@property (nonatomic, readwrite, NI_STRONG) NILauncherView* launcherView;
		[Export("launcherView")]
		NILauncherView LauncherView { get; set; }
	}

	[BaseType(typeof(UIView))]
	public partial interface NILauncherView
	{
		//@property (nonatomic, readwrite, assign) NSInteger maxNumberOfButtonsPerPage; // Default: NSIntegerMax
		[Export("maxNumberOfButtonsPerPage", ArgumentSemantic.Assign)]
		int MaxNumberOfButtonsPerPage { get; set; }

		//@property (nonatomic, readwrite, assign) UIEdgeInsets contentInsetForPages; // Default: 10px on all sides
		[Export("contentInsetForPages", ArgumentSemantic.Assign)]
		UIEdgeInsets ContentInsetForPages { get; set; }

		//@property (nonatomic, readwrite, assign) CGSize buttonSize; // Default: 80x80
		[Export("buttonSize", ArgumentSemantic.Assign)]
		SizeF ButtonSize { get; set; }

		//@property (nonatomic, readwrite, assign) NSInteger numberOfRows; // Default: NILauncherViewGridBasedOnButtonSize
		[Export("numberOfRows", ArgumentSemantic.Assign)]
		int NumberOfRows { get; set; }

		//@property (nonatomic, readwrite, assign) NSInteger numberOfColumns; // Default: NILauncherViewGridBasedOnButtonSize
		[Export("numberOfColumns", ArgumentSemantic.Assign)]
		int NumberOfColumns { get; set; }

		//- (void)reloadData;
		[Export("reloadData")]
		void ReloadData();

		//@property (nonatomic, readwrite, NI_WEAK) id<NILauncherDelegate> delegate;
		[Export ("delegate"), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap("WeakDelegate")]
		NILauncherDelegate Delegate { get; set; }

		//@property (nonatomic, readwrite, NI_WEAK) id<NILauncherDataSource> dataSource;
		[Export("dataSource"), NullAllowed]
		NSObject WeakDataSource { get; set; }

		[Wrap("WeakDataSource")]
		NILauncherDataSource DataSource { get; set; }

		//- (UIView<NILauncherButtonView> *)dequeueReusableViewWithIdentifier:(NSString *)identifier;
		[Export("dequeueReusableViewWithIdentifier:")]
		UIView DequeueReusableViewWithIdentifier (string identifier);

		//- (void)willRotateToInterfaceOrientation:(UIInterfaceOrientation)toInterfaceOrientation duration:(NSTimeInterval)duration;
		[Export("willRotateToInterfaceOrientation:duration:")]
		void WillRotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation, double duration);

		//- (void)willAnimateRotationToInterfaceOrientation:(UIInterfaceOrientation)toInterfaceOrientation duration:(NSTimeInterval)duration;
		[Export("willAnimateRotationToInterfaceOrientation:duration:")]
		void WillAnimateRotationToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation, double duration);
	}

	//@protocol NILauncherDataSource <NSObject>
	[BaseType(typeof(NSObject))]
	[Model]
	public partial interface NILauncherDataSource
	{
		//- (NSInteger)launcherView:(NILauncherView *)launcherView numberOfButtonsInPage:(NSInteger)page;
		[Export("launcherView:numberOfButtonsInPage:"), Abstract]
		int NumberOfButtonsInPage (NILauncherView launcherView, int page);

		//- (UIView<NILauncherButtonView> *)launcherView:(NILauncherView *)launcherView buttonViewForPage:(NSInteger)page atIndex:(NSInteger)index;
		[Export("launcherView:buttonViewForPage:atIndex:"), Abstract]
		UIView ButtonViewForPage (NILauncherView launcherView, int page, int index);

		//- (NSInteger)numberOfPagesInLauncherView:(NILauncherView *)launcherView;
		[Export("numberOfPagesInLauncherView:")]
		int NumberOfPagesInLauncherView(NILauncherView launcherView);

		//- (NSInteger)numberOfRowsPerPageInLauncherView:(NILauncherView *)launcherView;
		[Export("numberOfRowsPerPageInLauncherView:")]
		int NumberOfRowsPerPageInLauncherView(NILauncherView launcherView);
	
		//- (NSInteger)numberOfColumnsPerPageInLauncherView:(NILauncherView *)launcherView;
		[Export("numberOfColumnsPerPageInLauncherView:")]
		int NumberOfColumnsPerPageInLauncherView(NILauncherView launcherView);
	}

	//@protocol NILauncherDelegate <NSObject>
	[BaseType(typeof(NSObject))]
	[Model]
	public partial interface NILauncherDelegate
	{
		//- (void)launcherView:(NILauncherView *)launcherView didSelectItemOnPage:(NSInteger)page atIndex:(NSInteger)index;
		[Export("launcherView:didSelectItemOnPage:atIndex:")]
		void DidSelectItemOnPage (NILauncherView launcherView, int page, int index);
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

		[Export("initWithRequest:")]
		IntPtr Constructor(NSUrlRequest request);

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

		//- (void)setCompletionBlockWithSuccess:(void (^)(AFHTTPRequestOperation *operation, id responseObject))success
		//failure:(void (^)(AFHTTPRequestOperation *operation, NSError *error))failure;
		[Export ("setCompletionBlockWithSuccess:failure:")]
		void SetCompletionBlockWithSuccess(AFHTTPClientRequestSuccess success, AFHTTPClientRequestFailure failure);
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

