using NailBars.VistasModelo;
using System;

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
            var statusbar = new VMPrincipal();
            statusbar.CambiarColor();
        }

        public async void Reservaa_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new Contenedor());
        }
    }
}