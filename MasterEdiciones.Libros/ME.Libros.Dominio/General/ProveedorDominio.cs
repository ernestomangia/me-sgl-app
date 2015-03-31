namespace ME.Libros.Dominio.General
{
    public class ProveedorDominio : BaseDominio
    {
        #region Properties

        public virtual string RazorSocial { get; set; }
        public virtual string Cuil { get; set; }
        public virtual string Direccion { get; set; }
        public virtual string Numero { get; set; }   
        public virtual string TelefonoFijo { get; set; }
        public virtual string Celular { get; set; }
        public virtual string Email { get; set; }
        public virtual LocalidadDominio Localidad { get; set; }

        #endregion
    }
}
