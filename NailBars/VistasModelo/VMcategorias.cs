using NailBars.Modelo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NailBars.Servicios;
using Firebase.Database.Query;
using System.Linq;

namespace NailBars.VistasModelo
{
    public class VMcategorias
    {

        List<Trabajadores> estilistas = new List<Trabajadores>();
        List<MusuariosClientes> Usuarios = new List<MusuariosClientes>();

        public async Task InsertarCategorias(Mcategorias parametros)
        {

            await Conexionfirebase.firebase
                .Child("Categorias")
                .PostAsync(new Mcategorias()
                {
                    Categoria = parametros.Categoria,
                    Color = parametros.Color,
                    Foto = parametros.Foto,
                    Prioridad = parametros.Prioridad
                    
                });
        }


        public async Task InsertarTrabajadoras(Trabajadores parametros)
        {

            await Conexionfirebase.firebase
                .Child("Trabajadores")
                .PostAsync(new Trabajadores()
                {
                    nombre = parametros.nombre,
                    Pass = parametros.Pass,
                    Icono = parametros.Icono,
                    Correo = parametros.Correo,
                    tipoUser = parametros.tipoUser,

                });
        }






        public async Task<List<Mcategorias>> MostrarCategoriasNormal()
        {
            var Categorias = new List<Mcategorias>();
            var funcionNegocios = new VMusuarios();
            var parametrosNegocios = new VMcrearcuenta();
            var data = (await Conexionfirebase.firebase
                .Child("Categorias")
                .OrderByKey()
                .OnceAsync<Mcategorias>()).Where(a => a.Object.Prioridad == "0");
            foreach (var dt in data)
            {
                Mcategorias parametros = new Mcategorias();
                parametros.Categoria = dt.Object.Categoria;
                parametros.Foto = dt.Object.Foto;
                parametros.Idcategoria = dt.Key;
                parametros.Color = dt.Object.Color;
               // parametrosNegocios.idcategoria = dt.Key;
               
                Categorias.Add(parametros);
            }
            return Categorias;
        }


        public async Task<List<Trabajadores>> getestilistas()
        {
            var data = await Conexionfirebase.firebase
                .Child("Trabajadores")
                .OrderByKey()
                .OnceAsync<Trabajadores>();
            foreach (var rdr in data)
            {
                Trabajadores parametros = new Trabajadores();
               // parametros.idt = rdr.Key;
                parametros.nombre = rdr.Object.nombre;

                estilistas.Add(parametros);
                
            }
            return estilistas;
        }

    }
}
