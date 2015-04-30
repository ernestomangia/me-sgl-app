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

            //ModelError.Add("Stock", string.Format(ErrorMessages.Stock, productoDominio.Nombre, productoDominio.Stock));
            return false;
        }
    }
}
