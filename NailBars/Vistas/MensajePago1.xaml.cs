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
    public partial class MensajePago1 : ContentPage
    {
        public MensajePago1()
        {
            InitializeComponent();
        }

        public async void Reservaa_Clicked(object sender, EventArgs e)
        {
            String subir = txtsubir.Text;

            if (txtsubir == null)
            {
                await DisplayAlert("Error", "No se puede reallizar el Pago", "OK");
            }
            else
            {
                await DisplayAlert("AVISO", "RESERVACIÓN EXITOSA", "ACEPTAR");
                Application.Current.MainPage = new NavigationPage(new Contenedor());
            }

        }
    }
}