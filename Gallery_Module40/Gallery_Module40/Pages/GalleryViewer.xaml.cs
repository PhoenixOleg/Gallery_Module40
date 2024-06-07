using Android.App;
using Android.Content;
using Android.Media;
using Android.Provider;
using Android.Runtime;
using Android.Webkit;
using Gallery_Module40.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using static Android.Media.MediaDrm;

namespace Gallery_Module40.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GalleryViewer : ContentPage
    {
        private readonly List<SourceFolder> SourceFolders = new List<SourceFolder>();
        public ObservableCollection<Picture> Pictures { get; set; } = new ObservableCollection<Picture> { };

        public GalleryViewer()
        {
            InitializeComponent();

            GetTargetFolder();
            GetImages();

            BindingContext = this;
        }

        private void GetImages()
        {
            foreach (var folder in SourceFolders)
            {
                foreach (var file in new DirectoryInfo(folder.folderName).GetFiles())
                {
                    Pictures.Add(new Picture
                    {
                        ImagePath = file.FullName,
                        ImageFileName = file.Name,
                        ImageDate = file.CreationTime.ToLongDateString(),
                        ImageSource = folder.sourceName
                    });
                }
            }
        }

        private async void GetTargetFolder()
        {
            SourceFolders.Clear();

            if (chkDism.IsChecked)
            {
                var dcimFolder = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDcim).Path;
                if (Directory.Exists(dcimFolder))
                {
                    SourceFolders.Add(new SourceFolder(dcimFolder, "DISM"));
                }
                else
                {
                    await DisplayAlert("Warning", "The path to the Photos (Dism) does not exist", "Ok");
                }
            }

            if (chkPicture.IsChecked)
            {
                var picFolder = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures).Path;
                //var res = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures).SetWritable(true, true);
                //var resCanWrite = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures).CanWrite();

                if (Directory.Exists(picFolder))
                {
                    SourceFolders.Add(new SourceFolder(picFolder, "PIC"));
                }
                else
                {
                    await DisplayAlert("Warning", "The path to the Pictures does not exist", "Ok");
                }
            }

            if (chkScreenshots.IsChecked)
            {
                var scrFolder = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryScreenshots).Path;
                if (Directory.Exists(scrFolder))
                {
                    SourceFolders.Add(new SourceFolder(scrFolder, "SCR"));
                }
                else
                {
                    await DisplayAlert("Warning", "The path to the Screenshots does not exist", "Ok");
                }
            }
        }

        private void CheckBoxes_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            Pictures.Clear();
            GetTargetFolder();
            GetImages();
            PictureList.SelectedItem = null;
        }

        private async void ButtonOpen_Clicked(object sender, EventArgs e)
        {
            if (PictureList.SelectedItem is null)
                return;

            await Navigation.PushAsync(new ImageViewer((Picture)PictureList.SelectedItem));
        }

        private async void ButtonDelete_Clicked(object sender, EventArgs e)
        {
            Picture picForRemove = (Picture)PictureList.SelectedItem;

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

            try
            {
                if (File.Exists(picForRemove.ImagePath))
                {
                    File.Delete(picForRemove.ImagePath);
                    Pictures.Remove(picForRemove);

                    // Уведомляем пользователя
                    await DisplayAlert(null, $"The image '{picForRemove.ImageFileName}' has been deleted", "ОК");
                }
                else
                {
                    await DisplayAlert("Warning", "The image was not found. It may have already been deleted", "ОК");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "ОК");
            }

            PictureList.SelectedItem = null;
        }
    }
}
