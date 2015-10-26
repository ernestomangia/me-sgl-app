using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

using ME.Libros.Dominio.General;

namespace ME.Libros.Web.Models
{
    public class CobradorViewModel
    {
        #region Constructor(s)

        public CobradorViewModel()
        {
            LocalidadesAsignadas = new List<LocalidadViewModel>();
            LocalidadesNoAsignadas = new List<LocalidadViewModel>();
        }

        public CobradorViewModel(CobradorDominio cobrador)
        {
            Id = cobrador.Id;
            FechaAlta = cobrador.FechaAlta;
            Nombre = cobrador.Nombre;
            Apellido = cobrador.Apellido;
            Dni = cobrador.Dni;
            Localidad = new LocalidadViewModel(cobrador.Localidad);
            PorcentajeComision = cobrador.PorcentajeComision;
            Direccion = cobrador.Direccion;
            TelefonoFijo = cobrador.TelefonoFijo;
            Celular = cobrador.Celular;
            Email = cobrador.Email;
            LocalidadesAsignadas = new List<LocalidadViewModel>(cobrador.Localidades.Select(la => new LocalidadViewModel(la)));
            LocalidadId = cobrador.Localidad.Id;
            ProvinciaId = cobrador.Localidad.Provincia.Id;
        }
        #endregion

        #region Properties

        [Display(Name = "Codigo", ResourceType = typeof(Messages))]
        public long Id { get; set; }

        [Display(Name = "FechaAlta", ResourceType = typeof(Messages))]
        public DateTime FechaAlta { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        [StringLength(100, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "StringLength")]
        [RegularExpression(@"^([a-zA-ZñÑáéíóúÁÉÍÓÚ´]+\s)*[a-zA-ZñÑáéíóúÁÉÍÓÚ´]+$", ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "OnlyLetters")]
        public string Nombre { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        [StringLength(100, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "StringLength")]
        [RegularExpression(@"^([a-zA-ZñÑáéíóúÁÉÍÓÚ´]+\s)*[a-zA-ZñÑáéíóúÁÉÍÓÚ´]+$", ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "OnlyLetters")]
        public string Apellido { get; set; }

        [Display(Name = "Direccion", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerida")]
        [StringLength(200, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "StringLength")]
        public string Direccion { get; set; }

        [Display(Name = "Dni", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        [Range(1000000, 100000000, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "RangeValue")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public long Dni { get; set; }

        [Display(Name = "TelefonoFijo", ResourceType = typeof(Messages))]
        [StringLength(11, MinimumLength = 11, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "ExactLenght")]
        [RegularExpression(@"\d{2,4}[-. ]\d{6,8}", ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "FormatoInvalido")]
        public string TelefonoFijo { get; set; }

        [StringLength(11, MinimumLength = 11, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "ExactLenght")]
        [RegularExpression(@"\d{2,4}[-. ]\d{6,8}", ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "FormatoInvalido")]
        public string Celular { get; set; }

        [StringLength(100, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "StringLength")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "FormatoInvalido")]
        public string Email { get; set; }

        [Display(Name = "Localidad", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerida")]
        public long LocalidadId { get; set; }

        [Display(Name = "Provincia", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerida")]
        public long ProvinciaId { get; set; }

        [Display(Name = "PorcentajeComision", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerida")]
        [Range(0, 100, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "RangeValue")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal PorcentajeComision { get; set; }

        public LocalidadViewModel Localidad { get; set; }

        [Display(Name = "LocalidadesAsignadas", ResourceType = typeof(Messages))]
        public List<LocalidadViewModel> LocalidadesAsignadas { get; set; }

        public List<LocalidadViewModel> LocalidadesNoAsignadas { get; set; }

        public SelectList Provincias { get; set; }

        public SelectList Localidades { get; set; }

        #endregion
    }
}