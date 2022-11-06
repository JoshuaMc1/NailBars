using NailBars.VistasModelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NailBars.Modelo;
using NailBars.VistasModelo;
using NailBars.Vistas.Configuraciones;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Acr.UserDialogs;
using System.Diagnostics;

namespace NailBars.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VistaReservaciones : ContentPage
    {
        string fecReservacion1;
        string tipoReservacion;
        string idUser;
        string nombreUsuario;
        string Iduserclientes;
        string nombreStilista="-";
        int contReservaciones;
        int contREstilista;
        int contador;
        string horaReserv;
        string precio;


        List<string> horario = new List<string>();
        List<string> horiosvacio = new List<string>();
        MusuariosClientes infoUser = new MusuariosClientes();



        public VistaReservaciones()
        {
            InitializeComponent();
           
            mostrarestilistas();

            fecReservacion.Date = new DateTime(1900,1,1);



        }


        public VistaReservaciones(string idUsuario, string Reservacion, MusuariosClientes dat,string pre)
        {
            InitializeComponent();
            mostrarestilistas();
            tipoReservacion = Reservacion;
            idUser = idUsuario;
            llenarArrglo();
            infoUser = dat;
            precio = pre;
            txtPrecio.Text = precio;
            if(dat.tipoUser == "Cliente")
            {
                txtPrecio.IsEnabled=false;
            }else if (dat.tipoUser == "admin")
            {
                txtPrecio.IsEnabled = true;
            }


            mostrarTipoReser();

        }

        private async Task mostrarestilistas()
        {
            var funcion = new VMcategorias();
            var funcion1 = new VMusuarios();
            var dt = await funcion.getestilistas();
            PickEstilista.ItemsSource = dt;

            
            List<MoReservaciones> lista = new List<MoReservaciones>();
          //  lista = await funcion1.contar();
            string cont = Convert.ToString(lista.Count());

         
               
          
        }

 

        private async void fecReservacion_DateSelected(object sender, DateChangedEventArgs e)
        {

            // fecReservacion.MinimumDate = DateTime.Now;
            // fecReservacion1 = DateTime.Now.ToString("d/M/yyyy");
            
            fecReservacion1 = String.Concat(e.NewDate.Day.ToString(),"/",e.NewDate.Month.ToString(),"/", e.NewDate.Year.ToString());
            //await DisplayAlert("Aviso",fecReservacion1,"Ok");

            if(nombreStilista != "-") 
            {
                await ObtenerDatoUsuario();
                 await horariosEstilista();
            }else
            {
                await DisplayAlert("Aviso","Seleccione un estilista para ver horarios disponibles", "Ok");
            }
            

                //await DisplayAlert("Aviso","Selecione una fecha, para mostrar los horarios disponibles", "Ok");        
        }

        protected async override void OnAppearing()
        {
            // Obterner datos del usuario para guardar en la nueva reservación
            base.OnAppearing();
            fecReservacion.MinimumDate = DateTime.Now;
            //fecReservacion.Date = DateTime.Now;
        }

        private async Task ObtenerDatoUsuario()
        {
            VMusuarios funcion = new VMusuarios();


            MusuariosClientes parametros = new MusuariosClientes();
            parametros.Id_usuario = idUser;
            var dt = await funcion.ObtenerDatosUsuarios(parametros);
            foreach (var fila in dt)
            {
                nombreUsuario = fila.Nombres;            
            }
           
          
           


            MoReservaciones parametros2 = new MoReservaciones();
            parametros2.id_Cliente = idUser;
            parametros2.nombreEstilista = nombreStilista;
            var dt2 = await funcion.contar(parametros2,fecReservacion1);
            var dt3 = await funcion.contarEstilista(parametros2, fecReservacion1);

            foreach (var fila2 in dt2)
            {

                contReservaciones = contReservaciones + 1;
            }

            foreach (var fila2 in dt3)
            {

                contREstilista = contREstilista + 1;
            }

            string hora;

            VmReservaciones funcion2 = new VmReservaciones();
            MoReservaciones obtenerhoras = new MoReservaciones();
            obtenerhoras.fecha_Reserv = Convert.ToString(fecReservacion);
            obtenerhoras.nombreEstilista = nombreStilista;
            var horabase = await funcion2.obtenerlistabase(obtenerhoras, nombreStilista);
            foreach (var fila in horabase)
            {
                hora = fila.hora_Reserv;

               

            }

            //ListaHorasLocal = hora;






           // await DisplayAlert("cantidad", Convert.ToString(contREstilista), "ok");



        }
       
        private async void btnhora_Clicked(object sender, EventArgs e)
        {

            MoReservaciones reservacion = new MoReservaciones();
                            VmReservaciones db = new VmReservaciones();

            if (horaReserv!="" && horaReserv!=null)
            {
                                            if (contReservaciones<=60)
                                            {
                                                if (contREstilista <= 12) {
                                                    UserDialogs.Instance.ShowLoading("Creando Reservacion...");
                                                    reservacion.id_Cliente = idUser;
                                                    reservacion.nombreEstilista = nombreStilista;
                                                    reservacion.nombre_usuario = nombreUsuario;
                                                    reservacion.tipo_Reserv = tipoReservacion;
                                                    reservacion.fecha_Reserv = fecReservacion1;
                                                    reservacion.hora_Reserv = horaReserv;
                                                    reservacion.precio = precio;
                                                    reservacion.status = "Pendiente";
                                                    reservacion.calificacion = 0;

                                                    await db.InsertReservacion(reservacion);
                                                    UserDialogs.Instance.HideLoading();
                        await Navigation.PushAsync(new PagoReservacion());

                        //await DisplayAlert("Listo", "Reservacion enviada", "OK");

                                                    


                                                   // Application.Current.MainPage = new NavigationPage(new Contenedor());
                    }
                                                else
                                                {
                                                 await DisplayAlert("Aviso", "Estilista nos disponible", "OK");
                                                 } 
                                             }
                                            else
                                            {

                    await Navigation.PushAsync(new PagoReservacion());
                    //await DisplayAlert("Listo", "Reservacion enviada", "OK");
                                            }
            }
            else
            {
                
                await DisplayAlert("Aviso", "Seleccionar Hora!!", "OK");
            }
                                

           


        }

        private async void PickEstilista_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                
                string itemSelected = PickEstilista.Items[PickEstilista.SelectedIndex];
                nombreStilista = itemSelected;
              
                if (fecReservacion1 != "" && fecReservacion1 != null) 
                {  
                    await ObtenerDatoUsuario();
                    await horariosEstilista();
                }
            }
            catch (Exception)
            {

            }
        }

        private async Task horariosEstilista()
        {
            lstHorarios.ItemsSource = horiosvacio;
            llenarArrglo();

            int cot = 0;
            MoReservaciones horas = new MoReservaciones();
            VmReservaciones validar = new VmReservaciones();
            horas.fecha_Reserv = fecReservacion1;
            horas.nombreEstilista = nombreStilista;

            var rest = await validar.getReservacionesStilista(horas);
            foreach (var rst in rest)
            {
                cot = 0;
                while(cot < horario.Count)
                {
                    
                    if (rst.hora_Reserv == horario[cot]){
                        horario.Remove(rst.hora_Reserv);
                    }
                    cot += 1;

                }
            
            }

            lstHorarios.ItemsSource= horario;
            
        }

        private async Task llenarArrglo()
        {

            horario.Clear();
            horario.Add("13:00:00");
            horario.Add("13:30:00");
            horario.Add("14:00:00");
            horario.Add("14:30:00");
            horario.Add("15:00:00");
            horario.Add("15:30:00");
            horario.Add("16:00:00");
            horario.Add("16:30:00");
            horario.Add("17:00:00");
            horario.Add("17:30:00");
            horario.Add("18:00:00");
            horario.Add("18:30:00");

            /*ListaHoras hora1 = new ListaHoras();
            hora1.hora = "13:00:00";
            ListaHoras hora2 = new ListaHoras();
            hora2.hora = "13:00:00";
            ListaHoras hora3 = new ListaHoras();
            hora3.hora = "13:00:00";
            ListaHoras hora4 = new ListaHoras();
            hora4.hora = "13:00:00";
            ListaHoras hora5 = new ListaHoras();
            hora5.hora = "13:00:00";
            ListaHoras hora6 = new ListaHoras();
            hora6.hora = "13:00:00";
            ListaHoras hora7 = new ListaHoras();
            hora7.hora = "13:00:00";
            ListaHoras hora8 = new ListaHoras();
            hora8.hora = "13:00:00";
            ListaHoras hora9 = new ListaHoras();
            hora9.hora = "13:00:00";
            ListaHoras hora10 = new ListaHoras();
            hora10.hora = "13:00:00";
            ListaHoras hora11 = new ListaHoras();
            hora11.hora = "13:00:00";
            ListaHoras hora12 = new ListaHoras();
            hora12.hora = "13:00:00";

            horario.Add(hora1);
            horario.Add(hora2);
            horario.Add(hora3);
            horario.Add(hora4);
            horario.Add(hora5);
            horario.Add(hora6);
            horario.Add(hora7);
            horario.Add(hora8);
            horario.Add(hora9);
            horario.Add(hora10);
            horario.Add(hora11);
            horario.Add(hora12);*/
        }

        private void lstHorarios_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            horaReserv = lstHorarios.SelectedItem.ToString();
        }

        private async void fecReservacion_Focused(object sender, FocusEventArgs e)
        {
            if (fecReservacion1 == null)
            {
                if (nombreStilista != "-") {
                    fecReservacion1 = DateTime.Now.ToString("d/M/yyyy");
                    await horariosEstilista();
                    await ObtenerDatoUsuario();
                }
               
            }
        }

        public void mostrarTipoReser()
        {
            if (tipoReservacion == "PintadoYDecoracion")
            {
                lblTipoReser.Text = "Servicio: Retoque de Acrilico";
            }
            else if (tipoReservacion == "AplicaciondeAcrilico")
            {
                lblTipoReser.Text = "Servicio: Aplicación de Acrilico";
            }
            else if (tipoReservacion == "Manicure")
            {
                lblTipoReser.Text = "Servicio: Manicure";
            }
            else if (tipoReservacion == "Pedicure")
            {
                lblTipoReser.Text = "Servicio: Pedicure";
            }
        }

    }

}