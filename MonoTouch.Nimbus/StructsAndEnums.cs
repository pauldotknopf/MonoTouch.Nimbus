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
}

