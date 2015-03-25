using System.Collections.Generic;

using ME.Libros.Utils.Enums;

namespace ME.Libros.Dominio.General
{
    public class VentaDominio : BaseDominio
    {
        #region Properties

        public virtual decimal Total { get; set; }
        public virtual ClienteDominio Cliente { get; set; }
        public virtual EstadoVenta Estado { get; set; }
        public virtual VendedorDominio Vendedor { get; set; }
        public virtual CobradorDominio Cobrador { get; set; }
        public virtual ICollection<VentaDetalleDominio> VentaDetalles { get; set; }

        #endregion
    }
}
