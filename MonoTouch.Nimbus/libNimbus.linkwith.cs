using System;
using MonoTouch.ObjCRuntime;

[assembly: LinkWith ("libNimbus.a", 
                     LinkTarget.ArmV7 | LinkTarget.Simulator, 
                     ForceLoad = true,
                     Frameworks = "Foundation UIKit QuartzCore")]


// 	core
// 		Foundation.framework
//		UIKit.framework
// 	pagingscrollview
//		Foundation.framework
//		UIKit.framework
//	launcher
//		Foundation.framework
//		UIKit.framework
//		QuartzCore.framework