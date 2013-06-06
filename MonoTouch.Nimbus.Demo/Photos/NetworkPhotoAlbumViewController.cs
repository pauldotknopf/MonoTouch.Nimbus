using System;
using MonoTouch.Foundation;

namespace MonoTouch.Nimbus.Demo
{
	public class NetworkPhotoAlbumViewController : NIToolbarPhotoViewController
	{

		public NetworkPhotoAlbumViewController ()
		{
		}

		~NetworkPhotoAlbumViewController()
		{
			ShutdownNetworkPhotoAlbumViewController ();
		}

		public NIImageMemoryCache HighQualityImageCache{ get; private set; }

		public NIImageMemoryCache ThumbnailImageCache{ get; private set; }

		public NSOperationQueue Queue{ get; private set; }

		public NSMutableSet ActiveRequests{ get; private set; }

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			ActiveRequests = new NSMutableSet ();
			HighQualityImageCache = new NIImageMemoryCache ();
			ThumbnailImageCache = new NIImageMemoryCache ();

			HighQualityImageCache.MaxNumberOfPixels = 1024 * 1024 * 10;
			ThumbnailImageCache.MaxNumberOfPixels = 1024 * 1024 * 3;

			Queue = new NSOperationQueue ();
			Queue.MaxConcurrentOperationCount = 5;
		}

		protected void ShutdownNetworkPhotoAlbumViewController() {
			Queue.CancelAllOperations ();
		}

		protected string CacheKeyForPhotoIndex(int index)
		{
			return index.ToString ();
		}

		protected int IdentifierWithPhotoSize(NIPhotoScrollViewPhotoSize photoSize, int photoIndex)
		{
			var isThumbnail = photoSize == NIPhotoScrollViewPhotoSize.NIPhotoScrollViewPhotoSizeThumbnail;
			return isThumbnail ? -(photoIndex + 1) : photoIndex;
		}

		protected NSNumber IdentifierKeyFromIdentifier(int identifier)
		{
			return new NSNumber (identifier);
		}

		protected void RequestImageFromSource(string source, NIPhotoScrollViewPhotoSize photoSize, int photoIndex)
		{
			var isThumbnail = photoSize == NIPhotoScrollViewPhotoSize.NIPhotoScrollViewPhotoSizeThumbnail;
			var identifier = IdentifierWithPhotoSize (photoSize, photoIndex);
			var identifierKey = IdentifierKeyFromIdentifier (identifier);

			// avoid duplicate requests
			if(ActiveRequests.Contains(identifierKey))
				return;

			NSUrl url = null;
			if (source.StartsWith ("http")) {
				url = new NSUrl (source);
			} else {
				url = new NSUrl (source, true);
			}

			NSMutableUrlRequest request = new NSMutableUrlRequest (url);
			request.TimeoutInterval = 30;

			var photoIndexKey = CacheKeyForPhotoIndex (photoIndex);

			NSPropertyListReadOptions = 
		}

//		///////////////////////////////////////////////////////////////////////////////////////////////////
//		- (void)requestImageFromSource:(NSString *)source
//			photoSize:(NIPhotoScrollViewPhotoSize)photoSize
//				photoIndex:(NSInteger)photoIndex {
//			BOOL isThumbnail = (NIPhotoScrollViewPhotoSizeThumbnail == photoSize);
//			NSInteger identifier = [self identifierWithPhotoSize:photoSize photoIndex:photoIndex];
//			id identifierKey = [self identifierKeyFromIdentifier:identifier];
//
//			// Avoid duplicating requests.
//			if ([_activeRequests containsObject:identifierKey]) {
//				return;
//			}
//
//			NSURL* url = [NSURL URLWithString:source];
//			NSMutableURLRequest* request = [NSMutableURLRequest requestWithURL:url];
//			request.timeoutInterval = 30;
//
//			NSString* photoIndexKey = [self cacheKeyForPhotoIndex:photoIndex];
//
//			AFImageRequestOperation* readOp =
//				[AFImageRequestOperation imageRequestOperationWithRequest:request
//				 imageProcessingBlock:nil success:
//				 ^(NSURLRequest *request, NSHTTPURLResponse *response, UIImage *image) {
//					// Store the image in the correct image cache.
//					if (isThumbnail) {
//						[_thumbnailImageCache storeObject: image
//						 withName: photoIndexKey];
//
//					} else {
//						[_highQualityImageCache storeObject: image
//						 withName: photoIndexKey];
//					}
//
//					// If you decide to move this code around then ensure that this method is called from
//					// the main thread. Calling it from any other thread will have undefined results.
//					[self.photoAlbumView didLoadPhoto: image
//					 atIndex: photoIndex
//					 photoSize: photoSize];
//
//					if (isThumbnail) {
//						[self.photoScrubberView didLoadThumbnail:image atIndex:photoIndex];
//					}
//
//					[_activeRequests removeObject:identifierKey];
//
//				 } failure:
//				 ^(NSURLRequest *request, NSHTTPURLResponse *response, NSError *error) {
//
//				 }];
//
//			readOp.imageScale = 1;
//
//			// Set the operation priority level.
//
//			if (NIPhotoScrollViewPhotoSizeThumbnail == photoSize) {
//				// Thumbnail images should be lower priority than full-size images.
//				[readOp setQueuePriority:NSOperationQueuePriorityLow];
//
//			} else {
//				[readOp setQueuePriority:NSOperationQueuePriorityNormal];
//			}
//
//			// Start the operation.
//			[_activeRequests addObject:identifierKey];
//			[_queue addOperation:readOp];
//		}
	}
}

