using System.Linq;
using ME.Libros.Api.Repositorios;
using ME.Libros.Dominio.General;
using ME.Libros.Utils.Enums;

namespace ME.Libros.Servicios.General
{
    public class CompraService : AbstractService<CompraDominio>
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
                    // Guardar precio costo anterior
                    compraItemDominio.PrecioCostoAnterior = producto.PrecioCosto;
                    // Actualizar nuevo precio de costo en el producto
                    producto.PrecioCosto = compraItemDominio.PrecioCostoComprado;
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
            compraItemDominio.MontoComprado = compraItemDominio.Cantidad * compraItemDominio.PrecioCostoComprado;
        }

        private void CalcularTotalCompra(CompraDominio compraDominio)
        {
            compraDominio.MontoCalculado = compraDominio.CompraItems.Sum(vi => vi.MontoComprado);
        }

        #endregion
    }
}
