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
    public partial class VIntro : ContentPage
    {
        public VIntro()
        {
            InitializeComponent(); 
            var statusbar = new VMPrincipal();
            statusbar.Translucido();
        }

        private async void btnRegistrarse_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new VRegistro());
        }

        private async void btnIniciarSesion_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new VLogin());
        }
    }
}