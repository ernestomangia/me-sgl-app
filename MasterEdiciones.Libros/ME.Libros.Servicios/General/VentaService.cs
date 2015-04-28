using System;
using System.Linq;
using ME.Libros.Api.Repositorios;
using ME.Libros.Dominio.General;

namespace ME.Libros.Servicios.General
{
    public class VentaService : AbstractService<VentaDominio>
    {
        private ProductoService ProductoService { get; set; }

        public VentaService(IRepository<VentaDominio> repository, ProductoService productoService)
            : base(repository)
        {
            ProductoService = productoService;
        }

        public long CrearVenta(VentaDominio ventaDominio)
        {
            foreach (var ventaItemDominio in ventaDominio.VentaItems)
            {
                ActualizarStock(ventaItemDominio);
                CalcularTotalItem(ventaItemDominio);
            }
            CalcularTotalVenta(ventaDominio);

            return Guardar(ventaDominio);
        }

        private static void CalcularTotalItem(VentaItemDominio ventaItemDominio)
        {
            ventaItemDominio.Monto = ventaItemDominio.Cantidad * ventaItemDominio.PrecioVenta;
        }

        private static void ActualizarStock(VentaItemDominio ventaItemDominio)
        {
            ventaItemDominio.Producto.Stock -= ventaItemDominio.Cantidad;
        }

        private static void CalcularTotalVenta(VentaDominio entidad)
        {
            entidad.Monto = entidad.VentaItems.Sum(vi => vi.Monto);
        }
    }
}
