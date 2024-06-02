using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Security.Cryptography;
using System.IO;
using System.Threading;
using Xamarin.Essentials;

namespace Gallery_Module40.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PinLogin : ContentPage
    {
        public PinLogin()
        {
            InitializeComponent();
        }

        private async void ButtonSubmit_Clicked(object sender, EventArgs e)
        {
            string pinCode = pinEntry.Text;

            // Проверяем, есть ли в словаре значение пина
            if (App.Current.Properties.TryGetValue("UserPIN", out object savedPin))
            {
                if (pinCode != (string)savedPin)
                {
                    pinEntry.Text = string.Empty;
                    await DisplayAlert("Error", "An incorrect PIN has been entered", "Ok");
                }
                else 
                { 
                    GetStarted();
                }
            }
            else
            {
                //Запрашиваем подтверждение
                var waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
                var modalPage = new PinReEntered();

                modalPage.Disappearing += (sender2, e2) =>
                {
                    waitHandle.Set();
                };
                await Navigation.PushModalAsync(modalPage);

                await Task.Run(() => waitHandle.WaitOne());

                string pinConf = modalPage.PinConfirmed;
                if (pinConf == null)
                {
                    return;
                }

                if (pinCode != pinConf)
                {
                    pinEntry.Text = string.Empty;
                    await DisplayAlert("Error", "The PIN codes do not match", "Ok");
                }
                else
                {
                    // Добавляем, если нет
                    App.Current.Properties.Add("UserPIN", pinCode);
                    await App.Current.SavePropertiesAsync();
                    GetStarted();
                }
            }
        }

        private async void GetStarted()
        {
            buttonSubmit.IsEnabled = false;
            await Navigation.PushAsync(new GalleryViewer());
        }
    }
}