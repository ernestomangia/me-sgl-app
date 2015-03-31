namespace ME.Libros.Dominio.General
{
    public class ProductoDominio : BaseDominio
    {
        #region Properties

        public virtual string Nombre { get; set; }
        public virtual string Descripcion { get; set; }
        public virtual int Stock { get; set; }
        public virtual string CodigoBarra { get; set; }
        public virtual decimal PrecioCosto { get; set; }
        public virtual decimal PrecioVenta { get; set; }
        public virtual RubroDominio Rubro { get; set; }
        public virtual EditorialDominio Editorial { get; set; }

        #endregion
    }
}
