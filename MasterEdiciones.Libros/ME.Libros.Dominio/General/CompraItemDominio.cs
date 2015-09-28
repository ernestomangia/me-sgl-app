
namespace ME.Libros.Dominio.General
{
    public class CompraItemDominio : BaseDominio
    {
        #region Properties

        public virtual int Orden { get; set; }
        public virtual int Cantidad { get; set; }
        public virtual decimal PrecioCostoAnterior { get; set; }
        public virtual decimal PrecioCostoComprado { get; set; }
        public virtual decimal MontoComprado { get; set; }
        public virtual CompraDominio Compra { get; set; }
        public virtual ProductoDominio Producto { get; set; }

        #endregion
    }
}
