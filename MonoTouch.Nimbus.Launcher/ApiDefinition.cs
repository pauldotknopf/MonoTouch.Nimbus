using System;
using System.Drawing;

using MonoTouch.ObjCRuntime;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace MonoTouch.Nimbus.Launcher
{
	[BaseType(typeof(UIViewController))]
	interface NILauncherViewController
	{
	}

	/**
	* The launcher delegate used to inform of state changes and user interactions.
	*
	* @ingroup NimbusLauncher
	*/
	// @protocol NILauncherDelegate <NSObject>
	[BaseType(typeof(NSObject))]
	[Model]
	interface NILauncherDelegate
	{
		/**
	 	* Informs the receiver that the specified item on the specified page has been selected.
	 	*
	 	*      @param launcherView A launcher-view object informing the delegate about the new item
	 	*                          selection.
	 	*      @param page A page index locating the selected item in @c launcher.
	 	*      @param index An index locating the selected item in the given page.
	 	*/
		//- (void)launcherView:(NILauncherView *)launcherView didSelectItemOnPage:(NSInteger)page atIndex:(NSInteger)index;

		[Export("launcherView:didSelectItemOnPage")]
		void LauncherView(NILauncherView launcherView, int atIndex);

		//[Export("photoEditor:finishedWithImage:"), EventArgs("AFPhotoEditor")]
		//void PhotoEditor( AFPhotoEditorController editor, UIImage image);
		//- (void)photoEditor:(AFPhotoEditorController *)editor finishedWithImage:(UIImage *)image;
	}

	[BaseType(typeof(UIView))]
	interface NILauncherView
	{
	}

	// The first step to creating a binding is to add your native library ("libNativeLibrary.a")
	// to the project by right-clicking (or Control-clicking) the folder containing this source
	// file and clicking "Add files..." and then simply select the native library (or libraries)
	// that you want to bind.
	//
	// When you do that, you'll notice that MonoDevelop generates a code-behind file for each
	// native library which will contain a [LinkWith] attribute. MonoDevelop auto-detects the
	// architectures that the native library supports and fills in that information for you,
	// however, it cannot auto-detect any Frameworks or other system libraries that the
	// native library may depend on, so you'll need to fill in that information yourself.
	//
	// Once you've done that, you're ready to move on to binding the API...
	//
	//
	// Here is where you'd define your API definition for the native Objective-C library.
	//
	// For example, to bind the following Objective-C class:
	//
	//     @interface Widget : NSObject {
	//     }
	//
	// The C# binding would look like this:
	//
	//     [BaseType (typeof (NSObject))]
	//     interface Widget {
	//     }
	//
	// To bind Objective-C properties, such as:
	//
	//     @property (nonatomic, readwrite, assign) CGPoint center;
	//
	// You would add a property definition in the C# interface like so:
	//
	//     [Export ("center")]
	//     PointF Center { get; set; }
	//
	// To bind an Objective-C method, such as:
	//
	//     -(void) doSomething:(NSObject *)object atIndex:(NSInteger)index;
	//
	// You would add a method definition to the C# interface like so:
	//
	//     [Export ("doSomething:atIndex:")]
	//     void DoSomething (NSObject object, int index);
	//
	// Objective-C "constructors" such as:
	//
	//     -(id)initWithElmo:(ElmoMuppet *)elmo;
	//
	// Can be bound as:
	//
	//     [Export ("initWithElmo:")]
	//     IntPtr Constructor (ElmoMuppet elmo);
	//
	// For more information, see http://docs.xamarin.com/ios/advanced_topics/binding_objective-c_types
	//
}

