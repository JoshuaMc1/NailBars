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
    public partial class VPerfil : ContentPage
    {
        string Iduserlogin;

        public VPerfil()
        {
            InitializeComponent();
            ObtenerIdusuario();
        }


        private async Task ObtenerIdusuario()
        {
            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(Conexionfirebase.WebapyFirebase));
                //validar si el usuario se ha validado o no dentro de la aplicacion
                var guardarId = JsonConvert.DeserializeObject<FirebaseAuth>(Preferences.Get("MyFirebaseRefreshToken", ""));
                //var RefrescarContenido = await authProvider.RefreshAuthAsync(guardarId);
                //Preferences.Set("MyFirebaseRefreshToken", JsonConvert.SerializeObject(RefrescarContenido));
                //el ID

                Iduserlogin = guardarId.User.LocalId;

                await ObtenerDatoUsuario();
            }
            catch (Exception)
            {

                await DisplayAlert("Alerta", "Sesion expirada", "OK");
            }


        }

        private async Task ObtenerDatoUsuario()
        {
            VMusuarios funcion = new VMusuarios();
            MusuariosClientes parametros = new MusuariosClientes();
            parametros.Id_usuario = Iduserlogin;
            var dt = await funcion.ObtenerDatosMiPerfil(parametros);
            foreach (var fila in dt)
            {
                //txtNombres.Text = fila.Nombres;
                imagenCelular.Source = fila.Icono;
                //txtCorreo.Text = fila.Correo;
                //pass = fila.Pass;
                //rutafoto = fila.Icono;
                //IdUsuariosClientes = fila.IdUsuariosClientes;
                //nombre = fila.Nombres;
            }

        }
    }
}