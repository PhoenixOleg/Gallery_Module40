using Gallery_Module40.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Android.Content.PM;
using Xamarin.Forms.PlatformConfiguration;
using Android.Content;
using Gallery_Module40.Interfaces;
using Xamarin.Essentials;
using Android.Media;
using Android.Provider;
using Android.App;

namespace Gallery_Module40.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GalleryViewer : ContentPage
    {
        public ObservableCollection<PictureModel> Pictures { get; set; } = new ObservableCollection<PictureModel> { };

        public GalleryViewer()
        {
            InitializeComponent();

            GetImages();
            BindingContext = this;
        }

        protected async override void OnAppearing()
        {
            if (Device.RuntimePlatform != Device.Android)
            {
                await DisplayAlert("Warning", "This application has been tested only for the Android platform", "ОК");
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }

            base.OnAppearing();
        }

        private void GetImages()
        {
            Pictures.Clear();
            foreach (var picture in DependencyService.Get<IImageManager>().GetImages())
            {
                Pictures.Add(picture);
            }
        }

        private async void ButtonOpen_Clicked(object sender, EventArgs e)
        {
            if (PictureList.SelectedItem is null)
                return;

            await Navigation.PushAsync(new ImageViewer((PictureModel)PictureList.SelectedItem));
        }

        private async void ButtonDelete_Clicked(object sender, EventArgs e)
        {
            PictureModel picForRemove = (PictureModel)PictureList.SelectedItem;

            if (picForRemove == null)
            {
                await DisplayAlert("Warning", "The image has not been selected for deletion", "ОК");
                return;
            }

            var answer = await DisplayAlert("Confirmation", $"Do you really want to remove the image '{picForRemove.ImageFileName}'", "Yes", "No");
            if (answer == false)
            {
                return;
            }

            string message = string.Empty;

            bool result = DependencyService.Get<IImageManager>().DeleteFile(picForRemove, ref message);

            if (result == false)
            {
                await DisplayAlert("Error", message, "ОК");
            }
            else
            {
                Pictures.Remove(picForRemove);
                PictureList.SelectedItem = null;
                await DisplayAlert(null, message, "ОК");
            }

        }

        private void ButtonRefresh_Clicked(object sender, EventArgs e)
        {
            GetImages();
            PictureList.SelectedItem = null;
        }

        private async void ButtonGetFoto_Clicked(object sender, EventArgs e)
        {
            try
            {
                string title = App.Current.ToString() + "_" + DateTime.Now.ToString("yyyy.MM.dd_hh.mm.ss");
                string newFileName = title + ".jpeg";
                var photo = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions
                {
                    Title = newFileName
                });

                string dcimFolder = DependencyService.Get<IImageManager>().GetImagePath();

                string newFilePath = Path.Combine(dcimFolder, newFileName);
                using (var stream = await photo.OpenReadAsync())
                using (var newStream = File.OpenWrite(newFilePath))
                {
                    await stream.CopyToAsync(newStream);
                }

                List<PictureModel> newPictures = DependencyService.Get<IImageManager>().GetImageByName(title);

                if (newPictures != null && newPictures.Count > 0)
                {
                    foreach (var picture in newPictures)
                    {
                        Pictures.Add(picture);
                    }

                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
