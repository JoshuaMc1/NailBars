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
        public async void Pago_Clicked(object sender, EventArgs e)
        {
            String ntarjeta = txttarjeta.Text;
            String cvv = txtcvv.Text;
            String expiracion = txtexpiracion.Text;
            if (ntarjeta==null || cvv==null || expiracion==null)
            {
                await DisplayAlert("Error", "Campos Vacios", "OK");
            }
            else
            {
                await DisplayAlert("Listo", "Reservacion enviada", "OK");
                Application.Current.MainPage = new NavigationPage(new Contenedor());
            }
            
        }
        
    }
}