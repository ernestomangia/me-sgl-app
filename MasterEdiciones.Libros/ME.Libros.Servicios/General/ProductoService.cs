using ME.Libros.Api.Repositorios;
using ME.Libros.Dominio.General;

namespace ME.Libros.Servicios.General
{
    public class ProductoService : AbstractService<ProductoDominio>
    {
        public ProductoService(IRepository<ProductoDominio> repository)
            : base(repository)
        {

        }

        public bool VerificarStock(ProductoDominio productoDominio, int cantidad)
        {
            if (productoDominio.Stock - cantidad >= 0)
            {
                return true;
            }

            ModelError.Add("Stock", string.Format(ErrorMessages.StockInsuficiente, productoDominio.Nombre, productoDominio.Stock));
            return false;
        }

        public void RestarStock(ProductoDominio producto, int cantidad)
        {
            producto.Stock -= cantidad;
        }

        public void SumarStock(ProductoDominio producto, int cantidad)
        {
            producto.Stock += cantidad;
        }
    }
}
