using NailBars.Components;
using NailBars.Servicios;
using NailBars.VistasModelo;
using Rg.Plugins.Popup.Extensions;
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
                        await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Satisfactorio", "El correo electrónico se a enviado satisfactoriamente.", JMDialog.Success), true);
                        DependencyService.Get<INotification>().CreateNotification("NailBars", "El correo se a enviado exitosamente, para poder restablecer la contraseña debe dar click en el enlace enviado.");
                        await Navigation.PopAsync();
                    } else await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Advertencia", "Por favor revise si su correo electrónico esta escrito correctamente.", JMDialog.Warning), true);
                } else await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Advertencia", "El correo electrónico no es valido.", JMDialog.Warning), true);
            } else await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Advertencia", "Debe ingresar su correo electrónico.", JMDialog.Warning), true);
        }
    }
}