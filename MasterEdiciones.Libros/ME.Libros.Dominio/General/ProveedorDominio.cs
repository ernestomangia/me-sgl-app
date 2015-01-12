namespace ME.Libros.Dominio.General
{
    public class ProveedorDominio : BaseDominio
    {
        #region Properties

        public virtual string RazorSocial { get; set; }
        public virtual string Cuil { get; set; }
        public virtual string Telefono { get; set; }
        public virtual string Domicilio { get; set; }   
        public virtual string Email { get; set; }

        #endregion
    }
}
