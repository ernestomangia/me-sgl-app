using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ME.Libros.Dominio.General;
using Mono.CSharp.Linq;

namespace ME.Libros.Web.Models
{
    public class VentasPorCobrarViewModel
    {
        public VentasPorCobrarViewModel()
        {
            
        }

        public VentasPorCobrarViewModel(int year, int month,IEnumerable<VentaDominio> ventas)
        {
            Ventas = new List<VentaViewModel>(ventas.Select(v=> new VentaViewModel(v)));
            Month = month;
            Year = year;
        }

        public List<VentaViewModel> Ventas { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}