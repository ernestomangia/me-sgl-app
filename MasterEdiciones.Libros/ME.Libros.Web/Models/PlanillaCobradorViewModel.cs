using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ME.Libros.Dominio.General;

namespace ME.Libros.Web.Models
{
    public class PlanillaCobradorViewModel
    {
        public PlanillaCobradorViewModel()
        {
            
        }

        public PlanillaCobradorViewModel(CobradorDominio cobrador, LocalidadDominio localidad,
            IEnumerable<VentaDominio> ventas)
        {
            Cobrador = new CobradorViewModel(cobrador);
            Localidad=new LocalidadViewModel(localidad);
            Ventas = new List<VentaViewModel>(ventas.Select(v => new VentaViewModel(v)));

        }

        public CobradorViewModel Cobrador { get; set; }
        public LocalidadViewModel Localidad { get; set; }
        public List<VentaViewModel> Ventas { get; set; }


    }


}