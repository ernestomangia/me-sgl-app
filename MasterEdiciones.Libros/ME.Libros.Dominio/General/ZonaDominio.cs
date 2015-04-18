using System.Collections.Generic;

namespace ME.Libros.Dominio.General
{
    public class ZonaDominio : BaseDominio
    {
        #region Properties

        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        //public ICollection<LocalidadDominio> Localidades { get; set; }
        //public CobradorDominio Cobrador { get; set; }
        //public VendedorDominio Vendedor { get; set; }

        #endregion
    }
}
