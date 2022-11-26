using System;
using System.Threading.Tasks;
using Acr.UserDialogs;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NailBars.VistasModelo;
using NailBars.Modelo;
using NailBars.Components;
using Rg.Plugins.Popup.Extensions;

namespace NailBars.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        string tipoUser;
        string Iduserlogin;

        public Login()
        {
            InitializeComponent();
            var statusbar = new VMPrincipal();
            statusbar.Transparente();
        }

        private async void btncrearUser_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegistroClientes());
        }

        private async void btniniciar_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtUsuercorreo.Text))
            {
                if (!string.IsNullOrEmpty(txtUserPassword.Text))
                {
                    UserDialogs.Instance.ShowLoading("Validando Usuario...");

                    await validarDatos();
                }
                else
                {
                    await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Advertencia", "Debe ingresar su contraseña.", JMDialog.Warning), true);
                }
            }
            else
            {
                await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Advertencia", "Debe ingresar su correo electrónico.", JMDialog.Warning), true);
            }
        }

        private async Task validarDatos()
        {
            try
            {
                VMusuarios funcion2 = new VMusuarios();
                MusuariosClientes parametros = new MusuariosClientes();
                parametros.Correo = txtUsuercorreo.Text;
                var dt = await funcion2.ObtenerDatoscorreo1(parametros);
                foreach (var fila in dt)
                {
                    tipoUser = fila.tipoUser;
                }

                if (tipoUser == "Cliente")
                {
                    var funcion = new VMcrearcuenta();
                    await funcion.ValidarCuenta(txtUsuercorreo.Text, txtUserPassword.Text);
                    Application.Current.MainPage = new NavigationPage(new Contenedor());
                }
                else if (tipoUser == "admin")
                {
                    var funcion = new VMcrearcuenta();
                    await funcion.ValidarCuenta(txtUsuercorreo.Text, txtUserPassword.Text);

                    Application.Current.MainPage = new NavigationPage(new ContenedorAdmin());
                }
                else if (tipoUser == "Empleado")
                {
                    var funcion = new VMcrearcuenta();
                    await funcion.ValidarCuenta(txtUsuercorreo.Text, txtUserPassword.Text);

                    Application.Current.MainPage = new NavigationPage(new ContenedorEmpleado());
                }
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception)
            {
                UserDialogs.Instance.HideLoading();
                await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Aviso", "El correo electrónico o la contraseña son incorrectos.", JMDialog.Danger), true);
                await Navigation.PushAsync(new Login());
            }
        }

        private async void btnRecuperar_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RecuperarClave());
        }
    }
}