using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace NailBars.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Contenedor : Xamarin.Forms.TabbedPage
    {
        public Contenedor()
        {
            InitializeComponent();
            //aqui es para especificar donde quiere que aparesca el menu arriba, abajo etc.
          On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);

          

            

        }
    }
}