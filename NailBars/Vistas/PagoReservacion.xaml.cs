using Firebase.Auth;
using NailBars.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NailBars.VistasModelo;

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
            String pagardinero = txtpagardinero.Text;
            if (ntarjeta == null || cvv == null || expiracion == null || pagardinero == null)
            {
                await DisplayAlert("Error", "Campos Vacios", "OK");
            }
            else
            {
                await DisplayAlert("AVISO", "EL PAGO SE REALIZO EXITOSAMENTE", "ACEPTAR");
                Application.Current.MainPage = new NavigationPage(new MensajePago1());
            }

        }

    }
}