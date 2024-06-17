using Android.App;
using Android.Content;
using Android.Provider;
using Gallery_Module40.Interfaces;
using Gallery_Module40.Models;
using System;
using Xamarin.Forms;

using System.Collections.ObjectModel;
using Xamarin.Essentials;
using Android.Graphics;
using Java.Util;
using System.Collections.Generic;
using static Xamarin.Essentials.Platform;
using System.Security.Policy;

[assembly: Dependency(typeof(Gallery_Module40.Droid.ImageManagerAndroid))]
namespace Gallery_Module40.Droid
{
    public class ImageManagerAndroid : IImageManager
    {
        public List<PictureModel> GetImages() //ObservableCollection<PictureModel> GetImages()
        {
            //ObservableCollection<PictureModel> pictures = new ObservableCollection<PictureModel>();
            List<PictureModel> pictures = new List<PictureModel>();

            try
            {
                string[] projection =
                {
                    MediaStore.Images.Media.InterfaceConsts.Data,
                    MediaStore.Images.Media.InterfaceConsts.Title,
                    MediaStore.Images.Media.InterfaceConsts.DateTaken,
                    MediaStore.Images.Media.InterfaceConsts.Id,
                    MediaStore.Images.Media.InterfaceConsts.OwnerPackageName
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
                        var file = new System.IO.FileInfo(cursor.GetString(0));
                        PictureModel picture = new PictureModel()
                        {                            
                            ImagePath = cursor.GetString(0),
                            ImageFileName = cursor.GetString(1),
                            //ImageDate = FromMS(cursor.GetLong(2)).ToString(), //некорректно считает
                            ImageDate = file.CreationTime.ToString("dd.MM.yyyy hh.mm.ss"),
                            Id = cursor.GetLong(3),
                            ImageOwner = cursor.GetString(4),
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

        /// <summary>
        /// Метод удаления файла
        /// </summary>
        /// <param name="picture">Экземпляр класса с атрибутами целевой фотографии</param>
        /// <param name="message">Сообщение о результате работы метода</param>
        /// <returns>Результат работы</returns>
        public bool DeleteFile(PictureModel picture, ref string message)
        {
            if (!System.IO.File.Exists(picture.ImagePath))
            {
                message = "The file does not exist";
                return false;
            }    
                

            //Проверка возможности удаления
            if (!CanDeleteFile(picture.ImageOwner))
            {
                message = "It is not possible to delete the file because it was not created in this application";
                return false;
            }
                
            Android.Net.Uri fileUri = MediaStore.Images.Media.ExternalContentUri;
            string where = MediaStore.IMediaColumns.Data + "=?";
            string[] selectionArgs = new string[] { picture.ImagePath };

            try
            {
                //Само удаление
                int res = Android.App.Application.Context.ContentResolver.Delete(fileUri, where, selectionArgs);

                //Запрос на удаление
                //var uri = ContentUris.WithAppendedId(MediaStore.Images.Media.ExternalContentUri, picture.Id);
                //Android.Net.Uri[] uris = new Android.Net.Uri[] { uri };
                //PendingIntent pi = MediaStore.CreateDeleteRequest(Android.App.Application.Context.ContentResolver, uris);

                //И тут видимо надо сделать так, чтобы выскакивал запрос на разрешения, но хз как (runtime ask)
                //UPDATE: В API >=33 это походу невозможно
                //https://stackoverflow.com/questions/78050999/xamarin-forms-read-and-write-external-storage-issue-on-api33


                //// Уведомление медиа-сервера
                ////var uri = Android.Net.Uri.FromFile(new Java.IO.File(filePath));
                //var intent = new Intent(Intent.ActionMediaScannerScanFile, fileUri);
                //Android.App.Application.Context.SendBroadcast(intent);
            }
            catch (Exception e)
            {
                message = e.Message;
                return false;
            }

            //Повторная проверка
            if (System.IO.File.Exists(picture.ImagePath))
            {
                message = "The file could not be deleted";
                return false;
            }
                
            message = $"The image '{picture.ImageFileName}' has been deleted!";
            return true;
        }

        /// <summary>
        /// Получение пути к каталогу Pictures
        /// </summary>
        /// <returns></returns>
        public string GetImagePath()
        {
            return Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures).Path;
        }

        /// <summary>
        /// Проверка возможности удаления основываясь на пакете-владельце файла
        /// </summary>
        /// <param name="owner">Имя пакета-владельца</param>
        /// <returns>Результат</returns>
        private bool CanDeleteFile(string owner)
        {
            if (owner != AppInfo.PackageName)
            {
                return false;
            }

            return true;
        }

        public List<PictureModel> GetImageByName(string pictureName)
        {
            List<PictureModel> pictures = new List<PictureModel>();

            try
            {
                Android.Net.Uri fileUri = MediaStore.Images.Media.ExternalContentUri;
                
                string[] projection =
                {
                    MediaStore.Images.Media.InterfaceConsts.Data,
                    MediaStore.Images.Media.InterfaceConsts.Title,
                    MediaStore.Images.Media.InterfaceConsts.DateTaken,
                    MediaStore.Images.Media.InterfaceConsts.Id,
                    MediaStore.Images.Media.InterfaceConsts.OwnerPackageName
                };
                
                string where = MediaStore.IMediaColumns.Title + "=?";
                string[] selectionArgs = new string[] { pictureName };

                var cursor = Android.App.Application.Context.ContentResolver.Query(fileUri, projection, where, selectionArgs, null);

                int size = cursor.Count;

                if (size > 0)
                {
                    while (cursor.MoveToNext())
                    {
                        var file = new System.IO.FileInfo(cursor.GetString(0));
                        PictureModel picture = new PictureModel()
                        {
                            ImagePath = cursor.GetString(0),
                            ImageFileName = cursor.GetString(1),
                            //ImageDate = cursor.GetString(2),
                            ImageDate = file.CreationTime.ToString("dd.MM.yyyy hh.mm.ss"),
                            Id = cursor.GetLong(3),
                            ImageOwner = cursor.GetString(4),
                        };
                        pictures.Add(picture);
                    }
                    return pictures;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;                
            }
        }

        /// <summary>
        /// Получение даты их миллисекунд, начиная с 01.01.1970
        /// </summary>
        /// <param name="microSec"></param>
        /// <returns></returns>
        //private DateTime FromMS(long microSec)
        //{
        //    long milliSec = (long)(microSec / 1000);
        //    DateTime startTime = new DateTime(1970, 1, 1);

        //    TimeSpan time = TimeSpan.FromMilliseconds(milliSec);
        //    return startTime.Add(time);
        //}
    }
}