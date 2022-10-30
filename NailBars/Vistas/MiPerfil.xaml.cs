using Acr.UserDialogs;
using Firebase.Auth;
using NailBars.Modelo;
using NailBars.Servicios;
using NailBars.VistasModelo;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NailBars.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MiPerfil : ContentPage
    {
        public MiPerfil()
        {
            InitializeComponent();
            ObtenerIdusuario();


           
        }

        MediaFile file;
        string rutafoto;
        string Iduserlogin;
        string pass;
        string IdUsuariosClientes;
        string estado="vacio";
        string nombre;
        string tipoUser;
        string nombreEdit;

        /*
        
        string nombreEstilista;
        string status;
        string hora_Reserv;
        int calificacion;
        string fecha_Reserv;
        string tipo_Reserv;*/

        private async void btnActualizar_Clicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading("Procesando Solicitud...");

            if (estado == "lleno" || nombre != txtNombres.Text) {

                VMusuarios funcion2 = new VMusuarios();
                MusuariosClientes parametros = new MusuariosClientes();
                parametros.IdUsuariosClientes = IdUsuariosClientes;
                var dt = await funcion2.ObtenerDatostipo(parametros);
                foreach (var fila in dt)
                {
                    tipoUser = fila.tipoUser;
                }

                await EditarFoto();
            }
            else
            {
                await DisplayAlert("Aviso","No se encontro cambios","Ok");
            }


            UserDialogs.Instance.HideLoading();
        }

        private async void btnagregarimagen_Clicked(object sender, EventArgs e)
        {
           
           // animation.IsVisible = false;
           // imagenCelular.IsVisible = true;

            await CrossMedia.Current.Initialize();
            try
            {
                //para poder acceder a la galeria y agregar la img que queramos  NOTA: NIVEL LOCAL aun no se sube la imagen
                file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions()
                {
                    PhotoSize = PhotoSize.Medium
                });
                if (file == null)
                {

                    return;
                }
                else
                {
                    imagenCelular.Source = ImageSource.FromStream(() =>
                    {
                        //GetStream extrae toda la ruta de la imagen
                        var rutaImagen = file.GetStream();
                      
                        return rutaImagen;
                    });

                    await SubirImagenesStore();

                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private async Task SubirImagenesStore()
        {
            VMusuarios funcion = new VMusuarios();
            rutafoto = await funcion.SubirImagenesStorage(file.GetStream(), Iduserlogin);

            estado = "lleno";

        }

        private async Task EditarFoto()
        {

            if (tipoUser == "Empleado")
            {

            
                VMusuarios funcion = new VMusuarios();
                MusuariosClientes parametros = new MusuariosClientes();

                parametros.IdUsuariosClientes = IdUsuariosClientes;
                parametros.Id_usuario = Iduserlogin;
                parametros.Pass = pass;
                parametros.Nombres = txtNombres.Text;
                parametros.Icono = rutafoto;
                parametros.Correo = txtCorreo.Text;
                parametros.tipoUser = tipoUser;

                await funcion.EditarFoto(parametros);
                await ObtenerDatoReservacion();

            }
            else
            {
                VMusuarios funcion = new VMusuarios();
                MusuariosClientes parametros = new MusuariosClientes();

                parametros.IdUsuariosClientes = IdUsuariosClientes;
                parametros.Id_usuario = Iduserlogin;
                parametros.Pass = pass;
                parametros.Nombres = txtNombres.Text;
                parametros.Icono = rutafoto;
                parametros.Correo = txtCorreo.Text;
                parametros.tipoUser = tipoUser;

                await funcion.EditarFoto(parametros);
                await ObtenerDatoReservacion();
            }

            


            await DisplayAlert("Listo", "Datos Acualizados", "OK");

        }

        private async Task ObtenerDatoReservacion()
        {
            VmReservaciones obtener2 = new VmReservaciones();
            VmReservaciones consulta = new VmReservaciones();
            MoReservaciones parametros = new MoReservaciones();
            parametros.id_Cliente = Iduserlogin;
            parametros.nombre_usuario = txtNombres.Text;
            int contador=0;
            var dt = await obtener2.ObtenerDatosReservaciones(parametros);
            foreach (var fila in dt)
            {

                MoReservaciones data  = new MoReservaciones();
                data.id_Reserv = fila.id_Reserv;
                data.id_Cliente = fila.id_Cliente;
                data.nombre_usuario = txtNombres.Text;
                await consulta.postUserReservacion(data);
            
            }

        }

        /*
        private async Task obtener(MoReservaciones parame)
        {
            VmReservaciones funcionR = new VmReservaciones();
            MoReservaciones parametros2 = new MoReservaciones();

            parametros2.id_Cliente = Iduserlogin;
            parametros2.status = status;
            parametros2.nombreEstilista = nombreEstilista;
            parametros2.hora_Reserv = hora_Reserv;
            parametros2.calificacion = calificacion;
            parametros2.fecha_Reserv = fecha_Reserv;
            parametros2.nombre_usuario = txtNombres.Text;
            parametros2.tipo_Reserv = tipo_Reserv;


            await funcionR.Modificar(parametros2);
        }
        */
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
                txtNombres.Text = fila.Nombres;
                imagenCelular.Source = fila.Icono;
                txtCorreo.Text = fila.Correo;
                pass = fila.Pass;
                rutafoto = fila.Icono;
                IdUsuariosClientes = fila.IdUsuariosClientes;
                nombre = fila.Nombres;
            }

        }

        private void btncerrar_Clicked(object sender, EventArgs e)
        {
            Preferences.Remove("MyFirebaseRefreshToken");
            Application.Current.MainPage = new NavigationPage(new Login());
        }


    }
}