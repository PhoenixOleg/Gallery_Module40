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
        private bool isRegistration = false; //признак, что пользователь зарегистрирован в приложении (пин код установлен)

        public PinLogin()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Устанавливаем текст запроса на ввод пин-кода (первичная регистрация или вход)
        /// </summary>
        protected override void OnAppearing()
        {
            // Проверяем, есть ли в словаре ключ для значения пина
            if (App.Current.Properties.ContainsKey("UserPIN"))
            {
                isRegistration = true;
                LabelPrompt.Text = "Entry PIN code:";
            }
            else
            {
                LabelPrompt.Text = "Set PIN code for registration:";                
            }

            base.OnAppearing();
        }

        private void ButtonSubmit_Clicked(object sender, EventArgs e)
        {
            CheckPin();
        }

        private void PinEntry_Completed(object sender, EventArgs e)
        {
            if (PinEntry.Text.Length >= 4)
            {
                CheckPin();
            }
        }

        private async void CheckPin()
        {
            string pinCode = PinEntry.Text;

            if (isRegistration == true)
            {
                //Если пин уже установлен проверяем его валидность
                App.Current.Properties.TryGetValue("UserPIN", out object savedPin);
                if (pinCode != (string)savedPin)
                {
                    PinEntry.Text = string.Empty;
                    InfoMsg.Text = string.Empty;
                    await DisplayAlert("Error", "An incorrect PIN has been entered", "Ok");
                }
                else
                {
                    //На период отладки убиваем пин после успешного входа
                    //App.Current.Properties.Remove("UserPIN"); 
                    //await App.Current.SavePropertiesAsync();
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
                    PinEntry.Text = string.Empty;
                    InfoMsg.Text = string.Empty;
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
            ButtonSubmit.IsEnabled = false;
            await Navigation.PushAsync(new GalleryViewer());
        }

        private void Password_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (PinEntry.Text.Length < 4)
            {
                InfoMsg.Text = "The PIN code must be between 4 and 8 characters long";
                ButtonSubmit.IsEnabled = false; //Если ввели нормальный пин, а потом сохратили до 3 или менее символов
            }
            else
            {
                InfoMsg.Text = string.Empty;
                ButtonSubmit.IsEnabled = true;
            }
        }
    }
}