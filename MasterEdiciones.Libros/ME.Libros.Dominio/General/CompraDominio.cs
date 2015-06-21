using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ME.Libros.Utils.Enums;

namespace ME.Libros.Dominio.General
{
    public class CompraDominio: BaseDominio
    {
        #region Properties

        public virtual DateTime FechaCompra { get; set; }
        public virtual EstadoCompra Estado { get; set; }
        public virtual decimal MontoCalculado { get; set; }
        public virtual decimal MontoComprado { get; set; }
        public virtual string NroRemito { get; set; }
        public virtual string NroFactura { get; set; }

        public virtual ProveedorDominio Proveedor { get; set; }
        public virtual List<CompraItemDominio> CompraItems { get; set; }

        #endregion
    }
}
