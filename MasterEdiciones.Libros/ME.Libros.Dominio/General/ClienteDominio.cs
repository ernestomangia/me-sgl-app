using System;

using ME.Libros.Utils.Enums;

namespace ME.Libros.Dominio.General
{
    public class ClienteDominio : BaseDominio
    {
        #region Properties

        public virtual string Codigo { get; set; }
        public virtual string Nombre { get; set; }
        public virtual string Apellido { get; set; }
        public virtual string Cuil { get; set; }
        public virtual DateTime FechaNacimiento { get; set; }
        public virtual Sexo Sexo { get; set; }
        //public virtual LocalidadDominio Localidad { get; set; }
        public virtual string Direccion { get; set; }
        public virtual string CalleA { get; set; }
        public virtual string CalleB { get; set; }
        public virtual string Barrio { get; set; }
        public virtual string Manzana { get; set; }
        public virtual string Piso { get; set; }
        public virtual string Numero { get; set; }
        public virtual string TelefonoFijo { get; set; }
        public virtual string Celular { get; set; }

        #endregion
    }
}
