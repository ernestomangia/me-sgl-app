using System.Collections.Generic;

namespace ME.Libros.Dominio.General
{
    public class CobradorDominio : BaseDominio
    {
        #region Properties

        public virtual string Nombre { get; set; }
        public virtual string Apellido { get; set; }
        public virtual string Dni { get; set; }
        public virtual string Direccion { get; set; }
        public virtual string TelefonoFijo { get; set; }
        public virtual string Celular { get; set; }
        public virtual string Email { get; set; }
        public virtual decimal PorcentajeComision { get; set; }
        public virtual LocalidadDominio Localidad { get; set; }
        public virtual List<LocalidadDominio> Localidades { get; set; }
        #endregion
    }
}
