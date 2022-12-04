using Firebase.Auth;
using NailBars.Modelo;
using NailBars.Servicios;
using NailBars.VistasModelo;
using Newtonsoft.Json;
using Plugin.Media.Abstractions;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Acr.UserDialogs;
using NailBars.Components;
using Rg.Plugins.Popup.Extensions;

namespace NailBars.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VPerfil : ContentPage
    {
        string Iduserlogin, rutafoto, pass, IdUsuariosClientes, estado = "vacio", nombre, tipoUser;
        MediaFile file;

        public VPerfil()
        {
            InitializeComponent();
            ObtenerIdusuario();
            var statusbar = new VMPrincipal();
            statusbar.CambiarColor();
        }

        private async void btnFoto_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            try
            {
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

        private async void btnActualizarDatos_Clicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading("Procesando Solicitud...");

            if (estado == "lleno" || nombre != txtNombres.Text)
            {
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
                await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Notificación", "No se encontraron cambios.", JMDialog.Information), true);
            }
            UserDialogs.Instance.HideLoading();
        }

        private async void btnCerrarSesion_Clicked(object sender, EventArgs e)
        {
            await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Satisfactorio", "Sesión finalizada con exito.", JMDialog.Success), true);
            Preferences.Remove("MyFirebaseRefreshToken");
            Preferences.Remove("MyToken");
            Application.Current.MainPage = new NavigationPage(new VIntro());
        }

        private async void btnActualizarClave_Clicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading("Procesando Solicitud...");
            if (!string.IsNullOrEmpty(txtOldPassword.Text))
            {
                if (!string.IsNullOrEmpty(txtNewPassword.Text))
                {
                    if (txtOldPassword.Text.Equals(txtNewPassword.Text))
                    {
                        VMusuarios funcion = new VMusuarios();
                        bool response = await funcion.ChangePassword(txtNewPassword.Text.Trim());

                        if (response)
                        {
                            await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Satisfactorio", "Contraseña actualizada exitosamente.", JMDialog.Success), true);
                        }
                        else await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Aviso", "A ocurrido un error al actualizar su contraseña.", JMDialog.Danger), true);
                    }
                    else await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Advertencia", "Las contraseñas no coinciden, por favor ingresar de nuevo.", JMDialog.Warning), true);
                }
                else await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Advertencia", "Debe ingresar nuevamente la contraseña (Debe ser mayor a 7 caracteres).", JMDialog.Warning), true);
            }
            else await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Advertencia", "Debe ingresar la nueva contraseña (Debe ser mayor a 7 caracteres).", JMDialog.Warning), true);
            UserDialogs.Instance.HideLoading();
        }

        private async void btnEliminarCuenta_Clicked(object sender, EventArgs e)
        {
            bool borrar = await DisplayAlert("Aviso", "¿Desea eliminar su cuenta permanentemente?", "Aceptar", "Cancelar");
            if (borrar)
            {
                UserDialogs.Instance.ShowLoading("Procesando Solicitud...");
                VMusuarios funcion = new VMusuarios();
                bool response = await funcion.EliminarUsuarios(IdUsuariosClientes);

                if (response)
                {
                    await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Satisfactorio", "Su cuenta se a eliminado satisfactoriamente.", JMDialog.Success), true);
                    Preferences.Remove("MyFirebaseRefreshToken");
                    Preferences.Remove("MyToken");
                    Application.Current.MainPage = new NavigationPage(new VIntro());
                }
                else
                {
                    await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Aviso", "A ocurrido un error al borrar su cuenta.", JMDialog.Danger), true);
                }
                UserDialogs.Instance.HideLoading();
            }
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
            await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Satisfactorio", "Los datos se han actualizado satisfactoriamente.", JMDialog.Success), true);
        }

        private async Task ObtenerDatoReservacion()
        {
            VmReservaciones obtener2 = new VmReservaciones();
            VmReservaciones consulta = new VmReservaciones();
            MoReservaciones parametros = new MoReservaciones();
            parametros.id_Cliente = Iduserlogin;
            parametros.nombre_usuario = txtNombres.Text;
            int contador = 0;
            var dt = await obtener2.ObtenerDatosReservaciones(parametros);
            foreach (var fila in dt)
            {
                MoReservaciones data = new MoReservaciones();
                data.id_Reserv = fila.id_Reserv;
                data.id_Cliente = fila.id_Cliente;
                data.nombre_usuario = txtNombres.Text;
                await consulta.postUserReservacion(data);
            }
        }

        private async Task SubirImagenesStore()
        {
            VMusuarios funcion = new VMusuarios();
            rutafoto = await funcion.SubirImagenesStorage(file.GetStream(), Iduserlogin);
            estado = "lleno";
        }

        private async Task ObtenerIdusuario()
        {
            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(Conexionfirebase.WebapyFirebase));
                var guardarId = JsonConvert.DeserializeObject<FirebaseAuth>(Preferences.Get("MyFirebaseRefreshToken", ""));
                Iduserlogin = guardarId.User.LocalId;
                await ObtenerDatoUsuario();
            }
            catch (Exception)
            {
                await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Aviso", "Su sesión a expirado.", JMDialog.Danger), true);
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
    }
}