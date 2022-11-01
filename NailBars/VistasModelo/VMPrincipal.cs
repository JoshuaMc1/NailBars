using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace NailBars.VistasModelo
{
    public class VMPrincipal
    {
        public VMPrincipal() { }

        public void Ocultar()
        {
            DependencyService.Get<VMStatusBar>().OcultarStatusBar();
        }

        public void Mostrar()
        {
            DependencyService.Get<VMStatusBar>().MostrarStatusBar();
        }

        public void Transparente()
        {
            DependencyService.Get<VMStatusBar>().Transparente();
        }

        public void Translucido()
        {
            DependencyService.Get<VMStatusBar>().Translucido();
        }

        public void CambiarColor()
        {
            DependencyService.Get<VMStatusBar>().CambiarColor();
        }

        public ICommand CambiarColorCommand => new Command(CambiarColor);
    }
}
