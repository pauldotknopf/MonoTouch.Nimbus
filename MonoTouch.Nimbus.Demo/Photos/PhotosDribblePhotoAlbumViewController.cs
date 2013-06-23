using System;
using MonoTouch.Foundation;
using System.Drawing;
using System.Collections.Generic;
using MonoTouch.UIKit;

namespace MonoTouch.Nimbus.Demo
{
	public class PhotosDribblePhotoAlbumViewController : NetworkPhotoAlbumViewController
	{
		DribblePhotoAlbumDataSource _albumDataSource;
		DribblePhotoScrubberDataSource _scrubberDataSource;
		List<PhotoInfo> _photos;
		string _apiPath;

		public PhotosDribblePhotoAlbumViewController (string apiPath)
		{
			_apiPath = apiPath;
		}

		public class CustomPhotoAlbumScrollViewDelegate : NIPhotoAlbumScrollViewDelegate
		{
		}

		public override void LoadView ()
		{
			base.LoadView ();

			_albumDataSource = new DribblePhotoAlbumDataSource (this);
			_scrubberDataSource = new DribblePhotoScrubberDataSource (this);
			PhotoAlbumView.DataSource = _albumDataSource;
			PhotoAlbumView.Delegate = new CustomPhotoAlbumScrollViewDelegate ();
			if (PhotoScrubberView != null)
				PhotoScrubberView.DataSource = _scrubberDataSource;

			// Dribbble is for mockups and designs, so we don't want to allow the photos to be zoomed
			PhotoAlbumView.ZoomingAboveOriginalSizeIsEnabled = false;

			// This title will be displayed until we get the results back for the album information.
			Title = "Loading..";

			LoadAlbumInformation ();
		}

		private void LoadAlbumInformation ()
		{
			var albumUrlPath = "http://api.dribbble.com/" + _apiPath; 

			// Nimbus processors allow us to perform complex computations on a separate thread before
			// returning the object to the main thread. This is useful here because we perform sorting
			// operations and pruning on the results.
			var url = new NSUrl (albumUrlPath);
			var request = new NSMutableUrlRequest (url);

//			public delegate void ImageRequestOperationWithRequestSuccess1(UIImage image);
//			public delegate void ImageRequestOperationWithRequestSuccess2(NSUrlRequest request, NSHttpUrlResponse response, UIImage image);
//			public delegate void ImageRequestOperationWithRequestFailure(NSUrlRequest request, NSHttpUrlResponse response, NSError error);
//			public delegate UIImage ImageRequestOperationWithRequestProcessingBlock(UIImage image);
//			public delegate void AFJSONRequestOperationJsonRequestOperationWithRequestSuccess(NSUrlRequest request, NSHttpUrlResponse response, NSObject json);
//			public delegate void AFJSONRequestOperationJsonRequestOperationWithRequestFailure(NSUrlRequest request, NSHttpUrlResponse response, NSError error, NSObject json);
//			[BaseType (typeof (AFHTTPRequestOperation))]

			var albumRequest = AFJSONRequestOperation.JsonRequestOperationWithRequest (request, 
			                                                                           (req, res, json) => {
				BlockForAlbumProcessing (req, res, (NSDictionary)json);
			}, (req, resp, error, json) => {
				Console.WriteLine ("Error");
			});

			Queue.AddOperation (albumRequest);
		}

		private void BlockForAlbumProcessing (NSUrlRequest request, NSHttpUrlResponse response, NSDictionary json)
		{
			NSArray data = (NSArray)json.ObjectForKey (new NSString("shots"));

			var photos = new List<PhotoInfo> ();

			for (var x = 0; x < data.Count; x++) {
				var photo = new NSDictionary (data.ValueAt((uint)x));

				// Gather the high-quality photo information.
				var originalImageSource = (NSString)photo.ObjectForKey (new NSString("image_url"));
				var thumbnailImageSource = (NSString)photo.ObjectForKey (new NSString("image_teaser_url"));
				var width = (NSNumber)photo.ObjectForKey (new NSString("width"));
				var height = (NSNumber)photo.ObjectForKey (new NSString("height"));

				// We gather the highest-quality photo's dimensions so that we can size the thumbnails
				// correctly until the high-quality image is downloaded.
				var dimensions = new SizeF (width.Int32Value, height.Int32Value);

				photos.Add (new PhotoInfo{
					OriginalSource = originalImageSource.ToString(),
					ThumbnailSoruce = thumbnailImageSource.ToString(),
					Dimensions = dimensions
				});
			}

			_photos = photos;

			LoadThumbnails ();

			_albumDataSource.Photos = _photos;
			_scrubberDataSource.Photos = _photos;

			PhotoAlbumView.ReloadData ();
			if (PhotoScrubberView != null)
				PhotoScrubberView.ReloadData ();

			RefreshChromeState ();
		}

		private void LoadThumbnails ()
		{
			foreach (var photo in _photos) {

				var index = _photos.IndexOf (photo);
				var photoIndexKey = CacheKeyForPhotoIndex (index);

				// donl't load the thumbnail if it's already in memory
				if (!ThumbnailImageCache.ContainsObjectWithName (photoIndexKey)) {
					RequestImageFromSource (photo.OriginalSource, NIPhotoScrollViewPhotoSize.NIPhotoScrollViewPhotoSizeUnknown, index);
				}

			}
		}

		public class DribblePhotoAlbumDataSource : NIPhotoAlbumScrollViewDataSource
		{
			PhotosDribblePhotoAlbumViewController _controller;

			public DribblePhotoAlbumDataSource (PhotosDribblePhotoAlbumViewController controller)
			{
				_controller = controller;	
			}

			public List<PhotoInfo> Photos { get; set; }

			public override NSObject PhotoAlbumScrollView (NIPhotoAlbumScrollView photoAlbumScrollView, int photoIndex, out NIPhotoScrollViewPhotoSize photoSize, out bool isLoading, out SizeF originalPhotoDimensions)
			{
				isLoading = false;
				photoSize = NIPhotoScrollViewPhotoSize.NIPhotoScrollViewPhotoSizeUnknown;

				UIImage image;

				var photoIndexKey = _controller.CacheKeyForPhotoIndex (photoIndex);

				var photo = Photos [photoIndex];

				originalPhotoDimensions = photo.Dimensions;

				image = (UIImage)_controller.HighQualityImageCache.ObjectWithName (photoIndexKey);

				if (image != null) {
					photoSize = NIPhotoScrollViewPhotoSize.NIPhotoScrollViewPhotoSizeOriginal;
				} else {
					var source = photo.OriginalSource;
					_controller.RequestImageFromSource (source, NIPhotoScrollViewPhotoSize.NIPhotoScrollViewPhotoSizeOriginal, photoIndex);
					isLoading = true;

					// try to load the thumbnail if we can
					image = (UIImage)_controller.ThumbnailImageCache.ObjectWithName (photoIndexKey);
					if (image != null) {
						photoSize = NIPhotoScrollViewPhotoSize.NIPhotoScrollViewPhotoSizeThumbnail;
					} else {
						_controller.RequestImageFromSource(photo.ThumbnailSoruce, NIPhotoScrollViewPhotoSize.NIPhotoScrollViewPhotoSizeThumbnail, photoIndex);
					}
				}
				return image;
			}

			public override int NumberOfPagesInPagingScrollView (NIPagingScrollView pagingScrollView)
			{
				return Photos.Count;
			}

			public override NSObject PagingScrollView (NIPagingScrollView pagingScrollView, int pageIndex)
			{
				var result = (NIPhotoScrollView)_controller.PhotoAlbumView.PagingScrollView (pagingScrollView, pageIndex);
				result.PhotoScrollViewDelegate = new CustomScrollViewDelegate ();
				return result;
			}
		}

		public class CustomScrollViewDelegate : NIPhotoScrollViewDelegate
		{
		}

		public class DribblePhotoScrubberDataSource : NIPhotoScrubberViewDataSource
		{
			PhotosDribblePhotoAlbumViewController _controller;

			public DribblePhotoScrubberDataSource (PhotosDribblePhotoAlbumViewController controller)
			{
				_controller = controller;	
			}

			public List<PhotoInfo> Photos { get; set; }

			public override int NumberOfPhotosInScrubberView (NIPhotoScrubberView photoScrubberView)
			{
				return Photos.Count;
			}

			public override MonoTouch.UIKit.UIImage PhotoScrubberView (NIPhotoScrubberView photoScrubberView, int thumbnailIndex)
			{
				var photoIndexKey = _controller.CacheKeyForPhotoIndex (thumbnailIndex);

				var image = (UIImage)_controller.ThumbnailImageCache.ObjectWithName (photoIndexKey);
				if (image == null) {
					var photo = Photos [thumbnailIndex];
					_controller.RequestImageFromSource (photo.ThumbnailSoruce, NIPhotoScrollViewPhotoSize.NIPhotoScrollViewPhotoSizeThumbnail, thumbnailIndex);
				}

				return image;
			}
		}

		public class PhotoInfo
		{
			public string OriginalSource { get; set; }

			public string ThumbnailSoruce { get; set; }

			public SizeF Dimensions { get; set; }
		}
	}
}

