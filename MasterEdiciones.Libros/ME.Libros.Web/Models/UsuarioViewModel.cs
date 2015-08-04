using System;
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
            Apellido = usuario.Apellido;
            UserName = usuario.UserName;
            Password = usuario.Password;
            Email = usuario.Email;
            EmailConfirmado = usuario.EmailConfirmado;
            Habilitado = usuario.Habilitado;
            UltimoLogin = usuario.UltimoLogin;
            CantidadIntentosFallidos = usuario.CantidadIntentosFallidos;
        }

        #endregion

        #region Properties

        [Display(Name = "Codigo", ResourceType = typeof(Messages))]
        public long Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        [StringLength(80, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "StringLength")]
        public string Nombre { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        [StringLength(80, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "StringLength")]
        public string Apellido { get; set; }

        [Display(Name = "UserName", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        [StringLength(50, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "StringLength")]
        public string UserName { get; set; }

        [Display(Name = "Password", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerida")]
        [MinLength(8, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "StringMinLength")]
        [StringLength(50, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "StringLength")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "ConfirmarPassword", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "RepetirPasswordRequerido")]
        [MinLength(8, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "StringMinLength")]
        [StringLength(50, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "StringLength")]
        [Compare("Password", ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "PasswordEquals")]
        [DataType(DataType.Password)]
        public string ConfirmarPassword { get; set; }

        [StringLength(80, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "StringLength")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "InvalidEmail")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "EmailConfirmado", ResourceType = typeof(Messages))]
        public bool EmailConfirmado { get; set; }

        public bool Habilitado { get; set; }

        public DateTime? UltimoLogin { get; set; }

        public long CantidadIntentosFallidos { get; set; }

        #endregion
    }
}