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

        //public override long Guardar(VentaDominio entidad)
        //{
        //    foreach (var ventaItemDominio in entidad.VentaItems)
        //    {
        //        ActualizarStock(ventaItemDominio);
        //        CalcularTotalItem(ventaItemDominio);
                
        //    }
        //    CalcularTotalVenta(entidad);
            
        //    return base.Guardar(entidad);
        //}

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

        //public VentaItemDominio CrearVentaItem(long productoId, int cantidad)
        //{
        //    var producto = ProductoService.GetPorId(productoId);

        //    var ventaItemDominio = new VentaItemDominio
        //    {
        //        FechaAlta = DateTime.Now,
        //        Cantidad = cantidad,
        //        Producto = producto,
        //        PrecioVenta = producto.PrecioVenta,
        //        PrecioCosto = producto.PrecioCosto
        //    };

        //    ActualizarStock(ventaItemDominio);
        //    CalcularTotalItem(ventaItemDominio);

        //    return ventaItemDominio;
        //}

        private static void ActualizarStock(VentaItemDominio ventaItemDominio)
        {
            ventaItemDominio.Producto.Stock -= ventaItemDominio.Cantidad;
        }

        private static void CalcularTotalItem(VentaItemDominio ventaItemDominio)
        {
            ventaItemDominio.Monto = ventaItemDominio.Cantidad * ventaItemDominio.PrecioVenta;
        }

        private static void CalcularTotalVenta(VentaDominio entidad)
        {
            entidad.Monto = entidad.VentaItems.Sum(vi => vi.Monto);
        }
    }
}
