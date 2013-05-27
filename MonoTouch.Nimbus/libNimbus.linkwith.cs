using System;
using MonoTouch.ObjCRuntime;

[assembly: LinkWith ("libNimbus.a", 
                     LinkTarget.ArmV7 | LinkTarget.Simulator, 
                     ForceLoad = true,
                     Frameworks = "Foundation UIKit QuartzCore CoreText")]