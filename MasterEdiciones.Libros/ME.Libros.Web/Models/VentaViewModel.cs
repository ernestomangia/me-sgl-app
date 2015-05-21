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
            MontoVendido = ventaDominio.MontoVendido;
            MontoCalculado = ventaDominio.MontoCalculado;
            Saldo = ventaDominio.Saldo;
            Cliente = new ClienteViewModel(ventaDominio.Cliente);
            ClienteId = ventaDominio.Cliente.Id;
            CobradorId = ventaDominio.Cobrador.Id;
            
            if (ventaDominio.VentaItems  != null)
            {
                Items = new List<VentaItemViewModel>(ventaDominio.VentaItems.Select(vi => new VentaItemViewModel(vi)));
            }
        }

        #endregion

        #region Properties

        [Display(Name = "NumeroVenta", ResourceType = typeof(Messages))]
        public long Id { get; set; }

        [Display(Name = "FechaAlta", ResourceType = typeof(Messages))]
        public DateTime FechaAlta { get; set; }

        [Display(Name = "FechaVenta", ResourceType = typeof(Messages))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FechaVenta { get; set; }

        [Display(Name = "FechaCobro", ResourceType = typeof(Messages))]
        public DateTime FechaCobro { get; set; }

        [Display(Name = "MontoCalculado", ResourceType = typeof(Messages))]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal MontoCalculado { get; set; }

        [Display(Name = "MontoVendido", ResourceType = typeof(Messages))]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal MontoVendido { get; set; }

        [Display(Name = "Saldo", ResourceType = typeof(Messages))]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Saldo { get; set; }

        public EstadoVenta Estado { get; set; }
        
        [Display(Name = "Cliente", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        public long ClienteId { get; set; }

        [Display(Name = "Cobrador", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        public long CobradorId { get; set; }

        [Display(Name = "Vendedor", ResourceType = typeof(Messages))]
        public long? VendedorId { get; set; }

        public ClienteViewModel Cliente { get; set; }

        public List<VentaItemViewModel> Items { get; set; }

        public SelectList Clientes { get; set; }
        public SelectList Cobradores { get; set; }
        public SelectList Vendedores { get; set; }

        #endregion
    }
}