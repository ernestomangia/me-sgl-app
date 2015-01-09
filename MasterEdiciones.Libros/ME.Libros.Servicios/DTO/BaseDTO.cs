namespace ME.Libros.Servicios.DTO
{
    using System;

    public abstract class BaseDTO
    {
        public int Id { get; set; }
        public DateTime FechaAlta { get; set; }
    }
}
