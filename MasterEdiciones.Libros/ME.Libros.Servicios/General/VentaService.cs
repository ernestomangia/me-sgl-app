using System;
using System.Linq;
using ME.Libros.Api.Repositorios;
using ME.Libros.Dominio.General;

namespace ME.Libros.Servicios.General
{
    public class VentaService : AbstractService<VentaDominio>
    {
        private ProductoService ProductoService { get; set; }

        public VentaService(IRepository<VentaDominio> repository)
            : base(repository)
        {

        }

        public VentaService(IRepository<VentaDominio> repository, ProductoService productoService)
            : base(repository)
        {
            ProductoService = productoService;
        }



        public long CrearVenta(VentaDominio ventaDominio)
        {
            if (Validar(ventaDominio))
            {
                foreach (var ventaItemDominio in ventaDominio.VentaItems)
                {
                    //ProductoService.VerificarStock(ventaItemDominio.Producto, ventaItemDominio.Cantidad)
                    ActualizarStock(ventaItemDominio);
                    CalcularTotalItem(ventaItemDominio);
                }
                CalcularTotalVenta(ventaDominio);
            }
            return Guardar(ventaDominio);
        }

        public override bool Validar(VentaDominio entidad)
        {
            foreach (var ventaItemDominio in entidad.VentaItems.Where(ventaItemDominio => !ProductoService.VerificarStock(ventaItemDominio.Producto, ventaItemDominio.Cantidad)))
            {
                ModelError.Add("Stock", "El stock disponible de <b>" + ventaItemDominio.Producto.Nombre + "</b> es de " + ventaItemDominio.Producto.Stock + " unidades.");
                break;
            }
            return base.Validar(entidad);
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
