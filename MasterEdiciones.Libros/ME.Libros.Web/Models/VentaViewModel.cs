using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ME.Libros.Dominio.General;
using ME.Libros.Utils.Enums;

namespace ME.Libros.Web.Models
{
    using System.ComponentModel.DataAnnotations;

    public class VentaViewModel
    {
        #region Constructor(s)

        public VentaViewModel()
        {
            Cliente = new ClienteViewModel();
            Items = new List<VentaItemViewModel>();
        }

        public VentaViewModel(VentaDominio ventaDominio)
        {
            Id = ventaDominio.Id;
            FechaAlta = ventaDominio.FechaAlta;
            FechaVenta = ventaDominio.FechaVenta;
            Estado = ventaDominio.Estado;
            Cliente = new ClienteViewModel(ventaDominio.Cliente);
            Items = new List<VentaItemViewModel>(ventaDominio.VentaItems.Select(vi => new VentaItemViewModel(vi)));
        }

        #endregion

        #region Properties

        [Display(Name = "Codigo", ResourceType = typeof(Messages))]
        public long Id { get; set; }

        [Display(Name = "FechaAlta", ResourceType = typeof(Messages))]
        public DateTime FechaAlta { get; set; }

        [Display(Name = "Fecha", ResourceType = typeof(Messages))]
        public DateTime FechaVenta { get; set; }

        [Display(Name = "FechaCobro", ResourceType = typeof(Messages))]
        public DateTime FechaCobro { get; set; }

        [Display(Name = "Total", ResourceType = typeof(Messages))]
        public decimal Total { get; set; }

        public EstadoVenta Estado { get; set; }

        public ClienteViewModel Cliente { get; set; }
        [Display(Name = "Cliente", ResourceType = typeof(Messages))]
        public long ClienteId { get; set; }

        public List<VentaItemViewModel> Items { get; set; }

        public SelectList Clientes { get; set; }

        #endregion
    }
}