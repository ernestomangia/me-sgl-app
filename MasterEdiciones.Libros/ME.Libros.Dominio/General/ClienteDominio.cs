using System;

namespace ME.Libros.Dominio.General
{
    public class ClienteDominio : BaseDominio
    {
        #region Properties

        public virtual long Codigo { get; set; }
        public virtual string Nombre { get; set; }
        public virtual string Apellido { get; set; }
        public virtual long? Dni { get; set; }
        public virtual string Cuil { get; set; }
        public virtual DateTime? FechaNacimiento { get; set; }
        public virtual IvaDominio Iva { get; set; }
        public virtual string Direccion { get; set; }
        public virtual string Comentario { get; set; }
        public virtual string TelefonoFijo { get; set; }
        public virtual string Celular { get; set; }
        public virtual string Celular2 { get; set; }
        public virtual string Email { get; set; }
        public virtual LocalidadDominio Localidad { get; set; }

        #endregion
    }
}
