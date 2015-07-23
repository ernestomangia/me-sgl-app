using System;
using System.Collections.Generic;

using ME.Libros.Utils.Enums;

namespace ME.Libros.Web.Models
{
    public class VentaTodasViewModel
    {
        public VentaTodasViewModel()
        {
            VentaViewModels = new List<VentaViewModel>();
        }

        public string Cliente { get; set; }
        public string Cobrador { get; set; }
        public string Vendedor { get; set; }
        public EstadoVenta EstadoVenta { get; set; }
        public DateTime? Desde { get; set; }
        public DateTime? Hasta { get; set; }
        public List<VentaViewModel> VentaViewModels { get; set; }
    }
}