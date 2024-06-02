using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace Gallery_Module40.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PinReEntered : ContentPage
    {
        private string pinConf = null;

        public string PinConfirmed 
        { 
            get
            {
                return pinConf;
            }
}
        public PinReEntered()
        {
            InitializeComponent();
        }

        private async void ButtonOk_Clicked(object sender, EventArgs e)
        {
            pinConf = pinEntry.Text;
            await Navigation.PopModalAsync();
        }

        private async void ButtonCancel_Clicked(object sender, EventArgs e)
        {
            pinConf = null;
            await Navigation.PopModalAsync();
        }
    }
}