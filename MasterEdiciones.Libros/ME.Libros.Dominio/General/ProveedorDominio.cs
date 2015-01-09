namespace ME.Libros.Dominio.General
{
    public class ProveedorDominio : BaseDominio
    {
        #region Properties

        public virtual string Codigo { get; set; }
        public virtual string RazorSocial { get; set; }
        public virtual string Domicilio { get; set; }

        #endregion
    }
}
