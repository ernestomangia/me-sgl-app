using System.ComponentModel.DataAnnotations;

using ME.Libros.Dominio.General;

namespace ME.Libros.Web.Models
{
    public class UsuarioViewModel
    {
        #region Constructor(s)

        public UsuarioViewModel()
        {
        }

        public UsuarioViewModel(UsuarioDominio usuario)
        {
            Id = usuario.Id;
            Nombre = usuario.Nombre;
            Contrasena = usuario.Contrasena;
        }
        
        #endregion

        #region Properties

        [Display(Name = "Codigo", ResourceType = typeof(Messages))]
        public long Id { get; set; }

        [StringLength(80, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "StringLength")]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        public string Nombre { get; set; }

        [StringLength(250, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "StringLength")]
        [Display(Name = "Contrasena", ResourceType = typeof(Messages))]
        public string Contrasena { get; set; }

        #endregion
    }
}