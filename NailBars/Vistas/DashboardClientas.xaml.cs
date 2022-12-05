using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using NailBars.VistasModelo;
using NailBars.Modelo;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using Firebase.Auth;
using NailBars.Servicios;

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
            var statusbar = new VMPrincipal();
            statusbar.CambiarColor();
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
                var guardarId = JsonConvert.DeserializeObject<FirebaseAuth>(Preferences.Get("MyFirebaseRefreshToken", ""));
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
                tipo = rdr.tipoUser;
                nombreestilista = rdr.Nombres;
            }

            date = DateTime.Now.ToString("d/M/yyyy");

            if (tipo == "Cliente")
            {
                encabezado1.Text = "Reservaciones pendientes para Hoy";

                VmReservaciones consulta2 = new VmReservaciones();
                MoReservaciones parametro2 = new MoReservaciones();
                parametro2.id_Cliente = Idusuario;
                parametro2.fecha_Reserv = date;

                lstReserUser.ItemsSource = await consulta2.ObtenerDatosHoy(parametro2);
            }
            else if (tipo == "admin")
            {
                encabezado1.Text = "Reservaciones pendientes para Hoy!!";

                string fec = DateTime.Now.ToString("d/M/yyyy");

                VmReservaciones consulta = new VmReservaciones();
                VmReservaciones consulta2 = new VmReservaciones();
                MoReservaciones parametro1 = new MoReservaciones();
                parametro1.fecha_Reserv = fec;

                lstReserUser.ItemsSource = await consulta2.getResrOfDate(fec, "Pendiente");
            }
            else if (tipo == "Empleado")
            {
                encabezado1.Text = "Reservaciones pendientes para Hoy!!";

                string fec = DateTime.Now.ToString("d/M/yyyy");

                VmReservaciones consulta = new VmReservaciones();
                VmReservaciones consulta2 = new VmReservaciones();
                MoReservaciones parametro1 = new MoReservaciones();
                parametro1.fecha_Reserv = fec;
                parametro1.status = "Pendiente";
                parametro1.nombreEstilista = nombreestilista;

                lstReserUser.ItemsSource = await consulta2.getResevacionesEstilista(parametro1);
            }
        }
    }
}