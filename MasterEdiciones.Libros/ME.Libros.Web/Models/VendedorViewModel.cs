using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

using ME.Libros.Dominio.General;

namespace ME.Libros.Web.Models
{
    public class VendedorViewModel
    {
        #region Constructor(s)

        public VendedorViewModel()
        {
            Localidad = new LocalidadViewModel();
            LocalidadesAsignadas = new List<LocalidadViewModel>();
            LocalidadesNoAsignadas = new List<LocalidadViewModel>();
        }

        public VendedorViewModel(VendedorDominio vendedorDominio)
        {
            Id = vendedorDominio.Id;
            FechaAlta = vendedorDominio.FechaAlta;
            Nombre = vendedorDominio.Nombre;
            Apellido = vendedorDominio.Apellido;
            Dni = vendedorDominio.Dni;
            Localidad = new LocalidadViewModel(vendedorDominio.Localidad);
            PorcentajeComision = vendedorDominio.PorcentajeComision;
            Direccion = vendedorDominio.Direccion;
            TelefonoFijo = vendedorDominio.TelefonoFijo;
            Celular = vendedorDominio.Celular;
            Email = vendedorDominio.Email;
            LocalidadesAsignadas = new List<LocalidadViewModel>(vendedorDominio.Localidades.Select(la => new LocalidadViewModel(la)));
            LocalidadId = vendedorDominio.Localidad.Id;
            ProvinciaId = vendedorDominio.Localidad.Provincia.Id;
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

        [Display(Name = "Direccion", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerida")]
        [StringLength(100, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "StringLength")]
        public string Direccion { get; set; }

        [Display(Name = "Dni", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        [StringLength(8, MinimumLength = 8, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "ExactLenght")]
        [RegularExpression(@"^[0-9]+$", ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "OnlyNumbers")]
        public string Dni { get; set; }

        [Display(Name = "TelefonoFijo", ResourceType = typeof(Messages))]
        [StringLength(11, MinimumLength = 11, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "ExactLenght")]
        public string TelefonoFijo { get; set; }

        [StringLength(11, MinimumLength = 11, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "ExactLenght")]
        public string Celular { get; set; }

        public string Email { get; set; }

        [Display(Name = "Localidad", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerida")]
        public long LocalidadId { get; set; }

        [Display(Name = "Provincia", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerida")]
        public long ProvinciaId { get; set; }

        [Display(Name = "PorcentajeComision", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
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