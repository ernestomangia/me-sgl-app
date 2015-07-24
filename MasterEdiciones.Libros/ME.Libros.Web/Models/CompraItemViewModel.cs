using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ME.Libros.Dominio.General;

namespace ME.Libros.Web.Models
{
    public class CompraItemViewModel
    {
         #region Constructor(s)

        public CompraItemViewModel()
        {
            Producto = new ProductoViewModel();
        }

        public CompraItemViewModel(CompraItemDominio compraItemDominio)
        {
            Id = compraItemDominio.Id;
            FechaAlta = compraItemDominio.FechaAlta;
            Cantidad = compraItemDominio.Cantidad;
            MontoItemComprado = compraItemDominio.MontoComprado;
            PrecioCosto = compraItemDominio.PrecioCosto;
            MontoItemCalculado = compraItemDominio.MontoCalculado;
            PrecioCompraCalculado = compraItemDominio.PrecioCompraCalculado;
            PrecioCompraComprado = compraItemDominio.PrecioCompraComprado;
            Producto = new ProductoViewModel(compraItemDominio.Producto);
            ProductoId = compraItemDominio.Producto.Id;
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

        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        [Display(Name = "PrecioCosto", ResourceType = typeof(Messages))]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal PrecioCosto { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        [Display(Name = "MontoItemComprado", ResourceType = typeof(Messages))]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal MontoItemComprado { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        [Display(Name = "MontoItemCalculado", ResourceType = typeof(Messages))]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal MontoItemCalculado { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        [Display(Name = "PrecioCompraCalculado", ResourceType = typeof(Messages))]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal PrecioCompraCalculado { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        [Display(Name = "PrecioCompraComprado", ResourceType = typeof(Messages))]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal PrecioCompraComprado { get; set; }

        public List<CompraItemViewModel> Items { get; set; }

        [Display(Name = "Producto", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        public long ProductoId { get; set; }

        public ProductoViewModel Producto { get; set; }

        public CompraViewModel Compra { get; set; }

        public SelectList Productos { get; set; }

        #endregion
    }
}