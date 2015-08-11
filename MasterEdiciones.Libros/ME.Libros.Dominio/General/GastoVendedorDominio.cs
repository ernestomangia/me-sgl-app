using System;

namespace ME.Libros.Dominio.General
{
    public class GastoVendedorDominio : BaseDominio
    {
        public virtual DateTime FechaGasto { get; set; }
        public virtual decimal Monto { get; set; }
        public virtual GastoDominio Gasto { get; set; }
        public virtual VendedorDominio Vendedor { get; set; }
    }
}
