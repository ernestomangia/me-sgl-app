using System;
using ME.Libros.Utils.Enums;

namespace ME.Libros.DTO
{
    public class VentaSearchDto
    {
        public string Cliente { get; set; }
        public string Cobrador { get; set; }
        public string Vendedor { get; set; }
        public EstadoVenta? EstadoVenta { get; set; }
        public DateTime? Desde { get; set; }
        public DateTime? Hasta { get; set; }
    }
}