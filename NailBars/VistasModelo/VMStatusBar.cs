using System;
using System.Collections.Generic;
using System.Text;

namespace NailBars.VistasModelo
{
    public interface VMStatusBar
    {
        void OcultarStatusBar();
        void MostrarStatusBar();
        void Translucido();
        void Transparente();
        void CambiarColor();
    }
}
