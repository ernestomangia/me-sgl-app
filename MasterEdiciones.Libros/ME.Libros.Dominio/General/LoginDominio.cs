namespace ME.Libros.Dominio.General
{
    public class LoginDominio : BaseDominio
    {
        #region Properties

        public virtual string Usuario { get; set; }
        public virtual string Contrasena { get; set; }

        #endregion
    }
}
