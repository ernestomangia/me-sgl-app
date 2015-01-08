using System.Collections.Generic;

namespace Dominio.General
{
    public class CobradorDominio : BaseDominio
    {
        #region Properties

        public virtual string Codigo { get; set; }
        public virtual string Nombre { get; set; }
        public virtual string Apellido { get; set; }
        public virtual string Cuit { get; set; }
        public virtual string Domicilio { get; set; }
        public virtual decimal Comision { get; set; }
        public virtual ICollection<ZonaDominio> Zonas { get; set; }
        
        #endregion
    }
}
