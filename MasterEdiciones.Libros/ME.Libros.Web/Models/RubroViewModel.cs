using System.ComponentModel.DataAnnotations;
using ME.Libros.Dominio.General;

namespace ME.Libros.Web.Models
{
    public class RubroViewModel
    {
        #region Constructor(s)

        public RubroViewModel()
        {
        }

        public RubroViewModel(RubroDominio rubro)
        {
            Id = rubro.Id;
            Nombre = rubro.Nombre;
            Descripcion = rubro.Descripcion;
        }
        
        #endregion

        #region Properties

        [Display(Name = "Codigo", ResourceType = typeof(Messages))]
        public long Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        [StringLength(80, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "StringLength")]
        [RegularExpression(@"^([a-zA-ZñÑáéíóúÁÉÍÓÚ´]+\s)*[a-zA-ZñÑáéíóúÁÉÍÓÚ´]+$", ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "OnlyLetters")]
        public string Nombre { get; set; }

        [StringLength(250, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "StringLength")]
        public string Descripcion { get; set; }

        #endregion
    }
}