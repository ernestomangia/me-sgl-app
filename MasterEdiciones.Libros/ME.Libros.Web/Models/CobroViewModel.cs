using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ME.Libros.Dominio.General;
using ME.Libros.Utils.Enums;

namespace ME.Libros.Web.Models
{
    public class CobroViewModel
    {
         #region constructor(s)

        public CobroViewModel()
        {
           Venta= new VentaViewModel();

        }

        public CobroViewModel(CobroDominio cobro)
        {
            Id = cobro.Id;
            FechaAlta = cobro.FechaAlta;
            Monto = cobro.Monto;
            FechaCobro = cobro.FechaCobro;
            Venta= new VentaViewModel(cobro.Venta);
            Estado = cobro.EstadoCobro;
            Cobrador = new CobradorViewModel(cobro.Cobrador);
            VentaId = cobro.Venta.Id;
            ClienteId = cobro.Venta.Cliente.Id;

        }
        #endregion

        #region Properties

        [Display(Name = "Codigo", ResourceType = typeof(Messages))]
        public long Id { get; set; }

        [Display(Name = "FechaAlta", ResourceType = typeof(Messages))]
        public DateTime FechaAlta { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        public decimal Monto { get; set; }

        public EstadoCobro Estado;

        [Display(Name = "FechaCobro", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FechaCobro { get; set; }


        [Display(Name = "Venta", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        public long VentaId { get; set; }

        [Display(Name = "Cliente", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        public long ClienteId { get; set; }

        public CobradorViewModel Cobrador { get; set; }

        public VentaViewModel Venta { get; set; }

        public SelectList Ventas { get; set; }
        public SelectList Clientes { get; set; }

        #endregion
    }
}