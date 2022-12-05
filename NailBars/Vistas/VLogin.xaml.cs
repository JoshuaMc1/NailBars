using Acr.UserDialogs;
using NailBars.Components;
using NailBars.Modelo;
using NailBars.VistasModelo;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NailBars.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VLogin : ContentPage
    {
        string tipoUser;
        string Iduserlogin;

        public VLogin()
        {
            InitializeComponent();
            var statusbar = new VMPrincipal();
            statusbar.Transparente();
        }

        private async void RecuperarClaveTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new VRecuperarClave());
        }

        private async void GoBackTapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void btnIniciarSesion_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtUsuercorreo.Text))
            {
                if (!string.IsNullOrEmpty(txtUserPassword.Text))
                {
                    if (txtUsuercorreo.Text.Contains("@") && txtUsuercorreo.Text.Contains("."))
                    {
                        UserDialogs.Instance.ShowLoading("Validando Usuario...");
                        await validarDatos();
                    }
                    else await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Advertencia", "El correo electrónico no es valido.", JMDialog.Warning), true);
                }
                else await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Advertencia", "Debe ingresar su contraseña.", JMDialog.Warning), true);
            }
            else await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Advertencia", "Debe ingresar su correo electrónico.", JMDialog.Warning), true);
        }

        private async Task validarDatos()
        {
            try
            {
                VMusuarios funcion2 = new VMusuarios();
                MusuariosClientes parametros = new MusuariosClientes();
                parametros.Correo = txtUsuercorreo.Text.Trim();
                var dt = await funcion2.ObtenerDatoscorreo1(parametros);
                foreach (var fila in dt)
                {
                    tipoUser = fila.tipoUser;
                }

                if (tipoUser == "Cliente")
                {
                    var funcion = new VMcrearcuenta();
                    await funcion.ValidarCuenta(txtUsuercorreo.Text.Trim(), txtUserPassword.Text.Trim());
                    Application.Current.MainPage = new NavigationPage(new Contenedor());
                }
                else if (tipoUser == "admin")
                {
                    var funcion = new VMcrearcuenta();
                    await funcion.ValidarCuenta(txtUsuercorreo.Text.Trim(), txtUserPassword.Text.Trim());

                    Application.Current.MainPage = new NavigationPage(new ContenedorAdmin());
                }
                else if (tipoUser == "Empleado")
                {
                    var funcion = new VMcrearcuenta();
                    await funcion.ValidarCuenta(txtUsuercorreo.Text.Trim(), txtUserPassword.Text.Trim());

                    Application.Current.MainPage = new NavigationPage(new ContenedorEmpleado());
                }
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception)
            {
                UserDialogs.Instance.HideLoading();
                await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Aviso", "El correo electrónico o la contraseña son incorrectos.", JMDialog.Danger), true);
                await Navigation.PopAsync();
            }
        }
    }
}