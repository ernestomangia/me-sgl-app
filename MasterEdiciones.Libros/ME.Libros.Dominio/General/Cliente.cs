
namespace Dominio.General
{
    public class Cliente
    {
        #region Properties

        public virtual string Nombre { set; get; }
        public virtual string Apellido { set; get; }
        public virtual string Identificacion { set; get; }
        public virtual string FechaNacimiento { set; get; }

        #endregion
    }
}
