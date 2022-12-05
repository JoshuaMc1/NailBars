﻿using Acr.UserDialogs;
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
    public partial class VRegistro : ContentPage
    {
        MediaFile file;
        string rutafoto, Idusuario, Iduserclientes;

        public VRegistro()
        {
            InitializeComponent();
            var statusbar = new VMPrincipal();
            statusbar.Transparente();
        }

        private async void GoBackTapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void ImageTapped(object sender, EventArgs e)
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

        private async void btnRegistrar_Clicked(object sender, EventArgs e)
        {
            if (file != null)
            {
                if (!string.IsNullOrEmpty(txtNombres.Text))
                {
                    if (!string.IsNullOrEmpty(txtCorreo.Text))
                    {
                        if (txtCorreo.Text.Contains("@") && txtCorreo.Text.Contains("."))
                        {
                            if (!string.IsNullOrEmpty(txtContraseña.Text.Trim()) && txtContraseña.Text.Length > 6)
                            {
                                var responce = await Crearcuenta();
                                if (responce)
                                {
                                    UserDialogs.Instance.ShowLoading("Creando Usuario...");
                                    await IniciarSesion();
                                    await ObtenerIdusuario();

                                    await SubirImagenesStore();
                                    await InsertarUsuarios();
                                    DependencyService.Get<INotification>().CreateNotification("NailBars", "Usuario creado exitosamente");
                                }
                                else await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Advertencia", "El correo electronico ya existe.", JMDialog.Warning), true);
                            }
                            else await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Advertencia", "Dede de tener al menos 7 caracteres.", JMDialog.Warning), true);
                        }
                        else await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Advertencia", "El correo electrónico no es valido.", JMDialog.Warning), true);
                    }
                    else await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Advertencia", "Debe ingresar su correo electrónico.", JMDialog.Warning), true);
                }
                else await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Advertencia", "Debe ingresar su nombre.", JMDialog.Warning), true);
            }
            else await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Advertencia", "Debe seleccionar o tomar una foto.", JMDialog.Warning), true);
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
            parametros.Correo = txtCorreo.Text.Trim();
            parametros.Pass = txtContraseña.Text.Trim();
            parametros.Icono = rutafoto;
            parametros.tipoUser = "Cliente";
            parametros.Id_usuario = Idusuario;

            Iduserclientes = await funcion.insertar_usuario(parametros);

            EditarFoto();
        }

        private async Task EditarFoto()
        {
            VMusuarios funcion = new VMusuarios();
            MusuariosClientes parametros = new MusuariosClientes();

            parametros.IdUsuariosClientes = Iduserclientes;
            parametros.Nombres = txtNombres.Text;
            parametros.Correo = txtCorreo.Text.Trim();
            parametros.Pass = txtContraseña.Text.Trim();
            parametros.Icono = rutafoto;
            parametros.tipoUser = "Cliente";
            parametros.Id_usuario = Idusuario;

            await funcion.EditarFoto(parametros);
            UserDialogs.Instance.HideLoading();
            Cerrarsesion();
            await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Satisfactorio", "Se a registrado exitosamente.", JMDialog.Success), true);
            await Navigation.PopAsync();
        }

        private void Cerrarsesion()
        {
            Preferences.Remove("MyFirebaseRefreshToken");
        }

        private async Task<bool> Crearcuenta()
        {
            var funcion = new VMcrearcuenta();
            return await funcion.crearcuenta(txtCorreo.Text.Trim(), txtContraseña.Text.Trim());
        }

        private async Task IniciarSesion()
        {
            var funcion = new VMcrearcuenta();
            await funcion.ValidarCuentaRegistro(txtCorreo.Text.Trim(), txtContraseña.Text.Trim());
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
            }
            catch (Exception)
            {
                await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Aviso", "Su sesión a expirado.", JMDialog.Danger), true);
            }
        }
    }
}