using System;
using System.Collections.Generic;
using System.Text;

namespace NailBars.Servicios
{
    public interface INotification
    {
        void CreateNotification(String title, String message);
    }
}
