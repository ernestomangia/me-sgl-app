﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ME.Libros.Dominio.General;
using ME.Libros.Utils.Enums;

namespace ME.Libros.Web.Models
{
    public class CobradorViewModel
    {
        #region constructor(s)

        public CobradorViewModel()
        {
            Localidad = new LocalidadViewModel();
            LocalidadesAsignadas= new List<LocalidadViewModel>();
        }

        public CobradorViewModel(CobradorDominio cobrador)
        {
            Id = cobrador.Id;
            FechaAlta = cobrador.FechaAlta;
            Nombre = cobrador.Nombre;
            Apellido = cobrador.Apellido;
            Dni = cobrador.Dni;
            Localidad = new LocalidadViewModel(cobrador.Localidad);
            LocalidadesAsignadas = new List<LocalidadViewModel>(cobrador.Localidades.Select(la => new LocalidadViewModel(la)));
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

        [Display(Name = "Dni", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        [StringLength(8, MinimumLength = 8, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "ExactLenght")]
        [RegularExpression(@"^[0-9]+$", ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "OnlyNumbers")]
        public string Dni { get; set; }


        public LocalidadViewModel Localidad { get; set; }

        public SelectList Localidades { get; set; }

        [Display(Name = "LocalidadesAsignadas", ResourceType = typeof(Messages))]
        public List<LocalidadViewModel> LocalidadesAsignadas { get; set; }

        #endregion


    }
}