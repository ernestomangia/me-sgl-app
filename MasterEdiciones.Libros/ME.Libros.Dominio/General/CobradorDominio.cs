using System;
using System.Collections.Generic;

using ME.Libros.Utils.Enums;

namespace ME.Libros.Dominio.General
{
    public class CobradorDominio : BaseDominio
    {
        #region Properties

        public virtual string Nombre { get; set; }
        public virtual string Apellido { get; set; }
        public virtual string Dni { get; set; }
        public virtual LocalidadDominio Localidad { get; set; }
        public virtual List<LocalidadDominio> Localidades { get; set; }
        #endregion
    }
}
