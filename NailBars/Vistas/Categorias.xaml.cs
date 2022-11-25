using Firebase.Auth;
using NailBars.Modelo;
using NailBars.Servicios;
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
    public partial class Categorias : ContentPage
    {
        string tipoUser, IdusuarioUser;
        MusuariosClientes datUser = new MusuariosClientes();

        public Categorias()
        {
            InitializeComponent();
        }

        private async Task mostrarCategorias()
        {
            var funcion = new VMcategorias();
            var dt = await funcion.MostrarCategoriasNormal();
        }

        protected override async void OnAppearing()
        {
            await mostrarCategorias();
            await ObtenerIdusuario();
        }

        private async Task ObtenerIdusuario()
        {
            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(Conexionfirebase.WebapyFirebase));
                var guardarId = JsonConvert.DeserializeObject<FirebaseAuth>(Preferences.Get("MyFirebaseRefreshToken", ""));
                var RefrescarContenido = await authProvider.RefreshAuthAsync(guardarId);
                Preferences.Set("MyFirebaseRefreshToken", JsonConvert.SerializeObject(RefrescarContenido));
                IdusuarioUser = guardarId.User.LocalId;

                await validarTipoUser();
                await validarTipoUser();
                await validarTipoUser();
            }
            catch (Exception)
            {
                await DisplayAlert("Alerta", "Sesion expirada", "OK");
            }
        }

        private async void btnPedicure_Clicked(object sender, EventArgs e)
        {
            string[] descripcion = new string[3];
            descripcion[0] = "-";
            descripcion[1] = "-";
            descripcion[2] = "-";
            await Navigation.PushAsync(new VReservaciones(IdusuarioUser, 1, "Pedicure", datUser, "150", descripcion));
        }

        private async void btnManicure_Clicked(object sender, EventArgs e)
        {
            string[] descripcion = new string[3];
            descripcion[0] = "- Corte y formar a las uñas.";
            descripcion[1] = "- Tratamiento a cutículas.";
            descripcion[2] = "- Aplicación de esmalte (normal).";
            await Navigation.PushAsync(new VReservaciones(IdusuarioUser, 0, "Manicure", datUser, "130", descripcion));
        }

        private async void btnAplicaciondeAcrilico_Clicked(object sender, EventArgs e)
        {
            string[] descripcion = new string[3];
            descripcion[0] = "-";
            descripcion[1] = "-";
            descripcion[2] = "-";
            await Navigation.PushAsync(new VReservaciones(IdusuarioUser, 3, "AplicaciondeAcrilico", datUser, "200", descripcion));
        }

        private async void btnRetoquedeAcrilico_Clicked(object sender, EventArgs e)
        {
            string[] descripcion = new string[3];
            descripcion[0] = "-";
            descripcion[1] = "-";
            descripcion[2] = "-";
            await Navigation.PushAsync(new VReservaciones(IdusuarioUser, 2, "PintadoYDecoracion", datUser, "300", descripcion));
        }

        private async Task validarTipoUser()
        {
            VMusuarios funcion = new VMusuarios();
            MusuariosClientes parametro = new MusuariosClientes();
            parametro.Id_usuario = IdusuarioUser;
            var datos = await funcion.getadmin(parametro);
            foreach (var rdr in datos)
            {
                var nombre = new MusuariosClientes();

                datUser.tipoUser = rdr.tipoUser;
                datUser.IdUsuariosClientes = rdr.IdUsuariosClientes;
            }
        }
    }
}