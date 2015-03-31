
namespace ME.Libros.Dominio.General
{
    public class EditorialDominio : BaseDominio
    {
        #region Properties

        public virtual string Nombre { get; set; }
        public virtual string Descripcion { get; set; }

        #endregion
    }
}
