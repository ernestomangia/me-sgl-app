﻿using System;
using System.Web.Mvc;

using ME.Libros.Dominio.General;

namespace ME.Libros.Web.Models
{
    using System.ComponentModel.DataAnnotations;

    public class VentaItemViewModel
    {
        #region Constructor(s)

        public VentaItemViewModel()
        {
            Producto = new ProductoViewModel();
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

        [Display(Name = "Nro", ResourceType = typeof(Messages))]
        public int Orden { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerida")]
        public int Cantidad { get; set; }

        [Display(Name = "MontoItem", ResourceType = typeof(Messages))]
        public decimal Monto { get; set; }

        [Display(Name = "Producto", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        public long ProductoId { get; set; }

        public ProductoViewModel Producto { get; set; }

        public SelectList Productos { get; set; }

        #endregion
    }
}