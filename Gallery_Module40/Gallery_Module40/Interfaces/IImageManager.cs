using Gallery_Module40.Models;
using Java.Nio.FileNio;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Gallery_Module40.Interfaces
{
    public interface IImageManager
    {
        //ObservableCollection<PictureModel> GetImages();
        List<PictureModel> GetImages();
        bool DeleteFile(PictureModel picture, ref string message);
        List<PictureModel> GetImageByName(string pictureName);
        string GetImagePath();
    }
}
