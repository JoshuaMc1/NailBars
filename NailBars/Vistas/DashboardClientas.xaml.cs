using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using NailBars.VistasModelo;
using NailBars.Modelo;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using Firebase.Auth;
using NailBars.Servicios;
using Rg.Plugins.Popup.Services;
using Acr.UserDialogs;

namespace NailBars.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DashboardClientas : ContentPage
    {

        string Idusuario;
        string nombreestilista;
        string date;
        string tipo;
        MusuariosClientes datUser = new MusuariosClientes();
        public DashboardClientas()
        {
            InitializeComponent();
            ObtenerIdusuario();
            validarTipoUser();
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await validarTipoUser();


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

                validarTipoUser();


            }
            catch (Exception)
            {

                await DisplayAlert("Alerta", "Sesion expirada", "OK");
            }

        }

        private async Task validarTipoUser()
        {

            VMusuarios funcion = new VMusuarios();
            MusuariosClientes parametro = new MusuariosClientes();
            parametro.Id_usuario = Idusuario;
            var datos = await funcion.getadmin(parametro);
            foreach (var rdr in datos)
            {
                //var nombre = new MusuariosClientes();

                tipo = rdr.tipoUser;
                nombreestilista = rdr.Nombres;
                // datUser.IdUsuariosClientes = rdr.IdUsuariosClientes;

            }

            date = DateTime.Now.ToString("d/M/yyyy");

            if (tipo == "Cliente")
            {
                encabezado1.Text = "Hoy";

                VmReservaciones consulta = new VmReservaciones();
                MoReservaciones parametro5 = new MoReservaciones();
                parametro5.id_Cliente = Idusuario;
                lstGeneral.ItemsSource = await consulta.ObtenerDatosReservaciones(parametro5);

                VmReservaciones consulta2 = new VmReservaciones();
                MoReservaciones parametro2 = new MoReservaciones();
                parametro2.id_Cliente = Idusuario;
                parametro2.fecha_Reserv = date;

                lstReserUser.ItemsSource = await consulta2.ObtenerDatosHoy(parametro2);





            }
            else if (tipo == "admin")
            {
                encabezado1.Text = "Pendientes Hoy!!";


                string fec = DateTime.Now.ToString("d/M/yyyy");

                VmReservaciones consulta = new VmReservaciones();
                VmReservaciones consulta2 = new VmReservaciones();
                MoReservaciones parametro1 = new MoReservaciones();
                parametro1.fecha_Reserv = fec;

                lstReserUser.ItemsSource = await consulta2.getResrOfDate(fec, "Pendiente");
                lstGeneral.ItemsSource = await consulta.getReservaciones(parametro1);
                //lista2 = await consulta.getReservaciones(parametro1);

            }
            else if (tipo == "Empleado")
            {
                encabezado1.Text = "Pendientes Hoy!!";


                string fec = DateTime.Now.ToString("d/M/yyyy");

                VmReservaciones consulta = new VmReservaciones();
                VmReservaciones consulta2 = new VmReservaciones();
                MoReservaciones parametro1 = new MoReservaciones();
                parametro1.fecha_Reserv = fec;
                parametro1.status = "Pendiente";
                parametro1.nombreEstilista = nombreestilista;


                lstReserUser.ItemsSource = await consulta2.getResevacionesEstilista(parametro1);
                lstGeneral.ItemsSource = await consulta.getGeneralEstilista(parametro1);
                //lista2 = await consulta.getReservaciones(parametro1);

            }


        }


        private async void lstGeneral_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            


        }




    }
}