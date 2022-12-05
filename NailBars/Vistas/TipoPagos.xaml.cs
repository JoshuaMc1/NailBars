using NailBars.Components;
using NailBars.VistasModelo;
using Rg.Plugins.Popup.Extensions;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NailBars.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TipoPagos : ContentPage
    {
        public TipoPagos()
        {
            InitializeComponent();
            var statusbar = new VMPrincipal();
            statusbar.CambiarColor();
        }

        private async void ButtonPagarSitio_Clicked(object sender, EventArgs e)
        {
            await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Información", "Gracias por su reservación.", JMDialog.Information), true);
            Application.Current.MainPage = new NavigationPage(new Contenedor());
        }

        private async void ButtonPagarTarjeta_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new PagoReservacion());
        }
    }
}