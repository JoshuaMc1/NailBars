using NailBars.VistasModelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NailBars.Vistas.TutorialIntro
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Vista3 : ContentView
    {
        public Vista3()
        {
            InitializeComponent();
            var statusbar = new VMPrincipal();
            statusbar.Transparente();
        }
    }
}