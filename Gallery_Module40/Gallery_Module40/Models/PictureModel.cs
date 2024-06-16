using Java.Util;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;

namespace Gallery_Module40.Models
{
    public class PictureModel : INotifyPropertyChanged
    {
        private string imagePath;
        private string imageFileName;
        private string imageDate;
        private long id;

        public PictureModel() { }

        public string ImagePath
        {
            get
            {
                return imagePath;
            }
            
            set
            {
                imagePath = value;

                // Вызов уведомления при изменении
                OnPropertyChanged();
            }
        }

        public string ImageFileName
        { 
            get 
            { 
                return imageFileName; 
            } 
            
            set 
            { 
                imageFileName = value;

                // Вызов уведомления при изменении
                OnPropertyChanged();
            }
        }
        
        public string ImageDate 
        {  
            get 
            { 
                return imageDate; 
            } 
            
            set 
            {  
                imageDate = value;

                // Вызов уведомления при изменении
                OnPropertyChanged();
            }
        }

        public long Id
        { 
            get 
            { 
                return id; 
            } 
            
            set 
            { 
                id = value;

                // Вызов уведомления при изменении
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Делегат, указывающий на метод-обработчик события PropertyChanged, возникающего при изменении свойств компонента
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// Метод, вызывающий событие PropertyChanged
        /// </summary>
        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
