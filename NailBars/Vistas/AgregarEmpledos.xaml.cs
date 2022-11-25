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
	public partial class AgregarEmpledos : ContentPage
	{
		public AgregarEmpledos ()
		{
			InitializeComponent ();
		}

        MediaFile file;

        string rutafoto;
        string Idusuario;
        string Iduserclientes;
        string hola;

        private async void btnagregarimagen_Clicked(object sender, EventArgs e)
        {
            animation.IsVisible = false;
            imagenCelular.IsVisible = true;

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
            rutafoto = await funcion.SubirImagenesStorage(file.GetStream(), Idusuario);
        }

        private async Task InsertarUsuarios()
        {

            VMusuarios funcion = new VMusuarios();
            MusuariosClientes parametros = new MusuariosClientes();

            parametros.Nombres = txtNombres.Text;
            parametros.IdUsuariosClientes = "-";
            parametros.Correo = txtCorreo.Text;
            parametros.Pass = txtContraseña.Text;
            parametros.Icono = rutafoto;
            parametros.tipoUser = "Empleado";
            parametros.Id_usuario = Idusuario;

            Iduserclientes = await funcion.insertar_usuario(parametros);


          await EditarFoto();
        }

        private async Task EditarFoto()
        {
            VMusuarios funcion = new VMusuarios();
            MusuariosClientes parametros = new MusuariosClientes();

            parametros.Nombres = txtNombres.Text;
            parametros.IdUsuariosClientes = Iduserclientes;
            parametros.Correo = txtCorreo.Text;
            parametros.Pass = txtContraseña.Text;
            parametros.Icono = rutafoto;
            parametros.tipoUser = "Empleado";
            parametros.Id_usuario = Idusuario;

            await funcion.EditarFoto(parametros);
            UserDialogs.Instance.HideLoading();           
            await DisplayAlert("Listo", "Empleado Agregado", "OK");


        }


        private async void btnCrearcuenta_Clicked(object sender, EventArgs e)
        {

            if (file != null)
            {
                if (!string.IsNullOrEmpty(txtNombres.Text))
                {
                    if (!string.IsNullOrEmpty(txtCorreo.Text))
                    {
                        hola = txtContraseña.Text;
                        if (!string.IsNullOrEmpty(txtContraseña.Text) && hola.Length > 6)
                        {

                            UserDialogs.Instance.ShowLoading("Creando Usuario...");

                            await Crearcuenta();
                            await IniciarSesion();
                            await ObtenerIdusuario();
                            await SubirImagenesStore();
                            await InsertarUsuarios();
                            await saveTrabajador();
                        }
                        else
                        {
                            await DisplayAlert("Contraseña", "Dede de tener almenos 7 caracteres", "Ok");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Campos Vacios", "LLenar los campos", "Ok");
                    }
                }
                else
                {
                    await DisplayAlert("Campos Vacios", "LLenar los campos", "Ok");
                }
            }
            else
            {

                await DisplayAlert("Imagen Obligatoria", "LLenar los campos", "Ok");


            }



          
        }





        private void Cerrarsesion()
        {
            Preferences.Remove("MyFirebaseRefreshToken");
        }

        private async Task Crearcuenta()
        {
            var funcion = new VMcrearcuenta();
            await funcion.crearcuenta(txtCorreo.Text, txtContraseña.Text);
        }

        private async Task IniciarSesion()
        {
            var funcion = new VMcrearcuenta();
            await funcion.ValidarCuenta(txtCorreo.Text, txtContraseña.Text);
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

                Idusuario = guardarId.User.LocalId;

                Preferences.Remove("MyFirebaseRefreshToken");

            }
            catch (Exception)
            {

                await DisplayAlert("Alerta", "Sesion expirada", "OK");
            }

           

        }

        private async Task saveTrabajador()
        {
            VMusuarios consulta = new VMusuarios();
            Trabajadores dato = new Trabajadores();
            dato.nombre = txtNombres.Text;
            dato.Icono = rutafoto;
            await consulta.SaveTrabajador(dato);
        }

    }
}