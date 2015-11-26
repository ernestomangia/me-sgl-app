using System;
using System.Collections.Generic;

namespace ME.Libros.Dominio.General
{
    public class RendicionDominio : BaseDominio
    {
        #region Properties

        public virtual DateTime Periodo { get; set; }
        public virtual CobradorDominio Cobrador { get; set; }
        public virtual LocalidadDominio Localidad { get; set; }
        public virtual List<CobroDominio> Cobros { get; set; }
        public virtual decimal MontoFacturado { get; set; }
        public virtual decimal MontoNeto { get; set; }
        public virtual decimal Comision { get; set; }
        public virtual decimal MontoComision { get; set; }

        #endregion
    }
}
