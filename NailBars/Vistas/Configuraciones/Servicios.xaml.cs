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
using Rg.Plugins.Popup.Services;
using NailBars.Components;
using Rg.Plugins.Popup.Extensions;

namespace NailBars.Vistas.Configuraciones
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Servicios : ContentPage
    {
        string Idusuario;
        string nombreestilista;
        string date;
        string tipo;
        MusuariosClientes datUser = new MusuariosClientes();
        public Servicios()
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
                var guardarId = JsonConvert.DeserializeObject<FirebaseAuth>(Preferences.Get("MyFirebaseRefreshToken", ""));
                Idusuario = guardarId.User.LocalId;

                validarTipoUser();
            }
            catch (Exception)
            {
                await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Aviso", "Su sesión a expirado.", JMDialog.Danger), true);
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
                VmReservaciones consulta = new VmReservaciones();
                MoReservaciones parametro5 = new MoReservaciones();
                parametro5.id_Cliente = Idusuario;
                lstGeneral.ItemsSource = await consulta.ObtenerDatosReservaciones(parametro5);

                VmReservaciones consulta2 = new VmReservaciones();
                MoReservaciones parametro2 = new MoReservaciones();
                parametro2.id_Cliente = Idusuario;
                parametro2.fecha_Reserv = date;
            }
            else if (tipo == "admin")
            {
                string fec = DateTime.Now.ToString("d/M/yyyy");

                VmReservaciones consulta = new VmReservaciones();
                VmReservaciones consulta2 = new VmReservaciones();
                MoReservaciones parametro1 = new MoReservaciones();
                parametro1.fecha_Reserv = fec;

                lstGeneral.ItemsSource = await consulta.getReservaciones(parametro1);
            }
            else if (tipo == "Empleado")
            {
                string fec = DateTime.Now.ToString("d/M/yyyy");

                VmReservaciones consulta = new VmReservaciones();
                VmReservaciones consulta2 = new VmReservaciones();
                MoReservaciones parametro1 = new MoReservaciones();
                parametro1.fecha_Reserv = fec;
                parametro1.status = "Pendiente";
                parametro1.nombreEstilista = nombreestilista;

                lstGeneral.ItemsSource = await consulta.getGeneralEstilista(parametro1);
            }
        }

        private async void lstGeneral_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MoReservaciones itemSelect = new MoReservaciones();
            VmReservaciones consulta = new VmReservaciones();

            if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
            {
                itemSelect = e.CurrentSelection[0] as MoReservaciones;
            }

            if (tipo == "Empleado")
            {
                string selection = await DisplayActionSheet("Seleccione una opción", null, null, "Eliminar", "Dar de alta");
                if (selection == "Dar de alta")
                {
                    itemSelect.status = "Finalizada";
                    await consulta.setEstatus(itemSelect);
                    await validarTipoUser();
                }
                else if (selection == "Eliminar")
                {
                    VmReservaciones funcion = new VmReservaciones();
                    MoReservaciones parametros = new MoReservaciones();
                    parametros.id_Reserv = itemSelect.id_Reserv;
                    await funcion.EliminarReservacion(parametros);
                    await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Satisfactorio", "La reservación se a eliminado satisfactoriamente.", JMDialog.Success), true);
                    await validarTipoUser();
                }
            }
            else if (tipo == "Cliente")
            {
                if (itemSelect.status == "Finalizada")
                {
                    string selection = await DisplayActionSheet("Seleccione una opción", null, null, "Calificar");

                    if (selection == "Calificar")
                    {
                        await PopupNavigation.Instance.PushAsync(new Calificar(itemSelect));
                        await validarTipoUser();
                    }
                }
                else if (itemSelect.status == "Pendiente")
                {
                    string selection = await DisplayActionSheet("Seleccione una opción", null, null, "Eliminar");

                    if (selection == "Eliminar")
                    {

                        VmReservaciones funcion = new VmReservaciones();
                        MoReservaciones parametros = new MoReservaciones();
                        parametros.id_Reserv = itemSelect.id_Reserv;
                        await funcion.EliminarReservacion(parametros);
                        await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Satisfactorio", "La reservación se a eliminado satisfactoriamente.", JMDialog.Success), true);
                        await validarTipoUser();
                    }
                }
            }
        }
    }
}