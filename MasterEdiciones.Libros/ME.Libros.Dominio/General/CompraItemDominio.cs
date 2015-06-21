using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ME.Libros.Dominio.General
{
    public class CompraItemDominio: BaseDominio
    {
        #region Properties

        public virtual int Cantidad { get; set; }
        public virtual decimal PrecioCosto { get; set; }
        public virtual decimal Monto { get; set; }
        public virtual CompraDominio Compra { get; set; }
        public virtual ProductoDominio Producto { get; set; }

        #endregion
    }
}
