using System.Collections.Generic;

namespace Dominio.General
{
    public class ZonaDominio : BaseDominio
    {
        #region Properties

        public string Nombre { get; set; }
        public ICollection<LocalidadDominio> Localidades { get; set; }

        //TODO: averiguar si una zona puede estar asignada a mas de un cobrador y a mas de un vendedor
        //Puede ocurrir el caso de que se cambie de cobrador a mitad de una venta. Que pasaria con las comisiones?

        #endregion
    }
}
