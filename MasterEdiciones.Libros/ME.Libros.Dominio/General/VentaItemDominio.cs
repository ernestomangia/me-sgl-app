namespace ME.Libros.Dominio.General
{
    public class VentaItemDominio : BaseDominio
    {
        #region Properties

        public virtual int Cantidad { get; set; }
        public virtual decimal PrecioCosto { get; set; }
        public virtual decimal PrecioVenta { get; set; }
        public virtual decimal Monto { get; set; }
        public virtual VentaDominio Venta { get; set; }
        public virtual ProductoDominio Producto { get; set; }

        #endregion
    }
}
