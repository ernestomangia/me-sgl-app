using System;
using System.Collections.Generic;
using ME.Libros.Utils.Enums;

namespace ME.Libros.Dominio.General
{
    public class CuotaDominio : BaseDominio
    {
        #region Properties

        public virtual int Numero { get; set; }
        public virtual EstadoCuota Estado { get; set; }
        public virtual DateTime FechaVencimiento { get; set; }
        public virtual DateTime? FechaCobro { get; set; }
        public virtual long DiasAtraso { get; set; }
        public virtual decimal Monto { get; set; }
        public virtual decimal MontoCobro { get; set; }
        public virtual decimal Saldo { get; set; }
        public virtual decimal Interes { get; set; }
        public virtual VentaDominio Venta { get; set; }
        public virtual List<CobroDominio> Cobros { get; set; }

        #endregion
    }
}
