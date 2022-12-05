using Acr.UserDialogs;
using Firebase.Auth;
using NailBars.Modelo;
using NailBars.Servicios;
using NailBars.Vistas.TutorialIntro;
using NailBars.VistasModelo;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NailBars.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VIntro : ContentPage
    {
        string tipoUser;
        string Iduserlogin;

        public VIntro()
        {
            InitializeComponent();
            var statusbar = new VMPrincipal();
            statusbar.Transparente();
            UserDialogs.Instance.ShowLoading("Buscando sesión activa");
            ObtenerIdusuario();
            Cerrarsesion();
        }

        private async void btnRegistrarse_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new VRegistro());
        }

        private async void btnIniciarSesion_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new VLogin());
        }

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

            if (!string.IsNullOrEmpty(Preferences.Get("MyFirebaseRefreshToken", "")))
            {
                UserDialogs.Instance.HideLoading();
                if (tipoUser == "Cliente")
                {
                    Application.Current.MainPage = new NavigationPage(new Contenedor());
                }
                else if (tipoUser == "admin")
                {
                    Application.Current.MainPage = new NavigationPage(new ContenedorAdmin());
                }
                else if (tipoUser == "Empleado")
                {
                    Application.Current.MainPage = new NavigationPage(new ContenedorEmpleado());
                }
            } else UserDialogs.Instance.HideLoading();
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
                var guardarId = JsonConvert.DeserializeObject<FirebaseAuth>(Preferences.Get("MyFirebaseRefreshToken", ""));

                var RefrescarContenido = await authProvider.RefreshAuthAsync(guardarId);
                Preferences.Set("MyFirebaseRefreshToken", JsonConvert.SerializeObject(RefrescarContenido));
                Iduserlogin = guardarId.User.LocalId;

                Animacion();
            }
            catch (Exception)
            {
                Animacion();
            }
        }
    }
}