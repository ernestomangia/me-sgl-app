using System;

namespace ME.Libros.DTO
{
    public abstract class BaseDTO
    {
        public long Id { get; set; }
        public DateTime FechaAlta { get; set; }
    }
}
