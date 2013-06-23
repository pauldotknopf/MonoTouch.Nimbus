using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace MonoTouch.Nimbus.Demo
{
	public class NetworkPhotoAlbumViewController : NIToolbarPhotoViewController
	{

		public NetworkPhotoAlbumViewController ()
		{
		}

		~NetworkPhotoAlbumViewController()
		{
			NSThread.MainThread.InvokeOnMainThread (() => {
				Queue.CancelAllOperations ();
			});
		}

		public NIImageMemoryCache HighQualityImageCache{ get; private set; }

		public NIImageMemoryCache ThumbnailImageCache{ get; private set; }

		public NSOperationQueue Queue{ get; private set; }

		public NSMutableSet ActiveRequests{ get; private set; }

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			if(!IsViewLoaded)
			{

			}
		}

		public override void LoadView ()
		{
			base.LoadView ();

			ActiveRequests = new NSMutableSet ();
			HighQualityImageCache = new NIImageMemoryCache ();
			ThumbnailImageCache = new NIImageMemoryCache ();

			HighQualityImageCache.MaxNumberOfPixels = 1024 * 1024 * 10;
			ThumbnailImageCache.MaxNumberOfPixels = 1024 * 1024 * 3;

			Queue = new NSOperationQueue ();
			Queue.MaxConcurrentOperationCount = 5;

			PhotoAlbumView.LoadingImage = UIImage.FromFile ("NimbusPhotos.bundle/gfx/default.png");
		}

		protected void ShutdownNetworkPhotoAlbumViewController ()
		{
			try{
			Queue.CancelAllOperations ();
			}catch(Exception ex) {
				Console.WriteLine (ex.Message);
			}
		}

		protected string CacheKeyForPhotoIndex (int index)
		{
			return index.ToString ();
		}

		protected int IdentifierWithPhotoSize (NIPhotoScrollViewPhotoSize photoSize, int photoIndex)
		{
			var isThumbnail = photoSize == NIPhotoScrollViewPhotoSize.NIPhotoScrollViewPhotoSizeThumbnail;
			return isThumbnail ? -(photoIndex + 1) : photoIndex;
		}

		protected NSNumber IdentifierKeyFromIdentifier (int identifier)
		{
			return new NSNumber (identifier);
		}

		protected void RequestImageFromSource (string source, NIPhotoScrollViewPhotoSize photoSize, int photoIndex)
		{
			var isThumbnail = photoSize == NIPhotoScrollViewPhotoSize.NIPhotoScrollViewPhotoSizeThumbnail;
			var identifier = IdentifierWithPhotoSize (photoSize, photoIndex);
			var identifierKey = IdentifierKeyFromIdentifier (identifier);

			// avoid duplicate requests
			if (ActiveRequests.Contains (identifierKey))
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



			var readOp = AFImageRequestOperation.ImageRequestOperationWithRequest (request, null, 
       			(NSUrlRequest req, NSHttpUrlResponse resp, UIImage img) => 
               	{
					// Store the image in the correct image cache.
					if (isThumbnail) {
						ThumbnailImageCache.StoreObject(img, photoIndexKey);

					} else {
						HighQualityImageCache.StoreObject(img, photoIndexKey);
					}
					// If you decide to move this code around then ensure that this method is called from
					// the main thread. Calling it from any other thread will have undefined results.
					PhotoAlbumView.DidLoadPhoto(img, photoIndex, photoSize);
					
					if(isThumbnail) {
						if(PhotoScrubberView != null)
							PhotoScrubberView.DidLoadThumbnail(img, photoIndex);
					}

					this.ActiveRequests.Remove(identifierKey);

				}, (NSUrlRequest req, NSHttpUrlResponse resp, NSError er) => {

				});

			readOp.ImageScale = 1;

			// Set the operation priority level.
			if(photoSize == NIPhotoScrollViewPhotoSize.NIPhotoScrollViewPhotoSizeThumbnail)
			{
				readOp.QueuePriority = NSOperationQueuePriority.Low;
			}else
			{
				readOp.QueuePriority = NSOperationQueuePriority.Normal;
			}
					
			// Start the operation.
			ActiveRequests.Add(identifierKey);
			Queue.AddOperation(readOp);
		}
	}
}

