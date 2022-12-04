using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Text;
using NailBars.Servicios;
using Newtonsoft.Json;
using Xamarin.Essentials;
using System.Threading.Tasks;
using NailBars.Components;
using Rg.Plugins.Popup.Extensions;

namespace NailBars.VistasModelo
{
    public class VMcrearcuenta
    {
        public async Task<bool> crearcuenta(string correo, string contraseña)
        {
            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(Conexionfirebase.WebapyFirebase));
                await authProvider.CreateUserWithEmailAndPasswordAsync(correo, contraseña);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task ValidarCuentaRegistro(string correo, string contraseña)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(Conexionfirebase.WebapyFirebase));
            var auth = await authProvider.SignInWithEmailAndPasswordAsync(correo, contraseña);
            string gettoken = auth.FirebaseToken;
            var content = await auth.GetFreshAuthAsync();
            var serializartoken = JsonConvert.SerializeObject(auth);
            Preferences.Set("MyFirebaseRefreshToken", serializartoken);
            Preferences.Set("MyToken", gettoken);

            if (content.User.IsEmailVerified == false)
            {
                await authProvider.SendEmailVerificationAsync(gettoken);
            }
        }

        public async Task ValidarCuenta(string correo, string contraseña)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(Conexionfirebase.WebapyFirebase));
            var auth = await authProvider.SignInWithEmailAndPasswordAsync(correo, contraseña);
            string gettoken = auth.FirebaseToken;
            var content = await auth.GetFreshAuthAsync();
            var serializartoken = JsonConvert.SerializeObject(auth);
            Preferences.Set("MyFirebaseRefreshToken", serializartoken);
            Preferences.Set("MyToken", gettoken);
            string message = "";
            bool send = false;

            if (content.User.IsEmailVerified == false)
            {
                var popup = new JMConfirmation("Advertencia", "Su correo electrónico no se ha activado ¿desea enviar el código de activación de nuevo?", JMConfirmation.Warning);
                popup.OnDialogClosed += (s, arg) =>
                {
                    message = arg.Message;
                    send = arg.Success;
                };
                await App.Current.MainPage.Navigation.PushPopupAsync(popup, true);

                if (message.Equals("Accept") || send)
                {
                    await authProvider.SendEmailVerificationAsync(gettoken);
                }
            }
        }
    }
}
