namespace Dominio.General
{
    public class Cliente : BaseModel
    {
        #region Properties
        public virtual string Nombre { get; set; }

        public virtual string Apellido { get; set; }

        public virtual string Cuil { get; set; }

        public virtual string FechaNacimiento { get; set; }

        #endregion
    }
}
