using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Database.Query;
using Firebase.Storage;
using NailBars.Components;
using NailBars.Modelo;
using NailBars.Servicios;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Essentials;

namespace NailBars.VistasModelo
{
    public class VMusuarios
    {
        List<MusuariosClientes> Usuarios = new List<MusuariosClientes>();
        List<MusuariosClientes> Usuarios2 = new List<MusuariosClientes>();
        List<Trabajadores> trabajadores = new List<Trabajadores>();

        string rutafoto;
        string Idtrabajador1;
        string IdusuarioCliente;
        string usuarionombrecliente;

        public async Task<List<MusuariosClientes>> mostrar_usuarios()
        {
            var data = await Conexionfirebase.firebase
                .Child("UsuariosClientes")
                .OrderByKey()
                .OnceAsync<MusuariosClientes>();
            foreach (var rdr in data)
            {
                MusuariosClientes parametros = new MusuariosClientes();
                parametros.Id_usuario = rdr.Key;
                parametros.Nombres = rdr.Object.Nombres;
                parametros.Pass = rdr.Object.Pass;
                parametros.Icono = rdr.Object.Icono;
                parametros.Correo = rdr.Object.Correo;
                parametros.IdUsuariosClientes = rdr.Object.IdUsuariosClientes;
                Usuarios.Add(parametros);
            }
            return Usuarios;
        }

        public async Task<string> insertar_usuario(MusuariosClientes parametros)
        {
            var data = await Conexionfirebase.firebase
                  .Child("UsuariosClientes")
                  .PostAsync(new MusuariosClientes()
                  {
                      Id_usuario = parametros.Id_usuario,
                      IdUsuariosClientes = parametros.IdUsuariosClientes,
                      Nombres = parametros.Nombres,
                      Pass = parametros.Pass,
                      Icono = parametros.Icono,
                      Correo = parametros.Correo,
                      tipoUser = parametros.tipoUser

                  });

            IdusuarioCliente = data.Key;
            return IdusuarioCliente;
        }

        public async Task<string> SubirImagenesStorage(Stream ImagenStream, string Idusuarios)
        {
            var dataAlmacenamiento = await new FirebaseStorage("nailbars-c75a5.appspot.com")
                .Child("UsuariosClientes")
                .Child(Idusuarios + ".jpg")
                .PutAsync(ImagenStream);
            rutafoto = dataAlmacenamiento;
            return rutafoto;
        }

        public async Task EditarFoto(MusuariosClientes parametros)
        {
            var obtenerData = (await Conexionfirebase.firebase
                .Child("UsuariosClientes") //comparamos si es la misma key
                .OnceAsync<MusuariosClientes>()).Where(a => a.Key == parametros.IdUsuariosClientes).FirstOrDefault();

            await Conexionfirebase.firebase
                .Child("UsuariosClientes")
                .Child(obtenerData.Key)
                .PutAsync(new MusuariosClientes()
                {
                    IdUsuariosClientes = parametros.IdUsuariosClientes,
                    Id_usuario = parametros.Id_usuario,
                    Nombres = parametros.Nombres,
                    Pass = parametros.Pass,
                    Icono = parametros.Icono,
                    Correo = parametros.Correo,
                    tipoUser = parametros.tipoUser
                });
        }

        public async Task<bool> EliminarUsuarios(string clientid)
        {
            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(Conexionfirebase.WebapyFirebase));
                await authProvider.DeleteUserAsync(Preferences.Get("MyToken", "default_value"));
                await Conexionfirebase.firebase.Child("UsuariosClientes/" + clientid).DeleteAsync();
                return true;
            } catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> ChangePassword(string newPassword)
        {
            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(Conexionfirebase.WebapyFirebase));
                await authProvider.ChangeUserPassword(Preferences.Get("MyToken", "default_value"), newPassword);
                return true;
            } catch(Exception)
            {
                return false;
            }
        }

        public async Task EliminarImagen(string nombre)
        {
            await new FirebaseStorage("nailbars-c75a5.appspot.com")
                .Child("UsuariosClientes")
                .Child(nombre)
                .DeleteAsync();
        }

        public async Task SaveTrabajador(Trabajadores datos)
        {
            var data = await Conexionfirebase.firebase
                  .Child("Trabajadores")
                  .PostAsync(new Trabajadores()
                  {
                      nombre = datos.nombre,
                      Icono = datos.Icono
                  });
        }

        public async Task editarTrabajador(Trabajadores parametros)
        {
            var data = (await Conexionfirebase.firebase
                 .Child("Trabajadores")
                 .OnceAsync<Trabajadores>()).Where(a => a.Object.nombre == parametros.nombre).FirstOrDefault();
            data.Object.nombre = parametros.nombre;
            await Conexionfirebase.firebase
                .Child("Trabajadores")
                .Child(data.Key)
                .PutAsync(data.Object);
        }


        public async Task<List<MusuariosClientes>> ObtenerDatosUsuarios(MusuariosClientes parametros)
        {
            var data = (await Conexionfirebase.firebase
                .Child("UsuariosClientes")
                .OrderByKey()
                .OnceAsync<MusuariosClientes>()).Where(a => a.Object.Id_usuario == parametros.Id_usuario);
            foreach (var rdr in data)
            {
                parametros.Id_usuario = rdr.Object.Id_usuario;
                parametros.Nombres = rdr.Object.Nombres;
                parametros.Pass = rdr.Object.Pass;
                parametros.Icono = rdr.Object.Icono;
                parametros.Correo = rdr.Object.Correo;

                Usuarios.Add(parametros);
            }
            return Usuarios;
        }

        public async Task<List<MusuariosClientes>> ObtenerDatoscorreo(MusuariosClientes parametros)
        {
            var data = (await Conexionfirebase.firebase
                .Child("UsuariosClientes")
                .OrderByKey()
                .OnceAsync<MusuariosClientes>()).Where(a => a.Object.Id_usuario == parametros.Id_usuario);
            foreach (var rdr in data)
            {
                parametros.tipoUser = rdr.Object.tipoUser;

                Usuarios.Add(parametros);
            }
            return Usuarios;
        }

        public async Task<List<MusuariosClientes>> ObtenerDatoscorreo1(MusuariosClientes parametros)
        {
            var data = (await Conexionfirebase.firebase
                .Child("UsuariosClientes")
                .OrderByKey()
                .OnceAsync<MusuariosClientes>()).Where(a => a.Object.Correo == parametros.Correo);
            foreach (var rdr in data)
            {
                parametros.tipoUser = rdr.Object.tipoUser;

                Usuarios.Add(parametros);
            }
            return Usuarios;
        }

        public async Task<List<MusuariosClientes>> ObtenerDatostipo(MusuariosClientes parametros)
        {
            var data = (await Conexionfirebase.firebase
                .Child("UsuariosClientes")
                .OrderByKey()
                .OnceAsync<MusuariosClientes>()).Where(a => a.Object.IdUsuariosClientes == parametros.IdUsuariosClientes);
            foreach (var rdr in data)
            {
                parametros.tipoUser = rdr.Object.tipoUser;

                Usuarios.Add(parametros);
            }
            return Usuarios;
        }

        public async Task<List<MusuariosClientes>> ObtenerDatosMiPerfil(MusuariosClientes parametros)
        {
            var data = (await Conexionfirebase.firebase
                .Child("UsuariosClientes")
                .OrderByKey()
                .OnceAsync<MusuariosClientes>()).Where(a => a.Object.Id_usuario == parametros.Id_usuario);
            foreach (var rdr in data)
            {
                parametros.Id_usuario = rdr.Object.Id_usuario;
                parametros.IdUsuariosClientes = rdr.Object.IdUsuariosClientes;
                parametros.Nombres = rdr.Object.Nombres;
                parametros.Pass = rdr.Object.Pass;
                parametros.Icono = rdr.Object.Icono;
                parametros.Correo = rdr.Object.Correo;

                Usuarios.Add(parametros);
            }
            return Usuarios;
        }

        public async Task<List<MoReservaciones>> contar(MoReservaciones parametros, string fecha)
        {
            var contardor = new List<MoReservaciones>();
            var data = (await Conexionfirebase.firebase
                .Child("Reservaciones")
                .OrderByKey()
                .OnceAsync<MoReservaciones>()).Where(a => a.Object.id_Cliente == parametros.id_Cliente)
                .Where(b => b.Object.fecha_Reserv == fecha);
            foreach (var rdr in data)
            {
                parametros.id_Cliente = rdr.Object.id_Cliente;

                contardor.Add(parametros);
                contardor.Count();
            }
            return contardor;
        }

        public async Task<List<MoReservaciones>> contarEstilista(MoReservaciones parametros, string fecha)
        {
            var contardor = new List<MoReservaciones>();
            var data = (await Conexionfirebase.firebase
                .Child("Reservaciones")
                .OrderByKey()
                .OnceAsync<MoReservaciones>()).Where(a => a.Object.nombreEstilista == parametros.nombreEstilista)
                .Where(b => b.Object.fecha_Reserv == fecha);
            foreach (var rdr in data)
            {
                parametros.id_Cliente = rdr.Object.id_Cliente;

                contardor.Add(parametros);
                contardor.Count();
            }
            return contardor;
        }



        public async Task<List<MusuariosClientes>> getadmin(MusuariosClientes parametros)
        {
            var data = (await Conexionfirebase.firebase
                .Child("UsuariosClientes")
                .OrderByKey()
                .OnceAsync<MusuariosClientes>()).Where(a => a.Object.Id_usuario == parametros.Id_usuario);
            foreach (var rdr in data)
            {
                var nombre = new MusuariosClientes();

                nombre.tipoUser = rdr.Object.tipoUser;
                nombre.IdUsuariosClientes = rdr.Key;
                nombre.Nombres = rdr.Object.Nombres;

                Usuarios.Add(nombre);
            }
            return Usuarios;
        }
    }
}
