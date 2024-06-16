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

        private void GetImages()
        {
            Pictures = DependencyService.Get<IImageManager>().GetImages();
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

            //var answer = await DisplayAlert("Confirmation", $"Do you really want to remove the image '{picForRemove.ImageFileName}'", "Yes", "No");
            //if (answer == false)
            //{
            //    return;
            //}

            Pictures.Remove(picForRemove);
            DependencyService.Get<IImageManager>().DeleteFile(picForRemove);

            //try
            //{              

            //    }

            //    // Уведомляем пользователя
            //    await DisplayAlert(null, $"The image '{picForRemove.ImageFileName}' has been deleted", "ОК");
            //    }
            //    else
            //    {
            //        await DisplayAlert("Warning", "The image was not found. It may have already been deleted", "ОК");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    await DisplayAlert("Error", ex.Message, "ОК");
            //}

            PictureList.SelectedItem = null;
        }

        private void ButtonRefresh_Clicked(object sender, EventArgs e)
        {
            //Наданный момент не работает - зависимость походу отвязывается
            //Pictures.Clear();
            GetImages();
            PictureList.SelectedItem = null;
        }
    }
}
