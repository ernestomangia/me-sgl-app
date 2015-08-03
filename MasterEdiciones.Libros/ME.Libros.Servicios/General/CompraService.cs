using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ME.Libros.Api.Repositorios;
using ME.Libros.Dominio.General;
using ME.Libros.Utils.Enums;

namespace ME.Libros.Servicios.General
{
    public class CompraService:AbstractService<CompraDominio>
    {

        ProductoService ProductoService { get; set; }

             public CompraService(IRepository<CompraDominio> repository)
            : base(repository)
        {
        }

             public CompraService(IRepository<CompraDominio> repository, ProductoService productoService)
            : base(repository)
        {
            ProductoService = productoService;
        }

             public override long Guardar(CompraDominio compraDominio)
             {
                 if (compraDominio.Id == 0)
                 {
                     if (!Validar(compraDominio))
                     {
                         return -1;
                     }

                     foreach (var compraItemDominio in compraDominio.CompraItems)
                     {
                         var producto = compraItemDominio.Producto;
                         ProductoService.SumarStock(producto, compraItemDominio.Cantidad);
                         compraItemDominio.PrecioCosto = producto.PrecioCosto;
                         compraItemDominio.PrecioCompraCalculado = producto.PrecioVenta;
                         CalcularTotalItem(compraItemDominio);
                     }

                    
                     CalcularTotalCompra(compraDominio);
                 }

                 return base.Guardar(compraDominio);
             }


             public long AnularCompra(long id)
             {
                 var compraDominio = GetPorId(id);
                 if (compraDominio.Estado == EstadoCompra.Pagada)
                 {
                     compraDominio.Estado = EstadoCompra.Anulada;
                     foreach (var compraItemDominio in compraDominio.CompraItems)
                     {
                         ProductoService.RestarStock(compraItemDominio.Producto, compraItemDominio.Cantidad);
                     }
                 }

                 return Guardar(compraDominio);
             }


             #region Private Methods

             private void CalcularTotalItem(CompraItemDominio compraItemDominio)
             {
                 compraItemDominio.MontoCalculado = compraItemDominio.Cantidad * compraItemDominio.PrecioCompraCalculado;
             }

             private void CalcularTotalCompra(CompraDominio entidad)
             {
                 entidad.MontoCalculado = entidad.CompraItems.Sum(vi => vi.MontoComprado);
             }
             #endregion

    }

}
