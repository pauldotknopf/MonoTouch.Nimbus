using System;

namespace MonoTouch.Nimbus
{
	public enum  NINetworkImageViewScaleOptions: uint {
		NINetworkImageViewScaleToFitLeavesExcessAndScaleToFillCropsExcess = 0,
		NINetworkImageViewScaleToFitCropsExcess = 1,
		NINetworkImageViewScaleToFillLeavesExcess = 2
	}

	public enum NIPagingScrollViewType {
		NIPagingScrollViewHorizontal = 0,
		NIPagingScrollViewVertical = 1
	}

	public enum NIPhotoScrollViewPhotoSize {
		NIPhotoScrollViewPhotoSizeUnknown = 0,
		NIPhotoScrollViewPhotoSizeThumbnail = 1,
		NIPhotoScrollViewPhotoSizeOriginal = 2
	}

	public enum AFHTTPClientParameterEncoding {
		AFFormURLParameterEncoding = 0,
		AFJSONParameterEncoding = 1,
		AFPropertyListParameterEncoding = 2
	}
}

