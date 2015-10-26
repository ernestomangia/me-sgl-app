using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ME.Libros.Dominio.General;

namespace ME.Libros.Web.Models
{
    public class VentasAtrasadasViewModel
    {
        public VentasAtrasadasViewModel()
        {
        }

        public VentasAtrasadasViewModel(int year, int month,IEnumerable<VentaDominio> ventas)
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