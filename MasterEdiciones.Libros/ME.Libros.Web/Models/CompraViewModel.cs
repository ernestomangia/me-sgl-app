﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ME.Libros.Dominio.General;
using ME.Libros.Utils.Enums;

namespace ME.Libros.Web.Models
{
    public class CompraViewModel
    {
        #region Constructor(s)

        public CompraViewModel()
        {
            Proveedor = new ProveedorViewModel();
            //Items = new List<CompraItemViewModel>();
        }

        public CompraViewModel(CompraDominio compraDominio)
        {
            Id = compraDominio.Id;
            FechaAlta = compraDominio.FechaAlta;
            FechaCompra = compraDominio.FechaCompra;
            Estado = compraDominio.Estado;
            MontoComprado = compraDominio.MontoComprado;
            MontoCalculado = compraDominio.MontoCalculado;
            Proveedor = new ProveedorViewModel(compraDominio.Proveedor);
            ProveedorId = compraDominio.Proveedor.Id;

            //if (ventaDominio.VentaItems  != null)
            //{
            //    Items = new List<VentaItemViewModel>(ventaDominio.VentaItems.Select(vi => new VentaItemViewModel(vi)));
            //}
        }

        #endregion

        #region Properties

        [Display(Name = "NumeroCompra", ResourceType = typeof(Messages))]
        public long Id { get; set; }

        [Display(Name = "FechaAlta", ResourceType = typeof(Messages))]
        public DateTime FechaAlta { get; set; }

        [Display(Name = "FechaCompra", ResourceType = typeof(Messages))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FechaCompra { get; set; }

        [Display(Name = "NroRemito", ResourceType = typeof(Messages))]
        public string NroRemito { get; set; }

        [Display(Name = "NroFactura", ResourceType = typeof(Messages))]
        public string NroFactura { get; set; }

        [Display(Name = "MontoCalculado", ResourceType = typeof(Messages))]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal MontoCalculado { get; set; }

        [Display(Name = "MontoComprado", ResourceType = typeof(Messages))]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal MontoComprado { get; set; }

        public EstadoCompra Estado { get; set; }
        
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        public long ProveedorId { get; set; }

        public ProveedorViewModel Proveedor { get; set; }

        public List<VentaItemViewModel> Items { get; set; }

        public SelectList Proveedores { get; set; }

        #endregion
    
    }
}