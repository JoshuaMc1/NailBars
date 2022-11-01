using Acr.UserDialogs;
using Firebase.Auth;
using NailBars.Modelo;
using NailBars.Servicios;
using NailBars.VistasModelo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NailBars.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Categorias : ContentPage
    {

        string tipoUser;
        public Categorias()
        {
            InitializeComponent();

            var statusbar = new VMPrincipal();
            statusbar.CambiarColor();
        }

        
        string IdusuarioUser;

        MusuariosClientes datUser = new MusuariosClientes();


        private async Task mostrarCategorias()
        {
            var funcion = new VMcategorias();
            var dt = await funcion.MostrarCategoriasNormal();
           // listaCategoriasNormal.ItemsSource = dt;
           
        }
        protected override async void OnAppearing()
        {
            mostrarCategorias();
            await ObtenerIdusuario();



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
                IdusuarioUser = guardarId.User.LocalId;


               await validarTipoUser();
            }
            catch (Exception)
            {

                await DisplayAlert("Alerta", "Sesion expirada", "OK");
            }



        }



        private async Task ObtenerIdtrabajador()
        {
            VMusuarios funcion = new VMusuarios();

          // await funcion.getTrabajadores();

        }

        private async void btnPedicure_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new VistaReservaciones(IdusuarioUser, "Pedicure", datUser, "150"));
        }

        private async void btnManicure_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new VistaReservaciones(IdusuarioUser, "Manicure", datUser,"130"));
        }

        private async void btnAplicaciondeAcrilico_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new VistaReservaciones(IdusuarioUser, "AplicaciondeAcrilico", datUser,"200"));
        }

        private async void btnRetoquedeAcrilico_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new VistaReservaciones(IdusuarioUser, "PintadoYDecoracion", datUser,"300"));
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