namespace ME.Libros.DTO.General
{
    public class LocalidadDTO : BaseDTO
    {
        #region Properties

        public virtual string Nombre { get; set; }
        public virtual ProvinciaDTO Provincia { get; set; }

        #endregion
    }
}
