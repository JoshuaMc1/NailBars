using Acr.UserDialogs;
using Firebase.Auth;
using NailBars.Components;
using NailBars.Modelo;
using NailBars.Servicios;
using NailBars.VistasModelo;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NailBars.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VAgregarEmpleado : ContentPage
    {
        MediaFile file;
        string rutafoto, Idusuario, Iduserclientes;

        public VAgregarEmpleado()
        {
            InitializeComponent();
        }

        private async void btnAgregarEmpleado_Clicked(object sender, EventArgs e)
        {
            if (file != null)
            {
                if (!string.IsNullOrEmpty(txtNombres.Text))
                {
                    if (!string.IsNullOrEmpty(txtCorreo.Text))
                    {
                        if (!string.IsNullOrEmpty(txtOldPassword.Text))
                        {
                            if (!string.IsNullOrEmpty(txtNewPassword.Text))
                            {
                                if (txtOldPassword.Text.Equals(txtNewPassword.Text))
                                {
                                    if (txtCorreo.Text.Contains("@") && txtCorreo.Text.Contains("."))
                                    {
                                        UserDialogs.Instance.ShowLoading("Creando Usuario...");

                                        await Crearcuenta();
                                        await IniciarSesion();
                                        await ObtenerIdusuario();
                                        await SubirImagenesStore();
                                        await InsertarUsuarios();
                                        await saveTrabajador();
                                        CleanFields();
                                        await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Satisfactorio", "El empleado se a creado exitosamente.", JMDialog.Success), true);
                                        DependencyService.Get<INotification>().CreateNotification("NailBars", "El empleado/a " + txtNombres.Text + " se a creado exitosamente.");
                                    }
                                    else await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Advertencia", "El correo electrónico no es valido.", JMDialog.Warning), true);
                                }
                                else await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Advertencia", "Las contraseñas no coinciden, por favor ingresar de nuevo..", JMDialog.Warning), true);
                            }
                            else await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Advertencia", "Debe ingresar la nuevamente contraseña (Debe ser mayor a 7 caracteres).", JMDialog.Warning), true);
                        }
                        else await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Advertencia", "Debe ingresar la contraseña (Debe ser mayor a 7 caracteres).", JMDialog.Warning), true);
                    }
                    else await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Advertencia", "Debe ingresar su correo electrónico.", JMDialog.Warning), true);
                }
                else await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Advertencia", "Debe ingresar su nombre completo.", JMDialog.Warning), true);
            }
            else await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Advertencia", "Debe seleccionar una foto de perfil.", JMDialog.Warning), true);
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
            parametros.Pass = txtNewPassword.Text;
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
            parametros.Pass = txtNewPassword.Text;
            parametros.Icono = rutafoto;
            parametros.tipoUser = "Empleado";
            parametros.Id_usuario = Idusuario;

            await funcion.EditarFoto(parametros);
            UserDialogs.Instance.HideLoading();
        }

        private async Task Crearcuenta()
        {
            var funcion = new VMcrearcuenta();
            await funcion.crearcuenta(txtCorreo.Text.Trim(), txtNewPassword.Text);
        }

        private async Task IniciarSesion()
        {
            var funcion = new VMcrearcuenta();
            await funcion.ValidarCuentaRegistro(txtCorreo.Text.Trim(), txtNewPassword.Text);
        }

        private async Task ObtenerIdusuario()
        {
            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(Conexionfirebase.WebapyFirebase));

                var guardarId = JsonConvert.DeserializeObject<FirebaseAuth>(Preferences.Get("MyFirebaseRefreshToken", ""));
                var RefrescarContenido = await authProvider.RefreshAuthAsync(guardarId);
                Preferences.Set("MyFirebaseRefreshToken", JsonConvert.SerializeObject(RefrescarContenido));

                Idusuario = guardarId.User.LocalId;
                Preferences.Remove("MyFirebaseRefreshToken");
            }
            catch (Exception)
            {
                await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Aviso", "Su sesión a expirado.", JMDialog.Danger), true);
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

        private void CleanFields()
        {
            txtNombres.Text = string.Empty;
            txtCorreo.Text = string.Empty;
            txtNewPassword.Text = string.Empty;
            txtOldPassword.Text = string.Empty;
            imagenCelular.Source = "https://t4.ftcdn.net/jpg/04/70/29/97/360_F_470299797_UD0eoVMMSUbHCcNJCdv2t8B2g1GVqYgs.jpg";
        }
    }
}