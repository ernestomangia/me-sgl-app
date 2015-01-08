namespace Dominio.General
{
    public class ArticuloDominio : BaseDominio
    {
        #region Properties

        public virtual string Codigo { get; set; }
        public virtual string CodigoBarra { get; set; }
        public virtual string Nombre { get; set; }
        public virtual string Descripcion { get; set; }
        public virtual RubroDominio Rubro { get; set; }
        public virtual string Editorial { get; set; }
        public virtual decimal Precio { get; set; }

        #endregion
    }
}
