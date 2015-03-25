namespace ME.Libros.Dominio.General
{
    public class ProductoDominio : BaseDominio
    {
        #region Properties

        public virtual string CodigoBarra { get; set; }
        public virtual string Nombre { get; set; }
        public virtual string Descripcion { get; set; }
        public virtual decimal Precio { get; set; }
        public virtual RubroDominio Rubro { get; set; }
        public virtual Editorial Editorial { get; set; }

        #endregion
    }
}
