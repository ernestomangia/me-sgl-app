using System;

using ME.Libros.Utils.Enums;

namespace ME.Libros.Web.Models
{
    public class ReporteViewModel
    {
        public ReporteViewModel()
        {
        }

        public long VentaId { get; set; }
        public string Cliente { get; set; }
        public string Cobrador { get; set; }
        public string Vendedor { get; set; }
        public EstadoVenta? EstadoVenta { get; set; }
        public DateTime? Desde { get; set; }
        public DateTime? Hasta { get; set; }
    }
}