using Gallery_Module40.Pages;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Gallery_Module40
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new PinLogin());
            //MainPage = new NavigationPage(new GalleryViewer());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
