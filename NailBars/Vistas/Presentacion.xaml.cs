using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NailBars.Vistas.TutorialIntro;
using NailBars.VistasModelo;
using NailBars.Modelo;
using Acr.UserDialogs;
using Firebase.Auth;
using Newtonsoft.Json;
using NailBars.Servicios;

namespace NailBars.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Presentacion : ContentPage
    {
        public Presentacion()
        {
            InitializeComponent();
            
            ObtenerIdusuario();
            Cerrarsesion();

        }

        string tipoUser;
        string Iduserlogin;

        public async Task Animacion()
        {
            VMusuarios funcion2 = new VMusuarios();
            MusuariosClientes parametros = new MusuariosClientes();
            parametros.Id_usuario = Iduserlogin;
            var dt = await funcion2.ObtenerDatoscorreo(parametros);
            foreach (var fila in dt)
            {
                tipoUser = fila.tipoUser;
            }


            logo.Opacity = 0;
            await logo.FadeTo(1, 2000);
            if (!string.IsNullOrEmpty(Preferences.Get("MyFirebaseRefreshToken", "")))
            {

               

                if (tipoUser == "Cliente")
                {

                    //esto es pq se van a convertir las paginas de toolvar en navigationpage

                    Application.Current.MainPage = new NavigationPage(new Contenedor());


                }
                else if (tipoUser == "admin")
                {


                    //esto es pq se van a convertir las paginas de toolvar en navigationpage

                    Application.Current.MainPage = new NavigationPage(new ContenedorAdmin());

                }
                else if (tipoUser == "Empleado")
                {


                    //esto es pq se van a convertir las paginas de toolvar en navigationpage

                    Application.Current.MainPage = new NavigationPage(new ContenedorEmpleado());

                }
                // Application.Current.MainPage = new NavigationPage(new Contenedor());
            }
            else
            {
                Application.Current.MainPage = new Intro1();

            }


        }
        private void Cerrarsesion()
        {
            Preferences.Remove("MyFirebaseRefreshToken");
        }


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

                Animacion();
            }
            catch (Exception)
            {
                Animacion();
               // await DisplayAlert("Alerta", "Sesion expirada", "OK");
            }


        }

    }
}