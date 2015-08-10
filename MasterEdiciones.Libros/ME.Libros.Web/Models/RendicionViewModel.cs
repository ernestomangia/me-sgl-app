using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using ME.Libros.Dominio.General;

namespace ME.Libros.Web.Models
{
    public class RendicionViewModel
    {
        #region Constructor(s)

        public RendicionViewModel()
        {
        }

        public RendicionViewModel(RendicionDominio rendicionDominio)
        {
            Id = rendicionDominio.Id;
            FechaAlta = rendicionDominio.FechaAlta;
            Periodo = rendicionDominio.Periodo;
            Cobrador = new CobradorViewModel(rendicionDominio.Cobrador);
            CobradorId = rendicionDominio.Cobrador.Id;
            Zona = new ZonaViewModel(rendicionDominio.Zona);
            ZonaId = rendicionDominio.Zona.Id;
        }

        #endregion

        #region Properties

        [Display(Name = "Codigo", ResourceType = typeof(Messages))]
        public long Id { get; set; }

        [Display(Name = "FechaAlta", ResourceType = typeof(Messages))]
        public DateTime FechaAlta { get; set; }

        public DateTime Periodo { get; set; }

        [Display(Name = "Cobrador", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerida")]
        public long CobradorId { get; set; }

        [Display(Name = "Zona", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerida")]
        public long ZonaId { get; set; }

        [Display(Name = "MontoFacturado", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal MontoFacturado { get; set; }

        [Display(Name = "MontoNeto", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal MontoNeto { get; set; }

        [Display(Name = "PorcentajeComision", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal PorcentajeComision { get; set; }

        [Display(Name = "MontoComision", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerida")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal MontoComision { get; set; }

        public List<CobroViewModel> Cobros { get; set; } 

        public CobradorViewModel Cobrador { get; set; }
        public ZonaViewModel Zona { get; set; }
        public SelectList Cobradores { get; set; }
        public SelectList Zonas { get; set; }

        #endregion
    }
}