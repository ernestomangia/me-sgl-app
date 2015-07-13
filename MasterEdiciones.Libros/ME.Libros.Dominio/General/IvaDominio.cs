namespace ME.Libros.Dominio.General
{
    public class IvaDominio : BaseDominio
    {
        #region Properties

        public virtual string Nombre { get; set; }
        public virtual decimal Alicuota { get; set; }

        #endregion
    }
}
