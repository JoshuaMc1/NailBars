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
    public partial class Intro1 : ContentPage
    {
        public Intro1()
        {
            InitializeComponent();
            var statusbar = new VMPrincipal();
            statusbar.Transparente();
        }

        private void btnSaltar_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new Login());
        }
    }
}