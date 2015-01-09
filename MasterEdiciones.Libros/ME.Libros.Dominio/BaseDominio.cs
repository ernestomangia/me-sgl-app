using System;

namespace ME.Libros.Dominio
{
    public abstract class BaseDominio
    {
        public virtual int Id { get; set; }

        public virtual DateTime FechaAlta { get; set; }
    }
}
