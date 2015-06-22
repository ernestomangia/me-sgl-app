using System;
using ME.Libros.Utils.Enums;

namespace ME.Libros.Dominio.General
{
    class CuotaDominio : BaseDominio
    {
        #region Properties

        public virtual DateTime Periodo { get; set; }
        public virtual int Numero { get; set; }
        public virtual EstadoCuota Estado { get; set; }
        public virtual DateTime FechaVencimiento { get; set; }
        public virtual decimal Monto { get; set; }
        public virtual decimal Interes { get; set; }
        public virtual VentaDominio Venta { get; set; }

        #endregion
    }
}
