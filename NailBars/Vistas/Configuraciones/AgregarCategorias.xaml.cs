using NailBars.Modelo;
using NailBars.VistasModelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NailBars.Vistas.Configuraciones
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AgregarCategorias : ContentPage
    {
        public AgregarCategorias()
        {
            InitializeComponent();
        }

        private async void btnguardar_Clicked(object sender, EventArgs e)
        {
            await InsertarCategorias();
        }
        private async Task InsertarCategorias()
        {
            var funcion = new VMcategorias();
            var parametros = new Mcategorias();
            parametros.Categoria = txtcategoria.Text;
            parametros.Foto = "-";
            parametros.Prioridad = txtprioridad.Text;
            parametros.Color = txtcolor.Text;
            await funcion.InsertarCategorias(parametros);

            await DisplayAlert("Listo", "Categoria registrada", "OK");
        }

    }
}