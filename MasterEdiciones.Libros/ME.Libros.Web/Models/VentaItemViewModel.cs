using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ME.Libros.Dominio.General;

namespace ME.Libros.Web.Models
{
    using System.ComponentModel.DataAnnotations;

    public class VentaItemViewModel
    {
        #region Constructor(s)

        public VentaItemViewModel()
        {
        }

        public VentaItemViewModel(VentaItemDominio ventaItemDominio)
        {
            Id = ventaItemDominio.Id;
            FechaAlta = ventaItemDominio.FechaAlta;
            Cantidad = ventaItemDominio.Cantidad;
            Monto = ventaItemDominio.Monto;
            Producto = new ProductoViewModel(ventaItemDominio.Producto);
        }

        #endregion

        #region Properties

        [Display(Name = "Codigo", ResourceType = typeof(Messages))]
        public long Id { get; set; }

        [Display(Name = "FechaAlta", ResourceType = typeof(Messages))]
        public DateTime FechaAlta { get; set; }

        public int Cantidad { get; set; }

        public decimal Monto { get; set; }

        public ProductoViewModel Producto { get; set; }

        #endregion
    }
}