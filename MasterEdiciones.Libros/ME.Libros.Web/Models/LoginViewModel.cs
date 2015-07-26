using System.ComponentModel.DataAnnotations;

using ME.Libros.Dominio.General;

namespace ME.Libros.Web.Models
{
    public class LoginViewModel
    {
        #region Constructor(s)

        public LoginViewModel()
        {
        }

        public LoginViewModel(LoginDominio login)
        {
            Id = login.Id;
            Usuario = login.Usuario;
            Contrasena = login.Contrasena;
        }
        
        #endregion

        #region Properties

        [Display(Name = "Codigo", ResourceType = typeof(Messages))]
        public long Id { get; set; }

        [StringLength(30, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "StringLength")]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        public string Usuario { get; set; }

        [StringLength(30, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "StringLength")]
        [Display(Name = "Contrasena", ResourceType = typeof(Messages))]
        public string Contrasena { get; set; }

        #endregion
    }
}