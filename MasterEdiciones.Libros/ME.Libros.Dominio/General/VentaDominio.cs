using System.Collections.Generic;

using ME.Libros.Utils.Enums;

namespace Dominio.General
{
    public class VentaDominio : BaseDominio
    {
        #region Properties

        public virtual ClienteDominio Cliente { get; set; }
        public virtual ICollection<VentaDetalleDominio> VentaDetalles { get; set; }
        public virtual decimal Total { get; set; }
        public virtual EstadoVenta Estado { get; set; }

        #endregion
    }
}
