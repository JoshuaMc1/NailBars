using System;

namespace NailBars.Servicios
{
    public interface INotification
    {
        void CreateNotification(String title, String message);
    }
}
