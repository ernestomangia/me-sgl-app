using ME.Libros.DTO;
using ME.Libros.Web.Models;

namespace ME.Libros.Web.Extensions
{
    public class DtoHelper
    {
        public static VentaSearchDto ConvertToDto(VentaTodasViewModel ventaTodasViewModel)
        {
            return new VentaSearchDto
            {
                Cliente = ventaTodasViewModel.Cliente,
                Cobrador = ventaTodasViewModel.Cobrador,
                Vendedor = ventaTodasViewModel.Vendedor,
                EstadoVenta = ventaTodasViewModel.EstadoVenta,
                Desde = ventaTodasViewModel.Desde,
                Hasta = ventaTodasViewModel.Hasta
            };
        }
    }
}