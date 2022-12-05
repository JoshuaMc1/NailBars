using System;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Services;
using NailBars.VistasModelo;
using NailBars.Modelo;
using Newtonsoft.Json;
using Firebase.Auth;
using NailBars.Servicios;
using Xamarin.Essentials;
using NailBars.Components;
using Rg.Plugins.Popup.Extensions;

namespace NailBars.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Calificar : PopupPage
    {
        public static string idusuario;
        public static string idreservacion;
        public static string reseña = "";
        public static string calificacion;
        public static string idcalificacion;
        MoReservaciones DatReservacion = new MoReservaciones();

        public Calificar()
        {
            InitializeComponent();
        }

        public Calificar(MoReservaciones itemSelect)
        {
            InitializeComponent();
            ObtenerIdusuario();
            DatReservacion = itemSelect;
            datoscalificar();
        }

        private async Task datoscalificar()
        {
            var funcion = new VMresenias();
            var parametros = new Mresenias();
            parametros.idreservacion = DatReservacion.id_Reserv;
            var dt = await funcion.obtenerdatos(parametros);

            foreach (var data in dt)
            {
                reseña = data.reseña;
                calificacion = data.calificacion;
                idcalificacion = data.idcalificacion;
            }
            if (reseña != "")
            {
                txtreseña.Text = reseña;
                EstCalificacion.Rating = Convert.ToInt32(calificacion);
                lblTitulo.Text = "Edita tu reseña cuando desees";
                btnguardar.Text = "Actualizar";
            }
        }

        private async Task ObtenerIdusuario()
        {
            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(Conexionfirebase.WebapyFirebase));
                var guardarId = JsonConvert.DeserializeObject<FirebaseAuth>(Preferences.Get("MyFirebaseRefreshToken", ""));
                idusuario = guardarId.User.LocalId;
            }
            catch (Exception)
            {
                await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Aviso", "Su sesión a expirado.", JMDialog.Danger), true);
            }
        }

        private async Task EditarReseña()
        {
            var funcion = new VMresenias();
            var parametros = new Mresenias();
            parametros.reseña = txtreseña.Text;
            parametros.calificacion = EstCalificacion.Rating.ToString();
            parametros.idcalificacion = idcalificacion;
            await funcion.EditarReseña(parametros);

            var funcio2 = new Mresenias();
            var consulta = new VmReservaciones();
            funcio2.calificacion = EstCalificacion.Rating.ToString();
            funcio2.idreservacion = DatReservacion.id_Reserv;
            await consulta.EditarReseña(funcio2);

            await PopupNavigation.Instance.PopAsync();
        }

        private async Task InsertarReseñas()
        {
            var funcion = new VMresenias();
            var parametros = new Mresenias();
            parametros.calificacion = EstCalificacion.Rating.ToString();
            parametros.reseña = txtreseña.Text;
            parametros.idusuario = idusuario;
            parametros.idreservacion = DatReservacion.id_Reserv;
            await funcion.InsertarReseña(parametros);

            var funcio2 = new Mresenias();
            var consulta = new VmReservaciones();
            funcio2.calificacion = EstCalificacion.Rating.ToString();
            funcio2.idreservacion = DatReservacion.id_Reserv;
            await consulta.EditarReseña(funcio2);

            await PopupNavigation.Instance.PopAsync();
        }

        private async void btnguardar_Clicked(object sender, EventArgs e)
        {
            if (reseña != "")
            {
                await EditarReseña();
            }
            else
            {
                if (!string.IsNullOrEmpty(txtreseña.Text)) await InsertarReseñas();
                else await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Advertencia", "Debe ingresar una reseña.", JMDialog.Warning), true);
                await App.Current.MainPage.Navigation.PushPopupAsync(new JMDialog("Satisfactorio", "Gracias por su calificación.", JMDialog.Success), true);
            }
        }
    }
}