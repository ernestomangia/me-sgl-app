using System.ComponentModel.DataAnnotations;

using ME.Libros.Dominio.General;

namespace ME.Libros.Web.Models
{
    public class GastoViewModel
    {
        #region Constructor(s)

        public GastoViewModel()
        {
        }

        public GastoViewModel(GastoDominio gasto)
        {
            Id = gasto.Id;
            Nombre = gasto.Nombre;
            Descripcion = gasto.Descripcion;
        }
        
        #endregion

        #region Properties

        [Display(Name = "Codigo", ResourceType = typeof(Messages))]
        public long Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        [StringLength(80, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "StringLength")]
        [RegularExpression(@"^([a-zA-ZñÑáéíóúÁÉÍÓÚ´0-9]+\s)*[a-zA-ZñÑáéíóúÁÉÍÓÚ´0-9]+$", ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "OnlyLettersAndNumbers")]
        public string Nombre { get; set; }

        [StringLength(250, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "StringLength")]
        public string Descripcion { get; set; }

        #endregion
    }
}