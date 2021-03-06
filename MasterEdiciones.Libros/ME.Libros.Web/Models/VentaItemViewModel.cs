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
        }

        public VentaItemViewModel(VentaItemDominio ventaItemDominio)
        {
            Id = ventaItemDominio.Id;
            FechaAlta = ventaItemDominio.FechaAlta;
            Orden = ventaItemDominio.Orden;
            Cantidad = ventaItemDominio.Cantidad;
            PrecioVentaVendido = ventaItemDominio.PrecioVentaVendido;
            PrecioVentaCalculado = ventaItemDominio.PrecioVentaCalculado;
            MontoItemVendido = ventaItemDominio.MontoVendido;
            MontoItemCalculado = ventaItemDominio.MontoCalculado;
            PrecioCosto = ventaItemDominio.PrecioCosto;
            Producto = new ProductoViewModel(ventaItemDominio.Producto);
            ProductoId = ventaItemDominio.Producto.Id;
            CodigoBarra = ventaItemDominio.Producto.CodigoBarra;
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

        [Display(Name = "PrecioCosto", ResourceType = typeof(Messages))]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal PrecioCosto { get; set; }

        [Display(Name = "PrecioVentaCalculado", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal PrecioVentaCalculado { get; set; }

        [Display(Name = "PrecioVentaVendido", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal PrecioVentaVendido { get; set; }

        [Display(Name = "MontoItemCalculado", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal MontoItemCalculado { get; set; }

        [Display(Name = "MontoItemVendido", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal MontoItemVendido { get; set; }

        [Display(Name = "Producto", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        public long ProductoId { get; set; }

        [Display(Name = "CodigoBarra", ResourceType = typeof(Messages))]
        [StringLength(13, MinimumLength = 13, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "ExactLenght")]
        [RegularExpression(@"^[0-9]+$", ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "OnlyNumbers")]
        public string CodigoBarra { get; set; }

        public ProductoViewModel Producto { get; set; }

        public VentaViewModel Venta { get; set; }

        public SelectList Productos { get; set; }

        #endregion
    }
}