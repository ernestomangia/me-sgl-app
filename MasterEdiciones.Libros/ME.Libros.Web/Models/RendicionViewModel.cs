using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using ME.Libros.Dominio.General;

namespace ME.Libros.Web.Models
{
    public class RendicionViewModel
    {
        #region Constructor(s)

        public RendicionViewModel()
        {
            Cobrador = new CobradorViewModel();
            Cobros = new List<CobroViewModel>();
        }

        public RendicionViewModel(RendicionDominio rendicionDominio)
        {
            Id = rendicionDominio.Id;
            FechaAlta = rendicionDominio.FechaAlta;
            Periodo = rendicionDominio.Periodo;
            Cobrador = new CobradorViewModel(rendicionDominio.Cobrador);
            CobradorId = rendicionDominio.Cobrador.Id;
            Localidad = new LocalidadViewModel(rendicionDominio.Localidad);
            LocalidadId = rendicionDominio.Localidad.Id;
            MontoFacturado = rendicionDominio.MontoFacturado;
            MontoNeto = rendicionDominio.MontoNeto;
            Comision = rendicionDominio.Comision;
            MontoComision = rendicionDominio.MontoComision;
            Cobros = new List<CobroViewModel>(rendicionDominio.Cobros.Select(c => new CobroViewModel(c)));
            AutocompleteCobrador = string.Format("{0} {1}", rendicionDominio.Cobrador.Nombre, rendicionDominio.Cobrador.Apellido);
        }

        #endregion

        #region Properties

        [Display(Name = "Codigo", ResourceType = typeof(Messages))]
        public long Id { get; set; }

        [Display(Name = "FechaAlta", ResourceType = typeof(Messages))]
        public DateTime FechaAlta { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        [DisplayFormat(DataFormatString = "{0:y}")]
        public DateTime Periodo { get; set; }

        [Display(Name = "Cobrador", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        public long CobradorId { get; set; }

        [Display(Name = "Localidad", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerida")]
        public long LocalidadId { get; set; }

        [Display(Name = "MontoFacturado", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal MontoFacturado { get; set; }

        [Display(Name = "MontoNeto", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal MontoNeto { get; set; }

        [Display(Name = "PorcentajeComision", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerida")]
        [Range(0, 100, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "RangeValue")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Comision { get; set; }

        [Display(Name = "MontoComision", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerida")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal MontoComision { get; set; }

        [Display(Name = "Cobrador", ResourceType = typeof(Messages))]
        public string AutocompleteCobrador { get; set; }

        public List<CobroViewModel> Cobros { get; set; }

        public CobradorViewModel Cobrador { get; set; }
        public LocalidadViewModel Localidad { get; set; }
        public SelectList Localidades { get; set; }

        #endregion
    }
}