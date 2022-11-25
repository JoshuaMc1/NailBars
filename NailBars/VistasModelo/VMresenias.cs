using NailBars.Modelo;
using System;
using System.Collections.Generic;
using System.Text;
using NailBars.Servicios;
using System.Threading.Tasks;
using System.Linq;
using Firebase.Database.Query;

namespace NailBars.VistasModelo
{
    public class VMresenias
    {
        public async Task InsertarReseña(Mresenias parametros)
        {
            await Conexionfirebase.firebase
                .Child("Resenias")
                .PostAsync(new Mresenias()
                {
                    calificacion = parametros.calificacion,
                    idusuario = parametros.idusuario,
                    reseña = parametros.reseña,
                    idreservacion = parametros.idreservacion,
                });
        }

        public async Task EditarReseña(Mresenias parametros)
        {
            var data = (await Conexionfirebase.firebase
                .Child("Resenias")
                .OnceAsync<Mresenias>()).Where(a => a.Key == parametros.idcalificacion).FirstOrDefault();
            data.Object.reseña = parametros.reseña;
            data.Object.calificacion = parametros.calificacion;
            await Conexionfirebase.firebase
                .Child("Resenias")
                .Child(data.Key)
                .PutAsync(data.Object);
        }

        public async Task<List<Mresenias>> obtenerdatos(Mresenias parametrosPedir)
        {
            var Resenias = new List<Mresenias>();
            var data = (await Conexionfirebase.firebase
                .Child("Resenias")
                .OrderByKey()
                .OnceAsync<Mresenias>()).Where(a => a.Object.idreservacion == parametrosPedir.idreservacion);
            foreach (var dt in data)
            {
                var parametros = new Mresenias();
                parametros.reseña = dt.Object.reseña;
                parametros.calificacion = dt.Object.calificacion;
                parametros.idcalificacion = dt.Key;
                Resenias.Add(parametros);
            }
            return Resenias;
        }
    }
}
