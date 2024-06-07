using Gallery_Module40.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Gallery_Module40.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ImageViewer : ContentPage
	{
		public ImageViewer(Picture picture)
        {
			InitializeComponent();
			
			this.BindingContext = picture;
		}
	}
}