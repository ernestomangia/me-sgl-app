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
            Usuario = login.Usuario;
            Password = login.Contrasena;
        }
        
        #endregion

        #region Properties

        [StringLength(30, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "StringLength")]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        public string Usuario { get; set; }

        [Display(Name = "Contrasena", ResourceType = typeof(Messages))]
        [StringLength(30, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "StringLength")]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerida")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Recordarme?")]
        public bool Recordarme { get; set; }

        #endregion
    }
}