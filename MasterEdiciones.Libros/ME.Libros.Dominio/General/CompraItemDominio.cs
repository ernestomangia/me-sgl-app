
namespace ME.Libros.Dominio.General
{
    public class CompraItemDominio : BaseDominio
    {
        #region Properties

        public virtual int Orden { get; set; }
        public virtual int Cantidad { get; set; }
        public virtual decimal PrecioCosto { get; set; }
        public virtual decimal Monto { get; set; }
        public virtual decimal MontoCalculado { get; set; }
        public virtual decimal PrecioCompraCalculado { get; set; }
        public virtual decimal PrecioCompraComprado { get; set; }
        public virtual decimal MontoComprado { get; set; }
        public virtual CompraDominio Compra { get; set; }
        public virtual ProductoDominio Producto { get; set; }

        #endregion
    }
}
