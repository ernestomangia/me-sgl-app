using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using ME.Libros.Dominio.General;
using ME.Libros.Utils.Enums;

namespace ME.Libros.Web.Models
{
    public class CobroViewModel
    {
        #region Constructor(s)

        public CobroViewModel()
        {
            Cuotas = new List<CuotaViewModel>();
        }

        public CobroViewModel(CobroDominio cobroDominio)
        {
            Id = cobroDominio.Id;
            FechaAlta = cobroDominio.FechaAlta;
            FechaCobro = cobroDominio.FechaCobro;
            Monto = cobroDominio.Monto;
            Estado = cobroDominio.Estado;
            Cuotas = new List<CuotaViewModel>(cobroDominio.Cuotas.Select(c => new CuotaViewModel(c)));
            VentaId = cobroDominio.Cuotas.First().Venta.Id;
        }

        #endregion

        #region Properties

        [Display(Name = "Codigo", ResourceType = typeof(Messages))]
        public long Id { get; set; }

        [Display(Name = "FechaAlta", ResourceType = typeof(Messages))]
        public DateTime FechaAlta { get; set; }

        [Display(Name = "FechaCobro", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FechaCobro { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        public decimal Monto { get; set; }

        public EstadoCobro Estado;

        public List<CuotaViewModel> Cuotas { get; set; }

        public long VentaId { get; set; }

        public VentaViewModel Venta { get; set; }

        #endregion
    }
}