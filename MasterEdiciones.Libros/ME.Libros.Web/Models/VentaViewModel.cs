using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ME.Libros.Web.Models
{
    using System.ComponentModel.DataAnnotations;

    public class VentaViewModel
    {
        #region Constructor(s)

        public VentaViewModel()
        {
        }

        #endregion

        #region Properties

        [Display(Name = "Codigo", ResourceType = typeof(Messages))]
        public long Id { get; set; }

        [Display(Name = "FechaAlta", ResourceType = typeof(Messages))]
        public DateTime FechaAlta { get; set; }

        [Display(Name = "Fecha", ResourceType = typeof(Messages))]
        public DateTime FechaVenta { get; set; }

        [Display(Name = "Total", ResourceType = typeof(Messages))]
        public decimal Total { get; set; }

        public ClienteViewModel Cliente { get; set; }

        #endregion
    }
}