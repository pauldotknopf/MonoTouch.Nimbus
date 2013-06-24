using System;
using MonoTouch.Dialog;

namespace MonoTouch.Nimbus.Demo.Photos
{
	public class CatalogController : MonoTouch.Dialog.DialogViewController
	{
		public CatalogController ()
			:base(new RootElement("Photo Album Catalog"))
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			Root.Add (new Section("Dribbble"){
				new StyledStringElement("Popular Shots", () => { 
					this.NavigationController.PushViewController(new DribblePhotoAlbumViewController("shots"), true);
				})
			});
		}
	}
}

