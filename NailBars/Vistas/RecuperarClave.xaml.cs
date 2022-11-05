using Acr.UserDialogs;
using NailBars.Servicios;
using NailBars.VistasModelo;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NailBars.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecuperarClave : ContentPage
    {
        public RecuperarClave()
        {
            InitializeComponent();
        }

        private async void btnRecuperar_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCorreo.Text))
            {
                if (txtCorreo.Text.Contains("@") && txtCorreo.Text.Contains("."))
                {
                    var funcion = new VMRestablecer();
                    var result = await funcion.RestablecerClvae(txtCorreo.Text);
                    if (result)
                    {
                        await DisplayAlert("Informacion", "Correo enviado exitosamente.", "Ok");
                        DependencyService.Get<INotification>().CreateNotification("NailBars", "El correo se a enviado exitosamente, para poder restablecer la contraseña debe dar click en el enlace enviado.");
                        await Navigation.PushAsync(new Login());
                    } else await DisplayAlert("Advertencia", "Por favor revise si su correo esta escrito correctamente", "Ok");
                } else await DisplayAlert("Advertencia", "El correo electronico no es valido", "Ok");
            } else await DisplayAlert("Campo vacio", "Debe ingresar el correo electronico", "Ok");
        }
    }
}