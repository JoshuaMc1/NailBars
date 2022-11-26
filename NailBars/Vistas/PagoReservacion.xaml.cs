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
using NailBars.Components;
using Rg.Plugins.Popup.Extensions;

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


        private async void Btnpagar_Clicked(object sender, EventArgs e)
        {
            String ntarjeta = txttarjeta.Text;
            String cvv = txtcvv.Text;
            String expiracion = txtexpiracion.Text;
            if (ntarjeta == null || cvv == null || expiracion == null)
            {
                await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Advertencia", "Hay campos vacios", JMDialog.Warning), true);
                
            }
            else
            {
                await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Satisfactorio", "El pago se realizo correctamente", JMDialog.Success), true);
                Application.Current.MainPage = new NavigationPage(new MensajePago1());
            }
        }

    }
}