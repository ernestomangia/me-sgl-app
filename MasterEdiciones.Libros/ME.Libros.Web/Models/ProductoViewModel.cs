using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using ME.Libros.Dominio.General;
using Microsoft.SqlServer.Server;

namespace ME.Libros.Web.Models
{

    public class ProductoViewModel
    {
        #region Constructor(s)

        
        public ProductoViewModel()
        {
            Editorial = new EditorialViewModel();
            Rubro = new RubroViewModel();
        }

        public ProductoViewModel(ProductoDominio producto)
        {
            Id = producto.Id;
            Nombre = producto.Nombre;
            Descripcion = producto.Descripcion;
            Stock = producto.Stock;
            CodigoBarra = producto.CodigoBarra;
            PrecioVenta = producto.PrecioVenta;
            PrecioCosto = producto.PrecioCosto;



            Editorial = new EditorialViewModel(producto.Editorial);
            Rubro = new RubroViewModel(producto.Rubro);
        }

        #endregion


        #region Properties

        [Display(Name = "Codigo", ResourceType = typeof(Messages))]
        public long Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        [StringLength(80, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "StringLength")]
        [RegularExpression(@"^([a-zA-Z]+\s)*[a-zA-Z]+$", ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "OnlyLetters")]
        public string Nombre { get; set; }

        [Display(Name = "Descripcion", ResourceType = typeof(Messages))]
        [StringLength(250, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "StringLength")]
        public string Descripcion { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        [RegularExpression(@"^[0-9]+$", ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "OnlyNumbers")]
        public long Stock { get; set; }

        //[Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        [Display(Name = "CodigoBarra", ResourceType = typeof(Messages))]
        [StringLength(30, MinimumLength = 30, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "ExactLenght")]
        [RegularExpression(@"^[0-9]+$", ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "OnlyNumbers")]
        public string CodigoBarra { get; set; }


        // [RegularExpression(@"^(\d{1}\.)?(\d+\.?)+(,\d{2})?$", ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "OnlyNumbers")]
        [Display(Name = "PrecioVenta", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        public decimal PrecioVenta { get; set; }


        //  [RegularExpression(@"^(\d{1}\.)?(\d+\.?)+(,\d{2})?$", ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "OnlyNumbers")]
        [Display(Name = "PrecioCosto", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        public decimal PrecioCosto { get; set; }

        public EditorialViewModel Editorial { get; set; }
        public RubroViewModel Rubro { get; set; }

        public SelectList Editoriales { get; set; }
        public SelectList Rubros { get; set; }

        #endregion


    }
}