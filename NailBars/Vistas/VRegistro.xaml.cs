using NailBars.VistasModelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NailBars.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VRegistro : ContentPage
    {
        public VRegistro()
        {
            InitializeComponent();
            var statusbar = new VMPrincipal();
            statusbar.Translucido();
        }
    }
}