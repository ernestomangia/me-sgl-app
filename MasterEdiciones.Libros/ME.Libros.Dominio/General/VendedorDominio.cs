using System;
using System.Collections.Generic;

using ME.Libros.Utils.Enums;

namespace ME.Libros.Dominio.General
{
    public class VendedorDominio : BaseDominio
    {
        #region Properties

        public virtual string Nombre { get; set; }
        public virtual string Apellido { get; set; }
        public virtual string Cuil { get; set; }
        //public virtual DateTime FechaNacimiento { get; set; }
        //public virtual Sexo Sexo { get; set; }
        public virtual string Direccion { get; set; }
        public virtual string Numero { get; set; }
        public virtual string TelefonoFijo { get; set; }
        public virtual string Celular { get; set; }
        public virtual string Email { get; set; }
        public virtual decimal Comision { get; set; }
        //public virtual LocalidadDominio Localidad { get; set; }
        //public virtual ICollection<ZonaDominio> Zonas { get; set; }

        #endregion
    }
}
