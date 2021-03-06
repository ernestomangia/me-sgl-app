﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

using ME.Libros.Dominio.General;
using ME.Libros.Web.Validators;

namespace ME.Libros.Web.Models
{
    public class ProveedorViewModel
    {
        #region Constructor(s)

        public ProveedorViewModel()
        {
        }

        public ProveedorViewModel(ProveedorDominio proveedor)
        {
            Id = proveedor.Id;
            FechaAlta = proveedor.FechaAlta;
            RazonSocial = proveedor.RazonSocial;
            Cuil = proveedor.Cuil;
            Direccion = proveedor.Direccion;
            TelefonoFijo = proveedor.TelefonoFijo;
            Celular = proveedor.Celular;
            Email = proveedor.Email;
            Localidad = new LocalidadViewModel(proveedor.Localidad);
            LocalidadId = proveedor.Localidad.Id;
            ProvinciaId = proveedor.Localidad.Provincia.Id;
        }

        #endregion

        #region Properties

        [Display(Name = "Codigo", ResourceType = typeof(Messages))]
        public long Id { get; set; }

        [Display(Name = "FechaAlta", ResourceType = typeof(Messages))]
        public DateTime FechaAlta { get; set; }

        [Display(Name = "RazonSocial", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        [StringLength(100, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "StringLength")]
        [RegularExpression(@"^([a-zA-ZñÑáéíóúÁÉÍÓÚ´]+\s)*[a-zA-ZñÑáéíóúÁÉÍÓÚ´]+$", ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "OnlyLetters")]
        public string RazonSocial { get; set; }

        [Display(Name = "Cuil", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        [StringLength(13, MinimumLength = 13, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "ExactLenght")]
        [RegularExpression(@"\d{2}[-. ]\d{8}[-. ]\d{1}", ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "FormatoInvalido")]
        [Cuil]
        public string Cuil { get; set; }

        [Display(Name = "Direccion", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerida")]
        [StringLength(200, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "StringLength")]
        public string Direccion { get; set; }

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

        public LocalidadViewModel Localidad { get; set; }
        public SelectList Provincias { get; set; }
        public SelectList Localidades { get; set; }

        #endregion
    }
}