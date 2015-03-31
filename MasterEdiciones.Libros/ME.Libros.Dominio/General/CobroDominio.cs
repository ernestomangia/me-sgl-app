using System;

namespace ME.Libros.Dominio.General
{
    public class CobroDominio : BaseDominio
    {
        public virtual DateTime FechaCobro { get; set; }
        public virtual decimal Comision { get; set; }
        public virtual decimal Monto { get; set; }
        public virtual VentaDominio Venta { get; set; }
        public virtual CobradorDominio Cobrador { get; set; }

    }
}
