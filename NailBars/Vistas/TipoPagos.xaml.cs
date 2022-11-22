using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }

        private async void ButtonPagarSitio_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Aviso", "Pagar en el Sitio?", "Si");
            Application.Current.MainPage = new NavigationPage(new Contenedor());
        }

        private async void ButtonPagarTarjeta_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Aviso", "Pagar con Tarjeta Crédito o Debito?", "Si");
            Application.Current.MainPage = new NavigationPage(new PagoReservacion());
        }

    }
}