using System;

namespace ME.Libros.Dominio
{
    public abstract class BaseDominio
    {
        public virtual long Id { get; set; }

        public virtual DateTime FechaAlta { get; set; }
    }
}
