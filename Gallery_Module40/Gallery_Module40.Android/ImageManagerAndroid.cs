using Android;
using Android.App;
using Android.Content;
using AndroidX.Activity.Result.Contract;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using Gallery_Module40.Interfaces;
using Gallery_Module40.Models;
using Java.Nio.FileNio;
using Java.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using static Android.Content.IntentSender;
using Xamarin.Forms.PlatformConfiguration;
using static Android.Provider.MediaStore;
using System.Collections.ObjectModel;

[assembly: Dependency(typeof(Gallery_Module40.Droid.ImageManagerAndroid))]
namespace Gallery_Module40.Droid
{
    public class ImageManagerAndroid : IImageManager
    {
        public ObservableCollection<PictureModel> GetImages()
        {
            ObservableCollection<PictureModel> pictures = new ObservableCollection<PictureModel>();

            try
            {
                string[] projection =
                {
                    MediaStore.Images.Media.InterfaceConsts.Data,
                    MediaStore.Images.Media.InterfaceConsts.Title,
                    MediaStore.Images.Media.InterfaceConsts.DateAdded,
                    MediaStore.Images.Media.InterfaceConsts.Id
                };

                var cursor = Android.App.Application.Context.ContentResolver.Query(MediaStore.Images.Media.ExternalContentUri,
                                                   projection,
                                                   null,
                                                   null,
                                                   null);

                int size = cursor.Count;

                if (size > 0)
                {
                    while (cursor.MoveToNext())
                    {
                        PictureModel picture = new PictureModel()
                        {
                            ImagePath = cursor.GetString(0),
                            ImageFileName = cursor.GetString(1),
                            ImageDate = cursor.GetString(2),
                            Id = cursor.GetLong(3)
                        };
                        pictures.Add(picture);
                    }
                }

                return pictures;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteFile(PictureModel picture)
        {
            Android.Net.Uri fileUri;
            fileUri = MediaStore.Images.Media.ExternalContentUri;
            string where = MediaStore.IMediaColumns.Data + "=?";
            string[] selectionArgs = new string[] { picture.ImagePath };

            try
            {
                var uri = ContentUris.WithAppendedId(MediaStore.Images.Media.ExternalContentUri, picture.Id);
                Android.Net.Uri[] uris = new Android.Net.Uri[] { uri };

                //Само удаление
                int res = Android.App.Application.Context.ContentResolver.Delete(fileUri, where, selectionArgs);
                //Запрос на удаление
                PendingIntent pi = MediaStore.CreateDeleteRequest(Android.App.Application.Context.ContentResolver, uris);

                //И тут видимо надо сделать так, чтобы выскакивал запрос на разрешения, но хз как
            }
            catch (Exception e)
            {
                var a = e.Message;
            }

            #region TestSearcher Для проверки наличия файла

            string[] projection =
{
                    MediaStore.Images.Media.InterfaceConsts.Data,
                    MediaStore.Images.Media.InterfaceConsts.Title,
                    MediaStore.Images.Media.InterfaceConsts.DateAdded,
                    MediaStore.Images.Media.InterfaceConsts.Id
                };

            var cursor = Android.App.Application.Context.ContentResolver.Query(fileUri, projection, where, selectionArgs, null);

                int size = cursor.Count;

                if (size > 0)
                {
                    while (cursor.MoveToNext())
                    {
                        PictureModel pictureTest = new PictureModel()
                        {                           
                            ImagePath = cursor.GetString(0),
                            ImageFileName = cursor.GetString(1),
                            ImageDate = cursor.GetString(2),
                            Id = cursor.GetLong(3)
                        };
                    }
                }
            #endregion TestSearcher
        }
    }
}