using NailBars.VistasModelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NailBars.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VLogin : ContentPage
    {
        public VLogin()
        {
            InitializeComponent();
            var statusbar = new VMPrincipal();
            statusbar.Translucido();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            DisplayAlert("Hola", "Hola", "Ok");
        }

        private async void GoBackTapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}