using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NailBars.Vistas;
using NailBars.Vistas.Configuraciones;
using System.Collections.Generic;
using Plugin.FirebasePushNotification;

namespace NailBars
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new Presentacion();


        }




        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }





    }
}
