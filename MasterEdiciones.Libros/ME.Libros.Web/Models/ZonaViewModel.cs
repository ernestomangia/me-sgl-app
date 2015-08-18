using System.ComponentModel.DataAnnotations;
using ME.Libros.Dominio.General;

namespace ME.Libros.Web.Models
{
    public class ZonaViewModel
    {
        #region Constructor(s)

        public ZonaViewModel()
        {
        }

        public ZonaViewModel(ZonaDominio zona)
        {
            Id = zona.Id;
            Nombre = zona.Nombre;
            Descripcion = zona.Descripcion;
        }

        #endregion

        #region Properties

        [Display(Name = "Codigo", ResourceType = typeof(Messages))]
        public long Id { get; set; }

        [StringLength(80, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "StringLength")]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        public string Nombre { get; set; }

        [StringLength(250, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "StringLength")]
        public string Descripcion { get; set; }

        #endregion
    }
}