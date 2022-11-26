﻿using Xamarin.Forms;
using NailBars.Vistas;
using NailBars.Modelo;
using NailBars.Vistas.TutorialIntro;

namespace NailBars
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new Intro1());
           
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
