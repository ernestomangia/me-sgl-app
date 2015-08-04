using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

using ME.Libros.Utils.Enums;
using ME.Libros.Dominio.General;

namespace ME.Libros.Web.Models
{
    public class ClienteViewModel
    {
        #region Constructor(s)

        public ClienteViewModel()
        {
            Localidad = new LocalidadViewModel();
        }

        public ClienteViewModel(ClienteDominio cliente)
        {
            Id = cliente.Id;
            FechaAlta = cliente.FechaAlta;
            Nombre = cliente.Nombre;
            Apellido = cliente.Apellido;
            Cuil = cliente.Cuil;
            FechaNacimiento = cliente.FechaNacimiento.HasValue ? cliente.FechaNacimiento.Value : (DateTime?)null;
            Iva = new IvaViewModel(cliente.Iva);
            IvaId = cliente.Iva.Id;
            Direccion = cliente.Direccion;
            Comentario = cliente.Comentario;
            TelefonoFijo = cliente.TelefonoFijo;
            Celular = cliente.Celular;
            Email = cliente.Email;
            Localidad = new LocalidadViewModel(cliente.Localidad);
            LocalidadId = cliente.Localidad.Id;
            ProvinciaId = cliente.Localidad.Provincia.Id;
        }

        #endregion

        #region Properties

        [Display(Name = "Codigo", ResourceType = typeof(Messages))]
        public long Id { get; set; }
        
        [Display(Name = "FechaAlta", ResourceType = typeof(Messages))]
        public DateTime FechaAlta { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        [StringLength(80, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "StringLength")]
        [RegularExpression(@"^([a-zA-Z]+\s)*[a-zA-Z]+$", ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "OnlyLetters")]
        public string Nombre { get; set; }
        
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        [StringLength(80, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "StringLength")]
        [RegularExpression(@"^([a-zA-Z]+\s)*[a-zA-Z]+$", ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "OnlyLetters")]
        public string Apellido { get; set; }

        [Display(Name = "Cuil", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        [StringLength(11, MinimumLength = 11, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "ExactLenght")]
        [RegularExpression(@"^[0-9]+$", ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "OnlyNumbers")]
        public string Cuil { get; set; }

        [Display(Name = "FechaNacimiento", ResourceType = typeof(Messages))]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        //[DataType(DataType.Date, ErrorMessage = "Fecha incorrecta mvc5")]
        public DateTime? FechaNacimiento { get; set; }

        [Display(Name = "Iva", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerida")]
        public long IvaId { get; set; }

        
        [Display(Name = "Direccion", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerida")]
        [StringLength(100, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "StringLength")]
        public string Direccion { get; set; }
        
        [StringLength(250, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "StringLength")]
        public string Comentario { get; set; }
        
        [Display(Name = "TelefonoFijo", ResourceType = typeof(Messages))]
        [StringLength(11, MinimumLength = 11, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "ExactLenght")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{7})?$", ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "InvalidTel")]
        public string TelefonoFijo { get; set; }

        [StringLength(11, MinimumLength = 11, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "ExactLenght")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{7})?$", ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "InvalidTel")]
        public string Celular { get; set; }

        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "InvalidEmail")]
        public string Email { get; set; }
        
        [Display(Name = "Localidad", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerida")]
        public long LocalidadId { get; set; }
        
        [Display(Name = "Provincia", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerida")]
        public long ProvinciaId { get; set; }
        public LocalidadViewModel Localidad { get; set; }
        public IvaViewModel Iva { get; set; }
        public SelectList Provincias { get; set; }
        public SelectList Localidades { get; set; }
        public SelectList Ivas { get; set; }

        #endregion
    }
}