﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using ME.Libros.Dominio.General;
using ME.Libros.Utils.Enums;

namespace ME.Libros.Web.Models
{
    public class CuotaViewModel
    {
        #region Constructor(s)

        public CuotaViewModel()
        {
            //Venta = new VentaViewModel();
        }

        public CuotaViewModel(CuotaDominio cuotaDominio)
        {
            Id = cuotaDominio.Id;
            FechaAlta = cuotaDominio.FechaAlta;
            Numero = cuotaDominio.Numero;
            Estado = cuotaDominio.Estado;
            FechaVencimiento = cuotaDominio.FechaVencimiento;
            FechaCobro = cuotaDominio.FechaCobro;
            Atraso = cuotaDominio.DiasAtraso;
            Monto = cuotaDominio.Monto;
            MontoCobro = cuotaDominio.MontoCobro;
            Saldo = cuotaDominio.Saldo;
            //Venta = new VentaViewModel(cuotaDominio.Venta);
        }

        #endregion

        #region Properties

        [Display(Name = "Codigo", ResourceType = typeof(Messages))]
        public long Id { get; set; }

        [Display(Name = "FechaAlta", ResourceType = typeof(Messages))]
        public DateTime FechaAlta { get; set; }

        [Display(Name = "NroCuota", ResourceType = typeof(Messages))]
        public int Numero { get; set; }

        public EstadoCuota Estado { get; set; }

        [Display(Name = "FechaVencimiento", ResourceType = typeof(Messages))]
        public DateTime FechaVencimiento { get; set; }

        [Display(Name = "FechaCobro", ResourceType = typeof(Messages))]
        public DateTime? FechaCobro { get; set; }

        public int Atraso { get; set; }

        [Display(Name = "MontoCuota", ResourceType = typeof(Messages))]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Monto { get; set; }

        [Display(Name = "MontoCobroCuota", ResourceType = typeof(Messages))]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal MontoCobro { get; set; }

        [Display(Name = "SaldoCuota", ResourceType = typeof(Messages))]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Saldo { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Interes { get; set; }

        public VentaViewModel Venta { get; set; }

        #endregion
    }
}