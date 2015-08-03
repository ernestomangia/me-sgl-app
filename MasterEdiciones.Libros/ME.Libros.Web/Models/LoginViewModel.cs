using System.ComponentModel.DataAnnotations;

namespace ME.Libros.Web.Models
{
    public class LoginViewModel
    {
        #region Constructor(s)

        public LoginViewModel()
        {
        }
        
        #endregion

        #region Properties

        [StringLength(30, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "StringLength")]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        public string Usuario { get; set; }

        [Display(Name = "Password", ResourceType = typeof(Messages))]
        [StringLength(30, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "StringLength")]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerida")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Recordarme", ResourceType = typeof(Messages))]
        public bool Recordarme { get; set; }

        #endregion
    }
}