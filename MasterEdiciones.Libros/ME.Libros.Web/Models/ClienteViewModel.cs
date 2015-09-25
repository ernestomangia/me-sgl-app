using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using ME.Libros.Dominio.General;
using ME.Libros.Web.Validators;

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
            Codigo = cliente.Codigo;
            Nombre = cliente.Nombre;
            Apellido = cliente.Apellido;
            Dni = cliente.Dni;
            Cuil = cliente.Cuil;
            FechaNacimiento = cliente.FechaNacimiento.HasValue ? cliente.FechaNacimiento.Value : (DateTime?)null;
            Iva = new IvaViewModel(cliente.Iva);
            IvaId = cliente.Iva.Id;
            Direccion = cliente.Direccion;
            Comentario = cliente.Comentario;
            TelefonoFijo = cliente.TelefonoFijo;
            Celular = cliente.Celular;
            Celular2 = cliente.Celular2;
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
        [Range(1, 1000000000, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "RangeValue")]
        public long Codigo { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        [StringLength(80, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "StringLength")]
        [RegularExpression(@"^([a-zA-ZñÑáéíóúÁÉÍÓÚ´]+\s)*[a-zA-ZñÑáéíóúÁÉÍÓÚ´]+$", ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "OnlyLetters")]
        public string Nombre { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        [StringLength(80, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "StringLength")]
        [RegularExpression(@"^([a-zA-ZñÑáéíóúÁÉÍÓÚ´]+\s)*[a-zA-ZñÑáéíóúÁÉÍÓÚ´]+$", ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "OnlyLetters")]
        public string Apellido { get; set; }

        [Display(Name = "Cuil", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        [StringLength(13, MinimumLength = 13, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "ExactLenght")]
        [RegularExpression(@"\d{2}[-. ]\d{8}[-. ]\d{1}", ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "FormatoInvalido")]
        [Cuil]
        public string Cuil { get; set; }

        [Display(Name = "Dni", ResourceType = typeof(Messages))]
        [Range(1000000, 100000000, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "RangeValue")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public long? Dni { get; set; }

        [Display(Name = "FechaNacimiento", ResourceType = typeof(Messages))]
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
        [RegularExpression(@"\d{2,4}[-. ]\d{6,8}", ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "FormatoInvalido")]
        public string TelefonoFijo { get; set; }

        [StringLength(11, MinimumLength = 11, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "ExactLenght")]
        [RegularExpression(@"\d{2,4}[-. ]\d{6,8}", ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "FormatoInvalido")]
        public string Celular { get; set; }

        [Display(Name = "Celular2", ResourceType = typeof(Messages))]
        [StringLength(11, MinimumLength = 11, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "ExactLenght")]
        [RegularExpression(@"\d{2,4}[-. ]\d{6,8}", ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "FormatoInvalido")]
        public string Celular2 { get; set; }

        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "FormatoInvalido")]
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