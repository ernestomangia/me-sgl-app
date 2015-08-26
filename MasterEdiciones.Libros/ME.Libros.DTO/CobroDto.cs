using System;

namespace ME.Libros.DTO
{
    public class CobroDto
    {
        public long VentaId { get; set; }
        public DateTime FechaCobro { get; set; }
        public decimal Monto { get; set; }
    }
}
