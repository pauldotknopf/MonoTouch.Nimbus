using System;
using MonoTouch.Dialog;

namespace MonoTouch.Nimbus.Demo
{
	public class PhotosCatalogController : MonoTouch.Dialog.DialogViewController
	{
		public PhotosCatalogController ()
			:base(new RootElement("Photo Album Catalog"))
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			Root.Add (new Section("Dribbble"){
				new StyledStringElement("Popular Shots", () => { 
					this.NavigationController.PushViewController(new PhotosDribblePhotoAlbumViewController("shots"), true);
				})
			});
		}
	}
}

