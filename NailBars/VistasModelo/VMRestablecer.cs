using Firebase.Auth;
using NailBars.Servicios;
using System;
using System.Threading.Tasks;

namespace NailBars.VistasModelo
{
    public class VMRestablecer
    {
        public async Task<bool> RestablecerClvae(string correo)
        {
            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(Conexionfirebase.WebapyFirebase));
                await authProvider.SendPasswordResetEmailAsync(correo);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
