namespace ME.Libros.Dominio.General
{
    public class LocalidadDominio : BaseDominio
    {
        #region Properties

        public virtual string Nombre { get; set; }
        public virtual ProvinciaDominio Provincia { get; set; }

        #endregion
    }
}
