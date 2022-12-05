using Acr.UserDialogs;
using NailBars.Components;
using NailBars.Modelo;
using NailBars.VistasModelo;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NailBars.Vistas
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class VReservaciones : ContentPage
	{
        private string IdUsuario, Precio, FechaReservacion, NombreEstilista = "-", NombreUsuario, HoraReservacion, TipoReservacion;
        private string[] ImageService = { "manicure", "pedicure", "retoquemanos", "uacrilicas" };
        private string[] Servicio = { "Manicure", "Pedicure", "Retoque Acrílico", "Colocación y pintado de uñas acrílicas" };
        private int ContadorEstilista, ContadorReservaciones;

        List<string> Horario = new List<string>();
        List<string> HoriosVacios = new List<string>();

        private MusuariosClientes Data;

        public VReservaciones ()
		{
			InitializeComponent();
        }

        public VReservaciones(string idUsuario, int Service, string Reservacion, MusuariosClientes data, string Precio, string[] Descripcion)
        {
            InitializeComponent();
            var statusbar = new VMPrincipal();
            statusbar.Transparente();
            MostrarEstilistas();
            this.IdUsuario = idUsuario;
            this.TipoReservacion = Reservacion;
            this.Data = data;
            this.Precio = Precio;
            if (this.Data.tipoUser == "Cliente")
            {
                txtPrecio.IsEnabled = false;
            }
            else if (this.Data.tipoUser == "admin")
            {
                txtPrecio.IsEnabled = true;
            }
            txtServicio.Text = Servicio[Service];
            txtDesc1.Text = Descripcion[0];
            txtDesc2.Text = Descripcion[1];
            txtDesc3.Text = Descripcion[2];
            txtPrecio.Text = "L. " + Precio;
            ImagenServicio(Reservacion);
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            txtFechaReservacion.MinimumDate = DateTime.Now;
        }

        private void ImagenServicio(string Servicio)
        {
            if(Servicio.Equals("Manicure"))
            {
                txtImgServicio.Source = ImageService[0];
            } else if (Servicio.Equals("Pedicure"))
            {
                txtImgServicio.Source = ImageService[1];
            } else if (Servicio.Equals("AplicaciondeAcrilico"))
            {
                txtImgServicio.Source = ImageService[3];
            } else if (Servicio.Equals("PintadoYDecoracion"))
            {
                txtImgServicio.Source = ImageService[2];
            }
        }

        private async Task LlenarArrglo()
        {
            Horario.Clear();
            Horario.Add("13:00:00");
            Horario.Add("13:30:00");
            Horario.Add("14:00:00");
            Horario.Add("14:30:00");
            Horario.Add("15:00:00");
            Horario.Add("15:30:00");
            Horario.Add("16:00:00");
            Horario.Add("16:30:00");
            Horario.Add("17:00:00");
            Horario.Add("17:30:00");
            Horario.Add("18:00:00");
            Horario.Add("18:30:00");
        }

        private async void txtListaEstilista_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var itemSelected = (Trabajadores)e.CurrentSelection.FirstOrDefault();
                NombreEstilista = itemSelected.nombre;
                await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Notificación", "A seleccionado a: " + NombreEstilista + " como estilista que le atendera.", JMDialog.Information), true);
                if (FechaReservacion != "" && FechaReservacion != null)
                {
                    await ObtenerDatoUsuario();
                    await horariosEstilista();
                }
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }
        }

        private void txtHorario_SelectedIndexChanged(object sender, EventArgs e)
        {
            HoraReservacion = txtHorario.SelectedItem.ToString();
        }

        private async void txtReservar_Clicked(object sender, EventArgs e)
        {
            MoReservaciones reservacion = new MoReservaciones();
            VmReservaciones db = new VmReservaciones();

            if (HoraReservacion != "" && HoraReservacion != null)
            {
                if (ContadorReservaciones <= 60)
                {
                    if (ContadorEstilista <= 12)
                    {
                        UserDialogs.Instance.ShowLoading("Creando Reservacion...");
                        reservacion.id_Cliente = IdUsuario;
                        reservacion.nombreEstilista = NombreEstilista;
                        reservacion.nombre_usuario = NombreUsuario;
                        reservacion.tipo_Reserv = TipoReservacion;
                        reservacion.fecha_Reserv = FechaReservacion;
                        reservacion.hora_Reserv = HoraReservacion;
                        reservacion.precio = Precio;
                        reservacion.status = "Pendiente";
                        reservacion.calificacion = 0;

                        await db.InsertReservacion(reservacion);
                        UserDialogs.Instance.HideLoading();
                        Application.Current.MainPage = new NavigationPage(new TipoPagos());
                    }
                    else
                    {
                        await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Advertencia", "La estilista no esta disponible.", JMDialog.Warning), true);
                    }
                }
                else
                {
                    Application.Current.MainPage = new NavigationPage(new PagoReservacion());
                }
            }
            else
            {
                await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Advertencia", "Debe seleccionar una hora!", JMDialog.Warning), true);
            }
        }

        private async void txtFechaReservacion_DateSelected(object sender, DateChangedEventArgs e)
        {
            FechaReservacion = String.Concat(e.NewDate.Day.ToString(), "/", e.NewDate.Month.ToString(), "/", e.NewDate.Year.ToString());
            if (!NombreEstilista.Equals("-"))
            {
                await ObtenerDatoUsuario();
                await horariosEstilista();
            }
            else
            {
                await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Información", "Seleccione un estilista para ver los horarios disponibles...", JMDialog.Information), true);
            }
        }

        private async void txtFechaReservacion_Focused(object sender, FocusEventArgs e)
        {
            if (FechaReservacion == null)
            {
                if (NombreEstilista != "-")
                {
                    FechaReservacion = DateTime.Now.ToString("d/M/yyyy");
                    await horariosEstilista();
                    await ObtenerDatoUsuario();
                }
            }
        }

        private async Task MostrarEstilistas()
        {
            var funcion = new VMcategorias();
            var data = await funcion.getestilistas();
            txtListaEstilista.ItemsSource = data;
        }

        private async Task ObtenerDatoUsuario()
        {
            VMusuarios funcion = new VMusuarios();

            MusuariosClientes parametros = new MusuariosClientes();
            parametros.Id_usuario = IdUsuario;
            var dt = await funcion.ObtenerDatosUsuarios(parametros);
            foreach (var fila in dt)
            {
                NombreUsuario = fila.Nombres;
            }
            MoReservaciones parametros2 = new MoReservaciones();
            parametros2.id_Cliente = IdUsuario;
            parametros2.nombreEstilista = NombreEstilista;
            var dt2 = await funcion.contar(parametros2, FechaReservacion);
            var dt3 = await funcion.contarEstilista(parametros2, FechaReservacion);

            foreach (var fila2 in dt2)
            {

                ContadorReservaciones = ContadorReservaciones + 1;
            }

            foreach (var fila2 in dt3)
            {

                ContadorEstilista = ContadorEstilista + 1;
            }

            string hora;

            VmReservaciones funcion2 = new VmReservaciones();
            MoReservaciones obtenerhoras = new MoReservaciones();
            obtenerhoras.fecha_Reserv = Convert.ToString(txtFechaReservacion);
            obtenerhoras.nombreEstilista = NombreEstilista;
            var horabase = await funcion2.obtenerlistabase(obtenerhoras, NombreEstilista);
            foreach (var fila in horabase)
            {
                hora = fila.hora_Reserv;
            }
        }

        private async Task horariosEstilista()
        {
            txtHorario.ItemsSource = HoriosVacios;

            int cot = 0;
            MoReservaciones horas = new MoReservaciones();
            VmReservaciones validar = new VmReservaciones();
            horas.fecha_Reserv = FechaReservacion;
            horas.nombreEstilista = NombreEstilista;
            string diaActual = DateTime.Now.ToString("d/MM/yyyy");
            if(FechaReservacion.Equals(diaActual))
            {
                Int32 horaActual = DateTime.Now.Hour;
                Int32 minutoActual = DateTime.Now.Minute;
                Int32 horaFinal = 18;
                Int32 minutoFinal = 30;

                if (horaActual <= horaFinal && minutoActual <= minutoFinal)
                {
                    await LlenarArrglo();
                    var rest = await validar.getReservacionesStilista(horas);
                    foreach (var rst in rest)
                    {
                        cot = 0;
                        while (cot < Horario.Count)
                        {
                            if (rst.hora_Reserv == Horario[cot])
                            {
                                Horario.Remove(rst.hora_Reserv);
                            }
                            cot += 1;
                        }
                    }
                } else await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Información", "Se han agorado las reservaciones para el dia de hoy.", JMDialog.Warning), true);
            } else
            {
                await LlenarArrglo();
                var rest = await validar.getReservacionesStilista(horas);
                foreach (var rst in rest)
                {
                    cot = 0;
                    while (cot < Horario.Count)
                    {
                        if (rst.hora_Reserv == Horario[cot])
                        {
                            Horario.Remove(rst.hora_Reserv);
                        }
                        cot += 1;
                    }
                }
            }
            txtHorario.ItemsSource = Horario;
        }
    }
}