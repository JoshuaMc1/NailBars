using Firebase.Database.Query;
using NailBars.Modelo;
using NailBars.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NailBars.VistasModelo
{
    public class VmReservaciones
    {

        List<MoReservaciones> Reservaciones = new List<MoReservaciones>();

        public async Task<string> InsertReservacion(MoReservaciones reservacion)
        {
            var data = await Conexionfirebase.firebase
                  .Child("Reservaciones")
                  .PostAsync(new MoReservaciones()
                  {
                      id_Cliente = reservacion.id_Cliente,
                      nombreEstilista = reservacion.nombreEstilista,
                      nombre_usuario = reservacion.nombre_usuario,
                      fecha_Reserv = reservacion.fecha_Reserv,
                      hora_Reserv = reservacion.hora_Reserv,
                      tipo_Reserv = reservacion.tipo_Reserv,
                      precio = reservacion.precio,
                      status = reservacion.status,
                      calificacion = reservacion.calificacion

                  });

            return data.Key.ToString();
        }

        public async Task<List<MoReservaciones>> getResrOfDate(string fecha, string estado)
        {
            List<MoReservaciones> lstReservaciones = new List<MoReservaciones>();
            var data = (await Conexionfirebase.firebase
                .Child("Reservaciones")
                .OrderByKey()
                .OnceAsync<MoReservaciones>()).Where(a => a.Object.fecha_Reserv == fecha)
                .Where(b => b.Object.status == estado);
            foreach (var rdr in data)
            {
                MoReservaciones pila = new MoReservaciones();
                pila.id_Reserv = rdr.Key;
                pila.nombreEstilista = rdr.Object.nombreEstilista;
                pila.precio = rdr.Object.precio;
                pila.fecha_Reserv = rdr.Object.fecha_Reserv;
                pila.nombre_usuario = rdr.Object.nombre_usuario;
                pila.hora_Reserv = rdr.Object.hora_Reserv;
                pila.status = rdr.Object.status;
                pila.tipo_Reserv = rdr.Object.tipo_Reserv;
                pila.calificacion = rdr.Object.calificacion;

                lstReservaciones.Add(pila);
            }
            return lstReservaciones;
        }

        public async Task<List<MoReservaciones>> getReservaciones(MoReservaciones parametros)
        {
           List<MoReservaciones> lstReservaciones = new List<MoReservaciones>();

            var data = (await Conexionfirebase.firebase
                .Child("Reservaciones")
                .OrderByKey()
                .OnceAsync<MoReservaciones>()).Where(a => a.Object.fecha_Reserv == parametros.fecha_Reserv);
            foreach (var rdr in data)
            {
                MoReservaciones pila = new MoReservaciones();
                pila.id_Reserv = rdr.Key;
                pila.nombreEstilista = rdr.Object.nombreEstilista;
                pila.nombre_usuario = rdr.Object.nombre_usuario;
                pila.fecha_Reserv = rdr.Object.fecha_Reserv;
                pila.hora_Reserv = rdr.Object.hora_Reserv;
                pila.status = rdr.Object.status;
                pila.precio = rdr.Object.precio;
                pila.tipo_Reserv = rdr.Object.tipo_Reserv;
                pila.calificacion = rdr.Object.calificacion;

                Reservaciones.Add(pila);
            }
            return Reservaciones;
        }

        public async Task<List<MoReservaciones>> getReservacionesStilista(MoReservaciones idCliente)
        {

            List<MoReservaciones> list2 = new List<MoReservaciones>();

            var data = (await Conexionfirebase.firebase
                .Child("Reservaciones")
                .OrderByKey()
                .OnceAsync<MoReservaciones>())
                .Where(a => a.Object.fecha_Reserv == idCliente.fecha_Reserv)
                .Where(a => a.Object.nombreEstilista == idCliente.nombreEstilista);
            foreach (var rdr in data)
            {
                var lstHorarios = new MoReservaciones();
                lstHorarios.hora_Reserv = rdr.Object.hora_Reserv;



                list2.Add(lstHorarios);
            }
            return list2;
        }

        public async Task<List<MoReservaciones>> obtenerlistabase(MoReservaciones obtenerhoras, string nombreEstilista)
        {
            var data = (await Conexionfirebase.firebase
                .Child("Reservaciones")
                .OrderByKey()
                .OnceAsync<MoReservaciones>()).Where(a => a.Object.fecha_Reserv == obtenerhoras.fecha_Reserv)
                .Where(a => a.Object.nombreEstilista == obtenerhoras.nombreEstilista);
            foreach (var rdr in data)
            {
                obtenerhoras.id_Cliente = rdr.Object.id_Cliente;
                obtenerhoras.hora_Reserv = rdr.Object.hora_Reserv;
                obtenerhoras.precio = rdr.Object.precio;
                obtenerhoras.fecha_Reserv = rdr.Object.fecha_Reserv;
                obtenerhoras.nombreEstilista = rdr.Object.nombreEstilista;
                obtenerhoras.status = rdr.Object.status;


                Reservaciones.Add(obtenerhoras);

            }
            return Reservaciones;
        }

        public async Task Modificar(MoReservaciones parametros)
        {
            var obtenerData = (await Conexionfirebase.firebase
                .Child("Reservaciones") //comparamos si es la misma key
                .OnceAsync<MoReservaciones>()).Where(a => a.Object.id_Cliente == parametros.id_Cliente).FirstOrDefault();

            await Conexionfirebase.firebase
                .Child("Reservaciones")
                .Child(obtenerData.Key)
                .PutAsync(new MoReservaciones()
                {
                    id_Cliente = parametros.id_Cliente,
                    status = parametros.status,
                    precio = parametros.precio,
                    nombre_usuario = parametros.nombre_usuario,
                    nombreEstilista = parametros.nombreEstilista,
                    tipo_Reserv = parametros.tipo_Reserv,
                    hora_Reserv = parametros.hora_Reserv,
                    fecha_Reserv = parametros.fecha_Reserv,
                    calificacion = parametros.calificacion

                });
        }

        public async Task<List<MoReservaciones>> ObtenerDatosReservaciones(MoReservaciones parametros)
        {
            Reservaciones = new List<MoReservaciones>();
            var data = (await Conexionfirebase.firebase
                .Child("Reservaciones")
                .OrderByKey()
                .OnceAsync<MoReservaciones>()).Where(a => a.Object.id_Cliente == parametros.id_Cliente)
                ;
            foreach (var rdr in data)
            {
                var reser = new MoReservaciones();
                reser.id_Reserv = rdr.Key;
                reser.id_Cliente = rdr.Object.id_Cliente;
                reser.nombre_usuario = rdr.Object.nombre_usuario;
                reser.nombreEstilista = rdr.Object.nombreEstilista;
                reser.hora_Reserv = rdr.Object.hora_Reserv;
                reser.fecha_Reserv = rdr.Object.fecha_Reserv;
                reser.calificacion = rdr.Object.calificacion;
                reser.status = rdr.Object.status;
                reser.precio = rdr.Object.precio;
                reser.tipo_Reserv = rdr.Object.tipo_Reserv;

                Reservaciones.Add(reser);

            }
            return Reservaciones;

        }

        public async Task postUserReservacion(MoReservaciones datos)
        {
            var data = (await Conexionfirebase.firebase
                    .Child("Reservaciones")
                    .OnceAsync<MoReservaciones>()).Where(a => a.Key == datos.id_Reserv)
                    .Where(a => a.Object.id_Cliente == datos.id_Cliente).FirstOrDefault();
            data.Object.nombre_usuario = datos.nombre_usuario;
            await Conexionfirebase.firebase
                .Child("Reservaciones")
                .Child(data.Key)
                .PutAsync(data.Object);
        }

        public async Task<List<MoReservaciones>> ObtenerDatosHoy(MoReservaciones parametros)
        {
            var data = (await Conexionfirebase.firebase
                .Child("Reservaciones")
                .OrderByKey()
                .OnceAsync<MoReservaciones>()).Where(a => a.Object.id_Cliente == parametros.id_Cliente)
                .Where(b => b.Object.fecha_Reserv == parametros.fecha_Reserv);
            foreach (var rdr in data)
            {

                var ope = new MoReservaciones();
                ope.id_Reserv = rdr.Key;
                ope.id_Cliente = rdr.Object.id_Cliente;
                ope.nombre_usuario = rdr.Object.nombre_usuario;
                ope.nombreEstilista = rdr.Object.nombreEstilista;
                ope.hora_Reserv = rdr.Object.hora_Reserv;
                ope.fecha_Reserv = rdr.Object.fecha_Reserv;
                ope.calificacion = rdr.Object.calificacion;
                ope.status = rdr.Object.status;
                ope.precio = rdr.Object.precio;
                ope.tipo_Reserv = rdr.Object.tipo_Reserv;

                Reservaciones.Add(ope);

            }

            return Reservaciones;
        }

        public async Task EditarReseña(Mresenias idReservacion)
        {
            var data = (await Conexionfirebase.firebase
                .Child("Reservaciones")
                .OnceAsync<MoReservaciones>()).Where(a => a.Key == idReservacion.idreservacion).FirstOrDefault();
            data.Object.calificacion = int.Parse(idReservacion.calificacion);
            await Conexionfirebase.firebase
                .Child("Reservaciones")
                .Child(data.Key)
                .PutAsync(data.Object);
        }

        public async Task setEstatus(MoReservaciones idReservacion)
        {
            var data = (await Conexionfirebase.firebase
                .Child("Reservaciones")
                .OnceAsync<MoReservaciones>()).Where(a => a.Key == idReservacion.id_Reserv).FirstOrDefault();
            data.Object.status = idReservacion.status;
            await Conexionfirebase.firebase
                .Child("Reservaciones")
                .Child(data.Key)
                .PutAsync(data.Object);
        }

        public async Task EliminarReservacion(MoReservaciones parametros)
        {
            var data = (await Conexionfirebase.firebase
                .Child("Reservaciones")
                .OnceAsync<MoReservaciones>()).Where((a) => a.Key == parametros.id_Reserv).FirstOrDefault();
            //eliminar
            await Conexionfirebase.firebase.Child("Reservaciones").Child(data.Key).DeleteAsync();
        }


        public async Task<List<MoReservaciones>> getResevacionesEstilista(MoReservaciones parametros)
        {
            var data = (await Conexionfirebase.firebase
                .Child("Reservaciones")
                .OrderByKey()
                .OnceAsync<MoReservaciones>()).Where(a => a.Object.nombreEstilista == parametros.nombreEstilista)
                .Where(b => b.Object.fecha_Reserv == parametros.fecha_Reserv)
                .Where(c => c.Object.status == parametros.status);
            foreach (var rdr in data)
            {

                var ope = new MoReservaciones();
                ope.id_Reserv = rdr.Key;
                ope.id_Cliente = rdr.Object.id_Cliente;
                ope.nombre_usuario = rdr.Object.nombre_usuario;
                ope.nombreEstilista = rdr.Object.nombreEstilista;
                ope.hora_Reserv = rdr.Object.hora_Reserv;
                ope.fecha_Reserv = rdr.Object.fecha_Reserv;
                ope.calificacion = rdr.Object.calificacion;
                ope.status = rdr.Object.status;
                ope.precio = rdr.Object.precio;
                ope.tipo_Reserv = rdr.Object.tipo_Reserv;

                Reservaciones.Add(ope);

            }

            return Reservaciones;
        }


        public async Task<List<MoReservaciones>> getGeneralEstilista(MoReservaciones parametros)
        {
            var data = (await Conexionfirebase.firebase
                .Child("Reservaciones")
                .OrderByKey()
                .OnceAsync<MoReservaciones>()).Where(a => a.Object.nombreEstilista == parametros.nombreEstilista);
            foreach (var rdr in data)
            {
                var ope = new MoReservaciones();
                ope.id_Reserv = rdr.Key;
                ope.id_Cliente = rdr.Object.id_Cliente;
                ope.nombre_usuario = rdr.Object.nombre_usuario;
                ope.nombreEstilista = rdr.Object.nombreEstilista;
                ope.hora_Reserv = rdr.Object.hora_Reserv;
                ope.fecha_Reserv = rdr.Object.fecha_Reserv;
                ope.calificacion = rdr.Object.calificacion;
                ope.status = rdr.Object.status;
                ope.precio = rdr.Object.precio;
                ope.tipo_Reserv = rdr.Object.tipo_Reserv;

                Reservaciones.Add(ope);

            }

            return Reservaciones;
        }
    }
}
