﻿using System;
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
            Items = new List<VentaItemViewModel>();
            Cuotas = new List<CuotaViewModel>();
        }

        public VentaViewModel(VentaDominio ventaDominio)
        {
            Id = ventaDominio.Id;
            FechaAlta = ventaDominio.FechaAlta;
            FechaVenta = ventaDominio.FechaVenta;
            FechaCobro = ventaDominio.FechaCobro;
            Estado = ventaDominio.Estado;
            MontoVendido = ventaDominio.MontoVendido;
            MontoCalculado = ventaDominio.MontoCalculado;
            MontoCobrado = ventaDominio.MontoCobrado;
            Saldo = ventaDominio.Saldo;
            MontoNetoVendido = ventaDominio.MontoNetoVendido;
            Cliente = new ClienteViewModel(ventaDominio.Cliente);
            ClienteId = ventaDominio.Cliente.Id;
            Cobrador = new CobradorViewModel(ventaDominio.Cobrador);
            CobradorId = ventaDominio.Cobrador.Id;
            Vendedor = new VendedorViewModel(ventaDominio.Vendedor);
            VendedorId = ventaDominio.Vendedor.Id;
            Comision = ventaDominio.Comision;
            MontoComision = ventaDominio.MontoComision;
            PlanPago = new PlanPagoViewModel(ventaDominio.PlanPago);
            PlanPagoId = ventaDominio.PlanPago.Id;
            CantidadCuotas = ventaDominio.CantidadCuotas;
            MontoCuota = ventaDominio.MontoCuota;
            // Items
            Items = new List<VentaItemViewModel>(ventaDominio.VentaItems.Select(vi => new VentaItemViewModel(vi) { Venta = this }));
            // Cuotas
            Cuotas = new List<CuotaViewModel>(ventaDominio.Cuotas.Select(c => new CuotaViewModel(c) { Venta = this }));
            EsVigente = ventaDominio.Estado == EstadoVenta.Vigente;
            EsPagada = ventaDominio.Estado == EstadoVenta.Pagada;
            AutocompleteCliente = string.Format("{0} {1}", ventaDominio.Cliente.Nombre, ventaDominio.Cliente.Apellido);
            AutocompleteCobrador = string.Format("{0} {1}", ventaDominio.Cobrador.Nombre, ventaDominio.Cobrador.Apellido);
            AutocompleteVendedor = string.Format("{0} {1}", ventaDominio.Vendedor.Nombre, ventaDominio.Vendedor.Apellido);
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
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal MontoVendido { get; set; }

        [Display(Name = "MontoCobrado", ResourceType = typeof(Messages))]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal MontoCobrado { get; set; }

        [Display(Name = "Saldo", ResourceType = typeof(Messages))]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Saldo { get; set; }

        public EstadoVenta Estado { get; set; }

        public int CantidadCuotas { get; set; }

        public decimal MontoCuota { get; set; }

        [Display(Name = "PorcentajeComision", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerida")]
        [Range(0, 100, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "RangeValue")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Comision { get; set; }

        [Display(Name = "MontoComision", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerida")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal MontoComision { get; set; }

        [Display(Name = "MontoNetoVendido", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal MontoNetoVendido { get; set; }

        public bool EsVigente { get; set; }

        public bool EsPagada { get; set; }

        [Display(Name = "Cliente", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        public long ClienteId { get; set; }

        [Display(Name = "Cobrador", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        public long CobradorId { get; set; }

        [Display(Name = "Vendedor", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        public long VendedorId { get; set; }

        [Display(Name = "PlanPago", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        public long PlanPagoId { get; set; }

        [Display(Name = "Cliente", ResourceType = typeof(Messages))]
        public string AutocompleteCliente { get; set; }

        [Display(Name = "Cobrador", ResourceType = typeof(Messages))]
        public string AutocompleteCobrador { get; set; }

        [Display(Name = "Vendedor", ResourceType = typeof(Messages))]
        public string AutocompleteVendedor { get; set; }

        public List<VentaItemViewModel> Items { get; set; }
        public List<CuotaViewModel> Cuotas { get; set; }
        public ClienteViewModel Cliente { get; set; }
        public CobradorViewModel Cobrador { get; set; }
        public VendedorViewModel Vendedor { get; set; }
        public PlanPagoViewModel PlanPago { get; set; }
        public SelectList PlanesPago { get; set; }

        #endregion
    }
}