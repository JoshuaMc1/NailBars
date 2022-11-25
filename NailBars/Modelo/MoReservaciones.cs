using System;
using System.Collections.Generic;
using System.Text;

namespace NailBars.Modelo
{
    public class MoReservaciones
    {
        public string id_Reserv { get; set; }
        public string id_Cliente { get; set; }
        public string nombre_usuario { get; set; }
        public string nombreEstilista { get; set; }
        public string fecha_Reserv { get; set; }
        public string hora_Reserv { get; set; }
        public string tipo_Reserv { get; set; }
        public string precio { get; set; }
        public string status { get; set; }
        public int calificacion { get; set; }
    }
}
