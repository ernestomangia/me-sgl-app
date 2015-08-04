using System;

namespace ME.Libros.Dominio.General
{
    public class UsuarioDominio : BaseDominio
    {
        #region Properties

        public virtual string Nombre { get; set; }
        public virtual string Apellido { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Password { get; set; }
        public virtual string Email { get; set; }
        public virtual bool EmailConfirmado { get; set; }
        public virtual bool Habilitado { get; set; }
        public virtual DateTime? UltimoLogin { get; set; }
        public virtual long CantidadIntentosFallidos { get; set; }

        #endregion
    }
}
