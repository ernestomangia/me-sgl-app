namespace ME.Libros.Dominio.General
{
    public class VentaDetalleDominio : BaseDominio
    {
        #region Properties

        public virtual VentaDominio Venta { get; set; }
        public virtual ProductoDominio Producto { get; set; }
        public virtual int Cantidad { get; set; }
        public virtual decimal PrecioUnitario { get; set; }
        public virtual decimal Subtotal { get; set; }

        #endregion
    }
}
