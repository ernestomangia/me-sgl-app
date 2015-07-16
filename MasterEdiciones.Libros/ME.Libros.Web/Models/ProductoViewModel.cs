using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

using ME.Libros.Dominio.General;

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
            EditorialId = producto.Editorial.Id;
            Rubro = new RubroViewModel(producto.Rubro);
            RubroId = producto.Rubro.Id;
        }

        #endregion

        #region Properties

        [Display(Name = "Codigo", ResourceType = typeof(Messages))]
        public long Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        [StringLength(80, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "StringLength")]
        //[RegularExpression(@"^([a-zA-Z]+\s)*[a-zA-Z]+$", ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "OnlyLetters")]
        public string Nombre { get; set; }

        [Display(Name = "Descripcion", ResourceType = typeof(Messages))]
        [StringLength(250, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "StringLength")]
        public string Descripcion { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        //[RegularExpression(@"^[0-9]+$", ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "OnlyNumbers")]
        public long Stock { get; set; }

        [Display(Name = "CodigoBarra", ResourceType = typeof(Messages))]
        [StringLength(30, MinimumLength = 30, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "ExactLenght")]
        [RegularExpression(@"^[0-9]+$", ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "OnlyNumbers")]
        public string CodigoBarra { get; set; }

        // [RegularExpression(@"^(\d{1}\.)?(\d+\.?)+(,\d{2})?$", ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "OnlyNumbers")]
        [Display(Name = "PrecioVenta", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal PrecioVenta { get; set; }

        //  [RegularExpression(@"^(\d{1}\.)?(\d+\.?)+(,\d{2})?$", ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "OnlyNumbers")]
        [Display(Name = "PrecioCosto", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal PrecioCosto { get; set; }

        [Display(Name = "Editorial", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerida")]
        public long EditorialId { get; set; }

        [Display(Name = "Rubro", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        public long RubroId { get; set; }

        public EditorialViewModel Editorial { get; set; }
        public RubroViewModel Rubro { get; set; }
        public SelectList Editoriales { get; set; }
        public SelectList Rubros { get; set; }

        #endregion
    }
}