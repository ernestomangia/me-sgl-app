using System;
using System.Collections.Generic;

using ME.Libros.Utils.Enums;

namespace ME.Libros.Dominio.General
{
    public class VentaDominio : BaseDominio
    {
        #region Properties

        public virtual DateTime FechaVenta { get; set; }
        public virtual DateTime FechaCobro { get; set; }
        public virtual EstadoVenta Estado { get; set; }
        public virtual int CantidadCuotas { get; set; }
        public virtual decimal MontoCuota { get; set; }
        public virtual decimal MontoCalculado { get; set; }
        public virtual decimal MontoVendido { get; set; }
        public virtual decimal MontoCobrado { get; set; }
        public virtual decimal Saldo { get; set; }
        public virtual ClienteDominio Cliente { get; set; }
        public virtual List<VentaItemDominio> VentaItems { get; set; }
        public virtual VendedorDominio Vendedor { get; set; }
        public virtual CobradorDominio Cobrador { get; set; }
        public virtual PlanPagoDominio PlanPago { get; set; }
        public virtual List<CuotaDominio> Cuotas { get; set; }

        #endregion
    }
}
