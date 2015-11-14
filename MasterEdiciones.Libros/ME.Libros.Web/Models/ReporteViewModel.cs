using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using ME.Libros.Utils.Enums;

namespace ME.Libros.Web.Models
{
    public class ReporteViewModel
    {
        public ReporteViewModel()
        {
        }

        public long VentaId { get; set; }
        public long CantidadVigentes { get; set; }
        public long CantidadPagadas { get; set; }
        public long CantidadAnuladas { get; set; }
        public string Cliente { get; set; }
        public string Cobrador { get; set; }
        public string Vendedor { get; set; }
        public EstadoVenta? EstadoVenta { get; set; }
        public DateTime? Desde { get; set; }
        public DateTime? Hasta { get; set; }
        [Display(Name = "Localidad", ResourceType = typeof(Messages))]
        public long LocalidadId { get; set; }
        public SelectList Localidades { get; set; }
    }
}