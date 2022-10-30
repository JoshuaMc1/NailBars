using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NailBars.Vistas;
using Acr.UserDialogs;
using Xamarin.Essentials;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NailBars.VistasModelo;
using NailBars.Modelo;
using Firebase.Auth;
using NailBars.Servicios;
using Newtonsoft.Json;

namespace NailBars.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
            //esto es para ver si ya ha iniciado sesion que lo mande a la pagina principal
          

        }

        string tipoUser;
        string Iduserlogin;

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
                    await DisplayAlert("Alerta", "Ingrese su contraseña", "OK");
                }
            }
            else
            {
                await DisplayAlert("Alerta", "Ingrese su correo", "OK");
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

                    

                    //esto es pq se van a convertir las paginas de toolvar en navigationpage

                    Application.Current.MainPage = new NavigationPage(new Contenedor());


                }
                else if (tipoUser == "admin")
                {
                    var funcion = new VMcrearcuenta();
                    await funcion.ValidarCuenta(txtUsuercorreo.Text, txtUserPassword.Text);

                    //esto es pq se van a convertir las paginas de toolvar en navigationpage

                    Application.Current.MainPage = new NavigationPage(new ContenedorAdmin());

                }
                else if (tipoUser == "Empleado")
                {
                    var funcion = new VMcrearcuenta();
                    await funcion.ValidarCuenta(txtUsuercorreo.Text, txtUserPassword.Text);

                    //esto es pq se van a convertir las paginas de toolvar en navigationpage

                    Application.Current.MainPage = new NavigationPage(new ContenedorEmpleado());

                }


                UserDialogs.Instance.HideLoading();
            }
            catch (Exception)
            {

                UserDialogs.Instance.HideLoading();

                await DisplayAlert("Alerta", "Correo o Contraseña incorrectos", "OK");
                await Navigation.PushAsync(new Login());

            }



        }

        /*
        private async Task ObtenerIdusuario()
        {
            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(Conexionfirebase.WebapyFirebase));

                //validar si el usuario se ha validado o no dentro de la aplicacion
                var guardarId = JsonConvert.DeserializeObject<FirebaseAuth>(Preferences.Get("MyFirebaseRefreshToken", ""));

                var RefrescarContenido = await authProvider.RefreshAuthAsync(guardarId);
                Preferences.Set("MyFirebaseRefreshToken", JsonConvert.SerializeObject(RefrescarContenido));
                //el ID
                Iduserlogin = guardarId.User.LocalId;

             await validarDatos();
            }
            catch (Exception)
            {
                
                await DisplayAlert("Alerta", "Sesion expirada", "OK");
            }


        }

        */


    }
}