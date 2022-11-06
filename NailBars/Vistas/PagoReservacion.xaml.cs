using NailBars.Modelo;
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
    public partial class PagoReservacion : ContentPage
    {
        public PagoReservacion()
        {
            InitializeComponent();
            this.BindingContext = new MainPageViewModel();
        }

        private async void Pago_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Listo", "Reservacion enviada", "OK");
            Application.Current.MainPage = new NavigationPage(new Contenedor());
        }
    }
}