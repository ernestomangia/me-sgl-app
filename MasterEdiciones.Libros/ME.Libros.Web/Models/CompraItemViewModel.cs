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
            Orden = compraItemDominio.Orden;
            Cantidad = compraItemDominio.Cantidad;
            PrecioCostoAnterior = compraItemDominio.PrecioCostoAnterior;
            PrecioCostoComprado = compraItemDominio.PrecioCostoComprado;
            MontoItemComprado = compraItemDominio.MontoComprado;
            Producto = new ProductoViewModel(compraItemDominio.Producto);
            ProductoId = compraItemDominio.Producto.Id;
            CodigoBarra = compraItemDominio.Producto.CodigoBarra;
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

        [Display(Name = "PrecioCostoAnterior", ResourceType = typeof(Messages))]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal PrecioCostoAnterior { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        [Display(Name = "PrecioCostoComprado", ResourceType = typeof(Messages))]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal PrecioCostoComprado { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        [Display(Name = "MontoItemComprado", ResourceType = typeof(Messages))]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal MontoItemComprado { get; set; }

        public List<CompraItemViewModel> Items { get; set; }

        [Display(Name = "Producto", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        public long ProductoId { get; set; }

        [Display(Name = "CodigoBarra", ResourceType = typeof(Messages))]
        [StringLength(13, MinimumLength = 13, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "ExactLenght")]
        [RegularExpression(@"^[0-9]+$", ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "OnlyNumbers")]
        public string CodigoBarra { get; set; }

        public ProductoViewModel Producto { get; set; }

        public CompraViewModel Compra { get; set; }

        public SelectList Productos { get; set; }

        #endregion
    }
}