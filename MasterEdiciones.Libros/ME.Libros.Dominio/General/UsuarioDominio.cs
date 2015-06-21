namespace ME.Libros.Dominio.General
{
    public class UsuarioDominio : BaseDominio
    {
        #region Properties

        public virtual string Nombre { get; set; }
        public virtual string Contrasena { get; set; }

        #endregion
    }
}
